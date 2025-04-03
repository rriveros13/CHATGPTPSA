truncate table [PCA-CREDIFACIL-PROD].[dbo].[$pv_prestamos]

truncate table [PCA-CREDIFACIL-PROD].[dbo].[PrestamoDetalle]

delete from [PCA-CREDIFACIL-PROD].[dbo].[PrestamoCabecera]

insert into [PCA-CREDIFACIL-PROD].[dbo].[$pv_prestamos]
select * 
from OPENQUERY(CRFAPROD, 'select * from pv_prestamos p') p

update [PCA-CREDIFACIL-PROD].[dbo].[Persona]
set Estado = (select Oid 
	FROM [PCA-CREDIFACIL-PROD].[dbo].[EstadoPersona]
	where Activo=0),
      PoseeDemanda = 1

update [PCA-CREDIFACIL-PROD].[dbo].[Persona]
set Estado = (select Oid 
	FROM [PCA-CREDIFACIL-PROD].[dbo].[EstadoPersona]
	where Activo=1)
where Documento in (
	select p.DOCUMENTO
	from [PCA-CREDIFACIL-PROD].[dbo].[V_CuotaPagada] p
	group by p.DOCUMENTO, p.ACTIVO
	having p.ACTIVO=1
)

update [PCA-CREDIFACIL-PROD].[dbo].[Persona]
set PoseeDemanda = 0
where Documento in (
	select p.Documento
	from [PCA-CREDIFACIL-PROD].[dbo].[V_CuotaPagada] p
	group by p.Documento, p.EnJuicio
	having p.EnJuicio='S'
)

insert into [PCA-CREDIFACIL-PROD].[dbo].[PrestamoCabecera]
select  NEWID(),
		v.Persona,
		a.CodeudorOid Codeudor,
		a.ConyugeOid Conyuge,
		v.PrestamoNumero, 
		v.MontoPrestamo, 
		case
			when MAX(v.SaldoCapital) is null then 0
			else MAX(v.SaldoCapital)
		end SaldoCapital,
		case
			when MAX(v.SaldoInteres) is null then 0
			else MAX(v.SaldoInteres)
		end SaldoInteres,
		SUM(v.SaldoCuota) SaldoCuota,
		v.CantidadCuotas, 
		v.FechaTransaccion,
		MAX(v.FechaVencimiento) FechaVencimiento,
		case
			when MAX(v.Activo) is null and SUM(v.SaldoCuota)>0 then 0
			when MAX(v.Activo) is null and SUM(v.SaldoCuota)<=0 then 1
			when MAX(v.Activo)=1 then 0
			else 1
		end Estado,
		case
			when MAX(v.EnJuicio) is null then 1
			when MAX(v.EnJuicio)='N' then 1
			else 0
		end Judicial,
		0,
		null,
		null
from [PCA-CREDIFACIL-PROD].[dbo].[V_CuotaPagada] v
left join (
		select  p.PRESTAMONUMERO, 
				pgar.Oid CodeudorOid,
				case 
					when p.SOLICITUDNUMERO is not null then per.Oid
					when p.SOLICITUDNUMERO is null then pcon.Oid
					else null
				end ConyugeOid
		from [PCA-CREDIFACIL-PROD].[dbo].[$pv_prestamos] p
		left join [PCA-CREDIFACIL-PROD].[dbo].[Solicitud] s on p.SOLICITUDNUMERO=s.OID
		left join [PCA-CREDIFACIL-PROD].[dbo].[Solicitud] sc on s.SolicitudCodeudor=sc.OID
		left join [PCA-CREDIFACIL-PROD].[dbo].[SolicitudPersona] sp on s.OID=sp.Solicitud and sp.TipoPersona in (select tp.Oid from [PCA-CREDIFACIL-PROD].[dbo].[TipoPersona] tp where tp.Codigo='CON')
		left join [PCA-CREDIFACIL-PROD].[dbo].[Persona] per on sp.Persona=per.Oid
		left join (
			SELECT  p.NRO_CUENTA PrestamoNumero,
					c.DocumentoTitular,
					c.DocumentoCodeudor,
					c.DocumentoConyuge
			FROM [PCA-CREDIFACIL-PROD].[dbo].[$codeudor_conyuge] c
			inner join [PCA-CREDIFACIL-PROD].[dbo].[$prestamos_cuenta] p on c.PrestamoNro=p.VAL_PARAMETRO
		) a on p.PRESTAMONUMERO=a.PrestamoNumero
		left join [PCA-CREDIFACIL-PROD].[dbo].[Persona] pcon on a.DocumentoConyuge=pcon.Documento
		left join [PCA-CREDIFACIL-PROD].[dbo].[Persona] pgar on a.DocumentoCodeudor=pgar.Documento or sc.Documento=pgar.Documento
		group by p.DOCUMENTO, p.PRESTAMONUMERO, p.DOCUMENTOVINCULO1, p.DOCUMENTOVINCULO2, p.SOLICITUDNUMERO, s.OID, s.SolicitudCodeudor, sc.Documento, per.Documento, per.Oid, pcon.Oid, pgar.Oid, a.PrestamoNumero, a.DocumentoCodeudor, a.DocumentoConyuge
) a on v.PrestamoNumero=a.PRESTAMONUMERO
group by v.Documento, v.Persona, v.PrestamoNumero, v.CantidadCuotas, v.MontoPrestamo, v.FechaTransaccion, a.CodeudorOid, a.ConyugeOid
order by v.Documento, v.PrestamoNumero

update [PCA-CREDIFACIL-PROD].[dbo].[PrestamoCabecera]
set [PCA-CREDIFACIL-PROD].[dbo].[PrestamoCabecera].PromedioAtraso = a.PromedioAtraso
from [PCA-CREDIFACIL-PROD].[dbo].[PrestamoCabecera] p
inner join (
		select c.PrestamoNumero, floor(AVG(c.Diferencia)) PromedioAtraso
		from [PCA-CREDIFACIL-PROD].[dbo].[V_CuotaPagada] c
		group by c.PrestamoNumero
	) a on p.PrestamoNumero=a.PrestamoNumero

insert into [PCA-CREDIFACIL-PROD].[dbo].[PrestamoDetalle]
select  NEWID(),
		p.Oid PrestamoCabecera,
		c.CuotaNumero,
		c.MontoCuota,
		c.SaldoCuota,
		c.FechaVencimiento,
		c.FechaPago,
		c.Diferencia,
		null,
		null
from [PCA-CREDIFACIL-PROD].[dbo].[V_CuotaPagada] c
left join [PCA-CREDIFACIL-PROD].[dbo].[PrestamoCabecera] p on c.PrestamoNumero=p.PrestamoNumero

USE [PCA-CREDIFACIL-DEV]
GO

/****** Object:  View [dbo].[V_CuotaPagada]    Script Date: 10/9/2020 19:48:42 ******/
DROP VIEW [dbo].[V_CuotaPagada]
GO

/****** Object:  View [dbo].[V_CuotaPagada]    Script Date: 10/9/2020 19:48:42 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO








CREATE VIEW [dbo].[V_CuotaPagada]
 AS
select c.[Oid] 
	  ,c.[Documento]
	  ,case
			when c.PrestamoNumero is null then c.PrestamoNumeroOriginal
			else c.PrestamoNumero
	   end PrestamoNumero
	  ,c.[CantidadCuotas]
	  ,c.[CuotaNumero]
	  ,c.[MontoPrestamo]
	  ,c.[MontoCuota]
	  ,c.[SaldoCuota]
	  ,c.[SaldoCapital]
	  ,c.[SaldoInteres]
	  ,c.[FechaVencimiento]
	  ,c.[FechaPago]
	  ,c.[Diferencia]
	  ,c.[Activo]
	  ,c.[EnJuicio]
	  ,c.[FechaTransaccion]
	  ,p.Oid Persona
	  ,c.[OptimisticLockField]
	  ,c.[GCRecord]
from (
	select  c.Oid,
			case
				when p.documento is null then c.Documento
				else p.documento
			end Documento,
			case
				when p.prestamonumero is null then b.nro_cuenta
				else p.prestamonumero
			end PrestamoNumero,
			case
				when p.prestamonumerooriginal is null then c.PrestamoNumeroOriginal
				else p.prestamonumerooriginal
			end PrestamoNumeroOriginal,
			case
				when p.cantidadcuotas is null then c.CantidadCuotas
				else p.cantidadcuotas
			end CantidadCuotas,
			case
				when p.cuotanumero is null then c.CuotaNumero
				else p.cuotanumero
			end CuotaNumero,
			c.MontoPrestamo,
			case
				when p.montocuota is null then c.MontoCuota
				else p.montocuota
			end MontoCuota,
			case
				when p.saldocuota is null then c.SaldoCuota
				else p.saldocuota
			end SaldoCuota,
			p.SaldoCapital,
			p.SaldoInteres,
			case
				when p.fechavencimiento is null then c.FechaVencimiento
				else p.fechavencimiento
			end FechaVencimiento,
			case
				when p.fechapago is null and p.activo is null then c.FechaPago
				else p.fechapago
			end FechaPago,
			case
				when p.diferencia is null and p.activo is null then c.Diferencia
				else p.diferencia
			end Diferencia,
			p.activo Activo,
			case
				when p.EnJuicio='S' and p.activo=0 then 'N'
				else p.EnJuicio
			end EnJuicio,
			case
				when c.FechaTransaccion is null then p.fechatransaccion
				else c.FechaTransaccion
			end FechaTransaccion,
			c.OptimisticLockField,
			c.GCRecord
	FROM [PCA-CREDIFACIL-DEV].[dbo].[$pv_prestamos] p
	right join [PCA-CREDIFACIL-DEV].[dbo].[$prestamos_origen] c on p.prestamonumerooriginal=c.PrestamoNumeroOriginal and p.documento=c.Documento and p.cantidadcuotas=c.CantidadCuotas and p.cuotanumero=c.CuotaNumero and p.fechavencimiento=c.FechaVencimiento
	left join [PCA-CREDIFACIL-DEV].[dbo].[$prestamos_cuenta] b on c.PrestamoNumeroOriginal=b.val_parametro
	UNION ALL
	SELECT  NEWID() Oid,
			documento,
			prestamonumero,
			prestamonumerooriginal,
			cantidadcuotas,
			cuotanumero,
			montoprestamo,
			montocuota,
			saldocuota,
			saldocapital,
			saldointeres,
			fechavencimiento,
			fechapago,
			diferencia,
			activo,
			enjuicio,
			fechatransaccion,
			NULL OptimisticLockField,
			NULL GCRecord
	FROM [PCA-CREDIFACIL-DEV].[dbo].[$pv_prestamos]
	WHERE prestamonumerooriginal is null
) c
left join [PCA-CREDIFACIL-DEV].[dbo].[Persona] p on c.Documento=p.Documento;






GO



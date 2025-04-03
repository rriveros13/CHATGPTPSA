USE [PDN]
GO

/****** Object:  View [dbo].[vConsultasDashboard]    Script Date: 30/5/2019 14:14:49 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE view [dbo].[vConsultasDashboard]
as
select l.Numero, year(l.Fecha) as Año, Month(l.Fecha) as Mes, Day(l.Fecha) as Dia, r.Accion, m.Nombre, m.Version
, (select de.Monto from DatosEntrada de where de.LogConsultas = l.Oid) as Monto
 from LogConsultas l with (readuncommitted) join Resultado r  with (readuncommitted) on l.Oid = r.LogConsultas join Cliente c  with (readuncommitted) on c.Oid = l.Cliente
  join Modelo m  with (readuncommitted) on m.Oid = l.Modelo
where c.Nombre = 'ENTIDADX' and  l.Error = 0  and c.GCRecord is null
and l.GCRecord is null and r.Accion in ('Aceptar','Evaluar','Rechazar')
--and l.Fecha > Getdate() - 180
GO



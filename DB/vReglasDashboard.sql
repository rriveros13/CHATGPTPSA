USE [PDN]
GO

/****** Object:  View [dbo].[vReglasDashboard]    Script Date: 30/5/2019 14:15:43 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



/****** Script for SelectTopNRows command from SSMS  ******/
--Aceptar = 2,
--Rechazar = 0,
--Evaluar = 1



CREATE view [dbo].[vReglasDashboard]
as  
SELECT r.Oid ,Year(l.Fecha) as Año, Month(l.Fecha) as Mes, day(l.fecha) as Dia, 
--m.Nombre + ' v' + convert(Varchar(10),m.Version) as Modelo, m.Vigente, 
m.Nombre as Modelo, convert(Varchar(10),m.Version) as Version,
reg.Nombre as Regla, serv.Nombre as Servicio,
	    reg.Activo as Activa,
		--r.Mensaje
		--case r.Tipo when 0 then reg.CriterioRechazar when 1 then reg.CriterioEvaluar end as Criterio,
		case r.Tipo when 0 then 'Rechazar' when 1 then 'Evaluar' when 2 then 'Aceptar' end as Tipo
  FROM [PDN].[dbo].[ResultadoRegla] r with (readuncommitted) join Reglas reg with (readuncommitted) on reg.Oid = r.Regla join Modelo m with (readuncommitted) on m.oid = reg.Modelo
  join Cliente c with (readuncommitted) on c.Oid = m.Cliente join Resultado res with (readuncommitted) on res.Oid = r.Resultado
  join LogConsultas l on l.Oid = res.LogConsultas join Services serv on serv.Oid = reg.Service
  where c.Nombre = 'ENTIDADX' and r.Tipo IN (1, 0, 2) and r.CumpleCriterio = 1
 --and l.Fecha > Getdate() - 180
GO



-- PRIMERO CAMBIAR LA VISTA DE LA BD ORACLE: oracle-vista_itgf_pv_prestamos.sql

USE [PCA-CREDIFACIL-DEV]
GO

-- crear tablas nuevas 
select *
into [dbo].[$prestamos_origen]
from [PCA-CREDIFACIL-DEV].[dbo].[CuotaPagada]

select *
into [dbo].[$prestamos_cuenta]
from [PCA-CREDIFACIL-DEV].[dbo].[CuotaPagadaCuenta]

select *
into [dbo].[$codeudor_conyuge]
from [PCA-CREDIFACIL-DEV2].[dbo].[$codeudor_conyuge]

select *
into [dbo].[$pv_prestamos]
from OPENQUERY(CRFATEST, 'select * from pv_prestamos p')
where 1=2

-- ELIMINAR ESTAS TABLAS CON MUCHO CUIDADO:
/*
	[dbo].[CuotaPagada]
	[dbo].[CuotaPagadaCuenta]
	[dbo].[pv_prestamos$]
*/

--EJECUTAR SCRIPTS DEL JOB ARCHIVO: sqlserver-scripts_job_credifacil_sincronizacion_itgf.sql
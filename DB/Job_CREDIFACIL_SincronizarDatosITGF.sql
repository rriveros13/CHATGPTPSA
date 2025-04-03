USE [msdb]
GO

/****** Object:  Job [CREDIFACIL_SincronizarDatosITGF]    Script Date: 15/10/2020 14:10:44 ******/
BEGIN TRANSACTION
DECLARE @ReturnCode INT
SELECT @ReturnCode = 0
/****** Object:  JobCategory [[Uncategorized (Local)]]    Script Date: 15/10/2020 14:10:44 ******/
IF NOT EXISTS (SELECT name FROM msdb.dbo.syscategories WHERE name=N'[Uncategorized (Local)]' AND category_class=1)
BEGIN
EXEC @ReturnCode = msdb.dbo.sp_add_category @class=N'JOB', @type=N'LOCAL', @name=N'[Uncategorized (Local)]'
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback

END

DECLARE @jobId BINARY(16)
EXEC @ReturnCode =  msdb.dbo.sp_add_job @job_name=N'CREDIFACIL_SincronizarDatosITGF', 
		@enabled=1, 
		@notify_level_eventlog=0, 
		@notify_level_email=0, 
		@notify_level_netsend=0, 
		@notify_level_page=0, 
		@delete_level=0, 
		@description=N'No description available.', 
		@category_name=N'[Uncategorized (Local)]', 
		@owner_login_name=N'sa', @job_id = @jobId OUTPUT
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [Eliminar_$pv_prestamos]    Script Date: 15/10/2020 14:10:44 ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'Eliminar_$pv_prestamos', 
		@step_id=1, 
		@cmdexec_success_code=0, 
		@on_success_action=3, 
		@on_success_step_id=0, 
		@on_fail_action=2, 
		@on_fail_step_id=0, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'USE [PCA-CREDIFACIL-PROD]
GO

truncate table [PCA-CREDIFACIL-PROD].[dbo].[$pv_prestamos]


GO', 
		@database_name=N'master', 
		@flags=0
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [Eliminar_PrestamoDetalle]    Script Date: 15/10/2020 14:10:44 ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'Eliminar_PrestamoDetalle', 
		@step_id=2, 
		@cmdexec_success_code=0, 
		@on_success_action=3, 
		@on_success_step_id=0, 
		@on_fail_action=2, 
		@on_fail_step_id=0, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'USE [PCA-CREDIFACIL-PROD]
GO

truncate table [PCA-CREDIFACIL-PROD].[dbo].[PrestamoDetalle]


GO', 
		@database_name=N'master', 
		@flags=0
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [Eliminar_PrestamoCabecera]    Script Date: 15/10/2020 14:10:44 ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'Eliminar_PrestamoCabecera', 
		@step_id=3, 
		@cmdexec_success_code=0, 
		@on_success_action=3, 
		@on_success_step_id=0, 
		@on_fail_action=2, 
		@on_fail_step_id=0, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'USE [PCA-CREDIFACIL-PROD]
GO

delete from [PCA-CREDIFACIL-PROD].[dbo].[PrestamoCabecera]


GO', 
		@database_name=N'master', 
		@flags=0
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [Insertar_$pv_prestamos]    Script Date: 15/10/2020 14:10:44 ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'Insertar_$pv_prestamos', 
		@step_id=4, 
		@cmdexec_success_code=0, 
		@on_success_action=3, 
		@on_success_step_id=0, 
		@on_fail_action=2, 
		@on_fail_step_id=0, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'USE [PCA-CREDIFACIL-PROD]
GO

insert into [PCA-CREDIFACIL-PROD].[dbo].[$pv_prestamos]
select * 
from OPENQUERY(CRFAPROD, ''select * from pv_prestamos p'') p

GO', 
		@database_name=N'master', 
		@flags=0
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [Actualizar_Personas]    Script Date: 15/10/2020 14:10:44 ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'Actualizar_Personas', 
		@step_id=5, 
		@cmdexec_success_code=0, 
		@on_success_action=3, 
		@on_success_step_id=0, 
		@on_fail_action=2, 
		@on_fail_step_id=0, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'USE [PCA-CREDIFACIL-PROD]
GO

update [PCA-CREDIFACIL-PROD].[dbo].[Persona]
set Estado = (select Oid 
	FROM [PCA-CREDIFACIL-PROD].[dbo].[EstadoPersona]
	where Activo=0),
      PoseeDemanda = 1,
      Edad = [PCA-CREDIFACIL-PROD].[dbo].[getAge](FechaNacimiento)

GO', 
		@database_name=N'PCA-CREDIFACIL-PROD', 
		@flags=0
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [Sincronizar_Activos]    Script Date: 15/10/2020 14:10:44 ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'Sincronizar_Activos', 
		@step_id=6, 
		@cmdexec_success_code=0, 
		@on_success_action=3, 
		@on_success_step_id=0, 
		@on_fail_action=2, 
		@on_fail_step_id=0, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'USE [PCA-CREDIFACIL-PROD]
GO

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

GO', 
		@database_name=N'PCA-CREDIFACIL-PROD', 
		@flags=0
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [Sincronizar_Judiciales]    Script Date: 15/10/2020 14:10:44 ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'Sincronizar_Judiciales', 
		@step_id=7, 
		@cmdexec_success_code=0, 
		@on_success_action=3, 
		@on_success_step_id=0, 
		@on_fail_action=2, 
		@on_fail_step_id=0, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'USE [PCA-CREDIFACIL-PROD]
GO

update [PCA-CREDIFACIL-PROD].[dbo].[Persona]
set PoseeDemanda = 0
where Documento in (
	select p.Documento
	from [PCA-CREDIFACIL-PROD].[dbo].[V_CuotaPagada] p
	group by p.Documento, p.EnJuicio
	having p.EnJuicio=''S''
)

GO
', 
		@database_name=N'PCA-CREDIFACIL-PROD', 
		@flags=0
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [Insertar_PrestamoCabecera]    Script Date: 15/10/2020 14:10:44 ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'Insertar_PrestamoCabecera', 
		@step_id=8, 
		@cmdexec_success_code=0, 
		@on_success_action=3, 
		@on_success_step_id=0, 
		@on_fail_action=2, 
		@on_fail_step_id=0, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'USE [PCA-CREDIFACIL-PROD]
GO

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
			when MAX(v.EnJuicio)=''N'' then 1
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
		left join [PCA-CREDIFACIL-PROD].[dbo].[SolicitudPersona] sp on s.OID=sp.Solicitud and sp.TipoPersona in (select tp.Oid from [PCA-CREDIFACIL-PROD].[dbo].[TipoPersona] tp where tp.Codigo=''CON'')
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

GO', 
		@database_name=N'master', 
		@flags=0
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [Actualizar_PrestamoCabecera]    Script Date: 15/10/2020 14:10:44 ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'Actualizar_PrestamoCabecera', 
		@step_id=9, 
		@cmdexec_success_code=0, 
		@on_success_action=3, 
		@on_success_step_id=0, 
		@on_fail_action=2, 
		@on_fail_step_id=0, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'USE [PCA-CREDIFACIL-PROD]
GO

update [PCA-CREDIFACIL-PROD].[dbo].[PrestamoCabecera]
set [PCA-CREDIFACIL-PROD].[dbo].[PrestamoCabecera].PromedioAtraso = a.PromedioAtraso
from [PCA-CREDIFACIL-PROD].[dbo].[PrestamoCabecera] p
inner join (
		select c.PrestamoNumero, floor(AVG(c.Diferencia)) PromedioAtraso
		from [PCA-CREDIFACIL-PROD].[dbo].[V_CuotaPagada] c
		group by c.PrestamoNumero
	) a on p.PrestamoNumero=a.PrestamoNumero

GO', 
		@database_name=N'master', 
		@flags=0
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [Insertar_PrestamoDetalle]    Script Date: 15/10/2020 14:10:44 ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'Insertar_PrestamoDetalle', 
		@step_id=10, 
		@cmdexec_success_code=0, 
		@on_success_action=1, 
		@on_success_step_id=0, 
		@on_fail_action=2, 
		@on_fail_step_id=0, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'USE [PCA-CREDIFACIL-PROD]
GO

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

GO', 
		@database_name=N'master', 
		@flags=0
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
EXEC @ReturnCode = msdb.dbo.sp_update_job @job_id = @jobId, @start_step_id = 1
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
EXEC @ReturnCode = msdb.dbo.sp_add_jobschedule @job_id=@jobId, @name=N'Sincronizar_Datos_ITGF', 
		@enabled=1, 
		@freq_type=4, 
		@freq_interval=1, 
		@freq_subday_type=1, 
		@freq_subday_interval=0, 
		@freq_relative_interval=0, 
		@freq_recurrence_factor=0, 
		@active_start_date=20200709, 
		@active_end_date=99991231, 
		@active_start_time=193000, 
		@active_end_time=235959, 
		@schedule_uid=N'a9be234a-d007-447e-9e3e-59b260a62060'
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
EXEC @ReturnCode = msdb.dbo.sp_add_jobserver @job_id = @jobId, @server_name = N'(local)'
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
COMMIT TRANSACTION
GOTO EndSave
QuitWithRollback:
    IF (@@TRANCOUNT > 0) ROLLBACK TRANSACTION
EndSave:
GO



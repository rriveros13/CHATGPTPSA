USE [PCA-CREDIFACIL-PROD]
GO

/****** Object:  StoredProcedure [dbo].[EliminarSolicitud]    Script Date: 15/1/2021 15:04:19 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[EliminarSolicitud]
	-- Add the parameters for the stored procedure here
	@OID int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	delete [PCA-CREDIFACIL-PROD].[dbo].[Resultado]
	where Solicitud in (SELECT OID
	FROM [PCA-CREDIFACIL-PROD].[dbo].[Solicitud]
	where OID=@OID)

	delete [PCA-CREDIFACIL-PROD].[dbo].[Adjunto]
	where Solicitud in (SELECT OID
	FROM [PCA-CREDIFACIL-PROD].[dbo].[Solicitud]
	where OID=@OID)

	delete [PCA-CREDIFACIL-PROD].[dbo].[SolicitudPersonaIngreso]
	where SolicitudPersona in (SELECT Oid
	FROM [PCA-CREDIFACIL-PROD].[dbo].[SolicitudPersona]
	where Solicitud in (SELECT OID
	FROM [PCA-CREDIFACIL-PROD].[dbo].[Solicitud]
	where OID=@OID))

	delete [PCA-CREDIFACIL-PROD].[dbo].[SolicitudPersonaRefPer]
	where SolicitudPersona in (SELECT Oid
	FROM [PCA-CREDIFACIL-PROD].[dbo].[SolicitudPersona]
	where Solicitud in (SELECT OID
	FROM [PCA-CREDIFACIL-PROD].[dbo].[Solicitud]
	where OID=@OID))

	delete [PCA-CREDIFACIL-PROD].[dbo].[SolicitudPersonaRefCom]
	where SolicitudPersona in (SELECT Oid
	FROM [PCA-CREDIFACIL-PROD].[dbo].[SolicitudPersona]
	where Solicitud in (SELECT OID
	FROM [PCA-CREDIFACIL-PROD].[dbo].[Solicitud]
	where OID=@OID))

	delete [PCA-CREDIFACIL-PROD].[dbo].[SolicitudPersona]
	where Solicitud in (SELECT OID
	FROM [PCA-CREDIFACIL-PROD].[dbo].[Solicitud]
	where OID=@OID)

	delete [PCA-CREDIFACIL-PROD].[dbo].[HistorialTarea]
	where Tarea in (SELECT Oid
	FROM [PCA-CREDIFACIL-PROD].[dbo].[Tarea]
	where Solicitud in (SELECT OID
	FROM [PCA-CREDIFACIL-PROD].[dbo].[Solicitud]
	where OID=@OID))

	delete [PCA-CREDIFACIL-PROD].[dbo].[Tarea]
	where Solicitud in (SELECT OID
	FROM [PCA-CREDIFACIL-PROD].[dbo].[Solicitud]
	where OID=@OID)

	delete [PCA-CREDIFACIL-PROD].[dbo].[RegistroEstadoSolicitud]
	where Solicitud in (SELECT OID
	FROM [PCA-CREDIFACIL-PROD].[dbo].[Solicitud]
	where OID=@OID)

	delete [PCA-CREDIFACIL-PROD].[dbo].[NotaSolicitud]
	where Solicitud in (SELECT OID
	FROM [PCA-CREDIFACIL-PROD].[dbo].[Solicitud]
	where OID=@OID)

	delete [PCA-CREDIFACIL-PROD].[dbo].[PrestamoCuota]
	where Prestamo in (SELECT Oid
	FROM [PCA-CREDIFACIL-PROD].[dbo].[PresupuestoPrestamo]
	where Presupuesto in (SELECT Oid
	FROM [PCA-CREDIFACIL-PROD].[dbo].[Presupuesto]
	where Solicitud in (SELECT OID
	FROM [PCA-CREDIFACIL-PROD].[dbo].[Solicitud]
	where OID=@OID)))

	delete [PCA-CREDIFACIL-PROD].[dbo].[PresupuestoGasto]
	where PresupuestoPrestamo in (SELECT Oid
	FROM [PCA-CREDIFACIL-PROD].[dbo].[PresupuestoPrestamo]
	where Presupuesto in (SELECT Oid
	FROM [PCA-CREDIFACIL-PROD].[dbo].[Presupuesto]
	where Solicitud in (SELECT OID
	FROM [PCA-CREDIFACIL-PROD].[dbo].[Solicitud]
	where OID=@OID)))

	delete [PCA-CREDIFACIL-PROD].[dbo].[PresupuestoPrestamo]
	where Presupuesto in (SELECT Oid
	FROM [PCA-CREDIFACIL-PROD].[dbo].[Presupuesto]
	where Solicitud in (SELECT OID
	FROM [PCA-CREDIFACIL-PROD].[dbo].[Solicitud]
	where OID=@OID))

	delete [PCA-CREDIFACIL-PROD].[dbo].[Presupuesto]
	where Solicitud in (SELECT OID
	FROM [PCA-CREDIFACIL-PROD].[dbo].[Solicitud]
	where OID=@OID)

	delete [PCA-CREDIFACIL-PROD].[dbo].[Solicitud]
	where OID=@OID

END
GO



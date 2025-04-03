USE [PCA-CREDIFACIL-DEV]
GO

/****** Object:  UserDefinedFunction [dbo].[CalcEdad]    Script Date: 15/1/2021 09:48:13 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
CREATE FUNCTION [dbo].[CalcEdad] 
(
	-- Add the parameters for the function here
	@FECHA_NACIMIENTO as datetime
)
RETURNS int
AS
BEGIN
	declare @FECHA_REFERENCIA datetime = GETDATE()
	declare @edad int

	if (@FECHA_NACIMIENTO is null or @FECHA_NACIMIENTO > @FECHA_REFERENCIA) 
		RETURN 0
	
	set @edad = (SELECT ((YEAR(@FECHA_REFERENCIA)-YEAR(@FECHA_NACIMIENTO))
	- (CASE WHEN MONTH(@FECHA_REFERENCIA)>MONTH(@FECHA_NACIMIENTO) THEN 0 
		WHEN MONTH(@FECHA_REFERENCIA)<MONTH(@FECHA_NACIMIENTO) THEN 1 
		WHEN DAY(@FECHA_REFERENCIA)>=DAY(@FECHA_NACIMIENTO) THEN 0
		WHEN (MONTH(@FECHA_NACIMIENTO)=2 AND DAY(@FECHA_NACIMIENTO)=29) AND DAY(@FECHA_REFERENCIA)=28
			AND NOT (YEAR(@FECHA_REFERENCIA)%400=0 OR (YEAR(@FECHA_REFERENCIA)%4=0 AND YEAR(@FECHA_REFERENCIA)%100!=0))
				THEN 0
			ELSE 1 END)))

	RETURN @edad

END
GO



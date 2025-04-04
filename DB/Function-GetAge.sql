USE [PCA-CREDIFACIL-DEV]
GO
/****** Object:  UserDefinedFunction [dbo].[getAge]    Script Date: 15/10/2020 12:01:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
ALTER FUNCTION [dbo].[getAge] 
(
	-- Add the parameters for the function here
	@fechaNacimiento as datetime
)
RETURNS int
AS
BEGIN

	declare @edad int

	if (@fechaNacimiento is null or @fechaNacimiento > GETDATE()) 
		RETURN 0
	
	set @edad = DATEDIFF(YEAR, @fechaNacimiento, GETDATE())

	if (GETDATE()<@fechaNacimiento and DATEADD(YEAR, DATEDIFF(YEAR, @fechaNacimiento, GETDATE()), GETDATE()) > @fechaNacimiento)
		set @edad = @edad - 1

	if (GETDATE()>@fechaNacimiento and DATEADD(YEAR, DATEDIFF(YEAR, @fechaNacimiento, GETDATE()), GETDATE()) < @fechaNacimiento)
		set @edad = @edad + 1

	RETURN @edad

END

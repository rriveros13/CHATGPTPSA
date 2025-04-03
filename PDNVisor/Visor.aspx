<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Visor.aspx.cs" Inherits="PDNVisor.Visor" %>
<%@ Register TagPrefix="cc1" Namespace="GdPicture14.WEB" Assembly="GdPicture.NET.14.WEB.DocuVieware" %>

<!DOCTYPE html>

<html>

<head runat="server">
    <title>ITTI Visor de Imágenes/Documentos</title>
    <meta name="viewport" content="width=device-width, initial-scale=1"/>
    <link href="favicon.ico" rel="icon" />
    <link href="style/standalone_viewer.css" rel="stylesheet" type="text/css" />
</head>
<body>

<form id="form1" runat="server">
    <cc1:DocuVieware ID="DocuVieware1" runat="server"
                     Height="100%"
                     Width="100%"
                     SinglePageView="False"
                     ForceScrollBars="False"
                     AllowedExportFormats="*"
                     MaxUploadSize="52428800"
                     CollapsedSnapIn="True"
                     EnableMouseModeButtons="False"
                     AllowUpload="false"
                     EnableTwainAcquisitionButton="false"
                     EnableFormFieldsEdition="false"
                     EnableLoadFromUriButton="false"
                     ShowAnnotationsSnapIn="false"
                     ShowAnnotationsCommentsSnapIn="false"
                     AllowPrint="True"
                     />
</form>
</body>
</html>

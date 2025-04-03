<%@ Page Language="C#" AutoEventWireup="true" Inherits="Default" EnableViewState="false"
    ValidateRequest="false" CodeBehind="Default.aspx.cs" %>
<%@ Register Assembly="DevExpress.ExpressApp.Web.v24.2, Version=24.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.ExpressApp.Web.Controls" TagPrefix="cc4" %>
<!DOCTYPE html>
<html>
<head runat="server">
    <title>Main Page</title>
    <meta http-equiv="Expires" content="0" />
</head>
<body class="VerticalTemplate">
    <form id="form2" runat="server">
    <cc4:ASPxProgressControl ID="ProgressControl" runat="server" />
    <div runat="server" id="Content" />
    </form>
    <script>
        function CheckErrorMessageScroll() {
            var windowScroll = $(window.top.document).scrollTop();
            var headerTableScroll = document.getElementById("headerTable").scrollHeight;
            var errorMessage = $('.ErrorMessage');
            if (windowScroll > headerTableScroll) {
                errorMessage.css({
                    position: 'fixed',
                    top: '75px',
                    left: 0,
                    right: 0
                });
            } else {
                errorMessage.css({
                    position: 'inherit',
                });
            }
        }
        $(window).scroll(function () {
            CheckErrorMessageScroll();
        });
    </script>
</body>
</html>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RptViewer.aspx.cs" Inherits="BrilliantWMS.WMS.RptViewer" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rvwms" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Report Viewer</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <rvwms:ReportViewer ID="ReportViewer1" runat="server" Width="950px" Height="700px"></rvwms:ReportViewer>
    </div>
    </form>
</body>
</html>

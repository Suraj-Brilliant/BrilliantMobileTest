<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReportViewer.aspx.cs" Inherits="BrilliantWMS.WMS.WMSReport.ReportViewer" EnableEventValidation="false"%>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rvwms" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Brilliant WMS</title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <rvwms:ReportViewer ID="RptViewer1" runat="server" Width="100%" Font-Names="Verdana" Font-Size="8pt" SizeToReportContent="true" EnableExternalImages="true" OnLoad="RptViewer1_Load"
            WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" Height="100%">
            <LocalReport ReportPath="WMS/WMSReport/LabelPrinting.rdlc"></LocalReport>
        </rvwms:ReportViewer>
    <div>
    
    </div>
    </form>
</body>
</html>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReportViewer.aspx.cs" Inherits="BrilliantWMS.POR.Reports.ReportViewer"
    EnableEventValidation="false" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
   

</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="Scriptmanager1" runat="server">
    </asp:ScriptManager>     
    <div>
        <asp:Button ID="ntncsv" runat="server" Text="Export to CSV" OnClick="bntncsv_Click"/>
        <rsweb:ReportViewer ID="RptViewer1" Width="100%" Font-Names="Verdana" Font-Size="8pt" SizeToReportContent="true" EnableExternalImages="true" onload="RptViewer1_Load"
            WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" runat="server" Height="100%">
            <LocalReport ReportPath="POR/Reports/PartRequestList.rdlc">
            </LocalReport>
        </rsweb:ReportViewer>
    </div>
<%--<asp:Button ID="ntncsv" runat="server" Text="Export to CSV" OnClick="ntncsv_Click"/>--%>
    </form>
</body>
</html>

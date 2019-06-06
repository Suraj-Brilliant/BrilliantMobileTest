<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Loader.aspx.cs" Inherits="BrilliantWMS.WMS.Loader" Theme="Blue" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="Obout.Ajax.UI" Namespace="Obout.Ajax.UI.HTMLEditor" TagPrefix="obout" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Brilliant WMS</title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true" />
        <div>
            <table class="tableForm" width="100%" ><%--style="background-color: White;"--%>
                <tr>
                    <td>Loader Name :
                    </td>
                    <td style="text-align: left; font-weight: bold;">
                        <asp:DropDownList ID="ddlLoaderName" runat="server" Width="182px" DataTextField="Name"
                            DataValueField="ID" ClientIDMode="Static">
                        </asp:DropDownList>
                    </td>
                    <td>
                        In Time :
                    </td>
                    <td style="text-align: left; font-weight: bold;">
                        <asp:TextBox ID="txtInTime" runat="server" Width="180px" ></asp:TextBox>
                    </td>
                    <td>
                        Out Time :
                    </td>
                    <td style="text-align: left; font-weight: bold;">
                        <asp:TextBox ID="txtOutTime" runat="server" Width="180px" ></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        Box Handled :
                    </td>
                    <td style="text-align: left; font-weight: bold;">
                        <asp:TextBox ID="txtBoxhandled" runat="server" Width="180px" ></asp:TextBox>
                    </td>
                    <td>
                        Rate Per Box :
                    </td>
                    <td style="text-align: left; font-weight: bold;">
                        <asp:TextBox ID="txtRate" runat="server" Width="180px" ></asp:TextBox>
                    </td>
                    <td>
                        Total :
                    </td>
                    <td style="text-align: left; font-weight: bold;">
                        <asp:TextBox ID="txtTotal" runat="server" Width="180px" ></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="6" style="text-align: right; font-weight: bold;">
                        <asp:LinkButton ID="btnSubmit" runat="server" Text="Submit" Style="background: #1a527d;
                        border: 1px solid #1a527d; color: #ddd; list-style-type: none; margin: 0 !important;
                        padding: 2px 3px;"  ></asp:LinkButton> <%--OnClientClick="return jsSaveCorrespondance();" OnClick="btnSubmit_Click"--%>
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>

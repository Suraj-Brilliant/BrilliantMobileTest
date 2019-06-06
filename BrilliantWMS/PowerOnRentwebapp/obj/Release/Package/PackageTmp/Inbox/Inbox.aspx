<%@ Page Title="Power On Rent | Inbox" Language="C#" MasterPageFile="~/MasterPage/CRM.Master" AutoEventWireup="true"
    CodeBehind="Inbox.aspx.cs" Inherits="BrilliantWMS.Inbox.Inbox" %>

<%@ Register Src="../DashBoard/DashBoard.ascx" TagName="DashBoard" TagPrefix="uc1" %>
<%@ Register src="../CommonControls/UCFormHeader.ascx" tagname="UCFormHeader" tagprefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHolder_FormHeader" runat="server">
    <uc2:UCFormHeader ID="UCFormHeader1" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPHolder_Form" runat="server">
    <center>
        <table style="width: 100%;">
            <tr>
                <td style="width: 80%; vertical-align: top; background-color: White;">
                    <iframe runat="server" src="../Inbox/PORInbox.aspx" id="iframeInbox" width="99%"
                        style="border: none; min-height: 500px;"></iframe>
                </td>
                <td style="width: 20%; vertical-align: top;">
                    <uc1:DashBoard ID="DashBoard1" runat="server" />
                </td>
            </tr>
        </table>
    </center>
</asp:Content>

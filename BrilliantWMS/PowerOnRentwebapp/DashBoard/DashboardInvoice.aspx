<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/CRM.Master" AutoEventWireup="true"
    CodeBehind="DashboardInvoice.aspx.cs" Inherits="BrilliantWMS.DashBoard.DashboardInvoice" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHolder_FormHeader" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPHolder_Form" runat="server">
    <center>
        <asp:UpdatePanel ID="updPnl_Dashboard" runat="server">
            <ContentTemplate>
                <asp:RadioButtonList ID="rbtnyear" RepeatDirection="Horizontal" Font-Size="Medium"
                    Font-Bold="true" DataTextField="Year" DataValueField="Year" AutoPostBack="true"
                    runat="server" OnSelectedIndexChanged="rbtnyear_SelectedIndexChanged">
                </asp:RadioButtonList>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:PlaceHolder ID="plsDashboard" runat="server"></asp:PlaceHolder>
    </center>
</asp:Content>

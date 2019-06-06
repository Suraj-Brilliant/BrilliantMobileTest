<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/CRM.Master" AutoEventWireup="true"
    CodeBehind="TestFilter.aspx.cs" Inherits="BrilliantWMS.PowerOnRent.TestFilter"
    Theme="Blue" %>

<%@ Register Src="UCCommonFilter.ascx" TagName="UCCommonFilter" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHolder_FormHeader" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPHolder_Form" runat="server">
    <uc1:UCCommonFilter ID="UCCommonFilter1" runat="server" />
</asp:Content>

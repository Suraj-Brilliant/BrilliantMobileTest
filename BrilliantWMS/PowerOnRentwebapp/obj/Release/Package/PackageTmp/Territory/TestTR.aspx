<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/CRM.Master" AutoEventWireup="true"
    CodeBehind="TestTR.aspx.cs" Inherits="BrilliantWMS.Territory.TestTR" Theme="Blue" %>

<%@ Register Src="UC_Territory.ascx" TagName="UC_Territory" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHolder_FormHeader" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPHolder_Form" runat="server">
    <uc1:UC_Territory ID="UC_Territory1" runat="server" />
    <br />
    <uc1:UC_Territory ID="UC_Territory2" runat="server" />
</asp:Content>

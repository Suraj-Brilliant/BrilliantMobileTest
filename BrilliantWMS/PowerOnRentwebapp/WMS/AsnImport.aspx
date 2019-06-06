<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/CRM.Master" AutoEventWireup="true" CodeBehind="AsnImport.aspx.cs" Inherits="BrilliantWMS.WMS.AsnImport" Theme="Blue" %>
<%@ Register Src="../CommonControls/UCImport.ascx" TagName="UCImport" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHolder_FormHeader" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPHolder_Form" runat="server">
     <asp:Panel ID="tabContactInfo" runat="server" HeaderText="Contact Person Info">        
                <ContentTemplate>                    
                    <uc1:UCImport ID="UcImport" runat="server" />
                </ContentTemplate>
            </asp:Panel>
</asp:Content>

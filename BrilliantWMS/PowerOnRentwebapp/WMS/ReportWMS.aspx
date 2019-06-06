<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/CRM.Master" AutoEventWireup="true" CodeBehind="ReportWMS.aspx.cs" Inherits="BrilliantWMS.WMS.ReportWMS" Theme="Blue" %>

<%@ Register Src="~/CommonControlReport/UC_RptFilter.ascx" TagPrefix="uc1" TagName="UC_RptFilter" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHolder_FormHeader" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPHolder_Form" runat="server">
    <center>
        <div>
            <uc1:UC_RptFilter runat="server" ID="UC_RptFilter" style="flex-align:center"/>
        </div>
        <asp:HiddenField ID="hdnListVal" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="usrID" runat="server" ClientIDMode="Static" />
    </center>
    <style type="text/css">
    .modal
    {
        display: none; /* Hidden by default */
        position: fixed; /* Stay in place */
        z-index: 1; /* Sit on top */
        left: 0;
        top: 0;
        width: 100%; /* Full width */
        height: 100%; /* Full height */
        overflow: auto; /* Enable scroll if needed */
        background-color: rgb(0,0,0); /* Fallback color */
        background-color: rgba(0,0,0,0.4); /* Black w/ opacity */
    }

    /* Modal Content/Box */
    .modal-content
    {
        background-color: #fefefe;
        margin: 15% auto; /* 15% from the top and centered */
        padding: 20px;
        border: 1px solid #888;
        width: 80%; /* Could be more or less, depending on screen size */
    }

    /* The Close Button */
    .close
    {
        color: #aaa;
        float: right;
        font-size: 28px;
        font-weight: bold;
    }

        .close:hover,
        .close:focus
        {
            color: black;
            text-decoration: none;
            cursor: pointer;
        }
</style>
</asp:Content>

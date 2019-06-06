<%@ Page Title="Power On Rent | Dashboard" Language="C#" MasterPageFile="~/MasterPage/CRM.Master"
    AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="BrilliantWMS.DashBoard.Dashboard"
    Theme="Blue" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHolder_FormHeader" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPHolder_Form" runat="server">
    <style type="text/css">
        .divheader
        {
            border: solid 1px gray;
            border-top-left-radius: 10px;
            -moz-border-top-left-radius: 10px; /* Old Firefox */
            border-top-right-radius: 10px;
            -moz-border-top-right-radius: 10px; /* Old Firefox */
            height: 280px;
            border-bottom: solid 1px gray;
            text-align: left;
            padding: 4px 4px 0px 4px;
            margin-bottom: 4px;
            font-size: 14px;
            color: black;
            margin-left: 2px;
        }
        
        .dashboardtable
        {
            border: none;
            width: 100%;
        }
        .dashboardtable td
        {
            vertical-align: top;
            width: 33%;
        }
    </style>
    <table style="width: 100%;">
        <tr>
            <td style="width: 66%; vertical-align: top">
                <table class="dashboardtable" id="tableLeft" runat="server">
                </table>
            </td>
            <td style="width: 33%; vertical-align: top">
                <table class="dashboardtable" id="tableRight" runat="server">
                </table>
            </td>
        </tr>
    </table>
    
</asp:Content>

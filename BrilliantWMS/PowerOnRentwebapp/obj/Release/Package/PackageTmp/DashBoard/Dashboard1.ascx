<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Dashboard1.ascx.cs"
    Inherits="BrilliantWMS.DashBoard.Dashboard1" %>
<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>
<style type="text/css">
    .divheader
    {
        border: solid 1px gray;
        border-top-left-radius: 10px;
        -moz-border-top-left-radius: 10px; /* Old Firefox */
        border-top-right-radius: 10px;
        -moz-border-top-right-radius: 10px; /* Old Firefox */
        height: 22px;
        border-bottom: none;
        text-align: left;
        padding: 4px 4px 0px 4px;
        font-size: 14px;
        color: black;
    }
    
    .divdetail
    {
        border: solid 1px gray;
        height: 260px;
    }
</style>
<div id="divHeader1" class="divheader">
    <asp:Label runat="server" ID="lblDashboardTitle"></asp:Label>
</div>
<div id="divDetail1" class="divdetail">
    <asp:Chart ID="Chart1" runat="server">
        <Series>
        </Series>
        <ChartAreas>
        </ChartAreas>
    </asp:Chart>
</div>

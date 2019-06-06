<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BindDashboard.aspx.cs"
    Inherits="BrilliantWMS.DashBoard.BindDashboard" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <style type="text/css">
        .divDashboard
        {
            border: solid 1px gray;
            border-top-left-radius: 10px;
            -moz-border-top-left-radius: 10px; /* Old Firefox */
            border-top-right-radius: 10px;
            -moz-border-top-right-radius: 10px; /* Old Firefox */
            height: 280px;
            border-bottom: solid 1px gray;
            text-align: left; /*padding: 4px 4px 0px 4px;*/
            margin-bottom: 4px;
            font-size: 14px;
            color: black;
            margin-left: 2px;
        }
        
        .divDahsboardHeader
        {
            border-bottom: solid 1px gray;
            height: 22px;
            padding: 2px 5px 2px 5px;
            font-size: 14px;
            text-align: left;
            color: Black;
            font-family: Tahoma;
        }
        
        .divDashboardDetail
        {
            border: solid 1px red;
            height: 88%;
            padding: 2px 2px 2px 2px;
        }
    </style>
    <div>
        <div id="divHeader1" class="divDashboard">
            <div class="divDahsboardHeader">
                <asp:Label runat="server" ID="lblDashboardTitle">Site wise Consumption</asp:Label>
            </div>
            <div class="divDashboardDetail">
                <asp:Label runat="server" ID="lblMsg"></asp:Label>
                <asp:HiddenField runat="server" ID="hdnDashboardID" />
                <asp:Chart ID="Chart1" runat="server">
                    <Series>
                    </Series>
                    <ChartAreas>
                    </ChartAreas>
                </asp:Chart>
            </div>
        </div>
    </div>
    </form>
</body>
</html>

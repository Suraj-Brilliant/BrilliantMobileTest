<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DashBoard.ascx.cs" Inherits="BrilliantWMS.DashBoard.DashBoard" %>
<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>
<%@ Register Assembly="obout_Flyout2_NET" Namespace="OboutInc.Flyout2" TagPrefix="cc1" %>
<style type="text/css">
    #imgListReport: hover
    {
        cursor: pointer;
    }
</style>
<div style="float: left;">
    <center>
        <table>
            <tr>
                <td align="center">
                    <asp:Label ID="lblMsg" runat="server" Style="font-size: 12px; color: Gray;"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Panel runat="server" ID="pnlMainChart" ScrollBars="None">
                        <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="pdf_icon.jpg" Style="position: relative;
                            left: 0; float: left; z-index: 999;" OnClick="ImageButton1_Click" />
                        <asp:Image ID="imgListReport" runat="server" ImageUrl="data_icon.jpg" Style="position: relative;
                            left: 0; float: left; z-index: 999;" />
                        <div style="position: relative; height: auto; z-index: 9;">
                            <asp:Chart ID="Chart1" runat="server">
                                <Series>
                                </Series>
                                <ChartAreas>
                                </ChartAreas>
                            </asp:Chart>
                        </div>
                    </asp:Panel>
                </td>
            </tr>
        </table>
    </center>
    <asp:HiddenField ID="hfRepeortid" runat="server" />
    <asp:HiddenField ID="hfConnectionString" runat="server" />
    <asp:HiddenField ID="hfQueryParameter" runat="server" />
    <asp:HiddenField ID="hfQueryFlag" runat="server" />
    <asp:HiddenField ID="hfQuery" runat="server" />
    <cc1:Flyout ID="Flyout1" AttachTo="imgListReport" OpenEvent="ONCLICK" Position="BOTTOM_RIGHT"
        zIndex="1000" CloseEvent="NONE" runat="server">
        <div style="border: solid 2px black; background-color: #ffffff; padding: 10px; width: auto;
            margin: 10px;">
            <a href="#" onclick="<%=Flyout1.getClientID()%>.Close();" style="float: right; border: none;
                position: relative; margin: -20px -20px 0 0;">
                <img style="border: none;" src="../DashBoard/R.png" height="25px" width="25px" alt="close" />
            </a>
            <asp:ImageButton ID="ImageButton2" ImageUrl="excel_icon.jpg" runat="server" OnClick="ImageButton2_Click"
                Style="float: left;" />
            <asp:Panel runat="server" ID="pnl1" Height="300px" Width="300px" ScrollBars="Auto">
                <table>
                    <tr>
                        <td>
                            <asp:GridView ID="GV_DashData" runat="server" CellPadding="4" ForeColor="#333333"
                                GridLines="None" Width="100%" SkinID="x">
                                <AlternatingRowStyle BackColor="White" />
                                <EditRowStyle BackColor="#2461BF" />
                                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" Font-Size="10px"
                                    HorizontalAlign="Left" />
                                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" Font-Size="10px"
                                    HorizontalAlign="Left" />
                                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" Font-Size="10px" />
                                <RowStyle BackColor="#EFF3FB" Font-Size="10px" />
                                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                <SortedAscendingCellStyle BackColor="#F5F7FB" />
                                <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                                <SortedDescendingCellStyle BackColor="#E9EBEF" />
                                <SortedDescendingHeaderStyle BackColor="#4870BE" />
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </div>
    </cc1:Flyout>
</div>

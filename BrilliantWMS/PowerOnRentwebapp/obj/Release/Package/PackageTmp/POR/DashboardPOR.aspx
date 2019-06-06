<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/CRM.Master" AutoEventWireup="true"
    CodeBehind="DashboardPOR.aspx.cs" Inherits="BrilliantWMS.POR.DashboardPOR" %>

<%@ Register Src="../DashBoard/DashBoard.ascx" TagName="DashBoard" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHolder_FormHeader" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPHolder_Form" runat="server">
    <center>
         <%--<div>
            <table>
               <tr>
                    <td rowspan="2">
                        <a href="../POR/DashboardPOR.aspx?invoker=postatus">
                            <img alt="" src="../commonControls/HomeSetupImg/dashboard.jpg" />
                        </a>
                    </td>
                    <td style="text-align: left">
                        <a href="../POR/DashboardPOR.aspx?invoker=postatus" class="ParentGroup">PO Status Wise </a>
                    </td>
                    <td rowspan="2">
                        <a href="../POR/DashboardPOR.aspx?invoker=sostatus">
                            <img alt="" src="../commonControls/HomeSetupImg/dasbaord2.png" />
                        </a>
                    </td>
                    <td style="text-align: left">
                        <a href="../POR/DashboardPOR.aspx?invoker=sostatus" class="ParentGroup">SO Status Wise</a>
                    </td>
                    <td rowspan="2">
                        <a href="../POR/DashboardPOR.aspx?invoker=purchaseorder">
                            <img alt="" src="../commonControls/HomeSetupImg/Dashboad1.png" /></a>
                    </td>
                    <td style="text-align: left">
                        <a href="../POR/DashboardPOR.aspx?invoker=purchaseorder" class="ParentGroup">Purchase Order</a>
                    </td>

                     <td rowspan="2">
                        <a href="../POR/DashboardPOR.aspx?invoker=salesorder">
                            <img alt="" src="../commonControls/HomeSetupImg/Dashboad1.png" /></a>
                    </td>
                    <td style="text-align: left">
                        <a href="../POR/DashboardPOR.aspx?invoker=salesorder" class="ParentGroup">Sale Order</a>
                    </td>
                       <td rowspan="2">
                        <a href="../POR/DashboardPOR.aspx?invoker=utilization">
                            <img alt="" src="../commonControls/HomeSetupImg/Dashboad1.png" /></a>
                    </td>
                    <td style="text-align: left">
                        <a href="../POR/DashboardPOR.aspx?invoker=utilization" class="ParentGroup">Utilization</a>
                    </td>
                    <td rowspan="2">
                        <a href="../POR/DashboardPOR.aspx?invoker=taskperformance">
                            <img alt="" src="../commonControls/HomeSetupImg/Dashboad1.png" /></a>
                    </td>
                    <td style="text-align: left">
                        <a href="../POR/DashboardPOR.aspx?invoker=taskperformance" class="ParentGroup">Task Performance</a>
                    </td>
                    <td style="text-align: left">
                        <a href="../POR/Dashboard_Enginewise.aspx" class="ParentGroup">Engine Wise Consumption</a>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: left">
                        <a href="../POR/DashboardPOR.aspx?invoker=postatus">Purchase Order Status</a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        </br> Consumption All Site
                    </td>
                    <td style="text-align: left">
                        <a href="../POR/DashboardPOR.aspx?invoker=sostatus">Sale Order Status</a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        </br> Partwise Consumption As on date
                    </td>
                    <td style="text-align: left">
                        <a href="../POR/DashboardPOR.aspx?invoker=purchaseorder">Purchase Order</a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        </br> Consumption for All Engine
                    </td>
                      <td style="text-align: left">
                        <a href="../POR/DashboardPOR.aspx?invoker=salesorder">Sales Order</a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        </br> Consumption for All Engine
                    </td>
                    <td style="text-align: left">
                        <a href="../POR/DashboardPOR.aspx?invoker=utilization">Utilization</a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        </br> Consumption for All Engine
                    </td>
                     <td style="text-align: left">
                        <a href="../POR/DashboardPOR.aspx?invoker=taskperformance">Task Performance</a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        </br> Consumption for All Engine
                    </td>
                    <td style="text-align: left">
                        <a href="../POR/Dashboard_Enginewise.aspx">Engine Wise Consumption</a> </br> Engine
                        Wise Consumption of Part
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                    </td>
                </tr>
            </table>
        </div>--%>
        <center>
            <div>
                <table>
                    <tr>
                        <td rowspan="3">
                            <%-- <uc1:DashBoard ID="DashBoard1" runat="server" />--%>
                            <uc1:DashBoard ID="DashBoard1" runat="server" />
                        </td>
                    </tr>
                </table>
            </div>
        </center>
        <%--  <uc1:DashBoard ID="DashBoard2" runat="server" />
        <uc1:DashBoard ID="DashBoard3" runat="server" />--%>
    </center>
    <script type="text/javascript">
    </script>
</asp:Content>

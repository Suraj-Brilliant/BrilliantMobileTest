<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/CRM.Master" AutoEventWireup="true"
    EnableEventValidation="false" CodeBehind="CommonReport.aspx.cs" Inherits="BrilliantWMS.PowerOnRent.CommonReport"
    Theme="Blue" %>

<%@ Register Src="UCCommonFilter.ascx" TagName="UCCommonFilter" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHolder_FormHeader" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPHolder_Form" runat="server">
    <center>
        <div id="divLoading" style="height: 71%; width: 50%; display: none; top: 40; left: 260px; background-color: transparent;"class="modal">
            <center>
                <img src="../App_Themes/Blue/img/ajax-loader.gif" alt="" style="top: 50%; margin-top: 22%" />
                <br />
                <br />
                <b>Please Wait...</b>
            </center>
        </div>
        <%-- <div class="divDetailExpandPopUpOff" id="divPopUp">
            <center>
                <div class="popupClose" onclick="CloseShowReport()">
                </div>
                <div class="divDetailExpand" id="div1">
                    <iframe runat="server" id="iframePORRpt" clientidmode="Static" src="#" width="80%"
                        style="border: none; height: 550px;"></iframe>
                </div>
            </center>
        </div>--%>        
    </center>
    <center>
        <table>
            
            <tr>
                <td>
                    <uc1:UCCommonFilter ID="UCCommonFilter1" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    <%--<div id="rptbutton" runat="server" style="padding: 0 0 0 67%;">--%>
                    <div id="rptbutton" runat="server">
                        <input type="button" runat="server" value="View Report" id="btnViewReport" onclick="selectedProductRec(); jsGetReportData();" />
                        <input type="button" runat="server" value="SKU Transaction Report" id="btnSKUTransaction" onclick="selectedProductRec(); jsGetSKUTransactionReport();" />
                        <input type="button" runat="server" value="User Transaction Report" id="btnusertransaction" onclick="selectedProductRec(); jsGetUserTransactionReport();" />

                        <input type="button" runat="server" value="View Purchase Order List" id="btnPOList" onclick="selectedProductRec(); jsGetPOList();" />
                        <input type="button" runat="server" value="View Purchase Order Detail" id="btnPODetail" onclick="selectedProductRec(); jsGetReportData();" />

                        <input type="button" runat="server" value="View GRN List" id="btngrnList" onclick="selectedProductRec(); jsGetgrnList();" />
                        <input type="button" runat="server" value="View GRN Detail" id="btngrnDetail" onclick="selectedProductRec(); jsGetgrndetail();" />

                        <input type="button" runat="server" value="View QC List" id="btnqcList" onclick="selectedProductRec(); jsGetqcList();" />
                        <input type="button" runat="server" value="View QC Detail" id="btnqcDetail" onclick="selectedProductRec(); jsGetqcdetail();" />

                        <input type="button" runat="server" value="View PutIn List" id="btnputinList" onclick="selectedProductRec(); jsGetputinList();" />
                        <input type="button" runat="server" value="View PutIn Detail" id="btnputinDetail" onclick="selectedProductRec(); jsGetputindetail();" />

                         <input type="button" runat="server" value="View Sales Order List" id="btnOrderList" onclick="selectedProductRec(); jsGetorderList();" />
                        <input type="button" runat="server" value="View Sales Order Detail" id="btnOrderDetail" onclick="selectedProductRec(); jsGetorderdetail();" />

                        <input type="button" runat="server" value="View PickUP List" id="btnpickupList" onclick="selectedProductRec(); jsGetpickupList();" />
                        <input type="button" runat="server" value="View PickUP Detail" id="btnpickDetail" onclick="selectedProductRec(); jsGetpickupdetail();" />

                        <input type="button" runat="server" value="View Dispatch List" id="btndispatchList" onclick="selectedProductRec(); jsGetdispatchList();" />
                        <input type="button" runat="server" value="View Dispatch Detail" id="btndispatchDetail" onclick="selectedProductRec(); jsGetdispatchdetail();" />
                    </div>

                    <%-- <asp:Button ID="btnViewReport" Text="View Report" runat="server" OnClick="btnViewReport_Click"
                        CausesValidation="false" />--%>
                </td>
            </tr>
        </table>
    </center>
    <script type="text/javascript">
        //        jsCheckIssueHistory();
        function onselectA(invoker) {
            var allA = tblRptMenu.getElementsByTagName('a');
            for (var i = 0; i < allA.length; i++) {
                allA[i].className = '';
            }
            invoker.className = "aselected";
        }
        function CloseShowReport() {
            LoadingOff();
            divPopUp.className = "divDetailExpandPopUpOff";
        }
    </script>
    <style type="text/css">
        .popupClose {
            background: url(../App_Themes/Blue/img/icon_close.png) no-repeat;
            height: 32px;
            width: 32px;
            float: right;
            margin-top: -30px;
            margin-right: -25px;
        }

            .popupClose:hover {
                cursor: pointer;
            }

        .divDetailExpandPopUpOff {
            display: none;
        }

        .divDetailExpandPopUpOn {
            border: solid 3px gray;
            width: 65%;
            height: 98%;
            padding: 10px;
            background-color: #FFFFFF;
            margin-top: 50px;
            top: 1%;
            left: 3%;
            position: absolute;
            z-index: 99999;
        }
    </style>
</asp:Content>

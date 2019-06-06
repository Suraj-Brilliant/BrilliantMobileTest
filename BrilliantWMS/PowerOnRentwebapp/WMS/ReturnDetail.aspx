<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/CRM.Master" AutoEventWireup="true" CodeBehind="ReturnDetail.aspx.cs" Inherits="BrilliantWMS.WMS.ReturnDetail" 
    EnableEventValidation="false" Theme="Blue" %>

<%@ Register Src="../CommonControls/UCFormHeader.ascx" TagName="UCFormHeader" TagPrefix="uc2" %>
<%@ Register Src="../CommonControls/Toolbar.ascx" TagName="Toolbar" TagPrefix="uc2" %>
<%@ Register Assembly="obout_Grid_NET" Namespace="Obout.Grid" TagPrefix="obout" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHolder_FormHeader" runat="server">
    <uc2:Toolbar ID="Toolbar1" runat="server" />
    <uc2:UCFormHeader ID="UCFormHeader1" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPHolder_Form" runat="server">
    <h4 id="h4DivHead" runat="server"></h4>
        <table style="float: right; font-size: 15px; font-weight: bold; color: Black; "> <%--margin-top: -25px;--%>
            <tr>
                <td style="padding-right: 10px; padding-bottom: 0px; padding-top: 0px;">
                    <input type="button" id="btnMarkReturn" title="Mark For Return" value="Mark For Return" runat="server"  onclick="MarkForReturn()" />
                    <%--<input type="button" id="btnGenerateGroupTask" title="Generate Group Task" value="Generate Group Task" runat="server" onclick="GenerateGroupTask()" />--%>
                <%--<input type="button" id="btnCreateTask" title="Create Task" value="Create Task" runat="server" onclick="CreateTask()" />--%>
                </td>                
            </tr>
        </table>
     <div  id="divlinkRequestsList"><%-- class="divDetailExpand"--%>
        <center>
            <table class="gridFrame" width="100%">
                <tr>
                    <td>
                        <table style="width: 100%">
                            <tr>
                                <td>
                                    <asp:Label ID="lblOutboundlist" Style="color: white; font-size: 15px; font-weight: bold;" runat="server" Text="Outbound"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <obout:Grid ID="grdSalesOrder" runat="server" AllowAddingRecords="false" AutoGenerateColumns="false" AllowColumnResizing="true" AllowFiltering="true" AllowManualPaging="true" AllowColumnReordering="true" AllowMultiRecordSelection="false" AllowRecordSelection="true" AllowGrouping="true" Width="100%" Serialize="true" CallbackMode="true" PageSize="10" AllowPaging="true" AllowPageSizeSelection="true">
                            <ClientSideEvents ExposeSender="true" />
                            <Columns>
                                <obout:Column DataField="Id" HeaderText="SO No." HeaderAlign="center" Align="center" Width="10%"></obout:Column>                                
                                <obout:Column DataField="Type" HeaderText="Type" HeaderAlign="center" Align="center" Width="10%"></obout:Column>
                              <obout:Column DataField="JobCardName" HeaderText="Job Card Name" HeaderAlign="Center" Align="center" Width="10%"></obout:Column>                               
                                <obout:Column DataField="OrderDate" HeaderText="SO Date" HeaderAlign="left" Align="left" DataFormatString="{0:dd-MM-yyyy}" Width="10%"></obout:Column>
                                <obout:Column DataField="Title" HeaderText="Title" HeaderAlign="left" Align="left" Width="15%" Wrap="true"></obout:Column>
                                <obout:Column DataField="CustomerName" HeaderText="Customer Name" HeaderAlign="left" Align="left" Width="10%"></obout:Column>
                                <obout:Column DataField="SOBy" HeaderText="Sales Order By" HeaderAlign="left" Align="left" Width="10%"></obout:Column>
                                <obout:Column DataField="StatusName" HeaderText="SO Status" HeaderAlign="left" Align="left" Width="8%" Wrap="true"></obout:Column>                                                               
                            </Columns>                            
                        </obout:Grid>
                    </td>
                </tr>
            </table>
        </center>
    </div>
    <asp:HiddenField ID="SelectedOrder" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnSelectedRec" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnSOID" runat="server" ClientIDMode="Static" />
    <style type="text/css">
        /*POR Collapsable Div*/

        .PanelCaption {
            color: Black;
            font-size: 13px;
            font-weight: bold;
            margin-top: -22px;
            margin-left: -5px;
            position: absolute;
            background-color: White;
            padding: 0px 2px 0px 2px;
        }

        .divHead {
            border: solid 2px #F5DEB3;
            width: 99%;
            text-align: left;
        }

            .divHead h4 {
                /*color: #33CCFF;*/
                color: #483D8B;
                margin: 3px 3px 3px 3px;
            }

            .divHead a {
                float: right;
                margin-top: -15px;
                margin-right: 5px;
            }

                .divHead a:hover {
                    cursor: pointer;
                    color: Red;
                }

        .divDetailExpand {
            border: solid 2px #F5DEB3;
            border-top: none;
            width: 99%;
            padding: 5px 0 5px 0;
            overflow: hidden;
            height: 92%;
        }

        .divDetailCollapse {
            display: none;
        }
        /*End POR Collapsable Div*/
    </style>
    <script type="text/javascript">
        window.onload = function () {
            oboutGrid.prototype.restorePreviousSelectedRecord = function () {
                return;
            }
            oboutGrid.prototype.markRecordAsSelectedOld = oboutGrid.prototype.markRecordAsSelected;
            oboutGrid.prototype.markRecordAsSelected = function (row, param2, param3, param4, param5) {
                if (row.className != this.CSSRecordSelected) {
                    this.markRecordAsSelectedOld(row, param2, param3, param4, param5);

                } else {
                    var index = this.getRecordSelectionIndex(row);
                    if (index != -1) {
                        this.markRecordAsUnSelected(row, index);
                    }
                }
                SelectedRec();
            }
        }

        function SelectedRec() {
            var hdnSelectedRec = document.getElementById("hdnSelectedRec");
            hdnSelectedRec.value = "";
            if (grdSalesOrder.PageSelectedRecords.length > 0) {
                for (var i = 0; i < grdSalesOrder.PageSelectedRecords.length; i++) {
                    var record = grdSalesOrder.PageSelectedRecords[i];
                    if (hdnSelectedRec.value != "") hdnSelectedRec.value += ',' + record.Id;
                    if (hdnSelectedRec.value == "") hdnSelectedRec.value = record.Id;
                }
            }
        }

        onload();
        function onload() {
            var v = document.getElementById("btnConvertTo");
            v.style.visibility = 'hidden';

            var exp = document.getElementById("btnExport");
            exp.style.visibility = 'hidden';

            //var imp = document.getElementById("btnImport");
            //imp.style.visibility = 'hidden';

            var ml = document.getElementById("btnMail");
            ml.style.visibility = 'hidden';

            var pt = document.getElementById("btnPrint");
            pt.style.visibility = 'hidden';
        }

        function MarkForReturn() {
            var hdnSelectedRec = document.getElementById('hdnSelectedRec');
            if (hdnSelectedRec.value == "") {
                showAlert('Please Select Atleast One Order For Make For Return!!!', 'Error', '#');
            } else {
                PageMethods.WMChangeStatus(hdnSelectedRec.value, OnSuccessCheckStatus, null);
            }
        }
        function OnSuccessCheckStatus(result) {
            showAlert("Selected Sales Orders Are Marked For Return", "info", "../WMS/Outbound.aspx");
        }
    </script>
</asp:Content>

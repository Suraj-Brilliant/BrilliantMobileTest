<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/CRM.Master" AutoEventWireup="true" CodeBehind="LabelPrinting.aspx.cs"
    Inherits="BrilliantWMS.WMS.LabelPrinting" EnableEventValidation="false" Theme="Blue" %>

<%@ Register Assembly="obout_Flyout2_NET" Namespace="OboutInc.Flyout2" TagPrefix="obout" %>
<%@ Register Assembly="Obout.Ajax.UI" Namespace="Obout.Ajax.UI.FileUpload" TagPrefix="obout" %>
<%@ Register Assembly="obout_Interface" Namespace="Obout.Interface" TagPrefix="obout" %>
<%@ Register Assembly="obout_Grid_NET" Namespace="Obout.Grid" TagPrefix="obout" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../CommonControls/UCFormHeader.ascx" TagName="UCFormHeader" TagPrefix="uc1" %>
<%@ Register Src="../CommonControls/UCToolbar.ascx" TagName="UCToolbar" TagPrefix="uc2" %>
<%@ Register Src="../Document/UC_AttachDocument.ascx" TagName="UC_AttachDocument"
    TagPrefix="uc3" %>
<%@ Register Src="../CommonControls/Toolbar.ascx" TagName="Toolbar" TagPrefix="uc2" %>
<%@ Register Src="../CommonControls/UC_Date.ascx" TagName="UC_Date" TagPrefix="uc4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHolder_FormHeader" runat="server">
    <uc2:Toolbar ID="Toolbar1" runat="server" />
    <uc1:UCFormHeader ID="UCFormHeader1" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPHolder_Form" runat="server">
    <div class="divHead">
        <h4 id="h4DivHead" runat="server"></h4>
        <table style="float: right; font-size: 15px; font-weight: bold; color: Black;">
            <%--margin-top: -25px;--%>
            <tr>
                <td style="padding-right: 10px; padding-bottom: 0px; padding-top: 0px;">
                    <%--<input type="button" id="btnGenerateGroupTask" title="Generate Group Task" value="Generate Group Task" runat="server" onclick="GenerateGroupTask()" />--%>
                    <input type="button" id="btnPrintLables" title="Print Labels" value="Print Labels" runat="server" onclick="PrintLabel()" />
                    <input type="button" id="btnCreateTask" title="Create Task" value="Create Task" runat="server" onclick="CreateTask()" />
                </td>
                <td>
                    <div class="PORgray"></div>
                </td>
                <td>
                    <asp:Label ID="lblHeading" runat="server" Text="Not Applicable"></asp:Label>
                </td>
                <td>&nbsp;
                </td>
                <td>
                    <div class="PORgreen"></div>
                </td>
                <td>
                    <asp:Label ID="lblCompleted" runat="server" Text="Completed"></asp:Label>
                </td>
                <td>&nbsp;
                </td>
                <td>
                    <div class="PORred"></div>
                </td>
                <td>
                    <asp:Label ID="lblPending" runat="server" Text="Pending"></asp:Label>
                </td>
                <td>&nbsp;
                </td>
                <td>
                    <div class="PORorange"></div>
                </td>
                <td>
                    <asp:Label ID="lblCancelled" runat="server" Text="Cancelled"></asp:Label>
                </td>
            </tr>
        </table>
    </div>
    <div class="divDetailExpand" id="divlinkLabelPrintList">
        <center>
            <table class="gridFrame" width="100%">
                <tr>
                    <td>
                        <table style="width: 100%">
                            <tr>
                                <td>
                                    <asp:Label ID="lblLabelPrintlist" Style="color: white; font-size: 15px; font-weight: bold;" runat="server" Text="Label Print List"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <obout:Grid ID="grdLabelPrint" runat="server" AutoGenerateColumns="false" AllowAddingRecords="false" AllowFiltering="true" AllowGrouping="true" AllowMultiRecordSelection="false"
                            AllowRecordSelection="true" Width="100%" AllowColumnResizing="true" AllowColumnReordering="true">
                            <Columns>
                                <obout:Column DataField="ID" HeaderText="QC No" HeaderAlign="center" Align="center" Width="5%"></obout:Column>
                                <obout:Column DataField="GRNID" HeaderText="GRN ID" HeaderAlign="center" Align="center" Width="5%"></obout:Column>
                                <obout:Column DataField="PONO" HeaderText="PO No" HeaderAlign="center" Align="center" Width="7%"></obout:Column>
                                <obout:Column DataField="JobCardName" HeaderText="Job Card Name" HeaderAlign="left" Align="left" Width="10%" Wrap="true"></obout:Column>
                                <obout:Column DataField="QCDate" HeaderText="QC Date" HeaderAlign="left" Align="left" DataFormatString="{0:dd-MM-yyyy}" Width="10%"></obout:Column>
                                <obout:Column DataField="VendorName" HeaderText="Vendor Name" HeaderAlign="left" Align="left" Width="10%" Wrap="true"></obout:Column>
                                <obout:Column DataField="QCUser" HeaderText="QC By" HeaderAlign="left" Align="left" Width="10%"></obout:Column>
                                <obout:Column DataField="StatusName" HeaderText="Status" HeaderAlign="left" Align="left" Width="8%" Wrap="true"></obout:Column>
                                <obout:Column DataField="ImgLP" HeaderText="Label Printing" HeaderAlign="center" Align="center" Width="7%">
                                    <TemplateSettings TemplateId="LPStatusLP" />
                                </obout:Column>
                                <obout:Column DataField="ImgPutIn" HeaderText="Put In" HeaderAlign="center" Align="center" Width="7%">
                                    <TemplateSettings TemplateId="LPStatusPI" />
                                </obout:Column>
                            </Columns>
                            <Templates>
                                <obout:GridTemplate ID="LPStatusLP" runat="server">
                                    <Template>
                                        <center>
                                            <div class='<%# ("POR" + Container.Value) %>' onclick="parent.RequestOpenEntryForm('GRN','<%# Container.Value %>', '<%# Container.DataItem["ID"] %>')"></div>
                                        </center>
                                    </Template>
                                </obout:GridTemplate>
                                <obout:GridTemplate ID="LPStatusPI" runat="server">
                                    <Template>
                                        <center>
                                            <div class='<%# ("POR" + Container.Value) %>' onclick="RequestOpenEntryForm('PutIn','<%# Container.Value %>', '<%# Container.DataItem["ID"] %>')"></div>
                                        </center>
                                    </Template>
                                </obout:GridTemplate>
                            </Templates>
                        </obout:Grid>
                    </td>
                </tr>
            </table>
        </center>
        <asp:HiddenField ID="hdnSelectedRec" runat="server" ClientIDMode="Static" />
    </div>

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
            if (grdLabelPrint.PageSelectedRecords.length > 0) {
                for (var i = 0; i < grdLabelPrint.PageSelectedRecords.length; i++) {
                    var record = grdLabelPrint.PageSelectedRecords[i];
                    if (hdnSelectedRec.value != "") hdnSelectedRec.value += ',' + record.ID;
                    if (hdnSelectedRec.value == "") hdnSelectedRec.value = record.ID;
                }
            }
        }
        function CreateTask() {
            var hdnSelectedRec = document.getElementById('hdnSelectedRec');
            if (hdnSelectedRec.value == "") {
                showAlert('Please Select Atleast One Record For Assign Task!!!', 'Error', '#');
            } else {
                PageMethods.WMCheckStatus(hdnSelectedRec.value, OnSuccessCheckStatus, null);
            }
        }
        function OnSuccessCheckStatus(result) {
            if (result == 1) {
                window.open('../WMS/AssignTask.aspx', null, 'height=350, width=500,status= 0, resizable= 1, scrollbars=0, toolbar=0,location=center,menubar=0, screenX=300; screenY=50');
            } else if (result == 0) {
                showAlert('One Or More Record Have Already Job Card Number Assigned. Please Select Not Assigned Order!!!', 'Error', '#');
            } else {
                showAlert('Please Select Record With Same Status!!!', 'Error', '#');
            }
        }

        function CloseShowReport() {
            LoadingOff();
            divPopUp.className = "divDetailExpandPopUpOff";           
        }
        function PrintLabel() {
            var hdnSelectedRec = document.getElementById('hdnSelectedRec');
            if (hdnSelectedRec.value == "") {
                showAlert('Please Select Atleast One Record For Label Printing!!!', 'Error', '#');
            } else {
                PageMethods.WMShowReport(hdnSelectedRec.value, ShowReport_Onsuccess, null)
            }
        }
        function ShowReport_Onsuccess(result) {
            if (parseInt(result) > 0) {
                grdLabelPrint.refresh();
                document.getElementById("iframePORRpt").src = "../WMS/WMSReport/ReportViewer.aspx";
                divPopUp.className = "divDetailExpandPopUpOn";
            }
            else {
                CloseShowReport();
            }
        }

        function RequestOpenEntryForm(invoker, state, QCID) {
            if (state != "gray") {
                if (state == "red") { state = "Edit"; }
                else if (state != "red") { state = "View"; }
                PageMethods.WMSetSessionQC(invoker, QCID, state, RequestOpenEntryFormOnSuccess, null);
            }
            else {
                showAlert("Not Applicable", '', '#');
            }
        }

        function RequestOpenEntryFormOnSuccess(result) {
            switch (result) {
                case "PurchaseOrder":
                    window.open('../WMS/PurchaseOrder.aspx', '_self', '');
                    break;
                case "PutIn":
                    window.open('../WMS/PutIn.aspx', '_self', '');
                    break;                
                case "AccessDenied":
                    showAlert("Access Denied", '', '#');
                    break;
            }
        }
    </script>
</asp:Content>

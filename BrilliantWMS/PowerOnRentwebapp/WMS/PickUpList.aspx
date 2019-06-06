<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/CRM.Master" AutoEventWireup="true" CodeBehind="PickUpList.aspx.cs"
    Inherits="BrilliantWMS.Warehouse.PickUpList" EnableEventValidation="false" Theme="Blue" %>

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
    <style type="text/css">
        /*POR Collapsable Div*/


        .divHead {
            border: solid 2px #F5DEB3;
            width: 99%;
            text-align: left;
            margin-top: -5;
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

        /*End POR Collapsable Div*/
    </style>
    <center>
        <asp:UpdatePanel ID="UPTBPickUp" runat="server">
            <ContentTemplate>
                <%--<asp:TabContainer ID="TCPickUp" runat="server">
                    <asp:TabPanel ID="TabPickUp" runat="server" HeaderText="PickUp List">--%>
                <contenttemplate>
                            <asp:UpdateProgress ID="UPGRNGridProgress" runat="server" AssociatedUpdatePanelID="UpPnlGridPickUp">
                                <ProgressTemplate>
                                    <center>
                                        <div class="modal">
                                            <img src="../App_Themes/Blue/img/ajax-loader.gif" style="top: 50%;" />
                                        </div>
                                    </center>
                                </ProgressTemplate>
                            </asp:UpdateProgress>
                            <asp:UpdatePanel ID="UpPnlGridPickUp" runat="server">
                                <ContentTemplate>
                                   <%--  <div class="divHead">--%>
                                        <h4 id="h1" runat="server"></h4>
                                        <table style="float: right; font-size: 15px; font-weight: bold; color: Black; "> <%--margin-top: -25px;--%>
                                            <tr>
                                                <td style="padding-right: 10px; padding-bottom: 0px; padding-top: 0px;">
                                                    <input type="button" id="btnGenerateGroupTask" title="Generate Group Task" value="Generate Group Task" runat="server" onclick="GenerateGroupTask()" />
                                                   <input type="button" id="Button1" title="Change Status" value="Change Status" runat="server" />
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
                                  <%--  </div>--%>
                                    <div class="divDetailExpand" id="divlinkPutInList">
                                        <center>
                                            <table class="gridFrame" width="100%">
                                                <tr>
                                                    <td>
                                                        <table style="width: 100%">
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lblPickUplist" Style="color: white; font-size: 15px; font-weight: bold;" runat="server" Text="PickUp List"></asp:Label>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <obout:Grid ID="grdPickUp" runat="server" AutoGenerateColumns="false" AllowAddingRecords="false" AllowFiltering="true" AllowGrouping="true" AllowMultiRecordSelection="false"
                                                            AllowRecordSelection="true" Width="100%" AllowColumnResizing="true" AllowColumnReordering="true">
                                                            <Columns>
                                                                <obout:Column DataField="ID" HeaderText="PickUp No" HeaderAlign="center" Align="center" Width="5%"></obout:Column>
                                                                <obout:Column DataField="OID" HeaderText="Sale Order No" HeaderAlign="center" Align="center" Width="7%"></obout:Column>
                                                                <obout:Column DataField="JobCardName" HeaderText="Job Card Name" HeaderAlign="left" Align="left" Width="10%" Wrap="true"></obout:Column>
                                                                <obout:Column DataField="PickUpDate" HeaderText="PickUp Date" HeaderAlign="left" Align="left" DataFormatString="{0:dd-MM-yyyy}" Width="10%"></obout:Column>
                                                                <obout:Column DataField="CustomerName" HeaderText="Customer Name" HeaderAlign="left" Align="left" Width="10%" Wrap="true"></obout:Column>
                                                                <obout:Column DataField="PickByUser" HeaderText="PickUp By" HeaderAlign="left" Align="left" Width="10%"></obout:Column>
                                                                <obout:Column DataField="StatusName" HeaderText="Status" HeaderAlign="left" Align="left" Width="8%" Wrap="true"></obout:Column>
                                                                <obout:Column DataField="ImgPickUp" HeaderText="PickUp" HeaderAlign="center" Align="center" Width="7%">
                                                                        <TemplateSettings TemplateId="SOStatusPL" />
                                                                </obout:Column>
                                                                <obout:Column DataField="ImgQC" HeaderText="Quality Check" HeaderAlign="center" Align="center" Width="7%">
                                                                      <TemplateSettings TemplateId="SOStatusQC" />
                                                                </obout:Column>
                                                               <obout:Column DataField="ImgDispatch" HeaderText="Dispatch" HeaderAlign="center" Align="center" Width="7%">
                                                               <TemplateSettings TemplateId="SOStatusD" />
                                                               </obout:Column>  
                                                            </Columns>
                                                            <Templates>
                                                                <obout:GridTemplate ID="SOStatusPL" runat="server">
                                                                 <Template>
                                                                     <center>
                                                                      <div class='<%# ("POR" + Container.Value) %>' onclick="RequestOpenEntryForm('PickUp','<%# Container.Value %>', '<%# Container.DataItem["ID"] %>')"></div>
                                                                      </center>
                                                                   </Template>
                                </obout:GridTemplate>
                                <obout:GridTemplate ID="SOStatusQC" runat="server">
                                    <Template>
                                        <center>
                                            <div class='<%# ("POR" + Container.Value) %>' onclick="RequestOpenEntryForm('QC','<%# Container.Value %>', '<%# Container.DataItem["ID"] %>')"></div>
                                        </center>
                                    </Template>
                                </obout:GridTemplate>
                                <obout:GridTemplate ID="SOStatusD" runat="server">
                                    <Template>
                                        <center>
                                            <div class='<%# ("POR" + Container.Value) %>' onclick="RequestOpenEntryForm('Dispatch','<%# Container.Value %>', '<%# Container.DataItem["ID"] %>')"></div>
                                        </center>
                                    </Template>
                                </obout:GridTemplate>
                                                            </Templates>
                                                        </obout:Grid>
                                                    </td>
                                                </tr>
                                            </table>
                                        </center>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </contenttemplate>
                <%--  </asp:TabPanel>
                </asp:TabContainer>--%>
            </ContentTemplate>
        </asp:UpdatePanel>
    </center>
    <asp:HiddenField ID="hdndsvalue" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnselectedSO" runat="server" ClientIDMode="Static" />
    <%-- </center>--%>
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
                selectPORec();
            }
        }

        function selectPORec() {
            var hdnselectedSO = document.getElementById("hdnselectedSO");
            hdnselectedSO.value = "";
            for (i = 0; i < Grid1.PageSelectedRecords.length; i++) {
                var record = Grid1.PageSelectedRecords[i];
                if (hdnselectedSO.value != "") hdnselectedSO.value += ',' + record.ID;
                if (hdnselectedSO.value == "") hdnselectedSO.value = record.ID;
            }
        }

        function Grid1_Deselect(index) {
            selectPORec();
        }

        function selectedRec() {
            var selectedSO = document.getElementById("hdnselectedSO").value;
            if (selectedSO == "") {
                window.open('../POR/CreateGroup.aspx', null, 'height=120, width=800,status= 0, resizable= 1, scrollbars=0, toolbar=0,location=center,menubar=0, screenX=300; screenY=50');                // temporary code new code
                //showAlert("Please Select Sales Orders.", 'Error', '#');  // Correct Old Code
            }
            else {
                PageMethods.WMGetSOData(selectedSO, jsGetPODataOnSuccess, null);
            }
        }

        function jsGetPODataOnSuccess(result) {
            if (result == "1") {
                //showAlert("Please Select Purchase Orders of Status Entered Only.", 'Error', '#');
                alert("     Please Select Sales Orders of Status Entered Only  and Sales Order of NoGroup.    ", "nGroup");
            }
            else {
                window.open('../POR/CreateGroup.aspx?Sinvoker=' + result + '', null, 'height=120, width=800,status= 0, resizable= 1, scrollbars=0, toolbar=0,location=center,menubar=0, screenX=300; screenY=50');
            }
        }

        function jsAddNew() {
            PageMethods.WMpageAddNew(jsAddNewOnSuccess, null);
        }

        function jsAddNewOnSuccess() {
            window.open('../Warehouse/PickUp.aspx', '_self', '');
            //window.open('../POR/SalesOrderEntry.aspx', '_self', '');
        }

        function selectedRecShip() {
            var selectedSO = document.getElementById("hdnselectedSO").value;
            if (selectedSO == "") {
                window.open('../Warehouse/PickUp.aspx', '_self', '');     // new code temparory
                //showAlert("Please Select Sales Orders Or Sales Order Group.", 'Error', '#');      // old code correct
            } else {
                PageMethods.GetAllSOsOfGroup(selectedSO, GetAllSOsOnSuccess, null);

            }
        }

        function GetAllSOsOnSuccess(response) {
            var selectedSO = document.getElementById("hdnselectedSO");
            if (response != "") {
                if (confirm("This action will select all Sales orders from related group.Do you want to continue?") == true) {

                    selectedSO.value = "";
                    selectedSO.value = response;
                    PageMethods.ValidateGreenSOStatus(response, selectedRecShipOnSuccess, null);
                }
            }
            else {
                PageMethods.ValidateGreenSOStatus(selectedSO.value, selectedRecShipOnSuccess, null);
            }

        }
        function selectedRecShipOnSuccess(response) {
            var selectedPO = document.getElementById("hdnselectedSO").value;
            if (response == "1") {
                document.getElementById("hdnselectedSO").value = "";
                window.open('../POR/Shipping.aspx?SO=' + selectedPO, '_self', '');
                //window.open('../POR/Shipping.aspx', '_self', '');
            }
            else {
                showAlert("Please Select Sales Orders which are not already shipped!", 'Error', '#');
            }
        }
        function jsImport() {
            PageMethods.WMpageAddNew(jsImportOnSuccess, null);
        }
        function jsImportOnSuccess() {
            window.open('../POR/ImportDDSo.aspx', '_self', '');
        }

        function jsbtnSave_onclick() {
            showAlert("Not Applicable", 'info', '#');
        }

        function selectedRecStatus() {
            var selectedSO = document.getElementById("hdnselectedSO").value;

            if (selectedSO == "") {
                showAlert("Please Select Sales Orders to change the status.", 'Error', '#');
            } else {
                if (confirm("Do you want to mark status for this request / Master Ticket as Completed?") == true) {

                    PageMethods.WMChangeSOStatus(selectedSO, selRecStatOnSuccess, null);
                }

            }
        }
        function selRecStatOnSuccess(response) {
            var selectedSO = document.getElementById("hdnselectedSO").value;
            if (response == "0") {
                showAlert("This request is pending for shipping ; Please complete shipping process for this Request / MCN and then mark this as completed", "nGroup");
            }
            else if (response == "2") {
                showAlert("This request is already shipped ; Please select only those Request / MCN whose shipping process is completed and status is WIP!", "nGroup");
            }
            else {
                PageMethods.WMChangePOStatusNow(selectedSO, WMChngSOStatNowOnSuccess, null);
            }
        }


        function WMChngSOStatNowOnSuccess(result) {
            //  alert("Hi");
            showAlert("Status changed successfully", 'Error', '#');
            window.Grid1.refresh();
        }
        function CreateTask() {
            window.open('../WMS/AssignTask.aspx', null, 'height=350, width=500,status= 0, resizable= 1, scrollbars=0, toolbar=0,location=center,menubar=0, screenX=300; screenY=50');
        }

        function RequestOpenEntryForm(invoker, state, requestID) {
            if (state != "gray") {
                if (state == "red") { state = "Edit"; }
                else if (state != "red") { state = "View"; }
                PageMethods.WMSetSessionRequest(invoker, requestID, state, RequestOpenEntryFormOnSuccess, null);
            }
            else {
                showAlert("Not Applicable", '', '#');
            }
            //window.open('../WMS/PickUp.aspx', '_self', '');
        }
        function RequestOpenEntryFormOnSuccess(result) {
            switch (result) {
                case "SalesOrder":
                    window.open('../WMS/SalesOrder.aspx', '_self', '');
                    break;
                case "PickUp":
                    window.open('../WMS/PickUp.aspx', '_self', '');
                    break;
                case "QC":
                    var hdnSOID = document.getElementById('hdnSOID');
                    window.open('../WMS/QCDetail.aspx', '_self', '');
                    break;
                case "Dispatch":
                    var hdnSOID = document.getElementById('hdnSOID');
                    window.open('../WMS/QualityControl.aspx?inv=SO', '_self', '');
                    break;
                case "Receipt":
                    window.open('../PowerOnRent/PartReceiptEntry.aspx?invoker=Request', '_self', '');
                    break;
                case "Consumption":
                    window.open('../PowerOnRent/PartConsumptionEntry.aspx?invoker=' + getParameterByName("invoker"), '_self', '');
                    break;
                case "HQReceipt":
                    window.open('../PowerOnRent/HQGoodsReceiptEntry.aspx', '_self', '');
                    break;
                case "AccessDenied":
                    showAlert("Access Denied", '', '#');
                    break;
            }
        }
    </script>
</asp:Content>

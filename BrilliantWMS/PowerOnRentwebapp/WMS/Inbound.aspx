<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/CRM.Master" AutoEventWireup="true" CodeBehind="Inbound.aspx.cs"
    Inherits="BrilliantWMS.WMS.Inbound" EnableEventValidation="false" Theme="Blue" %>

<%@ Register Src="../CommonControls/UCFormHeader.ascx" TagName="UCFormHeader" TagPrefix="uc2" %>
<%@ Register Src="../CommonControls/Toolbar.ascx" TagName="Toolbar" TagPrefix="uc2" %>
<%@ Register Assembly="obout_Grid_NET" Namespace="Obout.Grid" TagPrefix="obout" %>
<%@ Register Src="~/CommonControls/UCImport.ascx" TagName="UCImport" TagPrefix="uci" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHolder_FormHeader" runat="server">
    <uc2:Toolbar ID="Toolbar1" runat="server" />
    <uc2:UCFormHeader ID="UCFormHeader1" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPHolder_Form" runat="server">
    <%-- <div class="divHead">--%>
    <h4 id="h4DivHead" runat="server"></h4>
    <table style="float: right; font-size: 15px; font-weight: bold; color: Black;">
        <%--margin-top: -25px;--%>
        <tr>
            <td style="padding-right: 10px; padding-bottom: 0px; padding-top: 0px;">
                <input type="button" id="btnCancelOrder" title="Cancel Order" value="Cancel Order" runat="server" onclick="CancelSelectedOrder()" />
                <%--<input type="button" id="btnGenerateGroupTask" title="Generate Group Task" value="Generate Group Task" runat="server" onclick="GenerateGroupTask()" />--%>
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
            <td>&nbsp;
            </td>
            <td>
                <div class="PORgreenRed"></div>
            </td>
            <td>
                <asp:Label ID="lblPartiallyCompleted" runat="server" Text="Partially Completed"></asp:Label>
            </td>
        </tr>
    </table>
    <%-- </div>--%>
    <div id="divlinkRequestsList">
        <%--class="divDetailExpand"--%>
        <center>
            <table class="gridFrame" width="100%">
                <tr>
                    <td>
                        <table style="width: 100%">
                            <tr>
                                <td>
                                    <asp:Label ID="lblInboundlist" Style="color: white; font-size: 15px; font-weight: bold;" runat="server" Text="Inbound"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <obout:Grid ID="grdPurchaseOrder" runat="server" AllowAddingRecords="false" AutoGenerateColumns="false" AllowColumnResizing="true" AllowFiltering="true"
                            AllowManualPaging="true" AllowColumnReordering="true" AllowMultiRecordSelection="true" AllowRecordSelection="true" AllowGrouping="true" Width="100%"
                            Serialize="true" CallbackMode="true" PageSize="10" AllowPaging="true" AllowPageSizeSelection="true">
                            <ClientSideEvents ExposeSender="true" />
                            <Columns>
                                <obout:Column DataField="Id" HeaderText="Order No." HeaderAlign="center" Align="center" Width="10%"></obout:Column>
                                <%--<obout:Column DataField="POOrderNo" HeaderText="PO No." HeaderAlign="center" Align="center" Width="10%"></obout:Column>--%>
                                <obout:Column DataField="Type" HeaderText="Type" HeaderAlign="center" Align="center" Width="0%" Visible="false"></obout:Column>
                                <obout:Column DataField="JobCardName" HeaderText="Job Card Name" HeaderAlign="Center" Align="center" Width="10%"></obout:Column>
                                <obout:Column DataField="WarehouseName" HeaderText="WarehouseName" HeaderAlign="left" Align="left" Width="10%"></obout:Column>
                                <obout:Column DataField="POdate" HeaderText="Order Date" HeaderAlign="left" Align="left" DataFormatString="{0:dd-MM-yyyy}" Width="10%"></obout:Column>
                                <obout:Column DataField="Title" HeaderText="Title" HeaderAlign="left" Align="left" Width="15%" Wrap="true"></obout:Column>
                                <obout:Column DataField="VendorName" HeaderText="Vendor Name" HeaderAlign="left" Align="left" Width="10%" Wrap="true"></obout:Column>
                                <obout:Column DataField="StatusName" HeaderText="Status" HeaderAlign="left" Align="left" Width="8%" Wrap="true"></obout:Column>
                                <obout:Column DataField="ImgPO" HeaderText="PO" HeaderAlign="center" Align="center" Width="7%">
                                    <TemplateSettings TemplateId="POStatusPO" />
                                </obout:Column>
                                <obout:Column DataField="ImgGRN" HeaderText="GRN" HeaderAlign="center" Align="center" Width="7%">
                                    <TemplateSettings TemplateId="POStatusGRN" />
                                </obout:Column>
                                <obout:Column DataField="ImgQC" HeaderText="Quility Check" HeaderAlign="center" Align="center" Wrap="true" Width="7%">
                                    <TemplateSettings TemplateId="POStatusQC" />
                                </obout:Column>
                                <obout:Column DataField="ImgLP" HeaderText="Label Printing" HeaderAlign="center" Align="center" Wrap="true" Width="7%">
                                    <TemplateSettings TemplateId="POStatusLP" />
                                </obout:Column>
                                <obout:Column DataField="ImgPutIn" HeaderText="Put In" HeaderAlign="center" Align="center" Width="7%">
                                    <TemplateSettings TemplateId="POStatusPI" />
                                </obout:Column>
                            </Columns>
                            <Templates>
                                <obout:GridTemplate ID="POStatusPO" runat="server">
                                    <Template>
                                        <center>
                                            <div class='<%# ("POR" + Container.Value) %>' onclick="RequestOpenEntryForm('PurchaseOrder','<%# Container.Value %>', '<%# Container.DataItem["Id"] %>','<%# Container.DataItem["Type"]%>')"></div>
                                        </center>
                                    </Template>
                                </obout:GridTemplate>
                                <obout:GridTemplate ID="POStatusGRN" runat="server">
                                    <Template>
                                        <center>
                                            <div class='<%# ("POR" + Container.Value) %>' onclick="RequestOpenEntryForm('GRN','<%# Container.Value %>', '<%# Container.DataItem["Id"] %>','<%# Container.DataItem["Type"]%>')"></div>
                                        </center>
                                    </Template>
                                </obout:GridTemplate>
                                <obout:GridTemplate ID="POStatusQC" runat="server">
                                    <Template>
                                        <center>
                                            <div class='<%# ("POR" + Container.Value) %>' onclick="RequestOpenEntryForm('QC','<%# Container.Value %>', '<%# Container.DataItem["Id"] %>','<%# Container.DataItem["Type"]%>')"></div>
                                        </center>
                                    </Template>
                                </obout:GridTemplate>
                                <obout:GridTemplate ID="POStatusLP" runat="server">
                                    <Template>
                                        <center>
                                            <div class='<%# ("POR" + Container.Value) %>' onclick="RequestOpenEntryForm('LabelPrinting','<%# Container.Value %>', '<%# Container.DataItem["Id"] %>','<%# Container.DataItem["Type"]%>')"></div>
                                        </center>
                                    </Template>
                                </obout:GridTemplate>
                                <obout:GridTemplate ID="POStatusPI" runat="server">
                                    <Template>
                                        <center>
                                            <div class='<%# ("POR" + Container.Value) %>' onclick="RequestOpenEntryForm('PutIn','<%# Container.Value %>', '<%# Container.DataItem["Id"] %>','<%# Container.DataItem["Type"]%>')"></div>
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
    <asp:HiddenField ID="SelectedOrder" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnSelectedRec" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnPOID" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnUsrCompany" runat="server" ClientIDMode="Static" Value="0" />
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
            if (grdPurchaseOrder.PageSelectedRecords.length > 0) {
                for (var i = 0; i < grdPurchaseOrder.PageSelectedRecords.length; i++) {
                    var record = grdPurchaseOrder.PageSelectedRecords[i];
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

            //  var imp = document.getElementById("btnImport");


            var ml = document.getElementById("btnMail");
            ml.style.visibility = 'hidden';

            var pt = document.getElementById("btnPrint");
            pt.style.visibility = 'hidden';
        }
        function jsImport() {
            window.open('../WMS/ImportDPO.aspx', '_self', '');
        }
        function jsAddNew() {
            PageMethods.WMSetSessionAddNew("Add", jsAddNewOnSuccess, null);
        }
        function jsAddNewOnSuccess() {
            var hdnUsrCompany = document.getElementById('hdnUsrCompany')
            if (hdnUsrCompany.value == '1') {
                window.open('../WMS/GRNDetail.aspx', '_self', '');
            } else {
                window.open('../WMS/PurchaseOrder.aspx', '_self', '');
            }
        }
        //    if (getParameterByName("invoker") == "Request" || getParameterByName("invoker") == "Issue" || getParameterByName("invoker") == "Receipt" || getParameterByName("invoker") == "Consumption" || getParameterByName("invoker") == "HQReceipt") {
        //        PageMethods.WMSetSessionAddNew(getParameterByName("invoker"), "Add", jsAddNewOnSuccess, null);
        //    }
        //    else {
        //        showAlert("Invalid url", "error", "../PowerOnRent/Default.aspx?invoker=Request");
        //    }
        //}
        //function jsAddNewOnSuccess() {
        //    if (getParameterByName("invoker") == "Request") {
        //        window.open('../PowerOnRent/PartRequestEntry.aspx', '_self', '');
        //    }
        //    else if (getParameterByName("invoker") == "Issue") {
        //        window.open('../PowerOnRent/PartIssueEntry.aspx', '_self', '');
        //    }
        //    else if (getParameterByName("invoker") == "Receipt") {
        //        window.open('../PowerOnRent/PartReceiptEntry.aspx', '_self', '');
        //    }
        //    else if (getParameterByName("invoker") == "Consumption") {
        //        window.open('../PowerOnRent/PartConsumptionEntry.aspx', '_self', '');
        //    }
        //    else if (getParameterByName("invoker") == "HQReceipt") {
        //        window.open('../PowerOnRent/HQGoodsReceiptEntry.aspx', '_self', '');
        //    }
        //}

        function OpenEntryForm(invoker, state, referenceID, requestID) {
            if (state != "gray") {
                if (state == "red") { state = "Edit"; }
                else if (state != "red") { state = "View"; }
                PageMethods.WMSetSession(invoker, referenceID, requestID, state, OpenEntryFormOnSuccess);
            }
            else {
                showAlert("Not Applicable", '', '#');
            }
        }

        function OpenEntryFormOnSuccess(result) {
            switch (result) {
                case "Request":
                    window.open('../PowerOnRent/PartRequestEntry.aspx', '_self', '');
                    break;
                case "Approval":
                    window.open('../PowerOnRent/PartRequestEntry.aspx', '_self', '');
                    break;
                case "Issue":
                    window.open('../PowerOnRent/PartIssueEntry.aspx', '_self', '');
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
            }
        }

        function ToolbarAccess() {

        }
    </script>
    <%--Code for Request Summary--%>
    <script type="text/javascript">
        function RequestOpenEntryForm(invoker, state, requestID,Type) {
            var hdnPOID = document.getElementById('hdnPOID');
            hdnPOID.value = requestID;
            if (state != "gray") {
                if (state == "red" || state == 'greenRed') { state = "Edit"; }
                else if (state != "red" || state != 'greenRed') { state = "View"; }
                PageMethods.WMSetSessionRequest(invoker, requestID, state, Type,RequestOpenEntryFormOnSuccess, null);
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
                case "GRN":
                    window.open('../WMS/GRNDetail.aspx', '_self', '');
                    break;
                case "QC":
                    var hdnPOID = document.getElementById('hdnPOID');
                    window.open('../WMS/GridGRN.aspx?POID=' + hdnPOID.value + '', '_self', '');
                    // window.open('../WMS/QCDetail.aspx', '_self', '');
                    break;
                case "LabelPrinting":
                    var hdnPOID = document.getElementById('hdnPOID');
                    window.open('../WMS/GridGRN.aspx?POID=' + hdnPOID.value + '', '_self', '');
                    break;
                case "PutIn":
                    var hdnPOID = document.getElementById('hdnPOID');
                    window.open('../WMS/GridGRN.aspx?POID=' + hdnPOID.value + '', '_self', '');
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
    <%--End Code for Request Summary--%>
    <%--Code for Issue Summary--%>
    <script type="text/javascript">
        function IssueOpenEntryForm(invoker, state, issueID) {
            if (state != "gray") {
                if (state == "red") { state = "Edit"; }
                else if (state != "red") { state = "View"; }
                PageMethods.WMSetSessionIssue(invoker, issueID, state, IssueOpenEntryFormOnSuccess, null);
            }
            else {
                showAlert("Not Applicable", '', '#');
            }
        }

        function IssueOpenEntryFormOnSuccess(result) {
            switch (result) {
                case "Issue":
                    window.open('../PowerOnRent/PartIssueEntry.aspx?invoker=Issue', '_self', '');
                    break;
                case "Receipt":
                    window.open('../PowerOnRent/PartReceiptEntry.aspx?invoker=Issue', '_self', '');
                    break;
                case "Consumption":
                    window.open('../PowerOnRent/PartConsumptionEntry.aspx', 'self', '');
                    break;
                case "AccessDenied":
                    showAlert("Access Denied", '', '#');
                    break;
            }
        }
    </script>
    <%--End Code for Issue Summary--%>
    <%--Code for Receipt Summary--%>
    <script type="text/javascript">
        function ReceiptOpenEntryForm(invoker, state, receiptID) {
            debugger;
            if (state != "gray") {
                if (state == "red") { state = "Edit"; }
                else if (state != "red") { state = "View"; }
                PageMethods.WMSetSessionReceipt(invoker, receiptID, state, ReceiptOpenEntryFormOnSuccess, null);
            }
            else {
                showAlert("Not Applicable", '', '#');
            }
        }

        function ReceiptOpenEntryFormOnSuccess(result) {
            debugger;
            switch (result) {
                case "Receipt":
                    window.open('../PowerOnRent/PartReceiptEntry.aspx?invoker=Receipt', '_self', '');
                    break;
                case "Consumption":
                    window.open('../PowerOnRent/PartConsumptionEntry.aspx', '_self', '');
                    break;
                case "AccessDenied":
                    showAlert("Access Denied", '', '#');
                    break;
            }
        }
    </script>
    <%--End Code for Receipt Summary--%>
    <%--Code for Consumption Summary--%>
    <script type="text/javascript">
        function ConsumptionOpenEntryForm(invoker, state, ConsumptionID) {
            if (state != "gray") {
                if (state == "red") { state = "Edit"; }
                else if (state != "red") { state = "View"; }
                PageMethods.WMSetSessionConsumption(invoker, ConsumptionID, state, ConsumptionOpenEntryFormOnSuccess, null);
            }
            else {
                showAlert("Not Applicable", '', '#');
            }
        }

        function ConsumptionOpenEntryFormOnSuccess(result) {
            switch (result) {
                case "Consumption":
                    window.open('../PowerOnRent/PartConsumptionEntry.aspx', '_self', '');
                    break;
                case "AccessDenied":
                    showAlert("Access Denied", '', '#');
                    break;
            }
        }
    </script>
    <%--End Code for Consumption Summary--%>
    <%--Code for Consumption Summary--%>
    <script type="text/javascript">
        function HQReceiptOpenEntryForm(invoker, state, HQReceiptID) {
            if (state != "gray") {
                if (state == "red") { state = "Edit"; }
                else if (state != "red") { state = "View"; }
                PageMethods.WMSetSessionHQReceipt(invoker, HQReceiptID, state, HQReceiptOpenEntryFormOnSuccess, null);
            }
            else {
                showAlert("Not Applicable", '', '#');
            }
        }

        function HQReceiptOpenEntryFormOnSuccess(result) {
            switch (result) {
                case "HQReceipt":
                    window.open('../PowerOnRent/HQGoodsReceiptEntry.aspx', '_self', '');
                    break;
                case "AccessDenied":
                    showAlert("Access Denied", '', '#');
                    break;
            }
        }
    </script>
    <%--End Code for Consumption Summary--%>
    <%--Start of Allocate Driver--%>
    <script type="text/javascript">
        function GetSelected(hdnSelectedRec) {
            var SelectedOrder = document.getElementById("SelectedOrder");
            SelectedOrder.value = hdnSelectedRec;
        }

        function AllocateDriver() {
            var SelectedOrder = document.getElementById("SelectedOrder");
            if (SelectedOrder.value == "") {
                showAlert('Please Select At Least One Order!!!', 'Error', '#');
            } else {
                PageMethods.WMChkDispatchedOrder(SelectedOrder.value, OnSuccessDispatchedOrder, null);
            }
        }
        function OnSuccessDispatchedOrder(result) {
            if (result >= 1) {
                showAlert('One or More Selected Orders Are Already Dispatched. Please Select only Not Dispatched Orders!!!', 'Error', '#');
            } else {
                var SelectedOrder = document.getElementById("SelectedOrder");
                // alert(SelectedOrder.value);
                window.open('../PowerOnRent/AllocateDriver.aspx?OID=' + SelectedOrder.value + '', null, 'height=550, width=700,status= 0, resizable= 1, scrollbars=0, toolbar=0,location=center,menubar=0, screenX=300; screenY=50');
            }
        }
        function AfterAssignDriver() {
            location.reload();
        }
        function CancelSelectedOrder() {
            var hdnSelectedRec = document.getElementById("hdnSelectedRec");
            var SelOrdr = hdnSelectedRec.value;
            var count = (SelOrdr.match(/,/g) || []).length;
            console.log(count);
            if (count >= 1) {
                showAlert("Select Only One Order", "Error", "#");
            } else if (hdnSelectedRec.value == "") {
                showAlert('Please Select One Order!!!', 'Error', '#');
            } else {
                var r = confirm("Are You Sure to Cancel This Order?")
                if (r == true) {
                    PageMethods.WMCancelOrder(hdnSelectedRec.value, OnSuccessCancelOrder, null);
                }
            }
        }
        function OnSuccessCancelOrder(result) {
            if (result == 0) {
                showAlert("Not Applicable", '', '#');
            } else {
                showAlert('Selected Order Is Cancelled!!!', 'Error', '#');
                location.reload();
            }
        }

        function CreateTask() {
            var hdnSelectedRec = document.getElementById('hdnSelectedRec');
            if (hdnSelectedRec.value == "") {
                showAlert('Please Select Atleast One Order For Assign Task!!!', 'Error', '#');
            } else {
                PageMethods.WMCheckStatus(hdnSelectedRec.value, OnSuccessCheckStatus, null);
            }
        }

        function OnSuccessCheckStatus(result) {
            if (result == 1) {
                window.open('../WMS/AssignTask.aspx', null, 'height=350, width=500,status= 0, resizable= 1, scrollbars=0, toolbar=0,location=center,menubar=0, screenX=300; screenY=50');
            } else if (result == 2) {
                showAlert('One Or More Order Have Already Job Card Number Assigned. Please Select Not Assigned Order!!!', 'Error', '#');
            } else {
                showAlert('Please Select Order With Same Status!!!', 'Error', '#');
            }
        }
    </script>
</asp:Content>

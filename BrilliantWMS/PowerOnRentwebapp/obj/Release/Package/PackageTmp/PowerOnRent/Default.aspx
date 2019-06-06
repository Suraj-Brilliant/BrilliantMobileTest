<%@ Page Title="GWC" Language="C#" MasterPageFile="~/MasterPage/CRM.Master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="BrilliantWMS.PowerOnRent.Default"
    EnableEventValidation="false" %>

<%@ Register Src="../CommonControls/UCFormHeader.ascx" TagName="UCFormHeader" TagPrefix="uc2" %>
<%@ Register Src="../CommonControls/Toolbar.ascx" TagName="Toolbar" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHolder_FormHeader" runat="server">
    <uc2:Toolbar ID="Toolbar1" runat="server" />
    <uc2:UCFormHeader ID="UCFormHeader1" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPHolder_Form" runat="server">
    <div class="divHead">
        <h4 id="h4DivHead" runat="server">
        </h4>
        <table style="float: right; font-size: 15px; font-weight: bold; color: Black; margin-top: -25px;">
            <tr>
                <td style="padding-right :10px; padding-bottom :0px; padding-top :0px;">
                    <input type="button" id="btnCancelOrder" title="Cancel Order" value="Cancel Order" runat="server" visible="false" onclick="CancelSelectedOrder()" />
                    <input type="button" id="btnDriver" title="Allocate Driver" value="Allocate Driver" runat="server" visible="false" onclick="AllocateDriver()" />
                </td>
                <td>
                    <div class="PORgray">
                    </div>
                </td>
                <td>
                    <asp:Label ID="lblHeading" runat="server" Text="Not Applicable"></asp:Label>
                </td>
                <td>
                    &nbsp;
                </td>
                <td>
                    <div class="PORgreen">
                    </div>
                </td>
                <td>
                    <asp:Label ID="lblCompleted" runat="server" Text="Completed"></asp:Label>
                </td>
                <td>
                    &nbsp;
                </td>
                <td>
                    <div class="PORred">
                    </div>
                </td>
                <td>
                    <asp:Label ID="lblPending" runat="server" Text="Pending"></asp:Label>
                </td>
                <td>
                    &nbsp;
                </td>
                <td>
                    <div class="PORorange">
                    </div>
                </td>
                <td>
                    <asp:Label ID="lblCancelled" runat="server" Text="Cancelled"></asp:Label>
                    
                </td>
                <%--<td>
                    &nbsp;
                </td>
                <td>
                    <div class="PORgreenRed">
                    </div>
                </td>
                <td>
                    Partially Completed
                </td>--%>
            </tr>
        </table>
    </div>
    <div class="divDetailExpand" id="divlinkRequestsList">
        <center>
            <iframe runat="server" id="iframePOR" width="99%" style="border: none; min-height: 400px;">
            </iframe>
        </center>
    </div>
    <asp:HiddenField ID="SelectedOrder" runat="server" ClientIDMode="Static" />
    <style type="text/css">
        /*POR Collapsable Div*/
        
        .PanelCaption
        {
            color: Black;
            font-size: 13px;
            font-weight: bold;
            margin-top: -22px;
            margin-left: -5px;
            position: absolute;
            background-color: White;
            padding: 0px 2px 0px 2px;
        }
        .divHead
        {
            border: solid 2px #F5DEB3;
            width: 99%;
            text-align: left;
        }
        .divHead h4
        {
            /*color: #33CCFF;*/
            color: #483D8B;
            margin: 3px 3px 3px 3px;
        }
        .divHead a
        {
            float: right;
            margin-top: -15px;
            margin-right: 5px;
        }
        .divHead a:hover
        {
            cursor: pointer;
            color: Red;
        }
        .divDetailExpand
        {
            border: solid 2px #F5DEB3;
            border-top: none;
            width: 99%;
            padding: 5px 0 5px 0;
            overflow: hidden;
            height: 92%;
        }
        .divDetailCollapse
        {
            display: none;
        }
        /*End POR Collapsable Div*/
    </style>
    <script type="text/javascript">
        onload();
        function onload() {
            var v = document.getElementById("btnConvertTo");
            v.style.visibility = 'hidden';

            var exp = document.getElementById("btnExport");
            exp.style.visibility = 'hidden';

            var imp = document.getElementById("btnImport");
            imp.style.visibility = 'hidden';

            var ml = document.getElementById("btnMail");
            ml.style.visibility = 'hidden';

            var pt = document.getElementById("btnPrint");
            pt.style.visibility = 'hidden';
        }
        function jsAddNew() {
            if (getParameterByName("invoker") == "Request" || getParameterByName("invoker") == "Issue" || getParameterByName("invoker") == "Receipt" || getParameterByName("invoker") == "Consumption" || getParameterByName("invoker") == "HQReceipt") {
                PageMethods.WMSetSessionAddNew(getParameterByName("invoker"), "Add", jsAddNewOnSuccess, null);
            }
            else {
                showAlert("Invalid url", "error", "../PowerOnRent/Default.aspx?invoker=Request");
            }
        }
        function jsAddNewOnSuccess() {
            if (getParameterByName("invoker") == "Request") {
                window.open('../PowerOnRent/PartRequestEntry.aspx', '_self', '');
            }
            else if (getParameterByName("invoker") == "Issue") {
                window.open('../PowerOnRent/PartIssueEntry.aspx', '_self', '');
            }
            else if (getParameterByName("invoker") == "Receipt") {
                window.open('../PowerOnRent/PartReceiptEntry.aspx', '_self', '');
            }
            else if (getParameterByName("invoker") == "Consumption") {
                window.open('../PowerOnRent/PartConsumptionEntry.aspx', '_self', '');
            }
            else if (getParameterByName("invoker") == "HQReceipt") {
                window.open('../PowerOnRent/HQGoodsReceiptEntry.aspx', '_self', '');
            }
        }

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
        function RequestOpenEntryForm(invoker, state, requestID) {
            if (state != "gray") {
                if (state == "red") { state = "Edit"; }
                else if (state != "red") { state = "View"; }
                PageMethods.WMSetSessionRequest(invoker, requestID, state, RequestOpenEntryFormOnSuccess, null);
            }
            else {
                showAlert("Not Applicable", '', '#');
            }
        }

        function RequestOpenEntryFormOnSuccess(result) {
            switch (result) {
                case "Request":
                    window.open('../PowerOnRent/PartRequestEntry.aspx', '_self', '');
                    break;
                case "Approval":
                    window.open('../PowerOnRent/PartRequestEntry.aspx', '_self', '');
                    break;
                case "Issue":
                    window.open('../PowerOnRent/PartIssueEntry.aspx?invoker=Request', '_self', '');
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
            var SelectedOrder = document.getElementById("SelectedOrder");
            var SelOrdr = SelectedOrder.value;
            var count = (SelOrdr.match(/,/g) || []).length;
            console.log(count);
            if (count >= 1) {
                showAlert("Select Only One Order", "Error", "#");
            }else if (SelectedOrder.value == "") {
                showAlert('Please Select One Order!!!', 'Error', '#');
            } else {
                 var r = confirm("Are You Sure to Cancel This Order?")
                 if (r == true) {
                     PageMethods.WMCancelOrder(SelectedOrder.value, OnSuccessCancelOrder, null);                    
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
    </script>
</asp:Content>

<%@ Page Title="Power On Rent | Material Receipt" Language="C#" MasterPageFile="~/MasterPage/CRM.Master"
    AutoEventWireup="true" CodeBehind="PartReceiptEntry.aspx.cs" Inherits="BrilliantWMS.PowerOnRent.PartReceiptEntry" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="obout_Grid_NET" Namespace="Obout.Grid" TagPrefix="obout" %>
<%@ Register Src="../CommonControls/UC_Date.ascx" TagName="UC_Date" TagPrefix="uc1" %>
<%@ Register Src="../Product/UCProductSearch.ascx" TagName="UCProductSearch" TagPrefix="uc1" %>
<%@ Register Src="../CommonControls/Toolbar.ascx" TagName="Toolbar" TagPrefix="uc2" %>
<%@ Register Src="../CommonControls/UCFormHeader.ascx" TagName="UCFormHeader" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHolder_FormHeader" runat="server">
    <uc2:Toolbar ID="Toolbar1" runat="server" />
    <uc2:UCFormHeader ID="UCFormHeader1" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPHolder_Form" runat="server">
    <center>
        <span id="imgProcessing" style="display: none;">Please wait... </span>
        <div id="divLoading" style="height: 110%; width: 100%; display: none; top: 0; left: 0;"
            class="modal">
            <center>
            </center>
        </div>
        <div class="divHead">
            <h4>
                Request Detail
            </h4>
            <a onclick="javascript:divcollapsOpen('divRequestDetail',this)" id="linkRequest">Expand</a>
        </div>
        <div class="divDetailCollapse" id="divRequestDetail">
            <table class="tableForm" style="width: 900px">
                <tr>
                    <td>
                        Request No.* :
                    </td>
                    <td style="text-align: left; font-weight: bold;">
                        <asp:Label runat="server" ID="lblRequestNo"></asp:Label>
                        <asp:HiddenField ID="hdnRequestID" runat="server" />
                    </td>
                    <td>
                        Request Date :
                    </td>
                    <td style="text-align: left; font-weight: bold;">
                        <asp:Label runat="server" ID="lblRequestDate" Width="182px"></asp:Label>
                    </td>
                    <td>
                        Status :
                    </td>
                    <td style="text-align: left; font-weight: bold;">
                        <asp:Label runat="server" ID="lblRequestStatus" Width="182px"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        Site / Warehouse :
                    </td>
                    <td style="text-align: left; font-weight: bold;">
                        <asp:Label runat="server" ID="lblSites" Width="182px"></asp:Label>
                        <asp:HiddenField ID="hdnSiteID" runat="server" />
                    </td>
                    <td>
                        Request Type :
                    </td>
                    <td style="text-align: left; font-weight: bold;">
                        <asp:Label runat="server" ID="lblRequestType" Width="182px"></asp:Label>
                    </td>
                    <td>
                        Requested By :
                    </td>
                    <td style="text-align: left; font-weight: bold;">
                        <asp:Label runat="server" ID="lblRequestedBy" Width="182px"></asp:Label>
                    </td>
                </tr>
            </table>
        </div>
        <div class="divHead" id="divIssueDetailHead">
            <h4>
                Issue Detail
            </h4>
            <a onclick="javascript:divcollapsOpen('divIssueDetail',this)" id="linkIssueDetail">Expand</a>
        </div>
        <div class="divDetailCollapse" id="divIssueDetail">
            <table class="tableForm" style="width: 900px">
                <tr>
                    <td>
                        Issue No. :
                    </td>
                    <td style="text-align: left; font-weight: bold;">
                        <asp:Label runat="server" ID="lblIssueNo"></asp:Label>
                        <asp:HiddenField runat="server" ID="hdnIssueID" />
                    </td>
                    <td>
                        Issue Date :
                    </td>
                    <td style="text-align: left; font-weight: bold;">
                        <asp:Label runat="server" ID="lblIssueDate"></asp:Label>
                    </td>
                    <td>
                        Issued By :
                    </td>
                    <td style="text-align: left; font-weight: bold;">
                        <asp:Label runat="server" ID="lblIssuedBy"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="6" style="font-style: italic; text-align: left; font-weight: bold;">
                        <hr style="width: 87%; margin-top: 8px; float: right;" />
                        <span>Transport Detail</span>
                    </td>
                </tr>
                <tr>
                    <td>
                        Airway Bill :
                    </td>
                    <td style="text-align: left; font-weight: bold;">
                        <asp:Label runat="server" ID="lblAirwayBill"></asp:Label>
                    </td>
                    <td>
                        Shipping Type :
                    </td>
                    <td style="text-align: left; font-weight: bold;">
                        <asp:Label runat="server" ID="lblShippingType"></asp:Label>
                    </td>
                    <td>
                        Shipping Date :
                    </td>
                    <td style="text-align: left; font-weight: bold;">
                        <asp:Label runat="server" ID="lblShippingDate"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        Exp. Delivery Date :
                    </td>
                    <td style="text-align: left; font-weight: bold;">
                        <asp:Label runat="server" ID="lblExpDelDate"></asp:Label>
                    </td>
                    <td>
                        Transporter Name :
                    </td>
                    <td style="text-align: left; font-weight: bold;">
                        <asp:Label runat="server" ID="lblTransporterName"></asp:Label>
                    </td>
                    <td>
                        Remark :
                    </td>
                    <td style="text-align: left; font-weight: bold;">
                        <asp:Label runat="server" ID="lblIssueRemark"></asp:Label>
                    </td>
                </tr>
            </table>
        </div>
        <div class="divHead" id="divReceiptDetailHead">
            <h4>
                Receipt Detail
            </h4>
            <a onclick="javascript:divcollapsOpen('divReceiptDetail',this)" id="linkReceiptDetail">
                Collapse</a>
        </div>
        <div class="divDetailExpand" id="divReceiptDetail">
            <table class="tableForm" style="width: 900px">
                <tr>
                    <td>
                        Receipt No.* :
                    </td>
                    <td style="text-align: left; font-weight: bold;">
                        <asp:Label runat="server" ID="lblReceiptNo"></asp:Label>
                        <asp:HiddenField ID="hdnReceiptID" runat="server" />
                    </td>
                    <td>
                        Receipt Date * :
                    </td>
                    <td style="text-align: left;">
                        <uc1:UC_Date ID="UC_ReceiptDate" runat="server" />
                    </td>
                    <td>
                        Status * :
                    </td>
                    <td style="text-align: left;">
                        <asp:DropDownList runat="server" ID="ddlStatus" Width="182px" AccessKey="1" DataTextField="Status"
                            DataValueField="ID">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        Received By * :
                    </td>
                    <td style="text-align: left;">
                        <asp:DropDownList runat="server" ID="ddlReceivedBy" Width="182px" AccessKey="1" DataTextField="userName"
                            DataValueField="userID">
                        </asp:DropDownList>
                    </td>
                    <td>
                        Remark :
                    </td>
                    <td colspan="3" style="text-align: left;">
                        <asp:TextBox runat="server" ID="txtReceiptRemark" Width="400px" MaxLength="100"></asp:TextBox>
                    </td>
                </tr>
            </table>
            <table class="gridFrame" id="tblCart" width="90%">
                <tr>
                    <td>
                        <a class="headerText">Receipt Part List </a>
                    </td>
                </tr>
                <tr>
                    <td>
                        <obout:Grid ID="GridReceipt" runat="server" CallbackMode="true" Serialize="true"
                            AllowColumnReordering="true" AllowColumnResizing="true" AutoGenerateColumns="false"
                            AllowPaging="false" ShowLoadingMessage="true" AllowSorting="false" AllowManualPaging="false"
                            AllowRecordSelection="false" ShowFooter="false" Width="100%" PageSize="-1" OnRebind="GridReceipt_OnRebind">
                            <ClientSideEvents ExposeSender="true" />
                            <Columns>
                                <obout:Column DataField="Sequence" HeaderText="Sr.No." AllowEdit="false" Width="7%"
                                    Align="center" HeaderAlign="center">
                                </obout:Column>
                                <obout:Column DataField="Prod_Code" HeaderText="Code" AllowEdit="false" Width="10%"
                                    HeaderAlign="left">
                                </obout:Column>
                                <obout:Column DataField="Prod_Name" HeaderText="Part Name" AllowEdit="false" Width="20%"
                                    HeaderAlign="left">
                                </obout:Column>
                                <obout:Column DataField="Prod_Description" HeaderText="Description" AllowEdit="false"
                                    Width="15%" HeaderAlign="left">
                                </obout:Column>
                                <obout:Column DataField="UOM" HeaderText="UOM" AllowEdit="false" Width="7%" HeaderAlign="left">
                                </obout:Column>
                                <obout:Column DataField="UOMID" AllowEdit="false" Width="0%" Visible="false">
                                </obout:Column>
                                <obout:Column DataField="ChallanQty" Width="10%" HeaderAlign="right" HeaderText="Issued Qty"
                                    Align="right" AllowEdit="false" Wrap="true">
                                    <TemplateSettings TemplateId="GridTemplateRightAlign" />
                                </obout:Column>
                                <obout:Column DataField="ReceivedQty" Width="10%" HeaderAlign="right" HeaderText="Receipt Qty"
                                    Align="right" AllowEdit="false">
                                    <TemplateSettings TemplateId="PlainEditTemplate" />
                                </obout:Column>
                                <obout:Column DataField="ShortQty" Width="10%" HeaderAlign="right" HeaderText="Short Qty"
                                    Align="right" AllowEdit="false">
                                </obout:Column>
                                <obout:Column DataField="ExcessQty" Width="10%" HeaderAlign="right" HeaderText="Excess Qty"
                                    Align="right" AllowEdit="false">
                                </obout:Column>
                                <obout:Column DataField="CurrentStock" HeaderText="Site Stock" AllowEdit="false"
                                    Width="10%" HeaderAlign="center" Align="right" Wrap="true">
                                    <TemplateSettings TemplateId="GridTemplateRightAlign" />
                                </obout:Column>
                            </Columns>
                            <Templates>
                                <obout:GridTemplate runat="server" ID="PlainEditTemplate">
                                    <Template>
                                        <input type="text" class="excel-textbox" value="<%# Container.Value %>" onfocus="markAsFocused(this)"
                                            onkeydown="AllowDecimal(this,event);" onkeypress="AllowDecimal(this,event);"
                                            onblur="markAsBlured(this, '<%# GridReceipt.Columns[Container.ColumnIndex].DataField %>', <%# Container.PageRecordIndex %>)" />
                                    </Template>
                                </obout:GridTemplate>
                                <obout:GridTemplate ID="GridTemplateRightAlign">
                                    <Template>
                                        <span style="text-align: right; width: 130px; margin-right: 10px;">
                                            <%# Container.Value  %></span>
                                    </Template>
                                </obout:GridTemplate>
                            </Templates>
                        </obout:Grid>
                    </td>
                </tr>
            </table>
        </div>
        <div class="divHead">
            <h4>
                Receipt History against Request No.
                <asp:Label runat="server" ID="lblRequestNo2"></asp:Label>
            </h4>
            <a onclick="javascript:divcollapsOpen('divReceiptHistory',this)" id="linkReceiptHistory">
                Collapse</a>
        </div>
        <div class="divDetailExpand" id="divReceiptHistory">
            <table style="font-size: 15px; font-weight: bold; color: Black; float: right;">
                <tr>
                    <td>
                        <div class="PORgray">
                        </div>
                    </td>
                    <td>
                        Not Applicable
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        <div class="PORgreen">
                        </div>
                    </td>
                    <td>
                        Completed
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        <div class="PORred">
                        </div>
                    </td>
                    <td>
                        Pending
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        <div class="PORgreenRed">
                        </div>
                    </td>
                    <td>
                        Partially Completed
                    </td>
                </tr>
            </table>
            <iframe runat="server" id="iframePORReceiptSummary" clientidmode="Static" src="#"
                width="100%" style="border: none;"></iframe>
        </div>
    </center>
    <%--Receipt Head--%>
    <script type="text/javascript">

        var lblRequestNo = document.getElementById("<%= lblRequestNo.ClientID %>");
        var hdnRequestID = document.getElementById("<%= hdnRequestID.ClientID %>");
        var lblSites = document.getElementById("<%= lblSites.ClientID %>");
        var hdnSiteID = document.getElementById("<%= hdnSiteID.ClientID %>");
        var lblRequestStatus = document.getElementById("<%= lblRequestStatus.ClientID %>");
        var lblRequestDate = document.getElementById("<%= lblRequestDate.ClientID %>");
        var lblRequestType = document.getElementById("<%= lblRequestType.ClientID %>");
        var lblRequestedBy = document.getElementById("<%= lblRequestedBy.ClientID %>");

        var lblIssueNo = document.getElementById("<%= lblIssueNo.ClientID %>");
        var hdnIssueID = document.getElementById("<%= hdnIssueID.ClientID %>");
        var lblIssueDate = document.getElementById("<%= lblIssueDate.ClientID %>");
        var lblIssuedBy = document.getElementById("<%= lblIssuedBy.ClientID %>");

        var lblAirwayBill = document.getElementById("<%= lblAirwayBill.ClientID %>");
        var lblShippingType = document.getElementById("<%= lblShippingType.ClientID %>");
        var lblShippingDate = document.getElementById("<%= lblShippingDate.ClientID %>");
        var lblExpDelDate = document.getElementById("<%= lblExpDelDate.ClientID %>");
        var lblTransporterName = document.getElementById("<%= lblTransporterName.ClientID %>");
        var lblIssueRemark = document.getElementById("<%= lblIssueRemark .ClientID %>");

        var lblReceiptNo = document.getElementById("<%= lblReceiptNo.ClientID %>");
        var hdnReceiptID = document.getElementById("<%= hdnReceiptID.ClientID %>");
        var txtReceiptDate = getDateTextBoxFromUC("<%= UC_ReceiptDate.ClientID %>");
        var ddlStatus = document.getElementById("<%= ddlStatus.ClientID %>");
        var ddlReceivedBy = document.getElementById("<%= ddlReceivedBy.ClientID %>");
        var txtReceiptRemark = document.getElementById("<%= txtReceiptRemark .ClientID %>");

        /*Toolbar Code*/
        function jsAddNewCallFromIssueHistory() {
            PageMethods.WMpageAddNew(jsAddNewOnSuccess, null);
        }
        function jsAddNewOnSuccess() {
            ClearMode('divReceiptDetail');

            IssueHistoryOff();
            GridReceipt.refresh();
        }

        function jsSaveData() {
            var validate = validateForm('divReceiptDetail');
            if (validate == false) {
                showAlert("Fill all mandatory fields", "Error", "#");
            }
            else {
                LoadingOn(true);

                var obj1 = new Object();
                obj1.SiteID = hdnSiteID.value;
                obj1.ObjectName = 'MaterialIssue';
                obj1.ReferenceID = hdnIssueID.value;
                obj1.GRN_No = '';
                obj1.GRN_Date = txtReceiptDate.value;
                obj1.ReceivedByUserID = ddlReceivedBy.options[ddlReceivedBy.selectedIndex].value;
                obj1.StatusID = ddlStatus.options[ddlStatus.selectedIndex].value;
                obj1.Remark = txtReceiptRemark.value;
                if (ddlStatus.selectedIndex == 1) { obj1.IsSubmit = "false"; }
                else { obj1.IsSubmit = "true"; }
                PageMethods.WMSaveReceiptHead(obj1, jsSaveData_onSuccessed, jsSaveData_onFailed);
            }
        }

        function jsSaveData_onSuccessed(result) {
            if (result == "Some error occurred" || result == "") { alert("Error occurred", "Error", '#'); }
            else { showAlert(result, "info", "../PowerOnRent/Default.aspx?invoker=Receipt"); }
        }

        function jsSaveData_onFailed() { showAlert("Error occurred", "Error", "#"); }
        /*End Toolbar code*/
    </script>
    <%--End Receipt Head--%>
    <%--Receipt Part Detail--%>
    <script type="text/javascript">
        function markAsFocused(textbox) {
            textbox.className = 'excel-textbox-focused';
            textbox.select();
        }
        var RowIndex = 0;
        function markAsBlured(textbox, dataField, rowIndex) {
            textbox.className = 'excel-textbox';
            RowIndex = rowIndex;
            var txtvalue = textbox.value;
            if (txtvalue == "") txtvalue = 0;
            textbox.value = parseFloat(txtvalue).toFixed(2);
            //ChallanQty, ReceivedQty, ShortQty, ExcessQty

            if (GridReceipt.Rows[rowIndex].Cells[dataField].Value != textbox.value) {
                GridReceipt.Rows[rowIndex].Cells[dataField].Value = textbox.value;
                if (parseInt(GridReceipt.Rows[rowIndex].Cells['ChallanQty'].Value) > parseInt(textbox.value)) {
                    GridReceipt.Rows[rowIndex].Cells['ShortQty'].Value = parseFloat(parseFloat(GridReceipt.Rows[RowIndex].Cells['ChallanQty'].Value) - parseFloat(textbox.value)).toFixed(2);
                    GridReceipt.Rows[rowIndex].Cells['ExcessQty'].Value = "0.00";
                }
                else if (parseInt(GridReceipt.Rows[rowIndex].Cells['ChallanQty'].Value) <= parseInt(textbox.value)) {
                    GridReceipt.Rows[rowIndex].Cells['ShortQty'].Value = "0.00";
                    GridReceipt.Rows[rowIndex].Cells['ExcessQty'].Value = parseFloat(parseFloat(textbox.value) - parseFloat(GridReceipt.Rows[RowIndex].Cells['ChallanQty'].Value)).toFixed(2);
                }

                var body = GridReceipt.GridBodyContainer.firstChild.firstChild.childNodes[1];
                var cell1 = body.childNodes[rowIndex].childNodes[8];
                cell1.firstChild.lastChild.innerHTML = parseFloat(GridReceipt.Rows[rowIndex].Cells['ShortQty'].Value).toFixed(2); ;

                var cell1 = body.childNodes[rowIndex].childNodes[9];
                cell1.firstChild.lastChild.innerHTML = parseFloat(GridReceipt.Rows[rowIndex].Cells['ExcessQty'].Value).toFixed(2);

                PageMethods.WMUpdateReceiptQty(getOrderObject(rowIndex), null, null);
            }
        }

        function getOrderObject(rowIndex) {
            var order = new Object();
            order.Sequence = parseInt(GridReceipt.Rows[rowIndex].Cells['Sequence'].Value);
            order.ReceivedQty = parseFloat(GridReceipt.Rows[rowIndex].Cells['ReceivedQty'].Value).toFixed(2);
            return order;
        }

        /*End Request Part List*/
    </script>
    <%--End Receipt Part Detail--%>
    <%--When PageLoad--%>
    <script type="text/javascript">
        onLoadCheck();
        function onLoadCheck() {

            switch (getParameterByName("invoker")) {
                case "Request":
                    divIssueDetail.style.display = "none";
                    divIssueDetailHead.style.display = "none";
                    divReceiptDetail.style.display = "none";
                    divReceiptDetailHead.style.display = "none";
                    break;
                case "Issue":
                    divReceiptHistory.className = "divDetailCollapse";
                    linkReceiptHistory.innerHTML = "Expand";
                    break;
                case "Receipt":
                    divReceiptHistory.className = "divDetailCollapse";
                    linkReceiptHistory.innerHTML = "Expand";
                    break;

            }
        }

        function ReceiptOpenEntryForm(invoker, state, receiptID) {
            if (state != "gray") {
                if (state == "red") { state = "Edit"; }
                else if (state != "red") { state = "View"; }
                LoadingOn(true);
                PageMethods.WMSetSessionReceipt(invoker, receiptID, state, ReceiptOpenEntryFormOnSuccess, null);
            }
            else {
                showAlert("Not Applicable", "Error", "#");
            }
        }

        function ReceiptOpenEntryFormOnSuccess(result) {
            if (result.Add == false && result.View == false) {
                showAlert("Access Denied", "orange", "#");
                LoadingOff();
            }
            else if (result.Add == false && result.View == true) {
                PageMethods.WMGetReceiptHead(jsGetReceiptHeadOnSuccess, null);
                changemode(true, "divReceiptDetail");
            }
            else {
                PageMethods.WMGetReceiptHead(jsGetReceiptHeadOnSuccess, null);
                changemode(false, "divReceiptDetail");
            }
        }

        function jsGetReceiptHeadOnSuccess(ReceiptRec) {
            divIssueDetail.style.display = "";
            divIssueDetailHead.style.display = "";
            divReceiptDetail.style.display = "";
            divReceiptDetailHead.style.display = "";

            if (txtReceiptDate == null) {
                txtReceiptDate = getDateTextBoxFromUC("<%= UC_ReceiptDate.ClientID %>");
            }

            if (ReceiptRec.GRNH_ID != 0) {
                hdnReceiptID.value = ReceiptRec.GRNH_ID;
                lblReceiptNo.innerHTML = ReceiptRec.GRNH_ID;
                if (ReceiptRec.GRN_Date != null) txtReceiptDate.value = get_ddMMMyyyy(ReceiptRec.GRN_Date);
                ddlStatus.value = ReceiptRec.StatusID;
                ddlReceivedBy.value = ReceiptRec.ReceivedByUserID;
                txtReceiptRemark.value = ReceiptRec.Remark;

                var refID = ReceiptRec.ReferenceID;

                if (ReceiptRec.StatusID != 1) changemode(false, "divReceiptDetail");
            }

            PageMethods.WMGetIssueHead(refID, jsGetIssueHeadOnSuccess, null);
        }

        function jsGetIssueHeadOnSuccess(result) {
            lblRequestNo.innerHTML = result.PRH_ID;
            hdnRequestID.value = result.PRH_ID;
            lblSites.innerHTML = result.SiteName;
            hdnSiteID.value = result.SiteID;
            lblRequestStatus.innerHTML = result.IssueStatus;
            lblRequestDate.innerHTML = get_ddMMMyyyy(result.RequestDate);
            lblRequestType.innerHTML = result.RequestType;
            lblRequestedBy.innerHTML = result.RequestByUserName;

            lblIssueNo.innerHTML = result.MINH_ID;
            hdnIssueID.value = result.MINH_ID;
            lblIssueDate.innerHTML = get_ddMMMyyyy(result.IssueDate);
            lblIssuedBy.innerHTML = result.IssuedByUserName;

            lblAirwayBill.innerHTML = result.AirwayBill;
            lblShippingType.innerHTML = result.ShippingType;
            if (result.ShippingDate != null) lblShippingDate.innerHTML = get_ddMMMyyyy(result.ShippingDate);
            if (result.ExpectedDelDate != null) lblExpDelDate.innerHTML = get_ddMMMyyyy(result.ExpectedDelDate);
            lblTransporterName.innerHTML = result.TransporterName;
            lblIssueRemark.innerHTML = result.IssueRemark;


            GridReceipt.refresh();
            LoadingOff();
        }

    </script>
    <%--End Page Load--%>
    <style type="text/css">
        /*Grid css*/
        .excel-textbox
        {
            background-color: transparent;
            border: 0px;
            padding: 0px;
            outline: 0;
            font: inherit;
            width: 91%;
            padding-top: 4px;
            padding-right: 2px;
            padding-bottom: 4px;
            text-align: right;
        }
        .excel-textbox-focused
        {
            background-color: #FFFFFF;
            border: 0px;
            padding: 0px;
            outline: 0;
            font: inherit;
            width: 91%;
            padding-top: 4px;
            padding-right: 2px;
            padding-bottom: 4px;
            text-align: right;
        }
        
        .excel-textbox-error
        {
            color: #FF0000;
        }
        
        .ob_gCc2
        {
            padding-left: 3px !important;
        }
        
        .ob_gBCont
        {
            border-bottom: 1px solid #C3C9CE;
        }
        
        .excel-checkbox
        {
            height: 20px;
            line-height: 20px;
        }
    </style>
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
        }
        
        .divDetailExpandPopUpOff
        {
            display: none;
        }
        .divDetailExpandPopUpOn
        {
            border: solid 3px gray;
            width: 90%;
            max-height: 500px;
            padding: 10px;
            background-color: #FFFFFF;
            margin-top: 50px;
            top: 5%;
            left: 3%;
            position: absolute;
            z-index: 99999;
        }
        
        .divDetailCollapse
        {
            display: none;
        }
        
        .popupClose
        {
            background: url(../App_Themes/Blue/img/icon_close.png) no-repeat;
            height: 32px;
            width: 32px;
            float: right;
            margin-top: -30px;
            margin-right: -25px;
        }
        .popupClose:hover
        {
            cursor: pointer;
        }
        
        /*End POR Collapsable Div*/
    </style>
</asp:Content>

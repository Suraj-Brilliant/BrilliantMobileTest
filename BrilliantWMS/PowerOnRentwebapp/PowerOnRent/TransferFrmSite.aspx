<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/CRM.Master" AutoEventWireup="true"
    CodeBehind="TransferFrmSite.aspx.cs" Inherits="BrilliantWMS.PowerOnRent.TransferFrmSite" %>

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
                Transfer Detail
            </h4>
            <a onclick="javascript:divcollapsOpen('divPODetail',this)" id="linkPO">Expand</a>
        </div>
        <div class="divDetailExpand" id="divPODetail">
            <table class="tableForm" style="width: 900px">
                <tr>
                    <%--<td>
                        PO No.* :
                    </td>
                    <td style="text-align: left; font-weight: bold;">
                        <asp:TextBox runat="server" ID="txtPONo" MaxLength="20" AccessKey="1" Width="176px"></asp:TextBox>
                    </td>--%>
                    <td>
                        Transfer Date :
                    </td>
                    <td style="text-align: left;">
                        <uc1:UC_Date ID="UC_PODate" runat="server" />
                    </td>
                     <td>
                        Transfered By* :
                    </td>
                    <td style="text-align: left;">
                        <asp:DropDownList runat="server" ID="ddlReceivedBy" Width="182px" AccessKey="1" DataTextField="userName"
                            DataValueField="userID">
                        </asp:DropDownList>
                    </td>

                    <%--<td>
                        Vendor :
                    </td>
                    <td style="text-align: left; font-weight: bold;">
                        <asp:TextBox runat="server" ID="txtVendor" MaxLength="200" Width="200"></asp:TextBox>
                    </td>--%>
                </tr>
                <tr>
                    <td>
                        Transfer from Site / Warehouse * :
                    </td>
                    <td style="text-align: left;">
                        <asp:DropDownList runat="server" ID="ddlSites" Width="182px" DataTextField="Territory"
                            DataValueField="ID" AccessKey="1" onchange="jsFillUsersList(); div1(this);" OnSelectedIndexChanged="ddlStatus_SelectedIndexChange">
                        </asp:DropDownList>
                    </td>
                    <td>
                        Status* :
                    </td>
                    <td style="text-align: left;">
                        <asp:DropDownList runat="server" ID="ddlStatus" Width="182px" AccessKey="1" DataTextField="Status"
                            DataValueField="ID" >
                        </asp:DropDownList>
                    </td>
                    
                    
                </tr>              
            </table>
        </div>
        <div class="divHead">
            <h4>
                Transport Detail
            </h4>
            <a onclick="javascript:divcollapsOpen('divReceiptDetail',this)" id="linkReceiptDetail">
                Collapse</a>
        </div>
        <div class="divDetailExpand" id="divReceiptDetail">
         <%--   <table  style="width: 900px">
                <tr>
                    <%--<td>
                        Receipt No.* :
                    </td>
                    <td style="text-align: left; font-weight: bold;">
                        <asp:Label runat="server" ID="lblReceiptNo" AccessKey="1"></asp:Label>
                        
                    </td>
                    <td>
                        Receipt Date* :
                    </td>
                    <td style="text-align: left;">
                        <uc1:UC_Date ID="UC_ReceiptDate" runat="server" />
                    </td>
                    
                </tr>
                <tr>
                   
                    
                </tr>
            </table>--%>
            <table class="tableForm">
                               
                <tr>
                    <td>
                        Airway Bill :
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="txtAirwayBill" MaxLength="50" Width="180px"></asp:TextBox>
                    </td>
                    <td>
                        Shipping Type :
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="txtShippingType" MaxLength="50" Width="180px"></asp:TextBox>
                    </td>
                    <td>
                        Shipping Date :
                    </td>
                    <td style="text-align: left;">
                        <uc1:UC_Date ID="UC_ShippingDate" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        Exp. Delivery Date :
                    </td>
                    <td style="text-align: left;">
                        <uc1:UC_Date ID="UC_ExpDeliveryDate" runat="server" />
                    </td>
                    <td>
                        Transporter Name :
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="txtTransporterName" MaxLength="50" Width="180px"></asp:TextBox>
                    </td>
                    <td>
                        Remark :
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="txtIssueRemark" MaxLength="100" Width="180px"></asp:TextBox>
                    </td>
                </tr>
                 <asp:HiddenField ID="hdnReceiptID" runat="server" />                 
            </table>
            <table class="gridFrame" id="tblCart" width="90%">
                <tr>
                    <td>
                        <a class="headerText">Receipt Part List </a>
                    </td>
                    <td style="text-align: right;">
                        <uc1:UCProductSearch ID="UCProductSearch1" runat="server" Visible="false" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <obout:Grid ID="Grid1" runat="server" CallbackMode="true" Serialize="true" AllowColumnReordering="true"
                            AllowColumnResizing="true" AutoGenerateColumns="false" AllowPaging="false" ShowLoadingMessage="true"
                            AllowSorting="false" AllowManualPaging="false" AllowRecordSelection="false" ShowFooter="false"
                            Width="100%" PageSize="-1" OnRebind="GridReceipt_OnRebind">
                            <ClientSideEvents ExposeSender="true" />
                            <Columns>
                                <obout:Column DataField="Sequence" HeaderText="Sr.No." AllowEdit="false" Width="7%"
                                    Align="center" HeaderAlign="center">
                                    <TemplateSettings TemplateId="ItemTempRemove" />
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
                                <obout:Column DataField="RequestQty" Width="12%" HeaderAlign="center" HeaderText="Pending For Transfer"
                                    Align="right" AllowEdit="false" Wrap="true">
                                    <TemplateSettings TemplateId="GridTemplateRightAlign" />
                                </obout:Column>
                                <obout:Column DataField="IssuedQty" Width="10%" HeaderAlign="right" HeaderText="Transfer Qty"
                                    Align="right" AllowEdit="false">
                                    <TemplateSettings TemplateId="PlainEditTemplate" />
                                </obout:Column>
                                 <obout:Column DataField="RemaningQty" Width="10%" HeaderAlign="right" HeaderText="Remaining Qty"
                                    Align="right" AllowEdit="false">
                                </obout:Column>
                                <obout:Column DataField="CurrentStockHQ" HeaderText="Current Stock" AllowEdit="false"
                                    Width="10%" HeaderAlign="center" Align="right" Wrap="true">
                                    <TemplateSettings TemplateId="GridTemplateRightAlign" />
                                </obout:Column>
                            </Columns>
                            <Templates>
                                <obout:GridTemplate ID="ItemTempRemove">
                                    <Template>
                                        <table>
                                            <tr>
                                                <td style="width: 20px; text-align: center;">
                                                    <img id="imgbtnRemove" src="../App_Themes/Grid/img/Remove16x16.png" title="Remove"
                                                        onclick="removePartFromList('<%# (Container.DataItem["Sequence"].ToString()) %>');"
                                                        style="cursor: pointer;" />
                                                </td>
                                                <td style="width: 35px; text-align: center;">
                                                    <%# Convert.ToInt32(Container.PageRecordIndex) + 1 %>
                                                </td>
                                            </tr>
                                        </table>
                                    </Template>
                                </obout:GridTemplate>
                                <obout:GridTemplate runat="server" ID="PlainEditTemplate">
                                    <Template>
                                        <input type="text" class="excel-textbox" value="<%# Container.Value %>" onfocus="markAsFocused(this)"
                                            onkeydown="AllowDecimal(this,event);" onkeypress="AllowDecimal(this,event);"
                                            onblur="markAsBlured(this, '<%# Grid1.Columns[Container.ColumnIndex].DataField %>', <%# Container.PageRecordIndex %>)" />
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
        <asp:HiddenField ID="hdnToSite" runat="server" />
        <asp:HiddenField ID="hdnIssueID" runat="server" />
    </center>
     <%--Receipt Head--%>
    <script type="text/javascript">
        window.onload = function jsGetToSiteID() {
            var IssueID = '<%= Session["PORIssueID"].ToString() %>';           
            PageMethods.GetToSiteID(IssueID, OnSuccess_SiteID, null)
        };

        function OnSuccess_SiteID(result) {
            var hdnToSite = document.getElementById("<%= hdnToSite.ClientID%>");
            hdnToSite.value = result;
        }

        function jsGetDates() {
            UC_TransferDate = getDateFromUC("<%= UC_PODate.ClientID %>");
            UC_ShippingDate = getDateFromUC("<%= UC_ShippingDate.ClientID %>");
            UC_ExpDeliveryDate = getDateFromUC("<%= UC_ExpDeliveryDate.ClientID %>");
        }

        

        var ddlStatus = document.getElementById("<%= ddlStatus.ClientID %>");
        var ddlTransferedBy = document.getElementById("<%= ddlReceivedBy.ClientID %>");
        var txtAirwayBill = document.getElementById("<%= txtAirwayBill.ClientID %>");
        var txtShippingType = document.getElementById("<%= txtShippingType.ClientID %>");
        var txtTransporterName = document.getElementById("<%= txtTransporterName.ClientID %>");
        var txtIssueRemark = document.getElementById("<%= txtIssueRemark.ClientID %>");

        var UC_TransferDate = "";
        var UC_ShippingDate = "";
        var UC_ExpDeliveryDate = "";
        var ddlSites = document.getElementById("<%= ddlSites.ClientID %>");
        var hdnReceiptID = document.getElementById("<%= hdnReceiptID.ClientID %>");

        var SelectedStatus = "0";
        var SelectedTransferedByUserID = "0";

       

        /*Toolbar Code*/
        function jsAddNew() {
            PageMethods.WMpageAddNew(jsAddNewOnSuccess, null);
        }
        function jsAddNewOnSuccess() {
            page.refresh();
           // window.open('../PowerOnRent/TransferFromSite.aspx', '_self', '');
        }

        function jsSaveData() {
           // jsGetToSiteID();
            var hdnToSite = document.getElementById("<%= hdnToSite.ClientID%>").value;
           
            var validate = validateForm('divPODetail');           
            if (validate == false) {
                showAlert("Fill all mandatory fields", "error", '#');
            }
            else {
                if (ddlStatus.options[ddlStatus.selectedIndex].text == "Transfer" && Grid1.Rows.length == 0) {
                    showAlert("Add atleast one part into the Transfer Part List", "error", "#");
                }
                else {
                    //LoadingOn(true);                    

                    var obj1 = new Object();
                    jsGetDates();

                    obj1.ToSiteID = hdnToSite;
                    obj1.TransferDate = UC_TransferDate;
                    obj1.TransferedByUserID = ddlTransferedBy.options[ddlTransferedBy.selectedIndex].value;
                    obj1.StatusID = ddlStatus.options[ddlStatus.selectedIndex].value;
                    obj1.AirwayBill = txtAirwayBill.value;
                    obj1.SupplierCoNo = "";
                    obj1.ShippingType = txtShippingType.value;
                    obj1.ShippingStatus = "";
                    obj1.ShippingDate = UC_ShippingDate;
                    obj1.ExpectedDelDate = UC_ExpDeliveryDate;
                    obj1.TransporterName = txtTransporterName.value;
                    obj1.Remark = txtIssueRemark.value;
                    if (ddlStatus.selectedIndex == 1) { obj1.IsSubmit = "false"; }
                    else { obj1.IsSubmit = "true"; }
                    obj1.FromSiteID = ddlSites.options[ddlSites.selectedIndex].value;

                    PageMethods.WMSaveTransferHead(obj1, jsSaveData_onSuccessed, jsSaveData_onFailed);
                }
            }
        }

        function jsSaveData_onSuccessed(result) {
            if (result == "") { alert("Error occurred", "Error", '#'); }
            else {
                var obj2 = new Object();
                jsGetDates();

//                var hdnIssueID = document.getElementById('hdnIssueID');
//                hdnIssueID = '<%= Session["PORIssueID"].ToString() %>';  
                var hdnToSite = document.getElementById("<%= hdnToSite.ClientID%>").value;
//                hdnIssueID = document.getElementById('hdnIssueID').value;

                obj2.MINH_ID = '<%= Session["PORIssueID"].ToString() %>';  
                obj2.SiteID = hdnToSite;
                obj2.IssueDate = UC_TransferDate;
                obj2.IssuedByUserID = ddlTransferedBy.options[ddlTransferedBy.selectedIndex].value;
                obj2.StatusID = ddlStatus.options[ddlStatus.selectedIndex].value;
                obj2.Title = "";
                obj2.AirwayBill = txtAirwayBill.value;
                obj2.SupplierCoNo = "";
                obj2.ShippingType = txtShippingType.value;
                obj2.ShippingStatus = "";
                obj2.ShippingDate = UC_ShippingDate;
                obj2.ExpectedDelDate = UC_ExpDeliveryDate;
                obj2.TransporterName = txtTransporterName.value;
                obj2.Remark = txtIssueRemark.value;
                obj2.TransH_ID = result;
                if (ddlStatus.selectedIndex == 1) { obj2.IsSubmit = "false"; }
                else { obj2.IsSubmit = "true"; }

                PageMethods.WMSaveIssueHead(obj2, jsSaveDataIssue_onSuccessed, jsSaveDataIssue_onFailed);
            }
        }

        function jsSaveDataIssue_onSuccessed(result) {
            if (result == "Some error occurred" || result == "") { alert("Error occurred", "Error", '#'); }
            else { showAlert(result, "info", "../PowerOnRent/Default.aspx?invoker=Request"); }
        }

        function jsSaveDataIssue_onFailed() {
            showAlert("Error occurred", "Error", '#');
        }

        function jsSaveData_onFailed() {
            showAlert("Error occurred", "Error", '#');
        }

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

            if (Grid1.Rows[rowIndex].Cells[dataField].Value != textbox.value) {
                Grid1.Rows[rowIndex].Cells[dataField].Value = textbox.value;

                var HQCurrentStock = parseFloat(Grid1.Rows[RowIndex].Cells['CurrentStockHQ'].Value).toFixed(2);
                var RequestQty = parseFloat(Grid1.Rows[RowIndex].Cells['RequestQty'].Value).toFixed(2);
                var IssueQty = parseFloat(textbox.value).toFixed(2);

                if ((parseFloat(textbox.value) > parseFloat(RequestQty)) || (parseFloat(textbox.value) > parseFloat(HQCurrentStock))) {
                    showAlert("Transfer Quantity should not be greater than Current Stock & Pending Transfer Qty", "error", '#');
                    if (parseFloat(RequestQty) > parseFloat(HQCurrentStock)) {
                        textbox.value = parseFloat(HQCurrentStock).toFixed(2);
                    }
                    else {
                        textbox.value = parseFloat(RequestQty).toFixed(2);
                    }
                }

                Grid1.Rows[RowIndex].Cells['RemaningQty'].Value = (parseFloat(RequestQty) - parseFloat(textbox.value)).toFixed(2);

                Grid1.Rows[RowIndex].Cells['CurrentStockHQ'].Value = (parseFloat(HQCurrentStock) - parseFloat(textbox.value)).toFixed(2);

                var body = Grid1.GridBodyContainer.firstChild.firstChild.childNodes[1];
                var cell1 = body.childNodes[RowIndex].childNodes[8];
                cell1.firstChild.lastChild.innerHTML = parseFloat(Grid1.Rows[RowIndex].Cells['RemaningQty'].Value).toFixed(2);

                PageMethods.WMUpdateIssueQty(getOrderObject(), null, null);
                var cell2 = body.childNodes[RowIndex].childNodes[9];
                cell2.firstChild.lastChild.innerHTML = parseFloat(Grid1.Rows[RowIndex].Cells['CurrentStockHQ'].Value).toFixed(2);
                PageMethods.WMUpdateHQStock(getOrderObject(), null, null);
            }
        }

        function getOrderObject() {
            var order = new Object();
            order.Sequence = parseInt(Grid1.Rows[RowIndex].Cells["Sequence"].Value);
            order.IssuedQty = parseFloat(Grid1.Rows[RowIndex].Cells["IssuedQty"].Value).toFixed(2);
            return order;
        }

        function removePartFromList(sequence) {
            /*Remove Part from list*/
//            var hdnProductSearchSelectedRec = document.getElementById('hdnProductSearchSelectedRec');
//            hdnProductSearchSelectedRec.value = "";
            PageMethods.WMRemovePartFromRequest(sequence, removePartFromListOnSussess, null);
        }

        function removePartFromListOnSussess() {
            document.getElementById('hdnProductSearchSelectedRec').value = "";
            Grid1.refresh();
        }
        /*End Request Part List*/
    </script>
    <%--End Receipt Part Detail--%>
    <%--Fill DropDown--%>
    <script type="text/javascript">
        function jsFillUsersList() {
            var ddlReceivedBy = document.getElementById("<%= ddlReceivedBy.ClientID %>");
            ddlReceivedBy.options.length = 0;
            if (parseInt(ddlSites.value) > 0) {
                ddlLoadingOn(ddlReceivedBy);
                PageMethods.WMFillUserList(parseInt(ddlSites.value), jsFillUsersListOnSuccess, jsFillUsersListOnFail);
            }
        }

        function jsFillUsersListOnSuccess(result) {
            var ddlR = document.getElementById("<%= ddlReceivedBy.ClientID %>");

            for (var i = 0; i < result.length; i++) {
                var option1 = document.createElement("option");
                option1.text = result[i].userName;
                option1.value = result[i].userID;
                try {
                    ddlR.add(option1, null); //Standard 
                } catch (error) {
                    ddlR.add(option1); // IE only
                }
            }
            ddlLoadingOff(ddlR);
        }

        function jsFillUsersListOnFail() {
            ddlLoadingOff(ddlReceivedBy);
        }

        function div1(obj) {
            Grid1.refresh();
        }

        /*End Fill Dropdown*/
    </script>
    <%--End Fill DropDown--%>
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

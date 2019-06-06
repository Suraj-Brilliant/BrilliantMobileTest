<%@ Page Title="Power On Rent | Goods Receipt [HQ]" Language="C#" MasterPageFile="~/MasterPage/CRM.Master"
    AutoEventWireup="true" CodeBehind="HQGoodsReceiptEntry.aspx.cs" Inherits="BrilliantWMS.PowerOnRent.HQGoodsReceiptEntry"
    EnableEventValidation="false" %>

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
                Purchases Order Detail
            </h4>
            <a onclick="javascript:divcollapsOpen('divPODetail',this)" id="linkPO">Expand</a>
        </div>
        <div class="divDetailExpand" id="divPODetail">
            <table class="tableForm" style="width: 900px">
                <tr>
                    <td>
                        PO No.* :
                    </td>
                    <td style="text-align: left; font-weight: bold;">
                        <asp:TextBox runat="server" ID="txtPONo" MaxLength="20" AccessKey="1" Width="176px"></asp:TextBox>
                    </td>
                    <td>
                        PO Date :
                    </td>
                    <td style="text-align: left;">
                        <uc1:UC_Date ID="UC_PODate" runat="server" />
                    </td>
                    <td>
                        Vendor :
                    </td>
                    <td style="text-align: left; font-weight: bold;">
                        <asp:TextBox runat="server" ID="txtVendor" MaxLength="200" Width="200"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        Site / Warehouse * :
                    </td>
                    <td style="text-align: left;">
                        <asp:DropDownList runat="server" ID="ddlSites" Width="182px" DataTextField="Territory"
                            DataValueField="ID" AccessKey="1" onchange="jsFillUsersList()">
                        </asp:DropDownList>
                    </td>
                    <td>
                    </td>
                    <td>
                    </td>
                    <td>
                    </td>
                    <td>
                    </td>
                </tr>
            </table>
        </div>
        <div class="divHead">
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
                        <asp:Label runat="server" ID="lblReceiptNo" AccessKey="1"></asp:Label>
                        <asp:HiddenField ID="hdnReceiptID" runat="server" />
                    </td>
                    <td>
                        Receipt Date* :
                    </td>
                    <td style="text-align: left;">
                        <uc1:UC_Date ID="UC_ReceiptDate" runat="server" />
                    </td>
                    <td>
                        Status* :
                    </td>
                    <td style="text-align: left;">
                        <asp:DropDownList runat="server" ID="ddlStatus" Width="182px" AccessKey="1" DataTextField="Status"
                            DataValueField="ID">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        Received By* :
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
                    <td style="text-align: right;">
                        <uc1:UCProductSearch ID="UCProductSearch1" runat="server" />
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
                                <obout:Column DataField="ReceivedQty" Width="10%" HeaderAlign="right" HeaderText="Receipt Qty"
                                    Align="right" AllowEdit="false">
                                    <TemplateSettings TemplateId="PlainEditTemplate" />
                                </obout:Column>
                                <obout:Column DataField="CurrentStock" HeaderText="Current Stock" AllowEdit="false"
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
    </center>
    <%--Receipt Head--%>
    <script type="text/javascript">
        var txtPONo = document.getElementById("<%= txtPONo.ClientID %>");
        var txt_PODate = getDateTextBoxFromUC("<%= UC_PODate.ClientID %>");
        var txtVendor = document.getElementById("<%= txtVendor.ClientID %>");
        var ddlSites = document.getElementById("<%= ddlSites.ClientID %>");

        var lblReceiptNo = document.getElementById("<%= lblReceiptNo.ClientID %>");
        var hdnReceiptID = document.getElementById("<%= hdnReceiptID.ClientID %>");
        var txtReceiptDate = getDateTextBoxFromUC("<%= UC_ReceiptDate.ClientID %>");
        var ddlStatus = document.getElementById("<%= ddlStatus.ClientID %>");
        var ddlReceivedBy = document.getElementById("<%= ddlReceivedBy.ClientID %>");
        var txtReceiptRemark = document.getElementById("<%= txtReceiptRemark .ClientID %>");

        /*Toolbar Code*/
        function jsAddNew() {
            PageMethods.WMpageAddNew(jsAddNewOnSuccess, null);
        }
        function jsAddNewOnSuccess() {
            window.open('../PowerOnRent/HQGoodsReceiptEntry.aspx', '_self', '');
        }

        function jsSaveData() {
            var validate = validateForm('divPODetail');
            if (validate == false) {

            }
            else {
                validate = validateForm('divReceiptDetail');
            }

            if (validate == false) {
                showAlert("Fill all mandatory fields", "Error", "#");
            }
            else {
                LoadingOn(true);
                var obj1 = new Object();
                obj1.SiteID = ddlSites.value;
                obj1.ObjectName = 'PurchasesOrder';
                obj1.ReferenceID = 0;
                obj1.GRN_No = '';
                obj1.GRN_Date = txtReceiptDate.value;
                obj1.ReceivedByUserID = ddlReceivedBy.options[ddlReceivedBy.selectedIndex].value;
                obj1.StatusID = ddlStatus.options[ddlStatus.selectedIndex].value;
                obj1.Remark = txtReceiptRemark.value;

                /*PO details*/
                obj1.ChallanNo = txtPONo.value;
                obj1.ChallanDate = null;
                if (txt_PODate.value != "") obj1.ChallanDate = txt_PODate.value;
                obj1.Vendor = txtVendor.value;

                if (ddlStatus.selectedIndex == 1) { obj1.IsSubmit = "false"; }
                else { obj1.IsSubmit = "true"; }
                PageMethods.WMSaveReceiptHead(obj1, jsSaveData_onSuccessed, jsSaveData_onFailed);
            }
        }

        function jsSaveData_onSuccessed(result) {
            if (result == "Some error occurred" || result == "") { alert("Error occurred", "Error", '#'); }
            else { showAlert(result, "info", "../PowerOnRent/Default.aspx?invoker=HQReceipt"); }
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

            if (Grid1.Rows[rowIndex].Cells[dataField].Value != textbox.value) {
                Grid1.Rows[rowIndex].Cells[dataField].Value = textbox.value;

                PageMethods.WMUpdateReceiptQty(getOrderObject(rowIndex), null, null);
            }
        }

        function getOrderObject(rowIndex) {
            var order = new Object();
            order.Sequence = Grid1.Rows[rowIndex].Cells['Sequence'].Value;
            order.ReceivedQty = Grid1.Rows[rowIndex].Cells['ReceivedQty'].Value;
            return order;
        }
        function removePartFromList(sequence) {
            /*Remove Part from list*/
            var hdnProductSearchSelectedRec = document.getElementById('hdnProductSearchSelectedRec');
            hdnProductSearchSelectedRec.value = "";
            PageMethods.WMRemovePartFromRequest(sequence, removePartFromListOnSussess, null);
        }

        function removePartFromListOnSussess() {
            Grid1.refresh();
        }
        /*End Request Part List*/
    </script>
    <%--End Receipt Part Detail--%>
    <%--Fill DropDown--%>
    <script type="text/javascript">
        function jsFillUsersList() {
            ddlReceivedBy.options.length = 0;
            if (parseInt(ddlSites.value) > 0) {
                ddlLoadingOn(ddlReceivedBy);
                PageMethods.WMFillUserList(parseInt(ddlSites.value), jsFillUsersListOnSuccess, jsFillUsersListOnFail);
            }
        }

        function jsFillUsersListOnSuccess(result) {
            var ddlR = ddlReceivedBy;

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

<%@ Page Title="Power On Rent | Consumption" Language="C#" MasterPageFile="~/MasterPage/CRM.Master"
    AutoEventWireup="true" CodeBehind="PartConsumptionEntry.aspx.cs" Inherits="PowerOnRentwebapp.PowerOnRent.PartConsumptionEntry"
    EnableEventValidation="false" %>

<%@ Register Src="../Product/UCProductSearch.ascx" TagName="UCProductSearch" TagPrefix="uc1" %>
<%@ Register Src="../CommonControls/Toolbar.ascx" TagName="Toolbar" TagPrefix="uc2" %>
<%@ Register Src="../CommonControls/UCFormHeader.ascx" TagName="UCFormHeader" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="obout_Grid_NET" Namespace="Obout.Grid" TagPrefix="obout" %>
<%@ Register Src="../CommonControls/UC_Date.ascx" TagName="UC_Date" TagPrefix="uc1" %>
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
                <span id="pupUpLoading" class="loadingText" style="display: none;">Loading...</span>
            </center>
        </div>
        <div class="divHead">
            <h4>
                Consumption Detail
            </h4>
            <a onclick="javascript:divcollapsOpen('divConsumptionDetail',this)" id="linkConsumption">
                Expand</a>
        </div>
        <div class="divDetailExpand" id="divConsumptionDetail">
            <table class="tableForm">
                <tr>
                    <td>
                        Site * :
                    </td>
                    <td style="text-align: left;">
                        <asp:DropDownList runat="server" ID="ddlSites" Width="182px" DataTextField="Territory"
                            DataValueField="ID" AccessKey="1" onchange="jsFillUsersList();">
                        </asp:DropDownList>
                    </td>
                    <td>
                        Consumption Date * :
                    </td>
                    <td style="text-align: left;">
                        <uc1:UC_Date ID="UC_DateConsumption" runat="server" AccessKey="1" />
                    </td>
                    <td>
                        Status * :
                    </td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlStatus" Width="182px" AccessKey="1" DataTextField="Status"
                            DataValueField="ID">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        Consumed By * :
                    </td>
                    <td style="text-align: left;">
                        <asp:DropDownList runat="server" ID="ddlConsumedBy" Width="182px" AccessKey="1" DataTextField="userName"
                            DataValueField="userID">
                        </asp:DropDownList>
                    </td>
                    <td>
                        Remark :
                    </td>
                    <td colspan="3" style="text-align: left;">
                        <asp:TextBox runat="server" ID="txtRemark" Width="415px" MaxLength="100"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="6" style="height: 3px;">
                    </td>
                </tr>
                <tr>
                    <td colspan="6" style="font-style: italic; text-align: left; font-weight: bold;">
                        <hr style="width: 85%; margin-top: 8px; float: right;" />
                        <span>Equipment Details</span>
                    </td>
                </tr>
                <tr id="TrReq1">
                    <td>
                        Container * :
                    </td>
                    <td style="text-align: left;">
                        <asp:DropDownList runat="server" ID="ddlContainer" Width="182px" onchange="jsGetEngineDetails(this)"
                            DataTextField="Container" DataValueField="ID" AccessKey="1">
                        </asp:DropDownList>
                    </td>
                    <td>
                        Engine Model :
                    </td>
                    <td style="text-align: left; font-weight: bold;">
                        <asp:Label runat="server" ID="lblEngineModel" Width="176px" AccessKey="1"></asp:Label>
                    </td>
                    <td>
                        Engine Serial :
                    </td>
                    <td style="text-align: left; font-weight: bold;">
                        <asp:Label runat="server" ID="lblEngineSerial" Width="176px" AccessKey="1"></asp:Label>
                    </td>
                </tr>
                <tr id="TrReq2">
                    <td>
                        Failure Hours* :
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="txtFailureHours" MaxLength="6" Width="176px" AccessKey="1"
                            onkeypress="AllowInt(this,event);"></asp:TextBox>
                    </td>
                    <td>
                        Cause of Failure* :
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="txtFailureCause" onkeyup="TextBox_KeyUp(this,'CharactersCounter1','100');"
                            ClientIDMode="Static" Width="176px" TextMode="MultiLine" AccessKey="1"></asp:TextBox>
                        <br />
                        <span class="watermark"><span id="CharactersCounter1" accesskey="2">100</span> / 100
                        </span>
                    </td>
                    <td>
                        Nature of Failure* :
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="txtFailureNature" onkeyup="TextBox_KeyUp(this,'CharactersCounter2','100');"
                            ClientIDMode="Static" Width="176px" TextMode="MultiLine" AccessKey="1"></asp:TextBox>
                        <br />
                        <span class="watermark"><span id="CharactersCounter2" accesskey="2">100</span> / 100
                        </span>
                    </td>
                </tr>
            </table>
            <table class="gridFrame" width="80%" id="tblCart">
                <tr>
                    <td>
                        <table style="width: 100%">
                            <tr>
                                <td style="text-align: left;">
                                    <a class="headerText">Consumption Part List </a>
                                </td>
                                <td style="text-align: right;">
                                    <uc1:UCProductSearch ID="UCProductSearch1" runat="server" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <obout:Grid ID="Grid1" runat="server" CallbackMode="true" Serialize="true" AllowColumnReordering="true"
                            AllowColumnResizing="true" AutoGenerateColumns="false" AllowPaging="false" ShowLoadingMessage="true"
                            AllowSorting="false" AllowManualPaging="false" AllowRecordSelection="false" ShowFooter="false"
                            Width="100%" PageSize="-1" OnRebind="Grid1_OnRebind">
                            <Columns>
                                <obout:Column DataField="Sequence" HeaderText="Sr.No." AllowEdit="false" Width="7%"
                                    Align="left" HeaderAlign="center">
                                    <TemplateSettings TemplateId="ItemTempRemove" />
                                </obout:Column>
                                <obout:Column DataField="Prod_Code" HeaderText="Code" AllowEdit="false" Width="8%"
                                    HeaderAlign="left">
                                </obout:Column>
                                <obout:Column DataField="Prod_Name" HeaderText="Product Name" AllowEdit="false" Width="20%"
                                    HeaderAlign="left">
                                </obout:Column>
                                <obout:Column DataField="Prod_Description" HeaderText="Description" AllowEdit="false"
                                    Width="20%" HeaderAlign="left">
                                </obout:Column>
                                <obout:Column DataField="UOM" HeaderText="UOM" AllowEdit="false" Width="7%" HeaderAlign="left">
                                </obout:Column>
                                <obout:Column DataField="UOMID" AllowEdit="false" Width="0%" Visible="false">
                                </obout:Column>
                                <obout:Column DataField="ConsumedQty" Width="10%" HeaderAlign="center" HeaderText="Consumed Quantity"
                                    Align="center" AllowEdit="false">
                                    <TemplateSettings TemplateId="PlainEditTemplate" />
                                </obout:Column>
                                <obout:Column DataField="CurrentStock" HeaderText="Current Stock" AllowEdit="false"
                                    Width="10%" HeaderAlign="center" Align="right">
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
                                                <td style="width: 35px; text-align: right;">
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
        <asp:HiddenField runat="server" ID="hdnSelectedProd_IDs" ClientIDMode="Static" />
        <asp:HiddenField runat="server" ID="hdnSelectedGRND_IDs" ClientIDMode="Static" />
    </center>
    <script type="text/javascript">
        var ddlSites = document.getElementById("<%= ddlSites.ClientID %>");
        var txtConsumptionDate = getDateTextBoxFromUC("<%= UC_DateConsumption.ClientID %>");
        var ddlStatus = document.getElementById("<%= ddlStatus.ClientID %>");
        var ddlConsumedBy = document.getElementById("<%= ddlConsumedBy.ClientID %>");
        var ddlContainer = document.getElementById("<%= ddlContainer.ClientID %>");
        var lblEngineModel = document.getElementById("<%= lblEngineModel.ClientID %>");
        var lblEngineSerial = document.getElementById("<%= lblEngineSerial.ClientID %>");
        var txtFailureHours = document.getElementById("<%= txtFailureHours.ClientID %>");
        var txtFailureCause = document.getElementById("<%= txtFailureCause.ClientID %>");
        var txtFailureNature = document.getElementById("<%= txtFailureNature.ClientID %>");
        var txtRemark = document.getElementById("<%= txtRemark.ClientID %>");
        /*AddNew Code*/
        function jsAddNew() {
            PageMethods.WMpageAddNew(jsAddNewOnSuccess, null);
        }
        function jsAddNewOnSuccess() {
            window.open('../PowerOnRent/PartConsumptionEntry.aspx', '_self', '');
        }
        /*End*/


        /*Save Code*/
        function jsSaveData() {
            var validate = validateForm('divConsumptionDetail');
            if (validate == false) {
                showAlert("Fill all mandatory fields", "error", "#");
            }
            else {
                LoadingOn(true);

                var obj1 = new Object();
                obj1.SiteID = parseInt(ddlSites.options[ddlSites.selectedIndex].value);
                obj1.ConsumptionDate = txtConsumptionDate.value;
                obj1.StatusID = parseInt(ddlStatus.options[ddlStatus.selectedIndex].value);

                obj1.ConsumedByUserID = parseInt(ddlConsumedBy.options[ddlConsumedBy.selectedIndex].value);
                obj1.Remark = txtRemark.value.toString();

                obj1.FailureCause = txtFailureCause.value.toString();
                obj1.FailureHours = txtFailureHours.value.toString();
                obj1.FailureNature = txtFailureNature.value.toString();

                obj1.EngineSerial = lblEngineSerial.innerHTML.toString();
                obj1.EngineModel = lblEngineModel.innerHTML.toString();
                obj1.GeneratorModel = "";
                obj1.GeneratorSerial = "";
                obj1.TransformerSerial = "";
                obj1.Container = ddlContainer.options[ddlContainer.selectedIndex].text;
                PageMethods.WMSaveConsumption(obj1, WMSave_onSuccessed, WMSave_onFailed);
            }
        }

        function WMSave_onSuccessed(result) {
            if (result == "Some error occurred") {
                showAlert(result, 'Error', "#"); ;
            }
            else {
                showAlert(result, "info", '../PowerOnRent/Default.aspx?invoker=Consumption');
            }
        }
        function WMSave_onFailed() {
            showAlert("Error occurred", 'Error', "#");
        }
        /*End save code*/
        /*Get Engine Details when Select Engine*/
        function jsGetEngineDetails(ddl) {
            if (ddl.selectedIndex > 0) {
                PageMethods.WMGetEngineDetails(ddl.options[ddl.selectedIndex].value, jsGetEngineDetailsOnSussess, null);
            }
            else {
                lblEngineModel.innerHTML = "";
                lblEngineSerial.innerHTML = "";
            }
        }

        function jsGetEngineDetailsOnSussess(result) {
            lblEngineModel.innerHTML = result.EngineModel;
            lblEngineSerial.innerHTML = result.EngineSerial;

        }
        /*End*/


        ///Fille Dropdown
        function jsFillUsersList() {
            ddlConsumedBy.options.length = 0;
            if (ddlSites.selectedIndex > 0) {
                ddlLoadingOn(ddlConsumedBy);
                PageMethods.WMFillUserList(ddlSites.options[ddlSites.selectedIndex].value, jsFillUsersListOnSuccess, jsFillUsersListOnFail);
            }
        }

        function jsFillUsersListOnSuccess(result) {
            var ddlR = ddlConsumedBy;

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
            jsFillEnginList();
        }

        function jsFillUsersListOnFail() {
            ddlLoadingOff(ddlConsumedBy);
        }


        function jsFillEnginList() {
            ddlContainer.options.length = 0;
            if (ddlSites.selectedIndex > 0) {
                ddlLoadingOn(ddlContainer);
                PageMethods.WMFillEnginList(ddlSites.options[ddlSites.selectedIndex].value, jsFillEnginListOnSuccess, jsFillEnginListOnFail);
            }
        }
        function jsFillEnginListOnSuccess(result) {
            var ddlEng = ddlContainer;

            for (var i = 0; i < result.length; i++) {
                var optionE1 = document.createElement("option");
                optionE1.text = result[i].Container;
                optionE1.value = result[i].ID;
                try {
                    ddlEng.add(optionE1, null); //Standard 
                } catch (error) {
                    ddlEng.add(optionE1); // IE only
                }
            }
            ddlLoadingOff(ddlEng);
        }

        function jsFillEnginListOnFail() {
            ddlLoadingOff(ddlContainer);
        }

        /*Consumption Part Detail*/
        function markAsFocused(textbox) {
            textbox.className = 'excel-textbox-focused';
            textbox.select();
        }

        function markAsBlured(textbox, dataField, rowIndex) {
            textbox.className = 'excel-textbox';
            if (textbox.value == "") textbox.value = "0.00";
            textbox.value = parseFloat(textbox.value).toFixed(2);
            var SiteCurrentStock = Grid1.Rows[rowIndex].Cells["CurrentStock"].Value;

            if (parseFloat(Grid1.Rows[rowIndex].Cells[dataField].Value) != parseFloat(textbox.value)) {
                if (parseFloat(SiteCurrentStock) < parseFloat(textbox.value)) {
                    showAlert("Consumed Quantity should not be greater than Current Stock", "error", "#");
                    if (parseFloat(SiteCurrentStock) < 0) { textbox.value = "0.00"; }
                    else { textbox.value = parseFloat(SiteCurrentStock).toFixed(2); }

                }
                Grid1.Rows[rowIndex].Cells[dataField].Value = parseFloat(textbox.value).toFixed(2);
                PageMethods.WMUpdateQty(getOrderObject(rowIndex), null, null);
            }
        }

        function getOrderObject(rowIndex) {
            var order = new Object();
            order.Sequence = parseInt(Grid1.Rows[rowIndex].Cells['Sequence'].Value);
            order.ConsumedQty = parseFloat(Grid1.Rows[rowIndex].Cells['ConsumedQty'].Value).toFixed(2);
            return order;
        }

        function removePartFromList(sequence) {
            /*Remove Part from list*/
            document.getElementById("hdnSelectedProd_IDs").value = "";
            document.getElementById("hdnSelectedGRND_IDs").value = "";
            PageMethods.WMRemovePartFromList(sequence, removePartFromListOnSussess, null);
        }

        function removePartFromListOnSussess() {
            Grid1.refresh();
        }
        /*End*/

        /*FormOnLoad*/
        checkOnLoad();
        function checkOnLoad() {
            //jsFillSites();
            if (getParameterByName("invoker") == 'Request') { openReceiptSummary(); }
            if (getParameterByName("invoker") == 'Receipt') { openConsumptionSummary(); }
            ddlSites.setAttribute("onchange", "jsFillUsersList()");
        }

        function openReceiptSummary() {
            LoadingOn(true);
            document.getElementById("iframePORReceipt").src = "../PowerOnRent/GridReceiptSummary.aspx?FillBy=RequestID";
            divPopUpReceiptSummary.className = "divDetailExpandPopUpOn";
        }
        function openConsumptionSummary() {
            LoadingOn();
            document.getElementById("iframePORConsumption").src = "../PowerOnRent/GridConsumptionSummary.aspx?FillBy=ReceiptID";
            divPopUpConsumptionSummary.className = "divDetailExpandPopUpOn";
        }
        /*End FormOnLoad*/

    </script>
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

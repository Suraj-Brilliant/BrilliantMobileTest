<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/CRM.Master" AutoEventWireup="true" CodeBehind="WToWTransferDetail.aspx.cs" Inherits="BrilliantWMS.WMS.WToWTransferDetail"
    EnableEventValidation="false" Theme="Blue" %>

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
<%@ Register Src="../Product/UCProductSearch.ascx" TagName="UCProductSearch" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHolder_FormHeader" runat="server">
    <uc2:Toolbar ID="Toolbar1" runat="server" />
    <uc1:UCFormHeader ID="UCFormHeader1" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPHolder_Form" runat="server">
    <center>
        <span id="imgProcessing" style="display: none; background-color:white;">
            <img src="../App_Themes/Blue/img/ajax-loader.gif" style="top: 50%;" />
        </span>
        <div id="divLoading" style="height: 110%; width: 100%; display: none; top: 0; left: 0;"
            class="modal" runat="server" clientidmode="Static">
            <center>
            </center>
        </div>
        <div class="divHead" id="divHeadWTWTransferHead">
            <h4>
                <asp:Label ID="lblWTWTransfer" runat="server" Text="Warehouse To Warehouse Transfer"></asp:Label>
            </h4>
            <a onclick="javascript:divcollapsOpen('divWTWTransferDetail',this)" id="A2" runat="server">Collapse</a>
        </div>
        <div class="divDetailExpand" id="divWTWTransferDetail">
            <table class="tableForm" width="100%">
                <tr>
                    <td>
                        <asp:Label ID="lblWTWTransferDetailNumber" runat="server" Text="Transfer Number "></asp:Label>:
                    </td>
                    <td style="text-align: left; font-weight: bold;">
                        <asp:Label ID="lblWTWTransferNo" runat="server"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblWTWTransferDate" runat="server" Text="Date "></asp:Label>:
                    </td>
                    <td style="text-align: left;">
                        <uc4:UC_Date ID="UCWTWTransferDate" runat="server" />
                    </td>
                    <td>
                        <asp:Label ID="lblWTWTransferBy" runat="server" Text="Transfer By "></asp:Label>:
                    </td>
                    <td style="text-align: left; font-weight: bold;">
                        <asp:DropDownList ID="ddlWTWTransferBy" runat="server" Width="182px" DataTextField="Name" DataValueField="ID" AccessKey="1"></asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="WTWTransferStatus" runat="server" Text="Status "></asp:Label>:
                    </td>
                    <td style="text-align: left; font-weight: bold;">
                        <asp:DropDownList ID="ddlWTWStatus" runat="server" Width="182px" DataTextField="Status" DataValueField="ID" AccessKey="1"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblFromW" runat="server" Text="From Warehouse"></asp:Label>:
                    </td>
                    <td style="text-align: left; font-weight: bold;">
                        <asp:DropDownList ID="ddlFWarehouse" runat="server" Width="182px" DataTextField="WarehouseName" DataValueField="ID" AccessKey="1"
                            ClientIDMode="Static" onchange="GetToWareHouse();">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblToW" runat="server" Text="To Warehouse"></asp:Label>:
                    </td>
                    <td style="text-align: left; font-weight: bold;">
                        <asp:DropDownList ID="ddlTWarehouse" runat="server" Width="182px" ClientIDMode="Static" AccessKey="1"></asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblWTWRemark" runat="server" Text="Remark"></asp:Label>:
                    </td>
                    <td style="text-align: left; font-weight: bold;" colspan="3">
                        <asp:TextBox ID="WTWTRemark" runat="server" Width="90%"></asp:TextBox>
                    </td>
                </tr>
            </table>
            <table class="tableForm" width="100%">
                <tr>
                    <td colspan="6" style="font-style: italic; text-align: left; font-weight: bold;">
                        <hr style="width: 87%; margin-top: 8px; float: right;" />
                        <span>Transport Detail</span>
                    </td>
                </tr>
                <tr>
                    <td>Airway Bill :
                    </td>
                    <td style="text-align: left; font-weight: bold;">
                        <asp:TextBox runat="server" ID="txtAirwayBill" MaxLength="50" Width="180px"></asp:TextBox>
                    </td>
                    <td>Shipping Type :
                    </td>
                    <td style="text-align: left; font-weight: bold;">
                        <asp:TextBox runat="server" ID="txtShippingType" MaxLength="50" Width="180px"></asp:TextBox>
                    </td>
                    <td>Shipping Date :
                    </td>
                    <td style="text-align: left;">
                        <uc4:UC_Date ID="UC_ShippingDate" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>Exp. Delivery Date :
                    </td>
                    <td style="text-align: left; font-weight: bold;">
                        <uc4:UC_Date ID="UCExpDeliveryDate" runat="server" />
                    </td>
                    <td>Transporter Name :
                    </td>
                    <td style="text-align: left; font-weight: bold;">
                        <asp:TextBox runat="server" ID="txtTransporterName" MaxLength="50" Width="180px"></asp:TextBox>
                    </td>
                    <td></td>
                    <td style="text-align: left; font-weight: bold;"></td>
                </tr>
            </table>
            <table class="gridFrame" width="100%" id="Table1">
                <tr>
                    <td>
                        <table style="width: 100%">
                            <tr>
                                <td style="text-align: left;">
                                    <asp:Label ID="lblWTWTransferpartlist" CssClass="headerText" runat="server" Text="Warehouse To Warehouse Transfer Part List"></asp:Label>
                                </td>
                                <td style="text-align: right;">
                                    <uc1:UCProductSearch ID="UCProductSearchWTW" runat="server" />
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
                            Width="100%" PageSize="-1" OnRebind="Grid1_OnRebind" OnRowDataBound="Grid1_OnRowDataBound">
                            <ClientSideEvents ExposeSender="true" />
                            <Columns>
                                <obout:Column DataField="Prod_ID" Visible="false">
                                </obout:Column>
                                <obout:Column DataField="Sequence" HeaderText="Sr.No." AllowEdit="false" Width="7%"
                                    Align="center" HeaderAlign="center">
                                    <TemplateSettings TemplateId="ItemTempRemove" />
                                </obout:Column>
                                <obout:Column DataField="Prod_Code" HeaderText="SKU Code" AllowEdit="false" Width="13%"
                                    HeaderAlign="left">
                                </obout:Column>
                                <obout:Column DataField="Prod_Name" HeaderText="Product Name" AllowEdit="false" Width="13%"
                                    HeaderAlign="left">
                                </obout:Column>
                                <obout:Column DataField="Prod_Description" HeaderText="Description" AllowEdit="false"
                                    Width="0%" HeaderAlign="left" Visible="false">
                                </obout:Column>
                                <obout:Column DataField="CurrentStock" HeaderText="Current Stock" AllowEdit="false"
                                    Width="13%" HeaderAlign="center" Align="right">
                                    <TemplateSettings TemplateId="GridTemplateRightAlign" />
                                </obout:Column>
                                <obout:Column DataField="Qty" Width="14%" HeaderAlign="center" HeaderText="Quantity" Wrap="true"
                                    Align="center" AllowEdit="false">
                                    <TemplateSettings TemplateId="PlainEditTemplate" />
                                </obout:Column>
                                <obout:Column DataField="UOM" HeaderText="UOM" AllowEdit="false" Width="14%" HeaderAlign="center"
                                    Align="center">
                                    <TemplateSettings TemplateId="UOMEditTemplate" />
                                </obout:Column>
                                <obout:Column DataField="UOMID" AllowEdit="false" Width="0%" Visible="false">
                                </obout:Column>
                                <obout:Column DataField="OrderQuantity" HeaderText="Order Quantity" AllowEdit="false" Wrap="true"
                                    Width="14%" HeaderAlign="center" Align="center">
                                    <TemplateSettings TemplateId="GridTemplateTotal" />
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
                                                <td style="width: 35px; text-align: left;">
                                                    <%# Convert.ToInt32(Container.PageRecordIndex) + 1 %>
                                                </td>
                                            </tr>
                                        </table>
                                    </Template>
                                </obout:GridTemplate>
                                <obout:GridTemplate ID="GridTemplateTotal">
                                    <Template>
                                        <div class="divrowQtyTotal">
                                            <asp:Label ID="rowQtyTotal" runat="server">1</asp:Label>
                                        </div>
                                    </Template>
                                </obout:GridTemplate>
                                <obout:GridTemplate runat="server" ID="PlainEditTemplate">
                                    <Template>
                                        <div class="divTxtUserQty">
                                            <asp:TextBox ID="txtUsrQty" Width="94%" Style="text-align: right;" runat="server"
                                                Text="<%# Container.Value %>"  onkeydown="AllowDecimal(this,event);" 
                                                onkeypress="AllowDecimal(this,event);" onblur="markAsBlured(this, '<%# Grid1.Columns[Container.ColumnIndex].DataField %>', <%# Container.PageRecordIndex %>);"></asp:TextBox>
                                        </div> 
                                    </Template>
                                </obout:GridTemplate>
                                <obout:GridTemplate ID="GridTemplateRightAlign">
                                    <Template>
                                        <span style="text-align: right; width: 130px; margin-right: 10px;">
                                            <%# Container.Value  %></span>
                                    </Template>
                                </obout:GridTemplate>
                                <obout:GridTemplate ID="UOMEditTemplate" runat="server">
                                    <Template>
                                        <asp:DropDownList ID="ddlUOM" runat="server" Width="100px" Style="text-align: left;">
                                        </asp:DropDownList>
                                        <asp:HiddenField ID="hdnMyUOM" Value="" runat="server" />
                                    </Template>
                                </obout:GridTemplate>
                            </Templates>
                        </obout:Grid>
                    </td>
                </tr>
            </table>
        </div>
        <asp:HiddenField ID="hdnFrmWarehouse" ClientIDMode="Static" runat="server" />
        <asp:HiddenField ID="hdnToWarehouse" ClientIDMode="Static" runat="server" />
        <asp:HiddenField ID="hdnSelectedQty" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnSelectedUMO" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnEnteredQty" runat="server" ClientIDMode="Static" />
    </center>
     <script src="//code.jquery.com/jquery-1.10.2.js"></script>
    <script type="text/javascript">
        var ddlWTWTransferBy = document.getElementById("<%= ddlWTWTransferBy.ClientID %>");
        var ddlWTWStatus = document.getElementById("<%= ddlWTWStatus.ClientID %>");
        var ddlFWarehouse = document.getElementById("<%= ddlFWarehouse.ClientID %>");
        var ddlTWarehouse = document.getElementById("<%= ddlTWarehouse.ClientID %>");
        var WTWTRemark = document.getElementById("<%= WTWTRemark.ClientID %>");
        var txtAirwayBill = document.getElementById("<%= txtAirwayBill.ClientID %>");
        var txtShippingType = document.getElementById("<%= txtShippingType.ClientID %>");
        var txtTransporterName = document.getElementById("<%= txtTransporterName.ClientID %>");

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

        function GetToWareHouse() {
            var hdnFrmWarehouse = document.getElementById('hdnFrmWarehouse');
            hdnFrmWarehouse.value = ddlFWarehouse.value;
            var frmW = ddlFWarehouse.value;
            PageMethods.WMGetToWarehouse(frmW, OnSuccessFromWarehouse, null);
        }

        function OnSuccessFromWarehouse(result) {
            ddlTWarehouse.options.length = 0;
            var option0 = document.createElement("option");
            if (result.length > 0) {
                option0.text = '--Select--';
                option0.value = '0';
            }
            else {
                option0.text = 'N/A';
                option0.value = '0';
            }

            try {
                ddlTWarehouse.add(option0, null);
            }
            catch (Error) {
                ddlTWarehouse.add(option0);
            }

            for (var i = 0; i < result.length; i++) {
                var option1 = document.createElement("option");
                option1.text = result[i].WarehouseName;
                option1.value = result[i].ID;
                try {
                    ddlTWarehouse.add(option1, null);
                }
                catch (Error) {
                    ddlTWarehouse.add(option1);
                }
            }
        }

        function removePartFromList(sequence) {
            /*Remove Part from list*/
            var hdnProductSearchSelectedRec = document.getElementById('hdnProductSearchSelectedRec');
            hdnProductSearchSelectedRec.value = "";
            PageMethods.WMRemovePartFromRequest(sequence, removePartFromListOnSussess, null);
        }

        function removePartFromListOnSussess(result) {
            if (result == 0) {
                showAlert("Not Applicable.......", "error", "#");
            } else {
                Grid1.refresh();
            }
        }

        function jsSaveData() {
            var validate = validateForm('divWTWTransferDetail');
            if (validate == true) {
                if (Grid1.Rows.length == 0) {
                    showAlert("Add atleast one part into the Request Part List", "error", "#");
                }
                else {
                    var isContainsZero = 'no';
                    var matches = 0;
                    $(".divTxtUserQty input").each(function (i, val) {
                        if (($(this).val() == '0.00') || ($(this).val() == '0') || ($(this).val() == 0)) {
                            isContainsZero = 'yes';
                        }
                    });
                    var isContnZro = 'no';
                    var mtch = 0;
                    $(".divrowQtyTotal span").each(function (i, html) {
                        if (($(this).html() == '0') || ($(this).html() == '0.00') || ($(this).html() == 0) || ($(this).html() == 'NaN')) {
                            isContnZro = 'yes';
                        }
                    });

                    if (isContainsZero != 'yes' && isContnZro != 'yes') {
                        LoadingOn(true);

                        var UCWTWTransferDate = getDateFromUC("<%= UCWTWTransferDate.ClientID %>");
                        var UC_ShippingDate = getDateFromUC("<%= UC_ShippingDate.ClientID %>");
                        var UCExpDeliveryDate = getDateFromUC("<%= UCExpDeliveryDate.ClientID %>");
                        var obj1 = new Object();
                        obj1.TransferDate = UCWTWTransferDate;
                        obj1.FromPosition = parseInt(ddlFWarehouse.options[ddlFWarehouse.selectedIndex].value);
                        obj1.ToPosition = parseInt(ddlTWarehouse.options[ddlTWarehouse.selectedIndex].value);
                        obj1.TransferBy = parseInt(ddlWTWTransferBy.options[ddlWTWTransferBy.selectedIndex].value);
                        obj1.Status = parseInt(ddlWTWStatus.options[ddlWTWStatus.selectedIndex].value);
                        obj1.Remark = WTWTRemark.value.toString();

                        obj1.AirwayBill = txtAirwayBill.value.toString();
                        obj1.ShippingType = txtShippingType.value.toString();
                        obj1.TransporterName = txtTransporterName.value.toString();
                        obj1.ShippingDate = UC_ShippingDate;
                        obj1.ExpDeliveryDate = UCExpDeliveryDate;

                        PageMethods.WMSaveTransferHead(obj1, WMSaveTransferHead_onSuccessed, WMSaveTransferHead_onFailed);

                    } else {
                        showAlert("One or more Transfer quantity is zero", "error", "#");
                    }
                }
            } else {
                showAlert("Fill All Mandatory Fields...", "error", "#");
            }
        }

        function WMSaveTransferHead_onSuccessed(result) {
            if (result == 0 || result == 0) { showAlert("Transfer Failed....", "Error", '../WMS/Transfer.aspx'); }
            else if (result >= 1) {
                showAlert("Transfer saved successfully", "info", "../WMS/Transfer.aspx");
            }
        }

        function WMSaveTransferHead_onFailed() { showAlert("Error occurred", "Error", "../WMS/Transfer.aspx"); }

        function GetIndex(myDD, myHdnUMO, mySpnTotalQty, usrInputQty, crntStock, Index) {
            var getMyHdnField = document.getElementById(myHdnUMO);
            if (getMyHdnField != null) {
                var myUMOval = myDD.value;
                var myFilterVal = myUMOval.split(":");

                var myFilteredUMO = myFilterVal[0];
                var myFilteredUnit = myFilterVal[1];

                var hdnSelectedQty = document.getElementById('hdnSelectedQty');
                hdnSelectedQty.value = myFilteredUnit;
                var hdnSelectedUMO = document.getElementById('hdnSelectedUMO');
                hdnSelectedUMO.value = myFilteredUMO;
                var hdnEnteredQty = document.getElementById('hdnEnteredQty');
                var QtyEntered = hdnEnteredQty.value;

                var getUserInputQtyObj = document.getElementById(usrInputQty);
                var getTotalQtyObj = document.getElementById(mySpnTotalQty);

                var numTotalQty = Number(getUserInputQtyObj.value) * Number(myFilteredUnit);
                getTotalQtyObj.innerHTML = numTotalQty;

                getMyHdnField.value = myFilteredUMO;

                if (crntStock < numTotalQty) {
                    getUserInputQtyObj.value = "0";
                    getTotalQtyObj.innerHTML = "0";
                    showAlert("Transfer Quantity is greater than Current Stock...!!!", "Error", '#');
                } else {
                    var order = new Object();
                    order.Sequence = Index + 1;
                    order.Qty = numTotalQty;
                    order.UOMID = myFilteredUMO; // myHdnUMO;  //myFilteredUnit;//  myFilteredUMO;                        
                    PageMethods.WMUpdTransferPart(order, null, null);
                }
            }
        }

        function GetIndexQty(myDD, myHdnQty, myHdnUMO, mySpnTotalQty, usrInputQty, crntStock, Index) {
            var hdnSelectedQty = document.getElementById('hdnSelectedQty');
            var selUMOQty = hdnSelectedQty.value;
            var hdnSelectedUMO = document.getElementById('hdnSelectedUMO');
            var selUMOID = hdnSelectedUMO.value;
            var hdnEnteredQty = document.getElementById('hdnEnteredQty');
            var eq = hdnEnteredQty.value;
            var enterQty = myDD.value;
            hdnEnteredQty.value = myDD.value;

            var getMyHdnField = myHdnQty;  //myHdnUMO;
            var myUMOval = myHdnUMO.value;
            var myFilteredUnit = 0;

            myFilteredUnit = myHdnQty;

            var getUserInputQtyObj = document.getElementById(usrInputQty);
            var getTotalQtyObj = document.getElementById(mySpnTotalQty);

            if (hdnEnteredQty.value == "") {
                var numTotalQty = Number(getUserInputQtyObj.value) * Number(myHdnQty); //myHdnUMO       
                getTotalQtyObj.innerHTML = numTotalQty;
                getMyHdnField.value = myFilteredUnit;  // myFilteredUMO;
            } else {
                if (selUMOQty == "") {
                    var numTotalQty = hdnEnteredQty.value * Number(myHdnQty);
                    getTotalQtyObj.innerHTML = numTotalQty;
                }
                else {
                    var numTotalQty = hdnEnteredQty.value * selUMOQty;
                    // var numTotalQty = hdnEnteredQty.value * Number(myHdnQty);
                    getTotalQtyObj.innerHTML = numTotalQty;
                }
            }

            if (crntStock < numTotalQty) {
                getUserInputQtyObj.value = "0";
                getTotalQtyObj.innerHTML = "0";
                showAlert("Transfer Quantity is greater than Current Stock...!!!", "Error", '#');
            } else {
                var order = new Object();
                order.Sequence = Index + 1;
                order.Qty = numTotalQty;
                if (selUMOID == "") {
                    order.UOMID = myHdnUMO;
                } else {
                    order.UOMID = selUMOID; //myHdnUMO;  //myFilteredUnit;//  myFilteredUMO;
                }
                PageMethods.WMUpdTransferPart(order, null, null);
            }
        }
    </script>
    <style type="text/css">
        /*Grid css*/
        .excel-textbox {
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

        .excel-textbox-focused {
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

        .excel-textbox-error {
            color: #FF0000;
        }

        .ob_gCc2 {
            padding-left: 3px !important;
        }

        .ob_gBCont {
            border-bottom: 1px solid #C3C9CE;
        }

        .excel-checkbox {
            height: 20px;
            line-height: 20px;
        }
    </style>
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
        }

        .divDetailCollapse {
            display: none;
        }
        /*End POR Collapsable Div*/
    </style>
    <script type="text/javascript">
        /*Checkbox js for css*/
        var d = document;
        var safari = (navigator.userAgent.toLowerCase().indexOf('safari') != -1) ? true : false;
        var gebtn = function (parEl, child) { return parEl.getElementsByTagName(child); };
        onload = function () {

            var body = gebtn(d, 'body')[0];
            body.className = body.className && body.className != '' ? body.className + ' has-js' : 'has-js';

            if (!d.getElementById || !d.createTextNode) return;
            var ls = gebtn(d, 'label');
            for (var i = 0; i < ls.length; i++) {
                var l = ls[i];
                if (l.className.indexOf('label_') == -1) continue;
                var inp = gebtn(l, 'input')[0];
                if (l.className == 'label_check') {
                    l.className = (safari && inp.checked == true || inp.checked) ? 'label_check c_on' : 'label_check c_off';
                    l.onclick = check_it;
                };
                if (l.className == 'label_radio') {
                    l.className = (safari && inp.checked == true || inp.checked) ? 'label_radio r_on' : 'label_radio r_off';
                    l.onclick = turn_radio;
                };
            };
        };
        var check_it = function () {
            var inp = gebtn(this, 'input')[0];
            if (this.className == 'label_check c_off' || (!safari && inp.checked)) {
                this.className = 'label_check c_on';
                if (safari) inp.click();
            } else {
                this.className = 'label_check c_off';
                if (safari) inp.click();
            };
        };
        var turn_radio = function () {
            var inp = gebtn(this, 'input')[0];
            if (this.className == 'label_radio r_off' || inp.checked) {
                var ls = gebtn(this.parentNode, 'label');
                for (var i = 0; i < ls.length; i++) {
                    var l = ls[i];
                    if (l.className.indexOf('label_radio') == -1) continue;
                    l.className = 'label_radio r_off';
                };
                this.className = 'label_radio r_on';
                if (safari) inp.click();
            } else {
                this.className = 'label_radio r_off';
                if (safari) inp.click();
            };
        };
        /*End*/

    </script>
    <style type="text/css">
        .has-js .label_ceck, .has- radio {
            padding-left: 25px;
            padding-bottom: 10px;
        }

        .has-js .label_radio {
            background: url(../App_Themes/Blue/img/radio-off.png) no-repeat;
        }

        .has-js .label_check {
            background: url("../App_Themes/Blue/img/check-off.png") no-repeat;
        }

        .has-js label.c_on {
            background: url("../App_Themes/Blue/img/check-on.png") no-repeat;
        }

        .has-js label.r_on {
            background: url(../App_Themes/Blue/img/radio-on.png) no-repeat;
        }

        .has-js .label_check input, .has-js .label_radio input {
            position: absolute;
            left: -9999px;
        }
    </style>
</asp:Content>

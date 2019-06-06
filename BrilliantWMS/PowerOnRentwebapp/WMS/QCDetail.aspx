<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/CRM.Master" AutoEventWireup="true" CodeBehind="QCDetail.aspx.cs" Inherits="BrilliantWMS.WMS.QCDetail" EnableEventValidation="false" Theme="Blue" %>

<%@ Register Assembly="obout_Flyout2_NET" Namespace="OboutInc.Flyout2" TagPrefix="obout" %>
<%@ Register Assembly="obout_Interface" Namespace="Obout.Interface" TagPrefix="obout" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="obout_Grid_NET" Namespace="Obout.Grid" TagPrefix="obout" %>
<%@ Register Assembly="Obout.Ajax.UI" Namespace="Obout.Ajax.UI.HTMLEditor" TagPrefix="obout" %>
<%@ Register Assembly="obout_ComboBox" Namespace="Obout.ComboBox" TagPrefix="obout" %>
<%@ Register Src="../CommonControls/UC_Date.ascx" TagName="UC_Date" TagPrefix="uc1" %>
<%@ Register Src="../Product/UCProductSearch.ascx" TagName="UCProductSearch" TagPrefix="uc1" %>
<%@ Register Src="../CommonControls/Toolbar.ascx" TagName="Toolbar" TagPrefix="uc2" %>
<%@ Register Src="../CommonControls/UCFormHeader.ascx" TagName="UCFormHeader" TagPrefix="uc2" %>
<%@ Register Src="../Document/UC_AttachDocument.ascx" TagName="UC_AttachDocument" TagPrefix="uc3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHolder_FormHeader" runat="server">
    <uc2:Toolbar ID="Toolbar1" runat="server" />
    <uc2:UCFormHeader ID="UCFormHeader1" runat="server" />
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
        <div class="divHead" id="divHeadQCHead">
            <h4>
                <asp:Label ID="lblQualityCheck" runat="server" Text="Quality Check"></asp:Label>
            </h4>
            <a onclick="javascript:divcollapsOpen('divQCDetail',this)" id="A1" runat="server">Collapse</a>
        </div>
        <div class="divDetailExpand" id="divQCDetail" runat="server" clientidmode="Static">
            <div id="dvQCDetail" runat="server" clientidmode="Static">
                <table class="tableForm" width="100%">
                    <tr>
                        <td>
                            <asp:Label ID="lblQCNo" runat="server" Text="QC Number "></asp:Label>:
                        </td>
                        <td style="text-align: left; font-weight: bold;">
                            <asp:Label ID="lblQCNumber" runat="server"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblQCDate" runat="server" Text="QC Date "></asp:Label>:
                        </td>
                        <td style="text-align: left;">
                            <uc1:UC_Date ID="UCQCDate" runat="server" />
                        </td>
                        <td>
                            <asp:Label ID="lblQCBy" runat="server" Text="QC By "></asp:Label>:
                        </td>
                        <td style="text-align: left; font-weight: bold;">
                            <asp:DropDownList ID="ddlQCBy" runat="server" Width="182px" DataTextField="userName" DataValueField="userID" AccessKey="1"></asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lblStatus" runat="server" Text="Status "></asp:Label>:
                        </td>
                        <td style="text-align: left; font-weight: bold;">
                            <asp:DropDownList ID="ddlStatus" runat="server" Width="182px" DataTextField="Status" DataValueField="ID" AccessKey="1"></asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblRemark" runat="server" Text="Remark "></asp:Label>:
                        </td>
                        <td colspan="7" style="text-align: left; font-weight: bold;">
                            <asp:TextBox ID="txtRemark" runat="server" Width="99%" TextMode="MultiLine"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                </div>
                <table class="gridFrame" width="100%" id="tblCart">
                    <tr>
                        <td>
                            <table style="width: 100%">
                                <tr>
                                    <td style="text-align: left;">
                                        <asp:Label ID="lblqcpartlist" CssClass="headerText" runat="server" Text="Quality Check Part List"></asp:Label>
                                    </td>
                                    <td style="text-align: right;">
                                        <input type="button" id="btnCreditNote" value="Credit Note" onclick="OpenCreditNote();" />
                                        <input type="button" id="btnDebitNote" value="Debit Note" onclick="OpenDebitNote();" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <obout:Grid ID="Grid1" runat="server" CallbackMode="true" Serialize="true" AllowColumnReordering="true"
                                AllowColumnResizing="true" AutoGenerateColumns="false" AllowPaging="false" ShowLoadingMessage="true"
                                AllowSorting="false" AllowManualPaging="false" AllowRecordSelection="false" ShowFooter="false" OnRebind="Grid1_OnRebind" OnRowDataBound="Grid1_OnRowDataBound"
                                Width="100%" PageSize="-1">
                                <ClientSideEvents ExposeSender="true" />
                                <Columns>
                                    <obout:Column DataField="Prod_ID" Visible="false"></obout:Column>
                                    <obout:Column DataField="Sequence" HeaderText="Sr.No." AllowEdit="false" Width="7%"
                                        Align="center" HeaderAlign="center">
                                        <TemplateSettings TemplateId="ItemTempRemove" />
                                    </obout:Column>
                                    <obout:Column DataField="Prod_Code" HeaderText="SKU Code" AllowEdit="false" Width="15%"
                                        HeaderAlign="left">
                                    </obout:Column>
                                    <obout:Column DataField="Prod_Name" HeaderText="Product Name" AllowEdit="false" Width="15%"
                                        HeaderAlign="left">
                                    </obout:Column>
                                    <obout:Column DataField="Prod_Description" HeaderText="Description" AllowEdit="false"
                                        Width="15%" HeaderAlign="left">
                                    </obout:Column>
                                    <obout:Column DataField="UOM" HeaderText="UOM" AllowEdit="false" Width="14%" HeaderAlign="center"
                                        Align="center">
                                    </obout:Column>
                                    <obout:Column DataField="POQty" HeaderText="Order Qty" AllowEdit="false"
                                        Width="13%" HeaderAlign="center" Align="center">
                                    </obout:Column>
                                    <obout:Column DataField="GRNQty" HeaderText="Received Qty" AllowEdit="false"
                                        Width="13%" HeaderAlign="center" Align="center">
                                    </obout:Column>
                                    <obout:Column DataField="QCQty" Width="14%" HeaderAlign="center" HeaderText="QC Qty"
                                        Align="center" AllowEdit="false">
                                        <TemplateSettings TemplateId="TemplateQCQty" />
                                    </obout:Column>
                                    <obout:Column DataField="SelectedQty" HeaderText="Selected Qty" AllowEdit="false"
                                        Width="13%" HeaderAlign="center" Align="center">
                                        <TemplateSettings TemplateId="PlainEditTemplate" />
                                    </obout:Column>
                                    <obout:Column DataField="RejectedQty" HeaderText="Rejected Qty" AllowEdit="false" Wrap="true"
                                        Width="13%" HeaderAlign="center" Align="center">
                                        <TemplateSettings TemplateId="TemplateRejectedQty" />
                                    </obout:Column>
                                    <obout:Column DataField="Reason" HeaderText="Reason" AllowEdit="false" Wrap="true" Width="13%" HeaderAlign="center" Align="center">
                                        <TemplateSettings TemplateId="TemplateReason" />
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
                                    <obout:GridTemplate runat="server" ID="PlainEditTemplate">
                                        <Template>
                                            <div class="divTxtUserQty">
                                                <asp:TextBox ID="txtUsrQty" Width="94%" Style="text-align: right;" runat="server"
                                                    Text="<%# Container.Value %>" onfocus="markAsFocused(this)" onkeydown="AllowDecimal(this,event);"
                                                    onkeypress="AllowDecimal(this,event);" onblur="markAsBlured(this, '<%# Grid1.Columns[Container.ColumnIndex].DataField %>', <%# Container.PageRecordIndex %>);"></asp:TextBox>
                                            </div>
                                        </Template>
                                    </obout:GridTemplate>
                                    <obout:GridTemplate runat="server" ID="TemplateQCQty">
                                        <Template>
                                            <div class="divRejectedQty">
                                                <asp:Label ID="SelQCQty" runat="server" Text="<%# Container.Value %>">0</asp:Label>
                                            </div>
                                        </Template>
                                    </obout:GridTemplate>
                                    <obout:GridTemplate runat="server" ID="TemplateRejectedQty">
                                        <Template>
                                            <div class="divRejectedQty">
                                                <asp:Label ID="SelRejectedQty" runat="server" Text="<%# Container.Value %>">0</asp:Label>
                                            </div>
                                        </Template>
                                    </obout:GridTemplate>
                                    <obout:GridTemplate runat="server" ID="TemplateReason">
                                        <Template>
                                            <div class="divtxtUsrReason">
                                                <asp:TextBox ID="txtUsrReason" Width="94%" runat="server" Style="text-align: right;"
                                                    Text="<%# Container.Value %>" onfocus="markAsFocused(this)" 
                                                     onblur="markAsBlured(this, '<%# Grid1.Columns[Container.ColumnIndex].DataField %>', <%# Container.PageRecordIndex %>);"></asp:TextBox>
                                            </div>
                                        </Template>
                                    </obout:GridTemplate>
                                </Templates>
                            </obout:Grid>
                        </td>
                    </tr>
                </table>
            
        </div>
        <asp:HiddenField ID="hdnProductSearchSelectedRec" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnQCStatus" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnCurrentSession" runat="server" ClientIDMode="Static" />
    </center>
    <script src="//code.jquery.com/jquery-1.10.2.js"></script>
    <script type="text/javascript">
        var ddlQCBy = document.getElementById("<%= ddlQCBy.ClientID %>");
        var ddlStatus = document.getElementById("<%= ddlStatus.ClientID %>");
        var txtRemark = document.getElementById("<%= txtRemark.ClientID %>");

        function jsSaveData() {
            var validate = validateForm('divQCDetail');
            var isContainsZero = 'no';
            var matches = 0;
            $(".divTxtUserQty input").each(function (i, val) {
                if (($(this).val() == '0.00') || ($(this).val() == '0') || ($(this).val() == 0)) {
                    isContainsZero = 'yes';
                }
            });

            if (isContainsZero != 'yes') {
                LoadingOn(true);

                var QCDate = getDateFromUC("<%= UCQCDate.ClientID %>");
                var obj1 = new Object();
                obj1.QCDate = QCDate;
                obj1.QCby = parseInt(ddlQCBy.options[ddlQCBy.selectedIndex].value);
                obj1.Status = parseInt(ddlStatus.options[ddlStatus.selectedIndex].value);
                obj1.Remark = txtRemark.value.toString();

                PageMethods.WMSaveQCHead(obj1, WMSaveQCHead_onSuccessed, WMSaveQCHead_onFailed);
            } else {
                showAlert("One or more Selected Quantity is zero", "error", "#");
            }
        }

        function WMSaveQCHead_onSuccessed(result) {
            if (result >= 1) {
                var sessiongrn = '<%= Session["GRNID"] %>';
                var sessionpkup = '<%= Session["PKUPID"] %>';
                var sessiontr = '<%= Session["TRID"] %>';
                if (sessiongrn != '') {
                    showAlert("Quality Check saved successfully", "info", "../WMS/Inbound.aspx");
                } else if (sessionpkup != '') {
                    showAlert("Quality Check saved successfully", "info", "../WMS/Outbound.aspx");
                } else if (sessiontr != '') {
                    showAlert("Quality Check saved successfully", "info", "../WMS/Transfer.aspx");
                }
            } else {
                if (sessiongrn != '') {
                    showAlert("Some Error Occured!", "error", "../WMS/Inbound.aspx");
                } else if (sessionpkup != '') {
                    showAlert("Some Error Occured!", "error", "../WMS/Outbound.aspx");
                } else if (sessiontr != '') {
                    showAlert("Some Error Occured!", "error", "../WMS/Transfer.aspx");
                }
            }
        }
        function WMSaveQCHead_onFailed() { showAlert("Error occurred", "Error", "../WMS/Inbound.aspx"); }

        function GetIndexQty(myDD, usrInputQty, Index, qcQty, rejectedQty, txtUserReason) {
            // poQty, shortQty, excessQty
            var enterQty = myDD.value;
            var ShowrejectedQty = document.getElementById(rejectedQty);
            //var ShowExcessQty = document.getElementById(excessQty);
            var Calculation = Number(qcQty) - Number(enterQty);
            if (Calculation == 0) {
                ShowrejectedQty.innerHTML = 0.00;
            } else if (Calculation > 0) {
                ShowrejectedQty.innerHTML = Calculation;
            } else if (Calculation < 0) {
                ShowrejectedQty.innerHTML = 0.00;
            }
            var order = new Object();
            order.Sequence = Index + 1;
            order.SelectedQty = enterQty;
            order.RejectedQty = ShowrejectedQty.innerHTML;
            order.Reason = txtUserReason.value;
            PageMethods.WMUpdQCPart(order, null, null);
        }

        function GetQCReason(reason, Index, qcQty, rejectedQty, usrInputQty) {
           
            var enteredReason = reason.value;
            
            var order = new Object();
            order.Sequence = Index + 1;           
            order.Reason = enteredReason;
            PageMethods.WMUpdQCPartReason(order, null, null);
        }

        function removePartFromList(sequence) {
            /*Remove Part from list*/
            var hdnProductSearchSelectedRec = document.getElementById('hdnProductSearchSelectedRec');
            hdnProductSearchSelectedRec.value = "1";
            PageMethods.WMRemovePartFromRequest(sequence, removePartFromListOnSussess, null);
        }

        function removePartFromListOnSussess(result) {
            if (result == 0) {
                showAlert("Not Applicable.......", "error", "#");
            } else {
                Grid1.refresh();
            }
        }

        function OpenCreditNote() {
            var hdnCurrentSession = document.getElementById('hdnCurrentSession');
            PageMethods.WMCheckQCStatusCN(hdnCurrentSession.value,CreditNoteOnSussess, null);
        }
        function CreditNoteOnSussess(result) {
            if (result == 1) {
                window.open('../WMS/CreditNote.aspx', null, 'height=380px, width=1010px,status= 0, resizable= 1, scrollbars=1, toolbar=0,location=center,menubar=0, screenX=300; screenY=50')
            } else {
                showAlert("Not Applicable.......", "error", "#");
            }
        }

        function OpenDebitNote() {
            var hdnCurrentSession = document.getElementById('hdnCurrentSession');
            var confirm_value = document.createElement("INPUT");
            confirm_value.type = "hidden";
            confirm_value.name = "confirm_value";
            if (confirm("This action will close the Purchase Order.Do you want to continue?")) {
                confirm_value.value = "Yes";
            } else {
                confirm_value.value = "No";
            }
           // document.forms[0].appendChild(confirm_value);
            PageMethods.WMCheckQCStatusDN(hdnCurrentSession.value, confirm_value.value,DebitNoteOnSussess, null);
        }

        function DebitNoteOnSussess(result) {
            if (result == 1) {
                window.open('../WMS/DebitNote.aspx', null, 'height=380px, width=1010px,status= 0, resizable= 1, scrollbars=1, toolbar=0,location=center,menubar=0, screenX=300; screenY=50')
            } else {
                showAlert("Not Applicable.......", "error", "#");
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
        .has-js .label_check, .has-js .label_radio {
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

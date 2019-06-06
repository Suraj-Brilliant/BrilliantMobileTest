<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/CRM.Master" AutoEventWireup="true" CodeBehind="DispatchDetails.aspx.cs" Inherits="BrilliantWMS.WMS.DispatchDetails" 
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
        <div class="divHead" id="divHeadDispatchHead">
            <h4>
                <asp:Label ID="lblDispatchDetail" runat="server" Text="Dispatch"></asp:Label>
            </h4>
            <a onclick="javascript:divcollapsOpen('divDispatchDetail',this)" id="A1" runat="server">Collapse</a>
        </div>
        <div class="divDetailExpand" id="divDispatchDetail" runat="server" clientidmode="Static">
            <div id="dvDDetail" runat="server" clientidmode="Static">
                <table class="tableForm" width="100%">
                    <tr>
                        <td>
                            <asp:Label ID="lblDispatchNumber" runat="server" Text="Dispatch Number "></asp:Label>:
                        </td>
                        <td style="text-align: left; font-weight: bold;">
                            <asp:Label ID="lblDispatchNo" runat="server"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblDispatchDate" runat="server" Text="Dispatch Date "></asp:Label>:
                        </td>
                        <td style="text-align: left;">
                            <uc4:UC_Date ID="UCDispatchDate" runat="server" />
                        </td>
                        <td>
                            <asp:Label ID="lblDispatchBy" runat="server" Text="Dispatch By "></asp:Label>:
                        </td>
                        <td style="text-align: left; font-weight: bold;">
                            <asp:DropDownList ID="ddlDispatchBy" runat="server" Width="182px" DataTextField="userName" DataValueField="userID" AccessKey="1"></asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lblStatus" runat="server" Text="Status "></asp:Label>:
                        </td>
                        <td style="text-align: left; font-weight: bold;">
                            <asp:DropDownList ID="ddlStatus" runat="server" Width="182px" DataTextField="Status" DataValueField="ID" AccessKey="1"></asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <%--<td>
                            <asp:Label ID="lblBatchNo" runat="server" Text="Batch No. "></asp:Label>:
                        </td>
                        <td style="text-align: left; font-weight: bold;">
                            <asp:TextBox ID="txtBatchNo" runat="server" AccessKey="1"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Label ID="lblShippingNo" runat="server" Text="Shipping No. "></asp:Label>:
                        </td>
                        <td style="text-align: left; font-weight: bold;">
                            <asp:TextBox ID="txtShippingNo" runat="server" AccessKey="1"></asp:TextBox>
                        </td>--%>
                        <td>
                            <asp:Label ID="lblRemark" runat="server" Text="Remark "></asp:Label>:
                        </td>
                        <td colspan="3" style="text-align: left; font-weight: bold;">
                            <asp:TextBox ID="txtRemark" runat="server" Width="99%"></asp:TextBox>
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
                        <td>Transporter Remark :
                        </td>
                        <td style="text-align: left; font-weight: bold;">
                            <asp:TextBox runat="server" ID="txtTransporterRemark" MaxLength="100" Width="180px"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <table class="gridFrame" width="100%" id="tblCart">
                    <tr>
                        <td>
                            <table style="width: 100%">
                                <tr>
                                    <td style="text-align: left;">
                                        <asp:Label ID="lblDispatchpartlist" CssClass="headerText" runat="server" Text="Dispatch Part List"></asp:Label>
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
                                    <obout:Column DataField="QCQty" HeaderText="QC Qty" AllowEdit="false"
                                        Width="13%" HeaderAlign="center" Align="center">
                                    </obout:Column>
                                    <obout:Column DataField="DispatchQty" Width="14%" HeaderAlign="center" HeaderText="Dispatch Qty"
                                        Align="center" AllowEdit="false">
                                       <%-- <TemplateSettings TemplateId="PlainEditTemplate" />--%>
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
                                </Templates>
                            </obout:Grid>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </center>
     <script src="//code.jquery.com/jquery-1.10.2.js"></script>
    <script type="text/javascript">
        var ddlDispatchBy = document.getElementById("<%= ddlDispatchBy.ClientID %>");
        var ddlStatus = document.getElementById("<%= ddlStatus.ClientID %>");
        var txtRemark = document.getElementById("<%= txtRemark.ClientID %>");
        var txtAirwayBill = document.getElementById("<%= txtAirwayBill.ClientID %>");
        var txtShippingType = document.getElementById("<%= txtShippingType.ClientID %>");
        var txtTransporterName = document.getElementById("<%= txtTransporterName.ClientID %>");
        var txtTransporterRemark = document.getElementById("<%= txtTransporterRemark.ClientID %>");

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
        function jsSaveData() {
            var validate = validateForm('divDispatchDetail');

            LoadingOn(true);

            var DispatchDate = getDateFromUC("<%= UCDispatchDate.ClientID %>");
            var ShippingDate = getDateFromUC("<%= UC_ShippingDate.ClientID %>"); 
            var ExpDeliveryDate = getDateFromUC("<%= UCExpDeliveryDate.ClientID %>"); 
            var obj1 = new Object();
            obj1.DispatchDate = DispatchDate;
            obj1.Status = parseInt(ddlStatus.options[ddlStatus.selectedIndex].value);
            obj1.Remark = txtRemark.value.toString();
            obj1.DispatchBy = parseInt(ddlDispatchBy.options[ddlDispatchBy.selectedIndex].value);
            obj1.AirwayBill = txtAirwayBill.value.toString();
            obj1.ShippingType = txtShippingType.value.toString();
            obj1.ShippingDate = ShippingDate;
            obj1.ExpDeliveryDate = ExpDeliveryDate;
            obj1.TransporterName = txtTransporterName.value.toString();
            obj1.TransporterRemark = txtTransporterRemark.value.toString();

            PageMethods.WMSaveDispatchHead(obj1, WMSaveDispatchHead_onSuccessed, WMSaveDispatchHead_onFailed);
        }

        function WMSaveDispatchHead_onSuccessed(result) {
            if (result >= 1) {
                var sessionqc = '<%= Session["QCID"] %>';
                var sessiondisp = '<%= Session["DispID"] %>';
                var sessiontr = '<%= Session["TRID"] %>';
                if (sessionqc != '') {
                    showAlert("Dispatch saved successfully", "info", "../WMS/Outbound.aspx");
                } else if(sessiondisp != ''){
                    showAlert("Dispatch saved successfully", "info", "../WMS/Dispatch.aspx");
                } else if (sessiontr != '') {
                    showAlert("Dispatch saved successfully", "info", "../WMS/Transfer.aspx");
                }
            } else {
                if (sessionqc != '') {
                    showAlert("Some Error Occured!", "error", "../WMS/Outbound.aspx");
                } else if (sessiondisp != '') {
                    showAlert("Some Error Occured!", "error", "../WMS/Dispatch.aspx");
                } else if (sessiontr != '') {
                    showAlert("Some Error Occured!", "error", "../WMS/Transfer.aspx");
                }
            }
        }
        function WMSaveDispatchHead_onFailed() { showAlert("Error occurred", "Error", "../WMS/Outbound.aspx"); }

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

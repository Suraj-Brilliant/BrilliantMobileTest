<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/CRM.Master" AutoEventWireup="true" CodeBehind="GridGRN.aspx.cs"
    Inherits="BrilliantWMS.WMS.GridGRN" EnableEventValidation="false" Theme="Blue" %>

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
    <asp:UpdatePanel ID="UPTBGRNList" runat="server">
        <ContentTemplate>
            <asp:TabContainer ID="TCGRN" runat="server">
                <asp:TabPanel ID="TabGRNList" runat="server" HeaderText="GRN List">
                    <ContentTemplate>
                        <asp:UpdateProgress ID="UPGRNGridProgress" runat="server" AssociatedUpdatePanelID="UpPnlGridGRN">
                            <ProgressTemplate>
                                <center>
                                    <div class="modal">
                                        <img src="../App_Themes/Blue/img/ajax-loader.gif" style="top: 50%;" />
                                    </div>
                                </center>
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                        <asp:UpdatePanel ID="UpPnlGridGRN" runat="server">
                            <ContentTemplate>
                                <%-- <div class="divHead">--%>
                                <h4 id="h4DivHead" runat="server"></h4>
                                <table style="float: right; font-size: 15px; font-weight: bold; color: Black;">
                                    <%--margin-top: -25px;--%>
                                    <tr>
                                        <td style="padding-right: 10px; padding-bottom: 0px; padding-top: 0px;">
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
                                    </tr>
                                </table>
                                <%--</div>--%>
                                <div id="divlinkGRNList">
                                    <%--class="divDetailExpand"--%>
                                    <center>
                                        <table class="gridFrame" width="100%">
                                            <tr>
                                                <td>
                                                    <table style="width: 100%">
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblgrnlist" Style="color: white; font-size: 15px; font-weight: bold;" runat="server" Text="Goods Receipt List"></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <obout:Grid ID="grdGRN" runat="server" AutoGenerateColumns="false" AllowAddingRecords="false" AllowFiltering="true" AllowGrouping="true" AllowMultiRecordSelection="false"
                                                        AllowRecordSelection="true" Width="100%" AllowColumnResizing="true" AllowColumnReordering="true">
                                                        <Columns>
                                                            <obout:Column DataField="ID" HeaderText="GRN No" HeaderAlign="center" Align="center" Width="5%"></obout:Column>
                                                            <obout:Column DataField="POOrderNo" HeaderText="PO No" HeaderAlign="center" Align="center" Width="7%"></obout:Column>
                                                            <obout:Column DataField="JobCardName" HeaderText="Job Card Name" HeaderAlign="left" Align="left" Width="10%" Wrap="true"></obout:Column>
                                                            <obout:Column DataField="GRNDate" HeaderText="GRN Date" HeaderAlign="left" Align="left" DataFormatString="{0:dd-MM-yyyy}" Width="10%"></obout:Column>
                                                            <obout:Column DataField="VendorName" HeaderText="Vendor Name" HeaderAlign="left" Align="left" Width="10%" Wrap="true"></obout:Column>
                                                            <obout:Column DataField="GRNBy" HeaderText="GRN By" HeaderAlign="left" Align="left" Width="10%"></obout:Column>
                                                            <obout:Column DataField="StatusName" HeaderText="Status" HeaderAlign="left" Align="left" Width="8%" Wrap="true"></obout:Column>
                                                            <obout:Column DataField="ImgGRN" HeaderText="GRN" HeaderAlign="center" Align="center" Width="7%">
                                                                <TemplateSettings TemplateId="GRNStatusGRN" />
                                                            </obout:Column>
                                                            <obout:Column DataField="ImgQC" HeaderText="Quality Check" HeaderAlign="center" Align="center" Width="7%">
                                                                <TemplateSettings TemplateId="GRNStatusQC" />
                                                            </obout:Column>
                                                            <obout:Column DataField="ImgLP" HeaderText="Label Printing" HeaderAlign="center" Align="center" Width="7%">
                                                                <TemplateSettings TemplateId="GRNStatusLP" />
                                                            </obout:Column>
                                                            <obout:Column DataField="ImgPutIn" HeaderText="Put In" HeaderAlign="center" Align="center" Width="7%">
                                                                <TemplateSettings TemplateId="GRNStatusPI" />
                                                            </obout:Column>
                                                        </Columns>
                                                        <Templates>
                                                            <obout:GridTemplate ID="GRNStatusGRN" runat="server">
                                                                <Template>
                                                                    <center>
                                                                        <div class='<%# ("POR" + Container.Value) %>' onclick="RequestOpenEntryForm('GRN','<%# Container.Value %>', '<%# Container.DataItem["ID"] %>')"></div>
                                                                    </center>
                                                                </Template>
                                                            </obout:GridTemplate>
                                                            <obout:GridTemplate ID="GRNStatusQC" runat="server">
                                                                <Template>
                                                                    <center>
                                                                        <div class='<%# ("POR" + Container.Value) %>' onclick="RequestOpenEntryForm('QC','<%# Container.Value %>', '<%# Container.DataItem["ID"] %>')"></div>
                                                                    </center>
                                                                </Template>
                                                            </obout:GridTemplate>
                                                            <obout:GridTemplate ID="GRNStatusLP" runat="server">
                                                                <Template>
                                                                    <center>
                                                                        <div class='<%# ("POR" + Container.Value) %>' onclick="RequestOpenEntryForm('LabelPrinting','<%# Container.Value %>', '<%# Container.DataItem["ID"] %>')"></div>
                                                                    </center>
                                                                </Template>
                                                            </obout:GridTemplate>
                                                            <obout:GridTemplate ID="GRNStatusPI" runat="server">
                                                                <Template>
                                                                    <center>
                                                                        <div class='<%# ("POR" + Container.Value) %>' onclick="RequestOpenEntryForm('PutIn','<%# Container.Value %>', '<%# Container.DataItem["ID"] %>')"></div>
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
                    </ContentTemplate>
                </asp:TabPanel>
               
            </asp:TabContainer>
            <asp:HiddenField ID="hdnSelectedRec" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hdnSelectedOneRec" runat="server" ClientIDMode="Static" />
        </ContentTemplate>         
    </asp:UpdatePanel>
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
            if (grdGRN.PageSelectedRecords.length > 0) {
                for (var i = 0; i < grdGRN.PageSelectedRecords.length; i++) {
                    var record = grdGRN.PageSelectedRecords[i];
                    if (hdnSelectedRec.value != "") hdnSelectedRec.value += ',' + record.ID;
                    if (hdnSelectedRec.value == "") hdnSelectedRec.value = record.ID;
                }
            }
        }

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
        function CreateTask() {
            var hdnSelectedRec = document.getElementById('hdnSelectedRec');
            if (hdnSelectedRec.value == "") {
                showAlert('Please Select Atleast One Record For Assign Task!!!', 'Error', '#');
            } else {
                PageMethods.WMCheckStatus(hdnSelectedRec.value, OnSuccessCheckStatus, null);
            }
            
        }
        function OnSuccessCheckStatus(result) {
            if (result == 1) {
                window.open('../WMS/AssignTask.aspx', null, 'height=350, width=500,status= 0, resizable= 1, scrollbars=0, toolbar=0,location=center,menubar=0, screenX=300; screenY=50');
            } else if (result == 0) {
                showAlert('One Or More Record Have Already Job Card Number Assigned. Please Select Not Assigned Order!!!', 'Error', '#');
            } else {
                showAlert('Please Select Record With Same Status!!!', 'Error', '#');
            }
        }

        function RequestOpenEntryForm(invoker, state, GRNID) {
            var hdnSelectedOneRec = document.getElementById('hdnSelectedOneRec');
            hdnSelectedOneRec.value = GRNID;
            if (state != "gray") {
                if (state == "red") { state = "Edit"; }
                else if (state != "red") { state = "View"; }
                PageMethods.WMSetSessionGRN(invoker, GRNID, state, RequestOpenEntryFormOnSuccess, null);
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
                    window.open('../WMS/QCDetail.aspx', '_self', '');
                    break;
                case "LabelPrinting":
                    var hdnSelectedOneRec = document.getElementById('hdnSelectedOneRec');
                    window.open('../WMS/LabelPrinting.aspx?ID=' + hdnSelectedOneRec.value + '', '_self', '');
                    break;
                case "PutIn":
                    var hdnSelectedOneRec = document.getElementById('hdnSelectedOneRec');
                    window.open('../WMS/LabelPrinting.aspx?ID=' + hdnSelectedOneRec.value + '', '_self', '');
                    break;                
                case "AccessDenied":
                    showAlert("Access Denied", '', '#');
                    break;
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

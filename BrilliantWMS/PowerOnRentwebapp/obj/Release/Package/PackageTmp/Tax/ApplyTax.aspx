<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/CRM2.Master" AutoEventWireup="true" 
    CodeBehind="ApplyTax.aspx.cs" Inherits="BrilliantWMS.Tax.ApplyTax" %>

<%@ Register Assembly="obout_Grid_NET" Namespace="Obout.Grid" TagPrefix="obout" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <table class="gridFrame" style="margin: 5px 5px 5px 5px; width: 650px">
        <tr>
            <td>
                <table width="100%">
                    <tr>
                        <td>
                            <a class="headerText">Tax List</a>
                        </td>
                        <td>
                            <div style="float: right;">
                                <input type="button" value="Submit" id="btnSubmit" onclick="UCApplyTaxSubmit();" />
                            </div>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <obout:Grid ID="GridTaxList" runat="server" AutoGenerateColumns="false" AllowGrouping="false"
                    AllowColumnResizing="false" AllowAddingRecords="false" AllowColumnReordering="true"
                    AllowMultiRecordSelection="true" AllowPageSizeSelection="false" AllowPaging="false"
                    AllowRecordSelection="false" AllowSorting="false" FilterType="Normal" AllowFiltering="false"
                    ShowLoadingMessage="true" Serialize="false" CallbackMode="true" OnRebind="RebindGrid"
                    Width="100%" ShowFooter="false" OnUpdateCommand="UpdateRecord" PageSize="-1"
                    ShowColumnsFooter="true">
                    <ClientSideEvents ExposeSender="true" OnClientCallback="setTaxAmount" />
                    <Columns>
                        <obout:Column DataField="ParentTaxID" Visible="false">
                        </obout:Column>
                        <obout:Column DataField="TaxID" Visible="false">
                        </obout:Column>
                        <obout:Column DataField="IsChecked" Visible="false">
                        </obout:Column>
                        <obout:Column DataField="CheckBoxVisible" Width="10%" Align="center" HeaderAlign="center">
                            <TemplateSettings TemplateId="ItemTempCheckBox" HeaderTemplateId="HeaderTempCheckBox" />
                        </obout:Column>
                        <obout:Column DataField="TaxName" HeaderText="Tax Name" Width="48%" HeaderAlign="left"
                            Align="left">
                        </obout:Column>
                        <obout:Column DataField="TaxPercent" HeaderText="Tax Rate" Width="20%" HeaderAlign="right"
                            Align="right">
                        </obout:Column>
                        <obout:Column DataField="TaxAmount" HeaderText="Tax Amount" Width="22%" HeaderAlign="right"
                            Align="right">
                            <TemplateSettings TemplateId="TemplateAmountTax" />
                        </obout:Column>
                    </Columns>
                    <Templates>
                        <obout:GridTemplate ID="HeaderTempCheckBox" runat="server">
                            <Template>
                                Apply
                                <br />
                                Tax
                            </Template>
                        </obout:GridTemplate>
                        <obout:GridTemplate ID="ItemTempCheckBox" runat="server">
                            <Template>
                                <asp:CheckBox runat="server" ID="chkboxID" Visible='<%# Convert.ToBoolean(Container.Value) %>'
                                    Checked='<%# Convert.ToBoolean(Container.DataItem["IsChecked"]) %>' onclick="chkboxIDClick('chk');"
                                    ToolTip='<%# Container.DataItem["TaxID"].ToString() %>' Style="cursor: hand;" />
                            </Template>
                        </obout:GridTemplate>
                        <obout:GridTemplate runat="server" ID="TemplateAmountTax">
                            <Template>
                                <span style="margin-right: 4px;">
                                    <%# Container.Value %>
                                </span>
                            </Template>
                        </obout:GridTemplate>
                    </Templates>
                </obout:Grid>
            </td>
        </tr>
        <tr>
            <td>
                <table width="100%">
                    <tr>
                        <td style="text-align: left; width: 50%">
                            <span class="headerText" id="Span1">Taxable Amount :</span>&nbsp;
                            <asp:Label class="headerText" ID="lblTaxableAmount" runat="server" ClientIDMode="Static"></asp:Label>
                        </td>
                        <td style="text-align: right; width: 50%">
                            <span class="headerText" id="Span2">Total Tax Amount :</span>&nbsp;<asp:Label class="headerText"
                                ID="lblTaxAmount" runat="server" ClientIDMode="Static"></asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <asp:HiddenField ID="hdnTaxSelectedRec" runat="server" ViewStateMode="Enabled" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnTaxIsUpdate" runat="server" ClientIDMode="Static" />
    <script type="text/javascript">
        function chkboxIDClick(state) {
            var hdnTaxSelectedRec = document.getElementById("hdnTaxSelectedRec");
            hdnTaxSelectedRec.value = "";
            var m = GridTaxList.GridBodyContainer;
            var allInput = m.getElementsByTagName('input');
            for (var c = 0; c < allInput.length; c++) {
                if (allInput[c].type == "checkbox") {
                    var chk = eval(allInput[c]);
                    if (chk.checked == true) {
                        var getchkbox = document.getElementById(chk.id);
                        var getParent = getchkbox.parentElement;
                        if (getchkbox != null) {
                            if (hdnTaxSelectedRec.value != "") hdnTaxSelectedRec.value += ',' + getParent.title;
                            if (hdnTaxSelectedRec.value == "") hdnTaxSelectedRec.value = getParent.title;
                        }
                    }
                }
            }
            var hdnTaxIsUpdate = document.getElementById("hdnTaxIsUpdate");
            hdnTaxIsUpdate.value = "false"
            var oRecord = new Object;
            oRecord.Save = "N";
            GridTaxList.executeUpdate(oRecord);

        }

        function setTaxAmount() {
            var lblTaxAmount = document.getElementById('lblTaxAmount');
            var totaltax = 0;
            for (var m = 0; m <= GridTaxList.Rows.length - 1; m++) {
                totaltax = totaltax + parseFloat(GridTaxList.Rows[m].Cells["TaxAmount"].Value);
            }
            lblTaxAmount.innerHTML = parseFloat(totaltax).toFixed(2);
            var hdnTaxSelectedRec = document.getElementById("hdnTaxSelectedRec");
            hdnTaxSelectedRec.value = "0";
        }

        function UCApplyTaxSubmit() {
            var hdnTaxSelectedRec = document.getElementById("hdnTaxSelectedRec");
            hdnTaxSelectedRec.value = "";
            var m = GridTaxList.GridBodyContainer;
            var allInput = m.getElementsByTagName('input');
            for (var c = 0; c < allInput.length; c++) {
                if (allInput[c].type == "checkbox") {
                    var chk = eval(allInput[c]);
                    if (chk.checked == true) {
                        var getchkbox = document.getElementById(chk.id);
                        var getParent = getchkbox.parentElement;
                        if (getchkbox != null) {
                            if (hdnTaxSelectedRec.value != "") hdnTaxSelectedRec.value += ',' + getParent.title;
                            if (hdnTaxSelectedRec.value == "") hdnTaxSelectedRec.value = getParent.title;
                        }
                    }
                }
            }
            //alert(hdnTaxSelectedRec.value);
            var oRecord = new Object;
            oRecord.Save = "Save";
            GridTaxList.executeUpdate(oRecord);
            alert("Total Tax Amount is : " + document.getElementById('lblTaxAmount').innerHTML);
           // window.opener.setCartTotalOnChange();  Temparary Comment
            window.opener.Grid1.refresh();
            self.close();
        }

        function setCartTotalOnFail() { }


    </script>
</asp:Content>

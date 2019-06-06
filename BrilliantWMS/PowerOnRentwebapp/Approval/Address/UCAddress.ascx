<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCAddress.ascx.cs" Inherits="PowerOnRentwebapp.Address.UCAddress"
    EnableViewState="false" %>
<%@ Register Assembly="obout_Grid_NET" Namespace="Obout.Grid" TagPrefix="obout" %>
<div>
    <center>
        <table class="gridFrame" style="width: 100%">
            <tr>
                <td style="text-align: left;">
                    <span id="headerText">Address List</span>
                </td>
                <td style="text-align: right;">
                    <input type="button" id="btnAddress" value="Add New" onclick="openAddressWindow('0')" />
                    &nbsp;
                    <input type="button" id="btnMoveToArchiveAddress" value="Archive" onclick="MoveToArchiveAddress()" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <obout:Grid ID="GvAddressInfo" runat="server" AutoGenerateColumns="false" AllowAddingRecords="false"
                        AllowFiltering="true" AllowGrouping="true" Width="100%" Serialize="false" CallbackMode="true"
                        OnRebind="GvAddressInfo_RebindGrid">
                        <ClientSideEvents ExposeSender="true" />
                        <Columns>
                            <obout:CheckBoxSelectColumn AllowSorting="true" ControlType="Obout" Width="3%" Align="center"
                                HeaderAlign="center">
                            </obout:CheckBoxSelectColumn>
                            <obout:Column DataField="Sequence" HeaderText="Edit" Width="4%" AllowSorting="false">
                                <TemplateSettings TemplateId="GvTempEdit" />
                            </obout:Column>
                            <obout:Column DataField="ID" HeaderText="ID" Width="0%" Visible="false">
                            </obout:Column>
                            <obout:Column DataField="AddressLine1" HeaderText="Address" Width="20%">
                            </obout:Column>
                            <obout:Column DataField="County" HeaderText="Country" Width="10%">
                            </obout:Column>
                            <obout:Column DataField="State" HeaderText="State" Width="10%">
                            </obout:Column>
                            <obout:Column DataField="City" HeaderText="City" Width="10%">
                            </obout:Column>
                            <obout:Column DataField="ZipCode" HeaderText="Postal Code" Width="10%">
                            </obout:Column>
                            <obout:Column DataField="PhoneNo" HeaderText="Phone No." Width="10%">
                            </obout:Column>
                            <%--   <obout:Column HeaderText="Active" DataField="Active" Width="5%">
                                <TemplateSettings TemplateId="tplActive" />
                            </obout:Column>--%>
                            <obout:Column DataField="BillIsChecked" HeaderText="Billing" Width="5%" AllowSorting="false"
                                Align="center" HeaderAlign="center">
                                <TemplateSettings TemplateId="GvTempBilling" />
                            </obout:Column>
                            <obout:Column DataField="ShipIsChecked" HeaderText="Shipping" Width="6%" AllowSorting="false"
                                Align="center" HeaderAlign="center">
                                <TemplateSettings TemplateId="GvTempShipping" />
                            </obout:Column>
                        </Columns>
                        <Templates>
                            <%--  <obout:GridTemplate runat="server" ID="tplActive">
                                <Template>
                                    <%# (Container.DataItem["Active"].ToString() == "Y" ? "Yes" : "No")%>
                                </Template>
                            </obout:GridTemplate>--%>
                            <obout:GridTemplate ID="GvTempEdit" runat="server">
                                <Template>
                                    <img id="imgBtnEdit" src="../App_Themes/Blue/img/Edit16.png" title="Edit" onclick="openAddressWindow('<%# (Container.DataItem["Sequence"].ToString()) %>');"
                                        style="cursor: pointer;" />
                                </Template>
                            </obout:GridTemplate>
                            <obout:GridTemplate ID="GvTempShipping" runat="server">
                                <Template>
                                    <input type="checkbox" id="rbtnShipingS" onclick="getShippingSeq(this,'<%# (Container.DataItem["Sequence"].ToString()) %>')"
                                        <%# Container.Value.ToString().ToLower()=="true" ? "checked='checked'" : "" %> />
                                </Template>
                            </obout:GridTemplate>
                            <obout:GridTemplate ID="GvTempBilling" runat="server">
                                <Template>
                                    <input type="checkbox" id="rbtnbillingB" onclick="getBillingSeq(this,'<%# (Container.DataItem["Sequence"].ToString()) %>')"
                                        <%# Container.Value.ToString().ToLower()=="true" ? "checked='checked'" : "" %> />
                                </Template>
                            </obout:GridTemplate>
                        </Templates>
                    </obout:Grid>
                </td>
            </tr>
        </table>
        <asp:HiddenField ID="hdnAddressTargetObject" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnshipping" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnbilling" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnSelectedRec" runat="server" ClientIDMode="Static" />
    </center>
    <script type="text/javascript">
        function setAddressTargetObjectName(TragetObjectName) {
            document.getElementById("hdnAddressTargetObject").value = TragetObjectName;
            document.getElementById("hdnshipping").value = "1";
            document.getElementById("hdnbilling").value = "1";
        }
        function openAddressWindow(sequence) {
            window.open('../Address/AddressInfo.aspx?Sequence=' + sequence + '&TargetObject=' + hdnAddressTargetObject.value + '', null, 'height=280, width=1000,status= 0, resizable= 1, scrollbars=0, toolbar=0,location=center,menubar=0, screenX=300; screenY=50');
        }

        function getShippingSeq(sender, seuqence) {
            var hdnshipping = document.getElementById("hdnshipping");
            hdnshipping.value = "";
            var rbtn = eval(sender);
            if (rbtn.checked == true) {
                var parentEle = rbtn.parentElement;
                hdnshipping.value = seuqence.toString();
                onlyoneselectAddress(sender, "S");
            }
        }

        function getBillingSeq(sender, seuqence) {
            var hdnbilling = document.getElementById("hdnbilling");
            hdnbilling.value = "";
            var rbtn = eval(sender);
            if (rbtn.checked == true) {
                var parentEle = rbtn.parentElement;
                hdnbilling.value = seuqence;
                onlyoneselectAddress(sender, "B");
            }
        }

        function onlyoneselectAddress(invoker, colname) {
            var m = GvAddressInfo.GridBodyContainer;
            var allInput = m.getElementsByTagName('input');
            for (var c = 0; c <= allInput.length - 1; c++) {
                var chk = allInput[c];
                if (chk.type == "checkbox") {
                    var strid = chk.id;
                    var rbtn = eval(chk);
                    getsubstring = strid.substring(strid.length - 1, strid.length);
                    var getParent = chk.parentElement;
                    if (getsubstring == "S" && colname == "S") { rbtn.checked = false; }
                    if (getsubstring == "B" && colname == "B") { rbtn.checked = false; }
                }
            }
            invoker.checked = true;
        }

        function MoveToArchiveAddress() {
            var Ids = document.getElementById("hdnSelectedRec");
            for (var i = 0; i < GvAddressInfo.SelectedRecords.length; i++) {
                var record = GvAddressInfo.SelectedRecords[i];
                if (Ids.value != "") Ids.value += ',' + record.Sequence;
                if (Ids.value == "") Ids.value = record.Sequence;
            }
            PageMethods.PMMoveAddressToArchive(Ids.value, "Y");
            Ids.value = "";
            GvAddressInfo.refresh();
        }

        

    </script>
</div>

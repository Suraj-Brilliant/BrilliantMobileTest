<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/CRM2.Master" AutoEventWireup="true" CodeBehind="AddEditSearchAddress.aspx.cs" Inherits="BrilliantWMS.PowerOnRent.AddEditSearchAddress"
    EnableEventValidation="false" Theme="Blue" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="obout_Interface" Namespace="Obout.Interface" TagPrefix="obout" %>
<%@ Register Assembly="obout_Grid_NET" Namespace="Obout.Grid" TagPrefix="obout" %>
<%@ Register Src="../CommonControls/UCFormHeader.ascx" TagName="UCFormHeader" TagPrefix="uc1" %>
<%@ Register Src="../CommonControls/UCToolbar.ascx" TagName="UCToolbar" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="../MasterPage/JavaScripts/countries.js" type="text/javascript"></script>
    <script src="../MasterPage/JavaScripts/crm.js" type="text/javascript"></script>
    <center>
        <asp:UpdateProgress ID="UpdateProgress_Contact" runat="server" AssociatedUpdatePanelID="updPnl_Contact">
            <ProgressTemplate>
                <center>
                    <div class="modal">
                        <img src="../App_Themes/Blue/img/ajax-loader.gif" style="top: 50%;" />
                    </div>
                </center>
            </ProgressTemplate>
        </asp:UpdateProgress>
        <asp:UpdatePanel ID="updPnl_Contact" runat="server">
            <ContentTemplate>
                <asp:HiddenField ID="hdnConID" runat="server" />
                <asp:HiddenField ID="hdnstate" runat="server" ClientIDMode="Static" />
                <table class="tableForm" id="tblAddAdrs" runat="server">
                    <tr>
                        <td>
                            <req><asp:Label Id="lbladdress1" runat="server" Text="Address Line"/></req>
                            :
                        </td>
                        <td style="text-align: left" colspan="4">
                            <asp:TextBox ID="txtAddress" TextMode="MultiLine" runat="server" Width="435px" MaxLength="100"
                                onkeyup="TextBox_KeyUp(this,'CharactersCounter1','100');" ClientIDMode="Static"
                                onchange="CheckDuplicateAddress();"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <%--<td>
                            <req><asp:Label Id="lblcountry" runat="server" Text="Country"/></req>
                            :
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlCountry" ClientIDMode="Static" runat="server" onchange="print_state('ddlState',this.selectedIndex);" Width="182px">
                            </asp:DropDownList>
                        </td>--%>
                        <td>
                            <req><asp:Label Id="lblstate" runat="server" Text="State"/> </req>
                            :
                        </td>
                        <td style="text-align:left;">
                            <asp:DropDownList ID="ddlState" ClientIDMode="Static" runat="server" DataValueField ="State" DataTextField ="State" Width="182px">
                                <%--onchange="CheckDuplicateAddress();printZone(); printSubZone();"--%>
                            </asp:DropDownList>
                        </td>
                        <td>
                            <req><asp:Label Id="lblcity" runat="server" Text="City"/></req>
                            :
                        </td>
                        <td style="text-align:left;">
                            <asp:TextBox ID="txtcity" MaxLength="50" Width="182px" runat="server" ClientIDMode="Static"></asp:TextBox>
                            <%--onchange="CheckDuplicateAddress()"--%>                                    
                        </td>
                    </tr>
                    <tr>                        
                        <td></td>
                        <td></td>
                        <td></td>
                        <td style="text-align: right">
                            <input type="button" id="btnSave" title="Save" runat="server" value="Save" onclick="SaveContactDetail();" />
                        </td>
                    </tr>
                </table>
                <asp:HiddenField ID="hdnContactID" runat="server" />
                <table class="gridFrame" width="90%">
                    <tr>
                        <td>
                            <table style="width: 100%">
                                <tr>
                                    <td style="text-align: left;">
                                        <a id="headerText"><asp:Label Id="lblAddressList" runat="server" Text="Address List"/></a>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td style="text-align: right;">
                            <input type="button" id="btnSubmit" title="Save" runat="server" value="Submit" onclick="GetContactPerson();" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <obout:Grid ID="gvContactPerson" runat="server" AutoGenerateColumns="False" AllowAddingRecords="False"
                                AllowFiltering="True" AllowGrouping="True" AllowMultiRecordSelection="False"
                                AllowRecordSelection="true" Width="100%" AllowColumnResizing="true" AllowColumnReordering="true" OnRebind="gvContactPerson_OnRebind">
                                <Columns>
                                    <obout:Column HeaderText="Edit" DataField="ID" Width="3%" Align="center" HeaderAlign="center">
                                        <TemplateSettings TemplateId="TemplateEdit" />
                                    </obout:Column>
                                    <%-- <obout:Column DataField="ID" HeaderText="ID" Visible="false">
                                    </obout:Column>--%>
                                    <obout:Column DataField="AddressLine1" HeaderText="Address" Width="15%">
                                    </obout:Column>
                                    <obout:Column DataField="City" HeaderText="City" Width="8%">
                                    </obout:Column>
                                    <obout:Column DataField="ContactName" HeaderText="Contact Name" Width="8%"></obout:Column>
                                    <obout:Column DataField="MobileNo" HeaderText="MobileNo" Width="10%">
                                    </obout:Column>
                                    <obout:Column DataField="ContactEmail" HeaderText="Email" Width="15%"></obout:Column>
                                </Columns>
                                <Templates>
                                    <obout:GridTemplate runat="server" ID="TemplateEdit">
                                        <Template>
                                            <asp:ImageButton ID="imgBtnEdit1" runat="server" ImageUrl="../App_Themes/Blue/img/Edit16.png"
                                                ToolTip='<%# (Container.Value) %>' CausesValidation="false" OnClick="imgBtnEdit_OnClick" />
                                        </Template>
                                    </obout:GridTemplate>
                                </Templates>
                            </obout:Grid>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </center>
    <asp:HiddenField ID="hdnSelectedRec" runat="server" ClientIDMode="Static" />
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
                SelectedPrdRec();                
            }
        }

        function SelectedPrdRec() {
            var hdnSelectedRec = document.getElementById("hdnSelectedRec");
            hdnSelectedRec.value = "";

            for (var i = 0; i < gvContactPerson.PageSelectedRecords.length; i++) {
                var record = gvContactPerson.PageSelectedRecords[i];
                if (hdnSelectedRec.value != "") hdnSelectedRec.value += ',' + record.ID;
                if (hdnSelectedRec.value == "") hdnSelectedRec.value = record.ID;
            }
        }



        function SaveContactDetail() {
            var txtAddress = document.getElementById("<%= txtAddress.ClientID %>");           
            var ddlState = document.getElementById("<%= ddlState.ClientID %>");
            var txtcity = document.getElementById("<%= txtcity.ClientID %>");
            var hdnstate = document.getElementById('hdnstate');
            if (txtAddress.value == "") {
                showAlert("Enter Address", "Error");
                txtAddress.focus();
            } else if (txtcity.value == "") {
                showAlert("Enter City", "Error");
                txtEmail.focus();
            } else {
                var AddressInfo = new Object();
                AddressInfo.AddressLine1 = document.getElementById("txtAddress").value;               
                AddressInfo.State = document.getElementById("ddlState").value;
                AddressInfo.City = txtcity.value.toString();
                PageMethods.WMSaveRequestHead(AddressInfo, hdnstate.value, WMSaveContact_onSuccessed, WMSaveContact_onFailed);
            }

            function WMSaveContact_onSuccessed(result) {
                if (result == "Some error occurred" || result == "") { showAlert("Error occurred", "Error", '#'); }
                else if (result == "Address saved successfully") {
                    showAlert(result, "info");
                    txtcity.value = "";
                    txtAddress.value = "";
                    gvContactPerson.refresh();
                    self.close();
                }
            }
            function WMSaveContact_onFailed() {
                showAlert("Error occurred", "Error", "#"); self.close();
            }
        }

        function GetContactPerson() {
            var hdnSelectedRec = document.getElementById('hdnSelectedRec');
            if (hdnSelectedRec.value == "") {
                showAlert("Select Atleast One Address...", "Error");
            }else{
                var SelCon = hdnSelectedRec.value;
                var count = (SelCon.match(/,/g) || []).length;
                console.log(count);
                if (count >= 1) {
                    showAlert("Select Only One Address", "Error", "#");
                } else {
                    var hdnSearchAddressID = window.opener.document.getElementById("hdnSearchAddressID");
                    var hdnSearchAddress = window.opener.document.getElementById("hdnSearchAddress");
                    hdnSearchAddressID.value = "";
                    hdnSearchAddress.value = "";
                    if (gvContactPerson.SelectedRecords.length > 0) {
                        for (var i = 0; i < gvContactPerson.SelectedRecords.length; i++) {
                            var record = gvContactPerson.SelectedRecords[i];
                            if (hdnSearchAddressID.value != "") hdnSearchAddressID.value += ',' + record.ID;
                            if (hdnSearchAddressID.value == "") {
                                hdnSearchAddressID.value = record.ID;
                                hdnSearchAddress.value = record.AddressLine1;
                                //hdnSearchAddress.value = record.AddressLine1 + ', Contact Name:' + record.ContactName + ',Mobile:' + record.MobileNo + ', Email:' + record.ContactEmail;
                            }
                        }
                        window.opener.AfterAddressselected(hdnSearchAddressID.value, hdnSearchAddress.value);
                        self.close();
                    }
                }
            }
        }
    </script>
</asp:Content>

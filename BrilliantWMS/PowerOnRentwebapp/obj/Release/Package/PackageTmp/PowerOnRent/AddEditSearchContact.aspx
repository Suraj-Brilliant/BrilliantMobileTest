<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/CRM2.Master" AutoEventWireup="true" CodeBehind="AddEditSearchContact.aspx.cs" EnableEventValidation="false"
    Inherits="BrilliantWMS.PowerOnRent.AddEditSearchContact" Theme="Blue" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="obout_Interface" Namespace="Obout.Interface" TagPrefix="obout" %>
<%@ Register Assembly="obout_Grid_NET" Namespace="Obout.Grid" TagPrefix="obout" %>
<%@ Register Src="../CommonControls/UCFormHeader.ascx" TagName="UCFormHeader" TagPrefix="uc1" %>
<%@ Register Src="../CommonControls/UCToolbar.ascx" TagName="UCToolbar" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <uc1:UCFormHeader ID="UCFormHeader1" runat="server" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
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
                <table class="tableForm">
                    <tr>
                        <td>
                            <req><asp:Label ID="lblContactName" runat="server" Text="Contact Name :"></asp:Label></req>
                        </td>
                        <td style="text-align: left">
                            <asp:TextBox ID="txtName" runat="server" Width="200px" onKeyPress="return alpha(event);"
                                MaxLength="50"></asp:TextBox>
                        </td>
                        <td>
                            <req><asp:Label ID="lblEmailID" runat="server" Text="Email ID :"></asp:Label></req>
                        </td>
                        <td style="text-align: left">
                            <asp:TextBox ID="txtEmail" runat="server" Width="200px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <req><asp:Label ID="lblMobileNo" runat="server" Text="Mobile No.:"></asp:Label></req>
                        </td>
                        <td style="text-align: left">
                            <asp:TextBox ID="txtMobileNo" runat="server" Width="200px" MaxLength="10" onkeydown="return AllowPhoneNo(this, event);"
                                onkeypress="return AllowPhoneNo(this, event);"></asp:TextBox>
                        </td>
                        <td></td>
                        <td style="text-align: right">
                            <input type="button" id="btnSave" runat="server" title="Save" value="Save" onclick="SaveContactDetail();" />
                            <%--<asp:Button ID="btnSaveAsTemplateNew" runat="server" Text="Save" Style="cursor: pointer;" OnClientClick="SaveContactDetail();" />--%>
                        </td>
                    </tr>
                </table>
                <asp:HiddenField ID="hdnContactID" runat="server" />
                <table class="gridFrame" width="70%">
                    <tr>
                        <td>
                            <table style="width: 100%">
                                <tr>
                                    <td style="text-align: left;">
                                        <a id="headerText"><asp:Label ID="lblContactPersonList" runat="server" Text="Contact Person List"></asp:Label></a>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td style="text-align: right;">
                            <input type="button" id="btnSubmit" runat="server" title="Save" value="Submit" onclick="GetContactPerson();" />
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
                                    <obout:Column DataField="Name" HeaderText="Name" Width="8%">
                                    </obout:Column>
                                    <obout:Column DataField="EmailID" HeaderText="EmailID" Width="12%">
                                    </obout:Column>
                                    <obout:Column DataField="MobileNo" HeaderText="MobileNo" Width="5%">
                                    </obout:Column>
                                </Columns>
                                <Templates>
                                    <obout:GridTemplate runat="server" ID="TemplateEdit">
                                        <Template>
                                            <asp:ImageButton ID="imgBtnEdit1" runat="server" ImageUrl="../App_Themes/Blue/img/Edit16.png"
                                                OnClick="imgBtnEdit_OnClick" ToolTip='<%# (Container.Value) %>' CausesValidation="false" />
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
            var txtName = document.getElementById("<%= txtName.ClientID %>");
            var txtEmail = document.getElementById("<%= txtEmail.ClientID %>");
            var txtMobileNo = document.getElementById("<%= txtMobileNo.ClientID %>");
            var hdnstate = document.getElementById('hdnstate');
            if (txtName.value == "") {
                showAlert("Enter Contact Person Name", "Error");
                txtName.focus();
            } else if (txtEmail.value == "") {
                showAlert("Enter Contact Person EmailID", "Error");
                txtEmail.focus();
            } else {
                //showAlert(" Contact Person  Saved", "info");
                if (!IsValidEmail(txtEmail.value)) {
                    showAlert("Enter Valid EmailID", "Error");
                    txtEmail.value = "";
                    txtEmail.focus();
                } else {
                    var obj1 = new Object();
                    obj1.Name = txtName.value.toString();
                    obj1.EmailID = txtEmail.value.toString();
                    obj1.MobileNo = txtMobileNo.value.toString();
                    PageMethods.WMSaveRequestHead(obj1, hdnstate.value, WMSaveContact_onSuccessed, WMSaveContact_onFailed);
                }
            }

            function WMSaveContact_onSuccessed(result) {
                if (result == "Some error occurred" || result == "") { showAlert("Error occurred", "Error", '#'); }
                else if (result == "Contact saved successfully") {
                    showAlert(result, "info");
                    txtEmail.value = "";
                    txtMobileNo.value = "";
                    txtName.value = "";
                    gvContactPerson.refresh();
                    self.close();
                }
            }
            function WMSaveContact_onFailed() {
                showAlert("Error occurred", "Error", "#");
                self.close();
            }
        }
        function IsValidEmail(email) {
            var expr = /^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/;
            return expr.test(email)
        }
        function GetContactPerson() {
            var hdnSelectedRec = document.getElementById('hdnSelectedRec');
            if (hdnSelectedRec.value == "") {
                showAlert("Select Atleast One Contact Person...", "Error");
            } else if (getParameterByName("Con").toString() == "1") {
                var SelCon = hdnSelectedRec.value;
                var count = (SelCon.match(/,/g) || []).length;
                console.log(count);
                if (count > 1) {
                    showAlert("Select Only One Contact Person", "Error", "#");
                } else {
                    var hdnSearchContactID1 = window.opener.document.getElementById("hdnSearchContactID1");
                    var hdnSearchContactName1 = window.opener.document.getElementById("hdnSearchContactName1");
                    hdnSearchContactID1.value = "";
                    hdnSearchContactName1.value = "";
                    if (gvContactPerson.SelectedRecords.length > 0) {
                        for (var i = 0; i < gvContactPerson.SelectedRecords.length; i++) {
                            var record = gvContactPerson.SelectedRecords[i];
                            if (hdnSearchContactID1.value != "") hdnSearchContactID1.value += ',' + record.ID;
                            if (hdnSearchContactID1.value == "") {
                                hdnSearchContactID1.value = record.ID;
                                hdnSearchContactName1.value = record.Name;
                            }
                        }
                        window.opener.AfterContact1Selected(hdnSearchContactID1.value, hdnSearchContactName1.value);
                        self.close();
                    }
                }
            } else if (getParameterByName("Con").toString() == "2") {
                var hdnSearchContactID2 = window.opener.document.getElementById("hdnSearchContactID2");
                var hdnSearchContactName2 = window.opener.document.getElementById("hdnSearchContactName2");
                hdnSearchContactID2.value = "";
                hdnSearchContactName2.value = "";
                if (gvContactPerson.SelectedRecords.length > 0) {
                    for (var i = 0; i < gvContactPerson.SelectedRecords.length; i++) {
                        var record = gvContactPerson.SelectedRecords[i];
                        if (hdnSearchContactID2.value != "") hdnSearchContactID2.value += ',' + record.ID;
                        if (hdnSearchContactID2.value == "") hdnSearchContactID2.value = record.ID;
                        if (hdnSearchContactName2.value != "") hdnSearchContactName2.value +=','+ record.Name;
                        if (hdnSearchContactName2.value == "") hdnSearchContactName2.value =  record.Name;                       
                    }
                    window.opener.AfterContact2Selected(hdnSearchContactID2.value, hdnSearchContactName2.value);
                    self.close();
                }
            }
        }
    </script>
</asp:Content>

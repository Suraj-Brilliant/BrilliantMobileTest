<%@ Page Title="Account Search" Language="C#" MasterPageFile="~/MasterPage/CRM2.Master"
    AutoEventWireup="true" CodeBehind="AccountSearch.aspx.cs" Inherits="WebClientElegantCRM.Account.AccountSearch" EnableEventValidation="false" %>

<%@ Register Assembly="obout_Grid_NET" Namespace="Obout.Grid" TagPrefix="obout" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style="margin: 3px 3px 3px 3px">
        <table class="gridFrame" width="820px">
            <tr>
                <td>
                    <table style="width: 100%">
                        <tr>
                            <td style="text-align: left;">
                                <asp:Label ID="lbluserlist" runat="server" CssClass="headerText" Text="User List"></asp:Label>
                            </td>
                            <td style="text-align: right;">
                                <input type="button" value="Submit" runat="server" id="btnSubmitAccountSearch" onclick="selectedUserRec();" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <center>
                        <obout:Grid ID="GvAccount" runat="server" AutoGenerateColumns="false" AllowAddingRecords="false"
                            AllowFiltering="true" AllowGrouping="true" Width="100%" AllowMultiRecordSelection="True"
                            AllowRecordSelection="true" Serialize="true" CallbackMode="true" AllowColumnResizing="true"
                            AllowColumnReordering="true" PageSize="50">
                            <ClientSideEvents ExposeSender="true" />
                            <Columns>
                                <%--  <obout:Column HeaderText="Select" ShowHeader="false" DataField="ID" Width="9%" Align="center"
                                    HeaderAlign="center">
                                    <TemplateSettings TemplateId="CustomerNameTemplate" />
                                </obout:Column>--%>
                                <obout:Column HeaderText="ID" DataField="userID" Visible="false">
                                </obout:Column>
                                <obout:Column HeaderText="Name" DataField="userName" Width="15%">
                                </obout:Column>
                                <obout:Column HeaderText="Email" DataField="EmailID" Width="15%">
                                </obout:Column>
                                <obout:Column HeaderText="Mobile" DataField="MobileNo" Width="10%">
                                </obout:Column>
                                <obout:Column HeaderText="Company" DataField="CompanyName" Width="10%">
                                </obout:Column>
                                <obout:Column HeaderText="Department" DataField="DeptName" Width="15%">
                                </obout:Column>
                                <%--<obout:Column HeaderText="Designation" DataField="Designation" Width="15%">
                                </obout:Column>    --%>
                            </Columns>
                            <Templates>
                                <obout:GridTemplate runat="server" ID="CustomerNameTemplate" ControlID="discontinued"
                                    ControlPropertyName="value">
                                    <Template>
                                        <input type="radio" id="radaccountselect" name="radaccountselect" value="True" />
                                    </Template>
                                </obout:GridTemplate>
                            </Templates>
                        </obout:Grid>
                    </center>
                </td>
            </tr>
            <tr>
                <td>
                    <table style="width: 100%">
                        <tr>
                            <td></td>
                            <td style="text-align: right;">
                                <input type="button" value="Submit" id="Button1" runat="server" onclick="selectedUserRec();" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <asp:HiddenField ID="hdnNoOfApprover" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnUserSelectedRec" runat="server" ClientIDMode="Static" />
    </div>
    <script type="text/javascript">
        window.onload = function () {
            oboutGrid.prototype.restorePreviousSelectedRecord = function () {
                return;
            }
            oboutGrid.prototype.markRecordAsSelectedOld = oboutGrid.prototype.markRecordAsSelected;
            oboutGrid.prototype.markRecordAsSelected = function (row, param2, param3, param4, param5) {
                if (row.className != this.CSSRecordSelected) {
                    this.markRecordAsSelectedOld(row, param2, param3, param4, param5);

                }
                else {
                    var index = this.getRecordSelectionIndex(row);
                    if (index != -1) {
                        this.markRecordAsUnSelected(row, index);
                    }
                }

            }
        }

        function selectedUserRec() {
            var hdnUserSelectedRec = document.getElementById("hdnUserSelectedRec");
            var hdnNoOfApprover = document.getElementById("hdnNoOfApprover").value;
            hdnUserSelectedRec.value = "";
            //if (GvAccount.SelectedRecords.length > 0) 
            if (GvAccount.SelectedRecords.length == hdnNoOfApprover) {
                for (var i = 0; i < GvAccount.PageSelectedRecords.length; i++) {
                    var record = GvAccount.PageSelectedRecords[i];
                    if (hdnUserSelectedRec.value != "") hdnUserSelectedRec.value += ',' + record.userID;
                    if (hdnUserSelectedRec.value == "") hdnUserSelectedRec.value = record.userID;
                }
                PageMethods.PMGetHiddenValue(hdnUserSelectedRec.value, onsuccess_getHiddenValue, null)

            } else if (GvAccount.SelectedRecords.length == 0) {
                //showAlert("Select " + hdnNoOfApprover + " Users", "Error");
                alert("Select Only " + hdnNoOfApprover + " Users");
            } else if (hdnNoOfApprover < GvAccount.SelectedRecords.length) {
                //showAlert("Select Only " + hdnNoOfApprover + " Users", "Error");
                alert("Select Only " + hdnNoOfApprover + " Users");
            } else if (hdnNoOfApprover > GvAccount.SelectedRecords.length) {
               // showAlert("Select Only " + hdnNoOfApprover + " Users", "Error");
                alert("Select Only " + hdnNoOfApprover + " Users");
            }
        }

        function onsuccess_getHiddenValue(result) {
            window.opener.gvUserCreation.refresh();
            self.close();
        }

        function GvAccount_Deselect(index) {
            selectedUserRec();
        }



    </script>
</asp:Content>

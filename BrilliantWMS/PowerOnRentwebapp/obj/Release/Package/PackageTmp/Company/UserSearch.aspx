<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/CRM2.Master" AutoEventWireup="true"
    CodeBehind="UserSearch.aspx.cs" Inherits="BrilliantWMS.Student.UserSearch" %>

<%@ Register Assembly="obout_Grid_NET" Namespace="Obout.Grid" TagPrefix="obout" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table class="gridFrame" width="750px" style="margin: 3px 3px 3px 3px;">
        <tr>
            <td>
                <table style="width: 100%">
                    <tr>
                        <td style="text-align: left;">
                            <a class="headerText">User List</a>
                        </td>
                        <td style="text-align: right;">
                            <input type="button" value="Submit" id="btnSubmitUser" onclick="selectedRecUser();" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <obout:Grid ID="GridUserSearch" runat="server" AutoGenerateColumns="false" AllowFiltering="true"
                    AllowGrouping="true" AllowColumnResizing="true" AllowAddingRecords="false" AllowColumnReordering="true"
                    AllowMultiRecordSelection="true" CallbackMode="true" OnRebind="RebindGrid" Width="100%"
                    Serialize="true" PageSize="50">
                    <ClientSideEvents ExposeSender="true" />
                    <Columns>
                        <obout:CheckBoxSelectColumn ShowHeaderCheckBox="true" ControlType="Obout" Align="center"
                            HeaderAlign="center" Width="5%">
                        </obout:CheckBoxSelectColumn>
                        <obout:Column DataField="UserID" Visible="false">
                        </obout:Column>
                        <obout:Column DataField="EmployeeID" Visible="true" HeaderText="User ID" Width="10%">
                        </obout:Column>
                        <obout:Column DataField="userName" Visible="true" HeaderText="UserName" Width="25%">
                        </obout:Column>
                        <obout:Column DataField="DeptName" Visible="true" HeaderText="Department" Width="25%">
                        </obout:Column>
                        <obout:Column DataField="DesiName" Visible="true" HeaderText="Designation" Width="25%">
                        </obout:Column>
                    </Columns>
                </obout:Grid>
            </td>
        </tr>
    </table>
    <script type="text/javascript">
        function selectedRecUser() {
            var hdnUserSearchSelectedRec = window.opener.document.getElementById("hdnUserSearchSelectedRec");
            var UserName = "";
            hdnUserSearchSelectedRec.value = "";
            if (GridUserSearch.SelectedRecords.length > 0) {
                for (var i = 0; i < GridUserSearch.SelectedRecords.length; i++) {
                    var record = GridUserSearch.SelectedRecords[i];
                    if (hdnUserSearchSelectedRec.value != "") {
                        hdnUserSearchSelectedRec.value += ',' + record.UserID;
                        UserName += ' | ' + record.userName;

                    }
                    if (hdnUserSearchSelectedRec.value == "") {
                        hdnUserSearchSelectedRec.value = record.UserID;
                        UserName = record.userName;
                    }
                }
                window.opener.AfterUserSelected(UserName);
                self.close();
            }
            if (GridUserSearch.SelectedRecords.length == 0) {
                alert("Select atleast one User");
            }
        }
       
    </script>
</asp:Content>

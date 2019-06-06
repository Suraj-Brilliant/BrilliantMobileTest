<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/CRM2.Master" AutoEventWireup="true" CodeBehind="ClientList.aspx.cs" Inherits="BrilliantWMS.WMS.ClientList" %>

<%@ Register Assembly="obout_Grid_NET" Namespace="Obout.Grid" TagPrefix="obout" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style="margin: 3px 3px 3px 3px">
        <table class="gridFrame" width="720px">
            <tr>
                <td>
                    <table style="width: 100%">
                        <tr>
                            <td style="text-align: left;">
                                <a class="headerText">Client List</a>
                            </td>
                            <td style="text-align: right;">
                                <input type="button" value="Submit" id="btnSubmitClientSearch" onclick="getSelectedClientSearch();" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <center>
                        <obout:Grid ID="grdClient" runat="server" AutoGenerateColumns="false" AllowAddingRecords="false"
                            AllowFiltering="true" AllowGrouping="true" Width="100%" AllowMultiRecordSelection="false"
                            AllowColumnResizing="true" AllowColumnReordering="true" PageSize="50">
                            <Columns>
                                <obout:Column HeaderText="ID" DataField="ID" Width="10%">
                                </obout:Column>
                                <obout:Column HeaderText="Name" DataField="Name" Width="15%">
                                </obout:Column>
                                <obout:Column HeaderText="Code" DataField="Code" Width="36%">
                                </obout:Column>
                            </Columns>
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
                                <input type="button" value="Submit" id="Button1" onclick="getSelectedClientSearch();" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    <script type="text/javascript">
        function getSelectedClientSearch() {
            var hdnSelectedClnt = window.opener.document.getElementById("hdnSelectedClnt");
            var hdnSelectedClntName = window.opener.document.getElementById("hdnSelectedClntName");
            hdnSelectedClnt.value = "";
            if (grdClient.SelectedRecords.length > 0) {
                var record = grdClient.SelectedRecords[0];
                hdnSelectedClnt.value = record.ID;
                hdnSelectedClntName.value = record.Name;
                window.opener.getClientInfo(hdnSelectedClnt.value, hdnSelectedClntName.value);
                self.close();
            }
            if (grdClient.SelectedRecords.length == 0) { showAlert("Please Select Client.", "Error", "#"); }
        }
    </script>
</asp:Content>

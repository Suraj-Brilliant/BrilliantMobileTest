<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserList.aspx.cs" MasterPageFile="~/MasterPage/CRM2.Master"
    Title="User List" Inherits="BrilliantWMS.UserManagement.UserList" %>

<%@ Register Assembly="obout_Interface" Namespace="Obout.Interface" TagPrefix="obout" %>
<%@ Register Assembly="obout_ComboBox" Namespace="Obout.ComboBox" TagPrefix="cc1" %>
<%@ Register Assembly="obout_Grid_NET" Namespace="Obout.Grid" TagPrefix="obout" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <table class="gridFrame" width="700px">
            <tr>
                <td>
                    <table style="width: 100%">
                        <tr>
                            <td style="text-align: left;">
                                <a class="headerText">Users List</a>
                            </td>
                            <td style="text-align: right;">
                                <input type="button" value="Submit" id="Button2" onclick="downloadClick();" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <obout:Grid ID="GvAccessTo" runat="server" AutoGenerateColumns="false" AllowFiltering="true"
                        AllowGrouping="true" AllowColumnResizing="true" AllowAddingRecords="false" AllowColumnReordering="true"
                        AllowMultiRecordSelection="true" AllowPageSizeSelection="true" AllowPaging="true"
                        AllowRecordSelection="true" AllowSorting="true" FilterType="Normal" ShowLoadingMessage="true"
                        Width="750px">
                        <ClientSideEvents ExposeSender="true" />
                        <Columns>
                            <obout:Column DataField="UserID" HeaderText="ID" Visible="false">
                            </obout:Column>
                            <obout:Column DataField="userName" HeaderText="Name">
                            </obout:Column>
                            <obout:Column DataField="DeptName" HeaderText="Department">
                            </obout:Column>
                            <obout:Column DataField="DesiName" HeaderText="Designation">
                            </obout:Column>
                            <obout:Column ID="ColDowloadAccess" DataField="Dowload Access" runat="Server" Align="center"
                                HeaderAlign="center">
                                <TemplateSettings TemplateId="TempletDownloadAccess" />
                            </obout:Column>
                            <obout:Column ID="ColDeleteAccess" DataField="Delete Access" runat="Server" Visible="false"
                                Align="center" HeaderAlign="center">
                                <TemplateSettings TemplateId="TempletDeleteAccess" />
                            </obout:Column>
                        </Columns>
                        <Templates>
                            <obout:GridTemplate runat="server" ID="TempletDownloadAccess">
                                <Template>
                                    <obout:OboutCheckBox ID="ChkBoxDownLoad1" runat="server" ToolTip='<%# (Container.DataItem["userID"].ToString())%>'>
                                    </obout:OboutCheckBox>
                                </Template>
                            </obout:GridTemplate>
                            <obout:GridTemplate runat="server" ID="TempletDeleteAccess">
                                <Template>
                                    <obout:OboutCheckBox ID="ChkBoxDownLoad2" runat="server" ToolTip='<%# (Container.DataItem["userID"].ToString())%>'>
                                    </obout:OboutCheckBox>
                                </Template>
                            </obout:GridTemplate>
                        </Templates>
                    </obout:Grid>
                </td>
            </tr>
        </table>
    </div>
    <script type="text/javascript">

        function downloadClick() {
            var hdDownLoadAccessIDs = window.opener.document.getElementById("hdDownLoadAccessIDs");
            var hdnDeleteAccessIDs = window.opener.document.getElementById("hdnDeleteAccessIDs");
            var m = GvAccessTo.GridBodyContainer;
            hdDownLoadAccessIDs.value = "";
            hdnDeleteAccessIDs.value = "";
            var allInput = m.getElementsByTagName('input');
            for (var c = 0; c < allInput.length - 1; c++) {
                var chk = allInput[c];
                if (chk.type == "checkbox") {
                    if (chk.checked == true) {
                        var strid = chk.id;
                        getsubstring = strid.substring(strid.length - 1, strid.length);
                        var getParent = chk.parentElement;
                        if (getsubstring == "1") {
                            if (hdDownLoadAccessIDs.value != "") hdDownLoadAccessIDs.value += ',' + getParent.title;
                            if (hdDownLoadAccessIDs.value == "") hdDownLoadAccessIDs.value = getParent.title;
                        }
                        if (getsubstring == "2") {
                            if (hdnDeleteAccessIDs.value != "") hdnDeleteAccessIDs.value += ',' + getParent.title;
                            if (hdnDeleteAccessIDs.value == "") hdnDeleteAccessIDs.value = getParent.title;
                        }
                    }
                }
            }
            self.close();
        }






       
    </script>
</asp:Content>

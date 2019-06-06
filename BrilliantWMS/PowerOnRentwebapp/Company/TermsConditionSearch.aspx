<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/CRM2.Master" AutoEventWireup="true" CodeBehind="TermsConditionSearch.aspx.cs" Inherits="BrilliantWMS.Company.TermsConditionSearch" %>

<%@ Register Assembly="obout_Grid_NET" Namespace="Obout.Grid" TagPrefix="obout" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <div>
        <table class="gridFrame" width="750px">
            <tr>
                <td>
                    <table style="width: 100%">
                        <tr>
                            <td style="text-align: left;">
                                <a class="headerText">Terms and Conditions List</a>
                            </td>
                            <td style="text-align: right;">
                                <input type="button" value="Submit" id="btnSubmitTerm" onclick="getSelectedTermCondSearch();" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <center>
                        <obout:grid id="GridTermConditionSearch" runat="server" autogeneratecolumns="false"
                            allowfiltering="true" allowgrouping="true" allowcolumnresizing="true" allowaddingrecords="false"
                            allowcolumnreordering="true" allowmultirecordselection="true" allowpagesizeselection="true"
                            allowpaging="true" allowrecordselection="true" allowsorting="true" filtertype="Normal"
                            showloadingmessage="true" serialize="false" callbackmode="true" onrebind="RebindGridTCS" PageSize="-1">
                            <ClientSideEvents ExposeSender="true" />
                            <ScrollingSettings EnableVirtualScrolling="true" />
                            <Columns>
                                <obout:CheckBoxSelectColumn ShowHeaderCheckBox="true" Align="center" HeaderAlign="center"
                                    ControlType="Obout">
                                </obout:CheckBoxSelectColumn>
                                <obout:Column DataField="ID" HeaderText="ID" Visible="false">
                                </obout:Column>
                                <obout:Column DataField="GroupName" HeaderText="GroupName" Width="150px">
                                </obout:Column>
                                <obout:Column DataField="Term" HeaderText="Term Name" Width="150px">
                                </obout:Column>
                                <obout:Column DataField="Condition" HeaderText="Condition" Width="300px">
                                </obout:Column>
                                <obout:Column DataField="Active" HeaderText="Active" Width="80px">
                                </obout:Column>
                            </Columns>
                        </obout:grid>
                    </center>
                </td>
            </tr>
        </table>
    </div>
    <script type="text/javascript">

        function getSelectedTermCondSearch() {

            var hdnSelectedRec = window.opener.document.getElementById("hdnSelectedRecTC");
            hdnSelectedRec.value = "";
            if (GridTermConditionSearch.SelectedRecords.length > 0) {
                for (var i = 0; i < GridTermConditionSearch.SelectedRecords.length; i++) {
                    var record = GridTermConditionSearch.SelectedRecords[i];
                    if (hdnSelectedRec.value != "") hdnSelectedRec.value += ',' + record.ID;
                    if (hdnSelectedRec.value == "") hdnSelectedRec.value = record.ID;
                }
                window.opener.TCSubmit();
                self.close();
            }
            if (GridTermConditionSearch.SelectedRecords.length == 0) { alert('Select atleast one Terms & Condition '); }
        }

    </script>
</asp:Content>

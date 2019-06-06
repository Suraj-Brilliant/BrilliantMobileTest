<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/CRM2.Master" AutoEventWireup="true" CodeBehind="LocationSearch.aspx.cs" 
    Inherits="BrilliantWMS.WMS.LocationSearch" EnableEventValidation="false" Theme="Blue" %>

<%@ Register Assembly="obout_Flyout2_NET" Namespace="OboutInc.Flyout2" TagPrefix="obout" %>
<%@ Register Assembly="obout_Grid_NET" Namespace="Obout.Grid" TagPrefix="obout" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table class="gridFrame" width="100%" style="margin: 3px 3px 3px 3px;">
        <tr>
            <td>
                <table style="width: 100%">
                    <tr>
                        <td style="text-align: left;">
                            <asp:Label ID="lblheader" CssClass="headerText" runat="server" Text="Location List"></asp:Label>
                        </td>
                        <td style="text-align: right;">
                            <table>
                                <tr>
                                    <td>
                                        <input type="text" id="txtLocationSearch" onkeyup="SearchLocation();" style="font-size: 15px; padding: 2px; width: 450px;" />
                                        <asp:HiddenField runat="server" ID="hdnFilterText" />
                                    </td>
                                    <td>
                                        <img src="../App_Themes/Blue/img/Search24.png" onclick="SearchLocation()" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td style="text-align: right;">
                            <input type="button" runat="server" value="Submit" id="btnSubmitLocationSearch1" onclick="selectedRec();" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <obout:Grid ID="grdLocationSearch" runat="server" AutoGenerateColumns="false" AllowFiltering="false"
                    AllowGrouping="true" AllowColumnResizing="true" AllowAddingRecords="false" AllowColumnReordering="true"
                    AllowMultiRecordSelection="false" CallbackMode="true" Width="100%" Serialize="false"
                    PageSize="25" AllowPageSizeSelection="false" AllowManualPaging="true" ShowTotalNumberOfPages="false"
                    KeepSelectedRecords="false">
                    <Columns>
                         <obout:Column DataField="ID" Visible="false">
                        </obout:Column>
                        <obout:Column DataField="Code" HeaderText="Code" Align="left" HeaderAlign="left"
                            Width="15%" AllowFilter="false" ParseHTML="true">
                        </obout:Column>
                        <obout:Column DataField="Capacity" HeaderText="Capacity" Align="center" HeaderAlign="center"
                            Width="15%" AllowFilter="false" ParseHTML="true">
                        </obout:Column>
                        <obout:Column DataField="AvailableBalance" HeaderText="Available Balance" Align="center" HeaderAlign="center"
                            Width="15%" AllowFilter="false" ParseHTML="true">
                        </obout:Column>
                        <obout:Column DataField="SortCode" Visible="false"></obout:Column>
                    </Columns>
                </obout:Grid>
            </td>
        </tr>
    </table>
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
            }
        }
        function selectedRec() {
            var hdnLocationSearchSelectedRec = window.opener.document.getElementById("hdnLocationSearchSelectedRec");
            hdnLocationSearchSelectedRec.value = "";
            var hdnLocationSearchCapacity = window.opener.document.getElementById("hdnLocationSearchCapacity");
            hdnLocationSearchCapacity.value = "";
            var hdnLoactionSearchAvlblc = window.opener.document.getElementById("hdnLoactionSearchAvlblc");
            hdnLoactionSearchAvlblc.value = "";
            var hdnLocationID = window.opener.document.getElementById("hdnLocationID");
            hdnLocationID.value = "";
            var hdnLocSortCode = window.opener.document.getElementById("hdnLocSortCode");
            hdnLocSortCode.value = "";

            if (grdLocationSearch.SelectedRecords.length > 0) {
                for (var i = 0; i < grdLocationSearch.SelectedRecords.length; i++) {
                    var record = grdLocationSearch.SelectedRecords[i];
                    if (hdnLocationSearchSelectedRec.value != "") hdnLocationSearchSelectedRec.value += ',' + record.Code;
                    if (hdnLocationSearchSelectedRec.value == "") hdnLocationSearchSelectedRec.value = record.Code;

                    if (hdnLocationSearchCapacity.value != "") hdnLocationSearchCapacity.value += ',' + record.Capacity;
                    if (hdnLocationSearchCapacity.value == "") hdnLocationSearchCapacity.value = record.Capacity;

                    if (hdnLoactionSearchAvlblc.value != "") hdnLoactionSearchAvlblc.value += ',' + record.AvailableBalance;
                    if (hdnLoactionSearchAvlblc.value == "") hdnLoactionSearchAvlblc.value = record.AvailableBalance;

                    if (hdnLocationID.value != "") hdnLocationID.value += ',' + record.ID;
                    if (hdnLocationID.value == "") hdnLocationID.value = record.ID;

                    if (hdnLocSortCode.value != "") hdnLocSortCode.value += ',' + record.SortCode;
                    if (hdnLocSortCode.value == "") hdnLocSortCode.value = record.SortCode;
                }
                var SelCon = hdnLocationSearchSelectedRec.value;
                var count = (SelCon.match(/,/g) || []).length;
                console.log(count);
                if (count >= 1) {
                    showAlert("Select Only One Location", "Error", "#");
                } else {
                    window.opener.AfterLocationSelected();
                    self.close();
                }
            }
            if (grdLocationSearch.SelectedRecords.length == 0) {
                alert("Select atleast one Location");
            }
        }
        var searchTimeout = null;
        function SearchLocation() {
            var hdnFilterText = document.getElementById("<%= hdnFilterText.ClientID %>");
            hdnFilterText.value = document.getElementById("txtLocationSearch").value;
            if (searchTimeout != null) {
                window.clearTimeout(searchTimeout);
            }
            searchTimeout = window.setTimeout(performSearch, 700);
        }

        function performSearch() {
            grdLocationSearch.refresh();
            searchTimeout = null;
            return false;
        }
    </script>
</asp:Content>

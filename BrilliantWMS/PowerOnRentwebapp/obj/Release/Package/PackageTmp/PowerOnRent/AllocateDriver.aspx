<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/CRM2.Master" AutoEventWireup="true" CodeBehind="AllocateDriver.aspx.cs" Theme="Blue"
    Inherits="BrilliantWMS.PowerOnRent.AllocateDriver" EnableEventValidation="false" %>

<%@ Register Assembly="obout_Flyout2_NET" Namespace="OboutInc.Flyout2" TagPrefix="obout" %>
<%@ Register Assembly="obout_Grid_NET" Namespace="Obout.Grid" TagPrefix="obout" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   <script src="../MasterPage/JavaScripts/jquery-3.1.1.min.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server"> 
    <table class="gridFrame" width="100%" style="margin: 3px 3px 3px 3px;">
        <tr>
            <td>
                <table style="width: 80%">
                    <tr>
                        <td>
                            <asp:Label ID="lblTruckDetail" CssClass="headerText" runat="server" Text="Truck Details"></asp:Label>
                        </td>
                        <td style="text-align: left;">
                            <table>
                                <tr>
                                    <td>
                                        <input type="text" id="txtTruckDetails" style="font-size: 15px; padding: 2px; width: 450px;" />
                                        <asp:HiddenField runat="server" ID="HiddenField1" />
                                    </td>
                                    <td>                                        
                                    </td>
                                </tr>
                            </table>                           
                        </td>
                        <td style="text-align: right;"></td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <table style="width: 100%">
                    <tr>
                        <td style="text-align: left;">
                            <asp:Label ID="lblheader" CssClass="headerText" runat="server" Text="Driver List "></asp:Label>
                        </td>
                        <td style="text-align: right;">
                            <table>
                                <tr>
                                    <td>
                                        <input type="text" id="txtDriverSearch" onkeyup="SearchDriver();" style="font-size: 15px; padding: 2px; width: 450px;" />
                                        <asp:HiddenField runat="server" ID="hdnFilterText" />
                                    </td>
                                    <td>
                                        <img src="../App_Themes/Blue/img/Search24.png" onclick="SearchDriver()" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td style="text-align: right;">
                           <%-- <input type="button" runat="server" value="Allocate" id="btnAllocateDriver" onclick="selectedRec();" />--%>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <obout:Grid ID="GVDriver" runat="server" AutoGenerateColumns="false" AllowFiltering="false"
                    AllowGrouping="true" AllowColumnResizing="true" AllowAddingRecords="false" AllowColumnReordering="true"
                    AllowMultiRecordSelection="false" CallbackMode="true" Width="100%" Serialize="false"
                    PageSize="25" AllowPageSizeSelection="false" AllowManualPaging="true" ShowTotalNumberOfPages="false"
                    KeepSelectedRecords="false">
                    <ClientSideEvents ExposeSender="true" />
                    <Columns>
                        <obout:Column DataField="ID" Visible="false">
                        </obout:Column>
                        <obout:Column DataField="Name" HeaderText="Driver Name" Width="10%" Align="center" HeaderAlign="center" Wrap="true"></obout:Column>
                        <obout:Column DataField="MobileNo" HeaderText="Contact No" Width="10%" Align="center" HeaderAlign="center" Wrap="true"></obout:Column>
                        <obout:Column DataField="EmailID" HeaderText="Email Id" Width="10%" Align="center" HeaderAlign="center" Wrap="true"></obout:Column>
                    </Columns>
                </obout:Grid>
            </td>
        </tr>
        <tr>
            <td>
                <table style="width: 100%">
                    <tr>
                        <td style="text-align: left;"></td>
                        <td style="text-align: right;">
                            <input type="button" runat="server" value="Allocate" id="btnAllocateDriver" onclick="selectedRec();" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <asp:HiddenField ID="hndSelectedRec" runat="server" ClientIDMode="Static" />
  
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
            var hndSelectedRec = document.getElementById("hndSelectedRec");
            var TruckDetails = $('#txtTruckDetails').val();
            hndSelectedRec.value = "";
            if (TruckDetails == "") {
                showAlert('Enter Truck Details', 'Error', '#'); TruckDetails.focus();
            }else if (GVDriver.SelectedRecords.length > 0) {
                for (var i = 0; i < GVDriver.SelectedRecords.length; i++) {
                    var record = GVDriver.SelectedRecords[i];
                    if (hndSelectedRec.value != "") hndSelectedRec.value += ',' + record.ID;
                    if (hndSelectedRec.value == "") hndSelectedRec.value = record.ID;
                }
                var TruckDetails = $('#txtTruckDetails').val();
                var SelectedOrders = getParameterByName("OID").toString();
                PageMethods.WMAssignDriver(hndSelectedRec.value, SelectedOrders,TruckDetails, OnSuccessAssignDriver,null)                
                //self.close();
            }
            if (GVDriver.SelectedRecords.length == 0) {
                showAlert('Select One Driver', 'Error', '#');
            }
        }
        function OnSuccessAssignDriver(result) {
            window.opener.AfterAssignDriver();
            self.close();
        }

        var searchTimeout = null;
        function SearchDriver() {
            var hdnFilterText = document.getElementById("<%= hdnFilterText.ClientID %>");
            hdnFilterText.value = document.getElementById("txtDriverSearch").value;
            if (searchTimeout != null) {
                window.clearTimeout(searchTimeout);
            }
            searchTimeout = window.setTimeout(performSearch, 700);
        }
        function performSearch() {
            GVDriver.refresh();
            searchTimeout = null;
            return false;
        }
    </script>
</asp:Content>

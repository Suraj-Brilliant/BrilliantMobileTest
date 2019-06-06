﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProductVendor.aspx.cs" Inherits="BrilliantWMS.Product.ProductVendor" Theme="Blue" %>

<%@ Register Assembly="obout_Grid_NET" Namespace="Obout.Grid" TagPrefix="obout" %>
<script src="../MasterPage/JavaScripts/countries.js" type="text/javascript"></script>
<script src="../MasterPage/JavaScripts/crm.js" type="text/javascript"></script>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
      <form id="form1" runat="server">
    <div>
        <asp:ScriptManager ID="ScriptmanagerVendorCustomer" runat="server" EnablePageMethods="true">
        </asp:ScriptManager>
        <center>
            <table class="gridFrame" width="90%" style="margin: 3px 3px 3px 3px;">
                <tr>
                    <td>
                         <table style="width: 100%">
                    <tr>
                        <td style="text-align: left;">
                           <asp:Label ID="lblheader" CssClass="headerText" runat="server" Text="Vendor List"></asp:Label>
                        </td>
                        <td style="text-align: right;">
                            <table>
                                <tr>
                                    <td>
                                        <input type="text" id="txtProductSearch" onkeyup="SearchProduct();" style="font-size: 15px;
                                            padding: 2px; width: 450px;" />
                                        <asp:HiddenField runat="server" ID="hdnFilterText" />
                                    </td>
                                    <td>
                                        <img src="../App_Themes/Blue/img/Search24.png" onclick="SearchProduct()" />
                                    </td>
                                    <td style="text-align: right;">        
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td style="text-align: right;">
                            <input type="button" runat="server" value="Submit" id="btnSubmitProductSearch1" onclick="selectedRec();" />
                        </td>
                    </tr>
                </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <obout:Grid ID="GVVendor" runat="server" AutoGenerateColumns="false" AllowFiltering="false"
                            AllowGrouping="true" AllowColumnResizing="true" AllowAddingRecords="false" AllowColumnReordering="true"
                            AllowMultiRecordSelection="true" CallbackMode="true" Width="100%" Serialize="false"
                            AllowPageSizeSelection="false" AllowManualPaging="true" ShowTotalNumberOfPages="false"
                            KeepSelectedRecords="false" PageSize="10">
                            <Columns>
                                <obout:Column DataField="ID" HeaderText="ID" Visible="false">
                                </obout:Column>
                                <obout:Column DataField="Name" HeaderText="Vendor Name" HeaderAlign="left"
                                    Align="left" Width="10%">
                                </obout:Column>
                                <obout:Column DataField="Code" HeaderText="Vendor Code" HeaderAlign="left"
                                    Align="left" Width="10%">
                                </obout:Column>
                                <obout:Column DataField="Value" HeaderText="Vendor Type" HeaderAlign="left" Align="left"
                                    Width="10%">
                                </obout:Column>
                            </Columns>
                        </obout:Grid>
                    </td>
                </tr>
            </table>
            <asp:HiddenField ID="hdncustvender" runat="server" ClientIDMode="Static" />
             <asp:HiddenField runat="server" ID="hdnskuid" ClientIDMode="Static" />
        </center>
    </div>
    </form>
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
        }                                                                                            //for (var i = 0; i < GridProductSearch.SelectedRecords.length; i++)
        function selectedRec() {


            var locationinfo = new Object();
            var Selectedrecord = "";
            var hdnProductSearchSelectedRec = window.opener.document.getElementById("hdnlocationName");
            var hdnproductsearchId = window.opener.document.getElementById("hdnlocationId");
            if (GVVendor.SelectedRecords.length > 0) {
                for (var i = 0; i < GVVendor.SelectedRecords.length; i++) {
                    var record = GVVendor.SelectedRecords[i];
                    if (Selectedrecord.value != "") Selectedrecord += ',' + record.ID;
                    if (Selectedrecord.value == "") {
                        Selectedrecord.value = record.LocationCode;
                        Selectedrecord.value = record.ID;
                    }
                }


                var SelectedContactIds = "";
                locationinfo.SelectedContactIds = Selectedrecord;
                var SKUId = document.getElementById("<%=hdnskuid.ClientID %>").value;
                PageMethods.PMSaveLocation(Selectedrecord, SKUId, SubmitLocation_onSuccess, SubmitLocation_onFail)

                //window.opener.AfterApproverSelect(hdnApproverSearchSelectedRec.value, hdnApproversearchId.value);
                //self.close();
            }
            if (GridProductSearch.SelectedRecords.length == 0) {
                alert("Please Select Vendor");
            }
        }

        function SubmitLocation_onSuccess() {
            //LoadingOff();
            window.opener.grvVendorDetails.refresh();
            self.close();
        }

        function SubmitLocation_onFail() { alert("error"); }

        var searchTimeout = null;
        function SearchProduct() {
            var hdnFilterText = document.getElementById("<%= hdnFilterText.ClientID %>");
            hdnFilterText.value = document.getElementById("txtProductSearch").value;
            if (searchTimeout != null) {
                window.clearTimeout(searchTimeout);
            }
            searchTimeout = window.setTimeout(performSearch, 700);
        }

        function performSearch() {
            GVVendor.refresh();
            searchTimeout = null;
            return false;
        }

    </script>
</body>
</html>

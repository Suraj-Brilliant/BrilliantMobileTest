<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/CRM2.Master" AutoEventWireup="true" Theme="Blue" CodeBehind="PaymentMethod.aspx.cs" 
 Inherits="BrilliantWMS.Account.PaymentMethod" %>
<%@ Register Assembly="obout_Grid_NET" Namespace="Obout.Grid" TagPrefix="obout" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table class="gridFrame" width="500px" style="margin: 3px 3px 3px 3px;">
        <tr>
            <td>
                <table style="width: 100%">
                    <tr>
                        <td style="text-align: left;">
                           <asp:Label ID="lblheader" CssClass="headerText" runat="server" Text="SKU List"></asp:Label>
                        </td>
                        <td style="text-align: right;">
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
                <obout:Grid ID="GridProductSearch" runat="server" AutoGenerateColumns="false" AllowFiltering="false" AllowMultiRecordSelection="false"
                    AllowGrouping="true" AllowColumnResizing="true" AllowAddingRecords="false" AllowColumnReordering="true"
                    CallbackMode="true" Width="100%" Serialize="false"
                    PageSize="25" AllowPageSizeSelection="false" AllowManualPaging="true" ShowTotalNumberOfPages="false"
                    KeepSelectedRecords="false">
                    <ClientSideEvents ExposeSender="true" />
                    <Columns>
                       <obout:Column DataField="ID" Visible="false">
                        </obout:Column>
                        <obout:Column DataField="MethodName" HeaderText="Method Name" Align="left" HeaderAlign="left"
                            Width="15%" AllowFilter="false" ParseHTML="true">
                        </obout:Column>
                        <obout:Column DataField="Sequence" HeaderText="Sequence" Align="left" HeaderAlign="left"
                            Width="10%" AllowFilter="false" ParseHTML="true">
                        </obout:Column>
                     </Columns>
                </obout:Grid>
            </td>
        </tr>
        <tr>
            <td>
                <table style="width: 100%">
                    <tr>
                        <td style="text-align: left;">
                        </td>
                        <td style="text-align: right;">
                            <input type="button" runat="server" value="Submit" id="btnSubmitProductSearch2" onclick="selectedRec();" />
                        </td>
                    </tr>
                </table>
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
            var AddressInfo = new Object();
            var Selectedrecord = "";
              if (GridProductSearch.SelectedRecords.length > 0) {
                for (var i = 0; i < GridProductSearch.SelectedRecords.length; i++) {
                    var record = GridProductSearch.SelectedRecords[i];
                    if (Selectedrecord.value != "") Selectedrecord += ',' + record.ID;
                    if (Selectedrecord.value == "") {
                        Selectedrecord.value = record.ProductCode;
                        Selectedrecord.value = record.ID;
                    }
                }
                // AddressInfo.SelectedIds = hdnProductSearchSelectedRec.value;
                var SelectedContactIds = "";
                AddressInfo.SelectedContactIds = Selectedrecord;
                PageMethods.PMSaveContactD(Selectedrecord, SubmitAddress_onSuccess, SubmitAddress_onFail)
                //                window.opener.AfterProductSelected(hdnProductSearchSelectedRec.value, hdnproductsearchId.value);
                //                self.close();
            }
            if (GridProductSearch.SelectedRecords.length == 0) {
                alert("Select product");
            }
        }

        function SubmitAddress_onSuccess() {
            //LoadingOff();
            window.opener.GridpayMethod.refresh();
            self.close();
        }

        function SubmitAddress_onFail() { alert("error"); }

    </script>
</asp:Content>

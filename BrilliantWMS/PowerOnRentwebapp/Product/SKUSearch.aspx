<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/CRM2.Master" AutoEventWireup="true" CodeBehind="SKUSearch.aspx.cs" Theme="Blue" Inherits="BrilliantWMS.Product.SKUSearch" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%@ Register Assembly="obout_Grid_NET" Namespace="Obout.Grid" TagPrefix="obout" %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <table class="gridFrame" width="1050px" style="margin: 3px 3px 3px 3px;">
        <tr>
            <td>
                <table style="width: 100%">
                    <tr>
                        <td style="text-align: left;">
                           <asp:Label ID="lblheader" CssClass="headerText" runat="server" Text="SKU List"></asp:Label>
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
                                    <%-- <input type="checkbox" id="chkWithBOM"  /> --%><%--onclick="SelectAllEngine(this);"--%>
                                     <%-- <asp:Label ID="lblwithbom" CssClass="headerText" runat="server" Text="With BOM"></asp:Label>    --%>                              
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td style="text-align: right;">
                           <%-- <input type="button" runat="server" value="Submit" id="btnSubmitProductSearch1" onclick="selectedRec();" />--%>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <obout:Grid ID="GridProductSearch" runat="server" AutoGenerateColumns="false" AllowFiltering="false"
                    AllowGrouping="true" AllowColumnResizing="true" AllowAddingRecords="false" AllowColumnReordering="true"
                    AllowMultiRecordSelection="false" CallbackMode="true" Width="100%" Serialize="false"
                    PageSize="25" AllowPageSizeSelection="false" AllowManualPaging="true" ShowTotalNumberOfPages="false"
                    KeepSelectedRecords="false">
                    <ClientSideEvents ExposeSender="true" />
                    <Columns>
                        <%-- <obout:CheckBoxSelectColumn ShowHeaderCheckBox="true" ControlType="Standard" Align="center"
                            HeaderAlign="center" Width="6%" ID="gvSelect" runat="server" AllowFilter="false"
                            ParseHTML="true">
                        </obout:CheckBoxSelectColumn>--%>
                        <obout:Column DataField="ID" Visible="false">
                        </obout:Column>
                        <obout:Column DataField="ProductType" Visible="false" HeaderText="Type" Width="0%"
                            AllowFilter="false" ParseHTML="true">
                        </obout:Column>
                        <obout:Column DataField="ProductCode" HeaderText="SKU Code" Align="left" HeaderAlign="left"
                            Width="13%" AllowFilter="false" ParseHTML="true">
                        </obout:Column>
                        <obout:Column DataField="Name" HeaderText="SKU Name" Align="left" HeaderAlign="left"
                            Width="16%" AllowFilter="false" ParseHTML="true">
                        </obout:Column>
                        <obout:Column DataField="Description" HeaderText="Description" Align="left" HeaderAlign="left"
                            Width="20%" AllowFilter="false" ParseHTML="true">
                        </obout:Column>
                       <obout:Column DataField="Company" HeaderText="Company" Align="left" HeaderAlign="left" Width="10%" ParseHTML="true">
                        </obout:Column>
                        <obout:Column DataField="Customer" HeaderText="Customer" Align="right" HeaderAlign="right"
                            Width="10%" ParseHTML="true">
                        </obout:Column>
                        <obout:Column DataField="WarehouseName" Visible="true" HeaderText="Warehouse" Width="13%"
                            AllowFilter="false" ParseHTML="true">
                        </obout:Column>
                        <obout:Column DataField="LocationCode" Visible="true" HeaderText="Location" Width="13%"
                            AllowFilter="false" ParseHTML="true">
                        </obout:Column>
                       <%-- <obout:Column DataField="ProductCategory" Visible="true" HeaderText="Category" Width="13%"
                            AllowFilter="false" ParseHTML="true">
                        </obout:Column>
                        <obout:Column DataField="ProductSubCategory" Visible="true" HeaderText="Sub Category"
                            Width="15%" AllowFilter="false" ParseHTML="true">
                        </obout:Column>--%>
                    </Columns>
                    <Templates>
                        <obout:GridTemplate runat="server" ID="TemplateFieldAmount">
                            <Template>
                                <span style="margin-right: 7px;">
                                    <%# Container.Value %></span>
                            </Template>
                        </obout:GridTemplate>
                    </Templates>
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
                           <%-- <input type="button" runat="server" value="Submit" id="btnSubmitProductSearch2" onclick="selectedRec();" />--%>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <asp:HiddenField ID="hndgrupByGrid" runat="server" />
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
            var hdnProductSearchSelectedRec = window.opener.document.getElementById("hdnProductSearchSelectedRec");
            var hdnproductsearchId = window.opener.document.getElementById("hdnproductsearchId");
            hdnProductSearchSelectedRec.value = "";
            hdnproductsearchId.value = "";
            if (GridProductSearch.SelectedRecords.length > 0) {
                for (var i = 0; i < GridProductSearch.SelectedRecords.length; i++) {
                    var record = GridProductSearch.SelectedRecords[i];
                    if (hdnProductSearchSelectedRec.value != "") hdnProductSearchSelectedRec.value += ',' + record.ID;
                    if (hdnProductSearchSelectedRec.value == "") {
                        hdnProductSearchSelectedRec.value = record.ProductCode;
                        hdnproductsearchId.value = record.ID;
                    }
                }

                window.opener.AfterProductSelected(hdnProductSearchSelectedRec.value, hdnproductsearchId.value);
                self.close();
            }
            if (GridProductSearch.SelectedRecords.length == 0) {
                alert("Select product");
            }
        }

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
            GridProductSearch.refresh();
            searchTimeout = null;
            return false;
        }

    </script>
</asp:Content>

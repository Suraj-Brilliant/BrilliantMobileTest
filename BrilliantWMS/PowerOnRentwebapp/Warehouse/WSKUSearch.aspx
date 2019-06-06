<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/CRM2.Master" AutoEventWireup="true" CodeBehind="WSKUSearch.aspx.cs"
    Inherits="BrilliantWMS.Warehouse.WSKUSearch" Theme="Blue" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
          <%@ Register Assembly="obout_Grid_NET" Namespace="Obout.Grid" TagPrefix="obout" %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <table class="gridFrame" width="1080px" style="margin: 3px 3px 3px 3px;">
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
                            <input type="button" runat="server" value="Submit" id="btnSubmitProductSearch1" onclick="selectedRec();" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <obout:Grid ID="GridProductSearch" runat="server" AutoGenerateColumns="false" AllowFiltering="false"
                    AllowGrouping="true" AllowColumnResizing="true" AllowAddingRecords="false" AllowColumnReordering="true"
                    AllowMultiRecordSelection="true" CallbackMode="true" Width="100%" Serialize="false"
                    PageSize="15" AllowPageSizeSelection="false" AllowManualPaging="true" ShowTotalNumberOfPages="false"
                    KeepSelectedRecords="false">
                    <ClientSideEvents ExposeSender="true" />
                      <ScrollingSettings ScrollHeight="450" />
                    <Columns>
                        <obout:Column DataField="ID" Visible="false">
                        </obout:Column>
                        <obout:Column DataField="ProductCode" HeaderText="Product Code" Align="left" HeaderAlign="left"
                            Width="20%" AllowFilter="false" ParseHTML="true">
                        </obout:Column>
                        <obout:Column DataField="Name" HeaderText="Product Name" Align="left" HeaderAlign="left"
                            Width="30%" AllowFilter="false" ParseHTML="true">
                        </obout:Column>
                        <obout:Column DataField="Description" HeaderText="Description" Align="left" HeaderAlign="left"
                            Width="50%" AllowFilter="false" ParseHTML="true">
                        </obout:Column>
                         <%-- <obout:Column DataField="Path" HeaderText="Image" Align="center" HeaderAlign="center" Visible="false"
                            ItemStyle-Height="30" Width="0%" AllowFilter="false" AllowGroupBy="false">
                            <TemplateSettings TemplateId="TemplateShowImage" />
                        </obout:Column>
                        <obout:Column DataField="SkuImage" HeaderText="Sku Image" Width="13%">
                            <TemplateSettings TemplateId="GetSkuImage" />
                        </obout:Column>--%>
                    </Columns>
                    <Templates>
                        <obout:GridTemplate runat="server" ID="GetSkuImage">
                            <Template>
                                <table>
                                    <tr>
                                        <td id="'<%# Container.DataItem["ID"].ToString() + "UpdTask" %>'" onmouseover="openFlyoutImage('<%# Container.DataItem["ID"].ToString()%>', this)">
                                            <a>
                                                <asp:Image ID="ImageDisplay" runat="server" ImageUrl='<%# "../Product/ShowImage.ashx?ID="+ (Container.DataItem["ID"].ToString()) %>' Height="50px" Width="50px" Style="border: solid 2px gray; cursor: pointer;" />
                                            </a>
                                        </td>
                                    </tr>
                                </table>

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
                            <input type="button" runat="server" value="Submit" id="btnSubmitProductSearch2" onclick="selectedRec();" />
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
            var hdnproductCode = window.opener.document.getElementById("hdnproductCode");
            var hdnProductID = window.opener.document.getElementById("hdnProductID");
            hdnproductCode.value = "";
            hdnProductID.value = "";
            if (GridProductSearch.SelectedRecords.length > 0) {
                for (var i = 0; i < GridProductSearch.SelectedRecords.length; i++) {
                    var record = GridProductSearch.SelectedRecords[i];
                    if (hdnProductID.value != "") hdnProductID.value += ',' + record.ID;
                    if (hdnProductID.value == "") {
                        hdnproductCode.value = record.ProductCode;
                        hdnProductID.value = record.ID;
                    }
                }

                window.opener.AfterSKUSelected(hdnproductCode.value, hdnProductID.value);
                self.close();
            }
            if (GridProductSearch.SelectedRecords.length == 0) {
                alert("Select SKU");
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

<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/CRM2.Master" AutoEventWireup="true" CodeBehind="ApproverList.aspx.cs" Theme="Blue" Inherits="BrilliantWMS.Account.ApproverList" %>
<%@ Register Assembly="obout_Grid_NET" Namespace="Obout.Grid" TagPrefix="obout" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <table class="gridFrame" width="800px" style="margin: 3px 3px 3px 3px;">
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
                        <obout:Column DataField="AName" HeaderText="Name" Align="left" HeaderAlign="left"
                            Width="15%" AllowFilter="false" ParseHTML="true">
                        </obout:Column>
                        <obout:Column DataField="EmailID" HeaderText="Email ID" Align="left" HeaderAlign="left"
                            Width="15%" AllowFilter="false" ParseHTML="true">
                        </obout:Column>
                        <obout:Column DataField="Territory" HeaderText="Department" Align="left" HeaderAlign="left"
                            Width="18%" AllowFilter="false" ParseHTML="true">
                        </obout:Column>
                        <obout:Column DataField="Name" HeaderText="Company" Align="left" HeaderAlign="left"
                            Width="18%" AllowFilter="false" ParseHTML="true">
                        </obout:Column>
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
        }                                                                                                 // for (var i = 0; i < GridProductSearch.SelectedRecords.length; i++)
        function selectedRec() {
            var hdnApproverSearchSelectedRec = window.opener.document.getElementById("hdnapproverSearchSelectedRec");
            var hdnApproversearchId = window.opener.document.getElementById("hdnapproversearchId");
            hdnApproverSearchSelectedRec.value = "";
            hdnApproversearchId.value = "";
            if (GridProductSearch.SelectedRecords.length > 0) {
                for (var i = 0; i < 1; i++) {
                    var record = GridProductSearch.SelectedRecords[i];
                    if (hdnApproverSearchSelectedRec.value != "") hdnApproverSearchSelectedRec.value += ',' + record.ID;
                    if (hdnApproverSearchSelectedRec.value == "") {
                        hdnApproverSearchSelectedRec.value = record.AName;
                        hdnApproversearchId.value = record.ID;
                    }
                }

                window.opener.AfterProductSelected(hdnApproverSearchSelectedRec.value, hdnApproversearchId.value);
                self.close();
            }
            if (GridProductSearch.SelectedRecords.length == 0) {
                alert("Select Approver");
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

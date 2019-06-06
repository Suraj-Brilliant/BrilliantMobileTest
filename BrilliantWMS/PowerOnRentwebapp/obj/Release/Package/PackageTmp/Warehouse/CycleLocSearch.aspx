<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/CRM2.Master" AutoEventWireup="true" CodeBehind="CycleLocSearch.aspx.cs"
    Inherits="BrilliantWMS.Warehouse.CycleLocSearch" EnableEventValidation="false" Theme="Blue" %>

<%@ Register Assembly="obout_Flyout2_NET" Namespace="OboutInc.Flyout2" TagPrefix="obout" %>
<%@ Register Assembly="obout_Grid_NET" Namespace="Obout.Grid" TagPrefix="obout" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
       <table class="gridFrame" width="100%" style="margin: 3px 3px 3px 3px;">
         <asp:HiddenField ID="hdnwarehouseID" runat="server" ClientIDMode="Static" />
         <asp:HiddenField ID="hdnobject" runat="server" ClientIDMode="Static" />
         <asp:HiddenField ID="hdnSessionID" runat="server" ClientIDMode="Static" />
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
                                        <input type="text" id="txtProductSearch" onkeyup="SearchProduct();" style="font-size: 15px; padding: 2px; width: 450px;"  />
                                        <asp:HiddenField runat="server" ID="hdnFilterText" />
                                    </td>
                                    <td>
                                        <img src="../App_Themes/Blue/img/Search24.png" onclick="SearchProduct()" />
                                    </td>
                                    <%--<td style="text-align: right;">
                                        <input type="checkbox" id="chkWithBOM" />                                      
                                        <asp:Label ID="lblwithbom" CssClass="headerText" runat="server" Text="With BOM"></asp:Label>
                                    </td>--%>
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
                <obout:Grid ID="GridProductSearch" runat="server" AutoGenerateColumns="false" AllowFiltering="true"
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
                        <obout:Column DataField="ID" HeaderText="ID"  Visible="false" Width="0%">
                        </obout:Column>
                        <obout:Column DataField="locationCode"  HeaderText="location Code" Width="10%" >
                        </obout:Column>
                         <obout:Column DataField="LocationType" Visible="true" HeaderText="Location Type" Width="8%">
                        </obout:Column>
                        <obout:Column DataField="Capacity" Visible="true" HeaderText="Capacity" Width="8%">
                        </obout:Column>
                        <obout:Column DataField="Shelf" HeaderText="Shelf" Align="left" HeaderAlign="left" Width="10%" >
                        </obout:Column>
                        <obout:Column DataField="Section" HeaderText="Section" Align="left" HeaderAlign="left" Width="10%" >
                        </obout:Column>
                        <obout:Column DataField="Passage" HeaderText="Passage" Align="left" HeaderAlign="left" Width="8%" >
                        </obout:Column>
                       <obout:Column DataField="Floar" HeaderText="Floar" Align="left" HeaderAlign="left" Width="8%">
                        </obout:Column>
                        <obout:Column DataField="Building" HeaderText="Building" Align="left" HeaderAlign="left" Width="8%" >
                        </obout:Column>
                         <obout:Column DataField="WarehouseName" HeaderText="Warehouse Name" Align="left" HeaderAlign="left" Width="10%" >
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
                <%--ToolTip='<%# (Container.Value) %>'--%>
            </td>
        </tr>
        <tr>
            <td>
                <table style="width: 100%">
                    <tr>
                        <td style="text-align: left;"></td>
                        <td style="text-align: right;">
                            <input type="button" runat="server" value="Submit" id="btnSubmitProductSearch2" onclick="selectedRec();" visible="false" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <input type="button" id="btnShowImg" style="display: none" />
    <obout:Flyout ID="FlyoutImg" runat="server" AttachTo="btnShowImg" OpenEvent="NONE" CloseEvent="ONMOUSEOUT" Position="MIDDLE_LEFT">
        <span id="spanImg"></span>
        <table class="tableForm" style="background-color: White; width: 370px">
            <tr>
                <td style="text-align: left" id="tdImg">
                    <asp:Image ID="ImageBig" runat="server" Height="350px" Width="350px" Style="border: solid 2px gray; cursor: pointer;" />
                </td>
            </tr>
        </table>
    </obout:Flyout>
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
            var hdnProductSearchSelectedRec;
            var SessionID = document.getElementById("<%=hdnSessionID.ClientID%>")
            var Selectedrecord = "";
            
            if (GridProductSearch.SelectedRecords.length > 0) {
                for (var i = 0; i < GridProductSearch.SelectedRecords.length; i++) {
                    var record = GridProductSearch.SelectedRecords[i];
                    if (Selectedrecord!= "") Selectedrecord += ',' + record.ID;
                    if (Selectedrecord == "") Selectedrecord = record.ID;
                }
                var Object = "Location";
                PageMethods.SaveCycleProductIds(Selectedrecord,Object,SessionID.value, SubmitAddress_onSuccess, SubmitAddress_onFail)

                //window.opener.AfterProductSelected();
                // self.close();
            }
            if (GridProductSearch.SelectedRecords.length == 0) {
                alert("Select atleast one product");
            }
        }
        
        function SubmitAddress_onSuccess() {
            window.opener.grdlocation.refresh();
            self.close();
        }

        function SubmitAddress_onFail() { alert("error"); }




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

       

        function openFlyoutImage(img,invoker){
            <%=FlyoutImg.getClientID()%>.AttachTo(invoker.id);          
            var imgg=document.getElementById('<%=ImageBig.ClientID%>');
            imgg.src="../Product/ShowImage.ashx?ID="+ img +"";            
            <%=FlyoutImg.getClientID()%>.Open();
        }

       
    </script>
</asp:Content>

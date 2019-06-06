<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/CRM2.Master" AutoEventWireup="true" CodeBehind="DocumentSearch.aspx.cs"
    Inherits="BrilliantWMS.Document.DocumentSearch" Theme="Blue" %>

<%@ Register Assembly="obout_Flyout2_NET" Namespace="OboutInc.Flyout2" TagPrefix="obout" %>
<%@ Register Assembly="obout_Grid_NET" Namespace="Obout.Grid" TagPrefix="obout" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
       <table class="gridFrame" width="100%" style="margin: 3px 3px 3px 3px;">
         <asp:HiddenField ID="hdnwarehouseID" runat="server" ClientIDMode="Static" />
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
                                        <input type="text" id="txtProductSearch" onkeyup="SearchProduct();" style="font-size: 15px; padding: 2px; width: 450px;" />
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
                            <input type="button" runat="server" value="Submit" id="btnSubmitProductSearch1" onclick="selectedRec();" visible="false" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                 <obout:Grid ID="GvDocument" runat="server" AutoGenerateColumns="false" AllowFiltering="true"
                    AllowGrouping="true" AllowColumnResizing="true" AllowAddingRecords="false" AllowColumnReordering="true"
                    AllowPageSizeSelection="true" AllowPaging="true" AllowRecordSelection="false"
                    AllowSorting="true" Width="100%" >
                    <Columns>
                        <obout:Column DataField="ID" HeaderText="ID" Visible="false" Width="0%">
                        </obout:Column>
                        <obout:Column ID="Delete" DataField="DeleteAccess" HeaderText="Delete" Width="0%" 
                            Align="center" HeaderAlign="center">
                            <TemplateSettings TemplateId="GvTempDelete" />
                        </obout:Column>
                        <obout:Column DataField="Sequence" Width="0%" HeaderAlign="center" Align="center"
                            Visible="false">
                        </obout:Column>
                        <obout:Column DataField="ObjectName" HeaderText="Object Name" Width="10%" Align="center"
                            HeaderAlign="center">
                        </obout:Column>
                        <obout:Column DataField="DocumentName" HeaderText="Document Title" Width="20%" Wrap="true" HeaderAlign="center" Align="center">
                        </obout:Column>
                        <obout:Column DataField="Description" HeaderText="Description" Width="25%" Wrap="true">
                        </obout:Column>
                        <obout:Column DataField="Keywords" HeaderText="Key Words" Width="25%" Wrap="true">
                        </obout:Column>
                        <obout:Column DataField="DocumentType" HeaderText="Document Type" Width="10%" Align="center"
                            HeaderAlign="center">
                        </obout:Column>
                        <obout:Column DataField="FileType" HeaderText="File Type" Width="10%" Align="center"
                            HeaderAlign="center">
                        </obout:Column>
                        <obout:Column ID="Download" DataField="DowloadAccess" HeaderText="Download" Width="10%"
                            HeaderAlign="center" Align="center">
                            <TemplateSettings TemplateId="GvTempDownload" />
                        </obout:Column>
                        <obout:Column ID="DocumentDownloadPath" DataField="DocumentDownloadPath" HeaderText="Delete"
                            Visible="false" Width="0%">
                        </obout:Column>
                    </Columns>
                    <Templates>
                        <obout:GridTemplate ID="GvTempDownload">
                            <Template>
                                <a href='<%# (Container.Value == "true" ? (Container.DataItem["DocumentDownloadPath"]) :'#') %>'
                                    target="_blank">
                                    <img src='<%# (Container.Value == "true" ? "../CommonControls/HomeSetupImg/download.png" : "../CommonControls/HomeSetupImg/accessdDenied.jpg") %>' />
                                </a>
                            </Template>
                        </obout:GridTemplate>
                    </Templates>
                    <Templates>
                        <obout:GridTemplate ID="GvTempDelete">
                            <Template>
                                <img src="../App_Themes/Blue/img/Delete16.png" onclick="deleteDocument('<%# Container.DataItem["Sequence"].ToString() %>')"
                                    style="cursor: pointer;" />
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
                        <td style="text-align: left;"></td>
                        <td style="text-align: right;">
                            <input type="button" runat="server" value="Submit" id="btnSubmitProductSearch2" onclick="selectedRec();" visible="false" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <input type="button" id="btnShowDescripion" style="display: none" />
    <obout:Flyout ID="FlyoutDescription" runat="server" AttachTo="btnShowDescripion" OpenEvent="NONE" CloseEvent="ONMOUSEOUT" Position="TOP_RIGHT">
        <span id="spanDescription"></span>
        <table class="tableForm" style="background-color: White; width: 300px">
            <tr>
                <td style="text-align: left" id="tdRemark">N/A
                </td>
            </tr>
        </table>
    </obout:Flyout>
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
            var hdnProductSearchSelectedRec = window.opener.document.getElementById("hdnProductSearchSelectedRec");
            hdnProductSearchSelectedRec.value = "";
            if (GridProductSearch.SelectedRecords.length > 0) {
                for (var i = 0; i < GridProductSearch.SelectedRecords.length; i++) {
                    var record = GridProductSearch.SelectedRecords[i];
                    if (hdnProductSearchSelectedRec.value != "") hdnProductSearchSelectedRec.value += ',' + record.ID;
                    if (hdnProductSearchSelectedRec.value == "") hdnProductSearchSelectedRec.value = record.ID;
                }
                window.opener.AfterProductSelected();
                self.close();
            }
            if (GridProductSearch.SelectedRecords.length == 0) {
                alert("Select atleast one product");
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
            GvDocument.refresh();
            searchTimeout = null;
            return false;
        }

        function openFlyoutDescription(description,invoker) {
            <%=FlyoutDescription.getClientID()%>.AttachTo(invoker.id);
            document.getElementById("tdRemark").innerHTML=description;
            <%=FlyoutDescription.getClientID()%>.Open();
        }

        function openFlyoutImage(img,invoker){
            <%=FlyoutImg.getClientID()%>.AttachTo(invoker.id);          
            var imgg=document.getElementById('<%=ImageBig.ClientID%>');
            imgg.src="../Product/ShowImage.ashx?ID="+ img +"";            
            <%=FlyoutImg.getClientID()%>.Open();
        }

   
    </script>
</asp:Content>

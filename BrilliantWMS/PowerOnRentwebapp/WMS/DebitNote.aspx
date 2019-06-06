<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/CRM2.Master" AutoEventWireup="true" CodeBehind="DebitNote.aspx.cs"
    Inherits="BrilliantWMS.WMS.DebitNote" EnableEventValidation="false" Theme="Blue" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="obout_Interface" Namespace="Obout.Interface" TagPrefix="obout" %>
<%@ Register Assembly="obout_Grid_NET" Namespace="Obout.Grid" TagPrefix="obout" %>
<%@ Register Src="../CommonControls/UCFormHeader.ascx" TagName="UCFormHeader" TagPrefix="uc1" %>
<%@ Register Src="../CommonControls/UCToolbar.ascx" TagName="UCToolbar" TagPrefix="uc2" %>
<%@ Register Src="../CommonControls/UC_Date.ascx" TagName="UC_Date" TagPrefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <center>
        <asp:UpdateProgress ID="UpdateProgress_DebitNote" runat="server" AssociatedUpdatePanelID="updPnl_DebitNote">
            <ProgressTemplate>
                <center>
                    <div class="modal">
                        <img src="../App_Themes/Blue/img/ajax-loader.gif" style="top: 50%;" />
                    </div>
                </center>
            </ProgressTemplate>
        </asp:UpdateProgress>
        <asp:UpdatePanel ID="updPnl_DebitNote" runat="server">
            <ContentTemplate>
                <table class="tableForm" id="tblDebitNote" runat="server">
                    <tr>
                        <td>
                            <asp:Label Id="lblVendorName" runat="server" />
                            :
                        </td>
                        <td style="text-align: left; font-weight: bold;">
                            <asp:Label Id="lblVndrName" runat="server" />
                        </td>
                        <td>
                            <asp:Label ID="lblPurchaseOrderNo" runat="server" ></asp:Label>:
                        </td>
                        <td style="text-align: left; font-weight: bold;">
                            <asp:Label ID="lblPONO" runat="server"></asp:Label>                            
                        </td>
                        <td>
                            <asp:Label ID="lblPODate" runat="server" ></asp:Label>:
                        </td>
                        <td style="text-align: left; font-weight: bold;">
                            <asp:Label ID="lblPODt" runat="server" ></asp:Label>
                        </td>
                    </tr>
                    <tr>
                       <td>
                            <asp:Label ID="lblDate" runat="server" Text="Debit Note Date"></asp:Label>:
                        </td>
                        <td style="text-align: left">
                            <uc3:UC_Date ID="UCDebitDate" runat="server" />
                        </td>                   
                        <td>
                            <asp:Label ID="lblRemark" runat="server" Text="Remark"></asp:Label>:
                        </td>
                        <td colspan="4" style="text-align: left;">
                            <asp:TextBox ID="txtRemark" runat="server" Style="width:100%;"></asp:TextBox>
                        </td>
                    </tr>                   
                </table>
                <table class="gridFrame" width="90%">
                    <tr>
                        <td>
                            <table style="width: 100%">
                                <tr>
                                    <td style="text-align: left;">
                                        <a id="headerText">
                                            <asp:Label ID="lblSKUList" runat="server" Text="SKU List" /></a>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <obout:Grid ID="grdSkuList" runat="server" AutoGenerateColumns="False" AllowAddingRecords="False"
                                AllowFiltering="True" AllowGrouping="True" AllowMultiRecordSelection="False"
                                AllowRecordSelection="true" Width="100%" AllowColumnResizing="true" AllowColumnReordering="true">
                                <%--OnRebind="gvContactPerson_OnRebind"--%>
                                <Columns>
                                    <obout:Column DataField="ID" HeaderText="ID" Visible="false">
                                    </obout:Column>
                                    <obout:Column DataField="Prod_Code" HeaderText="SKU Code" AllowEdit="false" Width="15%"
                                        HeaderAlign="left">
                                    </obout:Column>
                                    <obout:Column DataField="Prod_Name" HeaderText="Product Name" AllowEdit="false" Width="15%"
                                        HeaderAlign="left">
                                    </obout:Column>
                                    <obout:Column DataField="Prod_Description" HeaderText="Description" AllowEdit="false"
                                        Width="15%" HeaderAlign="left">
                                    </obout:Column>
                                    <obout:Column DataField="UOM" HeaderText="UOM" AllowEdit="false" Width="14%" HeaderAlign="center"
                                        Align="center">
                                    </obout:Column>
                                    <obout:Column DataField="RequestQty" HeaderText="Order Qty" AllowEdit="false" Width="11%"
                                        HeaderAlign="center" Align="center">
                                    </obout:Column>
                                    <obout:Column DataField="GRNQty" HeaderText="Received Qty" AllowEdit="false" Width="11%"
                                        HeaderAlign="center" Align="center">
                                    </obout:Column>
                                    <obout:Column DataField="RemaningQty" HeaderText="Debit Note Qty" AllowEdit="false" Width="11%" Wrap="true"
                                        HeaderAlign="center" Align="center">
                                    </obout:Column>
                                    <obout:Column DataField="Price" HeaderText="Price" AllowEdit="false" Width="11%"
                                        HeaderAlign="center" Align="center">
                                    </obout:Column>
                                    <obout:Column DataField="NewPrice" HeaderText="Total" AllowEdit="false" Width="11%"
                                        HeaderAlign="center" Align="center">
                                    </obout:Column>                                   
                                </Columns>
                                <Templates>
                                    <obout:GridTemplate runat="server" ID="PlainEditTemplate">
                                        <Template>
                                            <div class="divTxtUserQty">
                                                <asp:TextBox ID="txtUsrQty" Width="94%" Style="text-align: right;" runat="server"
                                                    Text="<%# Container.Value %>" onfocus="markAsFocused(this)" onkeydown="AllowDecimal(this,event);"
                                                    onkeypress="AllowDecimal(this,event);" onblur="markAsBlured(this, '<%# Grid1.Columns[Container.ColumnIndex].DataField %>', <%# Container.PageRecordIndex %>);"></asp:TextBox>
                                            </div>
                                        </Template>
                                    </obout:GridTemplate>
                                </Templates>
                            </obout:Grid>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right; background-color: White;">
                            <table style="float: right; text-align: right; border: none;" class="tableForm">
                                <tr>
                                    <td></td>
                                    <td>
                                       Total :
                                    </td>
                                    <td>
                                        <asp:Label ID="lblTotal" runat="server"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                 <table>
                    <tr>
                        <td style="text-align: right;">
                            <input type="button" id="btnSaveCN" title="Save" runat="server" value="Save" onclick="saveDebitNote();" />
                           
                            <%--<asp:Button ID="btnSave" runat="server" Text="Save" />
                            <asp:Button ID="btnPrint" runat="server" Text="Print" />--%>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </center>
    <asp:HiddenField ID="hdnSessionPONo" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnCompanyID" runat="server" ClientIDMode="Static" />    
    <asp:HiddenField ID="hdnWarehouseID" runat="server" ClientIDMode="Static" />
     <script type="text/javascript">
         function saveDebitNote() {
             var hdnSessionPONo = document.getElementById('hdnSessionPONo');
             var hdnCompanyID = document.getElementById('hdnCompanyID');
             var hdnWarehouseID = document.getElementById('hdnWarehouseID');
             var txtRemark = document.getElementById("<%= txtRemark.ClientID %>");
            var lblPONO = document.getElementById("<%= lblPONO.ClientID %>");
            var DebitDate = getDateFromUC("<%= UCDebitDate.ClientID %>");
            var lblTotal = document.getElementById("<%= lblTotal.ClientID %>");
            var obj = new Object();
            obj.Remark = txtRemark.value.toString();
            obj.ONO = hdnSessionPONo.value;
            obj.DebitNoteDate = DebitDate;
            obj.Total = lblTotal.innerHTML;
            obj.Company = hdnCompanyID.value;

            PageMethods.WMSaveDebitNote(obj, hdnSessionPONo.value, hdnWarehouseID.value, WMSaveCreditNote_onSuccessed, WMSaveCreditNote_onFailed);
        }

        function WMSaveCreditNote_onSuccessed(result) {
            if (result == "Some error occurred" || result == "") { showAlert("Error occurred", "Error", '#'); }
            else if (result == "Debit Note saved successfully") {
                showAlert(result, "info", "#");
                txtRemark.value = "";
                self.close();
            }
            self.close();
        }

        function WMSaveCreditNote_onFailed() { showAlert("Error occurred", "Error", "#"); self.close(); }
    </script>
</asp:Content>

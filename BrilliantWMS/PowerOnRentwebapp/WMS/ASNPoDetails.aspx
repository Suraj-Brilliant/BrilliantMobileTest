<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/CRM2.Master" AutoEventWireup="true" CodeBehind="ASNPoDetails.aspx.cs" Inherits="BrilliantWMS.WMS.ASNPoDetails"
    EnableEventValidation="false" Theme="Blue" %>

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
        <asp:UpdateProgress ID="UpdateProgress_DebitNote" runat="server" AssociatedUpdatePanelID="updPnl_ASNPoDetail">
            <ProgressTemplate>
                <center>
                    <div class="modal">
                        <img src="../App_Themes/Blue/img/ajax-loader.gif" style="top: 50%;" />
                    </div>
                </center>
            </ProgressTemplate>
        </asp:UpdateProgress>
        <asp:UpdatePanel ID="updPnl_ASNPoDetail" runat="server">
            <ContentTemplate>
                <table class="gridFrame" width="90%">
                    <tr>
                        <td>
                            <table style="width: 100%">
                                <tr>
                                    <td style="text-align: left;">
                                        <a id="headerText">
                                            <asp:Label ID="lblSKUList" runat="server" Text="ASN SKU List" /></a>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <obout:Grid ID="gdASNSku" runat="server" AutoGenerateColumns="False" AllowAddingRecords="False"
                                AllowFiltering="True" AllowGrouping="True" AllowMultiRecordSelection="False"
                                AllowRecordSelection="true" Width="100%" AllowColumnResizing="true" AllowColumnReordering="true">
                                <Columns>
                                    <obout:Column DataField="ID" HeaderText="ID" Visible="false">
                                    </obout:Column>
                                    <obout:Column DataField="Prod_Code" HeaderText="SKU Code" AllowEdit="false" Width="15%" Wrap="true"
                                        HeaderAlign="left">
                                    </obout:Column>
                                    <obout:Column DataField="Prod_Name" HeaderText="SKU Name" AllowEdit="false" Width="20%" Wrap="true"
                                        HeaderAlign="left">
                                    </obout:Column>
                                    <obout:Column DataField="Prod_Description" HeaderText="Description" AllowEdit="false" Wrap="true"
                                        Width="20%" HeaderAlign="left">
                                    </obout:Column>
                                    <obout:Column DataField="UOM" HeaderText="UOM" AllowEdit="false" Width="14%" HeaderAlign="center"
                                        Align="center">
                                    </obout:Column>
                                    <obout:Column DataField="POQty" HeaderText="PO Qty" AllowEdit="false" Width="11%" Wrap="true"
                                        HeaderAlign="center" Align="center">
                                    </obout:Column>
                                    <obout:Column DataField="AsnQty" HeaderText="ASN Qty" AllowEdit="false" Width="11%" Wrap="true"
                                        HeaderAlign="center" Align="center">
                                    </obout:Column>
                                    <obout:Column DataField="RemaningQty" HeaderText="Remaining Qty" AllowEdit="false" Width="11%" Wrap="true"
                                        HeaderAlign="center" Align="center">
                                    </obout:Column>
                                    <obout:Column DataField="ShippedQty" HeaderText="GRN Qty" AllowEdit="false" Width="11%" Wrap="true"
                                        HeaderAlign="center" Align="center">
                                    </obout:Column>
                                    <obout:Column DataField="ReturnQty" HeaderText="Return Qty" AllowEdit="false" Width="11%" Wrap="true"
                                        HeaderAlign="center" Align="center">
                                    </obout:Column>
                                </Columns>
                            </obout:Grid>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </center>
</asp:Content>

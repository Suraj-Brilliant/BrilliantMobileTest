<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GridReceiptSummary.aspx.cs"
    Inherits="BrilliantWMS.PowerOnRent.GridReceiptSummary" EnableEventValidation="false" %>

<%@ Register Assembly="obout_Grid_NET" Namespace="Obout.Grid" TagPrefix="obout" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <center>
            <table>
                <tr>
                    <td>
                        <obout:Grid ID="GVReceipt" runat="server" AllowAddingRecords="false" AutoGenerateColumns="false"
                            AllowGrouping="true" Serialize="false" CallbackMode="true" AllowRecordSelection="false"
                            AllowMultiRecordSelection="false" AllowColumnReordering="true" AllowFiltering="true"
                            Width="100%" PageSize="10">
                            <Columns>
                                <obout:Column DataField="SiteID" Visible="false" Width="0%">
                                </obout:Column>
                                <obout:Column DataField="SiteName" HeaderText="Site" HeaderAlign="left" Align="left"
                                    Width="6%">
                                </obout:Column>
                                <obout:Column DataField="PRH_ID" HeaderText="Request No." HeaderAlign="center" Align="center"
                                    Width="7%">
                                </obout:Column>
                                <obout:Column DataField="MINH_ID" HeaderText="Issue No." HeaderAlign="center" Align="center"
                                    Width="7%">
                                </obout:Column>
                                <obout:Column DataField="IssueDate" HeaderText="Issue Date" HeaderAlign="center"
                                    Align="center" Width="9%" DataFormatString="{0:dd-MMM-yy}">
                                </obout:Column>
                                <obout:Column DataField="ShippingType" Width="6%" HeaderAlign="left" Align="left">
                                </obout:Column>
                                <obout:Column DataField="IssuedByUserName" HeaderText="Issued By" Width="8%" HeaderAlign="left"
                                    Align="left">
                                </obout:Column>
                                <%--Receipt Details--%>
                                <obout:Column DataField="GRNH_ID" HeaderText="Receipt No." HeaderAlign="left" Align="left"
                                    Width="7%">
                                </obout:Column>
                                <obout:Column DataField="GRN_Date" HeaderText="Receipt Date" HeaderAlign="center"
                                    Align="center" Width="9%" DataFormatString="{0:dd-MMM-yy}">
                                </obout:Column>
                                <obout:Column DataField="ReceiptByUserName" HeaderText="Received By" HeaderAlign="left"
                                    Align="left" Width="10%">
                                </obout:Column>
                                <obout:Column DataField="ReceiptStatus" HeaderText="Receipt Status" HeaderAlign="left"
                                    Align="left" Width="10%">
                                </obout:Column>
                                <obout:Column DataField="StatusID" HeaderText="" Width="0%" Visible="false">
                                </obout:Column>
                                <obout:Column DataField="ImgReceipt" HeaderText="Receipt" HeaderAlign="center" Align="center"
                                    Width="6%">
                                    <TemplateSettings TemplateId="GTStatusGUIReceipt" />
                                </obout:Column>
                                <obout:Column DataField="ImgConsumption" HeaderText="Consumption" HeaderAlign="center"
                                    Align="center" Width="6%">
                                    <TemplateSettings TemplateId="GTStatusGUIConsumption" />
                                </obout:Column>
                            </Columns>
                            <Templates>
                                <obout:GridTemplate ID="GTStatusGUIReceipt" runat="server">
                                    <Template>
                                        <center>
                                            <div class='<%# ("POR" + Container.Value) %>' onclick="parent.ReceiptOpenEntryForm('Receipt','<%# Container.Value %>', '<%# Container.DataItem["GRNH_ID"] %>')">
                                            </div>
                                        </center>
                                    </Template>
                                </obout:GridTemplate>
                                <obout:GridTemplate ID="GTStatusGUIConsumption" runat="server">
                                    <Template>
                                        <center>
                                            <div class='<%# ("POR" + Container.Value) %>' onclick="parent.ReceiptOpenEntryForm('Consumption','<%# Container.Value %>', '<%# Container.DataItem["GRNH_ID"] %>')">
                                            </div>
                                        </center>
                                    </Template>
                                </obout:GridTemplate>
                            </Templates>
                        </obout:Grid>
                    </td>
                </tr>
            </table>
        </center>
    </div>
    </form>
</body>
</html>

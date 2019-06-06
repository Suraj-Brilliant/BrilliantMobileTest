<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GridIssueSummary.aspx.cs"
    Inherits="BrilliantWMS.PowerOnRent.GridIssueSummary" EnableEventValidation="false" %>

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
                        <obout:Grid ID="GVIssue" runat="server" AllowAddingRecords="false" AutoGenerateColumns="false"
                            AllowGrouping="true" Serialize="false" CallbackMode="true" AllowRecordSelection="false"
                            AllowMultiRecordSelection="false" AllowColumnReordering="true" AllowFiltering="true"
                            Width="100%" PageSize="10">
                            <Columns>
                                <obout:Column DataField="SiteID" Visible="false" Width="0px">
                                </obout:Column>
                                <obout:Column DataField="SiteName" HeaderText="Department" HeaderAlign="left" Align="left"
                                    Width="10%">
                                </obout:Column>
                                <obout:Column DataField="PRH_ID" HeaderText="Request No." HeaderAlign="left" Align="left"
                                    Width="8%">
                                </obout:Column>
                                <obout:Column DataField="RequestDate" Width="0%" Visible="false">
                                </obout:Column>
                                <obout:Column DataField="RequestBy" Width="0%" Visible="false">
                                </obout:Column>
                                <obout:Column DataField="RequestByUserName" Width="0%" Visible="false">
                                </obout:Column>
                                <%--Issue Details--%>
                                <obout:Column DataField="MINH_ID" HeaderText="Issue No." HeaderAlign="left" Align="left"
                                    Width="7%">
                                </obout:Column>
                                <obout:Column DataField="IssueDate" HeaderText="Issue Date" HeaderAlign="center"
                                    Align="center" Width="9%" DataFormatString="{0:dd-MMM-yy}">
                                    <TemplateSettings HeaderTemplateId="GTHeaderIssueDate" />
                                </obout:Column>
                                <obout:Column DataField="ShippingDate" HeaderText="Shipping Date" HeaderAlign="center"
                                    Align="center" Width="9%" DataFormatString="{0:dd-MMM-yy}">
                                    <TemplateSettings HeaderTemplateId="GTHeaderShippingDate" />
                                </obout:Column>
                                <obout:Column DataField="ExpectedDelDate" HeaderText="Exp.Delivery Dt" HeaderAlign="center"
                                    Align="center" Width="9%" DataFormatString="{0:dd-MMM-yy}">
                                    <TemplateSettings HeaderTemplateId="GTHeaderExpectedDelDate" />
                                </obout:Column>
                                <obout:Column DataField="AirwayBill" HeaderText="AirwayBill" HeaderAlign="left" Align="left"
                                    Width="10%">
                                </obout:Column>
                                <obout:Column DataField="TransporterName" HeaderText="Transporter Name" HeaderAlign="left"
                                    Align="left" Width="10%">
                                </obout:Column>
                                <obout:Column DataField="IssuedByUserID" Visible="false" Width="0%">
                                </obout:Column>
                                <%--<obout:Column DataField="IssuedByUserName" HeaderText="Issued By" HeaderAlign="left"
                                    Align="left" Width="10%">
                                </obout:Column>--%>
                                <obout:Column DataField="StatusID" HeaderText="" Width="0px" Visible="false">
                                </obout:Column>
                                <obout:Column DataField="IssueStatus" HeaderText="Status" HeaderAlign="left"
                                    Align="left" Width="9%">
                                </obout:Column>
                                <obout:Column DataField="ImgIssue" HeaderText="Dispatch" HeaderAlign="center" Align="center"
                                    Width="6%">
                                    <TemplateSettings TemplateId="GTStatusGUIIssue" />
                                </obout:Column>
                                <%--<obout:Column DataField="ImgReceipt" HeaderText="Receipt" HeaderAlign="center" Align="center"
                                    Width="6%">
                                    <TemplateSettings TemplateId="GTStatusGUIReceipt" />
                                </obout:Column>--%>
                            </Columns>
                            <Templates>
                                <obout:GridTemplate ID="GTHeaderIssueDate" runat="server">
                                    <Template>
                                        Issue
                                        <br />
                                        Date
                                    </Template>
                                </obout:GridTemplate>
                                <obout:GridTemplate ID="GTHeaderShippingDate" runat="server">
                                    <Template>
                                        Shipping
                                        <br />
                                        Date
                                    </Template>
                                </obout:GridTemplate>
                                <obout:GridTemplate ID="GTHeaderExpectedDelDate" runat="server">
                                    <Template>
                                        Exp.Delivery
                                        <br />
                                        Date
                                    </Template>
                                </obout:GridTemplate>
                                <obout:GridTemplate ID="GTStatusGUIIssue" runat="server">
                                    <Template>
                                        <center>
                                            <div class='<%# ("POR" + Container.Value) %>' onclick="parent.IssueOpenEntryForm('Issue','<%# Container.Value %>', '<%# Container.DataItem["MINH_ID"] %>')">
                                            </div>
                                        </center>
                                    </Template>
                                </obout:GridTemplate>
                               <%-- <obout:GridTemplate ID="GTStatusGUIReceipt" runat="server">
                                    <Template>
                                        <center>
                                            <div class='<%# ("POR" + Container.Value) %>' onclick="parent.IssueOpenEntryForm('Receipt','<%# Container.Value %>', '<%# Container.DataItem["MINH_ID"] %>')">
                                            </div>
                                        </center>
                                    </Template>
                                </obout:GridTemplate>--%>
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

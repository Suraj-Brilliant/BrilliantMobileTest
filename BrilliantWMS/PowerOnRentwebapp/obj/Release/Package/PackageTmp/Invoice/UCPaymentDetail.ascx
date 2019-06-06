<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCPaymentDetail.ascx.cs" Inherits="BrilliantWMS.Invoice.UCPaymentDetail" %>

<%@ Register Assembly="obout_Grid_NET" Namespace="Obout.Grid" TagPrefix="obout" %>

<center>
    <table style="float: right;">
        <tr>
            <td>
                Financial Year :
            </td>
            <td>
                <asp:DropDownList runat="server" ID="ddlFY" Width="150px">
                    <asp:ListItem Text="2011-2012" Value="2011-2012"></asp:ListItem>
                    <asp:ListItem Text="2012-2013" Value="2012-2013" Selected="True"></asp:ListItem>
                </asp:DropDownList>
            </td>
            <td>
                <asp:Button runat="server" ID="BtnExportToExcel" Text="Export To Excel" OnClick="BtnExportToExcel_OnClick" />
            </td>
            <td>
                <asp:Button runat="server" ID="BtnExportToPDF" Text="Export To PDF" OnClick="BtnExportToPDF_OnClick" />
            </td>
            <td>
                <asp:Button ID="BtnReload" runat="server" Text="Reload" OnClick="BtnReload_Click" />
            </td>
        </tr>
    </table>
    <asp:HiddenField ID="hdnPaymentDetailAccountID" runat="server" />
    <table class="gridFrame">
        <tr>
            <td style="text-align: left;">
                <a class="headerText">Account Details</a>
            </td>
        </tr>
        <tr>
            <td>
                <obout:Grid runat="server" ID="GVDisply" AllowAddingRecords="false" AllowFiltering="true"
                    AllowGrouping="true" AutoGenerateColumns="false" GroupBy="CustomerID,CustomerName"
                    Width="100%" ShowGroupFooter="false" HideColumnsWhenGrouping="true" ShowGroupsInfo="false">
                    <TemplateSettings GroupHeaderTemplateId="GroupHeaderTemplate" />
                    <Columns>
                        <obout:Column DataField="CustomerID" HeaderText="Account" Visible="false" Width="0%"
                            AllowGroupBy="false">
                        </obout:Column>
                        <obout:Column DataField="CustomerCode" Width="0%" Visible="false">
                        </obout:Column>
                        <obout:Column DataField="CustomerName" Width="0%" Visible="false">
                        </obout:Column>
                        <obout:Column DataField="Remark" Width="0%" Visible="false">
                        </obout:Column>
                        <obout:Column DataField="TransID" Width="0%" Visible="false">
                        </obout:Column>
                        <obout:Column DataField="TransDate" HeaderText="Transaction Date" Width="10%" Align="center"
                            HeaderAlign="center" DataFormatString="{0:dd-MMM-yy}" AllowGroupBy="false">
                        </obout:Column>
                        <obout:Column DataField="Particulars" HeaderText="Particulars" Width="40%" Align="left"
                            HeaderAlign="left" Wrap="true" AllowGroupBy="false">
                            <TemplateSettings TemplateId="TemplateParticulars" />
                        </obout:Column>
                        <obout:Column DataField="OpeningBalance" HeaderText="Opening Balance" Width="15%"
                            Align="right" HeaderAlign="right" AllowGroupBy="false">
                            <TemplateSettings TemplateId="TemplateAmount" />
                        </obout:Column>
                        <obout:Column DataField="DrAmount" HeaderText="Debit" Width="10%" Align="right" HeaderAlign="right"
                            AllowGroupBy="false">
                            <TemplateSettings TemplateId="TemplateAmount" />
                        </obout:Column>
                        <obout:Column DataField="CrAmount" HeaderText="Credit" Width="10%" Align="right"
                            HeaderAlign="right" AllowGroupBy="false">
                            <TemplateSettings TemplateId="TemplateAmount" />
                        </obout:Column>
                        <obout:Column DataField="ClosingBalance" HeaderText="Closing Balance" Width="15%"
                            Align="right" HeaderAlign="right" AllowGroupBy="false">
                            <TemplateSettings TemplateId="TemplateAmount" />
                        </obout:Column>
                        <%-- <obout:Column DataField="PBvisible" HeaderText="" Width="15%" Align="center" HeaderAlign="center"
                            AllowGroupBy="false">
                            <TemplateSettings TemplateId="TemplatePB" />
                        </obout:Column>--%>
                    </Columns>
                    <Templates>
                        <obout:GridTemplate runat="server" ID="TemplateAmount">
                            <Template>
                                <span style="margin-right: 8px;">
                                    <%# (Container.Value) == "0.00" ? "" : (Container.Value) %></span>
                            </Template>
                        </obout:GridTemplate>
                        <obout:GridTemplate runat="server" ID="TemplateParticulars">
                            <Template>
                                <table>
                                    <tr>
                                        <td>
                                            <%# (Container.Value) %>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <span style="font-size: 11px">
                                                <%# (Container.DataItem["Remark"].ToString()) %></span>
                                        </td>
                                    </tr>
                                </table>
                            </Template>
                        </obout:GridTemplate>
                        <obout:GridTemplate runat="server" ID="GroupHeaderTemplate">
                            <Template>
                                <%# Container.Value %>
                            </Template>
                        </obout:GridTemplate>
                        <%--  <obout:GridTemplate ID="TemplatePB" runat="server">
                            <Template>
                                <a onclick="">Payment [Add/Edit]</a>
                            </Template>
                        </obout:GridTemplate>--%>
                    </Templates>
                </obout:Grid>
            </td>
        </tr>
    </table>
    <script type="text/javascript">
        function openPaymentBooking(invoiceID) {
            window.open();
        }
    </script>
</center>
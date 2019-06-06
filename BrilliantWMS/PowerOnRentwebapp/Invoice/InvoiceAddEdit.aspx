<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/CRM.Master" AutoEventWireup="true" 
    CodeBehind="InvoiceAddEdit.aspx.cs" Inherits="BrilliantWMS.Invoice.InvoiceAddEdit" EnableEventValidation="false" %>

<%@ Register Assembly="obout_Interface" Namespace="Obout.Interface" TagPrefix="obout" %>
<%@ Register Assembly="obout_Grid_NET" Namespace="Obout.Grid" TagPrefix="obout" %>
<%@ Register Src="../CommonControls/UCToolbar.ascx" TagName="UCToolbar" TagPrefix="uc1" %>
<%@ Register Src="../CommonControls/UCFormHeader.ascx" TagName="UCFormHeader" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../CommonControls/UC_Date.ascx" TagName="UC_Date" TagPrefix="uc3" %>
<%@ Register Src="../Account/UC_AccountSearch.ascx" TagName="UC_AccountSearch" TagPrefix="uc4" %>
<%@ Register Src="../Document/UC_AttachDocument.ascx" TagName="UC_AttachDocument"
    TagPrefix="uc6" %>
<%@ Register Src="../Product/UC_AddToCart.ascx" TagName="UC_AddToCart" TagPrefix="uc7" %>
<%@ Register Assembly="obout_Flyout2_NET" Namespace="OboutInc.Flyout2" TagPrefix="cc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="obout_Interface" Namespace="Obout.Interface" TagPrefix="cc4" %>
<%@ Register Src="../CommonControls/UC_ActionHistory.ascx" TagName="UC_ActionHistory"
    TagPrefix="uc8" %>
<%@ Register Src="../ContactPerson/UCContactPerson.ascx" TagName="UCContactPerson"
    TagPrefix="uc11" %>
<%@ Register Src="../Competitor/UC_CompetitorDetail.ascx" TagName="UC_CompetitorDetail"
    TagPrefix="uc12" %>
<%@ Register Src="../Activity/UC_AssignTaskUC.ascx" TagName="UC_AssignTaskUC" TagPrefix="uc5" %>
<%@ Register Src="../Activity/UC_ActionTakenOnTaskUC.ascx" TagName="UC_ActionTakenOnTaskUC"
    TagPrefix="uc9" %>
<%@ Register Src="../Company/UC_TermsAndCondition.ascx" TagName="UC_TermsAndCondition"
    TagPrefix="uc14" %>
<%@ Register Src="../Address/UCAddress.ascx" TagName="UCAddress" TagPrefix="uc10" %>
<%@ Register Src="../Product/UC_TitleSearch.ascx" TagName="UC_TitleSearch" TagPrefix="uc13" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHolder_FormHeader" runat="server">
    <uc1:UCToolbar ID="UCToolbar1" runat="server" />
    <uc2:UCFormHeader ID="UCFormHeader1" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPHolder_Form" runat="server">
     <div id="tabletest">
        <asp:TextBox runat="server" ID="hdnCartCount" ValidationGroup="Save" ClientIDMode="Static"
            Style="display: none;"></asp:TextBox>
        <asp:RequiredFieldValidator ID="ReqFV" runat="server" ErrorMessage="Atleast one product add into Cart"
            Display="None" ControlToValidate="hdnCartCount" InitialValue="0" ValidationGroup="Save"></asp:RequiredFieldValidator>
        <asp:TabContainer runat="server" ID="Invoice" ActiveTabIndex="0">
            <asp:TabPanel ID="tabInvoiceInfo" runat="server" HeaderText="Invoice Info">
                <ContentTemplate>
                    <div>
                        <asp:UpdateProgress ID="UpdateProgress_Invoice" runat="server" AssociatedUpdatePanelID="updPnl_Invoice_Details">
                            <ProgressTemplate>
                                <center>
                                    <div class="modal">
                                        <img src="../App_Themes/Blue/img/ajax-loader.gif" style="top: 50%;" />
                                    </div>
                                </center>
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                        <asp:ValidationSummary ID="validationsummary_InvoiceAddEdit" runat="server" ShowMessageBox="True"
                            ShowSummary="False" ValidationGroup="Save" />
                        <asp:UpdatePanel ID="updPnl_Invoice_Details" runat="server">
                            <ContentTemplate>
                                <center>
                                    <table class="tableForm">
                                        <tr>
                                            <td>
                                                Title :
                                            </td>
                                            <td colspan="4" style="text-align: left">
                                                <asp:TextBox ID="txtTitle" runat="server" Width="480px" ClientIDMode="Static"></asp:TextBox>
                                                <asp:HiddenField ID="hdnTitleSearchID" runat="server" ClientIDMode="Static" />
                                                <uc13:UC_TitleSearch ID="UC_TitleSearch1" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Campaign :
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlCampaign" runat="server" Enabled="False" Width="196px">
                                                    <asp:ListItem Text="-Select-" Selected="True"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <req>Invoice No.:</req>
                                                <asp:RequiredFieldValidator ID="ReqtxtUserInvoiceNo" runat="server" Display="None"
                                                    ErrorMessage="Enter Invoice No." ControlToValidate="txtUserInvoiceNo" ValidationGroup="Save"
                                                    InitialValue="0"></asp:RequiredFieldValidator>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" Display="None"
                                                    ErrorMessage="Enter Invoice No." ControlToValidate="txtUserInvoiceNo" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtInvoiceNo" runat="server" Visible="False" Enabled="False" Width="190px"></asp:TextBox>
                                                <asp:TextBox ID="txtUserInvoiceNo" runat="server" Width="190px" MaxLength="20" onchange="CheckDuplicateInvoiceNo(this);"
                                                    ClientIDMode="Static"></asp:TextBox>
                                            </td>
                                            <td>
                                                <req> Invoice Date :</req>
                                            </td>
                                            <td style="text-align: left">
                                                <uc3:UC_Date ID="UC_InvoiceDate" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Lead Source :
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlLeadSource" runat="server" DataTextField="Name" DataValueField="ID"
                                                    Width="196px">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <req>Customer Name :</req>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtAccountName" runat="server" MaxLength="100" Width="171px" ClientIDMode="Static"></asp:TextBox>
                                                <uc4:UC_AccountSearch ID="UC_AccountSearch1" runat="server" />
                                                <asp:RequiredFieldValidator ID="req_txtAccountName" runat="server" ErrorMessage="Please Select/Enter Lead Name"
                                                    ControlToValidate="txtAccountName" ValidationGroup="Save" Display="None"></asp:RequiredFieldValidator>
                                                <asp:HiddenField ID="hdnCustomerID" runat="server" ClientIDMode="Static" />
                                            </td>
                                            <td>
                                                Website :
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtWebSite" runat="server" MaxLength="50" Width="190px" ClientIDMode="Static"></asp:TextBox>
                                                <asp:RegularExpressionValidator ID="req_txtWebSite" runat="server" ErrorMessage="Please Enter Valid Website Address"
                                                    ValidationExpression="(http(s)?://)?([\w-]+\.)+[\w-]+(/[\w- ;,./?%&=]*)?" ControlToValidate="txtWebSite"
                                                    Display="None"></asp:RegularExpressionValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <req>Status :</req>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlInvoiceStatus" runat="server" Width="196px">
                                                    <asp:ListItem Text="-Select-" Value="0"></asp:ListItem>
                                                    <asp:ListItem Text="Open" Value="Open"></asp:ListItem>
                                                    <asp:ListItem Text="Closed-Won" Value="Closed-Won"></asp:ListItem>
                                                    <asp:ListItem Text="Closed-Lost" Value="Closed-Lost"></asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="req_ddlInvoiceStatus" runat="server" ErrorMessage="Please Select Status"
                                                    ValidationGroup="Save" ControlToValidate="ddlInvoiceStatus" InitialValue="0"
                                                    Display="None"></asp:RequiredFieldValidator>
                                            </td>
                                            <td>
                                                <req> Sector :</req>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlSector" runat="server" DataTextField="Name" DataValueField="ID"
                                                    Width="196px" ClientIDMode="Static">
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="req_ddlSector" runat="server" ErrorMessage="Please Select Sector"
                                                    ValidationGroup="Save" ControlToValidate="ddlSector" InitialValue="0" Display="None"></asp:RequiredFieldValidator>
                                            </td>
                                            <td>
                                                Company Type :
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlCompanyType" runat="server" DataTextField="CompanyType"
                                                    DataValueField="ID" Width="196px" ClientIDMode="Static">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Invoice Type :
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlInvoiceType" runat="server" Width="196px">
                                                    <asp:ListItem Text="Final Invoice" Value="Final Invoice"></asp:ListItem>
                                                    <asp:ListItem Text="Proforma Invoice" Value="Proforma Invoice"></asp:ListItem>
                                                    <asp:ListItem Text="Excise Invoice" Value="Excise Invoice"></asp:ListItem>
                                                    <asp:ListItem Text="Commercial Invoice" Value="Commercial Invoice"></asp:ListItem>
                                                    <asp:ListItem Text="Tax Invoice" Value="Tax Invoice"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                Customer PO No. :
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtCustomerPONo" runat="server" Width="190px"></asp:TextBox>
                                            </td>
                                            <td>
                                                Customer PO Date :
                                            </td>
                                            <td style="text-align: left">
                                                <uc3:UC_Date ID="UC_CustomerPODate" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Parent Invoice No.:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtParentInvoiceNo" runat="server" ReadOnly="True" Width="190px"></asp:TextBox>
                                            </td>
                                            <td>
                                                Dispatch Through :
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtlDispatchThrough" runat="server" Width="190px" MaxLength="200"></asp:TextBox>
                                            </td>
                                            <td>
                                                Exp. Dispatch Date :
                                            </td>
                                            <td style="text-align: left">
                                                <uc3:UC_Date ID="UC_ExpDispatchDate" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Remark :
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtRemark" runat="server" MaxLength="150" Width="190px"></asp:TextBox>
                                            </td>
                                            <td>
                                                Executive :
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlExecutive" runat="server" Width="196" DataTextField="Assignto"
                                                    DataValueField="Id">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                        </tr>
                                        <asp:HiddenField ID="HndFlagActivity" runat="server" ClientIDMode="Static" />
                                        <asp:HiddenField ID="HndClose" runat="server" ClientIDMode="Static" />
                                    </table>
                                    <div>
                                        <uc7:UC_AddToCart ID="UC_AddToCart1" runat="server" />
                                    </div>
                                </center>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </ContentTemplate>
            </asp:TabPanel>
            <asp:TabPanel ID="tabAddressInfo" runat="server" HeaderText="Address Info">
                <ContentTemplate>
                    <uc10:UCAddress ID="UCAddress1" runat="server" />
                </ContentTemplate>
            </asp:TabPanel>
            <asp:TabPanel ID="tabContactInfo" runat="server" HeaderText="Contact Info">
                <ContentTemplate>
                    <uc11:UCContactPerson ID="UCContactPerson1" runat="server"></uc11:UCContactPerson>
                </ContentTemplate>
            </asp:TabPanel>
            <asp:TabPanel ID="tabDocument" runat="server" HeaderText="Documents">
                <ContentTemplate>
                    <uc6:UC_AttachDocument ID="UC_AttachDocument1" runat="server" />
                </ContentTemplate>
            </asp:TabPanel>
            <%--  <asp:TabPanel ID="tabCompetitorAnalysis" runat="server" HeaderText="Competitor Analysis">
                <ContentTemplate>
                    <uc12:UC_CompetitorDetail ID="UC_CompetitorDetail1" runat="server" />
                </ContentTemplate>
            </asp:TabPanel>--%>
            <asp:TabPanel ID="tabTermCondition" runat="server" HeaderText="Terms & Condition">
                <ContentTemplate>
                    <uc14:UC_TermsAndCondition ID="UC_TermsAndCondition1" runat="server" />
                </ContentTemplate>
            </asp:TabPanel>
            <asp:TabPanel ID="tabPayment" runat="server" HeaderText="Payment" OnClientClick="SetInvoicePBFooter">
                <ContentTemplate>
                    <center>
                        <table class="gridFrame" width="80%">
                            <tr>
                                <td>
                                    <table style="width: 100%">
                                        <tr>
                                            <td style="text-align: left;">
                                                <a style="color: white; font-size: 15px; font-weight: bold;">Payment Schedule</a>
                                            </td>
                                            <td>
                                                <input type="button" value="Add New" id="btnPaymentSchedule" onclick="<%=FlyoutPaymentSchedule.getClientID()%>.Open(); clearPaySchedule(); "
                                                    style="float: right;" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <obout:Grid ID="grvPaymentSchedule" runat="server" AutoGenerateColumns="False" AllowAddingRecords="false"
                                        AllowFiltering="True" AllowGrouping="True" AllowMultiRecordSelection="False"
                                        Width="100%" OnRebind="grvPaymentSchedule_OnRebind" CallbackMode="true">
                                        <Columns>
                                            <obout:Column HeaderText="ID" DataField="ID" Visible="false" />
                                            <obout:Column HeaderText="InvoiceID" DataField="InvoiceID" Visible="false" />
                                            <obout:Column HeaderText="Sequence" DataField="Sequence" Visible="false" />
                                            <obout:Column HeaderText="Edit" Width="4%" Align="center" HeaderAlign="center">
                                                <TemplateSettings TemplateId="tplEditBtn" />
                                            </obout:Column>
                                            <obout:Column HeaderText="Scheduled Date" DataField="PaymentScheduleDate" DataFormatString="{0:dd-MMM-yyyy}"
                                                Width="10%" Align="center" HeaderAlign="center">
                                            </obout:Column>
                                            <obout:Column HeaderText="Amount" DataField="PaymentScheduleAmount" Width="15%" Align="right"
                                                HeaderAlign="right">
                                            </obout:Column>
                                            <obout:Column HeaderText="Alert Date" DataField="AlertDate" Width="10%" DataFormatString="{0:dd-MMM-yyyy}"
                                                Align="center" HeaderAlign="center">
                                            </obout:Column>
                                            <obout:Column DataField="AlertEmail" HeaderText="Email Alert" Width="8%" HeaderAlign="center"
                                                Align="center">
                                                <TemplateSettings TemplateId="tplAlertEmail" />
                                            </obout:Column>
                                            <obout:Column DataField="AlertSMS" HeaderText="SMS Alert" Width="8%" HeaderAlign="center"
                                                Align="center">
                                                <TemplateSettings TemplateId="tplAlertSMS" />
                                            </obout:Column>
                                            <obout:Column HeaderText="Customer Alert" DataField="CustomerAlert" Width="8%" HeaderAlign="center"
                                                Align="center">
                                                <TemplateSettings HeaderTemplateId="HeaderTempCustomerAlert" TemplateId="tplCustomerAlert" />
                                            </obout:Column>
                                            <obout:Column HeaderText="Remark" DataField="Remark" Width="20%" />
                                        </Columns>
                                        <Templates>
                                            <obout:GridTemplate ID="HeaderTempCustomerAlert">
                                                <Template>
                                                    Customer
                                                    <br />
                                                    Alert
                                                </Template>
                                            </obout:GridTemplate>
                                            <obout:GridTemplate runat="server" ID="tplEditBtn">
                                                <Template>
                                                    <img alt="Edit" onclick="getPaySheData(this.title);" src="../App_Themes/Blue/img/Edit16.png"
                                                        style="cursor: pointer;" title="<%# (Container.PageRecordIndex) %>" />
                                                </Template>
                                            </obout:GridTemplate>
                                            <obout:GridTemplate runat="server" ID="tplAlertEmail">
                                                <Template>
                                                    <obout:OboutCheckBox ID="OboutCheckBox2" Enabled="false" runat="server" Checked='<%# (Container.DataItem["AlertEmail"].ToString() == "True" ? true : false)%>'>
                                                    </obout:OboutCheckBox>
                                                </Template>
                                            </obout:GridTemplate>
                                            <obout:GridTemplate runat="server" ID="tplAlertSMS">
                                                <Template>
                                                    <obout:OboutCheckBox ID="OboutCheckBox3" Enabled="false" runat="server" Checked='<%# (Container.DataItem["AlertSMS"].ToString() == "True" ? true : false)%>'>
                                                    </obout:OboutCheckBox>
                                                </Template>
                                            </obout:GridTemplate>
                                            <obout:GridTemplate runat="server" ID="tplActive">
                                                <Template>
                                                    <%# (Container.DataItem["Active"].ToString() == "Y" ? "Yes" : "No")%>
                                                </Template>
                                            </obout:GridTemplate>
                                            <obout:GridTemplate runat="server" ID="tplCustomerAlert">
                                                <Template>
                                                    <obout:OboutCheckBox ID="OboutCheckBox4" Enabled="false" runat="server" Checked='<%# (Container.DataItem["CustomerAlert"].ToString() == "True" ? true : false)%>'>
                                                    </obout:OboutCheckBox>
                                                </Template>
                                            </obout:GridTemplate>
                                        </Templates>
                                    </obout:Grid>
                                </td>
                            </tr>
                        </table>
                        <table class="gridFrame" width="80%">
                            <tr>
                                <td>
                                    <table style="width: 100%">
                                        <tr>
                                            <td style="text-align: left;">
                                                <a style="color: white; font-size: 15px; font-weight: bold;">Payment Booking</a>
                                            </td>
                                            <td>
                                                <input type="button" value="Add New" id="btnPaymentBooking" onclick="<%=FlyoutPaymentBooking.getClientID()%>.Open(); clearPayBooking();"
                                                    style="float: right;" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <obout:Grid ID="grvPaymentBooking" runat="server" AutoGenerateColumns="False" AllowAddingRecords="false"
                                        AllowFiltering="True" AllowGrouping="True" AllowMultiRecordSelection="False"
                                        OnRebind="grvPaymentBooking_OnRebind" CallbackMode="true" Width="100%">
                                        <Columns>
                                            <obout:Column HeaderText="ID" DataField="ID" Visible="false" />
                                            <obout:Column HeaderText="Sequence" DataField="Sequence" Visible="false" />
                                            <obout:Column HeaderText="InvoiceID" DataField="InvoiceID" Visible="false" />
                                            <obout:Column HeaderText="Edit" Width="5%" Align="center" HeaderAlign="center">
                                                <TemplateSettings TemplateId="tplEditBtnPAyBooking" />
                                            </obout:Column>
                                            <obout:Column HeaderText="Received Date" DataField="PaymentReceivedDate" DataFormatString="{0:dd-MMM-yyyy}"
                                                Width="15%" Align="center" HeaderAlign="center">
                                            </obout:Column>
                                            <obout:Column DataField="PaymentMode" HeaderText="Payment Mode" Width="15%" Align="center"
                                                HeaderAlign="center">
                                            </obout:Column>
                                            <obout:Column HeaderText="Payment Details" DataField="PaymentDetails" Width="25%">
                                            </obout:Column>
                                            <obout:Column ID="Column1" HeaderText="Remark" DataField="Remark" Width="25%" runat="Server">
                                            </obout:Column>
                                            <obout:Column HeaderText="Amount" DataField="PaymentAmount" Width="20%" Align="right"
                                                HeaderAlign="right">
                                                <TemplateSettings TemplateId="GridTemplate1Amount" />
                                            </obout:Column>
                                        </Columns>
                                        <Templates>
                                            <obout:GridTemplate runat="server" ID="tplEditBtnPAyBooking">
                                                <Template>
                                                    <img alt="Edit" onclick="<%=FlyoutPaymentBooking.getClientID()%>.Open(); getPayBookData(this.title);"
                                                        src="../App_Themes/Blue/img/Edit16.png" style="cursor: pointer;" title="<%# (Container.PageRecordIndex) %>" />
                                                </Template>
                                            </obout:GridTemplate>
                                            <obout:GridTemplate runat="server" ID="GridTemplate1Amount">
                                                <Template>
                                                    <span style="margin-right: 10px; text-align: right;">
                                                        <%# (Container.Value) %></span>
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
                                            <td>
                                            </td>
                                            <td style="padding-top: 3px; font-weight: bold;">
                                                Total Received Amount :
                                            </td>
                                            <td>
                                                <asp:Label ID="lblTotalReceivedAmountF" runat="server" Width="120px" Style="text-align: right;
                                                    border: none;" ClientIDMode="Static" Text="0.00"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="padding-top: 3px; font-weight: bold;">
                                                Invoice Amount :
                                                <asp:Label ID="lblInvoiceAmountF" runat="server" Width="120px" Style="text-align: right;
                                                    border: none;" ClientIDMode="Static"></asp:Label>
                                            </td>
                                            <td style="padding-top: 3px; font-weight: bold;">
                                                <req>Outstanding Amount :</req>
                                            </td>
                                            <td>
                                                <req><asp:Label ID="lblOutstandingAmountF" runat="server" Width="120px" Style="text-align: right;
                                                    border: none;" ClientIDMode="Static"></asp:Label></req>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </center>
                    <asp:HiddenField ID="hdnPaymentBookingSequenece" runat="server" />
                </ContentTemplate>
            </asp:TabPanel>
            <asp:TabPanel ID="tabActionHistory" runat="server" HeaderText="Action History">
                <ContentTemplate>
                    <uc8:UC_ActionHistory ID="UC_ActionHistory1" runat="server" />
                </ContentTemplate>
            </asp:TabPanel>           
        </asp:TabContainer>
        
        <cc2:Flyout ID="FlyoutPaymentSchedule" runat="server" AttachTo="btnPaymentSchedule"
            OpenEvent="NONE" CloseEvent="NONE" IsModal="true" PageColor="Black" PageOpacity="60"
            zIndex="999" Position="ABSOLUTE" RelativeLeft="-500">
            <table class="gridFrame">
                <tr>
                    <td>
                        <cc2:DragPanel ID="DragPanel1" runat="server">
                            <table style="width: 500px">
                                <tr>
                                    <td style="text-align: left;">
                                        <a class="headerText">Payment Schedule</a>
                                    </td>
                                    <td style="text-align: right;">
                                        <%-- <asp:Button runat="server" ID="btnPaySchSave" Text="Save" ValidationGroup="PaymentSchedule"
                                                        OnClick="btnSave_Click" ClientIDMode="Static" />--%>
                                        <input id="btnPaySchSave" onclick="TempSaveInvoicePS()" type="button" value="Save" />
                                        <input id="btnClear" onclick="clearPaySchedule()" type="button" value="Clear" />
                                        <input id="btnClose" onclick="<%=FlyoutPaymentSchedule.getClientID()%>.Close();"
                                            type="button" value="Close" />
                                    </td>
                                </tr>
                            </table>
                        </cc2:DragPanel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table class="tableForm" style="background-color: White;" width="500px">
                            <tr>
                                <td colspan="4" style="text-align: center;">
                                    <asp:Label ID="lblmgs" Font-Italic="true" Font-Size="Medium" Font-Bold="true" runat="server"
                                        ForeColor="Maroon" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <req>Date :</req>
                                </td>
                                <td style="text-align: left" id="tdUC_PayemntDate">
                                    <uc3:UC_Date ID="UC_PayemntDate" runat="server" />
                                </td>
                                <td>
                                    <req> Amount :</req>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtAmount" runat="server" Width="100%" MaxLength="18" Style="text-align: right;"
                                        onkeydown="AllowDecimal(this,event);" onkeypress="AllowDecimal(this,event);"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfv_txtAmount" runat="server" ErrorMessage="Please Enter Amount"
                                        ControlToValidate="txtAmount" ValidationGroup="PaymentSchedule" Display="None"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Alert Date :
                                </td>
                                <td style="text-align: left" id="tdUC_UCAlertDate">
                                    <uc3:UC_Date ID="UCAlertDate" runat="server" />
                                </td>
                                <td>
                                    Alert Type :
                                </td>
                                <td style="text-align: left;">
                                    <asp:CheckBox ID="chkbxSMS" runat="server" Text="SMS" />
                                    <asp:CheckBox ID="chkbxEmail" runat="server" Text="Email" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Remark :
                                </td>
                                <td colspan="3">
                                    <asp:TextBox ID="txtPaymentScheduleRemarks" runat="server" Width="100%" MaxLength="200"
                                        TextMode="MultiLine" onkeyup="TextBox_KeyUp(this,'SpanCharCounterAssignTask','200');"
                                        ClientIDMode="Static"></asp:TextBox>
                                    <br />
                                    <span class="watermark"><span id="SpanCharCounterAssignTask">200</span> characters remaning
                                        out of 200 </span>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <asp:CheckBox ID="chkCustomerAlert" runat="server" Text="Customer Alert" />
                                </td>
                            </tr>
                        </table>
                        <asp:HiddenField ID="hdnPaymentScheduleID" runat="server" />
                        <asp:HiddenField ID="hdnPaymentScheduleSequenece" runat="server" />
                    </td>
                </tr>
            </table>
        </cc2:Flyout>
        <cc2:Flyout ID="FlyoutPaymentBooking" runat="server" AttachTo="btnPaymentBooking"
            OpenEvent="NONE" CloseEvent="NONE" IsModal="true" PageColor="Black" PageOpacity="60"
            zIndex="999" Position="ABSOLUTE" RelativeLeft="-600">
            <table class="gridFrame">
                <tr>
                    <td>
                        <cc2:DragPanel ID="DragPanel2" runat="server">
                            <table style="width: 650px">
                                <tr>
                                    <td style="text-align: left;">
                                        <a id="headerText">Payment Booking</a>
                                    </td>
                                    <td style="text-align: right;">
                                        <input id="btnPayBookingSave" clientidmode="Static" onclick="TempSaveInvoicePB()"
                                            type="button" value="Save" />
                                        <input id="btnPayBookingClear" onclick="clearPayBooking()" type="button" value="Clear" />
                                        <input id="btnPayBookingClose" onclick="<%=FlyoutPaymentBooking.getClientID()%>.Close();"
                                            type="button" value="Close" />
                                    </td>
                                </tr>
                            </table>
                        </cc2:DragPanel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div id="divLoading" style="height: 200px; width: 650px; display: none" class="modal">
                            <center>
                                <br />
                                <span id="spanLoading" style="font-size: 17px; font-weight: bold; color: Yellow;">Processing
                                    please wait...</span>
                            </center>
                        </div>
                        <table class="tableForm" style="background-color: White;" width="650px">
                            <tr>
                                <td>
                                    <req>Date :</req>
                                </td>
                                <td style="text-align: left" id="tdUC_PaymentBookingDate">
                                    <uc3:UC_Date ID="UC_PaymentBookingDate" runat="server" />
                                </td>
                                <td>
                                    Outstanding Amount :
                                </td>
                                <td>
                                    <asp:Label ID="lblOutstandingAmount" runat="server" Style="text-align: right;" ClientIDMode="Static"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <req>Payment Mode :</req>
                                </td>
                                <td style="text-align: left">
                                    <asp:DropDownList ID="ddlMode" runat="server" Width="198px">
                                        <asp:ListItem Text="-Select-" Selected="True" Value="0" />
                                        <asp:ListItem Text="Cash" Value="Cash" />
                                        <asp:ListItem Text="Cheque" Value="Cheque" />
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="valRfddlMode" runat="server" ErrorMessage="Select Mode Of Payment"
                                        ControlToValidate="ddlMode" InitialValue="0" Display="None" ValidationGroup="SavePaymentBooking">
                                    </asp:RequiredFieldValidator>
                                </td>
                                <td>
                                    <req>Booking Amount :</req>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtPaymentBookingAmount" ClientIDMode="Static" runat="server" Width="100%"
                                        MaxLength="18" Style="text-align: right;" onkeydown="AllowDecimal(this,event);"
                                        onkeypress="AllowDecimal(this,event);" onkeyup="ShowBalanceAmountInvoicePB();"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfv_txtPaymentBookingAmount" runat="server" ErrorMessage="Please Enter Amount"
                                        ControlToValidate="txtPaymentBookingAmount" ForeColor="Red" ValidationGroup="PaymentBooking"
                                        Display="None"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td>
                                </td>
                                <td>
                                </td>
                                <td>
                                    <hr />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td>
                                </td>
                                <td>
                                    Balance Outstanding :
                                </td>
                                <td>
                                    <asp:Label ID="lblBalanceAmount" runat="server" Style="text-align: right; color: Maroon;
                                        font-weight: bold;" ClientIDMode="Static"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Payment Details :
                                </td>
                                <td>
                                    <asp:TextBox ID="txtPayDetails" runat="server" Width="198px" MaxLength="200" TextMode="MultiLine"
                                        onkeyup="TextBox_KeyUp(this,'InvoicePBPayDetails','200');" ClientIDMode="Static"></asp:TextBox>
                                    <br />
                                    <span class="watermark"><span id="InvoicePBPayDetails">200</span> characters remaning
                                        out of 200 </span>
                                </td>
                                <td>
                                    Remark :
                                </td>
                                <td colspan="3">
                                    <asp:TextBox ID="txtPaymentBookingRemark" runat="server" Width="100%" MaxLength="200"
                                        TextMode="MultiLine" onkeyup="TextBox_KeyUp(this,'SpanRemakrInvoicePB','200');"
                                        ClientIDMode="Static"></asp:TextBox>
                                    <br />
                                    <span class="watermark"><span id="SpanRemakrInvoicePB">200</span> characters remaning
                                        out of 200 </span>
                                </td>
                            </tr>
                        </table>
                        <asp:HiddenField ID="hdnPaymentBookingID" runat="server" />
                        <asp:HiddenField ID="hdnPaymentBookingSequence" runat="server" />
                    </td>
                </tr>
            </table>
        </cc2:Flyout>
    </div>
    <script type="text/javascript">

        onLoad();
        function onLoad() {
            var hdnUCToolbarCurrentObject = document.getElementById("hdnUCToolbarCurrentObject");
            var hdnUCToolbarReferenceID = document.getElementById("hdnUCToolbarReferenceID");
            var QuotationID = getParameterByName("QuotationID");
            hdnUCToolbarCurrentObject.value = "Quotation";
            hdnUCToolbarReferenceID.value = QuotationID;
        }
        /*Get Customer Info*/
        function getCustomerInfo(AccountID) {
            var hdnCustomerID = document.getElementById("hdnCustomerID");
            hdnCustomerID.value = AccountID;
            setAddressTargetObjectName("Invoice");
            setContactPersonTargetObjectName("Invoice");
            PageMethods.webGetCustomerHeadDetailByCustomerID(AccountID, onSuccessGetAccountDetails, onFailedGetAccountDetails);
        }
        function onSuccessGetAccountDetails(result) {
            var txtAccountName = document.getElementById("txtAccountName");
            txtAccountName.value = result.Name;

            var txtWebSite = document.getElementById("txtWebSite");
            txtWebSite.value = "";
            if (result.WebSite != null) txtWebSite.value = result.WebSite;


            var ddlSector = document.getElementById("ddlSector");
            ddlSector.selectedIndex = -1;
            if (result.SectorID != null) { document.getElementById("ddlSector").value = result.SectorID; }

            var ddlCompanyType = document.getElementById("ddlCompanyType");
            ddlCompanyType.selectedIndex = -1;
            if (result.CustomerTypeID != null) { document.getElementById("ddlCompanyType").value = result.CustomerTypeID; }
            GvAddressInfo.refresh();
            GVContactPerson.refresh();
        }

        function onFailedGetAccountDetails() { alert("Some error occurred"); }

        /*End*/
        /*Check Duplication Invoice No.*/

        function CheckDuplicateInvoiceNo(invoker) {
            if (invoker.value != "") {
                PageMethods.PMCheckDuplicateInvoiceNo(invoker.value, OnSuccessCheckDuplicateInvoiceNo, null);
            }
        }

        function OnSuccessCheckDuplicateInvoiceNo(result) {
            if (result != "") {
                document.getElementById("txtUserInvoiceNo").value = "";
                document.getElementById("txtUserInvoiceNo").focus();
                alert(result);
            }
        }

        /*End*/

        /*Payment Booking*/
        function SetInvoicePBFooter() {
            var InvoiceTotal = parseFloat(document.getElementById("hdnCartGrandTotal").value).toFixed(2);
            document.getElementById("lblInvoiceAmountF").innerHTML = InvoiceTotal;
            if (document.getElementById("lblTotalReceivedAmountF").innerHTML == "") document.getElementById("lblTotalReceivedAmountF").innerHTML = "0.00";
            var TotalReceived = document.getElementById("lblTotalReceivedAmountF").innerHTML;
            document.getElementById("lblOutstandingAmountF").innerHTML = parseFloat(parseFloat(InvoiceTotal) - parseFloat(TotalReceived)).toFixed(2);
            document.getElementById("lblOutstandingAmount").innerHTML = parseFloat(parseFloat(InvoiceTotal) - parseFloat(TotalReceived)).toFixed(2);
            ShowBalanceAmountInvoicePB();
        }
        function ShowBalanceAmountInvoicePB() {
            if(document.getElementById("txtPaymentBookingAmount").value == "") document.getElementById("txtPaymentBookingAmount").value =0;
            document.getElementById("lblBalanceAmount").innerHTML = parseFloat(document.getElementById("lblOutstandingAmount").innerHTML).toFixed(2) - parseFloat(document.getElementById("txtPaymentBookingAmount").value);
            document.getElementById("lblBalanceAmount").innerHTML = parseFloat(document.getElementById("lblBalanceAmount").innerHTML).toFixed(2);
        }

        function clearPayBooking() {
            document.getElementById("<%= hdnPaymentBookingID.ClientID %>").value = "0";
            document.getElementById("<%= hdnPaymentBookingSequenece.ClientID %>").value = "0";
            var PaymentRecdate = document.getElementById("tdUC_PaymentBookingDate");
            var PaymentRecdateallinput = PaymentRecdate.getElementsByTagName('input');
            for (var b = 0; b < PaymentRecdateallinput.length; b++) {
                if (PaymentRecdateallinput[b].type == "text") { PaymentRecdateallinput[b].value = ""; }
            }
            document.getElementById("<%= txtPaymentBookingAmount.ClientID %>").value = "0.00";
            document.getElementById("<%= ddlMode.ClientID %>").selectedIndex = 0;
            document.getElementById("lblOutstandingAmount").innerHTML = "0.00";
            document.getElementById("<%= txtPayDetails.ClientID %>").value = "";
            document.getElementById("<%= txtPaymentBookingRemark.ClientID %>").value = "";
            SetInvoicePBFooter();
        }

        function getPayBookData(iRecordIndex) {
            document.getElementById("<%= hdnPaymentBookingID.ClientID %>").value = grvPaymentBooking.Rows[iRecordIndex].Cells["ID"].Value;
            document.getElementById("<%= hdnPaymentBookingSequenece.ClientID %>").value = grvPaymentBooking.Rows[iRecordIndex].Cells["Sequence"].Value;

            var PaymentRecdate = document.getElementById("tdUC_PaymentBookingDate");
            var PaymentRecdateallinput = PaymentRecdate.getElementsByTagName('input');
            var formatdt = (grvPaymentBooking.Rows[iRecordIndex].Cells["PaymentReceivedDate"].Value);

            for (var i = 0; i < PaymentRecdateallinput.length; i++) {
                if (PaymentRecdateallinput[i].type == "text") { PaymentRecdateallinput[i].value = formatdt.toString().substring(0, 11); }
            }
            document.getElementById("<%= txtPaymentBookingAmount.ClientID %>").value = grvPaymentBooking.Rows[iRecordIndex].Cells["PaymentAmount"].Value;
            document.getElementById("<%= ddlMode.ClientID%>").value = grvPaymentBooking.Rows[iRecordIndex].Cells["PaymentMode"].Value;
            document.getElementById("<%= txtPayDetails.ClientID %>").value = grvPaymentBooking.Rows[iRecordIndex].Cells["PaymentDetails"].Value;
            document.getElementById("<%= txtPaymentBookingRemark.ClientID %>").value = grvPaymentBooking.Rows[iRecordIndex].Cells["Remark"].Value;
            document.getElementById("lblOutstandingAmount").innerHTML = document.getElementById("lblOutstandingAmountF").innerHTML;
            <%=FlyoutPaymentBooking.getClientID()%>.Open(); 
        }

        function TempSaveInvoicePB() {
            var obj = new Object();
            obj.ID = document.getElementById("<%= hdnPaymentBookingID.ClientID %>").value;
            obj.Sequence = document.getElementById("<%= hdnPaymentBookingSequenece.ClientID %>").value;

            obj.PaymentAmount = parseFloat(document.getElementById("<%= txtPaymentBookingAmount.ClientID %>").value).toFixed(2);
            obj.PaymentMode = document.getElementById("<%= ddlMode.ClientID %>").value;
            obj.PaymentDetails = document.getElementById("<%= txtPayDetails.ClientID %>").value;
            obj.Remark = document.getElementById("<%= txtPaymentBookingRemark.ClientID %>").value;

            var paymentdate = document.getElementById("tdUC_PaymentBookingDate");
            var paymentdateallinput = paymentdate.getElementsByTagName('input');
            var txtDateValue = "";
            for (var i = 0; i < paymentdateallinput.length; i++) {
                if (paymentdateallinput[i].type == "text") { obj.PaymentReceivedDate = paymentdateallinput[i].value; txtDateValue = paymentdateallinput[i].value; }
            }

            if (txtDateValue == "") {
                alert("Select date");
            }
            else if (obj.PaymentMode == "0") {
                document.getElementById("<%= ddlMode.ClientID %>").focus();
                alert("Select payment mode");
            }
            else if(parseFloat(document.getElementById("lblBalanceAmount").innerHTML) < 0){
                document.getElementById("txtPaymentBookingAmount").select();
                alert("Payment booking amount should not be greater than Outstanding Amount");
            }
            else {
                LoadingOn();
                PageMethods.PMSaveTempDataOfPaymentBooking(obj.Sequence.toString(), obj, TempSaveInvoicePB_OnSuccess, TempSaveInvoicePB_OnFail);
            }
    }

    function TempSaveInvoicePB_OnSuccess(result) {
        if (result[0] == "true") {
            if (document.getElementById("<%= hdnPaymentBookingID.ClientID %>").value = "0") alert("Payment booking added successfully");
                if (document.getElementById("<%= hdnPaymentBookingID.ClientID %>").value != "0") alert("Payment booking updated successfully");
                clearPayBooking();
                document.getElementById("lblTotalReceivedAmountF").innerHTML = parseFloat(result[1]).toFixed(2);
                SetInvoicePBFooter();
                grvPaymentBooking.refresh();
                LoadingOff();
                <%=FlyoutPaymentBooking.getClientID()%>.Close(); 
            }
            if (result == "false") { alert("Some error occurred"); }
        }

        function TempSaveInvoicePB_OnFail() { alert("Some error occurred"); }
        /*End*/


        /*Payment schedule*/
        function clearPaySchedule() {
            document.getElementById("<%= hdnPaymentScheduleSequenece.ClientID %>").value = "0";
            document.getElementById("<%= hdnPaymentScheduleID.ClientID %>").value = "0";
            var chkbxEmail = document.getElementById("<%= chkbxEmail.ClientID %>");
            chkbxEmail.checked = false;

            var chkbxSMS = document.getElementById("<%= chkbxSMS.ClientID %>");
            chkbxSMS.checked = false;

            var chkCustomerAlert = document.getElementById("<%= chkCustomerAlert.ClientID %>");
            chkCustomerAlert.checked = false;

            document.getElementById("<%= hdnPaymentScheduleID.ClientID %>").value = "";
            document.getElementById("<%= txtPaymentScheduleRemarks.ClientID %>").value = "";
            document.getElementById("<%= txtAmount.ClientID %>").value = "";
            var paymentdate = document.getElementById("tdUC_PayemntDate");
            var paymentdateallinput = paymentdate.getElementsByTagName('input');
            for (var i = 0; i < paymentdateallinput.length; i++) {
                if (paymentdateallinput[i].type == "text") { paymentdateallinput[i].value = ""; }
            }
            var alertdate = document.getElementById("tdUC_UCAlertDate");
            var alertdateallinput = alertdate.getElementsByTagName('input');
            for (var i = 0; i < alertdateallinput.length; i++) {
                if (alertdateallinput[i].type == "text") { alertdateallinput[i].value = ""; }
            }
        }


        function getPaySheData(iRecordIndex) {
            document.getElementById("<%= hdnPaymentScheduleID.ClientID %>").value = grvPaymentSchedule.Rows[iRecordIndex].Cells["ID"].Value;
            document.getElementById("<%= hdnPaymentScheduleSequenece.ClientID %>").value = grvPaymentSchedule.Rows[iRecordIndex].Cells["Sequence"].Value;

            var paymentdate = document.getElementById("tdUC_PayemntDate");
            var paymentdateallinput = paymentdate.getElementsByTagName('input');
            var formatdt = (grvPaymentSchedule.Rows[iRecordIndex].Cells['PaymentScheduleDate'].Value);
            for (var i = 0; i < paymentdateallinput.length; i++) {
                if (paymentdateallinput[i].type == "text") {
                    if (formatdt != "") { paymentdateallinput[i].value = formatdt.toString().substring(0, 11); }
                    else { paymentdateallinput[i].value = ""; }
                }
            }

            var alertdate = document.getElementById("tdUC_UCAlertDate");
            var alertdateallinput = alertdate.getElementsByTagName('input');
            formatdt = (grvPaymentSchedule.Rows[iRecordIndex].Cells['AlertDate'].Value);
            for (var i = 0; i < alertdateallinput.length; i++) {
                if (alertdateallinput[i].type == "text") {
                    if (formatdt != "") { alertdateallinput[i].value = formatdt.toString().substring(0, 11); }
                    else { alertdateallinput[i].value = ""; }
                }
            }

            document.getElementById("<%= txtAmount.ClientID %>").value = grvPaymentSchedule.Rows[iRecordIndex].Cells['PaymentScheduleAmount'].Value;
            var chkbxEmail = document.getElementById("<%= chkbxEmail.ClientID %>");
            chkbxEmail.checked = false;
            if (grvPaymentSchedule.Rows[iRecordIndex].Cells['AlertEmail'].Value == "True") { chkbxEmail.checked = true; }

            var chkbxSMS = document.getElementById("<%= chkbxSMS.ClientID %>");
            chkbxSMS.checked = false;
            if (grvPaymentSchedule.Rows[iRecordIndex].Cells['AlertSMS'].Value == "True") { chkbxSMS.checked = true; }

            var chkCustomerAlert = document.getElementById("<%= chkCustomerAlert.ClientID %>");
            chkCustomerAlert.checked = false;
            if (grvPaymentSchedule.Rows[iRecordIndex].Cells['CustomerAlert'].Value == "True") { chkCustomerAlert.checked = true; }

            document.getElementById("<%= txtPaymentScheduleRemarks.ClientID %>").value = grvPaymentSchedule.Rows[iRecordIndex].Cells['Remark'].Value;
            <%=FlyoutPaymentSchedule.getClientID()%>.Open(); 
        }


        function TempSaveInvoicePS() {
            var obj = new Object();

            obj.ID = document.getElementById("<%= hdnPaymentScheduleID.ClientID %>").value;
            obj.Sequence = document.getElementById("<%= hdnPaymentScheduleSequenece.ClientID %>").value;

            var chkbxEmail = document.getElementById("<%= chkbxEmail.ClientID %>");
            obj.AlertEmail = chkbxEmail.checked.toString();

            var chkbxSMS = document.getElementById("<%= chkbxSMS.ClientID %>");
            obj.AlertSMS = chkbxSMS.checked.toString();

            var chkCustomerAlert = document.getElementById("<%= chkCustomerAlert.ClientID %>");
            obj.CustomerAlert = chkCustomerAlert.checked.toString();

            obj.Remark = document.getElementById("<%= txtPaymentScheduleRemarks.ClientID %>").value;

            obj.PaymentScheduleAmount = parseFloat(document.getElementById("<%= txtAmount.ClientID %>").value).toFixed(2);

            var paymentdate = document.getElementById("tdUC_PayemntDate");
            var paymentdateallinput = paymentdate.getElementsByTagName('input');
            for (var i = 0; i < paymentdateallinput.length; i++) {
                if (paymentdateallinput[i].type == "text") { obj.PaymentScheduleDate = paymentdateallinput[i].value; }
            }
            var alertdate = document.getElementById("tdUC_UCAlertDate");

            var alertdateallinput = alertdate.getElementsByTagName('input');
            for (var a = 0; a < alertdateallinput.length; a++) {
                if (alertdateallinput[a].type == "text") { obj.AlertDate = alertdateallinput[a].value; }
            }
            PageMethods.PMSaveTempDataOfPaymentSchedule(obj.Sequence.toString(), obj, TempSaveInvoicePS_OnSuccess, TempSaveInvoicePS_OnFail);
        }

        function TempSaveInvoicePS_OnSuccess(result) {
            if (result == "true") {
                if (document.getElementById("<%= hdnPaymentScheduleSequenece.ClientID %>").value = "0") alert("Payment schedule added successfully");
                if (document.getElementById("<%= hdnPaymentScheduleSequenece.ClientID %>").value != "0") alert("Payment schedule updated successfully");
                clearPaySchedule();
                grvPaymentSchedule.refresh();
                <%=FlyoutPaymentSchedule.getClientID()%>.Close(); 
            }
            else if (result == "false") { alert("Some error occurred"); }
        }

        function TempSaveInvoicePS_OnFail() { alert("Some error occurred"); }
        /*End*/

        /* get TItle Record*/
        function getTitleRecord(TitleId) {
            var hdnTitleSearchID = document.getElementById("hdnTitleSearchID");
            hdnTitleSearchID.value = TitleId;
            PageMethods.GetTitleRecordByTitleID(TitleId, onSuccessGetTitleRecord, onFailedGetTitleRecord);
        }

        function onSuccessGetTitleRecord(result) {
            var txtTitle = document.getElementById("<%= txtTitle.ClientID %>");           
           txtTitle.value = result.Title;
           
       }

       function onFailedGetTitleRecord() {alert("Some error occurred");}

       function SetUnArchive(Ids, IsArchive) {
           PageMethods.PMMoveAddressToArchive(Ids, IsArchive, null, null);
       }

        
       function SetUnArchiveConatactPerson(Ids, IsArchive) {
           PageMethods.PMMoveContactPersonToArchive(Ids, IsArchive, null, null);
       }
    </script>
</asp:Content>

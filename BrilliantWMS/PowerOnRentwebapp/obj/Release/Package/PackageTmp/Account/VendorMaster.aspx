<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/CRM.Master" AutoEventWireup="true" CodeBehind="VendorMaster.aspx.cs" 
    Inherits="BrilliantWMS.Account.VendorMaster" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../ContactPerson/UCContactPerson.ascx" TagName="UCContactPerson"
    TagPrefix="uc1" %>
<%--<%@ Register Src="../Competitor/UC_CompetitorDetail.ascx" TagName="UC_CompetitorDetail" TagPrefix="uc3" %>--%>
<%@ Register Src="../Document/UC_AttachDocument.ascx" TagName="UC_AttachDocument"
    TagPrefix="uc5" %>
<%@ Register Src="../CommonControls/UC_Date.ascx" TagName="UC_Date" TagPrefix="uc6" %>
<%@ Register Assembly="obout_Interface" Namespace="Obout.Interface" TagPrefix="obout" %>
<%@ Register Assembly="obout_Grid_NET" Namespace="Obout.Grid" TagPrefix="obout" %>
<%@ Register Src="../CommonControls/UCFormHeader.ascx" TagName="UCFormHeader" TagPrefix="uc7" %>
<%@ Register Src="../CommonControls/UCToolbar.ascx" TagName="UCToolbar" TagPrefix="uc8" %>
<%@ Register Src="../Tax/UC_StatutoryDetails.ascx" TagName="UC_StatutoryDetails"
    TagPrefix="uc2" %>
<%@ Register Src="../Address/UCAddress.ascx" TagName="UCAddress" TagPrefix="uc4" %>
<%--<%@ Register Src="../Invoice/UCPaymentDetail.ascx" TagName="UCPaymentDetail" TagPrefix="uc10" %>--%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHolder_FormHeader" runat="server">
     <uc8:UCToolbar ID="UCToolbar1" runat="server" />
    <uc7:UCFormHeader ID="UCFormHeader1" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPHolder_Form" runat="server">
    <asp:ValidationSummary ID="validationsummary_AccountMaster" runat="server" ShowMessageBox="true"
        ShowSummary="false" DisplayMode="bulletlist" ValidationGroup="Save" />
    <asp:UpdatePanel ID="UpdatePanelTabPanelCompanyList" runat="server">
        <ContentTemplate>
            <asp:TabContainer runat="server" ID="tabAccountMaster" ActiveTabIndex="1" Width="100%">
                <asp:TabPanel ID="TabCustomerList" runat="server" HeaderText="Vendor List" TabIndex="1">
                    <ContentTemplate>
                        <asp:UpdateProgress ID="UpdateProgressUC_CustomerList" runat="server" AssociatedUpdatePanelID="upnl_customerlist">
                            <ProgressTemplate>
                                <center>
                                    <div class="modal">
                                        <img src="../App_Themes/Blue/img/ajax-loader.gif" style="top: 50%;" />
                                    </div>
                                </center>
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                        <asp:UpdatePanel ID="upnl_customerlist" runat="server">
                            <ContentTemplate>
                                <asp:HiddenField ID="HdnAccountId" runat="server"></asp:HiddenField>
                                <asp:HiddenField ID="HdnOpeningBalId" runat="server" />
                                <asp:HiddenField ID="hdnCompanyid" runat="server" ClientIDMode="Static" />
                                <asp:HiddenField ID="hdncustomerid" runat="server" ClientIDMode="Static" />

                                <center>
                                    <table class="gridFrame" style="width: 100%">
                                        <tr>
                                            <td>
                                                <table style="width: 100%">
                                                    <tr>
                                                        <td style="text-align: left;">
                                                            <a id="headerText">Vendor List</a>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <obout:Grid ID="GvCustomer" runat="server" AllowAddingRecords="False" AllowFiltering="True"
                                                    AllowGrouping="True" AutoGenerateColumns="False" Width="100%" OnSelect="GvCustomer_Select">
                                                    <Columns>
                                                        <obout:Column ID="Column1" AllowFilter="False" TemplateId="imgBtnEdit1" Index="1"
                                                            Width="5%" HeaderText="Edit">
                                                            <TemplateSettings TemplateId="imgBtnEdit1" />
                                                        </obout:Column>
                                                         <obout:Column DataField="ID" HeaderText="ID" Width="15%" Index="9" Visible="False">
                                                        </obout:Column>
                                                        <obout:Column DataField="Name" HeaderText="Vendor Name" Width="15%" Index="2">
                                                        </obout:Column>
                                                        <obout:Column DataField="Code" HeaderText="Vendor Code" Width="12%" Index="3">
                                                        </obout:Column>
                                                        <obout:Column DataField="Value" HeaderText="Vendor Type" Width="7%" Index="4">
                                                        </obout:Column>
                                                         <obout:Column DataField="Customer" HeaderText="Customer" Width="8%" Index="5">
                                                        </obout:Column>
                                                        <obout:Column DataField="Company" HeaderText="Company" Width="8%" Index="5">
                                                        </obout:Column>
                                                        <obout:Column DataField="Active" HeaderText="Active" Width="5%" Index="6">
                                                        </obout:Column>
                                                       
                                                    </Columns>
                                                    <Templates>
                                                        <obout:GridTemplate ID="imgBtnEdit1" runat="server">
                                                            <Template>
                                                                <asp:ImageButton ID="imgBtnEdit" runat="server" ImageUrl="../App_Themes/Blue/img/Edit16.png"
                                                                    CausesValidation="false" Style="height: 16px" ToolTip="Edit" /></Template>
                                                        </obout:GridTemplate>
                                                    </Templates>
                                                </obout:Grid>
                                            </td>
                                        </tr>
                                    </table>
                                </center>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </ContentTemplate>
                </asp:TabPanel>
                <asp:TabPanel ID="tabAccountInfo" runat="server" HeaderText="Vendor Info" TabIndex="2">
                    <ContentTemplate>
                        <center>
                            <table class="tableForm">
                                <tr>
                                    <td>
                                       <req><asp:Label ID="lblCompany" runat="server" Text="Company"></asp:Label></req>
                                    </td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="ddlcompany" Width="206px" DataValueField="ID" DataTextField="Name" onchange="GetCustomer()" ValidationGroup="Save">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="reqValCompany" runat="server" ControlToValidate="ddlcompany"
                                            ErrorMessage="Please Select Company" Display="None" InitialValue="0" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                    </td>
                                   <td>
                                        <req><asp:Label ID="lblcustomer" runat="server" Text="Customer"></asp:Label></req>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlcustomer" runat="server" Width="206px" DataTextField="Name" DataValueField="ID" onchange="GetCustomerID()">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="reqValCustomer" runat="server" ControlToValidate="ddlcustomer"
                                            ErrorMessage="Please Select Customer" Display="None" InitialValue="0" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <req> Vendor Name :</req>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_custname" MaxLength="50" Width="200px" runat="server" ValidationGroup="Save"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="reqtxt_custname" runat="server" ErrorMessage="Enter Customer Name"
                                            ControlToValidate="txt_custname" Display="None" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                    </td>
                                    <td>
                                        Vendor Code :
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_custcode" Width="200px" MaxLength="20" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <req> Sector :</req>
                                    </td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="Ddl_Sector" Width="206px" DataValueField="ID"
                                            DataTextField="Name" ValidationGroup="Save">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="ValRf_Ddl_Sector" runat="server" ControlToValidate="Ddl_Sector"
                                            ErrorMessage="Please Select Sector" Display="None" InitialValue="0" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                    </td>
                                   <td>
                                       <req> Vendor Types :</req>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlvendortype" runat="server" Width="206px"  DataTextField="Value" DataValueField="Id">
                                        </asp:DropDownList>
                                         <asp:RequiredFieldValidator ID="reqValVendorType" runat="server" ControlToValidate="ddlvendortype"
                                            ErrorMessage="Please Select Vendor Type" Display="None" InitialValue="0" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                    </td>


                                </tr>
                                <tr>
                                    <td>
                                        Credit days :
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_CreditDays" runat="server" class="medium text" MaxLength="15" style="text-align:right" onkeypress="return fnAllowDigitsOnly(event)"
                                            Width="200px"></asp:TextBox>
                                        <asp:RegularExpressionValidator ID="rev_txtFixedDisc" runat="server" ErrorMessage="Enter valid Credit Days"
                                            ControlToValidate="txt_CreditDays" ValidationExpression="[0-9]+(\.[0-9][0-9]?)?"
                                            ValidationGroup="Save" Display="None"></asp:RegularExpressionValidator>
                                    </td>
                                    <td>
                                        Turn Over(Rs.):
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_turnOver" Width="200px" MaxLength="15" runat="server" style="text-align:right" onkeypress="return fnAllowDigitsOnly(event)"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Active :
                                    </td>
                                    <td style="text-align: left">
                                        <obout:OboutRadioButton ID="rbtnYes" runat="server" Checked="True" FolderStyle=""
                                            GroupName="rbtnActive" Text="Yes">
                                        </obout:OboutRadioButton>
                                        <obout:OboutRadioButton ID="rbtnNo" runat="server" FolderStyle="" GroupName="rbtnActive"
                                            Text="No">
                                        </obout:OboutRadioButton>
                                    </td>
                                    <td>
                                        Website :
                                    </td>
                                    <td>
                                        <%-- <asp:TextBox ID="txt_displyName" MaxLength="50" Width="200px" runat="server"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="req_txt_displyName" runat="server" ErrorMessage="Enter Display Name"
                                        ControlToValidate="txt_displyName" Display="None" ValidationGroup="Save"></asp:RequiredFieldValidator>--%>
                                        <asp:TextBox ID="TxtWebsite" runat="server" Width="200px" MaxLength="50" ControlsToEnable=""></asp:TextBox>
                                        <asp:RegularExpressionValidator ID="req_txtWebSite" runat="server" ErrorMessage="Please Enter Valid Website Address"
                                            ValidationExpression="(http(s)?://)?([\w-]+\.)+[\w-]+(/[\w- ;,./?%&=]*)?" ControlToValidate="TxtWebsite"
                                            Display="None"></asp:RegularExpressionValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <hr />
                                        <asp:RequiredFieldValidator ID="reqtxtOpeningBalance" runat="server" ControlToValidate="txt_OpeningBalance"
                                            Display="None" ErrorMessage="Please Enter Opening Balance" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator ID="reqddl_DrCr" runat="server" ControlToValidate="ddl_DrCr"
                                            Display="None" ErrorMessage="Please Select Account Opening Type" ValidationGroup="Save"
                                            InitialValue="0"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <req>Financial Year :</req>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddl_FinancialYr" runat="server" Width="206px">
                                            <asp:ListItem Text="-Select-" Value="0"></asp:ListItem>
                                            <asp:ListItem Text="2015-2016" Value="2015-2016"></asp:ListItem>
                                            <asp:ListItem Text="2016-2017" Value="2016-2017"></asp:ListItem>
                                            <asp:ListItem Text="2017-2018" Value="2017-2018"></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="reqddlFinancialYr" runat="server" ControlToValidate="ddl_FinancialYr"
                                            InitialValue="0" ErrorMessage="Please Select Financial Year" Display="None" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                    </td>
                                    <td>
                                        <req>Opening Balance :</req>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_OpeningBalance" runat="server" Width="128px" style="text-align:right" onkeypress="return fnAllowDigitsOnly(event)"></asp:TextBox>
                                        <asp:DropDownList ID="ddl_DrCr" runat="server" Width="70px">
                                            <asp:ListItem Text="-Select-" Value="0"></asp:ListItem>
                                            <asp:ListItem Text="Dr" Value="Dr"> </asp:ListItem>
                                            <asp:ListItem Text="Cr" Value="Cr"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                     <td>
                                        Company Types :
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="Ddl_CompanyType" runat="server" Width="206px" DataValueField="ID"
                                            DataTextField="CompanyType">
                                        </asp:DropDownList>
                                    </td>
                                    <td></td>
                                     <td></td>
                                </tr>
                            </table>
                        </center>
                    </ContentTemplate>
                </asp:TabPanel>
                <asp:TabPanel ID="tabAddressInfo" runat="server" HeaderText="Address Info" TabIndex="3">
                    <ContentTemplate>
                        <uc4:UCAddress ID="UCAddress1" runat="server" />
                    </ContentTemplate>
                </asp:TabPanel>
                <asp:TabPanel ID="tabContactInfo" runat="server" HeaderText="Contact Person Info" TabIndex="4">
                    <ContentTemplate>
                        <uc1:UCContactPerson ID="UCContactPerson1" runat="server" />
                    </ContentTemplate>
                </asp:TabPanel>
               <%-- <asp:TabPanel ID="tabSatutoryInfo" runat="server" HeaderText="Statutory Info" TabIndex="5">
                    <ContentTemplate>
                        <center>
                            <uc2:UC_StatutoryDetails ID="UC_StatutoryDetails1" runat="server" />
                        </center>
                    </ContentTemplate>
                </asp:TabPanel>--%>
                <asp:TabPanel ID="tabAttachedDocumentInfo" runat="server" HeaderText="Document" TabIndex="5">
                    <ContentTemplate>
                        <uc5:UC_AttachDocument ID="UC_AttachDocument1" runat="server" />
                    </ContentTemplate>
                </asp:TabPanel>
                 <asp:TabPanel runat="server" ID="tabRateCard" HeaderText="Rate Card">
                    <ContentTemplate>
                        <center>
                            <%--<table class="tableForm" border="2" style="width: 700px;">--%>
                             <table class="gridFrame" width="100%">
                                <tr>
                                    <td>
                                        <asp:Label ID="lblratecard" CssClass="headerText" runat="server" Text="Rate Card List" />
                                    </td>
                                    <td align="right">
                                         <%-- <asp:Button ID="btnvirtualQty" runat="server" OnClientClick="openvirtualQty()" Text=" Add New" />--%>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <obout:Grid ID="grdratecard" runat="server" CallbackMode="true" Serialize="true"
                                            AllowColumnReordering="true" AllowColumnResizing="true" AutoGenerateColumns="false"
                                            AllowPaging="false" ShowLoadingMessage="true" AllowSorting="false" AllowManualPaging="false"
                                            AllowRecordSelection="false" ShowFooter="false" Width="100%" PageSize="-1" >
                                            <ClientSideEvents ExposeSender="true" />
                                            <Columns>
                                                <obout:Column DataField="ID" Visible="false" Width="10%">
                                                </obout:Column>
                                                <%-- <obout:Column DataField="Territory" HeaderText="Deartment" Width="15%">
                                                    <TemplateSettings TemplateId="GridTemplate1" />
                                                </obout:Column>--%>
                                                <obout:Column DataField="RateType" HeaderText="Rate Type" Align="Center" HeaderAlign="center" Width="15%">
                                                </obout:Column>
                                                <obout:Column DataField="RateDetails" HeaderText="Rate Details" Align="Center" HeaderAlign="center" Width="15%">
                                                </obout:Column>
                                                 <obout:Column DataField="Rate" HeaderText="Rate" Align="right" HeaderAlign="center" Width="10%">
                                                </obout:Column>
                                                 <obout:Column DataField="FromDate" HeaderText="From Date" Align="Center" HeaderAlign="center" Width="15%" DataFormatString="{0:dd-MM-yyyy}">
                                                </obout:Column>
                                                 <obout:Column DataField="ToDate" HeaderText="To Date" Align="Center" HeaderAlign="center" Width="15%" DataFormatString="{0:dd-MM-yyyy}">
                                                </obout:Column>
                                                <obout:Column DataField="EffDate" HeaderText="Effective date" Width="15%" Align="Center" HeaderAlign="center" DataFormatString="{0:dd-MM-yyyy}">
                                                </obout:Column>
                                                <obout:Column DataField="Active" HeaderText="Active" Width="15%" Align="Center" HeaderAlign="center" >
                                                </obout:Column>
                                            </Columns>
                                           
                                        </obout:Grid>
                                    </td>
                                </tr>
                            </table>
                        </center>
                    </ContentTemplate>
                </asp:TabPanel>
                 <asp:TabPanel runat="server" ID="tabInvoice" HeaderText="Invoice">
                    <ContentTemplate>
                        <center>
                            <%--<table class="tableForm" border="2" style="width: 700px;">--%>
                             <table class="gridFrame" width="100%">
                                <tr>
                                    <td>
                                        <asp:Label ID="Label1" CssClass="headerText" runat="server" Text="Invoice List" />
                                    </td>
                                    <td align="right">
                                         <%-- <asp:Button ID="btnvirtualQty" runat="server" OnClientClick="openvirtualQty()" Text=" Add New" />--%>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <obout:Grid ID="grdInvoice" runat="server" CallbackMode="true" Serialize="true"
                                            AllowColumnReordering="true" AllowColumnResizing="true" AutoGenerateColumns="false"
                                            AllowPaging="false" ShowLoadingMessage="true" AllowSorting="false" AllowManualPaging="false"
                                            AllowRecordSelection="false" ShowFooter="false" Width="100%" PageSize="-1">
                                            <ClientSideEvents ExposeSender="true" />
                                            <Columns>
                                                <obout:Column DataField="ID" Visible="false" Width="10%">
                                                </obout:Column>
                                                <%-- <obout:Column DataField="Territory" HeaderText="Deartment" Width="15%">
                                                    <TemplateSettings TemplateId="GridTemplate1" />
                                                </obout:Column>--%>
                                                <obout:Column DataField="OpeningStock1" HeaderText="Invoice No." Align="Center" HeaderAlign="center" Width="15%">
                                                </obout:Column>
                                                <obout:Column DataField="OpeningStock2" HeaderText="Title" Align="Center" HeaderAlign="center" Width="15%">
                                                </obout:Column>
                                                 <obout:Column DataField="OpeningStock3" HeaderText="Invoice Date" Align="Center" HeaderAlign="center" Width="15%">
                                                </obout:Column>
                                                 <obout:Column DataField="OpeningStoc4" HeaderText="Invoice Amount" Align="Center" HeaderAlign="center" Width="15%">
                                                </obout:Column>
                                                 <obout:Column DataField="OpeningStock" HeaderText="Status" Align="Center" HeaderAlign="center" Width="15%">
                                                </obout:Column>
                                                <obout:Column DataField="TotalReceiveQty" HeaderText="Invoice Type" Width="15%" Align="Center" HeaderAlign="center">
                                                </obout:Column>
                                            </Columns>                                        
                                        </obout:Grid>
                                    </td>
                                </tr>
                            </table>
                        </center>
                    </ContentTemplate>
                </asp:TabPanel>
                <asp:TabPanel runat="server" ID="tabpaymenthistory" HeaderText="Payment History">
                    <ContentTemplate>
                        <center>
                            <%--<table class="tableForm" border="2" style="width: 700px;">--%>
                             <table class="gridFrame" width="100%">
                                <tr>
                                    <td>
                                        <asp:Label ID="Label2" CssClass="headerText" runat="server" Text="Payment History" />
                                    </td>
                                    <td align="right">
                                         <%-- <asp:Button ID="btnvirtualQty" runat="server" OnClientClick="openvirtualQty()" Text=" Add New" />--%>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <obout:Grid ID="grdpayhistory" runat="server" CallbackMode="true" Serialize="true"
                                            AllowColumnReordering="true" AllowColumnResizing="true" AutoGenerateColumns="false"
                                            AllowPaging="false" ShowLoadingMessage="true" AllowSorting="false" AllowManualPaging="false"
                                            AllowRecordSelection="false" ShowFooter="false" Width="100%" PageSize="-1">
                                            <ClientSideEvents ExposeSender="true" />
                                            <Columns>
                                                <obout:Column DataField="ID" Visible="false" Width="10%">
                                                </obout:Column>
                                                <%-- <obout:Column DataField="Territory" HeaderText="Deartment" Width="15%">
                                                    <TemplateSettings TemplateId="GridTemplate1" />
                                                </obout:Column>--%>
                                                <obout:Column DataField="OpeningStock1" HeaderText="Invoice No." Align="Center" HeaderAlign="center" Width="15%">
                                                </obout:Column>
                                                <obout:Column DataField="OpeningStock2" HeaderText="Invoice Amount" Align="Center" HeaderAlign="center" Width="15%">
                                                </obout:Column>
                                                <obout:Column DataField="OpeningStoc4" HeaderText="Paid Amount" Align="Center" HeaderAlign="center" Width="15%">
                                                </obout:Column>
                                                <obout:Column DataField="OpeningStoc5" HeaderText="Outstanding Amount" Align="Center" HeaderAlign="center" Width="15%">
                                                </obout:Column>
                                                 <obout:Column DataField="OpeningStock6" HeaderText="Paid Date" Align="Center" HeaderAlign="center" Width="15%">
                                                </obout:Column>
                                                 <obout:Column DataField="OpeningStock" HeaderText="Payment Mode" Align="Center" HeaderAlign="center" Width="15%">
                                                </obout:Column>
                                            </Columns>                                        
                                        </obout:Grid>
                                    </td>
                                </tr>
                            </table>
                        </center>
                    </ContentTemplate>
                </asp:TabPanel>
                <%--  <asp:TabPanel ID="tabCompetitorInfo" runat="server" HeaderText="Competitor Analysis"
                    TabIndex="7">
                    <ContentTemplate>
                        <uc3:UC_CompetitorDetail ID="UC_CompetitorDetail1" runat="server"></uc3:UC_CompetitorDetail>
                        <asp:HiddenField ID="HdnAccountId" runat="server" />
                    </ContentTemplate>
                </asp:TabPanel>--%>
              <%--  <asp:TabPanel ID="tabAccountHistory" runat="server" HeaderText="Account Details">
                    <ContentTemplate>
                        <center>
                           <uc10:UCPaymentDetail ID="UCPaymentDetail1" runat="server" />
                        </center>
                    </ContentTemplate>
                </asp:TabPanel>--%>
            </asp:TabContainer>
        </ContentTemplate>
    </asp:UpdatePanel>
     <script type="text/javascript">
         var ddlcompany = document.getElementById("<%=ddlcompany.ClientID %>");
         var ddlcustomer = document.getElementById("<%=ddlcustomer.ClientID %>");

         function GetCustomer() {
             var obj1 = new Object();
             obj1.ddlcompanyId = ddlcompany.value.toString();
             document.getElementById("<%=hdnCompanyid.ClientID %>").value = ddlcompany.value.toString();
            PageMethods.GetCustomerByComp(obj1, getCust_onSuccessed);
        }

        function getCust_onSuccessed(result) {

            ddlcustomer.options.length = 0;
            for (var i in result) {
                AddOption(result[i].Name, result[i].Id);
            }
        }

        function AddOption(text, value) {

            var option = document.createElement('option');
            option.value = value;
            option.innerHTML = text;
            ddlcustomer.options.add(option);
        }

        function GetCustomerID() {
            document.getElementById("<%=hdncustomerid.ClientID %>").value = ddlcustomer.value.toString();
        }

         function fnAllowDigitsOnly(key) {
             var keycode = (key.which) ? key.which : key.keyCode;
             if ((keycode < 48 || keycode > 57) && (keycode != 8) && (keycode != 9) && (keycode != 11) && (keycode != 37) && (keycode != 38) && (keycode != 39) && (keycode != 40) && (keycode != 127)) {
                 return false;
             }
         }
    </script>
</asp:Content>

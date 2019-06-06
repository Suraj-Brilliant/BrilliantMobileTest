<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/CRM.Master" AutoEventWireup="true" CodeBehind="RateCardmaster.aspx.cs" EnableEventValidation="false"
     Inherits="BrilliantWMS.Account.RateCardmaster" Theme="Blue" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="Obout.Ajax.UI" Namespace="Obout.Ajax.UI.FileUpload" TagPrefix="obout" %>
<%@ Register Src="../ContactPerson/UCContactPerson.ascx" TagName="UCContactPerson" TagPrefix="uc1" %>
<%@ Register Src="../Document/UC_AttachDocument.ascx" TagName="UC_AttachDocument" TagPrefix="uc5" %>
<%@ Register Src="../CommonControls/UC_Date.ascx" TagName="UC_Date" TagPrefix="uc6" %>
<%@ Register Assembly="obout_Interface" Namespace="Obout.Interface" TagPrefix="obout" %>
<%@ Register Assembly="obout_Grid_NET" Namespace="Obout.Grid" TagPrefix="obout" %>
<%@ Register Src="../CommonControls/UCFormHeader.ascx" TagName="UCFormHeader" TagPrefix="uc7" %>
<%@ Register Src="../CommonControls/UCToolbar.ascx" TagName="UCToolbar" TagPrefix="uc8" %>
<%@ Register Src="../Address/UCAddress.ascx" TagName="UCAddress" TagPrefix="uc4" %>
<%@ Register Src="../CommonControls/UC_Date.ascx" TagName="UC_Date" TagPrefix="uc9" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHolder_FormHeader" runat="server">
    <uc8:UCToolbar ID="UCToolbar1" runat="server" />
    <uc7:UCFormHeader ID="UCFormHeader1" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPHolder_Form" runat="server">
     <script src="../MasterPage/JavaScripts/countries.js" type="text/javascript"></script>
    <asp:ValidationSummary ID="validationsummary_AccountMaster" runat="server" ShowMessageBox="true"
        ShowSummary="false" DisplayMode="bulletlist" ValidationGroup="Save" />
    <%--<asp:UpdatePanel ID="UpdatePanelTabPanelCompanyList" runat="server">--%>
    <%-- <ContentTemplate>--%>
    <center>
        <asp:TabContainer runat="server" ID="tabAccountMaster" ActiveTabIndex="2" Width="100%">
            <asp:TabPanel ID="TabCustomerList" runat="server" HeaderText="RateCard List">
                <ContentTemplate>
                    <%-- <asp:UpdateProgress ID="UpdateProgressUC_CustomerList" runat="server" AssociatedUpdatePanelID="upnl_customerlist">
                            <ProgressTemplate>
                                <center>
                                    <div class="modal">
                                        <img src="../App_Themes/Blue/img/ajax-loader.gif" style="top: 50%;" />
                                    </div>
                                </center>
                            </ProgressTemplate>
                        </asp:UpdateProgress>--%>
                    <%-- <asp:UpdatePanel ID="upnl_customerlist" runat="server">
                            <ContentTemplate>--%>
                    <asp:HiddenField ID="hdnselectedRec" runat="server" ClientIDMode="Static" />
                    <asp:HiddenField ID="hndCompanyid" runat="server"></asp:HiddenField>
                    <asp:HiddenField ID="hndState" runat="server" ClientIDMode="Static"></asp:HiddenField>
                    <asp:HiddenField ID="hdnmodestate" runat="server" ClientIDMode="Static" />
                    <asp:HiddenField ID="hdnAccountName" runat="server" ClientIDMode="Static" />
                    <asp:HiddenField ID="hdnAccountID" runat="server" ClientIDMode="Static" />
                    <asp:HiddenField ID="hdnratecardId" runat="server" ClientIDMode="Static" />
                    
                    <center>
                        <table class="gridFrame" style="width: 100%">
                            <tr>
                                <td>
                                    <table style="width: 100%">
                                        <tr>
                                            <td style="text-align: left;">
                                                <a id="headerText">
                                                    <asp:Label ID="lblheasertext" runat="server" Text="RateCard List"></asp:Label></a>
                                            </td>
                                            <td style="text-align: right;">
                                                <%-- <input type="button" value="Book Payment" id="btnPayment" onclick="selectedRecordBookPayment();" />
                                                <input type="button" value="Send Credentials" id="btnSendMail" onclick="selectedRecordSendMail();" />--%>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <obout:Grid ID="grdratecard" runat="server" AutoGenerateColumns="False" AllowAddingRecords="False"
                                        AllowGrouping="True" AllowFiltering="True" Width="100%" OnSelect="grdratecard_Select" >
                                        <ScrollingSettings ScrollHeight="250" />
                                        <Columns>
                                            <obout:Column ID="Edit" Width="4%" AllowFilter="False" HeaderText="Edit" Index="0"
                                                TemplateId="GvTempEdit">
                                                <TemplateSettings TemplateId="GvTempEdit" />
                                            </obout:Column>
                                            <obout:Column DataField="ID" Visible="False" HeaderText="ID" Index="1">
                                            </obout:Column>
                                             <obout:Column DataField="RateDetails" HeaderText="Rate Details" Width="16%" Index="2">
                                            </obout:Column>
                                             <obout:Column DataField="Rate" HeaderText="Rate" Align="right" Width="7%" Index="3">
                                            </obout:Column>
                                             <obout:Column HeaderText="From Date" DataField="FromDate" Width="8%" Index="4" DataFormatString="{0:dd-MM-yyyy}">
                                            </obout:Column>
                                             <obout:Column DataField="ToDate" HeaderText="To Date" Width="8%" Index="5" DataFormatString="{0:dd-MM-yyyy}">
                                            </obout:Column>
                                            <obout:Column DataField="EffDate" HeaderText="Eff. Date" Width="8%" Index="6" DataFormatString="{0:dd-MM-yyyy}">
                                            </obout:Column>
                                             <obout:Column HeaderText="Account Type" DataField="AccountType" Width="9%" Index="7">
                                            </obout:Column>
                                             <obout:Column DataField="AccountName" HeaderText="Account Name" Width="15%" Index="8">
                                            </obout:Column>
                                           <%-- <obout:Column DataField="Client" HeaderText="Client" Width="15%" Index="9">
                                            </obout:Column>--%>
                                            <obout:Column DataField="Company" HeaderText="Company" Width="17%" Index="10">
                                            </obout:Column>
                                             <obout:Column DataField="Active" HeaderText="Active" Width="6%" Index="11">
                                            </obout:Column>
                                        </Columns>
                                        <Templates>
                                            <obout:GridTemplate ID="GvTempEdit" runat="server" ControlID="" ControlPropertyName="">
                                                <Template>
                                                    <asp:ImageButton ID="imgBtnEdit" ToolTip="Edit" CausesValidation="false" runat="server"
                                                        ImageUrl="../App_Themes/Blue/img/Edit16.png" />
                                                </Template>
                                            </obout:GridTemplate>
                                        </Templates>
                                    </obout:Grid>
                                </td>
                            </tr>
                        </table>
                    </center>
                </ContentTemplate>
                <%--</asp:UpdatePanel>
                    </ContentTemplate>--%>
            </asp:TabPanel>
            <asp:TabPanel ID="tabAccountInfo" runat="server" HeaderText="RateCard Info">
                <ContentTemplate>
                    <center>
                        <table class="tableForm">
                            <tr>
                                 <td>
                                 <req>Company :</req>
                            </td>
                            <td style="text-align: left">
                                <asp:DropDownList ID="ddlgroupcompany" runat="server" DataTextField="Name" DataValueField="ID" onchange="GetCompany()"  Width="206px">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlgroupcompany" InitialValue="0"
                                        Display="None" ErrorMessage="Please Select Company" ValidationGroup="Save"></asp:RequiredFieldValidator>
                            </td>
                                  <td>
                                    <req><asp:Label ID="LblAccountType" runat="server" Text="Account Type" /></req>
                                    :
                                </td>
                                <td style="text-align: left;">
                                    <asp:DropDownList ID="ddlaccounttypeer" runat="server"  DataTextField="Name" DataValueField="ID" onchange="GetDepartment()"
                                    Width="206px">
                                        <asp:ListItem Text="-Select-" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="Customer" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="Vendor" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="Client" Value="3"></asp:ListItem>

                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="ddlaccounttypeer" InitialValue="0"
                                        Display="None" ErrorMessage="Please Select Account Type" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                </td>
                              <td>
                                    <req><asp:Label ID="lblratetype" runat="server" Text="Rate Type" /></req>
                                    :
                                </td>
                                <td style="text-align: left;">
                                    <asp:DropDownList ID="ddlratetype" runat="server"  DataTextField="Value" DataValueField="Id" onchange="GetDepartment()"
                                    Width="206px">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="ddlratetype" InitialValue="0"
                                        Display="None" ErrorMessage="Please Select Type" ValidationGroup="Save"></asp:RequiredFieldValidator>

                                </td>
                            </tr>
                            <tr>
                               
                                <td>
                                    <req> <asp:Label ID="lblname" runat="server" Text="Account Name" /> </req>
                                 </td>
                                <td style="text-align: left;">
                                    <asp:TextBox ID="txtname"  Enabled="false" runat="server"  Width="170px"></asp:TextBox>
                                     <img id="imgSearch" src="../App_Themes/Grid/img/search.jpg" title="Search SKU"
                                style="cursor: pointer;" onclick="openContactSearch('0')" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtname"
                                        Display="None" ErrorMessage="Please Select Account" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                </td>
                                 <td>
                                    <req><asp:Label Id="lblrate" runat="server" Text="Rate Title"/></req>
                                    :
                                </td>
                                <td style="text-align: left;">
                                   <asp:TextBox ID="txtratetitle" runat="server" MaxLength="100" Width="200px"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RFValEmailID" runat="server" ControlToValidate="txtratetitle"
                                        Display="None" ErrorMessage="Please enter Rate Title" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                </td>
                                <td>
                                    <req><asp:Label Id="lblprice" runat="server" Text="Price"/></req>
                                    :
                                </td>
                                <td style="text-align: left;">
                                   <asp:TextBox ID="txtprice" runat="server" MaxLength="100" Width="200px" style="text-align:right;"  onkeypress="return fnAllowDigitsOnly(event)"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtprice"
                                        Display="None" ErrorMessage="Please enter Price" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                </td>
                               
                            </tr>
                            <tr>
                                <td>
                                    <req><asp:Label ID="lblstartdate" runat="server" Text="Start Date" /> :</req>
                                </td>
                                <td style="text-align: left;">
                                    <uc9:UC_Date ID="UC_Startdt" runat="server" />
                                    <asp:CustomValidator ID="custValidatestartdate" runat="server" ClientValidationFunction="validatestartdate"
                                    ValidationGroup="Save" ErrorMessage="Please select Start Date" Display="None" ></asp:CustomValidator>  
                                </td>
                                 <td>
                                     <req><asp:Label ID="lblenddate" runat="server" Text="End Date" /> :</req>
                                 </td>
                                 <td style="text-align: left;">
                                    <req><uc9:UC_Date ID="UC_enddt" runat="server" /></req>
                                     <asp:CustomValidator ID="custValienddate" runat="server" ClientValidationFunction="validatesEndDate"
                                    ValidationGroup="Save" ErrorMessage="Please select To Date" Display="None" ></asp:CustomValidator> 
                                 </td>
                                 <td>
                                    <asp:Label ID="lblEffectiveDate" runat="server" Text="Effective From" /> :
                                 </td>
                                 <td style="text-align: left;">
                                    <uc9:UC_Date ID="UC_effectivedt" runat="server" />
                                     <asp:CustomValidator ID="custValidatedateeff" runat="server" ClientValidationFunction="validatesEffDate"
                                    ValidationGroup="Save" ErrorMessage="Please select Effective date" Display="None" ></asp:CustomValidator> 
                                 </td>
                            </tr>
                            <tr>
                                 <td>
                                    <req><asp:Label Id="lblactive" runat="server" Text="Active"/></req> :
                                </td>
                                <td style="text-align: left">
                                    <obout:OboutRadioButton ID="rbtnActiveYes" runat="server" Checked="True" FolderStyle=""
                                        GroupName="Active" Text="Yes">
                                    </obout:OboutRadioButton>
                                    <obout:OboutRadioButton ID="rbtnActiveNo" runat="server" FolderStyle="" GroupName="Active"
                                        Text="No">
                                    </obout:OboutRadioButton>
                                </td>
                                <td>
                                   <asp:Label Id="lblremark" runat="server" Text="Remark"/> :
                                </td>
                                <td style="text-align: left" colspan="3">
                                   <asp:TextBox ID="txtremark" runat="server" Width="500px" TextMode="MultiLine" Height="30px"></asp:TextBox>
                                </td>
                            </tr>
                                                    
                        </table>
                    </center>
                </ContentTemplate>
            </asp:TabPanel>
           
        </asp:TabContainer>
    </center>
    <%-- </ContentTemplate>
    </asp:UpdatePanel>--%>
    <script type="text/javascript">
        function fnAllowDigitsOnly(key) {
            var keycode = (key.which) ? key.which : key.keyCode;
            if ((keycode < 48 || keycode > 57) && (keycode != 8) && (keycode != 9) && (keycode != 11) && (keycode != 37) && (keycode != 38) && (keycode != 39) && (keycode != 40) && (keycode != 127)) {
                return false;
            }
        }

        function AllowAlphabet(key) {
            var keycode = (key.which) ? key.which : key.keyCode;
            if (!(((keycode >= '65') && (keycode <= '90')) || ((keycode >= '97') && (keycode <= '122')) || (keycode == '46') || (keycode == '32') || (keycode == '45') || (keycode == '11') || (keycode == '8') || (keycode == '127') || (keycode == '9') || (keycode == '11') || (keycode == '37') || (keycode == '38') || (keycode == '39') || (keycode == '40') || (keycode == '41'))) {
                return false;
            }
            return true;
        }

        function Checkvalidations() {

            var email = document.getElementById("<%=txtname.ClientID %>");
            var filter = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;


            if (document.getElementById("<%=txtname.ClientID %>").value == "") {
                showAlert("Please Enter Email Id!", "error", "#");
                document.getElementById("<%=txtname.ClientID %>").focus();
                return false;
            }

            if (!filter.test(email.value)) {
                showAlert("Please Enter Valid Email Id!", "error", "#");
                document.getElementById("<%=txtname.ClientID %>").focus();
                return false;
            }

            if (document.getElementById("<%=ddlgroupcompany.ClientID %>").value == "0") {
                showAlert("Please Select Country!", "error", "#");
                document.getElementById("<%=ddlgroupcompany.ClientID %>").focus();
                return false;
            }
            if (document.getElementById("<%=ddlgroupcompany.ClientID %>").value == "0") {
                showAlert("Please Select State!", "error", "#");
                document.getElementById("<%=ddlgroupcompany.ClientID %>").focus();
                return false;
            }
            if (document.getElementById("<%=txtname.ClientID %>").value == "") {
                showAlert("Please Enter City!", "error", "#");
                document.getElementById("<%=txtname.ClientID %>").focus();
                return false;
            }

            return true
        };

    </script>
    <script type="text/javascript">
        print_country('ddlCountry');
        function onCountryChange(ddlC) {
            document.getElementById('hdnCountry').value = ddlC.options[ddlC.selectedIndex].text;
        }
        function onStateChange(ddlS) {
            document.getElementById('hdnState').value = ddlS.options[ddlS.selectedIndex].text;
        }

        function setCountry(country, state) {
            var ddlCountry = document.getElementById("ddlCountry");
            ddlCountry.value = country;

            print_state('ddlState', ddlCountry.selectedIndex);
            ddlState = document.getElementById("ddlState");
            ddlState.value = state;

        }


    </script>

    <script type="text/javascript">
       


    </script>

    <script type="text/javascript">
        function openContactSearch(sequence) {
            var ddlaccounttype = document.getElementById("<%=ddlaccounttypeer.ClientID %>");
            var AccountType = ddlaccounttype.options[ddlaccounttype.selectedIndex].text;
            var CompanyID = document.getElementById("<%=hndCompanyid.ClientID %>").value;
            if (AccountType != "Customer") {
                window.open('../Account/SearchRateCardAcc.aspx?CompanyID=' + CompanyID + '&AccountType=' + AccountType + "", null, 'height=500px, width=825px,status= 0, resizable= 1, scrollbars=1, toolbar=0,location=center,menubar=0, screenX=300; screenY=50');
            }
            else {
                window.open('../Account/SearchCustomer.aspx?CompanyID=' + CompanyID + '&AccountType=' + AccountType + "", null, 'height=500px, width=825px,status= 0, resizable= 1, scrollbars=1, toolbar=0,location=center,menubar=0, screenX=300; screenY=50');
            }
        }

        function AfterContact1Selected(ConID, ConName, ConEmail, ConMob) {
            var Con1ID = document.getElementById("hdnSearchContactID1");
            var Con1NM = document.getElementById("hdnSearchContactName1");
            var ConEmail = document.getElementById("hdnSearchConEmail");
            var ConMob = document.getElementById("hdnSearchConMobNo");

            Con1ID.value = ConID;
            Con1NM.value = ConName;
            ConEmail.value = ConEmail.value;
            ConMob.value = ConMob.value;
            var txtconname = document.getElementById("<%= txtname.ClientID %>");
            txtconname.value = ConName;
            var txtemailid = document.getElementById("<%= txtname.ClientID %>");
            txtemailid.value = ConEmail.value;
            var txtphoneno = document.getElementById("<%= txtname.ClientID %>");
            txtphoneno.value = ConMob.value;

        }
    </script>

<%--    Get Account Name--%>
    <script type="text/javascript">

        function GetCompany()
        {
            var CompanyID = document.getElementById("<%=ddlgroupcompany.ClientID %>");
            document.getElementById("<%=hndCompanyid.ClientID %>").value = CompanyID.value;
        }


        function GetRateAccount(AccountID, AccountName) {
            var hdnAccountID = document.getElementById("hdnAccountID");
            var hdnAccountName = document.getElementById("hdnAccountName");

            hdnAccountID.value = AccountID;
            hdnAccountName.value = AccountName;

            var txtname = document.getElementById("<%=txtname.ClientID %>");
            txtname.value = AccountName;
            }


        function fnAllowDigitsOnly(key) {
            var keycode = (key.which) ? key.which : key.keyCode;
            if ((keycode < 48 || keycode > 57) && (keycode != 8) && (keycode != 9) && (keycode != 11) && (keycode != 16) && (keycode != 37) && (keycode != 38) && (keycode != 39) && (keycode != 40) && (keycode != 219) && (keycode != 127)) {
                return false;
            }
        }

        function validatestartdate(source, args) {
            var StartDate = getDateFromUC("<%= UC_Startdt.ClientID %>");
            if (StartDate != "") {
                isValid = true;
                return;
            }
            args.IsValid = false;
        }

        function validatesEndDate(source, args) {
            var EndDate = getDateFromUC("<%= UC_enddt.ClientID %>");
            if (EndDate != "") {
                isValid = true;
                return;
            }
            args.IsValid = false;
        }

        function validatesEffDate(source, args) {
            var EffDate = getDateFromUC("<%= UC_effectivedt.ClientID %>");
            if (EffDate != "") {
                isValid = true;
                return;
            }
            args.IsValid = false;
        }

    </script>
    
</asp:Content>

<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/CRM.Master" AutoEventWireup="true" CodeBehind="location.aspx.cs" EnableEventValidation="false" 
    Inherits="BrilliantWMS.Account.location" Theme="Blue" %>

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
            <asp:TabPanel ID="TabCustomerList" runat="server" HeaderText="Location List">
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
                    <asp:HiddenField ID="hdnCountry" runat="server" ClientIDMode="Static" />
                    <asp:HiddenField ID="hdnState" runat="server" ClientIDMode="Static" />
                    <asp:HiddenField ID="hdnSelectedCompany" runat="server" ClientIDMode="Static" />
                    <asp:HiddenField ID="hdnSelectedDepartment" runat="server" ClientIDMode="Static" />
                    <asp:HiddenField ID="hdnselectedLocation" runat="server"></asp:HiddenField>
                    <asp:HiddenField ID="hdnSearchContactID1" runat="server" ClientIDMode="Static" />
                    <asp:HiddenField ID="hdnSearchContactName1" runat="server" ClientIDMode="Static" />
                    <asp:HiddenField ID="hdnSearchConEmail" runat="server" ClientIDMode="Static" />
                    <asp:HiddenField ID="hdnSearchConMobNo" runat="server" ClientIDMode="Static" />
                    
                    <center>
                        <table class="gridFrame" style="width: 100%">
                            <tr>
                                <td>
                                    <table style="width: 100%">
                                        <tr>
                                            <td style="text-align: left;">
                                                <a id="headerText">
                                                    <asp:Label ID="lblheasertext" runat="server" Text="Location List"></asp:Label></a>
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
                                    <obout:Grid ID="Gvlocation" runat="server" AutoGenerateColumns="False" AllowAddingRecords="False"
                                        AllowGrouping="True" AllowFiltering="True" Width="100%" OnSelect="Gvlocation_Select" >
                                        <ScrollingSettings ScrollHeight="250" />
                                        <Columns>
                                            <obout:Column ID="Edit" Width="4%" AllowFilter="False" HeaderText="Edit" Index="0"
                                                TemplateId="GvTempEdit">
                                                <TemplateSettings TemplateId="GvTempEdit" />
                                            </obout:Column>
                                            <obout:Column DataField="ID" Visible="False" HeaderText="ID" Index="1">
                                            </obout:Column>
                                             <obout:Column DataField="LocationName" HeaderText="Location Name" Width="15%" Index="2">
                                            </obout:Column>
                                            <obout:Column DataField="LocationCode" HeaderText="Location Code" Width="15%" Index="3">
                                            </obout:Column>
                                            <obout:Column HeaderText="Address Line 1" DataField="AddressLine1" Width="25%" Index="4">
                                            </obout:Column>
                                            <obout:Column HeaderText="City" DataField="City" Width="10%" Index="5">
                                            </obout:Column>
                                             <obout:Column DataField="ContactName" HeaderText="Contact Name" Width="11%" Index="6">
                                            </obout:Column>
                                            <obout:Column DataField="MobileNo" HeaderText="Phone No" Width="8%" Index="7">
                                            </obout:Column>
                                            <obout:Column DataField="ContactEmail" HeaderText="Contact Email" Width="17%" Index="8">
                                            </obout:Column>
                                             <obout:Column DataField="Active" HeaderText="Active" Width="6%" Index="9">
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
            <asp:TabPanel ID="tabAccountInfo" runat="server" HeaderText="Location Info">
                <ContentTemplate>
                    <center>
                        <table class="tableForm">
                            <tr>
                                <%-- <td>
                                Group Company :
                            </td>
                            <td style="text-align: left">
                                <asp:DropDownList ID="ddlgroupcompany" runat="server" DataTextField="Name" DataValueField="ID"
                                    Width="206px">
                                </asp:DropDownList>
                            </td>--%>
                                <td>
                                    <req><asp:Label Id="lblcustname" runat="server" Text="Company"></asp:Label></req>
                                    :
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlcompany" runat="server"  DataTextField="Name" DataValueField="ID" onchange="GetDepartment()"
                                    Width="206px"></asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RfValCompany" runat="server" ControlToValidate="ddlcompany" InitialValue="0"
                                        Display="None" ErrorMessage="Please Select Company" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                </td>
                                <td>
                                    <req><asp:Label ID="lblwebsite" runat="server" Text="Location Name" /></req>
                                    :
                                </td>
                                <td>
                                    <asp:TextBox ID="txtlocationName" runat="server" MaxLength="100" Width="200px"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtlocationName"
                                        Display="None" ErrorMessage="Please enter location Name" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                    
                                    <%--<asp:DropDownList ID="ddldept" runat="server" DataTextField="Territory" DataValueField="ID"  Width="206px" onchange="Getdeptid()"></asp:DropDownList>
                                   <asp:RequiredFieldValidator ID="rfdept" runat="server" ControlToValidate="ddldept" InitialValue="0"
                                        Display="None" ErrorMessage="Please Select Department" ValidationGroup="Save"></asp:RequiredFieldValidator>--%>
                                </td>
                                <td>
                                    <req><asp:Label Id="lblcode" runat="server" Text="Location Code"/></req>
                                    :
                                </td>
                                <td>
                                   <asp:TextBox ID="txtlocation" runat="server" MaxLength="100" Width="200px"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RFValEmailID" runat="server" ControlToValidate="txtlocation"
                                        Display="None" ErrorMessage="Please enter location Code" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbladdress1" runat="server" Text="Address Line 1" />
                                    :
                                </td>
                                <td>

                                    <asp:TextBox ID="txtCAddress1" runat="server" MaxLength="100" onkeyup="TextBox_KeyUp(this,'SpanAddressLine1','100');"
                                        TextMode="MultiLine" Width="200px"></asp:TextBox>

                                    <br />
                                    <span class="watermark">
                                        <asp:Label ID="lblcharremain" runat="server" Text="characters remaining out of 100 " />&nbsp;<span
                                            id="SpanAddressLine1"> 100</span> </span>
                                    <asp:RequiredFieldValidator ID="address1val" runat="server" ControlToValidate="txtCAddress1"
                                        Display="None" ErrorMessage="Please Enter Address" ValidationGroup="Save"></asp:RequiredFieldValidator>


                                </td>
                                <td>
                                    <asp:Label ID="lbladdress2" runat="server" Text="Address Line 2" />
                                    :
                                </td>
                                <td>
                                    <asp:TextBox ID="txtAddress2" runat="server" MaxLength="100" onkeyup="TextBox_KeyUp(this,'SpanAddressLine2','100');"
                                        TextMode="MultiLine" Width="200px"></asp:TextBox>
                                     <br />
                                    <span class="watermark">
                                        <asp:Label ID="lblcharremain2" runat="server" Text="characters remaining
                                    out of 100 " />&nbsp;<span id="SpanAddressLine2"> 100</span></span>
                                </td>
                                <td>
                                     <req><asp:Label Id="lblcountry" runat="server" Text="Country"/></req>
                                    :
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlCountry" ClientIDMode="Static" runat="server" AutoPostBack="false"
                                        onchange="print_state('ddlState',this.selectedIndex); onCountryChange(this);"
                                        Width="206px">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RFValddlCountry" runat="server" ForeColor="Maroon"
                                        ValidationGroup="AddressSubmit" InitialValue="0" ControlToValidate="ddlCountry"
                                        ErrorMessage="Select Country" Display="None"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                  <td>
                                    <req><asp:Label Id="lblstate" runat="server" Text="State"/> </req>
                                    :
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlState" ClientIDMode="Static" runat="server" Width="206px"
                                        onchange="onStateChange(this);">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RFValstate" ForeColor="Maroon" ControlToValidate="ddlState"
                                        InitialValue="0" runat="server" ErrorMessage="Select State" Display="None"></asp:RequiredFieldValidator>
                                </td>
                                <td>
                                    <req><asp:Label Id="lblcity" runat="server" Text="City"/></req>
                                    :
                                </td>
                                <td>
                                    <asp:TextBox ID="txtCity" runat="server" MaxLength="50" Width="200px" onkeypress="return AllowAlphabet(event)"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RFValddlBCity" runat="server" ControlToValidate="txtCity"
                                        Display="None" ErrorMessage="Please Enter City" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                </td>
                                <td>
                                    <asp:Label ID="lblzip" runat="server" Text="ZIP Code" />
                                    :
                                </td>
                                <td>
                                    <asp:TextBox ID="txtZipCode" runat="server" MaxLength="10" Width="200px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbllandmark" runat="server" Text="Landmark" />
                                    :
                                </td>
                                <td>
                                    <asp:TextBox ID="txtLandMark" runat="server" MaxLength="50" Width="200px"></asp:TextBox>
                                </td>
                                 <td>
                                    <asp:Label ID="lblfax" runat="server" Text="Fax No. " />
                                    :
                                </td>
                                <td>
                                    <asp:TextBox ID="txtFax" runat="server" MaxLength="10" onkeydown="return AllowPhoneNo(this, event);"
                                        onkeypress="return AllowPhoneNo(this, event);" Width="200px"></asp:TextBox>
                                </td>
                                 <td>
                                    <req><asp:Label Id="lblactive" runat="server" Text="Active"/></req>
                                    :
                                </td>
                                <td style="text-align: left">
                                    <obout:OboutRadioButton ID="rbtnActiveYes" runat="server" Checked="True" FolderStyle=""
                                        GroupName="Active" Text="Yes">
                                    </obout:OboutRadioButton>
                                    <obout:OboutRadioButton ID="rbtnActiveNo" runat="server" FolderStyle="" GroupName="Active"
                                        Text="No">
                                    </obout:OboutRadioButton>
                                </td>
                               
                            </tr>
                            <tr>
                                 <td>
                                    <req> <asp:Label ID="lblname" runat="server" Text="Contact Name" /> </req>
                                 </td>
                                <td>
                                    <asp:TextBox ID="txtname"  Enabled="false" runat="server"  Width="170px"></asp:TextBox>
                                     <img id="imgSearch" src="../App_Themes/Grid/img/search.jpg" title="Search SKU"
                                style="cursor: pointer;" onclick="openContactSearch('0')" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtname"
                                        Display="None" ErrorMessage="Please Select Contact Person" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                </td>
                                <td>
                                    <asp:Label ID="lblphone" runat="server" Text="Contact Phone No." />
                                    :
                                </td>
                                <td>
                                    <asp:TextBox ID="txtphoneno" runat="server" Enabled="false"  MaxLength="12" onkeydown="return AllowPhoneNo(this, event);"
                                        onkeypress="return AllowPhoneNo(this, event);" Width="200px"></asp:TextBox>
                                      <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtphoneno"
                                        Display="None" ErrorMessage="Please Enter Phone No." ValidationGroup="Save"></asp:RequiredFieldValidator>
                                </td>
                                <td>
                                      <req><asp:Label Id="lblemailid" runat="server" Text="Contact Email ID"/></req>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtemailid" runat="server" MaxLength="100" Enabled="false" Width="200px" AutoPostBack="false"></asp:TextBox>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtemailid"
                                        Display="None" ErrorMessage="Please enter valid Email ID" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                        ValidationGroup="Save"></asp:RegularExpressionValidator>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtemailid"
                                        Display="None" ErrorMessage="Please enter Email ID" ValidationGroup="Save"></asp:RequiredFieldValidator>
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

            var email = document.getElementById("<%=txtemailid.ClientID %>");
            var filter = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;


            if (document.getElementById("<%=ddlcompany.ClientID %>").value == "0") {
                showAlert("Please Enter Company name!", "error", "#");
                document.getElementById("<%=ddlcompany.ClientID %>").focus();
                return false;
            }
            if (document.getElementById("<%=txtemailid.ClientID %>").value == "") {
                showAlert("Please Enter Email Id!", "error", "#");
                document.getElementById("<%=txtemailid.ClientID %>").focus();
                return false;
            }

            if (!filter.test(email.value)) {
                showAlert("Please Enter Valid Email Id!", "error", "#");
                document.getElementById("<%=txtemailid.ClientID %>").focus();
                return false;
            }

            if (document.getElementById("<%=ddlCountry.ClientID %>").value == "0") {
                showAlert("Please Select Country!", "error", "#");
                document.getElementById("<%=ddlCountry.ClientID %>").focus();
                return false;
            }
            if (document.getElementById("<%=ddlState.ClientID %>").value == "0") {
                showAlert("Please Select State!", "error", "#");
                document.getElementById("<%=ddlState.ClientID %>").focus();
                return false;
            }
            if (document.getElementById("<%=txtCity.ClientID %>").value == "") {
                showAlert("Please Enter City!", "error", "#");
                document.getElementById("<%=txtCity.ClientID %>").focus();
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
        var ddlcompany = document.getElementById("<%=ddlcompany.ClientID %>");
       

        function GetDepartment() {
            var obj1 = new Object();
            obj1.ddlcompanyId = ddlcompany.value.toString();
            document.getElementById("<%=hdnSelectedCompany.ClientID %>").value = ddlcompany.value.toString();
            //PageMethods.GetDepartment(obj1, getLoc_onSuccessed);
        }

        function getLoc_onSuccessed(result) {

            ddldeptid.options.length = 0;
            for (var i in result) {
                AddOption(result[i].Name, result[i].Id);
            }
        }

        function AddOption(text, value) {

            var option = document.createElement('option');
            option.value = value;
            option.innerHTML = text;
            ddldeptid.options.add(option);
        }

       

    </script>

    <script type="text/javascript">
        function openContactSearch(sequence) {
            var CompanyID = document.getElementById("<%=hdnSelectedCompany.ClientID %>").value
            window.open('../Account/SearchAddContact.aspx?CompanyID=' + CompanyID + "", null, 'height=500px, width=825px,status= 0, resizable= 1, scrollbars=1, toolbar=0,location=center,menubar=0, screenX=300; screenY=50');
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
            var txtemailid = document.getElementById("<%= txtemailid.ClientID %>");
            txtemailid.value = ConEmail.value;
            var txtphoneno = document.getElementById("<%= txtphoneno.ClientID %>");
            txtphoneno.value = ConMob.value;

        }
    </script>
    
 
</asp:Content>
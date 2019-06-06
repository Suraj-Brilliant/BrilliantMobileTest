<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/CRM.Master" AutoEventWireup="true" CodeBehind="DocumentMaster.aspx.cs"
    Inherits="BrilliantWMS.Document.DocumentMaster" Theme="Blue" %>

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
            <asp:TabPanel ID="TabCustomerList" runat="server" HeaderText="Document List">
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
                     <asp:HiddenField ID="hdDownLoadAccessIDs" runat="server" ViewStateMode="Enabled"  ClientIDMode="Static" />
                      <asp:HiddenField ID="hdnDeleteAccessIDs" runat="server" ViewStateMode="Enabled" ClientIDMode="Static" />
                    <center>
                        <table class="gridFrame" style="width: 100%">
                            <tr>
                                <td>
                                    <table style="width: 100%">
                                        <tr>
                                            <td style="text-align: left;">
                                                <a id="headerText">
                                                    <asp:Label ID="lblheasertext" runat="server" Text="Document List"></asp:Label></a>
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
                                    <obout:Grid ID="grddocument" runat="server" AutoGenerateColumns="False" AllowAddingRecords="False"
                                        AllowGrouping="True" AllowFiltering="True" Width="100%" >
                                        <ScrollingSettings ScrollHeight="250" />
                                        <Columns>
                                            <obout:Column ID="Edit" Width="4%" AllowFilter="False" HeaderText="Edit" Index="0"
                                                TemplateId="GvTempEdit">
                                                <TemplateSettings TemplateId="GvTempEdit" />
                                            </obout:Column>
                                            <obout:Column DataField="ID" Visible="False" HeaderText="ID" Index="1">
                                            </obout:Column>
                                            <obout:Column DataField="DocumentName" HeaderText="Document Title" Width="15%" Index="2">
                                            </obout:Column>
                                            <obout:Column DataField="DocumentType" HeaderText="Document Type" Width="15%" Index="3">
                                            </obout:Column>
                                            <obout:Column HeaderText="Description" DataField="Description" Width="25%" Index="4">
                                            </obout:Column>
                                            <obout:Column HeaderText="ObjectName" DataField="ObjectName" Width="10%" Index="5">
                                            </obout:Column>
                                            <obout:Column DataField="Company" HeaderText="Company" Width="11%" Index="6">
                                            </obout:Column>
                                            <obout:Column DataField="Customer" HeaderText="Customer" Width="8%" Index="7">
                                            </obout:Column>
                                            <obout:Column ID="Download" DataField="DowloadAccess" HeaderText="Download" Width="10%" HeaderAlign="center" Align="center">
                                                <TemplateSettings TemplateId="GvTempDownload" />
                                            </obout:Column>
                                        </Columns>
                                        <Templates>
                                            <obout:GridTemplate ID="GvTempEdit" runat="server" ControlID="" ControlPropertyName="">
                                                <Template>
                                                    <asp:ImageButton ID="imgBtnEdit" ToolTip="Edit" CausesValidation="false" runat="server"
                                                        ImageUrl="../App_Themes/Blue/img/Edit16.png" />
                                                </Template>
                                            </obout:GridTemplate>
                                            <obout:GridTemplate ID="GvTempDownload">
                                                <Template>
                                                    <a href='<%# (Container.Value == "true" ? (Container.DataItem["DocumentDownloadPath"]) :'#') %>' target="_blank">
                                                        <img src='<%# (Container.Value == "true" ? "../CommonControls/HomeSetupImg/download.png" : "../CommonControls/HomeSetupImg/download.png") %>' alt="download" /> </a>  <%--../CommonControls/HomeSetupImg/accessdDenied.jpg--%>
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
            <asp:TabPanel ID="tabAccountInfo" runat="server" HeaderText="Document Info">
                <ContentTemplate>
                    <center>
                        <table class="tableForm">
                            <tr>
                                <td>Company :
                                </td>
                                <td style="text-align: left">
                                    <asp:DropDownList ID="ddlgroupcompany" runat="server" DataTextField="Name" DataValueField="ID" Width="206px">
                                         <asp:ListItem Text="-Select-" Value="0" Selected="True"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <req><asp:Label Id="lblcustname" runat="server" Text="Customer"></asp:Label></req> :
                                </td>
                                <td style="text-align: left">
                                    <asp:DropDownList ID="ddlcompany" runat="server" DataTextField="Name" DataValueField="ID" onchange="GetDepartment()" Width="206px">
                                         <asp:ListItem Text="-Select-" Value="0" Selected="True"></asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RfValCompany" runat="server" ControlToValidate="ddlcompany" InitialValue="0"
                                        Display="None" ErrorMessage="Please Select Company" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                    <td>
                                        <req><asp:Label ID="lblDocumentTitle" runat="server" Text="Document Title"></asp:Label></req> :
                                    </td>
                                    <td style="text-align: left">
                                        <asp:TextBox ID="txtDocTitle" runat="server" MaxLength="50" onblur="CheckDocumentTitle()"
                                            ValidationGroup="DocumentValidation" Width="250px"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="reqValDocTitle" runat="server" ControlToValidate="txtDocTitle"
                                            ValidationGroup="DocumentValidation" ErrorMessage="Enter document title" Display="None"></asp:RequiredFieldValidator>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblDocumentType" runat="server" Text="Document Type"></asp:Label> :
                                    </td>
                                    <td style="text-align: left">
                                        <asp:DropDownList ID="ddlDocumentType" runat="server" Width="180px">
                                            <asp:ListItem Text="-Select-" Value="0" Selected="True"></asp:ListItem>
                                       </asp:DropDownList>
                                    </td>
                             </tr>
                            <tr>
                                    <td>
                                        <asp:Label ID="lblDescription" runat="server" Text="Description"></asp:Label>:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtDocDesc" runat="server" MaxLength="500" Width="250px" TextMode="MultiLine"
                                            onkeyup="TextBox_KeyUp(this,'CharactersCounterDocDesc','500');" ClientIDMode="Static"
                                            Height="50px"></asp:TextBox>
                                        <br />
                                        <span class="watermark"><span id="CharactersCounterDocDesc">500</span> characters remaining
                                            out of 500 </span>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblKeyWords" runat="server" Text="Key Words"></asp:Label> :
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtKeyword" runat="server" MaxLength="500" Width="250px" TextMode="MultiLine"
                                            onkeyup="TextBox_KeyUp(this,'CharactersCounterDocKeyWords','500');" ClientIDMode="Static"
                                            Height="50px"></asp:TextBox>
                                        <br />
                                        <span class="watermark"><span id="CharactersCounterDocKeyWords">500</span> characters
                                            remaining out of 500 </span>
                                    </td>
                            </tr>
                              <tr >
                                    <td>
                                        Access To :
                                    </td>
                                    <td style="text-align: left;">
                                        <obout:OboutRadioButton ID="rbtnPublic" runat="server" Text="Public" GroupName="rbtnAccessTo"
                                            Checked="true">
                                        </obout:OboutRadioButton>
                                        <obout:OboutRadioButton ID="rbtnSelf" runat="server" Text="Self" GroupName="rbtnAccessTo">
                                        </obout:OboutRadioButton>
                                        <obout:OboutRadioButton ID="rbtnPrivate" runat="server" Text="Private" GroupName="rbtnAccessTo"
                                            CausesValidation="false">
                                            <ClientSideEvents OnClick="openUserList" />
                                        </obout:OboutRadioButton>
                                    </td>
                                    <td>
                                        <req><asp:Label ID="lblSelectDocument" runat="server" Text="Select Document"></asp:Label></req> :
                                    </td>
                                    <td>
                                        <asp:FileUpload ID="FileUploadDocument" runat="server" />
                                        <asp:RequiredFieldValidator ID="ReqVal" runat="server" ControlToValidate="FileUploadDocument"
                                            ErrorMessage="Select document to upload" ValidationGroup="DocumentValidation"
                                            Display="None">
                                        </asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <span class="watermark">Max Limit 20 MB</span>
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
        function openUserList() {
            window.open('../UserManagement/UserList.aspx', null, 'height=400px, width=709px,status= 0, resizable= 0, scrollbars=0, toolbar=0,location=center,menubar=0, screenX=300; screenY=50');
        }

        function CheckDocumentTitle() {
            var DocumentTitle = document.getElementById("<%= txtDocTitle.ClientID %>").value;
            PageMethods.CheckDocumentTitle(DocumentTitle, OnSucessCheckDuplicateDocument, OnfailedCheckDuplicateDocument);
        }

        function OnSucessCheckDuplicateDocument(result) {
            var txtDocTitle = document.getElementById("<%= txtDocTitle.ClientID %>");
            if (result != "") {
                alert(result); txtDocTitle.value = '';
            }
        }
        function OnfailedCheckDuplicateDocument() {
        }

        function onSuccessTempSaveDocument() {
            if (window.opener.GvDocument != null) {
                window.opener.GvDocument.refresh();
            }
            alert("Document saved successfully");
            LoadingOff();
            self.close();
        }

        function DocumentLoadingOn() {
            if (typeof (Page_ClientValidate) == 'function') {
                Page_ClientValidate();
            }
            if (Page_IsValid) {
                LoadingOn();
            }
        }


    </script>


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

            var email = document.getElementById("<%=txtDocTitle.ClientID %>");
            var filter = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;


            if (document.getElementById("<%=ddlcompany.ClientID %>").value == "0") {
                showAlert("Please Enter Company name!", "error", "#");
                document.getElementById("<%=ddlcompany.ClientID %>").focus();
                return false;
            }
            if (document.getElementById("<%=txtDocTitle.ClientID %>").value == "") {
                showAlert("Please Enter Email Id!", "error", "#");
                document.getElementById("<%=txtDocTitle.ClientID %>").focus();
                return false;
            }

            if (!filter.test(email.value)) {
                showAlert("Please Enter Valid Email Id!", "error", "#");
                document.getElementById("<%=txtDocTitle.ClientID %>").focus();
                return false;
            }

            if (document.getElementById("<%=ddlgroupcompany.ClientID %>").value == "0") {
                showAlert("Please Select Country!", "error", "#");
                document.getElementById("<%=ddlgroupcompany.ClientID %>").focus();
                return false;
            }
            if (document.getElementById("<%=ddlDocumentType.ClientID %>").value == "0") {
                showAlert("Please Select State!", "error", "#");
                document.getElementById("<%=ddlDocumentType.ClientID %>").focus();
                return false;
            }
            if (document.getElementById("<%=txtDocTitle.ClientID %>").value == "") {
                showAlert("Please Enter City!", "error", "#");
                document.getElementById("<%=txtDocTitle.ClientID %>").focus();
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
            var txtconname = document.getElementById("<%= txtDocTitle.ClientID %>");
            txtconname.value = ConName;
            var txtemailid = document.getElementById("<%= txtDocTitle.ClientID %>");
            txtemailid.value = ConEmail.value;
            var txtphoneno = document.getElementById("<%= txtDocTitle.ClientID %>");
            txtphoneno.value = ConMob.value;

        }
    </script>

</asp:Content>

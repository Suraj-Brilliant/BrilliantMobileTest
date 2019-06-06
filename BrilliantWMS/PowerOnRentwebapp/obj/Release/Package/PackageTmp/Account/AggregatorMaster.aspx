<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/CRM.Master" AutoEventWireup="true" CodeBehind="AggregatorMaster.aspx.cs"
    Inherits="BrilliantWMS.Account.AggregatorMaster" Theme="Blue" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="Obout.Ajax.UI" Namespace="Obout.Ajax.UI.FileUpload" TagPrefix="obout" %>
<%@ Register Assembly="obout_Interface" Namespace="Obout.Interface" TagPrefix="obout" %>
<%@ Register Assembly="obout_Grid_NET" Namespace="Obout.Grid" TagPrefix="obout" %>
<%@ Register Src="../CommonControls/UCToolbar.ascx" TagName="UCToolbar" TagPrefix="uc1" %>
<%@ Register Src="../CommonControls/UCFormHeader.ascx" TagName="UCFormHeader" TagPrefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHolder_FormHeader" runat="server">
    <uc1:UCToolbar ID="UCToolbar1" runat="server" />
    <uc2:UCFormHeader ID="UCFormHeader1" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPHolder_Form" runat="server">
     <script src="../MasterPage/JavaScripts/countries.js" type="text/javascript"></script>
    <asp:ValidationSummary ID="validationsummary_AccountMaster" runat="server" ShowMessageBox="true"
        ShowSummary="false" DisplayMode="bulletlist" ValidationGroup="Save" />
    <center>
        <asp:TabContainer runat="server" ID="tabAggregatorMaster" ActiveTabIndex="0" Width="100%">
            <asp:TabPanel ID="tabaggregatorlist" runat="server" HeaderText="Aggregator List" TabIndex="0">
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
                    <asp:HiddenField ID="hdnCompanyid" runat="server"></asp:HiddenField>
                    <asp:HiddenField ID="hdncustomerid" runat="server" ClientIDMode="Static"></asp:HiddenField>
                    <asp:HiddenField ID="hdnCountry" runat="server" ClientIDMode="Static" />
                    <asp:HiddenField ID="hdnState" runat="server" ClientIDMode="Static" />
                     <asp:HiddenField ID="hdnAPIState" runat="server" ClientIDMode="Static" />
                    <asp:HiddenField ID="hdnselectedAggID" runat="server" ClientIDMode="Static" />
                    <asp:HiddenField ID="hdnAggreAPIID" runat="server" ClientIDMode="Static" />
                    <center>
                        <table class="gridFrame" style="width: 100%">
                            <tr>
                                <td>
                                    <table style="width: 100%">
                                        <tr>
                                            <td style="text-align: left;">
                                                <a id="headerText">
                                                    <asp:Label ID="lblheasertext" runat="server" Text="Aggregator List"></asp:Label></a>
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
                                    <obout:Grid ID="grdaggregator" runat="server" AllowAddingRecords="False" AllowFiltering="True"
                                                AllowGrouping="True" AutoGenerateColumns="False" Width="100%" OnSelect="grdaggregator_Select">
                                                <ScrollingSettings ScrollHeight="250" />
                                                <Columns>
                                                    <obout:Column ID="Column1" DataField="Edit" Width="2%" AllowFilter="False" Align="center"
                                                        HeaderAlign="Center" HeaderText="Edit" Index="0" TemplateId="imgBtnEdit1">
                                                        <TemplateSettings TemplateId="imgBtnEdit1" />
                                                    </obout:Column>
                                                    <obout:Column DataField="ID" HeaderText="ID" Visible="False">
                                                    </obout:Column>
                                                    <obout:Column DataField="Company" HeaderText="Company" Width="5%">
                                                    </obout:Column>
                                                    <obout:Column DataField="Customer" HeaderText="Customer" Width="5%">
                                                    </obout:Column>
                                                     <obout:Column DataField="AgreegatorName" HeaderText="Aggregator" Width="5%">
                                                    </obout:Column>
                                                    <obout:Column DataField="ContactPersonName" HeaderText="Contac tPerson" Width="5%">
                                                    </obout:Column>
                                                    <obout:Column DataField="EmailID" HeaderText="Email" Width="5%">
                                                    </obout:Column>
                                                    <obout:Column DataField="MobileNo" HeaderText="Contact No." Width="5%">
                                                    </obout:Column>
                                                    <obout:Column DataField="Active" HeaderText="Active" Width="5%">
                                                    </obout:Column>
                                                    </Columns>
                                                <Templates>
                                                    <obout:GridTemplate runat="server" ID="imgBtnEdit1">
                                                        <Template>
                                                            <asp:ImageButton ID="imgBtnEdit" runat="server" ImageUrl="../App_Themes/Blue/img/Edit16.png"
                                                                ToolTip="Edit" CausesValidation="false" />
                                                        </Template>
                                                    </obout:GridTemplate>
                                                </Templates>
                                            </obout:Grid>
                                </td>
                            </tr>
                        </table>
                    </center>
                </ContentTemplate>
            </asp:TabPanel>
            <asp:TabPanel ID="tabAggrgator" runat="server" HeaderText="Aggregator Info" TabIndex="1">
                <ContentTemplate>
                    <center>
                        <table class="tableForm">
                            <tr>
                                <td>Company :
                                </td>
                                <td style="text-align: left">
                                   <asp:DropDownList ID="ddlcompany" runat="server" DataTextField="Name" DataValueField="ID" onchange="GetCustomer()" Width="206px">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RFQCompany" runat="server" ControlToValidate="ddlcompany" InitialValue="0"
                                        Display="None" ErrorMessage="Please Select Company" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                </td>
                                <td>
                                    <req><asp:Label Id="lblcustname" runat="server" Text="Customer"></asp:Label></req> :
                                </td>
                                <td style="text-align: left">
                                   <asp:DropDownList ID="ddlcutomer" runat="server"  DataTextField="Name" DataValueField="ID" onchange="GetCustomerID()" Width="206px">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RfValCompany" runat="server" ControlToValidate="ddlcutomer" InitialValue="0"
                                        Display="None" ErrorMessage="Please Select Customer" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                    <td>
                                        <req><asp:Label ID="lblaggregator" runat="server" Text="Aggregator Name"></asp:Label></req> :
                                    </td>
                                    <td style="text-align: left">
                                         <asp:TextBox ID="txtaggregator" runat="server" MaxLength="50" onblur="CheckDocumentTitle()"
                                            ValidationGroup="DocumentValidation" Width="206px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblcontactPerson" runat="server" Text="Contact Person"></asp:Label> :
                                    </td>
                                    <td style="text-align: left">
                                       <asp:TextBox ID="txtcontactperson" runat="server" MaxLength="50" Width="206px"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="reqValDocTitle" runat="server" ControlToValidate="txtcontactperson"
                                            ValidationGroup="DocumentValidation" ErrorMessage="Enter Contact Person Name" Display="None"></asp:RequiredFieldValidator>
                                    </td>
                             </tr>
                            <tr>
                                <td>
                                        <req><asp:Label ID="lblemail" runat="server" Text="Email ID"></asp:Label></req> :
                                    </td>
                                    <td style="text-align: left">
                                         <asp:TextBox ID="txtemail" runat="server" MaxLength="50" onblur="CheckDocumentTitle()"
                                            ValidationGroup="DocumentValidation" Width="206px"></asp:TextBox>
                                        <asp:RegularExpressionValidator ID="RegExpEmailID" runat="server" ControlToValidate="txtemail"
                                        Display="None" ErrorMessage="Please enter valid Email ID" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                        ValidationGroup="Save"></asp:RegularExpressionValidator>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblmobnumber" runat="server" Text="Contact Number"></asp:Label> :
                                    </td>
                                    <td style="text-align: left">
                                       <asp:TextBox ID="txtmobno" runat="server" MaxLength="10" Width="206px" onkeypress="return fnAllowDigitsOnly(event)"></asp:TextBox>
                                    </td>
                            </tr>
                            <tr>
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
                                <td></td>
                                <td>
                                     <td class="auto-style1">
                                    <asp:Button ID="btncustomernext" runat="server" Text="  Next  " Height="30px" OnClick="btncustomernext_Click" OnClientClick="return Checkvalidations();" />
                                </td>
                                </td>

                            </tr>
                          
                        </table>
                    </center>
                </ContentTemplate>
            </asp:TabPanel>
            <asp:TabPanel ID="tabApiDetail" runat="server" TabIndex="2">
                        <HeaderTemplate>
                            <asp:Label ID="lbltabhead" runat="server" Text="API Details" />
                        </HeaderTemplate>
                        <ContentTemplate>
                            <center>
                                <table class="tableForm">
                                     <tr>
                                        <td>
                                            <req><asp:Label ID="lblapi" runat="server" Text="API Name"/></req>
                                            :
                                        </td>
                                        <td style="text-align: left">
                                            <asp:TextBox ID="txtapi" runat="server" Width="200px"></asp:TextBox>
                                        </td>
                                        <td>
                                           <asp:Label ID="lblpurpose" runat="server" Text="Purpose"/> :
                                        </td>
                                        <td style="text-align: left">
                                            <asp:TextBox ID="txtpurpose" runat="server" Width="200px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <req><asp:Label ID="lblinputpara" runat="server" Text="Input Parameters"/></req> :
                                        </td>
                                        <td style="text-align: left">
                                            <asp:TextBox ID="txtinputpara" runat="server" Width="200px"></asp:TextBox>
                                        </td>
                                        <td>
                                            <req><asp:Label ID="lbloutputpara" runat="server" Text="Output Parameters"/></req>
                                            :
                                        </td>
                                        <td style="text-align: left">
                                           <asp:TextBox ID="txtoutputpara" runat="server" Width="200px"></asp:TextBox>
                                       </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <req><asp:Label ID="lblurl" runat="server" Text="URL"/></req> :
                                        </td>
                                        <td style="text-align: left">
                                            <asp:TextBox ID="txturl" runat="server" Width="200px"></asp:TextBox>
                                        </td>
                                         <td>
                                            <req><asp:Label Id="Label1" runat="server" Text="Active"/></req> :
                                         </td>
                                         <td style="text-align: left">
                                                <obout:OboutRadioButton ID="apiactiveyes" runat="server" Checked="True" FolderStyle="" GroupName="Active" Text="Yes">
                                                </obout:OboutRadioButton>
                                                <obout:OboutRadioButton ID="apiactiveno" runat="server" FolderStyle="" GroupName="Active" Text="No">
                                                </obout:OboutRadioButton>
                                         </td>   
                                    </tr>
                                    <tr>
                                        <td>
                                           <asp:Label ID="lblremark" runat="server" Text="Remark"/> :
                                        </td>
                                        <td style="text-align: left" colspan="2">
                                            <asp:TextBox ID="txtremark" runat="server" Width="200px"></asp:TextBox>
                                        </td>
                                        <td align="right">
                                            <input type="button" id="btnaddi" runat="server" value="Add Details" onclick="SaveAggregatAPI()" />
                                          <%--  <asp:Button ID="btnSave" runat="server" Text="  Save  " />--%>
                                        </td>
                                    </tr>
                                </table>
                            </center>
                            <asp:HiddenField ID="hdnStatus" runat="server" />
                            <asp:HiddenField ID="hdnApprovalID" runat="server" />
                            <asp:HiddenField ID="hdnApprovalUserID" runat="server" />
                            <asp:HiddenField runat="server" ID="HiddenField1" ClientIDMode="Static" />
                            <asp:HiddenField runat="server" ID="HiddenField2" ClientIDMode="Static" />
                            <asp:HiddenField runat="server" ID="hdnSelectedLevel" ClientIDMode="Static" />
                             <asp:HiddenField runat="server" ID="Hdn1" ClientIDMode="Static" />
                            <asp:HiddenField runat="server" ID="hdnUserIDs" />
                            <table class="gridFrame" style="width: 100%">
                                <tr>
                                    <td>
                                        <table style="width: 100%">
                                            <tr>
                                                <td style="text-align: left;">
                                                    <asp:Label ID="lblselectapprovar" runat="server" CssClass="headerText" Text="API List" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <obout:Grid ID="grdapidetail" runat="server" AllowAddingRecords="False" AllowFiltering="True"
                                            Width="100%" AllowGrouping="True" AutoGenerateColumns="False" OnRebind="grdapidetail_OnRebind" OnSelect="grdapidetail_Select">
                                            <Columns>
                                             <obout:Column ID="Column2" DataField="Edit" Width="5%" AllowFilter="False" Align="center"
                                                        HeaderAlign="Center" HeaderText="Edit" Index="0" TemplateId="AggreAPIEdit">
                                                        <TemplateSettings TemplateId="AggreAPIEdit" />
                                                    </obout:Column>
                                                    <obout:Column DataField="ID" HeaderText="ID" Visible="False">
                                                    </obout:Column>
                                                <obout:Column DataField="APIName" HeaderText="API Name" Width="8%">
                                                </obout:Column>
                                                <obout:Column DataField="Purpose" HeaderText="Purpose" Width="15%">
                                                </obout:Column>
                                                <obout:Column DataField="APIURL" HeaderText="API URL" Width="10%">
                                                </obout:Column>
                                                <obout:Column DataField="InputParameter" HeaderText="Input Para" Width="10%">
                                                </obout:Column>
                                                <obout:Column DataField="OutputParameter" HeaderText="Output para" Width="15%">
                                                </obout:Column>
                                                <obout:Column DataField="Remark" HeaderText="Remark" Width="15%" Align="Center">
                                                </obout:Column>
                                                <obout:Column DataField="Active" HeaderText="Active" Width="6%">
                                                </obout:Column>
                                            </Columns>
                                             <Templates>
                                                    <obout:GridTemplate runat="server" ID="AggreAPIEdit">
                                                        <Template>
                                                            <asp:ImageButton ID="APIBtnEdit" runat="server" ImageUrl="../App_Themes/Blue/img/Edit16.png"
                                                                ToolTip="Edit" CausesValidation="false" />
                                                        </Template>
                                                    </obout:GridTemplate>
                                                </Templates>
                                        </obout:Grid>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:TabPanel>

        </asp:TabContainer>
    </center>
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
            if (document.getElementById("<%=ddlcompany.ClientID %>").value == "0") {
                showAlert("Please Enter Company name!", "error", "#");
                document.getElementById("<%=ddlcompany.ClientID %>").focus();
                return false;
            }
            if (document.getElementById("<%=ddlcutomer.ClientID %>").value == "0") {
                showAlert("Please Select Customer!", "error", "#");
                document.getElementById("<%=ddlcutomer.ClientID %>").focus();
                return false;
            }
            if (document.getElementById("<%=txtaggregator.ClientID %>").value == "") {
                showAlert("Please Enter Aggregator Name!", "error", "#");
                document.getElementById("<%=txtapi.ClientID %>").focus();
                return false;
            }
            return true
        };

    </script>
   
    <script type="text/javascript">
        var ddlcompany = document.getElementById("<%=ddlcompany.ClientID %>");
        var ddlcustomer = document.getElementById("<%=ddlcutomer.ClientID %>");

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
    </script>

   <%-- Code For Aggregator API --%>
    <script type="text/javascript">

        function SaveAggregatAPI()
        {
            var APIName = document.getElementById("<%= txtapi.ClientID %>");
            var Purpose = document.getElementById("<%= txtpurpose.ClientID %>");
            var InputPara = document.getElementById("<%= txtinputpara.ClientID %>");
            var OutPutPara = document.getElementById("<%= txtoutputpara.ClientID %>");
            var ApiURL = document.getElementById("<%= txturl.ClientID %>");
            var Remark = document.getElementById("<%= txtremark.ClientID %>");
            var AggregatorID = document.getElementById("<%=hdnselectedAggID.ClientID %>");
            var CompanyID = document.getElementById("<%=hdnCompanyid.ClientID %>");
            var CustomerID = document.getElementById("<%=hdncustomerid.ClientID %>");
            var RadioYes = document.getElementById("<%=apiactiveyes.ClientID %>");
            var hdnState = document.getElementById("<%=hdnAPIState.ClientID %>");
            if (APIName.value == "") {
                showAlert("Enter API Name", "Error");
                APIName.focus();
            } else if (InputPara.value == "") {
                showAlert("Enter Input Parameters", "Error");
                InputPara.focus();
            } else if (OutPutPara.value == "") {
                showAlert("Enter OutPut Parameters", "Error");
                OutPutPara.focus();
            } else if (ApiURL.value == "") {
                showAlert("Enter API URL", "Error");
                ApiURL.focus();
            }
           else {
                var obj1 = new Object();
                obj1.APIName = APIName.value.toString();
                obj1.Purpose = Purpose.value.toString();
                obj1.InputPara = InputPara.value.toString();
                obj1.OutPutPara = OutPutPara.value.toString();
                obj1.ApiURL = ApiURL.value.toString();
                obj1.Remark = Remark.value.toString();
                obj1.AggregatorID = AggregatorID.value.toString();
                obj1.CompanyID = CompanyID.value.toString();
                obj1.CustomerID = CustomerID.value.toString();
                if (RadioYes.checked == true) {
                    obj1.Active = "Yes";
                }
                else {
                    obj1.Active = "No";
                }
             
                PageMethods.WMSaveAPIDetail(obj1, hdnState.value, WMSaveContact_onSuccessed, WMSaveContact_onFailed);
            }
        }

        function WMSaveContact_onSuccessed(result) {
            if (result == "Some error occurred" || result == "") { showAlert("Error occurred", "Error", '#'); }
            else if (result == "API saved successfully") {
                showAlert(result, "info");
                document.getElementById("<%= txtapi.ClientID %>").value = "";
                document.getElementById("<%= txtpurpose.ClientID %>").value = "";
                document.getElementById("<%= txtinputpara.ClientID %>").value = "";
                document.getElementById("<%= txtoutputpara.ClientID %>").value = "";
                document.getElementById("<%= txturl.ClientID %>").value = "";
                document.getElementById("<%= txtremark.ClientID %>").value = "";
                document.getElementById("<%=apiactiveyes.ClientID %>").checked = true;
                grdapidetail.refresh();
                }
        }
        function WMSaveContact_onFailed() {
            showAlert("Error occurred", "Error", "#");
        }

    </script>
</asp:Content>

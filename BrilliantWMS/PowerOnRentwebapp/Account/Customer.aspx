<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/CRM.Master" AutoEventWireup="true" CodeBehind="Customer.aspx.cs"
    Inherits="BrilliantWMS.Account.Customer" EnableEventValidation="false"  %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="Obout.Ajax.UI" Namespace="Obout.Ajax.UI.FileUpload" TagPrefix="obout" %>
<%@ Register Src="../ContactPerson/UCContactPerson.ascx" TagName="UCContactPerson" TagPrefix="uc1" %>
<%@ Register Src="../Document/UC_AttachDocument.ascx" TagName="UC_AttachDocument"  TagPrefix="uc5" %>
<%@ Register Src="../CommonControls/UC_Date.ascx" TagName="UC_Date" TagPrefix="uc6" %>
<%@ Register Assembly="obout_Interface" Namespace="Obout.Interface" TagPrefix="obout" %>
<%@ Register Assembly="obout_Grid_NET" Namespace="Obout.Grid" TagPrefix="obout" %>
<%@ Register Src="../CommonControls/UCFormHeader.ascx" TagName="UCFormHeader" TagPrefix="uc7" %>
<%@ Register Src="../CommonControls/UCToolbar.ascx" TagName="UCToolbar" TagPrefix="uc8" %>
<%@ Register Src="../Tax/UC_StatutoryDetails.ascx" TagName="UC_StatutoryDetails" TagPrefix="uc2" %>
<%@ Register Src="../CommonControls/UC_Date.ascx" TagName="UC_Date" TagPrefix="uc2" %>
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
            <asp:TabPanel ID="TabCustomerList" runat="server" HeaderText="Customer List">
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
                    <asp:HiddenField ID="HdnAccountId" runat="server"></asp:HiddenField>
                    <asp:HiddenField ID="HdnOpeningBalId" runat="server" />
                    <asp:HiddenField ID="hdnselectedRec" runat="server" ClientIDMode="Static" />
                    <asp:HiddenField ID="hdnselectedRecPayment" runat="server" ClientIDMode="Static" />
                    <asp:HiddenField ID="hndCompanyid" runat="server"></asp:HiddenField>
                    <asp:HiddenField ID="hndState" runat="server" ClientIDMode="Static"></asp:HiddenField>
                    <asp:HiddenField ID="hdnmodestate" runat="server" ClientIDMode="Static" />
                    <asp:HiddenField ID="hdnCountry" runat="server" ClientIDMode="Static" />
                    <asp:HiddenField ID="hdnState" runat="server" ClientIDMode="Static" />
                    <asp:HiddenField ID="hdnLogo" runat="server" ClientIDMode="Static" />
                    <asp:HiddenField ID="hdnFilePath" runat="server" />
                    <asp:HiddenField ID="hdnFileTye" runat="server" />
                    <asp:HiddenField ID="hdnSelectedCompany" runat="server" ClientIDMode="Static" />
                    <asp:HiddenField ID="hdnSelectedDepartment" runat="server" ClientIDMode="Static" />
                    <asp:HiddenField ID="hdnapproverSearchSelectedRec" runat="server" ClientIDMode="Static" />
                    <asp:HiddenField ID="hdnapproversearchId" runat="server" ClientIDMode="Static" />
                     <asp:HiddenField ID="hdnCenterApproName" runat="server" ClientIDMode="Static" />
                    <asp:HiddenField ID="hdnCenterApproID" runat="server" ClientIDMode="Static" />
                    <asp:HiddenField ID="hdnPartSelectedRec" runat="server" ClientIDMode="Static" />
                     <asp:HiddenField ID="hdnlogovalue" runat="server"></asp:HiddenField>
                    <center>
                        <table class="gridFrame" style="width: 100%">
                            <tr>
                                <td>
                                    <table style="width: 100%">
                                        <tr>
                                            <td style="text-align: left;">
                                                <a id="headerText">
                                                    <asp:Label ID="lblheasertext" runat="server" Text="Customer List"></asp:Label></a>
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
                                    <obout:Grid ID="GvCustomer" runat="server" AutoGenerateColumns="False" AllowAddingRecords="False"
                                        AllowGrouping="True" AllowFiltering="True" Width="100%" OnSelect="GvCustomer_Select">
                                        <ScrollingSettings ScrollHeight="250" />
                                        <Columns>
                                            <obout:Column ID="Edit" Width="4%" AllowFilter="False" HeaderText="Edit" Index="0"
                                                TemplateId="GvTempEdit">
                                                <TemplateSettings TemplateId="GvTempEdit" />
                                            </obout:Column>
                                            <obout:Column DataField="ID" Visible="False" HeaderText="ID" Index="1">
                                            </obout:Column>
                                            <obout:Column DataField="CompanyName"  HeaderText="Company" Index="2" Width="15%"> </obout:Column>
                                            <obout:Column DataField="Name" HeaderText="Customer Name" Width="20%" Index="3">
                                            </obout:Column>
                                            <obout:Column HeaderText="Address Line 1" DataField="AddressLine1" Width="20%" Index="4">
                                            </obout:Column>
                                            <obout:Column HeaderText="City" DataField="City" Width="10%" Index="5">
                                            </obout:Column>
                                            <obout:Column DataField="PhoneNo" HeaderText="Phone No" Width="10%" Index="6">
                                            </obout:Column>
                                            <obout:Column DataField="EmailID" HeaderText="Email ID" Width="15%" Index="7">
                                            </obout:Column>
                                            <%-- <obout:Column DataField="CRMExpiryDate" HeaderText="Registration Expiry Date" Width="13%"
                                        Align="center" HeaderAlign="center" Index="7">
                                    </obout:Column>
                                    <obout:Column DataField="NoOfUsers" HeaderText="No.Of Users" Width="13%" HeaderAlign="right"
                                        Index="8">
                                    </obout:Column>--%>
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
            <asp:TabPanel ID="tabAccountInfo" runat="server" HeaderText="Customer Info">
                <ContentTemplate>
                    <center>
                        <table class="tableForm">
                            <tr>
                                 <td>
                                    <req>
                                    <asp:Label Id="lblcompanymain" runat="server" Text="Company"/>
                                    </req>
                                    :
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlcompanymain" ClientIDMode="Static" DataTextField="Name" DataValueField="ID" runat="server" Width="206px"
                                        onchange="onStateChange(this);">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ForeColor="Maroon" ControlToValidate="ddlcompanymain"
                                        InitialValue="0" runat="server" ErrorMessage="Please Select Company" Display="None"></asp:RequiredFieldValidator>
                                </td>
                                <td>
                                    <req>
                                    <asp:Label Id="lblcustname" runat="server" Text="Customer Name"></asp:Label>
                                    </req>
                                    :
                                </td>
                                <td>
                                    <asp:TextBox ID="txtcompname" runat="server" MaxLength="50" Width="200px"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RfValCompany" runat="server" ControlToValidate="txtcompname"
                                        Display="None" ErrorMessage="Please Enter Company Name" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                </td>
                                



                                <td>
                                    <req><asp:Label Id="lblemailid" runat="server" Text="Email ID"/> </req>
                                    :
                                </td>
                                <td>
                                    <asp:TextBox ID="txtemailid" runat="server" MaxLength="100" Width="200px"></asp:TextBox>
                                    <asp:RegularExpressionValidator ID="RegExpEmailID" runat="server" ControlToValidate="txtemailid"
                                        Display="None" ErrorMessage="Please enter valid Email ID" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                        ValidationGroup="Save"></asp:RegularExpressionValidator>
                                    <asp:RequiredFieldValidator ID="RFValEmailID" runat="server" ControlToValidate="txtemailid"
                                        Display="None" ErrorMessage="Please enter Email ID" ValidationGroup="Save"></asp:RequiredFieldValidator>
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
                                    <asp:Label ID="lbladdress3" runat="server" Text="Address Line 3" />
                                    :
                                </td>
                                <td>
                                    <asp:TextBox ID="txtAddress3" runat="server" MaxLength="100" onkeyup="TextBox_KeyUp(this,'SpanAddressLine3','100');"
                                        TextMode="MultiLine" Width="200px"></asp:TextBox>
                                    <br />
                                    <span class="watermark">
                                        <asp:Label ID="lblcharremain3" runat="server" Text="characters remaining
                                    out of 100 " />&nbsp;<span id="SpanAddressLine3"> 100</span></span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <req>
                                    <asp:Label Id="lblcountry" runat="server" Text="Country"/>
                                    </req>
                                    :
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlCountry" ClientIDMode="Static" runat="server"
                                        onchange="print_state('ddlState',this.selectedIndex); onCountryChange(this);"
                                        Width="206px">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RFValddlCountry" runat="server" ForeColor="Maroon"
                                        ValidationGroup="AddressSubmit" InitialValue="0" ControlToValidate="ddlCountry"
                                        ErrorMessage="Select Country" Display="None"></asp:RequiredFieldValidator>
                                </td>
                                <td>
                                    <req>
                                    <asp:Label Id="lblstate" runat="server" Text="State"/>
                                    </req>
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
                                    <req>
                                    <asp:Label Id="lblcity" runat="server" Text="City"/>
                                    </req>
                                    :
                                </td>
                                <td>
                                    <asp:TextBox ID="txtCity" runat="server" MaxLength="50" Width="200px" onkeypress="return AllowAlphabet(event)"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RFValddlBCity" runat="server" ControlToValidate="txtCity"
                                        Display="None" ErrorMessage="Please Select City" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblzip" runat="server" Text="ZIP Code" />
                                    :
                                </td>
                                <td>
                                    <asp:TextBox ID="txtZipCode" runat="server" MaxLength="10" Width="200px"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:Label ID="lbllandmark" runat="server" Text="Landmark" />
                                    :
                                </td>
                                <td>
                                    <asp:TextBox ID="txtLandMark" runat="server" MaxLength="50" Width="200px"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:Label ID="lblphone" runat="server" Text="Phone No" />
                                    :
                                </td>
                                <td>
                                    <asp:TextBox ID="txtphoneno" runat="server" MaxLength="10" onkeydown="return AllowPhoneNo(this, event);"
                                        onkeypress="return AllowPhoneNo(this, event);" Width="200px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblfax" runat="server" Text="Fax No. " />
                                    :
                                </td>
                                <td>
                                    <asp:TextBox ID="txtFax" runat="server" MaxLength="10" onkeydown="return AllowPhoneNo(this, event);"
                                        onkeypress="return AllowPhoneNo(this, event);" Width="200px"></asp:TextBox>
                                </td>
                                <td>
                                  <req>
                                    <asp:Label ID="lblorderformat" runat="server" Text="Order No. Format"></asp:Label>
                                    </req> :
                                </td>
                                <td style="text-align: left;">
                                    <asp:TextBox ID="TextBox1" runat="server" Style="text-align: right" MaxLength="50" Width="90px"></asp:TextBox> -
                                     <asp:TextBox ID="TextBox2" runat="server" Style="text-align: right" MaxLength="50"
                                        Width="90px" onkeypress="return fnAllowDigitsOnly(event)"></asp:TextBox>
                                     <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="TextBox1"
                                        Display="None" ErrorMessage="Please Enter Order Format" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                     <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="TextBox2"
                                        Display="None" ErrorMessage="Please Enter Order Format" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                </td>
                                <td>
                                    <asp:Label ID="lblwebsite" runat="server" Text="Website" />
                                    :
                                </td>
                                <td>
                                    <asp:TextBox ID="txtwebsite" runat="server" MaxLength="50" Width="200px"></asp:TextBox>
                                    <asp:RegularExpressionValidator ID="RegWebSite" runat="server" ControlToValidate="txtwebsite"
                                        Display="None" ErrorMessage="Please Enter Valid WebSite" ValidationExpression="(((file|gopher|news|nntp|telnet|http|ftp|https|ftps|sftp)://)|(www\.))+(([a-zA-Z0-9\._-]+\.[a-zA-Z]{2,6})|([0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}))(/[a-zA-Z0-9\&amp;%_\./-~-]*)?"
                                        ValidationGroup="Save"></asp:RegularExpressionValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <req>
                                    <asp:Label Id="lblactive" runat="server" Text="Active"/>
                                    </req>
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
                                <td></td>
                                <td></td>
                                <td></td>
                            </tr>
                            <tr>
                                <td colspan="6">
                                    <br />
                                    <asp:Label ID="lbllogo" runat="server" Text="Upload Logo" />
                                    :
                                    <asp:FileUpload ID="fileUploadCompanyLogo" runat="server" ClientIDMode="Static" onchange="fileUploaderValidationImgOnly(this,20480)" />
                                    <asp:LinkButton runat="server" ID="lnkUpdateProfileImg" Text="Upload" Visible="False" CausesValidation="False"></asp:LinkButton>
                                    <br />
                                    <span class="watermark">Only jpg|.jpeg|.gif|.png|.bmp files
                                        <br />
                                        Max Limit 20 KB </span>
                                </td>
                            </tr>
                            <tr>
                                <td class="auto-style1"></td>
                                <td class="auto-style1"></td>
                                <td class="auto-style1"></td>
                                <td class="auto-style1"></td>
                                <td colspan="2" class="auto-style1">
                                    <asp:Button ID="btncustomernext" runat="server" Text="  Next  " Visible="false" OnClick="btncustomernext_Click"
                                        OnClientClick="return Checkvalidations();" />
                                </td>
                            </tr>
                        </table>
                    </center>
                </ContentTemplate>
            </asp:TabPanel>
            <asp:TabPanel ID="tableDeptInfo" runat="server" HeaderText="Department Info">
                <ContentTemplate>
                    <center>
                        <table class="tableForm">
                            <tr>
                                <td>
                                    <asp:Label ID="lblcompany" runat="server" Text=" Customer"></asp:Label>
                                    :
                                </td>
                                <td style="text-align: left">
                                    <asp:TextBox ID="txtcompanynamedept" ReadOnly="True" runat="server" MaxLength="50"
                                        Width="200px"></asp:TextBox>
                                </td>
                                <td>
                                    <req>
                                    <asp:Label ID="lbldepartment" runat="server" Text=" Department Name :"></asp:Label>



                                    </req>
                                </td>
                                <td style="text-align: left">
                                    <asp:TextBox ID="txtdepartment" runat="server" MaxLength="50" Width="200px"></asp:TextBox>
                                </td>
                                <td>
                                    <req><asp:Label ID="lbldeptcode" runat="server" Text="Department Code :"></asp:Label>


</req>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtdeptcode" runat="server" MaxLength="50" Width="200px"></asp:TextBox> </td>
                            </tr>
                            <tr>
                                <td>
                                    <req><asp:Label ID="lblapproval" runat="server"  Text="Approval level"></asp:Label>


  </req>
                                    :
                                </td>
                                <td style="text-align: left">
                                    <asp:TextBox ID="txtapproval" runat="server" Style="text-align: right" MaxLength="50" ClientIDMode="Static" 
                                     onkeypress="return fnAllowDigitsOnly(event)"    Width="200px"></asp:TextBox>     
                                </td>
                                <td>
                                    <req>
                                    <asp:Label ID="lblautocancel" runat="server" Text=" Auto Cancel"></asp:Label>


 </req>
                                    :
                                </td>
                                <td style="text-align: left">
                                    <asp:DropDownList ID="ddlautocancel" runat="server" DataTextField="Name" DataValueField="ID" onchange="setvalue()"
                                        Width="206px">
                                        <asp:ListItem Text="-Select-" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="Yes" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="No" Value="3"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <req>
                                    <asp:Label ID="lblcanceldays" runat="server" Text="Auto Cancellation Days"></asp:Label>



                                    :</req>
                                </td>
                                <td style="text-align: left">
                                    <asp:TextBox ID="txtcanceldays" runat="server" Style="text-align: right" MaxLength="50" 
                                        Width="200px" onkeypress="return fnAllowDigitsOnly(event)"  ></asp:TextBox>  
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblappreminder" runat="server" Text="Approval Reminder"></asp:Label>
                                    :
                                </td>
                                <td style="text-align: left">
                                    <obout:OboutCheckBox ID="chkapproremyes" runat="server" OnClick="enableTextBox1()" ControlsToDisable="" ControlsToEnable="" FolderStyle="" ParentCheckBoxID="" State="Unchecked">
                                    </obout:OboutCheckBox>
                                </td>
                                <td>
                                    <asp:Label ID="lblremaprrosche" runat="server" Text="Reminder Schedule"></asp:Label>
                                    :
                                </td>
                                <td style="text-align: left">
                                    <asp:TextBox ID="txtremschedule" runat="server" Enabled="False" Style="text-align: right"
                                        MaxLength="50" Width="200px" onkeypress="return fnAllowDigitsOnly(event)"></asp:TextBox>
                                    &nbsp;&nbsp;<asp:Label ID="lblremdays" runat="server" Text="Days"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblGWCDeliveries" runat="server" Text="GWC Deliveries"></asp:Label>
                                    :
                                </td>
                                <td style="text-align: left;">
                                    <obout:OboutCheckBox ID="chkDeliver" runat="server" OnClick="enableSLA()" ControlsToDisable="" ControlsToEnable="" FolderStyle="" ParentCheckBoxID="" State="Unchecked"></obout:OboutCheckBox>
                                </td>
                            </tr>
                            <tr>
                                 <td>
                                   
                                     <asp:Label ID="lbldeliverydays" runat="server" Text="Max Delivery Days"></asp:Label>
                                    :
                                </td>
                                <td style="text-align: left">
                                   
                                 <asp:TextBox ID="txtdeliverydays" runat="server" Style="text-align: right" MaxLength="50" Text="10"
                                        Width="200px" onkeypress="return fnAllowDigitsOnly(event)"></asp:TextBox>
                                </td>
                                <td>
                                     <req><asp:Label ID="lblacti" runat="server" Text="Active"></asp:Label>



                                    :</req>
                               </td>
                                 <td style="text-align: left">
                                    <obout:OboutRadioButton ID="OboutRadioButton1" runat="server" Checked="True" FolderStyle=""
                                        GroupName="Active" Text="Yes">
                                    </obout:OboutRadioButton>
                                    <obout:OboutRadioButton ID="OboutRadioButton2" runat="server" FolderStyle="" GroupName="Active"
                                        Text="No">
                                    </obout:OboutRadioButton>
                                </td>
                                 <td>
                                     <asp:Label ID="lblecommerce" runat="server" Text="ECommerce"></asp:Label>
                                    :

                                </td>
                                <td style="text-align: left">
                                   <obout:OboutCheckBox ID="chkecommerce" runat="server" ControlsToDisable="" ControlsToEnable="" FolderStyle="" ParentCheckBoxID="" State="Unchecked"></obout:OboutCheckBox>

                                </td>

                            </tr>
                            <tr>
                                <td>
                                   <asp:Label ID="lblpricechange" runat="server" Text="Price Change" ></asp:Label>
                                    :
                                </td>
                                <td style="text-align: left">
                                    <obout:OboutCheckBox ID="chkpricechange" runat="server" Enabled="False" ControlsToDisable="" ControlsToEnable="" FolderStyle="" ParentCheckBoxID="" State="Unchecked" onclick=" RemoveFA()"></obout:OboutCheckBox>
                                </td>
                               
                                <td>
                                   <asp:Label ID="lblfinapprover" runat="server" Text="Financial Approver"></asp:Label>
                                    :
                                </td>
                                <td style="text-align: left;">
                                  <asp:TextBox ID="txtbomsku" Enabled="False" runat="server" MaxLength="50" Width="220px"></asp:TextBox>
                                  <img id="imgSearch" src="../App_Themes/Grid/img/search.jpg" title="Search SKU" runat="server" style="cursor: pointer;" onclick="openProductSearch('0')" />
                                 &nbsp;&nbsp;&nbsp;</td>
                                 <td>
                                </td>
                                <td style="text-align: left">
                                </td>
                            </tr>
                            <tr>
                                <td>
                                   
                                </td>
                                <td style="text-align: left">
                                   
                                </td>
                                <td></td>
                                <td></td>
                                <td></td>
                                 <td>
                                    <asp:Button ID="btnadd" runat="server" Text="  Submit  " OnClick="btnadd_Click" OnClientClick="return CheckDeptvalidations() && CheckValue();" />
                                </td>
                            </tr>
                            <tr style="visibility: hidden;">
                                <td>
                                    <asp:Label ID="lblaautoeminder" runat="server" Text="Auto Cancel Reminder"></asp:Label>
                                    :
                                </td>
                                <td style="text-align: left">
                                    <obout:OboutCheckBox ID="chkautocancelreminder" runat="server" OnClick="enableTextBox()" ControlsToDisable="" ControlsToEnable="" FolderStyle="" ParentCheckBoxID="" State="Unchecked">
                                    </obout:OboutCheckBox>
                                </td>
                                <td>
                                    <asp:Label ID="lblautoremschedule" runat="server" Text="Reminder Schedule"></asp:Label>
                                    :
                                </td>
                                <td style="text-align: left">
                                    <asp:TextBox ID="txtautoremsche" runat="server" Enabled="False" Style="text-align: right"
                                        MaxLength="50" Width="200px" onkeypress="return fnAllowDigitsOnly(event)"></asp:TextBox>
                                    &nbsp;&nbsp;<asp:Label ID="lblautodays" runat="server" Text="Days"></asp:Label>
                                </td>
                                <td></td>
                                <td style="text-align: left"></td>
                            </tr>
                           
                        </table>
                        <table class="gridFrame" width="100%">
                            <tr>
                                <td>
                                    <table style="width: 100%">
                                        <tr>
                                            <td style="text-align: left;">
                                                <a id="A1">
                                                    <asp:Label ID="lbldesilist" runat="server" Text="Department List"></asp:Label></a>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <obout:Grid ID="Grid1" runat="server" AllowAddingRecords="False" AllowFiltering="True"
                                        AllowGrouping="True" AutoGenerateColumns="False" Width="100%" PageSize="4" >                       
                                        <ScrollingSettings ScrollHeight="80px"/>
                                        <Columns>
                                            <obout:Column ID="Column2" DataField="Edit" Width="4%" AllowFilter="False" HeaderText="Edit"
                                                Index="0" TemplateId="GridTemplate1">
                                                <TemplateSettings TemplateId="GridTemplate1" />
                                            </obout:Column>
                                            <obout:Column ID="Column3" DataField="payment" Width="7%" Align="center" AllowFilter="False" HeaderText="Payment Method"
                                                Index="1" TemplateId="GridTemplate2">
                                                <TemplateSettings TemplateId="GridTemplate2" />
                                            </obout:Column>
                                            <obout:Column DataField="ID" HeaderText="ID" Visible="False" Index="2">
                                            </obout:Column>
                                            <obout:Column DataField="Territory" HeaderText="Department Name" Width="8%" Index="3">
                                            </obout:Column>
                                            <obout:Column DataField="StoreCode" HeaderText="Department Code" Width="5%" Index="4">
                                            </obout:Column>
                                            <obout:Column DataField="Approvallevel" HeaderText="Approval level" Align="center" Width="5%" Index="5">
                                            </obout:Column>
                                            <obout:Column DataField="AutoCancel" HeaderText="Auto Cancel" Align="center" Width="5%" Index="6">
                                            </obout:Column>
                                            <obout:Column DataField="cancelDays" HeaderText="Cancel Days" Align="center" Width="5%" Index="7">
                                            </obout:Column>
                                             <obout:Column DataField="Deliveries" HeaderText="GWC Deliveries" Align="center" Width="6%" Index="8">
                                            </obout:Column>
                                             <obout:Column DataField="Ecommerce" HeaderText="Ecommerce" Align="center" Width="5%" Index="9">
                                            </obout:Column>
                                             <obout:Column DataField="FinApprover" HeaderText="Fin Approver"  Width="6%" Index="11">
                                            </obout:Column>
                                            <obout:Column DataField="addressTypetext" HeaderText="address Type"  Width="6%" Index="12">
                                            </obout:Column>
                                            <obout:Column DataField="PrimeDays" HeaderText="Prime Hours" Align="center" Width="6%" Index="13" TemplateId="Prime">
                                                <TemplateSettings TemplateId="Prime" />
                                            </obout:Column>
                                            <obout:Column DataField="ExpressDays" HeaderText="Express Hours" Align="center" Width="6%" Index="14" TemplateId="Express">
                                                 <TemplateSettings TemplateId="Express" />
                                            </obout:Column>
                                            <obout:Column DataField="RegularDays" HeaderText="Regular Hours" Align="center" Width="6%" Index="15" TemplateId="Regular">
                                                 <TemplateSettings TemplateId="Regular" />
                                            </obout:Column>
                                        </Columns>
                                        <Templates>
                                            <obout:GridTemplate runat="server" ID="GridTemplate1">
                                                <Template>
                                                    <asp:ImageButton ID="imgBtnEdit" runat="server" ImageUrl="../App_Themes/Blue/img/Edit16.png"
                                                      OnClick="imgBtnEditbom_OnClick" OnClientClick="SelectedPrdRec()"   ToolTip='<%# (Container.DataItem["ID"].ToString()) %>' />
                                                </Template>
                                            </obout:GridTemplate>
                                            <obout:GridTemplate runat="server" ID="GridTemplate2">
                                                <Template>
                                                     <img id="imgbuttonremove" src="../App_Themes/Blue/img/add.png" alt="Method" title="Payment Method" onclick="Openmethod('<%# (Container.DataItem["ID"].ToString()) %>');"
                                                      style="cursor: pointer;" />
                                                </Template>
                                            </obout:GridTemplate>
                                            <obout:GridTemplate ID="Prime" runat="server" ControlID="" ControlPropertyName="">
                                                <Template>
                                                    <%#(Container.DataItem["PrimeDays"].ToString()=="0" ? "NA" : Container.DataItem["PrimeDays"].ToString()) %>                                                    
                                                </Template>
                                            </obout:GridTemplate>
                                            <obout:GridTemplate ID="Express" runat="server" ControlID="" ControlPropertyName="">
                                                <Template>
                                                    <%#(Container.DataItem["ExpressDays"].ToString()=="0" ? "NA" : Container.DataItem["ExpressDays"].ToString()) %>                                                    
                                                </Template>
                                            </obout:GridTemplate>
                                            <obout:GridTemplate ID="Regular" runat="server" ControlID="" ControlPropertyName="">
                                                <Template>
                                                    <%#(Container.DataItem["RegularDays"].ToString()=="0" ? "NA" : Container.DataItem["RegularDays"].ToString()) %>                                                    
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
            <asp:TabPanel ID="tabContactInfo" runat="server" HeaderText="Contact Person Info"
                Visible="false">
                <ContentTemplate>
                    <uc1:UCContactPerson ID="UCContactPerson1" runat="server" />
                </ContentTemplate>
            </asp:TabPanel>

            <asp:TabPanel ID="tabSatutoryInfo" runat="server" HeaderText="Parameter Info" TabIndex="5">
                    <ContentTemplate>
                        <center>
                            <uc2:UC_StatutoryDetails ID="UC_StatutoryDetails1" runat="server" />
                        </center>
                    </ContentTemplate>
                </asp:TabPanel>

            <asp:TabPanel ID="tabAddressInfo" runat="server" HeaderText="Address Info">
                <ContentTemplate>
                    <uc4:UCAddress ID="UCAddress1" runat="server" />
                </ContentTemplate>
            </asp:TabPanel>
            <asp:TabPanel ID="tabSatutoryInfo12" runat="server" HeaderText="Cost Center">
                <ContentTemplate>
                    <center>
                        <table class="tableForm">
                            <tr>
                                <td>
                                    <req><asp:Label ID="lblcostcenter" runat="server" Text="Cost Center Name"></asp:Label></req>
                                    :
                                </td>
                                <td style="text-align: left">
                                  <asp:TextBox ID="txtCenterName" runat="server" Width="180px"  onKeyPress="return alpha(this,event);"></asp:TextBox>
                                    
                                </td>
                                <td>
                                    <req><asp:Label ID="lblcode" runat="server" Text="Code"></asp:Label><req>:
                                </td>
                                <td style="text-align: left">
                                    <asp:TextBox ID="txtcode" runat="server" Width="180px" ></asp:TextBox>
                                    
                                </td>
                            </tr>
                            <tr">
                                <td>
                                    <req><asp:Label ID="lblcostapprover" runat="server" Text="Financial Approver"> </asp:Label></req>
                                    :
                                </td>
                                <td style="text-align: left;">
                                  <asp:TextBox ID="txtcostapprover" Enabled="false" runat="server" MaxLength="50" Width="180px"></asp:TextBox>
                                  <img id="img1" src="../App_Themes/Grid/img/search.jpg" title="Search SKU" runat="server" style="cursor: pointer;" onclick="OpenApprover('0')" />
                                 </td>
                                <td>
                                    <asp:Label ID="lblRemark" runat="server" Text="Remark"></asp:Label>
                                    :
                                </td>
                                <td style="text-align: left">
                                    <asp:TextBox ID="txtremark" runat="server" Width="360px" MaxLength="50" ValidationGroup="Save"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td></td>
                                <td></td>
                                 <td style="text-align: right;">
                                                  <input type="button" runat="server" value="  Add  " id="btnaddapprover" onclick="SaveCostCenter();" />
                                 </td>
                            </tr>
                        </table>
                        <asp:HiddenField ID="hdnDesignationID" runat="server" />
                        <table class="gridFrame" width="70%">
                            <tr>
                                <td>
                                    <table style="width: 100%">
                                        <tr>
                                            <td style="text-align: left;">
                                                <a style="color: white; font-size: 15px; font-weight: bold;">
                                                    <asp:Label ID="lbldeptList" runat="server" Text="Cost Center List"></asp:Label></a>
                                                    
                                            </td>
                                           
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <obout:Grid ID="gvCostCenter" runat="server" AllowAddingRecords="false" AllowFiltering="true"
                                        AllowGrouping="true" AutoGenerateColumns="false" Width="100%" OnRebind="gvCostCenter_RebindGrid">
                                        <Columns>
                                           <obout:Column DataField="Remove" HeaderText="Remove" Width="4%" AllowSorting="false"
                                              Align="Center" HeaderAlign="Center" Index="1">
                                             <TemplateSettings TemplateId="GvRemoveSku" />
                                           </obout:Column>
                                            <obout:Column DataField="ID" HeaderText="ID" Visible="false">
                                            </obout:Column>
                                             <obout:Column DataField="CenterName" HeaderText="Center Name" Width="8%">
                                            </obout:Column>
                                            <obout:Column DataField="Code" HeaderText="Center Code" Width="5%">
                                            </obout:Column>
                                            <obout:Column DataField="Approver" HeaderText="Approver Name" Width="8%">
                                            </obout:Column>
                                            <obout:Column DataField="Remark" HeaderText="Remark" Width="10%">
                                            </obout:Column>
                                        </Columns>
                                        <Templates>
                                            <obout:GridTemplate ID="GvRemoveSku" runat="server">
                                               <Template>
                                               <img id="imgbuttonremove" src="../App_Themes/Grid/img/Remove16x16.png" alt="Remove" title="Remove" onclick="RemoveMethod('<%# (Container.DataItem["ID"].ToString()) %>');"
                                                style="cursor: pointer;" />
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
            <asp:TabPanel ID="tabAttachedDocumentInfo" runat="server" HeaderText="Document">
                <ContentTemplate>
                    <uc5:UC_AttachDocument ID="UC_AttachDocument1" runat="server" />
                </ContentTemplate>
            </asp:TabPanel>
            <asp:TabPanel ID="tabAccountHistory" runat="server" HeaderText="Configuration">
                <ContentTemplate>
                    <center>
                        <table class="tableForm">
                            <tr>
                                <td>
                                    <req><asp:Label ID="lblsubscript" runat="server" Text="Subscription Type"></asp:Label></req>
                                    :
                                </td>
                                <td style="text-align: left">
                                    <asp:DropDownList ID="ddlsubscription" ClientIDMode="Static" DataTextField="Value" DataValueField="Id" runat="server" Width="206px" onchange="GetSubscription();">
                                   </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" ForeColor="Maroon" ControlToValidate="ddlsubscription"
                                        InitialValue="0" runat="server" ValidationGroup="Save" ErrorMessage="Please Select Subscription" Display="None"></asp:RequiredFieldValidator>
                                      <asp:HiddenField ID="hdnsubscription" runat="server"></asp:HiddenField>
                                    
                                </td>
                                <td>
                                    <req><asp:Label ID="lblstartdate" runat="server" Text="Start Date"></asp:Label><req>:
                                </td>
                                <td style="text-align: left">
                                    <uc2:UC_Date ID="SubscriptStartDate" runat="server" />
                                     <asp:CustomValidator ID="custValidatestartdate" runat="server" ClientValidationFunction="validatestartdate"
                                    ValidationGroup="Save" ErrorMessage="Please select Subscription Start Date" ></asp:CustomValidator>  
                                    
                                </td>
                                 <td>
                                    <req><asp:Label ID="lblenddate" runat="server" Text="End Date"></asp:Label><req>:
                                </td>
                                <td style="text-align: left">
                                    <uc2:UC_Date ID="SubscriptEndDate" runat="server" />
                                     <asp:CustomValidator ID="custValienddate" runat="server" ClientValidationFunction="validatesEndDate"
                                    ValidationGroup="Save" ErrorMessage="Please select Subscription End Date" ></asp:CustomValidator> 
                                </td>
                            </tr>
                            <tr">
                                <td>
                                    <req><asp:Label ID="lblnowarehouse" runat="server" Text="No. of Warehouse"> </asp:Label></req>
                                    :
                                </td>
                                <td style="text-align: left;">
                                  <asp:TextBox ID="txtnowarehouse" runat="server" Style="text-align: right" MaxLength="50" Width="160px" onkeypress="return fnAllowDigitsOnly(event)"></asp:TextBox>
                                  </td>
                                <td>
                                    <asp:Label ID="lblbusiness" runat="server" Text="Business Function"></asp:Label>
                                    :
                                </td>
                                <td style="text-align: left"> 
                                <asp:CheckBoxList ID="chkbusiness"  runat="server" RepeatDirection="Horizontal" CssClass="test">
                                      <asp:ListItem Text="WMS" Value="WMS"></asp:ListItem>
                                      <asp:ListItem Text="OMS" Value="OMS"></asp:ListItem>
                                      <asp:ListItem Text="Delivery" Value="Delivery"></asp:ListItem>
                                     <asp:ListItem Text="3PL" Value="3PL"></asp:ListItem>
                                </asp:CheckBoxList>
                                     <asp:CustomValidator ID="CustValidate" runat="server" ClientValidationFunction="ValidateCheckBoxList"
                                    ValidationGroup="Save" ErrorMessage="Please select at least one fuction" ></asp:CustomValidator>
                                </td>
                                <td></td> <td></td>
                            </tr>
                        </table>
                        <%-- <uc10:UCPaymentDetail ID="UCPaymentDetail1" runat="server" />--%>
                    </center>
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
                                                <obout:Column DataField="ID" Visible="false" Width="2%">
                                                </obout:Column>
                                                <%-- <obout:Column DataField="Territory" HeaderText="Deartment" Width="15%">
                                                    <TemplateSettings TemplateId="GridTemplate1" />
                                                </obout:Column>--%>
                                                <obout:Column DataField="RateDetails" HeaderText="Rate Details"   Width="20%">
                                                </obout:Column>
                                                <obout:Column DataField="Rate" HeaderText="Rate" Align="right" HeaderAlign="center" Width="7%">
                                                </obout:Column>
                                                <obout:Column DataField="RateType" HeaderText="Rate Type"  Width="10%">
                                                </obout:Column>
                                                 <obout:Column DataField="FromDate" HeaderText="From Date" Align="Center" HeaderAlign="center" Width="12%" DataFormatString="{0:dd-MM-yyyy}">
                                                </obout:Column>
                                                 <obout:Column DataField="ToDate" HeaderText="To Date" Align="Center" HeaderAlign="center" Width="12%" DataFormatString="{0:dd-MM-yyyy}">
                                                </obout:Column>
                                                 <obout:Column DataField="EffDate" HeaderText="Effective date" Align="Center" HeaderAlign="center" Width="12%" DataFormatString="{0:dd-MM-yyyy}">
                                                </obout:Column>
                                                <obout:Column DataField="Active" HeaderText="Active" Width="10%" Align="Center" HeaderAlign="center">
                                                </obout:Column>
                                            </Columns>
                                           
                                        </obout:Grid>
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
        function validatestartdate(source, args) {
            var SubscriptStartDate = getDateFromUC("<%= SubscriptStartDate.ClientID %>");
            if (SubscriptStartDate != "") {
                isValid = true;
                return;
            }
            args.IsValid = false;
        }

        function validatesEndDate(source, args) {
            var Subscriptend = getDateFromUC("<%= SubscriptEndDate.ClientID %>");
            if (Subscriptend != "") {
                isValid = true;
                return;
            }
            args.IsValid = false;
        }



        function ValidateCheckBoxList(source, args) {
            var checkBoxList = document.getElementById("<%=chkbusiness.ClientID %>");
            var checkboxes = checkBoxList.getElementsByTagName("input");
            //var isValid = false;
            for (var i = 0; i < checkboxes.length; i++) {
                if (checkboxes[i].checked) {
                    isValid = true;
                    return;
                }
            }
            args.IsValid = false;

        }


</script>
    <script type="text/javascript">
        function CheckLevel() {
            var levelvalue = document.getElementById('<%=txtapproval.ClientID%>').value;
            if (levelvalue > 3) {
                alert("Please Enter Approvel Level Less than or Equal to 3");
                document.getElementById('<%=txtapproval.ClientID%>').value = "";
            }
        }

        function fnAllowDigitsOnly(key) {
            var keycode = (key.which) ? key.which : key.keyCode;
            if ((keycode < 48 || keycode > 57) && (keycode != 8) && (keycode != 9) && (keycode != 11) && (keycode != 16) && (keycode != 37) && (keycode != 38) && (keycode != 39) && (keycode != 40) && (keycode != 219) && (keycode != 127)) {
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

        function SetTextInTextBox2() {
            alert("Please Enter Number Only!");
        }

        function chechnumber() {
            var orderno = document.getElementById("<%=TextBox2.ClientID %>").value;
            if (isNaN(orderno)) {
            } else {
                document.getElementById("<%=TextBox2.ClientID %>").value = "";
                alert("Please Enter Number Only!");
            }
        }


        function Checkvalidations() {

            var email = document.getElementById("<%=txtemailid.ClientID %>");
            var filter = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;


            if (document.getElementById("<%=txtcompname.ClientID %>").value == "") {
                showAlert("Please Enter Company name!", "error", "#");
                document.getElementById("<%=txtcompname.ClientID %>").focus();
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
            if (document.getElementById("<%=TextBox1.ClientID %>").value == "") {
                showAlert("Please Enter Order Format!", "error", "#");
                document.getElementById("<%=TextBox1.ClientID %>").focus();
                return false;
            }
            if (document.getElementById("<%=TextBox2.ClientID %>").value == "") {
                showAlert("Please Enter Order Format!", "error", "#");
                document.getElementById("<%=TextBox2.ClientID %>").focus();
                return false;
            }


            return true
        };

        function CheckDeptvalidations() {

            var textBoxID = document.getElementById("<%=txtremschedule.ClientID %>");
            var txtautoremsche = document.getElementById("<%=txtautoremsche.ClientID %>");

            if (document.getElementById("<%=txtdepartment.ClientID %>").value == "") {
                showAlert("Please Enter Department name!", "error", "#");
                document.getElementById("<%=txtdepartment.ClientID %>").focus();
                return false;
            }
            if (document.getElementById("<%=txtdeptcode.ClientID %>").value == "") {
                showAlert("Please Enter Department Code!", "error", "#");
                document.getElementById("<%=txtdeptcode.ClientID %>").focus();
                return false;
            }
            if (document.getElementById("<%=txtapproval.ClientID %>").value == "") {
                showAlert("Please Enter Approval Level!", "error", "#");
                document.getElementById("<%=txtapproval.ClientID %>").focus();
                return false;
            }
            if (document.getElementById("<%=ddlautocancel.ClientID %>").value == "1") {
                showAlert("Please Select Auto Cancel!", "error", "#");
                document.getElementById("<%=ddlautocancel.ClientID %>").focus();
                return false;
            }
            if (document.getElementById("<%=txtcanceldays.ClientID %>").value == "") {
                showAlert("Please Enter Auto Cancellation Days!", "error", "#");
                document.getElementById("<%=txtcanceldays.ClientID %>").focus();
                return false;
            }
            if (textBoxID.value == "" && document.getElementById("<%= chkapproremyes.ClientID %>").checked == true) {
                showAlert("Please Enter Approval Reminder Days!", "error", "#");
                document.getElementById("<%=txtremschedule.ClientID %>").focus();
                return false;
            }
            if (txtautoremsche.value == "" && document.getElementById("<%= chkautocancelreminder.ClientID %>").checked == true) {
                showAlert("Please Enter Approval Reminder Days!", "error", "#");
                document.getElementById("<%=txtautoremsche.ClientID %>").focus();
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
            var ddlCountry = document.getElementById("<%=ddlCountry.ClientID %>");
            ddlCountry.value = country;

            print_state('ddlState', ddlCountry.selectedIndex);
            ddlState = document.getElementById("<%=ddlState.ClientID %>");
            ddlState.value = state;

        }


        function enableTextBox() {
            var textBoxID = "<%= txtautoremsche.ClientID %>";
            if (document.getElementById("<%= chkautocancelreminder.ClientID %>").checked == true)
                document.getElementById(textBoxID).disabled = false;
            else
                document.getElementById(textBoxID).disabled = true;
        }

        function enableTextBox1() {
            var textBoxID = "<%= txtremschedule.ClientID %>";
            if (document.getElementById("<%= chkapproremyes.ClientID %>").checked == true)
                document.getElementById(textBoxID).disabled = false;
            else
                document.getElementById(textBoxID).disabled = true;
        }

        function enableSLA() {

            if (document.getElementById("<%= chkDeliver.ClientID %>").checked == true) {
                var hdnSelectedDepartment = document.getElementById("hdnSelectedDepartment");
                var $SelDept = hdnSelectedDepartment.value;
                if ($SelDept == "") $SelDept = 0;
                window.open('../Account/SLA.aspx?DEPTID=' + $SelDept + '', null, 'height=200, width=700,status= 0, resizable= 1, scrollbars=0, toolbar=0,location=center,menubar=0, screenX=300; screenY=50');
            }
        }

    </script>
    <script type="text/javascript">

        var companyname = document.getElementById("<%= txtcompanynamedept.ClientID %>");
        var companyid = document.getElementById("<%= hndCompanyid.ClientID %>");
        var deptname = document.getElementById("<%=txtdepartment.ClientID %>");
        var deptcode = document.getElementById("<%=txtdeptcode.ClientID%>");
        var approval = document.getElementById("<%=txtapproval.ClientID %>");
        var autocancel = document.getElementById("<%=ddlautocancel.ClientID %>");
        var canceldays = document.getElementById("<%=txtcanceldays.ClientID %>");
        //        var redbuttonyes = document.getElementById("<%=OboutRadioButton1.ClientID %>").value;
        var redbuttonno = document.getElementById("<%=OboutRadioButton2.ClientID %>");


        function saveissue() {
            var redbuttonyes = document.getElementById("<%=OboutRadioButton1.ClientID %>").Cheked;
            var obj1 = new Object();
            obj1.companyid = companyid.value;
            obj1.deptname = deptname.value.toString();
            obj1.deptcode = deptcode.value.toString();
            obj1.autocancel = autocancel.options[autocancel.selectedIndex].text;
            obj1.approval = approval.value.toString();
            obj1.canceldays = canceldays.value.toString();
            if (redbuttonyes == "1") {
                obj1.active = "NO";
            }
            else {
                obj1.active = "Yes";
            }
            PageMethods.saveissuedata(obj1, Save_onsuccess);
        }

        function Save_onsuccess(result) {

            if (result == "Some error occurred" || result == "") { showAlert("Error occurred", "Error", '#'); }
            else {
                showAlert(result, "info", "");
                //clearfield();
                // Grid1.Refresh();
                window.location.reload();
            }
        }

    </script>
    <script type="text/javascript">
        function setvalue() {
            var valueauto = document.getElementById("<%= ddlautocancel.ClientID %>").value;
            if (valueauto == 3) {
                document.getElementById("<%= txtcanceldays.ClientID %>").value = "0";
                document.getElementById("<%= txtcanceldays.ClientID %>").disabled = true;
            }
            else {
                document.getElementById("<%= txtcanceldays.ClientID %>").value = "";
                document.getElementById("<%= txtcanceldays.ClientID %>").disabled = false;
            }
        }

    </script>
    <script type="text/javascript">
        function openProductSearch(sequence) {

            if (document.getElementById("<%= chkpricechange.ClientID %>").checked == true) {
                var deptid = document.getElementById("<%=hdnSelectedDepartment.ClientID %>").value;
                window.open('../Account/ApproverList.aspx?deptid=' + deptid + "", null, 'height=700px, width=825px,status= 0, resizable= 1, scrollbars=1, toolbar=0,location=center,menubar=0, screenX=300; screenY=50');
            }
            else {
                showAlert("Please Check on Price !", "error", "#");
            }
        }

        function RemoveFA() {
            if (document.getElementById("<%= chkpricechange.ClientID %>").checked == false) {
                if (confirm('Are you sure to remove Price & Financial Approver?')) {
                    var deptid = document.getElementById("<%=hdnSelectedDepartment.ClientID %>").value;
                    if (deptid != "") {
                        PageMethods.DeleteFA(deptid, null, null);
                        window.location.reload();

                    }
                }
                else {
                }
            }
        }

        function AfterProductSelected(Name, ApproverID) {
            var hdnval = document.getElementById("hdnapproverSearchSelectedRec");
            var FApproverID = document.getElementById("hdnapproversearchId");
            hdnval.value = Name;
            FApproverID.value = ApproverID;
            var TxtProduct = document.getElementById("<%=txtbomsku.ClientID %>");
            TxtProduct.value = Name;
        }

        function Openmethod(deptid) {
            var departmentid = deptid;
            window.open('../Account/PMethodList.aspx?deptid=' + deptid + "", null, 'height=400px, width=825px,status= 0, resizable= 1, scrollbars=1, toolbar=0,location=center,menubar=0, screenX=300; screenY=50');
        }

        function OpenApprover(sequence) {
            var deptid = null;
            window.open('../Account/CApprovalSearch.aspx?deptid=' + deptid + "", null, 'height=700px, width=825px,status= 0, resizable= 1, scrollbars=1, toolbar=0,location=center,menubar=0, screenX=300; screenY=50');
        }

        function AfterApproverSelect(Name, ApproverID) {
            var hdnval = document.getElementById("hdnCenterApproName");
            var FApproverID = document.getElementById("hdnCenterApproID");
            hdnval.value = Name;
            FApproverID.value = ApproverID;
            var TxtProduct = document.getElementById("<%=txtcostapprover.ClientID %>");
            TxtProduct.value = Name;
        }

        var CenterName = document.getElementById('<%=txtCenterName.ClientID %>');
        var Centercode = document.getElementById('<%=txtcode.ClientID %>');
        var approver = document.getElementById('<%=hdnCenterApproID.ClientID %>');
        var Remark = document.getElementById('<%=txtremark.ClientID %>');
        function SaveCostCenter() {
            if (CenterName.value != "" && Centercode.value != "" && approver.value != "") {
                var obj1 = new Object();
                obj1.CenterName = CenterName.value.toString();
                obj1.code = Centercode.value.toString();
                obj1.approver = approver.value.toString();
                obj1.Remark = Remark.value.toString();
                PageMethods.SaveCostCenterData(obj1, Save_onsuccess);
            }
            else {
                showAlert("Please Enter Required Field!", "error", "#");
            }

        }

        function Save_onsuccess(result) {

            if (result == "fail" || result == "") { showAlert("Duplicate Center Name or Code", "Error", '#'); }
            else {
                gvCostCenter.refresh();
                document.getElementById('<%=txtCenterName.ClientID %>').value = "";
                document.getElementById('<%=txtcode.ClientID %>').value = "";
                document.getElementById('<%=hdnCenterApproID.ClientID %>').value = "";
                document.getElementById('<%=txtremark.ClientID %>').value = "";
                document.getElementById('<%=txtcostapprover.ClientID %>').value = "";
            }
        }

        function RemoveMethod(Id) {
            var obj1 = new Object();
            var CenterID = Id;
            obj1.CenterId = CenterID;
            PageMethods.RemoveSku(obj1, Removesku_onSuccess);
        }

        function Removesku_onSuccess(result) {
            gvCostCenter.refresh();
        }

    </script>

    <script type="text/javascript">
           function SelectedPrdRec() {
            oboutGrid.prototype.restorePreviousSelectedRecord = function () {
                return;
            }

            oboutGrid.prototype.markRecordAsSelectedOld = oboutGrid.prototype.markRecordAsSelected;
            oboutGrid.prototype.markRecordAsSelected = function (row, param2, param3, param4, param5) {
                if (row.className != this.CSSRecordSelected) {
                    this.markRecordAsSelectedOld(row, param2, param3, param4, param5);

                } else {
                    var index = this.getRecordSelectionIndex(row);
                    if (index != -1) {
                        this.markRecordAsUnSelected(row, index);
                    }
                }


                var hdnPartSelectedRec = document.getElementById("<%=hdnPartSelectedRec.ClientID%>");
                hdnPartSelectedRec.value = "";

                for (var i = 0; i < Grid1.PageSelectedRecords.length; i++) {
                    var record = Grid1.PageSelectedRecords[i];
                    if (hdnPartSelectedRec.value == "") hdnPartSelectedRec.value = record.ID;
                }
            }

        }

    </script>

    <script type="text/javascript">
        function CheckValue() {
            var box1 = parseInt(document.getElementById("<%=txtcanceldays.ClientID %>").value);
            var box2 = parseInt(document.getElementById("<%=txtremschedule.ClientID %>").value);

            if (box2 > box1) {

                showAlert("Approval Reminder should not more than autocancellation days.!", "error", "#");
                return false;
            }
            return true;
        }
    </script>

      <script type="text/javascript">
          function GetSubscription() {
              var ddlsubscription = document.getElementById("<%=ddlsubscription.ClientID %>");
            document.getElementById("<%=hdnsubscription.ClientID %>").value = ddlsubscription.value;
            var hdnSelectedCompany = document.getElementById("<%=hdnsubscription.ClientID %>");
            //alert(hdnSelectedCompany.value);
        }


    </script>

  </asp:Content>
<asp:Content ID="Content4" runat="server" contentplaceholderid="head">
    <style type="text/css">
        .auto-style1 {
            height: 30px;
        }
      .test tr input
{       border:1px solid red;
        margin-right:5px;
        padding-right:5px;
}
       
    </style>
</asp:Content>

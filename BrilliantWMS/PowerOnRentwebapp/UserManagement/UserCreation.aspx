<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/CRM.Master" AutoEventWireup="true"
    EnableEventValidation="false" CodeBehind="UserCreation.aspx.cs" Inherits="BrilliantWMS.UserManagement.UserCreation" %>

<%@ Register Assembly="obout_Grid_NET" Namespace="Obout.Grid" TagPrefix="obout" %>
<%@ Register Assembly="obout_Interface" Namespace="Obout.Interface" TagPrefix="obout" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../Territory/UC_Territory.ascx" TagName="UC_Territory" TagPrefix="uc1" %>
<%@ Register Src="../Address/UCAddress.ascx" TagName="UCAddress" TagPrefix="uc7" %>
<%@ Register Src="../CommonControls/UC_Date.ascx" TagName="UC_Date" TagPrefix="uc2" %>
<%@ Register Src="../CommonControls/UCFormHeader.ascx" TagName="UCFormHeader" TagPrefix="uc4" %>
<%@ Register Src="../CommonControls/UCToolbar.ascx" TagName="UCToolbar" TagPrefix="uc5" %>
<%--<%@ Register TagPrefix="asp" Namespace="Saplin.Controls" Assembly="DropDownCheckBoxes" %>--%>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHolder_FormHeader" runat="server">
    <uc5:UCToolbar ID="UCToolbar1" runat="server" />
    <uc4:UCFormHeader ID="UCFormHeader1" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPHolder_Form" runat="server">
    <asp:ValidationSummary ID="validationsummary_UserCreation" runat="server" ShowMessageBox="true"
        ShowSummary="false" DisplayMode="bulletlist" ValidationGroup="Save" />
    <center>
        <asp:TabContainer ID="TabContainerUserCreation1" runat="server" ActiveTabIndex="2">
            <asp:TabPanel ID="TabPanelUsersList" runat="server" HeaderText="Users List ">
                <ContentTemplate>
                    <%--<asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="Uppnl_productDetails">
                        <ProgressTemplate>
                            <center>
                                <div class="modal">
                                    <img src="../App_Themes/Blue/img/ajax-loader.gif" style="top: 50%;" />
                                </div>
                            </center>
                        </ProgressTemplate>
                    </asp:UpdateProgress>--%>
                    <center>
                        <table class="gridFrame" style="width: 100%">
                            <tr>
                                <td>
                                    <table style="width: 100%">
                                        <tr>
                                            <td style="text-align: left;">
                                                <asp:Label ID="lblheader" CssClass="headerText" runat="server" Text="User List"></asp:Label>
                                            </td>
                                            <td align="right">
                                                <input type="button" id="btnAllocate" runat="server" value="Lock/Unlock" style="cursor: pointer;
                                                    padding: 4px 14px;" onclick="lockunlock();return false;" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <obout:Grid ID="gvUserCreationM" runat="server" AllowAddingRecords="False" AllowFiltering="True"  
                                        Width="100%" AllowGrouping="True" AllowMultiRecordSelection="false" AutoGenerateColumns="False"
                                        OnSelect="gvUserCreationM_Select" OnRebind="gvUserCreationM_RebindGrid"> 
                                        <ScrollingSettings ScrollHeight="250" />
                                        <Columns>
                                            <obout:Column ID="Edit" DataField="userID" HeaderText="Edit" Width="10%" TemplateId="GvTempEdit" Index="0">
                                            <TemplateSettings TemplateId="GvTempEdit" />
                                            </obout:Column>
                                            <obout:Column DataField="EmployeeID" HeaderText="Employee ID" Width="15%" Index="1">
                                            </obout:Column>
                                            <obout:Column DataField="userName" HeaderText="Name" Width="30%" Index="2">
                                            </obout:Column>
                                            <obout:Column DataField="CompanyName" HeaderText="Company" Width="20%" Index="3">
                                            </obout:Column>
                                            <obout:Column DataField="UserType" HeaderText="User Type" Width="20%" Index="4">
                                            </obout:Column>
                                            <obout:Column DataField="EmailID" HeaderText="Email ID" Width="30%" Index="5">
                                            </obout:Column>
                                            <obout:Column DataField="MobileNo" HeaderText="Mobile No." Width="15%" Index="6">
                                            </obout:Column>
                                            <obout:Column DataField="userIDPass" HeaderText="User ID" Width="19%" Index="7">
                                            </obout:Column>
                                            <obout:Column DataField="locked" HeaderText="Lock" Width="8%" Index="8">
                                            </obout:Column>
                                            <obout:Column DataField="Active" HeaderText="Active" Width="8%" Index="9">
                                            </obout:Column>
                                            <obout:Column DataField="DepartmentID" Visible="false">
                                            </obout:Column>
                                        </Columns>
                                        <Templates>
                                            <obout:GridTemplate ID="GvTempEdit" runat="server">
                                                <Template>
                                                    <asp:ImageButton ID="imgBtnEdit" CausesValidation="false" runat="server" ImageUrl="../App_Themes/Blue/img/Edit16.png" />
                                                   <%-- <asp:ImageButton ID="imgBtnEdit1" runat="server" ImageUrl="../App_Themes/Blue/img/Edit16.png"
                                                                    OnClick="imgBtnEdit_OnClick" ToolTip='<%# (Container.Value) %>' CausesValidation="false" />--%>
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
            <asp:TabPanel ID="TabPanelUserInformation" runat="server" HeaderText="User Information">
                <ContentTemplate>
                    <center>
                        <table class="tableForm" border="0">
                            <tr>
                                <td>
                                    <req><asp:Label ID="lblfname" runat="server" Text="First Name"/></req>
                                    :
                                </td>
                                <td style="text-align: left;">
                                    <asp:TextBox ID="txtFirstName" runat="server" ValidationGroup="Save" MaxLength="50"
                                        Width="180px" ControlsToEnable="" FolderStyle="" WatermarkText=""></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="valRftxtFirstName" runat="server" ControlToValidate="txtFirstName"
                                        ErrorMessage="Enter First Name on User Information Tab" ValidationGroup="Save"
                                        Display="None"></asp:RequiredFieldValidator>
                                </td>
                                <td>
                                    <asp:Label ID="lblmname" runat="server" Text="Middle Name" />
                                    :
                                </td>
                                <td>
                                    <asp:TextBox ID="txtMiddleName" MaxLength="50" runat="server" Width="180px"></asp:TextBox>
                                </td>
                                <td>
                                    <req><asp:Label ID="lbllname"  runat="server" Text="Last Name"/></req>
                                    :
                                </td>
                                <td>
                                    <asp:TextBox ID="txtLastName" runat="server" MaxLength="50" ValidationGroup="Save"
                                        Width="180px"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="valRftxtLastName" runat="server" ControlToValidate="txtLastName"
                                        ErrorMessage="Enter Last Name" ValidationGroup="Save" Display="None"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <%--<req>--%> <asp:Label ID="lblgender"  runat="server" Text="Gender"/><%--</req>--%>
                                    :
                                </td>
                                <td style="text-align: left;">
                                    <asp:DropDownList ID="ddlUserGender" runat="server" Width="100px">
                                        <asp:ListItem Text="-Select-" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="Male" Value="M"></asp:ListItem>
                                        <asp:ListItem Text="Female" Value="F"></asp:ListItem>
                                    </asp:DropDownList>
                                   <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Select User Gender"
                                        ControlToValidate="ddlUserGender" InitialValue="0" ValidationGroup="Save" Display="None"></asp:RequiredFieldValidator>--%>
                                </td>
                                <td>
                                    <asp:Label ID="lbldob" runat="server" Text="Date Of Birth" />
                                    :
                                </td>
                                <td style="text-align: left;">
                                    <uc2:UC_Date ID="UC_DateofBirth" runat="server" />
                                </td>
                                <td>
                                    <req><asp:Label ID="lblemail"  runat="server" Text="Email ID"/></req>
                                    :
                                </td>
                                <td>
                                    <asp:TextBox ID="txtEmail" runat="server" ValidationGroup="Save" Width="180px"></asp:TextBox>
                                    <asp:RegularExpressionValidator ID="valRegExptxtEmail" runat="server" ControlToValidate="txtEmail"
                                        ValidationGroup="Save" ErrorMessage="Enter Valid Email Address" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                        Display="None"></asp:RegularExpressionValidator>
                                    <asp:RequiredFieldValidator ID="rfv_txtEmail" runat="server" ErrorMessage="Enter Email ID"
                                        ControlToValidate="txtEmail" ValidationGroup="Save" Display="None"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblmobno" runat="server" Text="Mobile No." />
                                    :
                                </td>
                                <td style="text-align: left;">
                                    <asp:TextBox ID="txtMobile" MaxLength="10" onkeydown="return AllowPhoneNo(this, event);"
                                        runat="server" Width="180px"></asp:TextBox>
                                </td>
                                <td>
                                    <req><asp:Label ID="lblemployeeno" runat="server" Text="Employee No."></asp:Label></req>
                                    :
                                </td>
                                <td>
                                    <asp:TextBox ID="txtEmpNo" runat="server" MaxLength="50" Width="180px"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="valRftxtEmpNo" runat="server" ControlToValidate="txtEmpNo"
                                        ErrorMessage="Enter Employee Number." ValidationGroup="Save" Display="None"></asp:RequiredFieldValidator>
                                </td>
                                <td rowspan="5">
                                </td>
                                <td rowspan="5" style="text-align: center;">
                                    <div id="profile">
                                        <img runat="server" id="Img1" width="110" height="132" src="~/App_Themes/Blue/img/Male.png" />
                                    </div>
                                </td>
                            </tr>
                            <tr>    
                                 <td>
                                    <req> <asp:Label ID="lblCompanyMain" runat="server" Text="Company" />  <req>
                                    :
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlCompanymain" runat="server" ValidationGroup="Save" Width="187px"
                                        DataTextField="Name" DataValueField="ID" >  
                                        <asp:ListItem Text="-Select-" Value="0"></asp:ListItem>                                     
                                    </asp:DropDownList> <%--BindDepartment(this)onchange="GetCompany(this); GetDepartmentnew()"--%>
                                    <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Select Company"
                                        ControlToValidate="ddlCompanymain" InitialValue="0" ValidationGroup="Save" Display="None"></asp:RequiredFieldValidator>--%>
                                </td>                       
                                <td>
                                    <req> <asp:Label ID="lblcompany" runat="server" Text="Company" />  <req>
                                    :
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlcompany" runat="server" ValidationGroup="Save" Width="187px"
                                        DataTextField="Name" DataValueField="ID" onchange="GetCompany(this); GetDepartmentnew()">                                       
                                    </asp:DropDownList> <%--BindDepartment(this)--%>
                                    <asp:RequiredFieldValidator ID="RFVddlcompany" runat="server" ErrorMessage="Select Customer"
                                        ControlToValidate="ddlcompany" InitialValue="0" ValidationGroup="Save" Display="None"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <req><asp:Label ID="lbldept"  runat="server" Text="Department"/></req>
                                    :
                                </td>
                                <td>
                                    <%--  <asp:DropDownList ID="ddlDepartment" runat="server" ValidationGroup="Save" DataTextField="Territory"
                                        DataValueField="ID" Width="187px" onchange="fillDesignation(this);GetDepartment(this);BindDesignation(this)"
                                        ClientIDMode="Static">
                                    </asp:DropDownList>--%>
                                    <asp:DropDownList ID="ddlDepartment" runat="server" ValidationGroup="Save" DataTextField="Territory"
                                        DataValueField="ID" Width="187px" onchange="GetDepartment(this);"
                                        ClientIDMode="Static">
                                    </asp:DropDownList>
                                    <a id="btnAssignDepartment" onclick="AssignLocation();" style="margin-top: -20px;"><asp:Label ID="lblassigndept" Visible="false"  runat="server" Text="Assign Department"/></a>
                                    <%--<asp:RequiredFieldValidator ID="RFVddldept" runat="server" ErrorMessage="Select Department"
                                        ControlToValidate="ddlDepartment" InitialValue="0" ValidationGroup="Save" Display="None"></asp:RequiredFieldValidator>--%>
                                </td>
                                <td>
                                    <req> <asp:Label ID="lblrole"  runat="server" Text="Role"/></req>
                                    :
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlRole" runat="server" ValidationGroup="Save" Width="187px"
                                        DataTextField="RoleName" DataValueField="Id" onchange="GetRoll(this)">
                                    </asp:DropDownList>
                                    <%--             <asp:RequiredFieldValidator ID="RFVddlRole" runat="server" ErrorMessage="Select Role"
                                        ControlToValidate="ddlRole" InitialValue="0" ValidationGroup="Save" Display="None"></asp:RequiredFieldValidator>--%>
                                    <asp:DropDownList ID="ddlUserType" runat="server" ValidationGroup="Save" Width="187px"
                                        Visible="false">
                                        <asp:ListItem Text="-Select-" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="Supar Admin" Value="Supar Admin"></asp:ListItem>
                                        <asp:ListItem Text="Admin" Value="Admin"></asp:ListItem>
                                        <asp:ListItem Text="User" Value="User"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr style="display:none;">
                                <td colspan="3" style="text-align: left; border: 1px solid Silver;">
                                    <div id="divSelectedLocation" runat="server" clientidmode="Static" style="width: 415px;
                                        float: right;">
                                    </div>
                                    <asp:HiddenField ID="hdnSelectedLocation" runat="server" ClientIDMode="Static" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblreporting" runat="server" Text="Reporting To" />
                                    :
                                </td>
                                <td style="text-align: left;">
                                    <asp:DropDownList ID="ddlReportingTo" runat="server" ValidationGroup="Save" Width="187px"
                                        DataTextField="Name" DataValueField="ID">
                                        <asp:ListItem Text="-Select-" Value="0"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="lblmobileinter" runat="server" Text="Mobile Interface Approval" />
                                    :
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlmobile" runat="server" ValidationGroup="Save" Width="187px"
                                        DataTextField="Name" DataValueField="ID">
                                        <asp:ListItem Text="-Select-" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="-Yes-" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="-No-" Value="2"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <req><asp:Label ID="lblactive"  runat="server" Text="Active"/></req>
                                    :
                                </td>
                                <td style="text-align: left;">
                                    <obout:OboutRadioButton ID="rbtnYes" runat="server" Text="Yes" GroupName="rbtnActive"
                                        Checked="True" FolderStyle="">
                                    </obout:OboutRadioButton>
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <obout:OboutRadioButton ID="rbtnNo" runat="server" Text="No" GroupName="rbtnActive"
                                        FolderStyle="">
                                    </obout:OboutRadioButton>
                                </td>
                                  <td>
                                    <asp:Label ID="lbldoj" runat="server" Text="Date Of Joining" />
                                    :
                                </td>
                                <td style="text-align: left;">
                                    <uc2:UC_Date ID="UC_Dateofjoining" runat="server" />
                                </td>

                            </tr>
                            <tr>
                                <%--<td>
                                checkbox dept :
                                </td>
                                <td>
                                      <asp:DropDownCheckBoxes ID="yearsDropDownCheckBoxes" runat="server" Width="187px" DataTextField="Territory" AddJQueryReference="true"
                                        DataValueField="ID">
                                         <Style SelectBoxWidth="300" />
                                      </asp:DropDownCheckBoxes>
                                </td>--%>
                                <td>
                                    <req><asp:Label ID="lbldesig"  runat="server" Text="Designation" visible="False"/></req>
                                </td>
                                <td>
                                    <%--   <asp:DropDownList ID="DropDownList1" runat="server" ValidationGroup="Save" Width="187px"
                                        ClientIDMode="Static" onchange="fillRoleDDL(this);GetDesignation(this)" DataTextField="Name"
                                        DataValueField="ID">
                                    </asp:DropDownList>--%>
                                    <asp:DropDownList ID="ddlDesignation" runat="server" ValidationGroup="Save" Width="187px"
                                        ClientIDMode="Static" onchange="GetDesignation(this)" DataTextField="Name" DataValueField="ID"
                                        Visible="false">
                                    </asp:DropDownList>
                                    <%--<asp:RequiredFieldValidator ID="valRfddlDesignation" runat="server" ErrorMessage="Select Designation"
                                        ControlToValidate="ddlDesignation" InitialValue="0" ForeColor="Red" ValidationGroup="Save"
                                        Display="None"></asp:RequiredFieldValidator>--%>
                                </td>
                                <td colspan="4">
                                    <asp:FileUpload ID="FileUploadProfileImg" runat="server" ClientIDMode="Static" onchange="fileUploaderValidationImgOnly(this,20480)" />
                                    <asp:LinkButton runat="server" ID="lnkUpdateProfileImg" Text="Upload"  OnClick="lnkUpdateProfileImg_OnClick" 
                                        CausesValidation="false" Visible="false"></asp:LinkButton>  
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
                                </td>
                                <td style="text-align: right;" colspan="2">
                                    <span class="watermark">.jpg|.jpeg|.gif|.png|.bmp files are allowed </span>
                                    <br />
                                    <span class="watermark">Max Limit 20 KB </span>
                                </td>
                            </tr>
                            <%--<tr>
                                <td>
                                    Assigned Location :
                                </td>
                                <td colspan="5" style="text-align: left;">
                                    <asp:Label ID="lblAsgLocation" runat="server" ClientIDMode="Static"></asp:Label>
                                    <asp:LinkButton ID="lnkbtnSiteLocation" runat="server" ClientIDMode="Static" Text="Change"
                                        OnClientClick="SetActiveTab();"></asp:LinkButton>
                                </td>
                            </tr>--%>
                            <tr>
                                <td colspan="6" style="text-align: left;">
                                    <hr />
                                    <asp:Label ID="lblloginheader" Style="font-weight: bold;" runat="server" Text="OMS Login Details" />
                                    :
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: left;">
                                    <req><asp:Label ID="lblusername" style="font-weight: bold;"  runat="server" Text="User Name"/></req>
                                    :
                                </td>
                                <td>
                                    <asp:TextBox ID="txtLoginId1" runat="server" MaxLength="50" Width="180px" ValidationGroup="Save"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="req_txtLoginId" runat="server" ErrorMessage="Enter User Name"
                                        ControlToValidate="txtLoginId1" ForeColor="Red" ValidationGroup="Save" Display="None"></asp:RequiredFieldValidator>
                                </td>
                                <td>
                                    <asp:Label ID="lblPassword" runat="server" Text="Password :" ForeColor="Maroon"></asp:Label>
                                    :
                                </td>
                                <td>
                                    <asp:TextBox ID="txtPassword1" runat="server"  Width="180px" ClientIDMode="Static"
                                        ValidationGroup="Save" onkeyup="divPasswordwstrength();"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="req_txtPassword" runat="server" ErrorMessage="Enter password"
                                        ControlToValidate="txtPassword1" ForeColor="Red" ValidationGroup="Save" Display="None"></asp:RequiredFieldValidator>       <%--TextMode="Password"--%>
                                </td>
                                <td>
                                    <asp:Label ID="lblConfirmPass" runat="server" Text="Confirm Password :" ForeColor="Maroon"></asp:Label>
                                    :   
                                </td>
                                <td>
                                    <asp:TextBox ID="txtConfirmPassword" runat="server"  onblur="Javascript:CheckPassword();"       
                                        Width="180px" ValidationGroup="Save" onkey="compareCPw()"></asp:TextBox>                           <%--TextMode="Password"--%>
                                    <asp:RequiredFieldValidator ID="rfValtxtConfirmPassword" runat="server" ErrorMessage="Enter Confirm Password"
                                        ControlToValidate="txtConfirmPassword" ForeColor="Red" ValidationGroup="Save"
                                        Display="None"></asp:RequiredFieldValidator>
                                    <asp:CompareValidator ID="cmpValtxtPassword" runat="server" ControlToValidate="txtConfirmPassword"
                                        ControlToCompare="txtPassword1" ErrorMessage="Confirm Password does not match to Password!"
                                        Display="None" Type="String" ValidationGroup="Save"></asp:CompareValidator>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                </td>
                                <td style="text-align: right;">
                                    <div id="divPasswordwstrength">
                                    </div>
                                    <span id="PasswordwstrengthMsg" style="font-size: 11px; margin-top: -11px; margin-right: 8px;
                                        float: right;"></span>
                                </td>
                                <td>
                                </td>
                                <td>
                                </td>
                            </tr>
                        </table>
                    </center>
                </ContentTemplate>
            </asp:TabPanel>
            <asp:TabPanel ID="TabPanelAddressInfo" runat="server" Visible="false">
                <HeaderTemplate>
                    <asp:Label ID="lbladdress" runat="server" Text="Address Info" />
                </HeaderTemplate>
                <ContentTemplate>
                    <uc7:UCAddress ID="UCAddress1" runat="server" />
                </ContentTemplate>
            </asp:TabPanel>
            <asp:TabPanel ID="tabProductDetails" runat="server" HeaderText="Access Delegation">
                <ContentTemplate>
                    <asp:UpdateProgress ID="UpdateProgress_ProductDetails" runat="server" AssociatedUpdatePanelID="Uppnl_productDetails">
                        <ProgressTemplate>
                            <center>
                                <div class="modal">
                                    <img src="../App_Themes/Blue/img/ajax-loader.gif" style="top: 50%;" />
                                </div>
                            </center>
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                    <asp:UpdatePanel ID="Uppnl_productDetails" runat="server">
                        <ContentTemplate>
                            <center>
                                <table class="tableForm" border="0">
                                    <tr>
                                        <td>
                                            <req><asp:Label ID="lblfrmdate"  runat="server" Text="From Date"/></req>
                                            :
                                        </td>
                                        <td style="text-align: left;">
                                            <uc2:UC_Date ID="UC_Date1" runat="server" />
                                        </td>
                                        <td>
                                            <req> <asp:Label ID="lbltodate"  runat="server" Text="To Date"/></req>
                                            :
                                        </td>
                                        <td style="text-align: left;">
                                            <uc2:UC_Date ID="UC_Date2" runat="server" />
                                        </td>
                                        <td>
                                            <req><asp:Label ID="lblrightsto"  runat="server" Text="Delegate Approval Rights to"/></req>
                                            :
                                        </td>
                                        <td style="text-align: left;">
                                            <asp:DropDownList ID="ddlUOM" runat="server" DataTextField="Name" DataValueField="ID"
                                                Width="200px">
                                                <asp:ListItem Text="-Select-" Value="0"></asp:ListItem>
                                            </asp:DropDownList>
                                            <%--<asp:RequiredFieldValidator ID="req_ddlUOM" runat="server" ErrorMessage="Please Select Delegate"
                                                ControlToValidate="ddlUOM" ValidationGroup="Save" Display="None" InitialValue="0"></asp:RequiredFieldValidator>--%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblremark" runat="server" Text="Remark" />
                                            :
                                        </td>
                                        <td colspan="5" style="text-align: left;">
                                            <asp:TextBox ID="txtPrincipalPrice" runat="server" TextMode="MultiLine" MaxLength="1000"
                                                Width="554px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="6" style="text-align: right;">
                                            <%--<asp:Button ID="btnsubmit" runat="server" Text="Submit" OnClientClick="SaveAccessDelegation()"  />--%>
                                            <input type="button" runat="server" id="btnsumit" value="Submit" onclick="SaveAccessDelegation()" />
                                        </td>
                                    </tr>
                                </table>
                                <table class="gridFrame" width="100%">
                                    <tr>
                                        <td>
                                            <table style="width: 100%">
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblaccdele" CssClass="headerText" runat="server" Text="Access Delegation" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <obout:Grid ID="grdaccessdele" runat="server" AllowFiltering="true" AllowGrouping="true"
                                                AllowSorting="true" AllowColumnResizing="true" AutoGenerateColumns="false" Width="100%"
                                                AllowAddingRecords="true" OnRebind="grdaccessdele_RebindGrid">
                                                 <ScrollingSettings ScrollHeight="250" />
                                                <Columns>
                                                    <obout:Column DataField="ID" HeaderText="Edit" Width="5%" AllowSorting="false" Align="center"
                                                        HeaderAlign="center">
                                                        <TemplateSettings TemplateId="GvEditBOM" />
                                                    </obout:Column>
                                                    <obout:Column ID="Column1" HeaderText="From Date" DataField="FromDate" Width="15%"
                                                        runat="server" DataFormatString="{0:d}">
                                                    </obout:Column>
                                                    <obout:Column ID="Column2" HeaderText="To Date" DataField="todate" Width="15%" runat="server"
                                                        DataFormatString="{0:d}">
                                                    </obout:Column>
                                                    <obout:Column ID="Column3" HeaderText="Approve Rights to" DataField="Name" Width="15%"
                                                        runat="server">
                                                    </obout:Column>
                                                    <obout:Column ID="Column4" HeaderText="Remark" DataField="remark" Width="20%" runat="server">
                                                    </obout:Column>
                                                </Columns>
                                                <Templates>
                                                    <obout:GridTemplate ID="GvEditBOM" runat="server" ControlID="" ControlPropertyName="">
                                                        <Template>
                                                            <asp:ImageButton ID="imgBtnEdit1bom" runat="server" ImageUrl="../App_Themes/Blue/img/Edit16.png"
                                                                OnClick="imgBtnEditbom_OnClick" ToolTip='<%# (Container.Value) %>' CausesValidation="false" />
                                                        </Template>
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
            <asp:TabPanel ID="TabPanelRoleConfiguration" runat="server">
                <HeaderTemplate>
                    Role Configuration
                </HeaderTemplate>
                <ContentTemplate>
                    <center>
                        <table class="tableForm">
                            <tr>
                                <td>
                                    <uc1:UC_Territory ID="UC_Territory1" runat="server" />
                                    <a id="btnAssignLocation" onclick="AssignLocation();" style="margin-top: -20px;">Assign
                                        Location</a>
                                </td>
                            </tr>
                            <tr>
                            </tr>
                            <tr>
                                <td>
                                    <hr />
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: left;">
                                    <req> Select Role :</req>
                                    <asp:DropDownList ID="ddlRoleList" runat="server" ValidationGroup="Save" Width="187px"
                                        DataTextField="RoleName" DataValueField="mrID" onchange="RefreshGVRoleConfig(this);"
                                        ClientIDMode="Static">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RFVddlRoleList" runat="server" ErrorMessage="Select Role"
                                        ControlToValidate="ddlRoleList" InitialValue="0" Display="None" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                        </table>
                        <table class="gridFrame" id="tblGridRole">
                            <tr>
                                <td>
                                    <a class="headerText">Role Configuration</a>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <obout:Grid ID="GridRoleConfiguration" runat="server" AutoGenerateColumns="false"
                                        AllowAddingRecords="false" AllowColumnReordering="true" AllowColumnResizing="false"
                                        AllowGrouping="true" AllowSorting="true" AllowFiltering="false" AllowManualPaging="false"
                                        AllowMultiRecordAdding="false" AllowPageSizeSelection="false" AllowPaging="false"
                                        AllowRecordSelection="false" ShowCollapsedGroups="false" ShowGroupFooter="false"
                                        ShowFooter="false" ShowTotalNumberOfPages="false" ShowGroupsInfo="false" PageSize="-1"
                                        HideColumnsWhenGrouping="true" Serialize="true" GroupBy="DisplayModuleName,DisplayPhaseName"
                                        Width="800px" OnRebind="GridRoleConfiguration_OnRebind">
                                        <TemplateSettings GroupHeaderTemplateId="GroupTemplate" />
                                        <Columns>
                                            <obout:Column DataField="DisplayModuleName" HeaderText="Module" AllowSorting="false"
                                                AllowGroupBy="true" Width="10%">
                                            </obout:Column>
                                            <obout:Column DataField="DisplayPhaseName" HeaderText="Phase" AllowSorting="false"
                                                AllowGroupBy="true" Width="10%">
                                            </obout:Column>
                                            <obout:Column DataField="ObjectDisplayName" Width="40%" HeaderText="Objects" AllowSorting="false"
                                                AllowGroupBy="false">
                                            </obout:Column>
                                            <obout:Column DataField="Add" Width="10%" Align="center" HeaderAlign="center" AllowSorting="false"
                                                Index="4" AllowGroupBy="false" HeaderText="Add/Edit">
                                                <TemplateSettings TemplateId="checkboxAccessLevel" />
                                            </obout:Column>
                                            <obout:Column DataField="View" Width="10%" Align="center" HeaderAlign="center" AllowSorting="false"
                                                AllowGroupBy="false" Visible="false">
                                                <TemplateSettings TemplateId="checkboxAccessLevel" />
                                            </obout:Column>
                                            <obout:Column DataField="Approval" Width="10%" Align="center" HeaderAlign="center"
                                                AllowSorting="false" AllowGroupBy="false">
                                                <TemplateSettings TemplateId="TemplateApproval" />
                                            </obout:Column>
                                            <obout:Column DataField="AssignTask" Width="10%" Align="center" HeaderAlign="center"
                                                AllowSorting="false" AllowGroupBy="false">
                                                <TemplateSettings TemplateId="TemplateAssignTask" HeaderTemplateId="HeaderTempAssignTask" />
                                            </obout:Column>
                                            <obout:Column DataField="mSequence" Visible="false" Width="0%">
                                            </obout:Column>
                                            <obout:Column DataField="pSequence" Visible="false" Width="0%">
                                            </obout:Column>
                                            <obout:Column DataField="oSequence" Visible="false" Width="0%">
                                            </obout:Column>
                                            <obout:Column DataField="ApprovalVisible" Visible="false" Width="0%">
                                            </obout:Column>
                                            <obout:Column DataField="AssignTaskVisible" Visible="false" Width="0%">
                                            </obout:Column>
                                        </Columns>
                                        <Templates>
                                            <obout:GridTemplate ID="HeaderTempAssignTask" runat="server">
                                                <Template>
                                                    Assign
                                                    <br />
                                                    Task
                                                </Template>
                                            </obout:GridTemplate>
                                            <obout:GridTemplate ID="checkboxAccessLevel" runat="server">
                                                <Template>
                                                    <input type="checkbox" style="cursor: pointer;" onclick="saveCheckBoxChangesRoleMaster(this, '<%# GridRoleConfiguration.Columns[Container.ColumnIndex].DataField %>', <%# Container.PageRecordIndex %>)"
                                                        <%# Container.Value.ToString().ToLower()=="true" ? "checked='checked'" : "" %> />
                                                </Template>
                                            </obout:GridTemplate>
                                            <obout:GridTemplate ID="TemplateApproval" runat="server">
                                                <Template>
                                                    <input type="checkbox" onclick="saveCheckBoxChangesRoleMaster(this, 'Approval', <%# Container.PageRecordIndex %>)"
                                                        <%# Container.Value.ToString().ToLower()=="true" ? "checked='checked'" : "" %>
                                                        <%# Container.DataItem["ApprovalVisible"].ToString().ToLower()=="true" ? "style='visibility: visible; cursor:pointer;'" : "style='visibility: hidden; cursor:pointer;'" %> />
                                                    <a style="margin-left: -8px;" title="Not Applicable">
                                                        <%# Container.DataItem["ApprovalVisible"].ToString().ToLower()=="false" ? "N/A" : "" %></a>
                                                </Template>
                                            </obout:GridTemplate>
                                            <obout:GridTemplate ID="TemplateAssignTask" runat="server">
                                                <Template>
                                                    <input type="checkbox" onclick="saveCheckBoxChangesRoleMaster(this, 'AssignTask', <%# Container.PageRecordIndex %>)"
                                                        <%# Container.Value.ToString().ToLower()=="true" ? "checked='checked'" : "" %>
                                                        <%# Container.DataItem["AssignTaskVisible"].ToString().ToLower()=="true" ? "style='visibility: visible; cursor:pointer;'" : "style='visibility: hidden; cursor:pointer;'" %> />
                                                    <a style="margin-left: -8px;" title="Not Applicable">
                                                        <%# Container.DataItem["AssignTaskVisible"].ToString().ToLower() == "false" ? "N/A" : ""%>
                                                    </a>
                                                </Template>
                                            </obout:GridTemplate>
                                            <obout:GridTemplate runat="server" ID="GroupTemplate">
                                                <Template>
                                                    <h7><%# Container.Value %></h7>
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
            <asp:TabPanel ID="pnllocation" runat="server" HeaderText="Add Retail Location" Visible="false">
                    <ContentTemplate>
                        <center>
                            <table class="gridFrame" width="100%">
                                <tr>
                                    <td>
                                        <table style="width: 100%">
                                            <tr>
                                                <td style="text-align: left;">
                                                    <%-- <a style="color: white; font-size: 15px; font-weight: bold;">Additional Distribution</a>--%>
                                                    <asp:Label ID="lbldisrtibution" Style="color: white; font-size: 15px; font-weight: bold;" runat="server" Text="Location list"></asp:Label>
                                                </td>
                                                <td style="text-align: right;">
                                                    <input type="button" id="btnContactPerson" runat="server" value="Add Location" onclick="openProductSearch('0')" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <obout:Grid ID="Grid1" runat="server" AllowAddingRecords="false" AllowRecordSelection="false" Width="100%"
                                            AutoGenerateColumns="false" AllowPaging="true" AllowFiltering="true" AllowGrouping="true"
                                            AllowSorting="true">
                                            <ScrollingSettings ScrollHeight="250" />
                                            <Columns>
                                                <obout:Column DataField="Sequence" HeaderText="Remove" Width="5%" AllowSorting="false"
                                                    Align="Center" HeaderAlign="Center">
                                                    <TemplateSettings TemplateId="GvRemoveSku" />
                                                </obout:Column>
                                                <obout:Column HeaderText="Location Code" DataField="LocationCode" Width="150px">
                                                </obout:Column>
                                                <obout:Column HeaderText="Address" DataField="AddressLine1" Width="150px">
                                                </obout:Column>
                                                <obout:Column HeaderText="City" DataField="City" Width="150px">
                                                </obout:Column>
                                                <obout:Column HeaderText="State" DataField="State" Width="150px">
                                                </obout:Column>
                                                <obout:Column HeaderText="ContactName" DataField="ContactName" Width="150px">
                                                </obout:Column>
                                            </Columns>
                                            <Templates>
                                                <obout:GridTemplate ID="GvRemoveSku" runat="server">
                                                    <Template>
                                                        <img id="imgbuttonremove" src="../App_Themes/Grid/img/Remove16x16.png" alt="Remove" title="Remove" onclick="RemoveSkuRecord('<%# (Container.DataItem["Id"].ToString()) %>');"
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
            <asp:TabPanel ID="TabPanelWarehouse" runat="server" HeaderText="Add Warehouse">
                    <ContentTemplate>
                        <center>
                            <table class="gridFrame" width="100%">
                                <tr>
                                    <td>
                                        <table style="width: 100%">
                                            <tr>
                                                <td style="text-align: left;">
                                                    <%-- <a style="color: white; font-size: 15px; font-weight: bold;">Additional Distribution</a>--%>
                                                    <asp:Label ID="lblwarehouselist" Style="color: white; font-size: 15px; font-weight: bold;" runat="server" Text="Warehouse list"></asp:Label>
                                                </td>
                                                <td style="text-align: right;">
                                                    <input type="button" id="btnaddwarehouse" runat="server" value="Add Warehouse" onclick="openWarehouseSearch('0')" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <obout:Grid ID="grdwarehouse" runat="server" AllowAddingRecords="false" AllowRecordSelection="false" Width="100%"
                                            AutoGenerateColumns="false" AllowPaging="true" AllowFiltering="true" AllowGrouping="true"
                                            AllowSorting="true">
                                            <ScrollingSettings ScrollHeight="250" />
                                            <Columns>
                                               <obout:Column DataField="Sequence" HeaderText="Remove" Width="5%" AllowSorting="false"
                                                    Align="Center" HeaderAlign="Center">
                                                    <TemplateSettings TemplateId="GvRemoveWarehouse" />
                                                </obout:Column>
                                                <obout:Column HeaderText="Warehouse Code" DataField="Code" Width="150px">
                                                </obout:Column>
                                                <obout:Column HeaderText="WarehouseName" DataField="WarehouseName" Width="150px">
                                                </obout:Column>
                                                <obout:Column HeaderText="Type" DataField="Type" Width="150px">
                                                </obout:Column>
                                                <obout:Column HeaderText="Active" DataField="Active" Width="150px">
                                                </obout:Column>
                                                <obout:Column HeaderText="Description" DataField="Description" Width="150px">
                                                </obout:Column>
                                            </Columns>
                                            <Templates>
                                                <obout:GridTemplate ID="GvRemoveWarehouse" runat="server">
                                                    <Template>
                                                        <img id="imgbuttonremove" src="../App_Themes/Grid/img/Remove16x16.png" alt="Remove" title="Remove" 
                                                            style="cursor: pointer;" /> <%--onclick="RemoveSkuRecord('<%# (Container.DataItem["Id"].ToString()) %>');"--%>
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
        </asp:TabContainer>
    </center>
    <asp:HiddenField runat="server" ID="hdndeligateeditstate" ClientIDMode="Static" />
    <asp:HiddenField runat="server" ID="hdndegateId" ClientIDMode="Static" />
    <asp:HiddenField runat="server" ID="hdnDivCount" ClientIDMode="Static" />
    <asp:HiddenField runat="server" ID="hdnDDLRoleSelectedValue" ClientIDMode="Static" />
    <asp:HiddenField runat="server" ID="hndDesignationIndex" ClientIDMode="Static" />
    <asp:HiddenField runat="server" ID="hndDesignationValue" ClientIDMode="Static" />
    <asp:HiddenField runat="server" ID="hdnSelectedCompany" ClientIDMode="Static" />
    <asp:HiddenField runat="server" ID="hdnSelectedDepartment" ClientIDMode="Static" />
    <asp:HiddenField runat="server" ID="hdnSelectedDesignation" ClientIDMode="Static" />
    <asp:HiddenField runat="server" ID="hdnDepartmentName" ClientIDMode="Static" />
    <asp:HiddenField runat="server" ID="hdnDesignation" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnlocationName" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnlocationId" runat="server" ClientIDMode="Static" />
    <style type="text/css">
        .newlocationdiv
        {
            top: 0;
            left: 0;
            width: 120px;
            margin: 0px 3px 2px 0px;
        }
        .removeAL
        {
            color: Red;
            padding: 0px 3px 0px 3px;
            border-right: solid 1px silver;
        }
        .removeAL:hover
        {
            cursor: pointer;
        }
    </style>
    <asp:HiddenField ID="hndstate" runat="server" />
    <asp:HiddenField ID="hnduserID" runat="server" />
    <asp:HiddenField ID="hndRoleSate" runat="server" />
    <asp:HiddenField ID="hdncurrentpassword" runat="server" />
    <asp:HiddenField ID="hdnbomeditstate" runat="server" />
    <asp:HiddenField ID="hdnrollid" runat="server" />
    <asp:HiddenField ID="hdnnewDelegateid" runat="server" />
    <asp:HiddenField ID="hdnSelectedRecUserID" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnuserlock" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnUsrSelectedRec" runat="server" ClientIDMode="Static" />
    <script type="text/javascript">
        window.onload = function () {
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
                SelectedUsrRec();
            }
        }
        function SelectedUsrRec() {
            var hdnUsrSelectedRec = document.getElementById("hdnUsrSelectedRec");
            hdnUsrSelectedRec.value = "";

            for (var i = 0; i < gvUserCreationM.PageSelectedRecords.length; i++) {
                var record = gvUserCreationM.PageSelectedRecords[i];
                if (hdnUsrSelectedRec.value == "") hdnUsrSelectedRec.value = record.userID;
            }
        }

        var removeHTML;
        var count;
        function saveCheckBoxChangesRoleMaster(element, dataField, rowIndex) {
            if (GridRoleConfiguration.Rows[rowIndex].Cells[dataField].Value != element.checked.toString()) {
                if (element.checked == true) GridRoleConfiguration.Rows[rowIndex].Cells[dataField].Value = "true";
                if (element.checked == false) GridRoleConfiguration.Rows[rowIndex].Cells[dataField].Value = "false";
                var role = new Object(); role.mSequence = GridRoleConfiguration.Rows[rowIndex].Cells['mSequence'].Value;
                role.pSequence = GridRoleConfiguration.Rows[rowIndex].Cells['pSequence'].Value;
                role.oSequence = GridRoleConfiguration.Rows[rowIndex].Cells['oSequence'].Value;
                role.Add = GridRoleConfiguration.Rows[rowIndex].Cells['Add'].Value;
                role.Edit = //GridRoleConfiguration.Rows[rowIndex].Cells['Edit'].Value; 
                role.View = GridRoleConfiguration.Rows[rowIndex].Cells['View'].Value;
                //role.Delete = GridRoleConfiguration.Rows[rowIndex].Cells['Delete'].Value; 
                role.Approval = GridRoleConfiguration.Rows[rowIndex].Cells['Approval'].Value;
                role.AssignTask = GridRoleConfiguration.Rows[rowIndex].Cells['AssignTask'].Value; PageMethods.UpdateRole(role, rowIndex, null, null);
            }
        } function UserCreationEditRole(sender, e) { }

        function SetUnArchive(Ids, IsArchive) {
            PageMethods.PMMoveAddressToArchive(Ids, IsArchive, null, null);
        }

        function compareCPw() {
            var txt = document.getElementById("txtConfirmNewPassword");
            txt.style.border = "red";
            if (document.getElementById("txtPassword1").value == document.getElementById("txtConfirmNewPassword").value) {
                txt.style.border = "green";
            }
        }

        function AssignLocation() {
            var divSelectedLocation = document.getElementById("divSelectedLocation");
            var hdnDivCount = document.getElementById("hdnDivCount");
            var ddlLevel3 = document.getElementById("<%=ddlDepartment.ClientID %>");
            //var ddlLevel3 = document.getElementById(childID);
            if (ddlLevel3.selectedIndex == 0) { alert(ddlLevel3.options[ddlLevel3.selectedIndex].text); }
            else {
                if (divSelectedLocation.innerHTML.toString().indexOf(ddlLevel3.options[ddlLevel3.selectedIndex].text) == -1) {
                    var newLocaion = ddlLevel3.options[ddlLevel3.selectedIndex].text;
                    if (hdnDivCount.value == "") { hdnDivCount.value = "1"; };
                    var remove = "<a onclick=removeLocation('div" + hdnDivCount.value.toString() + "'," + ddlLevel3.value + ") class='removeAL'>X</a>";
                    divSelectedLocation.innerHTML = divSelectedLocation.innerHTML.replace(/^\s+|\s+$/g, '');

                    divSelectedLocation.innerHTML += "<span class='newlocationdiv' id=div" + hdnDivCount.value.toString() + ">-" + newLocaion + remove + "</span>";
                    hdnDivCount.value = parseInt(hdnDivCount.value) + 1;
                    if (document.getElementById("hdnSelectedLocation").value != "") { document.getElementById("hdnSelectedLocation").value += ',' + ddlLevel3.value; }
                    else { document.getElementById("hdnSelectedLocation").value = ddlLevel3.value; }
                }
                else { alert(ddlLevel3.options[0].text.replace('-', '').replace('Select', '') + ' already exist'); }
            }
        }
        function removeLocation(divname, locationID) {
            var maindiv = document.getElementById("divSelectedLocation");
            var divs = maindiv.getElementsByTagName("span");
            for (var i = 0; i < divs.length; i++) {
                if (divs[i].id == divname) {
                    divs[i].parentNode.removeChild(divs[i]);
                    document.getElementById("hdnSelectedLocation").value = document.getElementById("hdnSelectedLocation").value.replace(locationID + ',', "");
                    document.getElementById("hdnSelectedLocation").value = document.getElementById("hdnSelectedLocation").value.replace(',' + locationID, "");
                    document.getElementById("hdnSelectedLocation").value = document.getElementById("hdnSelectedLocation").value.replace(locationID, "");
                    break;
                }
            }

            var divSelectedLocation = document.getElementById("divSelectedLocation");
            divSelectedLocation.innerHTML = divSelectedLocation.innerHTML.replace(/^\s+|\s+$/g, '');
        }

        function clearSelectedLocations() {
            document.getElementById("divSelectedLocation").innerHTML = "";
            document.getElementById("hdnSelectedLocation").value = "";
        }

        function SetActiveTab() {
            var tabContainer = $get('<%=TabContainerUserCreation1.ClientID%>');
            tabContainer.control.set_activeTabIndex('3');
        }

        /*Check password strength*/
        function divPasswordwstrength() {
            var txtlength = document.getElementById("txtPassword1").value.length;
            var dpw = document.getElementById("divPasswordwstrength");
            var msg = document.getElementById("PasswordwstrengthMsg");
            dpw.style.height = "10px";
            switch (txtlength) {
                case 0:
                    dpw.style.backgroundColor = "red";
                    dpw.style.width = "10px";
                    msg.innerHTML = "Weak";
                    break;
                case 1:
                    dpw.style.backgroundColor = "red";
                    dpw.style.width = "10px";
                    msg.innerHTML = "Weak";
                    break;
                case 2:
                    dpw.style.backgroundColor = "red";
                    dpw.style.width = "20px";
                    msg.innerHTML = "Weak";
                    break;
                case 3:
                    dpw.style.backgroundColor = "red";
                    dpw.style.width = "30px";
                    msg.innerHTML = "Weak";
                    break;
                case 4:
                    dpw.style.backgroundColor = "red";
                    dpw.style.width = "40px";
                    msg.innerHTML = "Weak";
                    break;
                case 5:
                    dpw.style.backgroundColor = "orange";
                    dpw.style.width = "50px";
                    msg.innerHTML = "Average";
                    break;
                case 6:
                    dpw.style.backgroundColor = "orange";
                    dpw.style.width = "60px";
                    msg.innerHTML = "Average";
                    break;
                case 7:
                    dpw.style.backgroundColor = "orange";
                    dpw.style.width = "70px";
                    msg.innerHTML = "Average";
                    break;
                case 8:
                    dpw.style.backgroundColor = "orange";
                    dpw.style.width = "80px";
                    msg.innerHTML = "Average";
                    break;
                case 9:
                    dpw.style.backgroundColor = "orange";
                    dpw.style.width = "90px";
                    msg.innerHTML = "Average";
                    break;
                case 10:
                    dpw.style.backgroundColor = "green";
                    dpw.style.width = "100px";
                    msg.innerHTML = "Strong";
                    break;
                case 11:
                    dpw.style.backgroundColor = "green";
                    dpw.style.width = "110px";
                    msg.innerHTML = "Strong";
                    break;
                case 12:
                    dpw.style.backgroundColor = "green";
                    dpw.style.width = "120px";
                    msg.innerHTML = "Strong";
                    break;
                case 13:
                    dpw.style.backgroundColor = "green";
                    dpw.style.width = "130px";
                    msg.innerHTML = "Strong";
                    break;
                case 14:
                    dpw.style.backgroundColor = "green";
                    dpw.style.width = "140px";
                    msg.innerHTML = "Strong";
                    break;
                case 15:
                    dpw.style.backgroundColor = "green";
                    dpw.style.width = "150px";
                    msg.innerHTML = "Strong";
                    break;
            }
        }
        /*End*/

        /*Fill Dropdown*/
        function fillDesignation(invoker) {
            ddlLoadingOn(document.getElementById("ddlDesignation"));
            document.getElementById("ddlRoleList").options.length = 0;
            PageMethods.PMfillDesignation(parseInt(invoker.value), fillDesignationOnSuccess, fillDesignationOnError)
        }
        function fillDesignationOnSuccess(result) {
            var ddl = document.getElementById("ddlDesignation");

            for (var i = 0; i < result.length; i++) {
                var option1 = document.createElement("option");
                option1.text = result[i].Name;
                option1.value = result[i].ID;
                try {
                    ddl.add(option1, null);
                }
                catch (Error) {
                    ddl.add(option1);
                }
            }

            ddlLoadingOff(ddl);
        }

        function fillDesignationOnError() {
            ddlFillError(document.getElementById("ddlDesignation"));
        }

        function fillRoleDDL(invoker) {
            var ddlDepartment = document.getElementById("ddlDepartment");
            var ddlDesignation = document.getElementById("ddlDesignation");

            document.getElementById("hndDesignationIndex").value = invoker.selectedIndex;

            var SelectedText = invoker.options[invoker.selectedIndex].value;
            document.getElementById("hndDesignationValue").value = SelectedText;

            //Add By Suresh
            var hndDesignationValue = document.getElementById('hndDesignationValue');
            hndDesignationValue.value = 10;

            //ddlLoadingOn(document.getElementById("ddlRole"));

            if (ddlDepartment.selectedIndex > 0) {
                PageMethods.PMfillRoleDDL(parseInt(ddlDepartment.value), 10, fillRoleDDLOnSuccess, fillRoleDDLOnError)
            }
        }

        function fillRoleDDLOnSuccess(result) {
            var ddl = document.getElementById("<%=ddlRole.ClientID %>");
            var option0 = document.createElement("option");
            if (result.length > 0) {
                option0.text = '-Select-';
                option0.value = "0";
                try {
                    ddl.add(option0, null);
                }
                catch (Error) {
                    ddl.add(option0);
                }
            }

            //            var option2 = document.createElement("option");
            //            var option3 = document.createElement("option");
            //            option3.text = '-Select-';
            //            option3.value = "0";
            //            option2.text = 'Custom';
            //            option2.value = "1";
            //            option2.style.color = "red";
            //            option2.style.fontWeight = "bold";
            //            try {
            //                ddl.add(option3, null);
            //                ddl.add(option2, null);

            //            }
            //            catch (Error) {
            //                ddl.add(option2);
            //            }
            for (var i = 0; i < result.length; i++) {
                var option1 = document.createElement("option");
                option1.text = result[i].RoleName;
                option1.value = result[i].ID;
                try {
                    ddl.add(option1, null);
                }
                catch (Error) {
                    ddl.add(option1);
                }
            }
            ddlLoadingOff(ddl);
        }

        function fillRoleDDLOnError() {
            ddlFillError(document.getElementById("ddlRole"));
        }

        function RefreshGVRoleConfig(invoker) {
            var hdnDDLRoleSelectedValue = document.getElementById('hdnDDLRoleSelectedValue');
            hdnDDLRoleSelectedValue.value = invoker.options[invoker.selectedIndex].value;
            GridRoleConfiguration.refresh();
        }

        function GetCompany() {
            var ddlcompany = document.getElementById("<%=ddlcompany.ClientID %>");

            var hdnSelectedCompany = document.getElementById('hdnSelectedCompany');
            hdnSelectedCompany.value = ddlcompany.value;
            // alert(hdnSelectedCompany.value);
        }

        function GetRoll() {
            var ddlcompany = document.getElementById("<%=ddlRole.ClientID %>");

            var hdnrollid = document.getElementById("<%=hdnrollid.ClientID %>");
            document.getElementById("<%=hdnrollid.ClientID %>").value = ddlcompany.value;
            hdnrollid.value = ddlcompany.value;
            // alert(hdnSelectedCompany.value);
        }

        function GetDepartment() {
            var ddlDepartment = document.getElementById("<%=ddlDepartment.ClientID %>");
            document.getElementById("<%=hdnSelectedDepartment.ClientID %>").value = ddlDepartment.value;

            var hdnSelectedDepartment = document.getElementById('hdnSelectedDepartment');
            var hdnDepartmentName = document.getElementById('hdnDepartmentName');

            hdnSelectedDepartment.value = ddlDepartment.value;
            hdnDepartmentName.value = ddlDepartment.options[ddlDepartment.selectedIndex].innerHTML;

            var obj1 = new Object();
            obj1.ddlDepartment = ddlDepartment.value.toString();
          //  PageMethods.GetRollById(obj1, getRoll_onSuccessed);
            PageMethods.Getdelegate(obj1, getLoc_onSuccessed);
        }

        //alert(hdnDepartmentName.value);


        function getLoc_onSuccessed(result) {
            var ddlUOM = document.getElementById("<%=ddlUOM.ClientID %>");
            ddlUOM.options.length = 0;
            for (var i in result) {
                AddOption(result[i].Name, result[i].Id);
            }
        }

        function AddOption(text, value) {
            var ddlUOM = document.getElementById("<%=ddlUOM.ClientID %>");
            var option = document.createElement('option');
            option.value = value;
            option.innerHTML = text;
            ddlUOM.options.add(option);
        }

        function getRoll_onSuccessed(result) {
            var ddlRole = document.getElementById("<%=ddlRole.ClientID %>");
            ddlRole.options.length = 0;
            for (var i in result) {
                AddOption1(result[i].Name, result[i].Id);
            }
        }

        function AddOption1(text, value) {
            var ddlRole = document.getElementById("<%=ddlRole.ClientID %>");
            var option1 = document.createElement('option');
            option1.value = value;
            option1.innerHTML = text;
            ddlRole.options.add(option1);
        }

        function GetDesignation() {
            var ddlDesignation = document.getElementById("<%=ddlDesignation.ClientID %>");

            var hdnSelectedDesignation = document.getElementById('hdnSelectedDesignation');
            hdnSelectedDesignation.value = ddlDesignation.value;

            var hdnDesignation = document.getElementById('hdnDesignation');
            hdnDesignation.value = ddlDesignation.options[ddlDesignation.selectedIndex].innerHTML;


            alert(hdnDesignation.value);
        }


        function BindDepartment() {

            PageMethods.PMGetDepartmentList(hdnSelectedCompany.value, onSuccessGetDepartmentList, null)
        }
        function onSuccessGetDepartmentList(result) {
            ddlDepartment = document.getElementById("<%=ddlDepartment.ClientID %>");
            ddlDepartment.options.length = 0;
            var option0 = document.createElement("option");
            var option3 = document.createElement("option");

            if (result.length > 0) {
                option0.text = "--Select--";
                option0.value = '0';
            }
            else {
                option0.text = 'N/A';
                option0.value = '0';
            }

            try {
                ddlDepartment.add(option0, null);
            }
            catch (Error) {
                ddlDepartment.add(option0);
            }

            for (var i = 0; i < result.length; i++) {
                var option1 = document.createElement("option");

                option1.text = result[i].Territory;
                option1.value = result[i].ID;
                try {
                    ddlDepartment.add(option1, null);
                }
                catch (Error) {
                    ddlDepartment.add(option1);
                }
            }
        }

        var ddlcompany = document.getElementById("<%=ddlcompany.ClientID %>");
        var ddldeptid = document.getElementById("<%=ddlDepartment.ClientID %>");

        function GetDepartmentnew() {
            var ddlcompany = document.getElementById("<%=ddlcompany.ClientID %>");

            var hdnSelectedCompany = document.getElementById('hdnSelectedCompany');
            hdnSelectedCompany.value = ddlcompany.value;

            var obj1 = new Object();
            obj1.ddlcompanyId = ddlcompany.value.toString();

            PageMethods.GetDepartment(obj1, getLocDept_onSuccessed);

            //PageMethods.GetDepartmentfrmSelCmpny(hdnSelectedCompany.value,getLocDept_onSuccessed);
        }

        function getLocDept_onSuccessed(result) {

            ddldeptid.options.length = 0;
            for (var i in result) {
                AddOption12(result[i].Name, result[i].Id);
            }
        }

        function AddOption12(text, value) {

            var option12 = document.createElement('option');
            option12.value = value;
            option12.innerHTML = text;
            ddldeptid.options.add(option12);
        }

        function BindDesignation() {

            PageMethods.PMGetDesignation(hdnSelectedCompany.value, hdnSelectedDepartment.value, onSuccessDesignationList, null)
        }

        function onSuccessDesignationList(result) {
            ddlDesignation = document.getElementById("<%=ddlDesignation.ClientID %>");
            ddlDesignation.options.length = 0;
            var option0 = document.createElement("option");

            if (result.length > 0) {
                option0.text = "--Select--";
                option0.value = '0';
            }
            else {
                option0.text = 'N/A';
                option0.value = '0';
            }

            try {
                ddlDesignation.add(option0, null);
            }
            catch (Error) {
                ddlDesignation.add(option0);
            }

            for (var i = 0; i < result.length; i++) {
                var option1 = document.createElement("option");
                option1.text = result[i].Name;
                option1.value = result[i].ID;
                try {
                    ddlDesignation.add(option1, null);
                }
                catch (Error) {
                    ddlDesignation.add(option1);
                }
            }

        }

        function CheckDate() {

            alert();
        }
       
    </script>
    <%-- Access Delegation javascript save code pagemethod--%>
    <%--Code for Location Add tab --%>>

     <script type="text/javascript">
         function openProductSearch(sequence) {
             var UserId = document.getElementById("<%=hnduserID.ClientID %>").value;
             window.open('../UserManagement/LocationList.aspx?UserId=' + UserId + "", null, 'height=700px, width=825px,status= 0, resizable= 1, scrollbars=1, toolbar=0,location=center,menubar=0, screenX=300; screenY=50');
         }

         function RemoveSkuRecord(Id) {
             var obj1 = new Object();
             var Detailid = Id;
             obj1.distribId = Detailid;
             PageMethods.RemoveSku(obj1, Removesku_onSuccess);
         }

         function Removesku_onSuccess(result) {
            Grid1.refresh();
         }
    </script>
    
    <%--Code for Warehouse Add tab --%>

    <script type="text/javascript">
        function openWarehouseSearch(sequence) {
           // var UserId = document.getElementById("<%=hnduserID.ClientID %>").value;
            var UserId = 0;
            window.open('../UserManagement/WarehouseList.aspx?UserId=' + UserId + "", null, 'height=700px, width=825px,status= 0, resizable= 1, scrollbars=1, toolbar=0,location=center,menubar=0, screenX=300; screenY=50');
        }

    </script>


    <script type="text/javascript">
        function SaveAccessDelegation() {
            var FromDate = getDateFromUC("<%= UC_Date1.ClientID %>");
            var ToDate = getDateFromUC("<%=UC_Date2.ClientID %>");
            var hdndeligateeditstate = document.getElementById("<%=hdndeligateeditstate.ClientID %>");
            var hdndegateId = document.getElementById("<%=hdndegateId.ClientID %>");
            var Delegateto = document.getElementById("<%=ddlUOM.ClientID %>");
            var Remark = document.getElementById("<%=txtPrincipalPrice.ClientID %>");
            var UserId = document.getElementById("<%=hnduserID.ClientID %>");
            var hndstate = document.getElementById("<%=hndstate.ClientID %>");
            var newdelegateid = document.getElementById("<%=hdnnewDelegateid.ClientID %>");
            var obj1 = new Object();
            obj1.hdndeligateeditstate = hdndeligateeditstate.value;
            obj1.hdndegateId = hdndegateId.value;
            obj1.Delegateto = Delegateto.value.toString();
            obj1.Remark = Remark.value.toString();
            obj1.UserId = UserId.value.toString();
            obj1.hndstate = hndstate.value.toString();
            obj1.FromDate = FromDate;
            obj1.ToDate = ToDate;
            obj1.newDelegate = newdelegateid.value;
            PageMethods.SaveAccessDelegation(obj1, getSave_onSuccessed)
        }

        function getSave_onSuccessed(result) {
            grdaccessdele.refresh();
            document.getElementById("<%=ddlUOM.ClientID %>").value = 0;
            document.getElementById("<%=txtPrincipalPrice.ClientID %>").value = "";
            getDateFromUC("<%= UC_Date1.ClientID %>").value = "";
            getDateFromUC("<%= UC_Date1.ClientID %>") = "";

        }
    </script>
    <script type="text/javascript">
        function CheckPassword() {
            var dpw = document.getElementById("divPasswordwstrength");
            var msg = document.getElementById("PasswordwstrengthMsg");
            var Password = document.getElementById("<%=txtPassword1.ClientID %>").value;
            var ConfirmNewPassword = document.getElementById("<%=txtConfirmPassword.ClientID %>").value;
            // var demo = /^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[^a-zA-Z0-9])(?!.*\s).{8,15}$/;
            var pass = /^(?=.*\d)(?=.*[a-z])(?=.*[^a-zA-Z0-9])(?!.*\s).{8,32}$/;

            if (Password != ConfirmNewPassword) {
                document.getElementById("<%=txtConfirmPassword.ClientID %>").value = "";
                showAlert("Password And Confirm Should Be Same","Error","#");
                document.getElementById("<%=txtConfirmPassword.ClientID %>").focus;
                return false;
            }
            else if (ConfirmNewPassword.length < 8 || ConfirmNewPassword.length > 32) {

                document.getElementById("<%=txtConfirmPassword.ClientID %>").value = "";
                document.getElementById("<%=txtPassword1.ClientID %>").value = "";
                showAlert("Password length should be grater than 8 characters and less than 32 characters","Error","#");
                document.getElementById("<%=txtPassword1.ClientID %>").focus;
                dpw.style.width = "0px";
                msg.innerHTML = "";
                return false;
            }
            else if (!ConfirmNewPassword.match(pass)) {

                document.getElementById("<%=txtConfirmPassword.ClientID %>").value = "";
                document.getElementById("<%=txtPassword1.ClientID %>").value = "";
                showAlert("Password should be combination of alphabets, numbers, special characters.","Error","#");
                document.getElementById("<%=txtPassword1.ClientID %>").focus;
                dpw.style.width = "0px";
                msg.innerHTML = "";
                return false;
            }
        }
    </script>
    <script type="text/javascript">
        window.onload = function () {
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
                selectRec();
            }
        }
        function selectRec() {
            var hdnSelectedRecUserID = document.getElementById("hdnSelectedRecUserID");
            var hdnuserlock = document.getElementById("hdnuserlock");
            hdnSelectedRecUserID.value = "";
            hdnuserlock.value = "";

            for (i = 0; i < gvUserCreationM.PageSelectedRecords.length; i++) {
                var record = gvUserCreationM.PageSelectedRecords[i];
                if (hdnSelectedRecUserID.value != "") hdnSelectedRecUserID.value += ',' + record.userIDPass;
                if (hdnSelectedRecUserID.value == "") hdnSelectedRecUserID.value = record.userIDPass;
                if (hdnuserlock.value != "") hdnuserlock.value += ',' + record.locked;
                if (hdnuserlock.value == "") hdnuserlock.value = record.locked;
            }
        }
        function lockunlock() {
            var hdnSelectedRecUserID = document.getElementById("hdnSelectedRecUserID").value;
            var hdnuserlock = document.getElementById("hdnuserlock").value;
            if (hdnSelectedRecUserID == "") {
                showAlert("Please Select User...","Error","#");
            }
            else {
                PageMethods.WMlockunlock(hdnSelectedRecUserID, hdnuserlock, OnSuccessOperation, null);
            }
        }
        function OnSuccessOperation(result) {
            // alert("Coupon Allocated");
            gvUserCreationM.refresh();
        }    
    </script>

    <script type="text/javascript">
        function getimagefileupload()
        {
            var file_uploader = document.getElementById(FileUploadProfileImg);  
        
            var file_content = file_uploader.files[0].getAsBinary();
            PageMethods.Get_image(file_content,onimgsuccess,onimgerror);
        }

        function onimgsuccess(result)
        {
            alert(result);
        }

        function onimgerror(result)
        {
            alert("Image upload failed");
        }
    </script>  
</asp:Content>

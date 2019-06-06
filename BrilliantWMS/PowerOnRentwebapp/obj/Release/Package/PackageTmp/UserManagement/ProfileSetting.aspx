<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/CRM.Master" AutoEventWireup="true"
    CodeBehind="ProfileSetting.aspx.cs" Inherits="BrilliantWMS.UserManagement.ProfileSetting" %>

<%@ Register Assembly="obout_Grid_NET" Namespace="Obout.Grid" TagPrefix="obout" %>
<%@ Register Assembly="obout_Interface" Namespace="Obout.Interface" TagPrefix="obout" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../CommonControls/UCFormHeader.ascx" TagName="UCFormHeader" TagPrefix="uc2" %>
<%@ Register Src="../CommonControls/UCToolbar.ascx" TagName="UCToolbar" TagPrefix="uc1" %>
<%@ Register Src="../Address/UCAddress.ascx" TagName="UCAddress" TagPrefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHolder_FormHeader" runat="server">
    <uc1:UCToolbar ID="UCToolbar1" runat="server" />
    <uc2:UCFormHeader ID="UCFormHeader1" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPHolder_Form" runat="server">
    <asp:TabContainer ID="TabContainerUpdateProfile" runat="server">
        <asp:TabPanel ID="TabPanelUsersDetails" runat="server" HeaderText="User Details">
            <ContentTemplate>
                <center>
                    <asp:ValidationSummary ID="validationsummary_ProfileSetting" runat="server" ShowMessageBox="True"
                        ShowSummary="False" ValidationGroup="Save" />
                    <table class="tableForm">
                        <tr>
                            <td>
                                Employee No. :
                            </td>
                            <td style="text-align: left;">
                                <asp:Label runat="server" ID="lblEmpNo" Style="font-weight: bold;"></asp:Label>
                            </td>
                            <td>
                                User Name :
                            </td>
                            <td style="text-align: left;">
                                <asp:Label runat="server" ID="lblUserName" Style="font-weight: bold;"></asp:Label>
                            </td>
                            <td rowspan="6">
                                <div id="profile">
                                    <img runat="server" id="ImgProfile" width="110" height="132" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Department :
                            </td>
                            <td style="text-align: left;">
                                <asp:Label runat="server" ID="lblDepartment" Style="font-weight: bold;"></asp:Label>
                            </td>
                            <td>
                                Designation :
                            </td>
                            <td style="text-align: left;">
                                <asp:Label runat="server" ID="lblDesignation" Style="font-weight: bold;"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Date Of Joining :
                            </td>
                            <td style="text-align: left;">
                                <asp:Label runat="server" ID="lblDOJ" Style="font-weight: bold;"></asp:Label>
                            </td>
                            <td>
                                Date of Birth :
                            </td>
                            <td style="text-align: left;">
                                <asp:Label runat="server" ID="lblDOB" Style="font-weight: bold;"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Phone No. :
                            </td>
                            <td>
                                <asp:TextBox ID="txtPhone" runat="server" Width="250px" onkeydown="AllowPhoneNo(this,event);"></asp:TextBox>
                            </td>
                            <td>
                                Mobile No. :
                            </td>
                            <td>
                                <asp:TextBox ID="txtMobile" runat="server" Width="250px" onkeydown="AllowPhoneNo(this,event);"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <req>Email ID :</req>
                            </td>
                            <td>
                                <asp:TextBox ID="txtEmailID" runat="server" ValidationGroup="Save" Width="250px"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidatortxtEmailID" runat="server"
                                    ControlToValidate="txtEmailID" ValidationGroup="Save" ErrorMessage="Enter valid email ID "
                                    ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" Display="None"
                                    SetFocusOnError="True"></asp:RegularExpressionValidator>
                            </td>
                            <td>
                                Other Email ID :
                            </td>
                            <td>
                                <asp:TextBox ID="txtOtherEmailID" runat="server" ValidationGroup="Save" Width="250px"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidatortxtOtherEmailID" runat="server"
                                    ControlToValidate="txtOtherEmailID" ValidationGroup="Save" ErrorMessage="Enter valid email ID"
                                    ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" Display="None"
                                    SetFocusOnError="True"></asp:RegularExpressionValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Highest Qualification :
                            </td>
                            <td>
                                <asp:TextBox ID="txtHighestQualification" runat="server" Width="250px"></asp:TextBox>
                            </td>
                            <td>
                                Interested In :
                            </td>
                            <td>
                                <asp:TextBox ID="txtInstratedIn" runat="server" Width="250px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Reporting To :
                            </td>
                            <td style="text-align: left;">
                                <asp:Label runat="server" ID="lblReportingTo" Style="font-weight: bold;"></asp:Label>
                            </td>
                            <td>
                                Profile Photo :
                            </td>
                            <td colspan="2" style="text-align: left;">
                                <asp:FileUpload ID="FileUploadProfileImg" runat="server" ClientIDMode ="Static" onchange="fileUploaderValidationImgOnly(this,20480)"/>
                                <asp:LinkButton runat="server" ID="lnkUpdateProfileImg" Text="Upload" OnClick="lnkUpdateProfileImg_Click"></asp:LinkButton>
                                <br />
                                <span class="watermark">.jpg|.jpeg|.gif|.png|.bmp files are allowed </span>
                                  <br/>
                                 <span class="watermark">Max Limit 20 KB </span>
                            </td>
                        </tr>
                    </table>
                </center>
            </ContentTemplate>
        </asp:TabPanel>
        <asp:TabPanel ID="TabPanelAddressInfo" runat="server">
            <HeaderTemplate>
                Address Info
            </HeaderTemplate>
            <ContentTemplate>
                <uc3:UCAddress ID="UCAddress1" runat="server" />
            </ContentTemplate>
        </asp:TabPanel>
    </asp:TabContainer>
    <style type="text/css">
        #profile img
        {
            border: solid 2px gray;
            background-color: White;
            width: 110px; /*border: 2px solid #236496;*/
            height: 132px;
            -moz-border-radius: 4px;
            -webkit-border-radius: 4px;
            border-radius: 4px;
            padding: 0px;
        }
    </style>
</asp:Content>

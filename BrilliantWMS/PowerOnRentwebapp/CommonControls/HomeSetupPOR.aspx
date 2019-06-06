<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/CRM.Master" AutoEventWireup="true"
    CodeBehind="HomeSetupPOR.aspx.cs" Inherits="BrilliantWMS.CommonControls.HomeSetupPOR" %>

<%@ Register Src="UCToolbar.ascx" TagName="UCToolbar" TagPrefix="uc1" %>
<%@ Register Src="UCFormHeader.ascx" TagName="UCFormHeader" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHolder_FormHeader" runat="server">
    <uc2:UCFormHeader ID="UCFormHeader1" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPHolder_Form" runat="server">
    <center>
        <asp:UpdateProgress ID="UpdateProgress_HomeSetup" runat="server" AssociatedUpdatePanelID="updPnl_HomeSetup">
            <ProgressTemplate>
                <center>
                    <div class="modal">
                        <img src="../App_Themes/Blue/img/ajax-loader.gif" style="top: 50%;" />
                    </div>
                </center>
            </ProgressTemplate>
        </asp:UpdateProgress>
        <asp:UpdatePanel ID="updPnl_HomeSetup" runat="server">
            <ContentTemplate>
                <br />
                <br />
                <br />
                <table class="tableForm">
                    <tr id="row1" runat="server">
                        <td id="ctd1" runat="server" rowspan="2">
                            <img alt="" src="HomeSetupImg/companymgm.jpg" />
                        </td>
                        <td id="ctd2" runat="server" style="text-align: left">
                            <asp:LinkButton ID="lnkBtnCompanyMang" runat="server" CssClass="ParentGroup" Text="Customer Management"></asp:LinkButton>
                        </td>
                        <td rowspan="2">
                            <img alt="" src="HomeSetupImg/user_Management.png" />
                        </td>
                        <td style="text-align: left">
                            <asp:LinkButton ID="lnkBtnUserMang0" runat="server" CssClass="ParentGroup" Text="User Management"></asp:LinkButton>
                        </td>
                    </tr>
                    <tr id="row2" runat="server">
                        <td id="ctd3" runat="server" style="text-align: left; width: 350px;">
                            <a href="../Account/AccountMaster.aspx">
                                <asp:Label ID="lblcustomermaster" runat="server" Text="Customer Master"></asp:Label></a> |
                            <a href="../Account/Location.aspx">
                                <asp:Label ID="Label1" runat="server" Text="Location Master"></asp:Label></a>
                        </td>
                        <td style="text-align: left;">
                             <a href="../UserManagement/RoleMaster.aspx">
                               <%--<asp:Label ID="lblrollmaster" runat="server" Text="Role Master" /></a> |--%> <a href="../UserManagement/UserCreation.aspx">
                                    <asp:Label ID="lblusermaster" runat="server" Text="User Master" /></a> |
                            <a href="../Approval/ApprovalLevelMaster.aspx">
                                <asp:Label ID="lblapproval" runat="server" Text="Approval Master" /></a>
                        </td>
                    </tr>
                    <tr>
                    </tr>
                    <tr>
                        <td colspan="4">
                        </td>
                    </tr>
                    <tr>
                        <td rowspan="2">
                            <img alt="" src="HomeSetupImg/productmanagement.jpg" />
                        </td>
                        <td style="text-align: left">
                            <asp:LinkButton ID="lnkBtnProductMang" runat="server" CssClass="ParentGroup" Text="SKU Management"></asp:LinkButton>
                        </td>
                        <td  id="ctd4" runat="server" rowspan="2">
                            <img alt="" src="HomeSetupImg/inventory.jpg" />
                        </td>
                        <td  id="ctd5" runat="server" style="text-align: left">
                            <asp:LinkButton ID="lnkbtninterface" runat="server" CssClass="ParentGroup" Text="Interface Management"></asp:LinkButton>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left">
                            <a href="../Product/ProductMaster.aspx">
                                <asp:Label ID="lblsku" runat="server" Text="SKU" /></a> |
                            <a href="../Product/ImportPriceD.aspx">
                                <asp:Label ID="lblimportprice" runat="server" Text="Import Price" /></a> |
                             <a href="../PowerOnRent/DirectImportD.aspx">
                                <asp:Label ID="Label2" runat="server" Text="Direct Order" /></a>
                        </td>
                        <td id="ctd7" runat="server" style="width:150px;"></td>
                        <td  id="ctd6" runat="server" style="text-align: left">
                            <a href="../PowerOnRent/InterfaceDefination.aspx">
                                <asp:Label ID="lblinterdef" runat="server" Text="Interface Defination" /></a>
                            | <a href="../PowerOnRent/MessageDefination.aspx">
                                <asp:Label ID="lblmsgdef" runat="server" Text="Message Defination" />
                            </a>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                        </td>
                    </tr>
                    <tr>
                        <td rowspan="2">
                            <img alt="" src="HomeSetupImg/reports.jpg" />
                        </td>
                        <td style="text-align: left">
                            <asp:LinkButton ID="lnkBtnActivityMang" runat="server" CssClass="ParentGroup" Text="Utility"></asp:LinkButton>
                        </td>
                        <td rowspan="2">
                            <img alt="" src="HomeSetupImg/help_64.png" />
                        </td>
                        <td style="text-align: left">
                            <asp:LinkButton ID="lnkHelp" runat="server" CssClass="ParentGroup" Text="Help"></asp:LinkButton>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left">
                            <a href="../Product/EmailTemplate.aspx">
                                <asp:Label ID="lblemailconfig" runat="server" Text="Email Configuration" /></a>
                            | <a href="../PowerOnRent/RequestTemplate.aspx">
                                <asp:Label ID="lblrequsttemp" runat="server" Text="Request Template" /></a><br />
                            <a href="../Product/ImportDSo.aspx">
                                <asp:Label ID="lblimageimport" runat="server" Text="Image Import" /></a>
                        </td>
                        <td style="text-align: left">
                        <a href="User Manual/GWC OMS User Guide 1.1.pdf">
                                <asp:Label ID="lblHelp" runat="server" Text="Help" /></a>
                        </td>
                        
                    </tr>
                    <tr>
                        <td colspan="4">
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </center>
</asp:Content>

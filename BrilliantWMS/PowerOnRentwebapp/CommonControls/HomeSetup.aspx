<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/CRM.Master" AutoEventWireup="true"
    CodeBehind="HomeSetup.aspx.cs" Inherits="BrilliantWMS.CommonControls.HomeSetup" %>

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
                    <tr>
                        <td rowspan="2">
                            <img alt="" src="HomeSetupImg/companymgm.jpg" />
                        </td>
                        <td style="text-align: left">
                            <asp:LinkButton ID="lnkBtnCompanyMang" runat="server" CssClass="ParentGroup">Company Management</asp:LinkButton>
                        </td>
                        <td rowspan="2">
                            <img alt="" src="HomeSetupImg/taxmagmt.png" />
                        </td>
                        <td style="text-align: left">
                            <asp:LinkButton ID="lnkBtnTaxMang" runat="server" CssClass="ParentGroup">Tax Management</asp:LinkButton>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 350px;">
                            <a href="../Company/CompanySetup.aspx">Company Setup</a> | <a href="../Company/TermsAndConditionMaster.aspx">
                               Terms and Conditions Master</a>
                        </td>
                        <td style="text-align: left; width: 350px;">
                            <a href="../Tax/StatutoryMaster.aspx">Statutory Master</a> | <a href="../Tax/TaxMaster.aspx">
                                Tax Master</a>
                        </td>
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
                            <asp:LinkButton ID="lnkBtnProductMang" runat="server" CssClass="ParentGroup">Product Management</asp:LinkButton>
                        </td>
                        <td rowspan="2">
                            <img alt="" src="HomeSetupImg/user_Management.png" />
                        </td>
                        <td style="text-align: left">
                            <asp:LinkButton ID="lnkBtnUserMang" runat="server" CssClass="ParentGroup">User Management</asp:LinkButton>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left">
                          <a href="../Product/ProductCategoryMaster.aspx">Category Master</a> |  <a href="../Product/ProductSubCategoryMaster.aspx">Sub Category Master</a> <br /> <a href="../Product/ProductMaster.aspx">Product Master</a> | <a href="../Product/DiscountMaster.aspx">
                                Discount Master</a> 
                          <span style="visibility:hidden;"> |<a href="../Inventory/InventoryManagement.aspx">Inventory Master</a>
                           <a href="../Vendor/VendorMaster.aspx">
                                Vendor Master</a> </span>
                        </td>
                        <td style="text-align: left">
                            <a href="../UserManagement/DepartmentMaster.aspx">Department Master</a> | <a href="../UserManagement/DesignationMaster.aspx">
                                Designation Master</a> | <a href="../UserManagement/RoleMaster.aspx">Role Master</a>
                            <br />
                            <a href="../UserManagement/UserCreation.aspx">User Master</a> | <a href="../Approval/ApprovalLevelMaster.aspx">
                                Approval Master</a>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                        </td>
                    </tr>
                    <tr>
                        <td rowspan="2">
                            <img alt="" src="HomeSetupImg/leadManagement.jpg" />
                        </td>
                        <td style="text-align: left">
                            <asp:LinkButton ID="lnkBtnLeadMang" runat="server" CssClass="ParentGroup">Lead Management</asp:LinkButton>
                        </td>
                        <td rowspan="2">
                            <img alt="" src="HomeSetupImg/ActivityManagement.jpg" />
                        </td>
                        <td style="text-align: left">
                            <asp:LinkButton ID="lnkBtnActivityMang" runat="server" CssClass="ParentGroup">Activity Management</asp:LinkButton>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left">
                            <a href="../ContactPerson/ContactTypeMaster.aspx">Contact Type Master</a> | <a href="../Lead/LeadSourceMaster.aspx">
                                Lead Source Master</a>
                            <br />
                            <a href="../Address/RouteMaster.aspx">Route Master</a>
                        </td>
                        <td style="text-align: left">
                            <a href="../Activity/ActivityMaster.aspx">Activity Master</a> | <a href="../Activity/SubActivityMaster.aspx">
                                SubActivity Master</a>
                            <%--| <a href="../Activity/StatusMaster.aspx">Status Master</a>--%>
                            <br />
                            <a href="../Activity/StatusReasonMaster.aspx">Status Reason Master</a>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                        </td>
                    </tr>
                    <%--<tr>
                        <td rowspan="2">
                            <img alt="" src="HomeSetupImg/account.png" />
                        </td>
                        <td style="text-align: left">
                            <asp:LinkButton ID="lnkBtnAccount" runat="server" CssClass="ParentGroup">Account</asp:LinkButton>
                        </td>
                        <td rowspan="2">
                            <img alt="" src="HomeSetupImg/customfields.png" />
                        </td>
                        <td style="text-align: left">
                            <asp:LinkButton ID="lnkBtnCustomeField" runat="server" CssClass="ParentGroup">Custom Fields</asp:LinkButton>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left">
                            <a href="../Account/AccountMaster.aspx">Account Master</a>
                            | <a href="">Rate Card Master</a>
                        </td>
                        <td style="text-align: left">
                              <a href="">Custom Fields Master</a>
                        </td>
                    </tr>--%>
                </table>
                <%-- <asp:LinkButton ID="LinkButton34" runat="server" CssClass="ChildGrp" OnClick="LinkButton34_Click">Status Master</asp:LinkButton>--%>
            </ContentTemplate>
        </asp:UpdatePanel>
    </center>
</asp:Content>

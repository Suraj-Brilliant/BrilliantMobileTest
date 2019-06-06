<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/CRM.Master" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="ContactTypeMaster.aspx.cs" Inherits="BrilliantWMS.ContactPerson.ContactTypeMaster" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="obout_Interface" Namespace="Obout.Interface" TagPrefix="obout" %>
<%@ Register Assembly="obout_Grid_NET" Namespace="Obout.Grid" TagPrefix="obout" %>
<%@ Register Src="../CommonControls/UCFormHeader.ascx" TagName="UCFormHeader" TagPrefix="uc1" %>
<%@ Register Src="../CommonControls/UCToolbar.ascx" TagName="UCToolbar" TagPrefix="uc2" %>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHolder_FormHeader" runat="server">
<uc2:uctoolbar id="UCToolbar1" runat="server" />
    <uc1:ucformheader id="UCFormHeader1" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPHolder_Form" runat="server">
<center>
        <asp:UpdateProgress ID="UpdateProgress_ContactTypeM" runat="server" AssociatedUpdatePanelID="updPnl_ContactTypeM">
            <ProgressTemplate>
                <center>
                    <div class="modal">
                        <img src="../App_Themes/Blue/img/ajax-loader.gif" style="top: 50%;" />
                    </div>
                </center>
            </ProgressTemplate>
        </asp:UpdateProgress>
        <asp:ValidationSummary ID="validationsummary_ContactMaster" runat="server" ShowMessageBox="true"
            ShowSummary="false" DisplayMode="bulletlist" ValidationGroup="Save" />
        <asp:UpdatePanel ID="updPnl_ContactTypeM" runat="server">
            <ContentTemplate>
                <table class="tableForm">
                    <tr>
                        <td>
                            <req>Contact Type :</req>
                        </td>
                        <td style="text-align: left">
                            <asp:TextBox ID="txtContactType" runat="server" Width="200px" onKeyPress="return alpha(this,event);"
                                ValidationGroup="Save" MaxLength="50"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="valRftxtContactType" runat="server" ControlToValidate="txtContactType"
                                ErrorMessage="Enter Contact Type" Display="None" ValidationGroup="Save"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            Sequence No.:
                        </td>
                        <td style="text-align: left">
                            <asp:TextBox ID="txtSequence" runat="server" Width="70px" MaxLength="3" onkeypress="return AllowInt(this, event);"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Remark :
                        </td>
                        <td style="text-align: left">
                            <asp:TextBox ID="txtRemark" runat="server" Width="200px" MaxLength="100"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <req>Active :</req>
                        </td>
                        <td style="text-align: left">
                            <obout:oboutradiobutton id="rbtnYes" runat="server" text="Yes" groupname="rbtnActive"
                                checked="true">
                            </obout:oboutradiobutton>
                            <obout:oboutradiobutton id="rbtnNo" runat="server" text="No" groupname="rbtnActive">
                            </obout:oboutradiobutton>
                        </td>
                    </tr>
                </table>
                <asp:HiddenField ID="hdnContactTypeID" runat="server" />
                <table class="gridFrame" width="70%">
                    <tr>
                        <td>
                            <table style="width: 100%">
                                <tr>
                                    <td style="text-align: left;">
                                        <a id="headerText">Contact Type List</a>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <obout:grid id="gvContact" runat="server" autogeneratecolumns="false" onselect="gvContact_Select"
                                allowfiltering="true" allowaddingrecords="False" allowgrouping="true" width="100%">
                                <Columns>
                                    <obout:Column ID="Column1" DataField="Edit" runat="Server" Width="1%" AllowFilter="false">
                                        <TemplateSettings TemplateId="ImageBtnEdit" />
                                    </obout:Column>
                                    <obout:Column DataField="ID" HeaderText="ID" Visible="false">
                                    </obout:Column>
                                    <obout:Column DataField="Sequence" HeaderText="Sequence No." Width="1%">
                                    </obout:Column>
                                    <obout:Column DataField="ContactType" HeaderText="Contact Type" Width="2%">
                                    </obout:Column>
                                    <obout:Column DataField="Remark" HeaderText="Remark" Width="4%">
                                    </obout:Column>
                                    <obout:Column DataField="Active" HeaderText="Active" Width="1%">
                                    </obout:Column>
                                </Columns>
                                <Templates>
                                    <obout:GridTemplate runat="server" ID="ImageBtnEdit">
                                        <Template>
                                            <asp:ImageButton ID="imgBtnEdit" runat="server" ImageUrl="../App_Themes/Blue/img/Edit16.png"
                                                ToolTip="Edit" />
                                        </Template>
                                    </obout:GridTemplate>
                                </Templates>
                            </obout:grid>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </center>
</asp:Content>

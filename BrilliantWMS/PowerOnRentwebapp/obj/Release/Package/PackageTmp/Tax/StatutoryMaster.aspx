<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/CRM.Master" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="StatutoryMaster.aspx.cs" 
    Inherits="BrilliantWMS.Tax.StatutoryMaster" Theme ="Blue" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="obout_Grid_NET" Namespace="Obout.Grid" TagPrefix="obout" %>
<%@ Register Assembly="obout_Interface" Namespace="Obout.Interface" TagPrefix="obout" %>
<%@ Register Src="../CommonControls/UCFormHeader.ascx" TagName="UCFormHeader" TagPrefix="uc1" %>
<%@ Register Src="../CommonControls/UCToolbar.ascx" TagName="UCToolbar" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .test tr input
{       border:1px solid red;
        margin-right:5px;
        padding-right:5px;
}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHolder_FormHeader" runat="server">
    <uc2:UCToolbar ID="UCToolbar1" runat="server" />
    <uc1:UCFormHeader ID="UCFormHeader1" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPHolder_Form" runat="server">
      <asp:HiddenField ID="hdncompanyid" runat="server" />
       <asp:HiddenField ID="hdncustomerid" runat="server" />
    <center>
        <asp:UpdateProgress ID="UpdateProgress_StatutoryM" runat="server" AssociatedUpdatePanelID="updPnl_StatutoryM">
            <ProgressTemplate>
                <center>
                    <div class="modal">
                        <img src="../App_Themes/Blue/img/ajax-loader.gif" style="top: 50%;" />
                    </div>
                </center>
            </ProgressTemplate>
        </asp:UpdateProgress>
        <asp:ValidationSummary ID="validationsummary_StatutoryMaster" runat="server" ShowMessageBox="true"
            ShowSummary="false" DisplayMode="bulletlist" ValidationGroup="Save" />
        <asp:UpdatePanel runat="server" ID="updPnl_StatutoryM">
            <ContentTemplate>
                <table class="tableForm">
                   <%-- <tr>
                         <td>
                                <req><asp:Label ID="lblcompany" runat="server" Text="Company"></asp:Label></req>
                                    :
                                </td>
                                <td style="text-align: left">
                                   <asp:DropDownList ID="ddlCompany" ClientIDMode="Static" runat="server" Width="200px"  DataTextField="Name" DataValueField="ID" onchange="GetDepartment()">
                                       <asp:ListItem>Select Company</asp:ListItem>
                                    </asp:DropDownList>
                                     <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ForeColor="Maroon" ControlToValidate="ddlCompany"
                                        InitialValue="0" runat="server" ErrorMessage="Please Select Company" Display="None"></asp:RequiredFieldValidator>
                                </td>
                         <td>
                                <req><asp:Label ID="lblcustomer" runat="server" Text="Customer"></asp:Label></req>
                                    :
                                </td>
                                <td style="text-align: left">
                                   <asp:DropDownList ID="ddlcustomer" ClientIDMode="Static" runat="server" Width="200px"  DataTextField="Name" DataValueField="ID" onchange="onStateChange(this);">
                                       <asp:ListItem>Select Customer</asp:ListItem>
                                    </asp:DropDownList>
                                    
                                </td>
                       
                    </tr>--%>
                    <tr>
                         <td>
                            <req>Group Name :</req>
                        </td>
                        <td style="text-align: left" >
                            <asp:CheckBoxList ID="chkBLstGroupname" runat="server" CssClass="test" RepeatLayout="Table" RepeatColumns="5"
                                RepeatDirection="Horizontal">
                                <asp:ListItem>Company</asp:ListItem>
                                <asp:ListItem>Customer</asp:ListItem>
                                <asp:ListItem>Vendor</asp:ListItem>
                                <asp:ListItem>Employee</asp:ListItem>
                                 <asp:ListItem>Product</asp:ListItem>
                            </asp:CheckBoxList>
                        </td>
                        <td>
                            <req>Statutory Name :</req>
                        </td>
                        <td style="text-align: left">
                            <asp:TextBox ID="txtName" runat="server" Width="198px" MaxLength="50" onKeyPress="return alpha(event);" 
                                ValidationGroup="Save"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="valRftxtName" runat="server" ControlToValidate="txtName"
                                ErrorMessage="Please Enter Statutory Name" Display="None" ValidationGroup="Save"></asp:RequiredFieldValidator>
                        </td>
                       
                    </tr>
                    <tr>
                        <td>
                            <req>Active :</req>
                        </td>
                        <td style="text-align: left">
                            <obout:OboutRadioButton ID="rbtnYes" runat="server" Text="Yes" GroupName="rbtnActive"
                                Checked="true">
                            </obout:OboutRadioButton>
                            <obout:OboutRadioButton ID="rbtnNo" runat="server" Text="No" GroupName="rbtnActive">
                            </obout:OboutRadioButton>
                        </td>
                        <td>
                            Remark :
                        </td>
                        <td style="text-align: left;">
                            <asp:TextBox ID="txtRemark" runat="server" Width="350px" MaxLength="200"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <asp:HiddenField ID="hdnStatutoryID" runat="server" />
                <table class="gridFrame" width="65%">
                    <tr>
                        <td>
                            <table style="width: 100%">
                                <tr>
                                    <td style="text-align: left;">
                                        <a id="headerText">Statutory List</a>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <obout:Grid ID="gvStatutoryM" runat="server" AllowAddingRecords="false" AllowFiltering="true"
                                AllowGrouping="true" AutoGenerateColumns="false" OnSelect="gvStatutoryM_Select"
                                Width="100%" GroupBy="ObjectName">
                                <Columns>
                                    <obout:Column ID="Column1" DataField="Edit" runat="Server" Width="3%" AllowFilter="false">
                                        <TemplateSettings TemplateId="imgBtnEdit1" />
                                    </obout:Column>
                                    <obout:Column DataField="ID" HeaderText="ID" Visible="false">
                                    </obout:Column>
                                    <%-- <obout:Column DataField="Sequence" HeaderText="Sequence No." Width="8%">
                                    </obout:Column>--%>
                                    <obout:Column DataField="ObjectName" HeaderText="Group Name" Width="8%">
                                    </obout:Column>
                                    <obout:Column DataField="Name" HeaderText="Statutory Name" Width="10%">
                                    </obout:Column>
                                    <obout:Column DataField="Remark" HeaderText="Remark" Width="20%">
                                    </obout:Column>
                                    <obout:Column DataField="Active" HeaderText="Active" Width="5%">
                                    </obout:Column>
                                </Columns>
                                <Templates>
                                    <obout:GridTemplate runat="server" ID="imgBtnEdit1">
                                        <Template>
                                            <asp:ImageButton ID="imgBtnEdit" runat="server" ImageUrl="../App_Themes/Blue/img/Edit16.png"
                                                ToolTip="Edit" />
                                        </Template>
                                    </obout:GridTemplate>
                                </Templates>
                            </obout:Grid>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </center>
    
</asp:Content>

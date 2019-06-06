<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/CRM.Master" AutoEventWireup="true" CodeBehind="DocumentTypeMaster.aspx.cs"
    Inherits="BrilliantWMS.Document.DocumentTypeMaster" EnableEventValidation="false" Theme="Blue" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="Obout.Ajax.UI" Namespace="Obout.Ajax.UI.FileUpload" TagPrefix="obout" %>
<%@ Register Assembly="obout_Interface" Namespace="Obout.Interface" TagPrefix="obout" %>
<%@ Register Assembly="obout_Grid_NET" Namespace="Obout.Grid" TagPrefix="obout" %>
<%@ Register Src="../CommonControls/UCFormHeader.ascx" TagName="UCFormHeader" TagPrefix="uc1" %>
<%@ Register Src="../CommonControls/UCToolbar.ascx" TagName="UCToolbar" TagPrefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHolder_FormHeader" runat="server">
    <uc2:UCToolbar ID="UCToolbar1" runat="server" />
    <uc1:UCFormHeader ID="UCFormHeader1" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPHolder_Form" runat="server">
    <center>
        <asp:UpdateProgress ID="Update_Designation" runat="server" AssociatedUpdatePanelID="updPnl_Designation">
            <ProgressTemplate>
                <center>
                    <div class="modal">
                        <img src="../App_Themes/Blue/img/ajax-loader.gif" style="top: 50%;" />
                    </div>
                </center>
            </ProgressTemplate>
        </asp:UpdateProgress>
        <asp:ValidationSummary ID="validationsummary_DesignationM" runat="server" ShowMessageBox="true"
            ShowSummary="false" DisplayMode="bulletlist" ValidationGroup="Save" />
        <asp:UpdatePanel ID="updPnl_Designation" runat="server">
            <ContentTemplate>
                <asp:HiddenField ID="hdnCompanyid" runat="server" ClientIDMode="Static"></asp:HiddenField>
                <asp:HiddenField ID="hdncustomerid" runat="server" ClientIDMode="Static"></asp:HiddenField>
                <asp:HiddenField ID="hdnstate" runat="server" ClientIDMode="Static"></asp:HiddenField>
                <asp:HiddenField ID="hdndocumenttypeID" runat="server" ClientIDMode="Static"></asp:HiddenField>
                <table class="tableForm">
                    <tr>
                        <td>
                            <req>
                                    <asp:Label Id="lblcompanymain" runat="server" Text="Company"/>
                                    </req>
                            :
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlcompany" ClientIDMode="Static" DataTextField="Name" DataValueField="ID" runat="server" Width="206px" onchange="GetCustomer()">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ForeColor="Maroon" ControlToValidate="ddlcompany"
                                InitialValue="0" runat="server" ValidationGroup="Save" ErrorMessage="Please Select Company" Display="None"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <req>
                                    <asp:Label Id="lblCustomer" runat="server" Text="Customer"/>
                                    </req>
                            :
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlcustomer" ClientIDMode="Static" DataTextField="Name" DataValueField="ID" runat="server" Width="206px" onchange=" GetCustomerID()">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ForeColor="Maroon" ControlToValidate="ddlcustomer"
                                InitialValue="0" runat="server" ValidationGroup="Save" ErrorMessage="Please Select Customer" Display="None"></asp:RequiredFieldValidator>
                        </td>
                    </tr>

                    <tr>
                        <td>
                            <req>
                                    <asp:Label Id="lbldoctype" runat="server" Text="Document Type"/>
                                    </req>
                            :
                        </td>
                        <td style="text-align: left">
                            <asp:TextBox ID="txtdoctype" runat="server" Width="200px" MaxLength="50" onKeyPress="return alpha(this,event);"
                                ValidationGroup="Save"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtdoctype"
                                ErrorMessage="Please Enter Document Type" Display="None" ValidationGroup="Save"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <req>
                                    <asp:Label Id="lbldescription" runat="server" Text="Description"/>
                                    </req>
                            :
                        </td>
                        <td style="text-align: left">
                            <asp:TextBox ID="txtDescription" runat="server" Width="200px" MaxLength="50" onKeyPress="return alpha(this,event);"
                                ValidationGroup="Save"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="valRftxtDesignation" runat="server" ControlToValidate="txtDescription"
                                ErrorMessage="Please Enter Description" Display="None" ValidationGroup="Save"></asp:RequiredFieldValidator>
                        </td>

                    </tr>
                    <tr>
                        <td>Sequence No.:
                        </td>
                        <td style="text-align: left">
                            <asp:TextBox ID="txtSequence" runat="server" Width="70px" MaxLength="3" onkeypress="return AllowInt(this, event);"></asp:TextBox>
                        </td>
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
                    </tr>
                    <tr>
                    </tr>
                </table>
                <asp:HiddenField ID="hdnDesignationID" runat="server" />
                <table class="gridFrame" width="80%">
                    <tr>
                        <td>
                            <table style="width: 100%">
                                <tr>
                                    <td style="text-align: left;">
                                        <a id="headerText">Document Type </a>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <obout:Grid ID="grddocument" runat="server" AllowAddingRecords="false" AllowFiltering="True"
                                AllowGrouping="true" AutoGenerateColumns="false" OnSelect="grddocument_Select" Width="100%">
                                <Columns>
                                    <obout:Column ID="Column1" DataField="Edit" runat="Server" Width="1%" AllowFilter="false" Index="0" TemplateId="imgBtnEdit1">
                                        <TemplateSettings TemplateId="imgBtnEdit1" />
                                    </obout:Column>
                                    <obout:Column DataField="ID" HeaderText="ID" Visible="false">
                                    </obout:Column>
                                    <obout:Column DataField="Sequence" HeaderText="Sequence" Align="center" Width="3%" Visible="false">
                                    </obout:Column>
                                    <obout:Column DataField="DocumentType" HeaderText="Document Type" Width="4%">
                                    </obout:Column>
                                    <obout:Column DataField="Description" HeaderText="Description" Width="5%">
                                    </obout:Column>
                                    <obout:Column DataField="Company" HeaderText="Company" Width="3%">
                                    </obout:Column>
                                    <obout:Column DataField="Customer" HeaderText="Customer" Width="3%">
                                    </obout:Column>
                                    <obout:Column DataField="Active" HeaderText="Active" Width="2%">
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
            </ContentTemplate>
        </asp:UpdatePanel>
    </center>
    <script type="text/javascript">
        var ddlcompany = document.getElementById("<%=ddlcompany.ClientID %>");
        var ddlcustomer = document.getElementById("<%=ddlcustomer.ClientID %>");

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
</asp:Content>

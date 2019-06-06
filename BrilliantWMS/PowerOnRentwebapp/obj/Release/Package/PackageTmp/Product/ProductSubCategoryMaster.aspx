<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/CRM.Master" AutoEventWireup="true"
    EnableEventValidation="false" CodeBehind="ProductSubCategoryMaster.aspx.cs" Inherits="BrilliantWMS.Product.ProductSubCategoryMaster" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="obout_Interface" Namespace="Obout.Interface" TagPrefix="obout" %>
<%@ Register Assembly="obout_Grid_NET" Namespace="Obout.Grid" TagPrefix="obout" %>
<%@ Register Src="../CommonControls/UCFormHeader.ascx" TagName="UCFormHeader" TagPrefix="uc1" %>
<%@ Register Src="../CommonControls/UCToolbar.ascx" TagName="UCToolbar" TagPrefix="uc2" %>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHolder_FormHeader" runat="server">
    <uc2:UCToolbar ID="UCToolbar1" runat="server" />
    <uc1:UCFormHeader ID="UCFormHeader1" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPHolder_Form" runat="server">
    <asp:HiddenField ID="hdncompanyid" runat="server" />
    <asp:HiddenField ID="hdncustomerid" runat="server"  ClientIDMode="Static"/>
    <asp:HiddenField ID="hdncategoryid" runat="server"  ClientIDMode="Static"/>
    <center>
        <asp:UpdateProgress ID="UpdateProgress_PrdSubCategoryM" runat="server" AssociatedUpdatePanelID="updPnl_PrdSubCategoryM">
            <ProgressTemplate>
                <center>
                    <div class="modal">
                        <img src="../App_Themes/Blue/img/ajax-loader.gif" style="top: 50%;" />
                    </div>
                </center>
            </ProgressTemplate>
        </asp:UpdateProgress>
        <asp:ValidationSummary ID="validationsummary_ProductSubCatMaster" runat="server"
            ShowMessageBox="true" ShowSummary="false" DisplayMode="bulletlist" ValidationGroup="Save" />
        <asp:UpdatePanel ID="updPnl_PrdSubCategoryM" runat="server">
            <ContentTemplate>
                <table class="tableForm">

                                       <tr>
                                            <td>
                                                    <req> <asp:Label Id="lblcompanymain" runat="server" Text="Company"/> </req> :
                                                </td>
                                                <td style="text-align: left">
                                                   <asp:DropDownList ID="ddlcompanymain" ClientIDMode="Static" runat="server" Width="206px" DataTextField="Name" DataValueField="ID" onchange="GetCustomer()">
                                                  </asp:DropDownList>
                                                  <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ForeColor="Maroon" ControlToValidate="ddlcompanymain"
                                                   InitialValue="0" runat="server" ErrorMessage="Please Select Company" Display="None" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                                </td>
                                                <td>
                                                    <req><asp:Label Id="lblcustomer" runat="server" Text="Cusomer"/></req>
                                                    :
                                                </td>
                                                <td style="text-align: left">
                                                   <asp:DropDownList ID="ddlcustomer" runat="server" ValidationGroup="Save" Width="206px"
                                                        DataTextField="Name" DataValueField="ID" onchange="Getdeptid()"> 
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="RFVddlCompany" runat="server" ErrorMessage="Please Select Customer"
                                                        ControlToValidate="ddlcustomer" InitialValue="0" Display="None" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                                </td>
                                               
                                            </tr>

                    <tr>
                        <td>
                            <req>Product Category :</req>
                        </td>
                        <td style="text-align: left">
                            <asp:DropDownList ID="ddlPrdCategory" runat="server" Width="206px" DataTextField="Name" DataValueField="ID" onchange="GetCategory()"
                                ValidationGroup="Save">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="valRfddlPrdCategory" runat="server" ErrorMessage="Please Select Category"
                                ControlToValidate="ddlPrdCategory" InitialValue="0" Display="None" ValidationGroup="Save">
                            </asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <req>Product Sub-Category :</req>
                        </td>
                        <td style="text-align: left">
                            <asp:TextBox ID="txtPrdSubCategory" runat="server" Width="206px" MaxLength="50" onKeyPress="return alpha(event);"
                                ValidationGroup="Save"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="valRftxtPrdSubCategory" runat="server" ControlToValidate="txtPrdSubCategory"
                                ErrorMessage="Enter Product SubCategory" Display="None" ValidationGroup="Save"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                         <td>
                            Sequence No.:
                        </td>
                        <td style="text-align: left">
                            <asp:TextBox ID="txtSequence" runat="server" Width="70px" MaxLength="3" onkeypress="return fnAllowDigitsOnly(event)"></asp:TextBox>
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
                </table>
                <asp:HiddenField ID="hdnPrdSubCategoryID" runat="server" />
                <table class="gridFrame" width="70%">
                    <tr>
                        <td>
                            <table style="width: 100%">
                                <tr>
                                    <td style="text-align: left;">
                                        <a id="headerText">Product Sub-Category List</a>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <obout:Grid ID="gvPrdSubCategoryM" runat="server" AllowAddingRecords="false" AllowFiltering="true"
                                AllowGrouping="true" AutoGenerateColumns="false" OnSelect="gvPrdSubCategoryM_Select"
                                Width="100%">
                                <Columns>
                                    <obout:Column ID="Column1" DataField="Edit" runat="Server" Width="1%" AllowFilter="false">
                                        <TemplateSettings TemplateId="imgBtnEdit1" />
                                    </obout:Column>
                                    <obout:Column DataField="ID" HeaderText="ID" Visible="false">
                                    </obout:Column>
                                    <obout:Column DataField="Sequence" HeaderText="Sequence No." Width="1%" Visible="false">
                                    </obout:Column>
                                    <obout:Column DataField="Customer" HeaderText="Customer" Width="3%">
                                    </obout:Column>
                                    <obout:Column DataField="PrdCategoryName" HeaderText="Product Category" Width="2%">
                                    </obout:Column>
                                    <obout:Column DataField="Name" HeaderText="Product Sub-Category" Width="3%">
                                    </obout:Column>
                                    <obout:Column DataField="Active" HeaderText="Active" Width="1%">
                                    </obout:Column>
                                    <obout:Column DataField="Companyid" HeaderText="Companyid" Width="1%" Visible="false">
                                    </obout:Column>
                                    <obout:Column DataField="CustomerID" HeaderText="CustomerID" Width="1%" Visible="false">
                                    </obout:Column>
                                     <obout:Column DataField="ProductCategoryID" HeaderText="ProductCategoryID" Width="1%" Visible="false">
                                    </obout:Column>
                                </Columns>
                                <Templates>
                                    <obout:GridTemplate runat="server" ID="imgBtnEdit1">
                                        <Template>
                                            <asp:ImageButton ID="imgBtnEdit" runat="server" CausesValidation="false" ImageUrl="../App_Themes/Blue/img/Edit16.png"
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
    <script type="text/javascript">
        var ddlcompany = document.getElementById("<%=ddlcompanymain.ClientID %>");
        var ddlcustomer = document.getElementById("<%=ddlcustomer.ClientID %>");
        var category = document.getElementById("<%=ddlPrdCategory.ClientID %>"); 

        function GetCustomer() {
            var obj1 = new Object();
            obj1.ddlcompanyId = ddlcompany.value.toString();
            document.getElementById("<%=hdncompanyid.ClientID %>").value = ddlcompany.value.toString();
            PageMethods.GetCustomer(obj1, getLoc_onSuccessed);
        }

        function getLoc_onSuccessed(result) {

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

        function Getdeptid() {
            var obj1 = new Object();
            obj1.ddlcustomer = ddlcustomer.value.toString();
            document.getElementById("<%=hdncustomerid.ClientID %>").value = ddlcustomer.value.toString();
            PageMethods.GetCategory(obj1, getCategory_onSuccessed);
        }

        function getCategory_onSuccessed(result) {

            category.options.length = 0;
            for (var i in result) {
                AddOption1(result[i].Name, result[i].Id);
            }
        }

        function AddOption1(text, value) {

            var option = document.createElement('option');
            option.value = value;
            option.innerHTML = text;
            category.options.add(option);
        }

        function GetCategory()
        {
            document.getElementById("<%=hdncategoryid.ClientID %>").value = category.value.toString();
        }

        function fnAllowDigitsOnly(key) {
            var keycode = (key.which) ? key.which : key.keyCode;
            if ((keycode < 48 || keycode > 57) && (keycode != 8) && (keycode != 9) && (keycode != 11) && (keycode != 16) && (keycode != 37) && (keycode != 38) && (keycode != 39) && (keycode != 40) && (keycode != 219) && (keycode != 127)) {
                return false;
            }
        }
  </script>
</asp:Content>

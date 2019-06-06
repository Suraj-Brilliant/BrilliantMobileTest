<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/CRM.Master" AutoEventWireup="true"
    EnableEventValidation="false" CodeBehind="TaxMaster.aspx.cs" Inherits="BrilliantWMS.Tax.TaxMaster" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="obout_Flyout2_NET" Namespace="OboutInc.Flyout2" TagPrefix="obout" %>
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
     <asp:UpdateProgress ID="UpdatetermProcess" runat="server" AssociatedUpdatePanelID="updPnl_TaxM">
        <ProgressTemplate>
            <center>
                <div class="modal">
                    <img src="../App_Themes/Blue/img/ajax-loader.gif" style="top: 50%;" />
                </div>
            </center>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:ValidationSummary ID="validationsummary_TaxMaster" runat="server" ShowMessageBox="true"
        ShowSummary="false" DisplayMode="bulletlist" ValidationGroup="Save" />
    <center>
        <asp:UpdatePanel ID="updPnl_TaxM" runat="server">
            <ContentTemplate>
                <asp:HiddenField ID="hdnCompanyid" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hdncustomerid" runat="server" ClientIDMode="Static" />
                <table class="tableForm">
                    <tr>
                        <td>
                                    <req>
                                    <asp:Label Id="lblcompanymain" runat="server" Text="Company"/></req> :
                                </td>
                                <td  style="text-align: left">
                                    <asp:DropDownList ID="ddlcompanymain" ClientIDMode="Static" runat="server" DataTextField="Name" DataValueField="ID" Width="206px" onchange="GetCustomer()">
                                        <asp:ListItem>Select</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ForeColor="Maroon" ControlToValidate="ddlcompanymain"
                                        InitialValue="0" runat="server" ErrorMessage="Please Select Company" Display="None" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                </td>
                                <td>
                                    <req>
                                    <asp:Label Id="lblCustomer" runat="server" Text="Customer"/></req>:
                                </td>
                                <td  style="text-align: left">
                                    <asp:DropDownList ID="ddlcustomer" ClientIDMode="Static" runat="server" ataTextField="Name" DataValueField="ID" Width="206px" onchange="GetCustomerID()">
                                         <asp:ListItem>Select</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ForeColor="Maroon" ControlToValidate="ddlcustomer"
                                        InitialValue="0" runat="server" ErrorMessage="Please Select Company" Display="None" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                </td>
                    </tr>
                    <tr>
                        <td>
                            <req>Tax Type :</req>
                        </td>
                        <td style="text-align: left">
                            <asp:DropDownList ID="ddlTaxType" runat="server" Width="205px" ValidationGroup="Save"
                                onchange="seeButton(this);" AutoPostBack="false">
                                <Items>
                                    <asp:ListItem>-Select-</asp:ListItem>
                                    <asp:ListItem>Tax On Principal</asp:ListItem>
                                    <asp:ListItem>Tax On Tax</asp:ListItem>
                                </Items>
                                <%--<clientsideevents onselectedindexchanged="seeButton" />--%>
                            </asp:DropDownList>
                            <img id="btnTax" src="../App_Themes/Blue/img/TaxCalculation16.png" onclick="<%=FlyoutTaxMapping.getClientID()%>.Open();gvTaxMappingM.refresh();"
                                style="cursor: pointer; visibility: hidden" />
                            <asp:RequiredFieldValidator ID="valRefddlTaxType" runat="server" ErrorMessage="Please Select Tax Type"
                                ControlToValidate="ddlTaxType" InitialValue="-Select-" Display="None" ValidationGroup="Save"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            Sequence No.:
                        </td>
                        <td style="text-align: left">
                            <asp:TextBox ID="txtSequence" runat="server" Width="100px" MaxLength="3" onkeypress="return AllowInt(this, event);"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <req>Tax Name :</req>
                        </td>
                        <td style="text-align: left">
                            <asp:TextBox ID="txtTaxName" runat="server" Width="200px" MaxLength="10" ValidationGroup="Save"
                                Onblur="return CheckSpace();" onKeyPress="return alpha(event);"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="valReftxtTaxName" runat="server" ControlToValidate="txtTaxName"
                                ErrorMessage="Please Enter Tax Name" Display="None" ValidationGroup="Save"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <req>Tax Percent :</req>
                        </td>
                        <td style="text-align: left">
                            <asp:TextBox ID="txtTaxPercent" runat="server" Width="100px" AutoCompleteType="None"
                                onkeyup="checkPercentage();" MaxLength="6" ValidationGroup="Save" Style="text-align: right;"
                                onkeydown="return AllowDecimal(this,event);checkPercentage();" onkeypress="return AllowDecimal(this,event);checkPercentage();"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="valReftxtTaxPercente" runat="server" ControlToValidate="txtTaxPercent"
                                ErrorMessage="Please Enter Tax Percent" Display="None" ValidationGroup="Save"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Description:
                        </td>
                        <td style="text-align: left">
                            <asp:TextBox ID="txtDescription" runat="server" Width="200px" MaxLength="50"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Label ID="lblTaxMapping" runat="server" Text="Tax Mapping"></asp:Label>
                        </td>
                        <td style="text-align: left">
                            <asp:Label ID="txtTaxMappingID1" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <req>Active :</req>
                        </td>
                        <td style="text-align: left" colspan="2">
                            <obout:OboutRadioButton ID="rbtnYes" runat="server" Text="Yes" GroupName="rbtnActive"
                                Checked="true">
                            </obout:OboutRadioButton>
                            <obout:OboutRadioButton ID="rbtnNo" runat="server" Text="No" GroupName="rbtnActive">
                            </obout:OboutRadioButton>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                    </tr>
                </table>
                <table class="gridFrame">
                    <tr>
                        <td>
                            <table style="width: 100%">
                                <tr>
                                    <td style="text-align: left;">
                                        <a id="headerText">Tax List</a>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <obout:Grid ID="gvTaxM" runat="server" AllowAddingRecords="false" AllowFiltering="true"
                                AllowGrouping="true" AutoGenerateColumns="false" OnSelect="gvTaxM_Select">
                                <Columns>
                                    <obout:Column ID="Column1" HeaderText="Edit" DataField="Edit" ShowHeader="false" runat="Server" Width="75px"
                                        AllowFilter="false" TemplateId="imgBtnEdit1">
                                        <TemplateSettings TemplateId="imgBtnEdit1" />
                                    </obout:Column>
                                    <obout:Column DataField="ID" HeaderText="ID" Visible="false">
                                    </obout:Column>
                                    <obout:Column DataField="Sequence" HeaderText="Sequence No." Width="100px" Visible="false">
                                    </obout:Column>
                                    <obout:Column DataField="Type" HeaderText="Tax Type" Width="150px">
                                    </obout:Column>
                                    <obout:Column DataField="Name" HeaderText="Tax Name" Width="150px">
                                    </obout:Column>
                                    <obout:Column DataField="Description" HeaderText="Description" Width="150px">
                                    </obout:Column>
                                    <obout:Column DataField="Percent" Width="150px" Align="right">
                                        <TemplateSettings HeaderTemplateId="HeaderTempTaxPercent" />
                                    </obout:Column>
                                    <obout:Column DataField="Customer" HeaderText="Customer" Width="150px">
                                    </obout:Column>
                                    <obout:Column DataField="Active" HeaderText="Active" Width="80px">
                                    </obout:Column>
                                     <obout:Column DataField="CompanyID" HeaderText="CompanyID" Width="80px" Visible="false">
                                    </obout:Column>
                                     <obout:Column DataField="CustomerID" HeaderText="CustomerID" Width="80px" Visible="false">
                                    </obout:Column>
                                </Columns>
                                <Templates>
                                    <obout:GridTemplate runat="server" ID="imgBtnEdit1">
                                                        <Template>
                                                            <asp:ImageButton ID="imgBtnEdit" runat="server" ImageUrl="../App_Themes/Blue/img/Edit16.png"
                                                                ToolTip="Edit" CausesValidation="false" />
                                                        </Template>
                                      </obout:GridTemplate>
                                    <%--<obout:GridTemplate runat="server" ID="imgBtnEdit12">
                                        <Template>
                                            <asp:ImageButton ID="imgBtnEdit" runat="server" ImageUrl="../App_Themes/Blue/img/Edit16.png"
                                                OnClick="imgBtnEdit_Click" ToolTip="Edit" />
                                        </Template>
                                    </obout:GridTemplate>--%>
                                    <obout:GridTemplate ID="HeaderTempTaxPercent" runat="server">
                                        <Template>
                                            &nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp Tax Percent
                                        </Template>
                                    </obout:GridTemplate>
                                </Templates>
                            </obout:Grid>
                        </td>
                    </tr>
                </table>
                <obout:Flyout runat="server" ID="FlyoutTaxMapping" AttachTo="btnTax" OpenEvent="NONE"
                    CloseEvent="NONE" IsModal="true" PageColor="Black" PageOpacity="60" zIndex="999"
                    Position="ABSOLUTE" RelativeLeft="-100" RelativeTop="10">
                    <asp:UpdatePanel ID="UpdatePanelTaxMapping" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div id="flyoutDiv">
                                <table class="gridFrame">
                                    <tr>
                                        <td>
                                            <obout:DragPanel ID="DragPanel1" runat="server">
                                                <table style="width: 100%">
                                                    <tr>
                                                        <td style="text-align: left;">
                                                            <a class="headerText">Principal List</a>
                                                        </td>
                                                        <td>
                                                            <div style="float: right;">
                                                                <input type="button" value="Submit" id="btnSubmit" onclick="selectRecord();" />
                                                                <input type="button" value="Close" id="btnCancel" onclick="<%=FlyoutTaxMapping.getClientID()%>.Close();" />
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </obout:DragPanel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <obout:Grid ID="gvTaxMappingM" runat="server" AllowAddingRecords="false" AllowFiltering="true"
                                                AllowGrouping="true" AutoGenerateColumns="false" AllowMultiRecordSelection="true"
                                                OnRebind="RebindGrid" CallbackMode="true">
                                                <ClientSideEvents ExposeSender="true" />
                                                <Columns>
                                                    <obout:CheckBoxSelectColumn ShowHeaderCheckBox="true" ControlType="Obout">
                                                    </obout:CheckBoxSelectColumn>
                                                    <obout:Column DataField="ID" Visible="false">
                                                    </obout:Column>
                                                    <obout:Column DataField="Type" HeaderText="Tax Type" Width="150px">
                                                    </obout:Column>
                                                    <obout:Column DataField="Name" HeaderText="Tax Name" Width="150px">
                                                    </obout:Column>
                                                    <obout:Column DataField="Percent" HeaderText="Tax Percent" Width="150px" Align="right">
                                                    </obout:Column>
                                                    <obout:Column DataField="Active" HeaderText="Active" Width="100px">
                                                    </obout:Column>
                                                </Columns>
                                            </obout:Grid>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </obout:Flyout>
                <asp:HiddenField ID="hdnTaxID" runat="server" />
                <asp:HiddenField ID="hdnTaxIDs" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </center>
    <script type="text/javascript">
        function seeButton(sender) 
        {
        
            var index = sender.selectedIndex;
            var btn = document.getElementById('btnTax');
            var txtTaxMappingID1 = document.getElementById("<%= txtTaxMappingID1.ClientID %>");
        var hdnTaxIDs = document.getElementById("<%= hdnTaxIDs.ClientID %>");
            var lblTaxMapping = document.getElementById("<%= lblTaxMapping.ClientID %>");
            btn.style.visibility = "hidden";
            hdnTaxIDs.value = "";
            lblTaxMapping.innerHTML = "";
            txtTaxMappingID1.innerHTML = "";
            if (index == 2) 
            {
                btn.style.visibility = "visible";
                txtTaxMappingID1.innerHTML = "";
                lblTaxMapping.innerHTML="Tax Mapping :";
            }   
        }   
             
        function selectRecord() {
            var hdnTaxIDs = document.getElementById("<%= hdnTaxIDs.ClientID %>");
            var txtTaxMappingID1 = document.getElementById("<%= txtTaxMappingID1.ClientID %>");

            hdnTaxIDs.value = "";
            txtTaxMappingID1.innerHTML = "";

            if (gvTaxMappingM.SelectedRecords.length > 0) {
                for (var i = 0; i < gvTaxMappingM.SelectedRecords.length; i++) {
                    var record = gvTaxMappingM.SelectedRecords[i];
                    if (hdnTaxIDs.value != "") {
                        hdnTaxIDs.value += ',' + record.ID;
                       
                        txtTaxMappingID1.innerHTML += '/' + record.Name;
                    }
                    if (hdnTaxIDs.value == "") {
                        hdnTaxIDs.value = record.ID;
                       
                        txtTaxMappingID1.innerHTML= record.Name;
                    }
                }
                gvTaxMappingM.refresh();
                gvTaxMappingM.SelectedRecords=false;
                <%=FlyoutTaxMapping.getClientID()%>.Close();
            }
            if (gvTaxMappingM.SelectedRecords.length == 0) { /*dhtmlx.alert({ type: 'alert-error', title: 'Product not selected', text: 'Select atleast one product' });*/alert('Select atleast one Record of Tax'); }
        }

        function checkPercentage() 
        {
            var txtTaxPercent = document.getElementById("<%= txtTaxPercent.ClientID  %>"); 
             if (txtTaxPercent.value != "") { 
                 if (parseFloat(txtTaxPercent.value) > 100.00) {
                     txtTaxPercent.value = "";
                     alert("Enter valid Tax Percent");
                 }
             }
         }
            
        
         function CheckSpace()
         {var txtTaxName=document.getElementById('<%=txtTaxName.ClientID %>');
         if(txtTaxName.value.indexOf(' ')>-1)
         { txtTaxName.value=''; alert('No space');  return false;}  
         else
         {return true;}}

    </script>

     <script type="text/javascript">
         var ddlcompany = document.getElementById("<%=ddlcompanymain.ClientID %>");
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

<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/CRM.Master" AutoEventWireup="true" EnableEventValidation="false"
     CodeBehind="TermsAndConditionMaster.aspx.cs" Inherits="BrilliantWMS.Company.TermsAndConditionMaster" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="obout_Grid_NET" Namespace="Obout.Grid" TagPrefix="obout" %>
<%@ Register Assembly="obout_Interface" Namespace="Obout.Interface" TagPrefix="obout" %>
<%@ Register Src="../CommonControls/UCFormHeader.ascx" TagName="UCFormHeader" TagPrefix="uc1" %>
<%@ Register Src="../CommonControls/UCToolbar.ascx" TagName="UCToolbar" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <style type="text/css">
        .test tr input
{       border:1px solid red;
        margin-right:4px;
        padding-right:4px;
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
     <asp:UpdateProgress ID="UpdatetermProcess" runat="server" AssociatedUpdatePanelID="updPnl_TermsCondition">
        <ProgressTemplate>
            <center>
                <div class="modal">
                    <img src="../App_Themes/Blue/img/ajax-loader.gif" style="top: 50%;" />
                </div>
            </center>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:ValidationSummary ID="validationsummary_TermConditionMaster" runat="server"
        ShowMessageBox="true" ShowSummary="false" DisplayMode="bulletlist" ValidationGroup="Save" />
    <center>
        <asp:UpdatePanel ID="updPnl_TermsCondition" runat="server">
            <ContentTemplate>
                <table class="tableForm">
                    <tr>
                        <td>
                                <req><asp:Label ID="lblcompany" runat="server" Text="Company"></asp:Label></req>
                                    :
                                </td>
                                <td style="text-align: left">
                                   <asp:DropDownList ID="ddlCompany" ClientIDMode="Static"  DataTextField="Name" DataValueField="ID" runat="server" Width="200px" onchange="GetDepartment()">
                                       <asp:ListItem>Select Company</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ForeColor="Maroon" ControlToValidate="ddlCompany"
                                        InitialValue="0" runat="server" ErrorMessage="Please Select Company" Display="None"></asp:RequiredFieldValidator>
                                    
                                </td>
                                <td>
                                    <req><asp:Label Id="lblcustomer" runat="server" Text="Customer"></asp:Label></req>
                                    :
                                </td>
                                <td style="text-align: left">
                                    <asp:DropDownList ID="ddlcutomer" runat="server"  DataTextField="Name" DataValueField="ID" Width="206px" onchange="Getdeptid()">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RfValCompany" runat="server" ControlToValidate="ddlcutomer" InitialValue="0"
                                        Display="None" ErrorMessage="Please Select Customer" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                </td>
                    </tr>
                    <tr>
                         <td>
                            <req>Group Name :</req>
                        </td>
                        <td style="text-align: left">
                            <asp:CheckBoxList ID="chkBLstGroupname" runat="server" DataTextField="Value"
                                DataValueField="Id" ValidationGroup="Save" RepeatLayout="Table" RepeatColumns="8"
                                RepeatDirection="Horizontal">
                            </asp:CheckBoxList>
                           <%-- <asp:CheckBoxList ID="chkBLstGroupname12" runat="server" CssClass="test" RepeatLayout="Table" RepeatColumns="5"
                                RepeatDirection="Horizontal">
                                <asp:ListItem>PO</asp:ListItem>
                                <asp:ListItem>SO</asp:ListItem>
                                <asp:ListItem>Credit Note</asp:ListItem>
                                <asp:ListItem>Debit Note</asp:ListItem>
                                 <asp:ListItem>Invoice</asp:ListItem>
                            </asp:CheckBoxList>--%>
                        </td>
                        <td>
                            <req>Term Name :</req>
                        </td>
                        <td style="text-align: left">
                            <asp:TextBox ID="txtTermName" runat="server" Width="200px" MaxLength="50" onkeypress="return alpha(event);"
                                ValidationGroup="Save"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="valRftxtTermName" runat="server" ControlToValidate="txtTermName"
                                ErrorMessage="Please Enter Term Name" Display="None" ValidationGroup="Save"></asp:RequiredFieldValidator>
                        </td>
                        
                    </tr>
                    <tr>
                        <td>
                            <req>Condition :</req>
                        </td>
                        <td rowspan="2">
                            <asp:TextBox ID="txtCondition" runat="server" Width="400px"  TextMode="MultiLine" onkeyup="TextBox_KeyUp(this,'CharactersCounter1','2000');"></asp:TextBox>
                             <br />
                             <span class="watermark"><span id="CharactersCounter1">2000</span> characters remaining
                            out of 2000 </span>
                            <asp:RequiredFieldValidator ID="valRftxtCondition" runat="server" ControlToValidate="txtCondition"
                                ErrorMessage="Please Enter Condition" Display="None" ValidationGroup="Save"></asp:RequiredFieldValidator>
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
                        <td>
                        </td>
                    </tr>
                </table>
                <asp:HiddenField ID="hdnTermID" runat="server" />
                <table class="gridFrame" width="80%">
                    <tr>
                        <td>
                            <table style="width: 100%">
                                <tr>
                                    <td style="text-align: left;">
                                        <a id="headerText">Terms and Conditions List</a>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <obout:Grid ID="gvTerm" runat="server" AllowAddingRecords="false" AllowFiltering="true"
                                AllowGrouping="true" AutoGenerateColumns="false" OnSelect="gvTerm_Select" Width="100%">
                                <Columns>
                                    <obout:Column ID="Column1" HeaderText="Edit" ShowHeader="false" runat="Server" Width="2%"
                                        AllowFilter="false">
                                        <TemplateSettings TemplateId="imgBtnEdit1" />
                                    </obout:Column>
                                    <obout:Column DataField="ID" HeaderText="ID" Visible="false">
                                    </obout:Column>
                                    <%-- <obout:Column DataField="Sequence" HeaderText="Sequence No." Width="4%">
                                    </obout:Column>--%>
                                    <obout:Column DataField="Groupname" HeaderText="Group Name" Width="4%">
                                    </obout:Column>
                                    <obout:Column DataField="Term" HeaderText="Term Name" Width="6%">
                                    </obout:Column>
                                    <obout:Column DataField="Condition" HeaderText="Condition" Width="12%" Wrap="true">
                                    </obout:Column>
                                    <obout:Column DataField="Company" HeaderText="Company" Width="6%">
                                    </obout:Column>
                                    <obout:Column DataField="Customer" HeaderText="Customer" Width="6%">
                                    </obout:Column>
                                    <obout:Column DataField="Active" HeaderText="Active" Width="2%">
                                    </obout:Column>
                                    <obout:Column DataField="CompanyID" HeaderText="CompanyID" Width="2%" Visible="false">
                                    </obout:Column>
                                    <obout:Column DataField="CustomerID" HeaderText="CustomerID" Width="2%" Visible="false">
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
        var ddlcompany = document.getElementById("<%=ddlCompany.ClientID %>");
        var ddldeptid = document.getElementById("<%=ddlcutomer.ClientID %>");

        function GetDepartment() {
            var obj1 = new Object();
            obj1.ddlcompanyId = ddlcompany.value.toString();
            document.getElementById("<%=hdncompanyid.ClientID %>").value = ddlcompany.value.toString();
            PageMethods.GetDepartment(obj1, getLoc_onSuccessed);
        }

        function getLoc_onSuccessed(result) {

            ddldeptid.options.length = 0;
            for (var i in result) {
                AddOption(result[i].Name, result[i].Id);
            }
        }

        function AddOption(text, value) {

            var option = document.createElement('option');
            option.value = value;
            option.innerHTML = text;
            ddldeptid.options.add(option);
        }

        function Getdeptid() {
            document.getElementById("<%=hdncustomerid.ClientID %>").value = ddldeptid.value.toString();
         }
  </script>
</asp:Content>

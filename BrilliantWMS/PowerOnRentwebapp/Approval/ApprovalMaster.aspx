<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/CRM.Master" AutoEventWireup="true" CodeBehind="ApprovalMaster.aspx.cs"
    Inherits="BrilliantWMS.Approval.ApprovalMaster" Theme="Blue" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="Obout.Ajax.UI" Namespace="Obout.Ajax.UI.FileUpload" TagPrefix="obout" %>
<%@ Register Assembly="obout_Interface" Namespace="Obout.Interface" TagPrefix="obout" %>
<%@ Register Assembly="obout_Grid_NET" Namespace="Obout.Grid" TagPrefix="obout" %>
<%@ Register Src="../CommonControls/UCToolbar.ascx" TagName="UCToolbar" TagPrefix="uc1" %>
<%@ Register Src="../CommonControls/UCFormHeader.ascx" TagName="UCFormHeader" TagPrefix="uc2" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHolder_FormHeader" runat="server">
   <uc1:UCToolbar ID="UCToolbar1" runat="server" />
    <uc2:UCFormHeader ID="UCFormHeader1" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPHolder_Form" runat="server">
    <script src="../MasterPage/JavaScripts/countries.js" type="text/javascript"></script>
    <asp:ValidationSummary ID="validationsummary_AccountMaster" runat="server" ShowMessageBox="true"
        ShowSummary="false" DisplayMode="bulletlist" ValidationGroup="Save" />
    <%--<asp:UpdatePanel ID="UpdatePanelTabPanelCompanyList" runat="server">--%>
    <%-- <ContentTemplate>--%>
    <center>
        <asp:TabContainer runat="server" ID="tabApprovalLevelMaster" ActiveTabIndex="0" Width="100%">
            <asp:TabPanel ID="tabapprovallist" runat="server" HeaderText="Approval List" TabIndex="0">
                <ContentTemplate>
                    <%-- <asp:UpdateProgress ID="UpdateProgressUC_CustomerList" runat="server" AssociatedUpdatePanelID="upnl_customerlist">
                            <ProgressTemplate>
                                <center>
                                    <div class="modal">
                                        <img src="../App_Themes/Blue/img/ajax-loader.gif" style="top: 50%;" />
                                    </div>
                                </center>
                            </ProgressTemplate>
                        </asp:UpdateProgress>--%>
                    <%-- <asp:UpdatePanel ID="upnl_customerlist" runat="server">
                            <ContentTemplate>--%>
                    <asp:HiddenField ID="hdnselectedRec" runat="server" ClientIDMode="Static" />
                    <asp:HiddenField ID="hndCompanyid" runat="server"></asp:HiddenField>
                    <asp:HiddenField ID="hndState" runat="server" ClientIDMode="Static"></asp:HiddenField>
                    <asp:HiddenField ID="hdnmodestate" runat="server" ClientIDMode="Static" />
                    <asp:HiddenField ID="hdnCountry" runat="server" ClientIDMode="Static" />
                    <asp:HiddenField ID="hdnState" runat="server" ClientIDMode="Static" />
                    <asp:HiddenField ID="hdnSelectedCompany" runat="server" ClientIDMode="Static" />
                    <asp:HiddenField ID="hdnSelectedDepartment" runat="server" ClientIDMode="Static" />
                    <asp:HiddenField ID="hdnselectedLocation" runat="server"></asp:HiddenField>
                    <asp:HiddenField ID="hdnSearchContactID1" runat="server" ClientIDMode="Static" />
                    <asp:HiddenField ID="hdnSearchContactName1" runat="server" ClientIDMode="Static" />
                    <asp:HiddenField ID="hdnSearchConEmail" runat="server" ClientIDMode="Static" />
                    <asp:HiddenField ID="hdnSearchConMobNo" runat="server" ClientIDMode="Static" />
                     <asp:HiddenField ID="hdDownLoadAccessIDs" runat="server" ViewStateMode="Enabled"  ClientIDMode="Static" />
                      <asp:HiddenField ID="hdnDeleteAccessIDs" runat="server" ViewStateMode="Enabled" ClientIDMode="Static" />
                    <center>
                        <table class="gridFrame" style="width: 100%">
                            <tr>
                                <td>
                                    <table style="width: 100%">
                                        <tr>
                                            <td style="text-align: left;">
                                                <a id="headerText">
                                                    <asp:Label ID="lblheasertext" runat="server" Text="Approval List"></asp:Label></a>
                                            </td>
                                            <td style="text-align: right;">
                                                <%-- <input type="button" value="Book Payment" id="btnPayment" onclick="selectedRecordBookPayment();" />
                                                <input type="button" value="Send Credentials" id="btnSendMail" onclick="selectedRecordSendMail();" />--%>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <obout:Grid ID="gvApprovalLevelM" runat="server" AllowAddingRecords="False" AllowFiltering="True"
                                                AllowGrouping="True" AutoGenerateColumns="False" Width="100%" OnSelect="gvApprovalLevelM_Select">
                                                <ScrollingSettings ScrollHeight="250" />
                                                <Columns>
                                                    <obout:Column ID="Column1" DataField="Edit" Width="2%" AllowFilter="False" Align="center"
                                                        HeaderAlign="Center" HeaderText="Edit" Index="0" TemplateId="imgBtnEdit1">
                                                        <TemplateSettings TemplateId="imgBtnEdit1" />
                                                    </obout:Column>
                                                    <obout:Column DataField="ID" HeaderText="ID" Visible="False">
                                                    </obout:Column>
                                                    <obout:Column DataField="Company" HeaderText="Company" Width="10%">
                                                    </obout:Column>
                                                    <obout:Column DataField="Customer" HeaderText="Customer" Width="10%">
                                                    </obout:Column>
                                                     <obout:Column DataField="ObjectName" HeaderText="Object" Width="8%">
                                                    </obout:Column>
                                                    <obout:Column DataField="NoOfApprovalLevl" HeaderText="No. of Apprroval Level" Align="center" HeaderAlign="center"
                                                        Width="6%">
                                                    </obout:Column>
                                                  <%--  <obout:Column DataField="NoOfApprovers" HeaderText="No. Of Approvers" Width="5%" Align="center" HeaderAlign="center">
                                                    </obout:Column>
                                                    <obout:Column DataField="AutoCancel" HeaderText="Auto Cancellation" Align="center" HeaderAlign="center"  Visible="false"
                                                        Width="5%">
                                                    </obout:Column>
                                                    <obout:Column DataField="canceldays" HeaderText="Auto Cancellation Days" Align="center" HeaderAlign="center" Visible="false" 
                                                        Width="5%">
                                                    </obout:Column>--%>
                                                     <obout:Column DataField="Active" HeaderText="Active" Width="5%">
                                                    </obout:Column>
                                                    <obout:Column DataField="CompanyID" HeaderText="CompanyID" Width="2%" Visible="false">
                                                    </obout:Column>
                                                    <obout:Column DataField="DepartmentID" HeaderText="DepartmentID" Width="2%" Visible="false">
                                                    </obout:Column>
                                                    <obout:Column DataField="CustomerID" HeaderText="CustomerID" Width="2%" Visible="false">
                                                    </obout:Column>
                                                    <obout:Column DataField="NoofApprovalLevel" HeaderText="NoofApprovalLevel" Width="2%"
                                                        Visible="false">
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
                    </center>
                </ContentTemplate>
                <%--</asp:UpdatePanel>
                    </ContentTemplate>--%>
            </asp:TabPanel>
            <asp:TabPanel ID="tabapprolevel" runat="server" HeaderText="Approval Level Info" TabIndex="1">
                <ContentTemplate>
                    <center>
                        <table class="tableForm">
                            <tr>
                                <td>Company :
                                </td>
                                <td style="text-align: left">
                                    <asp:DropDownList ID="ddlgroupcompany" runat="server" DataTextField="Name" DataValueField="ID" Width="206px">
                                         <asp:ListItem Text="-Select-" Value="0" Selected="True"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <req><asp:Label Id="lblcustname" runat="server" Text="Customer"></asp:Label></req> :
                                </td>
                                <td style="text-align: left">
                                    <asp:DropDownList ID="ddlcompany" runat="server" DataTextField="Name" DataValueField="ID" onchange="GetDepartment()" Width="206px">
                                         <asp:ListItem Text="-Select-" Value="0" Selected="True"></asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RfValCompany" runat="server" ControlToValidate="ddlcompany" InitialValue="0"
                                        Display="None" ErrorMessage="Please Select Company" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                    <td>
                                        <req><asp:Label ID="lblDocumentTitle" runat="server" Text="Object"></asp:Label></req> :
                                    </td>
                                    <td style="text-align: left">
                                       <asp:DropDownList ID="ddlobject" runat="server" DataTextField="Name" DataValueField="ID" Width="206px">
                                         <asp:ListItem Text="-Select-" Value="0" Selected="True"></asp:ListItem>
                                    </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblapprolevel" runat="server" Text="No. of Approval Level"></asp:Label> :
                                    </td>
                                    <td style="text-align: left">
                                       <asp:TextBox ID="txtNoApprovallevel" runat="server" MaxLength="50" onblur="CheckDocumentTitle()"
                                            ValidationGroup="DocumentValidation" Width="206px"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="reqValDocTitle" runat="server" ControlToValidate="txtNoApprovallevel"
                                            ValidationGroup="DocumentValidation" ErrorMessage="Enter document title" Display="None"></asp:RequiredFieldValidator>
                                    </td>
                             </tr>
                            <tr>
                                 <td>
                                    <req><asp:Label Id="lblactive" runat="server" Text="Active"/></req>
                                    :
                                </td>
                                <td style="text-align: left">
                                    <obout:OboutRadioButton ID="rbtnActiveYes" runat="server" Checked="True" FolderStyle=""
                                        GroupName="Active" Text="Yes">
                                    </obout:OboutRadioButton>
                                    <obout:OboutRadioButton ID="rbtnActiveNo" runat="server" FolderStyle="" GroupName="Active"
                                        Text="No">
                                    </obout:OboutRadioButton>
                                </td>
                                <td></td>
                                <td>
                                     <td class="auto-style1">
                                    <asp:Button ID="btncustomernext" runat="server" Text="  Next  " Height="30px" OnClientClick="return Checkvalidations();" />
                                </td>
                                </td>

                            </tr>
                          
                        </table>
                    </center>
                </ContentTemplate>
            </asp:TabPanel>
            <asp:TabPanel ID="panApprovalDetail" runat="server" TabIndex="2">
                        <HeaderTemplate>
                            <asp:Label ID="lbltabhead" runat="server" Text="Approval Level Detail" />
                        </HeaderTemplate>
                        <ContentTemplate>
                            <center>
                                <table class="tableForm">
                                    <%--<tr>
                                        <td colspan="4">
                                            <uc3:UC_Territory ID="UC_Territory1" runat="server" ClientIDMode="Static" />
                                        </td>
                                    </tr>comment by vishal--%> 
                                     <tr>
                                        <td>
                                            <req><asp:Label ID="lblnolevel" runat="server" Text="No. Of Approval level"/></req>
                                            :
                                        </td>
                                        <td style="text-align: left">
                                            <asp:TextBox ID="txtapproLevel" runat="server" Width="200px" onkeypress="return false"></asp:TextBox>
                                        </td>
                                        <td>
                                            <req><asp:Label ID="lblcurrentlevel" runat="server" Text="Current Approval level"/></req>
                                            :
                                        </td>
                                        <td style="text-align: left">
                                            <asp:DropDownList ID="ddlcurrntlevel" runat="server" Width="206px" DataTextField="Value"
                                                DataValueField="ID" ValidationGroup="Save" onchange="GetCurrentLevel(this)">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <req><asp:Label ID="lblapprovarno" runat="server" Text="No. Of Approvar"/></req>
                                            :
                                        </td>
                                        <td style="text-align: left">
                                            <asp:TextBox ID="txtnoapprovar" runat="server" Style="text-align: left;" Width="200px"></asp:TextBox>
                                        </td>
                                        <td>
                                            <req><asp:Label ID="lblapprovallogic" runat="server" Text="Approval Logic"/></req>
                                            :
                                        </td>
                                        <td style="text-align: left">
                                            <obout:OboutRadioButton ID="chkand" runat="server" Text="  And" GroupName="rbtnActive"
                                                Checked="true">
                                            </obout:OboutRadioButton>
                                            <obout:OboutRadioButton ID="chkor" runat="server" Text="  Or" GroupName="rbtnActive">
                                            </obout:OboutRadioButton>
                                            <%--
                                            <asp:CheckBox ID="chkand" runat="server" Text="  And" />
                                            &nbsp;&nbsp;&nbsp;
                                            <asp:CheckBox ID="chkor" runat="server" Text="  Or" />--%>
                                        </td>
                                    </tr>
                                    <%--<tr><td colspan="4" style="height:5px;"></td></tr>--%>
                                    <tr>
                                        <td align="right" colspan="4">
                                            
                                            <input type="button" id="btnaddi" runat="server" value="Add User" onclick="openUserSearch('0')" />
                                          <%--  <asp:Button ID="btnSave" runat="server" Text="  Save  " />--%>
                                        </td>
                                    </tr>
                                </table>
                            </center>
                            <asp:HiddenField ID="hdnStatus" runat="server" />
                            <asp:HiddenField ID="hdnApprovalID" runat="server" />
                            <asp:HiddenField ID="hdnApprovalUserID" runat="server" />
                            <asp:HiddenField runat="server" ID="HiddenField1" ClientIDMode="Static" />
                            <asp:HiddenField runat="server" ID="HiddenField2" ClientIDMode="Static" />
                            <asp:HiddenField runat="server" ID="hdnSelectedLevel" ClientIDMode="Static" />
                             <asp:HiddenField runat="server" ID="Hdn1" ClientIDMode="Static" />
                            <asp:HiddenField runat="server" ID="hdnUserIDs" />
                            <table class="gridFrame" style="width: 100%">
                                <tr>
                                    <td>
                                        <table style="width: 100%">
                                            <tr>
                                                <td style="text-align: left;">
                                                    <asp:Label ID="lblselectapprovar" runat="server" CssClass="headerText" Text="Select Approvers" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <obout:Grid ID="gvUserCreation" runat="server" AllowAddingRecords="False" AllowFiltering="True"
                                            Width="100%" AllowGrouping="True" AutoGenerateColumns="False" OnRebind="gvUserCreation_OnRebind">
                                            <Columns>
                                               <%-- <obout:Column DataField="checked" HeaderText="Select" Width="5%">
                                                    <TemplateSettings TemplateId="GvTempEdit" />
                                                </obout:Column>--%>
                                                <obout:Column DataField="ID" HeaderText="Remove" Visible="True" AllowEdit="false" Width="5%" Align="Center">
                                                    <TemplateSettings TemplateId="ItemTempRemove" />
                                                </obout:Column>
                                                <obout:Column DataField="EmployeeID" HeaderText="Employee ID" Width="8%">
                                                </obout:Column>
                                                <obout:Column DataField="userName" HeaderText="Employee Name" Width="15%">
                                                </obout:Column>
                                                <obout:Column DataField="EmailID" HeaderText="Email ID" Width="10%">
                                                </obout:Column>
                                                <obout:Column DataField="MobileNo" HeaderText="Mobile No." Width="10%">
                                                </obout:Column>
                                               <%-- <obout:Column DataField="RoleName" HeaderText="Role Name" Width="15%">
                                                </obout:Column>--%>
                                                <obout:Column DataField="DeptName" HeaderText="Department" Width="15%">
                                                </obout:Column>
                                                <obout:Column DataField="ApprovalLevel" HeaderText="Approval Level" Width="8%" Align="Center">
                                                </obout:Column>
                                                <obout:Column DataField="ApproverLogic" HeaderText="Approver Logic" Width="8%" Align="Center"></obout:Column>
                                                <obout:Column DataField="Active" HeaderText="Active" Width="6%">
                                                </obout:Column>
                                            </Columns>
                                            <Templates>
                                               <%-- <obout:GridTemplate ID="GvTempEdit">
                                                    <Template>
                                                        <asp:CheckBox runat="server" ID="chk" onclick="getSelectedUsersApprovalLevel(this);"
                                                            Checked='<%# (Container.Value == "true" ? true : false) %>' />
                                                    </Template>
                                                </obout:GridTemplate>--%>
                                                <obout:GridTemplate ID="ItemTempRemove">
                                                    <Template>
                                                        <table>
                                                            <tr>
                                                                <td style="width: 20px; text-align: center;">
                                                                    <img id="imgbtnRemove" src="../App_Themes/Grid/img/Remove16x16.png" title="Remove"
                                                                        onclick="removeUserFromList('<%# (Container.DataItem["ID"].ToString()) %>');"
                                                                        style="cursor: pointer;" />
                                                                </td>
                                                                <%--<td style="width: 35px; text-align: right;">
                                                                    <%# Convert.ToInt32(Container.PageRecordIndex) + 1 %>
                                                                </td>--%>
                                                            </tr>
                                                        </table>
                                                    </Template>
                                                </obout:GridTemplate>
                                            </Templates>
                                        </obout:Grid>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:TabPanel>

        </asp:TabContainer>
    </center>
    <%-- </ContentTemplate>
    </asp:UpdatePanel>--%>
    <script type="text/javascript">
        function fnAllowDigitsOnly(key) {
            var keycode = (key.which) ? key.which : key.keyCode;
            if ((keycode < 48 || keycode > 57) && (keycode != 8) && (keycode != 9) && (keycode != 11) && (keycode != 37) && (keycode != 38) && (keycode != 39) && (keycode != 40) && (keycode != 127)) {
                return false;
            }
        }

        function AllowAlphabet(key) {
            var keycode = (key.which) ? key.which : key.keyCode;
            if (!(((keycode >= '65') && (keycode <= '90')) || ((keycode >= '97') && (keycode <= '122')) || (keycode == '46') || (keycode == '32') || (keycode == '45') || (keycode == '11') || (keycode == '8') || (keycode == '127') || (keycode == '9') || (keycode == '11') || (keycode == '37') || (keycode == '38') || (keycode == '39') || (keycode == '40') || (keycode == '41'))) {
                return false;
            }
            return true;
        }

        function Checkvalidations() {

         

            if (document.getElementById("<%=ddlcompany.ClientID %>").value == "0") {
                showAlert("Please Enter Company name!", "error", "#");
                document.getElementById("<%=ddlcompany.ClientID %>").focus();
                return false;
            }
            if (document.getElementById("<%=txtNoApprovallevel.ClientID %>").value == "") {
                showAlert("Please Enter Email Id!", "error", "#");
                document.getElementById("<%=txtNoApprovallevel.ClientID %>").focus();
                return false;
            }

           
            if (document.getElementById("<%=ddlgroupcompany.ClientID %>").value == "0") {
                showAlert("Please Select Country!", "error", "#");
                document.getElementById("<%=ddlgroupcompany.ClientID %>").focus();
                return false;
            }
            if (document.getElementById("<%=ddlcurrntlevel.ClientID %>").value == "0") {
                showAlert("Please Select State!", "error", "#");
                document.getElementById("<%=ddlcurrntlevel.ClientID %>").focus();
                return false;
            }
          
            return true
        };

    </script>
   
    <script type="text/javascript">
        var ddlcompany = document.getElementById("<%=ddlcompany.ClientID %>");


        function GetDepartment() {
            var obj1 = new Object();
            obj1.ddlcompanyId = ddlcompany.value.toString();
            document.getElementById("<%=hdnSelectedCompany.ClientID %>").value = ddlcompany.value.toString();
            //PageMethods.GetDepartment(obj1, getLoc_onSuccessed);
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



    </script>

   
</asp:Content>

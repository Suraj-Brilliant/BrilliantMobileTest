<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/CRM.Master" AutoEventWireup="true"
    EnableEventValidation="false" CodeBehind="ApprovalLevelMaster.aspx.cs" Inherits="BrilliantWMS.Approval.ApprovalLevelMaster" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="obout_Interface" Namespace="Obout.Interface" TagPrefix="obout" %>
<%@ Register Assembly="obout_Grid_NET" Namespace="Obout.Grid" TagPrefix="obout" %>
<%@ Register Src="../CommonControls/UCToolbar.ascx" TagName="UCToolbar" TagPrefix="uc1" %>
<%@ Register Src="../CommonControls/UCFormHeader.ascx" TagName="UCFormHeader" TagPrefix="uc2" %>
<%@ Register Src="../Territory/UC_Territory.ascx" TagName="UC_Territory" TagPrefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHolder_FormHeader" runat="server">
    <uc1:UCToolbar ID="UCToolbar1" runat="server" />
    <uc2:UCFormHeader ID="UCFormHeader1" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPHolder_Form" runat="server">
    <asp:UpdateProgress ID="UpdateProgress_approvalM" runat="server" AssociatedUpdatePanelID="approvalM">
        <ProgressTemplate>
            <center>
                <div class="modal">
                    <img src="../App_Themes/Blue/img/ajax-loader.gif" style="top: 50%;" />
                </div>
            </center>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:ValidationSummary ID="validationsummary_ApprovalMaster" runat="server" ShowMessageBox="true"
        ShowSummary="false" DisplayMode="bulletlist" ValidationGroup="Save" />
    <asp:UpdatePanel ID="approvalM" runat="server">
        <ContentTemplate>
            <center>
                <asp:TabContainer ID="tabApprovalLevelMaster" runat="server" ActiveTabIndex="0" Width="100%">
                    <asp:TabPanel ID="tabApproval" runat="server" TabIndex="0">
                        <HeaderTemplate>
                            <asp:Label ID="lblheader" runat="server" Text="Approval Levels"></asp:Label></HeaderTemplate>
                        <ContentTemplate>
                            <center>
                                <asp:HiddenField ID="hdnDiscountID" runat="server" />
                                <table class="gridFrame" style="width: 100%">
                                    <tr>
                                        <td>
                                            <table style="width: 100%">
                                                <tr>
                                                    <td style="text-align: left;">
                                                        <asp:Label ID="lblheadtxt" CssClass="headerText" runat="server" Text="Approval Level List"></asp:Label>
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
                                                    <obout:Column DataField="CompanyName" HeaderText="Customer" Width="5%">
                                                    </obout:Column>
                                                    <obout:Column DataField="Territory" HeaderText="Department" Width="5%">
                                                    </obout:Column>
                                                    <%--<obout:Column DataField="ObjectName" HeaderText="Object Name" Width="7%" Index="4">
                                                    </obout:Column>--%>
                                                    <obout:Column DataField="ApprovalLevel" HeaderText="Approval Level" Align="center" HeaderAlign="center"
                                                        Width="5%">
                                                    </obout:Column>
                                                    <obout:Column DataField="NoOfApprovers" HeaderText="No. Of Approvers" Width="5%" Align="center" HeaderAlign="center">
                                                    </obout:Column>
                                                    <obout:Column DataField="AutoCancel" HeaderText="Auto Cancellation" Align="center" HeaderAlign="center" 
                                                        Width="5%">
                                                    </obout:Column>
                                                    <obout:Column DataField="canceldays" HeaderText="Auto Cancellation Days" Align="center" HeaderAlign="center" 
                                                        Width="5%">
                                                    </obout:Column>
                                                   <%-- <obout:Column DataField="Active" HeaderText="Active" Width="2%">
                                                    </obout:Column>--%>
                                                    <obout:Column DataField="CompanyID" HeaderText="CompanyID" Width="2%" Visible="false">
                                                    </obout:Column>
                                                    <obout:Column DataField="DepartmentID" HeaderText="DepartmentID" Width="2%" Visible="false">
                                                    </obout:Column>
                                                    <obout:Column DataField="NoofApprovalLevel" HeaderText="NoofApprovalLevel" Width="2%"
                                                        Visible="false">
                                                    </obout:Column>
                                                   <%-- <obout:Column DataField="ApproverLogic" HeaderText="NoofApprovalLevel" Width="2%"
                                                        Visible="false">
                                                    </obout:Column>--%>
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
                    </asp:TabPanel>
                    <asp:TabPanel ID="panApprovalDetail" runat="server" TabIndex="1">
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
                                            <req><asp:Label ID="lblcustomer" runat="server" Text="Customer"/></req>
                                            :
                                        </td>
                                        <td style="text-align: left">
                                            <asp:DropDownList ID="ddlcompany" runat="server" Width="206px" DataTextField="Name"
                                                DataValueField="ID" ValidationGroup="Save" onchange="GetCompany(this);GetDepartmentnew()">    <%--BindDepartment(this)--%>
                                            </asp:DropDownList>
                                            <%-- <asp:RequiredFieldValidator ID="valRefddlObjectName" runat="server" ErrorMessage="Please Select Company"
                                                ControlToValidate="ddlObjectName" InitialValue="0" ValidationGroup="Save" Display="None"></asp:RequiredFieldValidator>--%>
                                        </td>
                                        <td>
                                            <req><asp:Label ID="lbldept" runat="server" Text="Department"/></req>
                                            :
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddldepartment" runat="server" Width="206px" DataTextField="Territory"
                                                DataValueField="ID" ValidationGroup="Save" onchange="GetDepartment(this);GetApprovalLevel(this)">
                                                <asp:ListItem Text="-Select-" Value="0"></asp:ListItem>
                                            </asp:DropDownList>
                                            <%--<asp:RequiredFieldValidator ID="valddldept" runat="server" ErrorMessage="Please Select Department"
                                                ControlToValidate="ddlObjectName" InitialValue="0" ValidationGroup="Save" Display="None"></asp:RequiredFieldValidator>--%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <req><asp:Label ID="lblnolevel" runat="server" Text="No. Of Approval level"/></req>
                                            :
                                        </td>
                                        <td style="text-align: left">
                                            <asp:TextBox ID="lblApprovalLevel" runat="server" Width="200px" onkeypress="return false"></asp:TextBox>
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
                            <asp:HiddenField runat="server" ID="hdnSelectedCompany" ClientIDMode="Static" />
                            <asp:HiddenField runat="server" ID="hdnSelectedDepartment" ClientIDMode="Static" />
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
        </ContentTemplate>
    </asp:UpdatePanel>

    <script type="text/javascript">

        function openUserSearch(sequence) {
            var ddlcompany = document.getElementById("<%=ddlcompany.ClientID %>").value;
            var ddldepartment = document.getElementById("<%=ddldepartment.ClientID %>").value;
            var ddlcurrntlevel = document.getElementById("<%=ddlcurrntlevel.ClientID %>").value;
            var NoApprover = document.getElementById("<%=txtnoapprovar.ClientID %>").value;
            var hdnSelectedCompany = document.getElementById("<%=hdnSelectedCompany.ClientID%>");
            var hdnSelectedDepartment = document.getElementById("<%=hdnSelectedDepartment.ClientID%>");

            //window.open('../Account/AccountSearch.aspx', null, 'height=700px, width=825px,status= 0, resizable= 1, scrollbars=1, toolbar=0,location=center,menubar=0, screenX=300; screenY=50');

            if (ddlcompany == "" || ddlcompany == "0") {

                alert("Please Select Company");
                document.getElementById("<%=ddlcompany.ClientID %>").focus();
                return false;
            }
            else if (ddldepartment == "" || ddldepartment == "0") {
                alert("Please Select Department");
                document.getElementById("<%=ddldepartment.ClientID %>").focus();
                return false;
            }
            else if (ddlcurrntlevel == "" || ddlcurrntlevel == "0") {
                alert("Please Select Current Approval Level");
                document.getElementById("<%=ddlcurrntlevel.ClientID %>").focus();
                return false;
            }
            else if (NoApprover == "" || NoApprover == "0") {
                alert("Please Enter Number Of Approver");
                document.getElementById("<%=txtnoapprovar.ClientID %>").focus();
                return false;

            }
            else {
                window.open('../Account/AccountSearch.aspx?Com=' + ddlcompany + '  &Dept=' + ddldepartment + ' &NoApprover=' + document.getElementById("<%= txtnoapprovar.ClientID %>").value + '', null, 'height=1000px, width=825px,status= 0, resizable= 1, scrollbars=1, toolbar=0,location=center,menubar=0, screenX=300; screenY=50');
                //window.open('../Account/AccountSearch.aspx?Com=' + document.getElementById('hdnSelectedCompany').value + '  &Dept=' + document.getElementById('hdnSelectedDepartment').value + ' &NoApprover=' + document.getElementById("<%= txtnoapprovar.ClientID %>").value + '', null, 'height=400px, width=825px,status= 0, resizable= 1, scrollbars=1, toolbar=0,location=center,menubar=0, screenX=300; screenY=50');
                //window.location.href = "../Account/AccountSearch.aspx?Com=" + document.getElementById('hdnSelectedCompany').value + "&Dept=" + document.getElementById('hdnSelectedDepartment').value;
            }

        }
        function getSelectedUsersApprovalLevel(invoker) {
            var hdnApprovalUserID = document.getElementById("<%= hdnApprovalUserID.ClientID %>");

            var m = gvUserCreation.GridBodyContainer;
            hdnApprovalUserID.value = "";
            var checkedcount = 0;
            var allInput = m.getElementsByTagName('input');
            for (var c = 0; c < allInput.length; c++) {
                var chk = allInput[c];
                if (chk.type == "checkbox") {
                    if (chk.checked == true) {
                        checkedcount = checkedcount + 1;
                        if (parseInt(checkedcount) > parseInt(userno)) {
                            invoker.checked = false;
                            alert("You can't select more than " + userno + " users");
                            break;
                        }

                        if (hdnApprovalUserID.value != "") hdnApprovalUserID.value += ',' + gvUserCreation.Rows[c].Cells['ID'].Value;
                        if (hdnApprovalUserID.value == "") hdnApprovalUserID.value = gvUserCreation.Rows[c].Cells['ID'].Value;
                    }
                }
            }
        }

        function getNewApprovalLevel(invoker) {
            var TerritoryID = document.getElementById("hdnTerritoryID").value;
            PageMethods.PMGetApprovalLevelByObjectName(invoker.value, parseInt(TerritoryID), onsuccess_getNewApprovalLevel, null);
        }

        function onsuccess_getNewApprovalLevel(result) {
            document.getElementById("lblApprovalLevel").value = result;
        }

        function clearChild() {
            document.getElementById("lblApprovalLevel").value = "";

        }

        function GetCompany() {
            var ddlcompany = document.getElementById("<%=ddlcompany.ClientID %>").value;

            var hdnSelectedCompany = document.getElementById("<%=hdnSelectedCompany.ClientID%>");
            hdnSelectedCompany.value = ddlcompany;

        }

        function GetCurrentLevel() {
            var ddlcurrntlevel = document.getElementById("<%=ddlcurrntlevel.ClientID %>").value;

            var hdnSelectedLevel = document.getElementById('hdnSelectedLevel');
            hdnSelectedLevel.value = ddlcurrntlevel;
            

        }


        function BindDepartment() {

            PageMethods.PMGetDepartmentList(hdnSelectedCompany.value, onSuccessGetDepartmentList, null)
        }
        function onSuccessGetDepartmentList(result) {
            ddldepartment = document.getElementById("<%=ddldepartment.ClientID %>");
            ddldepartment.options.length = 0;
            var option0 = document.createElement("option");

            if (result.length > 0) {
                option0.text = "--Select--";
                option0.value = '0';
            }
            else {
                option0.text = 'N/A';
                option0.value = '0';
            }

            try {
                ddldepartment.add(option0, null);
            }
            catch (Error) {
                ddldepartment.add(option0);
            }

            for (var i = 0; i < result.length; i++) {
                var option1 = document.createElement("option");
                option1.text = result[i].Territory;
                option1.value = result[i].ID;
                try {
                    ddldepartment.add(option1, null);
                }
                catch (Error) {
                    ddldepartment.add(option1);
                }
            }
        }

        var ddlcompany = document.getElementById("<%=ddlcompany.ClientID %>");
        var ddldeptid = document.getElementById("<%=ddldepartment.ClientID %>");

        function GetDepartmentnew() {
            var obj1 = new Object();
            obj1.ddlcompanyId = ddlcompany.value.toString();

            PageMethods.GetDepartment(obj1, getLocDept_onSuccessed);
        }

        function getLocDept_onSuccessed(result) {

            ddldeptid.options.length = 0;
            for (var i in result) {
                AddOption12(result[i].Name, result[i].Id);
            }
        }

        function AddOption12(text, value) {

            var option12 = document.createElement('option');
            option12.value = value;
            option12.innerHTML = text;
            ddldeptid.options.add(option12);
        }

        function GetDepartment() {
            var ddldepartment = document.getElementById("<%=ddldepartment.ClientID %>").value;
            var hdnSelectedDepartment = document.getElementById("<%=hdnSelectedDepartment.ClientID%>");
            hdnSelectedDepartment.value = ddldepartment;
           
        }

        function GetApprovalLevel() {

            var hdnSelectedCompany = document.getElementById("<%=hdnSelectedCompany.ClientID%>");
            var hdnSelectedDepartment = document.getElementById("<%=hdnSelectedDepartment.ClientID%>");
            if (hdnSelectedCompany.value != "" && hdnSelectedDepartment.value != "") {

                PageMethods.PMApprovalLevel(hdnSelectedCompany.value, hdnSelectedDepartment.value, onSuccessGetApprovalLevel, null)
            }
        }

        function onSuccessGetApprovalLevel(result) {
            var ddlcurrntlevel = document.getElementById("<%=ddlcurrntlevel.ClientID %>");
            document.getElementById("<%=lblApprovalLevel.ClientID %>").value = result;
            var num = document.getElementById("<%=lblApprovalLevel.ClientID %>").value;
            ddlcurrntlevel.options.length = 0;
            //var select = document.getElementById("selectNumber");
            //var options = ["1", "2", "3", "4", "5"];
            for (var i = 0; i <= num; i++) {
                var el = document.createElement("option");
                var opt = i;
                if (opt == 0) {
                    el.textContent = "-Select--";
                    el.value = opt;
                    ddlcurrntlevel.appendChild(el);
                }
                else {


                    el.textContent = opt;
                    el.value = opt;
                    ddlcurrntlevel.appendChild(el);
                }

            }
        }

        function removeUserFromList(ID) {
            /*Remove Part from list*/
//            var hdnUserIDs = document.getElementById('hdnUserIDs');
            var Hdn1 = document.getElementById('Hdn1');
//            hdnUserIDs.value = "";
            Hdn1.value = "1";
            var r = confirm("Are you Sure To Delete All Approvers From This Level?");
            if (r == true) {
                PageMethods.WMRemoveUserFromUserList(ID, RemoveUserFromUserListOnSussess, null);
            } else { }
        }
        function RemoveUserFromUserListOnSussess() { gvUserCreation.refresh(); }

    </script>
</asp:Content>

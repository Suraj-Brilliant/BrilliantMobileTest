<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ContactPersonInfo.aspx.cs" Inherits="BrilliantWMS.ContactPerson.ContactPersonInfo" EnableEventValidation="false" %>

<%@ Register Assembly="obout_Grid_NET" Namespace="Obout.Grid" TagPrefix="obout" %>
<script src="../MasterPage/JavaScripts/countries.js" type="text/javascript"></script>
<script src="../MasterPage/JavaScripts/crm.js" type="text/javascript"></script>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>GWC</title>
</head>
<body>
    <form id="ContactPerson_Form" runat="server">
     <asp:HiddenField ID="hdncompanyid" runat="server" ClientIDMode="Static" />
    <div>
    <asp:ScriptManager ID="ScriptmanagerContactPerson" runat="server" EnablePageMethods="true">
        </asp:ScriptManager>
     <center>
            <div id="divLoading" style="height: 275px; width: 920px; display: none" class="modal">
                <center>
                    <br />
                    <span id="spanLoading" style="font-size: 17px; font-weight: bold; color: Yellow;">Processing
                        please wait...</span>
                </center>
            </div>
            <asp:ValidationSummary ID="validationsummary_UcContactPersonInfo" runat="server"
                ShowMessageBox="true" ShowSummary="false" DisplayMode="bulletlist" ValidationGroup="SubmitContactPerson" />
            <table class="gridFrame" style="margin: 3px 3px 3px 3px">
                <tr>
                    <td style="text-align: left;">
                        <asp:Label class="headerText" ID="lblContactPersonFormHeader" runat="server" Text="Add New Conatct Person Info"></asp:Label>
                    </td>
                    <td style="width: 20%">
                        <table style="float: right;">
                            <tr>
                                <td>
                                    <input type="button" id="btnAddressSubmit" runat="server" value="Submit" onclick="SubmitContactPerson()"
                                        style="width: 70px;" />
                                </td>
                                <td>
                                    <input type="button" id="btnAddressClear" runat="server" value="Clear" onclick="ClearContactPerson()"
                                        style="width: 70px;" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td style="background-color: White;" colspan="2">
                        <table class="tableForm">
                            <tr>
                                <td>
                                    <req><asp:Label ID="lblconname" runat="server" Text="Contact Name"/> </req> :
                                </td>
                                <td>
                                    <asp:TextBox ID="txtName" runat="server" Width="190px" onkeypress="return alpha(event);"
                                        ValidationGroup="SubmitContactPerson" MaxLength="100" onchange="CheckDuplicateContactInfo(this)"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RfValidator_Name" ValidationGroup="SubmitContactPerson"
                                        runat="server" Display="None" ControlToValidate="txtName" ErrorMessage="Enter contact name"></asp:RequiredFieldValidator>
                                </td>
                               <td>
                                    <asp:Label ID="lblconttype" runat="server" Text="Contact Type"/> :
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlcontacttype" DataTextField="ContactType" DataValueField="ID"
                                        Width="190px" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                   <asp:Label ID="lblemailid" runat="server" Text="Email ID "/> :
                                </td>
                                <td>
                                    <asp:TextBox ID="txtEmailID" runat="server" Width="190px" MaxLength="100"></asp:TextBox>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator_txtEmailID" runat="server"
                                        ValidationGroup="ContactPerSubmit" ControlToValidate="txtEmailID" Display="None"
                                        ErrorMessage="Please enter valid email address" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                                </td>
                               
                            </tr>
                            <tr>
                                
                                <td>
                                    <asp:Label ID="lblofficeno" runat="server" Text="Office No."/> :
                                </td>
                                <td>
                                    <asp:TextBox ID="txtOfficeNo" onkeydown="return AllowPhoneNo(this, event);" onkeypress="return AllowPhoneNo(this, event);"
                                        runat="server" Width="190px" MaxLength="10"></asp:TextBox>
                                </td>
                                  <td>
                                    <asp:Label ID="lblactive" runat="server" Text="Active"/> :
                                </td>
                                <td style="text-align: left;">
                                    <asp:RadioButton ID="rbtnActiveYes" runat="server" Text="Yes" GroupName="rbtnActive"
                                        Checked="true" />
                                    <asp:RadioButton ID="rbtnActiveNo" runat="server" Text="No" GroupName="rbtnActive" />
                                </td>
                                <td>
                                    <asp:Label ID="lblmobno" runat="server" Text="Mobile No."/> :
                                </td>
                                <td>
                                    <asp:TextBox ID="txtMobileNo" runat="server" Width="190px" onkeydown="return AllowPhoneNo(this, event);"
                                        onkeypress="return AllowPhoneNo(this, event);" MaxLength="10"></asp:TextBox>
                                </td>
                              
                            </tr>
                            <tr>
                                <td colspan="6">
                                    <span class="watermark" style="float: right;"><asp:Label ID="lblcontvalidn" runat="server" Text="Enter atleast one contact info [ EmailID / Office No. / Mobile No.]"/> </span>
                                </td>
                            </tr>
                           <%-- <tr>
                               
                                <td>
                                    Intrested In :
                                </td>
                                <td>
                                    <asp:TextBox ID="txtIntrestedIn" runat="server" Width="190px" MaxLength="200"></asp:TextBox>
                                </td>
                                <td>
                                    Hobbies :
                                </td>
                                <td>
                                    <asp:TextBox ID="txtHobbies" runat="server" Width="190px" MaxLength="200"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Facebook ID :
                                </td>
                                <td>
                                    <asp:TextBox ID="txtFacebookID" runat="server" Width="190px" MaxLength="100"></asp:TextBox>
                                    <asp:RegularExpressionValidator ID="RegExptxtFacebookID" runat="server" ValidationGroup="SubmitContactPerson"
                                        ControlToValidate="txtFacebookID" Display="None" ErrorMessage="Please enter valid email address"
                                        ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                                </td>
                                <td>
                                    Twitter ID :
                                </td>
                                <td>
                                    <asp:TextBox ID="txtTwitterID" runat="server" Width="190px" MaxLength="100"></asp:TextBox>
                                    <asp:RegularExpressionValidator ID="regexptxtTwitterID" runat="server" ValidationGroup="SubmitContactPerson"
                                        Display="None" ControlToValidate="txtTwitterID" ErrorMessage="Please enter valid email address"
                                        ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                                </td>
                                <td>
                                    Other ID(if any) :
                                </td>
                                <td>
                                    <asp:TextBox ID="txtOtherID" runat="server" ValidationGroup="ContactPerSubmit" Width="190px"
                                        MaxLength="100" Style="top: 0px; left: 0px"></asp:TextBox>
                                    <asp:RegularExpressionValidator ID="regexptxtOtherID" runat="server" ValidationGroup="SubmitContactPerson"
                                        Display="None" ControlToValidate="txtOtherID" ErrorMessage="Please enter valid email address"
                                        ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Qualification :
                                </td>
                                <td>
                                    <asp:TextBox ID="txtHighestQualification" runat="server" Width="190px" MaxLength="100"></asp:TextBox>
                                </td>
                                <td>
                                    College/University :
                                </td>
                                <td>
                                    <asp:TextBox ID="txtCollege" runat="server" Width="190px" MaxLength="100"></asp:TextBox>
                                </td>
                                <td>
                                    High School :
                                </td>
                                <td>
                                    <asp:TextBox ID="txtHighScholl" runat="server" Width="190px" MaxLength="100"></asp:TextBox>
                                </td>
                            </tr>--%>
                            <tr>
                                <td>
                                   <asp:Label ID="lblremark" runat="server" Text="Remark"/> :
                                </td>
                                <td style="text-align: left;" colspan="4">
                                    <asp:TextBox ID="txtremark" runat="server" Width="500px" MaxLength="200"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: left; width: 80%">
                    </td>
                    <td style="width: 20%">
                        <table style="float: right;">
                            <tr>
                                <td>
                                    <input type="button" id="Button1" runat="server" value="Submit" onclick="SubmitContactPerson();"
                                        style="width: 70px;" />
                                </td>
                                <td>
                                    <input type="button" id="Button2" runat="server" value="Clear" onclick="ClearContactPerson()" style="width: 70px;" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </center>
    </div>
     <script type="text/javascript">
       
         var ddlcontacttype = document.getElementById("ddlcontacttype");
         function SubmitContactPerson() {
             if (typeof (Page_ClientValidate) == 'function') {
                 Page_ClientValidate();
             }
             if (Page_IsValid) {
                 if ((document.getElementById("txtEmailID").value == "") && (document.getElementById("txtOfficeNo").value == "") && (document.getElementById("txtMobileNo").value == ""))
                 { alert("Enter atleast one contact info \n[ EmailID / Office No. / Mobile No.]"); document.getElementById("txtEmailID").focus(); }
                 else {
                     LoadingOn();
                     var ContactPersonInfo = new Object();
                     ContactPersonInfo.Name = document.getElementById("txtName").value;
                     ContactPersonInfo.Department = 1;
                     ContactPersonInfo.hdncompanyid = document.getElementById("hdncompanyid").value; 
                     ContactPersonInfo.Designation = 1;
                     ContactPersonInfo.EmailID = document.getElementById("txtEmailID").value;
                     ContactPersonInfo.OfficeNo = document.getElementById("txtOfficeNo").value;
                     ContactPersonInfo.MobileNo = document.getElementById("txtMobileNo").value;
                     ContactPersonInfo.ContactTypeID = document.getElementById("ddlcontacttype").value;
                     ContactPersonInfo.Remark = document.getElementById("txtremark").value;
                     if (document.getElementById("ddlcontacttype").value == "0") { ContactPersonInfo.ContactType = "N/A"; }
                     else { ContactPersonInfo.ContactType = ddlcontacttype.options[ddlcontacttype.selectedIndex].text; }
//                     ContactPersonInfo.Active = "N";
//                     if (rbtnActiveYes.checked == true) { ContactPersonInfo.Active = "Y"; }
                     PageMethods.PMSaveConatctPerson(ContactPersonInfo, SubmitContactPerson_onSuccess, SubmitContactPerson_onFail)
                 }
             }
         }

         function SubmitContactPerson_onSuccess() {
             //alert("Contact info saved successfully");
             LoadingOff();
             window.opener.GVContactPerson.refresh();
             self.close();
         }

         function SubmitContactPerson_onFail() { }

         function ClearContactPerson() {
             document.getElementById("<%= txtName.ClientID %>").value = '';
             document.getElementById("<%= txtEmailID.ClientID %>").value = '';
             document.getElementById("<%= txtOfficeNo.ClientID %>").value = '';
             document.getElementById("<%= txtMobileNo.ClientID %>").value = '';
             document.getElementById("<%= ddlcontacttype.ClientID %>").value = 0;
             document.getElementById("<%= txtremark.ClientID %>").value = '';
             rbtnActiveYes.checked = true; rbtnActiveNo.checked = false;
         }
         function CheckDuplicateContactInfo(invoker) {
             var Sequence = getParameterByName("Sequence");
             PageMethods.PMCheckDuplicate(invoker.value, parseInt(Sequence), OnSucessCheckDuplicate, null);
         }

         function OnSucessCheckDuplicate(result) {
             if (result != "") {
                 if (result != "Same Contact Person details already exists, do you want to continue") {

                     var msg = "Same Contact details already exists in Archive[ based on mandatory fields ] \n\n - Do you want to unarchive ?";
                     var answer1 = confirm(msg);
                     if (answer1) {
                         window.opener.SetUnArchiveConatactPerson(result, "N");
                         alert("Unarchive successful");
                         window.opener.GVContactPerson.refresh();
                         self.close();
                     }
                 }
                 else {
                     var answer = confirm(result);
                     if (answer == false) {
                         document.getElementById("<%= txtName.ClientID %>").value = "";
                     }
                 }
             }
         }
    </script>
    </form>
</body>
</html>

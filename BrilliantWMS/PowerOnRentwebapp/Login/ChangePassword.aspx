<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/CRM2.Master" AutoEventWireup="true"
    CodeBehind="ChangePassword.aspx.cs" Inherits="BrilliantWMS.Login.ChangePassword" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <center>
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="ChangePassword"
                DisplayMode="BulletList" ShowMessageBox="true" ShowSummary="false" />
            <div id="divLoading" style="height: 350px; width: 465px; display: none" class="modal">
                <center>
                    <br />
                    <span id="spanLoading" style="font-size: 17px; font-weight: bold; color: Yellow;">Processing
                        please wait...</span>
                </center>
            </div>
            <table class="gridFrame" style="margin: 3px; width: 390px;">
                <tr>
                    <td>
                        <span id="headerText"> <asp:Label ID="lblheader" CssClass="headerText" runat="server" Text="Change Password"></asp:Label></span>
                    </td>
                    <td style="text-align: right; width: 20%">
                        <table>
                            <tr>
                                <td>
                                    <input type="button" value="Save" id="btnSaveChangePassword" runat="server" onclick="SaveChangePassword()" />
                                </td>
                                <td>
                                    <input type="button" value="Clear" id="btnClearChangePassword" runat="server" onclick="clearChangePassword();" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <table class="tableForm" style="background-color: White; width: 100%">
                            <tr>
                                <td>
                                    <req> <asp:Label ID="lblusername" runat="server" Text="User Name"></asp:Label> : </req>
                                </td>
                                <td style="text-align: left;">
                                    <asp:Label ID="lblLoginName" Font-Bold="true" runat="server" ClientIDMode="Static"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <req><asp:Label ID="lblcurrentpass" runat="server" Text="Current Password"></asp:Label>:</req>
                                </td>
                                <td style="text-align: left;">
                                    <asp:TextBox ID="txtCurrentPassword" runat="server" Width="200px" ClientIDMode="Static"
                                        ValidationGroup="ChangePassword" TextMode="Password" MaxLength="50"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="req_txtCurrentPassword" runat="server" ErrorMessage="Enter Currenct Password"
                                        ControlToValidate="txtCurrentPassword" ValidationGroup="ChangePassword" Display="None">
                                    </asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <req><asp:Label ID="lblnewpassword" runat="server" Text="New Password"></asp:Label>:</req>
                                </td>
                                <td style="text-align: left;">
                                    <asp:TextBox ID="txtNewPassword" runat="server" ClientIDMode="Static" ValidationGroup="ChangePassword" 
                                     TextMode="Password" Width="200px"  onkeyup="divPasswordwstrength()" ></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidatortxtNewPassword" runat="server"
                                        ErrorMessage="Enter New Password" ControlToValidate="txtNewPassword" ValidationGroup="ChangePassword"
                                        Display="None">
                                    </asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <req><asp:Label ID="lblconfirmpass" runat="server" Text="Confirm Password"></asp:Label>:</req>
                                </td>
                                <td style="text-align: left;">
                                    <asp:TextBox ID="txtConfirmNewPassword" runat="server" ClientIDMode="Static" ValidationGroup="ChangePassword"
                                         TextMode="Password" Width="200px" 
                                        onkeyup="compareCPw();"></asp:TextBox> <%--onblur="Javascript:CheckPassword();"--%>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidatortxtConfirmNewPassword" runat="server"
                                        ErrorMessage="Enter Confirm Password" ControlToValidate="txtConfirmNewPassword"
                                        ValidationGroup="ChangePassword" Display="None">
                                    </asp:RequiredFieldValidator>
                                    <asp:CompareValidator ID="CompareValidatorCheckConfirmPassword" runat="server" ErrorMessage="Invalid Confirm Password"
                                        ControlToValidate="txtConfirmNewPassword" ControlToCompare="txtNewPassword" Display="None"
                                        ValidationGroup="ChangePassword"></asp:CompareValidator>
                                    <%-- <asp:RangeValidator ID="RFVConfirmPassword" runat="server" ControlToValidate="txtConfirmNewPassword"
                                        ErrorMessage="Invalid Confirm Password. Please enter the Password between 20 to 40."
                                        Display="None" ValidationGroup="ChangePassword" MinimumValue="8" MaximumValue="32">
                                    </asp:RangeValidator>--%>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td style="text-align: left;">
                                    <div id="divPasswordwstrength">
                                    </div>
                                    <span id="PasswordwstrengthMsg" style="font-size: 11px; margin-top: -11px; margin-right: 8px;
                                        float: right;"></span>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </center>
    </div>
    <script type="text/javascript">
        function clearChangePassword() {
            document.getElementById("txtCurrentPassword").value = "";
            document.getElementById("txtNewPassword").value = "";
            document.getElementById("txtConfirmNewPassword").value = "";
            document.getElementById("txtCurrentPassword").focus();
            divPasswordwstrength();
        }

        function SaveChangePassword() {
            if (typeof (Page_ClientValidate) == 'function') {
                Page_ClientValidate();
            }
            if (Page_IsValid) {
               // LoadingOn();
                var loginName = document.getElementById("lblLoginName").innerHTML;
                var CurrentPassword = document.getElementById("txtCurrentPassword").value;
                var NewPassword = document.getElementById("txtNewPassword").value;
                var ConfirmNewPassword = document.getElementById("txtConfirmNewPassword").value;

                var pass = /^(?=.*\d)(?=.*[a-z])(?=.*[^a-zA-Z0-9])(?!.*\s).{8,32}$/;
                if (document.getElementById("txtNewPassword").value != document.getElementById("txtConfirmNewPassword").value) {
                    alert("New Password And Confirm Should Be Same");
                    document.getElementById("txtConfirmNewPassword").value = "";
                    document.getElementById("txtNewPassword").value = "";
                    document.getElementById("txtNewPassword").focus;
                    dpw.style.width = "0px";
                    msg.innerHTML = "";
                    return false;
                }
                else if (ConfirmNewPassword.length < 8 || ConfirmNewPassword.length > 32) {
                    alert("Password length should be greater than 8 characters and less than 32 characters");
                    document.getElementById("txtConfirmNewPassword").value = "";
                    document.getElementById("txtNewPassword").value = "";
                    document.getElementById("txtNewPassword").focus;
                    dpw.style.width = "0px";
                    msg.innerHTML = "";
                    return false;
                }
                else if (!ConfirmNewPassword.match(pass)) {
                    alert("Password should be combination of alphabets, numbers, special characters.");
                    document.getElementById("txtConfirmNewPassword").value = "";
                    document.getElementById("txtNewPassword").value = "";
                    document.getElementById("txtNewPassword").focus;
                    dpw.style.width = "0px";
                    msg.innerHTML = "";
                    return false;
                }
                else {
                    PageMethods.PMCheckPassword(ConfirmNewPassword, OnSuccessPMCheckPassword);
                }

                //PageMethods.PMSaveChangePassword(loginName, CurrentPassword, NewPassword, OnSuccessPMSaveChangePassword, OnFailPMSaveChangePassword);
            }
        }

        function OnSuccessPMCheckPassword(result) {
            var loginName = document.getElementById("lblLoginName").innerHTML;
            var CurrentPassword = document.getElementById("txtCurrentPassword").value;
            var NewPassword = document.getElementById("txtNewPassword").value;
            var ConfirmNewPassword = document.getElementById("txtConfirmNewPassword").value;

            var dpw = document.getElementById("divPasswordwstrength");
            var msg = document.getElementById("PasswordwstrengthMsg");
            if (result == "PasswordFound") {
                alert("You Have Already Used This Password.");
                document.getElementById("txtNewPassword").value = "";
                document.getElementById("txtConfirmNewPassword").value = "";
                document.getElementById("txtNewPassword").focus();
                dpw.style.width = "0px";
                msg.innerHTML = "";
                return false;
            } else if (result == "SameDay") {
                alert('You Can Change Password Only Once In A Day...'); self.close();
                document.getElementById("txtNewPassword").value = "";
                document.getElementById("txtConfirmNewPassword").value = "";
                document.getElementById("txtNewPassword").focus();
                dpw.style.width = "0px";
                msg.innerHTML = "";
                return false;
            } else {
                PageMethods.PMSaveChangePassword(loginName, CurrentPassword, NewPassword, OnSuccessPMSaveChangePassword, OnFailPMSaveChangePassword);
                //return true;
            }

        }

        function OnSuccessPMSaveChangePassword(result) {
            if (result == "Saved") {
                alert("Password changed successfully");
                self.close();
            }
            else {
                alert("Password change failed.\n -Invalid Current [ Old ] Password");
                document.getElementById("txtCurrentPassword").focus();
                document.getElementById("txtCurrentPassword").select();
            }
          //  LoadingOff();
        }
        function OnFailPMSaveChangePassword() {
            alert("Password change failed.\n -Please re-enter your values and try again");
            clearChangePassword();
        }
        function compareCPw() {
            var txt = document.getElementById("txtConfirmNewPassword");
            txt.style.borderColor = "red";
            if (document.getElementById("txtNewPassword").value == document.getElementById("txtConfirmNewPassword").value) {
                txt.style.borderColor = "green";
            }
        }
        function divPasswordwstrength() {
            var txtlength = document.getElementById("txtNewPassword").value.length;
            var dpw = document.getElementById("divPasswordwstrength");
            var msg = document.getElementById("PasswordwstrengthMsg");
            dpw.style.height = "10px";
            switch (txtlength) {
                case 0:
                    dpw.style.backgroundColor = "red";
                    dpw.style.width = "10px";
                    msg.innerHTML = "Weak";
                    break;
                case 1:
                    dpw.style.backgroundColor = "red";
                    dpw.style.width = "10px";
                    msg.innerHTML = "Weak";
                    break;
                case 2:
                    dpw.style.backgroundColor = "red";
                    dpw.style.width = "20px";
                    msg.innerHTML = "Weak";
                    break;
                case 3:
                    dpw.style.backgroundColor = "red";
                    dpw.style.width = "30px";
                    msg.innerHTML = "Weak";
                    break;
                case 4:
                    dpw.style.backgroundColor = "red";
                    dpw.style.width = "40px";
                    msg.innerHTML = "Weak";
                    break;
                case 5:
                    dpw.style.backgroundColor = "orange";
                    dpw.style.width = "50px";
                    msg.innerHTML = "Average";
                    break;
                case 6:
                    dpw.style.backgroundColor = "orange";
                    dpw.style.width = "60px";
                    msg.innerHTML = "Average";
                    break;
                case 7:
                    dpw.style.backgroundColor = "orange";
                    dpw.style.width = "70px";
                    msg.innerHTML = "Average";
                    break;
                case 8:
                    dpw.style.backgroundColor = "orange";
                    dpw.style.width = "80px";
                    msg.innerHTML = "Average";
                    break;
                case 9:
                    dpw.style.backgroundColor = "orange";
                    dpw.style.width = "90px";
                    msg.innerHTML = "Average";
                    break;
                case 10:
                    dpw.style.backgroundColor = "green";
                    dpw.style.width = "100px";
                    msg.innerHTML = "Strong";
                    break;
                case 11:
                    dpw.style.backgroundColor = "green";
                    dpw.style.width = "110px";
                    msg.innerHTML = "Strong";
                    break;
                case 12:
                    dpw.style.backgroundColor = "green";
                    dpw.style.width = "120px";
                    msg.innerHTML = "Strong";
                    break;
                case 13:
                    dpw.style.backgroundColor = "green";
                    dpw.style.width = "130px";
                    msg.innerHTML = "Strong";
                    break;
                case 14:
                    dpw.style.backgroundColor = "green";
                    dpw.style.width = "140px";
                    msg.innerHTML = "Strong";
                    break;
                case 15:
                    dpw.style.backgroundColor = "green";
                    dpw.style.width = "150px";
                    msg.innerHTML = "Strong";
                    break;
            }

        }

    </script>    
</asp:Content>

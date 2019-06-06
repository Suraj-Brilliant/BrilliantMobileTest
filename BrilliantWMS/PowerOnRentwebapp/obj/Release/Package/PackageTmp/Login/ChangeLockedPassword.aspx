<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/CRM2.Master" AutoEventWireup="true"
    CodeBehind="ChangeLockedPassword.aspx.cs" Inherits="BrilliantWMS.Login.ChangeLockedPassword" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link href="../App_Themes/Login.css" rel="stylesheet" type="text/css" />
    <style>
        body {
            margin: 0px;
            padding: 0px;
        }

        .divLoginHolder table {
            /*width: 300px;*/
        }

        .divLoginHolder {
            width: 370px;
            border: solid 1px #ffffff;
            border-radius: 13px;
            box-shadow: 0px 0px 10px #636363;
            font-size: 14px;
            font-family: Arial;
            margin-bottom: 30px;
            overflow: hidden;
        }

            .divLoginHolder req {
                color: #4088BF;
                font-weight: bold;
            }

            .divLoginHolder input, .divLoginHolder select {
                font-size: 14px;
                font-family: Arial;
                padding: 10px 10px;
                border-radius: 13px;
                border: solid 1px #cccccc;
                width: 300px !important;
                color: #636363;
            }

            .divLoginHolder select {
                width: 324px !important;
            }

            .divLoginHolder input[type="button"], .divLoginHolder input[type="submit"], .divLoginHolder input[type="reset"] {
                width: 321px !important;
                color: #ffffff;
            }

        .divLoginBoxHeader {
            color: #ffffff;
            font-weight: bold;
            background-color: #4088BF;
            border: solid 1px #4088BF;
            padding: 10px 20px;
            border-radius: 13px;
            font-size: 24px;
            text-align: center;
            border-bottom-left-radius: 0px;
            border-bottom-right-radius: 0px;
        }

        .divLoginBoxContent {
            padding: 20px;
            background-image: url(../company/background/login-box-bg.png);
            padding-top: 0px;
        }

        .pnlLoginBg {
            background-color: #000000;
            position: fixed;
            top: 0px;
            left: 0px;
            width: 100%;
            height: 100%;
        }

            .pnlLoginBg img {
                opacity: 0.5;
            }

        .pnlLoginContent {
            position: absolute;
            z-index: 999;
            display: table;
            width: 100%;
            height: 100%;
        }

        .pnlLoginContentSub {
            display: table-cell;
            vertical-align: middle;
            text-align: center;
        }

        .loginFooter {
            background-color: #000000;
            position: absolute;
            bottom: 0px;
            left: 0px;
            opacity: 0.8 !important;
            width: 100%;
        }

            .loginFooter table:first-child {
                width: 90%;
            }

            .loginFooter td, .loginFooter div, .loginFooter a {
                color: #ffffff;
            }

        .tlbPoweredBy {
            width: auto !important;
        }

        .spnFailureMsg {
            margin-top: 10px;
            background-color: #990000;
            border: solid 1px #990000;
            border-radius: 7px;
            padding: 10px;
            display: inline-block;
            color: #ffffff;
            font-weight: bold;
            font-size: 12px;
        }
    </style>
    <div class="pnlLoginBg">
        <asp:Image ID="Image1" Width="100%" Height="100%" runat="server" ImageUrl="~/Company/Background/login-bg.jpg" />
    </div>
    <div class="pnlLoginContent">
        <div class="pnlLoginContentSub">
            <div>
                <center>
                    <table style="margin: 0px 0 0 0;">
                        <tr>
                            <td></td>
                            <td></td>
                            <td class="font"></td>
                        </tr>
                        <tr>
                            <td colspan="3" align="center">
                                <div class="divLoginHolder">
                                    <div class="divLoginBoxContent">
                                        <%--  <center>--%>
                                        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="ChangePassword"
                                            DisplayMode="BulletList" ShowMessageBox="true" ShowSummary="false" />
                                        <%-- <div id="divLoading" style="height: 350px; width: 465px; display: none" class="modal">
                <center>
                    <br />
                    <span id="spanLoading" style="font-size: 17px; font-weight: bold; color: Yellow;">Processing
                        please wait...</span>
                </center>
            </div>--%>
                                        <table  cellspacing="5" style="position: relative; top: 10px;">
                                            <tr>
                                                <td align="center">
                                                    <asp:Image ID="ElegantLogo" runat="server" ImageUrl="~/MasterPage/Logo/GWCBrandNewLogo.png"
                                                        Style="position: relative; left: 5px; width: 150px; margin-bottom: 10px;" />
                                                </td>
                                            </tr>
                                        </table>
                                        <table  cellspacing="5">
                                            <%--<tr>
                    <td>
                        <span id="headerText">Change Password</span>
                    </td>
                    <td style="text-align: right; width: 20%">
                        <table>
                            <tr>
                                <td>
                                    
                                </td>
                                <td>
                                    <input type="button" value="Clear" id="btnClearChangePassword" onclick="clearChangePassword();" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>--%>
                                            <%--<tr>--%>
                                                <tr>
                                                    <td >
                                                        <table  style=" width: 104%">
                                                            <tr>
                                                                <%--<td>
                                                                    <req>User Name : </req>
                                                                </td>--%>
                                                                <td style="text-align: left;">
                                                                    <asp:Label ID="lblLoginName" Font-Bold="true" runat="server" ClientIDMode="Static"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <%--<td>
                                                                    <req>Current Password :</req>
                                                                </td>--%>
                                                                <td style="text-align: left;">
                                                                    <asp:TextBox ID="txtCurrentPassword" runat="server" Width="200px" ClientIDMode="Static" CssClass="inputElement"
                                                                       Placeholder="Current Password"  ValidationGroup="ChangePassword" TextMode="Password" MaxLength="50"></asp:TextBox>
                                                                    <asp:RequiredFieldValidator ID="req_txtCurrentPassword" runat="server" ErrorMessage="Enter Currenct Password"
                                                                        ControlToValidate="txtCurrentPassword" ValidationGroup="ChangePassword" Display="None">
                                                                    </asp:RequiredFieldValidator>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <%--<td>
                                                                    <req>New Password :</req>
                                                                </td>--%>
                                                                <td style="text-align: left;">
                                                                    <asp:TextBox ID="txtNewPassword" runat="server" ClientIDMode="Static" ValidationGroup="ChangePassword" CssClass="inputElement"
                                                                      Placeholder="New Password"  TextMode="Password" Width="200px" onkeyup="divPasswordwstrength()"></asp:TextBox>
                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidatortxtNewPassword" runat="server"
                                                                        ErrorMessage="Enter New Password" ControlToValidate="txtNewPassword" ValidationGroup="ChangePassword"
                                                                        Display="None">
                                                                    </asp:RequiredFieldValidator>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <%--<td>
                                                                    <req>Confirm Password :</req>
                                                                </td>--%>
                                                                <td style="text-align: left;">
                                                                    <asp:TextBox ID="txtConfirmNewPassword" runat="server" ClientIDMode="Static" ValidationGroup="ChangePassword" CssClass="inputElement"
                                                                        TextMode="Password" Width="200px" Placeholder="Confirm Password"
                                                                        onkeyup="compareCPw();"></asp:TextBox>
                                                                    <%--onblur="Javascript:CheckPassword();"--%>
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
                                                               <%-- <td></td>--%>
                                                                <td style="text-align: left;">
                                                                    <div id="divPasswordwstrength">
                                                                    </div>
                                                                    <span id="PasswordwstrengthMsg" style="font-size: 11px; margin-top: -11px; margin-right: 8px; float: right;"></span>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <%--<td></td>--%>
                                                                <td style="text-align: left;">
                                                                    <input type="button" value="Save" id="btnSaveChangePassword" runat="server" onclick="SaveChangePassword()"  />                                                                    
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                        </table>
                                        <%--</center>--%>
                                    </div>

                                </div>
                            </td>
                        </tr>
                    </table>
                </center>
            </div>

        </div>
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
                    showAlert("New Password And Confirm Should Be Same","Error");
                    document.getElementById("txtConfirmNewPassword").value = "";
                    document.getElementById("txtNewPassword").value = "";
                    document.getElementById("txtNewPassword").focus;
                    dpw.style.width = "0px";
                    msg.innerHTML = "";
                    return false;
                }
                else if (ConfirmNewPassword.length < 8 || ConfirmNewPassword.length > 32) {
                    showAlert("Password length should be grater than 8 characters and less than 32 characters","Error");
                    document.getElementById("txtConfirmNewPassword").value = "";
                    document.getElementById("txtNewPassword").value = "";
                    document.getElementById("txtNewPassword").focus;
                    dpw.style.width = "0px";
                    msg.innerHTML = "";
                    return false;
                }
                else if (!ConfirmNewPassword.match(pass)) {
                    showAlert("Password should be combination of alphabets, numbers, special characters.","Error");
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

        function OnSuccessPMCheckPassword(result) {
            var loginName = document.getElementById("lblLoginName").innerHTML;
            var CurrentPassword = document.getElementById("txtCurrentPassword").value;
            var NewPassword = document.getElementById("txtNewPassword").value;
            var ConfirmNewPassword = document.getElementById("txtConfirmNewPassword").value;

            var dpw = document.getElementById("divPasswordwstrength");
            var msg = document.getElementById("PasswordwstrengthMsg");
            if (result == "PasswordFound") {
                showAlert("You Have Already Used This Password.","Error");
                document.getElementById("txtNewPassword").value = "";
                document.getElementById("txtConfirmNewPassword").value = "";
                document.getElementById("txtNewPassword").focus();
                dpw.style.width = "0px";
                msg.innerHTML = "";
                return false;
            } else if (result == "SameDay") {
                showAlert("You Can Change Password Only Once In A Day...","Error");
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
                showAlert("Password changed successfully !!!", "info", "../Login/Login.aspx");                          
            }
            else {
                showAlert("Password change failed.\n -Invalid Current [ Old ] Password","Error");
                document.getElementById("txtCurrentPassword").focus();
                document.getElementById("txtCurrentPassword").select();
            }
            //  LoadingOff();
        }
        function OnFailPMSaveChangePassword() {
            showAlert("Password change failed.\n -Please re-enter your values and try again","Error");
            clearChangePassword();
        }

    </script>   
</asp:Content>

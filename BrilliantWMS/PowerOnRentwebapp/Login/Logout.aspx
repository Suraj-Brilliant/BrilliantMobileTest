<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Logout.aspx.cs" Inherits="BrilliantWMS.Login.Logout" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<script src="../PopupMessages/Scripts/message.js" type="text/javascript"></script>
<link href="../App_Themes/Login.css" rel="stylesheet" type="text/css" />
<script src="../App_Themes/PIE.js" type="text/javascript"></script>
<script src="../App_Themes/PIE_uncompressed.js" type="text/javascript"></script>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>GWC</title>
</head>
<body>
    <form runat="server">
    <div style="position: relative; left: -20px; top: -7px;">
        <img src="../App_Themes/Blue/img/Background.jpg" />
    </div>
    <div>
        <center>
            <table style="margin: 0px 0 0 0;">
                <tr>
                    <td>
                    </td>
                    <td>
                    </td>
                    <td class="font">
                        Sign in
                    </td>
                </tr>
                <tr>
                    <td>
                        <center>
                            <table>
                                <tr>
                                    <td style="text-align: center;">
                                        <asp:Image ID="ClientLogo" runat="server" ImageUrl="#" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: center; vertical-align: bottom;">
                                        <asp:Image ID="ElegantLogo" runat="server" ImageUrl="~/MasterPage/Logo/ElegantCRM.png" />
                                    </td>
                                </tr>
                            </table>
                        </center>
                    </td>
                    <td>
                        <img src="../MasterPage/Logo/Partitionimg.png" />
                    </td>
                    <td>
                        <asp:Label ID="lblSessionMsg" runat="server" Text="Your session has been expired.."
                            Style="color: Red; font-weight: bold; font-size: 13px"></asp:Label>
                        <asp:Login ID="loginuser" runat="server" EnableViewState="false" RenderOuterTable="false"
                            OnLoggedIn="loginuser_OnLoggedIn" OnLoginError="loginuser_OnLoginError">
                            <LayoutTemplate>
                                <span style="color: Red; font-size: 12px">
                                    <asp:Literal ID="FailureText" runat="server"></asp:Literal>
                                </span>
                                <asp:ValidationSummary ID="validationsummary1" runat="server" ShowMessageBox="true"
                                    ShowSummary="false" DisplayMode="bulletlist" ValidationGroup="loginuservalidationgroup" />
                                <table class="tableForm" cellspacing="5">
                                    <tr>
                                        <td style="text-align: left;">
                                            <req>User Name</req>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: left;">
                                            <asp:TextBox ID="username" runat="server" Width="200px" MaxLength="50" CssClass="inputElement"
                                                ClientIDMode="Static"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="usernamerequired" runat="server" ControlToValidate="username"
                                                ErrorMessage="User Name is required" ValidationGroup="loginuservalidationgroup"
                                                Display="None"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: left;">
                                            <req>Password</req>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: left;">
                                            <asp:TextBox ID="password" runat="server" TextMode="password" MaxLength="20" Width="200px"
                                                CssClass="inputElement" ClientIDMode="Static"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="passwordrequired" runat="server" ControlToValidate="password"
                                                ErrorMessage="Password is required" ValidationGroup="loginuservalidationgroup"
                                                Display="None"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: left;">
                                            <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/login/ForgotPassword.aspx">Forgot Password</asp:HyperLink>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right;">
                                            <asp:Button ID="loginbutton" runat="server" CommandName="login" Text="Login" ValidationGroup="loginuservalidationgroup" />
                                            <input type="button" value="Reset" onclick="ClearTextboxes();" />
                                        </td>
                                    </tr>
                                </table>
                            </LayoutTemplate>
                        </asp:Login>
                    </td>
                </tr>
                <tr>
                    <td colspan="3" style="padding: 10px 0 0 0; border-bottom: 1px solid gray">
                    </td>
                </tr>
                <tr style="color: Gray; font-size: 14px;">
                    <td colspan="2" style="text-align: left;">
                        <a target="_blank" href="http://brilliantinfosys.com/">©2012 Brilliant Info Systems
                            Pvt Ltd</a>
                    </td>
                    <td style="text-align: right;">
                        <a href="http://www.elegantcrm.com/asp/contact.aspx">Contact </a>|<a href="http://www.elegantcrm.com/asp/contact.aspx">
                            Support</a>
                    </td>
                </tr>
                <tr>
                    <td colspan="3" style="padding: 10px 0 0 0;">
                    </td>
                </tr>
                <tr>
                    <td colspan="3" style="text-align: right;">
                        <a>Best viewed in resolution 1280 x 768 and above. Best viewed in browser Mozilla Firefox
                            and Google Chrome.</a>
                    </td>
                </tr>
            </table>
        </center>
    </div>
    </form>
    <script type="text/javascript" language="javascript">
        function ClearTextboxes() {
            document.getElementById('username').value = '';
            document.getElementById('password').value = '';
        }
    </script>
</body>
</html>

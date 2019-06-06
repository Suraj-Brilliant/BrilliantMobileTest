<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="BrilliantWMS.Login.Login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<script src="../PopupMessages/Scripts/message.js" type="text/javascript"></script>
<link href="../App_Themes/Login.css" rel="stylesheet" type="text/css" />
<script src="../App_Themes/PIE.js" type="text/javascript"></script>
<script src="../App_Themes/PIE_uncompressed.js" type="text/javascript"></script>
<link rel="icon" type="image/x-icon" href="../MasterPage/Logo/brillwmslogoNEW.ico" />
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Brilliant WMS | Login</title>
    <style>
        body
        {
            margin: 0px;
            padding: 0px;
        }
        .divLoginHolder table
        {
            width: 300px;
        }
        .divLoginHolder
        {
            width: 380px;
            border: solid 1px #ffffff;
            border-radius: 13px;
            box-shadow: 0px 0px 10px #636363;
            font-size: 14px;
            font-family: Arial;
            margin-bottom: 30px;
            overflow: hidden;
        }
        .divLoginHolder req
        {
            color: #4088BF;
            font-weight: bold;
        }
        .divLoginHolder input, .divLoginHolder select
        {
            font-size: 14px;
            font-family: Arial;
            padding: 10px 10px;
            border-radius: 13px;
            border: solid 1px #cccccc;
            width: 300px !important;
            color: #636363;
        }
        .divLoginHolder select
        {
            width: 324px !important;
        }
        .divLoginHolder input[type="button"], .divLoginHolder input[type="submit"], .divLoginHolder input[type="reset"]
        {
            width: 321px !important;
            color: #ffffff;
        }
        .divLoginBoxHeader
        {
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
        .divLoginBoxContent
        {
            padding: 20px;
            background-image: url(../company/background/login-box-bg.png);
            padding-top: 0px;
        }
        .pnlLoginBg
        {
            background-color: #000000;
            position: fixed;
            top: 0px;
            left: 0px;
            width: 100%;
            height: 100%;
        }
        .pnlLoginBg img
        {
            opacity: 0.5;
        }
        .pnlLoginContent
        {
            position: absolute;
            z-index: 999;
            display: table;
            width: 100%;
            height: 100%;
        }
        .pnlLoginContentSub
        {
            display: table-cell;
            vertical-align: middle;
            text-align: center;
        }
        .loginFooter
        {
            background-color: #000000;
            position: absolute;
            bottom: 0px;
            left: 0px;
            opacity: 0.8 !important;
            width: 100%;
        }
        .loginFooter table:first-child
        {
            width: 90%;
        }
        .loginFooter td, .loginFooter div, .loginFooter a
        {
            color: #ffffff;
        }
        .tlbPoweredBy
        {
            width: auto !important;
        }
        .spnFailureMsg
        {
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
</head>
<body>
    <div class="pnlLoginBg">
        <asp:Image Width="100%" Height="100%" runat="server" ImageUrl="~/Company/Background/login-bg.jpg" />
    </div>
    <div class="pnlLoginContent">
        <div class="pnlLoginContentSub">
            <form runat="server">
            <div>
                <center>
                    <table style="margin: 0px 0 0 0;">
                        <tr>
                            <td>
                            </td>
                            <td>
                            </td>
                            <td class="font">
                                <!--  Sign in -->
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3" align="center">
                                <div class="divLoginHolder">
                                    <!--<div class="divLoginBoxHeader">Sign In</div>-->
                                    <div class="divLoginBoxContent">
                                        <table class="tableForm" cellspacing="5" style="position: relative; top: 10px;">
                                            <tr>
                                                <td align="center">
                                                    <asp:Image ID="ElegantLogo" runat="server" ImageUrl="~/MasterPage/Logo/BrilliantWMSLogoOrange.png"
                                                        Style="position: relative; left: 5px; width: 300px; margin-bottom: 10px;" />
                                                </td>
                                            </tr>
                                            <!-- <tr>
                             <td style="text-align: left;">
                                 <req>Select Language</req>
                             </td>
                         </tr>-->
                                            <tr>
                                                <td style="text-align: left;">
                                                    <asp:DropDownList ID="ddllanguage" runat="server" Width="205px">
                                                        <%--onselectedindexchanged="ddllanguage_SelectedIndexChanged"--%>
                                                        <asp:ListItem Value="" Text="-- Select Language --" />
                                                        <asp:ListItem Value="en-US" Text="English" />
                                                        <asp:ListItem Value="ar-QA" Text="العربية" />
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                        </table>
                                        <asp:Label ID="lblSessionMsg" runat="server" Text="Your session has been expired.."
                                            Style="color: Red; font-weight: bold; font-size: 13px"></asp:Label>
                                        <asp:Login ID="loginuser" runat="server" EnableViewState="false" RenderOuterTable="false"
                                            OnLoggedIn="loginuser_OnLoggedIn" OnLoginError="loginuser_OnLoginError">
                                            <LayoutTemplate>
                                                <asp:ValidationSummary ID="validationsummary1" runat="server" ShowMessageBox="true"
                                                    ShowSummary="false" DisplayMode="bulletlist" ValidationGroup="loginuservalidationgroup" />
                                                <table class="tableForm" cellspacing="5">
                                                    <tr>
                                                        <!-- <tr>
                                        <td style="text-align: left;">
                                            <req>User Name</req>
                                        </td>
                                    </tr> -->
                                                        <tr>
                                                            <td style="text-align: left;">
                                                                <asp:TextBox ID="username" runat="server" Width="200px" MaxLength="50" CssClass="inputElement"
                                                                    ClientIDMode="Static" Placeholder="User Name"></asp:TextBox>
                                                                <asp:RequiredFieldValidator ID="usernamerequired" runat="server" ControlToValidate="username"
                                                                    ErrorMessage="User Name is required" ValidationGroup="loginuservalidationgroup"
                                                                    Display="None"></asp:RequiredFieldValidator>
                                                            </td>
                                                        </tr>
                                                        <!--  <tr>
                                        <td style="text-align: left;">
                                            <req>Password</req>
                                        </td>
                                    </tr> -->
                                                        <tr>
                                                            <td style="text-align: left;">
                                                                <asp:TextBox ID="password" placeholder="Password" runat="server" TextMode="password"
                                                                    MaxLength="20" Width="200px" CssClass="inputElement" ClientIDMode="Static"></asp:TextBox>
                                                                <asp:RequiredFieldValidator ID="passwordrequired" runat="server" ControlToValidate="password"
                                                                    ErrorMessage="Password is required" ValidationGroup="loginuservalidationgroup"
                                                                    Display="None"></asp:RequiredFieldValidator>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="text-align: left;">
                                                                <asp:Button ID="loginbutton" runat="server" CommandName="login" Text="Login" ValidationGroup="loginuservalidationgroup"
                                                                    Width="200px" />
                                                                <!--<input type="button" value="Reset" onclick="ClearTextboxes();" />-->
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="text-align: center;">
                                                                <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/login/ForgotPassword.aspx">Forgot Password</asp:HyperLink>
                                                              
                                                            </td>
                                                            <%--<td>
                                                                <asp:Label ID="Note" runat="server" Text="Best Viewed In Internet Explorer 11 & Above"></asp:Label>
                                                            </td>--%>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <span class="spnFailureMsg" id="spnFailureMsg" style="display: none;">
                                                                    <asp:Literal ID="FailureText" runat="server"></asp:Literal>
                                                                </span>
                                                                <script>
                                                                    var getErrorHolder = document.getElementById('spnFailureMsg');
                                                                    if (getErrorHolder != null) {
                                                                        var getErrHtml = (getErrorHolder.innerHTML).trim();
                                                                        if (getErrHtml != '') {
                                                                            getErrorHolder.style.display = '';
                                                                        }
                                                                    }
                                                                </script>
                                                            </td>
                                                        </tr>
                                                </table>
                                            </LayoutTemplate>
                                        </asp:Login>
                                    </div>
                                </div>
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
        </div>
    </div>
    <div class="loginFooter">
        <table cellpadding="0" cellspacing="0" border="0" width="100%" align="center">
            <tr>
                <td colspan="3" style="padding: 10px 0 0 0; border-top: 1px dashed gray">
                </td>
            </tr>
            <tr style="color: Gray; font-size: 14px;">
                <td colspan="2" style="text-align: left; vertical-align: top;">
                    <%--<a target="_blank" href="http://brilliantinfosys.com/">©2013 Cummins Middle East
                        </a>--%>
                    <a target="_blank" href="#">©2017 Brilliant WMS</a> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Release Version 1.0
                </td>
                
                <td style="text-align: right;">
                    <table align="right" class="tlbPoweredBy">
                        <tr>
                            <td>
                                Best Viewed In Internet Explorer 11 & Above, Google Chrome & Mozilla Firefox
                            </td>
                        </tr>
                        <%--<tr>
                            <td style="vertical-align: top; text-align: right;">
                                Powered by
                            </td>
                            <td style="text-align: left;">
                                <asp:Image ID="Image1" runat="server" ImageUrl="~/MasterPage/Logo/Brill-logo.png"
                                    Height="31px" Width="104px" />
                            </td>
                        </tr>--%>
                        <!--   <tr>
                                <td colspan="2">
                                    <a href="http://brilliantinfosys.com/">Contact </a>
                                </td>
                            </tr>-->
                    </table>
                </td>
            </tr>
            <!-- <tr>
                    <td colspan="3" style="text-align: right;">
                        <br />
                        <br />
                        <br />
                    </td>
                </tr>
                <tr>
                    <td colspan="3" style="text-align: right;">
                        <a>Best viewed in resolution 1280 x 768 and above. Best viewed in browser Mozilla Firefox
                            and Google Chrome.</a>
                    </td>
                </tr> -->
        </table>
    </div>
</body>
</html>

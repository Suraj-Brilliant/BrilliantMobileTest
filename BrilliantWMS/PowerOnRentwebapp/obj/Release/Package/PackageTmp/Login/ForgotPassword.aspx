<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ForgotPassword.aspx.cs"
    Inherits="BrilliantWMS.Login.ForgotPassword" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<script src="../PopupMessages/Scripts/message.js" type="text/javascript"></script>
<link href="../App_Themes/Login.css" rel="stylesheet" type="text/css" />
<script src="../App_Themes/PIE.js" type="text/javascript"></script>
<script src="../App_Themes/PIE_uncompressed.js" type="text/javascript"></script>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="ForgotPwHead1" runat="server">
    <title>GWC</title>
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
            padding: 10px 20px 0px;
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
        <asp:Image ID="Image1" Width="100%" Height="100%" runat="server" ImageUrl="~/Company/Background/login-bg.jpg" />
    </div>
    <div class="pnlLoginContent">
        <div class="pnlLoginContentSub">
            <form runat="server">
            <%-- <div style="position: relative; left: -20px; top: -7px;">
                <img src="../App_Themes/Blue/img/Background.jpg" />
            </div>--%>
            <div>
                <center>
                    <table style="margin: 0px 0 0 0;" border="0">
                        <%--<tr>
                            <td>
                            </td>
                            <td>
                            </td>
                            <td class="font">
                                Forgot Password
                            </td>
                        </tr>--%>
                        <tr>
                            <%-- <td>
                                <center>
                                    <table>
                                        <tr>
                                            <td>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: center; vertical-align: bottom;">
                                                <asp:Image ID="ElegantLogo1" runat="server" ImageUrl="~/MasterPage/Logo/GWCBrandNewLogo.png" />
                                            </td>
                                        </tr>
                                    </table>
                                </center>
                            </td>
                            <td>
                                <img src="../MasterPage/Logo/Partitionimg.png" />
                            </td>--%>
                            <td>
                                <div class="divLoginHolder">
                                    <div class="divLoginBoxContent">
                                        <asp:PasswordRecovery ID="PasswordRecovery1" runat="server">
                                            <UserNameTemplate>
                                                <asp:ValidationSummary ID="validationsummary1" runat="server" ShowMessageBox="true"
                                                    ShowSummary="false" DisplayMode="bulletlist" ValidationGroup="PasswordRecovery1" />
                                                <table class="tableForm" cellspacing="5" border="0">
                                                    <tr>
                                                        <td align="center">
                                                            <asp:Image ID="ElegantLogo" runat="server" ImageUrl="~/MasterPage/Logo/GWCBrandNewLogo.png"
                                                                Style="position: relative; left: 5px; width: 150px; margin-bottom: 10px;" />
                                                        </td>
                                                    </tr>
                                                    <%-- <tr>
                                                        <td>
                                                            <req> User Name :</req>
                                                        </td>
                                                    </tr>--%>
                                                    <tr>
                                                        <td style="text-align: center; font-size: 25px; padding: 0 0 0 15px;">
                                                            Forgot Password
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="text-align: left;">
                                                            <asp:TextBox ID="UserName" runat="server" Width="200px" CssClass="inputElement" Placeholder="User Name"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName"
                                                                ErrorMessage="User Name is required." ToolTip="User Name is required." Display="None"
                                                                ValidationGroup="PasswordRecovery1"></asp:RequiredFieldValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="text-align: left;">
                                                            <asp:Button ID="SubmitButton" runat="server" CommandName="Submit" Text="Submit" ValidationGroup="PasswordRecovery1" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="font-size: 12px; text-align: center;">
                                                            Enter your User Name to receive your password
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="color: red; font-size: 12px; text-align: left;">
                                                            <span class="spnFailureMsg" id="spnFailureMsg" style="display: none;">
                                                                <asp:Literal ID="FailureText" runat="server" EnableViewState="False"></asp:Literal>
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
                                            </UserNameTemplate>
                                            <SuccessTemplate>
                                                <table class="tableForm">
                                                    <tr>
                                                        <td>
                                                            Your password has been sent to your email address
                                                            <br />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:HyperLink ID="hyperlink1" runat="server" NavigateUrl="~/Login/Login.aspx">Go To Login</asp:HyperLink>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </SuccessTemplate>
                                        </asp:PasswordRecovery>
                                    </div>
                                </div>
                            </td>
                        </tr>
                        <%--<tr>
                            <td colspan="3" style="padding: 10px 0 0 0; border-bottom: 1px solid gray">
                            </td>
                        </tr>
                        <tr style="color: Gray; font-size: 14px;">
                            <td colspan="2" style="text-align: left;">
                                <a target="_blank" href="#">©2016 GWC Ltd</a>
                            </td>
                            <td style="text-align: right;">
                                <a>Contact </a>|<a> Support</a>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3" style="padding: 10px 0 0 0;">
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3" style="text-align: right;">
                                <a>Best viewed in Firefox and Google chrome</a>
                            </td>
                        </tr>--%>
                    </table>
                </center>
            </div>
            </form>
        </div>
    </div>
</body>
</html>

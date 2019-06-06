<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Document.aspx.cs" Inherits="BrilliantWMS.Document.Document" %>

<%@ Register Assembly="obout_Interface" Namespace="Obout.Interface" TagPrefix="obout" %>
<%@ Register Assembly="obout_Grid_NET" Namespace="Obout.Grid" TagPrefix="obout" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<script src="../MasterPage/JavaScripts/crm.js" type="text/javascript"></script>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>GWC</title>
</head>
<body>
    <form id="form_Document" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManagerDocument" runat="server" EnablePageMethods="true"
        EnablePartialRendering="true">
    </asp:ToolkitScriptManager>
    <div>
        <asp:HiddenField ID="hdDownLoadAccessIDs" runat="server" ViewStateMode="Enabled"  ClientIDMode="Static" />
        <asp:HiddenField ID="hdnDeleteAccessIDs" runat="server" ViewStateMode="Enabled" ClientIDMode="Static" />
        <center>
            <div id="divLoading" style="height: 250px; width: 765px; display: none" class="modal">
                <center>
                    <br />
                    <span id="spanLoading" style="font-size: 17px; font-weight: bold; color: Yellow;">Processing
                        please wait...</span>
                </center>
            </div>
            <asp:ValidationSummary ID="validationsummary_Document" runat="server" ShowMessageBox="True"
                ShowSummary="False" ValidationGroup="DocumentValidation" />
            <table class="gridFrame" style="margin: 2px 2px 2px 2px; width: 750px">
                <tr>
                    <td style="text-align: left;">
                        <asp:Label class="headerText" ID="lblDocumentFormHeader" runat="server">Add New Document</asp:Label>
                    </td>
                    <td style="width: 20%">
                        <table style="float: right;">
                            <tr>
                                <td>
                                    <asp:Button ID="btnUploadDocuemnt" runat="server" Text="Upload Files" ValidationGroup="DocumentValidation"
                                        OnClick="upload_LinkBtn_Click" OnClientClick="DocumentLoadingOn()" />
                                </td>
                                <td>
                                    <input type="button" id="btnDocumentClear" value="Clear" runat="server" onclick="ClearMode('divDocument');" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td style="background-color: White;" colspan="2">
                        <div id="divDocument">
                            <table class="tableForm">
                                <tr>
                                    <td>
                                        <req><asp:Label ID="lblDocumentTitle" runat="server" Text="Document Title"></asp:Label></req>
                                    </td>
                                    :
                                    <td style="text-align: left">
                                        <asp:TextBox ID="txtDocTitle" runat="server" MaxLength="50" onblur="CheckDocumentTitle()"
                                            ValidationGroup="DocumentValidation" Width="250px"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="reqValDocTitle" runat="server" ControlToValidate="txtDocTitle"
                                            ValidationGroup="DocumentValidation" ErrorMessage="Enter document title" Display="None"></asp:RequiredFieldValidator>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblDocumentType" runat="server" Text="Document Type"></asp:Label>
                                    </td>
                                    :
                                    <td style="text-align: left">
                                        <asp:DropDownList ID="ddlDocumentType" runat="server" Width="180px">
                                            <asp:ListItem Text="-Select-" Value="0" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="Photo ID" Value="Photo ID"></asp:ListItem>
                                            <asp:ListItem Text="Passport" Value="Passport"></asp:ListItem>
                                            <asp:ListItem Text="LPO" Value="LPO"></asp:ListItem>
                                            <asp:ListItem Text="Purchase Order" Value="Purchase Order"></asp:ListItem>
                                            <asp:ListItem Text="Invoice" Value="Invoice"></asp:ListItem>
                                            <asp:ListItem Text="Signature" Value="Signature"></asp:ListItem>
                                            <asp:ListItem Text="Other" Value="Other"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblDescription" runat="server" Text="Description"></asp:Label>
                                    </td>
                                    :
                                    <td>
                                        <asp:TextBox ID="txtDocDesc" runat="server" MaxLength="500" Width="250px" TextMode="MultiLine"
                                            onkeyup="TextBox_KeyUp(this,'CharactersCounterDocDesc','500');" ClientIDMode="Static"
                                            Height="50px"></asp:TextBox>
                                        <br />
                                        <span class="watermark"><span id="CharactersCounterDocDesc">500</span> characters remaining
                                            out of 500 </span>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblKeyWords" runat="server" Text="Key Words"></asp:Label>
                                    </td>
                                    :
                                    <td>
                                        <asp:TextBox ID="txtKeyword" runat="server" MaxLength="500" Width="250px" TextMode="MultiLine"
                                            onkeyup="TextBox_KeyUp(this,'CharactersCounterDocKeyWords','500');" ClientIDMode="Static"
                                            Height="50px"></asp:TextBox>
                                        <br />
                                        <span class="watermark"><span id="CharactersCounterDocKeyWords">500</span> characters
                                            remaining out of 500 </span>
                                    </td>
                                </tr>
                                <tr >
                                    <td style="visibility:hidden;">
                                        Access To :
                                    </td>
                                    <td style="text-align: left; visibility:hidden">
                                        <obout:OboutRadioButton ID="rbtnPublic" runat="server" Text="Public" GroupName="rbtnAccessTo"
                                            Checked="true">
                                        </obout:OboutRadioButton>
                                        <obout:OboutRadioButton ID="rbtnSelf" runat="server" Text="Self" GroupName="rbtnAccessTo">
                                        </obout:OboutRadioButton>
                                        <obout:OboutRadioButton ID="rbtnPrivate" runat="server" Text="Private" GroupName="rbtnAccessTo"
                                            CausesValidation="false">
                                            <ClientSideEvents OnClick="openUserList" />
                                        </obout:OboutRadioButton>
                                    </td>
                                    <td>
                                        <req><asp:Label ID="lblSelectDocument" runat="server" Text="Select Document"></asp:Label></req>
                                    </td>
                                    :
                                    <td>
                                        <asp:FileUpload ID="FileUploadDocument" runat="server" />
                                        <asp:RequiredFieldValidator ID="ReqVal" runat="server" ControlToValidate="FileUploadDocument"
                                            ErrorMessage="Select document to upload" ValidationGroup="DocumentValidation"
                                            Display="None">
                                        </asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <span class="watermark">Max Limit 20 MB</span>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: left; width: 80%">
                    </td>
                    <td style="width: 20%">
                        <table style="float: right;">
                            <tr>
                                <td>
                                    <asp:Button ID="btnUploadDocuemnt2" runat="server" Text="Upload Files" ValidationGroup="DocumentValidation"
                                        OnClick="upload_LinkBtn_Click" OnClientClick="DocumentLoadingOn()" />
                                </td>
                                <td>
                                    <input type="button" id="btnDocumentClear2" runat="server" value="Clear" onclick="ClearMode('divDocument');" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </center>
    </div>
    <script type="text/javascript">
        function openUserList() {
            window.open('../UserManagement/UserList.aspx', null, 'height=400px, width=709px,status= 0, resizable= 0, scrollbars=0, toolbar=0,location=center,menubar=0, screenX=300; screenY=50');
        }

        function CheckDocumentTitle() {
            var DocumentTitle = document.getElementById("<%= txtDocTitle.ClientID %>").value;
            PageMethods.CheckDocumentTitle(DocumentTitle, OnSucessCheckDuplicateDocument, OnfailedCheckDuplicateDocument);
        }

        function OnSucessCheckDuplicateDocument(result) {
            var txtDocTitle = document.getElementById("<%= txtDocTitle.ClientID %>");
            if (result != "") {
                alert(result); txtDocTitle.value = '';
            }
        }
        function OnfailedCheckDuplicateDocument() {
        }

        function onSuccessTempSaveDocument() {
            if (window.opener.GvDocument != null) {
                window.opener.GvDocument.refresh();
            }
            alert("Document saved successfully");
            LoadingOff();
            self.close();
        }

        function DocumentLoadingOn() {
            if (typeof (Page_ClientValidate) == 'function') {
                Page_ClientValidate();
            }
            if (Page_IsValid) {
                LoadingOn();
            }
        }

        
    </script>
    </form>
</body>
</html>

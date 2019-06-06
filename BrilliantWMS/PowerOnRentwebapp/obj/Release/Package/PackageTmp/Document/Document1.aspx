<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/CRM2.Master" AutoEventWireup="true"
    CodeBehind="Document1.aspx.cs" Inherits="BrilliantWMS.Document.Document1" %>

<%@ Register Assembly="obout_Interface" Namespace="Obout.Interface" TagPrefix="obout" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table class="tableForm">
        <tr>
            <td>
                <req> Document Title : </req>
            </td>
            <td>
                <asp:TextBox ID="txtDocTitle" runat="server" MaxLength="50" onKeyPress="return alpha(this,event);"
                    onblur="CheckDocumentTitle()" ValidationGroup="DocumentValidation" Width="250px"></asp:TextBox>
                <asp:RequiredFieldValidator ID="reqValDocTitle" runat="server" ControlToValidate="txtDocTitle"
                    ValidationGroup="DocumentValidation" ErrorMessage="Enter Document Title" Display="None"></asp:RequiredFieldValidator>
            </td>
            <td>
                Description :
            </td>
            <td style="text-align: left;">
                <asp:TextBox ID="txtDocDesc" runat="server" MaxLength="500" Width="250px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                Key Words :
            </td>
            <td>
                <asp:TextBox ID="txtKeyword" runat="server" onkeypress="alpha(event);" MaxLength="500"
                    Width="250px"></asp:TextBox>
            </td>
            <td>
                <req>Attach Document :</req>
            </td>
            <td>
                <asp:FileUpload ID="FileUploadDocument" runat="server" Width="250px" />
                <asp:RequiredFieldValidator ID="ReqVal" runat="server" ControlToValidate="FileUploadDocument"
                    ErrorMessage="File Required!" ValidationGroup="DocumentValidation" Display="None">
                </asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td>
                Access To :
            </td>
            <td style="text-align: left;" colspan="2">
                <obout:OboutRadioButton ID="rbtnSelf" runat="server" Text="Self" GroupName="rbtnAccessTo"
                    FolderStyle="">
                </obout:OboutRadioButton>
                <obout:OboutRadioButton ID="rbtnPublic" runat="server" Text="Public" GroupName="rbtnAccessTo"
                    Checked="true" FolderStyle="">
                </obout:OboutRadioButton>
                <obout:OboutRadioButton ID="rbtnPrivate" runat="server" Text="Private" GroupName="rbtnAccessTo"
                    CausesValidation="false">
                    <ClientSideEvents OnClick="openUserList" />
                </obout:OboutRadioButton>
            </td>
            <td>
                <asp:Button ID="btnUpload" runat="server" Text="Upload Files" ValidationGroup="DocumentValidation"
                    OnClick="upload_LinkBtn_Click" Style="float: right;" />
                <input type="button" value="Upload 1" onclick="jsUploadFile()" />
            </td>
        </tr>
    </table>
    <span class="watemarkText">Max Limit 20MB</span>
    <script type="text/javascript">
        function jsUploadFile() {
            var filupload = document.getElementById(<%= FileUploadDocument.ClientID %>);
            PageMethods.PMUploadFile(filupload,onSuccessFileUpload,onFialFileUpload);
        }
        function onSuccessFileUpload(){alert("Success");}
        function onFialFileUpload(){}
    </script>
</asp:Content>

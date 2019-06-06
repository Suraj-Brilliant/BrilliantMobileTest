<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="addimage.aspx.cs" Inherits="BrilliantWMS.Company.addimage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
     <asp:FileUpload ID="FileUpload1" runat="server" />

<asp:Button ID="btnUpload" runat="server" Text="Upload"

OnClick="btnUpload_Click" /> <br />

    <asp:TextBox ID="txtid" runat="server"></asp:TextBox>



<br />
    </div>
    </form>
</body>
</html>

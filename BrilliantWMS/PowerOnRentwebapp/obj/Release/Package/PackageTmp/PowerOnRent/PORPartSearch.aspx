<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PORPartSearch.aspx.cs"
    Inherits="BrilliantWMS.PowerOnRent.PORPartSearch" %>

<%@ Register Assembly="obout_Grid_NET" Namespace="Obout.Grid" TagPrefix="obout" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <obout:Grid ID="Grid1" runat="server" AllowPageSizeSelection="false" AllowManualPaging="true"
            OnRebind="RebindGrid" FilterType="ProgrammaticOnly">
            <Columns>
                <obout:Column DataField="ID" Visible="false" ID="Column1" runat="server">
                </obout:Column>
                <obout:Column ID="Column2" runat="server" DataField="ProductType" Visible="false"
                    HeaderText="Type" Width="0%">
                </obout:Column>
                <obout:Column ID="Column3" runat="server" DataField="ProductCode" HeaderText="Product Code"
                    Align="left" HeaderAlign="left" Width="13%">
                </obout:Column>
                <obout:Column ID="Column4" runat="server" DataField="Name" HeaderText="Product Name"
                    Align="left" HeaderAlign="left" Width="20%">
                </obout:Column>
            </Columns>
        </obout:Grid>
    </div>
    </form>
</body>
</html>

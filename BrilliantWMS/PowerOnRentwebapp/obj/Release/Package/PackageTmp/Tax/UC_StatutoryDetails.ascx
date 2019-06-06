<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UC_StatutoryDetails.ascx.cs" Inherits="BrilliantWMS.Tax.UC_StatutoryDetails" %>
<asp:ListView ID="LstStatutoryInfo" runat="server" GroupItemCount="3" Style="margin-bottom: 0px;
    width: 1000px">
    <GroupTemplate>
        <tr id="itemPlaceholderContainer" runat="server">
            <td id="itemPlaceholder" runat="server">
            </td>
        </tr>
    </GroupTemplate>
    <ItemTemplate>
        <td id="Td1" runat="server" style="">
            <table width="100%">
                <tr>
                    <td style="width: 150px;">
                        <asp:Label ToolTip='<%# Eval("StatutoryID") %>' ID="lblname" runat="server" Text='<%# Eval("Name") %>' />
                        :
                    </td>
                    <td style="text-align: left;" align="left">
                        <asp:TextBox ID="textbox" MaxLength="50" runat="server" Width="200px" Text='<%# Bind("StatutoryValue") %>'></asp:TextBox>
                    </td>
                </tr>
            </table>
        </td>
    </ItemTemplate>
    <LayoutTemplate>
        <%--<table runat="server" class="tableForm">
            <tr runat="server">
                <td runat="server">--%>
        <table id="groupPlaceholderContainer" runat="server" width="100%" cellpadding="0"
            cellspacing="0" class="tableForm">
            <tr id="groupPlaceholder" runat="server">
            </tr>
        </table>
        <%--</td>
            </tr>
        </table>--%>
    </LayoutTemplate>
</asp:ListView>
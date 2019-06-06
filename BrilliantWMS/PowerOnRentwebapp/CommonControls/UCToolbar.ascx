<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCToolbar.ascx.cs" Inherits="BrilliantWMS.CommonControls.UCToolbar" %>
<%@ Register Assembly="obout_Flyout2_NET" Namespace="OboutInc.Flyout2" TagPrefix="cc2" %>
<%--<asp:UpdatePanel ID="UpdatePanelToolbar" runat="server">
    <ContentTemplate>
--%>
<div id="dvbtn">
    <table style="margin-top: 0px; float: right;" id="tabletoolbar">
        <tr>            
            <td  id="tdEdit">
                <input type="button" value="Edit" runat="server" id="btnEdit" onclick="changemode(false,'tabletest'); CheckBtn(this);"
                    style="width: 0px; height: 0px; visibility:hidden;" clientidmode="Static" />
            </td>
            <td class="tdWidth">
                <asp:Button ID="btnAddNew" runat="server" Text="Add New" ToolTip="Add New" OnClick="ImgbtnAddNew_Click"
                    CausesValidation="false" Height="24px" Width="65px" class="buttonON" ClientIDMode="Static" />
            </td>
            <td class="tdWidth">
                <asp:Button ID="btnSave" runat="server" Text="Save" ToolTip="Save" OnClick="ImgbtnSave_Click"
                    ValidationGroup="Save" CssClass="FixWidth" ClientIDMode="Static" />
            </td>
            <td class="tdWidth">
                <asp:Button ID="btnClear" runat="server" Text="Clear" ToolTip="Clear" OnClick="ImgbtnClear_Click"
                    CausesValidation="false" CssClass="FixWidth" ClientIDMode="Static" />
            </td>
            <%--<td class="tdWidth">
                <asp:Button ID="btnExport" runat="server" Text="Export" ToolTip="Export" CausesValidation="false" ClientIDMode="Static" style="width:0px; visibility:hidden;" 
                    CssClass="FixWidth" />
            </td>
            <td class="tdWidth">
                <asp:Button ID="btnImport" runat="server" Text="Import" ToolTip="Import" OnClick="ImgbtnImport_Click" ClientIDMode="Static"  style="width:0px; visibility:hidden;"
                    CausesValidation="false" CssClass="FixWidth" />
            </td>
            <td class="tdWidth">
                <asp:Button ID="btmMail" runat="server" Text="Mail" ToolTip="Mail" CausesValidation="false" ClientIDMode="Static"  style="width:0px; visibility:hidden;"
                    CssClass="FixWidth" />
            </td>
            <td class="tdWidth">
                <asp:Button ID="btnPrint" runat="server" Text="Print" CausesValidation="false" ToolTip="Print" ClientIDMode="Static"  style="width:0px; visibility:hidden;"
                    OnClick="btnPrint_Click" OnClientClick="GetReferenceIDs_ForPrint()" CssClass="FixWidth" />
            </td>
            <td style="width: 74px;" runat="server" id="tdConvertTo">
                <input type="button" value="Convert To" id="btnConvertTo" runat="server" onclick="fillConvertTo()" style="width:0px; visibility:hidden;"
                    class="FixWidth" />
            </td>
            <td>
                
                <asp:ImageButton ID="btnHelp1" runat="server" ToolTip="Help" CausesValidation="false"
                    ImageUrl="../App_Themes/Blue/img/help24.png" CssClass="help" />
            </td>--%>
        </tr>
    </table>
</div>
<style type="text/css">
    /*Rajendra 03-Jan-2012*/
    /*HTML Toolbar*/
    .buttonON
    {
        width: 72px;
        height: 24px;
    }
    /**/
</style>
<asp:HiddenField ID="hdnUCToolbarCurrentObject" ClientIDMode="Static" runat="server">
</asp:HiddenField>

<asp:HiddenField ID="hdnUCToolbarReferenceID" runat="server" ClientIDMode="Static"></asp:HiddenField>



<script type="text/javascript">
   

</script>

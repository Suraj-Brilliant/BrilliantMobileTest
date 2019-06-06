<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Toolbar.ascx.cs" Inherits="BrilliantWMS.CommonControls.Toolbar" %>
<div id="dvbtn">
    <asp:HiddenField runat="server" ID="hdnIsFormChange" ClientIDMode="Static" />
    <table style="margin-top: 0px; float: right;" id="tabletoolbar">
        <tr>
            <td>
                <input type="button" value="Add New" runat="server" id="btnAddNew" class="buttonON"
                    clientidmode="Static" />
            </td>
            <td>
                <input type="button" value="Edit" runat="server" id="btnEdit" class="buttonON"
                    clientidmode="Static" />
            </td>
            <td>
                <input type="button" value="Save" runat="server" id="btnSave" class="buttonON" clientidmode="Static"
                    validationgroup="Save" />
            </td>
            <td>
                <input type="button" value="Clear" runat="server" id="btnClear" class="buttonON"
                    clientidmode="Static" causesvalidation="false" />
            </td>
           <%-- <td>
                <input type="button" value="Export" runat="server" id="btnExport" class="Off buttonON" style="width:0px;"
                    clientidmode="Static" causesvalidation="false" />
            </td>--%>
            <td>
                <input type="button" value="Import" runat="server" id="btnImport" class="buttonON" 
                    clientidmode="Static"  /> <%-- causesvalidation="false"--%>
            </td>
           <%--  <td>
                <input type="button" value="Mail" runat="server" id="btnMail" title="Not-Allowed" style="width:0px;"
                    class="Off buttonON" clientidmode="Static" causesvalidation="false" />
            </td>
            <td>
                <input type="button" value="Print" runat="server" id="btnPrint" title="Not-Allowed" style="width:0px;"
                    class="Off buttonON" clientidmode="Static" causesvalidation="false" />
            </td>
            <td runat="server" id="tdConvertTo">
                <input type="button" value="ConvertTo" runat="server" id="btnConvertTo" title="Not-Allowed" style="width:0px;"
                    class="Off buttonON" clientidmode="Static" causesvalidation="false" />
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
<script type="text/javascript">
    function ToolbarEndableDisable() {

    }

    function jsbtnAddNew_onclick() {
        //You have attempted to Add New Request.  If you have made any changes to the fields without clicking the Save button, your changes will be lost.  Are you sure you want to exit this page?"
        //You have attempted to leave this page.  If you have made any changes to the fields without clicking the Save button, your changes will be lost.  Are you sure you want to exit this page?"
    }

    function jsbtnSave_onclick() {
        jsSaveData();
    }

    function jsbtnEdit_onclick() {
        jsEditData();
    }

</script>

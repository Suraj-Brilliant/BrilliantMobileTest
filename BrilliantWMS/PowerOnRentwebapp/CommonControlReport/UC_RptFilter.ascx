<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UC_RptFilter.ascx.cs" Inherits="BrilliantWMS.CommonControlReport.UC_RptFilter" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<%--<link href="CSS/style.css" rel="stylesheet" />--%>
<script src="http://code.jquery.com/jquery-1.10.2.js"></script>
<script src="http://code.jquery.com/ui/1.11.2/jquery-ui.js"></script>

<%--<link href="CSS/userinfo.css" rel="stylesheet" />

<link href="CSS/general.css" rel="stylesheet" />--%>
<%@ Register Assembly="obout_Grid_NET" Namespace="Obout.Grid" TagPrefix="obout" %>
<%@ Register Assembly="obout_Interface" Namespace="Obout.Interface" TagPrefix="obout" %>
<%@ Register TagPrefix="obout" Namespace="Obout.Grid" Assembly="obout_Grid_NET" %>

<script src="../MasterPage/JavaScripts/Report.js" type="text/javascript"></script>
<%--<asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true"></asp:ScriptManager>--%>

<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <table>
            <tr>

                <td>

                    <asp:PlaceHolder ID="Rpt_placeH" runat="server"></asp:PlaceHolder>
                    <table>
                        <tr style="height: 10px">
                        </tr>
                        <tr></tr>
                        <tr>
                            <td style="width: 170px">

                                <asp:Button ID="btnExecute" runat="server" Text="Execute" OnClientClick="return ShowGrid();" ClientIDMode="Static" OnClick="ShowGrid_Click" />
                            </td>
                            <td style="width: 170px"></td>
                            <td style="width: 170px">

                                <asp:Button ID="btnShowList" runat="server" Text="Show List Report" OnClientClick="return  selectedRec();" Visible="false"/>
                            </td>
                            <td style="width: 170px">

                                <asp:Button ID="btnShowDtls" runat="server" Text="Show Detail Report" OnClientClick="return selectedRecDtls();"  Visible="false"/>
                            </td>
                            <%--     <td>
                       </td>--%>
                        </tr>

                    </table>

                </td>
            </tr>
            <tr>
                <td style="vertical-align: top;">

                    <table id="Table1" runat="server" class="gridFrame" width="650px" style="margin: 3px 3px 3px 3px; vertical-align: top;">
                        <tr>
                            <td>
                                <table style="width: 80%">
                                    <tr>
                                        <td style="text-align: left;">
                                            <a class="headerText">List</a>
                                        </td>
                                        <td></td>
                                        <td>
                                            <table style="width: 100%">
                                                <tr>
                                                    <td style="text-align: right;">
                                                        <input type="checkbox" id="Checkbox1" />
                                                        <a class="headerText">Select All</a>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>

                                <obout:Grid ID="GvList" runat="server" AllowMultiRecordSelection="true" AutoGenerateColumns="true"
                                    ClientIDMode="Static" KeepSelectedRecords="true" AllowRecordSelection="true" Width="650px" AllowGrouping="true" AllowFiltering="true"
                                    PageSize="5" AllowPaging="true" AllowPageSizeSelection="true" AllowManualPaging="true"
                                    OnRebind="ShowGrid_Click"
                                    Serialize="true" CallbackMode="true" AllowColumnReordering="true">
                                    <ScrollingSettings ScrollHeight="250" ScrollWidth="650" ScrollLeft="200" ScrollTop="250" />
                                    <ClientSideEvents ExposeSender="true" />
                                </obout:Grid>


                            </td>

                        </tr>
                        <tr>
                            <td></td>
                        </tr>

                    </table>
                    <asp:Label ID="lblText" runat="server" Text=""></asp:Label>
                    <div id="divImage" style="display: none" class="center">
                        <%--                          <asp:Image ID="img1" runat="server" ImageUrl="loader.gif" />--%>
                        <b>Loading...</b>
                    </div>



                </td>
            </tr>
        </table>
        <asp:HiddenField ID="hdnSelAll" runat="server" ClientIDMode="Static" EnableViewState="false" />
        <asp:HiddenField ID="hdnIsSubRpt" runat="server" ClientIDMode="Static" EnableViewState="false" />
    </ContentTemplate>
</asp:UpdatePanel>

<asp:HiddenField ID="hdnVenderId" runat="server" ClientIDMode="Static" />
<asp:HiddenField ID="hdnUserId" runat="server" ClientIDMode="Static" />
<asp:HiddenField ID="hdnWareHouseId" runat="server" ClientIDMode="Static" />


<asp:HiddenField ID="hdnSelVal" runat="server" ClientIDMode="Static" />
<asp:HiddenField ID="hdnCustId" runat="server" ClientIDMode="Static" />
<asp:HiddenField ID="hdnRptId" runat="server" ClientIDMode="Static" />


<script type="text/javascript">
    Sys.Application.add_init(appl_init);

    function appl_init() {
        var pgRegMgr = Sys.WebForms.PageRequestManager.getInstance();
        pgRegMgr.add_beginRequest(BeginHandler);
        pgRegMgr.add_endRequest(EndHandler);
    }


    function EndHandler() {
        afterAsyncPostBack();
    }


</script>

<style type="text/css">
    /*Calendar Control CSS*/


    .ui-datepicker
    {
        background: #333;
        color: #EEE;
        font-size: larger;
    }


    body
    {
        margin: 0;
        padding: 0;
        font-family: Arial;
    }

    .modal
    {
        position: fixed;
        z-index: 999;
        height: 100%;
        width: 100%;
        top: 0;
        background-color: Black;
        filter: alpha(opacity=60);
        opacity: 0.6;
        -moz-opacity: 0.8;
    }

    .center
    {
        z-index: 100;
        margin: 100px auto;
        padding: 10px;
        width: 130px;
        background-color: White;
        border-radius: 10px;
        filter: alpha(opacity=100);
        opacity: 1;
        -moz-opacity: 1;
    }
    	
.loader{
 position: fixed;
  left: 0px;
  top: 0px;
  width: 100%;
  height: 100%;
  z-index: 9999;
  background: url('loader.gif') 50% 50% no-repeat rgb(249,249,249);
}
        .center img
        {
            height: 128px;
            width: 128px;
        }
</style>

<script type="text/javascript">
    // Get the instance of PageRequestManager.
    var prm = Sys.WebForms.PageRequestManager.getInstance();
    // Add initializeRequest and endRequest
    prm.add_initializeRequest(prm_InitializeRequest);
    prm.add_endRequest(prm_EndRequest);

    // Called when async postback begins
    function prm_InitializeRequest(sender, args) {
        // get the divImage and set it to visible
        var panelProg = $get('divImage');
        panelProg.style.display = '';
        // reset label text
        var lbl = $get('<%= this.lblText.ClientID %>');
        lbl.innerHTML = '';

        // Disable button that caused a postback
        $get(args._postBackElement.id).disabled = true;
    }

    // Called when async postback ends
    function prm_EndRequest(sender, args) {
        // get the divImage and hide it again
        var panelProg = $get('divImage');
        panelProg.style.display = 'none';

        // Enable button that caused a postback
        $get(sender._postBackSettings.sourceElement.id).disabled = false;
    }
</script>


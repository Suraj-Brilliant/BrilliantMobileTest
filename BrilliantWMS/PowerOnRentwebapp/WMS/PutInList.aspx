<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/CRM.Master" AutoEventWireup="true" CodeBehind="PutInList.aspx.cs"
    Inherits="BrilliantWMS.Warehouse.PutInList" EnableEventValidation="false" Theme="Blue" %>

<%@ Register Assembly="obout_Flyout2_NET" Namespace="OboutInc.Flyout2" TagPrefix="obout" %>
<%@ Register Assembly="Obout.Ajax.UI" Namespace="Obout.Ajax.UI.FileUpload" TagPrefix="obout" %>
<%@ Register Assembly="obout_Interface" Namespace="Obout.Interface" TagPrefix="obout" %>
<%@ Register Assembly="obout_Grid_NET" Namespace="Obout.Grid" TagPrefix="obout" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../CommonControls/UCFormHeader.ascx" TagName="UCFormHeader" TagPrefix="uc1" %>
<%@ Register Src="../CommonControls/UCToolbar.ascx" TagName="UCToolbar" TagPrefix="uc2" %>
<%@ Register Src="../Document/UC_AttachDocument.ascx" TagName="UC_AttachDocument"
    TagPrefix="uc3" %>
<%@ Register Src="../CommonControls/Toolbar.ascx" TagName="Toolbar" TagPrefix="uc2" %>
<%@ Register Src="../CommonControls/UC_Date.ascx" TagName="UC_Date" TagPrefix="uc4" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHolder_FormHeader" runat="server">
    <uc2:Toolbar ID="Toolbar1" runat="server" />
    <uc1:UCFormHeader ID="UCFormHeader1" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPHolder_Form" runat="server">
    <style type="text/css">
        /*POR Collapsable Div*/

        .divHead {
            border: solid 2px #F5DEB3;
            width: 99%;
            text-align: left;
            margin-top: -5;
        }

            .divHead h4 {
                /*color: #33CCFF;*/
                color: #483D8B;
                margin: 3px 3px 3px 3px;
            }

            .divHead a {
                float: right;
                margin-top: -15px;
                margin-right: 5px;
            }

                .divHead a:hover {
                    cursor: pointer;
                    color: Red;
                }

        /*End POR Collapsable Div*/
    </style>
    <center>
        <asp:UpdatePanel ID="UPTBPutIn" runat="server">
            <ContentTemplate>
                <asp:TabContainer ID="TCPutIn" runat="server">
                    <asp:TabPanel ID="TabPutIn" runat="server" HeaderText="PutIn List">
                        <ContentTemplate>
                            <asp:UpdateProgress ID="UPGRNGridProgress" runat="server" AssociatedUpdatePanelID="UpPnlGridPutIn">
                                <ProgressTemplate>
                                    <center>
                                        <div class="modal">
                                            <img src="../App_Themes/Blue/img/ajax-loader.gif" style="top: 50%;" />
                                        </div>
                                    </center>
                                </ProgressTemplate>
                            </asp:UpdateProgress>
                            <asp:UpdatePanel ID="UpPnlGridPutIn" runat="server">
                                <ContentTemplate>
                                    <%--<div class="divHead">--%>
                                    <h4 id="h1" runat="server"></h4>
                                    <table style="float: right; font-size: 15px; font-weight: bold; color: Black;">
                                        <%-- margin-top: -25px;--%>
                                        <tr>
                                            <td style="padding-right: 10px; padding-bottom: 0px; padding-top: 0px;">
                                                <input type="button" id="btnGenerateGroupTask" title="Generate Group Task" value="Generate Group Task" runat="server" onclick="GenerateGroupTask()" />
                                                 <input type="button" id="Button1" title="Change Status" value="Change Status" runat="server" />
                                                <input type="button" id="btnCreateTask" title="Create Task" value="Create Task" runat="server" onclick="CreateTask()" />
                                            </td>
                                            <td>
                                                <div class="PORgray"></div>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblHeading" runat="server" Text="Not Applicable"></asp:Label>
                                            </td>
                                            <td>&nbsp;
                                            </td>
                                            <td>
                                                <div class="PORgreen"></div>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblCompleted" runat="server" Text="Completed"></asp:Label>
                                            </td>
                                            <td>&nbsp;
                                            </td>
                                            <td>
                                                <div class="PORred"></div>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblPending" runat="server" Text="Pending"></asp:Label>
                                            </td>
                                            <td>&nbsp;
                                            </td>
                                            <td>
                                                <div class="PORorange"></div>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblCancelled" runat="server" Text="Cancelled"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                    <%--</div>--%>
                                    <div class="divDetailExpand" id="divlinkPutInList">
                                        <center>
                                            <table class="gridFrame" width="100%">
                                                <tr>
                                                    <td>
                                                        <table style="width: 100%">
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lblPutInlist" Style="color: white; font-size: 15px; font-weight: bold;" runat="server" Text="Put In List"></asp:Label>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <obout:Grid ID="grdPutIn" runat="server" AutoGenerateColumns="false" AllowAddingRecords="false" AllowFiltering="true" AllowGrouping="true" AllowMultiRecordSelection="false"
                                                            AllowRecordSelection="true" Width="100%" AllowColumnResizing="true" AllowColumnReordering="true">
                                                            <Columns>
                                                                <obout:Column DataField="ID" HeaderText="PutIn No" HeaderAlign="center" Align="center" Width="5%"></obout:Column>
                                                                <obout:Column DataField="POOrderNo" HeaderText="PO No" HeaderAlign="center" Align="center" Width="7%"></obout:Column>
                                                                <%--<obout:Column DataField="GroupTaskName" HeaderText="Group Task Name" HeaderAlign="left" Align="left" Width="10%" Wrap="true"></obout:Column>--%>
                                                                <obout:Column DataField="PutInDate" HeaderText="PutIn Date" HeaderAlign="left" Align="left" DataFormatString="{0:dd-MM-yyyy}" Width="10%"></obout:Column>
                                                                <obout:Column DataField="VendorName" HeaderText="Vendor Name" HeaderAlign="left" Align="left" Width="10%" Wrap="true"></obout:Column>
                                                                <obout:Column DataField="PutInUser" HeaderText="PutIn By" HeaderAlign="left" Align="left" Width="10%"></obout:Column>
                                                                <obout:Column DataField="StatusName" HeaderText="Status" HeaderAlign="left" Align="left" Width="8%" Wrap="true"></obout:Column>
                                                                <obout:Column DataField="ImgPutIn" HeaderText="Put In" HeaderAlign="center" Align="center" Width="7%">
                                                                    <TemplateSettings TemplateId="PIStatusPI" />
                                                                </obout:Column>
                                                            </Columns>
                                                            <Templates>
                                                                <obout:GridTemplate ID="PIStatusPI" runat="server">
                                                                    <Template>
                                                                        <center>
                                                                            <div class='<%# ("POR" + Container.Value) %>' onclick="RequestOpenEntryForm('PutIn','<%# Container.Value %>', '<%# Container.DataItem["ID"] %>')"></div>
                                                                        </center>
                                                                    </Template>
                                                                </obout:GridTemplate>
                                                            </Templates>
                                                        </obout:Grid>
                                                    </td>
                                                </tr>
                                            </table>
                                        </center>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </ContentTemplate>
                    </asp:TabPanel>
                </asp:TabContainer>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:HiddenField ID="hdnselectedPO" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnSelectedRec" runat="server" ClientIDMode="Static" />
    </center>
    <script type="text/javascript">


        window.onload = function () {
            oboutGrid.prototype.restorePreviousSelectedRecord = function () {
                return;
            }
            oboutGrid.prototype.markRecordAsSelectedOld = oboutGrid.prototype.markRecordAsSelected;
            oboutGrid.prototype.markRecordAsSelected = function (row, param2, param3, param4, param5) {
                if (row.className != this.CSSRecordSelected) {
                    this.markRecordAsSelectedOld(row, param2, param3, param4, param5);

                } else {
                    var index = this.getRecordSelectionIndex(row);
                    if (index != -1) {
                        this.markRecordAsUnSelected(row, index);
                    }
                }
                selectPORec();
            }
        }

        function selectPORec() {
            var hdnselectedPO = document.getElementById("hdnselectedPO");
            hdnselectedPO.value = "";
            for (i = 0; i < grdPutIn.PageSelectedRecords.length; i++) {
                var record = grdPutIn.PageSelectedRecords[i];
                if (hdnselectedPO.value != "") hdnselectedPO.value += ',' + record.ID;
                if (hdnselectedPO.value == "") hdnselectedPO.value = record.ID;
            }
        }

        function Grid1_Deselect(index) {
            selectPORec();
        }
        //function jsAddNew() {
        //    PageMethods.WMpageAddNew(jsAddNewOnSuccess, null);
        //}

        //function jsAddNewOnSuccess() {
        //    window.open('../Warehouse/PutIn.aspx', '_self', '');
        //}
        function jsImport() {
            PageMethods.WMpageAddNew(jsImportOnSuccess, null);
        }
        function jsImportOnSuccess() {
            window.open('../POR/DImportPO.aspx', '_self', '');
        }

        // ************************  Code By Pallavi *********************************************************************
        function selectedRec() {
            var selectedPO = document.getElementById("hdnselectedPO").value;
            if (selectedPO == "") {
                // showAlert("Please Select Purchase Orders.", 'Error', '#');//  original code
                window.open('../Warehouse/CreateGroup.aspx', null, 'height=120, width=800,status= 0, resizable= 1, scrollbars=0, toolbar=0,location=center,menubar=0, screenX=300; screenY=50');                // temparory code
            }
            else {
                PageMethods.WMGetPOData(selectedPO, jsGetPODataOnSuccess, null);
            }
        }

        function jsGetPODataOnSuccess(result) {
            if (result == "1") {
                alert("     Please Select Purchase Orders of Status Entered Only  and Purchase Order of NoGroup.    ", "nGroup");
            }
            else {

                window.open('../POR/CreateGroup.aspx?Pinvoker=' + result + '', null, 'height=120, width=800,status= 0, resizable= 1, scrollbars=0, toolbar=0,location=center,menubar=0, screenX=300; screenY=50');
            }
        }





        function selectedRecReceive() {
            var selectedPO = document.getElementById("hdnselectedPO").value;
            if (selectedPO == "") {
                window.open('../Warehouse/PutIn.aspx');                 // temporary code new code
                // showAlert("Please Select Purchase Orders Or Purchase Order Group.", 'Error', '#'); // old correct code
            } else {
                PageMethods.GetAllPOsOfGroup(selectedPO, GetAllPOsOnSuccess, null);
                //PageMethods.ValidateGreenPOStatus(selectedPO, selectedRecReceiveOnSuccess, null); -- call this later

            }
        }
        function GetAllPOsOnSuccess(response) {
            var selectedPO = document.getElementById("hdnselectedPO");
            if (response != "") {
                if (confirm("This action will select all purchase orders from related group.Do you want to continue?") == true) {

                    selectedPO.value = "";
                    selectedPO.value = response;
                    // PageMethods.ValidateGreenPOStatus(response, selectedRecReceiveOnSuccess, null);
                }

            }
            else {

                PageMethods.ValidateGreenPOStatus(selectedPO.value, selectedRecReceiveOnSuccess, null); // original code old
            }
        }
        function selectedRecReceiveOnSuccess(response) {
            var selectedPO = document.getElementById("hdnselectedPO").value;
            if (response == "1") {
                document.getElementById("hdnselectedPO").value = "";
                window.open('../Warehouse/PutIn.aspx?PO=' + selectedPO, '_self', '');
            }
            else {
                showAlert("Please Select Purchase Orders which are not already received!", 'Error', '#');
            }
        }
        function selectedRecStatus() {
            var selectedPO = document.getElementById("hdnselectedPO").value;

            if (selectedPO == "") {
                showAlert("Please Select Purchase Orders to change the status.", 'Error', '#');
            } else {
                if (confirm("Do you want to mark status for this request / Master Ticket as Completed?") == true) {

                    PageMethods.WMChangePOStatus(selectedPO, selRecStatOnSuccess, null);
                }

            }
            function selRecStatOnSuccess(response) {
                var selectedPO = document.getElementById("hdnselectedPO").value;
                if (response == "0") {
                    showAlert("This request is pending for receiving ; Please complete receiving process for this Request / MCN and then mark this as completed", "nGroup");
                }
                else if (response == "2") {
                    showAlert("This request is already received ; Please select only those Request / MCN whose receiving process is completed and status is WIP!", "nGroup");
                }

                else {
                    PageMethods.WMChangePOStatusNow(selectedPO, WMChngPOStatNowOnSuccess, null);
                }
            }

        }
        function WMChngPOStatNowOnSuccess(result) {
            //  alert("Hi");
            showAlert("Status changed successfully", 'Error', '#');
            window.Grid1.refresh();
        }

        //****************************************************************************************************************


        function CreateTask() {
            var hdnselectedPO = document.getElementById('hdnselectedPO');
            if (hdnselectedPO.value == "") {
                showAlert('Please Select Atleast One Record For Assign Task!!!', 'Error', '#');
            } else {
                PageMethods.WMCheckStatus(hdnselectedPO.value, OnSuccessCheckStatus, null);
            }
            //window.open('../WMS/AssignTask.aspx', null, 'height=350, width=500,status= 0, resizable= 1, scrollbars=0, toolbar=0,location=center,menubar=0, screenX=300; screenY=50');
        }
        function OnSuccessCheckStatus(result) {
            if (result == 1) {
                window.open('../WMS/AssignTask.aspx', null, 'height=350, width=500,status= 0, resizable= 1, scrollbars=0, toolbar=0,location=center,menubar=0, screenX=300; screenY=50');
            } else if (result == 0) {
                showAlert('One Or More Record Have Already Job Card Number Assigned. Please Select Not Assigned Order!!!', 'Error', '#');
            } else {
                showAlert('Please Select Record With Same Status!!!', 'Error', '#');
            }
        }

        function RequestOpenEntryForm(invoker, state, requestID) {
            window.open('../WMS/PutIn.aspx', '_self', '');
        }
    </script>
</asp:Content>

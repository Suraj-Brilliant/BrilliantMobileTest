<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/CRM.Master" AutoEventWireup="true"
    CodeBehind="MessageDefination.aspx.cs" Inherits="BrilliantWMS.PowerOnRent.MessageDefination"
    EnableEventValidation="false" %>

<%@ Register Src="../CommonControls/UCFormHeader.ascx" TagName="UCFormHeader" TagPrefix="uc2" %>
<%@ Register Src="../CommonControls/UCToolbar.ascx" TagName="UCToolbar" TagPrefix="uc2" %>
<%@ Register Src="../CommonControls/UC_Date.ascx" TagName="UC_Date" TagPrefix="ucd55" %>
<%@ Register Assembly="obout_Grid_NET" Namespace="Obout.Grid" TagPrefix="obout" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHolder_FormHeader" runat="server">
    <uc2:UCToolbar ID="UCToolbar1" runat="server" />
    <uc2:UCFormHeader ID="UCFormHeader1" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPHolder_Form" runat="server">
    <asp:UpdatePanel ID="UpdatePanelTabPanelProductList" runat="server">
        <ContentTemplate>
            <asp:ValidationSummary ID="validationsummary_MessageDefination" runat="server" ShowMessageBox="true"
                ShowSummary="false" DisplayMode="bulletlist" ValidationGroup="Save" />
            <asp:HiddenField ID="hdnprodID" runat="server" />
            <asp:TabContainer runat="server" ID="tabContainerReqTemplate">
                <asp:TabPanel ID="tblInterfaceDefLst" runat="server" HeaderText="Message Defination List">
                    <ContentTemplate>
                        <asp:UpdateProgress ID="UpdateGirdProductProcess" runat="server" AssociatedUpdatePanelID="Up_PnlGirdInterfaceDef">
                            <ProgressTemplate>
                                <center>
                                    <div class="modal">
                                        <img src="../App_Themes/Blue/img/ajax-loader.gif" style="top: 50%;" />
                                    </div>
                                </center>
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                        <asp:UpdatePanel ID="Up_PnlGirdInterfaceDef" runat="server">
                            <ContentTemplate>
                                <%-- <div class="divHead" style="visibility:hidden;">
                                    <h4 id="h4DivHead" runat="server">
                                    </h4>
                                </div>--%>
                                <%-- <div class="divDetailExpand" id="divlinkRequestsList">--%>
                                <center>
                                    <table class="gridFrame" width="80%">
                                        <tr>
                                            <td>
                                                <table style="width: 80%">
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lmlmessagedef" Style="color: white; font-size: 15px; font-weight: bold;"
                                                                runat="server" Text="Message Defination List"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <obout:Grid ID="GVMessage" runat="server" AllowAddingRecords="False" AllowFiltering="True"
                                                    AllowGrouping="True" AutoGenerateColumns="False" Width="100%" PageSize="10">
                                                    <Columns>
                                                        <obout:Column HeaderText="Edit" DataField="Id" Width="5%" Align="center" HeaderAlign="center">
                                                            <TemplateSettings TemplateId="TemplateEdit" />
                                                        </obout:Column>
                                                        <obout:Column DataField="description" HeaderText="Title" HeaderAlign="left" Align="left"
                                                            Width="8%">
                                                        </obout:Column>
                                                        <obout:Column DataField="remark" HeaderText="Purpose" HeaderAlign="left" Align="left"
                                                            Width="8%">
                                                        </obout:Column>
                                                        <obout:Column DataField="Destination" HeaderText="Destination" HeaderAlign="left"
                                                            Align="left" Width="8%">
                                                        </obout:Column>
                                                        <obout:Column DataField="ActionType" HeaderText="Type" HeaderAlign="left" Align="left"
                                                            Width="5%">
                                                        </obout:Column>
                                                        <obout:Column DataField="TableName" HeaderText="Object" HeaderAlign="left" Align="left"
                                                            Width="5%">
                                                        </obout:Column>
                                                    </Columns>
                                                    <Templates>
                                                        <obout:GridTemplate runat="server" ID="TemplateEdit">
                                                            <Template>
                                                                <asp:ImageButton ID="imgBtnEdit1" runat="server" ImageUrl="../App_Themes/Blue/img/Edit16.png"
                                                                    OnClick="imgBtnEdit_OnClick" ToolTip='<%# (Container.Value) %>' CausesValidation="false" />
                                                            </Template>
                                                        </obout:GridTemplate>
                                                    </Templates>
                                                </obout:Grid>
                                            </td>
                                        </tr>
                                    </table>
                                </center>
                                <%-- </div>--%>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </ContentTemplate>
                </asp:TabPanel>
                <asp:TabPanel ID="tbTemplateDetail" runat="server" HeaderText="Message Defination Detail">
                    <ContentTemplate>
                        <asp:UpdateProgress ID="UpdateProgress_ProductDetails" runat="server" AssociatedUpdatePanelID="Uppnl_TemplateDetails">
                            <ProgressTemplate>
                                <center>
                                    <div class="modal">
                                        <img src="../App_Themes/Blue/img/ajax-loader.gif" style="top: 50%;" />
                                    </div>
                                </center>
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                        <asp:UpdatePanel ID="Uppnl_TemplateDetails" runat="server">
                            <ContentTemplate>
                                <center>
                                    <table class="tableForm" border="0" width="75%">
                                        <tr>
                                            <td style="text-align: right; vertical-align: middle;">
                                                <req> <asp:Label ID="lblTitle" runat="server" Text="Title"></asp:Label> </req>
                                                :
                                            </td>
                                            <td colspan="0" style="text-align: left;">
                                                <asp:TextBox ID="txtTitle" runat="server" Width="40%"></asp:TextBox>&nbsp;&nbsp;&nbsp;&nbsp;
                                                <asp:RequiredFieldValidator ID="RFVtxtTitle" runat="server" ErrorMessage="Enter Title"
                                                    ControlToValidate="txtTitle" ValidationGroup="Save" Display="None"></asp:RequiredFieldValidator>
                                                <req><asp:Label ID="lblDestination" runat="server" Text="Destination"></asp:Label> </req>
                                                :
                                                <asp:DropDownList ID="ddlDestination" runat="server" Width="182px" DataValueField="Id"
                                                    ClientIDMode="Static" DataTextField="Value">
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="RFVddlDestination" runat="server" ErrorMessage="Select Destination"
                                                    ControlToValidate="ddlDestination" InitialValue="0" ValidationGroup="Save" Display="None"></asp:RequiredFieldValidator>
                                                &nbsp;&nbsp;&nbsp;&nbsp;
                                                <req> <asp:Label ID="lblType" runat="server" Text="Type"></asp:Label></req>
                                                :
                                                <asp:DropDownList ID="ddlType" runat="server" Width="182px" DataValueField="Id" DataTextField="Value">
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="RFVddlType" runat="server" ErrorMessage="Select Type"
                                                    ControlToValidate="ddlType" InitialValue="0" ValidationGroup="Save" Display="None"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right;">
                                                <req> <asp:Label ID="lblPurpose" runat="server" Text="Purpose"></asp:Label> </req>
                                                :
                                            </td>
                                            <td style="text-align: left;" colspan="0">
                                                <asp:TextBox ID="txtPurpose" runat="server" Width="97%" TextMode="MultiLine"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RFVtxtPurpose" runat="server" ErrorMessage="Enter Purpose"
                                                    ControlToValidate="txtPurpose" ValidationGroup="Save" Display="None"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                    </table>
                                    <br />
                                    <table class="tableForm" border="0" width="75%">
                                        <tr>
                                            <td style="text-align: right; width: 70px;">
                                                <req> <asp:Label ID="lblObject" runat="server" Text="Object"></asp:Label> </req>
                                                :
                                            </td>
                                            <td style="text-align: left;">
                                                <asp:DropDownList ID="ddlObject" runat="server" Width="182px" onchange="GetObjectName(this);BindField(this)"
                                                    DataTextField="Value" DataValueField="Id">
                                                </asp:DropDownList>
                                                &nbsp;&nbsp;&nbsp;&nbsp;
                                                <req> <asp:Label ID="lblField" runat="server" Text="Field"></asp:Label></req>
                                                :
                                                <asp:DropDownList ID="ddlField" runat="server" Width="182px" DataTextField="Fieldname"
                                                    DataValueField="Id" onchange="GetField(this)">
                                                </asp:DropDownList>
                                                &nbsp;&nbsp;&nbsp;&nbsp;
                                                <%-- <input type="button" runat="server" id="btnAdd" value="Add" runat="server" />--%>
                                                <req><asp:Label ID="lblSequence" runat="server" Text="Sequence"></asp:Label></req>
                                                :
                                                <asp:TextBox ID="txtSequence" runat="server" MaxLength="3" Width="182px" Style="text-align: right"
                                                    onkeypress="return fnAllowDigitsOnly(event)"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" style="text-align: left; padding-left: 94%;">
                                                <%-- <input type="button" runat="server" id="btnAdd" value="Add" />--%>
                                                <asp:Button runat="server" ID="ADDButton" Text="Add" OnClick="AddBtn_Click" OnClientClick="javascript:return CheckValidation();" />
                                            </td>
                                        </tr>
                                    </table>
                                    <asp:HiddenField ID="hdnSelectedObject" runat="server" ClientIDMode="Static" />
                                    <asp:HiddenField ID="hdnState" runat="server" ClientIDMode="Static" />
                                    <asp:HiddenField ID="hdnSelectedField" runat="server" ClientIDMode="Static" />
                                    <asp:HiddenField ID="hdnMessageID" runat="server" ClientIDMode="Static" />
                                    <table class="gridFrame" width="75%">
                                        <tr>
                                            <td align="left" style="text-align: left">
                                                <asp:Label ID="lblfieldlist" Style="color: white; font-size: 15px; font-weight: bold;"
                                                    runat="server" Text="Field List"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <obout:Grid ID="GVFields" runat="server" AllowAddingRecords="false" AutoGenerateColumns="false"
                                                    AllowGrouping="true" Serialize="false" CallbackMode="true" AllowRecordSelection="false"
                                                    AllowMultiRecordSelection="false" AllowColumnReordering="true" AllowFiltering="true"
                                                    OnRebind="GVFields_OnRebind" Width="100%" PageSize="10">
                                                    <Columns>
                                                        <obout:Column DataField="Id" HeaderText="Remove" Visible="True" AllowEdit="false"
                                                            Width="9%" Align="center" HeaderAlign="left">
                                                            <TemplateSettings TemplateId="ItemTempRemove" />
                                                        </obout:Column>
                                                        <obout:Column DataField="Fieldname" HeaderText="Field Name" Align="left" HeaderAlign="left"
                                                            Width="20%">
                                                        </obout:Column>
                                                        <obout:Column DataField="FieldDataType" HeaderText="Data Type" HeaderAlign="left"
                                                            Align="left" Width="20%">
                                                        </obout:Column>
                                                        <obout:Column DataField="IsNull" HeaderText="Is Null" HeaderAlign="left" Align="left"
                                                            Width="20%">
                                                        </obout:Column>
                                                    </Columns>
                                                    <Templates>
                                                        <obout:GridTemplate ID="ItemTempRemove">
                                                            <Template>
                                                                <table>
                                                                    <tr>
                                                                        <%-- <td style="width: 20px; text-align: center;">--%>
                                                                        <td>
                                                                            <img id="imgbtnRemove" src="../App_Themes/Grid/img/Remove16x16.png" title="Remove"
                                                                                onclick="removeFieldFromList('<%# (Container.DataItem["Id"].ToString()) %>');"
                                                                                style="cursor: pointer;" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </Template>
                                                        </obout:GridTemplate>
                                                    </Templates>
                                                </obout:Grid>
                                            </td>
                                        </tr>
                                    </table>
                                </center>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </ContentTemplate>
                </asp:TabPanel>
            </asp:TabContainer>
        </ContentTemplate>
    </asp:UpdatePanel>
    <style type="text/css">
        /*POR Collapsable Div*/
        
        .PanelCaption
        {
            color: Black;
            font-size: 13px;
            font-weight: bold;
            margin-top: -22px;
            margin-left: -5px;
            position: absolute;
            background-color: White;
            padding: 0px 2px 0px 2px;
        }
        .divHead
        {
            border: solid 2px #F5DEB3;
            width: 99%;
            text-align: left;
        }
        .divHead h4
        {
            /*color: #33CCFF;*/
            color: #483D8B;
            margin: 3px 3px 3px 3px;
        }
        .divHead a
        {
            float: right;
            margin-top: -15px;
            margin-right: 5px;
        }
        .divHead a:hover
        {
            cursor: pointer;
            color: Red;
        }
        .divDetailExpand
        {
            border: solid 2px #F5DEB3;
            border-top: none;
            width: 99%;
            padding: 5px 0 5px 0;
            overflow: hidden;
            height: 92%;
        }
        .divDetailCollapse
        {
            display: none;
        }
        /*End POR Collapsable Div*/
    </style>
    <style type="text/css">
        /*Grid css*/
        .excel-textbox
        {
            background-color: transparent;
            border: 0px;
            padding: 0px;
            outline: 0;
            font: inherit;
            width: 91%;
            padding-top: 4px;
            padding-right: 2px;
            padding-bottom: 4px;
            text-align: right;
        }
        .excel-textbox-focused
        {
            background-color: #FFFFFF;
            border: 0px;
            padding: 0px;
            outline: 0;
            font: inherit;
            width: 91%;
            padding-top: 4px;
            padding-right: 2px;
            padding-bottom: 4px;
            text-align: right;
        }
        
        .excel-textbox-error
        {
            color: #FF0000;
        }
        
        .ob_gCc2
        {
            padding-left: 3px !important;
        }
        
        .ob_gBCont
        {
            border-bottom: 1px solid #C3C9CE;
        }
        
        .excel-checkbox
        {
            height: 20px;
            line-height: 20px;
        }
    </style>
    <script type="text/javascript">
        onload();
        function onload() {
            var v = document.getElementById("btnConvertTo");
            v.style.visibility = 'hidden';
        }

        /*Request Part List*/
        function markAsFocused(textbox) {
            textbox.className = 'excel-textbox-focused';
            textbox.select();
        }

        function markAsBlured(textbox, dataField, rowIndex) {
            textbox.className = 'excel-textbox';

            var txtvalue = textbox.value;
            if (txtvalue == "") txtvalue = 0;
            textbox.value = parseFloat(txtvalue).toFixed(2);
            if (Grid1.Rows[rowIndex].Cells[dataField].Value != textbox.value) {
                Grid1.Rows[rowIndex].Cells[dataField].Value = textbox.value;
                PageMethods.WMUpdateRequestQty(getOrderObject(rowIndex), null, null);
            }
        }

        function getOrderObject(rowIndex) {
            /*Save Request qty into TempData when changed*/
            var order = new Object();
            order.Sequence = Grid1.Rows[rowIndex].Cells['Sequence'].Value;
            order.RequestQty = Grid1.Rows[rowIndex].Cells['RequestQty'].Value;
            return order;
        }

        function GetObjectName() {
            var ddlObject = document.getElementById("<%=ddlObject.ClientID %>");

            var hdnSelectedObject = document.getElementById('hdnSelectedObject');
            hdnSelectedObject.value = ddlObject.value;
        }

        function BindField() {

            var ddlObject = document.getElementById("<%=ddlObject.ClientID %>");
            var selectedobject = ddlObject.options[ddlObject.selectedIndex].text;

            PageMethods.PMGetFieldList(selectedobject, onSuccessGetFieldList, null)
        }
        function onSuccessGetFieldList(result) {
            ddlField = document.getElementById("<%=ddlField.ClientID %>");
            ddlField.options.length = 0;
            var option0 = document.createElement("option");

            if (result.length > 0) {
                option0.text = "--Select--";
                option0.value = '0';
            }
            else {
                option0.text = 'N/A';
                option0.value = '0';
            }

            try {
                ddlField.add(option0, null);
            }
            catch (Error) {
                ddlField.add(option0);
            }

            for (var i = 0; i < result.length; i++) {
                var option1 = document.createElement("option");
                option1.text = result[i].Fieldname;
                option1.value = result[i].Id;
                try {
                    ddlField.add(option1, null);
                }
                catch (Error) {
                    ddlField.add(option1);
                }
            }


        }

        function GetField() {
            var ddlField = document.getElementById("<%=ddlField.ClientID %>");

            var hdnSelectedField = document.getElementById('hdnSelectedField');
            hdnSelectedField.value = ddlField.value;
        }

        function removeFieldFromList(Id) {
            /*Remove Part from list*/
            //            var hdnUserIDs = document.getElementById('hdnUserIDs');
            //            var Hdn1 = document.getElementById('Hdn1');
            //            hdnUserIDs.value = "";
            //            Hdn1.value = "1";
            if (hdnState.value == "Add") {
                PageMethods.WMRemoveFieldFromUserList(Id, RemoveFieldFromListOnSussess, null);
            }
            else if (hdnState.value == "Edit") {
                PageMethods.WMRemoveFieldFromUserListAtEditTime(Id, RemoveFieldFromListAtEditTimeOnSussess, null);
            }
        }
        function RemoveFieldFromListOnSussess() { GVFields.refresh(); }

        function RemoveFieldFromListAtEditTimeOnSussess() { GVFields.refresh(); }

        function CheckValidation() {

            var ddlObject = document.getElementById("<%=ddlObject.ClientID %>").value;
            var ddlField = document.getElementById("<%=ddlField.ClientID %>").value;
            var Sequence = document.getElementById("<%=txtSequence.ClientID %>").value;

            if (ddlObject == "0") {

                alert("Select object");
                document.getElementById("<%=ddlObject.ClientID%>").focus();
                return false;
            }
            else if (ddlField == "0") {

                alert("Select Field");
                document.getElementById("<%=ddlField.ClientID%>").focus();
                return false;
            }
            else if (Sequence == "") {

                alert("Enter Sequence");
                document.getElementById("<%=txtSequence.ClientID%>").focus();
                return false;
            }
            else {
                return true;
            }
        }

        function fnAllowDigitsOnly(key) {
            var keycode = (key.which) ? key.which : key.keyCode;
            if ((keycode < 48 || keycode > 57) && (keycode != 8) && (keycode != 9) && (keycode != 11) && (keycode != 37) && (keycode != 38) && (keycode != 39) && (keycode != 40) && (keycode != 127)) {
                return false;
            }
        }
                     
    </script>
</asp:Content>

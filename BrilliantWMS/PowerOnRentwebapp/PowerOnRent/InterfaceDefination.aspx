<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/CRM.Master" AutoEventWireup="true"
    CodeBehind="InterfaceDefination.aspx.cs" Inherits="BrilliantWMS.PowerOnRent.InterfaceDefination"
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
            <asp:ValidationSummary ID="validationsummary_ProductMaster" runat="server" ShowMessageBox="true"
                ShowSummary="false" DisplayMode="bulletlist" ValidationGroup="Save" />
            <asp:HiddenField ID="hdnprodID" runat="server" />
            <asp:TabContainer runat="server" ID="tabContainerReqTemplate">
                <asp:TabPanel ID="tblInterfaceDefLst" runat="server" HeaderText="Interface Defination List">
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
                                <%--<center>--%>
                                <table class="gridFrame" width="100%">
                                    <tr>
                                        <td>
                                            <table style="width: 100%">
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblTemplate" Style="color: white; font-size: 15px; font-weight: bold;"
                                                            runat="server" Text="Interface Defination List"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <obout:Grid ID="GVInterface" runat="server" AllowAddingRecords="false" AllowFiltering="True"
                                                AllowGrouping="true" AutoGenerateColumns="False" OnSelect="GVInterface_Select"
                                                Width="100%" PageSize="10">
                                                <Columns>
                                                    <obout:Column HeaderText="Edit" DataField="ID" Width="10%" Align="center" HeaderAlign="center">
                                                        <TemplateSettings TemplateId="TemplateEdit" />
                                                    </obout:Column>
                                                    <obout:Column DataField="TableName" HeaderText="Table Name" HeaderAlign="left" Align="left"
                                                        Width="20%">
                                                    </obout:Column>
                                                    <obout:Column DataField="Fieldname" HeaderText="Field Name" HeaderAlign="left" Align="left"
                                                        Width="20%">
                                                    </obout:Column>
                                                    <obout:Column DataField="FieldDataType" HeaderText="Data Type" HeaderAlign="left"
                                                        Align="left" Width="20%">
                                                    </obout:Column>
                                                    <obout:Column DataField="IsNull" HeaderText="Is Null" HeaderAlign="left" Align="left"
                                                        Width="20%">
                                                        <TemplateSettings TemplateId="tplIsNull" />
                                                    </obout:Column>
                                                </Columns>
                                                <Templates>
                                                    <obout:GridTemplate runat="server" ID="TemplateEdit">
                                                        <Template>
                                                            <asp:ImageButton ID="imgBtnEdit1" runat="server" ImageUrl="../App_Themes/Blue/img/Edit16.png"
                                                                OnClick="imgBtnEdit_OnClick" ToolTip='<%# (Container.Value) %>' CausesValidation="false" />
                                                        </Template>
                                                    </obout:GridTemplate>
                                                    <obout:GridTemplate runat="server" ID="tplIsNull">
                                                        <Template>
                                                            <%# (Container.DataItem["IsNull"].ToString() == "Y" ? "Yes" : "No")%>
                                                        </Template>
                                                    </obout:GridTemplate>
                                                </Templates>
                                            </obout:Grid>
                                        </td>
                                    </tr>
                                </table>
                                <%-- </center>--%>
                                <%-- </div>--%>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </ContentTemplate>
                </asp:TabPanel>
                <asp:TabPanel ID="tbTemplateDetail" runat="server" HeaderText="Interface Defination Detail">
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
                                    <table class="tableForm">
                                        <tr>
                                            <td>
                                                <req><asp:Label ID="lblSelectTable" runat="server" Text="Table"></asp:Label></req>
                                                :
                                            </td>
                                            <td align="left" style="text-align: left;">
                                                <asp:DropDownList ID="ddlTable" runat="server" Width="182px" DataTextField="Value"
                                                    DataValueField="Id">
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="RFVddlTable" runat="server" ErrorMessage="Select Table"
                                                    ControlToValidate="ddlTable" InitialValue="0" ValidationGroup="Save" Display="None"></asp:RequiredFieldValidator>
                                            </td>
                                            <td>
                                                <req><asp:Label ID="lblFieldName" runat="server" Text="Field Name"></asp:Label></req>
                                                :
                                            </td>
                                            <td style="text-align: left;">
                                                <asp:TextBox ID="txtFieldName" runat="server" Width="182px"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RFVFieldName" runat="server" ControlToValidate="txtFieldName"
                                                    ErrorMessage="Enter Field Name" ValidationGroup="Save" Display="None"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <req><asp:Label ID="lblDataType" runat="server" Text="Data Type"></asp:Label></req>
                                                :
                                            </td>
                                            <td style="text-align: left;">
                                                <asp:DropDownList ID="ddlDataype" runat="server" Width="182px" DataTextField="Value"
                                                    DataValueField="Id">
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="RFVDatatype" runat="server" ErrorMessage="Select Data Type"
                                                    ControlToValidate="ddlDataype" InitialValue="0" ValidationGroup="Save" Display="None"></asp:RequiredFieldValidator>
                                            </td>
                                            <td>
                                                <req><asp:Label ID="lblIsNull" runat="server" Text="Is Null"></asp:Label></req> :
                                            </td>
                                            <td style="text-align: left;">
                                                <asp:DropDownList ID="ddlIsNull" runat="server" Width="182px">
                                                    <asp:ListItem Selected="True" Text="-Select-" Value="0"></asp:ListItem>
                                                    <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
                                                    <asp:ListItem Text="No" Value="No"></asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="RFVISNull" runat="server" ErrorMessage="Select Is Null"
                                                    ControlToValidate="ddlIsNull" InitialValue="0" ValidationGroup="Save" Display="None"></asp:RequiredFieldValidator>
                                            </td>
                                            <asp:HiddenField ID="hdnSate" runat="server" ClientIDMode="Static" />
                                            <asp:HiddenField ID="hdnInterfaceID" runat="server" ClientIDMode="Static" />
                                            <%--<td>
                                                <asp:Label ID="lblIsUnique" runat="server" Text="Is Unique"></asp:Label>
                                            </td>
                                            <td style="text-align: left;">
                                                <asp:DropDownList ID="ddlUnique" runat="server" Width="182px">
                                                    <asp:ListItem Selected="True" Text="-Select-" Value="0"></asp:ListItem>
                                                    <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
                                                    <asp:ListItem Text="No" Value="No"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>   --%>
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
    </script>
</asp:Content>

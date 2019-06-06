<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/CRM.Master" AutoEventWireup="true" EnableEventValidation="false"
     CodeBehind="DiscountMaster.aspx.cs" Inherits="BrilliantWMS.Product.DiscountMaster" Theme="Blue" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="obout_Grid_NET" Namespace="Obout.Grid" TagPrefix="obout" %>
<%@ Register Assembly="obout_Interface" Namespace="Obout.Interface" TagPrefix="obout" %>
<%@ Register Src="../CommonControls/UC_Date.ascx" TagName="UC_Date" TagPrefix="uc1" %>
<%@ Register Src="../CommonControls/UCFormHeader.ascx" TagName="UCFormHeader" TagPrefix="uc2" %>
<%@ Register Src="../CommonControls/UCToolbar.ascx" TagName="UCToolbar" TagPrefix="uc3" %>
<%@ Register Src="UCProductSearch.ascx" TagName="UCProductSearch" TagPrefix="uc4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHolder_FormHeader" runat="server">
     <uc3:UCToolbar ID="UCToolbar1" runat="server" />
    <uc2:UCFormHeader ID="UCFormHeader1" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPHolder_Form" runat="server">
     <asp:UpdateProgress ID="UpdateProgress_Disc" runat="server" AssociatedUpdatePanelID="updPnl_Disc">
        <ProgressTemplate>
            <center>
                <div class="modal">
                    <img src="../App_Themes/Blue/img/ajax-loader.gif" style="top: 50%;" />
                </div>
            </center>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:ValidationSummary ID="validationsummary_DiscountMaster" runat="server" ShowMessageBox="true"
        ShowSummary="false" DisplayMode="bulletlist" ValidationGroup="Save" />
    <asp:UpdatePanel ID="updPnl_Disc" runat="server">
        <ContentTemplate>
            <center>
                <asp:TabContainer ID="tabDiscountM" runat="server" ActiveTabIndex="0" Width="100%">
                    <asp:TabPanel ID="tabDiscount" runat="server" TabIndex="1">
                        <HeaderTemplate>
                            Discount
                        </HeaderTemplate>
                        <ContentTemplate>
                            <center>
                                <asp:HiddenField ID="hdnDiscountID" runat="server" />
                                <table class="gridFrame">
                                    <tr>
                                        <td>
                                            <table style="width: 100%">
                                                <tr>
                                                    <td style="text-align: left;">
                                                        <a id="headerText">Discount Offers</a>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <obout:Grid ID="gvDiscountM" runat="server" AllowAddingRecords="False" AllowFiltering="True"
                                                AllowGrouping="True" AutoGenerateColumns="False" OnSelect="gvDiscountM_Select">
                                                <Columns>
                                                    <obout:Column ID="Column1" DataField="Edit" Width="75" AllowFilter="False" Align="center"
                                                        HeaderAlign="Center" HeaderText="Edit" Index="0" TemplateId="imgBtnEdit1">
                                                        <TemplateSettings TemplateId="imgBtnEdit1" />
                                                    </obout:Column>
                                                    <obout:Column DataField="ID" HeaderText="ID" Visible="False" Index="1">
                                                    </obout:Column>
                                                    <%-- <obout:Column DataField="Sequence" HeaderText="Sr No." Width="100" Align="center"
                                                        Index="2">
                                                    </obout:Column>--%>
                                                    <obout:Column DataField="Name" HeaderText="Discount Name" Width="250" Index="3">
                                                    </obout:Column>
                                                    <obout:Column DataField="FromDate" HeaderText="From Date" Width="150" Index="4">
                                                    </obout:Column>
                                                    <obout:Column DataField="ToDate" HeaderText="To Date" Width="150" Index="5">
                                                    </obout:Column>
                                                    <obout:Column DataField="Active" HeaderText="Active" Width="80" Index="6">
                                                    </obout:Column>
                                                </Columns>
                                                <Templates>
                                                    <obout:GridTemplate runat="server" ID="imgBtnEdit1" ControlID="" ControlPropertyName="">
                                                        <Template>
                                                            <asp:ImageButton ID="imgBtnEdit" runat="server" ImageUrl="../App_Themes/Blue/img/Edit16.png"
                                                                ToolTip="Edit" CausesValidation="false" />
                                                        </Template>
                                                    </obout:GridTemplate>
                                                </Templates>
                                            </obout:Grid>
                                        </td>
                                    </tr>
                                </table>
                            </center>
                        </ContentTemplate>
                    </asp:TabPanel>
                    <asp:TabPanel ID="tabDiscountProduct" runat="server" TabIndex="2">
                        <HeaderTemplate>
                            Discount Details
                        </HeaderTemplate>
                        <ContentTemplate>
                            <center>
                                <table class="tableForm">
                                    <tr>
                                        <td>
                                            <req>Discount Offers :</req>
                                        </td>
                                        <td colspan="3" style="text-align: left">
                                            <asp:TextBox ID="txtDiscount" runat="server" MaxLength="50" onkeydown="return NotAllowSpecialChar(this, event);"
                                                ValidationGroup="Save" Width="300px">                    
                                            </asp:TextBox>
                                            <asp:RequiredFieldValidator ID="valRftxtDiscount" runat="server" ControlToValidate="txtDiscount"
                                                ErrorMessage="Enter Discount Name" Display="None" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <req>From Date :</req>
                                        </td>
                                        <td style="text-align: left">
                                            <uc1:UC_Date ID="UC_FromDate" runat="server" />
                                        </td>
                                        <td>
                                            To Date :
                                        </td>
                                        <td style="text-align: left">
                                            <uc1:UC_Date ID="UC_ToDate" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <req>Active :</req>
                                        </td>
                                        <td style="text-align: left">
                                            <obout:OboutRadioButton ID="rbtnYes" runat="server" Text="Yes" GroupName="rbtnActive"
                                                Checked="true">
                                            </obout:OboutRadioButton>
                                            <obout:OboutRadioButton ID="rbtnNo" runat="server" Text="No" GroupName="rbtnActive">
                                            </obout:OboutRadioButton>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                </table>
                                <table class="gridFrame" width="80%">
                                    <tr>
                                        <td>
                                            <table style="width: 100%">
                                                <tr>
                                                    <td style="text-align: left;">
                                                        <a id="A1">Product List</a>
                                                    </td>
                                                    <td style="text-align: right">
                                                        <uc4:UCProductSearch ID="UCProductSearch1" runat="server" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:UpdatePanel ID="UpdatePannelDiscount" runat="server">
                                                <ContentTemplate>
                                                    <obout:Grid ID="Grid1" runat="server" CallbackMode="true" Serialize="true" AllowColumnReordering="true"
                                                        AllowColumnResizing="true" AutoGenerateColumns="false" AllowPaging="false" ShowLoadingMessage="false"
                                                        AllowSorting="false" AllowManualPaging="false" AllowRecordSelection="false" ShowFooter="false"
                                                        OnRebind="RebindGrid" AllowDataAccessOnServer="true" Width="100%" PageSize="-1">
                                                        <%-- <ScrollingSettings EnableVirtualScrolling="true" ScrollHeight="350" />--%>
                                                        <ClientSideEvents ExposeSender="true" />
                                                        <Columns>
                                                            <obout:Column Width="5%" HeaderText="Remove" HeaderAlign="Center" Align="center">
                                                                <TemplateSettings TemplateId="ItemTempRemove" />
                                                            </obout:Column>
                                                            <obout:Column DataField="Sequence" HeaderText="Sr.No." AllowEdit="false" Width="4%"
                                                                Align="center" HeaderAlign="center">
                                                            </obout:Column>
                                                            <obout:Column DataField="ProductCode" HeaderText="Product Code" AllowEdit="false"
                                                                Width="7%" HeaderAlign="left">
                                                            </obout:Column>
                                                            <obout:Column DataField="ProductName" HeaderText="Product Name" AllowEdit="false"
                                                                Width="13%" HeaderAlign="left">
                                                            </obout:Column>
                                                            <obout:Column DataField="UOM" HeaderText="UOM" AllowEdit="false" Width="3%">
                                                            </obout:Column>
                                                            <obout:Column DataField="PrincipalPrice" HeaderText="Product Price" HeaderAlign="right"
                                                                Width="8%" AllowEdit="false" Align="right">
                                                            </obout:Column>
                                                            <obout:Column DataField="DiscountRate" AllowEdit="false" Width="7%" HeaderAlign="center"
                                                                Align="left">
                                                                <TemplateSettings HeaderTemplateId="HeaderTempDiscount" TemplateId="PlainEditTemplate" />
                                                            </obout:Column>
                                                            <obout:Column DataField="IsDiscountPercent" HeaderText="Is % " AllowEdit="false"
                                                                Width="3%">
                                                                <TemplateSettings TemplateId="CheckBoxEditTemplate" />
                                                            </obout:Column>
                                                            <obout:Column DataField="AmountAfterDiscount" AllowEdit="false" Width="10%" HeaderAlign="center"
                                                                Align="right">
                                                                <TemplateSettings HeaderTemplateId="HeaderTempAmountAfterDiscount" />
                                                            </obout:Column>
                                                            <obout:Column DataField="MinOrderQuantity" AllowEdit="false" Width="5%" HeaderAlign="center"
                                                                Align="center">
                                                                <TemplateSettings TemplateId="PlainEditTemplate" HeaderTemplateId="HeaderTempMinimumOrderQty" />
                                                            </obout:Column>
                                                            <obout:Column DataField="Active" HeaderText="Active" AllowEdit="false" Width="3%"
                                                                Align="center">
                                                                <TemplateSettings TemplateId="CheckBoxActiveTemplate" />
                                                            </obout:Column>
                                                        </Columns>
                                                        <Templates>
                                                            <obout:GridTemplate ID="HeaderTempMinimumOrderQty">
                                                                <Template>
                                                                    Min.Order
                                                                    <br />
                                                                    Quantity
                                                                </Template>
                                                            </obout:GridTemplate>
                                                            <obout:GridTemplate ID="HeaderTempDiscount">
                                                                <Template>
                                                                    Discount
                                                                    <br />
                                                                    [Per Unit]
                                                                </Template>
                                                            </obout:GridTemplate>
                                                            <obout:GridTemplate ID="HeaderTempAmountAfterDiscount">
                                                                <Template>
                                                                    Amount
                                                                    <br />
                                                                    After Discount
                                                                </Template>
                                                            </obout:GridTemplate>
                                                            <obout:GridTemplate ID="ItemTempRemove">
                                                                <Template>
                                                                    <asp:ImageButton ID="imgbtnRemove" CausesValidation="false" runat="server" ImageUrl="../App_Themes/Blue/img/Delete16.png"
                                                                        Style="cursor: pointer;" ToolTip='<%# (Container.DataItem["Sequence"].ToString()) %>'
                                                                        OnClick="imgbtnRemove_Click" />
                                                                </Template>
                                                            </obout:GridTemplate>
                                                            <obout:GridTemplate runat="server" ID="PlainEditTemplate">
                                                                <Template>
                                                                    <input type="text" class="excel-textbox" value="<%# Container.Value %>" onfocus="markAsFocused(this)"
                                                                        onblur="markAsBlured(this, '<%# Grid1.Columns[Container.ColumnIndex].DataField %>', <%# Container.PageRecordIndex %>)" />
                                                                </Template>
                                                            </obout:GridTemplate>
                                                            <obout:GridTemplate runat="server" ID="CheckBoxEditTemplate">
                                                                <Template>
                                                                    <table>
                                                                        <tr>
                                                                            <td>
                                                                                <input type="checkbox" onclick="saveCheckBoxChanges(this, <%# Container.PageRecordIndex %>)"
                                                                                    <%# Container.Value.ToString().ToLower()=="true" ? "checked='checked'" : "" %> />
                                                                            </td>
                                                                            <td>
                                                                                <span>%</span>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </Template>
                                                            </obout:GridTemplate>
                                                            <obout:GridTemplate runat="server" ID="CheckBoxActiveTemplate">
                                                                <Template>
                                                                    <table>
                                                                        <tr>
                                                                            <td>
                                                                                <input type="checkbox" style="text-align: center" onclick="saveCheckBoxChangesActive(this, <%# Container.PageRecordIndex %>)"
                                                                                    <%# Container.Value.ToString()=="Y" ? "checked='checked'" : "" %> />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </Template>
                                                            </obout:GridTemplate>
                                                        </Templates>
                                                    </obout:Grid>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                </table>
                            </center>
                        </ContentTemplate>
                    </asp:TabPanel>
                </asp:TabContainer>
            </center>
        </ContentTemplate>
    </asp:UpdatePanel>
    <script type="text/javascript">
        function markAsFocused(textbox) {
            textbox.className = 'excel-textbox-focused';
        }

        function markAsBlured(textbox, dataField, rowIndex) {
            textbox.className = 'excel-textbox';

            var txtvalue = textbox.value;
            if (txtvalue == "") txtvalue = 0;
            var txtvalue = parseFloat(txtvalue);
            textbox.value = txtvalue.toFixed(2);

            if (Grid1.Rows[rowIndex].Cells[dataField].Value != textbox.value) {
                Grid1.Rows[rowIndex].Cells[dataField].Value = textbox.value;
                PageMethods.UpdateOrder(getOrderObject(rowIndex), null, null);
            }
        }

        function saveCheckBoxChanges(element, rowIndex) {
            getOrderObject(rowIndex);
            if (Grid1.Rows[rowIndex].Cells["IsDiscountPercent"].Value != element.checked.toString()) {
                if (element.checked == true) Grid1.Rows[rowIndex].Cells["IsDiscountPercent"].Value = "true";
                if (element.checked == false) Grid1.Rows[rowIndex].Cells["IsDiscountPercent"].Value = "false";
                PageMethods.UpdateOrder(getOrderObject(rowIndex), null, null);
            }
        }

        function saveCheckBoxChangesActive(element, rowIndex) {
            if (Grid1.Rows[rowIndex].Cells["Active"].Value != element.checked.toString()) {
                if (element.checked == true) Grid1.Rows[rowIndex].Cells["Active"].Value = "Y";
                if (element.checked == false) Grid1.Rows[rowIndex].Cells["Active"].Value = "N";
                PageMethods.UpdateOrder(getOrderObject(rowIndex), null, null);
            }
        }

        function getOrderObject(rowIndex) {

            var order = new Object();
            order.ID = 0;

            order.Sequence = Grid1.Rows[rowIndex].Cells['Sequence'].Value;
            order.ProductCode = Grid1.Rows[rowIndex].Cells['ProductCode'].Value;
            order.ProductName = Grid1.Rows[rowIndex].Cells['ProductName'].Value
            order.UOM = Grid1.Rows[rowIndex].Cells['UOM'].Value;
            order.PrincipalPrice = Grid1.Rows[rowIndex].Cells['PrincipalPrice'].Value;
            order.DiscountRate = Grid1.Rows[rowIndex].Cells['DiscountRate'].Value;
            order.IsDiscountPercent = Grid1.Rows[rowIndex].Cells['IsDiscountPercent'].Value;
            order.MinOrderQuantity = parseInt(Grid1.Rows[rowIndex].Cells['MinOrderQuantity'].Value);

            //New add---------------------
            var calRate = 0;
            if (order.IsDiscountPercent.toString() == "true") { calRate = parseFloat(order.PrincipalPrice) - (parseFloat(order.PrincipalPrice) * (parseFloat(order.DiscountRate) / 100.00)); }
            else { calRate = parseFloat(order.PrincipalPrice) - parseFloat(order.DiscountRate); }

            var body = Grid1.GridBodyContainer.firstChild.firstChild.childNodes[1]
            /*Amount after discount*/
            order.AmountAfterDiscount = parseFloat(calRate.toFixed(2));
            Grid1.Rows[rowIndex].Cells['AmountAfterDiscount'].Value = parseFloat(order.AmountAfterDiscount).toFixed(2);

            var cell1 = body.childNodes[rowIndex].childNodes[8];
            cell1.firstChild.lastChild.innerHTML = parseFloat(order.AmountAfterDiscount).toFixed(2);
            /*end*/

            order.Active = Grid1.Rows[rowIndex].Cells['Active'].Value;
            return order;
        }
    </script>
    <style type="text/css">
        .excel-textbox
        {
            background-color: transparent;
            border: 0px;
            margin: 0px 0px 0px -20px;
            padding: 0px;
            outline: 0;
            font: inherit;
            width: 98%;
            padding-top: 4px;
            padding-right: 2px;
            padding-bottom: 4px;
            text-align: right;
        }
        .excel-textbox-focused
        {
            background-color: #FFFFFF;
            border: 0px;
            margin: 0px 0px 0px -20px;
            padding: 0px;
            outline: 0;
            font: inherit;
            width: 95%;
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
            margin-left: -20px;
        }
    </style>
</asp:Content>

<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UC_TermsAndCondition.ascx.cs" Inherits="BrilliantWMS.Company.UC_TermsAndCondition" %>

<%@ Register Assembly="obout_Grid_NET" Namespace="Obout.Grid" TagPrefix="obout" %>
<%@ Register Assembly="obout_Flyout2_NET" Namespace="OboutInc.Flyout2" TagPrefix="obout" %>
<table class="gridFrame" width="100%">
    <tr>
        <td>
            <table style="width: 100%">
                <tr>
                    <td style="text-align: left;">
                        <a id="headerText">Terms and Conditions</a>
                    </td>
                    <td style="text-align: right;">
                        <input type="button" value="Terms and Conditions" runat="server" style="width: 150px;"
                            id="btnAddTermsConditiont" />
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td>
            <asp:UpdateProgress ID="UpdateProgress_UCTnC" runat="server" AssociatedUpdatePanelID="updPnl_TermDetails">
                <ProgressTemplate>
                    <center>
                        <div class="modal">
                            <img src="../App_Themes/Blue/img/ajax-loader.gif" style="top: 50%;" />
                        </div>
                    </center>
                </ProgressTemplate>
            </asp:UpdateProgress>
            <asp:UpdatePanel ID="updPnl_TermDetails" runat="server">
                <ContentTemplate>
                    <obout:Grid ID="gvTermConditionDetail" runat="server" Serialize="true" AllowColumnReordering="true"
                        AllowColumnResizing="true" AutoGenerateColumns="false" ShowLoadingMessage="true"
                        AllowSorting="false" AllowRecordSelection="false" OnRebind="RebindGridTCD" AllowDataAccessOnServer="true"
                        Width="100%" CallbackMode="true" AllowPaging="true" AllowFiltering="true" AllowGrouping="true"
                        AllowPageSizeSelection="true" PageSize="-1">
                        <ClientSideEvents ExposeSender="true" />
                        <Columns>
                            <obout:Column Width="1%" Align="center" HeaderText="Delete" HeaderAlign="Center">
                                <TemplateSettings TemplateId="ItemTempRemoveTerm" />
                            </obout:Column>
                            <obout:Column DataField="ID" HeaderText="ID" Visible="false">
                            </obout:Column>
                            <obout:Column DataField="Sequence" HeaderText="Sr.No." AllowEdit="false" Width="1%"
                                Align="center" HeaderAlign="center">
                            </obout:Column>
                            <obout:Column DataField="Term" HeaderText="Term Name" Width="5%">
                                <TemplateSettings TemplateId="PlainEditTemplateTerm" />
                            </obout:Column>
                            <obout:Column DataField="Condition" HeaderText="Condition" Width="10%">
                                <TemplateSettings TemplateId="PlainEditTemplateConditionTerm" />
                            </obout:Column>
                            <obout:Column DataField="Active" HeaderText="Active" AllowEdit="false" Width="1%"
                                Visible="false">
                                <TemplateSettings TemplateId="CheckBoxActiveTemplateTerm" />
                            </obout:Column>
                        </Columns>
                        <Templates>
                            <obout:GridTemplate ID="ItemTempRemoveTerm">
                                <Template>
                                    <asp:ImageButton ID="imgbtnRemove" CausesValidation="false" runat="server" ImageUrl="../App_Themes/Blue/img/Delete16.png"
                                        Style="cursor: pointer;" ToolTip='<%# (Container.DataItem["Sequence"].ToString()) %>'
                                        OnClick="imgbtnRemove_Click" />
                                </Template>
                            </obout:GridTemplate>
                            <obout:GridTemplate runat="server" ID="PlainEditTemplateTerm">
                                <Template>
                                    <input type="text" class="excel-textbox" style="text-align: left" value="<%# Container.Value %>"
                                        onkeyup="TextBox_KeyUp(this,'','50');" onfocus="markAsFocusedTerm(this)" onblur="markAsBluredTerm(this, '<%# gvTermConditionDetail.Columns[Container.ColumnIndex].DataField %>', <%# Container.PageRecordIndex %>)" />
                                </Template>
                            </obout:GridTemplate>
                            <obout:GridTemplate runat="server" ID="PlainEditTemplateConditionTerm">
                                <Template>
                                    <input type="text" class="excel-textbox" style="text-align: left" value="<%# Container.Value %>"
                                        onkeyup="TextBox_KeyUp(this,'','2000');" onfocus="markAsFocusedTerm(this)" onblur="markAsBluredTerm(this, '<%# gvTermConditionDetail.Columns[Container.ColumnIndex].DataField %>', <%# Container.PageRecordIndex %>)" />
                                </Template>
                            </obout:GridTemplate>
                            <obout:GridTemplate runat="server" ID="CheckBoxActiveTemplateTerm">
                                <Template>
                                    <table>
                                        <tr>
                                            <td>
                                                <input type="checkbox" onclick="saveCheckBoxChangesActiveTerm(this, <%# Container.PageRecordIndex %>)"
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
<asp:HiddenField ID="hdnSequenceTC" runat="server" />
<asp:HiddenField ID="hdnUserIDTC" runat="server" />
<asp:HiddenField ID="hdnObjectName_OldTC" runat="server" />
<asp:HiddenField ID="hdnObjectName_NewTC" runat="server" />
<asp:HiddenField ID="hdnReferenceIDTC" runat="server" />
<asp:HiddenField ID="hdnSessionIDTC" runat="server" />
<asp:HiddenField ID="hdnTermsCondTC" runat="server" />
<asp:HiddenField ID="hdnSelectedRecTC" runat="server" ViewStateMode="Enabled" ClientIDMode="Static" />
<script type="text/javascript">
    function markAsFocusedTerm(textbox) {
        textbox.className = 'excel-textbox-focused';
    }

    function markAsBluredTerm(textbox, dataField, rowIndex) {
        textbox.className = 'excel-textbox';

        var txtvalue = textbox.value;

        if (txtvalue == "") txtvalue = 0;

        textbox.value = txtvalue;

        if (gvTermConditionDetail.Rows[rowIndex].Cells[dataField].Value != textbox.value) {

            gvTermConditionDetail.Rows[rowIndex].Cells[dataField].Value = textbox.value;

            PageMethods.UpdateUcTermC(getOrderObjectTerm(rowIndex), null, null);

        }

    }

    function saveCheckBoxChangesActiveTerm(element, rowIndex) {

        if (gvTermConditionDetail.Rows[rowIndex].Cells["Active"].Value != element.checked.toString()) {

            if (element.checked == true) gvTermConditionDetail.Rows[rowIndex].Cells["Active"].Value = "Y";

            if (element.checked == false) gvTermConditionDetail.Rows[rowIndex].Cells["Active"].Value = "N";

            PageMethods.UpdateUcTermC(getOrderObjectTerm(rowIndex), null, null);

        }
    }

    function getOrderObjectTerm(rowIndex) {

        var order = new Object();
        order.ID = 0;

        order.Sequence = gvTermConditionDetail.Rows[rowIndex].Cells['Sequence'].Value;
        order.Term = gvTermConditionDetail.Rows[rowIndex].Cells['Term'].Value;
        order.Condition = gvTermConditionDetail.Rows[rowIndex].Cells['Condition'].Value;
        order.Active = gvTermConditionDetail.Rows[rowIndex].Cells['Active'].Value;

        return order;
    }

    function openTermConnditionSearchWindow(groupName) {
        window.open('../Company/TermsConditionSearch.aspx?object=' + groupName + '', null, 'height=500px, width=770px,status= 0, resizable= 0, scrollbars=1, toolbar=0,location=0,menubar=0,');
    }

    function TCSubmit() {
        gvTermConditionDetail.refresh();
    }
</script>
<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/CRM.Master" AutoEventWireup="true"
    CodeBehind="ToolTransfer.aspx.cs" Inherits="BrilliantWMS.POR.ToolTransfer"
    Theme="blue" EnableEventValidation="false" %>

<%@ Register Assembly="obout_Flyout2_NET" Namespace="OboutInc.Flyout2" TagPrefix="obout" %>
<%@ Register Assembly="Obout.Ajax.UI" Namespace="Obout.Ajax.UI.FileUpload" TagPrefix="obout" %>
<%@ Register Assembly="obout_Interface" Namespace="Obout.Interface" TagPrefix="obout" %>
<%@ Register Assembly="obout_Grid_NET" Namespace="Obout.Grid" TagPrefix="obout" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../CommonControls/UCFormHeader.ascx" TagName="UCFormHeader" TagPrefix="uc1" %>
<%@ Register Src="../CommonControls/UCToolbar.ascx" TagName="UCToolbar" TagPrefix="uc2" %>
<%@ Register Src="../Document/UC_AttachDocument.ascx" TagName="UC_AttachDocument"
    TagPrefix="uc3" %>
<%@ Register Src="../CommonControls/UC_Date.ascx" TagName="UC_Date" TagPrefix="uc4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHolder_FormHeader" runat="server">
    <uc2:UCToolbar ID="UCToolbar1" runat="server" />
    <uc1:UCFormHeader ID="UCFormHeader1" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPHolder_Form" runat="server">
    <asp:UpdatePanel ID="UpdPnlAssetTransferList" runat="server">
        <ContentTemplate>
            <asp:HiddenField ID="hdnprodID" runat="server" />
            <asp:TabContainer ID="TabConAssetTransfer" runat="server" ActiveTabIndex="2">
                <asp:TabPanel ID="TabPnlAssetTransferLst" runat="server" HeaderText="Asset Transfer List">
                    <ContentTemplate>
                        <asp:UpdateProgress ID="UpdateGirdAssetTransferLst" runat="server" AssociatedUpdatePanelID="Up_PnlGirdTransfer">
                            <ProgressTemplate>
                                <center>
                                    <div class="modal">
                                        <img src="../App_Themes/Blue/img/ajax-loader.gif" style="top: 50%;" />
                                    </div>
                                </center>
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                        <asp:UpdatePanel ID="Up_PnlGirdTransfer" runat="server">
                            <ContentTemplate>
                                <center>
                                    <table class="gridFrame" width="80%">
                                        <tr>
                                            <td>
                                                <table style="width: 100%">
                                                    <tr>
                                                        <td>
                                                            <a style="color: white; font-size: 15px; font-weight: bold;">Asset Transfer List</a>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <obout:Grid ID="GvAssetTransfer" runat="server" AutoGenerateColumns="False" AllowAddingRecords="False"
                                                    AllowFiltering="True" AllowGrouping="True" AllowMultiRecordSelection="False"
                                                    AllowRecordSelection="true" Width="100%" AllowColumnResizing="true" AllowColumnReordering="true">
                                                    <Columns>
                                                        <%--<obout:Column HeaderText="Edit" DataField="ID" Width="10%" Align="center" HeaderAlign="center">
                                                            <TemplateSettings TemplateId="TemplateEdit" />
                                                        </obout:Column>--%>
                                                        <obout:Column HeaderText="Transfer Date" DataField="TransferDate" Width="30%" DataFormatString="{0:dd-MMM-yyyy}" Align="center" HeaderAlign="center" />
                                                        <obout:Column HeaderText="Transfered By" DataField="TransferedByName" Width="25%" />
                                                        <obout:Column HeaderText="Transfer From Site" DataField="FromSite" Width="35%" />
                                                        <obout:Column HeaderText="Transfer To Site" DataField="ToSite" Width="35%" />
                                                    </Columns>
                                                    <%--<Templates>
                                                        <obout:GridTemplate runat="server" ID="TemplateEdit">
                                                            <Template>
                                                                <asp:ImageButton ID="imgBtnEdit1" runat="server" ImageUrl="../App_Themes/Blue/img/Edit16.png"
                                                                    ToolTip='<%# (Container.Value) %>' CausesValidation="false" OnClick="imgBtnEdit_OnClick"/>                                                                
                                                            </Template>
                                                        </obout:GridTemplate>
                                                    </Templates>--%>
                                                </obout:Grid>
                                            </td>
                                        </tr>
                                    </table>
                                </center>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </ContentTemplate>
                </asp:TabPanel>
                <asp:TabPanel ID="tabTransferDetails" runat="server" HeaderText="Asset Transfer Details">
                    <ContentTemplate>
                        <asp:UpdateProgress ID="UpdateProgress_AssetTransferDetails" runat="server" AssociatedUpdatePanelID="Uppnl_TransferDetails">
                            <ProgressTemplate>
                                <center>
                                    <div class="modal">
                                        <img src="../App_Themes/Blue/img/ajax-loader.gif" style="top: 50%;" />
                                    </div>
                                </center>
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                        <asp:UpdatePanel ID="Uppnl_TransferDetails" runat="server">
                            <ContentTemplate>
                                <center>
                                    <table class="tableForm">
                                        <tr>
                                            <td>
                                                Transfer Date* :
                                            </td>
                                            <td style="text-align: left;">
                                                <uc4:UC_Date ID="UCTransferDate" runat="server" />
                                            </td>
                                            <td>
                                                Transfered By* :
                                            </td>
                                            <td style="text-align: left;">
                                                <asp:DropDownList runat="server" ID="ddlTransferedBy" Width="182px" AccessKey="1" DataTextField="userName"
                                                    DataValueField="userID">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                Status* :
                                            </td>
                                            <td style="text-align: left;" colspan="4">
                                                <asp:DropDownList runat="server" ID="ddlStatus" Width="182px">
                                                    <asp:ListItem Value="0" Text="-Select-" Selected="True"></asp:ListItem>
                                                    <asp:ListItem Value="1" Text="Transfered"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Transfer From Site* :
                                            </td>
                                            <td style="text-align: left;">
                                                <asp:DropDownList runat="server" ID="ddlFrmSite" Width="182px" DataTextField="Territory"
                                                    DataValueField="ID" onchange="divTrAnsfer(this);">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                Transfer To Site* :
                                            </td>
                                            <td style="text-align: left;" colspan="2">
                                                <asp:DropDownList runat="server" ID="ddlToSite" Width="182px" ClientIDMode="Static" onchange="divTo(this);">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="6" style="font-style: italic; text-align: left; font-weight: bold;">
                                                <hr style="width: 87%; margin-top: 8px; float: right;" />
                                                <span>Transport Detail</span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Airway Bill :
                                            </td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtAirwayBill" MaxLength="50" Width="180px"></asp:TextBox>
                                            </td>
                                            <td>
                                                Shipping Type :
                                            </td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtShippingType" MaxLength="50" Width="180px"></asp:TextBox>
                                            </td>
                                            <td>
                                                Shipping Date :
                                            </td>
                                            <td style="text-align: left;">
                                                <uc4:UC_Date ID="UC_ShippingDate" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Exp. Delivery Date :
                                            </td>
                                            <td style="text-align: left;">
                                                <uc4:UC_Date ID="UC_ExpDeliveryDate" runat="server" />
                                            </td>
                                            <td>
                                                Transporter Name :
                                            </td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtTransporterName" MaxLength="50" Width="180px"></asp:TextBox>
                                            </td>
                                            <td>
                                                Remark :
                                            </td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtRemark" MaxLength="100" Width="180px"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                    <table class="gridFrame" width="60%">
                                        <tr>
                                            <td>
                                                <a class="headerText">Equipment / Tool List</a>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <obout:Grid ID="GVAssetList" runat="server" AutoGenerateColumns="false" AllowAddingRecords="false"
                                                    Width="100%" OnRebind="GVAssetList_OnRebind">
                                                    <ClientSideEvents ExposeSender="true" />
                                                    <Columns>
                                                        <%--<obout:Column DataField="ID" Visible="false" Width="0%">
                                                        </obout:Column>--%>
                                                        <obout:Column DataField="Sequence" HeaderText="Sr.No." AllowEdit="false" Width="7%"
                                                            Align="left" HeaderAlign="center">
                                                            <TemplateSettings TemplateId="ItemTempRemove" />
                                                        </obout:Column>
                                                        <obout:Column DataField="ProductCode" HeaderText="Code" Width="10%" Align="center"
                                                            HeaderAlign="center">
                                                        </obout:Column>
                                                        <obout:Column DataField="Name" HeaderText="Name" Width="10%" Align="center" HeaderAlign="center">
                                                        </obout:Column>
                                                    </Columns>
                                                    <Templates>
                                                       <obout:GridTemplate ID="ItemTempRemove">
                                                          <Template>
                                                             <table>
                                                                <tr>
                                                                   <td style="width:20px; text-align:center;">
                                                                       <img id="imgbtnRemove" src="../App_Themes/Grid/img/Remove16x16.png" title="Remove"
                                                                       onclick="removePartFromList('<%# (Container.DataItem["Sequence"].ToString()) %>');"
                                                                       style="cursor:pointer;" />
                                                                   </td>
                                                                   <td style="width: 35px; text-align: right;">
                                                                     <%# Convert.ToInt32(Container.PageRecordIndex) + 1 %>
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
    <asp:HiddenField ID="hdnSelectedFromSite" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnSelectedToSite" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnRemovePrd" runat="server" ClientIDMode="Static" Value="0" />
    <script type="text/javascript">
        function divTrAnsfer() {
            var ddlFrmSite = document.getElementById("<%=ddlFrmSite.ClientID %>");

            var hdnSelectedFromSite = document.getElementById('hdnSelectedFromSite');
            hdnSelectedFromSite.value = ddlFrmSite.value;

            var frmSite = ddlFrmSite.value;
            PageMethods.WMGetFromSite(frmSite, OnSuccessFromSite, null);
        }
        function OnSuccessFromSite(result) {
            ddlToSite = document.getElementById("<%=ddlToSite.ClientID %>");
            ddlToSite.options.length = 0;
            var option0 = document.createElement("option");

            if (result.length > 0) {
                option0.text = '--Select--';
                option0.value = '0';
            }
            else {
                option0.text = 'N/A';
                option0.value = '0';
            }

            try {
                ddlToSite.add(option0, null);
            }
            catch (Error) {
                ddlToSite.add(option0);
            }

            for (var i = 0; i < result.length; i++) {
                var option1 = document.createElement("option");
                option1.text = result[i].Territory;
                option1.value = result[i].ID;
                try {
                    ddlToSite.add(option1, null);
                }
                catch (Error) {
                    ddlToSite.add(option1);
                }
            }
            GVAssetList.refresh();
        }

        function divTo() {
            var ddlToSite = document.getElementById("<%=ddlToSite.ClientID %>");
            var hdnSelectedToSite = document.getElementById('hdnSelectedToSite');
            hdnSelectedToSite.value = ddlToSite.value;            
        }

        function removePartFromList(sequence) {
//            var hdnProductSearchSelectedRec = document.getElementById('hdnProductSearchSelectedRec');
//            hdnProductSearchSelectedRec.value = "";
            PageMethods.WMRemovePartFromList(sequence, removePartFrmListOnSuccess, null);
        }

        function removePartFrmListOnSuccess() {
            var hdnRemovePrd = document.getElementById('hdnRemovePrd');
            hdnRemovePrd.value = 1;
            GVAssetList.refresh();            
         }
    </script>
</asp:Content>

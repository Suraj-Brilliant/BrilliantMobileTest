<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/CRM.Master" AutoEventWireup="true" CodeBehind="Transfer.aspx.cs"
    Inherits="BrilliantWMS.WMS.Transfer" EnableEventValidation="false" Theme="Blue" %>

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
<%@ Register Src="../Product/UCProductSearch.ascx" TagName="UCProductSearch" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHolder_FormHeader" runat="server">
    <uc2:Toolbar ID="Toolbar1" runat="server" />
    <uc1:UCFormHeader ID="UCFormHeader1" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPHolder_Form" runat="server">
    <asp:UpdatePanel ID="UPTBTransferList" runat="server">
        <ContentTemplate>
            <asp:TabContainer ID="TCTransfer" runat="server">
                <asp:TabPanel ID="TabTransferList" runat="server" HeaderText="Transfer List">
              <ContentTemplate>
            <asp:UpdateProgress ID="UPTransferGridProgress" runat="server" AssociatedUpdatePanelID="UpPnlGridTransfer">
                <ProgressTemplate>
                    <center>
                        <div class="modal">
                            <img src="../App_Themes/Blue/img/ajax-loader.gif" style="top: 50%;" />
                        </div>
                    </center>
                </ProgressTemplate>
            </asp:UpdateProgress>
            <asp:UpdatePanel ID="UpPnlGridTransfer" runat="server">
                <ContentTemplate>
                    <%--<div class="divHead">--%>
                    <h4 id="h4DivHead" runat="server"></h4>
                    <table style="float: right; font-size: 15px; font-weight: bold; color: Black;">
                        <%--margin-top: -25px;--%>
                        <tr>
                            <td style="padding-right: 10px; padding-bottom: 0px; padding-top: 0px;">
                                <%--<input type="button" id="btnGenerateGroupTask" title="Generate Group Task" value="Generate Group Task" runat="server" onclick="GenerateGroupTask()" />
                                <input type="button" id="btnCreateTask" title="Create Task" value="Create Task" runat="server" onclick="CreateTask()" />--%>
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
                    <%-- </div>--%>
                    <div id="divlinkReturnList">
                        <%--class="divDetailExpand"--%>
                        <center>
                            <table class="gridFrame" width="100%">
                                <tr>
                                    <td>
                                        <table style="width: 100%">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblTransferlist" Style="color: white; font-size: 15px; font-weight: bold;" runat="server" Text="Transfer List"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <obout:Grid ID="grdTransfer" runat="server" AutoGenerateColumns="false" AllowAddingRecords="false" AllowFiltering="true" AllowGrouping="true" AllowMultiRecordSelection="false"
                                            AllowRecordSelection="true" Width="100%" AllowColumnResizing="true" AllowColumnReordering="true">
                                            <Columns>
                                                <obout:Column DataField="ID" HeaderText="Transfer No" HeaderAlign="center" Align="center" Width="5%" Wrap="true" ></obout:Column>
                                               <%-- <obout:Column DataField="SONo" HeaderText="SO No" HeaderAlign="center" Align="center" Width="7%"></obout:Column>--%>
                                               <%-- <obout:Column DataField="GroupTaskName" HeaderText="Group Task Name" HeaderAlign="left" Align="left" Width="10%" Wrap="true"></obout:Column>--%>
                                                <obout:Column DataField="TransferDate" HeaderText="Transfer Date" HeaderAlign="left" Align="left" DataFormatString="{0:dd-MM-yyyy}" Width="10%"></obout:Column>
                                                <obout:Column DataField="FromWarehouse" HeaderText="From Warehouse" HeaderAlign="left" Align="left" Width="10%" Wrap="true"></obout:Column>
                                                <obout:Column DataField="ToWarehouse" HeaderText="To Warehouse" HeaderAlign="left" Align="left" Width="10%" Wrap="true"></obout:Column>
                                                <obout:Column DataField="TransferByUser" HeaderText="Transfer By" HeaderAlign="left" Align="left" Width="10%"></obout:Column>
                                                <obout:Column DataField="StatusName" HeaderText="Status" HeaderAlign="left" Align="left" Width="8%" Wrap="true"></obout:Column>
                                                <obout:Column DataField="ImgTransfer" HeaderText="Transfer" HeaderAlign="center" Align="center" Width="7%">
                                                    <TemplateSettings TemplateId="TRStatusTR" />
                                                </obout:Column>
                                                <obout:Column DataField="ImgPickUP" HeaderText="Pick Up" HeaderAlign="center" Align="center" Width="7%">
                                                    <TemplateSettings TemplateId="TRStatusPK" />
                                                </obout:Column>
                                                <obout:Column DataField="ImgQCOut" HeaderText="QC Out" HeaderAlign="center" Align="center" Width="7%">
                                                    <TemplateSettings TemplateId="TRStatusQCOut" />
                                                </obout:Column>
                                                <obout:Column DataField="ImgDispatch" HeaderText="Dispatch" HeaderAlign="center" Align="center" Width="7%">
                                                    <TemplateSettings TemplateId="TRStatusD" />
                                                </obout:Column>
                                                <obout:Column DataField="ImgGRN" HeaderText="GRN" HeaderAlign="center" Align="center" Width="7%">
                                                    <TemplateSettings TemplateId="TRStatusGRN" />
                                                </obout:Column>
                                                <obout:Column DataField="ImgQCIn" HeaderText="QC In" HeaderAlign="center" Align="center" Width="7%">
                                                    <TemplateSettings TemplateId="TRStatusQCIn" />
                                                </obout:Column>
                                                <obout:Column DataField="ImgPutIn" HeaderText="Put In" HeaderAlign="center" Align="center" Width="7%">
                                                    <TemplateSettings TemplateId="TRStatusPT" />
                                                </obout:Column>
                                            </Columns>
                                            <Templates>
                                                <obout:GridTemplate ID="TRStatusTR" runat="server">
                                                    <Template>
                                                        <center>
                                                            <div class='<%# ("POR" + Container.Value) %>' onclick="RequestOpenEntryForm('Transfer','<%# Container.Value %>', '<%# Container.DataItem["ID"] %>')"></div>
                                                        </center>
                                                    </Template>
                                                </obout:GridTemplate>
                                                <obout:GridTemplate ID="TRStatusPK" runat="server">
                                                    <Template>
                                                        <center>
                                                            <div class='<%# ("POR" + Container.Value) %>' onclick="RequestOpenEntryForm('PickUp','<%# Container.Value %>', '<%# Container.DataItem["ID"] %>')"></div>
                                                        </center>
                                                    </Template>
                                                </obout:GridTemplate>
                                                <obout:GridTemplate ID="TRStatusQCOut" runat="server">
                                                    <Template>
                                                        <center>
                                                            <div class='<%# ("POR" + Container.Value) %>' onclick="RequestOpenEntryForm('QCOut','<%# Container.Value %>', '<%# Container.DataItem["ID"] %>')"></div>
                                                        </center>
                                                    </Template>
                                                </obout:GridTemplate>
                                                <obout:GridTemplate ID="TRStatusD" runat="server">
                                                    <Template>
                                                        <center>
                                                            <div class='<%# ("POR" + Container.Value) %>' onclick="RequestOpenEntryForm('Dispatch','<%# Container.Value %>', '<%# Container.DataItem["ID"] %>')"></div>
                                                        </center>
                                                    </Template>
                                                </obout:GridTemplate>
                                                <obout:GridTemplate ID="TRStatusGRN" runat="server">
                                                    <Template>
                                                        <center>
                                                            <div class='<%# ("POR" + Container.Value) %>' onclick="RequestOpenEntryForm('GRN','<%# Container.Value %>', '<%# Container.DataItem["ID"] %>')"></div>
                                                        </center>
                                                    </Template>
                                                </obout:GridTemplate>
                                                <obout:GridTemplate ID="TRStatusQCIn" runat="server">
                                                    <Template>
                                                        <center>
                                                            <div class='<%# ("POR" + Container.Value) %>' onclick="RequestOpenEntryForm('QCIn','<%# Container.Value %>', '<%# Container.DataItem["ID"] %>')"></div>
                                                        </center>
                                                    </Template>
                                                </obout:GridTemplate>
                                                 <obout:GridTemplate ID="TRStatusPT" runat="server">
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
                <asp:TabPanel ID="TPInternalTransfer" runat="server" HeaderText="Internal Transfer" Visible="false">
                    <ContentTemplate>
                        <asp:UpdateProgress ID="UpdProgressInternalTransfer" runat="server" AssociatedUpdatePanelID="UPInternalTransfer">
                            <ProgressTemplate>
                                <center>
                                    <div class="modal">
                                        <img src="../App_Themes/Blue/img/ajax-loader.gif" style="top: 50%;" />
                                    </div>
                                </center>
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                        <asp:UpdatePanel ID="UPInternalTransfer" runat="server">
                            <ContentTemplate>
                                <div class="divHead" id="divHeadInternalTransferHead">
                                    <h4>
                                        <asp:Label ID="lblInternalTransfer" runat="server" Text="Internal Transfer"></asp:Label>
                                    </h4>
                                    <a onclick="javascript:divcollapsOpen('divInternalTransferDetail',this)" id="A1" runat="server">Collapse</a>
                                </div>
                                <div class="divDetailExpand" id="divInternalTransferDetail">
                                    <table class="tableForm" width="100%">
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblInternalTransferNumber" runat="server" Text="Internal Transfer Number "></asp:Label>:
                                            </td>
                                            <td style="text-align: left; font-weight: bold;">
                                                <asp:Label ID="lblInternalTransferNo" runat="server"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblInternalTransferDate" runat="server" Text="Date "></asp:Label>:
                                            </td>
                                            <td style="text-align: left;">
                                                <uc4:UC_Date ID="UCInternalTransferDate" runat="server" />
                                            </td>
                                            <td>
                                                <asp:Label ID="lblInternalTransferBy" runat="server" Text="Transfer By "></asp:Label>:
                                            </td>
                                            <td style="text-align: left; font-weight: bold;">
                                                <asp:DropDownList ID="ddlInternalTransferBy" runat="server" Width="182px" DataTextField="" DataValueField="" AccessKey="1"></asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblStatus" runat="server" Text="Status "></asp:Label>:
                                            </td>
                                            <td style="text-align: left; font-weight: bold;">
                                                <asp:DropDownList ID="ddlStatus" runat="server" Width="182px" DataTextField="" DataValueField="" AccessKey="1"></asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblBatchNo" runat="server" Text="Batch No. "></asp:Label>:
                                            </td>
                                            <td style="text-align: left; font-weight: bold;">
                                                <asp:TextBox ID="txtBatchNo" runat="server" AccessKey="1"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblShippingNo" runat="server" Text="Shipping No. "></asp:Label>:
                                            </td>
                                            <td style="text-align: left; font-weight: bold;">
                                                <asp:TextBox ID="txtShippingNo" runat="server" AccessKey="1"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblRemark" runat="server" Text="Remark "></asp:Label>:
                                            </td>
                                            <td colspan="3" style="text-align: left; font-weight: bold;">
                                                <asp:TextBox ID="txtRemark" runat="server" Width="99%"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>                                   
                                    <table class="gridFrame" width="100%" id="tblCart">
                                        <tr>
                                            <td>
                                                <table style="width: 100%">
                                                    <tr>
                                                        <td style="text-align: left;">
                                                            <asp:Label ID="lblInternalTransferlist" CssClass="headerText" runat="server" Text="Internal Transfer Part List"></asp:Label>
                                                        </td>
                                                        <td style="text-align: right;">
                                                                <uc1:UCProductSearch ID="UCProductSearchIT" runat="server" />
                                                            </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <obout:Grid ID="Grid1" runat="server" CallbackMode="true" Serialize="true" AllowColumnReordering="true"
                                                    AllowColumnResizing="true" AutoGenerateColumns="false" AllowPaging="false" ShowLoadingMessage="true"
                                                    AllowSorting="false" AllowManualPaging="false" AllowRecordSelection="false" ShowFooter="false"
                                                    Width="100%" PageSize="-1">
                                                    <ClientSideEvents ExposeSender="true" />
                                                    <Columns>
                                                        <obout:Column DataField="ID" Visible="false"></obout:Column>
                                                        <obout:Column DataField="Sequence" HeaderText="Sr.No." AllowEdit="false" Width="7%"
                                                            Align="center" HeaderAlign="center">
                                                            <TemplateSettings TemplateId="ItemTempRemove" />
                                                        </obout:Column>
                                                        <obout:Column DataField="Prod_Code" HeaderText="SKU Code" AllowEdit="false" Width="15%"
                                                            HeaderAlign="left">
                                                        </obout:Column>
                                                        <obout:Column DataField="Prod_Name" HeaderText="Product Name" AllowEdit="false" Width="15%"
                                                            HeaderAlign="left">
                                                        </obout:Column>
                                                        <obout:Column DataField="Prod_Description" HeaderText="Description" AllowEdit="false"
                                                            Width="15%" HeaderAlign="left">
                                                        </obout:Column>
                                                        <obout:Column DataField="UOM" HeaderText="UOM" AllowEdit="false" Width="14%" HeaderAlign="center"
                                                            Align="center">
                                                        </obout:Column>
                                                        <obout:Column DataField="Qty" HeaderText="Qty" AllowEdit="false"
                                                            Width="13%" HeaderAlign="center" Align="right">
                                                        </obout:Column>
                                                        <obout:Column DataField="FrmLocation" HeaderText="From Location" Width="10%" HeaderAlign="center" Align="center"></obout:Column>                                                      
                                                        <obout:Column DataField="ToLocation" HeaderText="To Location" Width="10%" HeaderAlign="center" Align="center"></obout:Column>                                                      
                                                    </Columns>
                                                    <Templates>
                                                        <obout:GridTemplate ID="ItemTempRemove">
                                                            <Template>
                                                                <table>
                                                                    <tr>
                                                                        <td style="width: 20px; text-align: center;">
                                                                            <img id="imgbtnRemove" src="../App_Themes/Grid/img/Remove16x16.png" title="Remove"
                                                                                onclick="removePartFromList('<%# (Container.DataItem["Sequence"].ToString()) %>');"
                                                                                style="cursor: pointer;" />
                                                                        </td>
                                                                        <td style="width: 35px; text-align: left;">
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
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </ContentTemplate>
                </asp:TabPanel>
                <asp:TabPanel ID="TPWTWTransfer" runat="server" HeaderText="Warehouse To Warehouse Transfer" Visible="false">
                    <ContentTemplate>
                        <asp:UpdateProgress ID="UpdProgressWtWTransfer" runat="server" AssociatedUpdatePanelID="UPWTWTransfer">
                            <ProgressTemplate>
                                <center>
                                    <div class="modal">
                                        <img src="../App_Themes/Blue/img/ajax-loader.gif" style="top: 50%;" />
                                    </div>
                                </center>
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                        <asp:UpdatePanel ID="UPWTWTransfer" runat="server" Visible="false">
                            <ContentTemplate>
                                <div class="divHead" id="divHeadWTWTransferHead">
                                    <h4>
                                        <asp:Label ID="lblWTWTransfer" runat="server" Text="Warehouse To Warehouse Transfer"></asp:Label>
                                    </h4>
                                    <a onclick="javascript:divcollapsOpen('divWTWTransferDetail',this)" id="A2" runat="server">Collapse</a>
                                </div>
                                 <div class="divDetailExpand" id="divWTWTransferDetail">
                                        <table class="tableForm" width="100%">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblWTWTransferDetailNumber" runat="server" Text="Transfer Number "></asp:Label>:
                                                </td>
                                                <td style="text-align: left; font-weight: bold;">
                                                    <asp:Label ID="lblWTWTransferNo" runat="server"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblWTWTransferDate" runat="server" Text="Date "></asp:Label>:
                                                </td>
                                                <td style="text-align: left;">
                                                    <uc4:UC_Date ID="UCWTWTransferDate" runat="server" />
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblWTWTransferBy" runat="server" Text="Transfer By "></asp:Label>:
                                                </td>
                                                <td style="text-align: left; font-weight: bold;">
                                                    <asp:DropDownList ID="ddlWTWTransferBy" runat="server" Width="182px" DataTextField="" DataValueField="" AccessKey="1"></asp:DropDownList>
                                                </td>
                                                <td>
                                                    <asp:Label ID="WTWTransferStatus" runat="server" Text="Status "></asp:Label>:
                                                </td>
                                                <td style="text-align: left; font-weight: bold;">
                                                    <asp:DropDownList ID="ddlWTWStatus" runat="server" Width="182px" DataTextField="" DataValueField="" AccessKey="1"></asp:DropDownList>
                                                </td>
                                            </tr>           
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblFromW" runat="server" Text="From Warehouse"></asp:Label>:
                                                </td>
                                                <td style="text-align: left; font-weight: bold;">
                                                    <asp:DropDownList ID="ddlFWarehouse" runat="server" Width="182px" DataTextField="" DataValueField="" AccessKey="1"></asp:DropDownList>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblToW" runat="server" Text="To Warehouse"></asp:Label>:
                                                </td>
                                                <td style="text-align: left; font-weight: bold;">
                                                    <asp:DropDownList ID="ddlTWarehouse" runat="server" Width="182px" DataTextField="" DataValueField="" AccessKey="1"></asp:DropDownList>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblWTWRemark" runat="server" Text="Remark"></asp:Label>:
                                                </td>
                                                <td style="text-align: left; font-weight: bold;" colspan="3">
                                                    <asp:TextBox ID="WTWTRemark" runat="server" Width="90%"></asp:TextBox>
                                                </td>
                                            </tr>                                
                                        </table>
                                        <table class="tableForm" width="100%">
                                            <tr>
                                                <td colspan="6" style="font-style: italic; text-align: left; font-weight: bold;">
                                                    <hr style="width: 87%; margin-top: 8px; float: right;" />
                                                    <span>Transport Detail</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Airway Bill :
                                                </td>
                                                <td style="text-align: left; font-weight: bold;">
                                                    <asp:TextBox runat="server" ID="txtAirwayBill" MaxLength="50" Width="180px"></asp:TextBox>
                                                </td>
                                                <td>Shipping Type :
                                                </td>
                                                <td style="text-align: left; font-weight: bold;">
                                                    <asp:TextBox runat="server" ID="txtShippingType" MaxLength="50" Width="180px"></asp:TextBox>
                                                </td>
                                                <td>Shipping Date :
                                                </td>
                                                <td style="text-align: left;">
                                                    <uc4:UC_Date ID="UC_ShippingDate" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Exp. Delivery Date :
                                                </td>
                                                <td style="text-align: left; font-weight: bold;">
                                                    <uc4:UC_Date ID="UCExpDeliveryDate" runat="server" />
                                                </td>
                                                <td>Transporter Name :
                                                </td>
                                                <td style="text-align: left; font-weight: bold;">
                                                    <asp:TextBox runat="server" ID="txtTransporterName" MaxLength="50" Width="180px"></asp:TextBox>
                                                </td>
                                                <td>
                                                </td>
                                                <td style="text-align: left; font-weight: bold;">
                                                    
                                                </td>
                                            </tr>
                                        </table>
                                        <table class="gridFrame" width="100%" id="Table1">
                                            <tr>
                                                <td>
                                                    <table style="width: 100%">
                                                        <tr>
                                                            <td style="text-align: left;">
                                                                <asp:Label ID="lblWTWTransferpartlist" CssClass="headerText" runat="server" Text="Warehouse To Warehouse Transfer Part List"></asp:Label>
                                                            </td>
                                                            <td style="text-align: right;">
                                                                <uc1:UCProductSearch ID="UCProductSearchWTW" runat="server" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <obout:Grid ID="Grid2" runat="server" CallbackMode="true" Serialize="true" AllowColumnReordering="true"
                                                        AllowColumnResizing="true" AutoGenerateColumns="false" AllowPaging="false" ShowLoadingMessage="true"
                                                        AllowSorting="false" AllowManualPaging="false" AllowRecordSelection="false" ShowFooter="false"
                                                        Width="100%" PageSize="-1">
                                                        <ClientSideEvents ExposeSender="true" />
                                                        <Columns>
                                                            <obout:Column DataField="ID" Visible="false"></obout:Column>
                                                            <obout:Column DataField="Sequence" HeaderText="Sr.No." AllowEdit="false" Width="7%"
                                                                Align="center" HeaderAlign="center">
                                                                <TemplateSettings TemplateId="ItemTempRemove" />
                                                            </obout:Column>
                                                            <obout:Column DataField="Prod_Code" HeaderText="SKU Code" AllowEdit="false" Width="15%"
                                                                HeaderAlign="left">
                                                            </obout:Column>
                                                            <obout:Column DataField="Prod_Name" HeaderText="Product Name" AllowEdit="false" Width="15%"
                                                                HeaderAlign="left">
                                                            </obout:Column>
                                                            <obout:Column DataField="Prod_Description" HeaderText="Description" AllowEdit="false"
                                                                Width="15%" HeaderAlign="left">
                                                            </obout:Column>
                                                            <obout:Column DataField="UOM" HeaderText="UOM" AllowEdit="false" Width="14%" HeaderAlign="center"
                                                                Align="center">
                                                            </obout:Column>
                                                            <obout:Column DataField="TQty" HeaderText="Transfer Qty" AllowEdit="false"
                                                                Width="13%" HeaderAlign="center" Align="right">
                                                            </obout:Column>                                                                                                                 
                                                        </Columns>
                                                        <Templates>
                                                            <obout:GridTemplate ID="GridTemplate1">
                                                                <Template>
                                                                    <table>
                                                                        <tr>
                                                                            <td style="width: 20px; text-align: center;">
                                                                                <img id="imgbtnRemove" src="../App_Themes/Grid/img/Remove16x16.png" title="Remove"
                                                                                    onclick="removePartFromList('<%# (Container.DataItem["Sequence"].ToString()) %>');"
                                                                                    style="cursor: pointer;" />
                                                                            </td>
                                                                            <td style="width: 35px; text-align: left;">
                                                                                <%# Convert.ToInt32(Container.PageRecordIndex) + 1 %>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </Template>
                                                            </obout:GridTemplate>
                                                            <obout:GridTemplate runat="server" ID="PlainEditTemplate">
                                                                <Template>
                                                                    <div class="divTxtUserQty">
                                                                        <asp:TextBox ID="txtUsrQty" Width="94%" Style="text-align: right;" runat="server"
                                                                            Text="<%# Container.Value %>" onfocus="markAsFocused(this)" onkeydown="AllowDecimal(this,event);"
                                                                            onkeypress="AllowDecimal(this,event);" onblur="markAsBlured(this, '<%# Grid1.Columns[Container.ColumnIndex].DataField %>', <%# Container.PageRecordIndex %>);"></asp:TextBox>
                                                                    </div>
                                                                </Template>
                                                            </obout:GridTemplate>
                                                        </Templates>
                                                    </obout:Grid>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </ContentTemplate>
                </asp:TabPanel>
            </asp:TabContainer>
        </ContentTemplate>
    </asp:UpdatePanel>
    <script type="text/javascript">
        function jsImport() {
            window.open('../WMS/ImportDTrsnsfer.aspx', '_self', '');
        }
        function CreateTask() {
            window.open('../WMS/AssignTask.aspx', null, 'height=350, width=500,status= 0, resizable= 1, scrollbars=0, toolbar=0,location=center,menubar=0, screenX=300; screenY=50');
        }
        function jsAddNew() {
            PageMethods.WMSetSessionAddNew("Add", jsAddNewOnSuccess, null);
        }
        function jsAddNewOnSuccess() {
            window.open('../WMS/WToWTransferDetail.aspx', '_self', '');
        }

        function RequestOpenEntryForm(invoker, state, requestID) {           
            if (state != "gray") {
                if (state == "red") { state = "Edit"; }
                else if (state != "red") { state = "View"; }
                PageMethods.WMSetSessionRequest(invoker, requestID, state, TransferOpenEntryFormOnSuccess, null);
            }
            else {
                showAlert("Not Applicable", '', '#');
            }
        }

        function TransferOpenEntryFormOnSuccess(result) {
            switch (result) {
                case "PickUp":
                    window.open('../WMS/PickUp.aspx', '_self', '');
                    break;
                case "GRN":
                    window.open('../WMS/GRNDetail.aspx', '_self', '');
                    break;
                case "QCOut":
                     window.open('../WMS/QCDetail.aspx', '_self', '');
                    break;               
                case "PutIn":                    
                    window.open('../WMS/PutIn.aspx', '_self', '');
                    break;
                case "Dispatch":
                    window.open('../WMS/DispatchDetails.aspx', '_self', '');
                    break;
                case "QCIn":
                    window.open('../WMS/QCDetail.aspx', '_self', '');
                    break;
                case "AccessDenied":
                    showAlert("Access Denied", '', '#');
                    break;
            }
        }
    </script>
    <style type="text/css">
        /*Grid css*/
        .excel-textbox {
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

        .excel-textbox-focused {
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

        .excel-textbox-error {
            color: #FF0000;
        }

        .ob_gCc2 {
            padding-left: 3px !important;
        }

        .ob_gBCont {
            border-bottom: 1px solid #C3C9CE;
        }

        .excel-checkbox {
            height: 20px;
            line-height: 20px;
        }
    </style>
    <style type="text/css">
        /*POR Collapsable Div*/

        .PanelCaption {
            color: Black;
            font-size: 13px;
            font-weight: bold;
            margin-top: -22px;
            margin-left: -5px;
            position: absolute;
            background-color: White;
            padding: 0px 2px 0px 2px;
        }

        .divHead {
            border: solid 2px #F5DEB3;
            width: 99%;
            text-align: left;
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

        .divDetailExpand {
            border: solid 2px #F5DEB3;
            border-top: none;
            width: 99%;
            padding: 5px 0 5px 0;
        }

        .divDetailCollapse {
            display: none;
        }
        /*End POR Collapsable Div*/
    </style>
    <script type="text/javascript">
        /*Checkbox js for css*/
        var d = document;
        var safari = (navigator.userAgent.toLowerCase().indexOf('safari') != -1) ? true : false;
        var gebtn = function (parEl, child) { return parEl.getElementsByTagName(child); };
        onload = function () {

            var body = gebtn(d, 'body')[0];
            body.className = body.className && body.className != '' ? body.className + ' has-js' : 'has-js';

            if (!d.getElementById || !d.createTextNode) return;
            var ls = gebtn(d, 'label');
            for (var i = 0; i < ls.length; i++) {
                var l = ls[i];
                if (l.className.indexOf('label_') == -1) continue;
                var inp = gebtn(l, 'input')[0];
                if (l.className == 'label_check') {
                    l.className = (safari && inp.checked == true || inp.checked) ? 'label_check c_on' : 'label_check c_off';
                    l.onclick = check_it;
                };
                if (l.className == 'label_radio') {
                    l.className = (safari && inp.checked == true || inp.checked) ? 'label_radio r_on' : 'label_radio r_off';
                    l.onclick = turn_radio;
                };
            };
        };
        var check_it = function () {
            var inp = gebtn(this, 'input')[0];
            if (this.className == 'label_check c_off' || (!safari && inp.checked)) {
                this.className = 'label_check c_on';
                if (safari) inp.click();
            } else {
                this.className = 'label_check c_off';
                if (safari) inp.click();
            };
        };
        var turn_radio = function () {
            var inp = gebtn(this, 'input')[0];
            if (this.className == 'label_radio r_off' || inp.checked) {
                var ls = gebtn(this.parentNode, 'label');
                for (var i = 0; i < ls.length; i++) {
                    var l = ls[i];
                    if (l.className.indexOf('label_radio') == -1) continue;
                    l.className = 'label_radio r_off';
                };
                this.className = 'label_radio r_on';
                if (safari) inp.click();
            } else {
                this.className = 'label_radio r_off';
                if (safari) inp.click();
            };
        };
        /*End*/

    </script>
    <style type="text/css">
        .has-js .label_check, .has-js .label_radio {
            padding-left: 25px;
            padding-bottom: 10px;
        }

        .has-js .label_radio {
            background: url(../App_Themes/Blue/img/radio-off.png) no-repeat;
        }

        .has-js .label_check {
            background: url("../App_Themes/Blue/img/check-off.png") no-repeat;
        }

        .has-js label.c_on {
            background: url("../App_Themes/Blue/img/check-on.png") no-repeat;
        }

        .has-js label.r_on {
            background: url(../App_Themes/Blue/img/radio-on.png) no-repeat;
        }

        .has-js .label_check input, .has-js .label_radio input {
            position: absolute;
            left: -9999px;
        }
    </style>
</asp:Content>

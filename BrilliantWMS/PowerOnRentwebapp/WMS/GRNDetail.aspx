<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/CRM.Master" AutoEventWireup="true" CodeBehind="GRNDetail.aspx.cs" Inherits="BrilliantWMS.WMS.GRNDetail"
    EnableEventValidation="false" Theme="Blue" %>

<%@ Register Assembly="obout_Flyout2_NET" Namespace="OboutInc.Flyout2" TagPrefix="obout" %>
<%@ Register Assembly="obout_Interface" Namespace="Obout.Interface" TagPrefix="obout" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="obout_Grid_NET" Namespace="Obout.Grid" TagPrefix="obout" %>
<%@ Register Assembly="Obout.Ajax.UI" Namespace="Obout.Ajax.UI.HTMLEditor" TagPrefix="obout" %>
<%@ Register Assembly="obout_ComboBox" Namespace="Obout.ComboBox" TagPrefix="obout" %>
<%@ Register Src="../CommonControls/UC_Date.ascx" TagName="UC_Date" TagPrefix="uc1" %>
<%@ Register Src="../Product/UCProductSearch.ascx" TagName="UCProductSearch" TagPrefix="uc1" %>
<%@ Register Src="../CommonControls/Toolbar.ascx" TagName="Toolbar" TagPrefix="uc2" %>
<%@ Register Src="../CommonControls/UCFormHeader.ascx" TagName="UCFormHeader" TagPrefix="uc2" %>
<%@ Register Src="../Document/UC_AttachDocument.ascx" TagName="UC_AttachDocument" TagPrefix="uc3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHolder_FormHeader" runat="server">
    <uc2:Toolbar ID="Toolbar1" runat="server" />
    <uc2:UCFormHeader ID="UCFormHeader1" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPHolder_Form" runat="server">
    <center>
        <span id="imgProcessing" style="display: none; background-color: white;">
            <img src="../App_Themes/Blue/img/ajax-loader.gif" style="top: 50%;" />
        </span>
        <div id="divLoading" style="height: 110%; width: 100%; display: none; top: 0; left: 0;"
            class="modal" runat="server" clientidmode="Static">
            <center>
            </center>
        </div>
        <div class="divHead" id="divHeadPODetail" runat="server" clientidmode="Static">
            <h4>
                <asp:Label ID="lblPurchaseOrder" runat="server" Text="Purchase Order"></asp:Label>
            </h4>
            <a onclick="javascript:divcollapsOpen('divPODetail',this)" id="linkRequest" runat="server">Collapse</a>
        </div>
        <div class="divDetailExpand" id="divPODetail" runat="server" clientidmode="Static">
            <center>
                <table class="tableForm" width="100%">
                    <tr>
                        <td>
                            <asp:Label ID="lblPONumbers" runat="server" Text="Purchase Order Number "></asp:Label>:
                        </td>
                        <td style="text-align: left; font-weight: bold;">
                            <asp:Label runat="server" ID="lblSelectedPO"></asp:Label>
                            <asp:HiddenField ID="hdnRequestID" runat="server" />
                        </td>
                        <td>
                            <asp:Label ID="lblGroupTaskName" runat="server" Text="Job Card Number "></asp:Label>:
                        </td>
                        <td style="text-align: left; font-weight: bold;">
                            <asp:Label runat="server" ID="lblSelectedGTN"></asp:Label>
                            <asp:HiddenField ID="hdnSelectedGTN" runat="server" />
                        </td>
                        <td>
                            <asp:Label ID="lblPODate" runat="server" Text="Purchase Order Date "></asp:Label>:
                        </td>
                        <td style="text-align: left; font-weight: bold;">
                            <asp:Label runat="server" ID="lblPurchaseOrderDate"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblCreatedBy" runat="server" Text="Created By "></asp:Label>:
                        </td>
                        <td style="text-align: left; font-weight: bold;">
                            <asp:Label runat="server" ID="lblPOBy"></asp:Label>
                        </td>
                    </tr>
                </table>
            </center>
        </div>
        <div class="divHead" id="divHeadGRNHead">
            <h4>
                <asp:Label ID="lblGoodsReceiptNote" runat="server" Text="Goods Receipt Note"></asp:Label>
            </h4>
            <a onclick="javascript:divcollapsOpen('divGRNDetail',this)" id="A1" runat="server">Collapse</a>
        </div>
        <div class="divDetailExpand" id="divGRNDetail">
            <div id="dvGRNDetail" runat="server" clientidmode="Static">
                <table class="tableForm" width="100%">
                    <tr>
                        <td>
                            <asp:Label ID="lblGRNNo" runat="server" Text="GRN Number "></asp:Label>:
                        </td>
                        <td style="text-align: left; font-weight: bold;">
                            <asp:Label ID="lblGRNNumber" runat="server"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblGRNDate" runat="server" Text="GRN Date "></asp:Label>:
                        </td>
                        <td style="text-align: left;">
                            <uc1:UC_Date ID="UCGRNDate" runat="server" />
                        </td>
                        <td>
                            <asp:Label ID="lblGRNBy" runat="server" Text="GRN By *"></asp:Label>:
                        </td>
                        <td style="text-align: left; font-weight: bold;">
                            <asp:DropDownList ID="ddlGRNBy" runat="server" Width="182px" DataTextField="userName" DataValueField="userID" AccessKey="1"></asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lblStatus" runat="server" Text="Status *"></asp:Label>:
                        </td>
                        <td style="text-align: left; font-weight: bold;">
                            <asp:DropDownList ID="ddlStatus" runat="server" Width="182px" DataTextField="Status" DataValueField="ID" AccessKey="1"></asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblBatchNo" runat="server" Text="Batch No. *"></asp:Label>:
                        </td>
                        <td style="text-align: left; font-weight: bold;">
                            <asp:TextBox ID="txtBatchNo" runat="server" AccessKey="1"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Label ID="lblShippingNo" runat="server" Text="Shipping No. *"></asp:Label>:
                        </td>
                        <td style="text-align: left; font-weight: bold;">
                            <asp:TextBox ID="txtShippingNo" MaxLength="100" runat="server" AccessKey="1"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Label ID="lblRemark" runat="server" Text="Remark "></asp:Label>:
                        </td>
                        <td colspan="3" style="text-align: left; font-weight: bold;">
                            <asp:TextBox ID="txtRemark" runat="server" Width="99%"></asp:TextBox>
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
                    <%--<tr>
                        <td>
                            Docking Area :
                        </td>
                        <td style="text-align: left; font-weight: bold;">
                            <asp:DropDownList ID="ddlDocking" runat="server" Width="182px">
                                <asp:ListItem Text="Doc1" Value="Doc1" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="Doc2" Value="Doc2" ></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>
                            In Time :
                        </td>
                        <td style="text-align: left; font-weight: bold;">
                            <asp:TextBox runat="server" ID="txtInTime" MaxLength="50" Width="180px" Text="10.00 AM"></asp:TextBox>
                        </td>
                        <td>
                            Out Time :
                        </td>
                        <td style="text-align: left; font-weight: bold;">
                            <asp:TextBox runat="server" ID="txtOutTime" MaxLength="50" Width="180px" Text="10.50 AM"></asp:TextBox>
                        </td>
                    </tr>--%>
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
                            <uc1:UC_Date ID="UC_ShippingDate" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>Transporter Name :
                        </td>
                        <td style="text-align: left; font-weight: bold;">
                            <asp:TextBox runat="server" ID="txtTransporterName" MaxLength="50" Width="180px"></asp:TextBox>
                        </td>
                        <td>Transport Remark :
                        </td>
                        <td style="text-align: left; font-weight: bold;" colspan="3">
                            <asp:TextBox runat="server" ID="txtTransporterRemark" Width="99%"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <table class="gridFrame" width="100%" id="tblCart">
                    <tr>
                        <td>
                            <table style="width: 100%">
                                <tr>
                                    <td style="text-align: left;">
                                        <asp:Label ID="lblgrnpartlist" CssClass="headerText" runat="server" Text="Goods Receipt Note Part List"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <obout:Grid ID="Grid1" runat="server" CallbackMode="true" Serialize="true" AllowColumnReordering="true"
                                AllowColumnResizing="true" AutoGenerateColumns="false" AllowPaging="false" ShowLoadingMessage="true"
                                AllowSorting="false" AllowManualPaging="false" AllowRecordSelection="false" ShowFooter="false" OnRebind="Grid1_OnRebind" OnRowDataBound="Grid1_OnRowDataBound"
                                Width="100%" PageSize="-1">
                                <ClientSideEvents ExposeSender="true" />
                                <Columns>
                                    <obout:Column DataField="Prod_ID" Visible="false"></obout:Column>
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
                                    <obout:Column DataField="POQty" HeaderText="PO Qty" AllowEdit="false"
                                        Width="13%" HeaderAlign="center" Align="center">
                                        <TemplateSettings TemplateId="TemplatePOQty" />
                                    </obout:Column>
                                    <obout:Column DataField="GRNQty" Width="14%" HeaderAlign="center" HeaderText="GRN Qty"
                                        Align="center" AllowEdit="false">
                                        <TemplateSettings TemplateId="PlainEditTemplate" />
                                    </obout:Column>
                                    <obout:Column DataField="ShortQty" HeaderText="Short Qty" AllowEdit="false"
                                        Width="13%" HeaderAlign="center" Align="center">
                                        <TemplateSettings TemplateId="TemplateShortQty" />
                                    </obout:Column>
                                    <obout:Column DataField="ExcessQty" HeaderText="Excess Qty" AllowEdit="false"
                                        Width="13%" HeaderAlign="center" Align="center">
                                        <TemplateSettings TemplateId="TemplateExcessQty" />
                                    </obout:Column>
                                    <obout:Column DataField="ASNQty" HeaderText="ASN Qty" AllowEdit="false"
                                        Width="13%" HeaderAlign="center" Align="center">
                                        <TemplateSettings TemplateId="TemplateASNQty" />
                                    </obout:Column>
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
                                    <obout:GridTemplate runat="server" ID="PlainEditTemplate">
                                        <Template>
                                            <div class="divTxtUserQty">
                                                <asp:TextBox ID="txtUsrQty" Width="94%" Style="text-align: right;" runat="server"
                                                    Text="<%# Container.Value %>" onfocus="markAsFocused(this)" onkeydown="AllowDecimal(this,event);"
                                                    onkeypress="AllowDecimal(this,event);" onblur="markAsBlured(this, '<%# Grid1.Columns[Container.ColumnIndex].DataField %>', <%# Container.PageRecordIndex %>);"></asp:TextBox>
                                            </div>
                                        </Template>
                                    </obout:GridTemplate>
                                    <obout:GridTemplate runat="server" ID="TemplatePOQty">
                                        <Template>
                                            <div class="divPOQty">
                                                <asp:Label ID="SelPoQty" runat="server" Text="<%# Container.Value %>">0</asp:Label>
                                            </div>
                                        </Template>
                                    </obout:GridTemplate>
                                    <obout:GridTemplate runat="server" ID="TemplateShortQty">
                                        <Template>
                                            <div class="divPOQty">
                                                <asp:Label ID="ShortQty" runat="server" Text="<%# Container.Value %>">0</asp:Label>
                                            </div>
                                        </Template>
                                    </obout:GridTemplate>
                                    <obout:GridTemplate runat="server" ID="TemplateExcessQty">
                                        <Template>
                                            <div class="divPOQty">
                                                <asp:Label ID="ExcessQty" runat="server" Text="<%# Container.Value %>">0</asp:Label>
                                            </div>
                                        </Template>
                                    </obout:GridTemplate>
                                    <obout:GridTemplate runat="server" ID="TemplateASNQty">
                                        <Template>
                                            <div class="divPOQty">
                                                <asp:Label ID="ASNQty" runat="server" Text="<%# Container.Value %>">0</asp:Label>
                                            </div>
                                        </Template>
                                    </obout:GridTemplate>
                                    <obout:GridTemplate runat="server" ID="TemplateSerialnumber">
                                        <Template>
                                            <div class="divPOQty">
                                                <asp:Label ID="lblPrdSerialNo" runat="server" Text="<%# Container.Value %>">0</asp:Label>
                                            </div>
                                        </Template>
                                    </obout:GridTemplate>
                                </Templates>
                            </obout:Grid>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <div class="divHead" id="divLoaderHead" runat="server" clientidmode="Static">
            <h4>
                <asp:Label ID="lblLoader" runat="server" Text="Loader"></asp:Label>
            </h4>
            <a onclick="javascript:divcollapsOpen('divLoaderDetails',this)" id="linkLoaderDertail"
                runat="server">Collapse</a>
        </div>
        <div class="divDetailExpand" id="divLoaderDetails" runat="server" clientidmode="Static">
            <center>
                <table class="gridFrame" width="80%">
                    <tr>
                        <td>
                            <table style="width: 100%">
                                <tr>
                                    <td style="text-align: left;">
                                        <asp:Label ID="lblLoaderDetails" runat="server" Style="color: white; font-size: 15px; font-weight: bold;"
                                            Text="Loader"></asp:Label>
                                    </td>
                                    <td style="text-align: right;">
                                        <input type="button" id="Button2" runat="server" value="Add Loaders"
                                            onclick="AddLoader();" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <obout:Grid ID="grdLoader" runat="server" Width="100%" CallbackMode="true" Serialize="true"
                                AllowColumnReordering="true" AllowColumnResizing="true" AutoGenerateColumns="false"
                                AllowPaging="false" ShowLoadingMessage="true" AllowSorting="false" AllowManualPaging="false"
                                AllowRecordSelection="false" ShowFooter="false">
                                <Columns>
                                    <obout:Column HeaderText="View" DataField="ID" Width="4%" Align="center" HeaderAlign="center">
                                        <TemplateSettings TemplateId="TemplateVWLoader" />
                                    </obout:Column>
                                    <obout:Column DataField="LoaderName" HeaderText="Loader Name" Width="8%" Align="center"
                                        HeaderAlign="center">
                                    </obout:Column>
                                    <obout:Column DataField="InTime" HeaderText="In Time" Width="8%" Align="center"
                                        HeaderAlign="center">
                                    </obout:Column>
                                    <obout:Column DataField="OutTime" HeaderText="Out Time" Width="8%" Align="center"
                                        HeaderAlign="center">
                                    </obout:Column>
                                    <obout:Column DataField="Total" HeaderText="Total" Width="8%" Align="center"
                                        HeaderAlign="center">
                                    </obout:Column>
                                </Columns>
                                <Templates>
                                    <obout:GridTemplate runat="server" ID="TemplateVWLoader">
                                        <Template>
                                            <asp:ImageButton ID="imgBtnLoader" runat="server" ImageUrl="../App_Themes/Blue/img/Search16.png"
                                                CausesValidation="false" ToolTip='<%# (Container.Value) %>' data-containerId="<%# (Container.Value) %>" OnClick="imgBtnLoader_OnClick" OnClientClick="AddLoader(this);return false;" />
                                            <%-- OnClick="imgBtnView_OnClick" OnClientClick="AddCorrespondanceVW(this);return false;"  OnClick="imgBtnView_OnClick" OnClientClick="openCorrenpondance('<%# (Container.Value) %>')"--%>
                                        </Template>
                                    </obout:GridTemplate>
                                </Templates>
                            </obout:Grid>
                        </td>
                    </tr>
                </table>
            </center>
        </div>
        <div class="divHead" id="divCorrespondanceHead" runat="server" clientidmode="Static">
            <h4>
                <asp:Label ID="lblCorrespondance" runat="server" Text="Correspondance"></asp:Label>
            </h4>
            <a onclick="javascript:divcollapsOpen('divCorrespondanceDetails',this)" id="linkCorrespondanceDetail"
                runat="server">Collapse</a>
        </div>
        <div class="divDetailExpand" id="divCorrespondanceDetails" runat="server" clientidmode="Static">
            <center>
                <table class="gridFrame" width="80%">
                    <tr>
                        <td>
                            <table style="width: 100%">
                                <tr>
                                    <td style="text-align: left;">
                                        <asp:Label ID="lblinbox" runat="server" Style="color: white; font-size: 15px; font-weight: bold;"
                                            Text="Inbox"></asp:Label>
                                    </td>
                                    <td style="text-align: right;">
                                        <input type="button" id="btnAddNewCorrespondance" runat="server" value="Add New"
                                            onclick="AddCorrespondance();" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <obout:Grid ID="GVInboxPOR" runat="server" Width="100%" CallbackMode="true" Serialize="true"
                                AllowColumnReordering="true" AllowColumnResizing="true" AutoGenerateColumns="false"
                                AllowPaging="false" ShowLoadingMessage="true" AllowSorting="false" AllowManualPaging="false"
                                AllowRecordSelection="false" ShowFooter="false">
                                <%--OnRebind="GVInboxPOR_OnRebind" OnRebind="GVInboxPOR_OnRebind"--%>
                                <Columns>
                                    <obout:Column HeaderText="View" DataField="Id" Width="4%" Align="center" HeaderAlign="center">
                                        <TemplateSettings TemplateId="TemplateEdit" />
                                    </obout:Column>
                                    <obout:Column DataField="OrderHeadId" HeaderText="Request No." Width="8%" Align="center"
                                        HeaderAlign="center">
                                    </obout:Column>
                                    <obout:Column DataField="Orderdate" HeaderText="Requested Date" Width="8%" DataFormatString="{0:dd-MMM-yy}">
                                    </obout:Column>
                                    <obout:Column DataField="StatusName" HeaderText="Status" Wrap="true" Width="10%">
                                    </obout:Column>
                                    <obout:Column DataField="MessageFromName" HeaderText="Message From" Width="8%">
                                        <TemplateSettings TemplateId="msgFrm" />
                                    </obout:Column>
                                    <obout:Column DataField="MessageTitle" HeaderText="Message Title" Width="8%">
                                    </obout:Column>
                                    <%--<obout:Column DataField="Message" HeaderText="Message" Width="15%" Wrap="true">
                                        <TemplateSettings TemplateId="GetMessage" />
                                    </obout:Column>--%>
                                </Columns>
                                <Templates>
                                    <obout:GridTemplate runat="server" ID="msgFrm">
                                        <Template>
                                            <%# (Container.DataItem["MessageFromName"].ToString() == "" ? "System Generated" : Container.DataItem["MessageFromName"].ToString())%>
                                        </Template>
                                    </obout:GridTemplate>
                                    <obout:GridTemplate runat="server" ID="TemplateEdit">
                                        <Template>
                                            <asp:ImageButton ID="imgBtnView" runat="server" ImageUrl="../App_Themes/Blue/img/Search16.png"
                                                CausesValidation="false" ToolTip='<%# (Container.Value) %>' data-containerId="<%# (Container.Value) %>" />
                                            <%-- OnClick="imgBtnView_OnClick" OnClientClick="AddCorrespondanceVW(this);return false;"  OnClick="imgBtnView_OnClick" OnClientClick="openCorrenpondance('<%# (Container.Value) %>')"--%>
                                        </Template>
                                    </obout:GridTemplate>
                                    <obout:GridTemplate runat="server" ID="GetMessage">
                                        <Template>
                                            <asp:Panel ID="pnlQuizPlainText" runat="server">
                                                <asp:Label ID="setTextQCss" runat="server"><span id='QHolder<%# Container.DataItem["Id"] %>'>
                                                    <asp:Label ID="Label1" Text='<%# Container.DataItem["Message"] %>' runat="server"></asp:Label>
                                                </span>
                                                    <script type="text/javascript">
                                                        //var getQHolderObj = document.getElementById('<%#Container.DataItem["Message"] %>');
                                                        var getQHolderObj = document.getElementById('This is Message');
                                                        // if (getQHolderObj != null) {
                                                        var getHtmlQuestionStr = getQHolderObj; //.innerHTML;
                                                        var filterHtmlQuestionStr = getHtmlQuestionStr.split('andBrSt;').join('<');
                                                        getHtmlQuestionStr = filterHtmlQuestionStr;
                                                        filterHtmlQuestionStr = getHtmlQuestionStr.split('andBrEn;').join('>');
                                                        getHtmlQuestionStr = filterHtmlQuestionStr;
                                                        getQHolderObj.innerHTML = getHtmlQuestionStr;
                                                        // }        
                                                    </script>
                                                </asp:Label>
                                            </asp:Panel>
                                        </Template>
                                    </obout:GridTemplate>
                                </Templates>
                            </obout:Grid>
                        </td>
                    </tr>
                </table>
            </center>
        </div>
        <div class="divHead" id="divDocumentHead" runat="server" clientidmode="Static">
            <h4>
                <asp:Label ID="lblDocument" runat="server" Text="Document"></asp:Label>
            </h4>
            <a onclick="javascript:divcollapsOpen('divDocumentDetails',this)" id="linkDocumentDetail" runat="server">Collapse</a>
        </div>
        <div class="divDetailExpand" id="divDocumentDetails" runat="server" clientidmode="Static">
            <uc3:UC_AttachDocument ID="UC_AttachmentDocument1" runat="server" />
        </div>
        <asp:HiddenField ID="hdnProductSearchSelectedRec" runat="server" ClientIDMode="Static" />
    </center>
    <script src="//code.jquery.com/jquery-1.10.2.js"></script>
    <script type="text/javascript">
        var ddlGRNBy = document.getElementById("<%= ddlGRNBy.ClientID %>");
        var ddlStatus = document.getElementById("<%= ddlStatus.ClientID %>");
        var txtBatchNo = document.getElementById("<%= txtBatchNo.ClientID %>");
        var txtShippingNo = document.getElementById("<%= txtShippingNo.ClientID %>");
        var txtRemark = document.getElementById("<%= txtRemark.ClientID %>");
        var txtAirwayBill = document.getElementById("<%= txtAirwayBill.ClientID %>");
        var txtShippingType = document.getElementById("<%= txtShippingType.ClientID %>");
        var txtTransporterName = document.getElementById("<%= txtTransporterName.ClientID %>");
        var txtTransporterRemark = document.getElementById("<%= txtTransporterRemark.ClientID %>");

        onload();
        function onload() {
            var v = document.getElementById("btnConvertTo");
            v.style.visibility = 'hidden';

            var exp = document.getElementById("btnExport");
            exp.style.visibility = 'hidden';

            var imp = document.getElementById("btnImport");
            imp.style.visibility = 'hidden';

            var ml = document.getElementById("btnMail");
            ml.style.visibility = 'hidden';

            var pt = document.getElementById("btnPrint");
            pt.style.visibility = 'hidden';
        }

        function GetIndexQty(myDD, usrInputQty, Index, poQty, shortQty, excessQty, asnQty) {
            // alert(Number(asnQty));
            var enterQty = myDD.value;
            var ShowShortQty = document.getElementById(shortQty);
            var ShowExcessQty = document.getElementById(excessQty);

            if (Number(asnQty) > 0) {
                var Calculation = Number(asnQty) - Number(enterQty);
                if (Calculation == 0) {
                    ShowShortQty.innerHTML = 0.00;
                    ShowExcessQty.innerHTML = 0.00;

                    var order = new Object();
                    order.Sequence = Index + 1;
                    order.GRNQty = enterQty;
                    order.ShortQty = ShowShortQty.innerHTML;
                    order.ExcessQty = ShowExcessQty.innerHTML;
                    PageMethods.WMUpdGRNPart(order, null, null);

                } else if (Calculation > 0) {
                    ShowShortQty.innerHTML = Calculation;
                    ShowExcessQty.innerHTML = 0.00;

                    var order = new Object();
                    order.Sequence = Index + 1;
                    order.GRNQty = enterQty;
                    order.ShortQty = ShowShortQty.innerHTML;
                    order.ExcessQty = ShowExcessQty.innerHTML;
                    PageMethods.WMUpdGRNPart(order, null, null);

                } else if (Calculation < 0) {
                    ShowShortQty.innerHTML = 0.00;
                    ShowExcessQty.innerHTML = 0.00;
                    myDD.value = asnQty;
                    showAlert("GRN Qty Must Not Greater Than ASN Qty", "Error", "#");
                }
            }
            else {
                var Calculation = Number(poQty) - Number(enterQty);
                if (Calculation == 0) {
                    ShowShortQty.innerHTML = 0.00;
                    ShowExcessQty.innerHTML = 0.00;
                } else if (Calculation > 0) {
                    ShowShortQty.innerHTML = Calculation;
                    ShowExcessQty.innerHTML = 0.00;
                } else if (Calculation < 0) {
                    ShowShortQty.innerHTML = 0.00;
                    ShowExcessQty.innerHTML = Math.abs(Calculation);
                }
            }
            var order = new Object();
            order.Sequence = Index + 1;
            order.GRNQty = enterQty;
            order.ShortQty = ShowShortQty.innerHTML;
            order.ExcessQty = ShowExcessQty.innerHTML;
            PageMethods.WMUpdGRNPart(order, null, null);
        }

        function jsSaveData() {
            var validate = validateForm('divGRNDetail');
            if (validate == true) {
                var isContainsZero = 'no';
                var matches = 0;
                $(".divTxtUserQty input").each(function (i, val) {
                    if (($(this).val() == '0.00') || ($(this).val() == '0') || ($(this).val() == 0)) {
                        isContainsZero = 'yes';
                    }
                });

                if (isContainsZero != 'yes') {
                    LoadingOn(true);

                    var GRNDate = getDateFromUC("<%= UCGRNDate.ClientID %>");
                    var ShippingDate = getDateFromUC("<%= UC_ShippingDate.ClientID %>");
                    var obj1 = new Object();
                    obj1.ShipID = txtShippingNo.value.toString();
                    obj1.GRNDate = GRNDate;
                    obj1.ReceivedBy = parseInt(ddlGRNBy.options[ddlGRNBy.selectedIndex].value);
                    obj1.BatchNo = txtBatchNo.value.toString();
                    obj1.Remark = txtRemark.value.toString();
                    obj1.AirwayBill = txtAirwayBill.value.toString();
                    obj1.ShippingType = txtShippingType.value.toString();
                    obj1.Status = parseInt(ddlStatus.options[ddlStatus.selectedIndex].value);
                    obj1.ShippingDate = ShippingDate;
                    obj1.TransporterName = txtTransporterName.value.toString();
                    obj1.TransporterRemark = txtTransporterRemark.value.toString();

                    PageMethods.WMSaveGRNHead(obj1, WMSaveGRNHead_onSuccessed, WMSaveGRNHead_onFailed);

                } else {
                    showAlert("One or more GRN Quantity is zero", "error", "#");
                }
            } else {
                showAlert("Fill All Mandatory Fields...", "error", "#");
            }
        }
        function WMSaveGRNHead_onSuccessed(result) {
            if (result >= 1) {
                var sessiontr = '<%= Session["TRID"] %>';
                if (sessiontr != '') {
                    showAlert("GRN saved successfully", "info", "../WMS/Transfer.aspx");
                } else {
                    showAlert("GRN saved successfully", "info", "../WMS/Inbound.aspx");
                }
            } else {
                if (sessiontr != '') {
                    showAlert("Some Error Occured!", "error", "../WMS/Transfer.aspx");
                } else {
                    showAlert("Some Error Occured!", "error", "../WMS/Inbound.aspx");
                }
            }
        }
        function WMSaveGRNHead_onFailed() { showAlert("Error occurred", "Error", "../WMS/Inbound.aspx"); }

        function removePartFromList(sequence) {
            /*Remove Part from list*/
            var hdnProductSearchSelectedRec = document.getElementById('hdnProductSearchSelectedRec');
            hdnProductSearchSelectedRec.value = "1";
            PageMethods.WMRemovePartFromRequest(sequence, removePartFromListOnSussess, null);
        }

        function removePartFromListOnSussess(result) {
            if (result == 0) {
                showAlert("Not Applicable.......", "error", "#");
            } else {
                Grid1.refresh();
            }
        }

        function AddNewASN() {
            window.open('../WMS/AsnDetail.aspx', '_self', '');
        }
        function AddCorrespondance() {
            window.open('../PowerOnRent/Correspondance.aspx?VW=', null, 'height=475px, width=825px,status= 0, resizable= 1, scrollbars=1, toolbar=0,location=center,menubar=0, screenX=300; screenY=50');
        }

        function AddCorrespondanceVW(viewObj) {
            var getObjId = $(viewObj).attr('data-containerId');
            // alert(getTitle);
            //  return false;
            window.open('../PowerOnRent/Correspondance.aspx?VW=' + getObjId, null, 'height=450px, width=825px,status= 0, resizable= 1, scrollbars=1, toolbar=0,location=center,menubar=0, screenX=300; screenY=50');
        }

        function openCorrenpondance(CorID) {
            window.open('../PowerOnRent/Correspondance.aspx?CORID=' + CorID + '', null, 'height=450px, width=825px,status= 0, resizable= 1, scrollbars=1, toolbar=0,location=center,menubar=0, screenX=300; screenY=50');
        }
        function AddLoader() {
            window.open('../WMS/Loader.aspx?VW=', null, 'height=275px, width=825px,status= 0, resizable= 1, scrollbars=1, toolbar=0,location=center,menubar=0, screenX=300; screenY=50');
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

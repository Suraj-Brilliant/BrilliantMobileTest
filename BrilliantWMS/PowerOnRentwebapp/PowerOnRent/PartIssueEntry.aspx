<%@ Page Title="GWC | Dispatch" Language="C#" MasterPageFile="~/MasterPage/CRM.Master"
    AutoEventWireup="true" CodeBehind="PartIssueEntry.aspx.cs" Inherits="BrilliantWMS.PowerOnRent.PartIssueEntry"
    EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="obout_Grid_NET" Namespace="Obout.Grid" TagPrefix="obout" %>
<%@ Register Src="../CommonControls/UC_Date.ascx" TagName="UC_Date" TagPrefix="uc1" %>
<%@ Register Src="../Product/UCProductSearch.ascx" TagName="UCProductSearch" TagPrefix="uc1" %>
<%@ Register Src="../CommonControls/Toolbar.ascx" TagName="Toolbar" TagPrefix="uc2" %>
<%@ Register Src="../CommonControls/UCFormHeader.ascx" TagName="UCFormHeader" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHolder_FormHeader" runat="server">
    <uc2:Toolbar ID="Toolbar1" runat="server" />
    <uc2:UCFormHeader ID="UCFormHeader1" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPHolder_Form" runat="server">
    <center>
        <span id="imgProcessing" style="display: none;">Please wait... </span>
        <div id="divLoading" style="height: 110%; width: 100%; display: none; top: 0; left: 0;"
            class="modal">
            <center>
            </center>
        </div>
        <div class="divHead">
            <h4>
                Request 
            </h4>
            <a onclick="javascript:divcollapsOpen('divRequestDetail',this)" id="linkRequest" runat="server">Collapse</a>
        </div>
        <div class="divDetailExpand" id="divRequestDetail" runat="server" clientidmode="Static">
            <table class="tableForm">
                <tr>
                    <td>
                        Request No.* :
                    </td>
                    <td style="text-align: left; font-weight: bold;">
                        <asp:Label runat="server" ID="lblRequestNo2"></asp:Label>
                        <asp:HiddenField ID="hdnRequestID" runat="server" />
                    </td>
                    <td>
                        Site / Warehouse :
                    </td>
                    <td style="text-align: left; font-weight: bold;">
                        <asp:Label runat="server" ID="lblSites" Width="182px"></asp:Label>
                        <asp:HiddenField ID="hdnSiteID" runat="server" />
                    </td>
                    <td>
                        Status :
                    </td>
                    <td style="text-align: left; font-weight: bold;">
                        <asp:Label runat="server" ID="lblRequestStatus" Width="182px"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        Request Date :
                    </td>
                    <td style="text-align: left; font-weight: bold;">
                        <asp:Label runat="server" ID="lblRequestDate" Width="182px"></asp:Label>
                    </td>
                    <td>
                        Request Type :
                    </td>
                    <td style="text-align: left; font-weight: bold;">
                        <asp:Label runat="server" ID="lblRequestType" Width="182px"></asp:Label>
                    </td>
                    <td>
                        Requested By :
                    </td>
                    <td style="text-align: left; font-weight: bold;">
                        <asp:Label runat="server" ID="lblRequestedBy" Width="182px"></asp:Label>
                    </td>
                </tr>
            </table>
        </div>
        <div class="divHead" id="divHeadIssueHistory">
            <h4>
                Approval 
            </h4>
            <a onclick="javascript:divcollapsOpen('divIssueHistory',this)" id="linkIssueHistory">
                Collapse</a>
        </div>
        <div class="divDetailExpand" id="divIssueHistory">
            <table style="font-size: 15px; font-weight: bold; color: Black; width: 100%;">
              <tr>
                  <td>
                      <obout:Grid ID="gvApprovalRemark" runat="server" CallbackMode="true" Serialize="true"
                            AllowColumnReordering="true" AllowColumnResizing="true" AutoGenerateColumns="false"
                            AllowPaging="false" ShowLoadingMessage="true" AllowSorting="false" AllowManualPaging="false"
                            AllowRecordSelection="false" ShowFooter="false" Width="100%" PageSize="-1">
                            <Columns>
                                <obout:Column DataField="ApproverLevel" HeaderText="Approver Level"  AllowEdit="false"
                                    Width="15%" Align="center" HeaderAlign="center">
                                </obout:Column>
                                <obout:Column DataField="ApproverName" HeaderText="Approver Name" AllowEdit="false"
                                    Width="15%" Align="center" HeaderAlign="center">
                                </obout:Column>
                                <obout:Column DataField="ApprovedDate" HeaderText="Approved Date" AllowEdit="false"
                                    Width="15%" Align="center" HeaderAlign="center">
                                </obout:Column>                                
                                <obout:Column DataField="Remark" HeaderText="Remark" AllowEdit="false" Width="15%"
                                    Align="center" HeaderAlign="center">
                                </obout:Column>
                                <obout:Column DataField="Status" HeaderText="Status" AllowEdit="false"
                                    Width="15%" Align="center" HeaderAlign="center">
                                </obout:Column>
                                  <obout:Column DataField="ImgApproval" HeaderText="Approval" HeaderAlign="center"
                                    Align="center" Width="7%">
                                    <TemplateSettings TemplateId="GTStatusGUIApproval" />
                                </obout:Column>
                            </Columns>
                            <Templates>
                               <obout:GridTemplate ID="GTStatusGUIApproval" runat="server">
                                  <Template>
                                      <center>
                                         <div class='<%# ("POR" + Container.Value) %>' onclick="RequestOpenEntryForm('Approval','<%# Container.Value %>', '<%# Container.DataItem["PRH_ID"] %>')">
                                         </div>
                                      </center>
                                  </Template>
                               </obout:GridTemplate>
                            </Templates>
                        </obout:Grid>
                  </td>
              </tr>
               <%-- <tr>
                    <td>
                        <span style="font-style: italic; color: black; background-color: Silver; padding: 2px;">
                            Note : To AddNew Issue click on [Add New] or to Edit existing Issue Record Click
                            on Pending Issue Record [Red box] </span>
                    </td>
                    <td>
                        <div class="PORgray">
                        </div>
                    </td>
                    <td>
                        Not Applicable
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        <div class="PORgreen">
                        </div>
                    </td>
                    <td>
                        Completed
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        <div class="PORred">
                        </div>
                    </td>
                    <td>
                        Pending
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        <div class="PORgreenRed">
                        </div>
                    </td>
                    <td>
                        Partially Completed
                    </td>
                </tr>--%>
            </table>
           <%-- <iframe runat="server" id="iframePORIssue" clientidmode="Static" src="#" width="100%"
                style="border: none; max-height: 500px;"></iframe>--%>
            <span id="pupUpLoading" style="display: none; background-color: #FFFF99; padding: 4px;
                font-size: 15px; font-weight: bold; color: Black;">Loading...</span>
        </div>
        <div class="divHead" id="divIssueHead">
            <h4>
                Dispatch
            </h4>
            <a onclick="javascript:divcollapsOpen('divIssueDetail',this)" id="linkIssueDetail">Collapse</a>
        </div>
        <div class="divDetailExpand" id="divIssueDetail">
            <table class="tableForm">
                <tr>
                    <td>
                        Issue No.* :
                    </td>
                    <td style="text-align: left; font-weight: bold;">
                        <asp:Label runat="server" ID="lblIssueNo" Width="180px"></asp:Label>
                    </td>
                    <td colspan="2" style="visibility:hidden;">
                        <a style="font-weight: bold;" href="../PowerOnRent/TransferFrmSite.aspx">Click here
                            to Transfer From Site </a>
                    </td>
                    <td colspan="3" style="visibility:hidden;">
                        <a style="font-weight: bold;" href="../PowerOnRent/HQGoodsReceiptEntry.aspx">Click here
                            to Create Goods Receipt [HQ]</a>
                    </td>
                </tr>
                <tr>
                    <td>
                        Shipped Date :
                    </td>
                    <td style="text-align: left">
                        <asp:TextBox runat="server" ID="txtShippedDate" MaxLength="50" Width="100px" Text="17-Aug-2016"></asp:TextBox>
                        <asp:DropDownList runat="server" ID="ddlIssuedBy" Width="182px" AccessKey="1" DataTextField="userName"
                            DataValueField="userID" Visible="false">
                        </asp:DropDownList>
                    </td>
                    <td>
                        Received Date :
                    </td>
                    <td style="text-align: left;">
                        <uc1:UC_Date ID="UC_IssueDate" runat="server" />
                    </td>
                    <td>
                        Close Date :
                    </td>
                    <td style="text-align: left">
                    <asp:TextBox runat="server" ID="TextBox1" MaxLength="50" Width="100px" Text="31-Aug-2016"></asp:TextBox>
                        <asp:DropDownList runat="server" ID="ddlStatus" Width="182px" AccessKey="1" DataTextField="Status"
                            DataValueField="ID" Visible="false">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        Remark :
                    </td>
                    <td colspan="5" style="text-align: left">
                       <asp:TextBox runat="server" ID="txtRemark" TextMode="MultiLine"  Width="100%"></asp:TextBox>
                    </td>
                </tr>
               
            </table>
           
        </div>
         <div class="divHead" id="divCorrespondanceHead" runat="server" clientidmode="Static">
            <h4>
                Correspondance
            </h4>
            <a onclick="javascript:divcollapsOpen('divCorrespondanceDetails',this)" id="linkCorrespondanceDetail" runat="server">
                Collapse</a>
        </div>
        <div class="divDetailExpand" id="divCorrespondanceDetails" runat="server" clientidmode="Static">
            <center>
                <table class="gridFrame" width="80%">
                    <tr>
                        <td>
                            <table style="width: 100%">
                                <tr>
                                    <td style="text-align: left;">
                                        <a style="color: white; font-size: 15px; font-weight: bold;">Inbox</a>
                                    </td>
                                    <td style="text-align: right;">
                                        <input type="button" id="btnAddNewCorrespondance" value="Add New" onclick="AddCorrespondance();" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <obout:Grid ID="GVInboxPOR" runat="server" Width="100%" AllowColumnReordering="true"
                                AllowGrouping="false" AutoGenerateColumns="false">
                                <Columns>
                                    <obout:Column DataField="" HeaderText="Request No." Width="8%">
                                    </obout:Column>
                                    <obout:Column DataField="RequestedDate" HeaderText="Requested Date" Width="8%" DataFormatString="{0:dd-MMM-yy}">
                                    </obout:Column>
                                    <obout:Column DataField="ApprovalStatus" HeaderText="Status" Wrap="true" Width="8%">
                                    </obout:Column>
                                    <obout:Column DataField="" HeaderText="Message From" Width="8%">
                                    </obout:Column>
                                    <obout:Column DataField="" HeaderText="Message To" Width="8%">
                                    </obout:Column>
                                    <obout:Column DataField="Title" HeaderText="Message Title" Width="8%">
                                    </obout:Column>
                                    <obout:Column DataField="IsRead" HeaderText="Department" Width="8%">
                                    </obout:Column>
                                    <obout:Column DataField="" HeaderText="Company Name" Width="8%">
                                    </obout:Column>
                                </Columns>
                                <Templates>
                                    <obout:GridTemplate runat="server" ID="GTDetails">
                                        <Template>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <a class='<%# Container.DataItem["IsRead"] %>' style="max-width: 150px">
                                                            <%#  Container.DataItem["Particulars"]  %></a>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <a><a class="details" style="max-width: 150px">Title :
                                                            <%# Container.DataItem["Title"]  %></a> </a>
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
        </div>
         <table>
             <tr style="visibility:hidden;">
                    <td colspan="6" style="font-style: italic; text-align: left; font-weight: bold;">
                        <hr style="width: 87%; margin-top: 8px; float: right;" />
                        <span>Transport Detail</span>
                    </td>
                </tr>
                <tr style="visibility:hidden;">
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
                        <uc1:UC_Date ID="UC_ShippingDate" runat="server" />
                    </td>
                </tr>
                <tr style="visibility:hidden;">
                    <td>
                        Exp. Delivery Date :
                    </td>
                    <td style="text-align: left;">
                        <uc1:UC_Date ID="UC_ExpDeliveryDate" runat="server" />
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
                        <asp:TextBox runat="server" ID="txtIssueRemark" MaxLength="100" Width="180px"></asp:TextBox>
                    </td>
                </tr>
            </table>
            <table class="gridFrame" id="tblCart" style="visibility:hidden;">
                <tr>
                    <td>
                        <table style="width: 100%">
                            <tr>
                                <td style="text-align: left;">
                                    <a class="headerText">Issue Part List </a>
                                </td>                                
                                <td style="text-align: right;">
                                    <asp:Button ID="btnTransferFromSite" runat="server" Text="Transfer Form Site" OnClick="btnTransferFromSite_OnClick" style="cursor:pointer;" />
                                    <asp:Button ID="btnHQGRN" runat="server" Text="Create Goods Receipt [HQ]" OnClick="btnHQGRN_OnClick" style="cursor:pointer;" />
                                    <input type="button" id="btnAddPartToIssue" value="Add Items" onclick="openPendingList();" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <obout:Grid ID="Grid1" runat="server" CallbackMode="true" Serialize="true" AllowColumnReordering="true"
                            AllowColumnResizing="true" AutoGenerateColumns="false" AllowPaging="false" ShowLoadingMessage="true"
                            AllowSorting="false" AllowManualPaging="false" AllowRecordSelection="true" ShowFooter="false"
                            Width="100%" PageSize="-1" OnRebind="Grid1_OnRebind" OnRowDataBound="Grid1_RowDataBound">
                            <ClientSideEvents ExposeSender="true" />
                            <Columns>
                                <obout:Column DataField="Prod_ID" HeaderText="Edit" Width="7%" Align="center" HeaderAlign="center">
                                    <TemplateSettings TemplateId="TemplateEdit" />
                                </obout:Column>
                                <obout:Column DataField="Sequence" HeaderText="Sr.No." AllowEdit="false" Width="7%"
                                    Align="left" HeaderAlign="center">
                                    <TemplateSettings TemplateId="ItemTempRemove" />
                                </obout:Column>
                                <obout:Column DataField="Prod_Code" HeaderText="Code" AllowEdit="false" Width="10%"
                                    HeaderAlign="left">
                                    <%--<TemplateSettings TemplateId="TemplateDefaultView" />--%>
                                </obout:Column>
                                <obout:Column DataField="Prod_Name" HeaderText="Part Name" AllowEdit="false" Width="20%"
                                    HeaderAlign="left" Wrap="true">
                                    <%-- <TemplateSettings TemplateId="TemplateDefaultView" />--%>
                                </obout:Column>
                                <obout:Column DataField="Prod_Description" HeaderText="Description" AllowEdit="false"
                                    Width="15%" HeaderAlign="left">
                                    <%-- <TemplateSettings TemplateId="TemplateDefaultView" />--%>
                                </obout:Column>
                                <obout:Column DataField="UOM" HeaderText="UOM" AllowEdit="false" Width="7%" HeaderAlign="left">
                                </obout:Column>
                                <obout:Column DataField="UOMID" AllowEdit="false" Width="0%" Visible="false">
                                </obout:Column>
                                <obout:Column DataField="RequestQty" Width="12%" HeaderAlign="center" HeaderText="Pending For Issue"
                                    Align="right" AllowEdit="false" Wrap="true">
                                    <TemplateSettings TemplateId="GridTemplateRightAlign" />
                                </obout:Column>
                                <obout:Column DataField="IssuedQty" Width="10%" HeaderAlign="right" HeaderText="Issue Qty"
                                    Align="right" AllowEdit="false">
                                    <TemplateSettings TemplateId="PlainEditTemplate" />
                                </obout:Column>
                                <obout:Column DataField="RemaningQty" Width="10%" HeaderAlign="right" HeaderText="Remaining Qty"
                                    Align="right" AllowEdit="false">
                                </obout:Column>
                                <obout:Column DataField="CurrentStockHQ" HeaderText="HQ Stock" AllowEdit="false"
                                    Width="9%" HeaderAlign="center" Align="right" Wrap="true">
                                    <TemplateSettings TemplateId="GridTemplateRightAlign" />
                                </obout:Column>
                              <obout:Column DataField="Installable" HeaderText="Installable" Visible="false"></obout:Column>
                            </Columns>
                            <Templates>
                                <obout:GridTemplate ID="ItemTempRemove" runat="server">
                                    <Template>
                                        <table>
                                            <tr>
                                                <td style="width: 20px; text-align: center;">
                                                    <img id="imgbtnRemove" alt="Remove" src="../App_Themes/Grid/img/Remove16x16.png"
                                                        title="Remove" onclick="removePartFromList('<%# (Container.DataItem["Sequence"].ToString()) %>');"
                                                        style="cursor: pointer;" />
                                                </td>
                                                <td style="width: 35px; text-align: center;">
                                                    <%# Convert.ToInt32(Container.PageRecordIndex) + 1 %>
                                                </td>
                                            </tr>
                                        </table>
                                    </Template>
                                </obout:GridTemplate>
                                <obout:GridTemplate runat="server" ID="PlainEditTemplate">
                                    <Template>
                                        <input type="text" class="excel-textbox" value="<%# Container.Value %>" onfocus="markAsFocused(this)"
                                            onkeydown="AllowDecimal(this,event);" onkeypress="AllowDecimal(this,event);"
                                            onblur="markAsBlured(this, '<%# Grid1.Columns[Container.ColumnIndex].DataField %>', <%# Container.PageRecordIndex %>)" />
                                    </Template>
                                </obout:GridTemplate>
                                <obout:GridTemplate ID="GridTemplateRightAlign" runat="server">
                                    <Template>
                                        <span style="text-align: right; width: 130px; margin-right: 10px;">
                                            <%# Container.Value  %></span>
                                    </Template>
                                </obout:GridTemplate>
                                <%--Add By Suresh--%>
                                <obout:GridTemplate ID="TemplateEdit" runat="server">
                                    <Template>
                                        <asp:ImageButton ID="imgbtndetails" runat="server" ImageUrl="../App_Themes/Blue/img/Edit16.png"                                           
                                            OnClick="imgbtndetails_OnClick" ToolTip='<%# (Container.Value) %>' CausesValidation="false" />
                                    </Template>
                                </obout:GridTemplate>
                            </Templates>
                        </obout:Grid>
                        
                    </td>
                </tr>
            </table>
        <%--Add By Suresh--%>
        <asp:UpdatePanel ID="updatePnl" runat="server">
        <ContentTemplate>       
        
        <asp:Button ID="btnShowEdit" runat="server" Style="display: none;" Text="Button" />
                        <asp:ModalPopupExtender ID="ModelPopUp" runat="server" PopupControlID="PopUP"
                            TargetControlID="btnShowEdit" CancelControlID="btnClose" >
                        </asp:ModalPopupExtender>
                        <asp:HiddenField ID="hdnprodID" runat="server" />
                        <div id="PopUP" runat="server">
                        <asp:Panel ID="PanelProductEdit" runat="server">
                            <table class="gridFrame" id="tblNew" width="100%">
                                <tr>
                                    <td>
                                        <table width="100%">
                                            <tr>
                                                <td style="text-align: left;">
                                                    <a class="headerText">New Product</a>
                                                </td>
                                                <td style="text-align: right;">
                                                    <asp:Button ID="btnSubmit" runat="server" Text="Save" Style="cursor: pointer;" OnClick="btnSubmit_OnClick" />
                                                    <asp:Button ID="btnClose" runat="server" Text="Cancel" Style="cursor: pointer;" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <table class="tableForm" style="background-color: White;">
                                            <tr>
                                                <td style="color: Black; text-align: right;">
                                                    Product Type :
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlProductType" runat="server" DataTextField="ProductType"
                                                        DataValueField="ID" Width="200px" />
                                                </td>
                                                <td style="color: Black; text-align: right;">
                                                    Category :
                                                </td>                                                
                                                <td>
                                                    <asp:DropDownList ID="ddlCategory" runat="server" DataTextField="ProductCategory"
                                                        onchange="printProductSubCategory();" ClientIDMode="Static" DataValueField="ID"
                                                        Width="200px">
                                                    </asp:DropDownList>
                                                </td>
                                                <td style="color: Black; text-align: right;">
                                                    Sub Category :
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlSubCategory" runat="server" ClientIDMode="Static" Width="200px"
                                                        DataTextField="ProductSubCategory" DataValueField="ID">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="color: Black; text-align: right;">
                                                    Product Code :
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtProductCode" runat="server" MaxLength="50" Width="194px"></asp:TextBox>
                                                </td>                                           
                                                <td style="color: Black; text-align: right;">
                                                    Product Name :
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtProductName" runat="server" MaxLength="100" Width="194px"></asp:TextBox>
                                                </td>
                                                <td style="color: Black; text-align: right;">
                                                    UOM :
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlUOM" runat="server" DataTextField="Name" DataValueField="ID"
                                                        Width="200px">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                               <td style="color: Black; text-align: right;">
                                                  Description :
                                               </td>
                                               <td>
                                                   <asp:TextBox ID="txtPrdDesc" runat="server" MaxLength="100" Width="194px"></asp:TextBox>
                                               </td>
                                               <td style="color: Black; text-align: right;">
                                                  Price : 
                                               </td>
                                               <td>
                                                 <asp:TextBox ID="txtPrincipalPrice" runat="server" MaxLength="15" Width="194px" Style="text-align: right;" 
                                                    onkeydown="return AllowDecimal(this,event);" onkeypress="return AllowDecimal(this,event);"></asp:TextBox>
                                               </td>                                              
                                               <td></td>
                                               <td></td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        </div>
                        </ContentTemplate>
        </asp:UpdatePanel>
        
        <%--Add By Suresh--%>
        <div class="divDetailExpandPopUpOff" id="divPendingList" style="width: 800px;">
            <div class="popupClose" onclick="ClosePendingListPopup()">
            </div>
            <center>
                <div class="divHead">
                    <table style="width: 100%;">
                        <tr>
                            <td>
                                <h4>
                                    Pending for Issue against Request No.
                                    <asp:Label runat="server" ID="lblRequestNo3"></asp:Label>
                                </h4>
                            </td>
                            <td style="text-align: right;">
                                <input type="button" id="btnSubmitPendingIssue" value="Submit" onclick="selectedRec()" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="divDetailExpand" id="div3" style="max-height: 450px; overflow: visible;">
                    <obout:Grid ID="Grid2" runat="server" CallbackMode="true" Serialize="true" AllowColumnReordering="true"
                        AllowColumnResizing="true" AutoGenerateColumns="false" AllowPaging="false" ShowLoadingMessage="true"
                        AllowSorting="false" AllowManualPaging="false" AllowRecordSelection="true" ShowFooter="false"
                        Width="100%" PageSize="-1" OnRebind="Grid2_OnRebind" AllowMultiRecordSelection="true"
                        AllowPageSizeSelection="false">
                        <ClientSideEvents ExposeSender="true" />
                        <Columns>
                            <obout:CheckBoxSelectColumn ShowHeaderCheckBox="true" ControlType="Standard" Align="center"
                                HeaderAlign="center" Width="5%" AllowEdit="false">
                            </obout:CheckBoxSelectColumn>
                            <obout:Column DataField="Sequence" HeaderText="Sr.No." AllowEdit="false" Width="7%"
                                Align="left" HeaderAlign="left">
                            </obout:Column>
                            <obout:Column DataField="PRD_ID" Visible="false" AllowEdit="false" Width="0%">
                            </obout:Column>
                            <obout:Column DataField="Prod_Code" HeaderText="Code" AllowEdit="false" Width="15%"
                                HeaderAlign="left">
                            </obout:Column>
                            <obout:Column DataField="Prod_Name" HeaderText="Part Name" AllowEdit="false" Width="20%"
                                HeaderAlign="left">
                            </obout:Column>
                            <obout:Column DataField="Prod_Description" HeaderText="Description" AllowEdit="false"
                                Width="15%" HeaderAlign="left">
                            </obout:Column>
                            <obout:Column DataField="UOM" HeaderText="UOM" AllowEdit="false" Width="10%" HeaderAlign="left">
                            </obout:Column>
                            <obout:Column DataField="UOMID" AllowEdit="false" Width="0%" Visible="false">
                            </obout:Column>
                            <obout:Column DataField="RequestQty" Width="13%" HeaderAlign="center" HeaderText="Pending For Issue"
                                Align="center" AllowEdit="false" Wrap="true">
                            </obout:Column>
                            <obout:Column DataField="CurrentStockHQ" HeaderText="HQ Stock" AllowEdit="false"
                                Width="13%" HeaderAlign="center" Align="center" Wrap="true">
                                <%--<TemplateSettings TemplateId="GridTemplateRightAlign" />--%>
                            </obout:Column>
                        </Columns>
                    </obout:Grid>
                </div>
            </center>
        </div>
        <asp:HiddenField runat="server" ID="hdnPendingSelectedRec" ClientIDMode="Static" />
    </center>
    <%--Issue Part List [js & css]--%>
    <script type="text/javascript">
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
        function markAsFocused(textbox) {
            textbox.className = 'excel-textbox-focused';
            textbox.select();
        }
        var RowIndex = 0;
        function markAsBlured(textbox, dataField, rowIndex1) {
            textbox.className = 'excel-textbox';
            RowIndex = rowIndex1;
            var txtvalue = textbox.value;
            if (txtvalue == "") txtvalue = 0;
            textbox.value = parseFloat(txtvalue).toFixed(2);
            // if (parseFloat(Grid1.Rows[RowIndex].Cells[dataField].Value) != parseFloat(textbox.value)) {

            Grid1.Rows[RowIndex].Cells[dataField].Value = textbox.value;
            //CurrentStockHQ
            var HQCurrentStock = parseFloat(Grid1.Rows[RowIndex].Cells['CurrentStockHQ'].Value).toFixed(2);
            var RequestQty = parseFloat(Grid1.Rows[RowIndex].Cells['RequestQty'].Value).toFixed(2);
            var IssueQty = parseFloat(textbox.value).toFixed(2);

            //                if((IssueQty>RequestQty) || (IssueQty>HQCurrentStock)) {                       
            //                    showAlert("Issue Quantity should not be greater than HQ Current Stock & Pending Issue Qty", "error", '#');
            //                    if (RequestQty > HQCurrentStock) {
            //                        textbox.value = parseFloat(HQCurrentStock).toFixed(2);
            //                    }
            //                    else {
            //                        textbox.value = parseFloat(RequestQty).toFixed(2);
            //                    }
            //                }

            //add by suresh
            if ((parseFloat(textbox.value) > parseFloat(RequestQty)) || (parseFloat(textbox.value) > parseFloat(HQCurrentStock))) {
                showAlert("Issue Quantity should not be greater than HQ Current Stock & Pending Issue Qty", "error", '#');
                if (parseFloat(RequestQty) > parseFloat(HQCurrentStock)) {
                    textbox.value = parseFloat(HQCurrentStock).toFixed(2);
                }
                else {
                    textbox.value = parseFloat(RequestQty).toFixed(2);
                }
            }

            Grid1.Rows[RowIndex].Cells['RemaningQty'].Value = (parseFloat(RequestQty) - parseFloat(textbox.value)).toFixed(2);

            Grid1.Rows[RowIndex].Cells['CurrentStockHQ'].Value = (parseFloat(HQCurrentStock) - parseFloat(textbox.value)).toFixed(2);

            var body = Grid1.GridBodyContainer.firstChild.firstChild.childNodes[1];
            var cell1 = body.childNodes[RowIndex].childNodes[8];
            cell1.firstChild.lastChild.innerHTML = parseFloat(Grid1.Rows[RowIndex].Cells['RemaningQty'].Value).toFixed(2);
            PageMethods.WMUpdateIssueQty(getOrderObject(), null, null);
            //add by suresh
            var cell2 = body.childNodes[RowIndex].childNodes[9];
            cell2.firstChild.lastChild.innerHTML = parseFloat(Grid1.Rows[RowIndex].Cells['CurrentStockHQ'].Value).toFixed(2);
            PageMethods.WMUpdateHQStock(getOrderObject(), null, null);
            // }
        }

        function getOrderObject() {
            var order = new Object();
            order.Sequence = parseInt(Grid1.Rows[RowIndex].Cells["Sequence"].Value);
            order.IssuedQty = parseFloat(Grid1.Rows[RowIndex].Cells["IssuedQty"].Value).toFixed(2);
            return order;
        }
        //add by suresh
        //        function getHQObject() {
        //            var hq = new Object();
        //            hq.Sequence = parseInt(Grid1.Rows[RowIndex].Cells["Sequence"].Value);
        //            order.CurrentStockHQ = parseFloat(Grid1.Rows[RowIndex].Cells["CurrentStockHQ"].Value).toFixed(2);
        //            return order;
        //        }

        function removePartFromList(sequence) {
            /*Remove Part from list*/

            PageMethods.WMRemovePartFromIssue(sequence, removePartFromListOnSussess, null);
        }

        function removePartFromListOnSussess() { document.getElementById('hdnPendingSelectedRec').value = ""; Grid1.refresh(); }
        /*End Request Part List*/
    </script>
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
    <%--End Issue Part List --%>
    <%--Page OnLoad check--%>
    <script type="text/javascript">
        jsCheckIssueHistory();
        function jsCheckIssueHistory() {
            if (getParameterByName("invoker") != null) {
                if (getParameterByName("invoker") == "Request") {
                    LoadingOn(true);
                    PageMethods.WMGetIssueSummaryByRequestID(jsCheckIssueHistoryOnSuccess, null)
                }
                else if (getParameterByName("invoker") == "Issue") {
                    //LoadingOn(true);
                    divIssueHistory.className = "divDetailCollapse";
                    linkIssueHistory.innerHTML = "Expand";
                    divIssueDetail.style.display = "";
                    divIssueHead.style.display = "";
                }
            }
        }
        function jsCheckIssueHistoryOnSuccess(result) {
            if (parseInt(result) > 0) {
                IssueHistoryOn();
            }
            else {
                IssueHistoryOff();
            }
            LoadingOff();
        }
        function IssueHistoryOn() {
            document.getElementById("iframePORIssue").src = "../PowerOnRent/GridIssueSummary.aspx?FillBy=RequestID";
            divHeadIssueHistory.style.display = "";
            divIssueHistory.style.display = "";
            divIssueDetail.style.display = "none";
            divIssueHead.style.display = "none";
        }

        function IssueHistoryOff() {
            document.getElementById("iframePORIssue").src = "#";
            divHeadIssueHistory.style.display = "none";
            divIssueHistory.style.display = "none";
            divIssueDetail.style.display = "";
            divIssueHead.style.display = "";
        }

        function IssueOpenEntryForm(invoker, state, issueID) {
            if (state != "gray") {
                if (state == "red") {
                    state = "Edit";
                }
                else if (state != "red") {
                    state = "View";
                }
                LoadingOn(true);
                PageMethods.WMSetSessionIssue(invoker, issueID, state, IssueOpenEntryFormOnSuccess, null);
            }
            else {
                showAlert("Not Applicable", '', '#');
            }
        }

    </script>
    <%--End Page OnLoad check--%>
    <%--Main Scripts--%>
    <script type="text/javascript">

        var hdnSiteID = document.getElementById("<%= hdnSiteID.ClientID %>");
        var ddlStatus = document.getElementById("<%= ddlStatus.ClientID %>");
        var lblIssueNo = document.getElementById("<%= lblIssueNo.ClientID %>");

        var ddlIssuedBy = document.getElementById("<%= ddlIssuedBy.ClientID %>");
        var txtAirwayBill = document.getElementById("<%= txtAirwayBill.ClientID %>");
        var txtShippingType = document.getElementById("<%= txtShippingType.ClientID %>");
        var txtTransporterName = document.getElementById("<%= txtTransporterName.ClientID %>");
        var txtIssueRemark = document.getElementById("<%= txtIssueRemark.ClientID %>");

        var UC_IssueDate = "";
        var UC_ShippingDate = "";
        var UC_ExpDeliveryDate = "";
        function jsGetDates() {
            UC_IssueDate = getDateFromUC("<%= UC_IssueDate.ClientID %>");
            UC_ShippingDate = getDateFromUC("<%= UC_ShippingDate.ClientID %>");
            UC_ExpDeliveryDate = getDateFromUC("<%= UC_ExpDeliveryDate.ClientID %>");
        }

        var SelectedStatus = "0";
        var SelectedIssuedByUserID = "0";
        function IssueOpenEntryFormOnSuccess(obj1) {
            hdnSiteID.value = obj1.SiteID;
            lblIssueNo.innerHTML = obj1.MINH_ID;
            if (obj1.IssueDate != null) {
                getDateTextBoxFromUC("<%= UC_IssueDate.ClientID %>").value = get_ddMMMyyyy(obj1.IssueDate);
            }
            SelectedIssuedByUserID = obj1.IssuedByUserID;
            SelectedStatus = obj1.StatusID;
            txtAirwayBill.value = obj1.AirwayBill;
            txtShippingType.value = obj1.ShippingType;
            if (obj1.ShippingDate != null) { getDateTextBoxFromUC("<%= UC_ShippingDate.ClientID %>").value = get_ddMMMyyyy(obj1.ShippingDate); }
            if (obj1.ExpectedDelDate != null) { getDateTextBoxFromUC("<%= UC_ExpDeliveryDate.ClientID %>").value = get_ddMMMyyyy(obj1.ExpectedDelDate); }
            txtTransporterName.value = obj1.TransporterName;
            txtIssueRemark.value = obj1.Remark;
            jsFillStatusList();

            if (parseInt(obj1.StatusID) > 1) { changemode(true, 'divIssueDetail'); }
            else { changemode(false, 'divIssueDetail'); }
            divIssueHistory.className = "divDetailCollapse";
            linkIssueHistory.innerHTML = "Expand";
            divIssueDetail.style.display = "";
            divIssueHead.style.display = "";

            LoadingOff();
            Grid1.refresh();
            Grid2.refresh();
        }

        /*Toolbar Code*/
        function jsAddNew() { PageMethods.WMpageAddNew(jsAddNewOnSuccess, null); }

        function jsAddNewOnSuccess() {
            ClearMode('divIssueDetail');
            jsFillStatusList('Add');
            IssueHistoryOff();
            //Grid1.refresh(true);
            window.location.reload(true);   //  page.reload(true);
            //page.refresh();
        }

        function jsSaveData() {
            var validate = validateForm('divIssueDetail');
            if (validate == false) {
                showAlert("Fill all mandatory fields", "error", '#');
            }
            else {
                if (ddlStatus.options[ddlStatus.selectedIndex].text == "Issue" && Grid1.Rows.length == 0) {
                    showAlert("Add atleast one part into the Issue Part List", "error", "#");
                }
                else {
                    LoadingOn(true);
                    var obj1 = new Object();
                    jsGetDates();
                    obj1.SiteID = hdnSiteID.value;

                    //obj1.MIN_No = lblIssueNo.innerHTML;
                    obj1.IssueDate = UC_IssueDate;
                    obj1.IssuedByUserID = ddlIssuedBy.options[ddlIssuedBy.selectedIndex].value;
                    obj1.StatusID = ddlStatus.options[ddlStatus.selectedIndex].value;
                    obj1.Title = "";
                    obj1.AirwayBill = txtAirwayBill.value;
                    obj1.SupplierCoNo = "";
                    obj1.ShippingType = txtShippingType.value;
                    obj1.ShippingStatus = "";
                    obj1.ShippingDate = UC_ShippingDate;
                    obj1.ExpectedDelDate = UC_ExpDeliveryDate;
                    obj1.TransporterName = txtTransporterName.value;
                    obj1.Remark = txtIssueRemark.value;
                    if (ddlStatus.selectedIndex == 1) { obj1.IsSubmit = "false"; }
                    else { obj1.IsSubmit = "true"; }
                    PageMethods.WMSaveIssueHead(obj1, jsSaveData_onSuccessed, jsSaveData_onFailed);
                }
            }
        }

        function jsSaveData_onSuccessed(result) {
            if (result == "Some error occurred" || result == "") { alert("Error occurred", "Error", '#'); }
            //            else { showAlert(result, "info", "../PowerOnRent/Default.aspx?invoker=Issue"); }

            //add by suresh
            else { showAlert(result, "info", "../PowerOnRent/Default.aspx?invoker=Request"); }
        }

        function jsSaveData_onFailed() { showAlert("Error occurred", "Error", '#'); }
        /*End Toolbar code*/
    </script>
    <%--End Main Scripts--%>
    <%--Fill Dropdown--%>
    <script type="text/javascript">
        /*Fill DropDown*/
        function jsFillStatusList(state) {
            ddlStatus.options.length = 0;
            ddlLoadingOn(ddlStatus);
            PageMethods.WMFillStatus(jsFillStatusListOnSuccessed, jsFillStatusListOnFailed);
        }
        function jsFillStatusListOnSuccessed(result) {
            var ddlS = ddlStatus;
            for (var i = 0; i < result.length; i++) {
                var option1 = document.createElement("option");

                option1.text = result[i].Status;
                option1.value = result[i].ID;
                try {
                    ddlS.add(option1, null); //Standard 
                } catch (error) {
                    ddlS.add(option1); // IE only
                }
            }

            if (SelectedStatus != "0") ddlStatus.value = SelectedStatus;
            ddlLoadingOff(ddlS);
        }

        function jsFillStatusListOnFailed() {
            ddlLoadingOff(ddlStatus);
        }

        /*End Fill Dropdown*/
    </script>
    <%--End Fill Dropdown--%>
    <%--Open Pending List--%>
    <script type="text/javascript">
        /*Pending List*/
        function openPendingList() {           
            LoadingOn(false);
            divPendingList.className = "divDetailExpandPopUpOn";
            Grid2.refresh();
        }

        function ClosePendingListPopup() {
            divPendingList.className = "divDetailExpandPopUpOff";
            LoadingOff();
        }

        function selectedRec() {
            var hdnPendingSelectedRec = document.getElementById("hdnPendingSelectedRec");
            hdnPendingSelectedRec.value = "";

            if (Grid2.SelectedRecords.length > 0) {
                for (var i = 0; i < Grid2.SelectedRecords.length; i++) {
                    var record = Grid2.SelectedRecords[i];
                    if (hdnPendingSelectedRec.value != "") hdnPendingSelectedRec.value += ',' + record.PRD_ID;
                    if (hdnPendingSelectedRec.value == "") hdnPendingSelectedRec.value = record.PRD_ID;
                }

                ClosePendingListPopup();
                Grid1.refresh();
            }
            if (Grid2.SelectedRecords.length == 0) {
                alert("Select atleast one Part");
            }
        }

        //        Add By Suresh
       
        function printProductSubCategory() {

            PageMethods.PMprint_ProductSubCategory(document.getElementById("ddlCategory").value, onSuccessPMprint_ProductSubCategory, null)
        }


        function onSuccessPMprint_ProductSubCategory(result) {
            var ddlSubCategory = document.getElementById("ddlSubCategory")
            ddlSubCategory.options.length = 0;
            var option0 = document.createElement("option");


            if (result.length > 0) {
                option0.text = '-Select- ' + document.getElementById("ddlSubCategory").innerHTML;
                option0.value = "0"
            }
            else { option0.text = 'N/A'; option0.value = "0" }


            try {
                ddlSubCategory.add(option0, null); //Standard 
            } catch (error) {
                ddlSubCategory.add(option0); // IE only
            }


            for (var i = 0; i < result.length; i++) {
                var option1 = document.createElement("option");

                option1.text = result[i].ProductSubCategory;
                option1.value = result[i].ID;

                try {
                    ddlSubCategory.add(option1, null); //Standard 
                } catch (error) {
                    ddlSubCategory.add(option1); // IE only
                }
            }
            if (ddlSubCategory != "") {
                if (ddlSubCategory.length > 0) { ddlSubCategory.value = ddlSubCategory; ddlSubCategory = ""; }
            }
            LoadingOff();
        }

        function AddCorrespondance() {
            window.open('../PowerOnRent/Correspondance.aspx', null, 'height=550px, width=825px,status= 0, resizable= 1, scrollbars=1, toolbar=0,location=center,menubar=0, screenX=300; screenY=50');
        }
    </script>
    <%--End Open Pending List--%>
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
        }
        
        .divDetailExpandPopUpOff
        {
            display: none;
        }
        .divDetailExpandPopUpOn
        {
            border: solid 3px gray;
            width: 90%;
            max-height: 500px;
            padding: 10px;
            background-color: #FFFFFF;
            margin-top: 50px;
            top: 5%;
            left: 3%;
            position: absolute;
            z-index: 99999;
        }
        
        .divDetailCollapse
        {
            display: none;
        }
        
        .popupClose
        {
            background: url(../App_Themes/Blue/img/icon_close.png) no-repeat;
            height: 32px;
            width: 32px;
            float: right;
            margin-top: -30px;
            margin-right: -25px;
        }
        .popupClose:hover
        {
            cursor: pointer;
        }
        
        /*End POR Collapsable Div*/
    </style>
    <%--<script type="text/javascript" language="javascript">
        function fn() {
            WebForm_DoPostBackWithOptions(new WebForm_PostBackOptions('btnSubmit_OnClick', '', true, '', '', false, false));
        }
  </script> --%>  
</asp:Content>

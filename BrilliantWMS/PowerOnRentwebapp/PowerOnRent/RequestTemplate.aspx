<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/CRM.Master" AutoEventWireup="true"
    CodeBehind="RequestTemplate.aspx.cs" Inherits="BrilliantWMS.PowerOnRent.RequestTemplate"
    EnableEventValidation="false" %>

<%@ Register Assembly="obout_Flyout2_NET" Namespace="OboutInc.Flyout2" TagPrefix="obout" %>
<%@ Register Assembly="obout_Interface" Namespace="Obout.Interface" TagPrefix="obout" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="obout_Grid_NET" Namespace="Obout.Grid" TagPrefix="obout" %>
<%@ Register Assembly="Obout.Ajax.UI" Namespace="Obout.Ajax.UI.HTMLEditor" TagPrefix="obout" %>
<%@ Register Assembly="obout_ComboBox" Namespace="Obout.ComboBox" TagPrefix="obout" %>
<%@ Register Src="../CommonControls/UCFormHeader.ascx" TagName="UCFormHeader" TagPrefix="uc2" %>
<%--<%@ Register Src="../CommonControls/Toolbar.ascx" TagName="Toolbar" TagPrefix="uc2" %>--%>
<%@ Register Src="../CommonControls/UCToolbar.ascx" TagName="UCToolbar" TagPrefix="uc2" %>
<%@ Register Src="../CommonControls/UC_Date.ascx" TagName="UC_Date" TagPrefix="ucd55" %>
<%@ Register Src="../Product/UCProductSearch.ascx" TagName="UCProductSearch" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHolder_FormHeader" runat="server">
    <%-- <uc2:Toolbar ID="Toolbar1" runat="server" />--%>
    <uc2:UCToolbar ID="UCToolbar1" runat="server" />
    <uc2:UCFormHeader ID="UCFormHeader1" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPHolder_Form" runat="server">
    <script>
        function openBomDetails(bomProdId, isgroup) {
            if (isgroup == "Yes") {
                var myBomWin = window.open('bomDetails.aspx?id=' + bomProdId, null, 'height=450px, width=825px,status= 0, resizable= 1, scrollbars=1, toolbar=0,location=center,menubar=0, screenX=300; screenY=50'); //'directories=no,titlebar=no,toolbar=no,location=no,status=no,menubar=no,scrollbars=no,resizable=no,width=900,height=650');                       
            }
        }
    </script>
    <style>
        .bomImgStyleYes img {
            opacity: 1;
        }

        .bomImgStyleNo img {
            opacity: 0.2;
        }

        .bomImgStyleNo a {
            cursor: default !important;
        }

        .bomImgStyleYes a {
            cursor: pointer;
        }
    </style>
    <asp:ValidationSummary ID="validationsummary_RequestTemplate" runat="server" ShowMessageBox="true"
        ShowSummary="false" DisplayMode="bulletlist" ValidationGroup="Save" />
    <%-- <asp:UpdatePanel ID="UpdatePanelTabPanelProductList" runat="server">
        <ContentTemplate>--%>
    <asp:HiddenField ID="hdnTemplateID" runat="server" />
    <asp:TabContainer runat="server" ID="tabContainerReqTemplate">
        <asp:TabPanel ID="tbTemplateLst" runat="server" HeaderText="Request Template List">
            <ContentTemplate>
                <%-- <asp:UpdateProgress ID="UpdateGirdProductProcess" runat="server" AssociatedUpdatePanelID="Up_PnlGirdTemplate">
                            <ProgressTemplate>
                                <center>
                                    <div class="modal">
                                        <img src="../App_Themes/Blue/img/ajax-loader.gif" style="top: 50%;" />
                                    </div>
                                </center>
                            </ProgressTemplate>
                        </asp:UpdateProgress>--%>
                <%-- <asp:UpdatePanel ID="Up_PnlGirdTemplate" runat="server">
                            <ContentTemplate>--%>
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
                                        <asp:Label Style="color: white; font-size: 15px; font-weight: bold;" ID="lblreqtemplist"
                                            runat="server" Text="Request Template List"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <obout:Grid ID="GVRequest" runat="server" AllowAddingRecords="false" AutoGenerateColumns="false" AllowFiltering="true"
                                AllowGrouping="true" CallbackMode="true" AllowRecordSelection="true"
                                AllowMultiRecordSelection="false" AllowColumnResizing="true" AllowColumnReordering="true"
                                Width="100%" PageSize="10" OnRebind="GVRequest_OnRebind">
                                <ScrollingSettings ScrollHeight="250" />
                                <Columns>
                                    <obout:Column HeaderText="Edit" DataField="ID" Width="5%" Align="center" HeaderAlign="center">
                                        <TemplateSettings TemplateId="TemplateEdit" />
                                    </obout:Column>
                                    <obout:Column DataField="TemplateTitle" HeaderText="Template Title" HeaderAlign="left"
                                        Align="left" Width="8%">
                                    </obout:Column>
                                    <obout:Column DataField="CustomerName" HeaderText="Created By" HeaderAlign="left"
                                        Align="left" Width="8%">
                                    </obout:Column>
                                    <obout:Column DataField="AccessType" HeaderText="Access Type" HeaderAlign="left"
                                        Align="left" Width="8%">
                                    </obout:Column>
                                    <obout:Column DataField="Territory" HeaderText="Department" HeaderAlign="left" Align="left"
                                        Width="8%">
                                    </obout:Column>
                                    <%-- <obout:Column DataField="CustomerName" HeaderText="Customer Name" HeaderAlign="left"
                                        Align="left" Width="8%">
                                    </obout:Column>--%>
                                    <obout:Column DataField="CreatedDate" HeaderText="Created Date" HeaderAlign="center"
                                        DataFormatString="{0:dd-MMM-yyyy}" Align="center" Width="5%">
                                    </obout:Column>
                                    <obout:Column DataField="Active" HeaderText="Active" HeaderAlign="center" Align="center"
                                        Width="8%">
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
                <%-- </center>--%>
                <%-- </div>--%>
                <%-- </ContentTemplate>
                        </asp:UpdatePanel>--%>
            </ContentTemplate>
        </asp:TabPanel>
        <asp:TabPanel ID="tbTemplateDetail" runat="server" HeaderText="Template Detail">
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
                                        <req> <asp:Label ID="lblTitle" runat="server" Text="Title"></asp:Label></req>
                                        :
                                    </td>
                                    <td colspan="5" align="left" style="text-align: left;">
                                        <asp:TextBox ID="txtTitle" runat="server" Width="99%" MaxLength="100"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RFVTitle" runat="server" ControlToValidate="txtTitle"
                                            ErrorMessage="Enter Title" ValidationGroup="Save" Display="None"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <%-- <td>
                                                <asp:Label ID="lblDate" runat="server" Text="Date"></asp:Label>
                                                :
                                            </td>
                                            <td style="text-align: left;">
                                                <ucd55:UC_Date ID="Date" runat="server" />
                                            </td>--%>
                                    <%--<td>
                                                <asp:Label ID="lblTemplateCreatedby" runat="server" Text="Template Created By :"></asp:Label>
                                            </td>
                                            <td style="text-align: left;">
                                                <asp:DropDownList ID="ddlCreatedBy" runat="server" Width="182px">
                                                    <asp:ListItem Selected="True" Text="-Select-" Value="0"></asp:ListItem>
                                                    <asp:ListItem Text="Suresh Kumbhar" Value="1"></asp:ListItem>
                                                    <asp:ListItem Text="Vishal Dhure" Value="2"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>--%>
                                    <td>
                                        <req><asp:Label ID="lblAccessType" runat="server" Text="Access Type"></asp:Label></req>
                                        :
                                    </td>
                                    <td style="text-align: left;">
                                        <asp:DropDownList ID="ddlAccessType" runat="server" Width="182px">
                                            <asp:ListItem Selected="True" Text="-Select-" Value="0"></asp:ListItem>
                                            <asp:ListItem Text="Public" Value="Public"></asp:ListItem>
                                            <asp:ListItem Text="Private" Value="Private"></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RFVAccessType" runat="server" ControlToValidate="ddlAccessType"
                                            ErrorMessage="Select Access Type" InitialValue="0" ValidationGroup="Save" Display="None"></asp:RequiredFieldValidator>
                                    </td>
                                    <td style="text-align: right;">
                                        <req><asp:Label ID="lblCustomerName" runat="server" Text="Customer Name"></asp:Label></req>
                                        :
                                    </td>
                                    <td style="text-align: left;">
                                        <asp:DropDownList runat="server" ID="ddlCompany" Width="182px" DataTextField="Name"
                                            DataValueField="ID" ClientIDMode="Static" onchange="GetDept(this);">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RFVcompany" runat="server" ControlToValidate="ddlCompany"
                                            InitialValue="0" ErrorMessage="Select Customer" ValidationGroup="Save" Display="None"></asp:RequiredFieldValidator>
                                    </td>
                                    <td style="text-align: right;">
                                        <req><asp:Label ID="lblDepartment" runat="server" Text="Department"></asp:Label></req>
                                        :
                                    </td>
                                    <td style="text-align: left;">
                                        <asp:DropDownList runat="server" ID="ddlSites" Width="182px" DataTextField="Territory"
                                            DataValueField="ID" ClientIDMode="Static" onchange="GetDeptID(this);CheckParts();">
                                        </asp:DropDownList>
                                        <%--GetContact1(this);GetAddress(this);--%>
                                        <asp:RequiredFieldValidator ID="RFVddlSites" runat="server" ControlToValidate="ddlSites"
                                            ErrorMessage="Select Department" ValidationGroup="Save" Display="None"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <%-- <tr style ="visibility:hidden;">
                                    <td style="text-align: right;">
                                        <req><asp:Label ID="lblAddress" runat="server" Text="Address"></asp:Label></req>
                                        :
                                    </td>
                                    <td style="text-align: left;">
                                        <asp:DropDownList runat="server" ID="ddlAddress" Width="182px" DataTextField="AddressLine1"
                                            DataValueField="ID" onchange="PrintAddress();">
                                        </asp:DropDownList>
                                       
                                    </td>
                                    <td style="text-align: right;">
                                        <req>
                                                <asp:Label ID="lblContact" runat="server" Text="Contact1"></asp:Label></req>
                                        :
                                    </td>
                                    <td style="text-align: left;">
                                        <asp:DropDownList runat="server" ID="ddlContact1" Width="182px" DataTextField="Name"
                                            DataValueField="ID" ClientIDMode="Static" onchange="GetContact2(this);">
                                        </asp:DropDownList>
                                       
                                    </td>
                                    <td style="text-align: right;">
                                        <req><asp:Label ID="lblContact2" runat="server" Text="Contact2"></asp:Label></req>
                                        :
                                    </td>
                                    <td style="text-align: left;">
                                        <asp:DropDownList runat="server" ID="ddlContact2" Width="182px" DataTextField="Name"
                                            DataValueField="ID" onchange="ddl2Selvalue(this);">
                                        </asp:DropDownList>
                                       
                                    </td>
                                </tr>--%>
                                <tr>
                                    <td style="text-align: right;">
                                        <asp:Label ID="lblRemark" runat="server" Text="Remark"></asp:Label>
                                        :
                                    </td>
                                    <td colspan="5" align="left" style="text-align: left;">
                                        <asp:TextBox ID="txtRemark" runat="server" Width="99%" MaxLength="100"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                            <table class="gridFrame" width="80%" id="tblCart">
                                <tr>
                                    <td>
                                        <table style="width: 100%">
                                            <tr>
                                                <td style="text-align: left;">
                                                    <asp:Label ID="lblskulist" CssClass="headerText" runat="server" Text="SKU List"></asp:Label>
                                                </td>
                                                <td style="text-align: right;">
                                                    <uc1:UCProductSearch ID="UCProductSearch1" runat="server" />
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
                                            Width="100%" PageSize="-1" OnRebind="Grid1_OnRebind" OnRowDataBound="Grid1_OnRowDataBound"
                                            RowEditTemplateId="Grid1_RowCommand">
                                            <ClientSideEvents ExposeSender="true" />
                                            <Columns>
                                                <obout:Column DataField="Prod_ID" Visible="false">
                                                </obout:Column>
                                                <obout:Column DataField="Sequence" HeaderText="Sr.No." AllowEdit="false" Width="7%"
                                                    Align="left" HeaderAlign="center">
                                                    <TemplateSettings TemplateId="ItemTempRemove" />
                                                </obout:Column>
                                                <obout:Column DataField="Prod_Code" HeaderText="Code" AllowEdit="false" Width="15%"
                                                    HeaderAlign="left">
                                                </obout:Column>
                                                <obout:Column DataField="Prod_Name" HeaderText="Product Name" AllowEdit="false" Width="15%"
                                                    HeaderAlign="left">
                                                </obout:Column>
                                                <obout:Column DataField="Prod_Description" HeaderText="Description" AllowEdit="false"
                                                    Width="18%" HeaderAlign="left">
                                                </obout:Column>
                                                <obout:Column DataField="GroupSet" HeaderText="GroupSet" AllowEdit="false" Width="0%"
                                                    HeaderAlign="left" Align="center" Visible="false">
                                                </obout:Column>
                                                <obout:Column DataField="moq" HeaderText="MOQ" AllowEdit="false" Width="7%">
                                                    <TemplateSettings TemplateId="PrdMOQ" />
                                                </obout:Column>
                                                <obout:Column DataField="RequestQty" Width="14%" HeaderAlign="center" HeaderText="Request Quantity"
                                                    Align="center" AllowEdit="false">
                                                    <TemplateSettings TemplateId="PlainEditTemplate" />
                                                </obout:Column>
                                                <obout:Column DataField="UOM" HeaderText="UOM" AllowEdit="false" Width="12%" HeaderAlign="left">
                                                    <TemplateSettings TemplateId="UOMEditTemplate" />
                                                </obout:Column>
                                                <obout:Column DataField="UOMID" AllowEdit="false" Width="0%" Visible="false">
                                                </obout:Column>
                                                <obout:Column DataField="OrderQuantity" HeaderText="Order Quantity" AllowEdit="false"
                                                    Width="14%" HeaderAlign="center" Align="center">
                                                    <TemplateSettings TemplateId="GridTemplateTotal" />
                                                </obout:Column>
                                                <obout:Column DataField="Price" HeaderText="Price" Width="10%" HeaderAlign="center" Align="center">
                                                    <TemplateSettings TemplateId="TemplatePrice" />
                                                </obout:Column>
                                                <obout:Column DataField="Total" HeaderText="Total" AllowEdit="false" Width="10%" HeaderAlign="center" Align="center">
                                                    <TemplateSettings TemplateId="PrdPriceTotal" />
                                                </obout:Column>
                                                <obout:Column DataField="colBom" HeaderText="Group Set" AllowEdit="false" Width="10%"
                                                    HeaderAlign="center" Align="right">
                                                    <TemplateSettings TemplateId="GridTemplateBOM" />
                                                </obout:Column>
                                                <%--<obout:Column DataField="RequestQty" Width="10%" HeaderAlign="center" HeaderText="Request Quantity"
                                                    Align="center" AllowEdit="false">
                                                    <TemplateSettings TemplateId="PlainEditTemplate" />
                                                </obout:Column>--%>
                                            </Columns>
                                            <Templates>
                                                <obout:GridTemplate ID="GridTemplateBOM">
                                                    <Template>
                                                        <div align="center" class="bomImgStyle<%# (Container.DataItem["GroupSet"].ToString()) %>">
                                                            <a id="bomURL" href="#" onclick="openBomDetails(<%# (Container.DataItem["Prod_ID"].ToString()) %>,'<%# (Container.DataItem["GroupSet"].ToString()) %>');return false;">
                                                                <asp:Image ImageUrl="~/PowerOnRent/Img/bom.png" ID="BomImg" runat="server" Height="24" /></a>
                                                        </div>
                                                    </Template>
                                                </obout:GridTemplate>
                                                <obout:GridTemplate ID="PrdMOQ">
                                                    <Template>
                                                        <span style="text-align: right; width: 130px; margin-right: 10px;">
                                                            <%# Container.Value  %></span>
                                                    </Template>
                                                </obout:GridTemplate>
                                                <obout:GridTemplate ID="PrdPriceTotal">
                                                    <Template>
                                                        <div class="divrowpriceTotal">
                                                            <asp:Label ID="rowPriceTotal" runat="server">0</asp:Label>
                                                        </div>
                                                    </Template>
                                                </obout:GridTemplate>
                                                <obout:GridTemplate ID="GridTemplateTotal">
                                                    <Template>
                                                        <asp:Label ID="rowQtyTotal" runat="server">1</asp:Label>
                                                    </Template>
                                                </obout:GridTemplate>
                                                <obout:GridTemplate ID="ItemTempRemove">
                                                    <Template>
                                                        <table>
                                                            <tr>
                                                                <td style="width: 20px; text-align: center;">
                                                                    <img id="imgbtnRemove" src="../App_Themes/Grid/img/Remove16x16.png" title="Remove"
                                                                        onclick="removePartFromList('<%# (Container.DataItem["Sequence"].ToString()) %>');"
                                                                        style="cursor: pointer;" />
                                                                </td>
                                                                <td style="width: 35px; text-align: right;">
                                                                    <%# Convert.ToInt32(Container.PageRecordIndex) + 1 %>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </Template>
                                                </obout:GridTemplate>
                                                <obout:GridTemplate runat="server" ID="PlainEditTemplate">
                                                    <Template>
                                                        <asp:TextBox ID="txtUsrQty" Width="94%" Style="text-align: right;" runat="server"
                                                            Text="<%# Container.Value %>" onfocus="markAsFocused(this)" onkeydown="AllowDecimal(this,event);"
                                                            onkeypress="AllowDecimal(this,event);" onblur="markAsBlured(this, '<%# Grid1.Columns[Container.ColumnIndex].DataField %>', <%# Container.PageRecordIndex %>);"></asp:TextBox>
                                                    </Template>
                                                </obout:GridTemplate>
                                                <obout:GridTemplate ID="UOMEditTemplate" runat="server">
                                                    <Template>
                                                        <asp:DropDownList ID="ddlUOM" runat="server" Width="100px" Style="text-align: left;">
                                                        </asp:DropDownList>
                                                        <asp:HiddenField ID="hdnMyUOM" Value="" runat="server" />
                                                        <%--onchange="getselectedUOM(this,'<%# Grid1.Columns[Container.ColumnIndex].DataField %>',<%# Container.PageRecordIndex %>)" CssClass="ddlUOM" onchange="return GetSelValue(this);"--%>
                                                    </Template>
                                                </obout:GridTemplate>
                                                <obout:GridTemplate ID="TemplatePrice" runat="server">
                                                    <Template>
                                                        <div class="divTxtPrice">
                                                            <asp:TextBox ID="txtUsrPrice" Width="94%" Style="text-align: right;" runat="server" onkeydown="AllowDecimal(this,event);"
                                                                onkeypress="AllowDecimal(this,event);" Text="<%# Container.Value %>"></asp:TextBox>
                                                        </div>
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
    <%-- </ContentTemplate>
    </asp:UpdatePanel>--%>
    <asp:HiddenField ID="hdnselectedCompany" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnselectedDept" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnselectedCont1" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnselectedCont2" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnSelAddress" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnSelectedQty" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnTmplSelectedRec" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnChngDept" runat="server" />
    <asp:HiddenField ID="hdnSelectedUMO" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnEnteredPrice" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnEnteredQty" runat="server" ClientIDMode="Static" />
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
            overflow: hidden;
            height: 92%;
        }

        .divDetailCollapse {
            display: none;
        }
        /*End POR Collapsable Div*/
    </style>
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
                SelectedTmplRec();
            }
        }

        function SelectedTmplRec() {
            var hdnTmplSelectedRec = document.getElementById("hdnTmplSelectedRec");
            hdnTmplSelectedRec.value = "";

            for (var i = 0; i < GVRequest.PageSelectedRecords.length; i++) {
                var record = GVRequest.PageSelectedRecords[i];
                if (hdnTmplSelectedRec.value == "") hdnTmplSelectedRec.value = record.ID;
            }
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

        function removePartFromList(sequence) {
            /*Remove Part from list*/
            var hdnProductSearchSelectedRec = document.getElementById('hdnProductSearchSelectedRec');
            hdnProductSearchSelectedRec.value = "";
            PageMethods.WMRemovePartFromRequest(sequence, removePartFromListOnSussess, null);
        }
        function removePartFromListOnSussess() { Grid1.refresh(); }

        function GetDept() {
            var ddlCompany = document.getElementById("<%=ddlCompany.ClientID %>");
            var hdnselectedCompany = document.getElementById('hdnselectedCompany');
            hdnselectedCompany.value = ddlCompany.value;
            var Cmpny = ddlCompany.value;
            PageMethods.WMGetDept(Cmpny, OnSuccessCompany, null);
        }
        function OnSuccessCompany(result) {
            ddlSites = document.getElementById("<%=ddlSites.ClientID %>");
            ddlSites.options.length = 0;
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
                ddlSites.add(option0, null);
            }
            catch (Error) {
                ddlSites.add(option0);
            }

            for (var i = 0; i < result.length; i++) {
                var option1 = document.createElement("option");
                option1.text = result[i].Territory;
                option1.value = result[i].ID;
                try {
                    ddlSites.add(option1, null);
                }
                catch (Error) {
                    ddlSites.add(option1);
                }
            }
        }

        function GetDeptID() {
            ddlSites = document.getElementById("<%=ddlSites.ClientID %>");
            var Dept = ddlSites.value;
            var hdnselectedDept = document.getElementById('hdnselectedDept');
            hdnselectedDept.value = ddlSites.value;
            PageMethods.WMGetDepartmentSession(Dept);
        }
        function CheckParts() {
            ddlSites = document.getElementById("<%=ddlSites.ClientID %>");
            var Dept = ddlSites.value;
            var hdnChngDept = document.getElementById("<%=hdnChngDept.ClientID%>");
            if (Grid1.Rows.length == 0) { }
            else {
                showAlert("Order can be processed only for one department per request. Change in department selection will remove the Sku selected.", "Error");
                hdnChngDept.value = "0x00x0";
                Grid1.refresh();
                hdnChngDept.value = "1x1";
            }
        }

        function GetIndex(myDD, myHdnUMO, mySpnTotalQty, usrInputQty, Index, Price, TotPrice, moq) {
            var getMyHdnField = document.getElementById(myHdnUMO);
            if (getMyHdnField != null) {

                var myUMOval = myDD.value;
                // alert(myUMOval);
                var myFilterVal = myUMOval.split(":");

                var myFilteredUMO = myFilterVal[0];
                var myFilteredUnit = myFilterVal[1];

                var hdnSelectedQty = document.getElementById('hdnSelectedQty');
                hdnSelectedQty.value = myFilteredUnit;
                var hdnSelectedUMO = document.getElementById('hdnSelectedUMO');
                hdnSelectedUMO.value = myFilteredUMO;
                var hdnEnteredPrice = document.getElementById('hdnEnteredPrice');
                var EntPrice = hdnEnteredPrice.value;
                var hdnEnteredQty = document.getElementById('hdnEnteredQty');
                var QtyEntered = hdnEnteredQty.value;

                var getUserInputQtyObj = document.getElementById(usrInputQty);
                var getTotalQtyObj = document.getElementById(mySpnTotalQty);

                var numTotalQty = Number(getUserInputQtyObj.value) * Number(myFilteredUnit);
                getTotalQtyObj.innerHTML = numTotalQty;

                getMyHdnField.value = myFilteredUMO;
                if (hdnEnteredPrice.value == "") {
                    var CalPrice = numTotalQty * Price;
                } else { var CalPrice = numTotalQty * EntPrice; }

                var ShowTotPrice = document.getElementById(TotPrice);
                ShowTotPrice.innerHTML = CalPrice;

                var moqv = moq;
                if (moqv > 0) {
                    var rem = numTotalQty % moqv;
                    if (rem > 0) {
                        getUserInputQtyObj.value = "0";
                        getTotalQtyObj.innerHTML = "0";
                        showAlert("Requested Quantity is not in range of MOQ ...!!!", "Error", '#');
                    } else {
                        var order = new Object();
                        order.Sequence = Index + 1;
                        order.RequestQty = numTotalQty;
                        order.UOMID = myFilteredUMO;   // myHdnUMO;  //myFilteredUnit;//  myFilteredUMO;
                        order.Total = CalPrice;
                        PageMethods.WMUpdRequestPart(order, null, null);
                       // PageMethods.WMGetTotal(OnSuccessGetTotal, null);
                    }
                }
            }
        }

        function GetIndexQty(myDD, myHdnQty, myHdnUMO, mySpnTotalQty, usrInputQty, Index, Price, TotPrice, moq) {
            var hdnSelectedQty = document.getElementById('hdnSelectedQty');
            var selUMOQty = hdnSelectedQty.value;
            var hdnSelectedUMO = document.getElementById('hdnSelectedUMO');
            var selUMOID = hdnSelectedUMO.value;
            var hdnEnteredPrice = document.getElementById('hdnEnteredPrice');
            var EntPrice = hdnEnteredPrice.value;
            var hdnEnteredQty = document.getElementById('hdnEnteredQty');
            var eq = hdnEnteredQty.value;
            var enterQty = myDD.value;
            hdnEnteredQty.value = myDD.value;

            var getMyHdnField = myHdnQty;
            var myUMOval = myHdnUMO.value;
            var myFilteredUnit = 0;

            if (hdnSelectedQty.value == "") {
                myFilteredUnit = myHdnUMO;
            } else {
                myFilteredUnit = hdnSelectedQty.value;
            }

            var getUserInputQtyObj = document.getElementById(usrInputQty);
            var getTotalQtyObj = document.getElementById(mySpnTotalQty);

            // var numTotalQty = Number(getUserInputQtyObj.value) * Number(myHdnQty);               
            // getTotalQtyObj.innerHTML = numTotalQty;
            //getMyHdnField.value = myFilteredUnit; // myFilteredUMO;
            if (hdnEnteredQty.value == "") {
                var numTotalQty = Number(getUserInputQtyObj.value) * Number(myHdnQty); //myHdnUMO       
                getTotalQtyObj.innerHTML = numTotalQty;
                getMyHdnField.value = myFilteredUnit;  // myFilteredUMO;
            } else {
                if (selUMOQty == "") {
                    var numTotalQty = hdnEnteredQty.value * Number(myHdnQty);
                    getTotalQtyObj.innerHTML = numTotalQty;
                }
                else {
                    var numTotalQty = hdnEnteredQty.value * selUMOQty;
                    getTotalQtyObj.innerHTML = numTotalQty;
                }
            }

            if (hdnEnteredPrice.value == "") {
                var CalPrice = numTotalQty * Price;
                var ShowTotPrice = document.getElementById(TotPrice);
                ShowTotPrice.innerHTML = CalPrice;
            } else {
                var CalPrice = numTotalQty * hdnEnteredPrice.value;
                var ShowTotPrice = document.getElementById(TotPrice);
                ShowTotPrice.innerHTML = CalPrice;
            }

            var moqv = moq;
            if (moqv > 0) {
                var rem = numTotalQty % moqv;
                if (rem > 0) {
                    getUserInputQtyObj.value = "0";
                    getTotalQtyObj.innerHTML = "0";
                    showAlert("Requested Quantity is not in range of MOQ ...!!!", "Error", '#');
                } else {
                    var order = new Object();
                    order.Sequence = Index + 1;
                    order.RequestQty = numTotalQty;
                    if (selUMOID == "") {
                        order.UOMID = myHdnUMO;
                    } else {
                        order.UOMID = selUMOID; //myHdnUMO;  //myFilteredUnit;//  myFilteredUMO;
                    }
                    order.Total = CalPrice;
                    PageMethods.WMUpdRequestPart(order, null, null);
                    //PageMethods.WMGetTotal(OnSuccessGetTotal, null);
                }
            }
        }
        function OnSuccessGetTotal(result) {
          
            PageMethods.WMGetTotalQty(OnsuccessTotalQty, null);
        }
        function OnsuccessTotalQty(result) {
           
        }
        function GetChangedPrice(myTxt, myHdnQty, myHdnUMO, mySpnTotalQty, usrInputQty, Index, TotPrice, ProdID) {
            var ChangedPrice = myTxt.value;
            var hdnEnteredPrice = document.getElementById('hdnEnteredPrice');
            var CP = hdnEnteredPrice.value;
            hdnEnteredPrice.value = myTxt.value;
            var hdnSelectedQty = document.getElementById('hdnSelectedQty');
            var SelQty = hdnSelectedQty.value;
            var hdnSelectedUMO = document.getElementById('hdnSelectedUMO');
            var selUMOID = hdnSelectedUMO.value;
            var hdnEnteredQty = document.getElementById('hdnEnteredQty');
            var QtyEntered = hdnEnteredQty.value;


            var getMyHdnField = myHdnQty;
            var myUMOval = myHdnUMO.value;
            var myFilteredUnit = 0;
            myFilteredUnit = myHdnQty;

            var getUserInputQtyObj = document.getElementById(usrInputQty);
            var getTotalQtyObj = document.getElementById(mySpnTotalQty);

            if (hdnSelectedQty.value == "") {
                var numTotalQty = Number(getUserInputQtyObj.value) * Number(myHdnQty);
                getTotalQtyObj.innerHTML = numTotalQty;
                getMyHdnField.value = myFilteredUnit;
            } else {
                var numTotalQty = QtyEntered * SelQty;
                getTotalQtyObj.innerHTML = numTotalQty;
            }

            var CalPrice = numTotalQty * ChangedPrice;
            var ShowTotPrice = document.getElementById(TotPrice);
            ShowTotPrice.innerHTML = CalPrice;

            var order = new Object();
            order.Sequence = Index + 1;
            order.RequestQty = numTotalQty;
            if (selUMOID == "") {
                order.UOMID = myHdnUMO;
            } else {
                order.UOMID = selUMOID; //myHdnUMO;  //myFilteredUnit;//  myFilteredUMO;
            }
            order.Total = CalPrice;
            order.Price = ChangedPrice;
            order.IsPriceChange = 1;
            PageMethods.WMUpdRequestPartPrice(order, ProdID, null, null);
           // PageMethods.WMGetTotal(OnSuccessGetTotal, null);
        }
    </script>
</asp:Content>

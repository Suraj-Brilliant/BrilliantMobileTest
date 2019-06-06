<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/CRM.Master" AutoEventWireup="true" CodeBehind="PurchaseOrder.aspx.cs"
    Inherits="BrilliantWMS.WMS.PurchaseOrder" EnableEventValidation="false" Theme="Blue" %>

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
<%@ Register Src="~/CommonControls/UCImport.ascx" TagName="UCImport" TagPrefix="uci" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHolder_FormHeader" runat="server">
    <uc2:Toolbar ID="Toolbar1" runat="server" />
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

        .disableCylender a,
        .disableCylender img {
            display: none !important;
        }

        .active {
            display: none;
        }
    </style>
    <center>
        <span id="imgProcessing" style="display: none; background-color:white;"><img src="../App_Themes/Blue/img/ajax-loader.gif" style="top: 50%;" /> </span>
        <div id="divLoading" style="height: 110%; width: 100%; display: none; top: 0; left: 0;"
            class="modal" runat="server" clientidmode="Static">
            <center>
            </center>
        </div>
        <div class="divHead" id="divRequestHead" runat="server" clientidmode="Static">
            <h4>
                <asp:Label ID="lblrequest" runat="server" Text="Request"></asp:Label>
            </h4>
            <a onclick="javascript:divcollapsOpen('divRequestDetail',this)" id="linkRequest"
                runat="server">Collapse</a>
        </div>
        <div class="divDetailExpand" id="divRequestDetail" runat="server" clientidmode="Static">
            <div id="OrderHead" runat="server" clientidmode="Static">
                <table class="tableForm">
                    <%--<tr>
                        <td colspan="6">
                            <center>
                                <table>
                                    <tr>
                                        <td style="text-align: right;">
                                            <asp:Button ID="btnGenerateFromTemplate" runat="server" Text="Generate From Template" Style="cursor: pointer;" OnClientClick="OpenTelplateList();" />
                                            <asp:HiddenField ID="hdnSelTemplateID" runat="server" ClientIDMode="Static" ViewStateMode="Enabled" />
                                        </td>
                                        <td style="text-align: right;">
                                            <asp:Label ID="lblTemplateTitleNew" runat="server" Text="Template Title"></asp:Label>
                                            :
                                        </td>
                                        <td style="text-align: left;">
                                            <asp:TextBox ID="txtTemplateTitleNew" runat="server" Width="180px">
                                            </asp:TextBox>
                                        </td>
                                        <td style="text-align: right;">
                                            <asp:Label ID="lblAccessTypeNew" runat="server" Text="Access Type"></asp:Label>
                                            :
                                        </td>
                                        <td style="text-align: left;">
                                            <asp:DropDownList ID="ddlAccessTypeNew" runat="server" Width="182px">
                                                <asp:ListItem Selected="True" Text="-Select-" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="Public" Value="Public"></asp:ListItem>
                                                <asp:ListItem Text="Private" Value="Private"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td style="text-align: right;">
                                            <asp:Button ID="btnSaveAsTemplateNew" runat="server" Text="Save As Template" Style="cursor: pointer;" OnClientClick="SubmitTemplate()" />
                                        </td>
                                    </tr>
                                </table>
                            </center>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6" style="text-align: left;">
                            <hr style="border-color: #f5deb3; border-width: 1px;" />
                        </td>
                    </tr>--%>
                    <tr>
                        <td style="text-align: right;">
                            <req><asp:Label ID="lblTitle" runat="server" Text="Title*"></asp:Label></req>
                            :
                        </td>
                        <td colspan="5" style="text-align: left;">
                            <asp:TextBox runat="server" ID="txtTitle" Width="99%" MaxLength="100" AccessKey="1">
                            </asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right;">
                            <asp:Label ID="lblCustomerOrderRefNo" runat="server" Text="Ref. No."></asp:Label>
                            :
                        </td>
                        <td style="text-align: left;">
                            <asp:TextBox runat="server" ID="txtCustOrderRefNo" Width="180px"></asp:TextBox>
                            <%--AccessKey="1"--%>
                        </td>
                        <td style="text-align: right;">
                            <asp:Label ID="lblRequestNumber" runat="server" Text="PO No."></asp:Label>
                            :
                        </td>
                        <td style="text-align: left; font-weight: bold;">
                            <%--<asp:TextBox runat="server" ID="lblRequestNo" Width="176px" AccessKey="1"></asp:TextBox>--%>
                            <asp:Label runat="server" ID="lblRequestNo" Width="176px" AccessKey="1"></asp:Label>
                        </td>
                        <td style="text-align: right;">
                            <asp:Label ID="lblStatus" runat="server" Text="Status"></asp:Label>
                            :
                        </td>
                        <td style="text-align: left;">
                            <asp:DropDownList runat="server" ID="ddlStatus" Width="182px" AccessKey="1" DataTextField="Status"
                                DataValueField="ID">
                            </asp:DropDownList><%--Enabled="false"--%>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right;">
                            <asp:Label ID="lblRequestDate" runat="server" Text="PO Date"></asp:Label>
                            :
                        </td>
                        <td style="text-align: left;" class="disableCylender">
                            <asp:TextBox ID="txtRequestDate" runat="server" Enabled="false" AccessKey="1" Width="82px"></asp:TextBox>
                            <uc1:UC_Date ID="UC_DateRequestDate" runat="server" AccessKey="1" Visible="false" />
                        </td>
                        <%--<td style="text-align: right;">
                        <asp:Label ID="lblPriority" runat="server" Text="Priority"></asp:Label>
                        :
                    </td>
                    <td style="text-align: left;">
                        <asp:DropDownList runat="server" ID="ddlRequestType" Width="182px" onchange="ddlRequestType_onchange(this);"
                            AccessKey="1">
                            <asp:ListItem Text="-Select-" Value="0">
                            </asp:ListItem>
                            <asp:ListItem Text="High" Value="High">
                            </asp:ListItem>
                            <asp:ListItem Text="Medium" Value="Medium">
                            </asp:ListItem>
                            <asp:ListItem Text="Low" Value="Low">
                            </asp:ListItem>
                        </asp:DropDownList>
                    </td>--%>
                        <td style="text-align: right;">
                            <req><asp:Label ID="lblExpDeliveryDate" runat="server" Text="Exp. Delivery Date*"></asp:Label></req>
                            :
                        </td>
                        <td style="text-align: left;">
                            <asp:UpdatePanel ID="UpdExpDDate" runat="server">
                                <ContentTemplate>
                                    <uc1:UC_Date ID="UC_ExpDeliveryDate" runat="server" AccessKey="1" />
                                    <%--OnLoad="UC_ExpDeliveryDate_OnLoad"--%>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td style="text-align: right;">
                            <asp:Label ID="lblRequestedBy" runat="server" Text="Created By"></asp:Label>
                            :
                        </td>
                        <td style="text-align: left;">
                            <asp:DropDownList runat="server" ID="ddlRequestByUserID" Width="182px" AccessKey="1"
                                DataTextField="userName" DataValueField="userID">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right;">
                            <asp:Label ID="lblCustomerName" runat="server" Text="Vendor Name"></asp:Label>
                            :
                        </td>
                        <td style="text-align: left;">
                            <asp:DropDownList runat="server" ID="ddlCompany" Width="182px" DataTextField="Name"
                                DataValueField="ID" ClientIDMode="Static" onchange="GetVendor(this);GetContact1(this);">
                            </asp:DropDownList>
                        </td>
                        <td style="text-align: right;">
                            <asp:Label ID="lblDepartment" runat="server" Text="Warehouse"></asp:Label>
                            :
                        </td>
                        <td style="text-align: left;">

                            <asp:DropDownList runat="server" ID="ddlSites" Width="182px" DataTextField="WarehouseName"
                                DataValueField="ID" ClientIDMode="Static" onchange="GetAddress(this);GetDeptID(this);CheckParts();PaymentMethod();">
                            </asp:DropDownList>
                        </td>
                        <td style="text-align: right;">
                            <req><asp:Label ID="lblContact1" runat="server" Text="Default Contact*"></asp:Label></req>
                            :
                        </td>
                        <td style="text-align: left;">
                            <asp:DropDownList runat="server" ID="ddlContact1" Width="0px" DataTextField="Name"
                                DataValueField="ID" ClientIDMode="Static" onchange="GetContact2(this);" Visible="false">
                                <%--AccessKey="1"--%>
                            </asp:DropDownList>
                            <asp:TextBox ID="txtContact1" runat="server" Width="182px" Enabled="false" AccessKey="1"></asp:TextBox>
                            <img id="imgSearch" src="../App_Themes/Grid/img/search.jpg" title="Search SKU"
                                style="cursor: pointer;" onclick="openContactSearch('0')" />
                        </td>
                    </tr>
                    <tr>
                        <%-- <td style="text-align: right;">
                            <asp:Label ID="lblContact2" runat="server" Text="Alternate Contact"></asp:Label>
                            :
                        </td>
                        <td style="text-align: left;">
                            <asp:DropDownList runat="server" ID="ddlContact2" Width="0px" DataTextField="Name"
                                DataValueField="ID" onchange="ddl2Selvalue(this);" Visible="false">
                            </asp:DropDownList>
                            <asp:TextBox ID="txtContact2" runat="server" Width="178px" Enabled="false"></asp:TextBox>
                            <img id="img1" src="../App_Themes/Grid/img/search.jpg" title="Search SKU"
                                style="cursor: pointer;" onclick="openContactSearch2('0')" />                           
                        </td>
                        <td style="text-align: right;">
                            <asp:Label ID="lblLocationID" runat="server" Text="Location ID"></asp:Label>
                            :
                        </td>
                        <td>
                            <asp:TextBox ID="txtLocation" runat="server" Width="182px" Enabled="false"></asp:TextBox>
                            <img id="img3" src="../App_Themes/Grid/img/search.jpg" title="Search Location" style="cursor: pointer;" onclick="openLocationSearch('0')" />
                        </td>--%>
                        <td style="text-align: right;">
                            <asp:Label ID="lblCustomerAddress" runat="server" Text="Delivery Address"></asp:Label>
                            :
                        </td>
                        <td style="text-align: left;">
                            <asp:DropDownList runat="server" ID="ddlAddress" Width="0px" DataTextField="AddressLine1"
                                DataValueField="ID" onchange="PrintAddress();" Visible="false">
                            </asp:DropDownList>
                            <asp:TextBox ID="txtAddress" runat="server" Width="182px" Enabled="false"></asp:TextBox>
                            <img id="img2" src="../App_Themes/Grid/img/search.jpg" title="Search Address"
                                style="cursor: pointer;" onclick="openAddressSearch('0')" />
                        </td>
                        <td style="text-align: right;">
                            <asp:Label ID="lblAddressLabel" runat="server" Text="Address Details"></asp:Label>
                            :
                        </td>
                        <td colspan="3" style="text-align: left;">
                            <div style="width: 100%; height: 10px;">
                                <asp:Label ID="lblAddress" runat="server"></asp:Label>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right;">
                            <asp:Label ID="lblRemark" runat="server" Text="Remark"></asp:Label>
                            :
                        </td>
                        <td colspan="5" style="text-align: left;">
                            <asp:TextBox runat="server" ID="txtRemark" Width="99%" TextMode="MultiLine" Rows="1">
                            </asp:TextBox>
                            <%--AccessKey="1"--%>
                        </td>
                    </tr>
                    <tr style="visibility:hidden;">
                        <td style="text-align: right;">
                            <asp:Label ID="lblPaymentMethod" runat="server" Text="Payment Method"></asp:Label>
                            :
                        </td>
                        <td style="text-align: left;">
                            <asp:DropDownList ID="ddlPaymentMethod" runat="server" Width="182px" DataTextField="MethodName" onchange="divPMChng(); GetPaymentMethodID(); "
                                DataValueField="PMethodID" ClientIDMode="Static">
                            </asp:DropDownList>
                        </td>
                        <td colspan="4" style="text-align: left;">
                            <div id="dvLst" class="active">
                                <asp:UpdatePanel ID="UpdLst" runat="server">
                                    <ContentTemplate>
                                        <asp:ListView ID="LstStatutoryInfo" runat="server" GroupItemCount="3" Style="margin-bottom: 0px; width: 1000px">                                           
                                            <GroupTemplate>
                                                <tr id="itemPlaceholderContainer" runat="server">
                                                    <td id="itemPlaceholder" runat="server"></td>
                                                </tr>
                                            </GroupTemplate>
                                            <ItemTemplate>
                                                <center>
                                                    <td id="Td1" runat="server" style="">
                                                        <table width="100%">
                                                            <tr>
                                                                <td style="text-align: right;">
                                                                    <div class="lstLbl">
                                                                        <asp:Label ToolTip='<%# Eval("ID") %>' ID="lblname" runat="server" Text='<%# Eval("FieldName") %>' />
                                                                    </div>
                                                                </td>
                                                                <td style="text-align: left;" align="left">
                                                                    <div class="lstText">
                                                                        <asp:TextBox ID="textboxPM" CssClass="tbox" MaxLength="50" runat="server" Width="180px" Text='<%# Bind("StatutoryValue") %>'></asp:TextBox>
                                                                    </div>                                                                                                                                                                     
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </center>
                                            </ItemTemplate>
                                            <LayoutTemplate>

                                                <table id="groupPlaceholderContainer" runat="server" width="100%" cellpadding="0"
                                                    cellspacing="0">                                                    
                                                    <tr id="groupPlaceholder" runat="server">
                                                    </tr>
                                                </table>
                                            </LayoutTemplate>
                                        </asp:ListView>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                            <div id="dvFOC" class="active" style="margin-left: 35px;">
                                <asp:Label ID="lblFOC" runat="server" Text="Charge To" />                                :
                        <asp:DropDownList ID="ddlFOC" runat="server" Width="180px" DataValueField="ID" DataTextField="CenterName" ClientIDMode="Static"></asp:DropDownList>
                            </div>
                        </td>
                    </tr>
                </table>
                <script>
                    $(".disableCylender input").prop('disabled', true);
                    //  $(".disableCylender input").attr('disabled', 'disabled');
                </script>
            </div>
            <div id="OrderDetail" runat="server" clientidmode="Static">
                <table class="gridFrame" width="100%" id="tblCart">
                    <tr>
                        <td>
                            <table style="width: 100%">
                                <tr>
                                    <td style="text-align: left;">
                                        <asp:Label ID="lblrequestpartlist" CssClass="headerText" runat="server" Text="Request Part List"></asp:Label>
                                    </td>
                                    <td style="text-align: right;">
                                        <uc1:UCProductSearch ID="UCProductSearch1" runat="server" />
                                        <asp:Button ID="btnNewPrduct" runat="server" Text="Add New Product" Visible="false" />
                                        <asp:HiddenField ID="hdnprodID" runat="server" />
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
                                Width="100%" PageSize="-1" OnRebind="Grid1_OnRebind" OnRowDataBound="Grid1_OnRowDataBound">
                                <ClientSideEvents ExposeSender="true" OnClientCallback="setCartTotalOnChange" />
                                <Columns>
                                    <obout:Column DataField="Prod_ID" Visible="false">
                                    </obout:Column>
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
                                    <obout:Column DataField="moq" HeaderText="MOQ" AllowEdit="false" Width="7%" HeaderAlign="center" Align="center">
                                        <TemplateSettings TemplateId="PrdMOQ" />
                                    </obout:Column>
                                    <%--5--%>
                                    <obout:Column DataField="GroupSet" HeaderText="GroupSet" AllowEdit="false" Width="0%"
                                        HeaderAlign="center" Align="center" Visible="false">
                                    </obout:Column>
                                    <%--6 new--%>
                                    <%--5--%>
                                    <%--<obout:Column DataField="CurrentStock" HeaderText="Current Stock" AllowEdit="false"
                                        Width="13%" HeaderAlign="center" Align="right">
                                        <TemplateSettings TemplateId="GridTemplateRightAlign" />
                                    </obout:Column>--%>
                                    <%--7 new--%>
                                    <%--10--%>
                                    <%--<obout:Column DataField="ReserveQty" HeaderText="Reserve Qty" AllowEdit="false" Width="11%"
                                        HeaderAlign="center" Align="center">
                                    </obout:Column>--%>
                                    <%--8 new--%>
                                    <obout:Column DataField="RequestQty" Width="14%" HeaderAlign="center" HeaderText="Request Quantity"
                                        Align="center" AllowEdit="false">
                                        <TemplateSettings TemplateId="PlainEditTemplate" />
                                    </obout:Column>
                                    <%--7 new--%>
                                    <%--6--%>
                                    <obout:Column DataField="UOM" HeaderText="UOM" AllowEdit="false" Width="14%" HeaderAlign="center"
                                        Align="center">
                                        <%--8 new--%>
                                        <%--7--%>
                                        <TemplateSettings TemplateId="UOMEditTemplate" />
                                    </obout:Column>
                                    <obout:Column DataField="UOMID" AllowEdit="false" Width="0%" Visible="false">
                                    </obout:Column>
                                    <%--9 new--%>
                                    <%--8--%>
                                    <obout:Column DataField="OrderQuantity" HeaderText="Order Quantity" AllowEdit="false"
                                        Width="14%" HeaderAlign="center" Align="right">
                                        <TemplateSettings TemplateId="GridTemplateTotal" />
                                    </obout:Column>
                                    <%--10 new--%>
                                    <%--9--%>
                                    <obout:Column DataField="Price" HeaderText="Price" AllowEdit="false" Width="10%" HeaderAlign="center" Align="center">
                                        <TemplateSettings TemplateId="TemplatePrice" />
                                    </obout:Column>
                                    <%--11 new--%>
                                    <obout:Column DataField="Total" HeaderText="Total" AllowEdit="false" Width="10%" HeaderAlign="center" Align="center">
                                        <TemplateSettings TemplateId="PrdPriceTotal" />
                                    </obout:Column>
                                    <%--14 new--%>
                                    <obout:Column DataField="colBom" HeaderText="Group Set" AllowEdit="false" Width="10%"
                                        HeaderAlign="center" Align="right">
                                        <TemplateSettings TemplateId="GridTemplateBOM" />
                                    </obout:Column>
                                    <%--15 new--%>

                                    <%--<obout:Column DataField="IsEdit" HeaderText="Edit" AllowEdit="false" HeaderAlign="center" Align="right" Width="10%">
                                        <TemplateSettings TemplateId="ItemEdit" />
                                    </obout:Column>--%>
                                    <obout:Column DataField="IsPriceChange" HeaderText="IsPriceChange" AllowEdit="false" Width="0%" Visible="false"
                                        HeaderAlign="center" Align="center">
                                    </obout:Column>
                                    <obout:Column DataField="TotalTaxAmount" HeaderText="Total Tax Amount" AllowEdit="false" Width="9%" Align="right" Wrap="true"
                                        HeaderAlign="center">
                                        <TemplateSettings TemplateId="HeaderTempTotalTaxAmount" />
                                    </obout:Column>
                                    <%--15--%>
                                    <obout:Column DataField="" ShowHeader="false" AllowEdit="false" Align="right" HeaderAlign="center"
                                        Width="4%">
                                        <TemplateSettings TemplateId="ItemTempTotalTaxAmountPopUp" />
                                    </obout:Column>
                                    <%--16--%>
                                    <obout:Column DataField="AmountAfterTax" HeaderText="Amount After Tax" AllowEdit="false" Width="10%" Align="right" Wrap="true"
                                        HeaderAlign="center">
                                        <TemplateSettings TemplateId="HeaderTempAmountAfterTax" />
                                    </obout:Column>
                                    <%--17--%>
                                </Columns>
                                <Templates>
                                    <obout:GridTemplate ID="GridTemplateBOM">
                                        <Template>
                                            <div align="center" class="bomImgStyle<%# (Container.DataItem["GroupSet"].ToString()) %>">
                                                <a id="bomURL" href="#" onclick="openBomDetails(<%# (Container.DataItem["Prod_ID"].ToString()) %>,'<%# (Container.DataItem["GroupSet"].ToString()) %>');return false;">
                                                    <asp:Image ImageUrl="~/PowerOnRent/Img/bom.png" ID="BomImg" runat="server" Height="24" ToolTip='<%# (Container.Value) %>' /></a>
                                            </div>
                                        </Template>
                                    </obout:GridTemplate>
                                    <obout:GridTemplate ID="ItemEdit">
                                        <Template>
                                            <div id="dvItemEdit" align="center">
                                                <a id="EditPrdDetail" onclick="openEditProduct(<%# (Container.DataItem["Sequence"].ToString()) %>);return false;">
                                                    <asp:Image ID="imgBtnEdit" ImageUrl="../App_Themes/Blue/img/Edit16.png" runat="server" CausesValidation="false" /></a>
                                            </div>
                                        </Template>
                                    </obout:GridTemplate>
                                    <obout:GridTemplate ID="GridTemplateTotal">
                                        <Template>
                                            <div class="divrowQtyTotal">
                                                <asp:Label ID="rowQtyTotal" runat="server">1</asp:Label>
                                            </div>
                                        </Template>
                                    </obout:GridTemplate>
                                    <obout:GridTemplate ID="PrdPriceTotal">
                                        <Template>
                                            <div class="divrowpriceTotal">
                                                <asp:Label ID="rowPriceTotal" runat="server" Text="<%# Container.Value %>"></asp:Label>
                                            </div>
                                        </Template>
                                    </obout:GridTemplate>
                                    <obout:GridTemplate ID="PrdMOQ">
                                        <Template>
                                            <span style="text-align: right; width: 130px; margin-right: 10px;">
                                                <%# Container.Value  %></span>
                                        </Template>
                                    </obout:GridTemplate>
                                    <obout:GridTemplate ID="HeaderTempRequiredQuantity">
                                        <Template>
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
                                                <%-- <input type="text" class="excel-textbox" value="<%# Container.Value %>" onfocus="markAsFocused(this)" size="10"
                                                    onkeydown="AllowDecimal(this,event);" onkeypress="AllowDecimal(this,event);"
                                                    onblur="markAsBlured(this, '<%# Grid1.Columns[Container.ColumnIndex].DataField %>', <%# Container.PageRecordIndex %>)" />--%>
                                            </div>
                                        </Template>
                                    </obout:GridTemplate>
                                    <obout:GridTemplate ID="GridTemplateRightAlign">
                                        <Template>
                                            <span style="text-align: right; width: 130px; margin-right: 10px;">
                                                <%# Container.Value  %></span>
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
                                                <input type="text" class="excel-textbox" value="<%# Container.Value %>" onfocus="markAsFocused(this)" size="0" disabled="disabled"
                                                    onkeydown="AllowDecimal(this,event);" onkeypress="AllowDecimal(this,event);"
                                                    onblur="markAsBlured(this, '<%# Grid1.Columns[Container.ColumnIndex].DataField %>', <%# Container.PageRecordIndex %>)" />
                                            </div>
                                        </Template>
                                    </obout:GridTemplate>
                                    <obout:GridTemplate ID="HeaderTempTotalTaxAmount">
                                        <Template>
                                           <div class="divTotaltaxAmount">
                                                <asp:Label ID="rowTotalTax" runat="server" Text="<%# Container.Value %>">0</asp:Label>
                                            </div>                                           
                                        </Template>
                                    </obout:GridTemplate>
                                    <obout:GridTemplate ID="ItemTempTotalTaxAmountPopUp">
                                        <Template>
                                            <img id="btnApplyTax" src="../App_Themes/Blue/img/TaxCalculation16.png" title="<%# (Container.PageRecordIndex) %>"
                                                onclick="taxFlyoutOnOpen(this,'<%# (Container.DataItem["Sequence"].ToString()) %>');"
                                                style="cursor: pointer;" />
                                        </Template>
                                    </obout:GridTemplate>
                                    <obout:GridTemplate ID="HeaderTempAmountAfterTax">
                                        <Template>
                                            <div class="divrowpriceTaxTotal">
                                                <asp:Label ID="rowPriceTaxTotal" runat="server" Text="<%# Container.Value %>">0</asp:Label>
                                            </div>
                                        </Template>
                                    </obout:GridTemplate>
                                </Templates>
                            </obout:Grid>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right; background-color: White;">                            
                            <table style="float: right; text-align: right; border: none;" class="tableForm">
                                <tr>
                                    <td></td>
                                    <td colspan="2">Sub total
                                    </td>
                                    <td style="width: 150px;">
                                        <asp:HiddenField ID="hdnCartSubTotal" runat="server" ClientIDMode="Static" />
                                        <asp:Label ID="lblSubTotal" runat="server" Width="120px" ClientIDMode="Static" Enabled="false"
                                            Style="text-align: right; border: none;" AutoCompleteType="None"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        <asp:TextBox ID="txtDiscountOnSubTotal" runat="server" Style="text-align: right; width: 70px"
                                            onblur="setCartTotalOnChange();" MaxLength="7" onkeydown="AllowDecimal(this,event);"
                                            onkeypress="AllowDecimal(this,event);" ClientIDMode="Static"></asp:TextBox>
                                        <asp:CheckBox ID="chkboxDiscountOnSubTotal" runat="server" Text="%" onclick="setCartTotalOnChange()"
                                            ClientIDMode="Static" />
                                        Additional discount on sub total
                                    </td>
                                    <td style="border-bottom: solid 2px silver;">
                                        <asp:HiddenField ID="hdnCartDiscountOnSubTotal" runat="server" ClientIDMode="Static" />
                                        <asp:Label ID="lblDiscountOnSubTotal" runat="server" Width="120px" Style="text-align: right;"
                                            ClientIDMode="Static"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td colspan="2">Sub total after additional discount
                                    </td>
                                    <td style="width: 150px">
                                        <asp:HiddenField ID="hdnCartSubTotal2" runat="server" ClientIDMode="Static" />
                                        <asp:Label ID="lblSubTotal2" runat="server" Width="120px" Style="text-align: right;"
                                            ClientIDMode="Static"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td colspan="2">
                                        <span>Tax</span>
                                        <img id="ImgTaxOnSubtotal" src="../App_Themes/Blue/img/TaxCalculation16.png" style="cursor: pointer;"
                                            onclick="taxFlyoutOnOpen(this,'0');" />
                                    </td>
                                    <td>
                                        <asp:HiddenField ID="hdnCartTaxOnSubTotal" runat="server" ClientIDMode="Static" />
                                        <asp:Label ID="lblTaxOnSubTotal" runat="server" Width="120px" Style="text-align: right;"
                                            ClientIDMode="Static"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: left;" rowspan="2">Additional Charge Description
                                    </td>
                                    <td style="text-align: left;" rowspan="2">
                                        <asp:TextBox runat="server" ID="txtAdditionalChargeDescription" Width="160px" TextMode="MultiLine"
                                            Height="40px" ClientIDMode="Static"></asp:TextBox>
                                    </td>
                                    <td>Additional Charges
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtAdditionalCharges" runat="server" Style="text-align: right; width: 120px;"
                                            onchange="setCartTotalOnChange2();" MaxLength="10" onkeydown="AllowDecimal(this,event);"
                                            onkeypress="AllowDecimal(this,event);" ClientIDMode="Static"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Shipping Charges
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtShippingCharges" runat="server" Style="text-align: right; width: 120px;"
                                            onchange="setCartTotalOnChange2();" MaxLength="10" onkeydown="AllowDecimal(this,event);"
                                            onkeypress="AllowDecimal(this,event);" ClientIDMode="Static"> </asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td></td>
                                    <td style="border-top: solid 2px silver; padding-top: 3px; font-weight: bold;">
                                        <req>Grand Total</req>
                                    </td>
                                    <td style="border-top: solid 2px silver; padding-top: 3px; font-weight: bold;">
                                        <asp:HiddenField ID="hdnCartGrandTotal" runat="server" ClientIDMode="Static" />
                                        <asp:Label ID="lblGrandTotal" runat="server" Width="120px" Style="text-align: right; border: none;"
                                            Font-Bold="true" ClientIDMode="Static"></asp:Label>
                                    </td>
                                </tr>
                            </table>                               
                        </td>
                    </tr>
                </table>
                <table width="100%">
                    <tr>
                        <td>
                            <table>
                                <tr>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                </tr>
                            </table>
                        </td>
                        <td align="right"><%--align="right" style="margin-right: 50px;"--%>
                            <table style="text-align: left;">
                                <tr>
                                    <td align="left">
                                        <asp:Label ID="lblTQty" runat="server" Text="Total Quantity"></asp:Label>
                                        :
                                    <asp:TextBox ID="txtTotalQty" runat="server" Enabled="false" Width="60px"></asp:TextBox>
                                    </td>
                                    <td></td>
                                    <td style="visibility:hidden;">
                                        <asp:Label ID="lblGTotal" runat="server" Text="Grand Total"></asp:Label>
                                        :
                                    <asp:TextBox ID="txtGrandTotal" runat="server" Enabled="false" Width="125px"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <div class="divHead" id="divHeadASNDetail" runat="server" clientidmode="Static">
            <h4>
                <asp:Label ID="lblASNNo" runat="server" Text="ASN Details"></asp:Label>
            </h4>
            <a onclick="javascript:divcollapsOpen('divASNDetail',this)" id="linkASN" runat="server">Collapse</a>
        </div>
        <div class="divDetailExpand" id="divASNDetail" runat="server" clientidmode="Static">
            <center>
                <table class="gridFrame" width="90%" id="tblasn">
                    <tr>
                        <td>
                            <table style="width: 100%">
                                <tr>
                                    <td style="text-align: left;">
                                        <asp:Label ID="lblAsnDetails" runat="server" Style="color: white; font-size: 15px; font-weight: bold;"
                                            Text=" ASN Details"></asp:Label>
                                    </td>
                                    <td style="text-align: right;">
                                        <input type="button" id="Button1" runat="server" value="Add New"
                                            onclick="AddNewASN();" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <obout:Grid ID="grdASN" runat="server" Width="100%" CallbackMode="true" Serialize="true"
                                AllowColumnReordering="true" AllowColumnResizing="true" AutoGenerateColumns="false"
                                AllowPaging="false" ShowLoadingMessage="false" AllowSorting="false" AllowManualPaging="false"
                                AllowRecordSelection="false" ShowFooter="false">
                                <%--OnRebind="GVInboxPOR_OnRebind" OnRebind="GVInboxPOR_OnRebind"--%>
                                <Columns>
                                    <obout:Column HeaderText="View" DataField="ID" Width="4%" Align="center" HeaderAlign="center">
                                        <TemplateSettings TemplateId="TemplateasnEdit" />
                                    </obout:Column>
                                    <obout:Column DataField="ASNNumber" HeaderText="ASN Number" Width="8%" Align="center"
                                        HeaderAlign="center">
                                    </obout:Column>
                                    <obout:Column DataField="ASNDate" HeaderText="ASN Date" Width="8%" DataFormatString="{0:dd-MMM-yy}">
                                    </obout:Column>
                                    <obout:Column DataField="ASNBy" HeaderText="ASNBy" Wrap="true" Width="10%">
                                    </obout:Column>
                                    <obout:Column DataField="Remark" HeaderText="Remark" Width="8%">                                        
                                    </obout:Column>
                                   <%-- <obout:Column DataField="MessageTitle" HeaderText="Message Title" Width="8%">
                                    </obout:Column>--%>
                                    <%--<obout:Column DataField="Message" HeaderText="Message" Width="15%" Wrap="true">
                                        <TemplateSettings TemplateId="GetMessage" />
                                    </obout:Column>--%>
                                </Columns>
                                <Templates>                                   
                                    <obout:GridTemplate runat="server" ID="TemplateasnEdit">
                                        <Template>
                                            <asp:ImageButton ID="imgBtnViewASN" runat="server" ImageUrl="../App_Themes/Blue/img/Search16.png"
                                                CausesValidation="false" ToolTip='<%# (Container.Value) %>' data-containerId="<%# (Container.Value) %>"  OnClick="imgBtnViewASN_OnClick" />
                                            <%--  OnClick="imgBtnView_OnClick"--%>
                                        </Template>
                                    </obout:GridTemplate>                                   
                                </Templates>
                            </obout:Grid>
                        </td>
                    </tr>
                </table>
            </center>
        </div>
        <div class="divHead" id="divApprovalHead" runat="server" clientidmode="Static">
            <h4>
                <asp:Label ID="lblApproval" runat="server" Text="Approval"></asp:Label>
            </h4>
            <a onclick="javascript:divcollapsOpen('divApprovalDetail',this)" id="linkApprovalDetail"
                runat="server">Collapse</a>
        </div>
        <div class="divDetailExpand" id="divApprovalDetail" runat="server" clientidmode="Static">
            <br />
            <table class="gridFrame" width="100%" id="Table2">
                <tr>
                    <td>
                        <table style="width: 100%">
                            <tr>
                                <td style="text-align: left;">
                                    <asp:Label ID="lblApprovalHistory" CssClass="headerText" runat="server" Text="Approval"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <obout:Grid ID="gvApprovalRemark" runat="server" CallbackMode="true" Serialize="true"
                            AllowColumnReordering="true" AllowColumnResizing="true" AutoGenerateColumns="false"
                            AllowPaging="false" ShowLoadingMessage="true" AllowSorting="false" AllowManualPaging="false"
                            AllowRecordSelection="false" ShowFooter="false" Width="100%" PageSize="-1">
                            <%-- OnRebind="gvApprovalRemark_OnRebind"--%>
                            <Columns>
                                <obout:Column DataField="id" Visible="false">
                                </obout:Column>
                                <obout:Column DataField="OrderId" Visible="false">
                                </obout:Column>
                                <obout:Column DataField="ApprovalId" HeaderText="Level" AllowEdit="false" Width="4%"
                                    Align="center" HeaderAlign="center">
                                </obout:Column>
                                <obout:Column DataField="ApproverName" HeaderText="Approver Name" AllowEdit="false"
                                    Width="12%" Align="center" HeaderAlign="center">
                                </obout:Column>
                                <obout:Column DataField="Date" HeaderText="Approved Date" AllowEdit="false" DataFormatString="{0:dd-MMM-yyyy}"
                                    Width="8%" Align="center" HeaderAlign="center">
                                </obout:Column>
                                <obout:Column DataField="DeligateUser" HeaderText="Delegate User" AllowEdit="false"
                                    Width="12%" HeaderAlign="center" Align="center">
                                </obout:Column>
                                <obout:Column DataField="Remark" HeaderText="Remark" AllowEdit="false" Width="15%"
                                    HeaderAlign="left" Wrap="true">
                                </obout:Column>
                                <obout:Column DataField="StatusName" HeaderText="Status" AllowEdit="false" Width="10%"
                                    HeaderAlign="left" Wrap="true">
                                </obout:Column>
                                <obout:Column DataField="ImgApproval" HeaderText="Approve" HeaderAlign="center" Align="center"
                                    Width="7%">
                                    <TemplateSettings TemplateId="GTStatusGUIApproval" />
                                </obout:Column>
                                <obout:Column DataField="ImgReject" HeaderText="Reject" HeaderAlign="center" Align="center"
                                    Width="7%">
                                    <TemplateSettings TemplateId="GTStatusGUIReject" />
                                </obout:Column>
                                <obout:Column DataField="ImgApprovewithRevision" HeaderText="Approve With Revision"
                                    HeaderAlign="center" Align="center" Width="0%" Visible="false">
                                    <TemplateSettings TemplateId="GTStatusGUIApproveWithRevision" />
                                </obout:Column>
                            </Columns>
                            <Templates>
                                <obout:GridTemplate ID="GTStatusGUIApproval" runat="server">
                                    <Template>
                                        <center>
                                            <div class='<%# ("POR" + Container.Value) %>' onclick="RequestOpenEntryForm('Approval','<%# Container.Value %>', '<%# Container.DataItem["ApproverID"] %>','<%# Container.DataItem["DeligateTo"] %>','<%# Container.DataItem["OrderId"] %>','<%# Container.DataItem["ApprovalId"] %>')">
                                            </div>
                                        </center>
                                    </Template>
                                </obout:GridTemplate>
                                <obout:GridTemplate ID="GTStatusGUIReject" runat="server">
                                    <Template>
                                        <center>
                                            <div class='<%# ("POR" + Container.Value) %>' onclick="RequestOpenEntryFormReject('Approval','<%# Container.Value %>', '<%# Container.DataItem["ApproverID"] %>','<%# Container.DataItem["OrderId"] %>')">
                                            </div>
                                        </center>
                                    </Template>
                                </obout:GridTemplate>
                                <obout:GridTemplate ID="GTStatusGUIApproveWithRevision" runat="server">
                                    <Template>
                                        <center>
                                            <div class='<%# ("POR" + Container.Value) %>' onclick="RequestOpenEntryAppWithRevision('<%# Container.Value %>', '<%# Container.DataItem["id"] %>','<%# Container.DataItem["OrderId"] %>')">
                                            </div>
                                            <%--RequestOpenEntryFormReject('Approval','<%# Container.Value %>', '<%# Container.DataItem["id"] %>','<%# Container.DataItem["OrderId"] %>')--%>
                                        </center>
                                    </Template>
                                </obout:GridTemplate>
                            </Templates>
                        </obout:Grid>
                    </td>
                </tr>
            </table>
        </div>
        <div class="divHead" id="divIssueHead" runat="server" clientidmode="Static">
            <h4>
                <asp:Label ID="lblDispatch" runat="server" Text="Dispatch"></asp:Label>
            </h4>
            <a onclick="javascript:divcollapsOpen('divIssueDetail',this)" id="linkIssueDetail"
                runat="server">Collapse</a>
        </div>
        <div class="divDetailExpand" id="divIssueDetail" runat="server" clientidmode="Static">
            <table class="tableForm">
                <tr>
                </tr>
                <tr>
                    <td colspan="7" align="left" style="text-align: left; font-size: 15px; color: gray;">
                        <asp:Label ID="lblDispatchDetails" runat="server" Text="Dispatch Details"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblshipeddate" runat="server" Text="Ready For Dispatch Date"></asp:Label>
                        <%--  Shipped Date :--%>
                    </td>
                    <td style="text-align: left">
                        <asp:TextBox runat="server" ID="txtShippedDate" MaxLength="50" Width="200px" Enabled="false"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Label ID="lblReceivedDate" runat="server" Text="Dispatch Date"></asp:Label>
                    </td>
                    <td style="text-align: left;">
                        <asp:TextBox runat="server" ID="txtReceivedDate" MaxLength="50" Width="200px" Enabled="false"></asp:TextBox>
                    </td>
                    <%-- </tr>
                <tr>--%>
                    <td>
                        <asp:Label ID="lblCloseDate" runat="server" Text="Cancel Date"></asp:Label>
                    </td>
                    <td colspan="3" style="text-align: left">
                        <asp:TextBox runat="server" ID="txtCloseDate" MaxLength="50" Width="200px" Enabled="false"></asp:TextBox>
                    </td>
                    <%-- </tr>
                <tr>--%>
                    <td>
                        <asp:Label ID="lblRemark1" runat="server" Text="Remark"></asp:Label>
                    </td>
                    <td colspan="3" style="text-align: left">
                        <asp:TextBox runat="server" ID="txtDispatchRemark" Width="200px"
                            Enabled="false"></asp:TextBox>
                        <%--TextMode="MultiLine"--%>
                    </td>
                </tr>
            </table>
            <br />
            <table class="tableForm">
                <tr>
                    <td colspan="5" align="left" style="text-align: left; font-size: 15px; color: gray;">
                        <asp:Label ID="lblCustomerDetails" runat="server" Text="Customer Details"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblCustName" runat="server" Text="Customer Name"></asp:Label>
                    </td>
                    <td style="text-align: left;">
                        <asp:TextBox runat="server" ID="txtCustName" MaxLength="100" Width="200px" Enabled="false"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Label ID="lblContactNo" runat="server" Text="Contact No."></asp:Label>
                    </td>
                    <td style="text-align: left;">
                        <asp:TextBox runat="server" ID="txtContactNo" MaxLength="100" Width="200px" Enabled="false"></asp:TextBox>
                    </td>
                    <%--  <td>
                        <asp:Label ID="lblPhotoID" runat="server" Text="Photo ID"></asp:Label>
                    </td>
                    <td style="text-align: left;">
                        <asp:TextBox runat="server" ID="txtPhotoID" MaxLength="100" Width="200px" Enabled="false"></asp:TextBox>
                    </td>--%>
                    <td>
                        <asp:Label ID="lblEmail" runat="server" Text="Email Id"></asp:Label>
                    </td>
                    <td style="text-align: left;">
                        <asp:TextBox runat="server" ID="txtEmail" MaxLength="100" Width="200px" Enabled="false"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblCustAddress" runat="server" Text="Address"></asp:Label>
                    </td>
                    <td style="text-align: left;">
                        <asp:TextBox runat="server" ID="txtCustAddress" TextMode="MultiLine" Width="200px" Enabled="false"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Label ID="lblLandmark" runat="server" Text="Landmark"></asp:Label>
                    </td>
                    <td style="text-align: left;">
                        <asp:TextBox runat="server" ID="txtLandmark" Width="200px" Enabled="false"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Label ID="lblZipCode" runat="server" Text="Zip Code"></asp:Label>
                    </td>
                    <td style="text-align: left;">
                        <asp:TextBox runat="server" ID="txtZipcode" Width="200px" Enabled="false"></asp:TextBox>
                    </td>
                    <%--<td colspan="2">
                        <asp:Image ID="imgPhotoID" runat="server" Width="200px" />
                    </td>--%>
                </tr>
            </table>
            <br />
            <table class="tableForm">
                <tr>
                    <td colspan="5" align="left" style="text-align: left; font-size: 15px; color: gray;">
                        <asp:Label ID="lblPaymentDetail" runat="server" Text="Payment Details"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblPaymentMode" runat="server" Text="Payment Mode"></asp:Label>
                    </td>
                    <td style="text-align: left;">
                        <asp:TextBox runat="server" ID="txtPaymentMode" Width="200px" Enabled="false"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Label ID="lblCardNo" runat="server" Text="Card No"></asp:Label>
                    </td>
                    <td style="text-align: left;">
                        <asp:TextBox runat="server" ID="txtCardNo" Width="200px" Enabled="false"></asp:TextBox>
                        <img alt="Verified" src="Img/tick_16.png" id="Verified" runat="server" visible="false" />
                        <img alt="Decline" src="Img/delete_16.png" id="Decline" runat="server" visible="false" />
                        <img alt="Pending" src="Img/Pending.png" id="Pending" runat="server" visible="false" />
                    </td>
                    <td>
                        <asp:Label ID="lblPaymentRemark" runat="server" Text="Remark"></asp:Label>
                    </td>
                    <td style="text-align: left;">
                        <asp:TextBox runat="server" ID="txtPaymentRemark" Width="200px" Enabled="false"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblBankName" runat="server" Text="Bank Name"></asp:Label>
                    </td>
                    <td style="text-align: left;">
                        <asp:TextBox runat="server" ID="txtBankName" Width="200px" Enabled="false"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Label ID="lblPaymentDate" runat="server" Text="Payment Date"></asp:Label>
                    </td>
                    <td style="text-align: left;">
                        <asp:TextBox runat="server" ID="txtPaymentDate" Width="200px" Enabled="false"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <asp:Label ID="lblVerified" runat="server" Text="Verified"></asp:Label>
                        <img alt="Verified" src="Img/tick_16.png" id="Img4" runat="server" />
                        <asp:Label ID="lblDecline" runat="server" Text="Decline"></asp:Label>
                        <img alt="Decline" src="Img/delete_16.png" id="Img5" runat="server" />
                        <asp:Label ID="lblPending" runat="server" Text="Pending"></asp:Label>
                        <img alt="Pending" src="Img/Pending.png" id="Img6" runat="server" />
                    </td>
                </tr>
            </table>
            <br />
            <table class="tableForm">
                <tr>
                    <td colspan="5" align="left" style="text-align: left; font-size: 15px; color: gray;">
                        <asp:Label ID="lblDriverDetails" runat="server" Text="Driver Details"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblDriverName" runat="server" Text=" Driver Name"></asp:Label>
                    </td>
                    <td style="text-align: left;">
                        <asp:TextBox runat="server" ID="txtDriverName" Width="200px" Enabled="false"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Label ID="lblDriverContactNo" runat="server" Text="Contact No."></asp:Label>
                    </td>
                    <td style="text-align: left;">
                        <asp:TextBox runat="server" ID="txtDriverContactNo" Width="200px" Enabled="false"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Label ID="lblDriverEmail" runat="server" Text="Email ID"></asp:Label>
                    </td>
                    <td style="text-align: left;">
                        <asp:TextBox runat="server" ID="txtDriverEmail" Width="200px" Enabled="false"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblTruckDetail" runat="server" Text="Truck Detail"></asp:Label>
                    </td>
                    <td style="text-align: left;">
                        <asp:TextBox runat="server" ID="txtTruckDetails" Width="200px" Enabled="false"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Label ID="lblAssignDate" runat="server" Text="Assign Date"></asp:Label>
                    </td>
                    <td style="text-align: left;">
                        <asp:TextBox runat="server" ID="txtAssignDate" Width="200px" Enabled="false"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Label ID="lblDeliveryType" runat="server" Text="Delivery Type"></asp:Label>
                    </td>
                    <td style="text-align: left;">
                        <asp:TextBox runat="server" ID="txtDeliveryType" Width="200px" Enabled="false"></asp:TextBox>
                    </td>
                </tr>
            </table>
            <br />
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
        <table class="tableForm" style="visibility: hidden;">
            <tr>
                <td style="font-size: 13px; font-weight: bold;">
                    <asp:Label ID="lblOperationApproval" runat="server" Text="Operation Approval *"></asp:Label>
                    :
                </td>
                <td style="vertical-align: top; font-size: 13px; font-weight: bold; text-align: left;">
                    <label class="label_check" id="lblApproved" for="CheckBoxApproved">
                        <asp:CheckBox ID="CheckBoxApproved" runat="server" ClientIDMode="Static" onclick="OnlyOneCheckedA('CheckBoxApproved','CheckBoxRejected');" />
                        <asp:Label ID="lblApproved1" runat="server" Text="Approved"></asp:Label>
                    </label>
                    <label class="label_check" id="lblRejected" for="CheckBoxRejected">
                        <asp:CheckBox ID="CheckBoxRejected" runat="server" ClientIDMode="Static" onclick="OnlyOneCheckedR('CheckBoxApproved','CheckBoxRejected');" />
                        <asp:Label ID="lblCancelled" runat="server" Text="Cancelled"></asp:Label>
                    </label>
                    <label class="label_check" id="lblRevision" for="CheckBoxRevision">
                        <asp:CheckBox ID="CheckBoxRevison" runat="server" ClientIDMode="Static" onclick="OnlyOneCheckedR('CheckBoxApproved','CheckBoxRejected');" />
                        <%--<asp:Label ID=" ApproveWithRevision" runat="server" Text=" Approve With Revision"></asp:Label>--%>
                        <asp:Label ID="lblapprovrevision" runat="server" Text="Approve With Revision"></asp:Label>
                    </label>
                </td>
            </tr>
            <tr>
                <td style="text-align: right;">
                    <asp:Label ID="lblDate" runat="server" Text="Date"></asp:Label>
                    :
                </td>
                <td style="text-align: left;">
                    <asp:Label runat="server" ID="lblApprovalDate"></asp:Label>
                </td>
            </tr>
            <tr>
                <td id="tdApprovalRemark" style="text-align: right;">
                    <asp:Label ID="lblApprovRemark" runat="server" Text="Remark"></asp:Label>
                    :
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtApprovalRemark" onkeyup="TextBox_KeyUp(this,'CharactersCountertxtApprovalRemark','200');"
                        ClientIDMode="Static" Width="400px" TextMode="MultiLine">
                    </asp:TextBox>
                </td>
            </tr>
            <tr>
                <td></td>
                <td style="text-align: left;">
                    <span class="watermark"><span id="CharactersCountertxtApprovalRemark">
                        <asp:Label ID="lbl200" runat="server" Text="200"></asp:Label></span><asp:Label ID="lbl2001"
                            runat="server" Text="/ 200"></asp:Label>
                    </span>
                    <input type="button" id="btnSaveApproval" value="Submit" style="float: right;" onclick="jsSaveApproval()"
                        runat="server" />
                </td>
            </tr>
        </table>
        <asp:HiddenField ID="hdnselectedCompany" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnselectedDept" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnSelectedWarehouse" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnSelectedVendor" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnselectedCont1" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnselectedCont2" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnSelAddress" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnSelectedQty" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnSearchContactID1" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnSearchContactName1" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnSearchContactID2" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnSearchContactName2" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnSearchAddressID" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnSearchAddress" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnSearchLocationID" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnSearchLocationName" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnSearchLocation" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnChngDept" runat="server" />
        <asp:HiddenField ID="hdnSelPaymentMethod" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnEnteredPrice" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnEnteredQty" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnSelectedUMO" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnMaxDeliveryDays" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="PMText" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="PMLable" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="PMReturn" runat="server" ClientIDMode="Static" Value="0" />
        <asp:HiddenField ID="hdnApprovalId" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnSelectedSequenceNo" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnChangePrdQtyPrice" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnNewOrderID" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnOrderStatus" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnPmethodChng" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnLocConID" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnLocConName" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnCartCurrentObjectName" runat="server" />
        <asp:HiddenField ID="hdnSelectedRowTax" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnSelectedRowPercnt" runat="server" ClientIDMode="Static" />
    </center>
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

        /*Navigation code*/
        function OpenEntryForm(invoker, state, referenceID, requestID) {
            if (state != "gray") {
                if (state == "red") { state = "Edit"; }
                else if (state != "red") { state = "View"; }
                pupUpLoading.style.display = "";
                PageMethods.WMSetSession(referenceID, requestID, state, OpenEntryFormOnSuccess, null);

            }
            else {
                showAlert("Not Applicable", "Error", "#");
            }
        }

    </script>
    <script type="text/javascript">
        /*Approval JS*/
        /*Approval checkbox Only one checked Approved or Rejected*/
        function OnlyOneCheckedA(chkA, chkR) {
            var findtd = document.getElementById('tdApprovalRemark');

            if (document.getElementById(chkA).checked == true) {
                findtd.innerHTML = "Remark / Reason : ";
                document.getElementById('txtApprovalRemark').accessKey = "";
                document.getElementById(chkR).checked = false;
                lblRejected.className = "label_check c_off";
            }
        }

        function OnlyOneCheckedR(chkA, chkR) {
            var findtd = document.getElementById('tdApprovalRemark');

            if (document.getElementById(chkR).checked == true) {
                findtd.innerHTML = "Remark / Reason * :";
                document.getElementById('txtApprovalRemark').accessKey = "1";
                document.getElementById(chkA).checked = false;
                lblApproved.className = "label_check c_off";
            }
        }

        function jsSaveApproval() {
            if (document.getElementById('CheckBoxApproved').checked == false && document.getElementById('CheckBoxRejected').checked == false) {
                showAlert("Approval status should not be left blank [ Approved or Rejected ]", "Error", "#");
            }
            else if (document.getElementById('CheckBoxRejected').checked == true && document.getElementById('txtApprovalRemark').value == "") {
                showAlert("Fill rejection reason", "Error", "#");
                document.getElementById('txtApprovalRemark').focus();
            }
            else {
                LoadingOn(true);
                debugger;
                PageMethods.WMSaveApproval(document.getElementById('CheckBoxApproved').checked, document.getElementById('txtApprovalRemark').value, jsSaveApprovalOnSuccess, jsSaveApprovalOnFail);
            }
        }

        function jsSaveApprovalOnSuccess(result) {
            if (result.toString().toLowerCase() == 'true') {
                showAlert("Approval status has been saved successfully", "info", "../PowerOnRent/Default.aspx?invoker=Request");
            }
            else {
                showAlert("Some error occurred", "error", "#");
            }
        }
        function jsSaveApprovalOnFail() { }

    </script>
    <script type="text/javascript">
        var txtTitle = document.getElementById("<%= txtTitle.ClientID %>");
        var ddlSites = document.getElementById("<%= ddlSites.ClientID %>");
        var ddlCompany = document.getElementById("<%= ddlCompany.ClientID %>");
        var lblRequestNo = document.getElementById("<%= lblRequestNo.ClientID %>");
        var ddlStatus = document.getElementById("<%= ddlStatus.ClientID %>");


        var ddlRequestByUserID = document.getElementById("<%= ddlRequestByUserID.ClientID %>");
        var txtRemark = document.getElementById("<%= txtRemark.ClientID %>");
        var ddlContact1 = document.getElementById("<%= ddlContact1.ClientID %>");

        var ddlAddress = document.getElementById("<%= ddlAddress.ClientID %>");
        var txtCustOrderRefNo = document.getElementById("<%= txtCustOrderRefNo.ClientID %>");

        /*Toolbar Code*/
        function jsAddNew() {
           PageMethods.WMpageAddNew(jsAddNewOnSuccess, null);
        }
        function jsAddNewOnSuccess() {
            //Grid1.refresh();
            //ClearMode('divRequestDetail');
            //jsFillStatusList('Add');
            window.open('../WMS/PurchaseOrder.aspx', '_self', '');
        }

        /*Add By Suresh */

        function OpenProductScreen() {
            window.open('../Product/ProductMaster.aspx', '_self', '');
        }

        /*Add By Suresh */

        //function jsSaveData() {
       // function GetPaymentMethodDetails() {
            function jsSaveData() {
                var validate = validateForm('divRequestDetail');
                if (validate == true) {
                    var Con1ID = document.getElementById("hdnSearchContactID1");
                    var Con2ID = document.getElementById("hdnSearchContactID2");
                    var AdrsID = document.getElementById("hdnSearchAddressID");

                    var AdrsSelID = document.getElementById("hdnSelAddress");
                    var Con1Sel = document.getElementById("hdnselectedCont1");
                    var Con2Sel = document.getElementById("hdnselectedCont2");
                    var LocationID = document.getElementById("hdnSearchLocationID");

                    //var PMD = GetPaymentMethodDetails();
                    //var PMReturn = document.getElementById('PMReturn');
                    //var PMD = PMReturn.value;

                    //var DTDIFF = GetDateDifference();
                    //var hdnMaxDeliveryDays = document.getElementById('hdnMaxDeliveryDays');

                    //if (PMD == 0) {
                    //    showAlert("Fill all mandatory fields of Payment Methods", "Error", "#");
                    //} else if (validate == false) {
                    //    showAlert("Fill all mandatory fields", "Error", "#");
                    //}
                    //else if (getDateFromUC("<%= UC_ExpDeliveryDate.ClientID %>") == "") {
                    //   showAlert("Please Select Exp. Delivery Date", "Error", "#");
                    //}
                    //else if (DTDIFF == 0) {
                    //    showAlert("Selected Exp. Delivery Date is must less than " + hdnMaxDeliveryDays.value + " Days", "Error", "#");
                    //}
                    //else if (DTDIFF == 2) {
                    //    showAlert("Please Select Exp. Delivery Date", "Error", "#");
                    //}
                    //else {
                    // if (ddlStatus.options[ddlStatus.selectedIndex].value == 2 && Grid1.Rows.length == 0) {
                    if (Grid1.Rows.length == 0) {
                        showAlert("Add atleast one part into the Request Part List", "error", "#");
                    }
                    else if (getDateFromUC("<%= UC_ExpDeliveryDate.ClientID %>") == "") {
                        showAlert("Please Select Exp. Delivery Date", "Error", "#");
                    }
                    else {
                        // Code to check all txtUserQty values
                        //var getLength = $('.divTxtUserQty input').length();
                        //alert(getLength);
                        var isContainsZero = 'no';
                        var matches = 0;
                        $(".divTxtUserQty input").each(function (i, val) {
                            if (($(this).val() == '0.00') || ($(this).val() == '0') || ($(this).val() == 0)) {
                                isContainsZero = 'yes';
                            }
                        });

                        var isContnZro = 'no';
                        var mtch = 0;
                        $(".divrowQtyTotal span").each(function (i, html) {
                            if (($(this).html() == '0') || ($(this).html() == '0.00') || ($(this).html() == 0) || ($(this).html() == 'NaN')) {
                                isContnZro = 'yes';
                            }
                        });

                        var isPriceZro = 'no';
                        var mth = 0;
                        $(".divTxtPrice input").each(function (i, val) {
                            if (($(this).val() == '0.00') || ($(this).val() == '0') || ($(this).val() == 0)) {
                                isPriceZro = 'yes';
                            }
                        });

                        if (isContainsZero != 'yes' && isContnZro != 'yes') {
                            LoadingOn(true);

                            var Deliverydate = getDateFromUC("<%= UC_ExpDeliveryDate.ClientID %>");
                            var obj1 = new Object();
                            obj1.StoreId = parseInt(ddlSites.options[ddlSites.selectedIndex].value);
                            obj1.VendorID = parseInt(ddlCompany.options[ddlCompany.selectedIndex].value);
                            // obj1.OrderNumber = lblRequestNo.innerHTML.toString();
                            obj1.OrderNumber = txtCustOrderRefNo.value.toString();
                            obj1.Priority = "Medium";// ddlRequestType.options[ddlRequestType.selectedIndex].value.toString();
                            obj1.Status = parseInt(ddlStatus.options[ddlStatus.selectedIndex].value);
                            obj1.Title = txtTitle.value.toString();

                            obj1.RequestBy = parseInt(ddlRequestByUserID.options[ddlRequestByUserID.selectedIndex].value);
                            obj1.Remark = txtRemark.value.toString();
                            obj1.Deliverydate = Deliverydate;
                            //obj1.ContactId1 = parseInt(ddlContact1.options[ddlContact1.selectedIndex].value);                        
                            if (Con1ID.value == "" && Con1Sel.value != "") { obj1.ContactId1 = parseInt(Con1Sel.value); }
                            else {
                                obj1.ContactId1 = parseInt(Con1ID.value);
                            }


                            if (obj1.ContactId1 == 0) {
                                obj1.ContactId2 = 0;
                            } else {
                                //if (Con2ID.value != "") {
                                //    obj1.ContactId2 = Con2ID.value;
                                //} else { obj1.ContactId2 = 0; }
                            }
                            // obj1.AddressId = parseInt(ddlAddress.options[ddlAddress.selectedIndex].value);
                            if (AdrsID.value == "" && AdrsSelID.value != "") {
                                obj1.AddressId = parseInt(AdrsSelID.value);
                            } else if (AdrsID.value != "" && AdrsSelID.value == "") {
                                obj1.AddressId = parseInt(AdrsID.value);
                            } else {
                                obj1.AddressId = 0;
                            }

                            if (LocationID.value == "") {
                                obj1.LocationID = 0
                            } else {
                                obj1.LocationID = LocationID.value;
                            }
                            //var pm = $('#ddlPaymentMethod').val();
                            //obj1.PaymentID = pm;

                            var txtGrandTotal = document.getElementById("<%=txtGrandTotal.ClientID%>");
                            var txtTotalQty = document.getElementById("<%=txtTotalQty.ClientID%>");

                            obj1.TotalQty = txtTotalQty.value;
                            obj1.GrandTotal = txtGrandTotal.value;

                            /*New Added WMS Fields For Tax & Total Start*/
                            var lblSubTotal = document.getElementById("<%=lblSubTotal.ClientID%>");
                            obj1.SubTotal = lblSubTotal.innerHTML;
                            var txtDiscountOnSubTotal = document.getElementById("<%=txtDiscountOnSubTotal.ClientID%>");
                            obj1.DiscountOnSubTotal = txtDiscountOnSubTotal.value;
                            var chkboxDiscountOnSubTotal = document.getElementById("<%=chkboxDiscountOnSubTotal.ClientID%>");
                            if (chkboxDiscountOnSubTotal.checked == true) {
                                obj1.DiscountOnSubTotalPercent = true;
                            } else { obj1.DiscountOnSubTotalPercent = false; }
                            var lblDiscountOnSubTotal = document.getElementById("<%=lblDiscountOnSubTotal.ClientID%>");
                            obj1.TotalDiscount = lblDiscountOnSubTotal.innerHTML;
                            var lblSubTotal2 = document.getElementById("<%=lblSubTotal2.ClientID%>");
                            obj1.TotalAfterDiscount = lblSubTotal2.innerHTML;
                            var lblTaxOnSubTotal = document.getElementById("<%=lblTaxOnSubTotal.ClientID%>");
                            obj1.TotalTax = lblTaxOnSubTotal.innerHTML;
                            var txtAdditionalChargeDescription = document.getElementById("<%=txtAdditionalChargeDescription.ClientID%>");
                            obj1.OtherChargesDescription = txtAdditionalChargeDescription.value;
                            var txtAdditionalCharges = document.getElementById("<%=txtAdditionalCharges.ClientID%>");
                            obj1.OtherCharges = txtAdditionalCharges.value;
                            var txtShippingCharges = document.getElementById("<%=txtShippingCharges.ClientID%>");
                            obj1.ShippingCharges = txtShippingCharges.value;
                            var lblGrandTotal = document.getElementById("<%=lblGrandTotal.ClientID%>");
                            obj1.TotalAmount = lblGrandTotal.innerHTML;

                            /*New Added WMS Fields For Tax & Total End*/

                            //if (ddlStatus.selectedIndex == 1) { obj1.IsSubmit = "false"; }
                            //else {
                            obj1.IsSubmit = "true";
                            //}

                            // SavePaymentMethod();
                            PageMethods.WMSaveRequestHead(obj1, WMSaveRequestHead_onSuccessed, WMSaveRequestHead_onFailed);

                        } else {
                            showAlert("One or more request or order quantity is zero", "error", "#");
                        }                    //return false;
                        // Code to check all txtUserQty values
                    }
                } else {
                    showAlert("Fill All Mandatory Fields...", "error", "#");
                }
            //}
}

function WMSaveRequestHead_onSuccessed(result) {
    var hdnNewOrderID = document.getElementById('hdnNewOrderID');
    if (result == 0 || result == 0) { showAlert("Order Generation Failed. Please Check the Order Input and Department Configurations...", "Error", '../WMS/Inbound.aspx'); }
    else if (result >= 1) {
        //"Request saved successfully"
        hdnNewOrderID.value = result;
        SavePaymentMethod();
        showAlert("Request saved successfully", "info", "../WMS/Inbound.aspx");
    }
    else if (result == -3) {
        showAlert("Request saved successfully. Email Notification Failed...", "info", "../WMS/Inbound.aspx");
    }
    else {
        window.open('../PowerOnRent/Approval.aspx?REQID=' + result + '&ST=24&APL=0', null, 'height=180px, width=595px,status= 0, resizable= 1, scrollbars=1, toolbar=0,location=center,menubar=0, screenX=300; screenY=50');
        showAlert("Request Edited Successfully", "info", "../WMS/Inbound.aspx");
    }
}

function WMSaveRequestHead_onFailed() { showAlert("Error occurred", "Error", "../WMS/Inbound.aspx"); }

function OnSuccessMandatory(result) {
    var SelCon = result
    var pm = $('#ddlPaymentMethod').val();
    var PMReturn = document.getElementById('PMReturn');
    //PMReturn.value = 0;
    var count = (SelCon.match(/,/g) || []).length;
    console.log(count);
    var res = result.split(",");

    var PMText = document.getElementById('PMText');
    var txtvl = 0;
    $(".lstText input").each(function (i, val) {
        txtval = $(this).val();
        if (i == 0) { PMText.value = $(this).val(); }
        else { PMText.value = PMText.value + '|' + $(this).val(); }

    });

    var addmth = PMText.value.split("|");
    //for (var i = 0; i <= res.length - 1; i++) {
    //for (var i = 0; i <= addmth.length - 1; i++) {
    //    if (addmth[i] == "") {
    //        PMReturn.value = 0;
    //        i = (addmth.length);
    //    } else {
    //        PMReturn.value = 1;           
    //    }
    //}

    for (var i = 1; i <= addmth.length; i++) {
        if (i == result) {
            if (addmth[i] == "") {
                PMReturn.value = 0;
                i = (addmth.length);
            } else {
                PMReturn.value = 1;
            }
        }

    }


    GetPaymentMethodDetails();
    // alertGlow('Wait');

    // return PMReturn.value;
}

//function GetPaymentMethodDetails() {
        //function jsSaveData() {
function GetPaymentMethodDetails() {
    var PMR = document.getElementById('PMReturn');
    // PMReturn.value = 0;
    var pm = $('#ddlPaymentMethod').val(); //alert(pm);
    if (pm <= 4 && pm > 1) {
        if (pm == 3) {
            var txtvl = 0;
            $(".lstText input").each(function (i, val) {
                txtval = $(this).val();
            })
            var txtGrandTotal = document.getElementById("<%=txtGrandTotal.ClientID%>");
            if (txtval == txtGrandTotal.value) {
                PageMethods.WMGetMandatoryDetails(pm, OnSuccessMandatory, null);
            }
            else {
                showAlert("Cash to be collected price must be Grandtotal price ...!!!", "Error", '#');

            }

        }
        else if (pm == 2) {
            var txtvl = 0;
            $(".lstText input").each(function (i, val) {
                txtval = $(this).val();
                if (i == 0) { PMText.value = $(this).val(); }
                else {
                    PMText.value = PMText.value + '|' + $(this).val();
                }
            })

            var addmth = PMText.value.split("|");
            var msisdn = addmth[1];
            if (msisdn != "") {
                if (msisdn.length == 8) {
                    PageMethods.WMGetMandatoryDetails(pm, OnSuccessMandatory, null);

                }
                else {
                    showAlert("MSISDN length must be exactly 8 characters", "Error", '#');
                }

            }
            else {
                showAlert("Please Enter MSISDN", "Error", '#');
            }

        }
        else {
            PageMethods.WMGetMandatoryDetails(pm, OnSuccessMandatory, null);
        }
    } else if (pm == 5) {
        var dpm = $('#ddlFOC').val();
        if (dpm == 0) {
            PMR.value = 0; GetPaymentMethodDetails();
        } else {
            PMR.value = 1; GetPaymentMethodDetails();
        }
    } else if (pm == 1) {
        PMR.value = 1; GetPaymentMethodDetails();
    } else if (pm >= 6) {
        PageMethods.WMGetMandatoryDetails(pm, OnSuccessMandatory, null);
    }
}

function SavePaymentMethod() {
    var hdnNewOrderID = document.getElementById('hdnNewOrderID');
    OrderID = hdnNewOrderID.value;
    var PMReturn = document.getElementById('PMReturn');
    var pm = $('#ddlPaymentMethod').val();
    if (pm <= 4 && pm > 1) {
        PageMethods.WMGetMandatoryDetails(pm, OnSuccessMandatorySave, null);
    } else if (pm == 5) {
        var dpm = $('#ddlFOC').val();
        PageMethods.WmGetPaymentMethodLabelText('Charge To', dpm, pm, 1, OrderID);
        //PMReturn.value = 1;
    } else if (pm == 1) {
        PageMethods.WMPaymentMethodNone(pm, OrderID);
    } else if (pm >= 6) {
        PageMethods.WMGetMandatoryDetails(pm, OnSuccessMandatorySave, null);
    }
}
function OnSuccessMandatorySave(result) {
    var hdnNewOrderID = document.getElementById('hdnNewOrderID');
    OrderID = hdnNewOrderID.value;
    var SelCon = result
    var pm = $('#ddlPaymentMethod').val();
    var PMReturn = document.getElementById('PMReturn');
    var count = (SelCon.match(/,/g) || []).length;
    console.log(count);
    var res = result.split(",");

    var PMText = document.getElementById('PMText');
    var txtvl = 0;
    $(".lstText input").each(function (i, val) {
        txtval = $(this).val();
        if (i == 0) { PMText.value = $(this).val(); }
        else { PMText.value = PMText.value + '|' + $(this).val(); }
    });

    var addmth = PMText.value.split("|");
    // for (var i = 0; i <= res.length - 1; i++) {
    for (var i = 0; i <= addmth.length - 1; i++) {
        var PMLable = document.getElementById('PMLable');
        var lblhtml = 0;
        $(".lstLbl span").each(function (i, html) {
            lblhtml = $(this).html();
            if (i == 0) { PMLable.value = $(this).html(); }
            else { PMLable.value = PMLable.value + '|' + $(this).html(); }
        });
        var Seq = i + 1;
        var addmthLbl = PMLable.value.split("|");
        PageMethods.WmGetPaymentMethodLabelText(addmthLbl[i], addmth[i], pm, Seq, OrderID);
    }
}

function GetDateDifference() {
    var retValue = 0;
    var today = new Date();
    var d = today.getDate(); var m = today.getMonth() + 1; var y = today.getFullYear();
    var cdt = m + '/' + d + '/' + y; //Current Date
    var hdnMaxDeliveryDays = document.getElementById('hdnMaxDeliveryDays');
    // alert(hdnMaxDeliveryDays.value);
    if (getDateFromUC("<%= UC_ExpDeliveryDate.ClientID %>") == "") {
        // showAlert("Please Select Exp. Delivery Date", "Error", "#");
        retValue = 2;
    } else {
        var sdt = getDateFromUC("<%= UC_ExpDeliveryDate.ClientID %>");//Selected Date
        var getselmonth = sdt.split("-");
        var mmmmm = getMonth(getselmonth[1]);
        var selectedDate = mmmmm + '/' + getselmonth[0] + '/' + getselmonth[2]
        //alert(selectedDate);

        var date1 = new Date(selectedDate);
        var date2 = new Date(cdt);
        var timeDiff = Math.abs(date2.getTime() - date1.getTime());
        var diffDays = Math.ceil(timeDiff / (1000 * 3600 * 24));
        // alert(diffDays);
        if (diffDays > hdnMaxDeliveryDays.value) {
            //alert('Not OK');
            retValue = 0;
        }
        else if (diffDays <= hdnMaxDeliveryDays.value) {
            //alert('OK');
            retValue = 1;
        }
    }
    return retValue;
}

/*If Request Type is Machine Fuilure then Mandatory Expected Consuption Detials*/
function ddlRequestType_onchange(ddl) {
    var TrReq1 = document.getElementById('TrReq1');
    var TrReq2 = document.getElementById('TrReq2');

    var allinput1 = TrReq1.getElementsByTagName('input');
    var allinput2 = TrReq2.getElementsByTagName('input');
    var alltd1 = TrReq1.getElementsByTagName('td');
    var alltd2 = TrReq2.getElementsByTagName('td');
    var allselect1 = TrReq1.getElementsByTagName('select');
    var allselect2 = TrReq1.getElementsByTagName('select');
    var alltextarea2 = TrReq2.getElementsByTagName('textarea');

    var i = 0;
    for (i = 0; i < alltextarea2.length; i++) {
        alltextarea2[i].accessKey = "";
    }

    for (i = 0; i < alltd1.length; i++) {
        //alltd1[i].className = "";
        alltd1[i].innerHTML = alltd1[i].innerHTML.toString().replace('*', '');
        i = i + 1;
    }
    for (i = 0; i < allinput1.length; i++) {
        allinput1[i].accessKey = "";
    }
    for (i = 0; i < allselect1.length; i++) {
        allselect1[i].accessKey = "";
    }


    for (i = 0; i < alltd2.length; i++) {
        //alltd2[i].className = "";
        alltd2[i].innerHTML = alltd2[i].innerHTML.toString().replace('*', '');
        i = i + 1;
    }
    for (i = 0; i < allinput2.length; i++) {
        allinput2[i].accessKey = "";
    }
    for (i = 0; i < allselect2.length; i++) {
        allselect2[i].accessKey = "";
    }

    if (ddl.selectedIndex > 1) {
        for (i = 0; i < alltextarea2.length; i++) {
            alltextarea2[i].accessKey = "1";
        }

        for (i = 0; i < alltd1.length; i++) {
            //alltd1[i].className = "req";
            alltd1[i].innerHTML = alltd1[i].innerHTML.toString().replace(':', '* :');
            i = i + 1;
        }
        for (i = 0; i < allinput1.length; i++) {
            allinput1[i].accessKey = "1";
        }
        for (i = 0; i < allselect1.length; i++) {
            allselect1[i].accessKey = "1";
        }


        for (i = 0; i < alltd2.length; i++) {
            //alltd2[i].className = "req";
            alltd2[i].innerHTML = alltd2[i].innerHTML.toString().replace(':', '* :');
            i = i + 1;
        }
        for (i = 0; i < allinput2.length; i++) {
            allinput2[i].accessKey = "1";
        }
        for (i = 0; i < allselect2.length; i++) {
            allselect2[i].accessKey = "1";
        }
    }

}
/*End*/

/*Request Part List*/
function GetSelValue(sel) {
    alert("Hi");
}
function GetIndex(myDD, myHdnUMO, mySpnTotalQty, usrInputQty, Index, Price, TotPrice, TotalTaxPrice,rowTotalTax, moq,rowTaxPer,rowTT) {

    var getMyHdnField = document.getElementById(myHdnUMO);
    if (getMyHdnField != null) {

        var myUMOval = myDD.value;
        // alert(myUMOval);
        var hdnSelectedRowTax = document.getElementById('hdnSelectedRowTax');
        var rtt = hdnSelectedRowTax.value;
        var rttNew = rowTotalTax;

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

        // var getPrdPrice = document.getElementById(Price);
        if (hdnEnteredPrice.value == "") {
            var CalPrice = numTotalQty * Price;
        } else { var CalPrice = numTotalQty * EntPrice; }

        var ShowTotPrice = document.getElementById(TotPrice);
        ShowTotPrice.innerHTML = CalPrice;
        

        if (rttNew == '0.00') {
            var pertx = rowTaxPer;
            var taxPrice = CalPrice * pertx;
            var CaltaxPrice = Number(taxPrice) / 100;
            var rowTaxvalue = document.getElementById(rowTT);
            rowTaxvalue.innerHTML = CaltaxPrice;
            var PriceAfterTax = Number(CalPrice) + Number(CaltaxPrice);
            var ShowTotTaxPrice = document.getElementById(TotalTaxPrice);
            ShowTotTaxPrice.innerHTML = PriceAfterTax;
        } else {
            var taxPrice = CalPrice * rttNew;
            var CaltaxPrice = Number(taxPrice) / 100;
            var rowTaxvalue = document.getElementById(rowTT);
            rowTaxvalue.innerHTML = CaltaxPrice;
            var PriceAfterTax = Number(CalPrice) + Number(CaltaxPrice);
            var ShowTotTaxPrice = document.getElementById(TotalTaxPrice);
            ShowTotTaxPrice.innerHTML = PriceAfterTax;
        }

        //var PriceAfterTax = Number(CalPrice) + Number(rttNew);
        //var ShowTotTaxPrice = document.getElementById(TotalTaxPrice);
        //ShowTotTaxPrice.innerHTML = PriceAfterTax;

        //if (crntStock < numTotalQty) {
        //    getUserInputQtyObj.value = "0";
        //    getTotalQtyObj.innerHTML = "0";
        //    showAlert("Requested Quantity is greater than Current Stock...!!!", "Error", '#');
        //    //alert("Requested quantity is greater than.... samjun ghya!!!");
        //} else {
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
                order.AmountAfterTax = PriceAfterTax;
                PageMethods.WMUpdRequestPart(order, null, null);
                PageMethods.WMGetTotal(OnSuccessGetTotal, null);
            }
        } else if (moqv == 0) {
            var order = new Object();
            order.Sequence = Index + 1;
            order.RequestQty = numTotalQty;
            order.UOMID = myFilteredUMO; // myHdnUMO;  //myFilteredUnit;//  myFilteredUMO;
            order.Total = CalPrice;
            order.AmountAfterTax = PriceAfterTax;
            PageMethods.WMUpdRequestPart(order, null, null);
            PageMethods.WMGetTotal(OnSuccessGetTotal, null);
        }
        setCartTotalOnChange();
        // }

        // alert("My UMO: " + myFilteredUMO + "My Unit: " + myFilteredUnit + " My IndexNo: " + Index);
    }
}

function GetIndexQty(myDD, myHdnQty, myHdnUMO, mySpnTotalQty, usrInputQty, Index, Price, TotPrice, TotalTaxPrice, rowTotalTax, moq, rowTaxPer,rowTT) {
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

    var hdnSelectedRowTax = document.getElementById('hdnSelectedRowTax');
    var rtt = hdnSelectedRowTax.value;
    var rttNew = rowTotalTax;
    
   
    var getMyHdnField = myHdnQty;  //myHdnUMO;
    var myUMOval = myHdnUMO.value;
    var myFilteredUnit = 0;
    
    myFilteredUnit = myHdnQty;

    var getUserInputQtyObj = document.getElementById(usrInputQty);
    var getTotalQtyObj = document.getElementById(mySpnTotalQty);

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
            // var numTotalQty = hdnEnteredQty.value * Number(myHdnQty);
            getTotalQtyObj.innerHTML = numTotalQty;
        }
    }

    if (hdnEnteredPrice.value == "") {
        var CalPrice = numTotalQty * Price;
        var ShowTotPrice = document.getElementById(TotPrice);
        ShowTotPrice.innerHTML = CalPrice;
       
        if (rttNew == '0.00') {
            var pertx = rowTaxPer;
            var taxPrice = CalPrice * pertx; 
            var CaltaxPrice = Number(taxPrice) / 100;
            var rowTaxvalue = document.getElementById(rowTT);
            rowTaxvalue.innerHTML = CaltaxPrice;
            var PriceAfterTax = Number(CalPrice) + Number(CaltaxPrice);
            var ShowTotTaxPrice = document.getElementById(TotalTaxPrice);
            ShowTotTaxPrice.innerHTML = PriceAfterTax;
        } else {
            var taxPrice = CalPrice * rttNew;
            var CaltaxPrice = Number(taxPrice) / 100;
            var rowTaxvalue = document.getElementById(rowTT);
            rowTaxvalue.innerHTML = CaltaxPrice;
            var PriceAfterTax = Number(CalPrice) + Number(CaltaxPrice);
            var ShowTotTaxPrice = document.getElementById(TotalTaxPrice);
            ShowTotTaxPrice.innerHTML = PriceAfterTax;
        }
    } else {
        var CalPrice = numTotalQty * hdnEnteredPrice.value;
        var ShowTotPrice = document.getElementById(TotPrice);
        ShowTotPrice.innerHTML = CalPrice;
       
        if (rttNew == '0.00') {
            var pertx = rowTaxPer;
            var taxPrice = CalPrice * pertx;
            var CaltaxPrice = Number(taxPrice) / 100;
            var rowTaxvalue = document.getElementById(rowTT);
            rowTaxvalue.innerHTML = CaltaxPrice;
            var PriceAfterTax = Number(CalPrice) + Number(CaltaxPrice);
            var ShowTotTaxPrice = document.getElementById(TotalTaxPrice);
            ShowTotTaxPrice.innerHTML = PriceAfterTax;
        } else {
            var taxPrice = CalPrice * rttNew;
            var CaltaxPrice = Number(taxPrice) / 100;
            var rowTaxvalue = document.getElementById(rowTT);
            rowTaxvalue.innerHTML = CaltaxPrice;
            var PriceAfterTax = Number(CalPrice) + Number(CaltaxPrice);
            var ShowTotTaxPrice = document.getElementById(TotalTaxPrice);
            ShowTotTaxPrice.innerHTML = PriceAfterTax;
        }
    }

    //if (crntStock < numTotalQty) {
    //    getUserInputQtyObj.value = "0";
    //    getTotalQtyObj.innerHTML = "0";
    //    showAlert("Requested Quantity is greater than Current Stock...!!!", "Error", '#');
    //} else {
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
            order.AmountAfterTax = PriceAfterTax;
            PageMethods.WMUpdRequestPart(order, null, null);
            PageMethods.WMGetTotal(OnSuccessGetTotal, null);
        }
    } else if (moqv == 0) {
        var order = new Object();
        order.Sequence = Index + 1;
        order.RequestQty = numTotalQty;
        if (selUMOID == "") {
            order.UOMID = myHdnUMO;
        } else {
            order.UOMID = selUMOID; //myHdnUMO;  //myFilteredUnit;//  myFilteredUMO;
        }
        order.Total = CalPrice;
        order.AmountAfterTax = PriceAfterTax;
        PageMethods.WMUpdRequestPart(order, null, null);
        PageMethods.WMGetTotal(OnSuccessGetTotal, null);
    }
    setCartTotalOnChange();
    //}
}
function OnSuccessGetTotal(result) {
    var txtGrandTotal = document.getElementById("<%=txtGrandTotal.ClientID%>");
    txtGrandTotal.value = result;
    PageMethods.WMGetTotalQty(OnsuccessTotalQty, null);
}
function OnsuccessTotalQty(result) {
    var txtTotalQty = document.getElementById("<%=txtTotalQty.ClientID%>");
    txtTotalQty.value = result;
}

        function GetChangedPrice(myTxt, myHdnQty, myHdnUMO, mySpnTotalQty, usrInputQty, Index, TotPrice, TotalTaxPrice, rowTotalTax, ProdID, rowTaxPer,rowTT) {
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

    var hdnSelectedRowTax = document.getElementById('hdnSelectedRowTax');
    var rtt = hdnSelectedRowTax.value;
    var rttNew = rowTotalTax;

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

    if (rttNew == '0.00') {
        var pertx = rowTaxPer;
        var taxPrice = CalPrice * pertx;
        var CaltaxPrice = Number(taxPrice) / 100;
        var rowTaxvalue = document.getElementById(rowTT);
        rowTaxvalue.innerHTML = CaltaxPrice;
        var PriceAfterTax = Number(CalPrice) + Number(CaltaxPrice);
        var ShowTotTaxPrice = document.getElementById(TotalTaxPrice);
        ShowTotTaxPrice.innerHTML = PriceAfterTax;
    } else {
        var taxPrice = CalPrice * rttNew;
        var CaltaxPrice = Number(taxPrice) / 100;
        var rowTaxvalue = document.getElementById(rowTT);
        rowTaxvalue.innerHTML = CaltaxPrice;
        var PriceAfterTax = Number(CalPrice) + Number(CaltaxPrice);
        var ShowTotTaxPrice = document.getElementById(TotalTaxPrice);
        ShowTotTaxPrice.innerHTML = PriceAfterTax;
    }
   
    //var PriceAfterTax = Number(CalPrice) + Number(rttNew);
    //var ShowTotTaxPrice = document.getElementById(TotalTaxPrice);
    //ShowTotTaxPrice.innerHTML = PriceAfterTax;

    //if (crntStock < numTotalQty) {
    //    getUserInputQtyObj.value = "0";
    //    getTotalQtyObj.innerHTML = "0";
    //    showAlert("Requested Quantity is greater than Current Stock...!!!", "Error", '#');
    //} else {
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
    order.AmountAfterTax = PriceAfterTax;
    PageMethods.WMUpdRequestPartPrice(order, ProdID, null, null);
    PageMethods.WMGetTotal(OnSuccessGetTotal, null);
    // }
    setCartTotalOnChange();
}

function getselectedUOM(dropdown, datafield, rowIndex) {
    var ddlvalue = dropdown.value;
    if (ddlvalue == "") ddlvalue = 0;
    if (Grid1.Rows[rowIndex].Cells[datafield].Value != ddlvalue.value) {
        Grid1.Rows[rowIndex].Cells[datafield].Value = ddlvalue.value;
        PageMethods.WmUpdateRequestPartUOM(getPartUOM(rowIndex), null, null);
    }
}

function getPartUOM(rowIndex) {
    /*Save Request Part UOM into TempData when changed*/
    var order = new Object();
    order.Sequence = Grid1.Rows[rowIndex].Cells['Sequence'].Value;
    order.UOMID = Grid1.Rows[rowIndex].Cells['UOMID'].Value;
    return order;
}

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

function removePartFromListOnSussess(result) {
    if (result == 0) {
        showAlert("Not Applicable.......", "error", "#");
    } else {
        Grid1.refresh();
    }
}
/*End Request Part List*/

/*Exp*/

/*Get Engine Details when Select Engine*/
function jsGetEngineDetails(ddl) {
    if (ddl.selectedIndex > 0) {
        PageMethods.WMGetEngineDetails(ddl.options[ddl.selectedIndex].value, jsGetEngineDetailsOnSussess, null);
    }
    else {
        //  lblEngineModel.innerHTML = "";
        //  lblEngineSerial.innerHTML = "";
    }
}

function jsGetEngineDetailsOnSussess(result) {
    //  lblEngineModel.innerHTML = result.EngineModel;
    //  lblEngineSerial.innerHTML = result.EngineSerial;

}
/*End*/


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
    ddlLoadingOff(ddlS);
    divVisibility();

}

function jsFillStatusListOnFailed() {
    ddlLoadingOff(ddlStatus);
}
function jsFillUsersList() {

    ddlRequestByUserID.options.length = 0;
    if (ddlSites.selectedIndex > 0) {
        ddlLoadingOn(ddlRequestByUserID);
        PageMethods.WMFillUserList(ddlSites.options[ddlSites.selectedIndex].value, jsFillUsersListOnSuccess, jsFillUsersListOnFail);
    }
}

function jsFillUsersListOnSuccess(result) {
    var ddlR = ddlRequestByUserID;

    for (var i = 0; i < result.length; i++) {
        var option1 = document.createElement("option");
        option1.text = result[i].userName;
        option1.value = result[i].userID;
        try {
            ddlR.add(option1, null); //Standard 
        } catch (error) {
            ddlR.add(option1); // IE only
        }
    }
    ddlLoadingOff(ddlR);
}

function jsFillUsersListOnFail() {
    ddlLoadingOff(ddlRequestByUserID);
}


function jsFillEnginList() {
    // ddlContainer.options.length = 0;
    if (ddlSites.selectedIndex > 0) {
        //   ddlLoadingOn(ddlContainer);
        PageMethods.WMFillEnginList(ddlSites.options[ddlSites.selectedIndex].value, jsFillEnginListOnSuccess, jsFillEnginListOnFail);
    }
}
function jsFillEnginListOnSuccess(result) {
    // var ddlEng = ddlContainer;

    for (var i = 0; i < result.length; i++) {
        var optionE1 = document.createElement("option");
        optionE1.text = result[i].Container;
        optionE1.value = result[i].ID;
        try {
            ddlEng.add(optionE1, null); //Standard 
        } catch (error) {
            ddlEng.add(optionE1); // IE only
        }
    }
    ddlLoadingOff(ddlEng);
}

function jsFillEnginListOnFail() {
    // ddlLoadingOff(ddlContainer);
}

/*End*/

function PrintAddress() {
    var ddlAddress = document.getElementById("<%=ddlAddress.ClientID %>");
    var hdnSelAddress = document.getElementById('hdnSelAddress');
    hdnSelAddress.value = ddlAddress.value;
    var Adrs = ddlAddress.options[ddlAddress.selectedIndex].text;

    var lblAddress = document.getElementById("<%=lblAddress.ClientID %>");
    lblAddress.innerHTML = Adrs;
}

function SubmitTemplate() {
    //alert('Submit Template');
    if (Grid1.Rows.length == 0) {
        showAlert("Add atleast one part into the Request Part List", "error", "#");
    }
    else {
        if (txtTemplateTitleNew.value == "") {
            showAlert("Enter Template Title", "error", "#");
        }
        else if (ddlAccessTypeNew.options[ddlAccessTypeNew.selectedIndex].value.toString() == "0") {
            showAlert("Select Access Type", "error", "#");
        }
        else {
            var obj1 = new Object();
            obj1.TemplateTitle = txtTemplateTitleNew.value.toString();
            obj1.Accesstype = ddlAccessTypeNew.options[ddlAccessTypeNew.selectedIndex].value.toString();
            obj1.StoreId = parseInt(ddlSites.options[ddlSites.selectedIndex].value);
            obj1.Remark = txtRemark.value.toString();
            if (ddlStatus.selectedIndex == 1) { obj1.IsSubmit = "false"; }
            else { obj1.IsSubmit = "true"; }
            PageMethods.WMSaveTemplateHead(obj1, WMSaveTemplateHead_onSuccessed, WMSaveTemplateHead_onFailed);
        }
    }
}

function WMSaveTemplateHead_onSuccessed(result) {
    //LoadingOn(false);
    if (result == "Some error occurred" || result == "") {
        showAlert("Error occurred", "Error", "#");
    }
    else if (result == "Title Already Available") {
        showAlert("Template With This Title Already Available", "Error", "#");
        txtTemplateTitleNew.focus();
    }
    else if (result == "Template Saved Successfully") {
        showAlert(result, "info", "#");
        txtTemplateTitleNew.value = "";
        ddlAccessTypeNew.selectedIndex = 0;
    }
}
function WMSaveTemplateHead_onFailed() { showAlert("Error occurred", "Error", "#"); }

    </script>
    <script src="//code.jquery.com/jquery-1.10.2.js"></script>
    <script>
        $(function () {
            //loop the studentID dropdownlist in gridview
            $(".ddlUOM").each(function () {
                //get every studentID dropdownlist selected index and selected value
                alert($(this)[0].selectedIndex);
                //get every studentID dropdownlist selected value
                alert($(this).val());
            });
        })
    </script>
    <script type="text/javascript">
        /*sections[Collapsable] code*/

        function divVisibility() {
            divApproval(false);
            divIssue(false);
            divReceipt(false);
            divConsumption(false);
            for (var i = 0; i < ddlStatus.options.length; i++) {
                if (ddlStatus.options[i].text == 'Approved') { divApproval(true); }
                else if (ddlStatus.options[i].text == 'Issued') { divIssue(true); }
                else if (ddlStatus.options[i].text == 'Received') { divReceipt(true); }
                else if (ddlStatus.options[i].text == 'Consumed') { divConsumption(true); }
            }
        }

        function divApproval(val) {
            if (val == true) {

                linkApprovalDetail.innerHTML = "Expand";
                divApprovalDetail.className = "divDetailCollapse"
            }
            else if (val == false) {
                document.getElementById('divApprovalHead').style.display = "none";
                document.getElementById('divApprovalDetail').style.display = "none";
            }
        }

        function divIssue(val) {
            if (val == true) {
                document.getElementById('divIssueHead').style.display = "";
                document.getElementById('divIssueDetail').style.display = "";
            }
            else if (val == false) {
                document.getElementById('divIssueHead').style.display = "none";
                document.getElementById('divIssueDetail').style.display = "none";
            }
        }
        function divReceipt(val) {
            if (val == true) {
                document.getElementById('divReceiptHead').style.display = "";
                document.getElementById('divReceiptDetail').style.display = "";
            }
            else if (val == false) {
                document.getElementById('divReceiptHead').style.display = "none";
                document.getElementById('divReceiptDetail').style.display = "none";
            }
        }
        function divConsumption(val) {
            if (val == true) {
                document.getElementById('divConsumptionHead').style.display = "";
                document.getElementById('divConsumptionDetail').style.display = "";
            }
            else if (val == false) {
                document.getElementById('divConsumptionHead').style.display = "none";
                document.getElementById('divConsumptionDetail').style.display = "none";
            }
        }

        function OpenTelplateList() {
            window.open('../PowerOnRent/TemplateList.aspx', null, 'height=380px, width=810px,status= 0, resizable= 1, scrollbars=1, toolbar=0,location=center,menubar=0, screenX=300; screenY=50');
        }
        function GetVendor() {
            var ddlCompany = document.getElementById("<%=ddlCompany.ClientID %>");
            var hdnSelectedVendor = document.getElementById('hdnSelectedVendor');
            hdnSelectedVendor.value = ddlCompany.value;
            var Vndr = ddlCompany.value;
            PageMethods.WMGetVendorSession(Vndr);
        }

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

        function GetContact1() {
            ddlSites = document.getElementById("<%=ddlSites.ClientID %>");
            ddlCompany = document.getElementById("<%=ddlCompany.ClientID %>");
            var hdnselectedDept = document.getElementById('hdnselectedDept');
            hdnselectedDept.value = ddlSites.value;

            var Dept = ddlSites.value;
            var Company = ddlCompany.value;
            //PageMethods.WMGetContactPersonLst(Dept, OnSuccessContactPerson, null);
            PageMethods.WMGetContactPersonLst(Company, OnSuccessContactPerson, null);
        }
        function OnSuccessContactPerson(result) {
            ddlContact1 = document.getElementById("<%=ddlContact1.ClientID %>");
            ddlContact1.options.length = 0;
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
                ddlContact1.add(option0, null);
            }
            catch (Error) {
                ddlContact1.add(option0);
            }

            for (var i = 0; i < result.length; i++) {
                var option1 = document.createElement("option");
                option1.text = result[i].Name;
                option1.value = result[i].ID;
                try {
                    ddlContact1.add(option1, null);
                }
                catch (Error) {
                    ddlContact1.add(option1);
                }
            }
        }


        function GetDeptID() {
            ddlSites = document.getElementById("<%=ddlSites.ClientID %>");
            var Warehouse = ddlSites.value; //var Dept = ddlSites.value;
            var hdnSelectedWarehouse = document.getElementById('hdnSelectedWarehouse');
            hdnSelectedWarehouse.value = ddlSites.value;
            PageMethods.WMGetWarehouseSession(Warehouse);

            //var hdnselectedDept = document.getElementById('hdnselectedDept');
            //hdnselectedDept.value = ddlSites.value;
            //PageMethods.WMGetDepartmentSession(Dept);
            //var hdnMaxDeliveryDays = document.getElementById('hdnMaxDeliveryDays');
            //PageMethods.WMGetMaxDeliveryDays(Dept, OnsuccessmaxDeliveryDays, null);
        }
        function OnsuccessmaxDeliveryDays(result) {
            var hdnMaxDeliveryDays = document.getElementById('hdnMaxDeliveryDays');
            hdnMaxDeliveryDays.value = result;
            __doPostBack('<%=UpdExpDDate.ClientID %>', '');
        }

        function CheckParts() {
            ddlSites = document.getElementById("<%=ddlSites.ClientID %>");
            var Dept = ddlSites.value;
            var hdnChngDept = document.getElementById("<%=hdnChngDept.ClientID%>");
            if (Grid1.Rows.length == 0) { }
            else {
                showAlert("Order can be processed only for one department per request. Change in department selection will remove the Sku selected.", "Error");
                //    var r = confirm("Order can be processed only for one department per request. Change in department selection will remove the Sku selected. Do you want to change department?")
                //    if (r == true) {                   
                hdnChngDept.value = "0x00x0";
                Grid1.refresh();
                hdnChngDept.value = "1x1";
                //    } else {
                //       //ddlSites.disabled = true;
                //    }
            }
        }
        function AnotherFunction() {
            ddlSites = document.getElementById("<%=ddlSites.ClientID %>");
            var Dept = ddlSites.value;
            ddlSites.disabled = true;
        }
        function GetAddress() {
            ddlSites = document.getElementById("<%=ddlSites.ClientID %>");
            var hdnSelectedWarehouse = document.getElementById('hdnSelectedWarehouse');
            hdnSelectedWarehouse.value = ddlSites.value;

            PageMethods.WMGetWarehouseAddress(hdnSelectedWarehouse.value, OnSuccessWarehouseAddress, null);
            //PageMethods.WMGetDeptAddress(hdnSelectedWarehouse.value, OnSuccessDeptAddress, null);
        }

        function OnSuccessWarehouseAddress(result) {
            var ddlAddress = document.getElementById("<%=ddlAddress.ClientID %>");
            ddlAddress.options.length = 0;
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
                ddlAddress.add(option0, null);
            }
            catch (Error) {
                ddlAddress.add(option0);
            }

            for (var i = 0; i < result.length; i++) {
                var option1 = document.createElement("option");
                option1.text = result[i].AddressLine1;
                option1.value = result[i].ID;
                try {
                    ddlAddress.add(option1, null);
                }
                catch (Error) {
                    ddlAddress.add(option1);
                }
            }
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

        function RequestOpenEntryForm(invoker, state, AprvlID, DeligateID, requestID, ApprovalId) {
            if (state != "gray") {
                if (state == "red") {
                    var DelID = DeligateID;
                    if (DelID == "") DelID = "0";
                    var hdnApprovalId = document.getElementById("hdnApprovalId");
                    hdnApprovalId.value = ApprovalId;
                    PageMethods.WMGetApproverForApprove(AprvlID, requestID, DelID, OnSuccessApproverForApprove, null);
                    ////window.open('../PowerOnRent/Approval.aspx?APID='+ AprvlID +'&REQID='+ requestID +'&ST=3', null, 'height=180px, width=595px,status= 0, resizable= 1, scrollbars=1, toolbar=0,location=center,menubar=0, screenX=300; screenY=50');
                    // window.open('../PowerOnRent/Approval.aspx?REQID=' + requestID + '&ST=3', null, 'height=180px, width=595px,status= 0, resizable= 1, scrollbars=1, toolbar=0,location=center,menubar=0, screenX=300; screenY=50');
                }
                else {
                    showAlert("Not Applicable", '', '#');
                }
            }
            else {
                showAlert("Not Applicable", '', '#');
            }
        }
        function OnSuccessApproverForApprove(result) {
            if (result == "AccessDenied") {
                showAlert("Access Denied", '', '#');
            } else {
                var hdnApprovalId = document.getElementById("hdnApprovalId");
                window.open('../PowerOnRent/Approval.aspx?REQID=' + result + '&ST=3&APL=' + hdnApprovalId.value + '', null, 'height=180px, width=595px,status= 0, resizable= 1, scrollbars=1, toolbar=0,location=center,menubar=0, screenX=300; screenY=50');
                // window.open('../PowerOnRent/Approval.aspx?REQID=' + requestID + '&ST=3', null, 'height=180px, width=595px,status= 0, resizable= 1, scrollbars=1, toolbar=0,location=center,menubar=0, screenX=300; screenY=50');
            }
        }

        function RequestOpenEntryFormReject(invoker, state, AprvlID, requestID) {
            if (state != "gray") {
                if (state == "red") {
                    PageMethods.WMGetApproverForReject(AprvlID, requestID, OnSuccessApproverForReject, null);
                    //window.open('../PowerOnRent/Approval.aspx?REQID=' + requestID + '&ST=4', null, 'height=180px, width=595px,status= 0, resizable= 1, scrollbars=1, toolbar=0,location=center,menubar=0, screenX=300; screenY=50');
                }
                else {
                    showAlert("Not Applicable", '', '#');
                }
            }
            else {
                showAlert("Not Applicable", '', '#');
            }
        }
        function OnSuccessApproverForReject(result) {
            if (result == "AccessDenied") {
                showAlert("Access Denied", '', '#');
            } else {
                window.open('../PowerOnRent/Approval.aspx?REQID=' + result + '&ST=4&APL=0', null, 'height=180px, width=595px,status= 0, resizable= 1, scrollbars=1, toolbar=0,location=center,menubar=0, screenX=300; screenY=50');
                // window.open('../PowerOnRent/Approval.aspx?REQID=' + requestID + '&ST=4', null, 'height=180px, width=595px,status= 0, resizable= 1, scrollbars=1, toolbar=0,location=center,menubar=0, screenX=300; screenY=50');
            }
        }
        function RequestOpenEntryAppWithRevision(state, AprvlID, requestID) {
            if (state != "gray") {
                if (state == "red") {
                    showAlert("OMS enabled request # " + requestID + " for editing. Please edit the request for your revision & save for Approval with revision.", '', '#');
                    jsEditData();
                }
                else {
                    showAlert("Not Applicable", '', '#');
                }
            }
            else {
                showAlert("Not Applicable", '', '#');
            }

        }

        function jsEditData() {
            // var txtTitle = document.getElementById("<%= txtTitle.ClientID %>");
            changemode(false, 'divRequestDetail');
            //document.getElementById(txtTitle).disabled = true;
        }

        function openContactSearch(sequence) {
            window.open('../PowerOnRent/AddEditSearchContact.aspx?inv=Warehouse&Con=1', null, 'height=500px, width=825px,status= 0, resizable= 1, scrollbars=1, toolbar=0,location=center,menubar=0, screenX=300; screenY=50');
        }
        function openContactSearch2(sequence) {
            window.open('../PowerOnRent/AddEditSearchContact.aspx?Con=2', null, 'height=500px, width=825px,status= 0, resizable= 1, scrollbars=1, toolbar=0,location=center,menubar=0, screenX=300; screenY=50');
        }
        function openAddressSearch(sequence) {
            window.open('../PowerOnRent/AddEditSearchAddress.aspx?inv=Warehouse', null, 'height=500px, width=825px,status= 0, resizable= 1, scrollbars=1, toolbar=0,location=center,menubar=0, screenX=300; screenY=50');
        }
        function openLocationSearch(sequence) {
            window.open('../PowerOnRent/GetLocation.aspx', null, 'height=500px, width=825px,status= 0, resizable= 1, scrollbars=1, toolbar=0,location=center,menubar=0, screenX=300; screenY=50');
        }
        function AfterContact1Selected(ConID, ConName) {
            var Con1ID = document.getElementById("hdnSearchContactID1");
            var Con1NM = document.getElementById("hdnSearchContactName1");
            Con1ID.value = ConID;
            Con1NM.value = ConName;
            var txtContact1 = document.getElementById("<%= txtContact1.ClientID %>");
            txtContact1.value = ConName;
        }
        function AfterContact2Selected(ConID, ConName) {
            var Con2ID = document.getElementById("hdnSearchContactID2");
            var Con2NM = document.getElementById("hdnSearchContactName2");
            Con2ID.value = ConID;
            Con2NM.value = ConName;

        }

        function AfterAddressselected(AdrID, AdrName) {
            var AdrsID = document.getElementById("hdnSearchAddressID");
            var AdrsNm = document.getElementById("hdnSearchAddress");
            AdrsID.value = AdrID;
            AdrsNm.value = AdrName;
            var txtAddress = document.getElementById("<%= txtAddress.ClientID %>");
            var lblAddress = document.getElementById("<%= lblAddress.ClientID %>");
            txtAddress.value = AdrName;
            lblAddress.innerHTML = AdrName;
        }

        function AfterLocationSelected(LocID, LocName, LocConID, LocConNM, LocationName) {
            var LocationID = document.getElementById("hdnSearchLocationID");
            var LocationName = document.getElementById("hdnSearchLocation");
            var SearchedLocationName = document.getElementById("hdnSearchLocationName");

            LocationID.value = LocID;
            LocationName.value = LocName;
            SearchedLocationName.value = LocationName;


            var LocContactID = document.getElementById("hdnLocConID");
            var LocConName = document.getElementById("hdnLocConName");
            LocContactID.value = LocConID;
            LocConName.value = LocConNM;
            var txtContact1 = document.getElementById("<%= txtContact1.ClientID %>");
            txtContact1.value = LocConNM;
            var Con1ID = document.getElementById("hdnSearchContactID1");
            Con1ID.value = LocConID;
        }

        function divPMChng() {
            var hdnPmethodChng = document.getElementById("hdnPmethodChng");
            hdnPmethodChng.value = "1";
        }

        function GetPaymentMethodID() {
            var hdnSelPaymentMethod = document.getElementById("hdnSelPaymentMethod");
            $selval = $('#ddlPaymentMethod').val();
            hdnSelPaymentMethod.value = $selval;
            if ($selval == 5) {
                $('#dvFOC').removeClass('active');
                $('#dvLst').addClass('active');
            }
            else {
                __doPostBack('<%=UpdLst.ClientID %>', '');
                $('#dvLst').removeClass('active');
                $('#dvFOC').addClass('active');
            }
        }

        function PaymentMethod() {
            ddlSites = document.getElementById("<%=ddlSites.ClientID %>");
            var Dept = ddlSites.value;
            var hdnselectedDept = document.getElementById('hdnselectedDept');
            hdnselectedDept.value = ddlSites.value;
            PageMethods.WMGetPaymentMethod(Dept, onSuccessGetPaymentMEthod, null);
        }
        function onSuccessGetPaymentMEthod(result) {
            var ddlPaymentMethod = document.getElementById("<%=ddlPaymentMethod.ClientID %>");
            ddlPaymentMethod.options.length = 0;
            var option0 = document.createElement("option");

            if (result.length > 0) {
                //option0.text = '--Select--';
                //option0.value = '0';               
            }
            else {
                option0.text = 'N/A';
                option0.value = '0';
            }

            //try {
            //    ddlPaymentMethod.add(option0, null);
            //}
            //catch (Error) {
            //    ddlPaymentMethod.add(option0);
            //}

            for (var i = 0; i < result.length; i++) {
                var option1 = document.createElement("option");
                option1.text = result[i].MethodName;
                option1.value = result[i].PMethodID;
                try {
                    ddlPaymentMethod.add(option1, null);
                }
                catch (Error) {
                    ddlPaymentMethod.add(option1);
                }
            }
            ddlSites = document.getElementById("<%=ddlSites.ClientID %>");
            var Dept = ddlSites.value;
            var hdnselectedDept = document.getElementById('hdnselectedDept');
            hdnselectedDept.value = ddlSites.value;
            PageMethods.WMGetCostCenter(Dept, onSuccessGetCostCenter, null);
        }
        function onSuccessGetCostCenter(result) {
            var ddlFOC = document.getElementById("<%=ddlFOC.ClientID %>");
            ddlFOC.options.length = 0;
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
                ddlFOC.add(option0, null);
            }
            catch (Error) {
                ddlFOC.add(option0);
            }

            for (var i = 0; i < result.length; i++) {
                var option1 = document.createElement("option");
                option1.text = result[i].CenterName;
                option1.value = result[i].ID;
                try {
                    ddlFOC.add(option1, null);
                }
                catch (Error) {
                    ddlFOC.add(option1);
                }
            }
        }

        function openEditProduct(Seq) {
            var hdnSelectedSequenceNo = document.getElementById('hdnSelectedSequenceNo');
            hdnSelectedSequenceNo.value = Seq;
            var ddlStatus = document.getElementById("<%= ddlStatus.ClientID %>");
            var Status = ddlStatus.value;
            if (Status == 2 || Status == 21 || Status == 22) {
                PageMethods.WMGetAccessOfProductChange(Seq, OnsuccessGetAccessProductChange, null);
            } else {
                showAlert("Not Applicable", '', '#');
            }
        }
        function OnsuccessGetAccessProductChange(result) {
            if (result == 0) {
                showAlert("Not Applicable", '', '#');
            } else {
                window.open('../PowerOnRent/ChangeOrderProduct.aspx', null, 'height=300px, width=925px,status= 0, resizable= 1, scrollbars=1, toolbar=0,location=center,menubar=0, screenX=300; screenY=50');
            }
        }

        function AfterProductQtyChange(PrdQtyChng) {
            PageMethods.WMGetTotalQtyGrandTotal(OnsuccessTotalQtyGrandTotal, null);
        }
        function OnsuccessTotalQtyGrandTotal(result) {
            var txtTotalQty = document.getElementById("<%= txtTotalQty.ClientID %>");
            var txtGrandTotal = document.getElementById("<%= txtGrandTotal.ClientID %>");
            txtTotalQty.value = result[0].TotalQty;
            txtGrandTotal.value = result[0].GrandTotal;
            Grid1.refresh();
        }
        function disableExpDeliveryDate() {
            $('#UC_ExpDeliveryDate').attr("disabled", "disabled");
        }

        function taxFlyoutOnOpen(invoker, cartsequence) {
            var hdnSelectedSequenceNo = document.getElementById('hdnSelectedSequenceNo');
            hdnSelectedSequenceNo.value = cartsequence;
            PageMethods.WMGetTotalForTax(cartsequence, OnsuccessGetTotalFortax, null);
        }
        function OnsuccessGetTotalFortax(result) {
            var hdnSelectedSequenceNo = document.getElementById('hdnSelectedSequenceNo');
            var cartsequence = hdnSelectedSequenceNo.value;
            var myresult= result.split(",");
            if (cartsequence != 0) {
                var TaxableAmount = myresult[0];
                var PrdID = myresult[1];
                window.open('../Tax/ApplyTax.aspx?CartSeq=' + cartsequence + '&Object=PurchaseOrderTax&TaxableAmt=' + TaxableAmount + '&PrdID=' + PrdID + '', null, 'height=700px, width=660px,status= 0, resizable= 0, scrollbars=0, toolbar=0,location=center,menubar=0, screenX=300; screenY=50');
            }
            if (cartsequence == 0) {
                var lblSubTotal2 = document.getElementById("<%= lblSubTotal2.ClientID %>");
                TaxableAmount = parseFloat(lblSubTotal2.innerHTML).toFixed(2);
                window.open('../Tax/ApplyTax.aspx?CartSeq=' + cartsequence + '&Object=PurchaseOrderTotalTax&TaxableAmt=' + TaxableAmount + '&PrdID=' + PrdID + '', null, 'height=700px, width=660px,status= 0, resizable= 0, scrollbars=0, toolbar=0,location=center,menubar=0, screenX=300; screenY=50');
            }

            //var TaxableAmount=result;
            //if (cartsequence != 0) TaxableAmount = Grid1.Rows[parseInt(invoker.title)].Cells['Total'].Value;
           // if (cartsequence != 0) TaxableAmount = Grid1.Rows[parseInt(invoker.title)].Cells[12].Value;
           // if (cartsequence == 0) {
           //     var lblSubTotal2 = document.getElementById("<%= lblSubTotal2.ClientID %>");
          //      TaxableAmount = parseFloat(lblSubTotal2.innerHTML).toFixed(2);
           // }
           // var hdnObjectName_New = document.getElementById("<%= hdnCartCurrentObjectName.ClientID  %>");
            //document.getElementById("hdnIsTaxAmountChange").value = "true";
            //window.open('../Tax/ApplyTax.aspx?CartSeq=' + cartsequence + '&Object=PurchaseOrderTotalTax&TaxableAmt=' + TaxableAmount + '&PrdID=' + PrdID + '', null, 'height=700px, width=660px,status= 0, resizable= 0, scrollbars=0, toolbar=0,location=center,menubar=0, screenX=300; screenY=50');
            //GridTaxList.refresh();
        }

        function setCartTotalOnChange() {
            var txtDiscountOnSubTotal = document.getElementById("<%= txtDiscountOnSubTotal.ClientID %>");

            if (txtDiscountOnSubTotal.value == "") txtDiscountOnSubTotal.value = "0.00";

            txtDiscountOnSubTotal.value = parseFloat(txtDiscountOnSubTotal.value).toFixed(2);
            var chkboxDiscountOnSubTotal = document.getElementById("<%= chkboxDiscountOnSubTotal.ClientID %>");

            PageMethods.webGetFooterTotal(parseFloat(txtDiscountOnSubTotal.value), chkboxDiscountOnSubTotal.checked.toString(), setCartTotalOnSuccess, setCartTotalOnFail);
        }

        function setCartTotalOnFail() { alert("Error occurred"); }

        function setCartTotalOnSuccess(result) {
            document.getElementById("<%= hdnCartSubTotal.ClientID %>").value = parseFloat(result[0]).toFixed(2);
            document.getElementById("<%= lblSubTotal.ClientID %>").innerHTML = parseFloat(result[0]).toFixed(2);

            document.getElementById("<%= hdnCartDiscountOnSubTotal.ClientID %>").value = parseFloat(result[1]).toFixed(2);
            document.getElementById("<%= lblDiscountOnSubTotal.ClientID %>").innerHTML = parseFloat(result[1]).toFixed(2);

            document.getElementById("<%= hdnCartSubTotal2.ClientID %>").value = parseFloat(result[2]).toFixed(2);
            document.getElementById("<%= lblSubTotal2.ClientID %>").innerHTML = parseFloat(result[2]).toFixed(2);

            document.getElementById("<%= hdnCartTaxOnSubTotal.ClientID %>").value = parseFloat(result[3]).toFixed(2);
            document.getElementById("<%= lblTaxOnSubTotal.ClientID %>").innerHTML = parseFloat(result[3]).toFixed(2);

            var txtAdditionalCharges = document.getElementById("<%= txtAdditionalCharges.ClientID %>");
            if (txtAdditionalCharges.value == "") txtAdditionalCharges.value = "0.00";

            var txtShippingCharges = document.getElementById("<%= txtShippingCharges.ClientID %>");
            if (txtShippingCharges.value == "") txtShippingCharges.value = "0.00";

            var GrandTotal = parseFloat(result[2]) + parseFloat(result[3]) + parseFloat(txtAdditionalCharges.value) + parseFloat(txtShippingCharges.value);

            document.getElementById("hdnCartGrandTotal").value = parseFloat(GrandTotal).toFixed(2);
            document.getElementById("<%= lblGrandTotal.ClientID %>").innerHTML = parseFloat(GrandTotal).toFixed(2);

            //Grid1.refresh();
        var hdnItemCount = document.getElementById("hdnItemCount");
        var hdnCartCount = document.getElementById("hdnCartCount");
        if (hdnItemCount != null) {
            hdnItemCount.value = Grid1.Rows.length;
        }
        if (hdnCartCount != null) {
            hdnCartCount.value = Grid1.Rows.length;
        }


    }

    function setCartTotalOnChange2() {

        var lblSubTotal2 = document.getElementById("<%= lblSubTotal2.ClientID %>");
        var lblTaxOnSubTotal = document.getElementById("<%= lblTaxOnSubTotal.ClientID %>");
        var txtAdditionalCharges = document.getElementById("<%= txtAdditionalCharges.ClientID %>");
        if (txtAdditionalCharges.value == "") txtAdditionalCharges.value = "0.00";
        txtAdditionalCharges.value = parseFloat(txtAdditionalCharges.value).toFixed(2);

        var txtShippingCharges = document.getElementById("<%= txtShippingCharges.ClientID %>");
        if (txtShippingCharges.value == "") txtShippingCharges.value = "0.00";
        txtShippingCharges.value = parseFloat(txtShippingCharges.value).toFixed(2);

        var hdnCartGrandTotal = document.getElementById("hdnCartGrandTotal");
        var lblGrandTotal = document.getElementById("<%= lblGrandTotal.ClientID %>");
            lblGrandTotal.innerHTML = parseFloat(parseFloat(lblSubTotal2.innerHTML) + parseFloat(lblTaxOnSubTotal.innerHTML) + parseFloat(txtAdditionalCharges.value) + parseFloat(txtShippingCharges.value)).toFixed(2);
            hdnCartGrandTotal.value = lblGrandTotal.innerHTML;
    }

        function AddNewASN() {
            window.open('../WMS/AsnImport.aspx', '_self', '');
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

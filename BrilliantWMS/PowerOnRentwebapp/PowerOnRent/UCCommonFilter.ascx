<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCCommonFilter.ascx.cs"
    Inherits="BrilliantWMS.PowerOnRent.UCCommonFilter" %>
<%@ Register Src="../CommonControls/UC_Date.ascx" TagName="UC_Date" TagPrefix="ucd55" %>
<%@ Register Assembly="obout_Grid_NET" Namespace="Obout.Grid" TagPrefix="obout" %>
<div>
    <center>
        <table>
            <tr>
                <td>
                    <div>                        
                        <table>                         
                            <tr>
                                <td>                                                                     
                                    <div id="FDate" runat="server">
                                        <asp:Label ID="lblFromDate" runat="server" Text="From Date"></asp:Label>
                                        :
                                        <%-- From Date--%>
                                        <ucd55:UC_Date ID="FrmDate" runat="server"/>
                                    </div>
                                </td>
                                <td>
                                    <div id="TDate" runat="server">
                                        <asp:Label ID="lblToDate" runat="server" Text="To Date"></asp:Label>
                                        :
                                        <%-- To Date--%>
                                        <ucd55:UC_Date ID="To_Date" runat="server" />
                                    </div>
                                </td>
                                <td>
                                    <div id="PrdCategory" runat="server">
                                        <asp:Label ID="lblproductcategory" runat="server" Text="Product Category"></asp:Label>
                                        :
                                        <%-- Product Category--%>
                                        <asp:DropDownList ID="ddlCategory" runat="server" DataValueField="ID" DataTextField="ProductCategory"
                                            Width="200px" ClientIDMode="Static" onchange="div2(this);">
                                        </asp:DropDownList>
                                    </div>
                                </td>
                                <td>
                                    <div id="SiteList" runat="server">
                                        <asp:Label ID="lblSiteList" runat="server" Text="  Site List"></asp:Label>
                                        :
                                        <%--  Site List--%>
                                        <asp:DropDownList ID="ddlSite" runat="server" DataValueField="ID" DataTextField="Territory"
                                            Width="200px" ClientIDMode="Static" onchange="div1(this);">
                                        </asp:DropDownList>
                                    </div>
                                </td>
                                <td>
                                    <div id="ExcludeZero" runat="server">
                                        <input type="checkbox" id="chkExcludeZero" onclick="ExcludeZero(this);" />
                                        <a>Exclude Zero Available Balance</a>
                                    </div>
                                </td>
                                <td>
                                    <div id="frmSite" runat="server">
                                        <asp:Label ID="lblFromSite" runat="server" Text="From Site"></asp:Label>
                                        <%-- From Site--%>
                                        <asp:DropDownList ID="ddlFrmSite" runat="server" DataValueField="ID" DataTextField="Territory"
                                            Width="200px" ClientIDMode="Static" onchange="divTrAnsfer(this);">
                                        </asp:DropDownList>
                                    </div>
                                </td>
                                <td>
                                    <div id="toSite" runat="server">
                                        <asp:Label ID="lblToSite" runat="server" Text="To Site"></asp:Label>
                                        <%--  To Site--%>
                                        <asp:DropDownList ID="ddlToSite" runat="server" Width="200px" ClientIDMode="Static">
                                        </asp:DropDownList>
                                    </div>
                                </td>
                                <td>
                                    <div id="Company" runat="server">
                                        <asp:Label ID="lblcompany" runat="server" Text="Company"></asp:Label>
                                        :
                                        <%-- Company :--%>
                                        <asp:DropDownList ID="ddlcompany" runat="server" Width="150px" ClientIDMode="Static"
                                            DataValueField="ID" DataTextField="Name" onchange="GetCompany(this);BindDepartment()">
                                        </asp:DropDownList>
                                    </div>
                                </td>
                                <td>
                                    <div id="Department" runat="server">
                                        <asp:Label ID="lbldept" runat="server" Text="Department"></asp:Label>
                                        :
                                        <%--   Department :--%>
                                        <asp:DropDownList ID="ddldepartment" runat="server" Width="150px" ClientIDMode="Static"
                                            DataValueField="ID" DataTextField="Territory" onchange="GetDepartment(this);BindUser(this);PaymentMethod()" >
                                        </asp:DropDownList>
                                    </div>
                                </td>
                               <td>
                                    <div id="Groupset1" runat="server">
                                        <asp:Label ID="lblGroupSet" runat="server" Text="BOM"></asp:Label>
                                        :
                                        <asp:DropDownList ID="ddlgset" runat="server" Width="150px" ClientIDMode="Static"
                                            onchange="GetGroupSet(this)">
                                            <asp:ListItem Text="-Select All-" Value="0"></asp:ListItem>
                                            <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
                                            <asp:ListItem Text="No" Value="No"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </td>
                                <td>
                                    <div id="Image" runat="server">
                                        <asp:Label ID="lblimage" runat="server" Text="Image"></asp:Label>
                                        :
                                        <%--  Image :--%>
                                        <asp:DropDownList ID="ddlImage" runat="server" Width="150px" ClientIDMode="Static"
                                            onchange="GetImage(this);div6(this)">
                                            <asp:ListItem Selected="True" Text="-Select-" Value="0"></asp:ListItem>
                                            <asp:ListItem Text="-SelectAll-" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
                                            <asp:ListItem Text="No" Value="No"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </td>
                                <td>
                                    <div id="Status" runat="server">
                                        <asp:Label ID="lblStatus" runat="server" Text="Status"></asp:Label>
                                        :
                                        <%-- Status :--%>
                                        <asp:DropDownList ID="ddlStatus" runat="server" Width="150px" ClientIDMode="Static"
                                            DataValueField="ID" DataTextField="Status" onchange="GetStatus(this)">
                                        </asp:DropDownList>
                                    </div>
                                </td>
                                <td>
                                    <div id="User" runat="server">
                                        <asp:Label ID="lblUser" runat="server" Text="User"></asp:Label>
                                        :
                                        <%--  User :--%>
                                        <asp:DropDownList ID="ddlUser" runat="server" Width="150px" ClientIDMode="Static" DataValueField="ID" DataTextField="Name"
                                            onchange="GetUser(this);div6(this)">
                                        </asp:DropDownList>
                                    </div>
                                </td>
                                <td>
                                    <div id="Role" runat="server">
                                        <asp:Label ID="lblRole" runat="server" Text="Role"></asp:Label>
                                        :
                                        <%--  User :--%>
                                        <asp:DropDownList ID="ddlrole" runat="server" Width="150px" ClientIDMode="Static"
                                            DataTextField="RoleName" DataValueField="ID" onchange="GetRole(this)">
                                        </asp:DropDownList>
                                    </div>
                                </td>
                                <td>
                                    <div id="Active" runat="server">
                                        <asp:Label ID="lblActive" runat="server" Text="Active"></asp:Label>
                                        :
                                        <%--  User :--%>
                                        <asp:DropDownList ID="ddlActive" runat="server" Width="150px" ClientIDMode="Static"
                                            onchange="div7(this);BindUser(this);GetActive(this)">
                                            <asp:ListItem Text="-Select-" Value="0"></asp:ListItem>
                                            <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
                                            <asp:ListItem Text="No" Value="No"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </td>
                                <td>
                                    <div id="ZeroBalance" runat="server">
                                        <input type="checkbox" id="ChkZero" onclick="WithZroBalance(this);" />
                                        <a><asp:Label ID="lblWithZeroBalance" runat="server" Text="With Zero Balance"></asp:Label></a>
                                    </div>
                                </td>
                                <td>
                                    <div id="ImgStatus" runat="server">
                                        <asp:Label ID="lblImgStstus" runat="server" Text="Status"></asp:Label>
                                        :
                                        <asp:DropDownList ID="ddlImgstatus" runat="server" Width="150px" ClientIDMode="Static"
                                            onchange="divImg(this);GetImgeStatus(this);">
                                            <asp:ListItem Text="-Select-" Value="0"></asp:ListItem>
                                            <asp:ListItem Text="Success" Value="Success"></asp:ListItem>
                                            <asp:ListItem Text="Fail" Value="Fail"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </td>
                                
                            <%---------%>
                                 <td>
                                    <div id="Driver" runat="server">
                                        <asp:Label ID="lblDriver" runat="server" Text ="Driver"></asp:Label>
                                        :
                                        <asp:DropDownList ID="ddlDriver" runat="server" Width="120px" ClientIDMode="Static" DataTextField="DName" DataValueField="ID"  onchange="GetDriver(this)">
                                        </asp:DropDownList>
                                    </div>
                                </td>
                                 <td>
                                    <div id="PytMode" runat="server">
                                        <asp:Label ID="lblPytMode" runat="server" Text ="Payment Mode"></asp:Label>
                                        :
                                        <asp:DropDownList ID="ddlPytMode" runat="server" Width="120px" ClientIDMode="Static" DataTextField="MethodName" DataValueField="ID" onchange="GetPaymentMode(this)">
                                             <%--<asp:ListItem Text="-Select All-" Value="0"></asp:ListItem>
                                            <asp:ListItem Text="Cash" Value="COD - Cash On Delivery (QAR)"></asp:ListItem>
                                            <asp:ListItem Text="Card" Value="Credit Card"></asp:ListItem>
                                            <asp:ListItem Text="Other" Value="Other"></asp:ListItem>--%>
                                        </asp:DropDownList>
                                    </div>
                                </td>  
                                 <td>
                                     <div id="dvCustomer" runat="server">
                                          <asp:Label ID="Label1" runat="server" Text ="Customer"></asp:Label>
                                         :
                                         <asp:DropDownList ID="ddlCustomer" runat="server" Width="120px" DataTextField="Name" DataValueField="ID" ClientIDMode="Static" onchange="getCustomer(this)"></asp:DropDownList>
                                     </div>
                                 </td>
                                 <td>
                                    <div id="Vendor" runat="server">
                                        <asp:Label ID="lblVendor" runat="server" Text ="Vendor"></asp:Label>
                                        :
                                        <asp:DropDownList ID="ddlVendor" runat="server" Width="120px" ClientIDMode="Static" DataTextField="Name" DataValueField="ID" onchange="GetVendor(this)" >                                           
                                        </asp:DropDownList>
                                    </div>
                                </td>                                       
                                 <td>
                                      <div id="dvClient" runat="server">
                                        <asp:Label ID="Label2" runat="server" Text ="Client"></asp:Label>
                                        :
                                        <asp:DropDownList ID="ddlClient" runat="server" Width="120px" ClientIDMode="Static" DataTextField="Name" DataValueField="ID" onchange="GetVendor(this)" >                                           
                                        </asp:DropDownList>
                                    </div>
                                 </td>
                            <%---------%>
                                <td>
                                    <div id="exQuery" runat="server">
                                        <input type="button" runat="server" value="Execute" id="btnexQuery" onclick="div6();" />
                                    </div>
                                </td>
                            </tr>
                        </table>                                                  
                    </div>
                </td>
            </tr>
            <tr>
                <td style="padding-left: 980px;">
                    <div id="DlryType" runat="server">
                     <asp:Label ID="lbldeliverytype" runat="server" Text ="Delivery Type"></asp:Label>
                     :
                     <asp:DropDownList ID="ddlDlrytype" runat="server" Width="120px" ClientIDMode="Static" onchange="GetDeliveryType(this);" >
                           <asp:ListItem Text="-Select All-" Value="0"></asp:ListItem>
                           <asp:ListItem Text="Prime" Value="Prime"></asp:ListItem>
                           <asp:ListItem Text="Express" Value="Express"></asp:ListItem>
                           <asp:ListItem Text="Regular" Value="Regular"></asp:ListItem>
                     </asp:DropDownList>
                     </div>
                </td>
            </tr>
            <tr>
                <td>
                    <table style="margin: 0px auto;">
                        <tr>
                            <td style="vertical-align: top;">
                                <table id="tblEngine" runat="server" class="gridFrame" width="650px" style="margin: 3px 3px 3px 3px;
                                    vertical-align: top;">
                                    <tr>
                                        <td>
                                            <table style="width: 100%">
                                                <tr>
                                                    <td style="text-align: left;">
                                                        <a class="headerText">Engine List</a>
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <table style="width: 100%">
                                                            <tr>
                                                                <td style="text-align: right;">
                                                                    <input type="checkbox" id="chkSelectAllEngine" onclick="SelectAllEngine(this);" />
                                                                    <a class="headerText">Select All Engine</a>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <obout:Grid ID="GVEngineInfo" runat="server" AllowAddingRecords="false" AutoGenerateColumns="false"
                                                AllowColumnResizing="true" AllowFiltering="true" AllowManualPaging="true" AllowColumnReordering="true"
                                                AllowRecordSelection="true" AllowMultiRecordSelection="true" AllowGrouping="true"
                                                Width="100%" Serialize="true" CallbackMode="true" OnRebind="GVEngineInfo_OnRebind"
                                                PageSize="5" AllowPaging="true" AllowPageSizeSelection="true">
                                                <ClientSideEvents ExposeSender="true" />
                                                <Columns>
                                                    <%-- <obout:CheckBoxSelectColumn AllowSorting="true" ControlType="Standard" Align="left"
                                                        ShowHeaderCheckBox="true" HeaderAlign="left" Width="5%">
                                                    </obout:CheckBoxSelectColumn>--%>
                                                    <obout:Column DataField="ID" HeaderText="ID" Visible="false">
                                                    </obout:Column>
                                                    <obout:Column DataField="EngineSerial" HeaderText="Engine Serial No." Width="10%">
                                                    </obout:Column>
                                                    <obout:Column DataField="Container" HeaderText="Container" Width="10%">
                                                    </obout:Column>
                                                    <obout:Column DataField="EngineModel" HeaderText="Engine Model" Width="10%">
                                                    </obout:Column>
                                                    <obout:Column DataField="EngineSerial" HeaderText="Engine Serial" Width="10%">
                                                    </obout:Column>
                                                    <obout:Column DataField="GeneratorModel" HeaderText="Generator Model" Width="10%">
                                                    </obout:Column>
                                                    <obout:Column DataField="Territory" HeaderText="Site" Width="10%">
                                                    </obout:Column>
                                                </Columns>
                                            </obout:Grid>
                                        </td>
                                    </tr>
                                </table>
                                <table id="tblRequest" runat="server" class="gridFrame" width="650px" style="margin: 3px 3px 3px 3px;
                                    vertical-align: top;">
                                    <tr>
                                        <td>
                                            <table style="width: 100%">
                                                <tr>
                                                    <td style="text-align: left;">
                                                        <asp:Label ID="lblorderlist" CssClass="headerText" runat="server" Text="Order List"></asp:Label>
                                                    </td>
                                                    <%-- <td style="text-align: right;">
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:TextBox ID="txtRequestSearch" runat="server" ClientIDMode="Static" Style="font-size: 15px;
                                                                        padding: 2px; width: 250px;" onkeyup="SearchRequest();"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <img src="../App_Themes/Blue/img/Search24.png" onclick="SearchRequest()" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>--%>
                                                    <%--<td style="text-align: right;">
                                                        <input type="button" value=">>" id="btnRequest" onclick="SelectedRequestRec();" />
                                                    </td>--%>
                                                    <td>
                                                        <table style="width: 100%">
                                                            <tr>
                                                                <td style="text-align: right;">
                                                                    <input type="checkbox" id="chkSelectAll" onclick="SelectAllRequest(this);" runat="server" />
                                                                    <a class="headerText">
                                                                        <asp:Label ID="lblselectallorder" CssClass="headerText" runat="server" Text="Select All Order"></asp:Label></a>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <obout:Grid ID="GVRequestInfo" runat="server" AutoGenerateColumns="false" AllowFiltering="false"
                                                AllowGrouping="true" AllowColumnResizing="true" AllowAddingRecords="false" AllowColumnReordering="true"
                                                AllowMultiRecordSelection="true" AllowRecordSelection="true" CallbackMode="true"
                                                Width="100%" Serialize="true" PageSize="5" AllowPageSizeSelection="false" AllowManualPaging="true"
                                                ShowTotalNumberOfPages="false" OnRebind="GVRequestInfo_OnRebind">
                                                <ClientSideEvents ExposeSender="true" />
                                                <Columns>
                                                    <%-- <obout:CheckBoxSelectColumn AllowSorting="true" ShowHeaderCheckBox="true" ControlType="Standard"
                                                        Width="3%" Align="left" HeaderAlign="left" ParseHTML="true">
                                                    </obout:CheckBoxSelectColumn>--%>
                                                    <obout:Column DataField="Id" HeaderText="Request No." Width="10%" AllowFilter="false" 
                                                        ParseHTML="true">
                                                    </obout:Column>
                                                     <obout:Column DataField="OrderNo" HeaderText="Request No." Width="10%" AllowFilter="false" Visible="false"
                                                        ParseHTML="true">
                                                    </obout:Column>
                                                    <obout:Column DataField="OrderDate" HeaderText="Requisition Date" Width="10%" DataFormatString="{0:dd-MMM-yyyy}"
                                                        AllowFilter="false" ParseHTML="true">
                                                    </obout:Column>
                                                    <obout:Column DataField="UserName" HeaderText="Request By" Width="10%" AllowFilter="false"
                                                        ParseHTML="true">
                                                    </obout:Column>
                                                </Columns>
                                            </obout:Grid>
                                        </td>
                                    </tr>
                                </table>
                                <table id="tblIssue" runat="server" class="gridFrame" width="650px" style="margin: 3px 3px 3px 3px;
                                    vertical-align: top;">
                                    <tr>
                                        <td>
                                            <table style="width: 100%">
                                                <tr>
                                                    <td style="text-align: left;">
                                                        <asp:Label ID="lblissuelist" CssClass="headerText" runat="server" Text="Issue List"></asp:Label>
                                                    </td>
                                                    <%-- <td style="text-align: right;">
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:TextBox ID="txtIssueSearch" runat="server" ClientIDMode="Static" Style="font-size: 15px;
                                                                        padding: 2px; width: 250px;" onkeyup="SearchIssue();"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <img src="../App_Themes/Blue/img/Search24.png" onclick="SearchIssue()" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>--%>
                                                    <%--<td style="text-align: right;">
                                                        <input type="button" value=">>" id="btnIssue" onclick="SelectedIssueRec();" />
                                                    </td>--%>
                                                    <td>
                                                        <table style="width: 100%">
                                                            <tr>
                                                                <td style="text-align: right;">
                                                                    <input type="checkbox" id="chkSelectAllIssue" onclick="SelectAllIssue(this);" />
                                                                    <asp:Label ID="lblallissue" CssClass="headerText" runat="server" Text="Select All Issue"></asp:Label>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <obout:Grid ID="GVIssueInfo" runat="server" AutoGenerateColumns="false" AllowFiltering="false"
                                                AllowGrouping="true" AllowColumnResizing="true" AllowAddingRecords="false" AllowColumnReordering="true"
                                                AllowMultiRecordSelection="true" AllowRecordSelection="true" CallbackMode="true"
                                                Width="100%" Serialize="true" PageSize="5" AllowPageSizeSelection="false" AllowManualPaging="true"
                                                ShowTotalNumberOfPages="false" OnRebind="GVIssueInfo_OnRebind" KeepSelectedRecords="true">
                                                <ClientSideEvents ExposeSender="true" />
                                                <Columns>
                                                    <%-- <obout:CheckBoxSelectColumn AllowSorting="true" ControlType="Standard" Width="3%"
                                                        Align="left" ShowHeaderCheckBox="true" HeaderAlign="left">
                                                    </obout:CheckBoxSelectColumn>--%>
                                                    <%-- <obout:Column Dat  aField="MINH_ID" HeaderText="ID" Visible="false">
                                                    </obout:Column>--%>
                                                    <obout:Column DataField="MINH_ID" HeaderText="Issue No" Width="10%">
                                                    </obout:Column>
                                                    <obout:Column DataField="IssueDate" HeaderText="Issue Date" Width="10%" DataFormatString="{0:dd-MMM-yyyy}">
                                                    </obout:Column>
                                                    <obout:Column DataField="IssuedByUserName" HeaderText="Issued By" Width="10%">
                                                    </obout:Column>
                                                </Columns>
                                            </obout:Grid>
                                        </td>
                                    </tr>
                                </table>
                                <table id="tblReceipt" runat="server" class="gridFrame" width="650px" style="margin: 3px 3px 3px 3px;
                                    vertical-align: top;">
                                    <tr>
                                        <td>
                                            <table style="width: 100%">
                                                <tr>
                                                    <td style="text-align: left">
                                                        <a class="headerText">Receipt List</a>
                                                    </td>
                                                    <%--<td style="text-align: right">
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:TextBox ID="txtReceiptSearch" runat="server" ClientIDMode="Static" Style="font-size: 15px;
                                                                        padding: 2px; width: 250px;" onkeyup="SearchReceipt();"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <img src="../App_Themes/Blue/img/Search24.png" onclick="SearchReceipt()" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>--%>
                                                    <td>
                                                        <table style="width: 100%">
                                                            <tr>
                                                                <td style="text-align: right;">
                                                                    <input type="checkbox" id="chkSelectAllReceipt" onclick="SelectAllReceipt(this);" />
                                                                    <a class="headerText">Select All Receipt</a>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <obout:Grid ID="GVReceiptInfo" runat="server" AllowAddingRecords="false" AutoGenerateColumns="false"
                                                AllowColumnResizing="true" AllowFiltering="true" AllowManualPaging="true" AllowColumnReordering="true"
                                                AllowMultiRecordSelection="true" AllowRecordSelection="true" AllowGrouping="true"
                                                Width="100%" Serialize="true" CallbackMode="true" PageSize="10" OnRebind="GVReceiptInfo_OnRebind"
                                                AllowPaging="true" AllowPageSizeSelection="true">
                                                <ClientSideEvents ExposeSender="true" />
                                                <Columns>
                                                    <%-- <obout:CheckBoxSelectColumn AllowSorting="true" ControlType="Standard" Width="3%"
                                                        Align="left" ShowHeaderCheckBox="true" HeaderAlign="left">
                                                    </obout:CheckBoxSelectColumn>--%>
                                                    <%-- <obout:Column DataField="GRNH_ID" HeaderText="ID" Visible="false">
                                                    </obout:Column>--%>
                                                    <obout:Column DataField="GRNH_ID" HeaderText="Receipt No." Width="10%">
                                                    </obout:Column>
                                                    <obout:Column DataField="GRN_Date" HeaderText="Receipt Date" Width="10%" DataFormatString="{0:dd-MMM-yyyy}">
                                                    </obout:Column>
                                                    <obout:Column DataField="ReceiptByUserName" HeaderText="Receipt By" Width="10%">
                                                    </obout:Column>
                                                </Columns>
                                            </obout:Grid>
                                        </td>
                                    </tr>
                                </table>
                                <table id="tblGRN" runat="server" class="gridFrame" width="650px" style="margin: 3px 3px 3px 3px;
                                    vertical-align: top;">
                                    <tr>
                                        <td>
                                            <table style="width: 100%">
                                                <tr>
                                                    <td style="text-align: left">
                                                        <a class="headerText">GRN List</a>
                                                    </td>                                                   
                                                    <td>
                                                        <table style="width: 100%">
                                                            <tr>
                                                                <td style="text-align: right;">
                                                                    <input type="checkbox" id="Checkbox3" onclick="SelectAllReceipt(this);" />
                                                                    <a class="headerText">Select All GRN</a>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <obout:Grid ID="gvGrnInfo" runat="server" AllowAddingRecords="false" AutoGenerateColumns="false"
                                                AllowColumnResizing="true" AllowFiltering="true" AllowManualPaging="true" AllowColumnReordering="true"
                                                AllowMultiRecordSelection="true" AllowRecordSelection="true" AllowGrouping="true"
                                                Width="100%" Serialize="true" CallbackMode="true" PageSize="10" OnRebind="gvGrnInfo_OnRebind"
                                                AllowPaging="true" AllowPageSizeSelection="true">
                                                <ClientSideEvents ExposeSender="true" />
                                                <Columns>                                                    
                                                    <obout:Column DataField="ID" HeaderText="GRN No." Width="10%">
                                                    </obout:Column>
                                                    <obout:Column DataField="GRNDate" HeaderText="GRN Date" Width="10%" DataFormatString="{0:dd-MMM-yyyy}">
                                                    </obout:Column>
                                                    <obout:Column DataField="Grnby" HeaderText="GRN By" Width="10%">
                                                    </obout:Column>
                                                </Columns>
                                            </obout:Grid>
                                        </td>
                                    </tr>
                                </table>
                                 <table id="tblQC" runat="server" class="gridFrame" width="650px" style="margin: 3px 3px 3px 3px;
                                    vertical-align: top;">
                                    <tr>
                                        <td>
                                            <table style="width: 100%">
                                                <tr>
                                                    <td style="text-align: left">
                                                        <a class="headerText">QC List</a>
                                                    </td>                                                   
                                                    <td>
                                                        <table style="width: 100%">
                                                            <tr>
                                                                <td style="text-align: right;">
                                                                    <input type="checkbox" id="Checkbox4" onclick="SelectAllReceipt(this);" />
                                                                    <a class="headerText">Select All QC</a>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <obout:Grid ID="gvQC" runat="server" AllowAddingRecords="false" AutoGenerateColumns="false"
                                                AllowColumnResizing="true" AllowFiltering="true" AllowManualPaging="true" AllowColumnReordering="true"
                                                AllowMultiRecordSelection="true" AllowRecordSelection="true" AllowGrouping="true"
                                                Width="100%" Serialize="true" CallbackMode="true" PageSize="10" OnRebind="gvQC_OnRebind"
                                                AllowPaging="true" AllowPageSizeSelection="true">
                                                <ClientSideEvents ExposeSender="true" />
                                                <Columns>                                                    
                                                    <obout:Column DataField="ID" HeaderText="QC No." Width="10%">
                                                    </obout:Column>
                                                    <obout:Column DataField="QCDate" HeaderText="QC Date" Width="10%" DataFormatString="{0:dd-MMM-yyyy}">
                                                    </obout:Column>
                                                    <obout:Column DataField="QCby" HeaderText="QC By" Width="10%">
                                                    </obout:Column>
                                                </Columns>
                                            </obout:Grid>
                                        </td>
                                    </tr>
                                </table>
                                <table id="tblPutIn" runat="server" class="gridFrame" width="650px" style="margin: 3px 3px 3px 3px;
                                    vertical-align: top;">
                                    <tr>
                                        <td>
                                            <table style="width: 100%">
                                                <tr>
                                                    <td style="text-align: left">
                                                        <a class="headerText">PutIn List</a>
                                                    </td>                                                   
                                                    <td>
                                                        <table style="width: 100%">
                                                            <tr>
                                                                <td style="text-align: right;">
                                                                    <input type="checkbox" id="Checkbox5" onclick="SelectAllReceipt(this);" />
                                                                    <a class="headerText">Select All PutIn</a>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <obout:Grid ID="gvPutIn" runat="server" AllowAddingRecords="false" AutoGenerateColumns="false"
                                                AllowColumnResizing="true" AllowFiltering="true" AllowManualPaging="true" AllowColumnReordering="true"
                                                AllowMultiRecordSelection="true" AllowRecordSelection="true" AllowGrouping="true"
                                                Width="100%" Serialize="true" CallbackMode="true" PageSize="10" OnRebind="gvPutIn_OnRebind"
                                                AllowPaging="true" AllowPageSizeSelection="true">
                                                <ClientSideEvents ExposeSender="true" />
                                                <Columns>                                                    
                                                    <obout:Column DataField="ID" HeaderText="PutIn No." Width="10%">
                                                    </obout:Column>
                                                    <obout:Column DataField="PutInDate" HeaderText="PutIn Date" Width="10%" DataFormatString="{0:dd-MMM-yyyy}">
                                                    </obout:Column>
                                                    <obout:Column DataField="PutInby" HeaderText="PutIn By" Width="10%">
                                                    </obout:Column>
                                                </Columns>
                                            </obout:Grid>
                                        </td>
                                    </tr>
                                </table>
                                 <table id="tblPickUP" runat="server" class="gridFrame" width="650px" style="margin: 3px 3px 3px 3px;
                                    vertical-align: top;">
                                    <tr>
                                        <td>
                                            <table style="width: 100%">
                                                <tr>
                                                    <td style="text-align: left">
                                                        <a class="headerText">PickUp List</a>
                                                    </td>                                                   
                                                    <td>
                                                        <table style="width: 100%">
                                                            <tr>
                                                                <td style="text-align: right;">
                                                                    <input type="checkbox" id="Checkbox6" onclick="SelectAllReceipt(this);" />
                                                                    <a class="headerText">Select All PickUp</a>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <obout:Grid ID="gvPickUp" runat="server" AllowAddingRecords="false" AutoGenerateColumns="false"
                                                AllowColumnResizing="true" AllowFiltering="true" AllowManualPaging="true" AllowColumnReordering="true"
                                                AllowMultiRecordSelection="true" AllowRecordSelection="true" AllowGrouping="true"
                                                Width="100%" Serialize="true" CallbackMode="true" PageSize="10" OnRebind="gvPickUp_OnRebind"
                                                AllowPaging="true" AllowPageSizeSelection="true">
                                                <ClientSideEvents ExposeSender="true" />
                                                <Columns>                                                    
                                                    <obout:Column DataField="ID" HeaderText="PickUp No." Width="10%">
                                                    </obout:Column>
                                                    <obout:Column DataField="PickUpDate" HeaderText="PickUp Date" Width="10%" DataFormatString="{0:dd-MMM-yyyy}">
                                                    </obout:Column>
                                                    <obout:Column DataField="PickUpby" HeaderText="PickUp By" Width="10%">
                                                    </obout:Column>
                                                </Columns>
                                            </obout:Grid>
                                        </td>
                                    </tr>
                                </table>
                                 <table id="tblDispatch" runat="server" class="gridFrame" width="650px" style="margin: 3px 3px 3px 3px;
                                    vertical-align: top;">
                                    <tr>
                                        <td>
                                            <table style="width: 100%">
                                                <tr>
                                                    <td style="text-align: left">
                                                        <a class="headerText">Dispatch List</a>
                                                    </td>                                                   
                                                    <td>
                                                        <table style="width: 100%">
                                                            <tr>
                                                                <td style="text-align: right;">
                                                                    <input type="checkbox" id="Checkbox7" onclick="SelectAllReceipt(this);" />
                                                                    <a class="headerText">Select All Dispatch</a>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <obout:Grid ID="gvDispatch" runat="server" AllowAddingRecords="false" AutoGenerateColumns="false"
                                                AllowColumnResizing="true" AllowFiltering="true" AllowManualPaging="true" AllowColumnReordering="true"
                                                AllowMultiRecordSelection="true" AllowRecordSelection="true" AllowGrouping="true"
                                                Width="100%" Serialize="true" CallbackMode="true" PageSize="10" OnRebind="gvDispatch_OnRebind"
                                                AllowPaging="true" AllowPageSizeSelection="true">
                                                <ClientSideEvents ExposeSender="true" />
                                                <Columns>                                                    
                                                    <obout:Column DataField="ID" HeaderText="Dispatch No." Width="10%">
                                                    </obout:Column>
                                                    <obout:Column DataField="DispatchDate" HeaderText="Dispatch Date" Width="10%" DataFormatString="{0:dd-MMM-yyyy}">
                                                    </obout:Column>
                                                    <obout:Column DataField="DispatchByUser" HeaderText="Dispatch By" Width="10%">
                                                    </obout:Column>
                                                </Columns>
                                            </obout:Grid>
                                        </td>
                                    </tr>
                                </table>
                            </td>                           
                             <td style="vertical-align: top;">
                                <table id="tblPurchaseOrderInfo" runat="server" class="gridFrame" width="600px" style="margin: 3px 3px 3px 3px;
                                    vertical-align: top;">
                                    <tr>
                                        <td>
                                            <table style="width: 100%">
                                                <tr>
                                                    <td style="text-align: left;">
                                                        <asp:Label ID="lblpurchaseorder" CssClass="headerText" runat="server" Text="Purchase Order List"></asp:Label>
                                                    </td>
                                                    <td style="text-align: right;">
                                                        <table>
                                                            <tr>
                                                               <td>
                                                                    <%-- <asp:TextBox runat="server" ID="txtProductSearch" ClientIDMode="Static" Style="font-size: 15px;
                                                                        padding: 2px; width: 250px;" onkeyup="SearchProduct();"></asp:TextBox>
                                                                    <input type="text" id="Text1" onkeyup="SearchUser();" style="font-size: 15px;
                                                                        padding: 2px; width: 325px;" />
                                                                    <asp:HiddenField runat="server" ID="HiddenField2" />
                                                                </td>
                                                                <td>
                                                                    <img src="../App_Themes/Blue/img/Search24.png" alt="" onclick="SearchUser()" />
                                                                </td>
                                                                <td>
                                                                </td>
                                                                <td>--%>
                                                                    <table style="width: 100%">
                                                                        <tr>
                                                                            <td style="text-align: right;">
                                                                                <input type="checkbox" id="Checkbox2" onclick="SelectAllPurchaseOrder(this);" />
                                                                                <asp:Label ID="lblallpurchaseorder" CssClass="headerText" runat="server" Text="Select All Purchase Order"></asp:Label>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <%-- <td style="text-align: right;">
                                                    <input type="button" value=">>" id="btnProduct" onclick="selectedProductRec();" />
                                                    </td> --%>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="vertical-align: top;">
                                            <obout:Grid ID="GVpurchaseInfo" runat="server" AllowAddingRecords="false" AutoGenerateColumns="false"
                                                AllowColumnResizing="true" AllowFiltering="true" AllowManualPaging="true" AllowColumnReordering="true"
                                                AllowMultiRecordSelection="true" AllowRecordSelection="true" AllowGrouping="true"
                                                Width="100%" Serialize="true" CallbackMode="true" PageSize="5" AllowPaging="true"
                                                 AllowPageSizeSelection="true" OnRebind="GVpurchaseInfo_Rebind">
                                                <ClientSideEvents ExposeSender="true" />
                                                <Columns>
                                                    <%--  <obout:CheckBoxSelectColumn AllowSorting="true" ControlType="Standard" Width="5%"
                                                        Align="left" ShowHeaderCheckBox="true" HeaderAlign="left">
                                                    </obout:CheckBoxSelectColumn>--%>
                                                    <obout:Column DataField="Id" HeaderText="ID" Width="10%" Visible="false">
                                                    </obout:Column>
                                                  
                                                    <obout:Column DataField="POOrderNo" HeaderText="Purchase Order No" Width="10%">
                                                    </obout:Column>
                                                    <%--<obout:Column DataField="Description" HeaderText="Part Description" Width="10%">
                                                    </obout:Column>--%>
                                                    <obout:Column DataField="PODate" HeaderText="Purchase Order Date" Width="10%" DataFormatString="{0:dd-MM-yyyy}">
                                                    </obout:Column>
                                                    <obout:Column DataField="StatusName" HeaderText="Status" Width="10%">
                                                    </obout:Column>
                                                </Columns>
                                            </obout:Grid>
                                        </td>
                                         
                                    </tr>
                                </table>
                            </td>

                            <td style="vertical-align: top;">
                                <table id="tblProduct" runat="server" class="gridFrame" width="600px" style="margin: 3px 3px 3px 3px;
                                    vertical-align: top;">
                                    <tr>
                                        <td>
                                            <table style="width: 100%">
                                                <tr>
                                                    <td style="text-align: left;">
                                                        <asp:Label ID="lblskulist" CssClass="headerText" runat="server" Text="SKU List"></asp:Label>
                                                    </td>
                                                    <td style="text-align: right;">
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <%-- <asp:TextBox runat="server" ID="txtProductSearch" ClientIDMode="Static" Style="font-size: 15px;
                                                                        padding: 2px; width: 250px;" onkeyup="SearchProduct();"></asp:TextBox>--%>
                                                                    <input type="text" id="txtProductSearch" onkeyup="SearchProduct();" style="font-size: 15px;
                                                                        padding: 2px; width: 325px;" />
                                                                    <asp:HiddenField runat="server" ID="hdnFilterText" />
                                                                </td>
                                                                <td>
                                                                    <img src="../App_Themes/Blue/img/Search24.png" onclick="SearchProduct()" />
                                                                </td>
                                                                <td>
                                                                </td>
                                                                <td>
                                                                    <table style="width: 100%">
                                                                        <tr>
                                                                            <td style="text-align: right;">
                                                                                <input type="checkbox" id="chkSelectProduct" onclick="SelectAllProduct(this);" runat="server" />
                                                                                <asp:Label ID="lblallsku" CssClass="headerText" runat="server" Text="Select All SKU"></asp:Label>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <%-- <td style="text-align: right;">
                                                    <input type="button" value=">>" id="btnProduct" onclick="selectedProductRec();" />
                                                    </td> --%>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="vertical-align: top;">
                                            <obout:Grid ID="GVProductInfo" runat="server" AllowAddingRecords="false" AutoGenerateColumns="false"
                                                AllowColumnResizing="true" AllowFiltering="false" AllowManualPaging="true" AllowColumnReordering="true"
                                                AllowMultiRecordSelection="true" AllowRecordSelection="true" AllowGrouping="true"
                                                Width="100%" Serialize="true" CallbackMode="true" OnRebind="GVProductInfo_OnRebind"
                                                PageSize="5" AllowPaging="true" AllowPageSizeSelection="true">
                                                <ClientSideEvents ExposeSender="true" />
                                                <Columns>
                                                    <%--  <obout:CheckBoxSelectColumn AllowSorting="true" ControlType="Standard" Width="5%"
                                                        Align="left" ShowHeaderCheckBox="true" HeaderAlign="left">
                                                    </obout:CheckBoxSelectColumn>--%>
                                                    <obout:Column DataField="ID" HeaderText="ID" Visible="false">
                                                    </obout:Column>
                                                    <obout:Column DataField="ProductCode" HeaderText="SKU Code" Width="10%">
                                                    </obout:Column>
                                                    <obout:Column DataField="Name" HeaderText="SKU Name" Width="10%">
                                                    </obout:Column>
                                                    <obout:Column DataField="Description" HeaderText="Description" Width="10%">
                                                    </obout:Column>
                                                    <%--<obout:Column DataField="Description" HeaderText="Part Description" Width="10%">
                                                    </obout:Column>--%>
                                                    <%-- <obout:Column DataField="ProductCategory" HeaderText="Part Category" Width="10%">
                                                    </obout:Column>--%>
                                                </Columns>
                                            </obout:Grid>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td style="vertical-align: top;">
                                <table id="tblUserInfo" runat="server" class="gridFrame" width="800px" style="margin: 3px 3px 3px 3px;
                                    vertical-align: top;">
                                    <tr>
                                        <td>
                                            <table style="width: 100%">
                                                <tr>
                                                    <td style="text-align: left;">
                                                        <asp:Label ID="lbluserlist" CssClass="headerText" runat="server" Text="User List"></asp:Label>
                                                    </td>
                                                    <td style="text-align: right;">
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <%-- <asp:TextBox runat="server" ID="txtProductSearch" ClientIDMode="Static" Style="font-size: 15px;
                                                                        padding: 2px; width: 250px;" onkeyup="SearchProduct();"></asp:TextBox>--%>
                                                                    <input type="text" id="txtUserInfo" onkeyup="SearchUser();" style="font-size: 15px;
                                                                        padding: 2px; width: 325px;" />
                                                                    <asp:HiddenField runat="server" ID="HiddenField1" />
                                                                </td>
                                                                <td>
                                                                    <img src="../App_Themes/Blue/img/Search24.png" alt="" onclick="SearchUser()" />
                                                                </td>
                                                                <td>
                                                                </td>
                                                                <td>
                                                                    <table style="width: 100%">
                                                                        <tr>
                                                                            <td style="text-align: right;">
                                                                                <input type="checkbox" id="Checkbox1" onclick="SelectAllUser(this);" />
                                                                                <asp:Label ID="lblalluser" CssClass="headerText" runat="server" Text="Select All User"></asp:Label>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <%-- <td style="text-align: right;">
                                                    <input type="button" value=">>" id="btnProduct" onclick="selectedProductRec();" />
                                                    </td> --%>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="vertical-align: top;">
                                            <obout:Grid ID="GVUserInfo" runat="server" AllowAddingRecords="false" AutoGenerateColumns="false"
                                                AllowColumnResizing="true" AllowFiltering="true" AllowManualPaging="true" AllowColumnReordering="true"
                                                AllowMultiRecordSelection="true" AllowRecordSelection="true" AllowGrouping="true"
                                                Width="100%" Serialize="true" CallbackMode="true" PageSize="5" AllowPaging="true"
                                                OnRebind="GVUserInfo_OnRebind" AllowPageSizeSelection="true">
                                                <ClientSideEvents ExposeSender="true" />
                                                <Columns>
                                                    <%--  <obout:CheckBoxSelectColumn AllowSorting="true" ControlType="Standard" Width="5%"
                                                        Align="left" ShowHeaderCheckBox="true" HeaderAlign="left">
                                                    </obout:CheckBoxSelectColumn>--%>
                                                    <obout:Column DataField="ID" HeaderText="ID" Width="10%" Visible="false">
                                                    </obout:Column>
                                                    <obout:Column DataField="EmployeeID" HeaderText="ID" Width="10%">
                                                    </obout:Column>
                                                    <obout:Column DataField="Name" HeaderText="User Name" Width="10%">
                                                    </obout:Column>
                                                    <%--<obout:Column DataField="Description" HeaderText="Part Description" Width="10%">
                                                    </obout:Column>--%>
                                                    <obout:Column DataField="MobileNo" HeaderText="Mobile" Width="10%">
                                                    </obout:Column>
                                                    <obout:Column DataField="EmailID" HeaderText="Email" Width="10%">
                                                    </obout:Column>
                                                </Columns>
                                            </obout:Grid>
                                        </td>
                                    </tr>
                                </table>
                            </td>


                            
    





                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <asp:HiddenField ID="hfCount" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hfEng" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hndGroupByGrid" runat="server" />
        <asp:HiddenField ID="hndGroupByPrd" runat="server" />
        <asp:HiddenField ID="hdnEngineSelectedRec" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnProductSelectedRec" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnRequestSelectedRec" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnIssueSelectedRec" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnReceiptSelectedRec" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnProductCategory" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnAllReq" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnAllPrd" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnAllIsue" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnAllRecpt" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnAllEng" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnExcludeZero" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnSelectedFromSite" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnSelectedCompany" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnSelectedDepartment" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnSelectedImage" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnSelectedGroupSet" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnSelectedStatus" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnSelectedUser" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnUserSelectedRec" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnAlluser" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnSelectedActive" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnSelectedRole" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnUser" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnSKU" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnWithZero" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnImgStatus" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnNewFDt" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnNewTDt" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnSelectedDriver" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnSelectedPaymentMode" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnSelectedDeliveryType" runat="server" ClientIDMode="Static" />
         <asp:HiddenField ID="hdnSelectedVendor" runat="server" ClientIDMode="Static" />
    </center>
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
                if (getParameterByName("invoker").toString() == "partissue") {
                    SelectedIssueRec();
                }
                if (getParameterByName("invoker").toString() == "partrequest") {
                    SelectedRequestRec();
                }
                if (getParameterByName("invoker").toString() == "partconsumption") {
                    selectedEngineRec();
                }
                if (getParameterByName("invoker").toString() == "partreceipt") {
                    SelectedReceiptRec();
                }
                if (getParameterByName("invoker").toString() == "sku") {
                    selectedProductRec();
                }
                if (getParameterByName("invoker").toString() == "SkuDetails") {
                    selectedProductRec();
                }
                if (getParameterByName("invoker").toString() == "BomDetail") {
                    selectedProductRec();
                }
                if (getParameterByName("invoker").toString() == "user") {
                    selectedUserRec();
                }
                if (getParameterByName("invoker").toString() == "order") {
                    SelectedRequestRec();
                }
                if (getParameterByName("invoker").toString() == "orderdetail") {
                    SelectedRequestRec();
                }
                if (getParameterByName("invoker").toString() == "orderlead") {
                    SelectedRequestRec();
                }
                if (getParameterByName("invoker").toString() == "orderdelivery") {
                    SelectedRequestRec();
                }
                if (getParameterByName("invoker").toString() == "sla") {
                    SelectedRequestRec();
                }
                if (getParameterByName("invoker").toString() == "totaldeliveryvstotalrequest") {
                    selectedProductRec();
                }
                if (getParameterByName("invoker").toString() == "purchaseorder") {
                    SelectedPurchaseOrderRec();
                }
            }
        }
        function div1(obj) {
            var hfCount = document.getElementById("hfCount");
            hfCount.value = null;
            if (obj.options[obj.selectedIndex].text == 'Select All') {
                var hfCount = document.getElementById("hfCount");
                var opt = document.getElementById("ddlSite");
                for (i = 0; i < opt.options.length; i++) {
                    if (opt.options[i].selected) {

                    }

                    else {
                        if (hfCount.value == "") {
                            hfCount.value = obj.options[i].value;
                        }
                        else {
                            hfCount.value = hfCount.value + "," + obj.options[i].value;
                        }
                    }
                }
            }
            else {
                hfCount.value = obj.options[obj.selectedIndex].value;
            }

            if (getParameterByName('invoker') == "partrequest") {
                GVRequestInfo.refresh();
            }
            else if (getParameterByName('invoker') == "partconsumption") {
                GVEngineInfo.refresh();
            }
            else if (getParameterByName('invoker') == "partissue") {
                GVIssueInfo.refresh();
            }
            else if (getParameterByName('invoker') == "partreceipt") {
                GVReceiptInfo.refresh();
            }
            else if (getParameterByName('invoker') == "productdtl") {
                GVProductInfo.refresh();
            }

        }

        function selectedUserRec() {
            var hdnUserSelectedRec = document.getElementById("hdnUserSelectedRec");
            hdnUserSelectedRec.value = "";

            for (var i = 0; i < GVUserInfo.PageSelectedRecords.length; i++) {
                var record = GVUserInfo.PageSelectedRecords[i];
                if (hdnUserSelectedRec.value != "") hdnUserSelectedRec.value += ',' + record.ID;
                if (hdnUserSelectedRec.value == "") hdnUserSelectedRec.value = record.ID;
            }


        }

        function GVUserInfo_Deselect(index) {
            selectedUserRec();
        }

        function selectedEngineRec() {
            var hdnEngineSelectedRec = document.getElementById("hdnEngineSelectedRec");
            var oldEngineSelected = hdnEngineSelectedRec.value;
            hdnEngineSelectedRec.value = "0";
            if (GVEngineInfo.PageSelectedRecords.length > 0) {
                for (var i = 0; i < GVEngineInfo.PageSelectedRecords.length; i++) {
                    var record = GVEngineInfo.PageSelectedRecords[i];
                    if (hdnEngineSelectedRec.value != "0") hdnEngineSelectedRec.value += ',' + record.EngineSerial;
                    if (hdnEngineSelectedRec.value == "0") hdnEngineSelectedRec.value = record.EngineSerial;
                }
                if (oldEngineSelected != hdnEngineSelectedRec.value) {
                    GVProductInfo.refresh();
                }
            }

        }
        function GVEngineInfo_Deselect(index) {
            selectedEngineRec();
        }
        function selectedProductRec() {
            var hdnProductSelectedRec = document.getElementById("hdnProductSelectedRec");
            hdnProductSelectedRec.value = "";
            //            if (GVProductInfo.PageSelectedRecords.length > 0) {
            for (var i = 0; i < GVProductInfo.PageSelectedRecords.length; i++) {
                var record = GVProductInfo.PageSelectedRecords[i];
                if (hdnProductSelectedRec.value != "") hdnProductSelectedRec.value += ',' + record.ID;
                if (hdnProductSelectedRec.value == "") hdnProductSelectedRec.value = record.ID;
            }


        }

        function GVProductInfo_Deselect(index) {
            selectedProductRec();
        }

        function SelectedRequestRec() {
            var hdnRequestSelectedRec = document.getElementById("hdnRequestSelectedRec");
            var oldReqSelected = hdnRequestSelectedRec.value;
            hdnRequestSelectedRec.value = "";
            for (var i = 0; i < GVRequestInfo.PageSelectedRecords.length; i++) {
                var record = GVRequestInfo.PageSelectedRecords[i];
                if (hdnRequestSelectedRec.value != "") hdnRequestSelectedRec.value += ',' + record.Id;
                if (hdnRequestSelectedRec.value == "") hdnRequestSelectedRec.value = record.Id;

            }

            if (oldReqSelected != hdnRequestSelectedRec.value) {
                GVProductInfo.refresh();
            }
        }

        function GVRequestInfo_Deselect(index) {
            SelectedRequestRec();
        }


        function SelectedPurchaseOrderRec() {
            var hdnRequestSelectedRec = document.getElementById("hdnRequestSelectedRec");
            var oldReqSelected = hdnRequestSelectedRec.value;
            hdnRequestSelectedRec.value = "";
            for (var i = 0; i < GVpurchaseInfo.PageSelectedRecords.length; i++) {
                var record = GVpurchaseInfo.PageSelectedRecords[i];
                if (hdnRequestSelectedRec.value != "") hdnRequestSelectedRec.value += ',' + record.Id;
                if (hdnRequestSelectedRec.value == "") hdnRequestSelectedRec.value = record.Id;

            }

            if (oldReqSelected != hdnRequestSelectedRec.value) {
                GVProductInfo.refresh();
            }
        }


        function GVpurchaseInfo_Deselect(index) {
            SelectedPurchaseOrderRec();
        }





        function SelectedIssueRec() {
            var hdnIssueSelectedRec = document.getElementById("hdnIssueSelectedRec");
            var oldIssueSelected = hdnIssueSelectedRec.value;
            hdnIssueSelectedRec.value = "0";
            if (GVIssueInfo.PageSelectedRecords.length > 0) {
                for (var i = 0; i < GVIssueInfo.PageSelectedRecords.length; i++) {
                    var record = GVIssueInfo.PageSelectedRecords[i];
                    if (hdnIssueSelectedRec.value != "0") hdnIssueSelectedRec.value += ',' + record.MINH_ID;
                    if (hdnIssueSelectedRec.value == "0") hdnIssueSelectedRec.value = record.MINH_ID;
                }
                if (oldIssueSelected != hdnIssueSelectedRec.value) {
                    GVProductInfo.refresh();
                }
            }
        }
        function GVIssueInfo_Deselect(index) {
            SelectedIssueRec();
        }
        function SelectedReceiptRec() {
            var hdnReceiptSelectedRec = document.getElementById("hdnReceiptSelectedRec");
            var oldReceiptSelected = hdnReceiptSelectedRec.value;
            hdnReceiptSelectedRec.value = "0";
            if (GVReceiptInfo.PageSelectedRecords.length > 0) {
                for (var i = 0; i < GVReceiptInfo.PageSelectedRecords.length; i++) {
                    var record = GVReceiptInfo.PageSelectedRecords[i];
                    if (hdnReceiptSelectedRec.value != "0") hdnReceiptSelectedRec.value += ',' + record.GRNH_ID;
                    if (hdnReceiptSelectedRec.value == "0") hdnReceiptSelectedRec.value = record.GRNH_ID;
                }
                if (oldReceiptSelected != hdnReceiptSelectedRec.value) {
                    GVProductInfo.refresh();
                }
            }
        }
        function GVReceiptInfo_Deselect(index) {
            SelectedReceiptRec();
        }
        var searchlength;
        function SearchEngine() {
            if (searchlength != document.getElementById("txtEngineSearch").value.length) {
                searchlength = document.getElementById("txtEngineSearch").value.length;
                if (document.getElementById("txtEngineSearch").value.length % 2 == 0) {
                    GVEngineInfo.refresh();
                }
            }
        }
        //        var searchlength1;
        //        function SearchProduct() {
        //            if (searchlength1 != document.getElementById("txtProductSearch").value.length) {
        //                searchlength1 = document.getElementById("txtProductSearch").value.length;
        //                if (document.getElementById("txtProductSearch").value.length % 2 == 0) {
        //                    GVProductInfo.refresh();
        //                }
        //            }
        //        }

        var searchTimeout = null;
        function SearchProduct() {
            var hdnFilterText = document.getElementById("<%= hdnFilterText.ClientID %>");
            hdnFilterText.value = document.getElementById("txtProductSearch").value;
            if (searchTimeout != null) {
                window.clearTimeout(searchTimeout);
            }
            searchTimeout = window.setTimeout(performSearch, 700);
        }

        function performSearch() {
            GVProductInfo.refresh();
            searchTimeout = null;
            return false;
        }

        function SearchUser() {
            var hdnFilterText = document.getElementById("<%= hdnFilterText.ClientID %>");
            hdnFilterText.value = document.getElementById("txtUserInfo").value;
            if (searchTimeout != null) {
                window.clearTimeout(searchTimeout);
            }
            searchTimeout = window.setTimeout(performSearchUser, 700);
        }

        function performSearchUser() {
            GVUserInfo.refresh();
            searchTimeout = null;
            return false;
        }
        var searchlength1;
        function SearchRequest() {
            if (searchlength1 != document.getElementById("txtRequestSearch").value.length) {
                searchlength1 = document.getElementById("txtRequestSearch").value.length;
                if (document.getElementById("txtRequestSearch").value.length % 2 == 0) {
                    GVRequestInfo.refresh();
                }
            }
        }
        var searchlength1;
        function SearchIssue() {
            if (searchlength1 != document.getElementById("txtIssueSearch").value.length) {
                searchlength1 = document.getElementById("txtIssueSearch").value.length;
                if (document.getElementById("txtIssueSearch").value.length % 2 == 0) {
                    GVIssueInfo.refresh();
                }
            }
        }
        var searchlength1;
        function SearchReceipt() {
            if (searchlength1 != document.getElementById("txtReceiptSearch").value.length) {
                searchlength1 = document.getElementById("txtReceiptSearch").value.length;
                if (document.getElementById("txtReceiptSearch").value.length % 2 == 0) {
                    GVReceiptInfo.refresh();
                }
            }
        }

        /*PO GRN Reports New*/
        function jsGetPOList() {
            LoadingOn();
            PageMethods.WMGetpoListALL( jsGetGWCSKUTransactionReportOnSuccess, null);
        }

        function jsGetgrnList() {
            LoadingOn();
            PageMethods.WMGetgrnListALL(jsGetGWCSKUTransactionReportOnSuccess, null);
        }

        function jsGetgrndetail() {
            LoadingOn();
            PageMethods.WMGetgrnDetail(jsGetGWCSKUTransactionReportOnSuccess, null);
        }

        function jsGetqcList() {
            LoadingOn();
            PageMethods.WMGetqcListALL(jsGetGWCSKUTransactionReportOnSuccess, null);
        }

        function jsGetqcdetail() {
            LoadingOn();
            PageMethods.WMGetqcDetail(jsGetGWCSKUTransactionReportOnSuccess, null);
        }

        function jsGetputinList() {
            LoadingOn();
            PageMethods.WMGetputinListALL(jsGetGWCSKUTransactionReportOnSuccess, null);
        }

        function jsGetputindetail() {
            LoadingOn();
            PageMethods.WMGetputinDetail(jsGetGWCSKUTransactionReportOnSuccess, null);
        }

        function jsGetorderList() {
            LoadingOn();
            PageMethods.WMGetorderListALL(jsGetGWCSKUTransactionReportOnSuccess, null);
        }

        function jsGetorderdetail() {
            LoadingOn();
            PageMethods.WMGetorderDetail(jsGetGWCSKUTransactionReportOnSuccess, null);
        }

        function jsGetpickupList() {
            LoadingOn();
            PageMethods.WMGetpickupListALL(jsGetGWCSKUTransactionReportOnSuccess, null);
        }

        function jsGetpickupdetail() {
            LoadingOn();
            PageMethods.WMGetpickupDetail(jsGetGWCSKUTransactionReportOnSuccess, null);
        }

        function jsGetdispatchList() {
            LoadingOn();
            PageMethods.WMGetdispatchListALL(jsGetGWCSKUTransactionReportOnSuccess, null);
        } 

        function jsGetdispatchdetail() {
            LoadingOn();
            PageMethods.WMGetdispatchDetail(jsGetGWCSKUTransactionReportOnSuccess, null);
        }
        /*PO Reports New*/

        /*Report viewer code*/
        function jsGetReportData() {
            var hdnProductSelectedRec = document.getElementById("hdnProductSelectedRec");
            var hdnSelectedCompany = document.getElementById('hdnSelectedCompany');
            var hdnSelectedDepartment = document.getElementById('hdnSelectedDepartment');
            var hdnSelectedGroupSet = document.getElementById('hdnSelectedGroupSet');
            var hdnSelectedImage = document.getElementById('hdnSelectedImage');
            var hdnSelectedUser = document.getElementById('hdnSelectedUser');
            var hdnRequestSelectedRec = document.getElementById('hdnRequestSelectedRec');
            var hdnSelectedStatus = document.getElementById('hdnSelectedStatus');
            var hdnUserSelectedRec = document.getElementById('hdnUserSelectedRec');
            var hdnAlluser = document.getElementById('hdnAlluser');
            var hdnSelectedRole = document.getElementById('hdnSelectedRole');
            var hdnSelectedActive = document.getElementById('hdnSelectedActive');

            var SelectedPart = document.getElementById("hdnProductSelectedRec").value;
            var SelectedReq = document.getElementById("hdnRequestSelectedRec").value;
            var SelectedIssue = document.getElementById("hdnIssueSelectedRec").value;
            var SelectedReceipt = document.getElementById("hdnReceiptSelectedRec").value;
            var SelectedConsumption = document.getElementById("hdnEngineSelectedRec").value;
            var SiteIDs = document.getElementById("hfCount").value;
            var txtFromDt = getDateTextBoxFromUC("<%= FrmDate.ClientID %>");
            var txtToDt = getDateTextBoxFromUC("<%= To_Date.ClientID %>");
            var SelectedCategory = document.getElementById("hdnProductCategory").value;
            var hdnAllReq = document.getElementById("hdnAllReq");
            var hdnAllPrd = document.getElementById("hdnAllPrd");
            var hdnAllIsue = document.getElementById("hdnAllIsue");
            var hdnAllRecpt = document.getElementById("hdnAllRecpt");
            var hdnAllEng = document.getElementById("hdnAllEng");
            var hdnExcludeZero = document.getElementById("hdnExcludeZero");

            var ddlFrmSite = document.getElementById("<%=ddlFrmSite.ClientID %>");
            var ddlToSite = document.getElementById("<%=ddlToSite.ClientID %>");

            var hdnWithZero = document.getElementById("hdnWithZero");
            var hdnImgStatus = document.getElementById("hdnImgStatus");
            var hdnSelectedDriver = document.getElementById("hdnSelectedDriver");
            var hdnSelectedPaymentMode = document.getElementById("hdnSelectedPaymentMode");
            var hdnSelectedDeliveryType = document.getElementById("hdnSelectedDeliveryType");
            var hdnSelectedVendor = document.getElementById("hdnSelectedVendor");

            if (getParameterByName("invoker").toString() == "BomDetail") {
                if (hdnProductSelectedRec.value == "" && hdnAllPrd.value != "1") {
                    showAlert("Select Atleast One SKU", "Error", "#");
                }
                else if (hdnSelectedGroupSet.value == "No") {
                    showAlert("Select BOM Value Yes For BOM Detail Report...", "Error", "#");
                }
                else {
                    LoadingOn();
                    PageMethods.WMGetGWCBOMDetailsReportData(getParameterByName("invoker").toString(), hdnProductSelectedRec.value, hdnSelectedCompany.value, hdnSelectedDepartment.value, hdnSelectedGroupSet.value, hdnSelectedImage.value, hdnAllPrd.value, jsGetReportDataOnSuccess, null)
                }
            }

            if (getParameterByName("invoker").toString() == "imgaudit") {
                if (hdnImgStatus == "0") {
                    showAlert("Select Status", "Error", "#");
                }
                else if (hdnImgStatus.value == "Success") {
                    if (hdnProductSelectedRec.value == "" && hdnAllPrd.value != "1") {
                        showAlert("Select Atleast One SKU", "Error", "#");
                    }
                    else {
                        LoadingOn();
                        PageMethods.WMGetImageAudit(getParameterByName("invoker").toString(), txtFromDt.value, txtToDt.value, hdnProductSelectedRec.value, hdnSelectedCompany.value, hdnSelectedDepartment.value, hdnSelectedUser.value, hdnAllPrd.value, hdnImgStatus.value, jsGetReportDataOnSuccess, null)
                    }
                }
                else {
                    if (hdnProductSelectedRec.value == "" && hdnAllPrd.value != "1") {
                        showAlert("Select Atleast One SKU", "Error", "#");
                    } else {
                        LoadingOn();
                        PageMethods.WMGetImageAudit(getParameterByName("invoker").toString(), txtFromDt.value, txtToDt.value, hdnProductSelectedRec.value, hdnSelectedCompany.value, hdnSelectedDepartment.value, hdnSelectedUser.value, hdnAllPrd.value, hdnImgStatus.value, jsGetReportDataOnSuccess, null)
                    }
                }
            }

            //            if (getParameterByName("invoker").toString() == "SkuDetails") {
            //                LoadingOn();
            //                PageMethods.WMGetGWCSKUDetailsReportData(getParameterByName("invoker").toString(), txtFromDt.value, txtToDt.value, hdnRequestSelectedRec.value, hdnProductSelectedRec.value, hdnSelectedCompany.value, hdnSelectedDepartment.value, hdnAllReq.value, hdnAllPrd.value, hdnSelectedUser.value, hdnSelectedStatus.value, jsGetReportDataOnSuccess, null)
            //            }

            if (getParameterByName("invoker").toString() == "orderdetail") {
                if (hdnRequestSelectedRec.value == "") {
                    showAlert("Select Atleast One Order", "Error", "#");
                }
                else {
                    var SelOrder = hdnRequestSelectedRec.value;
                    var count = (SelOrder.match(/,/g) || []).length;
                    console.log(count);
                    if (count >= 1) {
                        showAlert("Select Only One Order", "Error", "#");
                    } else {
                        LoadingOn();
                        var hdnNewFDt = document.getElementById('hdnNewFDt');
                        var hdnNewTDt = document.getElementById('hdnNewTDt');
                        PageMethods.WMGetGWCOrderDetailsReportData(getParameterByName("invoker").toString(), txtFromDt.value, txtToDt.value, hdnRequestSelectedRec.value, hdnProductSelectedRec.value, hdnSelectedCompany.value, hdnSelectedDepartment.value, hdnAllReq.value, hdnAllPrd.value, hdnSelectedUser.value, hdnSelectedStatus.value, jsGetReportDataOnSuccess, null)
                    }
                }
            }

            if (getParameterByName("invoker").toString() == "orderlead") {
                if (hdnRequestSelectedRec.value == "" && hdnAllReq.value != "1") {
                    showAlert("Select Atleast One Order", "Error", "#");
                }
                else {
                    LoadingOn();
                    var hdnNewFDt = document.getElementById('hdnNewFDt');
                    var hdnNewTDt = document.getElementById('hdnNewTDt');
                    PageMethods.WMGetGWCOrderLeadReportData(getParameterByName("invoker").toString(), txtFromDt.value, txtToDt.value, hdnRequestSelectedRec.value, hdnProductSelectedRec.value, hdnSelectedCompany.value, hdnSelectedDepartment.value, hdnAllReq.value, hdnAllPrd.value, hdnSelectedUser.value, hdnSelectedStatus.value, jsGetReportDataOnSuccess, null)
                }
            }


            if (getParameterByName("invoker").toString() == "order") {
                if (hdnRequestSelectedRec.value == "" && hdnAllReq.value != "1") {
                    showAlert("Select Atleast One Order", "Error", "#");
                }
                else {
                    LoadingOn();
                    var hdnNewFDt = document.getElementById('hdnNewFDt');
                    var hdnNewTDt = document.getElementById('hdnNewTDt');
                    PageMethods.WMGetGWCOrderReportData(getParameterByName("invoker").toString(), txtFromDt.value, txtToDt.value, hdnRequestSelectedRec.value, hdnProductSelectedRec.value, hdnSelectedCompany.value, hdnSelectedDepartment.value, hdnAllReq.value, hdnAllPrd.value, hdnSelectedUser.value, hdnSelectedStatus.value, jsGetReportDataOnSuccess, null)
                }
            }         

            /***************/
            if (getParameterByName("invoker").toString() == "orderdelivery") {
                if (hdnRequestSelectedRec.value == "" && hdnAllReq.value != "1") {
                    showAlert("Select Atleast One Order", "Error", "#");
                }
                else {
                    LoadingOn();
                    var hdnNewFDt = document.getElementById('hdnNewFDt');
                    var hdnNewTDt = document.getElementById('hdnNewTDt');
                    PageMethods.WMGetGWCOrderDeliveryReport(getParameterByName("invoker").toString(), txtFromDt.value, txtToDt.value, hdnRequestSelectedRec.value, hdnProductSelectedRec.value, hdnSelectedCompany.value, hdnSelectedDepartment.value, hdnAllReq.value, hdnAllPrd.value, hdnSelectedDriver.value, hdnSelectedPaymentMode.value, jsGetReportDataOnSuccess, null)
                }
            }

            if (getParameterByName("invoker").toString() == "purchaseorder") {
                if (hdnRequestSelectedRec.value == "" && hdnAllReq.value != "1") {
                    showAlert("Select Atleast One Order", "Error", "#");
                }
                else {
                    LoadingOn();
                    var hdnNewFDt = document.getElementById('hdnNewFDt');
                    var hdnNewTDt = document.getElementById('hdnNewTDt');
                    PageMethods.WMGetPurchaseOrderReport(getParameterByName("invoker").toString(), txtFromDt.value, txtToDt.value, hdnRequestSelectedRec.value, hdnProductSelectedRec.value, hdnAllReq.value, hdnAllPrd.value, hdnSelectedVendor.value, hdnSelectedStatus.value, jsGetReportDataOnSuccess, null)
                }
            }

            if (getParameterByName("invoker").toString() == "sla") {
                if (hdnRequestSelectedRec.value == "" && hdnAllReq.value != "1") {
                    showAlert("Select Atleast One Order", "Error", "#");
                }
                else {
                    LoadingOn();
                    var hdnNewFDt = document.getElementById('hdnNewFDt');
                    var hdnNewTDt = document.getElementById('hdnNewTDt');
                    PageMethods.WMGetGWCSLAReport(getParameterByName("invoker").toString(), txtFromDt.value, txtToDt.value, hdnRequestSelectedRec.value, hdnProductSelectedRec.value, hdnSelectedCompany.value, hdnSelectedDepartment.value, hdnAllReq.value, hdnAllPrd.value, hdnSelectedStatus.value, hdnSelectedDriver.value, hdnSelectedDeliveryType.value, jsGetReportDataOnSuccess, null)
                }
            }
            if (getParameterByName("invoker").toString() == "totaldeliveryvstotalrequest") {
                if (hdnProductSelectedRec.value == "" && hdnAllPrd.value != "1") {
                    showAlert("Select Atleast One SKU", "Error", "#");
                }
                else {
                    LoadingOn();
                    var hdnNewFDt = document.getElementById('hdnNewFDt');
                    var hdnNewTDt = document.getElementById('hdnNewTDt');
                    PageMethods.WMGetGWCTotalDeliveryVsTotalRequestReport(getParameterByName("invoker").toString(), txtFromDt.value, txtToDt.value, hdnProductSelectedRec.value, hdnSelectedCompany.value, hdnSelectedDepartment.value, hdnAllPrd.value, jsGetReportDataOnSuccess, null)
                }
            }



            /***************/

            if (getParameterByName("invoker").toString() == "sku") {
                if (hdnProductSelectedRec.value == "" && hdnAllPrd.value != "1") {
                    showAlert("Select Atleast One SKU", "Error", "#");
                }
                else {
                    LoadingOn();
                    PageMethods.WMGetGWCSKUReportData(getParameterByName("invoker").toString(), hdnProductSelectedRec.value, hdnSelectedCompany.value, hdnSelectedDepartment.value, hdnSelectedGroupSet.value, hdnSelectedImage.value, hdnAllPrd.value, hdnWithZero.value, jsGetReportDataOnSuccess, null)
                }
            }

            if (getParameterByName("invoker").toString() == "SkuDetails") {
                if (hdnProductSelectedRec.value == "") {
                    showAlert("Select Atleast One SKU", "Error", "#");
                }
                else {
                    var SelOrder = hdnProductSelectedRec.value;
                    var count = (SelOrder.match(/,/g) || []).length;
                    console.log(count);
                    if (count >= 1) {
                        showAlert("Select Only One SKU", "Error", "#");
                    } else {
                        LoadingOn();
                        PageMethods.WMGetGWCSKUDetailsReportData(getParameterByName("invoker").toString(), hdnProductSelectedRec.value, hdnSelectedCompany.value, hdnSelectedDepartment.value, hdnSelectedGroupSet.value, hdnSelectedImage.value, hdnAllPrd.value, jsGetReportDataOnSuccess, null);
                    }
                }
            }



            if (getParameterByName("invoker").toString() == "user") {
                if (hdnUserSelectedRec.value == "" && hdnAlluser.value != "1") { showAlert("Select Atleast One User", "Error", "#"); }
                else {
                    LoadingOn();
                    PageMethods.WMGetGWCUserReportData(getParameterByName("invoker").toString(), hdnUserSelectedRec.value, hdnSelectedCompany.value, hdnSelectedDepartment.value, hdnAlluser.value, hdnSelectedRole.value, hdnSelectedActive.value, jsGetReportDataOnSuccess, null)
                }
            }

            if (getParameterByName("invoker").toString() == "partrequest") {
                if (SiteIDs == "") { showAlert("Select Site ", "Error", "#"); }
                else if (SelectedReq == "") { showAlert("Select atleast one Request", "Error", "#"); }
                else if (SelectedPart == "") { showAlert("Select atleast one part", "Error", "#"); }
                else {
                    LoadingOn();
                    PageMethods.WMGetReportData(getParameterByName("invoker").toString(), SelectedPart, SelectedReq, txtFromDt.value, txtToDt.value, SiteIDs, hdnAllReq.value, hdnAllPrd.value, jsGetReportDataOnSuccess, null)
                }
            }
            if (getParameterByName("invoker").toString() == "partissue") {
                if (SelectedIssue == "") { showAlert("Select atleast one Issue", "Error", "#"); }
                else if (SelectedPart == "") { showAlert("Select atleast one part", "Error", "#"); }
                else {
                    LoadingOn();
                    PageMethods.WMGetReportData(getParameterByName("invoker").toString(), SelectedPart, SelectedIssue, txtFromDt.value, txtToDt.value, SiteIDs, hdnAllIsue.value, hdnAllPrd.value, jsGetReportDataOnSuccess, null)
                }
            }
            if (getParameterByName("invoker").toString() == "partreceipt") {
                if (SelectedReceipt == "") { showAlert("Select atleast one Receipt", "Error", "#"); }
                else if (SelectedPart == "") { showAlert("Select atleast one part", "Error", "#"); }
                else {
                    LoadingOn();
                    PageMethods.WMGetReportData(getParameterByName("invoker").toString(), SelectedPart, SelectedReceipt, txtFromDt.value, txtToDt.value, SiteIDs, hdnAllRecpt.value, hdnAllPrd.value, jsGetReportDataOnSuccess, null)
                }
            }
            if (getParameterByName("invoker").toString() == "partconsumption") {
                if (SelectedConsumption == "") { showAlert("Select atleast one Engine", "Error", "#"); }
                else if (SelectedPart == "") { showAlert("Select atleast one part", "Error", "#"); }
                else {
                    LoadingOn();
                    PageMethods.WMGetReportData(getParameterByName("invoker").toString(), SelectedPart, SelectedConsumption, txtFromDt.value, txtToDt.value, SiteIDs, hdnAllEng.value, hdnAllPrd.value, jsGetReportDataOnSuccess, null)
                }
            }
            if (getParameterByName("invoker").toString() == "monthly") {
                if (SiteIDs == "") { showAlert("Select Site ", "Error", "#"); }
                else {
                    LoadingOn();
                    PageMethods.WMGetReportData(getParameterByName("invoker").toString(), "", "", txtFromDt.value, txtToDt.value, SiteIDs, "", "", jsGetReportDataOnSuccess, null)
                }
            }
            if (getParameterByName("invoker").toString() == "weeklylst") {
                if (SiteIDs == "") { showAlert("Select Site ", "Error", "#"); }
                else {
                    LoadingOn();
                    PageMethods.WMGetReportData(getParameterByName("invoker").toString(), "", "", txtFromDt.value, txtToDt.value, SiteIDs, "", "", jsGetReportDataOnSuccess, null)
                }
            }
            if (getParameterByName("invoker").toString() == "consumabletracker") {
                if (SelectedCategory == "") { showAlert("Select Atleast one Product Category", "Error", "#"); }
                else if (SiteIDs == "") { showAlert("Select Site ", "Error", "#"); }
                else {
                    LoadingOn();
                    PageMethods.WMGetReportData(getParameterByName("invoker").toString(), "", SelectedCategory, txtFromDt.value, txtToDt.value, SiteIDs, "", "", jsGetReportDataOnSuccess, null)
                }
            }
            if (getParameterByName("invoker").toString() == "productdtl") {
                if (SelectedPart == "") { showAlert("Select Atleast one Part ", "Error", "#"); }
                else if (SiteIDs == "") { showAlert("Select Site ", "Error", "#"); }
                else {
                    LoadingOn();
                    PageMethods.WMGetReportData(getParameterByName("invoker").toString(), SelectedPart, "", txtFromDt.value, txtToDt.value, SiteIDs, hdnExcludeZero.value, hdnAllPrd.value, jsGetReportDataOnSuccess, null)
                }
            }
            if (getParameterByName("invoker").toString() == "transfer") {
                if (ddlFrmSite == "") { showAlert("Select From Site ", "Error", "#"); }
                else if (ddlToSite == "") { showAlert("Select To Site ", "Error", "#"); }
                else {
                    LoadingOn();
                    PageMethods.WMGetReportData(getParameterByName("invoker").toString(), ddlToSite.value, ddlFrmSite.value, txtFromDt.value, txtToDt.value, "", "", "", jsGetReportDataOnSuccess, null)
                }
            }
            if (getParameterByName("invoker").toString() == "asset") {
                if (ddlFrmSite == "") {
                    showAlert("Select From Site ", "Error", "#");
                }
                else if (ddlToSite == "") { showAlert("Select To Site ", "Error", "#"); }
                else {
                    LoadingOn();
                    PageMethods.WMGetReportData(getParameterByName("invoker").toString(), ddlToSite.value, ddlFrmSite.value, txtFromDt.value, txtToDt.value, "", "", "", jsGetReportDataOnSuccess, null)
                }
            }
        }

        function jsGetReportDataOnSuccess(result) {
            if (parseInt(result) > 0) {
                ShowReportOn();
            }
            else {
                showAlert("Data Not Available... ", "Error", "#");
                LoadingOff();
                ShowReportOff();
            }
        }
        function ShowReportOn() {
            document.getElementById("iframePORRpt").src = "../POR/Reports/ReportViewer.aspx";
            divPopUp.className = "divDetailExpandPopUpOn";
        }

        function ShowReportOff() {
            divPopUp.className = "divDetailExpandPopUpOff";
            LoadingOff();
        }

        function jsGetSKUTransactionReport() {
            var hdnProductSelectedRec = document.getElementById("hdnProductSelectedRec");
            var hdnSelectedCompany = document.getElementById('hdnSelectedCompany');
            var hdnSelectedDepartment = document.getElementById('hdnSelectedDepartment');
            var hdnSelectedGroupSet = document.getElementById('hdnSelectedGroupSet');
            var hdnSelectedImage = document.getElementById('hdnSelectedImage');
            var hdnSelectedUser = document.getElementById('hdnSelectedUser');

            var txtFromDt = getDateTextBoxFromUC("<%= FrmDate.ClientID %>");
            var txtToDt = getDateTextBoxFromUC("<%= To_Date.ClientID %>");
            var hdnProductSelectedRec = document.getElementById("<%=hdnProductSelectedRec.ClientID%>");

            if (hdnProductSelectedRec.value == "") {
                showAlert("Select Atleast One SKU", "Error", "#");
            }
            else {
                var SelOrder = hdnProductSelectedRec.value;
                var count = (SelOrder.match(/,/g) || []).length;
                console.log(count);
                if (count >= 1) {
                    showAlert("Select Only One SKU", "Error", "#");
                } else {
                    LoadingOn();
                    PageMethods.WMGetGWCSKUTransactionReportData(txtFromDt.value, txtToDt.value, hdnProductSelectedRec.value, hdnSelectedCompany.value, hdnSelectedDepartment.value, hdnSelectedGroupSet.value, hdnSelectedImage.value, jsGetGWCSKUTransactionReportOnSuccess, null);
                }
            }
        }
        function jsGetGWCSKUTransactionReportOnSuccess(result) {
            if (parseInt(result) > 0) {
                ShowReportOn();
            }
            else {
                showAlert("Data Not Available... ", "Error", "#");
                LoadingOff();
                ShowReportOff();
            }
        }

        function jsGetUserTransactionReport() {
            var hdnProductSelectedRec = document.getElementById("hdnProductSelectedRec");
            var hdnSelectedCompany = document.getElementById('hdnSelectedCompany');
            var hdnSelectedDepartment = document.getElementById('hdnSelectedDepartment');
            var hdnSelectedGroupSet = document.getElementById('hdnSelectedGroupSet');
            var hdnSelectedImage = document.getElementById('hdnSelectedImage');
            var hdnSelectedUser = document.getElementById('hdnSelectedUser');
            var hdnAlluser = document.getElementById('hdnAlluser');
            var hdnUserSelectedRec = document.getElementById('hdnUserSelectedRec');

            var txtFromDt = getDateTextBoxFromUC("<%= FrmDate.ClientID %>");
            var txtToDt = getDateTextBoxFromUC("<%= To_Date.ClientID %>");
            if (hdnUserSelectedRec.value == "" && hdnAlluser.value != "1") { showAlert("Select Atleast One User", "Error", "#"); }
            else {
                var Selusr = hdnUserSelectedRec.value;
                var count = (Selusr.match(/,/g) || []).length;
                console.log(count);
                if (count >= 1) {
                    showAlert("Select Only One User", "Error", "#");
                } else {
                    LoadingOn();
                    PageMethods.WMGetGWCUserTransactionReportData(hdnUserSelectedRec.value, jsGetReportDataOnSuccess, null)
                }
            }

        }

        function div2(obj) {
            var hdnProductCategory = document.getElementById('hdnProductCategory');
            hdnProductCategory.value = obj.options[obj.selectedIndex].value;
        }



        function SelectAllRequest(chk) {
            var hdnAllReq = document.getElementById("hdnAllReq");
            hdnAllReq.value = "0";
            if (chk.checked == true) {
                hdnAllReq.value = "1";
                document.getElementById("hdnRequestSelectedRec").value = "0";
                GVRequestInfo.refresh();
            }
            else {
                deselectAll(GVRequestInfo);
                document.getElementById("hdnRequestSelectedRec").value = "";
            }
        }

        function deselectAll(gv) {
            for (i = 0; i < gv.Rows.length; i++) {
                gv.deselectRecord(i);
            }
            gv.SelectedRecordsContainer.value = "";
            gv.SelectedRecords = new Array();
        }

        function SelectAllPurchaseOrder(chk) {
            var hdnAllReq = document.getElementById("hdnAllReq");
            hdnAllReq.value = "0";
            if (chk.checked == true) {
                hdnAllReq.value = "1";
                document.getElementById("hdnRequestSelectedRec").value = "0";
                GVProductInfo.refresh();
            }
            else {
                deselectAll(GVProductInfo);
                document.getElementById("hdnRequestSelectedRec").value = "";
                GVProductInfo.refresh();
            }
        }

   

        function SelectAllProduct(chk) {
            var hdnAllPrd = document.getElementById("hdnAllPrd");
            hdnAllPrd.value = "0";
            if (chk.checked == true) {
                hdnAllPrd.value = "1";
                document.getElementById("hdnProductSelectedRec").value = "0";
                GVProductInfo.refresh();
            }
            else {
                deselectAll(GVProductInfo);
                document.getElementById("hdnProductSelectedRec").value = "";
            }
        }

        function SelectAllEngine(chk) {
            var hdnAllEng = document.getElementById("hdnAllEng");
            hdnAllEng.value = "0";
            if (chk.checked == true) {
                hdnAllEng.value = "1";
                document.getElementById("hdnEngineSelectedRec").value = "0";
                GVEngineInfo.refresh();
            }
            else {
                deselectAll(GVEngineInfo);
                document.getElementById("hdnEngineSelectedRec").value = "";
            }
        }

        function SelectAllIssue(chk) {
            var hdnAllIsue = document.getElementById("hdnAllIsue");
            hdnAllIsue.value = "0";
            if (chk.checked == true) {
                hdnAllIsue.value = "1";
                document.getElementById("hdnIssueSelectedRec").value = "0";
                GVIssueInfo.refresh();
            }
            else {
                deselectAll(GVIssueInfo);
                document.getElementById("hdnIssueSelectedRec").value = "";
            }
        }

        function SelectAllReceipt(chk) {
            var hdnAllRecpt = document.getElementById("hdnAllRecpt");
            hdnAllRecpt.value = "0";
            if (chk.checked == true) {
                hdnAllRecpt.value = "1";
                document.getElementById("hdnReceiptSelectedRec").value = "0";
                GVReceiptInfo.refresh();
            }
            else {
                deselectAll(GVReceiptInfo);
                document.getElementById("hdnReceiptSelectedRec").value = "";
            }
        }

       


     
        function ExcludeZero(chk) {
            var hdnExcludeZero = document.getElementById("hdnExcludeZero");
            hdnExcludeZero.value = "0";
            if (chk.checked == true) {
                hdnExcludeZero.value = "1";
            }
        }

        function WithZroBalance(chk) {
            var hdnWithZero = document.getElementById("hdnWithZero");
            hdnWithZero.value = "1";
            if (chk.checked == true) {
                hdnWithZero.value = "0";
            }
        }

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

        }

        function SelectAllUser(chk) {
            var hdnAlluser = document.getElementById("hdnAlluser");
            hdnAlluser.value = "0";
            if (chk.checked == true) {
                hdnAlluser.value = "1";
                document.getElementById("hdnUserSelectedRec").value = "0";
                GVUserInfo.refresh();
            }
            else {
                deselectAll(GVReceiptInfo);
                document.getElementById("hdnReceiptSelectedRec").value = "";
            }
        }


        //       All Reports selction criteria dropdown logic

        function GetRole(obj) {
            var hdnSelectedRole = document.getElementById('hdnSelectedRole');
            hdnSelectedRole.value = obj.options[obj.selectedIndex].value;
        }

        function GetActive(obj) {
            var hdnSelectedActive = document.getElementById('hdnSelectedActive');
            hdnSelectedActive.value = obj.options[obj.selectedIndex].value;
        }

        function GetImgeStatus(obj) {
            var ddlImgstatus = document.getElementById("<%=ddlImgstatus.ClientID %>");
            var hdnImgStatus = document.getElementById('hdnImgStatus');
            hdnImgStatus.value = obj.options[obj.selectedIndex].value;
        }

        function GetStatus(obj) {
            var hdnSelectedStatus = document.getElementById('hdnSelectedStatus');
            hdnSelectedStatus.value = obj.options[obj.selectedIndex].value;

        }

        function GetUser(obj) {
            var hdnSelectedUser = document.getElementById('hdnSelectedUser');
            hdnSelectedUser.value = obj.options[obj.selectedIndex].value;
        }

        function GetGroupSet(obj) {
            var hdnSelectedGroupSet = document.getElementById('hdnSelectedGroupSet');
            hdnSelectedGroupSet.value = obj.options[obj.selectedIndex].value;
        }

        function GetImage() {

            var ddlImage = document.getElementById("<%=ddlImage.ClientID %>");

            var hdnSelectedImage = document.getElementById('hdnSelectedImage');
            hdnSelectedImage.value = ddlImage.value;
        }
        function GetCompany(obj) {
            var ddlcompany = document.getElementById("<%=ddlcompany.ClientID %>");
            var hdnImgStatus = document.getElementById('hdnImgStatus');
            var hdnSelectedCompany = document.getElementById('hdnSelectedCompany');
            hdnImgStatus.value = obj.options[obj.selectedIndex].value;
            hdnSelectedCompany.value = ddlcompany.value;
        }
        function GetDepartment() {
            var ddldepartment = document.getElementById("<%=ddldepartment.ClientID %>");
            var hdnSelectedDepartment = document.getElementById('hdnSelectedDepartment');
            hdnSelectedDepartment.value = ddldepartment.value;
        }
        function BindDepartment() {
            var hdnSelectedCompany = document.getElementById('hdnSelectedCompany');
            PageMethods.PMGetDepartmentList(hdnSelectedCompany.value, onSuccessGetDepartmentList, null)
            // PageMethods.PMGetDepartmentList(hdnSelectedCompany.value, null, null)
        }
        /**********/
        function GetDriver(obj) {

            var hdnSelectedDriver = document.getElementById('hdnSelectedDriver');
            hdnSelectedDriver.value = obj.options[obj.selectedIndex].value;

            //var ddlDriver = document.getElementById("<%=ddlDriver.ClientID %>");
            //var hdnSelectedDriver = document.getElementById('hdnSelectedDriver');
            //hdnSelectedDriver.value = ddlDriver.value;
        }

        function GetPaymentMode() {
            var ddlPytMode = document.getElementById("<%=ddlPytMode.ClientID %>");
            var hdnSelectedPaymentMode = document.getElementById('hdnSelectedPaymentMode');
            hdnSelectedPaymentMode.value = ddlPytMode.value;
        }

        function GetVendor() {
            var ddlVendor = document.getElementById("<%=ddlVendor.ClientID %>");
            var hdnSelectedVendor = document.getElementById('hdnSelectedVendor');
            hdnSelectedVendor.value = ddlVendor.value;
        }

        function GetDeliveryType() {
            var ddlDlrytype = document.getElementById("<%=ddlDlrytype.ClientID %>");
            var hdnSelectedDeliveryType = document.getElementById('hdnSelectedDeliveryType');
            hdnSelectedDeliveryType.value = ddlDlrytype.value;
        }
        /*******/
        function BindUser() {
            var ddldepartment = document.getElementById("<%=ddldepartment.ClientID %>");
            var hdnSelectedDepartment = document.getElementById('hdnSelectedDepartment');
            hdnSelectedDepartment.value = ddldepartment.value;
            var ddlcompany = document.getElementById("<%=ddlcompany.ClientID %>");
            var hdnSelectedCompany = document.getElementById('hdnSelectedCompany');
            hdnSelectedCompany.value = ddlcompany.value;

            PageMethods.WMGetUserList(hdnSelectedDepartment.Value, hdnSelectedCompany.value, OnSuccessGetUserLst, null)
        }

        function PaymentMethod() {
            ddldepartment = document.getElementById("<%=ddldepartment.ClientID %>");
            var Dept = ddldepartment.value;
            var hdnselectedDept = document.getElementById('hdnSelectedDepartment');
            hdnselectedDept.value = ddldepartment.value;
            PageMethods.WMGetPaymentMethod(Dept, onSuccessGetPaymentMEthod, null);
        }

        function onSuccessGetPaymentMEthod(result) {
            var ddlPytMode = document.getElementById("<%=ddlPytMode.ClientID %>");
            ddlPytMode.options.length = 0;
            var option0 = document.createElement("option");

            if (result.length > 0) {
                option0.text = '-Select All-';
                option0.value = '0';
            }
            else {
                option0.text = 'N/A';
                option0.value = '0';
            }

            try {
                ddlPytMode.add(option0, null);
            }
            catch (Error) {
                ddlPytMode.add(option0);
            }

            for (var i = 0; i < result.length; i++) {
                var option1 = document.createElement("option");
                option1.text = result[i].MethodName;
                option1.value = result[i].PMethodID;
                try {
                    ddlPytMode.add(option1, null);
                }
                catch (Error) {
                    ddlPytMode.add(option1);
                }
            }

        }


        function OnSuccessGetUserLst(result) {
            var ddlUser = document.getElementById("<%=ddlUser.ClientID %>");
            ddlUser.options.length = 0;
            var option0 = document.createElement("option");
            if (result.length > 0) {
                option0.text = "-Select All-";
                option0.value = '0';
            } else {
                option0.text == 'N/A';
                option0.value = '0';
            }

            try {
                ddlUser.add(option0, null);
            }
            catch (Error) {
                ddlUser.add(option0);
            }
            for (var i = 0; i < result.length; i++) {
                var option1 = document.createElement("option");
                option1.text = result[i].Name;
                option1.value = result[i].ID;
                try {
                    ddlUser.add(option1, null);
                }
                catch (Error) {
                    ddlUser.add(option1);
                }
            }
        }
        function onSuccessGetGroupSetListByCompanyId(result) {
            ddlgset = document.getElementById("<%=ddlgset.ClientID %>");
            ddlgset.options.length = 0;
            var option0 = document.createElement("option");

            if (result.length > 0) {
                option0.text = "-Select All-";
                option0.value = '0';
            }
            else {
                option0.text = 'N/A';
                option0.value = '0';
            }

            try {
                ddlgset.add(option0, null);
            }
            catch (Error) {
                ddlgset.add(option0);
            }

            for (var i = 0; i < result.length; i++) {
                var option1 = document.createElement("option");
                option1.text = result[i].Description;
                option1.value = result[i].ID;
                try {
                    ddlgset.add(option1, null);
                }
                catch (Error) {
                    ddlgset.add(option1);
                }
            }

        }


        function onSuccessGetDepartmentList(result) {

            ddlDepartment = document.getElementById("<%=ddldepartment.ClientID %>");
            var hdnSelectedCompany = document.getElementById("<%=hdnSelectedCompany.ClientID %>");
            ddlDepartment.options.length = 0;
            var option0 = document.createElement("option");

            if (result.length > 0) {

                option0.text = "-Select All-";
                option0.value = "0";

            }
            else {
                if (hdnSelectedCompany.value == "0") {

                    PageMethods.PMGetAllDepartment(onSuccessGetAllDepartmentList, null)
                    function onSuccessGetAllDepartmentList(result) {
                        ddlDepartment = document.getElementById("<%=ddldepartment.ClientID %>");
                        ddlDepartment.options.length = 0;
                        var option0 = document.createElement("option");
                        if (result.length > 0) {
                            option0.text = "Select All";
                            option0.value = "0";
                        }
                        try {
                            ddlDepartment.add(option0, null);
                        }
                        catch (Error) {
                            ddlDepartment.add(option0);
                        }

                        for (var i = 0; i < result.length; i++) {
                            var option1 = document.createElement("option");
                            option1.text = result[i].Territory;
                            option1.value = result[i].ID;
                            try {
                                ddlDepartment.add(option1, null);
                            }
                            catch (Error) {
                                ddlDepartment.add(option1);
                            }
                        }

                    }

                    //PageMethods.PMGetAllGroupsetList(onSuccessGetAllGroupSetList, null)
                    function onSuccessGetAllGroupSetList(result) {
                        ddlgset = document.getElementById("<%=ddlgset.ClientID %>");
                        ddlgset.options.length = 0;
                        var option0 = document.createElement("option");
                        if (result.length > 0) {
                            option0.text = "Select All";
                            option0.value = "0";
                        }
                        try {
                            ddlgset.add(option0, null);
                        }
                        catch (Error) {
                            ddlgset.add(option0);
                        }

                        for (var i = 0; i < result.length; i++) {
                            var option1 = document.createElement("option");
                            option1.text = result[i].Description;
                            option1.value = result[i].ID;
                            try {
                                ddlgset.add(option1, null);
                            }
                            catch (Error) {
                                ddlgset.add(option1);
                            }
                        }

                    }


                    }
                    else {
                        option0.text = "N/A"; option0.value = "0";
                    }
                }

                try {
                    ddlDepartment.add(option0, null);

                }
                catch (error) {
                    ddlDepartment.add(option0);
                }

                for (i = 0; i < result.length; i++) {
                    var option1 = document.createElement("option");

                    option1.text = result[i].Territory;
                    option1.value = result[i].ID;

                    try {
                        ddlDepartment.add(option1, null);
                    }
                    catch (error) { ddlDepartment.add(option1); }
                }
            }

            function BindGroupSet() {

                // PageMethods.PMGetGroupsetList(hdnSelectedCompany.value, hdnSelectedDepartment.value, onSuccessGetGroupSetList, null)
            }

            function onSuccessGetGroupSetList(result) {

                ddlgset = document.getElementById("<%=ddlgset.ClientID %>");
                ddlgset.options.length = 0;
                var option0 = document.createElement("option");

                if (result.length > 0) {

                    option0.text = "-Select All-";
                    option0.value = "0";

                }
                else {

                    if (hdnSelectedDepartment.value == "1") {

                        // PageMethods.PMGetAllGroupsetList(onSuccessGetAllGroupSetList, null)
                        function onSuccessGetAllGroupSetList(result) {
                            ddlgset = document.getElementById("<%=ddlgset.ClientID %>");
                            ddlgset.options.length = 0;
                            var option0 = document.createElement("option");
                            if (result.length > 0) {
                                option0.text = "-Select All-";
                                option0.value = "0";
                            }
                            try {
                                ddlgset.add(option0, null);
                            }
                            catch (Error) {
                                ddlgset.add(option0);
                            }

                            for (var i = 0; i < result.length; i++) {
                                var option1 = document.createElement("option");
                                option1.text = result[i].Description;
                                option1.value = result[i].ID;
                                try {
                                    ddlgset.add(option1, null);
                                }
                                catch (Error) {
                                    ddlgset.add(option1);
                                }
                            }

                        }

                        }
                        else if (hdnSelectedDepartment.value > 1) {
                            // PageMethods.PMGetGroupsetListByDept(hdnSelectedDepartment.value, onSuccessGetGroupsetListByDept, null)
                            PageMethods.PMGetGroupsetListByDept(hdnSelectedDepartment.value, null, null)
                            function onSuccessGetGroupsetListByDept(result) {
                                ddlgset = document.getElementById("<%=ddlgset.ClientID %>");
                            ddlgset.options.length = 0;
                            var option0 = document.createElement("option");
                            if (result.length > 0) {

                                option0.text = "-Select All-";
                                option0.value = "0";
                            }
                            else {

                                option0.text = "N/A"; option0.value = "0";
                            }
                            try {
                                ddlgset.add(option0, null);
                            }
                            catch (Error) {
                                ddlgset.add(option0);
                            }

                            for (var i = 0; i < result.length; i++) {
                                var option1 = document.createElement("option");
                                option1.text = result[i].Description;
                                option1.value = result[i].ID;
                                try {
                                    ddlgset.add(option1, null);
                                }
                                catch (Error) {
                                    ddlgset.add(option1);
                                }
                            }
                        }

                        }
                        else {
                            option0.text = "N/A"; option0.value = "0";
                        }
                }

                try {
                    ddlgset.add(option0, null);

                }
                catch (error) {
                    ddlgset.add(option0);
                }

                for (i = 0; i < result.length; i++) {
                    var option1 = document.createElement("option");

                    option1.text = result[i].Description;
                    option1.value = result[i].Id;

                    try {
                        ddlgset.add(option1, null);
                    }
                    catch (error) { ddlgset.add(option1); }
                }
            }

            //        function BindUser() {
            //            PageMethods.PMGetUserList(hdnSelectedCompany.value, hdnSelectedDepartment.value, OnSuccessBinduser, null);
            //        }

            function OnSuccessBinduser(result) {
                ddlUser = document.getElementById("<%=ddlUser.ClientID %>");
                ddlUser.options.length = 0;
                var option0 = document.createElement("option");

                if (result.length > 0) {
                    option0.text = '-Select All-';
                    option0.value = '0';
                }
                else {
                    option0.text = 'N/A';
                    option0.value = '0';
                }

                try {
                    ddlUser.add(option0, null);
                }
                catch (Error) {
                    ddlUser.add(option0);
                }

                for (var i = 0; i < result.length; i++) {
                    var option1 = document.createElement("option");
                    option1.text = result[i].Name;
                    option1.value = result[i].ID;
                    try {
                        ddlUser.add(option1, null);
                    }
                    catch (Error) {
                        ddlUser.add(option1);
                    }
                }
            }

            //       End dropdown logic

            function div6(obj) {

                if (getParameterByName('invoker') == "sku") {
                    var hdnSKU = document.getElementById('hdnSKU');
                    hdnSKU.value = "sku";
                    GVProductInfo.refresh();
                }
                if (getParameterByName('invoker') == "order") {
                    GVRequestInfo.refresh();
                }
                if (getParameterByName('invoker') == "orderdetail") {
                    GVRequestInfo.refresh();
                }
                if (getParameterByName('invoker') == "SkuDetails") {
                    var hdnSKU = document.getElementById('hdnSKU');
                    hdnSKU.value = "sku";
                    GVProductInfo.refresh();
                }
                if (getParameterByName('invoker') == "BomDetail") {
                    var hdnSKU = document.getElementById('hdnSKU');
                    hdnSKU.value = "sku";
                    GVProductInfo.refresh();
                }
                if (getParameterByName('invoker') == "orderlead") {
                    GVRequestInfo.refresh();
                }
                if (getParameterByName('invoker') == "user") {
                    var hdnUser = document.getElementById('hdnUser');
                    hdnUser.value = "1";
                    GVUserInfo.refresh();
                }
                if (getParameterByName('invoker') == "imgaudit") {
                    var hdnSKU = document.getElementById('hdnSKU');
                    hdnSKU.value = "sku";
                    GVProductInfo.refresh();
                }

                if (getParameterByName('invoker') == "orderdelivery") {
                    GVRequestInfo.refresh();
                }
                if (getParameterByName('invoker') == "sla") {
                    GVRequestInfo.refresh();
                }
                if (getParameterByName('invoker') == "totaldeliveryvstotalrequest") {
                    var hdnSKU = document.getElementById('hdnSKU');
                    hdnSKU.value = "sku";
                    GVProductInfo.refresh();
                }

                if (getParameterByName('invoker') == "purchaseorder") {
                    GVpurchaseInfo.refresh();
                }
            }

            function divImg(obj) {
                if (getParameterByName('invoker') == "imgaudit") {
                    var hdnSKU = document.getElementById('hdnSKU');
                    hdnSKU.value = "sku";
                    GVProductInfo.refresh();
                }
            }

            function div7(obj) {

                //            if (getParameterByName('invoker') == "order") {

                //                GVProductInfo.refresh();
                //            }

                if (getParameterByName('invoker') == "user") {
                    var hdnUser = document.getElementById('hdnUser');
                    hdnUser.value = "1";
                    GVUserInfo.refresh();
                }
            }




    </script>
</div>

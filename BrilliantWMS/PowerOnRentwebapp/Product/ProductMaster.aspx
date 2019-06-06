<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/CRM.Master" EnableEventValidation="false"
    AutoEventWireup="true" CodeBehind="ProductMaster.aspx.cs" Inherits="BrilliantWMS.Product.ProductMaster" %>

<%@ Register Assembly="obout_Flyout2_NET" Namespace="OboutInc.Flyout2" TagPrefix="obout" %>
<%@ Register Assembly="Obout.Ajax.UI" Namespace="Obout.Ajax.UI.FileUpload" TagPrefix="obout" %>
<%@ Register Assembly="obout_Interface" Namespace="Obout.Interface" TagPrefix="obout" %>
<%@ Register Assembly="obout_Grid_NET" Namespace="Obout.Grid" TagPrefix="obout" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../CommonControls/UCFormHeader.ascx" TagName="UCFormHeader" TagPrefix="uc1" %>
<%@ Register Src="../CommonControls/UCToolbar.ascx" TagName="UCToolbar" TagPrefix="uc2" %>
<%@ Register Src="../CommonControls/UC_Date.ascx" TagName="UC_Date" TagPrefix="uc8" %>
<%@ Register Src="../Document/UC_AttachDocument.ascx" TagName="UC_AttachDocument" TagPrefix="uc3" %>
<%@ Register Src="../CommonControls/UC_Date.ascx" TagName="UC_Date" TagPrefix="uc4" %>
<%@ Register Src="../Tax/UC_StatutoryDetails.ascx" TagName="UC_StatutoryDetails" TagPrefix="uc6" %>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHolder_FormHeader" runat="server">
    <uc2:UCToolbar ID="UCToolbar1" runat="server" />
    <uc1:UCFormHeader ID="UCFormHeader1" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPHolder_Form" runat="server">
    <asp:UpdatePanel ID="UpdatePanelTabPanelProductList" runat="server">
        <ContentTemplate>
            <asp:ValidationSummary ID="validationsummary_ProductMaster" runat="server" ShowMessageBox="true"
                ShowSummary="false" DisplayMode="bulletlist" ValidationGroup="Save" />
            <asp:HiddenField ID="hdnprodID" runat="server" />
            <asp:HiddenField ID="hdncompanyid" runat="server" />

            <asp:HiddenField ID="hdndeptid" runat="server" />
            <asp:HiddenField ID="hdnSelectedPrdId" runat="server" />
            <asp:HiddenField ID="hdnstate" runat="server" />
            <asp:HiddenField ID="hdnImgState" runat="server" />
            <asp:HiddenField ID="hdnProductSearchSelectedRec" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hdnproductsearchId" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hdnBomDetailId" runat="server" />
            <asp:HiddenField ID="hdnSelImage" runat="server" />
            <asp:TabContainer runat="server" ID="TabContainerProductMaster" ActiveTabIndex="4">
                <asp:TabPanel ID="TabPanelProductList" runat="server" HeaderText="SKU List">
                    <ContentTemplate>
                        <asp:UpdateProgress ID="UpdateGirdProductProcess" runat="server" AssociatedUpdatePanelID="Up_PnlGirdProduct">
                            <ProgressTemplate>
                                <center>
                                    <div class="modal">
                                        <img src="../App_Themes/Blue/img/ajax-loader.gif" style="top: 50%;" />
                                    </div>
                                </center>
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                        <asp:UpdatePanel ID="Up_PnlGirdProduct" runat="server">
                            <ContentTemplate>
                                <center>
                                    <table class="gridFrame" width="100%">
                                        <tr>
                                            <td>
                                                <table style="width: 100%">
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblskulist" Style="color: white; font-size: 15px; font-weight: bold;" runat="server" Text="SKU List"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <obout:Grid ID="grvProduct" runat="server" AutoGenerateColumns="False" AllowAddingRecords="False"
                                                    AllowFiltering="True" AllowGrouping="True" AllowMultiRecordSelection="False" 
                                                    AllowRecordSelection="true" Width="100%" AllowColumnResizing="true" AllowColumnReordering="true">
                                                    <ScrollingSettings ScrollHeight="250"/>
                                                    <Columns>
                                                        <obout:Column HeaderText="Edit" DataField="ID" Width="7%" Align="center" HeaderAlign="center">
                                                            <TemplateSettings TemplateId="TemplateEdit" />
                                                        </obout:Column>
                                                        <%--<obout:Column HeaderText="Type" DataField="ProductType" Width="20%" />--%>
                                                        <obout:Column HeaderText="WMS SKU Code" DataField="ProductCode" Width="20%" />
                                                        <obout:Column HeaderText="SKU Code" DataField="OMSSKUCode" Visible="false" Width="20%" />
                                                        <obout:Column HeaderText="SKU Name" DataField="Name" Width="20%" />
                                                        <obout:Column HeaderText="Description" DataField="description" Width="25%" />
                                                        <obout:Column HeaderText="Company" DataField="CompanyName" Width="19%" />
                                                        <obout:Column HeaderText="Customer" DataField="CustomerName" Width="15%" />
                                                        <obout:Column HeaderText="Group Set" DataField="GroupSet" Width="7%" Align="center" />
                                                        <%-- <obout:Column HeaderText="UOM" DataField="uom1" Width="15%" />--%>
                                                        <obout:Column HeaderText="Retail Price" DataField="PrincipalPrice" Width="12%" Align="right" HeaderAlign="center" />
                                                        <obout:Column HeaderText="MOQ" DataField="moq" Width="7%" Align="right" HeaderAlign="center" />
                                                          <obout:Column HeaderText="Virtual Quantity" DataField="VirtualQty" Width="10%" Align="right" HeaderAlign="center" Visible="false" />
                                                         <%--<obout:Column HeaderText="Available Virtual Qty" DataField="AvailVirtualQty" Width="12%" Align="right" HeaderAlign="center" />--%>
                                                        <obout:Column DataField="AvailableBalance" Width="14%" Align="right" HeaderAlign="center">
                                                            <%-- <TemplateSettings HeaderTemplateId="HeaderTempAmount" />--%>
                                                        </obout:Column>
                                                        <%--   <obout:Column HeaderText="Price"  DataField="PrincipalPrice" Width="30%" Align="right" />--%>
                                                        <obout:Column HeaderText="Active" HeaderAlign="center" DataField="active" Width="8%" Align="center">
                                                            <TemplateSettings TemplateId="tplActive" />
                                                        </obout:Column>
                                                    </Columns>
                                                    <Templates>
                                                        <obout:GridTemplate ID="HeaderTempAmount" runat="server">
                                                            <Template>
                                                                &nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp Price
                                                            </Template>
                                                        </obout:GridTemplate>
                                                        <obout:GridTemplate runat="server" ID="tplActive">
                                                            <Template>
                                                                <%# (Container.DataItem["Active"].ToString() == "Y" ? "Yes" : "No")%>
                                                            </Template>
                                                        </obout:GridTemplate>
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
                                </center>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </ContentTemplate>
                </asp:TabPanel>
                <asp:TabPanel ID="tabProductDetails" runat="server" HeaderText="SKU Details" >
                    <ContentTemplate>
                        <asp:UpdateProgress ID="UpdateProgress_ProductDetails" runat="server" AssociatedUpdatePanelID="Uppnl_productDetails">
                            <ProgressTemplate>
                                <center>
                                    <div class="modal">
                                        <img src="../App_Themes/Blue/img/ajax-loader.gif" style="top: 50%;" />
                                    </div>
                                </center>
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                        <asp:UpdatePanel ID="Uppnl_productDetails" runat="server">
                            <ContentTemplate>
                                <center>
                                    <table class="tableForm">
                                        <%--<tr>
                                            <td>
                                                <req>Product Type :</req>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlProductType" runat="server" DataTextField="ProductType"
                                                    DataValueField="ID" Width="200px" />
                                                <asp:RequiredFieldValidator ID="req_ddlProductType" runat="server" ErrorMessage="Select Product Type"
                                                    ControlToValidate="ddlProductType" InitialValue="0" ValidationGroup="Save" Display="None"></asp:RequiredFieldValidator>
                                            </td>
                                            <td>
                                                <req>Category :</req>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlCategory" runat="server" DataTextField="ProductCategory"
                                                    onchange="printProductSubCategory();" ClientIDMode="Static" DataValueField="ID"
                                                    Width="200px">
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="rfvddlCategory" runat="server" ErrorMessage="Select Category"
                                                    DataValueField="ID" DataTextField="ProductCategory" ControlToValidate="ddlCategory"
                                                    InitialValue="0" ValidationGroup="Save" Display="None"></asp:RequiredFieldValidator>
                                            </td>
                                            <td>
                                                <req> Sub Category :</req>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlSubCategory" runat="server" ClientIDMode="Static" Width="200px"
                                                    DataTextField="ProductSubCategory" DataValueField="ID">
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Select Sub Category"
                                                    ControlToValidate="ddlSubCategory" InitialValue="0" ValidationGroup="Save" Display="None"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>--%>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblskucode" runat="server" Text="SKU code"></asp:Label>
                                                :
                                            </td>
                                            <td style="text-align: left;">
                                                <asp:TextBox ID="txtProductCode" runat="server" MaxLength="50" Width="194px"></asp:TextBox>
                                            </td>
                                            <td>
                                                <req> <asp:Label ID="lblskuname" runat="server" Text="SKU Name"></asp:Label> </req>
                                                :
                                            </td>
                                            <td style="text-align: left">
                                                <asp:TextBox ID="txtProductName" runat="server" MaxLength="100" Width="194px"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="reqtxtProductName" runat="server" ErrorMessage="Enter Product Name"
                                                    ControlToValidate="txtProductName" ValidationGroup="Save" Display="None"></asp:RequiredFieldValidator>
                                            </td>
                                            <td>
                                                <req><asp:Label ID="lblCompany" runat="server" Text="Company"></asp:Label></req>
                                                :
                                            </td>
                                            <td style="text-align: left">
                                                <asp:DropDownList ID="ddlcompany" DataTextField="Name" DataValueField="ID" onchange="GetCustomer()"
                                                    Width="200px" runat="server">
                                                </asp:DropDownList>
                                                <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtdepartment"
                                                        Display="None" ErrorMessage="Please Enter Department Name" ValidationGroup="Save"></asp:RequiredFieldValidator>--%>
                                            </td>

                                        </tr>
                                        <tr>
                                            <td>
                                                <req><asp:Label ID="lbldepartment" runat="server" Text=" Customer"></asp:Label></req>
                                                :
                                            </td>
                                            <td style="text-align: left">
                                                <asp:DropDownList ID="ddldepartment"  DataTextField="Name" DataValueField="ID" onchange="Getdeptid()"
                                                    Width="200px" runat="server">
                                                </asp:DropDownList>
                                                <asp:HiddenField ID="hdnCustomerIDNew" runat="server" ClientIDMode="Static" />
                                                <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtdepartment"
                                                        Display="None" ErrorMessage="Please Enter Department Name" ValidationGroup="Save"></asp:RequiredFieldValidator>--%>
                                            </td>
                                             <td>
                                                <req>Category :</req>
                                            </td>
                                            <td style="text-align: left">
                                                <asp:DropDownList ID="ddlCategory" runat="server" DataTextField="Name" DataValueField="ID" onchange="printProductSubCategory();"
                                                     ClientIDMode="Static" Width="200px">
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="rfvddlCategory" runat="server" ErrorMessage="Select Category"
                                                    DataValueField="ID" DataTextField="ProductCategory" ControlToValidate="ddlCategory"
                                                    InitialValue="0" ValidationGroup="Save" Display="None"></asp:RequiredFieldValidator>
                                            </td>
                                            <td>
                                                 Sub Category :
                                            </td>
                                            <td style="text-align: left">
                                                <asp:DropDownList ID="ddlSubCategory" runat="server" ClientIDMode="Static" Width="200px" onchange="GetSubCategory()"
                                                    DataTextField="Name" DataValueField="ID">
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Select Sub Category"
                                                    ControlToValidate="ddlSubCategory" InitialValue="0" ValidationGroup="Save" Display="None"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                             <td>
                                                <asp:Label ID="lblomsskucode" runat="server" Text="Alias SKU Code"></asp:Label>
                                                :
                                            </td>
                                            <td style="text-align: left">
                                                <asp:TextBox ID="txtomsskucode" runat="server" MaxLength="50" Width="194px"></asp:TextBox>
                                            </td>
                                            <td>
                                                <req><asp:Label ID="lblcost" runat="server" Text="Cost"></asp:Label></req>
                                                :
                                            </td>
                                            <td style="text-align: left;">
                                                <asp:TextBox ID="txtcost" runat="server" MaxLength="15" Width="184px" Style="text-align: right;"
                                                    onkeydown="return AllowDecimal(this,event);" onkeypress="return AllowDecimal(this,event);"></asp:TextBox>

                                                <asp:DropDownList ID="ddlUOM" runat="server" DataTextField="Name" DataValueField="ID" Visible="false"
                                                    Width="187px">
                                                </asp:DropDownList>
                                                <%--<asp:RequiredFieldValidator ID="req_ddlUOM" runat="server" ErrorMessage="Please Select Unit"
                                                    ControlToValidate="ddlUOM" ValidationGroup="Save" Display="None" InitialValue="0"></asp:RequiredFieldValidator>--%>
                                            </td>
                                            <td>
                                                <req><asp:Label ID="lblpriciprice" runat="server" Text="Principal Price"></asp:Label></req>
                                                :
                                            </td>
                                            <td style="text-align: left;">
                                                <asp:TextBox ID="txtPrincipalPrice" runat="server" MaxLength="15" Width="194px" Style="text-align: right;"
                                                    onkeydown="return AllowDecimal(this,event);" onkeypress="return AllowDecimal(this,event);"></asp:TextBox>

                                                <span style="position: absolute; top: 0px; left: 0px; visibility: hidden;"><a id="changePrice1" runat="server" style="visibility: hidden;"></a></span>
                                            </td>
                                        </tr>
                                        <tr>
                                             <td>
                                                <asp:Label ID="lblgrpset" runat="server" Text="Group Set" />
                                                :
                                            </td>
                                            <td style="text-align: left;">    
                                                <asp:DropDownList ID="ddlBom" runat="server" ValidationGroup="Save" Width="200px" >      <%-- onchange="ShowHideMOQ()"--%>
                                                    <asp:ListItem Text="No" Value="0"></asp:ListItem>
                                                    <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                                </asp:DropDownList>                                       
                                            </td>
                                             <td>
                                                  <asp:Label ID="lblMOQ" runat="server" Text="MOQ" /> 
                                                :
                                            </td>
                                            <td style="text-align: left;">
                                                <asp:TextBox ID="txtMOQ" runat="server" Style="text-align: right" MaxLength="50" Text="1"
                                                 Width="180px" onkeypress="return fnAllowDigitsOnly(event)"></asp:TextBox>
                                            </td>
                                            <td>
                                                <req> <asp:Label ID="lblpickmethod" runat="server" Text="Picking Method" /> </req>
                                                :
                                            </td>
                                            <td style="text-align: left">
                                                <asp:DropDownList ID="ddlpickmethod" runat="server" ValidationGroup="Save" Width="200px">
                                                    <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                                    <asp:ListItem Text="FIFO" Value="1"></asp:ListItem>
                                                    <asp:ListItem Text="LIFO" Value="2"></asp:ListItem>
                                                    <asp:ListItem Text="Random" Value="3"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                         <tr>
                                              <td>
                                                <req><asp:Label ID="lblactive" runat="server" Text="Active"/></req>
                                                :
                                            </td>
                                            <td style="text-align: left">
                                                <obout:OboutRadioButton ID="rbtYes" runat="server" Text="Yes  " GroupName="rbtActive"
                                                    Checked="True" FolderStyle="">
                                                </obout:OboutRadioButton>
                                                &nbsp;&nbsp;
                                                    <obout:OboutRadioButton ID="rbtNo" runat="server" Text="No"
                                                        GroupName="rbtActive" FolderStyle="">
                                                    </obout:OboutRadioButton>
                                            </td>
                                             <td>
                                                <req><asp:Label ID="lblbarcode" runat="server" Text="Barcode"></asp:Label></req>
                                                :
                                            </td>
                                            <td style="text-align: left">
                                                <asp:Label ID="lblbarcodeshow" runat="server"  Style="font-family: '3 of 9 Barcode';
                                                    float: left;" Font-Size="XX-Large" Text=" "></asp:Label>      <%--Free 3 of 9 Extended--%>
                                            </td>
                                             <td>
                                                <asp:Label ID="Label1" runat="server" Text="Select Tax"></asp:Label>
                                             </td>
                                             <td>
                                                  <asp:DropDownList ID="ddlproducttax" runat="server" DataTextField="Name" DataValueField="ID" onchange="GetProductTax();"
                                                     ClientIDMode="Static" Width="200px">
                                                </asp:DropDownList>
                                             </td>
                                         </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lbldescri" runat="server" Text="Description"></asp:Label>
                                                :
                                            </td>
                                            <td style="text-align: left" colspan="4">
                                                <asp:TextBox ID="txtdescription" runat="server" TextMode="MultiLine" Width="600px"></asp:TextBox>
                                            </td>
                                            <td style="text-align: left">
                                                <asp:Button ID="btncustomernext" runat="server" CssClass="nextbutton" Text="  Next  " OnClick="btncustomernext_Click"
                                                 OnClientClick="return Checkvalidations();" />
                                            </td> 

                                        </tr>
                                        <%-- <tr>
                                            <td>
                                                Product Specification :
                                            </td>
                                            <td style="text-align: left;">
                                                <asp:CheckBoxList ID="chkProductSpe" runat="server" RepeatDirection="Vertical" RepeatColumns="2">
                                                    <asp:ListItem>Installable</asp:ListItem>
                                                    <asp:ListItem>AMC</asp:ListItem>
                                                </asp:CheckBoxList>
                                            </td>
                                            <td>
                                                Warranty :
                                            </td>
                                            <td style="text-align: left;">
                                                <asp:TextBox ID="txtWarrenyInDays" runat="server" Width="100px" MaxLength="5" Style="text-align: right;"
                                                    onkeydown="AllowDecimal(this,event);" onkeypress="AllowInt(this,event);"></asp:TextBox>
                                                <span class="watermark">( In Days )</span>
                                            </td>
                                            <td>
                                                Guarantee :
                                            </td>
                                            <td style="text-align: left;">
                                                <asp:TextBox ID="txtGuaranteeInDays" runat="server" Width="100px" MaxLength="5" Style="text-align: right;"
                                                    onkeydown="AllowDecimal(this,event);" onkeypress="AllowInt(this,event);"></asp:TextBox>
                                                <span class="watermark">( In Days )</span>
                                            </td>
                                        </tr>--%>
                                    </table>
                                    <table class="gridFrame" width="69%">
                                        <tr>
                                            <td>
                                                <table style="width: 70%">
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblspecifictn" CssClass="headerText" runat="server" Text="Price History" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <obout:Grid ID="Grid1" runat="server" AllowFiltering="True" AllowGrouping="True"
                                                    AllowColumnResizing="true" AutoGenerateColumns="False" Width="100%"
                                                    AllowAddingRecords="False" PageSize="6" OnInsertCommand="GVProductSpecification_InsertRecord" OnRebind="Grid1_RebindGrid">
                                                    <ScrollingSettings ScrollHeight="150" />
                                                    <Columns>
                                                        <%--<obout:Column ID="Column8" HeaderText="ID" DataField="ID" Width="10%" runat="server" Visible="false">
                                                          </obout:Column>--%>
                                                        <obout:Column ID="Column0" DataField="ID" HeaderText="Edit" Width="5%" AllowSorting="false" Index="2"
                                                            Align="center" HeaderAlign="center">
                                                            <TemplateSettings TemplateId="GridTemplate9" />
                                                        </obout:Column>
                                                        <obout:Column DataField="Sequence" HeaderText="Edit" Width="5%" AllowSorting="false" Visible="false"
                                                            Align="center" HeaderAlign="center">
                                                            <TemplateSettings TemplateId="GvTempEdit" />
                                                        </obout:Column>
                                                        <obout:Column ID="Column1" HeaderText="Start Date" DataField="SpecificationTitle" Width="30%"
                                                            runat="server" Align="center" HeaderAlign="Center">
                                                        </obout:Column>
                                                        <obout:Column ID="Column2" HeaderText="End Date" DataField="SpecificationDescription"
                                                            Width="30%" runat="server" Align="center" HeaderAlign="Center">
                                                        </obout:Column>
                                                         <obout:Column ID="Column8" HeaderText="Effective date" DataField="SpecificationDescription"
                                                            Width="30%" runat="server" Align="center" HeaderAlign="Center">
                                                        </obout:Column>
                                                        <obout:Column ID="Column3" HeaderText="Price" DataField="Active" Width="30%" runat="server" Align="center" HeaderAlign="Center">
                                                        </obout:Column>
                                                    </Columns>
                                                    <Templates>
                                                        <obout:GridTemplate ID="GridTemplate9" runat="server">
                                                            <Template>
                                                                <img id="imgBtnEdit" src="../App_Themes/Blue/img/Edit16.png" title="Edit" onclick="openSpecificWindow('<%# (Container.DataItem["ID"].ToString()) %>');"
                                                                    style="cursor: pointer;" />
                                                            </Template>
                                                        </obout:GridTemplate>
                                                        <obout:GridTemplate ID="GridTemplate2" runat="server">
                                                            <Template>
                                                                <asp:ImageButton ID="imgBtnEdit" CausesValidation="false" runat="server" ImageUrl="../App_Themes/Blue/img/Edit16.png"
                                                                    ToolTip='<%# Container.DataItem["Sequence"].ToString() %>' OnClick="imgBtnEdit_Click" />
                                                            </Template>
                                                        </obout:GridTemplate>
                                                        <obout:GridTemplate runat="server" ID="GridTemplate3">
                                                            <Template>
                                                                <%# (Container.Value)%>
                                                            </Template>
                                                        </obout:GridTemplate>
                                                        <obout:GridTemplate runat="server" ID="GridTemplate4" ControlPropertyName="value">
                                                            <Template>
                                                                <asp:TextBox ID="txtTitle" runat="server" MaxLength="50" Width="200px" Text='<%# (Container.Value)%>'></asp:TextBox>
                                                                <asp:RequiredFieldValidator runat="server" ID="req_txtTitle" ControlToValidate="txtTitle"
                                                                    ErrorMessage="Enter Value" Display="None" />
                                                            </Template>
                                                        </obout:GridTemplate>
                                                        <obout:GridTemplate runat="server" ID="GridTemplate5">
                                                            <Template>
                                                                <%# (Container.Value)%>
                                                            </Template>
                                                        </obout:GridTemplate>
                                                        <obout:GridTemplate runat="server" ID="GridTemplate6" ControlPropertyName="value">
                                                            <Template>
                                                                <asp:TextBox ID="txtDescription" runat="server" MaxLength="500" Width="200px" Text='<%# (Container.Value)%>'></asp:TextBox>
                                                                <asp:RequiredFieldValidator runat="server" ID="reqtxtDescription" ControlToValidate="txtDescription"
                                                                    ErrorMessage="Enter Value" Display="None" />
                                                            </Template>
                                                        </obout:GridTemplate>
                                                        <obout:GridTemplate ID="GridTemplate7" runat="server">
                                                            <Template>
                                                                <%# (Container.DataItem["Active"].ToString() == "Y" ? "Yes" : "No")%>
                                                            </Template>
                                                        </obout:GridTemplate>
                                                        <obout:GridTemplate ID="GridTemplate8" runat="server" ControlPropertyName="value">
                                                            <Template>
                                                                <asp:CheckBox ID="chkSpecificationIsActive" runat="server" />
                                                            </Template>
                                                        </obout:GridTemplate>
                                                    </Templates>
                                                </obout:Grid>
                                            </td>
                                        </tr>
                                    </table>
                                    <obout:Flyout runat="server" ID="FlyoutChangeProdPrice" OpenEvent="ONCLICK" CloseEvent="NONE"
                                        Position="TOP_CENTER" AttachTo="changePrice1">
                                        <div id="flyoutDiv">
                                            <table class="tableForm" cellspacing="5" cellpadding="5" style="text-align: left; margin: 10px;">
                                                <tr>
                                                    <td>
                                                        <req>New Price :</req>
                                                    </td>
                                                    <td style="text-align: left;">
                                                        <asp:TextBox ID="txtNewPrice" runat="server" MaxLength="15" Width="100px" ClientIDMode="Static"
                                                            Style="text-align: right;" onkeydown="return AllowDecimal(this,event);" onkeypress="return AllowDecimal(this,event);"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <req>Effective Date :</req>
                                                    </td>
                                                    <td>
                                                        <uc4:UC_Date ID="UC_EffectiveDateNewPrice" runat="server" />
                                                    </td>
                                                    <td>
                                                        <span class="remove_a" style="position: absolute; margin-top: -18px; margin-right: -10px;"
                                                            onclick='<%=FlyoutChangeProdPrice.getClientID()%>.Close();'>X</span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <req>Start Date :</req>
                                                    </td>
                                                    <td>
                                                        <uc4:UC_Date ID="UC_StartDateNewPrice" runat="server" />
                                                    </td>
                                                    <td>End Date :
                                                    </td>
                                                    <td>
                                                        <uc4:UC_Date ID="UC_EndDateNewPrice" runat="server" />
                                                    </td>
                                                    <td></td>
                                                </tr>
                                                <tr>
                                                    <td></td>
                                                    <td></td>
                                                    <td></td>
                                                    <td>
                                                        <input type="button" value="Submit" id="btnSaveNewPrice" onclick="SaveNewPrice()" />
                                                        <input type="button" value="Clear" id="btnClearNewPrice" />
                                                    </td>
                                                    <td></td>
                                                </tr>
                                            </table>
                                        </div>
                                        <script type="text/javascript">
                                            function SaveNewPrice() {
                                                alert("called");
                                                var txtEffectiveDtNP = getDateTextBoxFromUC("<%= UC_EffectiveDateNewPrice.ClientID %>");
                                                var txtStartDtNP = getDateTextBoxFromUC("<%= UC_StartDateNewPrice.ClientID %>");
                                                var txtEndDtNP = getDateTextBoxFromUC("<%= UC_EndDateNewPrice.ClientID %>");
                                                var txtNewPrice = document.getElementById("txtNewPrice");
                                                var validateNewPrice = validateNewRate();
                                                if (validateNewPrice != "") {
                                                    alert(validateNewPrice);
                                                }
                                                else {

                                                    hdnprodID = document.getElementById("<%= hdnprodID.ClientID %>");

                                                    if (hdnprodID.value == "") hdnprodID = "0";
                                                    if (hdnprodID.value != "0") {
                                                        var obj = new Object;
                                                        obj.ProdID = parseInt(hdnprodID.value);
                                                        obj.Rate = parseFloat(txtNewPrice.value);
                                                        obj.EffectiveDate = txtEffectiveDtNP.value;
                                                        obj.StartDate = txtStartDtNP.value;
                                                        obj.EndDate = txtEndDtNP.value;
                                                        PageMethods.PMSaveNewRates(obj, onSuccessSaveNewPrice, null);
                                                    }
                                                }
                                            }

                                            function onSuccessSaveNewPrice(result) {
                                                if (parseInt(result) > 0) {
                                                    GVRateHistory1.refresh();
                                                    alert("New Rate saved successfully");
                                                }
                                            }

                                            function validateNewRate() {
                                                var txtEffectiveDtNP = getDateTextBoxFromUC("<%= UC_EffectiveDateNewPrice.ClientID %>");
                                                var txtStartDtNP = getDateTextBoxFromUC("<%= UC_StartDateNewPrice.ClientID %>");
                                                var txtEndDtNP = getDateTextBoxFromUC("<%= UC_EndDateNewPrice.ClientID %>");
                                                var txtNewPrice = document.getElementById("txtNewPrice");
                                                var result = "";
                                                if (txtNewPrice.value == "") { result = "-Enter new price"; }
                                                if (txtEffectiveDtNP.value == "") {
                                                    if (result == "") { result = "-Enter effective date"; }
                                                    else { result += "\n-Enter effective date"; }
                                                }
                                                if (txtStartDtNP.value == "") {
                                                    if (result == "") { result = "-Enter start date"; }
                                                    else { result += "\n-Enter start date"; }
                                                }
                                                return result;
                                            }

                                            function clearNewPrice() {
                                                var txtEffectiveDtNP = getDateTextBoxFromUC("<%= UC_EffectiveDateNewPrice.ClientID %>");
                                                var txtStartDtNP = getDateTextBoxFromUC("<%= UC_StartDateNewPrice.ClientID %>");
                                                var txtEndDtNP = getDateTextBoxFromUC("<%= UC_EndDateNewPrice.ClientID %>");
                                                var txtNewPrice = document.getElementById("txtNewPrice");
                                                txtEffectiveDtNP.value = "";
                                                txtStartDtNP.value = "";
                                                txtEndDtNP.value = "";
                                                txtNewPrice.value = "";
                                            }
                                        </script>
                                    </obout:Flyout>
                                    <%--<table class="gridFrame" width="60%">
                                        <tr>
                                            <td>
                                                <a class="headerText">Price History</a>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <obout:Grid ID="GVRateHistory1" runat="server" AutoGenerateColumns="false" AllowAddingRecords="false"
                                                    Width="100%" OnRebind="GVRateHistory_OnRebind">
                                                    <Columns>
                                                        <obout:Column DataField="ID" Visible="false" Width="0%">
                                                        </obout:Column>
                                                        <obout:Column DataField="EffectiveDate" HeaderText="Effective Date" Width="10%" Align="center"
                                                            HeaderAlign="center" DataFormatString="{0:dd-MMM-yyyy}">
                                                        </obout:Column>
                                                        <obout:Column DataField="StartDate" HeaderText="Start Date" Width="10%" Align="center"
                                                            HeaderAlign="center" DataFormatString="{0:dd-MMM-yyyy}">
                                                        </obout:Column>
                                                        <obout:Column DataField="EndDate" HeaderText="End Date" Width="10%" Align="center"
                                                            HeaderAlign="center" DataFormatString="{0:dd-MMM-yyyy}">
                                                        </obout:Column>
                                                        <obout:Column DataField="Rate" HeaderText="Product Price" Width="15%" Align="right"
                                                            HeaderAlign="right">
                                                        </obout:Column>
                                                    </Columns>
                                                </obout:Grid>
                                            </td>
                                        </tr>
                                    </table>--%>
                                </center>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </ContentTemplate>
                </asp:TabPanel>
                <asp:TabPanel runat="server" ID="tabTaxSetup" HeaderText="Tax Setup" Visible="false">
                    <ContentTemplate>
                        <asp:UpdateProgress ID="UpdateProgress_TaxSetUp" runat="server" AssociatedUpdatePanelID="Up_pnl_TaxSetUp">
                            <ProgressTemplate>
                                <center>
                                    <div class="modal">
                                        <img src="../App_Themes/Blue/img/ajax-loader.gif" style="top: 50%;" />
                                    </div>
                                </center>
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                        <asp:UpdatePanel ID="Up_pnl_TaxSetUp" runat="server">
                            <ContentTemplate>
                                <center>
                                    <table class="gridFrame" width="70%">
                                        <tr>
                                            <td>
                                                <table style="width: 100%">
                                                    <tr>
                                                        <td>
                                                            <a style="color: white; font-size: 15px; font-weight: bold;">Tax List</a>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <obout:Grid ID="GVTaxSetup" runat="server" ShowLoadingMessage="false" AutoGenerateColumns="false"
                                                    AllowGrouping="false" AllowColumnResizing="false" AllowAddingRecords="false"
                                                    AllowColumnReordering="true" AllowMultiRecordSelection="true" AllowPageSizeSelection="false"
                                                    AllowPaging="false" AllowRecordSelection="false" AllowSorting="false" FilterType="Normal"
                                                    AllowFiltering="false" Serialize="false" CallbackMode="true" Width="100%" ShowFooter="false"
                                                    PageSize="-1" ShowColumnsFooter="true" OnRebind="GVTaxSetup_OnRebind" OnLoad="GVTaxSetup_OnRebind">
                                                    <ClientSideEvents ExposeSender="true" />
                                                    <Columns>
                                                        <obout:Column DataField="ParentID" Visible="false">
                                                        </obout:Column>
                                                        <obout:Column DataField="ID" Visible="false">
                                                        </obout:Column>
                                                        <obout:Column DataField="Checked" Visible="false">
                                                        </obout:Column>
                                                        <obout:Column DataField="CheckBoxVisible" Width="10%" Align="center" HeaderAlign="center"
                                                            HeaderText="Select Tax">
                                                            <TemplateSettings TemplateId="ItemTempCheckBox" HeaderTemplateId="HeaderTempCheckBox" />
                                                        </obout:Column>
                                                        <obout:Column DataField="Name" HeaderText="Tax Name" Width="35%" HeaderAlign="left"
                                                            Align="left">
                                                        </obout:Column>
                                                        <obout:Column DataField="Description" HeaderText="Description" Width="35%" HeaderAlign="left"
                                                            Align="left">
                                                        </obout:Column>
                                                        <obout:Column DataField="Percent" HeaderText="Tax Rate [ % ]" Width="20%" HeaderAlign="right"
                                                            Align="right">
                                                            <TemplateSettings TemplateId="GridTemplatePercent" />
                                                        </obout:Column>
                                                    </Columns>
                                                    <Templates>
                                                        <obout:GridTemplate ID="ItemTempCheckBox" runat="server">
                                                            <Template>
                                                                <asp:CheckBox runat="server" ID="chkboxID" Visible='<%# Convert.ToBoolean(Container.Value) %>'
                                                                    Checked='<%# Convert.ToBoolean(Container.DataItem["Checked"]) %>' Style="cursor: hand;"
                                                                    ToolTip='<%# Container.DataItem["ID"].ToString() %>' onclick="ProductMasterTaxSetup(this)" />
                                                            </Template>
                                                        </obout:GridTemplate>
                                                        <obout:GridTemplate ID="GridTemplatePercent" runat="server">
                                                            <Template>
                                                                <span style="margin-right: 10px;">
                                                                    <%# Container.Value %></span>
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
                <asp:TabPanel runat="server" ID="tabImages" HeaderText="Images" >
                    <ContentTemplate>
                        <center>
                            <asp:UpdateProgress ID="UpdateProgressProductMasterImages" runat="server" AssociatedUpdatePanelID="UpdatePanelProductMasterImages">
                                <ProgressTemplate>
                                    <center>
                                        <div class="modal">
                                            <img src="../App_Themes/Blue/img/ajax-loader.gif" style="top: 50%;" />
                                        </div>
                                    </center>
                                </ProgressTemplate>
                            </asp:UpdateProgress>
                            <asp:UpdatePanel ID="UpdatePanelProductMasterImages" runat="server">
                                <ContentTemplate>
                                    <table class="tableForm">
                                        <tr>
                                            <td>
                                                <req> <asp:Label ID="lblimagetitle" runat="server" Text="Image Title"/></req>
                                                :
                                            </td>
                                            <td style="text-align: left;">
                                                <asp:TextBox ID="txtImageTitle" Width="200px" runat="server"></asp:TextBox>
                                            </td>
                                            <td>
                                                <req><asp:Label ID="lblimageselect" runat="server" Text="Select Image"/></req>
                                                :
                                            </td>
                                            <td rowspan="2">
                                                <asp:UpdatePanel ID="Updpnl" runat="server" UpdateMode="Conditional">
                                                    <Triggers>
                                                        <asp:PostBackTrigger ControlID="btnProductMasterUploadImg" />
                                                    </Triggers>
                                                    <ContentTemplate>
                                                        <asp:FileUpload ID="FileUpload1" runat="server" />
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                                </br>
                                                <span class="watermark">Only bmp | jpg | jpeg | jpe | gif | tiff | tif | png | pdf
                                                    <br />
                                        Max Limit 60 KB 
                                                </span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblimagedescri" runat="server" Text="Image Description" />
                                                :
                                            </td>
                                            <td style="text-align: left;">
                                                <asp:TextBox ID="txtImageDescription" Width="200px" TextMode="MultiLine" runat="server"></asp:TextBox>
                                            </td>
                                            <td></td>
                                        </tr>
                                        <tr>
                                            <td style="visibility:hidden;">
                                                <req> <asp:Label ID="lblactive1" runat="server" Text="Active"/></req>
                                                :
                                            </td>
                                            <td align="left" style="text-align: left; visibility :hidden;">
                                                <obout:OboutRadioButton ID="rbtnYes1" runat="server" Text="Yes" GroupName="rbtnActive"
                                                    Checked="True" FolderStyle="">
                                                </obout:OboutRadioButton>
                                                <obout:OboutRadioButton ID="rbtnNo1" runat="server" Text="No" GroupName="rbtnActive"
                                                    FolderStyle="">
                                                </obout:OboutRadioButton>
                                            </td>
                                            <td colspan="2">
                                                <asp:Button ID="btnProductMasterUploadImg" runat="server" Text="Upload Image" CausesValidation="false"
                                                    OnClick="btnProductMasterUploadImg_OnClick"></asp:Button>
                                            </td>
                                        </tr>
                                    </table>
                                    <table class="gridFrame" width="700px">
                                        <tr>
                                            <td>
                                                <table style="width: 100%">
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblimages" CssClass="headerText" runat="server" Text="Images" />
                                                            <%--<a class="headerText">Images</a>--%>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <obout:Grid ID="GVImages" runat="server" AutoGenerateColumns="false" EnableTypeValidation="true"
                                                    AllowAddingRecords="false" AllowFiltering="true" AllowGrouping="true" AllowSorting="true"
                                                    Width="100%" AllowRecordSelection="false">
                                                    <Columns>
                                                        <%-- <obout:Column DataField="ID" HeaderText="ID" Visible="false">
                                                        </obout:Column>--%>
                                                        <obout:Column HeaderText="Edit" DataField="ID" Width="10%" Align="center" HeaderAlign="center">
                                                            <TemplateSettings TemplateId="TemplateEditImg" />
                                                        </obout:Column>
                                                        <%-- <obout:Column DataField="ID1" ReadOnly="true" HeaderText="Sr.No." HeaderAlign="center" runat="server" Align="center"
                                                            ItemStyle-Height="30" Width="12%">
                                                            <TemplateSettings TemplateId="tplNumbering" />
                                                        </obout:Column>--%>
                                                        <obout:Column DataField="ImgeTitle" HeaderText="Image Title" Width="35%" ItemStyle-Height="30px">
                                                            <TemplateSettings TemplateId="TemplateImageName" />
                                                        </obout:Column>
                                                        <obout:Column DataField="ImageDesc" HeaderText="Image Description" Width="35%" ItemStyle-Height="30px">
                                                        </obout:Column>
                                                        <obout:Column DataField="Path" HeaderText="Image" Align="center" HeaderAlign="center"
                                                            ItemStyle-Height="30" Width="20%" AllowFilter="false" AllowGroupBy="false" Visible="false">
                                                            <TemplateSettings TemplateId="TemplateShowImage" />
                                                        </obout:Column>
                                                        <%-- <obout:Column ID="Download" DataField="Path" HeaderText="Download" Width="10%">
                                                            <TemplateSettings TemplateId="GvTempDownload" />
                                                        </obout:Column>--%>
                                                        <obout:Column DataField="SkuImage" HeaderText="Sku Image" Width="13%">
                                                            <TemplateSettings TemplateId="GetSkuImage" />
                                                        </obout:Column>
                                                        <obout:Column DataField="Active" HeaderText="Active" Width="10%" Visible="false">
                                                        </obout:Column>

                                                    </Columns>
                                                    <Templates>
                                                        <%--<obout:GridTemplate ID="GvTempDownload">
                                                            <Template>
                                                                <a href='<%# (Container.DataItem["Path"]) %>' target="_blank">
                                                                    <img src='<%# "../CommonControls/HomeSetupImg/download.png" %>' />
                                                                </a>
                                                            </Template>
                                                        </obout:GridTemplate>--%>
                                                        <obout:GridTemplate runat="server" ID="TemplateEditImg">
                                                            <Template>
                                                                <asp:ImageButton ID="imgBtnEdit1" runat="server" ImageUrl="../App_Themes/Blue/img/Edit16.png"
                                                                    OnClick="imgImageBtnEdit_OnClick" ToolTip='<%# (Container.Value) %>' CausesValidation="false" />
                                                            </Template>
                                                        </obout:GridTemplate>

                                                        <%--<obout:GridTemplate runat="server" ID="tplNumbering">
                                                            <Template>
                                                                <%# (Container.RecordIndex + 1) %>
                                                            </Template>
                                                        </obout:GridTemplate>--%>
                                                        <obout:GridTemplate runat="server" ID="TemplateImageName">
                                                            <Template>
                                                                <%# (Container.Value.ToString().Trim()) %>
                                                            </Template>
                                                        </obout:GridTemplate>
                                                        <obout:GridTemplate runat="server" ID="TemplateShowImage">
                                                            <Template>
                                                                <a target="_blank" href='<%# (Container.Value) %>' style="cursor: pointer;">
                                                                    <asp:Image ID="Image1" runat="server" ImageUrl='<%# (Container.Value) %>' Height="30px"
                                                                        Width="30px" Style="border: solid 2px gray;" ToolTip="Click here to Download" />
                                                                </a>
                                                            </Template>
                                                        </obout:GridTemplate>
                                                        <obout:GridTemplate runat="server" ID="GetSkuImage">
                                                            <Template>
                                                                <asp:Image ID="ImageDisplay" runat="server" ImageUrl='<%#"ShowImage.ashx?ID="+ hdnprodID.Value +"" %>' Height="50px" Width="50px" Style="border: solid 2px gray;" />
                                                                <%--<asp:Image ID="Image2" runat="server" ImageUrl='<%#"DisplaySkuImage.ashx?ID="+ hdnprodID.Value +"" %>' Height="50px" Width="50px" Style="border: solid 2px gray;" />--%>
                                                            </Template>
                                                        </obout:GridTemplate>
                                                    </Templates>
                                                </obout:Grid>
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:PostBackTrigger ControlID="btnProductMasterUploadImg" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </center>
                    </ContentTemplate>
                </asp:TabPanel>
                <asp:TabPanel runat="server" ID="tabDocuments" HeaderText="Documents">
                    <ContentTemplate>
                        <uc3:UC_AttachDocument ID="UC_AttachDocument1" runat="server" />
                    </ContentTemplate>
                </asp:TabPanel>
                <asp:TabPanel ID="TabBOM" runat="server" HeaderText="BOM">
                    <ContentTemplate>
                        <asp:UpdateProgress ID="UpdateProgressBOM" runat="server" AssociatedUpdatePanelID="Uppnl_BOMDetails">
                            <ProgressTemplate>
                                <center>
                                    <div class="modal">
                                        <img src="../App_Themes/Blue/img/ajax-loader.gif" style="top: 50%;" />
                                    </div>
                                </center>
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                        <asp:UpdatePanel ID="Uppnl_BOMDetails" runat="server">
                            <ContentTemplate>
                                <center>
                                    <table class="tableForm">
                                        <tr>
                                            <td>
                                                <req><asp:Label ID="lblbomsku" runat="server" Text="Select BOM SKU"/></req>
                                                :
                                            </td>
                                            <td style="text-align: left;">
                                                <asp:TextBox ID="txtbomsku" Enabled="false" runat="server" MaxLength="50" Width="220px"></asp:TextBox>
                                                <img id="imgSearch" src="../App_Themes/Grid/img/search.jpg" title="Search SKU"
                                                    style="cursor: pointer;" onclick="openProductSearch('0')" />
                                            </td>
                                            <td>
                                                <req><asp:Label ID="lblquantity" runat="server" Text="Quantity"/></req>
                                                :
                                            </td>
                                            <td style="text-align: left;">
                                                <asp:TextBox ID="txtQuantity" runat="server" MaxLength="50" Width="194px" onkeypress="return fnAllowDigitsOnly(event)" Style="text-align: right;"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <req><asp:Label ID="lblBomSequence" runat="server" Text="Sequance"/></req>
                                                :
                                            </td>
                                            <td style="text-align: left;">
                                                <asp:TextBox ID="txtBOMSequence" runat="server" MaxLength="50" Width="194px" onkeypress="return fnAllowDigitsOnly(event)" Style="text-align: right;"></asp:TextBox>
                                            </td>

                                            <td>
                                                <asp:Label ID="lblrwmark" runat="server" Text="Remark" />
                                                :
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="txtremarkbom" runat="server" MaxLength="500" Width="511px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4" align="right">
                                                <asp:Button ID="btnsubmit" runat="server" Text="Submit" OnClientClick="SaveBomDetail()" />
                                            </td>
                                        </tr>
                                    </table>
                                    <table class="gridFrame" width="70%">
                                        <tr>
                                            <td>
                                                <table style="width: 70%">
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblbomskulist" CssClass="headerText" runat="server" Text="BOM SKU List" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <obout:Grid ID="grdaccessdele" runat="server" AllowFiltering="true" AllowGrouping="true"
                                                    AllowSorting="true" AllowColumnResizing="true" AutoGenerateColumns="false" Width="100%"
                                                    AllowAddingRecords="false" OnRebind="grdaccessdele_RebindGrid">
                                                    <Columns>
                                                        <%-- <obout:Column DataField="Id" HeaderText="Id" Visible="False" >
                                                   </obout:Column>--%>
                                                        <obout:Column DataField="Sequence" HeaderText="Remove" Width="5%" AllowSorting="false"
                                                            Align="Center" HeaderAlign="Center">
                                                            <TemplateSettings TemplateId="GvRemoveSku" />
                                                        </obout:Column>
                                                        <obout:Column DataField="Id" HeaderText="Edit" Width="5%" AllowSorting="false"
                                                            Align="Center" HeaderAlign="Center">
                                                            <TemplateSettings TemplateId="GvEditBOM" />
                                                        </obout:Column>
                                                        <obout:Column ID="Column4" HeaderText="SKU" DataField="ProductCode" Width="15%" runat="server" Align="Center" HeaderAlign="Center">
                                                        </obout:Column>
                                                        <obout:Column ID="Column7" HeaderText="SKU Name" DataField="Name" Width="15%" runat="server" Align="Center" HeaderAlign="Center">
                                                        </obout:Column>
                                                        <obout:Column ID="Column5" HeaderText="Quantity" DataField="Quantity" Width="15%" runat="server" Align="Center" HeaderAlign="Center">
                                                        </obout:Column>
                                                        <obout:Column ID="Column6" HeaderText="Remark" DataField="Remark" Width="15%" runat="server" Align="Center" HeaderAlign="Center">
                                                        </obout:Column>
                                                    </Columns>
                                                    <Templates>
                                                        <obout:GridTemplate ID="GvEditBOM" runat="server" ControlID="" ControlPropertyName="">
                                                            <Template>
                                                                <asp:ImageButton ID="imgBtnEdit1bom" runat="server" ImageUrl="../App_Themes/Blue/img/Edit16.png"
                                                                    OnClick="imgBtnEditbom_OnClick" ToolTip='<%# (Container.Value) %>' CausesValidation="false" />
                                                            </Template>
                                                        </obout:GridTemplate>
                                                        <obout:GridTemplate ID="GvRemoveSku" runat="server">
                                                            <Template>
                                                                <img id="imgbuttonremove" src="../App_Themes/Grid/img/Remove16x16.png" alt="Remove" title="Remove" onclick="RemoveSkuRecord('<%# (Container.DataItem["Id"].ToString()) %>');"
                                                                    style="cursor: pointer;" />
                                                                <%--RemoveSkuRecord--%>
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
                 <asp:TabPanel runat="server" ID="tabInventory" HeaderText="Inventory">
                    <ContentTemplate>
                        <center>
                            <%--<table class="tableForm" border="2" style="width: 700px;">--%>
                            <table class="tableForm" style="visibility: hidden;">
                                <tr>
                                    <td>
                                        <asp:Label ID="lblopeningbal" runat="server" Text="Opening Balance"></asp:Label>
                                        :
                                    </td>
                                    <td style="text-align: left;">
                                        <asp:TextBox ID="txtOpbalance" Width="200px" runat="server" onkeydown="return AllowDecimal(this,event);" onkeypress="return AllowDecimal(this,event);"></asp:TextBox>
                                        <%-- <uc4:UC_Date ID="UC_EffectiveDateInventory" runat="server" /> --%>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblopbaldate" runat="server" Text="Date"></asp:Label>
                                        :
                                    </td>
                                    <td style="text-align: left;">
                                        <uc4:UC_Date ID="UC_EffectiveDateInventory" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td colspan="4" style="text-align: right;">
                                        <asp:Button ID="btninvsubmit" runat="server" Text="Submit" Visible="false" />
                                    </td>
                                </tr>
                            </table>
                            <table class="gridFrame" width="100%">
                                <tr>
                                    <td>
                                        <asp:Label ID="lbldeptinventry" CssClass="headerText" runat="server" Text="Location wise Inventory" />
                                    </td>
                                    <td align="right">
                                          <asp:Button ID="btnvirtualQty" runat="server" OnClientClick="openvirtualQty()" Text=" Add New" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <obout:Grid ID="GVInventory" runat="server" CallbackMode="true" Serialize="true"
                                            AllowColumnReordering="true" AllowColumnResizing="true" AutoGenerateColumns="false"
                                            AllowPaging="false" ShowLoadingMessage="true" AllowSorting="false" AllowManualPaging="false"
                                            AllowRecordSelection="false" ShowFooter="false" Width="105%" PageSize="-1" OnRebind="GVInventory_RebindGrid">
                                            <ClientSideEvents ExposeSender="true" />
                                            <Columns>
                                                <obout:Column DataField="LocationId" Visible="false" Width="10%">
                                                </obout:Column>
                                                <obout:Column DataField="Id" HeaderText="Edit" Width="5%" AllowSorting="false"
                                                            Align="center" HeaderAlign="center">
                                                            <TemplateSettings TemplateId="grdeditloc" />
                                                </obout:Column>
                                                <obout:Column DataField="WarehouseName" HeaderText="Warehoouse" Align="Center" HeaderAlign="center" Width="15%">
                                                </obout:Column>
                                                <obout:Column DataField="Building" HeaderText="Building" Align="Center" HeaderAlign="center" Width="12%">
                                                </obout:Column>
                                                 <obout:Column DataField="Section" HeaderText="Section" Align="Center" HeaderAlign="center" Width="15%">
                                                </obout:Column>
                                                 <obout:Column DataField="Shelf" HeaderText="Shelf" Align="Center" HeaderAlign="center" Width="12%">
                                                </obout:Column>
                                                 <obout:Column DataField="Location" HeaderText="Location" Align="Center" HeaderAlign="center" Width="15%">
                                                </obout:Column>
                                                  <obout:Column DataField="LocType" HeaderText="Type" Align="Center" HeaderAlign="center" Width="10%">
                                                </obout:Column>
                                                 <obout:Column DataField="OpeningStock" HeaderText="Opening Qty" Align="Center" HeaderAlign="center" Width="10%">
                                                </obout:Column>
                                                <obout:Column DataField="AvailableBalance" HeaderText="Current Balance" Width="10%" Align="Center" HeaderAlign="center">
                                                </obout:Column>
                                                <obout:Column DataField="TotalDispatchQty" HeaderText="Dispatch Qty" Width="10%" Align="Center" HeaderAlign="center">
                                                </obout:Column>
                                                 <obout:Column DataField="MinOrderQty" HeaderText="Min. Order Qty" Width="10%" Align="Center" HeaderAlign="center">
                                                </obout:Column>
                                                 <obout:Column DataField="MaxOrderQty" HeaderText="Max. Order Qty" Width="10%" Align="Center" HeaderAlign="center">
                                                </obout:Column>
                                            </Columns>
                                            <Templates>
                                                 <obout:GridTemplate ID="grdeditloc" runat="server">
                                                            <Template>
                                                                <img id="imgBtnEdit" src="../App_Themes/Blue/img/Edit16.png" title="Edit" onclick="OpenProdLocation('<%# (Container.DataItem["Id"].ToString()) %>');"
                                                                    style="cursor: pointer;" />
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
                <asp:TabPanel ID="tabChannel" runat="server" HeaderText="Channel">
                    <ContentTemplate>
                        <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
                            <ProgressTemplate>
                                <center>
                                    <div class="modal">
                                        <img src="../App_Themes/Blue/img/ajax-loader.gif" style="top: 50%;" />
                                    </div>
                                </center>
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <center>
                                    <table class="tableForm">
                                        <tr>
                                            <td>
                                             <req><asp:Label ID="lblchannelname" runat="server" Text="Channel"/></req>
                                            </td>
                                            <td style="text-align: left;">
                                               <asp:DropDownList ID="ddlchannel" runat="server" DataTextField="ChannelName" DataValueField="ID" Width="200px">
                                               </asp:DropDownList>
                                            <%--<asp:RequiredFieldValidator ID="RfValtxtAddress1" ForeColor="Maroon" runat="server"
                                               ControlToValidate="ddlLocation" ErrorMessage="Select Location" ValidationGroup="AddressSubmit"
                                                  Display="None"></asp:RequiredFieldValidator>--%>
                                            </td>
                                            <td>
                                                <req><asp:Label ID="lblchanProductCode" runat="server" Text="List Code"/></req>
                                                :
                                            </td>
                                            <td style="text-align: left;">
                                                <asp:TextBox ID="txtChanProductCode" runat="server"  Width="194px"></asp:TextBox>
                                            </td>
                                            <td>
                                                <req><asp:Label ID="lblaliaccode" runat="server" Text="Alias SKU code"/></req>
                                                :
                                            </td>
                                            <td style="text-align: left;">
                                                <asp:TextBox ID="txtaliascode" runat="server"  Width="194px" ></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <req><asp:Label ID="lblChannelPrice" runat="server" Text="Price"/></req>
                                                :
                                            </td>
                                            <td style="text-align: left;">
                                                <asp:TextBox ID="txtprice" runat="server" MaxLength="50" Width="194px" onkeypress="return fnAllowDigitsOnly(event)" Style="text-align: right;"></asp:TextBox>
                                            </td>

                                            <td>
                                                <req><asp:Label ID="lblEffectiveDate" runat="server" Text="Effective From" /></req>
                                                :
                                            </td>
                                            <td style="text-align: left;">
                                                <uc8:UC_Date ID="UC_ChannEffDate" runat="server" />
                                            </td>
                                              <td>
                                                <req><asp:Label ID="lblchanTodate" runat="server" Text="To" /></req>
                                                :
                                            </td>
                                            <td style="text-align: left;">
                                                <uc8:UC_Date ID="UC_ChannToDate" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                             <td>
                                               <asp:Label ID="lbldescription" runat="server" Text="Description"/>
                                                :
                                            </td>
                                            <td style="text-align: left;" colspan="3">
                                                <asp:TextBox ID="txtdescriptionchanel" runat="server" Width="600px" ></asp:TextBox>
                                                <asp:HiddenField ID="hdnidofprodchann" runat="server" ClientIDMode="Static" />
                                            </td>
                                             <td>
                                                <req><asp:Label ID="lblchanactive" runat="server" Text="Active"/></req>
                                                :
                                            </td>
                                            <td style="text-align: left">
                                                <obout:OboutRadioButton ID="radiochannelY" runat="server" Text="Yes" GroupName="rbtActive"
                                                    Checked="True" FolderStyle="">
                                                </obout:OboutRadioButton>
                                                &nbsp;&nbsp;
                                                    <obout:OboutRadioButton ID="radiochannelN" runat="server" Text="No"
                                                        GroupName="rbtActive" FolderStyle="">
                                                    </obout:OboutRadioButton>
                                            </td>
                                            <td colspan="4" align="right">
                                                <asp:Button ID="btnsavechannel" runat="server" Text="Submit" OnClientClick="SaveProductChannel()" />
                                            </td>
                                        </tr>
                                    </table>
                                    <table class="gridFrame" width="90%">
                                        <tr>
                                            <td>
                                                <table style="width: 90%">
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblchannellist" CssClass="headerText" runat="server" Text="Channel List" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <obout:Grid ID="grdchannelList" runat="server" AllowFiltering="true" AllowGrouping="true"
                                                    AllowSorting="true" AllowColumnResizing="true" AutoGenerateColumns="false" Width="100%"
                                                    AllowAddingRecords="false" OnRebind="grdchannelList_RebindGrid" OnSelect="grdchannelList_Select">
                                                    <Columns>
                                                         <obout:Column ID="Edit" Width="7%" AllowFilter="False" HeaderText="Edit" Index="0"  Align="Center" HeaderAlign="Center"
                                                              TemplateId="GvEditCHannel">
                                                              <TemplateSettings TemplateId="GvEditCHannel" />
                                                          </obout:Column>
                                                         <obout:Column DataField="ID" Visible="False" HeaderText="ID" Index="1">
                                                         </obout:Column>
                                                        <obout:Column ID="Column9" HeaderText="Channel Name" DataField="ChannelName" Width="10%" runat="server" Align="Center" HeaderAlign="Center">
                                                        </obout:Column>
                                                        <obout:Column ID="Column10" HeaderText="SKU Code" DataField="ChannelProductcode" Width="10%" runat="server" Align="Center" HeaderAlign="Center">
                                                        </obout:Column>
                                                         <obout:Column ID="Column11" HeaderText="Alias Code" DataField="AliasProductCode" Width="10%" runat="server" Align="Center" HeaderAlign="Center">
                                                        </obout:Column>
                                                        <obout:Column ID="Column12" HeaderText="Description" DataField="ChannelProductDescription" Width="18%" runat="server" Align="Center" HeaderAlign="Center">
                                                        </obout:Column>
                                                        <obout:Column ID="Column13" HeaderText="Price" DataField="ChannelPrice" Width="10%" runat="server" Align="Center" HeaderAlign="Center">
                                                        </obout:Column>
                                                          <obout:Column ID="Column14" HeaderText="Effective From" DataField="FromDate" Width="10%" runat="server" Align="Center" HeaderAlign="Center" DataFormatString="{0:dd-MM-yyyy}">
                                                        </obout:Column>
                                                          <obout:Column ID="Column19" HeaderText="Effective To" DataField="ToDate" Width="10%" runat="server" Align="Center" HeaderAlign="Center" DataFormatString="{0:dd-MM-yyyy}">
                                                        </obout:Column>
                                                         <obout:Column ID="Column18" HeaderText="Active" DataField="Active" Width="16%" runat="server" Align="Center" HeaderAlign="Center" DataFormatString="{0:dd-MM-yyyy}">
                                                        </obout:Column>
                                                    </Columns>
                                                    <Templates>
                                                        <obout:GridTemplate ID="GvEditCHannel" runat="server" ControlID="" ControlPropertyName="">
                                                             <Template>
                                                                   <asp:ImageButton ID="imgBtnEdit" ToolTip="Edit" CausesValidation="false" runat="server"
                                                                    ImageUrl="../App_Themes/Blue/img/Edit16.png" />
                                                              </Template>
                                            </obout:GridTemplate>
                                                        <%--<obout:GridTemplate ID="GvEditCHannel2" runat="server" ControlID="" ControlPropertyName="">
                                                            <Template>
                                                                <asp:ImageButton ID="imgBtnEdit1bom" runat="server" ImageUrl="../App_Themes/Blue/img/Edit16.png"
                                                                    OnClick="imgBtnEditChannel_OnClick" ToolTip='<%# (Container.Value) %>' CausesValidation="false" />
                                                            </Template>
                                                        </obout:GridTemplate>--%>
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
                <asp:TabPanel ID="Tabpack" runat="server" HeaderText="UOM">
                    <ContentTemplate>
                        <asp:UpdateProgress ID="UpdateProgressPack" runat="server" AssociatedUpdatePanelID="UpdatePack">
                            <ProgressTemplate>
                                <center>
                                    <div class="modal">
                                        <img src="../App_Themes/Blue/img/ajax-loader.gif" style="top: 50%;" />
                                    </div>
                                </center>
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                        <asp:UpdatePanel ID="UpdatePack" runat="server">
                            <ContentTemplate>
                                <center>
                                    <table class="gridFrame" width="100%">
                                        <tr>
                                            <td>
                                                <table style="width: 100%">
                                                    <tr>
                                                        <td>
                                                            <a class="headerText"><asp:Label ID="lblpack" runat="server" Text="UOM"></asp:Label></a>
                                                        </td>
                                                        <td align="right">
                                                            <asp:Button ID="btnadd" runat="server" OnClientClick="openpackadd('0')" Text="  Add  " />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <obout:Grid ID="Grid2" runat="server" AllowFiltering="true" AllowGrouping="true"
                                                    AllowSorting="true" AllowColumnResizing="true" AutoGenerateColumns="false" Width="100%"
                                                    AllowAddingRecords="true" OnRebind="Grid2_RebindGrid">
                                                    <Columns>
                                                        <%-- <obout:Column HeaderText="Id" DataField="Id" Width="5%" runat="server" Visible="false" Index="1">
                                                        </obout:Column>--%>
                                                        <obout:Column DataField="Id" HeaderText="Edit" Width="5%" AllowSorting="false" Index="2"
                                                            Align="center" HeaderAlign="center">
                                                            <TemplateSettings TemplateId="GvTempEdit1" />
                                                        </obout:Column>
                                                        <%--<obout:Column ID="Column7" HeaderText="UOM" DataField="UOM" Width="7%" runat="server">
                                                        </obout:Column>--%>
                                                        <obout:Column HeaderText="Short Description" DataField="ShortDescri" Width="7%" runat="server" Index="3">
                                                        </obout:Column>
                                                        <obout:Column HeaderText="Description" DataField="Description" Width="10%" runat="server" Index="4">
                                                        </obout:Column>
                                                        <obout:Column HeaderText="Quantity" DataField="Quantity" Width="7%" runat="server" Index="5">
                                                        </obout:Column>
                                                        <obout:Column HeaderText="Sequence" DataField="Sequence" Width="7%" runat="server" Index="6">
                                                        </obout:Column>
                                                    </Columns>
                                                    <Templates>
                                                        <obout:GridTemplate ID="GvTempEdit1" runat="server">
                                                            <Template>
                                                                <img id="imgBtnEdit" src="../App_Themes/Blue/img/Edit16.png" title="Edit" onclick="openAddressWindow('<%# (Container.DataItem["Id"].ToString()) %>');"
                                                                    style="cursor: pointer;" />
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
                <asp:TabPanel ID="tabVendor" runat="server" HeaderText="Vendor">
                    <ContentTemplate>
                        <center>
                            <table class="gridFrame" width="80%">
                                <tr>
                                    <td>
                                        <table style="width: 100%">
                                            <tr>
                                                <td>
                                                    <a style="color: white; font-size: 15px; font-weight: bold;">Vendor Details</a>
                                                </td>
                                                 <td style="text-align: right;">   
                                      <input type="button" value="Add Vendor" id="btnVendorCustomer" onclick="openwindowVendorCustomer('0');" />
                                        <asp:HiddenField ID="hdncustvender" runat="server" ViewStateMode="Enabled" ClientIDMode="Static" />
                                    </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <obout:Grid ID="grvVendorDetails" runat="server" AllowAddingRecords="false" AllowRecordSelection="false" Width="100%"
                                            AutoGenerateColumns="false" AllowPaging="true" AllowFiltering="true" AllowGrouping="true" OnRebind="grvVendorDetails_RebindGrid"
                                            AllowSorting="true">
                                            <Columns>
                                                <%--<obout:Column HeaderText="product ID" DataField="productID" Visible="false">
                                                </obout:Column>--%>
                                                <obout:Column HeaderText="ID" DataField="PrdvendID" Visible="false" Width="2%">
                                                </obout:Column>
                                                <obout:Column HeaderText="Vendor Name" DataField="Name" Width="13%">
                                                </obout:Column>
                                                <obout:Column HeaderText="Vendor Code" DataField="Code" Width="8%">
                                                </obout:Column>
                                                <obout:Column HeaderText="Address" DataField="AddressLine1" Width="18%">
                                                </obout:Column>
                                                <obout:Column HeaderText="City" DataField="City" Width="7%">
                                                </obout:Column>
                                                <obout:Column HeaderText="EmailID" DataField="EmailID" Width="12%">
                                                </obout:Column>
                                                <obout:Column HeaderText="MobileNo" DataField="MobileNo" Width="8%">
                                                </obout:Column>
                                            </Columns>
                                        </obout:Grid>
                                    </td>
                                </tr>
                            </table>
                        </center>
                    </ContentTemplate>
                </asp:TabPanel>
                <asp:TabPanel ID="Tabparameter" runat="server" HeaderText="Parameter">
                    <ContentTemplate>
                        <center>
                            <uc6:UC_StatutoryDetails ID="UC_StatutoryDetails1" runat="server" />
                        </center>
                    </ContentTemplate>
                </asp:TabPanel>
                <asp:TabPanel runat="server" ID="tabSpecification" HeaderText="UDF">
                    <ContentTemplate>
                        <asp:ValidationSummary ID="validationsummary1" runat="server" ShowMessageBox="true"
                            ShowSummary="false" DisplayMode="bulletlist" ValidationGroup="ProductSpe" />
                        <asp:UpdateProgress ID="UpdateProgress_Specification" runat="server" AssociatedUpdatePanelID="Up_pnl_Spefication">
                            <ProgressTemplate>
                                <center>
                                    <div class="modal">
                                        <img src="../App_Themes/Blue/img/ajax-loader.gif" style="top: 50%;" />
                                    </div>
                                </center>
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                        <asp:UpdatePanel ID="Up_pnl_Spefication" runat="server">
                            <ContentTemplate>
                                <center>
                                    <table class="gridFrame" width="80%">
                                        <tr>
                                            <td>
                                                <table style="width: 80%">
                                                    <tr>
                                                        <td>
                                                            <a class="headerText">SKU UDF List</a>
                                                        </td>
                                                        <td align="right">
                                                            <asp:Button ID="Button1" Visible="false" runat="server" OnClientClick="openSpecificWindow(0)" Text="  Add  " />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <obout:Grid ID="GVProductSpecification" runat="server" AllowFiltering="true" AllowGrouping="true"
                                                    AllowSorting="true" AllowColumnResizing="true" AutoGenerateColumns="false" Width="100%"
                                                    AllowAddingRecords="true" OnInsertCommand="GVProductSpecification_InsertRecord">
                                                    <Columns>
                                                        <obout:Column DataField="Sequence" HeaderText="Edit" Width="5%" AllowSorting="false"
                                                            Align="center" HeaderAlign="center">
                                                            <TemplateSettings TemplateId="GvTempEdit" />
                                                        </obout:Column>
                                                        <obout:Column ID="Column15" HeaderText="User Defined Field" DataField="SpecificationTitle" Width="25%"
                                                            runat="server">
                                                        </obout:Column>
                                                        <obout:Column ID="Column16" HeaderText="Value" DataField="SpecificationDescription"
                                                            Width="50%" runat="server">
                                                        </obout:Column>
                                                        <obout:Column ID="Column17" HeaderText="Active" DataField="Active" Width="10%" runat="server">
                                                        </obout:Column>
                                                    </Columns>
                                                    <Templates>
                                                        <obout:GridTemplate ID="GvTempEdit" runat="server">
                                                            <Template>
                                                                <img id="imgBtnEdit" src="../App_Themes/Blue/img/Edit16.png" title="Edit" onclick="openSpecificWindow('<%# (Container.DataItem["ID"].ToString()) %>');"
                                                                    style="cursor: pointer;" />
                                                            </Template>
                                                        </obout:GridTemplate>
                                                    </Templates>
                                                </obout:Grid>
                                            </td>
                                        </tr>
                                    </table>
                                    <asp:HiddenField ID="hndsequence" runat="server" />
                                    <asp:HiddenField ID="Hndstate" runat="server" />
                                    <asp:HiddenField ID="hndproductid" runat="server" />
                                </center>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </ContentTemplate>
                </asp:TabPanel>
            </asp:TabContainer>
        </ContentTemplate>
    </asp:UpdatePanel>
    <style type="text/css">
        .remove_a {
            color: Red;
            font-size: 14px;
            font-weight: bold;
        }

            .remove_a:hover {
                color: Red;
                font-size: 15px;
                font-weight: bold;
                cursor: pointer;
            }
        .nextbutton {
         padding:5px;
        }
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
            Padding-bottom: 4px;
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

    <asp:HiddenField ID="hdnbomeditstate" runat="server" />
    <asp:HiddenField ID="hdnPartSelectedRec" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="HiddenField1" runat="server" />
    <asp:HiddenField ID="hdncustomerid" runat="server"  ClientIDMode="Static"/>
    <asp:HiddenField ID="hdncategoryid" runat="server"  ClientIDMode="Static"/>
    <asp:HiddenField ID="hdnsubcategory" runat="server"  ClientIDMode="Static"/>
    <asp:HiddenField ID="hdnProdchannelid" runat="server" ClientIDMode="Static"/>
     <asp:HiddenField ID="hdneditchannel" runat="server"  ClientIDMode="Static"/>
    <asp:HiddenField ID="hdnproducttax" runat="server"  ClientIDMode="Static"/>


     <script type="text/javascript">
         var ddlcompany = document.getElementById("<%=ddlcompany.ClientID %>");
         var ddlcustomer = document.getElementById("<%=ddldepartment.ClientID %>");
         var category = document.getElementById("<%=ddlCategory.ClientID %>");
         var ddlsubcategory = document.getElementById("<%=ddlSubCategory.ClientID %>");
         var ddlchannel = document.getElementById("<%=ddlchannel.ClientID %>");
         var barcodevalue = document.getElementById("<%=txtProductCode.ClientID %>"); 
         var producttax = document.getElementById("<%=ddlproducttax.ClientID %>");

         function GetCustomer() {
             var obj1 = new Object();
             obj1.ddlcompanyId = ddlcompany.value.toString();
             document.getElementById("<%=hdncompanyid.ClientID %>").value = ddlcompany.value.toString();
            PageMethods.GetCustomer(obj1, getLoc_onSuccessed);
        }

        function getLoc_onSuccessed(result) {

            ddlcustomer.options.length = 0;
            for (var i in result) {
                AddOption(result[i].Name, result[i].Id);
            }
        }

        function AddOption(text, value) {

            var option = document.createElement('option');
            option.value = value;
            option.innerHTML = text;
            ddlcustomer.options.add(option);
        }

        function Getdeptid() {
            var obj1 = new Object();
            obj1.ddlcustomer = ddlcustomer.value.toString();
            document.getElementById("<%=hdncustomerid.ClientID %>").value = ddlcustomer.value.toString();
            document.getElementById("<%=hdndeptid.ClientID %>").value = ddlcustomer.value.toString();
            document.getElementById("<%=lblbarcodeshow.ClientID %>").innerHTML = barcodevalue.value.toString();
            PageMethods.GetCategory(obj1, getCategory_onSuccessed);
            PageMethods.GetProductTax(obj1, getprodtax_onSuccessed);
           
        }

        function getCategory_onSuccessed(result) {

            category.options.length = 0;
            for (var i in result) {
                AddOption1(result[i].Name, result[i].Id);
            }
        }

        function AddOption1(text, value) {

            var option = document.createElement('option');
            option.value = value;
            option.innerHTML = text;
            category.options.add(option);
        }

        //function getChannel_onSuccessed(result) {

        //    ddlchannel.options.length = 0;
        //    for (var i in result) {
        //        AddOption3(result[i].Name, result[i].Id);
        //    }
        //}

        //function AddOption3(text, value) {

        //    var option = document.createElement('option');
        //    option.value = value;
        //    option.innerHTML = text;
        //    ddlchannel.options.add(option);
        //}

        function getprodtax_onSuccessed(result) {

            producttax.options.length = 0;
            for (var i in result) {
                AddOption4(result[i].Name, result[i].Id);
            }
        }

        function AddOption4(text, value) {

            var option = document.createElement('option');
            option.value = value;
            option.innerHTML = text;
            producttax.options.add(option);
        }




        function GetSubCategory() {
            document.getElementById("<%=hdnsubcategory.ClientID %>").value = ddlsubcategory.value.toString();
        }

        function fnAllowDigitsOnly(key) {
            var keycode = (key.which) ? key.which : key.keyCode;
            if ((keycode < 48 || keycode > 57) && (keycode != 8) && (keycode != 9) && (keycode != 11) && (keycode != 16) && (keycode != 37) && (keycode != 38) && (keycode != 39) && (keycode != 40) && (keycode != 219) && (keycode != 127)) {
                return false;
            }
        }
  </script>

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
                SelectedPrdRec();
            }
        }

        function SelectedPrdRec() {
            var hdnPartSelectedRec = document.getElementById("hdnPartSelectedRec");
            hdnPartSelectedRec.value = "";

            for (var i = 0; i < grvProduct.PageSelectedRecords.length; i++) {
                var record = grvProduct.PageSelectedRecords[i];
                if (hdnPartSelectedRec.value == "") hdnPartSelectedRec.value = record.ID;
            }
        }

        var ddlcompany = document.getElementById("<%=ddlcompany.ClientID %>");
        var ddldeptid = document.getElementById("<%=ddldepartment.ClientID %>");
        var hdncompanyid = document.getElementById("<%=hdncompanyid.ClientID %>");
        var hdnstate = document.getElementById("<%=hdnstate.ClientID %>");
        var hdnproductsearchId = document.getElementById("<%=hdnproductsearchId.ClientID %>");
        var Quantity = document.getElementById("<%=txtQuantity.ClientID %>");
        var BomRemark = document.getElementById("<%=txtremarkbom.ClientID %>");
        var TxtProduct = document.getElementById("<%=txtbomsku.ClientID %>");
        var hdnBomDetailId = document.getElementById("<%=hdnBomDetailId.ClientID %>");             


        function GetDepartment() {
            var obj1 = new Object();
            obj1.ddlcompanyId = ddlcompany.value.toString();
            document.getElementById("<%=hdncompanyid.ClientID %>").value = ddlcompany.value.toString();
            PageMethods.GetDepartment(obj1, getLoc_onSuccessed);
        }

        function getLoc_onSuccessed(result) {

            ddldeptid.options.length = 0;
            for (var i in result) {
                AddOption(result[i].Name, result[i].Id);
            }
        }

        function AddOption(text, value) {

            var option = document.createElement('option');
            option.value = value;
            option.innerHTML = text;
            ddldeptid.options.add(option);
        }

        //function Getdeptid() {
          //  document.getElementById("<%=hdndeptid.ClientID %>").value = ddldeptid.value.toString();
          //  var DepartmentId = ddldeptid.value.toString();
          //  var CompanyId = ddlcompany.value.toString();
          //  var skucode = document.getElementById("<%=txtProductCode.ClientID %>").value
          //  var OMSskuCode = skucode.concat("-", CompanyId, "-", DepartmentId);
           // document.getElementById("<%=txtomsskucode.ClientID %>").value = OMSskuCode;
        //}

        function openAddressWindow(Id) {
            var hdnAddressTargetObject = document.getElementById("hdnAddressTargetObject");
            //window.open('../Address/AddressInfo.aspx?Sequence=' + sequence + '&TargetObject=' + hdnAddressTargetObject.value + '', null, 'height=280, width=1000,status= 0, resizable= 1, scrollbars=0, toolbar=0,location=center,menubar=0, screenX=300; screenY=50');
            window.open('../Product/AddPacks.aspx?Id=' + Id + "", null, 'height=150px, width=850px,status= 0, resizable= 1, scrollbars=1, toolbar=0,location=center,menubar=0, screenX=300; screenY=50');
        }

        function OpenProdLocation(Id)
        {
            var skuid = 0;
            var CustomerID;
            CustomerID = document.getElementById("<%= hdncustomerid.ClientID %>").value;
            if (CustomerID == "")
            {
                CustomerID = document.getElementById("<%= hdnCustomerIDNew.ClientID %>").value;
            }
            window.open('../Product/InventryLocation.aspx?skuid=' + skuid + '&CustomerID=' + CustomerID + '&prodLoc=' + Id + "", null, 'height=250px, width=1070px,status= 0, resizable= 1, scrollbars=1, toolbar=0,location=center,menubar=0, screenX=300; screenY=50');

        }

        function openSpecificWindow(Id) {
            var hdnAddressTargetObject = document.getElementById("hdnAddressTargetObject");
            //window.open('../Address/AddressInfo.aspx?Sequence=' + sequence + '&TargetObject=' + hdnAddressTargetObject.value + '', null, 'height=280, width=1000,status= 0, resizable= 1, scrollbars=0, toolbar=0,location=center,menubar=0, screenX=300; screenY=50');
            window.open('../Product/UDF.aspx?Id=' + Id + "", null, 'height=150px, width=850px,status= 0, resizable= 1, scrollbars=1, toolbar=0,location=center,menubar=0, screenX=300; screenY=50');
        }


        function AfterProductSelected(Code, skuid) {
            var hdnval = document.getElementById("hdnProductSearchSelectedRec");
            var searchdkuid = document.getElementById("hdnproductsearchId");
            hdnval.value = Code;
            searchdkuid.value = skuid;
            var TxtProduct = document.getElementById("<%=txtbomsku.ClientID %>");
            TxtProduct.value = Code;
        }


        function SaveBomDetail() {
            var hdnbomeditstate = document.getElementById("<%=hdnbomeditstate.ClientID %>");
            var hdnstate = document.getElementById("<%=hdnstate.ClientID %>");
            var hdnproductsearchId = document.getElementById("<%=hdnproductsearchId.ClientID %>");
            var Quantity = document.getElementById("<%=txtQuantity.ClientID %>");
            var BomRemark = document.getElementById("<%=txtremarkbom.ClientID %>");
            var TxtProduct = document.getElementById("<%=txtbomsku.ClientID %>");
            var hdnBomDetailId = document.getElementById("<%=hdnBomDetailId.ClientID %>");
            var txtBOMSequence = document.getElementById("<%=txtBOMSequence.ClientID %>");
            var obj1 = new Object();
            obj1.hdnbomeditstate = hdnbomeditstate.value.toString();
            obj1.hdnproductsearchId = hdnproductsearchId.value.toString();
            obj1.BomDetailId = hdnBomDetailId.value;
            obj1.Quantity = Quantity.value.toString();
            obj1.BomRemark = BomRemark.value.toString();
            obj1.TxtProduct = TxtProduct.value.toString();
            obj1.hdnstate = hdnstate.value.toString();
            obj1.Sequence = txtBOMSequence.value.toString();
            PageMethods.SaveBomDetail(obj1, getSave_onSuccessed);
        }

        function getSave_onSuccessed(result) {
            grdaccessdele.refresh();
            document.getElementById("<%=txtQuantity.ClientID %>").value = "";
            document.getElementById("<%=txtremarkbom.ClientID %>").value = "";
            document.getElementById("<%=txtbomsku.ClientID %>").value = "";
            document.getElementById("<%=hdnproductsearchId.ClientID %>").value = "";
            document.getElementById("<%=txtBOMSequence.ClientID %>").value = "";
        }

        function fnAllowDigitsOnly(key) {
            var keycode = (key.which) ? key.which : key.keyCode;
            if ((keycode < 48 || keycode > 57) && (keycode != 8) && (keycode != 9) && (keycode != 11) && (keycode != 37) && (keycode != 38) && (keycode != 39) && (keycode != 40) && (keycode != 127)) {
                return false;
            }
        }

    </script>

    <script type="text/javascript">
        function RemoveSkuRecord(Id) {
            var obj1 = new Object();
            var Detailid = Id;
            obj1.SkuDetailId = Detailid;
            PageMethods.RemoveSku(obj1, Removesku_onSuccess);
        }

        function Removesku_onSuccess(result) {
            grdaccessdele.refresh();
        }

        function fnAllowDigitsOnly(key) {
            var keycode = (key.which) ? key.which : key.keyCode;
            if ((keycode < 48 || keycode > 57) && (keycode != 8) && (keycode != 9) && (keycode != 11) && (keycode != 37) && (keycode != 38) && (keycode != 39) && (keycode != 40) && (keycode != 127)) {
                return false;
            }
        }

    </script>

    <script type="text/javascript">

        function openwindowVendorCustomer(sequence) {
            var CustomerID;
            CustomerID = document.getElementById("<%= hdncustomerid.ClientID %>").value;
            if (CustomerID == "") {
                CustomerID = document.getElementById("<%= hdnCustomerIDNew.ClientID %>").value;
             }
            var skuid = document.getElementById("<%= hdnprodID.ClientID %>").value;
            window.open('../Product/ProductVendor.aspx?CustomerID=' + CustomerID + '&skuid=' + skuid + "", null, 'height=700px, width=825px,status= 0, resizable= 1, scrollbars=1, toolbar=0,location=center,menubar=0, screenX=300; screenY=50');
        }

        function openProductSearch(sequence) {
            var deptid = document.getElementById("<%=hdndeptid.ClientID %>").value
             window.open('../Product/SearchProduct.aspx?deptid=' + deptid + "", null, 'height=700px, width=825px,status= 0, resizable= 1, scrollbars=1, toolbar=0,location=center,menubar=0, screenX=300; screenY=50');
         }

         function openpackadd(sequence) {
             var Ids = "0";
             var skuid = document.getElementById("<%= hdnprodID.ClientID %>").value;
             window.open('../Product/AddPacks.aspx?Id=' + Ids + '&skuid=' + skuid + "", null, 'height=150px, width=850px,status= 0, resizable= 1, scrollbars=1, toolbar=0,location=center,menubar=0, screenX=300; screenY=50');
         }

         function openvirtualQty() {
             var skuid = document.getElementById("<%= hdnprodID.ClientID %>").value;
             var CustomerID;
             CustomerID = document.getElementById("<%= hdncustomerid.ClientID %>").value;
             if (CustomerID == "") {
                 CustomerID = document.getElementById("<%= hdnCustomerIDNew.ClientID %>").value;
            }
            var prodLoc = 0;
             var Ids = "0";
             window.open('../Product/InventryLocation.aspx?skuid=' + skuid + '&CustomerID=' + CustomerID + '&prodLoc=' + prodLoc + "", null, 'height=250px, width=1070px,status= 0, resizable= 1, scrollbars=1, toolbar=0,location=center,menubar=0, screenX=300; screenY=50');
         }

        function ShowHideMOQ()
        {
            var GroupSetValue = document.getElementById("<%= ddlBom.ClientID %>").value;
            if (GroupSetValue == 2) {
                document.getElementById("<%= ddlBom.ClientID %>").disabled = true;
            }
            else
            {
                document.getElementById("<%= ddlBom.ClientID %>").disabled = false;
            }
        }


    </script>
    <script type="text/javascript">

        function ProductMasterTaxSetup(invoker) {
            var getParent = invoker.parentElement;
            PageMethods.TempSaveTaxSetup(invoker.checked, getParent.title, null, null);
        }

        function checkDiscount() {
            if (txtDiscount.value != "") {
                if (checkbox.checked == true) {
                    if (parseFloat(txtDiscount.value) > 100.00) {
                        txtDiscount.value = "";
                        alert("Enter valid Discount in Percent(%)");
                    }
                }
            }
            if (parseFloat(txtDiscount.value) > parseFloat(txtPrice.value)) {
                if (checkbox.checked == false)
                { alert("Discount Price can not be greater than Principal Price"); txtDiscount.value = ""; }
            }
        }
    </script>
    <script type="text/javascript">

        function downloadClick() {
            var chk = eval(sender); if (chk.checked(true))
                alert("Checked True");
        }

        function onBeforeClientInsert() { return validate(); }

        function onBeforeClientUpdate() { return validate(); }

        function validate() {
            if (!Page_ClientValidate()) {
                alert('Error: Mandantory fields required');
                return false;
            }
            return true;
        }


        function printProductSubCategory() {

            document.getElementById("<%=hdncategoryid.ClientID %>").value = category.value.toString();
            PageMethods.PMprint_ProductSubCategory(document.getElementById("ddlCategory").value, getSubCategory_onSuccessed, null)          //onSuccessPMprint_ProductSubCategory
        }


        function getSubCategory_onSuccessed(result) {

            ddlsubcategory.options.length = 0;
            for (var i in result) {
                AddOption2(result[i].Name, result[i].Id);
            }
        }

        function AddOption2(text, value) {

            var option = document.createElement('option');
            option.value = value;
            option.innerHTML = text;
            ddlsubcategory.options.add(option);
        }


        function GetProductTax()
        {
            document.getElementById("<%=hdnproducttax.ClientID %>").value = producttax.value.toString();
        }







        function onSuccessPMprint_ProductSubCategory(result) {
            var ddlSubCategory = document.getElementById("ddlSubCategory")
            ddlSubCategory.options.length = 0;
            var option0 = document.createElement("option");


            if (result.length > 0) {
                option0.text = '-Select-' + document.getElementById("ddlSubCategory").innerHTML;
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

    </script>
    <%--Inventory Code--%>
    <script type="text/javascript">
        /*Issue Part List*/
        function markAsFocused(textbox) {
            textbox.className = 'excel-textbox-focused';
            textbox.select();
        }
        var RowIndex = 0;
        function markAsBlured(textbox, dataField, rowIndex) {
            textbox.className = 'excel-textbox';
            RowIndex = rowIndex;
            var txtvalue = textbox.value;
            if (txtvalue == "") txtvalue = 0;
            textbox.value = parseInt(txtvalue);
            if (GVInventory.Rows[rowIndex].Cells[dataField].Value != textbox.value) {
                GVInventory.Rows[rowIndex].Cells[dataField].Value = textbox.value;
                getOrderObject(rowIndex);
            }
        }

        function getOrderObject(rowIndex) {
            var order = new Object();
            order.SiteID = GVInventory.Rows[rowIndex].Cells['SiteID'].Value;
            order.OpeningStock = parseInt(GVInventory.Rows[rowIndex].Cells['OpeningStock'].Value);
            order.MaxStockLimit = parseInt(GVInventory.Rows[rowIndex].Cells['MaxStockLimit'].Value);
            order.ReorderQty = parseInt(GVInventory.Rows[rowIndex].Cells['ReorderQty'].Value);
            PageMethods.WMUpdateInventoryQty(order.SiteID, order.OpeningStock, order.MaxStockLimit, order.ReorderQty, WMUpdateInventoryQtyOnSuccess, null);

        }
        function WMUpdateInventoryQtyOnSuccess(result) {
            GVInventory.Rows[RowIndex].Cells["AvailableBalance"].Value = result;
            var body = GVInventory.GridBodyContainer.firstChild.firstChild.childNodes[1];
            var cell1 = body.childNodes[RowIndex].childNodes[5];
            cell1.firstChild.lastChild.innerHTML = result;
        }
    </script>
    <%--End Inventory Code--%>


    <%--Channel Tab Code--%>
    <script type="text/javascript">
       


        function SaveProductChannel() {

                var SKUID = document.getElementById("<%=hdnprodID.ClientID %>");
                var ddlchannel = document.getElementById("<%=ddlchannel.ClientID %>");
                var listcode = document.getElementById("<%=txtChanProductCode.ClientID %>");
                var aliascode = document.getElementById("<%=txtaliascode.ClientID %>");
                var Price = document.getElementById("<%=txtprice.ClientID %>");
                var EffDate = getDateFromUC("<%= UC_ChannEffDate.ClientID %>");
                var ToDate = getDateFromUC("<%= UC_ChannToDate.ClientID %>");
                var Description = document.getElementById("<%=txtdescriptionchanel.ClientID %>");
                var EditChannel = document.getElementById("<%=hdneditchannel.ClientID %>");
                var hdnProdchannelid = document.getElementById("<%=hdnProdchannelid.ClientID %>");
                var hdncustomerid = document.getElementById("<%=hdncustomerid.ClientID %>");
                var hdnidofprodchann = document.getElementById("<%=hdnidofprodchann.ClientID %>");
                var newcustomerID = document.getElementById("<%=hdnCustomerIDNew.ClientID %>");
                var radiochannelY = document.getElementById("<%=radiochannelY.ClientID %>");

            if (document.getElementById("<%=txtChanProductCode.ClientID%>").value == "" || document.getElementById("<%=txtaliascode.ClientID%>").value == "" || document.getElementById("<%=txtprice.ClientID%>").value == "" || ddlchannel.options[ddlchannel.selectedIndex].text == "-Select-" || getDateFromUC("<%= UC_ChannEffDate.ClientID %>") == "" || getDateFromUC("<%= UC_ChannToDate.ClientID %>") == "") {

                    if (ddlchannel.options[ddlchannel.selectedIndex].text == "-Select-") {
                        showAlert("Please select Channel!", "Error", "#");
                        ddltype.focus();

                    }
                    else if (document.getElementById("<%=txtChanProductCode.ClientID%>").value == "") {
                        showAlert("Please Enter List Code", "Error", "#");
                        document.getElementById("<%=txtChanProductCode.ClientID%>").focus();

                    }
                    else if (document.getElementById("<%=txtaliascode.ClientID%>").value == "") {
                        showAlert("Please enter alias Code!", "Error", "#");
                        document.getElementById("<%=txtaliascode.ClientID%>").focus();

                    }

                    else if (document.getElementById("<%=txtprice.ClientID%>").value == "") {
                        showAlert("Please enter Price!", "Error", "#");
                        document.getElementById("<%=txtprice.ClientID%>").focus();

                    }
                    else if (getDateFromUC("<%= UC_ChannEffDate.ClientID %>") == "") {
                        showAlert("Please Select Eff. Date!", "Error", "#");
                    }
                    else if (getDateFromUC("<%= UC_ChannToDate.ClientID %>") == "") {
                        showAlert("Please Select To Date!", "Error", "#");
                    }
                }
                else {

                    var obj1 = new Object();
                    obj1.SKUID = SKUID.value.toString();
                    obj1.ddlchannel = ddlchannel.value.toString();
                    obj1.listcode = listcode.value.toString();
                    obj1.aliascode = aliascode.value.toString();
                    obj1.Price = Price.value.toString();
                    obj1.EffDate = getDateFromUC("<%= UC_ChannEffDate.ClientID %>");
                    obj1.ToDate = getDateFromUC("<%= UC_ChannToDate.ClientID %>");
                    obj1.Description = Description.value.toString();
                    obj1.hdnProdchannelid = hdnProdchannelid.value.toString();
                    obj1.hdncustomerid = hdncustomerid.value.toString();
                    obj1.NewCustyomerID = newcustomerID.value.toString();
                    obj1.hdnidofprodchann = hdnidofprodchann.value.toString();
                    if (radiochannelY.checked == true) {
                        obj1.Active = "Yes"
                    }
                    else {
                        obj1.Active = "No"
                    }
                    PageMethods.SaveProductChannel(obj1, getSave_onSuccessed);
                }
            }

       function getSave_onSuccessed(result) {
           grdchannelList.refresh();
           if (result == "Save")
           {
               showAlert("Save Succesfully", "info", "");
           }
           else if (result == "Update") {
               showAlert("Update Succesfully", "info", "");
           }
           else
           {
               showAlert("Error Occured Please Check !", "error", "#");
           }
           document.getElementById("<%=txtChanProductCode.ClientID %>").value = "";
           document.getElementById("<%=txtaliascode.ClientID %>").value = "";
           document.getElementById("<%=txtprice.ClientID %>").value = "";
           document.getElementById("<%=txtdescriptionchanel.ClientID %>").value = "";
           getDateFromUC("<%= UC_ChannEffDate.ClientID %>").value == "";
           getDateFromUC("<%= UC_ChannEffDate.ClientID %>") == "";
           document.getElementById("<%=ddlchannel.ClientID %>").value = "0";
        }

    </script>
    <%--End Channel Tab Code--%>

    <%-- Next Button validation Code--%>
    <script type="text/javascript">
    function Checkvalidations() {

          
            if (document.getElementById("<%=txtProductName.ClientID %>").value == "") {
                showAlert("Please Enter SKU name!", "error", "#");
                document.getElementById("<%=txtProductName.ClientID %>").focus();
                return false;
            }
            if (document.getElementById("<%=ddlcompany.ClientID %>").value == "0") {
                showAlert("Please Select Company!", "error", "#");
                document.getElementById("<%=ddlcompany.ClientID %>").focus();
                return false;
            }

            if (document.getElementById("<%=ddldepartment.ClientID %>").value == "0") {
                showAlert("Please Select Customer!", "error", "#");
                document.getElementById("<%=ddldepartment.ClientID %>").focus();
                return false;
            }
            if (document.getElementById("<%=ddlCategory.ClientID %>").value == "0") {
                showAlert("Please Select SKU Category!", "error", "#");
                document.getElementById("<%=ddlCategory.ClientID %>").focus();
                return false;
            }

           <%-- if (document.getElementById("<%=ddlSubCategory.ClientID %>").value == "0") {
                showAlert("Please Enter City!", "error", "#");
                document.getElementById("<%=ddlSubCategory.ClientID %>").focus();
                return false;
            }--%>
            if (document.getElementById("<%=txtcost.ClientID %>").value == "") {
                showAlert("Please Enter Cost!", "error", "#");
                document.getElementById("<%=txtcost.ClientID %>").focus();
                return false;
            }
            if (document.getElementById("<%=txtPrincipalPrice.ClientID %>").value == "") {
                showAlert("Please Enter Retail Price!", "error", "#");
                document.getElementById("<%=txtPrincipalPrice.ClientID %>").focus();
                return false;
            }
           
            if (document.getElementById("<%=ddlpickmethod.ClientID %>").value == "0") {
                showAlert("Please Select Picking Method!", "error", "#");
                document.getElementById("<%=ddlpickmethod.ClientID %>").focus();
                return false;
            }

            return true
        };
</script>

     <%--End Of validation Code--%>

</asp:Content>

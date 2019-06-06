<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/CRM.Master" AutoEventWireup="true"
    EnableEventValidation="false" CodeBehind="AssetMaster.aspx.cs" Inherits="BrilliantWMS.POR.AssetMaster" %>

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
<%--<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>--%>
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
            <asp:TabContainer runat="server" ID="TabContainerProductMaster" ActiveTabIndex="4">
                <asp:TabPanel ID="TabPanelProductList" runat="server" HeaderText="Asset List">
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
                                    <table class="gridFrame" width="80%">
                                        <tr>
                                            <td>
                                                <table style="width: 100%">
                                                    <tr>
                                                        <td>
                                                            <a style="color: white; font-size: 15px; font-weight: bold;">Asset List</a>
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
                                                    <Columns>
                                                        <obout:Column HeaderText="Edit" DataField="ID" Width="10%" Align="center" HeaderAlign="center">
                                                            <TemplateSettings TemplateId="TemplateEdit" />
                                                        </obout:Column>
                                                        <obout:Column HeaderText="Type" DataField="ProductType" Visible="false" />
                                                        <obout:Column HeaderText="Category" DataField="ProductCategory" Width="25%" />
                                                        <obout:Column HeaderText="Sub Category" DataField="ProductSubCategory" Visible="false" />
                                                        <obout:Column HeaderText="Code" DataField="ProductCode" Width="30%" />
                                                        <obout:Column HeaderText="Name" DataField="Name" Width="30%" />
                                                        <obout:Column HeaderText="UOM" DataField="UOM" Width="15%" />
                                                        <obout:Column HeaderText="Price" DataField="PrincipalPrice" Width="25%">
                                                        </obout:Column>
                                                        <obout:Column HeaderText="Active" DataField="active" Width="15%">
                                                            <TemplateSettings TemplateId="tplActive" />
                                                        </obout:Column>
                                                    </Columns>
                                                    <Templates>
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
                <asp:TabPanel ID="tabProductDetails" runat="server" HeaderText="Asset Details">
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
                                        <tr>
                                            <td>
                                                <req>Type :</req>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlProductType" runat="server" Width="200px">
                                                    <asp:ListItem Text="Asset" Value="3" Selected="True"></asp:ListItem>
                                                </asp:DropDownList>
                                                <%-- <asp:RequiredFieldValidator ID="req_ddlProductType" runat="server" ErrorMessage="Select Product Type"
                                                    ControlToValidate="ddlProductType" InitialValue="0" ValidationGroup="Save" Display="None"></asp:RequiredFieldValidator>--%>
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
                                                Sub Category :
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlSubCategory" runat="server" ClientIDMode="Static" Width="200px"
                                                    DataTextField="ProductSubCategory" DataValueField="ID">
                                                </asp:DropDownList>
                                                <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Select Sub Category"
                                                    ControlToValidate="ddlSubCategory" InitialValue="0" ValidationGroup="Save" Display="None"></asp:RequiredFieldValidator>--%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Code :
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtProductCode" runat="server" MaxLength="50" Width="194px"></asp:TextBox>
                                            </td>
                                            <td>
                                                <req> Name : </req>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtProductName" runat="server" MaxLength="100" Width="194px"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="reqtxtProductName" runat="server" ErrorMessage="Enter Product Name"
                                                    ControlToValidate="txtProductName" ValidationGroup="Save" Display="None"></asp:RequiredFieldValidator>
                                            </td>
                                            <td>
                                                <req>UOM : </req>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlUOM" runat="server" DataTextField="Name" DataValueField="ID"
                                                    Width="200px">
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="req_ddlUOM" runat="server" ErrorMessage="Please Select Unit"
                                                    ControlToValidate="ddlUOM" ValidationGroup="Save" Display="None" InitialValue="0"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <req>Principal Price :</req>
                                            </td>
                                            <td style="text-align: left;">
                                                <asp:TextBox ID="txtPrincipalPrice" runat="server" MaxLength="15" Width="100px" Style="text-align: right;"
                                                    onkeydown="return AllowDecimal(this,event);" onkeypress="return AllowDecimal(this,event);"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator_txtPrincipalPrice" runat="server"
                                                    ErrorMessage="Please enter Principal Price" ControlToValidate="txtPrincipalPrice"
                                                    ValidationGroup="Save" Display="None"></asp:RequiredFieldValidator>
                                                <%-- <a id="changePrice1" runat="server" style="visibility: hidden;">Change</a>--%>
                                            </td>
                                            <td>
                                                Fixed Discount :
                                            </td>
                                            <td style="text-align: left;">
                                                <asp:TextBox ID="txtFixedDisc" runat="server" MaxLength="10" Style="text-align: right;"
                                                    Width="100px" onkeydown="AllowDecimal(this,event);checkDiscount();" onkeyup="checkDiscount();"
                                                    onkeypress="AllowDecimal(this,event);checkDiscount();"></asp:TextBox>
                                                <asp:CheckBox ID="chkboxFixedDiscIsPercent" onclick="checkDiscount();" runat="server"
                                                    Text="%" />
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                        </tr>
                                        <tr>
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
                                        </tr>
                                        <tr>
                                            <td>
                                                <req>Active :</req>
                                            </td>
                                            <td style="text-align: left" colspan="5">
                                                <obout:OboutRadioButton ID="rbtYes" runat="server" Text="Yes" GroupName="rbtActive"
                                                    Checked="True" FolderStyle="">
                                                </obout:OboutRadioButton><obout:OboutRadioButton ID="rbtNo" runat="server" Text="No"
                                                    GroupName="rbtActive" FolderStyle="">
                                                </obout:OboutRadioButton>
                                            </td>
                                        </tr>
                                    </table>
                                    <%--<obout:Flyout runat="server" ID="FlyoutChangeProdPrice" OpenEvent="ONCLICK" CloseEvent="NONE"
                                        Position="TOP_CENTER" AttachTo="btnAddTransferToSite">
                                        <div id="flyoutDiv">
                                            <table class="tableForm" cellspacing="5" cellpadding="5" style="text-align: left;
                                                margin: 10px;">
                                                <tr>
                                                    <td>
                                                        <req>New Site :</req>
                                                    </td>
                                                    <td style="text-align: left;">                                                       
                                                        <asp:DropDownList ID="ddlSite" runat="server" DataValueField="ID" DataTextField="Territory"
                                                            Width="111px" ClientIDMode="Static">
                                                        </asp:DropDownList>
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
                                                    <td>
                                                        End Date :
                                                    </td>
                                                    <td>
                                                        <uc4:UC_Date ID="UC_EndDateNewPrice" runat="server" />
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <input type="button" value="Submit" id="btnSaveNewPrice" onclick="SaveNewPrice()" />
                                                        <input type="button" value="Clear" id="btnClearNewPrice" />
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>                                        
                                    </obout:Flyout>--%>
                                    <asp:Panel ID="PnlHistory" runat="server" align="center">
                                        <table class="gridFrame" id="tblNew" width="100%">
                                            <tr>
                                                <td>
                                                    <table width="100%">
                                                        <tr>
                                                            <td style="text-align: left;">
                                                                <a class="headerText">New Site</a>
                                                            </td>
                                                            <td style="text-align: right;">
                                                                <asp:Button ID="btnSubmit" runat="server" Text="Submit" Style="cursor: pointer;"
                                                                    OnClientClick="SaveNewPrice()"  OnClick="btnSubmit_Onclick" />
                                                                <asp:Button ID="btnClose" runat="server" Text="Cancel" Style="cursor: pointer;" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <table style="background-color: White;">
                                                        <tr>
                                                            <td style="color: Black; text-align: right;">
                                                                <req>New Site :</req>
                                                            </td>
                                                            <td style="text-align: left;">
                                                                <asp:DropDownList ID="ddlNewSite" runat="server" DataValueField="ID" DataTextField="Territory"
                                                                    Width="111px" ClientIDMode="Static">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td style="color: Black; text-align: right;">
                                                                <req>Effective Date :</req>
                                                            </td>
                                                            <td>
                                                                <uc4:UC_Date ID="UC_EffectiveDateNewPrice" runat="server" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="color: Black; text-align: right;">
                                                                <req>Start Date :</req>
                                                            </td>
                                                            <td>
                                                                <uc4:UC_Date ID="UC_StartDateNewPrice" runat="server" />
                                                            </td>
                                                            <td style="color: Black; text-align: right;">
                                                                End Date :
                                                            </td>
                                                            <td>
                                                                <uc4:UC_Date ID="UC_EndDateNewPrice" runat="server" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                    <asp:HiddenField ID="hdnSiteID" runat="server" />
                                    <asp:ModalPopupExtender ID="ModelPopup1" runat="server" PopupControlID="PnlHistory"
                                        TargetControlID="btnNewHistory" CancelControlID="btnClose" Drag="true">
                                    </asp:ModalPopupExtender>
                                    <table class="gridFrame" width="60%">
                                        <tr>
                                            <td>
                                                <a class="headerText">Site History</a>
                                            </td>
                                            <td style="text-align: right;">
                                               <%-- <input type="button" id="btnAddTransferToSite" value="Add" />--%>
                                                <asp:Button ID="btnNewHistory" runat="server" Text="Add" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
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
                                                        <obout:Column DataField="Enddate" HeaderText="End Date" Width="10%" Align="center"
                                                            HeaderAlign="center" DataFormatString="{0:dd-MMM-yyyy}">
                                                        </obout:Column>
                                                        <obout:Column DataField="Territory" HeaderText="Site Name" Width="15%" Align="center"
                                                            HeaderAlign="center">
                                                        </obout:Column>
                                                    </Columns>
                                                </obout:Grid>
                                            </td>
                                        </tr>
                                    </table>
                                </center>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </ContentTemplate>
                </asp:TabPanel>
                <asp:TabPanel runat="server" ID="tabSpecification" HeaderText="Specifications">
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
                                    <table class="tableForm">
                                        <tr>
                                            <td>
                                                <req> Specification Title :</req>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtspecificationtitle" runat="server" MaxLength="50" Width="200px"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="valRftxtspecificationtitle" runat="server" ErrorMessage="Please enter Specification Title"
                                                    ControlToValidate="txtspecificationtitle" ValidationGroup="ProductSpe" Display="None"></asp:RequiredFieldValidator>
                                            </td>
                                            <td>
                                                <req> Specification Description :</req>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtSpecificationDesc" runat="server" TextMode="MultiLine" Width="200px"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="valRftxtSpecificationDesc" runat="server" ErrorMessage="Please enter Specification Description"
                                                    ControlToValidate="txtSpecificationDesc" ValidationGroup="ProductSpe" Display="None"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4" style="text-align: right;">
                                                <asp:Button runat="server" ID="BtnSubMitproductSp" ValidationGroup="ProductSpe" OnClick="BtnSubMitproductSp_Click"
                                                    Text="Submit" />
                                            </td>
                                        </tr>
                                    </table>
                                    <table class="gridFrame" width="100%">
                                        <tr>
                                            <td>
                                                <table style="width: 100%">
                                                    <tr>
                                                        <td>
                                                            <a class="headerText">Product Specification List</a>
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
                                                        <obout:Column ID="Column1" HeaderText="Specification Title" DataField="SpecificationTitle"
                                                            Width="25%" runat="server">
                                                            <TemplateSettings TemplateId="TemplateProductSpecificationTitle" EditTemplateId="EditTemplateProductSpecificationTitle" />
                                                        </obout:Column>
                                                        <obout:Column ID="Column2" HeaderText="Specification Description" DataField="SpecificationDescription"
                                                            Width="50%" runat="server">
                                                            <TemplateSettings TemplateId="TemplateDescription" EditTemplateId="EditTemplateDescription" />
                                                        </obout:Column>
                                                        <obout:Column ID="Column3" HeaderText="Active" DataField="Active" Width="10%" runat="server">
                                                            <TemplateSettings TemplateId="TemplateProductSpecificationActive" EditTemplateId="EditTemplateProductSpecificationActive" />
                                                        </obout:Column>
                                                    </Columns>
                                                    <Templates>
                                                        <obout:GridTemplate ID="GvTempEdit" runat="server">
                                                            <Template>
                                                                <asp:ImageButton ID="imgBtnEdit" CausesValidation="false" runat="server" ImageUrl="../App_Themes/Blue/img/Edit16.png"
                                                                    ToolTip='<%# Container.DataItem["Sequence"].ToString() %>' OnClick="imgBtnEdit_Click" />
                                                            </Template>
                                                        </obout:GridTemplate>
                                                        <obout:GridTemplate runat="server" ID="TemplateProductSpecificationTitle">
                                                            <Template>
                                                                <%# (Container.Value)%>
                                                            </Template>
                                                        </obout:GridTemplate>
                                                        <obout:GridTemplate runat="server" ID="EditTemplateProductSpecificationTitle" ControlPropertyName="value">
                                                            <Template>
                                                                <asp:TextBox ID="txtTitle" runat="server" MaxLength="50" Width="200px" Text='<%# (Container.Value)%>'></asp:TextBox>
                                                                <asp:RequiredFieldValidator runat="server" ID="req_txtTitle" ControlToValidate="txtTitle"
                                                                    ErrorMessage="Enter Value" Display="None" />
                                                            </Template>
                                                        </obout:GridTemplate>
                                                        <obout:GridTemplate runat="server" ID="TemplateDescription">
                                                            <Template>
                                                                <%# (Container.Value)%>
                                                            </Template>
                                                        </obout:GridTemplate>
                                                        <obout:GridTemplate runat="server" ID="EditTemplateDescription" ControlPropertyName="value">
                                                            <Template>
                                                                <asp:TextBox ID="txtDescription" runat="server" MaxLength="500" Width="200px" Text='<%# (Container.Value)%>'></asp:TextBox>
                                                                <asp:RequiredFieldValidator runat="server" ID="reqtxtDescription" ControlToValidate="txtDescription"
                                                                    ErrorMessage="Enter Value" Display="None" />
                                                            </Template>
                                                        </obout:GridTemplate>
                                                        <obout:GridTemplate ID="TemplateProductSpecificationActive" runat="server">
                                                            <Template>
                                                                <%# (Container.DataItem["Active"].ToString() == "Y" ? "Yes" : "No")%>
                                                            </Template>
                                                        </obout:GridTemplate>
                                                        <obout:GridTemplate ID="EditTemplateProductSpecificationActive" runat="server" ControlPropertyName="value">
                                                            <Template>
                                                                <asp:CheckBox ID="chkSpecificationIsActive" runat="server" />
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
                <asp:TabPanel runat="server" ID="tabImages" HeaderText="Images">
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
                                                <req>Image Title :</req>
                                            </td>
                                            <td style="text-align: left;">
                                                <asp:TextBox ID="txtImageTitle" Width="200px" runat="server"></asp:TextBox>
                                            </td>
                                            <td>
                                                <req>Select Image :</req>
                                            </td>
                                            <td rowspan="2">
                                                <obout:FileUpload ID="FileUploadProductMasterImg" runat="server" Width="250px" ValidFileExtensions="bmp;jpg;jpeg;jpe;gif;tiff;tif;png;pdf"
                                                    MaximumTotalFileSize="10240" MaximumFileSize="1000" />
                                                <span class="watermark">Only bmp | jpg | jpeg | jpe | gif | tiff | tif | png | pdf
                                                </span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Image Description :
                                            </td>
                                            <td style="text-align: left;">
                                                <asp:TextBox ID="txtImageDescription" Width="200px" TextMode="MultiLine" runat="server"></asp:TextBox>
                                            </td>
                                            <td>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <req>Active :</req>
                                            </td>
                                            <td align="left" style="text-align: left;">
                                                <obout:OboutRadioButton ID="rbtnYes" runat="server" Text="Yes" GroupName="rbtnActive"
                                                    Checked="True" FolderStyle="">
                                                </obout:OboutRadioButton>
                                                <obout:OboutRadioButton ID="rbtnNo" runat="server" Text="No" GroupName="rbtnActive"
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
                                                            <a class="headerText">Images</a>
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
                                                        <obout:Column DataField="ID" HeaderText="ID" Visible="false">
                                                        </obout:Column>
                                                        <obout:Column ID="Column4" DataField="ID1" ReadOnly="true" HeaderText="Sr.No." runat="server"
                                                            ItemStyle-Height="30" Width="10%">
                                                            <TemplateSettings TemplateId="tplNumbering" />
                                                        </obout:Column>
                                                        <obout:Column DataField="ImgeTitle" HeaderText="Image Title" Width="35%" ItemStyle-Height="30px">
                                                            <TemplateSettings TemplateId="TemplateImageName" />
                                                        </obout:Column>
                                                        <obout:Column DataField="ImageDesc" HeaderText="Image Description" Width="35%" ItemStyle-Height="30px">
                                                        </obout:Column>
                                                        <obout:Column DataField="Path" HeaderText="Image" Align="center" HeaderAlign="center"
                                                            ItemStyle-Height="30" Width="20%" AllowFilter="false" AllowGroupBy="false">
                                                            <TemplateSettings TemplateId="TemplateShowImage" />
                                                        </obout:Column>
                                                        <%-- <obout:Column ID="Download" DataField="Path" HeaderText="Download" Width="10%">
                                                            <TemplateSettings TemplateId="GvTempDownload" />
                                                        </obout:Column>--%>
                                                        <obout:Column DataField="Active" HeaderText="Active" Width="10%">
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
                                                        <obout:GridTemplate runat="server" ID="tplNumbering">
                                                            <Template>
                                                                <%# (Container.RecordIndex + 1) %>
                                                            </Template>
                                                        </obout:GridTemplate>
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
                                                    </Templates>
                                                </obout:Grid>
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </center>
                    </ContentTemplate>
                </asp:TabPanel>
                <asp:TabPanel runat="server" ID="tabDocuments" HeaderText="Documents">
                    <ContentTemplate>
                        <asp:UpdatePanel ID="UpdtPnlDoc" runat="server">
                            <ContentTemplate>
                                <uc3:UC_AttachDocument ID="UC_AttachDocument1" runat="server" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </ContentTemplate>
                </asp:TabPanel>
                <asp:TabPanel runat="server" ID="tabInventory" HeaderText="Inventory">
                    <ContentTemplate>
                        <center>
                            <table class="tableForm" style="width: 800px;">
                                <tr>
                                    <td style="text-align: left;">
                                        Effective Date * :
                                        <uc4:UC_Date ID="UC_EffectiveDateInventory" runat="server" />
                                    </td>
                                </tr>
                            </table>
                            <table class="gridFrame" width="800px">
                                <tr>
                                    <td>
                                        <a class="headerText">Site wise Inventory</a>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <obout:Grid ID="GVInventory" runat="server" CallbackMode="true" Serialize="true"
                                            AllowColumnReordering="true" AllowColumnResizing="true" AutoGenerateColumns="false"
                                            AllowPaging="false" ShowLoadingMessage="true" AllowSorting="false" AllowManualPaging="false"
                                            AllowRecordSelection="false" ShowFooter="false" Width="100%" PageSize="-1">
                                            <ClientSideEvents ExposeSender="true" />
                                            <Columns>
                                                <obout:Column DataField="SiteID" Visible="false" Width="5%">
                                                </obout:Column>
                                                <obout:Column DataField="Territory" HeaderText="Site / Location" Width="40%">
                                                    <TemplateSettings TemplateId="GridTemplate1" />
                                                </obout:Column>
                                                <obout:Column DataField="OpeningStock" HeaderText="Opening Stock" Width="15%" Align="right"
                                                    HeaderAlign="right">
                                                    <TemplateSettings TemplateId="PlainEditTemplate" />
                                                </obout:Column>
                                                <obout:Column DataField="MaxStockLimit" HeaderText="Max Stock Limit" Width="15%"
                                                    Align="right" HeaderAlign="right">
                                                    <TemplateSettings TemplateId="PlainEditTemplate" />
                                                </obout:Column>
                                                <obout:Column DataField="ReorderQty" HeaderText="Reorder Qty" Width="15%" Align="right"
                                                    HeaderAlign="right">
                                                    <TemplateSettings TemplateId="PlainEditTemplate" />
                                                </obout:Column>
                                                <obout:Column DataField="AvailableBalance" HeaderText="Current Balance" Width="15%"
                                                    Align="right" HeaderAlign="right">
                                                </obout:Column>
                                            </Columns>
                                            <Templates>
                                                <obout:GridTemplate runat="server" ID="GridTemplate1">
                                                    <Template>
                                                        <span style="font-weight: bold; margin-left: 5px;">
                                                            <%# Container.Value %></span>
                                                    </Template>
                                                </obout:GridTemplate>
                                                <obout:GridTemplate runat="server" ID="PlainEditTemplate">
                                                    <Template>
                                                        <input type="text" class="excel-textbox" maxlength="12" value="<%# Container.Value %>"
                                                            onfocus="markAsFocused(this)" onkeydown="AllowInt(this,event);" onkeypress="AllowInt(this,event);"
                                                            onblur="markAsBlured(this, '<%# GVInventory.Columns[Container.ColumnIndex].DataField %>', <%# Container.PageRecordIndex %>)" />
                                                    </Template>
                                                </obout:GridTemplate>
                                                <obout:GridTemplate ID="GridTemplateRightAlign">
                                                    <Template>
                                                        <span style="text-align: right; width: 130px; margin-right: 10px;">
                                                            <%# Container.Value  %></span>
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
                <asp:TabPanel ID="tabVendor" runat="server" HeaderText="Vendor">
                    <ContentTemplate>
                        <center>
                            <table class="gridFrame" width="50%">
                                <tr>
                                    <td>
                                        <table style="width: 100%">
                                            <tr>
                                                <td>
                                                    <a style="color: white; font-size: 15px; font-weight: bold;">Vendor Details</a>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <obout:Grid ID="grvVendorDetails" runat="server" AllowAddingRecords="false" AllowRecordSelection="false"
                                            AutoGenerateColumns="false" AllowPaging="true" AllowFiltering="true" AllowGrouping="true"
                                            AllowSorting="true">
                                            <Columns>
                                                <obout:Column HeaderText="product ID" DataField="productID" Visible="false">
                                                </obout:Column>
                                                <obout:Column HeaderText="Vendor ID" DataField="vendorID" Visible="false">
                                                </obout:Column>
                                                <obout:Column HeaderText="Name" DataField="VendorName" Width="150px">
                                                </obout:Column>
                                                <obout:Column HeaderText="Address" DataField="VendorAddress" Width="150px">
                                                </obout:Column>
                                                <obout:Column HeaderText="Contact No" DataField="VendorContactNo" Width="140px">
                                                </obout:Column>
                                                <obout:Column HeaderText="Email ID" DataField="VendorEmailID" Width="140px">
                                                </obout:Column>
                                                <obout:Column HeaderText="Min. Order Qty" DataField="MinOrderQty" Width="160px">
                                                </obout:Column>
                                                <obout:Column HeaderText="Lead Time" DataField="LeadTime" Width="80px">
                                                </obout:Column>
                                                <obout:Column HeaderText="Active" DataField="VendorActive" Width="100px">
                                                </obout:Column>
                                            </Columns>
                                        </obout:Grid>
                                    </td>
                                </tr>
                            </table>
                        </center>
                    </ContentTemplate>
                </asp:TabPanel>
            </asp:TabContainer>
        </ContentTemplate>
    </asp:UpdatePanel>
    <style type="text/css">
        .remove_a
        {
            color: Red;
            font-size: 14px;
            font-weight: bold;
        }
        .remove_a:hover
        {
            color: Red;
            font-size: 15px;
            font-weight: bold;
            cursor: pointer;
        }
    </style>
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
    <script type="text/javascript">
        onload();
        function onload() {
            var v = document.getElementById("btnConvertTo");
            v.style.visibility = 'hidden';
        }
        function ProductMasterTaxSetup(invoker) {
            var getParent = invoker.parentElement;
            PageMethods.TempSaveTaxSetup(invoker.checked, getParent.title, null, null);
        }

        function checkDiscount() {
            var checkbox = document.getElementById("<%=chkboxFixedDiscIsPercent.ClientID %>");
            var txtDiscount = document.getElementById("<%=txtFixedDisc.ClientID %>");
            var txtPrice = document.getElementById("<%= txtPrincipalPrice.ClientID%>");
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


        function SaveNewPrice() {            
            var txtEffectiveDtNP = getDateTextBoxFromUC("<%= UC_EffectiveDateNewPrice.ClientID %>");
            var txtStartDtNP = getDateTextBoxFromUC("<%= UC_StartDateNewPrice.ClientID %>");
            var txtEndDtNP = getDateTextBoxFromUC("<%= UC_EndDateNewPrice.ClientID %>");
            var ddlNewSite = document.getElementById("ddlNewSite").value;          

            var validateNewPrice = validateNewRate();
            if (validateNewPrice != "") {
                alert(validateNewPrice);
            }
            else {
              
                hdnprodID = document.getElementById("<%= hdnprodID.ClientID %>");               
               // if (hdnprodID.value == "") hdnprodID = "0";
                if (hdnprodID.value != "0") {
                    var obj = new Object;
                    obj.ProdID = parseInt(hdnprodID.value);
                    obj.Site = parseInt(ddlNewSite);
                    obj.EffectiveDate = txtEffectiveDtNP.value;
                    obj.StartDate = txtStartDtNP.value;
                    obj.EndDate = txtEndDtNP.value;
                    obj.Active = true;
                   
                    PageMethods.PMSaveNewRates(obj, onSuccessSaveNewPrice, null);
                }
                else {
                    alert('First Save the Tool');
                }
            }
        }

        function onSuccessSaveNewPrice(result) {
            if (parseInt(result) > 0) {
                GVRateHistory1.refresh();
                alert("Site History Saved successfully");                
                ModelPopup1.hide();
            }
            else {
                alert("Save Tool Details First!!!");
                ModelPopup1.hide();
            }
        }

        function validateNewRate() {
            var txtEffectiveDtNP = getDateTextBoxFromUC("<%= UC_EffectiveDateNewPrice.ClientID %>");
            var txtStartDtNP = getDateTextBoxFromUC("<%= UC_StartDateNewPrice.ClientID %>");
            var txtEndDtNP = getDateTextBoxFromUC("<%= UC_EndDateNewPrice.ClientID %>");
            var ddlNewSite = document.getElementById("ddlNewSite");
            var ddlNewSite1 = parseInt(ddlNewSite.options[ddlNewSite.selectedIndex].value);
            //var txtNewPrice = document.getElementById("txtNewPrice");

            var result = "";
            //  if (txtNewPrice.value == "") { result = "-Enter new price"; }
            if (ddlNewSite1 == "0") {
                if (result == "") { result = "-Select Site"; }
                else { result += "\n-Select Site"; }
            }
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


//        window.WebForm_OnSubmit = null;
//        window.WebForm_DoPostBackWithOptions = null;
    </script>
    <%--End Inventory Code--%>
</asp:Content>

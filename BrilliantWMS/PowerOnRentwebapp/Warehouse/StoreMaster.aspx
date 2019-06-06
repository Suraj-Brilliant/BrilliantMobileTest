<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/CRM.Master" AutoEventWireup="true" CodeBehind="StoreMaster.aspx.cs"
    Inherits="BrilliantWMS.Warehouse.StoreMaster"  EnableEventValidation="false" Theme="Blue" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="Obout.Ajax.UI" Namespace="Obout.Ajax.UI.FileUpload" TagPrefix="obout" %>
<%@ Register Src="../CommonControls/UC_Date.ascx" TagName="UC_Date" TagPrefix="uc6" %>
<%@ Register Assembly="obout_Interface" Namespace="Obout.Interface" TagPrefix="obout" %>
<%@ Register Assembly="obout_Grid_NET" Namespace="Obout.Grid" TagPrefix="obout" %>
<%@ Register Src="../CommonControls/UCFormHeader.ascx" TagName="UCFormHeader" TagPrefix="uc7" %>
<%@ Register Src="../CommonControls/UCToolbar.ascx" TagName="UCToolbar" TagPrefix="uc8" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHolder_FormHeader" runat="server">
        <uc8:UCToolbar ID="UCToolbar1" runat="server" />
    <uc7:UCFormHeader ID="UCFormHeader1" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPHolder_Form" runat="server">
     <asp:ValidationSummary ID="validationsummary_AccountMaster" runat="server" ShowMessageBox="true"
        ShowSummary="false" DisplayMode="bulletlist" ValidationGroup="Save" />
    <center>
        <asp:TabContainer runat="server" ID="tabStoreMaster" ActiveTabIndex="2" Width="100%">
            <asp:TabPanel ID="TabCustomerList" runat="server" HeaderText="Store List">
                <ContentTemplate>
                    <%-- <asp:UpdateProgress ID="UpdateProgressUC_CustomerList" runat="server" AssociatedUpdatePanelID="upnl_customerlist">
                            <ProgressTemplate>
                                <center>
                                    <div class="modal">
                                        <img src="../App_Themes/Blue/img/ajax-loader.gif" style="top: 50%;" />
                                    </div>
                                </center>
                            </ProgressTemplate>
                        </asp:UpdateProgress>--%>
                    <%-- <asp:UpdatePanel ID="upnl_customerlist" runat="server">
                            <ContentTemplate>--%>
                    <asp:HiddenField ID="HdnAccountId" runat="server"></asp:HiddenField>
                    <asp:HiddenField ID="HdnOpeningBalId" runat="server" />
                    <asp:HiddenField ID="hdnselectedRec" runat="server" ClientIDMode="Static" />
                    <asp:HiddenField ID="hdnselectedRecPayment" runat="server" ClientIDMode="Static" />
                    <asp:HiddenField ID="hndCompanyid" runat="server"></asp:HiddenField>
                    <asp:HiddenField ID="hndState" runat="server" ClientIDMode="Static"></asp:HiddenField>
                    <asp:HiddenField ID="hdnmodestate" runat="server" ClientIDMode="Static" />
                    <asp:HiddenField ID="hdnCountry" runat="server" ClientIDMode="Static" />
                    <asp:HiddenField ID="hdnState" runat="server" ClientIDMode="Static" />
                    <asp:HiddenField ID="hdnLogo" runat="server" ClientIDMode="Static" />
                    <asp:HiddenField ID="hdnFilePath" runat="server" />
                    <asp:HiddenField ID="hdnFileTye" runat="server" />
                    <asp:HiddenField ID="hdnSelectedCompany" runat="server" ClientIDMode="Static" />
                    <asp:HiddenField ID="hdnSelectedDepartment" runat="server" ClientIDMode="Static" />
                    <asp:HiddenField ID="hdnapproverSearchSelectedRec" runat="server" ClientIDMode="Static" />
                    <asp:HiddenField ID="hdnapproversearchId" runat="server" ClientIDMode="Static" />
                     <asp:HiddenField ID="hdnCenterApproName" runat="server" ClientIDMode="Static" />
                    <asp:HiddenField ID="hdnCenterApproID" runat="server" ClientIDMode="Static" />
                    <asp:HiddenField ID="hdnPartSelectedRec" runat="server" ClientIDMode="Static" />
                    
                    <center>
                        <table class="gridFrame" style="width: 100%">
                            <tr>
                                <td>
                                    <table style="width: 100%">
                                        <tr>
                                            <td style="text-align: left;">
                                                <a id="headerText">
                                                    <asp:Label ID="lblheasertext" runat="server" Text="Store List"></asp:Label></a>
                                            </td>
                                            <td style="text-align: right;">
                                                <%-- <input type="button" value="Book Payment" id="btnPayment" onclick="selectedRecordBookPayment();" />
                                                <input type="button" value="Send Credentials" id="btnSendMail" onclick="selectedRecordSendMail();" />--%>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <obout:Grid ID="Grid1" runat="server" AllowAddingRecords="False" AllowFiltering="True"
                                        AllowGrouping="True" AutoGenerateColumns="False" Width="100%" PageSize="10" >                       
                                        <%--<ScrollingSettings ScrollHeight="80px"/>--%>
                                        <Columns>
                                            <obout:Column ID="Column2" DataField="Edit" Width="4%" AllowFilter="False" HeaderText="Edit"
                                                Index="0" TemplateId="GridTemplate1">
                                                <TemplateSettings TemplateId="GridTemplate1" />
                                            </obout:Column>
                                            <obout:Column ID="Column3" DataField="payment" Width="7%" Align="center" AllowFilter="False" HeaderText="Payment Method"
                                                Index="1" TemplateId="GridTemplate2">
                                                <TemplateSettings TemplateId="GridTemplate2" />
                                            </obout:Column>
                                            <obout:Column DataField="ID" HeaderText="ID" Visible="False" Index="2">
                                            </obout:Column>
                                            <obout:Column DataField="Territory" HeaderText="Department Name" Width="8%" Index="3">
                                            </obout:Column>
                                            <obout:Column DataField="StoreCode" HeaderText="Department Code" Width="5%" Index="4">
                                            </obout:Column>
                                            <obout:Column DataField="Approvallevel" HeaderText="Approval level" Align="center" Width="5%" Index="5">
                                            </obout:Column>
                                            <obout:Column DataField="AutoCancel" HeaderText="Auto Cancel" Align="center" Width="5%" Index="6">
                                            </obout:Column>
                                            <obout:Column DataField="cancelDays" HeaderText="Cancel Days" Align="center" Width="5%" Index="7">
                                            </obout:Column>
                                             <obout:Column DataField="Deliveries" HeaderText="GWC Deliveries" Align="center" Width="6%" Index="8">
                                            </obout:Column>
                                             <obout:Column DataField="Ecommerce" HeaderText="Ecommerce" Align="center" Width="5%" Index="9">
                                            </obout:Column>
                                             <obout:Column DataField="FinApprover" HeaderText="Fin Approver"  Width="6%" Index="11">
                                            </obout:Column>
                                            <obout:Column DataField="addressTypetext" HeaderText="address Type"  Width="6%" Index="12">
                                            </obout:Column>
                                            <obout:Column DataField="PrimeDays" HeaderText="Prime Hours" Align="center" Width="6%" Index="13" TemplateId="Prime">
                                                <TemplateSettings TemplateId="Prime" />
                                            </obout:Column>
                                            <obout:Column DataField="ExpressDays" HeaderText="Express Hours" Align="center" Width="6%" Index="14" TemplateId="Express">
                                                 <TemplateSettings TemplateId="Express" />
                                            </obout:Column>
                                            <obout:Column DataField="RegularDays" HeaderText="Regular Hours" Align="center" Width="6%" Index="15" TemplateId="Regular">
                                                 <TemplateSettings TemplateId="Regular" />
                                            </obout:Column>
                                        </Columns>
                                        <Templates>
                                            <obout:GridTemplate runat="server" ID="GridTemplate1">
                                                <Template>
                                                    <asp:ImageButton ID="imgBtnEdit" runat="server" ImageUrl="../App_Themes/Blue/img/Edit16.png"
                                                      OnClick="imgBtnEditbom_OnClick" OnClientClick="SelectedPrdRec()"   ToolTip='<%# (Container.DataItem["ID"].ToString()) %>' />
                                                </Template>
                                            </obout:GridTemplate>
                                            <obout:GridTemplate runat="server" ID="GridTemplate2">
                                                <Template>
                                                     <img id="imgbuttonremove" src="../App_Themes/Blue/img/add.png" alt="Method" title="Payment Method" onclick="Openmethod('<%# (Container.DataItem["ID"].ToString()) %>');"
                                                      style="cursor: pointer;" />
                                                </Template>
                                            </obout:GridTemplate>
                                            <obout:GridTemplate ID="Prime" runat="server" ControlID="" ControlPropertyName="">
                                                <Template>
                                                    <%#(Container.DataItem["PrimeDays"].ToString()=="0" ? "NA" : Container.DataItem["PrimeDays"].ToString()) %>                                                    
                                                </Template>
                                            </obout:GridTemplate>
                                            <obout:GridTemplate ID="Express" runat="server" ControlID="" ControlPropertyName="">
                                                <Template>
                                                    <%#(Container.DataItem["ExpressDays"].ToString()=="0" ? "NA" : Container.DataItem["ExpressDays"].ToString()) %>                                                    
                                                </Template>
                                            </obout:GridTemplate>
                                            <obout:GridTemplate ID="Regular" runat="server" ControlID="" ControlPropertyName="">
                                                <Template>
                                                    <%#(Container.DataItem["RegularDays"].ToString()=="0" ? "NA" : Container.DataItem["RegularDays"].ToString()) %>                                                    
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
      <asp:TabPanel ID="tabStoreInfo" runat="server" HeaderText="Store Info">
                <ContentTemplate>
                    <center>
                        <table class="tableForm">
                            <tr>
                                 <td>
                                <req><asp:Label Id="lblcompany" runat="server" Text="Company"></asp:Label></req> :
                            </td>
                            <td style="text-align: left">
                                <asp:DropDownList ID="ddlcompany" runat="server" DataTextField="Name" DataValueField="ID" Width="206px">
                                    <asp:ListItem>--Select--</asp:ListItem>
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RFQCompany" runat="server" ControlToValidate="ddlcompany" InitialValue="0"
                                        Display="None" ErrorMessage="Please Select Company" ValidationGroup="Save"></asp:RequiredFieldValidator>
                            </td>
                                 <td>
                                    <req><asp:Label Id="lblcustomer" runat="server" Text="Warehouse"></asp:Label></req>
                                    :
                                </td>
                                <td style="text-align: left">
                                    <asp:DropDownList ID="ddlcutomer" runat="server"  DataTextField="Name" DataValueField="ID" onchange="GetDepartment()" Width="206px">
                                         <asp:ListItem>--Select--</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RfValCompany" runat="server" ControlToValidate="ddlcutomer" InitialValue="0"
                                        Display="None" ErrorMessage="Please Select Customer" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                </td>
                                <td>
                                    <req>
                                    <asp:Label ID="lbldepartment" runat="server" Text=" Store Name :"></asp:Label>
                                    </req>
                                </td>
                                <td style="text-align: left">
                                    <asp:TextBox ID="txtdepartment" runat="server" MaxLength="50" Width="200px"></asp:TextBox>
                                </td>
                               
                            </tr>
                            <tr>
                                 
                                <td>
                                    <req><asp:Label ID="lbldeptcode" runat="server" Text="Store Code :"></asp:Label> </req>
                                </td>
                                <td style="text-align: left">
                                    <asp:TextBox ID="txtdeptcode" runat="server" MaxLength="50" Width="200px"></asp:TextBox> 

                                </td>
                                <td>
                                    <req><asp:Label ID="lblapproval" runat="server"  Text="Approval level"></asp:Label> </req> :
                                </td>
                                <td style="text-align: left">
                                    <asp:TextBox ID="txtapproval" runat="server" Style="text-align: right" MaxLength="50" ClientIDMode="Static" 
                                     onkeypress="return fnAllowDigitsOnly(event)"    Width="200px"></asp:TextBox>     
                                </td>
                                <td>
                                    <req>
                                    <asp:Label ID="lblautocancel" runat="server" Text=" Auto Cancel"></asp:Label> </req>
                                    :
                                </td>
                                <td style="text-align: left">
                                    <asp:DropDownList ID="ddlautocancel" runat="server" DataTextField="Name" DataValueField="ID" onchange="setvalue()"
                                        Width="206px">
                                        <asp:ListItem Text="-Select-" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="Yes" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="No" Value="3"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                               
                            </tr>
                            <tr>
                                 <td>
                                    <req>
                                    <asp:Label ID="lblcanceldays" runat="server" Text="Auto Cancellation Days"></asp:Label>
                                    :</req>
                                </td>
                                <td style="text-align: left">
                                    <asp:TextBox ID="txtcanceldays" runat="server" Style="text-align: right" MaxLength="50" 
                                        Width="200px" onkeypress="return fnAllowDigitsOnly(event)"  ></asp:TextBox>  
                                </td>
                                <td>
                                    <asp:Label ID="lblappreminder" runat="server" Text="Approval Reminder"></asp:Label>
                                    :
                                </td>
                                <td style="text-align: left">
                                    <obout:OboutCheckBox ID="chkapproremyes" runat="server" OnClick="enableTextBox1()" ControlsToDisable="" ControlsToEnable="" FolderStyle="" ParentCheckBoxID="" State="Unchecked">
                                    </obout:OboutCheckBox>
                                </td>
                                <td>
                                    <asp:Label ID="lblremaprrosche" runat="server" Text="Reminder Schedule"></asp:Label>
                                    :
                                </td>
                                <td style="text-align: left">
                                    <asp:TextBox ID="txtremschedule" runat="server" Enabled="False" Style="text-align: right"
                                        MaxLength="50" Width="200px" onkeypress="return fnAllowDigitsOnly(event)"></asp:TextBox>
                                    &nbsp;&nbsp;<asp:Label ID="lblremdays" runat="server" Text="Days"></asp:Label>
                                </td>
                               
                            </tr>
                            <tr>
                                 <td>
                                    <asp:Label ID="lblGWCDeliveries" runat="server" Text="GWC Deliveries"></asp:Label>
                                    :
                                </td>
                                <td style="text-align: left;">
                                    <obout:OboutCheckBox ID="chkDeliver" runat="server" OnClick="enableSLA()" ControlsToDisable="" ControlsToEnable="" FolderStyle="" ParentCheckBoxID="" State="Unchecked"></obout:OboutCheckBox>
                                </td>
                                 <td>
                                   
                                     <asp:Label ID="lbldeliverydays" runat="server" Text="Max Delivery Days"></asp:Label>
                                    :
                                </td>
                                <td style="text-align: left">
                                   
                                 <asp:TextBox ID="txtdeliverydays" runat="server" Style="text-align: right" MaxLength="50" Text="10"
                                        Width="200px" onkeypress="return fnAllowDigitsOnly(event)"></asp:TextBox>
                                </td>
                                <td>
                                     <req><asp:Label ID="lblacti" runat="server" Text="Active"></asp:Label>
                                    :</req>
                               </td>
                                 <td style="text-align: left">
                                    <obout:OboutRadioButton ID="OboutRadioButton1" runat="server" Checked="True" FolderStyle=""
                                        GroupName="Active" Text="Yes">
                                    </obout:OboutRadioButton>
                                    <obout:OboutRadioButton ID="OboutRadioButton2" runat="server" FolderStyle="" GroupName="Active"
                                        Text="No">
                                    </obout:OboutRadioButton>
                                </td>
                            </tr>
                            <tr>
                                 <td>
                                     <asp:Label ID="lblecommerce" runat="server" Text="ECommerce"></asp:Label> :
                                </td>
                                <td style="text-align: left">
                                   <obout:OboutCheckBox ID="chkecommerce" runat="server" ControlsToDisable="" ControlsToEnable="" FolderStyle="" ParentCheckBoxID="" State="Unchecked"></obout:OboutCheckBox>

                                </td>
                                <td>
                                   <asp:Label ID="lblpricechange" runat="server" Text="Price Change" ></asp:Label>
                                    :
                                </td>
                                <td style="text-align: left">
                                    <obout:OboutCheckBox ID="chkpricechange" runat="server" Enabled="False" ControlsToDisable="" ControlsToEnable="" FolderStyle="" ParentCheckBoxID="" State="Unchecked" onclick=" RemoveFA()"></obout:OboutCheckBox>
                                </td>
                               
                                <td>
                                   <asp:Label ID="lblfinapprover" runat="server" Text="Financial Approver"></asp:Label>
                                    :
                                </td>
                                <td style="text-align: left;">
                                  <asp:TextBox ID="txtbomsku" Enabled="False" runat="server" MaxLength="50" Width="220px"></asp:TextBox>
                                  <img id="imgSearch" src="../App_Themes/Grid/img/search.jpg" title="Search SKU" runat="server" style="cursor: pointer;" onclick="openProductSearch('0')" />
                                 &nbsp;&nbsp;&nbsp;</td>
                                 <td>
                                </td>
                                <td style="text-align: left">
                                </td>
                            </tr>
                        </table>
                    </center>
                </ContentTemplate>
            </asp:TabPanel>
       </asp:TabContainer>
    </center>
</asp:Content>

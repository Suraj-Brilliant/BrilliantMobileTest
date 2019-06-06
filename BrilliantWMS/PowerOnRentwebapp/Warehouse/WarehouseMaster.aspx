<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/CRM.Master" AutoEventWireup="true" CodeBehind="WarehouseMaster.aspx.cs" EnableEventValidation="false"
    Inherits="BrilliantWMS.POR.WarehouseMaster" Theme="Blue" %>

<%@ Register Assembly="obout_Grid_NET" Namespace="Obout.Grid" TagPrefix="obout" %>
<%@ Register Src="../ContactPerson/UCContactPerson.ascx" TagName="UCContactPerson" TagPrefix="uc1" %>
<%@ Register Src="../CommonControls/UC_Date.ascx" TagName="UC_Date" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../CommonControls/UCToolbar.ascx" TagName="UCToolbar" TagPrefix="uc2" %>
<%@ Register Src="../Address/UCAddress.ascx" TagName="UCAddress" TagPrefix="uc4" %>
<%@ Register Src="../CommonControls/UCFormHeader.ascx" TagName="UCFormHeader" TagPrefix="uc6" %>
<%@ Register Src="../Warehouse/UCLocation.ascx" TagName="UCLocation" TagPrefix="uc5" %>
<%@ Register Assembly="obout_Interface" Namespace="Obout.Interface" TagPrefix="obout" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHolder_FormHeader" runat="server">
    <uc2:UCToolbar ID="UCToolbar1" runat="server" />
    <uc6:UCFormHeader ID="UCFormHeader1" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPHolder_Form" runat="server">
    <script src="../MasterPage/JavaScripts/countries.js" type="text/javascript"></script>

    <div id="tabletest">
        <center>
            <asp:ValidationSummary ID="validationsummary_EngineMaster" runat="server" ShowMessageBox="True"
                DisplayMode="bulletlist" ShowSummary="False" ValidationGroup="Save" />
            <asp:UpdatePanel ID="UpdatePanelTabPanelCompanyList" runat="server">
                <ContentTemplate>
                    <asp:TabContainer runat="server" ID="tabAccountMaster" Width="100%">
                        <asp:TabPanel ID="TabPanelWarehouseList" runat="server" HeaderText="Warehouse List">
                            <ContentTemplate>
                                <asp:HiddenField ID="hdnPartSelectedRec" runat="server" ClientIDMode="Static" />
                                <asp:HiddenField ID="hdnwarehouseID" runat="server" ClientIDMode="Static" />
                                <asp:HiddenField ID="hdnstate" runat="server" ClientIDMode="Static" />
                                <asp:HiddenField ID="hdncompanyid" runat="server" ClientIDMode="Static" />
                                <asp:HiddenField ID="hdncustomerid" runat="server" ClientIDMode="Static" />
                                <asp:HiddenField ID="hdnNewCustomerID" runat="server" ClientIDMode="Static" />
                                <asp:HiddenField ID="hdnCountry" runat="server" ClientIDMode="Static" />
                                <asp:HiddenField ID="hdncountryState" runat="server" ClientIDMode="Static" />
                                <asp:HiddenField ID="hdnWarehouseName" runat="server" ClientIDMode="Static" />
                                <asp:HiddenField ID="hdnlocationID" runat="server" ClientIDMode="Static" />
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
                                                                    <asp:Label ID="lblskulist" Style="color: white; font-size: 15px; font-weight: bold;" runat="server" Text="Warehouse List"></asp:Label>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <obout:Grid ID="grdWarehouseList" runat="server" AutoGenerateColumns="False" AllowAddingRecords="False"
                                                            AllowFiltering="True" AllowGrouping="True" AllowMultiRecordSelection="False"
                                                            AllowRecordSelection="true" Width="100%" AllowColumnResizing="true" AllowColumnReordering="true" OnSelect="GvCustomer_Select">
                                                            <ScrollingSettings ScrollHeight="250" />
                                                            <Columns>
                                                                <obout:Column ID="Column1" Width="4%" AllowFilter="False" HeaderText="Edit" Index="0"
                                                                    TemplateId="GvTempEdit">
                                                                    <TemplateSettings TemplateId="GvTempEdit" />
                                                                </obout:Column>
                                                                <obout:Column DataField="ID" Visible="False" HeaderText="ID" Index="1">
                                                                </obout:Column>
                                                                <obout:Column HeaderText="Warehouse Code" DataField="Code" Width="25%" />
                                                                <obout:Column HeaderText="Warehouse Name" DataField="WarehouseName" Width="20%" />
                                                                <obout:Column HeaderText="Type" DataField="Type" Width="12%" />
                                                                <obout:Column HeaderText="Description" DataField="Description" Width="23%" />
                                                                <obout:Column HeaderText="Company" DataField="Company" Width="19%" />
                                                                <obout:Column HeaderText="Contact Name" DataField="ContactName" Width="12%" />
                                                                <obout:Column HeaderText="Contact No." DataField="MobileNo" Width="12%" Align="right" HeaderAlign="center" />
                                                                <obout:Column HeaderText="Active" HeaderAlign="center" DataField="Active" Width="8%" Align="center">
                                                                </obout:Column>
                                                            </Columns>
                                                            <Templates>

                                                                <obout:GridTemplate ID="GvTempEdit" runat="server" ControlID="" ControlPropertyName="">
                                                                    <Template>
                                                                        <asp:ImageButton ID="imgBtnEdit" ToolTip="Edit" CausesValidation="false" runat="server"
                                                                            ImageUrl="../App_Themes/Blue/img/Edit16.png" />
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
                        <asp:TabPanel ID="TabCustomerList" runat="server" HeaderText="Warehouse Detail" TabIndex="1">
                            <ContentTemplate>
                                <%-- <asp:HiddenField ID="HdnWareHouseId" runat="server"></asp:HiddenField>--%>
                                <center>
                                    <table class="tableForm">
                                        <tr>
                                            <td>
                                                <req><asp:Label ID="lblcompany" runat="server" Text="Company"></asp:Label></req>
                                                :
                                            </td>
                                            <td style="text-align: left;">
                                                <asp:DropDownList ID="ddlcompany" DataTextField="Name" DataValueField="ID" onchange="GetCustomer()"
                                                    Width="190px" runat="server">
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="reqddlcompany" runat="server" ErrorMessage="Please Select Company"
                                                    ControlToValidate="ddlcompany" ValidationGroup="Save" Display="None"></asp:RequiredFieldValidator>
                                            </td>
                                            <td>
                                                <req><asp:Label ID="lblcustomer" runat="server" Text="Customer"></asp:Label></req>
                                                :
                                            </td>
                                            <td style="text-align: left;">
                                                <asp:DropDownList ID="ddlcustomer" DataTextField="Name" DataValueField="ID" onchange="GetCustomerID()"
                                                    Width="190px" runat="server">
                                                </asp:DropDownList>
                                                <%--<asp:RequiredFieldValidator ID="reqddlcustomer" runat="server" ErrorMessage="Please select Customer"
                                                    ControlToValidate="ddlcustomer" ValidationGroup="Save" Display="None"></asp:RequiredFieldValidator>--%>
                                            </td>
                                            <td>
                                                <req><asp:Label ID="lblwarecode" runat="server" Text="Warehouse Code"></asp:Label></req>
                                                :
                                            </td>
                                            <td style="text-align: left">
                                                <asp:TextBox ID="txtcode" runat="server" Width="190px"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Enter Warehouse Code"
                                                    ControlToValidate="txtcode" ForeColor="Red" ValidationGroup="Save"
                                                    Display="None"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <req><asp:Label ID="lblwarename" runat="server" Text="Warehouse Name"></asp:Label></req>
                                                :
                                            </td>
                                            <td style="text-align: left">
                                                <asp:TextBox ID="txtwarehousename" runat="server" Width="190px"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="req_txtEngineSerialNo" runat="server" ErrorMessage="Enter Warehouse Name"
                                                    ControlToValidate="txtwarehousename" ForeColor="Red" ValidationGroup="Save"
                                                    Display="None"></asp:RequiredFieldValidator>
                                            </td>
                                            <td>
                                                <req><asp:Label ID="lblType" runat="server" Text="Type"></asp:Label></req>
                                                :
                                            </td>
                                            <td style="text-align: left;">
                                                <asp:DropDownList ID="ddltype" DataTextField="Name" DataValueField="ID" onchange="GetDepartment()"
                                                    Width="190px" runat="server">
                                                    <asp:ListItem Text="-Select-" Value="0"></asp:ListItem>
                                                    <asp:ListItem Text="Standard" Value="1"></asp:ListItem>
                                                    <asp:ListItem Text="Virtual" Value="2"></asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="reqtxtProductName" runat="server" ErrorMessage="Please Select Warehouse Type"
                                                    ControlToValidate="ddltype" ValidationGroup="Save" Display="None"></asp:RequiredFieldValidator>
                                            </td>
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
                                        </tr>
                                        <tr>
                                            <td>
                                                <req><asp:Label ID="lbladdress1" runat="server" Text="Address Line 1" /></req>
                                                :
                                            </td>
                                            <td style="text-align: left;">
                                                <asp:TextBox ID="txtCAddress1" runat="server" MaxLength="100" onkeyup="TextBox_KeyUp(this,'SpanAddressLine1','100');"
                                                    TextMode="MultiLine" Width="190px"></asp:TextBox>

                                                <br />
                                                <span class="watermark">
                                                    <asp:Label ID="lblcharremain" runat="server" Text="characters remaining out of 100 " />&nbsp;<span
                                                        id="SpanAddressLine1"> 100</span> </span>
                                                <asp:RequiredFieldValidator ID="address1val" runat="server" ControlToValidate="txtCAddress1"
                                                    Display="None" ErrorMessage="Please Enter Address" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                            </td>
                                            <td>
                                                <asp:Label ID="lbladdress2" runat="server" Text="Address Line 2" />
                                                :
                                            </td>
                                            <td style="text-align: left;">
                                                <asp:TextBox ID="txtAddress2" runat="server" MaxLength="100" onkeyup="TextBox_KeyUp(this,'SpanAddressLine2','100');"
                                                    TextMode="MultiLine" Width="190px"></asp:TextBox>
                                                <br />
                                                <span class="watermark">
                                                    <asp:Label ID="lblcharremain2" runat="server" Text="characters remaining out of 100 " />&nbsp;<span id="SpanAddressLine2"> 100</span></span>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblzip" runat="server" Text="ZIP Code" />
                                                :
                                            </td>
                                            <td style="text-align: left;">
                                                <asp:TextBox ID="txtZipCode" runat="server" MaxLength="10" Width="190px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <req>
                                    <asp:Label Id="lblcountry" runat="server" Text="Country"/>
                                    </req>
                                                :
                                            </td>
                                            <td style="text-align: left;">
                                                <asp:DropDownList ID="ddlCountry" ClientIDMode="Static" runat="server"
                                                    onchange="print_state('ddlState',this.selectedIndex); onCountryChange(this);"
                                                    Width="190px">
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="RFValddlCountry" runat="server" ForeColor="Maroon"
                                                    ValidationGroup="AddressSubmit" InitialValue="0" ControlToValidate="ddlCountry"
                                                    ErrorMessage="Select Country" Display="None"></asp:RequiredFieldValidator>
                                            </td>
                                            <td>
                                                <req>
                                    <asp:Label Id="lblstate" runat="server" Text="State"/>
                                    </req>
                                                :
                                            </td>
                                            <td style="text-align: left;">
                                                <asp:DropDownList ID="ddlState" ClientIDMode="Static" runat="server" Width="190px"
                                                    onchange="onStateChange(this);">
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="RFValstate" ForeColor="Maroon" ControlToValidate="ddlState"
                                                    InitialValue="0" runat="server" ErrorMessage="Select State" Display="None"></asp:RequiredFieldValidator>
                                            </td>
                                            <td>
                                                <req>
                                    <asp:Label Id="lblcity" runat="server" Text="City"/>
                                    </req>
                                                :
                                            </td>
                                            <td style="text-align: left;">
                                                <asp:TextBox ID="txtCity" runat="server" MaxLength="50" Width="190px" onkeypress="return AllowAlphabet(event)"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RFValddlBCity" runat="server" ControlToValidate="txtCity"
                                                    Display="None" ErrorMessage="Please Select City" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Description :
                                            </td>
                                            <td style="text-align: left" colspan="5">
                                                <asp:TextBox ID="txtdescription" runat="server" Width="650px"></asp:TextBox>

                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Remark :
                                            </td>
                                            <td style="text-align: left" colspan="4">
                                                <asp:TextBox ID="txtremark" runat="server" Width="650px"></asp:TextBox>

                                            </td>
                                            <td>
                                                <asp:Button ID="btncustomernext" runat="server" Text="  Next  " OnClick="btncustomernext_Click" OnClientClick="return Checkvalidations();" />
                                                <%--OnClientClick="return Checkvalidations();"--%>
                                            </td>

                                        </tr>
                                    </table>
                                    <br />
                            </ContentTemplate>
                        </asp:TabPanel>
                        <asp:TabPanel ID="tabAddressInfo" runat="server" HeaderText="Address Info" TabIndex="2">
                            <ContentTemplate>
                                <uc4:UCAddress ID="UCAddress1" runat="server" />
                            </ContentTemplate>
                        </asp:TabPanel>
                        <asp:TabPanel ID="tabContactInfo" runat="server" HeaderText="Contact Person Info" TabIndex="3">
                            <ContentTemplate>
                                <uc1:UCContactPerson ID="UCContactPerson1" runat="server" />
                            </ContentTemplate>
                        </asp:TabPanel>
                        <asp:TabPanel ID="tabLocation" runat="server" HeaderText="Location Info" TabIndex="4">
                            <ContentTemplate>
                                <%--  <uc5:UCLocation ID="UCLocation" runat="server" />--%>
                                <center>
                                    <table class="gridFrame" style="margin-top: 10px; width: 100%">
                                        <tr>
                                            <td style="text-align: left;">
                                                <span class="headerText">Location List</span>
                                            </td>
                                            <td style="text-align: right;">
                                                <input type="button" id="btnImport" value="Import Location" onclick="importLocation();" />
                                                <input type="button" id="btnContactPerson" value="Add New" onclick="openLocationWindow('0')" />
                                                &nbsp;&nbsp;
                                                 <%-- <input type="button" id="btnMoveToArchiveContactPerson" value="Archive" onclick="MoveToArchiveContactPerson()" />--%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <obout:Grid ID="GVLocation" runat="server" AutoGenerateColumns="false" AllowAddingRecords="false"
                                                    AllowFiltering="true" AllowGrouping="true" AllowMultiRecordSelection="False"
                                                    AllowRecordSelection="true" Width="100%" AllowColumnResizing="true" AllowColumnReordering="true"
                                                    OnRebind="GVLocation_OnRebind">
                                                    <ClientSideEvents ExposeSender="true" />
                                                    <Columns>
                                                           <obout:Column DataField="ID" HeaderText="Edit" Width="5%" AllowSorting="false"
                                                            Align="center" HeaderAlign="center">
                                                            <TemplateSettings TemplateId="grdeditloc" />
                                                          </obout:Column>
                                                          <obout:Column HeaderText="Building" DataField="Building" Width="10%" Wrap="true" Align="center" HeaderAlign="center">
                                                        </obout:Column>
                                                         <obout:Column HeaderText="Floar" DataField="Floar" Width="10%" Wrap="true" Align="center" HeaderAlign="center">
                                                        </obout:Column>
                                                         <obout:Column HeaderText="Passage" DataField="Passage" Width="10%" Wrap="true" Align="center" HeaderAlign="center">
                                                        </obout:Column>
                                                         <obout:Column HeaderText="Section" DataField="Section" Width="10%" Wrap="true" Align="center" HeaderAlign="center">
                                                        </obout:Column>
                                                         <obout:Column HeaderText="Shelf" DataField="Shelf" Width="10%" Wrap="true" Align="center" HeaderAlign="center">
                                                        </obout:Column>
                                                        <obout:Column HeaderText="Location Code" DataField="Code" Width="10%" Wrap="true" Align="center" HeaderAlign="center">
                                                        </obout:Column>
                                                        <obout:Column HeaderText="Sort Code" DataField="SortCode" Width="10%" Wrap="true">
                                                        </obout:Column>
                                                        <obout:Column HeaderText="Velocity Type" DataField="VelocityType" Width="10%" Wrap="true">
                                                        </obout:Column>
                                                        <obout:Column HeaderText="Capacity In" DataField="CapacityIn" Width="8%" Wrap="true">
                                                        </obout:Column>
                                                        <obout:Column HeaderText="Capacity" DataField="Capacity" Width="8%" Wrap="true">
                                                        </obout:Column>
                                                        <obout:Column HeaderText="Active" DataField="Active" Width="6%" Wrap="true">
                                                        </obout:Column>
                                                    </Columns>
                                                     <Templates>
                                                 <obout:GridTemplate ID="grdeditloc" runat="server">
                                                            <Template>
                                                                <img id="imgBtnEdit" src="../App_Themes/Blue/img/Edit16.png" title="Edit" onclick="OpenLocation('<%# (Container.DataItem["ID"].ToString()) %>');"
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
                    </asp:TabContainer>
                </ContentTemplate>
            </asp:UpdatePanel>
        </center>
    </div>

    <%-- Edit Mode Select Code To Get ID--%>
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

            for (var i = 0; i < grdWarehouseList.PageSelectedRecords.length; i++) {
                var record = grdWarehouseList.PageSelectedRecords[i];
                if (hdnPartSelectedRec.value == "") hdnPartSelectedRec.value = record.ID;
            }
        }
    </script>

    <%--Code To Get Customer in DropDown List and Assign Value to hidden Field--%>
    <script type="text/javascript">
        var ddlcompany = document.getElementById("<%=ddlcompany.ClientID %>");
        var ddlcustomer = document.getElementById("<%=ddlcustomer.ClientID %>");

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
        function GetCustomerID() {
            document.getElementById("<%=hdncustomerid.ClientID %>").value = ddlcustomer.value.toString();
         }

    </script>

    <%--Validation Code On Next Button Click--%>
    <script type="text/javascript">
        function Checkvalidations() {



            if (document.getElementById("<%=ddlcompany.ClientID %>").value == "0") {
                showAlert("Please Select Company!", "error", "#");
                document.getElementById("<%=ddlcompany.ClientID %>").focus();
            return false;
        }

        if (document.getElementById("<%=ddlcustomer.ClientID %>").value == "0") {
                showAlert("Please Select Customer!", "error", "#");
                document.getElementById("<%=ddlcustomer.ClientID %>").focus();
            return false;
        }
        if (document.getElementById("<%=txtcode.ClientID %>").value == "") {
                showAlert("Please Enter Warehouse Code!", "error", "#");
                document.getElementById("<%=txtcode.ClientID %>").focus();
            return false;
        }

        if (document.getElementById("<%=txtwarehousename.ClientID %>").value == "") {
                showAlert("Please Enter Warehouse Name!", "error", "#");
                document.getElementById("<%=txtwarehousename.ClientID %>").focus();
            return false;
        }
        if (document.getElementById("<%=ddltype.ClientID %>").value == "0") {
                showAlert("Please Select Warehouse type!", "error", "#");
                document.getElementById("<%=ddltype.ClientID %>").focus();
            return false;
        }
        if (document.getElementById("<%=txtCAddress1.ClientID %>").value == "") {
                showAlert("Please Enter Address!", "error", "#");
                document.getElementById("<%=txtCAddress1.ClientID %>").focus();
                return false;
            }
            if (document.getElementById("<%=ddlCountry.ClientID %>").value == "0") {
                showAlert("Please Select Country!", "error", "#");
                document.getElementById("<%=ddltype.ClientID %>").focus();
                return false;
            }
            if (document.getElementById("<%=ddlState.ClientID %>").value == "0") {
                showAlert("Please Select State!", "error", "#");
                document.getElementById("<%=ddlState.ClientID %>").focus();
                return false;
            }
            if (document.getElementById("<%=txtCity.ClientID %>").value == "") {
                showAlert("Please Enter City!", "error", "#");
                document.getElementById("<%=txtCity.ClientID %>").focus();
                return false;
            }
            return true
        };
    </script>

    <%--Code For Country State--%>
    <script type="text/javascript">
        print_country('ddlCountry');
        function onCountryChange(ddlC) {
            document.getElementById('hdnCountry').value = ddlC.options[ddlC.selectedIndex].text;
        }
        function onStateChange(ddlS) {
            document.getElementById('hdncountryState').value = ddlS.options[ddlS.selectedIndex].text;
        }

        function setCountry(country, state) {
            var ddlCountry = document.getElementById("<%=ddlCountry.ClientID %>");
            ddlCountry.value = country;

            print_state('ddlState', ddlCountry.selectedIndex);
            ddlState = document.getElementById("<%=ddlState.ClientID %>");
            ddlState.value = state;

        }

    </script>

    <%--Code Related To Location & Open Location Page--%>
    <script type="text/javascript">
        function openLocationWindow(Sequence) {
            var warehouseID = document.getElementById("<%=hdnwarehouseID.ClientID %>").value;
            var CustomerID = document.getElementById("<%=hdncustomerid.ClientID %>").value;
            var LocationID = 0;
            window.open('../Warehouse/WarehouseLocation.aspx?warehouseID=' + warehouseID + '&CustomerID=' + CustomerID + '&LocationID=' + LocationID + '', null, 'height=350px, width=1190px,status=0, resizable=0, scrollbars=0, toolbar=0,menubar=0');
        }

        function OpenLocation(ID) {
            var CustomerID;
            var warehouseID = document.getElementById("<%=hdnwarehouseID.ClientID %>").value;
            CustomerID = document.getElementById("<%= hdncustomerid.ClientID %>").value;
            if (CustomerID == "") {
                CustomerID = document.getElementById("<%= hdnNewCustomerID.ClientID %>").value;
            }
            //var LocationID = document.getElementById("<%=hdnlocationID.ClientID %>").value;
            var locationID = ID;
            window.open('../Warehouse/WarehouseLocation.aspx?warehouseID=' + warehouseID + '&CustomerID=' + CustomerID + '&LocationID=' + locationID + "", null, 'height=250px, width=1070px,status= 0, resizable= 1, scrollbars=1, toolbar=0,location=center,menubar=0, screenX=300; screenY=50');

        }

        function importLocation() {
            window.open('../Warehouse/ImportLocation.aspx', '_self', '');
        }

    </script>

</asp:Content>

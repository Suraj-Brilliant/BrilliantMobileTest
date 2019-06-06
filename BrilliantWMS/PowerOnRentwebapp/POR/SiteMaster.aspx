<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/CRM.Master" AutoEventWireup="true"
    EnableEventValidation="false" CodeBehind="SiteMaster.aspx.cs" Inherits="BrilliantWMS.POR.SiteMaster" %>

<%@ Register Assembly="obout_Grid_NET" Namespace="Obout.Grid" TagPrefix="obout" %>
<%@ Register Src="../CommonControls/UCFormHeader.ascx" TagName="UCFormHeader" TagPrefix="uc4" %>
<%@ Register Src="../CommonControls/UCToolbar.ascx" TagName="UCToolbar" TagPrefix="uc5" %>
<%@ Register Src="../CommonControls/UC_Date.ascx" TagName="UC_Date" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHolder_FormHeader" runat="server">
    <uc5:UCToolbar ID="UCToolbar1" runat="server" />
    <uc4:UCFormHeader ID="UCFormHeader1" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPHolder_Form" runat="server">
    <script src="../MasterPage/JavaScripts/countries.js" type="text/javascript"></script>
    <asp:ValidationSummary ID="validationsummary_SiteM" runat="server" ShowMessageBox="true"
        ShowSummary="false" DisplayMode="bulletlist" ValidationGroup="Save" />
    <center>
        <table class="tableForm">
            <tr>
                <td>
                    <req>Site Name :</req>
                </td>
                <td style="text-align: left">
                    <asp:TextBox ID="txtSiteName" runat="server" Width="200px"></asp:TextBox>
                </td>
                <td>
                    <req>Address :</req>
                </td>
                <td style="text-align: left" colspan="3">
                    <asp:TextBox ID="txtCAddress1" runat="server" MaxLength="100" onkeyup="TextBox_KeyUp(this,'SpanAddressLine1','100');"
                        Width="470px"></asp:TextBox>
                    <br />
                    <span class="watermark"><span id="SpanAddressLine1">100</span> characters remaining
                        out of 100 </span>
                    <asp:RequiredFieldValidator ID="RfValtxtAddress1" runat="server" ControlToValidate="txtCAddress1"
                        Display="None" ErrorMessage="Fill Address" ValidationGroup="Save"></asp:RequiredFieldValidator>
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td>
                    <req>Country :</req>
                </td>
                <td style="text-align: left;">
                    <asp:DropDownList ID="ddlCountry" ClientIDMode="Static" runat="server" AutoPostBack="false"
                        onchange="print_state('ddlState',this.selectedIndex); onCountryChange(this);"
                        Width="205px">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="RFValddlCountry" runat="server" ForeColor="Maroon"
                        ValidationGroup="Save" InitialValue="0" ControlToValidate="ddlCountry" ErrorMessage="Select Country"
                        Display="None"></asp:RequiredFieldValidator>
                </td>
                <td>
                    <req>State :</req>
                </td>
                <td>
                    <asp:DropDownList ID="ddlState" ClientIDMode="Static" runat="server" Width="205px"
                        onchange="onStateChange(this);">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="RFValstate" ForeColor="Maroon" ControlToValidate="ddlState"
                        ValidationGroup="Save" InitialValue="0" runat="server" ErrorMessage="Select State"
                        Display="None"></asp:RequiredFieldValidator>
                </td>
                <td>
                    <req>City :</req>
                </td>
                <td>
                    <asp:TextBox ID="txtCity" runat="server" MaxLength="50" Width="200px"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RFValddlBCity" runat="server" ControlToValidate="txtCity"
                        Display="None" ErrorMessage="Please Select City" ValidationGroup="Save"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td>
                    ZIP Code :
                </td>
                <td style="text-align: left">
                    <asp:TextBox ID="txtZipCode" runat="server" MaxLength="7" Width="200px"></asp:TextBox>
                </td>
                <td>
                    Landmark :
                </td>
                <td>
                    <asp:TextBox ID="txtLandMark" runat="server" MaxLength="50" Width="200px"></asp:TextBox>
                </td>
                <td>
                    Email ID :
                </td>
                <td>
                    <asp:TextBox ID="txtemailid" runat="server" MaxLength="100" Width="200px" AutoPostBack="false"></asp:TextBox>
                    <asp:RegularExpressionValidator ID="RegExpEmailID" runat="server" ControlToValidate="txtemailid"
                        Display="None" ErrorMessage="Please enter valid Email ID" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                        ValidationGroup="Save"></asp:RegularExpressionValidator>
                </td>
            </tr>
            <tr>
                <td>
                    Phone No. :
                </td>
                <td style="text-align: left">
                    <asp:TextBox ID="txtphoneno" runat="server" MaxLength="50" onkeydown="return AllowPhoneNo(this, event);"
                        onkeypress="return AllowPhoneNo(this, event);" Width="200px"></asp:TextBox>
                </td>
                <td>
                    Fax No. :
                </td>
                <td>
                    <asp:TextBox ID="txtFax" runat="server" MaxLength="50" onkeydown="return AllowPhoneNo(this, event);"
                        onkeypress="return AllowPhoneNo(this, event);" Width="200px"></asp:TextBox>
                </td>
                <%--  <td>
                            Mobile No. :
                        </td>
                        <td>
                            <asp:TextBox ID="txtmobileno" runat="server" MaxLength="50" onkeydown="return AllowPhoneNo(this, event);"
                                onkeypress="return AllowPhoneNo(this, event);" Width="200px"></asp:TextBox>
                        </td>--%>
            </tr>
        </table>
        <table class="gridFrame" style="width: 100%">
            <tr>
                <td>
                    <table style="width: 100%">
                        <tr>
                            <td>
                                <a id="headerText">Site List</a>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <obout:Grid ID="GvSite" runat="server" AutoGenerateColumns="False" AllowAddingRecords="False"
                        OnSelect="GvSite_Select" AllowGrouping="True" AllowFiltering="True" Width="100%">
                        <Columns>
                            <obout:Column ID="Edit" Width="5%" AllowFilter="False" HeaderText="Edit" Index="0"
                                TemplateId="GvTempEdit">
                                <TemplateSettings TemplateId="GvTempEdit" />
                            </obout:Column>
                            <obout:Column DataField="ID" Visible="False" HeaderText="ID" Index="1">
                            </obout:Column>
                            <obout:Column DataField="Territory" HeaderText="Site Name" Width="20%" Index="2">
                            </obout:Column>
                            <obout:Column HeaderText="Address" DataField="AddressLine1" Width="20%" Index="3">
                            </obout:Column>
                            <obout:Column HeaderText="Country" DataField="County" Width="15%" Index="4">
                            </obout:Column>
                            <obout:Column DataField="State" HeaderText="State" Width="15%" Index="5">
                            </obout:Column>
                            <obout:Column DataField="City" HeaderText="City" Width="15%" Index="6">
                            </obout:Column>
                            <obout:Column DataField="Phoneno" HeaderText="Phone No." Width="10%" Index="7">
                            </obout:Column>
                            <obout:Column DataField="Emailid" HeaderText="Email Id" Width="10%" Index="8">
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
    <asp:HiddenField ID="hdnState" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnCountry" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="HdnSiteId" runat="server" ClientIDMode="Static" />
    <script type="text/javascript">
        print_country('ddlCountry');
        function onCountryChange(ddlC) {
            document.getElementById('hdnCountry').value = ddlC.options[ddlC.selectedIndex].text;
        }
        function onStateChange(ddlS) {
            document.getElementById('hdnState').value = ddlS.options[ddlS.selectedIndex].text;
        }

        function setCountry(country, state) {
            var ddlCountry = document.getElementById("ddlCountry");
            ddlCountry.value = country;

            print_state('ddlState', ddlCountry.selectedIndex);
            ddlState = document.getElementById("ddlState");
            ddlState.value = state;

        }

    </script>
</asp:Content>

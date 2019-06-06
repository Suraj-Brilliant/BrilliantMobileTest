<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/CRM.Master" AutoEventWireup="true" CodeBehind="ChannelMaster.aspx.cs" EnableEventValidation="false"
    Inherits="BrilliantWMS.Account.ChannelMaster" Theme="Blue" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="Obout.Ajax.UI" Namespace="Obout.Ajax.UI.FileUpload" TagPrefix="obout" %>
<%@ Register Src="../ContactPerson/UCContactPerson.ascx" TagName="UCContactPerson" TagPrefix="uc1" %>
<%@ Register Src="../Document/UC_AttachDocument.ascx" TagName="UC_AttachDocument" TagPrefix="uc5" %>
<%@ Register Src="../CommonControls/UC_Date.ascx" TagName="UC_Date" TagPrefix="uc6" %>
<%@ Register Assembly="obout_Interface" Namespace="Obout.Interface" TagPrefix="obout" %>
<%@ Register Assembly="obout_Grid_NET" Namespace="Obout.Grid" TagPrefix="obout" %>
<%@ Register Src="../CommonControls/UCFormHeader.ascx" TagName="UCFormHeader" TagPrefix="uc7" %>
<%@ Register Src="../CommonControls/UCToolbar.ascx" TagName="UCToolbar" TagPrefix="uc8" %>
<%@ Register Src="../Address/UCAddress.ascx" TagName="UCAddress" TagPrefix="uc4" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHolder_FormHeader" runat="server">
    <uc8:UCToolbar ID="UCToolbar1" runat="server" />
    <uc7:UCFormHeader ID="UCFormHeader1" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPHolder_Form" runat="server">
    <script src="../MasterPage/JavaScripts/countries.js" type="text/javascript"></script>
    <asp:ValidationSummary ID="validationsummary_AccountMaster" runat="server" ShowMessageBox="true"
        ShowSummary="false" DisplayMode="bulletlist" ValidationGroup="Save" />
    <center>
        <asp:TabContainer runat="server" ID="tabChannelMaster" ActiveTabIndex="2" Width="100%">
            <asp:TabPanel ID="TabChannelList" runat="server" HeaderText="Channel List">
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
                    <asp:HiddenField ID="hdnselectedRec" runat="server" ClientIDMode="Static" />
                    <asp:HiddenField ID="hndCompanyid" runat="server"></asp:HiddenField>
                    <asp:HiddenField ID="hdncustomerid" runat="server" ClientIDMode="Static"></asp:HiddenField>
                    <asp:HiddenField ID="hdnchannelid" runat="server" ClientIDMode="Static" />
                    <asp:HiddenField ID="hdnCountry" runat="server" ClientIDMode="Static" />
                    <asp:HiddenField ID="hdnState" runat="server" ClientIDMode="Static" />
                    <asp:HiddenField ID="hdnselectedchanID" runat="server" ClientIDMode="Static" />
                   
                    <center>
                        <table class="gridFrame" style="width: 100%">
                            <tr>
                                <td>
                                    <table style="width: 100%">
                                        <tr>
                                            <td style="text-align: left;">
                                                <a id="headerText">
                                                    <asp:Label ID="lblheasertext" runat="server" Text="Channel List"></asp:Label></a>
                                            </td>
                                            <td style="text-align: right;">
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <obout:Grid ID="grdchannel" runat="server" AutoGenerateColumns="False" AllowAddingRecords="False"
                                        AllowGrouping="True" AllowFiltering="True" Width="100%" OnSelect="grdchannel_Select" >
                                        <ScrollingSettings ScrollHeight="250" />
                                        <Columns>
                                            <obout:Column ID="Edit" Width="4%" AllowFilter="False" HeaderText="Edit" Index="0"
                                                TemplateId="GvTempEdit">
                                                <TemplateSettings TemplateId="GvTempEdit" />
                                            </obout:Column>
                                            <obout:Column DataField="ID" Visible="False" HeaderText="ID" Index="1">
                                            </obout:Column>
                                             <obout:Column DataField="Company" HeaderText="Company" Width="12%" Index="2">
                                            </obout:Column>
                                            <obout:Column DataField="Customer" HeaderText="Customer" Width="12%" Index="3">
                                            </obout:Column>
                                            <obout:Column HeaderText="Type" DataField="Type" Width="10%" Index="4">
                                            </obout:Column>
                                            <obout:Column HeaderText="Channel" DataField="ChannelName" Width="10%" Index="5">
                                            </obout:Column>
                                             <obout:Column DataField="ChannelURL" HeaderText="Channel URL" Width="20%" Index="6">
                                            </obout:Column>
                                            <obout:Column DataField="UserName" HeaderText="User Name" Width="8%" Index="7">
                                            </obout:Column>
                                            <obout:Column DataField="Active" HeaderText="Active" Width="6%" Index="9">
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
            </asp:TabPanel>
            <asp:TabPanel ID="tabChannelInfo" runat="server" HeaderText="Channel Info">
                <ContentTemplate>
                    <center>
                        <table class="tableForm">
                            <tr>
                                 <td>
                                <req><asp:Label Id="lblcompany" runat="server" Text="Company"></asp:Label></req> :
                            </td>
                            <td style="text-align: left">
                                <asp:DropDownList ID="ddlcompany" runat="server" DataTextField="Name" DataValueField="ID" onchange="GetCustomer()" Width="206px">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RFQCompany" runat="server" ControlToValidate="ddlcompany" InitialValue="0"
                                        Display="None" ErrorMessage="Please Select Company" ValidationGroup="Save"></asp:RequiredFieldValidator>
                            </td>
                                <td>
                                    <req><asp:Label Id="lblcustomer" runat="server" Text="Customer"></asp:Label></req>
                                    :
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlcutomer" runat="server"  DataTextField="Name" DataValueField="ID" onchange="GetCustomerID()" Width="206px">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RfValCompany" runat="server" ControlToValidate="ddlcutomer" InitialValue="0"
                                        Display="None" ErrorMessage="Please Select Customer" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                </td>
                                <td>
                                    <req><asp:Label ID="lbltype" runat="server" Text="Channel Type" /></req>
                                    :
                                </td>
                                <td>
                                     <asp:DropDownList ID="ddlchantype" runat="server" onchange="GetChannelName()" Width="206px">
                                         <asp:ListItem Text="-Select-" Value="0"></asp:ListItem>
                                         <asp:ListItem Text="Standard" Value="1"></asp:ListItem>
                                         <asp:ListItem Text="Company" Value="2"></asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="ddlchantype" InitialValue="0"
                                        Display="None" ErrorMessage="Please Select Channel Type" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                  </td>
                               
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label Id="lblchannel" runat="server" Text="Channel"/>
                                    :
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlchannel" runat="server"  DataTextField="Value" DataValueField="Id" Width="206px" onchange="GetChannelID()" >
                                    </asp:DropDownList>
                                    <%-- <asp:RequiredFieldValidator ID="RFValchanlname" runat="server" ControlToValidate="ddlchannel"
                                        Display="None" ErrorMessage="Please Select Channel" ValidationGroup="Save"></asp:RequiredFieldValidator>--%>
                                </td>
                                 <td>
                                    <req><asp:Label Id="lblchannelName" runat="server" Text="Channel Name"/></req>
                                    :
                                </td>
                                <td>
                                     <asp:TextBox ID="txtchannelname" runat="server" MaxLength="100" Width="200px"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtchannelname"
                                        Display="None" ErrorMessage="Please Enter Channel Name" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                </td>
                               <td>
                                    <req><asp:Label Id="lblchannelUrl" runat="server" Text="Channel URL"/></req>
                                    :
                                </td>
                                <td>
                                   <asp:TextBox ID="txtchanlUrl" runat="server" MaxLength="100" Width="200px"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtchanlUrl"
                                        Display="None" ErrorMessage="Please enter Channel URL" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                  <td>
                                    <req><asp:Label Id="lblusername" runat="server" Text="User Name"/></req>
                                    :
                                </td>
                                <td>
                                   <asp:TextBox ID="txtusername" runat="server" MaxLength="100" Width="200px"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtusername"
                                        Display="None" ErrorMessage="Please enter User Name" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                </td>
                                 <td>
                                    <req><asp:Label Id="lblpassword" runat="server" Text="Password"/></req>
                                    :
                                </td>
                                <td>
                                   <asp:TextBox ID="txtpassword" runat="server"  MaxLength="100" Width="200px" TextMode="Password"></asp:TextBox>      <%--TextMode="Password"--%>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="txtpassword"
                                        Display="None" ErrorMessage="Please enter Password" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                </td>
                                 <td>
                                    <req><asp:Label Id="lblactive" runat="server" Text="Active"/></req>
                                    :
                                </td>
                                <td style="text-align: left">
                                    <obout:OboutRadioButton ID="rbtnActiveYes" runat="server" Checked="True" FolderStyle=""
                                        GroupName="Active" Text="Yes">
                                    </obout:OboutRadioButton>
                                    <obout:OboutRadioButton ID="rbtnActiveNo" runat="server" FolderStyle="" GroupName="Active"
                                        Text="No">
                                    </obout:OboutRadioButton>
                                </td>
                            </tr>
                        </table>
                    </center>
                </ContentTemplate>
            </asp:TabPanel>
           
        </asp:TabContainer>
    </center>

    <%--Get Customer List From CompanyID--%>
    <script type="text/javascript">
        var ddlcompany = document.getElementById("<%=ddlcompany.ClientID %>");
        var ddlcustomer = document.getElementById("<%=ddlcutomer.ClientID %>");

        function GetCustomer() {
            var obj1 = new Object();
            obj1.ddlcompanyId = ddlcompany.value.toString();
            document.getElementById("<%=hndCompanyid.ClientID %>").value = ddlcompany.value.toString();
            PageMethods.GetCustomerByComp(obj1, getCust_onSuccessed);
        }

        function getCust_onSuccessed(result) {

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

   <%-- Get Channel List from channel Type--%>
    <script type="text/javascript">
        var ddlchannelType = document.getElementById("<%=ddlchantype.ClientID %>");
        var ddlchannel = document.getElementById("<%=ddlchannel.ClientID %>");

        function GetChannelName() {
            if (ddlchannelType.value == "1") {
                document.getElementById("<%=ddlchannel.ClientID %>").disabled = false;
                var obj1 = new Object();
                obj1.ChannelType = ddlchannelType.value.toString();
                // document.getElementById("<%=hndCompanyid.ClientID %>").value = ddlcompany.value.toString();
                PageMethods.GetChannel(obj1, getChan_onSuccessed);
            }
            else
            {
                document.getElementById("<%=ddlchannel.ClientID %>").disabled = true;
            }
        }

        function getChan_onSuccessed(result) {

            ddlchannel.options.length = 0;
            for (var i in result) {
                AddOption1(result[i].Name, result[i].Id);
            }
        }

        function AddOption1(text, value) {

            var option = document.createElement('option');
            option.value = value;
            option.innerHTML = text;
            ddlchannel.options.add(option);
        }

        function GetChannelID() {
            document.getElementById("<%=hdnchannelid.ClientID %>").value = ddlchannel.value.toString();
        }
    </script>
</asp:Content>

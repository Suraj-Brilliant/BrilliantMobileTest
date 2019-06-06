<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/CRM.Master" AutoEventWireup="true"
    EnableEventValidation="false" CodeBehind="EngineMaster.aspx.cs" Inherits="BrilliantWMS.POR.EngineMaster" %>

<%@ Register Assembly="obout_Grid_NET" Namespace="Obout.Grid" TagPrefix="obout" %>
<%@ Register Src="../CommonControls/UCFormHeader.ascx" TagName="UCFormHeader" TagPrefix="uc2" %>
<%@ Register Src="../CommonControls/UC_Date.ascx" TagName="UC_Date" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../CommonControls/UCToolbar.ascx" TagName="UCToolbar" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHolder_FormHeader" runat="server">
    <uc1:UCToolbar ID="UCToolbar1" runat="server" />
    <uc2:UCFormHeader ID="UCFormHeader1" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPHolder_Form" runat="server">
    <div id="tabletest">
        <center>
            <asp:ValidationSummary ID="validationsummary_EngineMaster" runat="server" ShowMessageBox="True"
                DisplayMode="bulletlist" ShowSummary="False" ValidationGroup="Save" />
            <center>
                <table class="tableForm">
                    <tr>
                        <td>
                            <req> Site/Ware House :</req>
                        </td>
                        <td style="text-align: left">
                            <asp:DropDownList ID="ddlSite" runat="server" DataValueField="ID" DataTextField="Territory"
                                Width="165px">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="ReqValddlSite" runat="server" ErrorMessage="Select Site/Ware House"
                                ValidationGroup="Save" ControlToValidate="ddlSite" ForeColor="Red" InitialValue="0"
                                Display="None"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <req>Container :</req>
                        </td>
                        <td style="text-align: left">
                            <asp:TextBox ID="txtContainer" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="req_txtContainer" runat="server" ErrorMessage="Enter Container"
                                ControlToValidate="txtContainer" ForeColor="Red" ValidationGroup="Save" Display="None"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <req> Engine Serial No :</req>
                        </td>
                        <td style="text-align: left">
                            <asp:TextBox ID="txtEngineSerialNo" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="req_txtEngineSerialNo" runat="server" ErrorMessage="Enter Engine Serial Number"
                                ControlToValidate="txtEngineSerialNo" ForeColor="Red" ValidationGroup="Save"
                                Display="None"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <req>Engine Model :</req>
                        </td>
                        <td style="text-align: left">
                            <asp:TextBox ID="txtEngineModel" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="Req_txtEngineModel" runat="server" ErrorMessage="Enter Engine Model "
                                ControlToValidate="txtEngineModel" ForeColor="Red" ValidationGroup="Save" Display="None"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <req>Generator Serial No :</req>
                        </td>
                        <td style="text-align: left">
                            <asp:TextBox ID="txtGenratorSerialNo" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="Req_txtGenratorSerialNo" runat="server" ErrorMessage="Enter Genrator Serial Number"
                                ControlToValidate="txtGenratorSerialNo" ForeColor="Red" ValidationGroup="Save"
                                Display="None"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <req>Generator Model :</req>
                        </td>
                        <td style="text-align: left">
                            <asp:TextBox ID="txtGenratorModel" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="Req_txtGenratorModel" runat="server" ErrorMessage="Enter Genrator Model"
                                ControlToValidate="txtGenratorModel" ForeColor="Red" ValidationGroup="Save" Display="None"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <req> Transformer Serial No</req>
                        </td>
                        <td style="text-align: left">
                            <asp:TextBox ID="txtTransformerSerialNo" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="req_txtTransformerSerialNo" runat="server" ErrorMessage="Enter Transformer Serial Number"
                                ControlToValidate="txtTransformerSerialNo" ForeColor="Red" ValidationGroup="Save"
                                Display="None"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <req> Date Received :</req>
                        </td>
                        <td style="text-align: left">
                            <uc1:UC_Date ID="UC_DateRecived" runat="server" />
                            <td>
                                <req>Transformer Date Recevied :</req>
                            </td>
                            <td style="text-align: left">
                                <uc1:UC_Date ID="UC_TrasformerDateRecv" runat="server" />
                            </td>
                    </tr>
                    <tr>
                        <td>
                            Remark/Description :
                        </td>
                        <td colspan="5" style="text-align: left">
                            <asp:TextBox ID="txtRemark" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <br />
                <table class="gridFrame" style="width: 100%">
                    <tr>
                        <td>
                            <table style="width: 100%">
                                <tr>
                                    <td style="text-align: left;">
                                        <a id="headerText">Engine List</a>
                                    </td>
                                </tr>
                                <asp:HiddenField ID="hdnEngineId" runat="server" />
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <obout:Grid ID="GvMEngine" runat="server" AllowAddingRecords="False" AutoGenerateColumns="False"
                                AllowGrouping="True" AllowFiltering="True" Width="100%" PageSize="7" OnSelect="GvEngine_Select">
                                <Columns>
                                    <obout:Column ID="Edit" DataField="" HeaderText="Edit" Width="5%" TemplateId="GvTempEdit"
                                        Index="0">
                                        <TemplateSettings TemplateId="GvTempEdit" />
                                    </obout:Column>
                                    <obout:Column DataField="ID" HeaderText="ID" Visible="false">
                                    </obout:Column>
                                    <obout:Column HeaderText="Date" DataFormatString="{0:dd-MMM-yy}" Width="10%" DataField="DateRecevied"
                                        DataFormatString_GroupHeader="{0:dd-MMM-yy}" Index="1">
                                    </obout:Column>
                                    <obout:Column DataField="EngineSerial" HeaderAlign="center" Align="center" Width="10%"
                                        HeaderText="Engine Sr. No" Index="2">
                                    </obout:Column>
                                    <obout:Column DataField="Container" Index="3" HeaderText="Container" Width="10%">
                                    </obout:Column>
                                    <obout:Column DataField="EngineModel" HeaderText="Engine Model" Width="10%" Index="4">
                                    </obout:Column>
                                    <obout:Column DataField="GeneratorModel" HeaderText="Generator Model" Width="10%" Index="5">
                                    </obout:Column>
                                    <obout:Column DataField="EngineSerial" HeaderText="Engine Sr. No." Width="10%" Index="6">
                                    </obout:Column>
                                    <obout:Column DataField="Territory" HeaderText="Site/Ware House" Index="7" Width="10%">
                                    </obout:Column>
                                    <obout:Column DataField="TransformerDateRecevied" HeaderText="Transformer Date Received"
                                        DataFormatString="{0:dd-MMM-yy}" Width="15%" DataFormatString_GroupHeader="{0:dd-MMM-yy}"
                                        Index="8">
                                    </obout:Column>
                                    <obout:Column DataField="Remark" HeaderText="Remarks" Index="9" Width="10%">
                                    </obout:Column>
                                </Columns>
                                <Templates>
                                    <obout:GridTemplate ID="GvTempEdit" runat="server" ControlID="" ControlPropertyName="">
                                        <Template>
                                            <asp:ImageButton ID="imgBtnEdit" CausesValidation="false" runat="server" ImageUrl="../App_Themes/Blue/img/Edit16.png" />
                                        </Template>
                                    </obout:GridTemplate>
                                </Templates>
                            </obout:Grid>
                        </td>
                    </tr>
                </table>
            </center>
        </center>
    </div>
</asp:Content>

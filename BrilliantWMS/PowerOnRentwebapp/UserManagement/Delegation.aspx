<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/CRM2.Master" AutoEventWireup="true" CodeBehind="Delegation.aspx.cs"
     Inherits="BrilliantWMS.UserManagement.Delegation" Theme="Blue" EnableEventValidation="false" %>
<%@ Register Assembly="obout_Grid_NET" Namespace="Obout.Grid" TagPrefix="obout" %>
<%@ Register Assembly="obout_Interface" Namespace="Obout.Interface" TagPrefix="obout" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../Territory/UC_Territory.ascx" TagName="UC_Territory" TagPrefix="uc1" %>
<%@ Register Src="../Address/UCAddress.ascx" TagName="UCAddress" TagPrefix="uc7" %>
<%@ Register Src="../CommonControls/UC_Date.ascx" TagName="UC_Date" TagPrefix="uc2" %>
<%@ Register Src="../CommonControls/UCFormHeader.ascx" TagName="UCFormHeader" TagPrefix="uc4" %>
<%@ Register Src="../CommonControls/UCToolbar.ascx" TagName="UCToolbar" TagPrefix="uc5" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


   
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

                                <table class="tableForm" border="0">
                                    <tr>
                                         <td colspan="2" style="text-align: left;">
                                              <asp:Label ID="lblheader" CssClass="headerText" runat="server" Text="Access Delegation"></asp:Label>
                                        </td>

                                    </tr>
                                    <tr>
                                        <td>
                                            <req><asp:Label ID="lblfrmdate"  runat="server" Text="From Date"/>*</req>
                                            :
                                        </td>
                                        <td style="text-align: left;">
                                            <uc2:UC_Date ID="UC_Date1" runat="server" />
                                        </td>
                                        <td>
                                            <req> <asp:Label ID="lbltodate"  runat="server" Text="To Date"/>*</req>
                                            :
                                        </td>
                                        <td style="text-align: left;">
                                            <uc2:UC_Date ID="UC_Date2" runat="server" />
                                        </td>
                                        <td>
                                            <req><asp:Label ID="lbldept"  runat="server" Text="Select Department "/>*</req>:
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlDepartment" runat="server" ValidationGroup="Save" DataTextField="Territory"
                                        DataValueField="ID" Width="240px" onchange="GetDepartment();"
                                        ClientIDMode="Static">
                                    </asp:DropDownList>

                                        </td>
                                        <td>
                                            <req><asp:Label ID="lblrightsto"  runat="server" Text="Delegate Approval Rights to"/>*</req>
                                            :
                                        </td>
                                        <td style="text-align: left;">
                                            <asp:DropDownList ID="ddlUOM" runat="server" DataTextField="Name" DataValueField="ID"
                                                Width="243px">
                                                <asp:ListItem Text="-Select-" Value="0"></asp:ListItem>
                                            </asp:DropDownList>
                                            <%--<asp:RequiredFieldValidator ID="req_ddlUOM" runat="server" ErrorMessage="Please Select Delegate"
                                                ControlToValidate="ddlUOM" ValidationGroup="Save" Display="None" InitialValue="0"></asp:RequiredFieldValidator>--%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblremark" runat="server" Text="Remark" />
                                            :
                                        </td>
                                        <td colspan="8" style="text-align: left;">
                                            <asp:TextBox ID="txtPrincipalPrice" runat="server" TextMode="MultiLine" MaxLength="1000"
                                                Width="934px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="9" style="text-align: right;">
                                            <%--<asp:Button ID="btnsubmit" runat="server" Text="Submit" OnClientClick="SaveAccessDelegation()"  />--%>
                                            <input type="button" runat="server" id="btnsumit" value="Submit" onclick="SaveAccessDelegation()" />
                                        </td>
                                    </tr>
                                </table>
                                <table class="gridFrame" width="90%">
                                    <tr>
                                        <td>
                                            <table style="width: 90%">
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblaccdele" CssClass="headerText" runat="server" Text="Access Delegation" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>         <%-- OnSelect="grdaccessdele_Select">--%>
                                            <obout:Grid ID="grdaccessdele" runat="server" AllowFiltering="true" AllowGrouping="true"
                                                AllowSorting="true" AllowColumnResizing="true" AutoGenerateColumns="false" Width="100%"
                                                AllowAddingRecords="true" OnRebind="grdaccessdele_RebindGrid">
                                                 <ScrollingSettings ScrollHeight="250" />       
                                                <Columns>
                                                 <obout:Column DataField="ID" HeaderText="Remove" Width="5%" AllowSorting="false" Align="Center" HeaderAlign="Center" Index="0">
                                                            <TemplateSettings TemplateId="GvRemoveSku" />
                                                     </obout:Column>
                                                  <%--  <obout:Column ID="Edit" DataField="ID" HeaderText="Edit" Width="0%" TemplateId="GvTempEdit" Index="1" Visible="false">
                                                          <TemplateSettings TemplateId="GvTempEdit" />
                                                    </obout:Column>--%>
                                                    <%--<obout:Column DataField="ID" HeaderText="Edit" Width="5%" AllowSorting="false" Align="center" te
                                                        HeaderAlign="center">
                                                        <TemplateSettings TemplateId="GvEditBOM" />
                                                    </obout:Column>--%>
                                                    <obout:Column ID="Column1" HeaderText="From Date" DataField="FromDate" Width="15%"
                                                        runat="server" DataFormatString="{0:d}">
                                                    </obout:Column>
                                                    <obout:Column ID="Column2" HeaderText="To Date" DataField="todate" Width="15%" runat="server"
                                                        DataFormatString="{0:d}">
                                                    </obout:Column>
                                                    <obout:Column ID="Column3" HeaderText="Approve Rights to" DataField="Name" Width="15%"
                                                        runat="server">
                                                    </obout:Column>
                                                    <obout:Column ID="Column4" HeaderText="Remark" DataField="remark" Width="20%" runat="server">
                                                    </obout:Column>
                                                     <obout:Column ID="Column5" HeaderText="Department" DataField="Territory" Width="20%" runat="server">
                                                    </obout:Column>
                                                </Columns>
                                                <Templates>
                                                    <%--<obout:GridTemplate ID="GvEditBOM" runat="server" ControlID="" ControlPropertyName="">
                                                        <Template>
                                                            <asp:ImageButton ID="imgBtnEdit1bom" runat="server" ImageUrl="../App_Themes/Blue/img/Edit16.png"
                                                               OnClick="imgBtnEditbom_OnClick" ToolTip='<%# (Container.Value) %>' CausesValidation="true"/>
                                                        </Template>
                                                    </obout:GridTemplate>--%>
                                                     <%--<obout:GridTemplate ID="GvTempEdit" runat="server">
                                                <Template>
                                                    <asp:ImageButton ID="imgBtnEdit" CausesValidation="false" runat="server" ImageUrl="../App_Themes/Blue/img/Edit16.png" />
                                                </Template>
                                            </obout:GridTemplate>--%>
                                                     <obout:GridTemplate ID="GvRemoveSku" runat="server">
                                                              <Template>
                                                                <img id="imgbuttonremove" src="../App_Themes/Grid/img/Remove16x16.png" alt="Remove" title="Remove" onclick="RemoveSkuRecord('<%# (Container.DataItem["ID"].ToString()) %>');"     <%--RemoveSkuRecord--%>
                                                                     style="cursor: pointer;" />
                                                              </Template>
                                                   </obout:GridTemplate>
                                                </Templates>
                                            </obout:Grid>
                                        </td>
                                    </tr>
                                </table>
                            </center>
     <asp:HiddenField runat="server" ID="hdndeligateeditstate" ClientIDMode="Static" />
    <asp:HiddenField runat="server" ID="hdndegateId" ClientIDMode="Static" />
    <asp:HiddenField ID="hndstate" runat="server" />
    <asp:HiddenField ID="hnduserID" runat="server" />
    <asp:HiddenField ID="hdnnewDelegateid" runat="server" />
     <asp:HiddenField ID="hdnSelectedDepartment" runat="server" />
     <asp:HiddenField ID="hdnSelectedLocation" runat="server" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
      <script type="text/javascript">
       function SaveAccessDelegation() {
        var FromDate = getDateFromUC("<%= UC_Date1.ClientID %>");
                var ToDate = getDateFromUC("<%=UC_Date2.ClientID %>");
                var hdndeligateeditstate = document.getElementById("<%=hdndeligateeditstate.ClientID %>");
           var hdndegateId = document.getElementById("<%=hdndegateId.ClientID %>");
           var department = document.getElementById("<%=ddlDepartment.ClientID %>")
                var Delegateto = document.getElementById("<%=ddlUOM.ClientID %>");
                var Remark = document.getElementById("<%=txtPrincipalPrice.ClientID %>");
                var UserId = document.getElementById("<%=hnduserID.ClientID %>");
                var hndstate = document.getElementById("<%=hndstate.ClientID %>");
                var newdelegateid = document.getElementById("<%=hdnnewDelegateid.ClientID %>");
                var obj1 = new Object();
                obj1.hdndeligateeditstate = hdndeligateeditstate.value;
                obj1.hdndegateId = hdndegateId.value;
                obj1.department = department.value.toString();
                obj1.Delegateto = Delegateto.value.toString();
                obj1.Remark = Remark.value.toString();
                obj1.UserId = UserId.value.toString();
                obj1.hndstate = hndstate.value.toString();
                obj1.FromDate = FromDate;
                obj1.ToDate = ToDate;
                obj1.newDelegate = newdelegateid.value;
                if (FromDate != "" && ToDate != "" && department.value.toString() != "0" && Delegateto.value.toString() !="0") {
                    PageMethods.SaveAccessDelegation(obj1, getSave_onSuccessed)
                }
                else {
                    alert("Please Select all the required field");
                }
            }

          function getSave_onSuccessed(result) {
              if (result != 'Exist') {
                  grdaccessdele.refresh();
                  document.getElementById("<%=ddlDepartment.ClientID %>").value = 0;
                  document.getElementById("<%=ddlUOM.ClientID %>").value = 0;
                  document.getElementById("<%=txtPrincipalPrice.ClientID %>").value = "";
                  getDateFromUC("<%= UC_Date1.ClientID %>").value = "";
                  getDateFromUC("<%= UC_Date1.ClientID %>") = "";
              }
              else {
                  alert("Same Delegation for same date already present");
                  document.getElementById("<%=ddlDepartment.ClientID %>").value = 0;
                  document.getElementById("<%=ddlUOM.ClientID %>").value = 0;
                  document.getElementById("<%=txtPrincipalPrice.ClientID %>").value = "";
                  getDateFromUC("<%= UC_Date1.ClientID %>").value = "";
                  getDateFromUC("<%= UC_Date1.ClientID %>") = "";
              }
            }

          function RemoveSkuRecord(Id) {
              var obj1 = new Object();
              var Detailid = Id;
              obj1.Delegateid = Detailid;
              PageMethods.RemoveSku(obj1, Removesku_onSuccess);
          }

          function Removesku_onSuccess(result) {
              grdaccessdele.refresh();
          }


    </script>  
    <script type="text/javascript">
        function GetDepartment() {
            var ddlDepartment = document.getElementById("<%=ddlDepartment.ClientID %>").value;
            document.getElementById("<%=hdnSelectedDepartment.ClientID %>").value = ddlDepartment;

            var hdnSelectedDepartment = document.getElementById("<%=hdnSelectedDepartment.ClientID %>");
            hdnSelectedDepartment.value = ddlDepartment;
            var obj1 = new Object();
            obj1.ddlDepartment = ddlDepartment;
           // PageMethods.GetRollById(obj1, getRoll_onSuccessed);
            PageMethods.Getdelegate(obj1, getLoc_onSuccessed);
        }

        function getLoc_onSuccessed(result) {
            var ddlUOM = document.getElementById("<%=ddlUOM.ClientID %>");
            ddlUOM.options.length = 0;
            for (var i in result) {
                AddOption(result[i].Name, result[i].Id);
            }
        }

        function AddOption(text, value) {
            var ddlUOM = document.getElementById("<%=ddlUOM.ClientID %>");
            var option = document.createElement('option');
            option.value = value;
            option.innerHTML = text;
            ddlUOM.options.add(option);
        }
    </script>
              
</asp:Content>

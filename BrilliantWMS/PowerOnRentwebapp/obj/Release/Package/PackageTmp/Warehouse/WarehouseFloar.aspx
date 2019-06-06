<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/CRM2.Master" AutoEventWireup="true" CodeBehind="WarehouseFloar.aspx.cs"
   Inherits="BrilliantWMS.Warehouse.WarehouseFloar"  EnableEventValidation="false" Theme="Blue" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="obout_Interface" Namespace="Obout.Interface" TagPrefix="obout" %>
<%@ Register Assembly="obout_Grid_NET" Namespace="Obout.Grid" TagPrefix="obout" %>
<%@ Register Src="../CommonControls/UCFormHeader.ascx" TagName="UCFormHeader" TagPrefix="uc1" %>
<%@ Register Src="../CommonControls/UCToolbar.ascx" TagName="UCToolbar" TagPrefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
       <uc1:UCFormHeader ID="UCFormHeader1" runat="server" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
         <center>
        <asp:UpdateProgress ID="UpdateProgress_Contact" runat="server" AssociatedUpdatePanelID="updPnl_Contact">
            <ProgressTemplate>
                <center>
                    <div class="modal">
                        <img src="../App_Themes/Blue/img/ajax-loader.gif" style="top: 50%;" />
                    </div>
                </center>
            </ProgressTemplate>
        </asp:UpdateProgress>
        <asp:UpdatePanel ID="updPnl_Contact" runat="server">
            <ContentTemplate>
                 <asp:HiddenField ID="hdnbuilding" runat="server" />
                <asp:HiddenField ID="HiddenField1" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hdncustomerID" runat="server" ClientIDMode="Static"/>
                <asp:HiddenField ID="hdnCompanyID" runat="server" ClientIDMode="Static"/>
                 <asp:HiddenField ID="BuildingID" runat="server" ClientIDMode="Static"/>
                <asp:HiddenField ID="hdnfloarID" runat="server" ClientIDMode="Static"/>

                <asp:HiddenField ID="hdnConID" runat="server" />
                <asp:HiddenField ID="hdnstate" runat="server" ClientIDMode="Static" />
                <table class="tableForm">
                    <tr>
                        <td>
                            <req><asp:Label ID="lblFloarname" runat="server" Text="Floar Name:"></asp:Label></req>
                        </td>
                        <td style="text-align: left">
                            <asp:TextBox ID="txtfloarname" runat="server" Width="200px" onKeyPress="return alpha(event);"
                                MaxLength="50"></asp:TextBox>
                        </td>
                        <td>
                            <req><asp:Label ID="lblsortcode" runat="server" Text="Sort Code :"></asp:Label></req>
                        </td>
                        <td style="text-align: left">
                            <asp:TextBox ID="txtsortcode" runat="server" Width="200px" onkeypress="return fnAllowDigitsOnly(event)"></asp:TextBox>
                        </td>
                        <td>
                            <req><asp:Label ID="lblcapacity" runat="server" Text="Capacity:"></asp:Label></req>
                        </td>
                        <td style="text-align: left">
                            <asp:TextBox ID="txtcapacity" runat="server" Width="200px" style="text-align:right" onkeydown="return AllowPhoneNo(this, event);"
                                onkeypress="return AllowPhoneNo(this, event);"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <req><asp:Label ID="lblremark" runat="server" Text="Desciption:"></asp:Label></req>
                        </td>
                        <td style="text-align: left" colspan="4">
                            <asp:TextBox ID="txtdescription" runat="server" Width="620px"  ></asp:TextBox>
                        </td>
                        <td style="text-align: right">
                            <input type="button" id="btnSave" runat="server" title="Save" value="Save" onclick="SaveFloarDetail();" />
                            <%--<asp:Button ID="btnSaveAsTemplateNew" runat="server" Text="Save" Style="cursor: pointer;" OnClientClick="SaveContactDetail();" />--%>
                        </td>
                    </tr>
                    <tr>

                    </tr>
                </table>
                <asp:HiddenField ID="hdnContactID" runat="server" />
                <table class="gridFrame" width="100%">
                    <tr>
                        <td>
                            <table style="width: 100%">
                                <tr>
                                    <td style="text-align: left;">
                                        <a id="headerText"><asp:Label ID="lblContactPersonList" runat="server" Text="Floar List"></asp:Label></a>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td style="text-align: right;">
                            <input type="button" id="btnSubmit" runat="server" title="Save" value="Submit" onclick="GetFloar();" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <obout:Grid ID="grdWareFloar" runat="server" AutoGenerateColumns="False" AllowAddingRecords="False"
                                AllowFiltering="True" AllowGrouping="True" AllowMultiRecordSelection="False"
                                AllowRecordSelection="true" Width="100%" AllowColumnResizing="true" AllowColumnReordering="true" OnRebind="grdWareFloar_OnRebind">
                                <Columns>
                                    <obout:Column HeaderText="Edit" DataField="ID" Width="5%" Align="center" HeaderAlign="center">
                                        <TemplateSettings TemplateId="TemplateEdit" />
                                    </obout:Column>
                                  <%--   <obout:Column DataField="ID" HeaderText="ID" Visible="false">
                                    </obout:Column>--%>
                                    <obout:Column DataField="Name" HeaderText="Floar Name" Width="8%">
                                    </obout:Column>
                                    <obout:Column DataField="SortCode" HeaderText="Sort Code" Width="8%">
                                    </obout:Column>
                                    <obout:Column DataField="Capacity" HeaderText="Capacity" Width="5%">
                                    </obout:Column>
                                     <obout:Column DataField="Description" HeaderText="Description" Width="12%">
                                    </obout:Column>
                                </Columns>
                                <Templates>
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
            </ContentTemplate>
        </asp:UpdatePanel>
    </center>
    <asp:HiddenField ID="hdnSelectedRec" runat="server" ClientIDMode="Static" />
   

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
            var hdnSelectedRec = document.getElementById("hdnSelectedRec");
            hdnSelectedRec.value = "";

            for (var i = 0; i < grdWareFloar.PageSelectedRecords.length; i++) {
                var record = grdWareFloar.PageSelectedRecords[i];
                if (hdnSelectedRec.value != "") hdnSelectedRec.value += ',' + record.ID;
                if (hdnSelectedRec.value == "") hdnSelectedRec.value = record.ID;
            }
        }

        function SaveFloarDetail() {
            var Floar = document.getElementById("<%= txtfloarname.ClientID %>");
            var SortCode = document.getElementById("<%= txtsortcode.ClientID %>");
            var Capacity = document.getElementById("<%= txtcapacity.ClientID %>");
            var CompanyId = document.getElementById("<%= hdnCompanyID.ClientID %>")
            var description = document.getElementById("<%= txtdescription.ClientID %>");
            var CompanyId = document.getElementById("<%= hdnCompanyID.ClientID %>")
            var CustomerID = document.getElementById("<%= hdncustomerID.ClientID %>")
            var BuildingID = document.getElementById("<%= hdnbuilding.ClientID %>")
            var hdnstate = document.getElementById('hdnstate');
            if (Floar.value == "") {
                showAlert("Enter Floar name", "Error");
                Floar.focus();
            } else if (SortCode.value == "") {
                showAlert("Enter Sort Code", "Error");
                SortCode.focus();
            } else if (Capacity.value == "") {
                showAlert("Enter Capacity", "Error");
                Capacity.focus();
            }
            else {
                var obj1 = new Object();
                obj1.Name = Floar.value.toString();
                obj1.SortCode = SortCode.value.toString();
                obj1.Capacity = Capacity.value.toString();
                obj1.description = description.value.toString();
                obj1.CompanyId = CompanyId.value.toString();
                obj1.CustomerID = CustomerID.value.toString();
                obj1.BuildingID = BuildingID.value.toString();
                PageMethods.WMSaveRequestHead(obj1, hdnstate.value, WMSaveContact_onSuccessed, WMSaveContact_onFailed);
            }
            }

            function WMSaveContact_onSuccessed(result) {
                if (result == "Some error occurred" || result == "") { showAlert("Error occurred", "Error", '#'); }
                else if (result == "Floar saved successfully") {
                    showAlert(result, "info");
                    document.getElementById("<%= txtfloarname.ClientID %>").value = "";
                    document.getElementById("<%= txtsortcode.ClientID %>").value = "";
                    document.getElementById("<%= txtcapacity.ClientID %>").value = "";
                    document.getElementById("<%= txtdescription.ClientID %>").value = "";    
                    grdWareFloar.refresh();
                    self.close();
                }
            }
            function WMSaveContact_onFailed() {
                showAlert("Error occurred", "Error", "#");
                self.close();
            }
       

            function fnAllowDigitsOnly(key) {
                var keycode = (key.which) ? key.which : key.keyCode;
                if ((keycode < 48 || keycode > 57) && (keycode != 8) && (keycode != 9) && (keycode != 11) && (keycode != 16) && (keycode != 37) && (keycode != 38) && (keycode != 39) && (keycode != 40) && (keycode != 219) && (keycode != 127)) {
                    return false;
                }
            }



            function GetFloar() {
                var hdnSelectedRec = document.getElementById('hdnSelectedRec');
                if (hdnSelectedRec.value == "") {
                    showAlert("Select Atleast One Floar...", "Error");
                }
                var SelCon = hdnSelectedRec.value;
                var count = (SelCon.match(/,/g) || []).length;
                console.log(count);
                if (count > 1) {
                    showAlert("Select Only One Floar", "Error", "#");
                } else {
                    var hdnFloarID = window.opener.document.getElementById("hdnFloarID");
                    var hdnFloarName = window.opener.document.getElementById("hdnFloarName");
                    hdnFloarID.value = "";
                    hdnFloarName.value = "";
                    if (grdWareFloar.SelectedRecords.length > 0) {
                        for (var i = 0; i < grdWareFloar.SelectedRecords.length; i++) {
                            var record = grdWareFloar.SelectedRecords[i];
                            if (hdnFloarID.value != "") hdnFloarID.value += ',' + record.ID;
                            if (hdnFloarID.value == "") {
                                hdnFloarID.value = record.ID;
                                hdnFloarName.value = record.Name;
                            }
                        }
                        window.opener.GetFloar(hdnFloarID.value, hdnFloarName.value);
                        self.close();
                    }
                }
            }
    </script>
</asp:Content>

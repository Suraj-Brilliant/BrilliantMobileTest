<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/CRM2.Master" AutoEventWireup="true" CodeBehind="WarehouseBuilding.aspx.cs"
   Inherits="BrilliantWMS.Warehouse.WarehouseBuilding" EnableEventValidation="false" Theme="Blue" %>

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
                <asp:HiddenField ID="hdnstate" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hdncustomerID" runat="server" ClientIDMode="Static"/>
                <asp:HiddenField ID="hdnCompanyID" runat="server" ClientIDMode="Static"/>
                 <asp:HiddenField ID="hdnwarehouseID" runat="server" ClientIDMode="Static"/>
                <table class="tableForm">
                    <tr>
                        <td>
                            <req><asp:Label ID="lblbuildName" runat="server" Text="Building Name:"></asp:Label></req>
                        </td>
                        <td style="text-align: left">
                            <asp:TextBox ID="txtbuildname" runat="server" Width="200px" onKeyPress="return alpha(event);"
                                MaxLength="50"></asp:TextBox>
                        </td>
                        <td>
                            <req><asp:Label ID="lblEmailID" runat="server" Text="Sort Code :"></asp:Label></req>
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
                            <asp:TextBox ID="txtdescription" runat="server" Width="620px" ></asp:TextBox>
                        </td>
                        <td style="text-align: right">
                            <input type="button" id="btnSave" runat="server" title="Save" value="Save" onclick="SaveContactDetail();" />
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
                                        <a id="headerText"><asp:Label ID="lblContactPersonList" runat="server" Text="Building List"></asp:Label></a>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td style="text-align: right;">
                            <input type="button" id="btnSubmit" runat="server" title="Save" value="Submit" onclick="GetContactPerson();" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <obout:Grid ID="gvBuilding" runat="server" AutoGenerateColumns="False" AllowAddingRecords="False"
                                AllowFiltering="True" AllowGrouping="True" AllowMultiRecordSelection="False"
                                AllowRecordSelection="true" Width="100%" AllowColumnResizing="true" AllowColumnReordering="true" OnRebind="gvBuilding_OnRebind">
                                <Columns>
                                    <obout:Column HeaderText="Edit" DataField="ID" Width="5%" Align="center" HeaderAlign="center">
                                        <TemplateSettings TemplateId="TemplateEdit" />
                                    </obout:Column>
                                    <%-- <obout:Column DataField="ID" HeaderText="ID" Visible="false">
                                    </obout:Column>--%>
                                    <obout:Column DataField="Name" HeaderText="Bulding Name" Width="8%">
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

            for (var i = 0; i < gvBuilding.PageSelectedRecords.length; i++) {
                var record = gvBuilding.PageSelectedRecords[i];
                if (hdnSelectedRec.value != "") hdnSelectedRec.value += ',' + record.ID;
                if (hdnSelectedRec.value == "") hdnSelectedRec.value = record.ID;
            }
        }

        function SaveContactDetail() {
            var txtName = document.getElementById("<%= txtbuildname.ClientID %>");
            var txtsortcode = document.getElementById("<%= txtsortcode.ClientID %>");
            var txtcapacity = document.getElementById("<%= txtcapacity.ClientID %>");
            var description = document.getElementById("<%= txtdescription.ClientID %>");
            var CompanyId = document.getElementById("<%= hdnCompanyID.ClientID %>")
            var CustomerID = document.getElementById("<%= hdncustomerID.ClientID %>")
            var WarehouseID = document.getElementById("<%= hdnwarehouseID.ClientID %>")
            var hdnstate = document.getElementById('hdnstate');
            if (txtName.value == "") {
                showAlert("Enter Building Name", "Error");
                txtName.focus();
            } else if (txtsortcode.value == "") {
                showAlert("Enter Sortcode", "Error");
                txtEmail.focus();
            } else if (txtcapacity.value =="") {
                showAlert("Enter Capacity", "Error");
                } else {
                    var obj1 = new Object();
                    obj1.Name = txtName.value.toString();
                    obj1.SortCode = txtsortcode.value.toString();
                    obj1.Capacity = txtcapacity.value.toString();
                    obj1.description = description.value.toString();
                    obj1.CompanyId = CompanyId.value.toString();
                    obj1.CustomerID = CustomerID.value.toString();
                    obj1.WarehouseID = WarehouseID.value.toString();
                    PageMethods.WMSaveRequestHead(obj1, hdnstate.value, WMSaveContact_onSuccessed, WMSaveContact_onFailed);
                }
            }

            function WMSaveContact_onSuccessed(result) {
                if (result == "Some error occurred" || result == "") { showAlert("Error occurred", "Error", '#'); }
                else if (result == "Building saved successfully") {
                    showAlert(result, "info");
                    gvBuilding.refresh();
                    txtName.value = "";
                    txtsortcode.value = "";
                    txtcapacity.value = "";
                    description.value = "";
                    document.getElementById("<%= txtbuildname.ClientID %>").value = "";
                    document.getElementById("<%= txtsortcode.ClientID %>").value = "";
                    document.getElementById("<%= txtcapacity.ClientID %>").value = "";
                    document.getElementById("<%= txtdescription.ClientID %>").value = "";                
                }
            }
            function WMSaveContact_onFailed() {
                showAlert("Error occurred", "Error", "#");
                document.getElementById("<%= txtbuildname.ClientID %>").value = "";
                document.getElementById("<%= txtsortcode.ClientID %>").value = "";
                document.getElementById("<%= txtcapacity.ClientID %>").value = "";
                document.getElementById("<%= txtdescription.ClientID %>").value = "";  
                //self.close();
            }

        function IsValidEmail(email) {
            var expr = /^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/;
            return expr.test(email)
        }

            function fnAllowDigitsOnly(key) {
                var keycode = (key.which) ? key.which : key.keyCode;
                if ((keycode < 48 || keycode > 57) && (keycode != 8) && (keycode != 9) && (keycode != 11) && (keycode != 16) && (keycode != 37) && (keycode != 38) && (keycode != 39) && (keycode != 40) && (keycode != 219) && (keycode != 127)) {
                    return false;
                }
            }






        function GetContactPerson() {
            var hdnSelectedRec = document.getElementById('hdnSelectedRec');
            if (hdnSelectedRec.value == "") {
                showAlert("Select Atleast One Building...", "Error");
            }
            var SelCon = hdnSelectedRec.value;
            var count = (SelCon.match(/,/g) || []).length;
            console.log(count);
            if (count > 1) {
                showAlert("Select Only One Building", "Error", "#");
            } else {
                var hdnBuildingID = window.opener.document.getElementById("hdnBuildingID");
                var hdnBuildingName = window.opener.document.getElementById("hdnBuildingName");
                hdnBuildingID.value = "";
                hdnBuildingName.value = "";
                if (gvBuilding.SelectedRecords.length > 0) {
                    for (var i = 0; i < gvBuilding.SelectedRecords.length; i++) {
                        var record = gvBuilding.SelectedRecords[i];
                        if (hdnBuildingID.value != "") hdnBuildingID.value += ',' + record.ID;
                        if (hdnBuildingID.value == "") {
                            hdnBuildingID.value = record.ID;
                            hdnBuildingName.value = record.Name;
                       }
                    }
                    window.opener.GetBuilding(hdnBuildingID.value, hdnBuildingName.value);
                    self.close();
                }
            }
        }
    </script>
</asp:Content>

<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/CRM2.Master" AutoEventWireup="true" CodeBehind="ChangeOrderProduct.aspx.cs"
    Inherits="BrilliantWMS.PowerOnRent.ChangeOrderProduct" EnableEventValidation="false" Theme="Blue" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="obout_Interface" Namespace="Obout.Interface" TagPrefix="obout" %>
<%@ Register Assembly="obout_Grid_NET" Namespace="Obout.Grid" TagPrefix="obout" %>
<%@ Register Src="../CommonControls/UCFormHeader.ascx" TagName="UCFormHeader" TagPrefix="uc1" %>
<%@ Register Src="../CommonControls/UCToolbar.ascx" TagName="UCToolbar" TagPrefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <uc1:UCFormHeader ID="UCFormHeader1" runat="server" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="../MasterPage/JavaScripts/jquery-3.1.1.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        function AllDec(sender, event) {
            var keyPressed = getKeyPressed_e(event);
            if ((parseInt(keyPressed) == 46 || parseInt(keyPressed) == 8) ||
                (parseInt(keyPressed) >= 48 && parseInt(keyPressed) <= 57) ||
                (parseInt(keyPressed) >= 96 && parseInt(keyPressed) <= 105) ||
                (parseInt(keyPressed) >= 35 && parseInt(keyPressed) <= 40) ||
                parseInt(keyPressed) == 110 ||  /*decimal point*/
                parseInt(keyPressed) == 190 ||  /*period*/
                parseInt(keyPressed) == 9 || /*tab*/
                (event.ctrlKey == true && (event.which == '118' || event.which == '86'))
                ) {
                if (keyPressed == 110 || keyPressed == 190) {
                    var patt1 = new RegExp("\\."); var ch = patt1.exec(sender.value);
                    if (ch == ".") { event.preventDefault ? event.preventDefault() : event.returnValue = false; }
                    else { return true; }
                }
                else if (event.ctrlKey == true && (event.which == '118' || event.which == '86')) {
                    sender.value = parseFloat(sender.value);
                    return true;
                }
                else { return true; }
            }
            else { event.preventDefault ? event.preventDefault() : event.returnValue = false; }
        }
    </script>
    <center>
        <asp:UpdateProgress ID="UpdateProgress_Contact" runat="server" AssociatedUpdatePanelID="updPnl_Prod">
            <ProgressTemplate>
                <center>
                    <div class="modal">
                        <img src="../App_Themes/Blue/img/ajax-loader.gif" style="top: 50%;" />
                    </div>
                </center>
            </ProgressTemplate>
        </asp:UpdateProgress>
        <asp:UpdatePanel ID="updPnl_Prod" runat="server">
            <ContentTemplate>
                <table class="tableForm">
                    <tr>
                        <td>
                            <asp:Label ID="lblProductCode" runat="server" Text="Product Code"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtProductCode" runat="server" Enabled="false" Width="180px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Label ID="lblProductName" runat="server" Text="Product Name"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtProductName" runat="server" Enabled="false" Width="180px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Label ID="lblProdDescription" runat="server" Text="Product Description"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtProductDescription" runat="server" Enabled="false" Width="180px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblMOQ" runat="server" Text="MOQ"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtMOQ" runat="server" Enabled="false" Width="180px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Label ID="lblCurrentStock" runat="server" Text="Current Stock"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtCurrentStock" runat="server" Enabled="false" Width="180px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Label ID="lblReserveQty" runat="server" Text="Reserve Qty"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtReserveStock" runat="server" Enabled="false" Width="180px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblRequestQty" runat="server" Text="Request Qty"></asp:Label>
                        </td>
                        <td>
                            <input type="text" id="txtReqQty" runat="server" onblur="GetIndexQty()"  />
                            <%-- <asp:TextBox ID="txtRequestQty" runat="server" Enabled="false" Width="180px" onblur="return GetIndexQty()"></asp:TextBox>--%>
                        </td>
                        <td>
                            <asp:Label ID="lblUOM" runat="server" Text="UOM"></asp:Label>
                        </td>
                        <td style="text-align: left;">
                            <asp:DropDownList ID="ddlUOM" runat="server" DataTextField="Description" DataValueField="UOMID" Enabled="false" onchange="GetSelValue()"></asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lblOrderQty" runat="server" Text="Order Qty"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtOrderQty" runat="server" Enabled="false" Width="180px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblPrice" runat="server" Text="Price"></asp:Label>
                        </td>
                        <td>
                            <input type="text" id="txtPrc" runat="server" onblur="GetSelValue()"  />
                            <%--<asp:TextBox ID="txtPrice" runat="server" Enabled="false" Width="180px"></asp:TextBox>--%>
                        </td>
                        <td>
                            <asp:Label ID="lblTotal" runat="server" Text="Total"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtTotal" runat="server" Enabled="false" Width="180px"></asp:TextBox>
                        </td>
                        <td></td>
                        <td style="text-align: right;">
                            <input type="button" id="btnSubmit" runat="server" title="Save" value="Submit" onclick="GetUpdatedDetails();" />
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </center>
    <asp:HiddenField ID="hdnSelectedProduct" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnUomQty" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnSelddl" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnEnteredQty" runat="server" ClientIDMode="Static" />
    <script type="text/javascript">
        function GetIndexQty() {
            // var hdnSelddl = document.getElementById('hdnSelddl');
            var hdnEnteredQty = document.getElementById('hdnEnteredQty');
            var selID = document.getElementById("<%= ddlUOM.ClientID %>");
            var SelUom = selID.value; // selID.options[selID.selectedIndex].value; hdnSelddl.value = selID.options[selID.selectedIndex].value;
            var txtReqQty = document.getElementById("<%= txtReqQty.ClientID %>");
            var EnteredValue = txtReqQty.value; hdnEnteredQty.value = txtReqQty.value;
            var UOMQTY = GetQtyofSelectedUOM(SelUom);

            var numTotalQty = Number(UOMQTY) * Number(hdnEnteredQty.value);  //var numTotalQty = Number(UOMQTY) * Number(EnteredValue);
            var txtOrderQty = document.getElementById("<%= txtOrderQty.ClientID %>");
            txtOrderQty.value = numTotalQty;
            var txtPrice = document.getElementById("<%= txtPrc.ClientID %>");
            var Price = txtPrice.value;
            var txtTotal = document.getElementById("<%= txtTotal.ClientID %>");
            txtTotal.value = Number(Price) * Number(numTotalQty);
            var txtCurrentStock = document.getElementById("<%= txtCurrentStock.ClientID %>");
            var CurrentStock = txtCurrentStock.value;
            if (CurrentStock < numTotalQty) {
                txtReqQty.value = "0";
                txtOrderQty.value = "0";
                showAlert("Requested Quantity is greater than Current Stock...!!!", "Error", '#');
            } else {
                var moq = document.getElementById("<%= txtMOQ.ClientID %>");
                var moqv = moq.value;
                if (moqv > 0) {
                    var rem = numTotalQty % moqv;
                    if (rem > 0) {
                        txtReqQty.value = "0";
                        txtOrderQty.value = "0";
                        showAlert("Requested Quantity is not in range of MOQ ...!!!", "Error", '#');
                    }
                }
            }
        }
        function GetSelValue() {
            var selectedValue = document.getElementById("<%= ddlUOM.ClientID %>");
            var selUom = selectedValue.value;
            var txtReqQty = document.getElementById("<%= txtReqQty.ClientID %>");
            var EnteredValue = txtReqQty.value;
            var UOMQTY = GetQtyofSelectedUOM(selUom);

            var numTotalQty = Number(UOMQTY) * Number(EnteredValue);
            var txtOrderQty = document.getElementById("<%= txtOrderQty.ClientID %>");
            txtOrderQty.value = numTotalQty;
            var txtPrice = document.getElementById("<%= txtPrc.ClientID %>");
            var Price = txtPrice.value;
            var txtTotal = document.getElementById("<%= txtTotal.ClientID %>");
            txtTotal.value = Number(Price) * Number(numTotalQty);
            var txtCurrentStock = document.getElementById("<%= txtCurrentStock.ClientID %>");
            var CurrentStock = txtCurrentStock.value;
            if (CurrentStock < numTotalQty) {
                txtReqQty.value = "0";
                txtOrderQty.value = "0";
                showAlert("Requested Quantity is greater than Current Stock...!!!", "Error", '#');
            } else {
                var moq = document.getElementById("<%= txtMOQ.ClientID %>");
                var moqv = moq.value;
                if (moqv > 0) {
                    var rem = numTotalQty % moqv;
                    if (rem > 0) {
                        txtReqQty.value = "0";
                        txtOrderQty.value = "0";
                        showAlert("Requested Quantity is not in range of MOQ ...!!!", "Error", '#');
                    }
                }
            }
        }
        function GetQtyofSelectedUOM(SelUom) {
            var hdnSelectedProduct = document.getElementById('hdnSelectedProduct');
            var hdnUomQty = document.getElementById('hdnUomQty');
            var SelectedUOM = SelUom;
            PageMethods.WMGetQty(hdnSelectedProduct.value, SelectedUOM, OnsuccessGetQty, null);

            return hdnUomQty.value;
        }
        function OnsuccessGetQty(result) {
            var hdnUomQty = document.getElementById('hdnUomQty');
            hdnUomQty.value = result;
        }
        function GetUpdatedDetails() {
            var txtReqQty = document.getElementById("<%= txtReqQty.ClientID %>");
            var txtPrice = document.getElementById("<%= txtPrc.ClientID %>");
            var txtOrderQty = document.getElementById("<%= txtOrderQty.ClientID %>"); 
            var RQty = txtReqQty.value; var TTL = txtPrice.value; var OrderQty = txtOrderQty.value;
            if (RQty<=0){//if (txtReqQty.value == "0" || txtReqQty.value == "0.00" || txtReqQty.value == "00") {            
                showAlert("Requested Quantity should not be Zero...!!!", "Error", '#');
            } 
            else if (OrderQty <= 0) {
                showAlert("Order Quantity should not be Zero...!!!", "Error", '#');
            }else {
                var txtOrderQty = document.getElementById("<%= txtOrderQty.ClientID %>"); 
                var txtPrice = document.getElementById("<%= txtPrc.ClientID %>"); var Price = txtPrice.value;
                var txtTotal = document.getElementById("<%= txtTotal.ClientID %>"); var Total = txtTotal.value;
                PageMethods.WMUpdateOrderProductDetails(OrderQty, Price, Total, OnSuccessUpdate, null);
            }
    }
    function OnSuccessUpdate(result) {
        if (result = 1) {
            var hdnChangePrdQtyPrice = window.opener.document.getElementById("hdnChangePrdQtyPrice");
            hdnChangePrdQtyPrice.value = "1";
            window.opener.AfterProductQtyChange(hdnChangePrdQtyPrice.value);
            self.close();
        }
    }

    
    </script>
</asp:Content>

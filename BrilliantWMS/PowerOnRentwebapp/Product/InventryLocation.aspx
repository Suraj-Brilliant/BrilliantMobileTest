<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InventryLocation.aspx.cs" Inherits="BrilliantWMS.Product.InventryLocation" %>

<script src="../MasterPage/JavaScripts/countries.js" type="text/javascript"></script>
<script src="../MasterPage/JavaScripts/crm.js" type="text/javascript"></script>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:HiddenField ID="hdncustomerId" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hdnskuid" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hdnlocationSearchName" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hdnLocationSearchID" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hdnprodlocID" runat="server" ClientIDMode="Static" />
            <asp:ScriptManager ID="ScriptmanagerAddress" runat="server" EnablePageMethods="true">
            </asp:ScriptManager>
            <center>
            <div id="divLoading" style="height: 280px; width: 1000px; display: none" class="modal">
                <center>
                    <br />
                    <span id="spanLoading" style="font-size: 17px; font-weight: bold; color: Yellow;">Processing
                        please wait...</span>
                </center>
            </div>
            <asp:ValidationSummary ID="validationsummary_UcAddressInfo" runat="server" ShowMessageBox="true"
                ShowSummary="false" DisplayMode="bulletlist" ValidationGroup="AddressSubmit" />
            <table class="gridFrame" style="margin: 2px 2px 2px 2px; width: 990px">
                <tr>
                    <td style="text-align: left;">
                        <asp:Label class="headerText" ID="lblAddressFormHeader" Text="Add Inventory Location"
                            runat="server"></asp:Label>
                    </td>
                    <td style="width: 20%">
                        <table style="float: right;">
                            <tr>
                                <td>
                                    <input type="button" id="btnAddressSubmit" value="Submit" onclick="SubmitLocation();"
                                        style="width: 70px;" />
                                </td>
                                <td>
                                    <input type="button" id="btnAddressClear" value="Clear" onclick="ClearLocation()"
                                        style="width: 70px;" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td style="background-color: White;" colspan="2">
                        <table class="tableForm" style="border: none; width: 990px">
                            <tr>
                                <td>
                                    <req>Warehouse :</req>
                                </td>
                                <td>
                                    <asp:DropDownList ID="DropDownList1" runat="server" Width="200px">
                                        <asp:ListItem Text="--Select--" Value="0" Selected="True"></asp:ListItem>
                                        <asp:ListItem Text="Supreme Warehouse" Value="10013"></asp:ListItem>                                       
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    Product Object :
                                </td>
                                <td>
                                     <asp:DropDownList ID="DropDownList2" runat="server" Width="200px">
                                        <asp:ListItem Text="--Select--" Value="0" ></asp:ListItem>
                                        <asp:ListItem Text="Location" Value="Location" Selected="True"></asp:ListItem>     
                                         <asp:ListItem Text="Shelf" Value="Shelf" ></asp:ListItem>     
                                         <asp:ListItem Text="Section" Value="Section" ></asp:ListItem>     
                                          </asp:DropDownList>
                                </td>
                            </tr>
                              <tr>
                                 <td>
                                    <req>Location :</req>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtlocation" runat="server" Enabled="false" Width="175" MaxLength="100" ></asp:TextBox>
                                    <img id="img1" src="../App_Themes/Grid/img/search.jpg" title="Search Location" runat="server" style="cursor: pointer;" onclick="openLocationSearch('1')" />
                                 
                                </td>
                                  <td>
                                    <req> Type :</req>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlType" runat="server" Width="200px">
                                        <asp:ListItem Text="--Select--" Value="0" Selected="True"></asp:ListItem>
                                        <asp:ListItem Text="Primary" Value="Primary"></asp:ListItem>
                                        <asp:ListItem Text="Secondary" Value="Secondary"></asp:ListItem>
                                        <asp:ListItem Text="Sub-Secondary" Value="Sub-Secondary"></asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ForeColor="Maroon" runat="server"
                                        ControlToValidate="ddlType" ErrorMessage="Select Type" ValidationGroup="AddressSubmit"
                                        Display="None"></asp:RequiredFieldValidator>
                                </td>
                                <td>
                                    Opening Balance :
                                </td>
                                <td>
                                    <asp:TextBox ID="txtOPeningBalance" runat="server" Style="text-align: right" Width="200px" onkeydown="AllowDecimal(this,event);"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                
                                <td>
                                    Minimum Reorder Qty. :
                                </td>
                                <td>
                                    <asp:TextBox ID="txtminReordrQty" runat="server" Width="200px"  Style="text-align: right" onkeydown="AllowDecimal(this,event);"
                                        onkeypress="AllowInt(this,event);"></asp:TextBox>
                                </td>
                                <td>
                                    Maximum Qty. :
                                </td>
                                <td>
                                    <asp:TextBox ID="txtMaxQty" runat="server" Width="200px"  Style="text-align: right" onkeydown="AllowDecimal(this,event);"
                                        onkeypress="AllowInt(this,event);"></asp:TextBox>
                                </td>
                            </tr>
                           
                        </table>
                    </td>
                </tr>
            </table>
        </center>
        </div>
        <script type="text/ecmascript">
            function openLocationSearch(sequence) {
                var CustomerID = document.getElementById("<%= hdncustomerId.ClientID %>").value;
            var WarehouseID = 0;
            window.open('../Product/WLocationSearch.aspx?CustomerID=' + CustomerID + '&WarehouseID=' + WarehouseID + '', 'height=500px, width=1080px,status= 0, resizable= 1, scrollbars=1, toolbar=0,location=center,menubar=0, screenX=300; screenY=50', target = "_blank");
        }

        function AfterProductSelected(Code, skuid) {
            var hdnval = document.getElementById("<%=hdnlocationSearchName.ClientID %>");
            var searchdkuid = document.getElementById("<%=hdnLocationSearchID.ClientID %>");
            hdnval.value = Code;
            searchdkuid.value = skuid;
            var TxtProduct = document.getElementById("<%=txtlocation.ClientID %>");
            TxtProduct.value = Code;
        }




        function SubmitLocation() {
            if (typeof (Page_ClientValidate) == 'function') {
                Page_ClientValidate();
            }
            if (Page_IsValid) {
                var ddltype = document.getElementById("<%=ddlType.ClientID%>");

                if (document.getElementById("<%=txtlocation.ClientID%>").value == "" || document.getElementById("<%=txtMaxQty.ClientID%>").value == "" || document.getElementById("<%=txtOPeningBalance.ClientID%>").value == "" || document.getElementById("<%=txtminReordrQty.ClientID%>").value == "" || ddltype.options[ddltype.selectedIndex].text == "--Select--") {

                    if (ddltype.options[ddltype.selectedIndex].text == "--Select--") {
                        showAlert("Please select Type!", "Error", "#");
                        ddltype.focus();

                    }
                    else if (document.getElementById("<%=txtlocation.ClientID%>").value == "") {
                       showAlert("Please Select location!", "Error", "#");
                       document.getElementById("<%=txtOPeningBalance.ClientID%>").focus();

                   }
                   else if (document.getElementById("<%=txtOPeningBalance.ClientID%>").value == "") {
                       showAlert("Please enter Opening Balance!", "Error", "#");
                       document.getElementById("<%=txtOPeningBalance.ClientID%>").focus();

                    }

                    else if (document.getElementById("<%=txtminReordrQty.ClientID%>").value == "") {
                        showAlert("Please enter Reorder quantity!", "Error", "#");
                        document.getElementById("<%=txtminReordrQty.ClientID%>").focus();

                    }
                    else if (document.getElementById("<%=txtMaxQty.ClientID%>").value == "") {
                        showAlert("Please enter Maximum quantity!", "Error", "#");
                        document.getElementById("<%=txtMaxQty.ClientID%>").focus();

                    }
}
else {
    var LocationInfo = new Object();
    LocationInfo.ProdLocID = document.getElementById("<%=hdnprodlocID.ClientID%>").value;
              LocationInfo.SKUID = document.getElementById("<%=hdnskuid.ClientID%>").value;
                         LocationInfo.LocationID = document.getElementById("<%=hdnLocationSearchID.ClientID%>").value;
                         LocationInfo.LocationType = document.getElementById("ddlType").value;
                         LocationInfo.OpeningBalance = document.getElementById("<%=txtOPeningBalance.ClientID%>").value;
              LocationInfo.MinQty = document.getElementById("<%=txtminReordrQty.ClientID%>").value;
                         LocationInfo.MaxQty = document.getElementById("<%=txtMaxQty.ClientID%>").value;
                         PageMethods.PMSaveLocation(LocationInfo, SubmitLocation_onSuccess, SubmitLocation_onFail);
                     }
                 }
             }


             function SubmitLocation_onSuccess() {
                 showAlert("Inventory Information Saved Successfully!", "info", "#");
                 window.opener.GVInventory.refresh();
                 self.close();
             }
             function SubmitLocation_onFail() { showAlert("Error On Inventory Save!", "error", "#"); }

             function ClearLocation() {
                 document.getElementById("ddlLocation").selectedIndex = 0;
                 document.getElementById("ddlType").selectedIndex = 0;
                 document.getElementById("txtOPeningBalance").value = "";
                 document.getElementById("txtminReordrQty").value = "";
                 document.getElementById("txtMaxQty").value = "";
             }
        </script>
    </form>
</body>
</html>

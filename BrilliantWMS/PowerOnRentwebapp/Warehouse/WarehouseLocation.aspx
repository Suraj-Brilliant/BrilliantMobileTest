<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WarehouseLocation.aspx.cs"
    Inherits="PowerOnRentwebapp.Location.WarehouseLocation" %>

<%@ Register Assembly="obout_Interface" Namespace="Obout.Interface" TagPrefix="obout" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>nGroup</title>
    <script src="../MasterPage/JavaScripts/crm.js" type="text/javascript"></script>
</head>
<body>
    <form id="WarehouseLocation_form" runat="server">
        <div>
            <asp:ScriptManager ID="ScriptmanagerAddress" runat="server" EnablePageMethods="true">
            </asp:ScriptManager>
            <asp:HiddenField ID="hdnCompanyID" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hdncustomerID" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hdnWarehouseID" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hdnWarehouseName" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hdnBuildingID" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hdnBuildingName" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hdnFloarID" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hdnFloarName" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hdnpassageID" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hdnpassageName" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hdnsectionID" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hdnsectionName" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hdnShelfID" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hdnShelfName" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hdnlocType" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hdnlocationID" runat="server" ClientIDMode="Static" />
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
                <table class="gridFrame" style="margin: 2px 2px 2px 2px; width: 1000px">
                    <tr>
                        <td style="text-align: left;">
                            <asp:Label class="headerText" ID="lblAddressFormHeader" runat="server" Text="Location Information"></asp:Label>
                        </td>
                        <td style="width: 20%">
                            <table style="float: right;">
                                <tr>
                                    <td>
                                        <input type="button" id="btnAddressSubmit" value="Submit" onclick="SubmitAddress();"
                                            style="width: 70px;" />
                                    </td>
                                    <td>
                                        <input type="button" id="btnAddressClear" value="Clear" onclick="ClearAddress()"
                                            style="width: 70px;" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="background-color: White;" colspan="2">
                            <table class="tableForm" style="border: none; width: 1000px; height: 210px">
                                <tr>
                                    <td style="text-align: left;">
                                        <req>Warehouse :</req>
                                    </td>
                                    <td style="text-align: left;">
                                        <asp:TextBox ID="txtwarehouse" runat="server" Width="150px" Enabled="false" MaxLength="100" onchange="CheckDuplicateAddress();"></asp:TextBox>
                                    </td>
                                    <td style="text-align: left;">
                                        <req>Building :</req>
                                    </td>
                                    <td style="text-align: left;">
                                        <asp:TextBox ID="txtbuilding" runat="server" Width="150" MaxLength="100" Enabled="false"></asp:TextBox>
                                        <img id="imgSearch" src="../App_Themes/Grid/img/search.jpg" title="Search SKU" runat="server" style="cursor: pointer;" onclick="openLocationSearch('0')" />
                                    </td>
                                    <td style="text-align: left;">
                                        <req>Floar :</req>
                                    </td>
                                    <td style="text-align: left;">
                                        <asp:TextBox ID="txtfloar" runat="server" Width="150" MaxLength="100" Enabled="false"></asp:TextBox>
                                        <img id="img1" src="../App_Themes/Grid/img/search.jpg" title="Search SKU" runat="server" style="cursor: pointer;" onclick="openLocationSearch('1')" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: left;">
                                        <req>Pathway :</req>
                                    </td>
                                    <td style="text-align: left;">
                                        <asp:TextBox ID="txtpathway" runat="server" Width="150" MaxLength="100" Enabled="false"></asp:TextBox>
                                        <img id="img2" src="../App_Themes/Grid/img/search.jpg" title="Search SKU" runat="server" style="cursor: pointer;" onclick="openLocationSearch('2')" />
                                    </td>
                                    <td style="text-align: left;">
                                        <req>Section :</req>
                                    </td>
                                    <td style="text-align: left;">
                                        <asp:TextBox ID="txtsection" runat="server" Width="150px" MaxLength="100" Enabled="false"></asp:TextBox>
                                        <img id="img3" src="../App_Themes/Grid/img/search.jpg" title="Search SKU" runat="server" style="cursor: pointer;" onclick="openLocationSearch('3')" />
                                    </td>
                                    <td style="text-align: left;">
                                        <req>Shelf :</req>
                                    </td>
                                    <td style="text-align: left;">
                                        <asp:TextBox ID="txtshelf" runat="server" Width="150" MaxLength="100" Enabled="false"></asp:TextBox>
                                        <img id="img4" src="../App_Themes/Grid/img/search.jpg" title="Search SKU" runat="server" style="cursor: pointer;" onclick="openLocationSearch('4')" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: left;">
                                        <req>Location Code :</req>
                                    </td>
                                    <td style="text-align: left;">
                                        <asp:TextBox ID="txtlocationCode" runat="server" Width="150" MaxLength="100"></asp:TextBox>
                                    </td>
                                    <td style="text-align: left;">Alias Code :
                                    </td>
                                    <td style="text-align: left;">
                                        <asp:TextBox ID="txtaliascode" runat="server" Width="150" MaxLength="100"></asp:TextBox>
                                    </td>
                                    <td style="text-align: left;">
                                        <req> Sort Code:</req>
                                    </td>
                                    <td style="text-align: left;">
                                        <asp:TextBox ID="txtsortCode" runat="server" Width="150px" MaxLength="50" onkeypress="return fnAllowDigitsOnly(event)"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: left;">
                                        <req> Location Type :</req>
                                    </td>
                                    <td style="text-align: left;">
                                        <asp:DropDownList ID="ddllocationtype" ClientIDMode="Static" DataTextField="Value" DataValueField="Id" runat="server" Width="206px" onchange="GetLocationType();">
                                        </asp:DropDownList>

                                    </td>
                                    <td style="text-align: left;">
                                        <req> Capacity In :</req>
                                    </td>
                                    <td style="text-align: left;">
                                        <asp:DropDownList ID="ddlcapacityin" ClientIDMode="Static" DataTextField="Value" DataValueField="Id" runat="server" Width="206px"></asp:DropDownList>

                                    </td>
                                    <td style="text-align: left;">
                                        <req> Capacity :</req>
                                    </td>
                                    <td style="text-align: left;">
                                        <asp:TextBox ID="txtcapacity" runat="server" Width="150px" MaxLength="50" Style="text-align: right" ClientIDMode="Static" onkeypress="return fnAllowDigitsOnly(event)"></asp:TextBox>

                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: left;">Location Type :</td>
                                    <td style="text-align: left;">
                                        <asp:DropDownList ID="ddlvelocityType" ClientIDMode="Static" runat="server" Width="206px">
                                            <asp:ListItem Text="-Select-" Value="0"></asp:ListItem>
                                            <asp:ListItem Text="Bin Area" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="Dock Area" Value="2"></asp:ListItem>
                                            <asp:ListItem Text="QC Area" Value="3"></asp:ListItem>
                                            <asp:ListItem Text="Assembly Area" Value="4"></asp:ListItem>
                                            <asp:ListItem Text="Loading Area" Value="5"></asp:ListItem>
                                            <asp:ListItem Text="GRN Area" Value="6"></asp:ListItem>
                                            <asp:ListItem Text="Dispatch Area" Value="7"></asp:ListItem>
                                            <asp:ListItem Text="Rejection Area" Value="8"></asp:ListItem>
                                            <asp:ListItem Text="Order Area" Value="9"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <req><asp:Label ID="lblchanactive" runat="server" Text="Active"/></req>
                                        :
                                    </td>
                                    <td style="text-align: left">
                                        <obout:OboutRadioButton ID="radiochannelY" runat="server" Text="Yes" GroupName="rbtActive" Checked="True" FolderStyle="">
                                        </obout:OboutRadioButton>
                                        &nbsp;&nbsp;
                                        <obout:OboutRadioButton ID="radiochannelN" runat="server" Text="No" GroupName="rbtActive" FolderStyle="">
                                        </obout:OboutRadioButton>
                                    </td>
                                    <td></td>
                                    <td></td>

                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </center>
        </div>
        <script type="text/javascript">

            // Check All the javascript code need to re write again//////

            function SubmitAddress() {
                if (document.getElementById("<%= txtfloar.ClientID %>").value = "") {
                showAlert("Please Select Floar!", "Error", "#");
                document.getElementById("<%=txtfloar.ClientID %>").focus();
            }
            else if (document.getElementById("<%=txtpathway.ClientID %>").value == "") {
                showAlert("Please Select Passage!", "Error", "#");
                document.getElementById("<%=txtpathway.ClientID %>").focus();
            }
            else if (document.getElementById("<%=txtsection.ClientID %>").value == "") {
                showAlert("Please Select Section", "Error", "#");
                document.getElementById("<%=txtsection.ClientID %>").focus();
            }
            else if (document.getElementById("<%=txtshelf.ClientID %>").value == "") {
                showAlert("Please Select Shelf", "Error", "#");
                document.getElementById("<%=txtshelf.ClientID %>").focus();
            }
            else if (document.getElementById("<%=txtlocationCode.ClientID %>").value == "") {
                showAlert("Please enter Location Code!", "Error", "#");
                document.getElementById("<%=txtlocationCode.ClientID %>").focus();
                    }
                    else if (document.getElementById("<%=txtsortCode.ClientID %>").value == "") {
              showAlert("Please enter Sort Code!", "Error", "#");
              document.getElementById("<%=txtsortCode.ClientID %>").focus();
                    }

                    else if (document.getElementById("<%=ddllocationtype.ClientID %>").value == "0") {
                showAlert("Please Select Location Type!", "Error", "#");
                document.getElementById("<%=ddllocationtype.ClientID %>").focus();
                    }
                    else if (document.getElementById("<%=ddlcapacityin.ClientID %>").value == "0") {
                showAlert("Please Select Capacity In!", "Error", "#");
                document.getElementById("<%=ddlcapacityin.ClientID %>").focus();
                    }
                    else if (document.getElementById("<%=txtcapacity.ClientID %>").value == "") {
                showAlert("Please enter Capacity!", "Error", "#");
                document.getElementById("<%=txtcapacity.ClientID %>").focus();
            }
            else {
                var WLocationInfo = new Object();
                WLocationInfo.WarehouseID = document.getElementById("<%=hdnWarehouseID.ClientID %>").value;
                WLocationInfo.ShelfID = document.getElementById("<%=hdnShelfID.ClientID %>").value;
                WLocationInfo.CompanyID = document.getElementById("<%=hdnCompanyID.ClientID %>").value;
                WLocationInfo.CustomerID = document.getElementById("<%=hdncustomerID.ClientID %>").value;
                WLocationInfo.LocationCode = document.getElementById("<%=txtlocationCode.ClientID %>").value;
                WLocationInfo.AliasCode = document.getElementById("<%=txtaliascode.ClientID %>").value;
                WLocationInfo.SortCode = document.getElementById("<%=txtsortCode.ClientID %>").value;
                WLocationInfo.LocationType = document.getElementById("<%=ddllocationtype.ClientID %>").value;
                WLocationInfo.CapacityIn = document.getElementById("<%=ddlcapacityin.ClientID %>").value;
                WLocationInfo.Capacity = document.getElementById("<%=txtcapacity.ClientID %>").value;
                WLocationInfo.VelocityType = document.getElementById("<%=ddlvelocityType.ClientID %>").value;
                var radiochannelY = document.getElementById("<%=radiochannelY.ClientID %>");
                if (radiochannelY.checked == true) {
                    WLocationInfo.Active = "Yes"
                }
                else {
                    WLocationInfo.Active = "No"
                }
                var LocationID = document.getElementById("<%=hdnlocationID.ClientID %>").value;
                PageMethods.PMSaveWLocation(WLocationInfo, LocationID, SubmitWLocation_onSuccess, SubmitWLocation_onFail);
            }
}

function SubmitWLocation_onSuccess(result) {
    if (result == "Some error occurred" || result == "") { showAlert("Error occurred", "Error", '#'); }
    else if (result == "Location saved successfully") {
        showAlert(result, "info");
        document.getElementById("<%= txtlocationCode.ClientID %>").value = "";
                document.getElementById("<%= txtaliascode.ClientID %>").value = "";
                document.getElementById("<%= txtcapacity.ClientID %>").value = "";
                document.getElementById("<%= txtsortCode.ClientID %>").value = "";
                document.getElementById("<%=ddllocationtype.ClientID %>").value = "0";
                document.getElementById("<%=ddlcapacityin.ClientID %>").value = "0";
                document.getElementById("<%=ddlvelocityType.ClientID %>").value = "0";
                document.getElementById("<%=radiochannelY.ClientID %>").checked = true;
                window.opener.GVLocation.refresh();
            }
    }

    function SubmitWLocation_onFail() {
        showAlert("Error occurred", "Error", "#");
    }

    function ClearAddress() {
        document.getElementById("<%=txtbuilding.ClientID %>").value = '';
            document.getElementById("<%=txtfloar.ClientID %>").value = '';
            document.getElementById("<%=txtpathway.ClientID %>").value = '';
            document.getElementById("<%=txtsection.ClientID %>").value = '';
            document.getElementById("<%= txtshelf.ClientID %>").value = "";
            document.getElementById("<%= txtlocationCode.ClientID %>").value = "";
            document.getElementById("<%= txtaliascode.ClientID %>").value = "";
            document.getElementById("<%= txtcapacity.ClientID %>").value = "";
            document.getElementById("<%= txtsortCode.ClientID %>").value = "";
            document.getElementById("<%=ddllocationtype.ClientID %>").value = "0";
            document.getElementById("<%=ddlcapacityin.ClientID %>").value = "0";
            document.getElementById("<%=ddlvelocityType.ClientID %>").value = "0";
            document.getElementById("<%=radiochannelY.ClientID %>").checked = true;
        }

        function CheckDuplicateAddress(invoker) {
            var sequence = getParameterByName("Sequence");
            PageMethods.PMCheckDuplicate(invoker.value, parseInt(Sequence), OnSucessCheckDuplicate, null);
        }

        function OnSucessCheckDuplicate(result) {
            if (result != "") {
                if (result != "Same Location details already exists, do you want to continue") {

                    var msg = "Same Location details already exists in Archive[ based on mandatory fields ] \n\n - Do you want to unarchive ?";
                    var answer1 = confirm(msg);
                    if (answer1) {
                        window.opener.SetUnArchiveConatactPerson(result, "N");
                        alert("Unarchive successful");
                        window.opener.GVLocation.refresh();
                        self.close();
                    }
                }
                else {
                    var answer = confirm(result);
                    if (answer == false) {
                        document.getElementById("<%= txtbuilding.ClientID %>").value = "";
                    }
                }
            }
        }

        </script>

        <script type="text/javascript">

            function openLocationSearch(sequence) {
                var CompanyID = 0;
                var warehouseID = document.getElementById("<%=hdnWarehouseID.ClientID %>").value;
             var CustomerID = document.getElementById("<%=hdncustomerID.ClientID %>").value;
             if (sequence == 0) {
                 // open bulding WarehouseBuilding

                 window.open('../Warehouse/WarehouseBuilding.aspx?warehouseID=' + warehouseID + '&CustomerID=' + CustomerID + "", '', 'height=500px, width=980px,status= 0, resizable= 1, scrollbars=1, toolbar=0,location=center,menubar=0, screenX=300; screenY=50', target = "_blank");
             }
             else if (sequence == 1) {

                 //open floar
                 var BuildingID = document.getElementById("<%=hdnBuildingID.ClientID %>").value;
                 window.open('../Warehouse/WarehouseFloar.aspx?BuildingID=' + BuildingID + '&CustomerID=' + CustomerID + "", '', 'height=500px, width=955px,status= 0, resizable= 1, scrollbars=1, toolbar=0,location=center,menubar=0, screenX=300; screenY=50', target = "_blank");
             }
             else if (sequence == 2) {
                 //open pathway
                 var FloarID = document.getElementById("<%=hdnFloarID.ClientID %>").value;
                 window.open('../Warehouse/WarehousePassage.aspx?FloarID=' + FloarID + '&CustomerID=' + CustomerID + "", '', 'height=500px, width=955px,status= 0, resizable= 1, scrollbars=1, toolbar=0,location=center,menubar=0, screenX=300; screenY=50', target = "_blank");
             }
             else if (sequence == 3) {
                 // open section
                 var PassageID = document.getElementById("<%=hdnpassageID.ClientID %>").value;
                 window.open('../Warehouse/WarehouseSection.aspx?PassageID=' + PassageID + '&CustomerID=' + CustomerID + "", '', 'height=500px, width=955px,status= 0, resizable= 1, scrollbars=1, toolbar=0,location=center,menubar=0, screenX=300; screenY=50', target = "_blank");
             }
             else {
                 //open shelf
                 var SectionID = document.getElementById("<%=hdnsectionID.ClientID %>").value;
                 window.open('../Warehouse/WarehouseShelf.aspx?SectionID=' + SectionID + '&CustomerID=' + CustomerID + "", '', 'height=500px, width=955px,status= 0, resizable= 1, scrollbars=1, toolbar=0,location=center,menubar=0, screenX=300; screenY=50', target = "_blank");
             }
}
        </script>

        <%--Code To Get values From Child Form--%>
        <script type="text/javascript">
            function GetBuilding(BuildID, BuildName) {
                var BuildingID = document.getElementById("hdnBuildingID");
                var BuildingName = document.getElementById("hdnBuildingName");
                BuildingID.value = BuildID;
                BuildingName.value = BuildName;
                var txtbuilding = document.getElementById("<%= txtbuilding.ClientID %>");
                txtbuilding.value = BuildName;
                document.getElementById("<%= txtfloar.ClientID %>").value = "";
                document.getElementById("<%= txtpathway.ClientID %>").value = "";
                document.getElementById("<%= txtsection.ClientID %>").value = "";
                document.getElementById("<%= txtshelf.ClientID %>").value = "";
            }

            function GetFloar(FloarID, FloarName) {
                var hdnFloarID = document.getElementById("hdnFloarID");
                var hdnFloarName = document.getElementById("hdnFloarName");

                hdnFloarID.value = FloarID;
                hdnFloarName.value = FloarName;

                var txtfloar = document.getElementById("<%=txtfloar.ClientID %>");
                txtfloar.value = FloarName;
                document.getElementById("<%= txtpathway.ClientID %>").value = "";
                document.getElementById("<%= txtsection.ClientID %>").value = "";
                document.getElementById("<%= txtshelf.ClientID %>").value = "";
            }

            function GetPasssage(PassageID, PassageName) {
                var hdnpassageID = document.getElementById("hdnpassageID");
                var hdnpassageName = document.getElementById("hdnpassageName");

                hdnpassageID.value = PassageID;
                hdnpassageName.value = PassageName;

                var txtpathway = document.getElementById("<%=txtpathway.ClientID %>");
                txtpathway.value = PassageName;
                document.getElementById("<%= txtsection.ClientID %>").value = "";
                document.getElementById("<%= txtshelf.ClientID %>").value = "";
            }

            function GetSection(SectionID, SectionName) {
                var hdnsectionID = document.getElementById("hdnsectionID");
                var hdnsectionName = document.getElementById("hdnsectionName");

                hdnsectionID.value = SectionID;
                hdnsectionName.value = SectionName;

                var txtsection = document.getElementById("<%=txtsection.ClientID %>");
                txtsection.value = SectionName;
                document.getElementById("<%= txtshelf.ClientID %>").value = "";
            }

            function GetShelf(ShelfID, ShelfName) {
                var hdnShelfID = document.getElementById("hdnShelfID");
                var hdnShelfName = document.getElementById("hdnShelfName");

                hdnShelfID.value = ShelfID;
                hdnShelfName.value = ShelfName;

                var txtshelf = document.getElementById("<%=txtshelf.ClientID %>");
                txtshelf.value = ShelfName;
            }

        </script>

        <script type="text/javascript">
            function GetLocationType() {
                var ddllocationtype = document.getElementById("<%=ddllocationtype.ClientID %>");
                document.getElementById("<%=hdnlocType.ClientID %>").value = ddllocationtype.value;
                var hdnlocType = document.getElementById("<%=hdnlocType.ClientID %>");
                //alert(hdnSelectedCompany.value);
            }

            function fnAllowDigitsOnly(key) {
                var keycode = (key.which) ? key.which : key.keyCode;
                if ((keycode < 48 || keycode > 57) && (keycode != 8) && (keycode != 9) && (keycode != 11) && (keycode != 16) && (keycode != 37) && (keycode != 38) && (keycode != 39) && (keycode != 40) && (keycode != 219) && (keycode != 127)) {
                    return false;
                }
            }
        </script>

        <%-- Location Code and Sort Code Validation--%>
        <script type="text/javascript">
            function validateLocCode() {
                var LocCode = document.getElementById("<%=txtlocationCode.ClientID %>");
                var WarehouseID = document.getElementById("<%=hdnWarehouseID.ClientID %>");
                PageMethods.ChkduplicateLoc(LocCode.value, WarehouseID.value, LocDuplicate_onSuccess, LocDuplicate_onFail);
            }

            function LocDuplicate_onSuccess(result) {
                if (result != "") {
                    if (result == "Duplicate Found") {
                        showAlert("Location Code Already Present!", "Error", "#");
                        document.getElementById("<%=txtlocationCode.ClientID %>").value = "";
                        document.getElementById("<%=txtlocationCode.ClientID %>").focus();
                    }
                }
            }

            function LocDuplicate_onFail() { }


            function Validatesortcode() {
                var SortCode = document.getElementById("<%=txtsortCode.ClientID %>");
                var WarehouseID = document.getElementById("<%=hdnWarehouseID.ClientID %>");
                PageMethods.ChkDuplicateSortCode(SortCode.value, WarehouseID.value, SortDuplicate_onSuccess, SortDuplicate_onFail)
            }

            function SortDuplicate_onSuccess(result) {
                if (result != "") {
                    if (result == "Duplicate Found") {
                        showAlert("Sort Code  Already Present!", "Error", "#");
                        document.getElementById("<%=txtsortCode.ClientID %>").value = "";
                    }
                }
            }

            function SortDuplicate_onFail() { }


        </script>
    </form>
</body>
</html>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddressInfo.aspx.cs" Inherits="BrilliantWMS.Address.AddressInfo" %>

<script src="../MasterPage/JavaScripts/countries.js" type="text/javascript"></script>
<script src="../MasterPage/JavaScripts/crm.js" type="text/javascript"></script>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>GWC</title>
      
</head>
<body>
    <form id="form_Address" runat="server" style="background: white;">
    <div>
        <asp:ScriptManager ID="ScriptmanagerAddress" runat="server" EnablePageMethods="true">
        </asp:ScriptManager>
        <center>
             <asp:HiddenField ID="hdnCountry" runat="server" ClientIDMode="Static" />
             <asp:HiddenField ID="hdncountryState" runat="server" ClientIDMode="Static" />
            <div id="divLoading" style="height: 280px; width: 1000px; display: none" class="modal">
                <center>
                    <br />
                    <span id="spanLoading" style="font-size: 17px; font-weight: bold; color: Yellow;">Processing
                        please wait...</span>
                </center>
            </div>
            <asp:ValidationSummary ID="validationsummary_UcAddressInfo" runat="server" ShowMessageBox="true"
                ShowSummary="false" DisplayMode="bulletlist" ValidationGroup="AddressSubmit" />
            <table class="gridFrame" style="margin: 2px 2px 2px 2px; width: 970px">
                <tr>
                    <td style="text-align: left;">
                        <asp:Label class="headerText" ID="lblAddressFormHeader" runat="server"></asp:Label>
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
                        <table class="tableForm" style="border: none; width: 970px">
                            <tr>
                                <td>
                                    <req>Address Line 1 :</req>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtAddress1" TextMode="MultiLine" runat="server" Width="212px" MaxLength="100"
                                        onkeyup="TextBox_KeyUp(this,'CharactersCounter1','100');" ClientIDMode="Static"
                                        onchange="CheckDuplicateAddress();"></asp:TextBox>
                                    <br />
                                    <span class="watermark"><span id="CharactersCounter1">100</span> characters remaining
                                        out of 100 </span>
                                   <%-- <asp:RequiredFieldValidator ID="RfValtxtAddress1" ForeColor="Maroon" runat="server"
                                        ControlToValidate="txtAddress1" ErrorMessage="Fill Address Line1" ValidationGroup="AddressSubmit"
                                        Display="None"></asp:RequiredFieldValidator>--%>
                                </td>
                                <td>
                                    Address Line 2 :
                                </td>
                                <td>
                                    <asp:TextBox ID="txtAddress2" TextMode="MultiLine" runat="server" Width="212px" MaxLength="100"
                                        onkeyup="TextBox_KeyUp(this,'CharactersCounter2','100');" ClientIDMode="Static"></asp:TextBox>
                                    <br />
                                    <span class="watermark"><span id="CharactersCounter2">100</span> characters remaining
                                        out of 100 </span>
                                </td>
                                <td>
                                    Address Line 3 :
                                </td>
                                <td>
                                    <asp:TextBox ID="txtAddress3" TextMode="MultiLine" runat="server" Width="212px" MaxLength="100"
                                        onkeyup="TextBox_KeyUp(this,'CharactersCounter3','100')" ClientIDMode="Static"></asp:TextBox>
                                    <br />
                                    <span class="watermark"><span id="CharactersCounter3">100</span> characters remaining
                                        out of 100 </span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Route :
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlRoute" runat="server" Width="211px" DataValueField="ID"
                                        DataTextField="Name" ClientIDMode="Static">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    Landmark :
                                </td>
                                <td>
                                    <asp:TextBox ID="txtLandMark" runat="server" Width="210px" MaxLength="50" ClientIDMode="Static"></asp:TextBox>
                                </td>
                                <td>
                                    Postal Code :
                                </td>
                                <td>
                                    <asp:TextBox ID="txtPostalCode" runat="server" Width="210px" MaxLength="7" onkeydown="return AllowPostalCode(this, event);"
                                        onkeypress="return AllowPostalCode(this, event);" ClientIDMode="Static"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <req>Country :</req>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlCountry" ClientIDMode="Static" runat="server" onchange="print_state('ddlState',this.selectedIndex); onCountryChange(this);"
                                        Width="211px">
                                    </asp:DropDownList>
                                 <%--   <asp:RequiredFieldValidator ID="RFValddlCountry" runat="server" ForeColor="Maroon"
                                        ValidationGroup="AddressSubmit" InitialValue="0" ControlToValidate="ddlCountry"
                                        ErrorMessage="Select Country" Display="None"></asp:RequiredFieldValidator>--%>
                                </td>
                                <td>
                                    <req>State :</req>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlState" ClientIDMode="Static" runat="server" Width="211px">
                                    </asp:DropDownList>  <%--onchange="CheckDuplicateAddress();printZone(); printSubZone();"--%>
                                  <%--  <asp:RequiredFieldValidator ID="RFValstate" ForeColor="Maroon" ControlToValidate="ddlState"
                                        InitialValue="0" runat="server" ErrorMessage="Select State" ValidationGroup="AddressSubmit"
                                        Display="None"></asp:RequiredFieldValidator>--%>
                                </td>
                                <td>
                                    <req>City : </req>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtcity" MaxLength="50" onchange="CheckDuplicateAddress()" Width="210px"
                                        runat="server" ClientIDMode="Static"></asp:TextBox>
                                   <%-- <asp:RequiredFieldValidator ID="rfvalcity" runat="server" Display="None" ControlToValidate="txtcity"
                                        ValidationGroup="AddressSubmit" ErrorMessage="Enter City"></asp:RequiredFieldValidator>--%>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label runat="server" ID="lblZone" Text="Zone" ClientIDMode="Static"></asp:Label>
                                    :
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlZone" ClientIDMode="Static" runat="server" onchange="printSubZone();"
                                        Width="211px">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblSubZone" Text="Sub-zone" ClientIDMode="Static"></asp:Label>
                                    :
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlSubZone" ClientIDMode="Static" runat="server" Width="211px">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    Email ID :
                                </td>
                                <td>
                                    <asp:TextBox ID="txtEmailID" runat="server" Width="210px" MaxLength="100" ClientIDMode="Static"></asp:TextBox>
                                    <asp:RegularExpressionValidator ID="RegularExpressionEmail" runat="server" ControlToValidate="txtEmailID"
                                        ForeColor="Maroon" ErrorMessage="Enter Valid Email Address" ValidationGroup="AddressSubmit"
                                        ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" Display="None"></asp:RegularExpressionValidator>
                                </td>
                            </tr>
                            <tr>
                                <%--  <td>
                                    <req>Active : </req>
                                </td>
                                <td style="text-align: left;">
                                    <asp:RadioButton ID="rbtnActiveYes" runat="server" Text="Yes" GroupName="rbtnActive"
                                        Checked="true" />
                                    <asp:RadioButton ID="rbtnActiveNo" runat="server" Text="No" GroupName="rbtnActive" />
                                </td>--%>
                                <td>
                                    Phone No. :
                                </td>
                                <td>
                                    <asp:TextBox ID="txtPhone" runat="server" Width="210px" MaxLength="50" 
                                        ClientIDMode="Static" onkeypress="return AllowPhoneNo(this, event);" onkeydown="return AllowPhoneNo(this, event);"></asp:TextBox>
                                </td>
                                <td>
                                    Fax No. :
                                </td>
                                <td>
                                    <asp:TextBox ID="txtFax" runat="server" Width="210px" MaxLength="50" onkeydown="return AllowPhoneNo(this, event);"
                                        ClientIDMode="Static" onkeypress="return AllowPhoneNo(this, event);"></asp:TextBox>
                                </td>
                                <td colspan="2">
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: left; width: 80%">
                    </td>
                    <td style="width: 20%">
                        <table style="float: right;">
                            <tr>
                                <td>
                                    <input type="button" id="Button1" value="Submit" onclick="SubmitAddress();" style="width: 70px;" />
                                </td>
                                <td>
                                    <input type="button" id="Button2" value="Clear" onclick="ClearAddress()" style="width: 70px;" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </center>
    </div>

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


    <script type="text/javascript">
       // print_country("ddlCountry");
        //printZone();
        //printSubZone();
        //var ZoneID;
        //var SubZoneID;


        /*On Edit*/
        //function setCountry(country, state, zone, subzone) {
        //    var ddlCountry = document.getElementById("ddlCountry");
        //    ddlCountry.value = country;

        //    print_state('ddlState', ddlCountry.selectedIndex);
        //    ddlState = document.getElementById("ddlState");
        //    ddlState.value = state;

        //    printZone();
        //    ZoneID = zone;
        //    SubZoneID = subzone;

        //    TextBox_KeyUp(document.getElementById("txtAddress1"), 'CharactersCounter1', '100');
        //    TextBox_KeyUp(document.getElementById("txtAddress2"), 'CharactersCounter2', '100');
        //    TextBox_KeyUp(document.getElementById("txtAddress3"), 'CharactersCounter3', '100');
        //}

        function ClearAddress() {

            document.getElementById("txtAddress1").value = "";
            document.getElementById("txtAddress2").value = "";
            document.getElementById("txtAddress3").value = "";
            document.getElementById("ddlRoute").value = 0;
            document.getElementById("txtLandMark").value = "";
            document.getElementById("ddlCountry").selectedIndex = 0;
            document.getElementById("ddlState").options.length = 0;
            document.getElementById("ddlZone").options.length = 0;
            document.getElementById("ddlSubZone").options.length = 0;

            document.getElementById("txtcity").value = "";
            document.getElementById("txtPostalCode").value = "";
            document.getElementById("txtPhone").value = "";
            document.getElementById("txtFax").value = "";
            document.getElementById("txtEmailID").value = "";
            rbtnActiveYes.checked = true; rbtnActiveNo.checked = false;
            TextBox_KeyUp(document.getElementById("txtAddress1"), 'CharactersCounter1', '100');
            TextBox_KeyUp(document.getElementById("txtAddress2"), 'CharactersCounter2', '100');
            TextBox_KeyUp(document.getElementById("txtAddress3"), 'CharactersCounter3', '100');
        }

        function SubmitAddress() {
            if (typeof (Page_ClientValidate) == 'function') {
                Page_ClientValidate();
            }
            if (Page_IsValid) {

                var ddlCountry = document.getElementById("<%=ddlCountry.ClientID%>");
                var ddlState = document.getElementById("<%=ddlState.ClientID%>");

                if (document.getElementById("<%=txtAddress1.ClientID%>").value == "" || document.getElementById("txtcity").value == "" || ddlCountry.options[ddlCountry.selectedIndex].text == "Select Country" || ddlState.options[ddlState.selectedIndex].text == "Select State") {

                    if (document.getElementById("<%=txtAddress1.ClientID%>").value == "") {
                        showAlert("Please enter address!", "Error", "#");
                        txtAddress1.focus();

                    }
                    else if (ddlCountry.options[ddlCountry.selectedIndex].text == "Select Country") {
                        showAlert("Please select Country!", "Error", "#");
                        ddlCountry.focus();

                    }
                    else if (ddlState.options[ddlState.selectedIndex].text == "Select State") {
                        showAlert("Please select State!", "Error", "#");
                        ddlState.focus();

                    }
                    else if (document.getElementById("txtcity").value == "") {
                        showAlert("Please enter City!", "Error", "#");
                        txtcity.focus();

                    }
                }
                else {
                    LoadingOn();
                    var AddressInfo = new Object();

                    AddressInfo.AddressLine1 = document.getElementById("txtAddress1").value;
                    AddressInfo.AddressLine2 = document.getElementById("txtAddress2").value;
                    AddressInfo.AddressLine3 = document.getElementById("txtAddress3").value;
                    AddressInfo.RouteID = document.getElementById("ddlRoute").value;
                    AddressInfo.Landmark = document.getElementById("txtLandMark").value;
                    AddressInfo.County = document.getElementById("ddlCountry").value;
                    AddressInfo.State = document.getElementById("ddlState").value;
                    AddressInfo.Zone = "0";
                    if (document.getElementById("ddlZone").selectedIndex > 0) AddressInfo.Zone = document.getElementById("ddlZone").value; //new add

                    AddressInfo.SubZone = "0";
                    if (document.getElementById("ddlSubZone").selectedIndex > 0) AddressInfo.SubZone = document.getElementById("ddlSubZone").value; //new add
                    AddressInfo.City = document.getElementById("txtcity").value;
                    AddressInfo.Zipcode = document.getElementById("txtPostalCode").value;
                    AddressInfo.PhoneNo = document.getElementById("txtPhone").value;
                    AddressInfo.FaxNo = document.getElementById("txtFax").value;
                    AddressInfo.EmailID = document.getElementById("txtEmailID").value;
                    //                AddressInfo.Active = "N";
                    //                if (rbtnActiveYes.checked == true) { AddressInfo.Active = "Y"; }

                    PageMethods.PMSaveAddress(AddressInfo, SubmitAddress_onSuccess, SubmitAddress_onFail)
                }

            }
            else { }
        }

        function SubmitAddress_onSuccess() {
            LoadingOff();
            alert("Address details saved successfully");
            window.opener.GvAddressInfo.refresh();
            self.close();
        }

        function SubmitAddress_onFail() { alert("error"); }

        function CheckDuplicateAddress() {
            var Address = document.getElementById("<%= txtAddress1.ClientID %>").value;
            var Country = document.getElementById("<%= ddlCountry.ClientID %>").value;
            var State = document.getElementById("<%= ddlState.ClientID %>").value;
            var City = document.getElementById("<%= txtcity.ClientID %>").value;

            if (Address != "" && Country != "" && State != "" && City != "") {
                PageMethods.PMCheckDuplicateAddress(Address, Country, State, City, OnSucessCheckDuplicateAddress, OnfailedCheckDuplicateAddress);
            }
        }

        function OnSucessCheckDuplicateAddress(result) {
            if (result != "") {
                if (result != "Same address details already exists, do you want to continue") {
                    var msg = "Same address details already exists in Archive[ based on mandatory fields ] \n\n - Do you want to unarchive ?";
                    var answer1 = confirm(msg);
                    if (answer1) {
                        window.opener.SetUnArchive(result, "N");
                        alert("Unarchive successful");
                        window.opener.GvAddressInfo.refresh();
                        self.close();
                    }

                }

                else {
                    var msg = "Same address details already exists [ based on mandatory fields ] \n\n - Do you want to continue ?";
                    var answer = confirm(msg);
                    if (answer) { }
                    else {
                        document.getElementById("<%= txtAddress1.ClientID %>").value = "";
                        document.getElementById("<%= ddlCountry.ClientID %>").selectedIndex = 0;
                        document.getElementById("<%= ddlState.ClientID %>").value = null;
                        document.getElementById("<%= txtcity.ClientID %>").value = "";
                    }
                }
            }
        }



        function OnfailedCheckDuplicateAddress() {
        }

        function printZone() {
            document.getElementById("ddlZone").options.length = 0;
            if (document.getElementById("ddlCountry").selectedIndex > 0 && document.getElementById("ddlState").selectedIndex > 0) {
                var Country = document.getElementById("ddlCountry").value;
                var State = document.getElementById("ddlState").value;
                LoadingOn();
                PageMethods.PMprintZone(Country, State, OnSuccessprintZone, OnFailprintZone);
            }
            else {
                var ddlZone = document.getElementById("ddlZone");
                var ddlSubZone = document.getElementById("ddlSubZone");

                var option0 = document.createElement("option");
                option0.text = 'N/A'; option0.value = "0"

                var option1 = document.createElement("option");
                option1.text = 'N/A'; option1.value = "0"
                try {
                    ddlZone.add(option0, null); //Standard 
                    ddlSubZone.add(option1, null); //Standard 
                } catch (error) {
                    ddlZone.add(option0); // IE only
                    ddlSubZone.add(option1); // IE only
                }
            }
        }

        function OnSuccessprintZone(result) {
            var ddlZone = document.getElementById("ddlZone");

            var option0 = document.createElement("option");
            if (result.length > 0) {
                option0.text = 'Select ' + document.getElementById("lblZone").innerHTML;
                option0.value = "0"
            }
            else { option0.text = 'N/A'; option0.value = "0" }

            try {
                ddlZone.add(option0, null); //Standard 

            } catch (error) {
                ddlZone.add(option0); // IE only

            }

            for (var i = 0; i < result.length; i++) {
                var option1 = document.createElement("option");
                option1.text = result[i].Zone;
                option1.value = result[i].ID;

                try {
                    ddlZone.add(option1, null); //Standard 
                } catch (error) {
                    ddlZone.add(option1); // IE only
                }
            }
            if (ZoneID != "") {
                ddlZone.value = ZoneID;
                ZoneID = "";
                printSubZone();
            }
            LoadingOff();
        }

        function OnFailprintZone() {
            LoadingOff();
        }

        function printSubZone() {
            document.getElementById("ddlSubZone").options.length = 0;
            if (document.getElementById("ddlZone").selectedIndex > 0) {
                LoadingOn();
                PageMethods.PMprintSubZone(document.getElementById("ddlZone").value, OnSuccessprintSubZone, OnFailprintSubZone);
            }
            else {
                var ddlSubZone = document.getElementById("ddlSubZone");
                var option0 = document.createElement("option");
                option0.text = 'N/A'; option0.value = "0"
                try {
                    ddlSubZone.add(option0, null); //Standard 
                } catch (error) {
                    ddlSubZone.add(option0); // IE only
                }
            }
        }

        function OnSuccessprintSubZone(result) {
            var ddlSubZone = document.getElementById("ddlSubZone");

            var option0 = document.createElement("option");
            if (result.length > 0) {
                option0.text = 'Select ' + document.getElementById("lblSubZone").innerHTML;
                option0.value = "0"
            }
            else { option0.text = 'N/A'; option0.value = "0" }

            try {
                ddlSubZone.add(option0, null); //Standard 
            } catch (error) {
                ddlSubZone.add(option0); // IE only
            }


            for (var i = 0; i < result.length; i++) {
                var option1 = document.createElement("option");
                option1.text = result[i].SubZone;
                option1.value = result[i].ID;

                try {
                    ddlSubZone.add(option1, null); //Standard 
                } catch (error) {
                    ddlSubZone.add(option1); // IE only
                }
            }
            if (SubZoneID != "") {
                if (ddlSubZone.length > 0) { ddlSubZone.value = SubZoneID; SubZoneID = ""; }
            }
            LoadingOff();

        }
        function OnFailprintSubZone() { LoadingOff(); }
    </script>
    </form>
</body>
</html>

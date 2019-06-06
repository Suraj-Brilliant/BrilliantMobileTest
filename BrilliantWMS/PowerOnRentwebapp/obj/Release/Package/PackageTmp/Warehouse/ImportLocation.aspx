<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/CRM.Master" AutoEventWireup="true" CodeBehind="ImportLocation.aspx.cs" Inherits="BrilliantWMS.Warehouse.ImportLocation"
     EnableEventValidation="false" Theme="Blue" %>

<%@ Register Assembly="obout_Grid_NET" Namespace="Obout.Grid" TagPrefix="obout" %>
<%@ Register Src="../Product/UCProductSearch.ascx" TagName="UCProductSearch" TagPrefix="uc1" %>
<%@ Register Src="../CommonControls/UCFormHeader.ascx" TagName="UCFormHeader" TagPrefix="uc2" %>
<%@ Register Src="../CommonControls/UC_Date.ascx" TagName="UC_Date" TagPrefix="uc2" %>
<%@ Register Src="../CommonControls/Toolbar.ascx" TagName="Toolbar" TagPrefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHolder_FormHeader" runat="server">
    <uc2:UCFormHeader ID="UCFormHeader1" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPHolder_Form" runat="server">
    <asp:HiddenField ID="hdncompanyid" runat="server" />
    <asp:HiddenField ID="hdndeptid" runat="server" />
    <asp:HiddenField ID="hdnnotavailableimageprod" runat="server" />
    <asp:HiddenField ID="hdnlocation" runat="server" />
   <span id="imgProcessing" style="display: none;">Please wait... </span>
    <div id="divLoading" style="height: 110%; width: 100%; display: none; top: 0; left: 0;"
        class="modal" runat="server" clientidmode="Static">
    </div>
    <div class="divHead" id="divRequestHead">
        <h4>
            <asp:Label ID="lblHeading" runat="server" Text="Import Location"></asp:Label>
        </h4>
    </div>
    <center>
        <table class="tblcls" cellpadding="0" cellspacing="20px" border="0" width="95%">
            <tr>
                <td align="center">
                    <table align="center" border="0">
                        <tr>
                            <td class="style3" align="center">
                                <span class="cartStepHolder"><span class="cartStepTitle cartStepCurrentTitle">
                                    <span class="divCartSymbol divCartCurrentSymbol">
                                        <asp:Label ID="lbl1" runat="server" Text="1" /></span><asp:Label ID="lblstep1" runat="server" Text="Upload File"></asp:Label></span>
                                    <span class="cartStepTitle"><span class="divCartSymbol">
                                        <asp:Label ID="lbl2" runat="server" Text="2" /></span><asp:Label ID="lblstep2" runat="server" Text="Data Validation & Verification"></asp:Label></span>
                                    <span class="cartStepTitle"><span class="divCartSymbol">
                                        <asp:Label ID="lbl3" runat="server" Text="3" /></span><asp:Label ID="lblstep3" runat="server" Text="Finished"></asp:Label></span>
                                </span>
                            </td>
                        </tr>
                    </table>
                    <table cellpadding="0" width="100%" cellspacing="5px" border="0" class="tableForm">
                        <tr>
                            <td align="left" colspan="4">

                                <asp:Label ID="lbltext1" runat="server" Font-Size="14px" Text="Import will faciliated Location Data to be directly imported into WMS."></asp:Label>
                                <%-- <span style="font-size:14px;"> Import will faciliated Order Data to be directly imported into OMS.</span><br />--%>
                                <asp:Label ID="Lbl" runat="server" Font-Size="14px" Text="For Downloading Order Import Data Template"></asp:Label>
                                <a href="LocationImport.xlsx" target="_blank"><font color="blue"><u>Click Here.
                                </u></font></a>

                            </td>
                        </tr>
                        <tr>
                            <td align="left" colspan="4"></td>
                        </tr>
                        <tr>
                            <td></td>
                            <td align="right" colspan="4">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblSelecFile" runat="server" Text="Select Import File"></asp:Label>
                                            :
                                        </td>
                                        <td style="text-align: left;">
                                            <asp:FileUpload ID="FileuploadPO" runat="server" multiple="multiple" />
                                        </td>
                                        <td style="text-align: right;">
                                            <asp:Button ID="btnUploadPo" runat="server" Text="  Upload  "  /> <%--OnClientClick="return ProgressBar()" OnClick="btnUploadPo_Click"--%>
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="FileuploadPO" ForeColor="Red"
                                                ErrorMessage="Upload only excel file" ValidationExpression="^.+(.xls|.XLS|.xlsx|.XLSX)$">
                                            </asp:RegularExpressionValidator>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td></td>
                        </tr>

                        <tr>
                            <td colspan="4">
                                <div id="divUpload" style="display: none">
                                    <%-- <b><i>Uploading file...</i></b><br />--%>
                                    <span style="text-align: center; float: inherit">
                                        <asp:Label ID="lbluploading" runat="server" Font-Bold="true" Font-Italic="true" Text="Uploading file..."></asp:Label></span><br />
                                    <asp:Image ID="uploadimg" runat="server" ImageUrl="~/images/upload-animation.gif" Width="380" Height="20" />

                                    <%-- <div style="text-align: center;">Uploading (<asp:Label ID="lblPercentage" runat="server" Text="   "></asp:Label>)</div>
                            <div align="center">
                            <div style="background-color:#ffffff;width: 300pt;height:20px;border:solid 1px #636363;border-radius:7px;overflow:hidden;text-align:left;" align="left">
                            <div id="divProgress" style="width: 200pt; height: 20px; border: solid 1pt gray; text-align: left;
                                background-color: #6BC048; display: none"></div>
                            </div>
                            </div>--%>
                                    <asp:Panel ID="uploadMessage" runat="server" Font-Bold="true" Style="text-align: center;">File uploaded successfully! Click On Next Button </asp:Panel>
                                </div>

                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <asp:Label ID="lblmessagesuccess" runat="server" Font-Size="16px" ForeColor="#206295" Font-Bold="true" Text="File uploaded successfully! Click On Next Button"></asp:Label>
                            </td>
                        </tr>

                        <tr style="visibility: hidden;">
                            <td>
                                <req><asp:Label ID="lblcompany" runat="server" Text="Company"></asp:Label> :</req>
                            </td>
                            <td style="text-align: left;">
                                <asp:DropDownList ID="ddlcompany" runat="server" DataTextField="Name" DataValueField="ID" onchange="GetDepartment()"
                                    Width="206px">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RfValCompany" runat="server" ControlToValidate="ddlcompany" InitialValue="0"
                                    Display="None" ErrorMessage="Please Select Company" ValidationGroup="Save"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <req><asp:Label ID="lblDept" runat="server" Text="Department"></asp:Label></req>
                                :
                            </td>
                            <td style="text-align: left;">
                                <asp:DropDownList ID="ddldepartment" runat="server" DataTextField="Territory" DataValueField="ID" onchange="Getdeptid()"
                                    Width="190px">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfdept" runat="server" ControlToValidate="ddldepartment" InitialValue="0"
                                    Display="None" ErrorMessage="Please Select Department" ValidationGroup="Save"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <req><asp:Label ID="lbllocation" runat="server" Text="Location"></asp:Label></req>
                                :
                            </td>
                            <td style="text-align: left;">
                                <asp:DropDownList ID="ddllocation" runat="server" DataTextField="AddressLine1" DataValueField="ID" Width="220px" onchange="Getlocatn()">
                                </asp:DropDownList>
                            </td>
                            <%-- --%>
                        </tr>
                        <tr style="visibility: hidden;">
                            <td>
                                <req><asp:Label ID="lblpaymentMethod" runat="server" Text="Payment Method"></asp:Label> :</req>
                            </td>
                            <td style="text-align: left;">
                                <asp:DropDownList ID="ddlpayment" runat="server" DataTextField="Name" DataValueField="ID"
                                    Width="206px">
                                    <asp:ListItem Text="None(Default)" Value="1"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                <req><asp:Label ID="lblexpDate" runat="server" Text="Expected Delivery Date"></asp:Label> :</req>
                            </td>
                            <td style="text-align: left;">
                                <uc2:UC_Date ID="UC_DateofBirth" runat="server" />
                            </td>
                            <td></td>
                            <td style="text-align: left;"></td>
                        </tr>

                    </table>
                </td>
            </tr>

            <tr>
                <td>
                    <table align="right">
                        <tr>
                            <td>

                                <asp:Button ID="Button12" runat="server" Text="  Next  "  CausesValidation="false" />
                                <%-- OnClientClick="return CheckDeptvalidations();" OnClick="Button12_Click"--%>
                            </td>
                            <td>
                                <asp:Button ID="Btncancel" runat="server" Text="Cancel"  CausesValidation="false" /> <%--OnClick="Btncancel_Click"--%>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </center>

    <script type="text/javascript">
        var ddlcompany = document.getElementById("<%=ddlcompany.ClientID %>");
        var ddldeptid = document.getElementById("<%=ddldepartment.ClientID %>");
        var ddlloccation = document.getElementById("<%=ddllocation.ClientID %>");

        function GetDepartment() {
            var obj1 = new Object();
            obj1.ddlcompanyId = ddlcompany.value.toString();
            document.getElementById("<%=hdncompanyid.ClientID %>").value = ddlcompany.value.toString();
            PageMethods.GetDepartment(obj1, getLoc_onSuccessed);
            PageMethods.GetLocation(obj1, GetLocation_OnSuccess);
        }

        function getLoc_onSuccessed(result) {

            ddldeptid.options.length = 0;
            for (var i in result) {
                AddOption(result[i].Name, result[i].Id);
            }
        }

        function AddOption(text, value) {

            var option = document.createElement('option');
            option.value = value;
            option.innerHTML = text;
            ddldeptid.options.add(option);
        }

        function GetLocation_OnSuccess(result) {
            ddlloccation.options.length = 0;
            for (var i in result) {
                AddOption1(result[i].Name, result[i].Id);
            }
        }

        function AddOption1(text, value) {
            var option1 = document.createElement('option');
            option1.value = value;
            option1.innerHTML = text;
            ddlloccation.options.add(option1);
        }


        function Getdeptid() {
            document.getElementById("<%=hdndeptid.ClientID %>").value = ddldeptid.value.toString();
        }

        function Getlocatn() {
            document.getElementById("<%=hdnlocation.ClientID%>").value = ddlloccation.value.toString();
        }
    </script>

    <script type="text/javascript">
        function CheckDeptvalidations() {
            if (document.getElementById("<%=ddlcompany.ClientID %>").value == "0") {
                showAlert("Please Select Company!", "error", "#");
                document.getElementById("<%=ddlcompany.ClientID %>").focus();
                return false;
            }
            if (document.getElementById("<%=ddldepartment.ClientID %>").value == "0") {
                showAlert("Please Select Department!", "error", "#");
                document.getElementById("<%=ddldepartment.ClientID %>").focus();
                return false;
            }
            if (document.getElementById("<%=ddllocation.ClientID %>").value == "0") {
                showAlert("Please Select Location!", "error", "#");
                document.getElementById("<%=ddllocation.ClientID %>").focus();
                return false;
            }
            if (getDateFromUC("<%= UC_DateofBirth.ClientID %>") == "") {
                showAlert("Please Select Exp. Delivery Date!", "error", "#");
                return false;
            }

            return true;
        };

    </script>

    <style type="text/css">
        /*Grid css*/
        .class1 {
            opacity: 0.4;
            filter: alpha(opacity=40);
            cursor: wait;
        }

        .class2 {
            opacity: 1;
            filter: alpha(opacity=100);
            cursor: pointer;
        }

        .excel-textbox {
            background-color: transparent;
            border: 0px;
            padding: 0px;
            outline: 0;
            font: inherit;
            width: 100%;
            padding-top: 8px;
            padding-left: 4px;
            padding-bottom: 8px;
            text-align: Left;
        }

        .excel-textbox-focused {
            background-color: #FFFFFF;
            border: 0px;
            padding: 0px;
            outline: 0;
            font: inherit;
            width: 100%;
            padding-top: 8px;
            padding-left: 2px;
            padding-bottom: 8px;
            text-align: right;
        }

        .excel-textbox-error {
            color: #FF0000;
        }

        .ob_gCc2 {
            padding-left: 3px !important;
        }

        .ob_gBCont {
            border-bottom: 1px solid #C3C9CE;
        }

        .excel-checkbox {
            height: 20px;
            line-height: 20px;
        }
    </style>
    <style type="text/css">
        /*POR Collapsable Div*/

        .PanelCaption {
            color: Black;
            font-size: 13px; /*font-weight: bold;*/
            margin-top: -22px;
            margin-left: -5px;
            position: absolute;
            background-color: White;
            padding: 0px 2px 0px 2px;
        }

        .divHead {
            border: solid 2px #F5DEB3;
            width: 99%;
            text-align: left;
        }

            .divHead h4 {
                /*color: #33CCFF;*/
                color: #483D8B; /*margin: 3px 3px 3px 3px;*/
            }

            .divHead a {
                float: left;
                margin-top: -15px;
                margin-right: 5px;
            }

                .divHead a:hover {
                    cursor: pointer;
                    color: Red;
                }

        .divDetailExpand {
            border: solid 2px #F5DEB3;
            border-top: none;
            width: 99%;
            padding: 5px 0 5px 0;
        }

        .divDetailCollapse {
            display: none;
        }
        /*End POR Collapsable Div*/
    </style>

    <%--******Progress bar script*******--%>
    <script type="text/javascript">
        function ProgressBar() {
            if (document.getElementById('<%=FileuploadPO.ClientID %>').value != "") {
                document.getElementById("divUpload").style.display = "block";
                var getFileUploadMessageObj = document.getElementById('<%=uploadMessage.ClientID %>');
                if (getFileUploadMessageObj != null) {
                    getFileUploadMessageObj.style.display = 'none';
                }

                return true;
            }
            else {
                alert("Select a file to upload");
                return false;
            }

        }
    </script>

    <style type="text/css">
        .has-js .label_check, .has-js .label_radio {
            padding-left: 25px;
            padding-bottom: 10px;
        }

        .has-js .label_radio {
            background: url(../App_Themes/Blue/img/radio-off.png) no-repeat;
        }

        .has-js .label_check {
            background: url("../App_Themes/Blue/img/check-off.png") no-repeat;
        }

        .has-js label.c_on {
            background: url("../App_Themes/Blue/img/check-on.png") no-repeat;
        }

        .has-js label.r_on {
            background: url(../App_Themes/Blue/img/radio-on.png) no-repeat;
        }

        .has-js .label_check input, .has-js .label_radio input {
            position: absolute;
            left: -9999px;
        }
    </style>
    <%-- style to show three steps--%>
    <style type="text/css">
        .cartStepTitle
        {
            font-size: 20px;
            color: #cccccc;
            padding: 0px 50px 20px 0px;
            display: inline-block;
            padding-right: 60px;
            font-weight: bold;
        }
        .divCartSymbol, .divCartCurrentSymbol
        {
            position: relative;
            top: 30px;
            left: -10px;
            display: inline-block;
            background-image: url(../images/opt-bg-normal1.png);
            background-repeat: no-repeat;
            background-position: center center;
            font-size: 30px;
            font-family: Trebuchet MS, Arial;
            color: #ffffff;
            padding: 20px;
            text-decoration: none;
            overflow: hidden;
            opacity: 0.7;
        }
        .tdCartStepHolder
        {
            padding-left: 10px;
        }
        .cartStepHolder
        {
            position: relative;
            top: -10px;
        }
        .divCartCurrentSymbol
        {
            background-image: url(../images/opt-bg-selected2.png);
            opacity: 1;
        }
        .cartStepCurrentTitle
        {
            color: #000000;
        }
        
        
        
        .style2
        {
            height: 47px;
        }
        
        .btnCommonStyle
        {
            font-family: inherit;
            font-weight: bold;
            font-size: 20px;
            color: #ffffff;
            text-decoration: none !important;
            padding-left: 50px;
            padding-right: 50px;
            padding-top: 14px;
            padding-bottom: 14px;
            border-radius: 7px; /* fallback */
            background-color: #1a82f7;
            background: url(../images/btn-common-bg.jpg);
            background-repeat: repeat-x; /* Safari 4-5, Chrome 1-9 */
            background: -webkit-gradient(linear, 0% 0%, 0% 100%, from(#3FC3C3), to(#339E9E)); /* Safari 5.1, Chrome 10+ */
            background: -webkit-linear-gradient(top, #3FC3C3, #339E9E); /* Firefox 3.6+ */
            background: -moz-linear-gradient(top, #3FC3C3, #339E9E); /* IE 10 */
            background: -ms-linear-gradient(top, #3FC3C3, #339E9E); /* Opera 11.10+ */
            background: -o-linear-gradient(top, #3FC3C3, #339E9E);
        }
        
        .tblcls
        {
            border: solid 2px #F5DEB3;
        }
    </style>
</asp:Content>

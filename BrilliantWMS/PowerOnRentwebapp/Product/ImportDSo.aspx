<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/CRM.Master" AutoEventWireup="true" EnableEventValidation="false"
    CodeBehind="ImportDSo.aspx.cs" Inherits="BrilliantWMS.POR.ImportDSo" Theme="Blue" %>

<%@ Register Assembly="obout_Grid_NET" Namespace="Obout.Grid" TagPrefix="obout" %>
<%--<%@ Register Src="~/CommonControls/Toolbar.ascx"  TagName="Toolbar" TagPrefix="uc2"%>--%>
<%--<%@ Register Src="../CommonControls/UC_Date.ascx" TagName="UC_Date" TagPrefix="uc1" %>--%>
<%@ Register Src="UCProductSearch.ascx" TagName="UCProductSearch" TagPrefix="uc1" %>
<%@ Register Src="../CommonControls/UCFormHeader.ascx" TagName="UCFormHeader" TagPrefix="uc2" %>
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
    <span id="imgProcessing" style="display: none;">Please wait... </span>
    <div id="divLoading" style="height: 110%; width: 100%; display: none; top: 0; left: 0;"
        class="modal" runat="server" clientidmode="Static">
    </div>
    <div class="divHead" id="divRequestHead">
        <h4>
            <asp:Label ID="lblHeading" runat="server" Text="Import Images"></asp:Label>
        </h4>
    </div>
    <center>
        <table class="tblcls" cellpadding="0" cellspacing="20px" border="0" width="75%">
            <tr>
                <td align="center">
                    <table align="center" border="0">
                        <tr>
                            <td class="style3" align="center">
                                <span class="cartStepHolder"><span class="cartStepTitle cartStepCurrentTitle"><span
                                    class="divCartSymbol divCartCurrentSymbol"><asp:Label ID="lbl1" runat="server" Text="1"/></span><asp:Label ID="lblstep1" runat="server" Text="Upload File"/></span><span class="cartStepTitle"><span
                                        class="divCartSymbol"><asp:Label ID="lbl2" runat="server" Text="2"/></span><asp:Label ID="lblstep2" runat="server" Text="Data Validation & Verification"/></span></span>
                            </td>
                        </tr>
                    </table>
                    <table cellpadding="0" cellspacing="10px" border="0" class="tableForm">
                        <tr>
                            <td style="text-align: right;">
                                <req><asp:Label ID="lblcompany" runat="server" Text="Company"></asp:Label> :</req>
                            </td>
                            <td style="text-align: left;">
                                <asp:DropDownList ID="ddlcompany" runat="server"  DataTextField="Name" DataValueField="ID" onchange="GetDepartment()"
                                    Width="206px">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <req><asp:Label ID="lblDept" runat="server" Text="Customer"></asp:Label></req> :
                            </td>
                            <td style="text-align: left;">
                                <asp:DropDownList ID="ddldepartment" runat="server" DataTextField="Territory" DataValueField="ID" onchange="Getdeptid()"
                                    Width="206px">
                                   </asp:DropDownList>
                            </td>
                            <%-- <h4>
                                    Import will faciliated Student Data to be directly imported into IMS.<br />
                                    <asp:Label ID="Lbl" runat="server" Text="For Downloading Import Data Template"></asp:Label>
                                    <a href="StudentImportTemplate.xlsx" target="_blank"><font color="blue"><u>Click Here.
                                    </u></font></a>
                                </h4>--%>
                        </tr>
                        <tr>
                            <td>
                                    <asp:Label ID="lblSelecFile" runat="server" Text="Select Import File"></asp:Label> :
                                    
                            </td>
                            <td>
                                <asp:FileUpload ID="FileuploadPO" runat="server" multiple="multiple" />
                            </td>
                            <td align="right" colspan="2">
                                <asp:Button ID="btnUploadPo" runat="server" Text="  Upload  " OnClientClick="return ProgressBar()" OnClick="btnUploadPo_Click" />  <%--OnClientClick="return ProgressBar()"--%>
<%--                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="FileuploadPO"
                                    ErrorMessage="Upload only excel file" ValidationExpression="^.+(.xlsx|.XLSX)$">
                                </asp:RegularExpressionValidator>--%>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <hr style="border-color: #F5DEB3">
                            </td>
                        </tr>
                        <tr>
                        <td colspan="4">
                        <div id="divUpload" style="display: none" align="center">
                        <b><i>Uploading file(s)...</i></b><br />
                        <asp:Image ID="uploadimg" runat="server" ImageUrl="~/images/upload-animation.gif" Width="250" Height="20" />
                           <%-- <div style="text-align: center;">Uploading (<asp:Label ID="lblPercentage" runat="server" Text="   "></asp:Label>)</div>
                            <div align="center">
                            <div style="background-color:#ffffff;width: 300pt;height:20px;border:solid 1px #636363;border-radius:7px;overflow:hidden;text-align:left;" align="left">
                            <div id="divProgress" style="width: 200pt; height: 20px; border: solid 1pt gray; text-align: left;
                                background-color: #6BC048; display: none"></div>
                            </div>
                            </div>--%>
                            </div>
                            <asp:Panel ID="uploadMessage" runat="server" Font-Bold="true" Visible="false" style="text-align:center;">File uploaded successfully</asp:Panel>
                        </td>
                        </tr>
                        <tr>
                            <td >
                                
                            </td>
                            <td>
                                <div style="width: 300pt; text-align: center;">
                                    
                                 <!--   <asp:Label ID="Label1" Font-Size="X-Large" runat="server" Text=""></asp:Label> -->
                                </div>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td style="margin-right">
                    <table align="right">
                        <tr>
                            <td>
                                <asp:Button ID="Button12" runat="server" Text="Next" OnClick="Button12_Click" />
                                <%-- <asp:Button ID="btnNext" runat="server" Text="Next" OnClick="btnNext_Click" />--%>
                            </td>
                            <td>
                                <asp:Button ID="Btncancel" runat="server" Text="Cancel" OnClick="Btncancel_Click" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </center>

    <script>
        var ddlcompany = document.getElementById("<%=ddlcompany.ClientID %>");
        var ddldeptid = document.getElementById("<%=ddldepartment.ClientID %>");

        function GetDepartment() {
            var obj1 = new Object();
            obj1.ddlcompanyId = ddlcompany.value.toString();
            document.getElementById("<%=hdncompanyid.ClientID %>").value = ddlcompany.value.toString();
            PageMethods.GetDepartment(obj1, getLoc_onSuccessed);
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

        function Getdeptid() {
            document.getElementById("<%=hdndeptid.ClientID %>").value = ddldeptid.value.toString();
        }
    
    </script>


    <style type="text/css">
        /*Grid css*/
        .excel-textbox
        {
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
        .excel-textbox-focused
        {
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
        
        .excel-textbox-error
        {
            color: #FF0000;
        }
        
        .ob_gCc2
        {
            padding-left: 3px !important;
        }
        
        .ob_gBCont
        {
            border-bottom: 1px solid #C3C9CE;
        }
        
        .excel-checkbox
        {
            height: 20px;
            line-height: 20px;
        }
    </style>
    <style type="text/css">
        /*POR Collapsable Div*/
        
        .PanelCaption
        {
            color: Black;
            font-size: 13px; /*font-weight: bold;*/
            margin-top: -22px;
            margin-left: -5px;
            position: absolute;
            background-color: White;
            padding: 0px 2px 0px 2px;
        }
        .divHead
        {
            border: solid 2px #F5DEB3;
            width: 99%;
            text-align: left;
        }
        .divHead h4
        {
            /*color: #33CCFF;*/
            color: #483D8B; /*margin: 3px 3px 3px 3px;*/
        }
        .divHead a
        {
            float: left;
            margin-top: -15px;
            margin-right: 5px;
        }
        .divHead a:hover
        {
            cursor: pointer;
            color: Red;
        }
        .divDetailExpand
        {
            border: solid 2px #F5DEB3;
            border-top: none;
            width: 99%;
            padding: 5px 0 5px 0;
        }
        .divDetailCollapse
        {
            display: none;
        }
        /*End POR Collapsable Div*/
    </style>
    <script type="text/javascript">
        /*Checkbox js for css*/
        var d = document;
        var safari = (navigator.userAgent.toLowerCase().indexOf('safari') != -1) ? true : false;
        var gebtn = function (parEl, child) { return parEl.getElementsByTagName(child); };
        onload = function () {

            var body = gebtn(d, 'body')[0];
            body.className = body.className && body.className != '' ? body.className + ' has-js' : 'has-js';

            if (!d.getElementById || !d.createTextNode) return;
            var ls = gebtn(d, 'label');
            for (var i = 0; i < ls.length; i++) {
                var l = ls[i];
                if (l.className.indexOf('label_') == -1) continue;
                var inp = gebtn(l, 'input')[0];
                if (l.className == 'label_check') {
                    l.className = (safari && inp.checked == true || inp.checked) ? 'label_check c_on' : 'label_check c_off';
                    l.onclick = check_it;
                };
                if (l.className == 'label_radio') {
                    l.className = (safari && inp.checked == true || inp.checked) ? 'label_radio r_on' : 'label_radio r_off';
                    l.onclick = turn_radio;
                };
            };
        };
        var check_it = function () {
            var inp = gebtn(this, 'input')[0];
            if (this.className == 'label_check c_off' || (!safari && inp.checked)) {
                this.className = 'label_check c_on';
                if (safari) inp.click();
            } else {
                this.className = 'label_check c_off';
                if (safari) inp.click();
            };
        };
        var turn_radio = function () {
            var inp = gebtn(this, 'input')[0];
            if (this.className == 'label_radio r_off' || inp.checked) {
                var ls = gebtn(this.parentNode, 'label');
                for (var i = 0; i < ls.length; i++) {
                    var l = ls[i];
                    if (l.className.indexOf('label_radio') == -1) continue;
                    l.className = 'label_radio r_off';
                };
                this.className = 'label_radio r_on';
                if (safari) inp.click();
            } else {
                this.className = 'label_radio r_off';
                if (safari) inp.click();
            };
        };
        /*End*/

    </script>
    <%--******Progress bar script*******--%>
    <script>
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
   <%-- <script language="javascript" type="text/javascript">
        var size = 2;
        var id = 0;

        function ProgressBar() {
            if (document.getElementById('<%=FileuploadPO.ClientID %>').value != "") {
                document.getElementById("divProgress").style.display = "block";
                document.getElementById("divUpload").style.display = "block";
                id = setInterval("progress()", 500);
                return true;
            }
            else {
                alert("Select a file to upload");
                return false;
            }

        }

        function progress() {
            //alert(id);
            size = size + 1;
            if (size > 299) {
                clearTimeout(id);
            }
            document.getElementById("divProgress").style.width = size + "pt";
            document.getElementById("<%=lblPercentage.ClientID %>").firstChild.data = parseInt(size / 3) + "%";

        }

        function checkProgress(fileNum) {
          document.title('No: ' + fileNum);
        }

    </script>--%>
    <style type="text/css">
        .has-js .label_check, .has-js .label_radio
        {
            padding-left: 25px;
            padding-bottom: 10px;
        }
        .has-js .label_radio
        {
            background: url(../App_Themes/Blue/img/radio-off.png) no-repeat;
        }
        .has-js .label_check
        {
            background: url("../App_Themes/Blue/img/check-off.png") no-repeat;
        }
        .has-js label.c_on
        {
            background: url("../App_Themes/Blue/img/check-on.png") no-repeat;
        }
        .has-js label.r_on
        {
            background: url(../App_Themes/Blue/img/radio-on.png) no-repeat;
        }
        .has-js .label_check input, .has-js .label_radio input
        {
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

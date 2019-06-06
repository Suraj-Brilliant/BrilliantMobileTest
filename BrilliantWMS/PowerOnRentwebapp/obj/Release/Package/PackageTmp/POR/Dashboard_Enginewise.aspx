<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/CRM.Master" AutoEventWireup="true"
    CodeBehind="Dashboard_Enginewise.aspx.cs" Inherits="BrilliantWMS.POR.Dashboard_Enginewise" %>

<%@ Register Src="../DashBoard/DashBoard.ascx" TagName="DashBoard" TagPrefix="uc1" %>
<%@ Register Src="../CommonControls/UC_Date.ascx" TagName="UC_Date" TagPrefix="ucd" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHolder_FormHeader" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPHolder_Form" runat="server">
    <style type="text/css">
        #options
        {
            width: 100%;
            text-align: right;
            color: #9ac1c9;
        }
        #options a
        {
            text-decoration: none;
            color: #9ac1c9;
        }
        #options a:hover
        {
            color: #033;
        }
        
        #acc
        {
            width: 100%;
            list-style: none;
            color: #033;
        }
        #acc h3
        {
            width: 98.8%;
            border: 1px solid #9ac1c9;
            padding: 6px 6px 8px;
            font-weight: bold;
            margin-top: 5px;
            cursor: pointer;
            background: url(images/header.gif);
        }
        #acc h3:hover
        {
            background: #88B1E7;
        }
        #acc .acc-section
        {
            overflow: hidden;
            background: #fff;
        }
        #acc .acc-content
        {
            width: 100%;
            padding: 15px;
            border: 1px solid #9ac1c9;
            border-top: none;
            background: #fff;
        }
    </style>
    <script type="text/javascript">
        function ChekBox1Check() {
            var ddl1 = document.getElementById("<%= chkSite.ClientID %>");
            var chkAllSite = document.getElementById("<%= chkAllSite.ClientID %>");
            var Lengtha = ddl1.getElementsByTagName("input").length;
            var allInput = ddl1.getElementsByTagName("input");
            var checkedcount = 0;
            for (var i = 0; i <= Lengtha - 1; i++) {
                if (allInput[i].checked == true) {
                    checkedcount = checkedcount + 1;
                }
                else {
                    break;
                }
            }
            if (checkedcount == Lengtha) { chkAllSite.checked = true; }
            else { chkAllSite.checked = false; }
        }
        function CkeckAll() {
            var ddl1 = document.getElementById("<%= chkSite.ClientID %>");
            var chkAllSite = document.getElementById("<%= chkAllSite.ClientID %>");
            var Lengtha = ddl1.getElementsByTagName("input").length;
            var allInput = ddl1.getElementsByTagName("input");
            for (var i = 0; i <= Lengtha - 1; i++) {
                allInput[i].checked = chkAllSite.checked;
            }
        }
        function checkboxcheck2() {
            var d2 = document.getElementById("<%= chkEngineList.ClientID %>");
            var chkallEng = document.getElementById("<%= chkAll.ClientID %>");
            var l2 = d2.getElementsByTagName("input").length;
            var alI = d2.getElementsByTagName("input");
            var chkcnt = 0;
            for (var j = 0; j <= l2 - 1; j++) {
                if (alI[j].checked == true) {
                    chkcnt = chkcnt + 1;
                }
                else {
                    break;
                }
            }
            if (chkcnt == l2) { chkallEng.checked = true; }
            else { chkallEng.checked = false; }
        }
        function checkallengine() {
            var d2 = document.getElementById("<%= chkEngineList.ClientID%>");
            var chkallEng = document.getElementById("<%= chkAll.ClientID %>");
            var l2 = d2.getElementsByTagName("input").length;
            var alI = d2.getElementsByTagName("input");
            for (var j = 0; j <= l2 - 1; j++) {
                alI[j].checked = chkallEng.checked;
            }
        }
        function checkboxcheck3() {
            var d2 = document.getElementById("<%= chkProductLst.ClientID %>");
            var chkallEng = document.getElementById("<%= chkAllPrd.ClientID %>");
            var l2 = d2.getElementsByTagName("input").length;
            var alI = d2.getElementsByTagName("input");
            var chkcnt = 0;
            for (var j = 0; j <= l2 - 1; j++) {
                if (alI[j].checked == true) {
                    chkcnt = chkcnt + 1;
                }
                else {
                    break;
                }
            }
            if (chkcnt == l2) { chkallEng.checked = true; }
            else { chkallEng.checked = false; }
        }
        function checkallProduct() {
            var d2 = document.getElementById("<%= chkProductLst.ClientID%>");
            var chkallEng = document.getElementById("<%= chkAllPrd.ClientID %>");
            var l2 = d2.getElementsByTagName("input").length;
            var alI = d2.getElementsByTagName("input");
            for (var j = 0; j <= l2 - 1; j++) {
                alI[j].checked = chkallEng.checked;
            }
        }
        function selectenddate() {
            var endDate = document.getElementById("frmdate");
            var enddateinput = endDate.getgetElementsByTagName('input');
            for (i = 0; i < enddateinput.length; i++) {
                if (enddateinput[i].type == "text") {
                    enddateinput[i].value = '';
                }
            }
        }
        function div1(obj) {
            var hfCount = document.getElementById("hfCount");
            hfCount.value = null;
            if (obj.options[obj.selectedIndex].text == 'Select All') {
                var hfCount = document.getElementById("hfCount");
                var opt = document.getElementById("ddlSite");
                for (i = 0; i < opt.options.length; i++) {
                    if (opt.options[i].selected) { }
                    else {
                        if (hfCount.value == "") {
                            hfCount.value = obj.options[i].text;
                        }
                        else {
                            hfCount.value = hfCount.value + "," + obj.options[i].text;
                        }
                    }
                }
            }
            else {
                hfCount.value = obj.options[obj.selectedIndex].text;
            }
        }
        function readCheckBoxList() {
            var hfEng = document.getElementById("hfEng");
            hfEng.value = getCheckBoxListItemsChecked("<%=chkEngineList.ClientID %>");
        }
        function getCheckBoxListItemsChecked(elementId) {
            var elementRef = document.getElementById(elementId);
            var checkBoxArray = elementRef.getElementsByTagName('input');
            var checkedValues = '';
            for (var i = 0; i < checkBoxArray.length; i++) {
                var checkBoxRef = checkBoxArray[i];
                if (checkBoxRef.checked == true) {
                    var labelArray = checkBoxRef.parentNode.getElementsByTagName('label');

                    if (labelArray.length > 0) {
                        if (checkedValues.length > 0)
                            checkedValues += ', ';

                        checkedValues += labelArray[0].innerHTML;
                    }
                }
            }

            return checkedValues;
        }        
    </script>
    <asp:ValidationSummary ID="vsDate" runat="server" ShowMessageBox="true" ShowSummary="false"
        ValidationGroup="Prd" />
    <h3 style="color: #234089; margin: 10px; border-bottom: solid 1px #234089; text-align: left">
        Enginewise Dashboard
    </h3>
    <table>
        <tr>
            <td>
                <table>
                    <tr>
                        <td>
                            <table>
                                <tr>
                                    <td>
                                        From Date
                                    </td>
                                    <td>
                                        <ucd:UC_Date ID="frmdate" runat="server" datechange="selectenddate()" ClientIDMode="Static" />
                                    </td>
                                    <td>
                                        To Date
                                    </td>
                                    <td>
                                        <ucd:UC_Date ID="todate" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 100%; height: 30px; border: 1px solid #9ac1c9; padding: 6px,6px,8px;
                            margin-bottom: 5px;">
                            <div>
                                <h3 style="margin-left: 5px; color: #033;">
                                    Site List
                                </h3>
                            </div>
                            <div class="acc-section">
                                <div class="acc-content" style="margin: 0px 0px 5px 5px;">
                                    <asp:DropDownList ID="ddlSite" runat="server" DataValueField="ID" DataTextField="Territory"
                                        Width="200px" OnSelectedIndexChanged="ddlSite_SelectedIndexChanged" AutoPostBack="true"
                                        ValidationGroup="Savet" ClientIDMode="Static" onchange="div1(this);">
                                    </asp:DropDownList>
                                    <asp:CheckBox ID="chkAllSite" runat="server" Text="Select All" onclick="CkeckAll()"
                                        Visible="false" />
                                    <asp:CheckBoxList ID="chkSite" runat="server" RepeatDirection="Horizontal" onclick="CheckBox1Check()"
                                        Visible="false" DataValueField="ID" DataTextField="Territory" RepeatColumns="5">
                                    </asp:CheckBoxList>
                                    <asp:Button ID="btnNext" runat="server" Text="Next" OnClick="btnNext_Click" Visible="false" />
                                </div>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <ul class="acc" id="acc">
                                <li>
                                    <h3>
                                        Engine List
                                    </h3>
                                    <div class="acc-section">
                                        <div class="acc-content">
                                            <asp:CheckBox ID="chkAll" runat="server" Text="Select All" onclick="checkallengine()" /><hr />
                                            <br />
                                            <asp:CheckBoxList ID="chkEngineList" runat="server" RepeatDirection="Horizontal"
                                                onclick="checkboxCheck2()" DataValueField="EngineSerial" DataTextField="EngineSerial"
                                                RepeatColumns="10">
                                            </asp:CheckBoxList>
                                            <br />
                                            <asp:Button ID="btnSubmit" runat="server" Text="Select Product" OnClick="btnSubmit_Click"
                                                onblur="readCheckBoxList();" ValidationGroup="Save" CausesValidation="false" />
                                        </div>
                                    </div>
                                </li>
                            </ul>
                            <script type="text/javascript" src="../MasterPage/JavaScripts/Script.js"></script>
                            <script type="text/javascript">

                                var parentAccordion = new TINY.accordion.slider("parentAccordion");
                                parentAccordion.init("acc", "h3", 0, 1);                             

                            </script>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <ul class="acc" id="acc">
                                <li>
                                    <h3>
                                        Product List
                                    </h3>
                                    <div class="acc-section">
                                        <div class="acc-content">
                                            <asp:CheckBox ID="chkAllPrd" runat="server" Text="Select All" onclick="checkallProduct()" />
                                            <hr />
                                            <br />
                                            <asp:CheckBoxList ID="chkProductLst" runat="server" RepeatDirection="Horizontal"
                                                onclick="checkboxcheck3()" DataValueField="Name" DataTextField="Name" RepeatColumns="10">
                                            </asp:CheckBoxList>
                                            <br />
                                            <asp:Button ID="btn_SubmitFinal" runat="server" Text="Submit" OnClick="btnSubmitFinal_Click"
                                                CausesValidation="false" />
                                        </div>
                                    </div>
                                </li>
                            </ul>
                            <script type="text/javascript" src="../MasterPage/JavaScripts/Script.js"></script>
                            <script type="text/javascript">

                                var parentAccordion = new TINY.accordion.slider("parentAccordion");
                                parentAccordion.init("acc", "h3", 0, 1);                             

                            </script>
                        </td>
                    </tr>
                    <tr>
                        <asp:Label ID="lbldisplay" runat="server"></asp:Label>
                    </tr>
                    <uc1:DashBoard ID="Dashboard1" runat="server" />
                </table>
            </td>
        </tr>
        <tr>
            <td>
            </td>
        </tr>
    </table>
    <asp:HiddenField ID="hfCount" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hfEng" runat="server" ClientIDMode="Static" />
</asp:Content>

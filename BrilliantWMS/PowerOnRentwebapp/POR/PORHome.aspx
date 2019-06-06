<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/CRM.Master" AutoEventWireup="true"
    CodeBehind="PORHome.aspx.cs" Inherits="BrilliantWMS.POR.Home" %>

<%@ Register Src="../Approval/UC_Approval.ascx" TagName="UC_Approval" TagPrefix="uc5" %>
<%@ Register Src="../DashBoard/DashBoard.ascx" TagName="DashBoard" TagPrefix="uc1" %>
<%@ Register Assembly="obout_Grid_NET" Namespace="Obout.Grid" TagPrefix="obout" %>
<%@ Register Assembly="Obout.Ajax.UI" Namespace="Obout.Ajax.UI.TreeView" TagPrefix="obout" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHolder_FormHeader" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPHolder_Form" runat="server">
    <link href="../CSS/basic.css" rel="stylesheet" type="text/css" />
    <script src="../js/basic.js" type="text/javascript"></script>
    <script src="../js/jquery.js" type="text/javascript"></script>
    <script src="../js/jquery.simplemodal.js" type="text/javascript"></script>
    <div id="divLeft" class="leftDiv_open1">
        <div id="divLeftChild" class="expand-image" onclick="expandLeftdiv()">
        </div>
        <div id="divLeftData" style="width: 100%;">
            <a class="aparent">Sites</a>
            <br />
            <a id="at1" class="achild" onclick="LinkValueAssign(this, 'CNS');">- CNS</a>
            <br />
            <a id="at2" class="achild" onclick="LinkValueAssign(this, 'Marmal');">- Marmal</a>
            <br />
            <a id="at3" class="achild" onclick="LinkValueAssign(this, 'Konduz');">- Konduz</a>
            <br />
            <a id="at4" class="achild" onclick="LinkValueAssign(this, 'TK');">- TK</a>
        </div>
        <input id="hndLinkValue" type="hidden" value="" runat="server" clientidmode="Static" />
    </div>
    <div id="divMainright" style="width: 84%; float: right; overflow: auto;">
        <center>
            <div>
                <obout:Grid ID="GVInboxPOR" runat="server" Width="100%" AllowColumnReordering="true"
                    AllowGrouping="false" AutoGenerateColumns="false" PageSize="3">
                    <Columns>
                        <obout:CheckBoxSelectColumn AllowSorting="true" ControlType="Obout" Width="3%" Align="center"
                            HeaderAlign="center">
                        </obout:CheckBoxSelectColumn>
                        <obout:Column DataField="RequestedDate" HeaderText="Requested Date" Width="8%" DataFormatString="{0:dd-MMM-yy}">
                        </obout:Column>
                        <obout:Column DataField="TransID" HeaderText="" Width="0%" Visible="false">
                        </obout:Column>
                        <obout:Column DataField="Title" Visible="false" Width="0%">
                        </obout:Column>
                        <obout:Column DataField="IsRead" Visible="false" Width="0%">
                        </obout:Column>
                        <obout:Column DataField="Particulars" HeaderText="Particular" Width="30%">
                            <TemplateSettings TemplateId="GTDetails" />
                        </obout:Column>
                        <obout:Column DataField="Site" HeaderText="Site" Wrap="true" Width="5%">
                        </obout:Column>
                        <obout:Column DataField="RequestedBy" HeaderText="Requested By" Wrap="true" Width="10%">
                        </obout:Column>
                        <obout:Column DataField="ApprovalStatus" HeaderText="Approval Status" Wrap="true"
                            Width="20%">
                        </obout:Column>
                    </Columns>
                    <Templates>
                        <obout:GridTemplate runat="server" ID="GTDetails">
                            <Template>
                                <table>
                                    <tr>
                                        <td>
                                            <a class='<%# Container.DataItem["IsRead"] %>' style="max-width: 150px">
                                                <%#  Container.DataItem["Particulars"]  %></a>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <a><a class="details" style="max-width: 150px">Title :
                                                <%# Container.DataItem["Title"]  %></a> </a>
                                        </td>
                                    </tr>
                                </table>
                            </Template>
                        </obout:GridTemplate>
                    </Templates>
                </obout:Grid>
            </div>
        </center>
    </div>
    <uc1:DashBoard ID="DashBoard1" runat="server" />
    <uc1:DashBoard ID="DashBoard2" runat="server" />
    <style type="text/css">
        .details
        {
            color: Gray;
        }
        .UnRead
        {
            color: Black;
            font-weight: bold;
        }
        .Read
        {
            color: Black;
            font-weight: normal;
        }
        .UnRead:hover, Read:hover
        {
            cursor: pointer;
        }
        
        .aparent
        {
            font-weight: bold;
            margin: 2px 0px 2px 3px;
            color: Black;
            font-size: 17px;
        }
        
        .achild
        {
            font-weight: normal;
            margin: 3px 0px 2px 10px;
            word-wrap: break-word;
            font-size: 13px;
        }
        
        .achildselected
        {
            color: Black;
            font-weight: bold;
            margin: 3px 0px 2px 10px;
            font-size: 14px;
        }
        
        .achild:hover
        {
            color: Black;
            cursor: pointer;
        }
        .expand-image
        {
            float: right;
            top: 0px;
            margin-right: -7px;
            margin-top: 15px;
            width: 7px;
            height: 23px;
            background: url('../App_Themes/Blue/img/sliderOpen.png') no-repeat;
        }
        
        .close-image
        {
            float: right;
            top: 0px;
            margin-right: -7px;
            margin-top: 15px;
            width: 7px;
            height: 23px;
            background: url('../App_Themes/Blue/img/sliderClose.png') no-repeat;
        }
        
        .expand-image:hover, .close-image:hover
        {
            cursor: pointer;
        }
        
        .leftDiv_open1
        {
            position: relative;
            width: 15%;
            border-right: solid 2px silver;
            float: left;
        }
    </style>
    <script type="text/javascript">

        var expandcount = 1;
        function expandLeftdiv() {
            expandcount += 1;
            switch (expandcount) {
                case 1:
                    document.getElementById("divLeftData").style.display = "block";
                    document.getElementById("divLeft").style.width = "15%";
                    document.getElementById("divMainright").style.width = "84%";
                    break;
                case 2:
                    document.getElementById("divLeft").style.width = "20%";
                    document.getElementById("divMainright").style.width = "79%";
                    break;
                case 3:
                    document.getElementById("divLeft").style.width = "25%";
                    document.getElementById("divLeftChild").className = "close-image";
                    document.getElementById("divMainright").style.width = "74%";
                    break;
                case 4:
                    document.getElementById("divLeftData").style.display = "none";
                    document.getElementById("divLeft").style.width = "0%";
                    document.getElementById("divLeftChild").className = "expand-image";
                    document.getElementById("divMainright").style.width = "99%";
                    expandcount = 0;
                    break;
            }

        }
        var isFullScreen = false;
        var callcount = 0;

        window.onresize = chnageHeight1;
        chnageHeight1();
        function getPageDetails() {
            var scnWid, scnHei;
            if (self.innerHeight) // all except Explorer
            {
                scnWid = self.innerWidth;
                scnHei = self.innerHeight;
            }
            else if (document.documentElement && document.documentElement.clientHeight)
            // Explorer 6 Strict Mode
            {
                scnWid = document.documentElement.clientWidth;
                scnHei = document.documentElement.clientHeight;
            }
            else if (document.body) // other Explorers
            {
                scnWid = document.body.clientWidth;
                scnHei = document.body.clientHeight;
            }
            debugger;
            document.getElementById('divMainright').style.height = ((scnHei / 100) * 33).toString() + 'px';
            document.getElementById('divLeftData').style.height = ((scnHei / 100) * 33).toString() + 'px';

            document.getElementById("divIFrame").style.height = ((scnHei / 100) * 92).toString() + 'px';
        }

        function chnageHeight1() {
            getPageDetails();
        }

        function LinkValueAssign(invoker, linkValue) {
            for (var i = 1; i <= 4; i++) {
                document.getElementById('at' + i.toString()).className = 'achild';
            }
            document.getElementById(invoker.id).className = 'achildselected';
            var v = document.getElementById('hndLinkValue');
            v.value = linkValue;
            GVInboxPOR.refresh();
        }
    </script>
</asp:Content>

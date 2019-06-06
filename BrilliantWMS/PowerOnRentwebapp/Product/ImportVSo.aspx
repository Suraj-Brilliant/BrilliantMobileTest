<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/CRM.Master" AutoEventWireup="true"
    CodeBehind="ImportVSo.aspx.cs" Inherits="BrilliantWMS.POR.ImportVSo" Theme="Blue" %>

<%@ Register Assembly="obout_Grid_NET" Namespace="Obout.Grid" TagPrefix="obout" %>
<%@ Register Src="../CommonControls/UCFormHeader.ascx" TagName="UCFormHeader" TagPrefix="uc2" %>
<%@ Register Src="../CommonControls/Toolbar.ascx" TagName="Toolbar" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHolder_FormHeader" runat="server">
    <%--<uc2:Toolbar ID="Toolbar1" runat="server" /--%>
    <uc2:UCFormHeader ID="UCFormHeader1" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPHolder_Form" runat="server">
   <%-- <div class="divDetailExpandPopUpOff1" id="divPopUp">
        <center>
            <div class="popupClose1" onclick="CloseShowReport()">
            </div>
            <div class="divDetailExpand" id="div2">
                <iframe runat="server" id="iframeCpnRpt" clientidmode="Static" src="#" width="100%"
                    style="border: none; height: 500px;"></iframe>
            </div>
        </center>
    </div>--%>
    <span id="imgProcessing" style="display: none;">Please wait... </span>
    <div id="divLoading" style="height: 110%; width: 100%; display: none; top: 0; left: 0;"
        class="modal" runat="server" clientidmode="Static">
    </div>
    <div class="divHead" id="divRequestHead">
        <h4>
            <asp:Label ID="lblHeading" runat="server" Text="Import Images"></asp:Label>
        </h4>
    </div>
    <div class="divHead" id="div1">
        <table>
            <tr>
                <td>
                    <center>
                        <table align="center">
                            <tr>
                                <td align="center">
                                    <span class="cartStepHolder"><span class="cartStepTitle "><span class="divCartSymbol">
                                        <asp:Label ID="lbl1" runat="server" Text="1" /></span><asp:Label ID="lblstep1" runat="server"
                                            Text="Upload File" /></span><span class="cartStepTitle cartStepCurrentTitle"><span
                                                class="divCartSymbol divCartCurrentSymbol"><asp:Label ID="lbl2" runat="server" Text="2" /></span><asp:Label
                                                    ID="lblstep2" runat="server" Text="Data Validation & Verification" /></span><%--<span class="cartStepTitle"><span
                                            class="divCartSymbol"><asp:Label ID="lbl3" runat="server" Text="3"/></span><asp:Label ID="lblstep3" runat="server" Text="Finished"/></span></span>--%>
                                </td>
                            </tr>
                        </table>
                    </center>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <hr style="border-color: #F5DEB3" />
                </td>
            </tr>
            <tr>
                <td class="style3" style="height: 5px;">
                    <%--<span>
                        <h4>
                            Upload filled Purchase Order Excel template. 
                        </h4>
                    </span>--%>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:Label ID="lblbackMessage" runat="server" Style="float: inherit;" ForeColor="Red"
                        Font-Size="Large" Font-Bold="true" Text=""></asp:Label>
                    <asp:Label ID="lblOkMessage" runat="server" Style="float: inherit;" ForeColor="Blue"
                        Font-Size="Large" Font-Bold="true" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <table>
                        <tr>
                            <td align="left">
                                <asp:Label ID="lblheadingtext" Font-Bold="true" Font-Size="18px" ForeColor="#483d8b"
                                    runat="server" Text="Import Status:" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td align="center" colspan="2">
                    <table align="center" class="tblcls" style="padding-left: 104px; padding-top: 12px;
                        padding-bottom: 5px; padding-right: 75px;">
                        <tr>
                            <td align="left">
                                <asp:Label ID="lbltotalnumber" class="cartStepTitle1" runat="server" Text="Total Number Of Image For Import"></asp:Label> :
                                <asp:Label ID="lbltotalimagetxt" class="cartStepTitle1 cartStepCurrentTitle" ForeColor="Red"
                                    runat="server" Text=""></asp:Label>
                            </td>
                            <td align="left">
                                <asp:Label ID="lblsuccessful" class="cartStepTitle1" runat="server" Text="Image Imported Successfully"></asp:Label> :
                                <asp:Label ID="lblsuccessfultxt" class="cartStepTitle1 cartStepCurrentTitle" ForeColor="Red"
                                    runat="server" Text=""></asp:Label>
                            </td>
                            <td align="left">
                                <asp:Label ID="lblfailedimages" class="cartStepTitle1" runat="server" Text="Image Import Failed For"></asp:Label> :
                                <asp:Label ID="lblfailedtxt" class="cartStepTitle1 cartStepCurrentTitle" ForeColor="Red"
                                    runat="server" Text=""></asp:Label>
                            </td>
                             </tr>
                            <tr>
                                <td colspan="6" align="right">
                                    <input type="button" id="btnshowreport" runat="server" value="Show Failed Images" onclick="generateReport()" />
                                    <input type="button" id="btnfinish" runat="server" value=" Finished " onclick="Finish()" />
                                </td>
                            </tr>
                       
                    </table>
                </td>
            </tr>
            <%-- <tr>
                <td colspan="2">
                  <obout:Grid ID="grdImportView" runat="server" AllowGrouping="true" 
                        Serialize="false" CallbackMode="true" AllowRecordSelection="false"
                            AllowMultiRecordSelection="false" AllowColumnReordering="true" AllowFiltering="true"
                            Width="100%" PageSize="6" AllowAddingRecords="False"  onrowdatabound="grdImportView_RowDataBound"
                        AutoGenerateColumns="False">
                             <Columns>
                              <obout:Column DataField="Sequence" HeaderText="Sr.No." AllowEdit="false" Width="2%"
                                    Align="center" HeaderAlign="center">
                                    <TemplateSettings TemplateId="grdSrNo" />
                                </obout:Column>
                             
                              
                              <obout:Column DataField="ProductCode" HeaderText="Company" HeaderAlign="left" Align="left"
                                    Width="7%">
                              </obout:Column>
                              <obout:Column DataField="CustomerName" HeaderText="Department" HeaderAlign="left" Align="left"
                                    Width="7%">
                              </obout:Column> 
                              <obout:Column DataField="SoNumber" HeaderText="SKU" HeaderAlign="left" Align="left"
                                    Width="5%">
                              </obout:Column>
                               <obout:Column DataField="SalesOrderDate" HeaderText="Image" HeaderAlign="left" Align="left"
                                    Width="7%">
                                </obout:Column>                       
                                                                              
                            </Columns>
                            <Templates>
                             <obout:GridTemplate runat="server" ID="grdSrNo">
                                    <Template>
                                        <%#Container.DataRecordIndex+1 %>
                                    </Template>
                                </obout:GridTemplate>
                            </Templates>
                              
                </obout:Grid>
          

            </td>
            </tr>--%>
            <tr>
                <td class="style3">
                </td>
            </tr>
            <tr>
                <td>
                </td>
            </tr>
            <tr>
                <td>
                </td>
            </tr>
            <tr>
                <br />
                <table align="right">
                    <tr align="right">
                        <td>
                            <%--<a href="ImportFSo.aspx"><input type="button" id="btnfinished" value="   Next   " runat="server"  style="font-weight: bold;
                        float: left;" class="btnCommonStyle"/></a>--%>
                            &nbsp;<%--<a href="ImportDSo.aspx"> <asp:Button ID="btnUploadPo" runat="server" Text="   Back   "   /></a>--%>
                        </td>
                    </tr>
                </table>
            </tr>
        </table>
    </div>
    <script type="text/javascript">
        function Finish() {
            window.open("../Product/ImportDSo.aspx","_Self");
        }
       
        function CloseShowReport() {
            divPopUp.className = "divDetailExpandPopUpOff";
        }
        function generateReport() {
            PageMethods.WmGetCouponReport("Coupon", OnSuccessCouponReport, null);
        }
           function OnSuccessCouponReport(result) {
            if (parseInt(result) > 0) {
                ShowReportOn();
            }
            else {
                showAlert("Data Not Available... ", "Error", "#");
                LoadingOff();
                ShowReportOff();
            }
        }
        function ShowReportOn() {
            document.getElementById("iframePORRpt").src = "../POR/Reports/ReportViewer.aspx";
            divPopUp.className = "divDetailExpandPopUpOn";
        }

        function ShowReportOff() {
            divPopUp.className = "divDetailExpandPopUpOff";
            LoadingOff();
        }
    
    </script>
    <style type="text/css">
        .popupClose
        {
            background: url(../App_Themes/Blue/img/icon_close.png) no-repeat;
            height: 32px;
            width: 32px;
            float: right;
            margin-top: -30px;
            margin-right: -25px;
        }
        .popupClose:hover
        {
            cursor: pointer;
        }
        .divDetailExpandPopUpOff
        {
            display: none;
        }
        .divDetailExpandPopUpOn
        {
            border: solid 3px gray;
            width: 65%;
            height: 98%;
            padding: 10px;
            background-color: #FFFFFF;
            margin-top: 50px;
            top: 1%;
            left: 3%;
            position: absolute;
            z-index: 99999;
        }
    </style>
    <style type="text/css">
        /*Grid css*/
        .excel-textbox
        {
            background-color: transparent;
            border: 0px;
            padding: 0px;
            outline: 0;
            font: inherit;
            width: 91%;
            padding-top: 4px;
            padding-left: 2px;
            padding-bottom: 4px;
            text-align: left;
        }
        .excel-textbox-focused
        {
            background-color: #FFFFFF;
            border: 0px;
            padding: 0px;
            outline: 0;
            font: inherit;
            width: 91%;
            padding-top: 4px;
            padding-right: 2px;
            padding-bottom: 4px;
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
            font-size: 13px;
            font-weight: bold;
            margin-top: -22px;
            margin-left: -5px;
            position: absolute;
            background-color: White;
            padding: 0px 2px 0px 2px;
        }
        .divHead
        {
            border: solid 2px #F5DEB3;
            width: 100%;
            text-align: left;
        }
        .divHead h4
        {
            /*color: #33CCFF;*/
            color: #483D8B;
            margin: 3px 3px 3px 3px;
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
            font-size: 18px;
            color: #cccccc;
            padding: 0px 50px 40px 0px;
            display: inline-block;
            padding-right: 60px;
            font-weight: bold;
        }
        
        .cartStepTitle1
        {
            font-size: 17px;
            color: #848484;
            padding: 0px 50px 40px 0px;
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
        
        
        #btnNext
        {
            width: 69px;
        }
        #btnCancle
        {
            width: 69px;
        }
        
        
        .style2
        {
            height: 47px;
            width: 1338px;
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
        .style3
        {
            width: 1338px;
        }
    </style>
</asp:Content>

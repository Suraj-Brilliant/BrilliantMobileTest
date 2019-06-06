<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Approval.aspx.cs" Inherits="BrilliantWMS.PowerOnRent.Approval"
    Theme="Blue" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server" style="background-color: White;">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true" />
    <div class="divDetailExpand" id="divApprovalDetail" runat="server" clientidmode="Static">
    <center>
   <%-- <span id="imgProcessing" style="display: none;">Please wait... </span>--%>
        <div id="divLoading" style=" display: none; top: 10px; left: 50%; height: 30%; width: 20%;"
            class="modal" runat="server" clientidmode="Static"> <%----%>
            <center>
             <img src="../App_Themes/Blue/img/ajax-loader.gif" alt="" style="top: 0%; margin-top: 1%" />
                <br />
                <br />
                <b>Please Wait...</b>
            </center>
        </div>
        <table class="tableForm">
           <%-- <tr style="visibility:hidden;">
                <td style="font-size: 13px; font-weight: bold;">
                   <asp:Label ID="lblapprovalop" runat="server" Text="Operation Approval"></asp:Label>  * :
                </td>
                <td style="vertical-align: top; font-size: 13px; font-weight: bold; text-align: left;">
                    <label class="label_check" id="lblApproved" for="CheckBoxApproved">
                        <asp:CheckBox ID="CheckBoxApproved" runat="server" ClientIDMode="Static"  onclick="OnlyOneCheckedA('CheckBoxApproved','CheckBoxRejected','CheckBoxRevison');" />
                        <asp:Label ID="lblApproved" runat="server" Text="Approve"></asp:Label>
                    </label>
                    <label class="label_check" id="lblRejected"for="CheckBoxRejected">
                        <asp:CheckBox ID="CheckBoxRejected" runat="server" ClientIDMode="Static"  onclick="OnlyOneCheckedR('CheckBoxApproved','CheckBoxRejected','CheckBoxRevison');" />
                         <asp:Label ID="lblRejected" runat="server" Text="Reject"></asp:Label>
                        </label>
                    <label class="label_check" id="lblRevision" for="CheckBoxRevision">
                        <asp:CheckBox ID="CheckBoxRevison" runat="server" ClientIDMode="Static"  onclick="OnlyOneCheckedRV('CheckBoxApproved','CheckBoxRejected','CheckBoxRevison');" />
                        <asp:Label ID="lblRevison" runat="server" Text="Approve with Revision"></asp:Label>
                       </label>
                </td>
            </tr>--%>
           <%-- <tr style="visibility:hidden;">
                <td style="text-align: right;">
                    <asp:Label ID="lblDate" runat="server" Text="Date"></asp:Label> :
                </td>
                <td style="text-align: left;">
                    <asp:Label runat="server" ID="lblApprovalDate"></asp:Label>
                </td>
            </tr>--%>
            <tr>
                <td id="tdInvoiceNo" style="text-align:right;">
                    <asp:Label ID="lblInvoiceNo" runat="server" Text="Invoice No."></asp:Label> :
                </td>
                <td style="text-align:left;">
                    <asp:TextBox ID="txtInvoiceNo" runat="server" ClientIDMode="Static" Width="200px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td id="tdApprovalRemark" style="text-align: right;">
                    <asp:Label ID="lblApprovRemark" runat="server" Text="Approver Comments"></asp:Label> :
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtApprovalRemark" onkeyup="TextBox_KeyUp(this,'CharactersCountertxtApprovalRemark','200');"
                        ClientIDMode="Static" Width="400px" TextMode="MultiLine">
                    </asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td style="text-align: left;">
                    <span class="watermark"><span id="CharactersCountertxtApprovalRemark">200</span> / 200
                    </span>
                    <input type="button" id="btnSaveApproval" runat="server" value="Submit" style="float: right;" onclick="jsSaveApproval()"/>
                </td>
            </tr>
        </table>
        </center>
    </div>
    <script type="text/javascript">
        onload();
        function onload() {
            document.getElementById("btnSaveApproval").style.visibility = "visible";
        }
        function getParameterByName(name) {
            name = name.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
            var regexS = "[\\?&]" + name + "=([^&#]*)";
            var regex = new RegExp(regexS);
            var results = regex.exec(window.location.search);
            if (results == null)
                return "";
            else
                return decodeURIComponent(results[1].replace(/\+/g, " "));
        }

        /*Approval JS*/
        /*Approval checkbox Only one checked Approved or Rejected*/
        function OnlyOneCheckedA(chkA, chkR,chkRV) {
            var findtd = document.getElementById('tdApprovalRemark');

            if (document.getElementById(chkA).checked == true) {
                findtd.innerHTML = "Remark / Reason : ";
                document.getElementById('txtApprovalRemark').accessKey = "";
                document.getElementById(chkR).checked = false;
                document.getElementById(chkRV).checked = false;
                lblRejected.className = "label_check c_off";
            }
        }

        function OnlyOneCheckedR(chkA, chkR, chkRV) {
            var findtd = document.getElementById('tdApprovalRemark');

            if (document.getElementById(chkR).checked == true) {
                findtd.innerHTML = "Remark / Reason * :";
                document.getElementById('txtApprovalRemark').accessKey = "1";
                document.getElementById(chkA).checked = false;
                document.getElementById(chkRV).checked = false;
                lblApproved.className = "label_check c_off";
            }
        }

        function OnlyOneCheckedRV(chkA, chkR, chkRV) {
            var findtd = document.getElementById('tdApprovalRemark');

            if (document.getElementById(chkR).checked == true) {
                findtd.innerHTML = "Remark / Reason * :";
                document.getElementById('txtApprovalRemark').accessKey = "1";
                document.getElementById(chkA).checked = false;
                document.getElementById(chkR).checked = false;
                lblApproved.className = "label_check c_off";
            }
        }

        function jsSaveApproval() {
//            if (document.getElementById('CheckBoxApproved').checked == false && document.getElementById('CheckBoxRejected').checked == false && document.getElementById('CheckBoxRevision')) {
//                showAlert("Approval status should not be left blank [ Approved or Rejected ]", "Error", "#");
//            }
//            else if (document.getElementById('CheckBoxRejected').checked == true && document.getElementById('txtApprovalRemark').value == "") {
//                showAlert("Fill Remark", "Error", "#");
//                document.getElementById('txtApprovalRemark').focus();
//            }
//            if (document.getElementById('txtApprovalRemark').value == "") {
//                showAlert("Approval Comments", "Error", "#");
//                document.getElementById('txtApprovalRemark').focus();
//            }
//            else {                
            // var APID = getParameterByName("APID").toString();
            document.getElementById("btnSaveApproval").style.visibility = "hidden";
                var ST=getParameterByName("ST").toString();
                var REQID = getParameterByName("REQID").toString();
                var APL = getParameterByName("APL").toString();

                //PageMethods.WMSaveApproval(REQID, document.getElementById('CheckBoxApproved').checked, document.getElementById('CheckBoxRejected').checked, document.getElementById('CheckBoxRevison').checked, document.getElementById('txtApprovalRemark').value, jsSaveApprovalOnSuccess, jsSaveApprovalOnFail);
                PageMethods.WMSaveApproval(REQID, ST, APL, document.getElementById('txtApprovalRemark').value, document.getElementById('txtInvoiceNo').value, jsSaveApprovalOnSuccess, jsSaveApprovalOnFail);
//            }
        }

        function jsSaveApprovalOnSuccess(result) {
            if (result.toString().toLowerCase() == 'true') {
                // showAlert("Approval status has been saved successfully", "info", "../PowerOnRent/Default.aspx?invoker=Request");
                LoadingOn(true);
                window.opener.gvApprovalRemark.refresh();
                self.close();
            }
            else if (result.toString().toLowerCase() == 'truerev') {
                self.close();
            }
            else {
                showAlert("Some error occurred", "error", "#");
            }
        }
        function jsSaveApprovalOnFail() { }

        function checkLength(sender, maxLength) {
            if (sender.value.length > parseInt(maxLength)) {
                sender.value = sender.value.substr(0, parseInt(maxLength));
                showAlert("Character limit is exceed", 'error', '#');
            }
        }

        function TextBox_KeyUp(sender, conunterspan, maxLength) {
            checkLength(sender, maxLength);

            var counterId = conunterspan;
            if (counterId != "") document.getElementById(counterId).innerHTML = parseInt(maxLength) - sender.value.length;
        }

        var myTimer;
        function showAlert(msg, msytype, formpath) {
            var newdiv = document.createElement('div');
            newdiv.id = "divAlerts";
            newdiv.className = "alertBox0";

            var newdiv2 = document.createElement('div');
            msytype = msytype.toString().toLowerCase();
            switch (msytype) {
                case "info":
                    newdiv2.style.borderTopColor = "green";
                    break;
                case "error":
                    newdiv2.style.borderTopColor = "red";
                    break;
                case "p": /*Process*/
                    newdiv2.style.borderTopColor = "navy";
                    break;
                case "":
                    newdiv2.style.borderTopColor = "orange";
                    break;
            }


            var span1 = document.createElement('span');
            span1.id = 'alertmsg';
            newdiv2.appendChild(span1);
            newdiv.appendChild(newdiv2);
            //var strdiv = "<div id='divAlerts' class='alertBox0'><div><span id='alertmsg'></span></div></div>"
            document.body.appendChild(newdiv);
            document.getElementById('alertmsg').innerHTML = msg;
            document.getElementById('divAlerts').className = "alertBox1";
            myTimer = self.setInterval(function () { msgclock(formpath) }, 3500);
        }
        function msgclock(formpath) {
            document.body.removeChild(document.getElementById("divAlerts"));
            if (formpath != '#' && formpath != '') {
                window.open(formpath, '_self', '');
                LoadingOff();
            }
            clearInterval(myTimer);
        }

        /*Show Loading div*/
        function LoadingOn() {
            document.getElementById("divLoading").style.display = "block";
            var imgProcessing = document.getElementById("imgProcessing");
            if (imgProcessing != null) { imgProcessing.style.display = ""; }
        }

        function LoadingOn(ShowWaitMsg) {
            document.getElementById("divLoading").style.display = "block";
            var imgProcessing = document.getElementById("imgProcessing");
            if (imgProcessing != null) {
                if (ShowWaitMsg == true) { imgProcessing.style.display = ""; }
                if (ShowWaitMsg == false) { imgProcessing.style.display = "none"; }
            }
        }

        function LoadingText(val) {
            if (val == true) { document.getElementById("pupUpLoading").style.display = ""; }
            else { document.getElementById("pupUpLoading").style.display = "none"; }
        }

        function LoadingOff() {
            if (document.getElementById("divLoading") != null) {
                document.getElementById("divLoading").style.display = "none";
            }
            var imgProcessing = document.getElementById("imgProcessing");
            if (imgProcessing != null) { imgProcessing.style.display = "none"; }
        }
        /*End Show Loading div*/

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
    </form>
</body>
</html>

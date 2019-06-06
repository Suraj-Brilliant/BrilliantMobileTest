<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Correspondance.aspx.cs"
    Inherits="BrilliantWMS.PowerOnRent.Correspondance" Theme="Blue" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="Obout.Ajax.UI" Namespace="Obout.Ajax.UI.HTMLEditor" TagPrefix="obout" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
<link href="http://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.0.3/css/bootstrap.min.css"
    rel="stylesheet" type="text/css" />
<script type="text/javascript" src="http://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.0.3/js/bootstrap.min.js"></script>
<link href="http://cdn.rawgit.com/davidstutz/bootstrap-multiselect/master/dist/css/bootstrap-multiselect.css"
    rel="stylesheet" type="text/css" />
<script src="http://cdn.rawgit.com/davidstutz/bootstrap-multiselect/master/dist/js/bootstrap-multiselect.js"
    type="text/javascript"></script>
<script type="text/javascript">
    $(function () {
        $('[id*=lstCorrespondance]').multiselect({
            includeSelectAllOption: true
        });
    });
</script>
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true" />
    <%-- <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnablePageMethods="true"
        AsyncPostBackTimeout="360000" EnablePartialRendering="true">
    </asp:ToolkitScriptManager>--%>
    <div>
        <table class="tableForm" width="100%" style="background-color: White;">
            <tr>
                <td>
                </td>
                <td>
                </td>
            </tr>
            <tr style="text-align: left;">
               <%-- <td style="text-align: right;">--%>
               <td>
                    <asp:Label ID="lblSubject" runat="server" Text="Subject"></asp:Label> :
                </td>
                <td style="text-align: left;">
                    <asp:TextBox ID="txtSubject" runat="server" Width="100%"> </asp:TextBox>
                </td>
            </tr>
            <%--<tr>
                <td style="text-align: right;">
                    <asp:Label ID="lblTo" runat="server" Text="To"></asp:Label>
                </td>
                <td style="text-align: left;">
                  
                <asp:ListBox ID="lstCorrespondance" runat="server" Width="182px"  SelectionMode="Multiple">                
                        <asp:ListItem Value="1" Text="Danny B"></asp:ListItem>
                        <asp:ListItem Value="2" Text="Suresh K"></asp:ListItem>
                        <asp:ListItem Value="3" Text="Jenny K"></asp:ListItem>
                </asp:ListBox>
                 <asp:Button ID="Button2" Text="Submit" runat="server" OnClick="Submit" />
                
                </td>
            </tr>--%>
            <tr>
                <td>
                    <asp:Label ID="lblBody" runat="server" Text="Body"></asp:Label> :
                </td>
                <td colspan="4">
                    <div id="oboutEditor">
                        <obout:Editor ID="editEmail" runat="server" Height="280px" Width="100%" TopToolbar-Visible="true"
                            BottomToolbar-Visible="true">
                            <EditPanel DefaultFontFamily="Arial" DefaultFontSize="12pt" Height="280px" />
                        </obout:Editor>
                        <asp:HiddenField ID="hdnmsgbody" runat="server" Value="" />
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="4" style="text-align: right;">
                    <asp:LinkButton ID="btnSubmit" runat="server" Text="Submit" Style="background: #1a527d;
                        border: 1px solid #1a527d; color: #ddd; list-style-type: none; margin: 0 !important;
                        padding: 2px 3px;" OnClientClick="return jsSaveCorrespondance();" OnClick="btnSubmit_Click" ></asp:LinkButton>
                    <%-- OnClick="btnSubmit_Click"--%>
                    <%-- <input type="button" runat="server" id="Button1" value="Submit" style="float: right;"
                        onclick="jsSaveCorrespondance()" />--%>
                </td>
            </tr>
        </table>
    </div>
    <script type="text/javascript">
        function jsSaveCorrespondance() {
            var txtSubject = document.getElementById("<%=txtSubject.ClientID %>");
            if (txtSubject.value == "") {
                showAlert("Enter Subject ", "Error", "#");
                txtSubject.focus();
            }
            else {
                var getObtControlHolder = document.getElementById('oboutEditor');
                var getObtIframeWin = getObtControlHolder.getElementsByTagName('iframe');
                var getObtIframeWinFirstChild = getObtIframeWin[0];
                var getHdnFldForHTML = document.getElementById('<%=hdnmsgbody.ClientID %>');
                if (getObtIframeWinFirstChild != null) {
                    var getHtmlQuestionStr = getObtIframeWinFirstChild.contentWindow.document.body.innerHTML;
                    var filterHtmlQuestionStr = getHtmlQuestionStr.split('<').join('andBrSt;');
                    getHtmlQuestionStr = filterHtmlQuestionStr;
                    filterHtmlQuestionStr = getHtmlQuestionStr.split('>').join('andBrEn;');
                    getHtmlQuestionStr = filterHtmlQuestionStr;
                    getHdnFldForHTML.value = getHtmlQuestionStr;
                } //alert(hdnmsgbody.value);
                //window.opener.GVInboxPOR.refresh();
                //self.close();
                return true;
               // PageMethods.WMAddMessage(hdnmsgbody.value,txtSubject.value,OnSuccessAddmessage, null);
            }
                    
        }
//        function OnSuccessAddmessage(result) {
//            window.opener.GVInboxPOR.refresh();
//            self.close();
//        }

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
    </script>
    <asp:Label ID="lblAddHTMLQuestionInRichBox" runat="server" Visible="false" >
      <script>
          window.onload = function () {
              var getObtControlHolder = document.getElementById('oboutEditor');
              var getObtIframeWin = getObtControlHolder.getElementsByTagName('iframe');
              var getObtIframeWinFirstChild = getObtIframeWin[0];
              var getHdnFldForHTML = document.getElementById('<%=hdnmsgbody.ClientID %>');
              if (getObtIframeWinFirstChild != null) {
                  // var getHtmlQuestionStr = getObtIframeWinFirstChild.contentWindow.document.body.innerHTML;
                  var getHtmlQuestionStr = getHdnFldForHTML.value;
                  var filterHtmlQuestionStr = getHtmlQuestionStr.split('andBrSt;').join('<');
                  getHtmlQuestionStr = filterHtmlQuestionStr;
                  filterHtmlQuestionStr = getHtmlQuestionStr.split('andBrEn;').join('>');
                  getHtmlQuestionStr = filterHtmlQuestionStr;
                  getObtIframeWinFirstChild.contentWindow.document.body.innerHTML = getHtmlQuestionStr;
              }
          }
      </script>
    </asp:Label>
    </form>
</body>
</html>

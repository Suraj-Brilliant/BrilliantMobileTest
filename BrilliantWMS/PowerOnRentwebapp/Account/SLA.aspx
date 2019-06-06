<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SLA.aspx.cs" Inherits="BrilliantWMS.Account.SLA" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="obout_Interface" Namespace="Obout.Interface" TagPrefix="obout" %>
<%@ Register Assembly="obout_Grid_NET" Namespace="Obout.Grid" TagPrefix="obout" %>
<script src="../MasterPage/JavaScripts/crm.js" type="text/javascript"></script>
<script src="../MasterPage/JavaScripts/jquery-3.1.1.min.js" type="text/javascript"></script>
<link href="../App_Themes/Blue/css/userinfo.css" rel="stylesheet" type="text/css" />
<link href="../App_Themes/Blue/css/general.css" rel="stylesheet" type="text/css" />
<link href="../App_Themes/Blue/css/style.css" rel="stylesheet" type="text/css" />

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>GWC Deliveries</title>
     <script type="text/javascript">
         function ActivePrime() {
             $isChecked = 'no';
             $myval = $('#chkPrime').is(':checked') ? 1 : 0;
             if ($myval == 1) {
                 $('#txtPrime').removeAttr('disabled');
                 $('#txtPrime').val('0');
             } else {
                 $('#txtPrime').attr('disabled', 'disabled');
                 $('#txtPrime').val('NA');
             }
         }
         function ActiveExpress() {
             $isChecked = 'no';
             $myval = $('#chkExpress').is(':checked') ? 1 : 0;
             if ($myval == 1) {
                 $('#txtExpress').removeAttr('disabled');
                 $('#txtExpress').val('0');
             } else {
                 $('#txtExpress').attr('disabled', 'disabled');
                 $('#txtExpress').val('NA');
             }
         }
         function ActiveRegular() {
             $isChecked = 'no';
             $myval = $('#chkRegular').is(':checked') ? 1 : 0;
             if ($myval == 1) {
                 $('#txtRegular').removeAttr('disabled');
                 $('#txtRegular').val('0');
             } else {
                 $('#txtRegular').attr('disabled', 'disabled');
                 $('#txtRegular').val('NA');
             }
         }
        </script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true" />
        <div class="divDetailExpand" id="divApprovalDetail" runat="server" clientidmode="Static">
            <center>
        <table class="gridFrame">
            <tr>
                <td>
                     <table style="width: 100%">
                        <tr>
                            <td style="text-align: left;">
                                 <a id="headerText">
                                   <asp:Label ID="lblheasertext" runat="server" Text="Service Level Agreement"></asp:Label></a>
                            </td>                                            
                        </tr>
                     </table>
                </td>
            </tr>
            <tr>
                <td>
                    <table class="tableForm" style="width:100%">
                        <tr>
                             <td style="text-align: left">
                                 <asp:Label ID="lblDeliveryMode" runat="server" Text="Delivery Mode"></asp:Label>
                             </td>
                             <td style="text-align: left">
                                 <obout:OboutCheckBox ID="chkPrime" runat="server" OnClick="ActivePrime()" ControlsToDisable="" ControlsToEnable="" FolderStyle="" ParentCheckBoxID="" State="Unchecked">
                                    </obout:OboutCheckBox>
                                 <asp:Label ID="lblPrime" runat="server" Text="Prime"></asp:Label>
                             </td>
                            <td style="text-align: left">
                                <obout:OboutCheckBox ID="chkExpress" runat="server" OnClick="ActiveExpress()" ControlsToDisable="" ControlsToEnable="" FolderStyle="" ParentCheckBoxID="" State="Unchecked">
                                    </obout:OboutCheckBox>
                                <asp:Label ID="lblExpress" runat="server" Text="Express"></asp:Label>
                            </td>
                            <td style="text-align: left">
                                <obout:OboutCheckBox ID="chkRegular" runat="server" OnClick="ActiveRegular()" ControlsToDisable="" ControlsToEnable="" FolderStyle="" ParentCheckBoxID="" State="Unchecked">
                                    </obout:OboutCheckBox>
                                <asp:Label ID="lblRegular" runat="server" Text="Regular"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left">
                                <asp:Label ID="lblSLADays" runat="server" Text="SLA(Hours)"></asp:Label>
                            </td>
                            <td style="text-align: left">
                                <asp:TextBox ID="txtPrime" runat="server" Text="NA" Enabled="false" onkeypress="return fnAllowDigitsOnly(event)" MaxLength="3"></asp:TextBox>
                            </td>
                            <td style="text-align: left">
                                <asp:TextBox ID="txtExpress" runat="server" Text="NA" Enabled="false" onkeypress="return fnAllowDigitsOnly(event)" MaxLength="3"></asp:TextBox>
                            </td>
                            <td style="text-align: left">
                                <asp:TextBox ID="txtRegular" runat="server" Text="NA" Enabled="false" onkeypress="return fnAllowDigitsOnly(event)" MaxLength="3"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </td>               
            </tr>
            <tr>
                <td style="text-align:right;">
                    <asp:Button ID="btnSubmit" runat="server" Text="  Submit  " OnClick="btnSubmit_Click"  />  <%--OnClientClick="CloseSLA()"--%>   <%--OnClick="btnSubmit_Click"--%>
                </td>
            </tr>
        </table>
    </center>
            <asp:HiddenField ID="hdndeptid" runat="server" ClientIDMode="Static" />
        </div>       
        <script type="text/javascript">
            function fnAllowDigitsOnly(key) {
                var keycode = (key.which) ? key.which : key.keyCode;
                if ((keycode < 48 || keycode > 57) && (keycode != 8) && (keycode != 9) && (keycode != 11) && (keycode != 37) && (keycode != 38) && (keycode != 39) && (keycode != 40) && (keycode != 127)) {
                    return false;
                }
            }

            var gprime = document.getElementById("<%=txtPrime.ClientID %>");
            var gexpress = document.getElementById("<%=txtExpress.ClientID %>");
            var gRegular = document.getElementById("<%=txtRegular.ClientID %>");
            var geptid = document.getElementById("<%=hdndeptid.ClientID %>");
            function CloseSLA() {
                //var Prime = $('#txtPrime').val();
                //var Express = $('#txtExpress').val();
                //var Regular = $('#txtRegular').val();
                var Prime = gprime.value;
                var Express = gexpress.value;
                var Regular = gRegular.value;
                var deptid = geptid.value;
                if (Prime == "" || Express == "" || Regular == "") {
                    showAlert('Please Enter Values...!!!', 'Error', '#');
                } else {
                    PageMethods.WMAddIntomSLA(Prime, Express, Regular, deptid, OnSuccessAddIntomSLA, null);
                    //PageMethods.WMAddIntomSLA(Prime, Express, Regular,getParameterByName("DEPTID").toString(), OnSuccessAddIntomSLA, null);
                    //self.close();
                }
            }
            function OnSuccessAddIntomSLA(result) {
                if (result > 0) {
                    document.getElementById("<%=txtPrime.ClientID %>").value = "";
                    document.getElementById("<%=txtExpress.ClientID %>").value = "";
                    document.getElementById("<%=txtRegular.ClientID %>").value = "";
                    document.getElementById("<%=hdndeptid.ClientID %>").value = "";
                    self.close();
                }
            }
        </script>
    </form>
</body>
</html>

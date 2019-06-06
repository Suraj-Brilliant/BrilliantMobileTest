<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CreateGroup.aspx.cs" Inherits="BrilliantWMS.Warehouse.CreateGroup" Theme="Blue" %>

<script src="../MasterPage/JavaScripts/countries.js" type="text/javascript"></script>
<script src="../MasterPage/JavaScripts/crm.js" type="text/javascript"></script>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
     <div class="pageLoadingImg" style="display: none">       
    </div>
    <form id="form_CreateGroup" runat="server">
    <div>
        <asp:ScriptManager ID="ScriptmanagerAddress" runat="server" EnablePageMethods="true">
        </asp:ScriptManager>
        <center>
            <div id="divLoading" style="height: 280px; width: 600px; display: none" class="modal">
                <center>
                    <br />
                    <span id="spanLoading" style="font-size: 17px; font-weight: bold; color: Yellow;">Processing
                        please wait...</span>
                </center>
            </div>
            <asp:ValidationSummary ID="validationsummary_UcAddressInfo" runat="server" ShowMessageBox="true"
                ShowSummary="false" DisplayMode="bulletlist" ValidationGroup="AddressSubmit" />
            <table class="gridFrame" style="margin: 2px 2px 2px 2px; width: 600px">
                <tr>
                    <td style="text-align: left;">
                        <asp:Label class="headerText" ID="lblAddressFormHeader" Text="Create Master Ticket Number" runat="server"></asp:Label>
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
                    <td colspan="2" style="background: White;">
                        <table class="tableForm" style="width: 600px;">
                            <tr>
                                <td>
                                    Master Ticket Number :
                                </td>
                                <td style="float: left">
                                    <asp:TextBox ID="txtGroupName" runat="server" Width="200px"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RfValtxtAddress1" ForeColor="Maroon" runat="server"
                                        ControlToValidate="txtGroupName" ErrorMessage="Enter Group Name" ValidationGroup="AddressSubmit"
                                        Display="None"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Description :
                                </td>
                                <td style="float: left">
                                    <asp:TextBox ID="txtDesc" runat="server" Width="400px"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ForeColor="Maroon" runat="server"
                                        ControlToValidate="txtDesc" ErrorMessage="Enter Group Description" ValidationGroup="AddressSubmit"
                                        Display="None"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </center>
    </div>
    <script type="text/javascript">
        function SubmitAddress() {

            var grpNm = document.getElementById('txtGroupName');
            var grpDesc = document.getElementById('txtDesc');
            var PONO = getParameterByName("Pinvoker").toString();
            var SONO = getParameterByName("Sinvoker").toString();
            if (PONO != "") {
                PageMethods.WMGetGroupData(PONO, grpNm.value, grpDesc.value, jsGroupDataOnSuccess, null)
            }
            else {
                PageMethods.WMGetSOGroupData(SONO, grpNm.value, grpDesc.value, jsGroupDataOnSuccess, null)
            }
        }
        function jsGroupDataOnSuccess(result) {
            window.opener.location.reload();
            // window.opener.document.location.href = 'CycleCount.aspx';
            self.close();
        }

        function ClearAddress() {
            document.getElementById('txtGroupName').value = "";
            document.getElementById('txtDesc').value = "";
        }

    </script>
    </form>
    <style type="text/css">
        .pageLoadingImg
        {
            background-image: url(../App_Themes/Blue/img/ajax-loader.gif);
            width: 78px;
            height: 118px;
            background-repeat: no-repeat;
            display: inline-block;
        }
    </style>
</body>
</html>

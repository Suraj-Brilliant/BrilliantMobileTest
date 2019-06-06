<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VirtualQty.aspx.cs" Inherits="BrilliantWMS.Product.VirtualQty" Theme="Blue" %>
<%@ Register Assembly="obout_Grid_NET" Namespace="Obout.Grid" TagPrefix="obout" %>
<script src="../MasterPage/JavaScripts/countries.js" type="text/javascript"></script>
<script src="../MasterPage/JavaScripts/crm.js" type="text/javascript"></script>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <div>
        <asp:HiddenField ID="hdnskuid" runat="server" />
        <asp:HiddenField ID="hdnpackuomid" runat="server" />
        <asp:ScriptManager ID="ScriptmanagerContactPerson" runat="server" EnablePageMethods="true">
        </asp:ScriptManager>
        <center>
            <div id="divLoading" style="height: 275px; width: 920px; display: none" class="modal">
                <center>
                    <br />
                    <span id="spanLoading" style="font-size: 17px; font-weight: bold; color: Yellow;">Processing
                        please wait...</span>
                </center>
            </div>
            <asp:ValidationSummary ID="validationsummary_UcContactPersonInfo" runat="server"
                ShowMessageBox="true" ShowSummary="false" DisplayMode="bulletlist" ValidationGroup="SubmitContactPerson" />
            <table class="gridFrame" style="margin: 3px 3px 3px 3px">
                <tr>
                    <td style="text-align: left;">
                        <asp:Label class="headerText" ID="lblContactPersonFormHeader" runat="server" Text="Add Virtual Quantity"></asp:Label>
                    </td>
                    <td style="width: 20%">
                        <table style="float: right;">
                            <tr>
                                <td>
                                    <input type="button" id="btnAddressSubmit" runat="server" value="Submit" onclick="SubmitUOM()"
                                        style="width: 70px;" />
                                </td>
                                <td>
                                    <input type="button" id="btnAddressClear" runat="server" value="Clear" onclick="ClearContactPerson()"
                                        style="width: 70px;" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td style="background-color: White;" colspan="2" align="center">
                        <table class="tableForm">
                            <tr>
                                <td>
                                    <asp:Label ID="lblVirtualQuantity" runat="server" Text="Enter Virtual Quantity"></asp:Label>
                                    :
                                </td>
                                <td style="text-align: left;">
                                    <asp:TextBox ID="txtQuantity" runat="server" MaxLength="15" Width="194px" Style="text-align: right;"
                                        onkeydown="return AllowPhoneNo(this, event);" onkeypress="return AllowPhoneNo(this, event);"></asp:TextBox>
                                </td>
                                <td>
                                  
                                </td>
                                <td style="text-align: left;">
                                   
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td>
                                </td>
                                <td>
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
                                    <input type="button" id="Button1" runat="server" value="Submit" onclick="SubmitUOM();"
                                        style="width: 70px;" />
                                </td>
                                <td>
                                    <input type="button" id="Button2" runat="server" value="Clear" onclick="ClearContactPerson()"
                                        style="width: 70px;" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </center>
    </div>
    </div>
        <script type="text/javascript">
            function ClearContactPerson() {
                document.getElementById("txtQuantity").value = '';
            }

            function SubmitUOM() {
               
                LoadingOn();
                var VirtualQty = new Object();
                VirtualQty.quantity = document.getElementById("txtQuantity").value;
                VirtualQty.hdnskuid = document.getElementById("hdnskuid").value;
                PageMethods.SaveVirtualQty(VirtualQty, virtualUpdate_onSuccess, virtualUpdate_onFail)
               
            }

            function virtualUpdate_onFail() { alert("error"); }

            function virtualUpdate_onSuccess() {
                LoadingOff();
                window.opener.GVInventory.refresh();
                self.close();
            }
        </script>
    </form>
</body>
</html>

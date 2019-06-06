<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddPacks.aspx.cs" Inherits="BrilliantWMS.Product.AddPacks"
    Theme="Blue" %>

<%@ Register Assembly="obout_Grid_NET" Namespace="Obout.Grid" TagPrefix="obout" %>
<script src="../MasterPage/JavaScripts/countries.js" type="text/javascript"></script>
<script src="../MasterPage/JavaScripts/crm.js" type="text/javascript"></script>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="ContactPerson_Form" runat="server">
    <div>
        <asp:HiddenField ID="hdnstate" runat="server" />
        <asp:HiddenField ID="hdnpackuomid" runat="server" />
        <asp:HiddenField ID="hdnskuid" runat="server" />
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
                        <asp:Label class="headerText" ID="lblContactPersonFormHeader" runat="server" Text="Add New Pack"></asp:Label>
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
                                    <asp:Label ID="lbluom1" runat="server" Text="UOM"></asp:Label>
                                    :
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddluom1" DataTextField="Description" DataValueField="ID" Width="194px"
                                        runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="lblQuantity" runat="server" Text="Quantity"></asp:Label>
                                    :
                                </td>
                                <td style="text-align: left;">
                                    <asp:TextBox ID="txtQuantity" runat="server" MaxLength="15" Width="194px" Style="text-align: right;"
                                        onkeydown="return AllowPhoneNo(this, event);" onkeypress="return AllowPhoneNo(this, event);"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:Label ID="lblsequence" runat="server" Text="Sequence"></asp:Label>
                                    :
                                </td>
                                <td style="text-align: left;">
                                    <asp:TextBox ID="txtsequence" runat="server" MaxLength="3" Width="194px" Style="text-align: right;"
                                        onkeydown="return AllowPhoneNo(this, event);" onkeypress="return AllowPhoneNo(this, event);"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
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
    <script type="text/javascript">
        var Description = document.getElementById("ddluom1");
        function SubmitUOM() {
            //            if (typeof (Page_ClientValidate) == 'function') {
            //                Page_ClientValidate();
            //            }
            //if (Page_IsValid) {
            LoadingOn();
            var AddressInfo = new Object();

            AddressInfo.UOMDescri = document.getElementById("ddluom1").value;
            AddressInfo.quantity = document.getElementById("txtQuantity").value;
            AddressInfo.sequence = document.getElementById("txtsequence").value;
            AddressInfo.Description = Description.options[Description.selectedIndex].text;
            AddressInfo.hdnstate = document.getElementById("hdnstate").value;
            AddressInfo.hdnpackuomid = document.getElementById("hdnpackuomid").value;
            AddressInfo.hdnskuid = document.getElementById("hdnskuid").value;
            PageMethods.PMSaveAddress(AddressInfo, SubmitAddress_onSuccess, SubmitAddress_onFail)
            //            }
            //            else { }
        }

        function SubmitAddress_onFail() { alert("error"); }

        function SubmitAddress_onSuccess() {
            LoadingOff();
            window.opener.Grid2.refresh();
            self.close();
        }

        function ClearContactPerson() 
        {
            document.getElementById("ddluom1").selectedIndex = 0;
            document.getElementById("txtQuantity").value = '';
            document.getElementById("txtsequence").value = '';
        }
    </script>
    </form>
</body>
</html>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UDF.aspx.cs" Inherits="BrilliantWMS.Product.UDF" Theme="Blue" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="Obout.Ajax.UI" Namespace="Obout.Ajax.UI.FileUpload" TagPrefix="obout" %>
<%@ Register Assembly="obout_Interface" Namespace="Obout.Interface" TagPrefix="obout" %>
<%@ Register Assembly="obout_Grid_NET" Namespace="Obout.Grid" TagPrefix="obout" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:HiddenField ID="hdnstate" runat="server" />
            <asp:HiddenField ID="hdnspecifid" runat="server" />
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
                            <asp:Label class="headerText" ID="lblContactPersonFormHeader" runat="server" Text="Update UDF"></asp:Label>
                        </td>
                        <td style="width: 20%">
                            <table style="float: right;">
                                <tr>
                                    <td>
                                        <input type="button" id="btnAddressSubmit" runat="server" value="Submit" onclick="SubmitUOM()"
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
                                        <asp:Label ID="lbluom1" runat="server" Text="UDF"></asp:Label>
                                        :
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtudf" runat="server" Width="200px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblvaue" runat="server" Text="Description"></asp:Label>
                                        :
                                    </td>
                                    <td style="text-align: left;">
                                        <asp:TextBox ID="txtvalue" runat="server" Width="250px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <req><asp:Label ID="lblactive" runat="server" Text="Active"/></req>
                                        :
                                    </td>
                                    <td style="text-align: left">
                                        <obout:oboutradiobutton id="rbtYes" runat="server" text="Yes  " groupname="rbtActive"
                                            checked="True" folderstyle="">
                                         </obout:oboutradiobutton>
                                        &nbsp;&nbsp;
                                         <obout:oboutradiobutton id="rbtNo" runat="server" text="No"
                                               groupname="rbtActive" folderstyle="">
                                         </obout:oboutradiobutton>
                                    </td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 80%"></td>
                        <td style="width: 20%">
                            <table style="float: right;">
                                <tr>
                                    <td>
                                        <input type="button" id="Button1" runat="server" value="Submit" onclick="SubmitUOM();"
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
            function SubmitUOM() {
                //            if (typeof (Page_ClientValidate) == 'function') {
                //                Page_ClientValidate();
                //            }
                //if (Page_IsValid) {

                var AddressInfo = new Object();
                AddressInfo.udf = document.getElementById("txtudf").value;
                AddressInfo.value = document.getElementById("txtvalue").value;
                AddressInfo.hdnspecifid = document.getElementById("hdnspecifid").value;
                PageMethods.PMSaveAddress(AddressInfo, SubmitAddress_onSuccess, SubmitAddress_onFail)
                //            }
                //            else { }
            }

            function SubmitAddress_onFail() { alert("error"); }

            function SubmitAddress_onSuccess() {
                window.opener.Grid1.refresh();
                self.close();
            }
        </script>
    </form>
</body>
</html>

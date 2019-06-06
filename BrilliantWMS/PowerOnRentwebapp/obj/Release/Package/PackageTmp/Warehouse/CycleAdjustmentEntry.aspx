<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CycleAdjustmentEntry.aspx.cs" Inherits="BrilliantWMS.POR.CycleAdjustmentEntry" Theme="Blue" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<script src="../MasterPage/JavaScripts/countries.js" type="text/javascript"></script>
<script src="../MasterPage/JavaScripts/crm.js" type="text/javascript"></script>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
     <form id="form1" runat="server">
     <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnablePageMethods="true"
        EnablePartialRendering="true">
    </asp:ToolkitScriptManager>
    <div>
    <center>
            <div id="divLoading" style="height: 280px; width: 1000px; display: none" class="modal">
                <center>
                    <br />
                    <span id="spanLoading" style="font-size: 17px; font-weight: bold; color: Yellow;">Processing
                        please wait...</span>
                </center>
            </div>
            <table class="gridFrame" style=" width: 1050px">
            <tr>
               <td style="text-align: left;">
               <a id="headerText">Cycle Count Adjustment</a>
               
                        <table style="float: right;">
                            <tr>
                                <td>
                                    <input type="button" id="btnAddressSubmit" value="Submit" onclick="UpdateCycleDetail();"
                                        style="width: 70px;" />
                                </td>
                               <%-- <td>
                                    <input type="button" id="btnAddressClear" value="Clear" onclick="ClearAddress()"
                                        style="width: 70px;" />
                                </td>--%>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                                             
                    <td style="background-color: White;"  >
                        <table style="border: none; width: 1000px">
                            <tr>
                                 <td style="height:10px"></td>
                             </tr>
                             <tr style="padding-bottom:10px;">
                               <td >
                                   Product Code :
                                </td>
                                <td style="text-align: left">
                                    <asp:TextBox ID="txtprodCode" runat="server" Width="182px" MaxLength="100" ClientIDMode="Static" ReadOnly="True"></asp:TextBox>
                                                                    
                                </td>
                                <td>
                                    Location :
                                </td>
                                <td style="text-align: left">
                                     <asp:TextBox ID="txtlocCode"  runat="server" Width="152px" MaxLength="50" ClientIDMode="Static" ReadOnly="True" ></asp:TextBox>
                                </td>
                                <td>
                                     Batch Code :
                                </td>
                                 <td style="text-align: left">
                                    <asp:TextBox ID="txtbatch"  runat="server" Width="152px" MaxLength="50" ClientIDMode="Static" ReadOnly="True" ></asp:TextBox>
                                </td>
                              </tr>
                             <tr>
                                 <td style="height:10px"></td>
                             </tr>
                              <tr style="padding-bottom:50px; height:20px">
                                  <td> System Qty : </td>
                                   <td style="text-align: left">
                                   <asp:TextBox ID="txtsysQty"  runat="server" Width="100px" MaxLength="50" Style="text-align: right"
                                        ClientIDMode="Static" ReadOnly="True" ></asp:TextBox>
                                 </td>
                                 <td>
                                    Actual Qty :
                                 </td>
                                 <td style="text-align: left">
                                     <asp:TextBox ID="txtActualQty" runat="server" Width="100px" MaxLength="50" Style="text-align: right" onchange="RemainingQty()"
                                      ForeColor="Black"></asp:TextBox>
                                 </td>
                                 <td>
                                   <asp:Label ID="Label1" runat="server" Text="Adjustment Qty :"></asp:Label>
                                 </td>
                                 <td style="text-align: left">
                                      <asp:TextBox ID="txtAdjQty" runat="server" Width="100px" ClientIDMode="Static" Style="text-align: right" ReadOnly="True"></asp:TextBox>
                                 </td>
                                </tr>
                               <tr>
                                 <td style="height:10px"></td>
                                </tr>
                                 <tr style="padding-bottom:10px; height:10px;">
                                     <td>
                                         <asp:Label ID="Label2" runat="server" Text="Adjustment Location :"> </asp:Label> 
                                     </td>
                                    <td style="text-align: left">
                                     <asp:TextBox runat="server" ID="txtlocation" Width="190px" MaxLength="100" ></asp:TextBox>
                                     <img id="imgSearch" src="../App_Themes/Grid/img/search.jpg" title="Search Product"
                                        style="cursor: pointer;" onclick="openProductSearch('0')"/>
                                </td>
                                <td>Remark :
                                </td>
                                <td style="text-align: left" colspan="3">
                                    <asp:TextBox ID="txtRemark" runat="server" Width="450px" ClientIDMode="Static"></asp:TextBox>
                                </td>
                                </tr>
                               <tr>
                                 <td style="height:10px"></td>
                             </tr>
                                </table>
                                 
                    </td>
                </tr>
            </table>
            <asp:HiddenField ID="hdnCycleHeadID" runat="server" ClientIDMode="Static" />
             <asp:HiddenField ID="hdncycleDetailID" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hdnCheckEditAdjust" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hdnproductId" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hdndiffQty" runat="server" ClientIDMode="Static" />
             <asp:HiddenField ID="hdnfromlocID" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hdnlocationSearchName" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hdnLocationSearchID" runat="server" ClientIDMode="Static" />

            <script type="text/javascript">
                var ProductCode = document.getElementById("<%=txtprodCode.ClientID%>");
                var Location = document.getElementById("<%=txtlocCode.ClientID%>");
                var FromLocID = document.getElementById("<%=hdnfromlocID.ClientID%>");
                var ProductID = document.getElementById("<%=hdnproductId.ClientID%>");
                var BatchCode = document.getElementById("<%=txtbatch.ClientID%>");
                var SystemQty = document.getElementById("<%=txtsysQty.ClientID%>");
                var ActualQty = document.getElementById("<%=txtActualQty.ClientID%>");
                var AjustmentQty = document.getElementById("<%=txtAdjQty.ClientID%>");
                var AdjustmentLocIds = document.getElementById("<%=hdnLocationSearchID.ClientID%>");
                var Remark = document.getElementById("<%=txtRemark.ClientID%>");
                var hdnCycleHeadID = document.getElementById("<%=hdnCycleHeadID.ClientID%>");
                var hdncycleDetailID = document.getElementById("<%=hdncycleDetailID.ClientID%>");

                function UpdateCycleDetail() {

                    var obj1 = new Object();
                    obj1.ProductCode = ProductCode.value.toString();
                    obj1.Location = Location.value.toString();
                    obj1.FromLocID = FromLocID.value.toString();
                    obj1.ProductID = ProductID.value.toString();
                    obj1.BatchCode = BatchCode.value.toString();
                    obj1.SystemQty = parseFloat(SystemQty.value.toString());
                    obj1.ActualQty = parseFloat(ActualQty.value.toString());
                    obj1.AjustmentQty = parseFloat(AjustmentQty.value.toString());
                    obj1.AdjustmentLocIds = AdjustmentLocIds.value.toString();
                    obj1.Remark = Remark.value.toString();
                    obj1.CycleHeadID = hdnCycleHeadID.value.toString();
                    obj1.CycleDetailID = hdncycleDetailID.value.toString();
                    PageMethods.GetlocationQty(obj1, WMGet_LocQty);
                    //obj1.EditQueryString = hdnCheckEditAdjust.value;
                   // if (hdnCheckEditAdjust.value == "") {
                    //    obj1.AdjustLoc = parseInt(AdjustmentLoc.options[AdjustmentLoc.selectedIndex].value);
                     //   PageMethods.GetlocationQty(obj1, WMGet_LocQty);

                   // }
                   // else {

                    //    PageMethods.EditLocation(obj1, WMGet_LocQty);
                  //  }
                }

                function WMGet_LocQty(result) {

                    if (result == "Some error occurred" || result == "") { showAlert("Error occurred", "Error", '#'); }
                    else {
                        showAlert(result, "info", "");
                        // window.opener.getCycleTitleCode(hdnCycleHeadID.value);
                        //window.opener.location.reload()
                        window.opener.grdImportView.refresh();
                       // window.opener.document.location.href = 'CycleCount.aspx';
                        self.close();
                        return false;
                    }

                }

                function RemainingQty() {
                    var SysQty = document.getElementById("<%=txtsysQty.ClientID %>").value;
                    var actQty = document.getElementById("<%=txtActualQty.ClientID %>").value;
                    document.getElementById("<%=txtAdjQty.ClientID %>").value = parseFloat(SysQty - actQty).toFixed(2);
                    document.getElementById("<%=hdndiffQty.ClientID %>").value = parseFloat(SysQty - actQty).toFixed(2);
                }

                function openLocation1(sequence) {
                    window.open('../POR/LocationSearch.aspx', null, 'height=700px, width=825px,status= 0, resizable= 1, scrollbars=1, toolbar=0,location=center,menubar=0, screenX=300; screenY=50');
                }
    </script>

         <script type="text/javascript">

             function openProductSearch(sequence) {
                 var CustomerID = 0;
                 var ProductID = document.getElementById("<%=hdnproductId.ClientID%>").value;
                 window.open('../Warehouse/LocationBySKU.aspx?ProductID=' + ProductID + '', 'height=500px, width=1080px,status= 0, resizable= 1, scrollbars=1, toolbar=0,location=center,menubar=0, screenX=300; screenY=50', target = "_blank");
             }

             function AfterProductSelected(LocName, LocID)
             {
                 var Locationname = document.getElementById("<%=hdnlocationSearchName.ClientID%>");
                 var LocationID = document.getElementById("<%=hdnLocationSearchID.ClientID%>");
                 LocationID.value = LocID;
                 document.getElementById("<%=txtlocation.ClientID%>").value = LocName;
             }

         </script>
        </center>
    </div>
    </form>
</body>
</html>

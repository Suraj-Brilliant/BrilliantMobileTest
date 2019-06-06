<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/CRM.Master" AutoEventWireup="true" CodeBehind="PutIn.aspx.cs" EnableEventValidation="false"
  Inherits="BrilliantWMS.Warehouse.PutIn" Theme="Blue" %>

<%@ Register Assembly="obout_Grid_NET" Namespace="Obout.Grid" TagPrefix="obout" %>
<%@ Register Src="../CommonControls/Toolbar.ascx" TagName="Toolbar" TagPrefix="uc2" %>
<%@ Register Src="../CommonControls/UCFormHeader.ascx" TagName="UCFormHeader" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../CommonControls/UC_Date.ascx" TagName="UC_Date" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHolder_FormHeader" runat="server">
     <asp:HiddenField ID="HdnLoc" runat="server" Value="" />        
    <asp:HiddenField ID="HdnLocTxt" runat="server" Value="" />
    <asp:HiddenField ID="HdnPoId" runat="server" Value="" />
    <uc2:Toolbar ID="Toolbar1" runat="server" />
    <uc2:UCFormHeader ID="UCFormHeader1" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPHolder_Form" runat="server">
     <center>
       <%-- <div class="divDetailExpandPopUpOff1" id="divPopUp">
            <center>
                <div class="popupClose1" onclick="CloseShowReport()">
                </div>
                <div class="divDetailExpand" id="div1">
                    <iframe runat="server" id="iframePORRpt" clientidmode="Static" src="#" width="100%"
                        style="border: none; height: 700px;"></iframe>
                </div>
            </center>
        </div>--%>
        <span id="imgProcessing" style="display: none;">
            <img src="../App_Themes/Blue/img/ajax-loader.gif" style="top: 50%;" />
        </span>
        <div id="divLoading" style="height: 110%; width: 100%; display: none; top: 0; left: 0;"
            class="modal">
            <center>
            </center>
        </div>
        <%--<div class="divHead">
            <h4>
                PO Group Detail
            </h4>
            <a onclick="javascript:divcollapsOpen('divRequestDetail',this)" id="linkRequest">Expand</a>
        </div>
        <div class="divDetailCollapse" id="divRequestDetail">
            <table class="tableForm" style="width: 1255px">
                <tr style="margin-bottom: 10px;">
                    <td style="margin-bottom: 10px;">
                        <b>Master Control Number : </b>
                    </td>
                    <td style="text-align: left;">
                        <asp:Label runat="server" ID="lblRequestNo" Text="NPO001"></asp:Label>&nbsp;&nbsp;
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <b>Description : </b>
                        <asp:Label runat="server" ID="lblDesc"  Text="Purchase order Group"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <b>PO Number :</b>
                    </td>
                    <td style="text-align: left;">
                        <asp:Label runat="server" ID="lblSites" Width="422px" Text="711650|711651|711652|"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <b>Product Code:</b>
                    </td>
                    <td style="text-align: left;">
                        <asp:Label runat="server" ID="lblProdCode" Width="700px" Text="5-WS12007BR-022-SE-36C-0 | 5-WS12007BR-022-SE-36B-0 |  5-WS12007BR-022-SE-36A-0 | 5-WS12007BR-022-SE-34C-0"></asp:Label>
                    </td>
                </tr>
            </table>
        </div>--%>
        <div class="divHead" id="divReceiptDetailHead">
            <h4>
                PutIn Detail
            </h4>
            <a onclick="javascript:divcollapsOpen('divReceiptDetail',this)" id="linkReceiptDetail">
                Collapse</a>
        </div>
        <div class="divDetailExpand" id="divReceiptDetail">
            <table class="tableForm" border="0" style="width: 97%">
                <tr>
                    <td style="width: 100px; text-align: right;">
                        Product Code.* :
                    </td>
                    <td style="text-align: left; width: 200px;">
                        <asp:TextBox runat="server" ID="TextBox1" MaxLength="100" Style="height:20px;" Enabled="false"></asp:TextBox>
                         <%-- <asp:DropDownList runat="server" ID="DDLProd" Width="230px" onchange="ChkFun();"
                            AccessKey="1" DataTextField="Status" DataValueField="ID" EnableTheming="True">
                        </asp:DropDownList>--%>
                       <%-- <input type="text" id="btn" value="" clientidmode="Static" runat="server" style="vertical-align: top;height:20px;width:200px;"  onchange="ChkFun();" />
                        <img id="imgSearch" src="../App_Themes/Grid/img/search.jpg" title="Search Product" style="cursor: pointer;" onclick="openProductSearch('0')" />
                        &nbsp--%>
                    </td>
                    <td style="width: 120px; text-align: right;">
                        Put In Quantity * :
                    </td>
                    <td style="text-align: left; width: 170px;">
                        <asp:TextBox runat="server" ID="txtReceiQty"  onchange="ChkQty();" Text="" Style="text-align: right"></asp:TextBox>
                    </td>
                    <td style="width: 145px; text-align:right;">
                        Location Capacity :
                    </td>
                    <td style="text-align: left; width: 170px;">
                        <asp:TextBox runat="server" ID="txtLocationCapacity" MaxLength="100" Style="height:20px;" Enabled="false"></asp:TextBox>
                    </td>
                    <%--<td style="text-align:left;" >
                        Barcode * :
                    </td>--%>
                   <%-- <td  style="width: 220px; text-align: left;">
                        <asp:Label runat="server" ID="lblBarcode" Style="font-family: 'Free 3 of 9 Extended';vertical-align:bottom"
                            Font-Size="X-Large" Text=""></asp:Label>
                    </td>--%>
                    <%--<td style="width: 85px; text-align: left;">
                        PO Quantity * :
                    </td>
                    <td style="text-align: left; width: 60px;">
                        <%--   <div id="divPoQty"  style="text-align: right;width:50px;"></div>
                        <asp:Label runat="server" ID="lblPoQty" Style="text-align: right" Text=""></asp:Label>
                    </td>--%>
                    <td style="width: 100px; text-align: right;">
                        Location Avl. Blc. :
                    </td>
                    <td style="text-align: left; width: 170px;">
                        <asp:TextBox runat="server" ID="txtAvlBlc" MaxLength="100" Style="height:20px;" Enabled="false"></asp:TextBox>
                    </td>
                    
                </tr>
                <tr>        
                    <td style="width: 100px; text-align: right;">
                        Location :
                    </td>
                    <td  style="text-align: left; width: 190px;" class="style1">
                        <asp:TextBox runat="server" ID="txtLocation" MaxLength="100" Style="height:20px;" Enabled="false"></asp:TextBox>
                        <img id="imgSearch" runat="server" visible="false" src="../App_Themes/Grid/img/search.jpg" title="Search Location" style="cursor: pointer; " onclick="openProductSearch('0')" />
                        <%--<asp:DropDownList runat="server" ID="ddlLocation" Width="200px" onchange="SetVal(1);"
                            AccessKey="1" DataTextField="Status" DataValueField="ID" EnableTheming="True">
                        </asp:DropDownList>--%>
                    </td>           
                    <td style="text-align: center; width: 120px;">
                        <asp:Button ID="BtnClearGrid" runat="server" style="width:90px;" Text="Clear"  
                            OnClick="BtnClearGrid_Click"  Visible="false" />
                    </td>
                    <td style="text-align: center; width: 100px;">
                        <asp:Button ID="Button1" runat="server" style="width:90px;" Text="Submit" OnClientClick="return ValidateC();" 
                            OnClick="Button1_Click" Visible="false" />
                    </td>
                    <%--<td style="text-align: left;">
                        Purchase Orders :
                    </td>
                    <td colspan="" style="text-align: left; width: 160px;" class="style1">
                        <asp:DropDownList runat="server" ID="ddlPoList" onchange="SetVal(2);" Width="160px"
                            AccessKey="1" DataTextField="Status" DataValueField="ID" EnableTheming="True">
                        </asp:DropDownList>
                    </td>--%>
                    
                    <%-- <td></td><td></td>--%>
                </tr>
            </table>
            <table class="gridFrame" id="tblCart" width="94%">
                <tr>
                    <td>
                        <table style="width: 100%" cellpadding="0" cellspacing="1">
                            <tr>
                                <td style="text-align: left;">
                                    <a class="headerText">PutIn Products List</a>
                                </td>
                                <td style="text-align: right; width: -1px;">
                               <%-- <asp:Button ID="btnViewReport" runat="server" OnClientClick="selectImage();return false;" Text="View Receipt" Enabled="false" />--%>
                                    <input type="button" value="Make Unpacked" id="btnUnpacked" onclick="MakeUnPacked();" runat="server" />
                                    <input type="button"  value="View Receipt" id="btnViewReport" onclick="selectImage();" />
                                </td>
                                <td style="text-align: right; width: 0px;">
                                    <asp:Button ID="BtnSequence" runat="server" OnClick="BtnSequence_Click" OnClientClick="Confirm();" Text="Save PutIn"  />
                                </td>
                            </tr> 
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <obout:Grid ID="GridReceipt" runat="server" AllowAddingRecords="false"  AutoGenerateColumns="false" AllowColumnResizing="true" AllowFiltering="true"
                            AllowManualPaging="true" AllowColumnReordering="true" AllowMultiRecordSelection="true" AllowRecordSelection="true" AllowGrouping="true" Width="100%"
                                Serialize="true" CallbackMode="true" PageSize="10" AllowPaging="true" AllowPageSizeSelection="true" OnRebind="GridReceipt_OnRebind">        <%-- OnSelect="gvPrdPutIn_Select"--%>                    
                            <ClientSideEvents ExposeSender="true" />
                            <Columns>
                                <obout:Column ID="Column1" DataField="Edit" HeaderText="Edit" runat="Server" Width="5%" Align="center" HeaderAlign="center"
                                        AllowFilter="false">
                                        <TemplateSettings TemplateId="imgBtnEdit1" />
                                    </obout:Column>
                                <obout:Column DataField="Sequence" HeaderText="Sequence No." AllowEdit="false" Width="7%"
                                    Align="center" HeaderAlign="center">
                                   <%-- <TemplateSettings TemplateId="grdSrNo" />--%>
                                </obout:Column>
                                <obout:Column DataField="Code" HeaderText="Location Code" AllowEdit="false" Width="8%" HeaderAlign="center" Align="center" >
                                    <TemplateSettings TemplateId="GTLocationCode" />
                                </obout:Column>
                                <obout:Column DataField="LocationBarCode" HeaderText="Location Bar Code" AllowEdit="false" Width="8%" HeaderAlign="center" Align="center" >
                                    <TemplateSettings TemplateId="GTLocationBarCode" />
                                </obout:Column>
                                <obout:Column DataField="Capacity" HeaderText="Location Capacity" AllowEdit="false" Width="8%" HeaderAlign="center" Align="center" Wrap="true">
                                    <TemplateSettings TemplateId="GTLocationCapacity" />
                                </obout:Column>
                                <obout:Column DataField="AvailableBalance" HeaderText="Current Stock" AllowEdit="false" Width="8%" HeaderAlign="center" Align="center" Wrap="true">
                                </obout:Column>
                                <obout:Column DataField="ProductCode" HeaderText="Product Code" AllowEdit="false" Width="10%"
                                    HeaderAlign="left">
                                </obout:Column>
                                <obout:Column DataField="OMSSKUCode" HeaderText="Product BarCode" AllowEdit="false" Width="10%" HeaderAlign="left">
                                    <TemplateSettings TemplateId="ProductBarCode" />                                    
                                </obout:Column>
                                <obout:Column DataField="LocQty" HeaderText="Put In Qty." AllowEdit="false" Wrap="true"
                                    Width="7%" HeaderAlign="left">
                                </obout:Column>
                                <obout:Column DataField="Pack" HeaderText="Pack" AllowEdit="false" Width="7%"
                                    HeaderAlign="left">
                                </obout:Column>
                                <obout:Column DataField="BatchNo" HeaderText="Batch No" AllowEdit="false" Width="7%"
                                    HeaderAlign="left">
                                </obout:Column>
                               <%-- <obout:Column DataField="ID" HeaderText="Serial No" AllowEdit="false" Width="7%"
                                    HeaderAlign="left">
                                </obout:Column>
                                <obout:Column DataField="SortCode" HeaderText="Put Sequence" AllowEdit="false" Width="7%"
                                    HeaderAlign="left">
                                </obout:Column>--%>
                                                            
                            </Columns>
                            <Templates>
                                <obout:GridTemplate runat="server" ID="PlainEditTemplate">
                                    <Template>
                                        <input type="text" class="excel-textbox" value="<%# Container.Value %>" onfocus="markAsFocused(this)"
                                            onkeydown="AllowDecimal(this,event);" onkeypress="AllowDecimal(this,event);"
                                            onblur="markAsBlured(this, '<%# GridReceipt.Columns[Container.ColumnIndex].DataField %>', <%# Container.PageRecordIndex %>)" />
                                    </Template>
                                </obout:GridTemplate>
                                <obout:GridTemplate ID="GridTemplateRightAlign">
                                    <Template>
                                        <span style="text-align: right; width: 130px; margin-right: 10px;">
                                            <%# Container.Value  %></span>
                                    </Template>
                                </obout:GridTemplate>
                                <obout:GridTemplate runat="server" ID="imgBtnEdit1">
                                        <Template>
                                            <asp:ImageButton ID="imgBtnEdit" runat="server" ImageUrl="../App_Themes/Blue/img/Edit16.png"
                                                ToolTip="Edit" OnClick="gvPrdPutIn_Select" />
                                        </Template>
                                    </obout:GridTemplate>
                                <obout:GridTemplate ID="GTLocationCode" runat="server">
                                    <Template>
                                        <div class="divLocationCode">
                                            <asp:Label ID="rowLocationCode" runat="server" Text="<%# Container.Value %>"></asp:Label> 
                                        </div>
                                    </Template>
                                </obout:GridTemplate>
                                <obout:GridTemplate ID="GTLocationBarCode" runat="server">
                                    <Template>
                                        <div class="divLocationBarCode" style="font-family:'3 of 9 Barcode'">
                                            <asp:Label ID="rowLocationBarCode" runat="server" Text="<%# Container.Value %>"></asp:Label> 
                                        </div>
                                    </Template>
                                </obout:GridTemplate>
                                <obout:GridTemplate ID="ProductBarCode" runat="server">
                                    <Template>
                                        <div class="divProductBarCode" style="font-family:'3 of 9 Barcode'">
                                            <asp:Label ID="rowProductBarCode" runat="server" Text="<%# Container.Value %>"></asp:Label> 
                                        </div>
                                    </Template>
                                </obout:GridTemplate>                               
                                <obout:GridTemplate ID="GTLocationCapacity" runat="server">
                                    <Template>
                                        <div class="divLacationCapacity">
                                            <asp:Label ID="rowLocationCapacity" runat="server" Text="<%# Container.Value%>"></asp:Label>
                                        </div>
                                    </Template>
                                </obout:GridTemplate>

                                <%--<obout:GridTemplate runat="server" ID="grdSrNo">
                                    <Template>
                                        <%#Container.DataRecordIndex+1 %>
                                    </Template>
                                </obout:GridTemplate>        --%>                        
                            </Templates>
                        </obout:Grid>
                    </td>
                </tr>
            </table>
        </div>
    </center>
    <asp:HiddenField ID="hdnSelectedrec" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnSelectedPutInRec" runat="server" ClientIDMode="Static"  />
    <asp:HiddenField ID="SelectedPOs" runat="server" Value="" />
    <asp:HiddenField ID="hdnLocationSearchSelectedRec" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnLocationSearchCapacity" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnLoactionSearchAvlblc" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnLocationID" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnLocSortCode" runat="server" ClientIDMode="Static" />
     <asp:HiddenField ID="MCNNo" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="PONO" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="PrdNo" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnChangedQty" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnNewSelectedLocation" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnPutInNo" runat="server" ClientIDMode="Static" />
      
 <asp:DropDownList runat="server" ID="DDLSortcode" Width="160px" AccessKey="1" DataTextField="SortCode" DataValueField="ID" 
                    EnableTheming="True" Style="display:none;">         </asp:DropDownList>
    <script type="text/javascript">
        window.onload = function () {
            oboutGrid.prototype.restorePreviousSelectedRecord = function () {
                return;
            }
            oboutGrid.prototype.markRecordAsSelectedOld = oboutGrid.prototype.markRecordAsSelected;
            oboutGrid.prototype.markRecordAsSelected = function (row, param2, param3, param4, param5) {
                if (row.className != this.CSSRecordSelected) {
                    this.markRecordAsSelectedOld(row, param2, param3, param4, param5);

                } else {
                    var index = this.getRecordSelectionIndex(row);
                    if (index != -1) {
                        this.markRecordAsUnSelected(row, index);
                    }
                }
                selectPutInRec();
            }
        }

        function selectPutInRec() {
            var hdnSelectedrec = document.getElementById("hdnSelectedrec");
            hdnSelectedrec.value = "";
            if (GridReceipt.PageSelectedRecords.length > 0) {
                for (var i = 0; i < GridReceipt.PageSelectedRecords.length; i++) {
                    var record = GridReceipt.PageSelectedRecords[i];
                    if (hdnSelectedrec.value != "") hdnSelectedrec.value += ',' + record.Sequence;
                    if (hdnSelectedrec.value == "") hdnSelectedrec.value = record.Sequence;
                }
            }
        }

        onload();
        function onload() {
            var v = document.getElementById("btnConvertTo");
            v.style.visibility = 'hidden';
            var exp = document.getElementById("btnExport");
            exp.style.visibility = 'hidden';
            //  var imp = document.getElementById("btnImport");
            var ml = document.getElementById("btnMail");
            ml.style.visibility = 'hidden';
            var pt = document.getElementById("btnPrint");
            pt.style.visibility = 'hidden';
        }

        function MakeUnPacked() {
            var hdnSelectedrec = document.getElementById("hdnSelectedrec");            
            if (hdnSelectedrec.value == "") {
                showAlert("Please Select At Least One Record...!", 'error', '#');
            } else {
                var obj1 = new Object();
                obj1.Sequence = hdnSelectedrec.value;
                obj1.Pack = 'Unpacked';
                PageMethods.WMUpdatePutInListPack(obj1, null, null);
                GridReceipt.refresh();
            }
        }

        function openProductSearch(sequence) {
            var selectedPO = document.getElementById("<%=SelectedPOs.ClientID %>").value;
            window.open('../WMS/LocationSearch.aspx', null, 'height=700px, width=825px,status= 0, resizable= 1, scrollbars=1, toolbar=0,location=center,menubar=0, screenX=300; screenY=50');
        }       

        function AfterLocationSelected() {
            var hdnLocationSearchSelectedRec = document.getElementById('hdnLocationSearchSelectedRec');
            var hdnLocationSearchCapacity = document.getElementById('hdnLocationSearchCapacity');
            var hdnLoactionSearchAvlblc = document.getElementById('hdnLoactionSearchAvlblc');
            var hdnLocationID = document.getElementById('hdnLocationID');
            var hdnLocSortCode = document.getElementById('hdnLocSortCode');

            var hdnSelectedPutInRec = document.getElementById("hdnSelectedPutInRec");

            //alert(hdnLocSortCode.value);
            var txtLocation = document.getElementById("<%= txtLocation.ClientID %>");
            var txtLocationCapacity = document.getElementById("<%= txtLocationCapacity.ClientID %>");
            var txtAvlBlc = document.getElementById("<%= txtAvlBlc.ClientID %>");

            txtLocation.value = hdnLocationSearchSelectedRec.value;
            txtLocationCapacity.value = hdnLocationSearchCapacity.value;
            txtAvlBlc.value = hdnLoactionSearchAvlblc.value;

            var obj1 = new Object();
            obj1.Sequence = hdnSelectedPutInRec.value;
            obj1.LocationID = hdnLocationID.value;
            obj1.Code = hdnLocationSearchSelectedRec.value;
            obj1.SortCode = hdnLocSortCode.value;
            obj1.Capacity = hdnLocationSearchCapacity.value;
            obj1.AvailableBalance = hdnLoactionSearchAvlblc.value;
            
            PageMethods.WMUpdatePutInListLoc(obj1, null, null);
        }
    </script>
    <%--Receipt Head--%>
    <%-- <script type="text/javascript">--%>
    <%--End Receipt Head--%>
    <%--Receipt Part Detail--%>
    <script type="text/javascript">
        function JSBarcode() {
            var txtPrdCode = document.getElementById("<%=TextBox1.ClientID %>");
            var lblBarCode = document.getElementById("lblBarcode");
            lblBarCode.innerHTML = txtPrdCode.value;
        }

        function CloseShowReport() {
            LoadingOff();
            divPopUp.className = "divDetailExpandPopUpOff";
        }

        function selectImage() {            
            var hdnPutInNo = document.getElementById('hdnPutInNo');           
            PageMethods.WMShowReport(hdnPutInNo.value, ShowReport_Onsuccess);
        }
        function ShowReport_Onsuccess(result) {
            if (parseInt(result) > 0) {
                document.getElementById("iframePORRpt").src = "../POR/Reports/ReportViewer.aspx";
                divPopUp.className = "divDetailExpandPopUpOn";
            }
            else {
                CloseShowReport();
            }
        }

        function markAsFocused(textbox) {
            textbox.className = 'excel-textbox-focused';
            textbox.select();
        }
        var RowIndex = 0;
        function markAsBlured(textbox, dataField, rowIndex) {
            textbox.className = 'excel-textbox';
            RowIndex = rowIndex;
            var txtvalue = textbox.value;
            if (txtvalue == "") txtvalue = 0;
            textbox.value = parseFloat(txtvalue).toFixed(2);
            //ChallanQty, ReceivedQty, ShortQty, ExcessQty

            if (GridReceipt.Rows[rowIndex].Cells[dataField].Value != textbox.value) {
                GridReceipt.Rows[rowIndex].Cells[dataField].Value = textbox.value;
                if (parseInt(GridReceipt.Rows[rowIndex].Cells['ChallanQty'].Value) > parseInt(textbox.value)) {
                    GridReceipt.Rows[rowIndex].Cells['ShortQty'].Value = parseFloat(parseFloat(GridReceipt.Rows[RowIndex].Cells['ChallanQty'].Value) - parseFloat(textbox.value)).toFixed(2);
                    GridReceipt.Rows[rowIndex].Cells['ExcessQty'].Value = "0.00";
                }
                else if (parseInt(GridReceipt.Rows[rowIndex].Cells['ChallanQty'].Value) <= parseInt(textbox.value)) {
                    GridReceipt.Rows[rowIndex].Cells['ShortQty'].Value = "0.00";
                    GridReceipt.Rows[rowIndex].Cells['ExcessQty'].Value = parseFloat(parseFloat(textbox.value) - parseFloat(GridReceipt.Rows[RowIndex].Cells['ChallanQty'].Value)).toFixed(2);
                }

                var body = GridReceipt.GridBodyContainer.firstChild.firstChild.childNodes[1];
                var cell1 = body.childNodes[rowIndex].childNodes[8];
                cell1.firstChild.lastChild.innerHTML = parseFloat(GridReceipt.Rows[rowIndex].Cells['ShortQty'].Value).toFixed(2);;

                var cell1 = body.childNodes[rowIndex].childNodes[9];
                cell1.firstChild.lastChild.innerHTML = parseFloat(GridReceipt.Rows[rowIndex].Cells['ExcessQty'].Value).toFixed(2);

                PageMethods.WMUpdateReceiptQty(getOrderObject(rowIndex), null, null);
            }
        }

        function getOrderObject(rowIndex) {
            var order = new Object();
            order.Sequence = parseInt(GridReceipt.Rows[rowIndex].Cells['Sequence'].Value);
            order.ReceivedQty = parseFloat(GridReceipt.Rows[rowIndex].Cells['ReceivedQty'].Value).toFixed(2);
            return order;
        }

        /*End Request Part List*/
    </script>
   
    <%--End Page Load--%>
    <style type="text/css">
        /*Grid css*/
        .excel-textbox {
            background-color: transparent;
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

        .excel-textbox-focused {
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

        .excel-textbox-error {
            color: #FF0000;
        }

        .ob_gCc2 {
            padding-left: 3px !important;
        }

        .ob_gBCont {
            border-bottom: 1px solid #C3C9CE;
        }

        .excel-checkbox {
            height: 20px;
            line-height: 20px;
        }
    </style>
    <style type="text/css">
        /*POR Collapsable Div*/

        .PanelCaption {
            color: Black;
            font-size: 13px;
            font-weight: bold;
            margin-top: -22px;
            margin-left: -5px;
            position: absolute;
            background-color: White;
            padding: 0px 2px 0px 2px;
        }

        .divHead {
            border: solid 2px #F5DEB3;
            width: 99%;
            text-align: left;
        }

            .divHead h4 {
                /*color: #33CCFF;*/
                color: #483D8B;
                margin: 3px 3px 3px 3px;
            }

            .divHead a {
                float: right;
                margin-top: -15px;
                margin-right: 5px;
            }

                .divHead a:hover {
                    cursor: pointer;
                    color: Red;
                }

        .divDetailExpand {
            border: solid 2px #F5DEB3;
            border-top: none;
            width: 99%;
            padding: 5px 0 5px 0;
        }

        .divDetailCollapse {
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
        .has-js .label_check, .has-js .label_radio {
            padding-left: 25px;
            padding-bottom: 10px;
        }

        .has-js .label_radio {
            background: url(../App_Themes/Blue/img/radio-off.png) no-repeat;
        }

        .has-js .label_check {
            background: url("../App_Themes/Blue/img/check-off.png") no-repeat;
        }

        .has-js label.c_on {
            background: url("../App_Themes/Blue/img/check-on.png") no-repeat;
        }

        .has-js label.r_on {
            background: url(../App_Themes/Blue/img/radio-on.png) no-repeat;
        }

        .has-js .label_check input, .has-js .label_radio input {
            position: absolute;
            left: -9999px;
        }
    </style>
    <asp:HiddenField ID="hdnSelectedPrdId" runat="server" ClientIDMode="Static" />
    <script src="//code.jquery.com/jquery-1.10.2.js"></script>
    <script type="text/javascript">

        function ChkFun() {
            var SelectedProdval = document.getElementById("btn").value;
            var SelPos = document.getElementById("<%=SelectedPOs.ClientID %>").value;
            ProdId = SelectedProdval;
            //CHK PRODUCT EXIXS
        }
        function OnSuccessValidateProduct(result) {
            if (result == "0") {
                showAlert("Please Enter Product from Selected Purchase Orders Only!", 'error', '#');
                document.getElementById("btn").focus();
            }
            else if (result == 2) {
                var SelectedProdval = document.getElementById("btn").value;
                var SelPos = document.getElementById("<%=SelectedPOs.ClientID %>").value;
                ProdId = SelectedProdval;
                PageMethods.GetProdQty(ProdId, SelPos, OnSuccess, Onfail);
            }
    }
    function OnfailValidateProduct(result) {
        showAlert(result.get_message(), 'error', "#");
    }

    function OnSuccess(result) {
        //alert("Success");
        var SelectedProdval = document.getElementById("btn").value;
        document.getElementById("<%=TextBox1.ClientID%>").value = SelectedProdval;
            var ProdId = document.getElementById("<%=TextBox1.ClientID%>");
            document.getElementById("<%=txtReceiQty.ClientID%>").value = result;
            PageMethods.FillLocations(ProdId.value, OnSuccessFillLocations, OnfailFillLocations);
        }

        function Onfail(result) {
            showAlert(result.get_message(), 'error', "#");
        }

        function OnSuccessFillLocations(response) {

            // alert("successloc");
            DDLLoc.options.length = 0;

            for (var i in response) {
                AddOption(response[i].Name, response[i].Id);
            }
            //fill polist
            DDLLoc.options[1].selected = true;
            SetVal(1);
            var ProdId = document.getElementById("<%=TextBox1.ClientID%>");
            var POstr = document.getElementById("<%=SelectedPOs.ClientID%>");
            PageMethods.FillPOList(POstr.value, ProdId.value, OnSuccessFillPOList, OnfailFillPOList);
        }
        function OnSuccessFillPOList(response) {
            // alert(response);

            DDPOLst.options.length = 0;

            for (var i in response) {
                AddOptionPO(response[i].Name, response[i].Id);
            }
            DDPOLst.options[1].selected = true;
            SetVal(2);
        }
        function OnfailFillPOList(response) {
            alert(response.get_message());
        }
        function OnfailFillLocations(response) {
            alert(response.get_message());
        }
        function AddOption(text, value) {

            var option = document.createElement('option');
            option.value = value;
            option.innerHTML = text;
            DDLLoc.options.add(option);

        }

        function AddOptionPO(text, value) {

            var option = document.createElement('option');
            option.value = value;
            option.innerHTML = text;
            DDPOLst.options.add(option);
        }
        //------------------- Save Data----------------------------------//

        function jsSaveData() {
            PageMethods.WMSaveGridData("Save", WMSaveGridDataonSuccessed, WMSaveGridDataonFailed);
        }
        function WMSaveGridDataonSuccessed(response) {
            if (response != "") {
                showAlert("Products received successfully!", 'info', '#');
                //  document.getElementById("btnViewReport").disabled = false;
                window.GridReceipt.outerHTML = "";
            }
        }
        function WMSaveGridDataonFailed(response) {
            alert(response.get_message());
        }
        function SetVal(val) {
            var hdnPO = document.getElementById("<%=HdnPoId.ClientID%>");
            var hdnloc = document.getElementById("<%=HdnLoc.ClientID%>");
            var hdnloctxt = document.getElementById("<%=HdnLocTxt.ClientID%>");
            if (val == 1) {
                hdnloc.value = ddl.value;
                hdnloctxt.value = ddl.options[ddl.selectedIndex].text;
            }
            else if (val == 2) {
                hdnPO.value = ddlPOlst.value;
            }
            // alert(ddl.options[ddl.selectedIndex].text);
        }

        function ValidateC() {
            if (document.getElementById("<%=TextBox1.ClientID%>").value == "") {
                showAlert("Enter all mandatory fields", "Error", "#");
                document.getElementById("<%=TextBox1.ClientID%>").focus();
                return false;
            }
            if (document.getElementById("<%=txtReceiQty.ClientID%>").value == "") {
                showAlert("Enter all mandatory fields", "Error", "#");
                document.getElementById("<%=txtReceiQty.ClientID%>").focus();
                return false;
            }
            if (ddl.options[ddl.selectedIndex].text == "Select Location") {
                showAlert("Enter all mandatory fields", "Error", "#");
                ddl.focus();
                return false;
            }
            if (ddlPOlst.options[ddlPOlst.selectedIndex].text == "Select Purchase order") {
                showAlert("Enter all mandatory fields", "Error", "#");
                ddlPOlst.focus();
                return false;
            }
            return true;
        }

        function ChkQty() {
            var Qtyentered = document.getElementById("<%=txtReceiQty.ClientID%>").value;
            var hdnSelectedPutInRec = document.getElementById("hdnSelectedPutInRec");
            var obj1 = new Object();
            obj1.Sequence = hdnSelectedPutInRec.value;
            obj1.LocQty = Qtyentered;

            PageMethods.WMUpdatePutInList(obj1, null, null);

           // alert(Qtyentered);
            //alert(hdnSelectedPutInRec.value);

            //if (isNaN(Qtyentered)) {
           //     showAlert("Please enter correct quantity", "Error", "#");
           //     document.getElementById("<%=txtReceiQty.ClientID%>").focus();
          //      document.getElementById("<%=txtReceiQty.ClientID%>").value = "";
          //  }
          //  else {
         //       if (parseInt(Qtyentered) > parseInt(Qty)) {
          //          showAlert("Please enter correct quantity", "Error", "#");
         //           document.getElementById("<%=txtReceiQty.ClientID%>").focus();
         //           document.getElementById("<%=txtReceiQty.ClientID%>").value = "";
         //       }
         //   }
        }

        function Confirm() {
            var isLocationCodeZro = 'no';
            var match = 0;
            $(".divLocationCode span").each(function (i, html) {
                if (($(this).html() == '')) {
                    isLocationCodeZro = 'yes';
                }
            });

            var confirm_value = document.createElement("INPUT");
            confirm_value.type = "hidden";
            confirm_value.name = "confirm_value";
            if (isLocationCodeZro != 'yes') {
                if (confirm("This action will generate the sequence & will save Received Purchase Orders.Do you want to continue?")) {
                    confirm_value.value = "Yes";
                } else {
                    confirm_value.value = "No";
                }
                document.forms[0].appendChild(confirm_value);
            } else {
                confirm_value.value = "No";
            }
        }

        function getProductCode(Code) {
            var hdnval = document.getElementById("hdnSelectedPrdId");
            hdnval.value = Code;
            var TxtProduct = document.getElementById("btn");
            TxtProduct.value = Code;
            // PageMethods.webGetCustomerHeadDetailByCustomerID(AccountID, onSuccessGetAccountName, onFailedGetAccountDetails);
            ChkFun();
        }
        //////////////////////////////////////////////////////////////////
    </script>
</asp:Content>

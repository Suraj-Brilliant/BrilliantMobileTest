<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/CRM.Master" AutoEventWireup="true" CodeBehind="PickUp.aspx.cs" Inherits="BrilliantWMS.Warehouse.PickUp" 
    EnableEventValidation="false" Theme="Blue" %>

<%@ Register Assembly="obout_Grid_NET" Namespace="Obout.Grid" TagPrefix="obout" %>
<%@ Register Src="../CommonControls/Toolbar.ascx" TagName="Toolbar" TagPrefix="uc2" %>
<%@ Register Src="../CommonControls/UCFormHeader.ascx" TagName="UCFormHeader" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../CommonControls/UC_Date.ascx" TagName="UC_Date" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHolder_FormHeader" runat="server">
    <uc2:Toolbar ID="Toolbar1" runat="server" />
    <uc2:UCFormHeader ID="UCFormHeader1" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPHolder_Form" runat="server">
    <center>
        <span id="imgProcessing" style="display: none;">
            <img src="../App_Themes/Blue/img/ajax-loader.gif" style="top: 50%;" />
        </span>
        <div id="divLoading" style="height: 110%; width: 100%; display: none; top: 0; left: 0;"
            class="modal">
            <center>
            </center>
        </div>
        <%--  <div class="divHead">
            <h4>SO Group Detail
            </h4>
            <a onclick="javascript:divcollapsOpen('divRequestDetail',this)" id="linkRequest">Expand</a>
        </div>
        <div class="divDetailCollapse" id="divRequestDetail">
            <table class="tableForm" style="width: 1255px">
                <tr style="margin-bottom: 10px;">
                    <td style="margin-bottom: 10px;">
                        <b>Master Control Number :</b>
                    </td>
                    <td style="text-align: left;">
                        <asp:Label runat="server" ID="lblRequestNo" Text="NSO001"></asp:Label>

                        <b>Description :</b>

                        <asp:Label runat="server" ID="lblDesc" Text="Sales order Group"></asp:Label>
                    </td>

                </tr>
                <tr>
                    <td>
                        <b>SO Number :</b>
                    </td>
                    <td style="text-align: left;">
                        <asp:Label runat="server" ID="lblSites" Width="422px" Text="711650|711651|711652|"></asp:Label>

                    </td>
                </tr>
                <tr>
                    <td>
                        <b>Product Code :</b>
                    </td>
                    <td style="text-align: left;">
                        <asp:Label runat="server" ID="lblProdCode" Width="700px" Text="5-WS12007BR-022-SE-36C-0 | 5-WS12007BR-022-SE-36B-0 |  5-WS12007BR-022-SE-36A-0 | 5-WS12007BR-022-SE-34C-0"></asp:Label>
                    </td>

                </tr>
            </table>
        </div>--%>
        <div class="divHead" id="divReceiptDetailHead">
            <h4>Pick Up Detail
            </h4>
            <a onclick="javascript:divcollapsOpen('divReceiptDetail',this)" id="linkReceiptDetail">Collapse</a>
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
                    </td>           
                    <td style="text-align: center; width: 120px;">
                        <asp:Button ID="BtnClearGrid" runat="server" style="width:90px;" Text="Clear"  
                             Visible="false" /> <%--OnClick="BtnClearGrid_Click" --%>
                    </td>
                    <td style="text-align: center; width: 100px;">
                        <asp:Button ID="Button1" runat="server" style="width:90px;" Text="Submit"  
                             Visible="false" /> <%--OnClick="Button1_Click"--%> <%--OnClientClick="return ValidateC();"--%>
                    </td>                    
                </tr>
            </table>
            <table class="gridFrame" id="tblCart" width="94%">
                <tr>
                    <td>
                        <table style="width: 100%">
                            <tr>
                                <td style="text-align: left;">
                                    <a class="headerText">Pick Up Products </a>
                                </td>
                                <td style="text-align: right; width: 0px;">
                                    <input type="button" value="View Receipt" id="btnViewReport" onclick="selectImage();" />
                                </td>
                                <td style="text-align: right; width: 0px;">
                                    <asp:Button ID="BtnSequence" runat="server" OnClick="BtnSequence_Click" OnClientClick="Confirm();" Text="Save Pick Up"  />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <obout:Grid ID="GridReceipt" runat="server" AllowAddingRecords="false" AllowFiltering="true"
                            AllowGrouping="true" AutoGenerateColumns="false"  Width="100%"> <%--OnSelect="gvPrdPickUp_Select"--%>
                            <ClientSideEvents ExposeSender="true" />
                            <Columns>
                                <obout:Column ID="Column1" DataField="Edit" HeaderText="Edit" runat="Server" Width="5%" Align="center" HeaderAlign="center"
                                    AllowFilter="false">
                                    <TemplateSettings TemplateId="imgBtnEdit1" />
                                </obout:Column>
                                <obout:Column DataField="Sequence" HeaderText="Sequence No." AllowEdit="false" Width="7%"
                                    Align="center" HeaderAlign="center">
                                </obout:Column>
                                <obout:Column DataField="Code" HeaderText="Location Code" AllowEdit="false" Width="8%" HeaderAlign="center" Align="center">
                                    <TemplateSettings TemplateId="GTLocationCode" />
                                </obout:Column>
                                <obout:Column DataField="LocationBarCode" HeaderText="Location Bar Code" AllowEdit="false" Width="8%" HeaderAlign="center" Align="center">
                                    <TemplateSettings TemplateId="GTLocationBarCode" />
                                </obout:Column>
                              <obout:Column DataField="Capacity" HeaderText="Location Capacity" AllowEdit="false" Width="8%" HeaderAlign="center" Align="center" Wrap="true">
                                </obout:Column>
                                <obout:Column DataField="AvailableBalance" HeaderText="Current Stock" AllowEdit="false" Width="8%" HeaderAlign="center" Align="center" Wrap="true">
                                </obout:Column>
                                <obout:Column DataField="ProductCode" HeaderText="Product Code" AllowEdit="false" Width="10%"
                                    HeaderAlign="left">
                                </obout:Column>
                                <obout:Column DataField="OMSSKUCode" HeaderText="Product Bar Code" AllowEdit="false" Width="10%"
                                    HeaderAlign="left">
                                    <TemplateSettings TemplateId="GTProductBarCode" />
                                </obout:Column>
                                <obout:Column DataField="LocQty" HeaderText="Pick Up Qty." AllowEdit="false" Wrap="true"
                                    Width="7%" HeaderAlign="left">
                                </obout:Column>
                                <obout:Column DataField="BatchNo" HeaderText="BatchNo" AllowEdit="false" Wrap="true"
                                    Width="7%" HeaderAlign="left">
                                </obout:Column>
                                <%-- <obout:Column DataField="ID" HeaderText="Serial No" AllowEdit="false" Wrap="true"
                                    Width="7%" HeaderAlign="left">
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
                                <obout:GridTemplate runat="server" ID="grdSrNo">
                                    <Template>
                                        <%#Container.DataRecordIndex+1 %>
                                    </Template>
                                </obout:GridTemplate>
                                <obout:GridTemplate runat="server" ID="grdbarcodetemplate">
                                    <Template>
                                        <asp:Label runat="server" ID="lblgrdBarcode" Width="180px" Style="font-family: 'Free 3 of 9 Extended';"
                                            Font-Size="Large" Text="<%# Container.Value %>"></asp:Label>
                                    </Template>
                                </obout:GridTemplate>
                                <obout:GridTemplate runat="server" ID="imgBtnEdit1">
                                    <Template>
                                        <asp:ImageButton ID="imgBtnEdit" runat="server" ImageUrl="../App_Themes/Blue/img/Edit16.png"  OnClick="GridReceipt_Select"
                                            ToolTip="Edit" />
                                    </Template>
                                </obout:GridTemplate>
                                <obout:GridTemplate ID="GTLocationCode" runat="server">
                                    <Template>
                                        <div class="divLocationCode">
                                            <asp:Label ID="rowLocationCode" runat="server" Text="<%# Container.Value %>"></asp:Label> 
                                        </div>
                                    </Template>
                                </obout:GridTemplate>
                                <obout:GridTemplate ID="GTLocationBarCode" runat="server"  >
                                    <Template>
                                        <div class="divLocationBarCode" style="font-family:'3 of 9 Barcode'" >
                                            <asp:Label ID="rowLocationBarCode" runat="server" Text="<%# Container.Value %>"></asp:Label> 
                                        </div>
                                    </Template>
                                </obout:GridTemplate>
                                <obout:GridTemplate ID="GTProductBarCode" runat="server"  >
                                    <Template>
                                        <div class="divProductBarCode" style="font-family:'3 of 9 Barcode'">
                                            <asp:Label ID="rowProductBarCode" runat="server" Text="<%# Container.Value %>"></asp:Label> 
                                        </div>
                                    </Template>
                                </obout:GridTemplate>
                            </Templates>
                        </obout:Grid>
                    </td>
                </tr>
            </table>
        </div>
    </center>
    <asp:HiddenField ID="hdnSelectedPrdId" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnSelectedPickUpRec" runat="server" />
    <asp:HiddenField ID="HdnLoc" runat="server" Value="" />
    <asp:HiddenField ID="HdnLocTxt" runat="server" Value="" />
    <asp:HiddenField ID="HdnSoId" runat="server" Value="" />
    <asp:HiddenField ID="SelectedSOs" runat="server" Value="" />
    <asp:HiddenField ID="hdnLocationSearchSelectedRec" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnLocationSearchCapacity" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnLoactionSearchAvlblc" runat="server" ClientIDMode="Static" />

    <asp:HiddenField ID="MCNNo" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="SONO" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="PrdNo" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnPickUpNo" runat="server" ClientIDMode="Static" />

    <asp:DropDownList runat="server" ID="DDLSortcode" Width="160px" AccessKey="1" DataTextField="SortCode" DataValueField="ID"
        EnableTheming="True" Style="display: none;">
    </asp:DropDownList>
     <script src="//code.jquery.com/jquery-1.10.2.js"></script>
    <script type="text/javascript">

        function openProductSearch(sequence) {
            var SelectedSO = document.getElementById("<%=SelectedSOs.ClientID %>").value;
            window.open('../WMS/LocationSearch.aspx', null, 'height=700px, width=825px,status= 0, resizable= 1, scrollbars=1, toolbar=0,location=center,menubar=0, screenX=300; screenY=50');
        }

        function AfterLocationSelected() {
            var hdnLocationSearchSelectedRec = document.getElementById('hdnLocationSearchSelectedRec');
            var hdnLocationSearchCapacity = document.getElementById('hdnLocationSearchCapacity');
            var hdnLoactionSearchAvlblc = document.getElementById('hdnLoactionSearchAvlblc');
            
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
                if (confirm("This action will generate the sequence & will save Sales Orders Picked Products.Do you want to continue?")) {
                    confirm_value.value = "Yes";
                } else {
                    confirm_value.value = "No";
                }
                document.forms[0].appendChild(confirm_value);
            } else {
                confirm_value.value = "No";
            }
        }
    </script>
    <%--Receipt Head--%>
    <%--<script type="text/javascript">--%>


    <%--End Receipt Head--%>
    <%--Receipt Part Detail--%>
    <script type="text/javascript">

        function CloseShowReport() {
            LoadingOff();
            divPopUp.className = "divDetailExpandPopUpOff";
        }

        function selectImage() {
            var hdnPickUpNo = document.getElementById('hdnPickUpNo'); 
            PageMethods.WMShowReport(hdnPickUpNo.value, ShowReport_Onsuccess);
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

        .divDetailExpandPopUpOff {
            display: none;
        }

        .divDetailExpandPopUpOn {
            border: solid 3px gray;
            width: 90%;
            max-height: 500px;
            padding: 10px;
            background-color: #FFFFFF;
            margin-top: 50px;
            top: 5%;
            left: 3%;
            position: absolute;
            z-index: 99999;
        }

        .divDetailCollapse {
            display: none;
        }

        .popupClose {
            background: url(../App_Themes/Blue/img/icon_close.png) no-repeat;
            height: 32px;
            width: 32px;
            float: right;
            margin-top: -30px;
            margin-right: -25px;
        }

            .popupClose:hover {
                cursor: pointer;
            }

        .btnCommonStyle {
            font-family: inherit;
            //font-weight: bold;
            font-size: 15px;
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

        /*End POR Collapsable Div*/
    </style>

    <style type="text/css">
        .popupClose1 {
            background: url(../App_Themes/Blue/img/icon_close.png) no-repeat;
            height: 32px;
            width: 32px;
            float: right;
            margin-top: -30px;
            margin-right: -25px;
        }

        .popupClose:hover {
            cursor: pointer;
        }

        .divDetailExpandPopUpOff1 {
            display: none;
        }

        .divDetailExpandPopUpOn1 {
            border: solid 3px gray;
            width: 90%;
            height: 100%;
            padding: 10px;
            background-color: #FFFFFF;
            margin-top: 50px;
            top: 1%;
            left: 3%;
            position: absolute;
            z-index: 99999;
        }
    </style>
    <script type="text/javascript">       
        function ChkFun() {

            var SelPos = document.getElementById("<%=SelectedSOs.ClientID %>").value;
            ProdId = DDLProd.options[DDLProd.selectedIndex].text;
            PageMethods.GetProdQty(ProdId, SelPos, OnSuccess, Onfail)
        }        
        function Onfail(result) {
            alert(result.get_message());
        }

        function OnSuccessFillLocations(response) {

            // alert("successloc");
            DDLLoc.options.length = 0;

            for (var i in response) {
                AddOption(response[i].Name, response[i].Id);
            }
            //fill polist
           
            var SOstr = document.getElementById("<%=SelectedSOs.ClientID%>");
            PageMethods.FillSOList(SOstr.value, ProdId.value, OnSuccessFillSOList, OnfailFillSOList);
        }
        function OnSuccessFillSOList(response) {
            // alert(response);

            DDSOLst.options.length = 0;

            for (var i in response) {
                AddOptionSO(response[i].Name, response[i].Id);
            }
        }
        function OnfailFillSOList(response) {
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

        function AddOptionSO(text, value) {

            var option = document.createElement('option');
            option.value = value;
            option.innerHTML = text;
            DDSOLst.options.add(option);
        }
        //------------------- Save Data----------------------------------//

        function jsSaveData() {
            PageMethods.WMSaveGridData("Save", WMSaveGridDataonSuccessed, WMSaveGridDataonFailed);

        }
        function WMSaveGridDataonSuccessed(response) {
            if (response != "") {
                showAlert("Products received successfully", 'info', '#');
            }
        }
        function WMSaveGridDataonFailed(response) {
            alert(response.get_message());
        }
        function SetVal(val) {
            var hdnSO = document.getElementById("<%=HdnSoId.ClientID%>");
            var hdnloc = document.getElementById("<%=HdnLoc.ClientID%>");
            var hdnloctxt = document.getElementById("<%=HdnLocTxt.ClientID%>");
            if (val == 1) {
                hdnloc.value = ddl.value;
                hdnloctxt.value = ddl.options[ddl.selectedIndex].text;
            }
            else if (val == 2) {
                hdnSO.value = ddlSOlst.value;
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
            if (ddlSOlst.options[ddlSOlst.selectedIndex].text == "Select Sales order") {
                showAlert("Enter all mandatory fields", "Error", "#");
                ddlSOlst.focus();
                return false;
            }
            return true;
        }

        function ChkQty() {
            var Qtyentered = document.getElementById("<%=txtReceiQty.ClientID%>").value;
            var hdnSelectedPickUpRec = document.getElementById("hdnSelectedPickUpRec");
            var obj1 = new Object();
            obj1.Sequence = hdnSelectedPickUpRec.value;
            obj1.LocQty = Qtyentered;

            PageMethods.WMUpdatePickUpList(obj1, null, null);
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

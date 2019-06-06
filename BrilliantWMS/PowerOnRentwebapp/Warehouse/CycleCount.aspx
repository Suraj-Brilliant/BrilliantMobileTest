<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/CRM.Master" AutoEventWireup="true" EnableEventValidation="false"
   CodeBehind="CycleCount.aspx.cs" Inherits="BrilliantWMS.POR.CycleCount" Theme ="Blue" %>

<%@ Register Assembly="obout_Interface" Namespace="Obout.Interface" TagPrefix="cc1" %>
<%@ Register Assembly="obout_Grid_NET" Namespace="Obout.Grid" TagPrefix="obout" %>
<%@ Register Src="../CommonControls/UCFormHeader.ascx" TagName="UCFormHeader" TagPrefix="uc7" %>
<%@ Register Src="../CommonControls/UCToolbar.ascx" TagName="UCToolbar" TagPrefix="uc8" %>
<%@ Register Src="../Product/UCProductSearch.ascx" TagName="UCProductSearch" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="obout_Grid_NET" Namespace="Obout.Grid" TagPrefix="obout" %>
<%@ Register Src="../CommonControls/UC_Date.ascx" TagName="UC_Date" TagPrefix="uc3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHolder_FormHeader" runat="server">
    <uc8:UCToolbar ID="UCToolbar1" runat="server" />
    <uc7:UCFormHeader ID="UCFormHeader1" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPHolder_Form" runat="server">
    <span id="imgProcessing" style="display: none;">Please wait... </span>
    <center>
      <asp:TabContainer runat="server" ID="tabChannelMaster" ActiveTabIndex="2" Width="100%">
        <asp:TabPanel ID="TabChannelList" runat="server" HeaderText="Cycle Count List">
                <ContentTemplate>
                   <asp:HiddenField ID="hdnlocationcode" runat="server" ClientIDMode="Static" />
                   <asp:HiddenField ID="hdnSelectedPrd" runat="server" ClientIDMode="Static" />
                    <asp:HiddenField ID="hdnSelectedPrdId" runat="server" ClientIDMode="Static" />
                     <asp:HiddenField ID="hdnselectedLocID" runat="server" ClientIDMode="Static" />

                     <asp:HiddenField ID="hdnloctext" runat="server" ClientIDMode="Static" />
           <asp:HiddenField ID="SystemQty" runat="server" ClientIDMode="Static" />
           <asp:HiddenField ID="Productadjust" runat="server" ClientIDMode="Static" />
           <asp:HiddenField ID="hdnCycleCountTitle" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hdnwarehouseId" runat="server" ClientIDMode="Static" />
             <asp:HiddenField ID="hdnCompanyid" runat="server"></asp:HiddenField>
             <asp:HiddenField ID="hdnstate" runat="server" ClientIDMode="Static"></asp:HiddenField>
             <asp:HiddenField ID="hdnCycleheadID" runat="server" ClientIDMode="Static"></asp:HiddenField> 
              <asp:HiddenField ID="hdnlocationSearchName" runat="server" ClientIDMode="Static"></asp:HiddenField> 
              <asp:HiddenField ID="hdnLocationSearchID" runat="server" ClientIDMode="Static"></asp:HiddenField>  
              <asp:HiddenField ID="hdnproductCode" runat="server" ClientIDMode="Static"></asp:HiddenField> 
              <asp:HiddenField ID="hdnProductID" runat="server" ClientIDMode="Static"></asp:HiddenField>    
               <asp:HiddenField ID="hdnfrequency" runat="server" ClientIDMode="Static"></asp:HiddenField>                           
                    <center>
                        <table class="gridFrame" style="width: 100%">
                            <tr>
                                <td>
                                    <table style="width: 100%">
                                        <tr>
                                            <td style="text-align: left;">
                                                <a id="headerText">
                                                    <asp:Label ID="lblheasertext" runat="server" Text="Cycle Count List"></asp:Label></a>
                                            </td>
                                            <td style="text-align: right;">
                                                 <input type="button" id="btnContactPerson" runat="server" value="Add New" onclick="OpenCycleCountHead()" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <obout:Grid ID="grdcyclecount" runat="server" AutoGenerateColumns="False" AllowAddingRecords="False"
                                        AllowGrouping="True" AllowFiltering="True" Width="100%" OnSelect="grdcyclecount_Select" OnRebind="grdcyclecount_RebindGrid" >
                                        <ScrollingSettings ScrollHeight="250" />
                                        <Columns>
                                            <obout:Column ID="Edit" Width="6%" AllowFilter="False" Align="center" HeaderAlign="center" HeaderText="Edit" Index="0"
                                                TemplateId="GvTempEdit">
                                                <TemplateSettings TemplateId="GvTempEdit" />
                                            </obout:Column>
                                            <obout:Column DataField="ID" Visible="False" HeaderText="ID" Index="1">
                                            </obout:Column>
                                             <obout:Column DataField="Title" HeaderText="Title" Width="15%" Index="2">
                                            </obout:Column>
                                            <obout:Column HeaderText="Date" DataField="CycleCountDate" Width="10%" Index="3" DataFormatString="{0:dd-MM-yyyy}">
                                            </obout:Column>
                                            <obout:Column HeaderText="Frequency" DataField="Frequency" Width="10%" Index="4">
                                            </obout:Column>
                                             <obout:Column DataField="CountBasis" HeaderText="CountBasis" Width="12%" Index="5">
                                            </obout:Column>
                                            <obout:Column DataField="Status" HeaderText="Status" Width="8%" Index="6">
                                            </obout:Column>
                                             <obout:Column DataField="Active" HeaderText="Active" Width="7%" Index="7">
                                            </obout:Column>
                                        </Columns>
                                        <Templates>
                                            <obout:GridTemplate ID="GvTempEdit" runat="server" ControlID="" ControlPropertyName="">
                                                <Template>
                                                    <asp:ImageButton ID="imgBtnEdit" ToolTip="Edit" CausesValidation="false" runat="server"
                                                        ImageUrl="../App_Themes/Blue/img/Edit16.png" />
                                                </Template>
                                            </obout:GridTemplate>
                                        </Templates>
                                    </obout:Grid>
                                </td>
                            </tr>
                        </table>
                    </center>
                </ContentTemplate>
            </asp:TabPanel>

        <asp:TabPanel ID="tabChannelInfo" runat="server" HeaderText="Cycle Count Info">
                <ContentTemplate>
                    <center>
       <table cellpadding="0" cellspacing="15px" border="0">
                     
                <tr>
                    <td style="font-size: 17px">
                         <%--Select Title:
                            <asp:DropDownList ID="DrpCycleCounttitle" runat="server" Width="212px" AutoPostBack="True" DataTextField ="Title" DataValueField="ID">                             
                            </asp:DropDownList>                       
                            <input style="position:relative; top:-2px; font-size:17px"  type="button" id="btnNext" value=" Add New " runat="server" onclick="openLocation('0')"/>--%>
                        <center>
                        <table class="tableForm">
                            <tr>
                                 <td>
                                    <req><asp:Label ID="lbltitle" runat="server" Text="Title:"></asp:Label></req>
                                </td>
                                <td style="text-align: left">
                                    <asp:TextBox ID="txtTitle" runat="server"  Width="300px"></asp:TextBox>

                                </td>
                                <td>
                                <req><asp:Label Id="lblwarehouse" runat="server" Text="Warehouse"></asp:Label></req> :
                            </td>
                            <td style="text-align: left">
                                <asp:DropDownList ID="ddlWarehouse" runat="server"  DataTextField="WarehouseName" DataValueField="ID" onchange="GetWarehouse()" Width="206px">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RfValCompany" runat="server" ControlToValidate="ddlWarehouse" InitialValue="0"
                                        Display="None" ErrorMessage="Please Select Waehouse" ValidationGroup="Save"></asp:RequiredFieldValidator>
                            </td>
                               <td>
                                       <req><asp:Label ID="lblfrmDate" runat="server" Text=" Date " /></req> :
                                 </td>
                                 <td style="text-align: left;">
                                       <uc3:UC_Date ID="UC_FromDate" runat="server" />
                                 </td>
                               
                                </tr>
                            <tr>
                                <td>
                                    <req><asp:Label Id="lblexecutive" runat="server" Text="Cycle Count By"></asp:Label></req>
                                    :
                                </td>
                                <td style="text-align: left">
                                   <asp:TextBox ID="txtexicutive" runat="server"  Width="200px"></asp:TextBox>
                                </td>
                                 <td>
                                    <req><asp:Label Id="lblstatus" runat="server" Text="Status"/></req>
                                    :
                                </td>
                                <td style="text-align: left">
                                     <asp:DropDownList ID="ddlstatus" runat="server" DataTextField="Name" DataValueField="ID" Width="206px">
                                      <asp:ListItem Text="Planned" Value="1"></asp:ListItem>
                                      <asp:ListItem Text="Completed" Value="3"></asp:ListItem>
                                </asp:DropDownList>
                                </td>
                              <td>
                                    <req><asp:Label ID="lblcountbasis" runat="server" Text="Count Basis:"></asp:Label></req>
                                </td>
                                <td style="text-align: left">
                                   <asp:DropDownList ID="ddlcountbasis" runat="server"  DataTextField="Name" DataValueField="ID" Width="206px" onchange="Showgrid()">
                                         <asp:ListItem Text="-Select-" Value="0"></asp:ListItem>
                                         <asp:ListItem Text="Product" Value="Product"></asp:ListItem>
                                         <asp:ListItem Text="Location" Value="Location"></asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlcountbasis" InitialValue="0"
                                        Display="None" ErrorMessage="Please Select Executive" ValidationGroup="Save"></asp:RequiredFieldValidator>

                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <req>
                                    <asp:Label Id="lblactive" runat="server" Text="Active"/>
                                    </req>
                                    :
                                </td>
                                <td style="text-align: left">
                                    <cc1:OboutRadioButton ID="rbtnActiveYes" runat="server" Checked="True" FolderStyle=""
                                        GroupName="Active" Text="Yes">
                                    </cc1:OboutRadioButton>
                                    <cc1:OboutRadioButton ID="rbtnActiveNo" runat="server" FolderStyle="" GroupName="Active"
                                        Text="No">
                                    </cc1:OboutRadioButton>
                                </td>
                            </tr>
                            </table>
                            </center>
                   </td>
                </tr>
                <tr>
                <td>
                <table class="tableForm" style="width: 1295px">
                <tr>
               <td>
                        Location*:
                    </td>
                    <td style="text-align: left;">
                        <asp:TextBox runat="server" ID="txtlocation" Width="190px" MaxLength="100" ></asp:TextBox>
                       
                        <img id="imgSearch" src="../App_Themes/Grid/img/search.jpg" title="Search Product"
                            style="cursor: pointer;" onclick="openProductSearch('0')"/>
                             &nbsp<asp:Label runat="server" ID="lblBarcode" Width="2px" Visible="false"  Style="font-family: '3 of 9 Barcode';" Font-Size="X-Large" Text=""></asp:Label>
                    </td>
                     <td>
                        Product:
                    </td>
                    <td colspan="" style="text-align: left;">
                       <asp:TextBox runat="server" ID="txtproduct" Width="190px" ></asp:TextBox>
                        <img id="img1" src="../App_Themes/Grid/img/search.jpg" title="Search Product"
                            style="cursor: pointer;" onclick="openSkuSearchCycle('0')"/>
                    </td>
                     <td>
                        Batch Code:
                    </td>
                    <td colspan="" style="text-align: left;">
                       <asp:DropDownList ID="ddlbatch" runat="server"  DataTextField="BatchCode" DataValueField="ID" Width="190px">
                                    </asp:DropDownList>
                    </td>
                    <td>
                        Quantity*:
                    </td>
                    <td style="text-align: left;">
                        <asp:TextBox runat="server" ID="txtactualQty" Width="65px"  Style="text-align: right"></asp:TextBox> <%-- onchange="RemainingQty()"--%>
                    </td>
                     <td style="text-align: right;">
                       <%-- <asp:Button ID="Button1" runat="server" style="position:relative; font-size:17px;  top:-4px;" Text="   Add   " 
                            OnClick="Button1_Click" />--%>
                          <input type="button" runat="server" value="Submit" id="btnSubmitProductSearch1" onclick="SaveCycleCount();" />
                    </td>
                </tr>
                </table>
                </td>
                </tr>
              
            </table>
       <table class="gridFrame" width="98%" id="tblCart" style="margin: 40px 20px 20px 20px;">
        <tr>
        <td>
           <table style="width: 100%">
            <tr>
                <td>
                  <b> Cycle Count Products </b>
                </td>
               <td style="text-align: right;">
                    <%--<uc1:UCProductSearch ID="UCProductSearch1" runat="server" />--%>
                </td>
            </tr>
             </table>
            </td>
            </tr>
           <tr>
                <td colspan="2">
                       <obout:Grid ID="grdImportView" runat="server" AllowGrouping="true" Serialize="false"
                        CallbackMode="true" AllowMultiRecordSelection="false" AllowColumnReordering="true"
                        AllowFiltering="false" Width="100%" PageSize="-1" AllowAddingRecords="False" AutoGenerateColumns="False" OnRebind="grdImportView_RebindGrid" >
                     <%--  <ClientSideEvents ExposeSender="true" />--%>
                        <Columns>
                            <obout:Column DataField="" HeaderText="Edit/Adjustment" HeaderAlign="Center" Align="Center" Width="3%">
                                <TemplateSettings TemplateId="GridTemplate1" />
                            </obout:Column>
                            <obout:Column DataField="" HeaderText="Sr.No." HeaderAlign="left" Align="left" Width="2%">
                                <TemplateSettings TemplateId="grdSrNo" />
                            </obout:Column>
                             <obout:Column DataField="ID" HeaderText="ID" HeaderAlign="left"
                                Align="left" Width="1%" Visible="false">
                            </obout:Column>
                            <obout:Column DataField="LocationCode" HeaderText="Location Code" HeaderAlign="left"
                                Align="left" Width="3%">
                            </obout:Column>
                            <obout:Column DataField="ProductCode" HeaderText="Product Code" HeaderAlign="left"
                                Align="left" Width="7%">
                            </obout:Column>
                             <obout:Column DataField="BatchCode" HeaderText="Batch Code" HeaderAlign="left"
                                Align="left" Width="5%">
                            </obout:Column>
                            <obout:Column DataField="QtyBalance" HeaderText="System Qty" HeaderAlign="left" Align="left"
                                Width="3%">
                               <%-- <TemplateSettings TemplateId="QtyBalance" />--%>
                            </obout:Column>
                            <obout:Column DataField="ActualQty" HeaderText="Actual Qty" HeaderAlign="left" Align="left"
                                Width="3%">
                                <%--<TemplateSettings TemplateId="txtgrdadqty" />--%>
                            </obout:Column>
                            <obout:Column DataField="DiffQty" HeaderText="Difference Qty" HeaderAlign="left"
                                Align="left" Width="3%">
                                </obout:Column>
                            <obout:Column DataField="AdjustmentQty" HeaderText="Adjustment Qty" HeaderAlign="left"
                                Align="left" Width="3%">
                                </obout:Column>
                            <obout:Column DataField="AdjustLocation" HeaderText="Adjustment Loc" HeaderAlign="left"
                                Align="left" Width="3%">
                            </obout:Column>
                            <obout:Column DataField="Remark" HeaderText="Remark" HeaderAlign="left" Align="left"
                                Width="9%">
                               <%-- <TemplateSettings TemplateId="gerdtemplateremark" />--%>
                            </obout:Column>
                        </Columns>
                        <Templates>
                              <obout:GridTemplate runat="server" ID="gerdtemplateremark">
                                <Template>
                                    <asp:TextBox ID="txtgrdremark" runat="server" Width="231px"></asp:TextBox>
                                </Template>
                             </obout:GridTemplate>
                            <%--<obout:GridTemplate runat="server" ID="GridTemplate2">
                                <Template>
                                    <asp:TextBox ID="txtgrdremark" runat="server" Width="231px"></asp:TextBox>
                                </Template>
                            </obout:GridTemplate>--%>
                            <obout:GridTemplate runat="server" ID="GridTemplate1">
                                <Template>
                                    <img alt="" src="../App_Themes/Grid/img/Edit.png" onclick="SelectedRecProdEdit()" onmouseover="Adjustment"/>
                                    <img alt="" src="../App_Themes/Grid/img/Adjustment.png" onclick="SelectedRecProd()" onmouseover="Adjustment"/>
                                </Template>
                            </obout:GridTemplate>
                            <obout:GridTemplate runat="server" ID="grdSrNo">
                                <Template>
                                    <%#Container.DataRecordIndex+1 %>
                                </Template>
                            </obout:GridTemplate>
                        </Templates>
                    </obout:Grid>
                </td>
            </tr>
        </table>
                      </center>
                </ContentTemplate>
            </asp:TabPanel>
       
         </asp:TabContainer>
</center>
    <script type="text/javascript">
        var productcode = document.getElementById("<%=txtlocation.ClientID%>");
        var DDLLoc = document.getElementById("<%=txtlocation.ClientID %>");
        var SysQty = document.getElementById("<%=txtlocation.ClientID%>");
        var ActQty = document.getElementById("<%=txtactualQty.ClientID %>");
        var Diffrence = document.getElementById("<%=txtlocation.ClientID %>");
        var CountTytle = document.getElementById("<%=txtTitle.ClientID %>");

        function savetemptable() {

            var obj1 = new Object();
            obj1.productcode = productcode.value.toString();
            //obj1.DDlLoc = parseInt(DDLLoc.options[DDLLoc.selectedIndex].value);
            obj1.SysQty = SysQty.innerHTML;
            obj1.ActQty = ActQty.value.toString();
            obj1.Diffrence = Diffrence.value.toString();
            PageMethods.savetempdata(obj1);
        }

        //        window.onload = function () {
        //            oboutGrid.prototype.restorePreviousSelectedRecord = function () {
        //                return;
        //            }
        //            oboutGrid.prototype.markRecordAsSelectedOld = oboutGrid.prototype.markRecordAsSelected;
        //            oboutGrid.prototype.markRecordAsSelected = function (row, param2, param3, param4, param5, param6, param7, param8) {
        //                if (row.className != this.CSSRecordSelected) {
        //                    this.markRecordAsSelectedOld(row, param2, param3, param4, param5, param6, param7, param8);

        //                } else {
        //                    var index = this.getRecordSelectionIndex(row);
        //                    if (index != -1) {
        //                        this.markRecordAsUnSelected(row, index);
        //                    }
        //                }

        //                SelectedRec();
        //            }
        //        }



        function SelectedRecProd() {

            var Productadjust = document.getElementById("Productadjust");
            Productadjust.value = "";
            var record = grdImportView.SelectedRecords[0];
            Productadjust.value = record.ID;
            var CycleHeadID = document.getElementById("<%= hdnCycleheadID.ClientID %>").value;
            if (Productadjust.value != "") {
                window.open('../Warehouse/CycleAdjustmentEntry.aspx?Sequence=' + Productadjust.value + '&CountHeadID=' + CycleHeadID + '', null, 'height=200, width=1050,status= 0, resizable= 1, scrollbars=0, toolbar=0,location=center,menubar=0, screenX=300; screenY=50');
            }
        }

        function SelectedRecProdEdit() {

            var Productadjust = document.getElementById("Productadjust");
            Productadjust.value = "";
            var record = grdImportView.SelectedRecords[0];
            Productadjust.value = record.ProductCode;
            var CountTitle = parseInt(CountTytle.options[CountTytle.selectedIndex].value);
            var Edit = "Edit";
            if (Productadjust.value != "") {
                window.open('../POR/CycleAdjustmentEntry.aspx?Sequence=' + Productadjust.value + '&CountHeadID=' + CountTitle + '&Edit=' + Edit + '', null, 'height=200, width=1050,status= 0, resizable= 1, scrollbars=0, toolbar=0,location=center,menubar=0, screenX=300; screenY=50');
            }
        }

        //        function SelectedRec() {
        //            var hdnSelectedPrd = document.getElementById("hdnSelectedPrd");
        //            hdnSelectedPrd.value = "";
        //            for (i = 0; i < grdImportView.PageSelectedRecords.length; i++) {
        //                var record = grdImportView.PageSelectedRecords[i];
        //                if (hdnSelectedPrd.value != "") hdnSelectedPrd.value += ',' + record.ProductCode;
        //                if (hdnSelectedPrd.value == "") hdnSelectedPrd.value = record.ProductCode;
        //             }
        //             if (hdnSelectedPrd.value != "") {
        //                 window.open('../POR/CycleAdjustmentEntry.aspx?Sequence=' + hdnSelectedPrd.value + '', null, 'height=200, width=1050,status= 0, resizable= 1, scrollbars=0, toolbar=0,location=center,menubar=0, screenX=300; screenY=50');
        //             }
        //        }

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
            if (GridReceipt.Rows[rowIndex].Cells[dataField].Value != textbox.value) {
                GridReceipt.Rows[rowIndex].Cells[dataField].Value = textbox.value;
                if (parseInt(GridReceipt.Rows[rowIndex].Cells['SystemQty'].Value) > parseInt(textbox.value)) {
                    GridReceipt.Rows[rowIndex].Cells['AdjustmentQty'].Value = parseFloat(parseFloat(GridReceipt.Rows[RowIndex].Cells['SystemQty'].Value) - parseFloat(textbox.value)).toFixed(2);
                }
            }
        }


       


        //        function WMCheckProduct_onSuccessed(result) {
        //            if (result == "1") {
        //                showAlert("Product Already Exist", "Error", '#');
        //                document.getElementById("<%=txtlocation.ClientID%>").value == "";
        //                productcode.value = "";
        //                document.getElementById("<%=txtlocation.ClientID%>").focus();
        //            }
        //            else {
        //                var txtProductCode = document.getElementById("<%= txtlocation.ClientID %>").value.trim();
        //               PageMethods.CheckSONumber(txtProductCode, WMCheckPO_onSuccessed);  
        //                 }
        //        }



       


        function AfterProductSelected(locCode, locID) {
            var hdnval = document.getElementById("<%=hdnSelectedPrdId.ClientID %>");
            var locationID = document.getElementById("<%=hdnselectedLocID.ClientID %>");
            locationID.value = locID;
            hdnval.value = locCode;
            var TxtProduct = document.getElementById("<%=txtlocation.ClientID %>");
            TxtProduct.value = locCode;
             JSBarCode();
        }

        function JSBarCode() {
            var txtlocationCode = document.getElementById("<%= txtlocation.ClientID %>").value.trim();
             var CountBasis = document.getElementById("<%= ddlcountbasis.ClientID %>").value;
             var CycleheadID = document.getElementById("<%= hdnCycleheadID.ClientID %>").value;
             var WarehouseID = document.getElementById("<%= hdnwarehouseId.ClientID %>").value;

             if (CountBasis == "Location") {
                 PageMethods.CheckLocInPlan(txtlocationCode, CycleheadID, WarehouseID, CountBasis, WMCheckPO_onSuccessed);
             }
             else {
                // var lblBarCode = document.getElementById("<%=lblBarcode.ClientID %>");
                // lblBarCode.innerHTML = txtlocationCode;
            }
        }

        function WMCheckPO_onSuccessed(result) {
            var arr = result.split("-");
            var Value = arr[0];
            var LocationID = arr[1];
            if (Value == "Not In Plan") {
                if (confirm('Location Not In Plan do You Want to continue ?')) {
                    var txtlocationcode = document.getElementById("<%=txtlocation.ClientID %>");
                   // var lblBarCode = document.getElementById("<%=lblBarcode.ClientID %>");
                    //  lblBarCode.innerHTML = txtlocationcode.value;
                    document.getElementById("<%=hdnselectedLocID.ClientID %>").value = LocationID;
                } else {
                     document.getElementById("<%=txtlocation.ClientID %>").value = "";
                }
               
            }
            else if (Value == "Location Not Found")
            {
                showAlert("Location Not Available in System", "Error", "#");
                document.getElementById("<%=txtlocation.ClientID %>").value = "";
            }
            else {
                var txtlocationcode = document.getElementById("<%=txtlocation.ClientID %>");
               // var lblBarCode = document.getElementById("<%=lblBarcode.ClientID %>");
                // lblBarCode.innerHTML = txtlocationcode.value;
                document.getElementById("<%=hdnselectedLocID.ClientID %>").value = LocationID;
            }
        }

        function AfterSKUSelected(SKUCode,SKUId)
        {
            var hdnproductCode  = document.getElementById("<%=hdnproductCode.ClientID %>");
            var hdnProductID = document.getElementById("<%=hdnProductID.ClientID %>");
            hdnProductID.value = SKUId;
            var txtproduct = document.getElementById("<%=txtproduct.ClientID %>");
            txtproduct.value = SKUCode;
            document.getElementById("<%=txtactualQty.ClientID %>").value = 1;
            CheckSKUinPlan();
           // SaveCycleCount();
        }

        function CheckSKUinPlan()
        {
            var txtproduct = document.getElementById("<%=txtproduct.ClientID %>").value;
            var CycleheadID = document.getElementById("<%= hdnCycleheadID.ClientID %>").value;
            var WarehouseID = document.getElementById("<%= hdnwarehouseId.ClientID %>").value;
            var CountBasis = document.getElementById("<%= ddlcountbasis.ClientID %>").value;
            var locationID = document.getElementById("<%=hdnselectedLocID.ClientID %>").value;
            if (CountBasis == "Product") {
                PageMethods.CheckSKUInPlan(txtproduct, CycleheadID, WarehouseID, CountBasis, locationID, WMCheckSKU_onSuccessed);
            }
            else {
                GetbatchCode();
            }
        }

        function WMCheckSKU_onSuccessed(result) {
            var arr = result.split("-");
            var Value = arr[0];
            var SKUID = arr[1];
            if (Value == "Not In Plan") {
                if (confirm('SKU Not In Plan do You Want to continue ?')) {
                    document.getElementById("<%=hdnProductID.ClientID %>").value = SKUID;
                    GetbatchCode();

                } else {
                    document.getElementById("<%=txtproduct.ClientID %>").value = "";
                    document.getElementById("<%=txtactualQty.ClientID %>").value = "";
                }

            }
            else if (Value == "SKU Not Found") {
                showAlert("SKU Not Available in System", "Error", "#");
                document.getElementById("<%=txtproduct.ClientID %>").value = "";
                document.getElementById("<%=txtactualQty.ClientID %>").value = "";
            }
            else {
                var txtlocationcode = document.getElementById("<%=txtlocation.ClientID %>");
                document.getElementById("<%=hdnProductID.ClientID %>").value = SKUID;
                GetbatchCode();
            }
        }

        var ddlbatchcode = document.getElementById("<%= ddlbatch.ClientID %>");
        function GetbatchCode()
        {
            var obj1 = new Object();
            var WarehouseID = document.getElementById("<%= hdnwarehouseId.ClientID %>");
            var SKUID = document.getElementById("<%=hdnProductID.ClientID %>");
            var locationID = document.getElementById("<%=hdnselectedLocID.ClientID %>");
            obj1.WarehouseID = WarehouseID.value.toString();
            obj1.SKUID = SKUID.value.toString();
            obj1.locationID = locationID.value.toString();
            PageMethods.GetBatchCodeBySKU(obj1, getBatch_onSuccessed);

        }

        function getBatch_onSuccessed(result) {

            ddlbatchcode.options.length = 0;
            for (var i in result) {
                AddOption12(result[i].Name, result[i].Id);
            }
        }

        function AddOption12(text, value) {

            var option = document.createElement('option');
            option.value = value;
            option.innerHTML = text;
            ddlbatchcode.options.add(option);
        }



        function SaveCycleCount()
        {
            var CycleheadID = document.getElementById("<%= hdnCycleheadID.ClientID %>");
            var WarehouseID = document.getElementById("<%= hdnwarehouseId.ClientID %>");
            var hdnProductID = document.getElementById("<%=hdnProductID.ClientID %>");
            var locationID = document.getElementById("<%=hdnselectedLocID.ClientID %>");
            var Quantity = document.getElementById("<%=txtactualQty.ClientID %>");
            var txtproduct = document.getElementById("<%=txtproduct.ClientID %>");
            var txtlocationCode = document.getElementById("<%= txtlocation.ClientID %>");
            var BatchCode = document.getElementById("<%= ddlbatch.ClientID %>");

            if (document.getElementById("<%=txtlocation.ClientID %>").value == "") {
                showAlert("Please Enter or Select Location!", "Error", "#");
                document.getElementById("<%=txtlocation.ClientID %>").focus();
            }
            else if (document.getElementById("<%=txtproduct.ClientID %>").value == "") {
                showAlert("Please Enter or Select SKU!", "Error", "#");
                 document.getElementById("<%=txtproduct.ClientID %>").focus();
            }
            else if (document.getElementById("<%=ddlbatch.ClientID %>").value == "0") {
                showAlert("Please Select Batch!", "Error", "#");
                document.getElementById("<%=ddlbatch.ClientID %>").focus();
            }
            else if (document.getElementById("<%=txtactualQty.ClientID %>").value == "") {
                showAlert("Please Enter Quantity!", "Error", "#");
                document.getElementById("<%=txtactualQty.ClientID %>").focus();
            }
            else {
                var obj = new Object();
                obj.CycleheadID = CycleheadID.value;
                obj.txtlocationCode = txtlocationCode.value;
                obj.locationID = locationID.value;
                obj.txtproduct = txtproduct.value;
                obj.hdnProductID = hdnProductID.value;
                obj.Quantity = Quantity.value;
                obj.WarehouseID = WarehouseID.value;
                obj.BatchCode = BatchCode.options[BatchCode.selectedIndex].text;
                PageMethods.PMSaveCycleCount(obj, SubmitCycleCount_onSuccess, SubmitCycleCount_onFail);
            }

        }

        function SubmitCycleCount_onSuccess(result) {
            if (result == "Some error occurred" || result == "") { showAlert("Error occurred", "Error", '#'); }
              else if (result == "Successfully Submitted") {
                showAlert(result, "info");
                document.getElementById("<%= hdnProductID.ClientID %>").value = "";
                //  document.getElementById("<%= hdnselectedLocID.ClientID %>").value = "";
                document.getElementById("<%= txtactualQty.ClientID %>").value = "";
                document.getElementById("<%= txtproduct.ClientID %>").value = "";
                // document.getElementById("<%= txtlocation.ClientID %>").value = "";
                // document.getElementById("<%= lblBarcode.ClientID %>").innerHTML = "";
                document.getElementById("<%= ddlbatch.ClientID %>").value = "0";

                grdImportView.refresh();
            }
        }

        function SubmitCycleCount_onFail() {
            showAlert("Error occurred", "Error", "#");
        }




         function getCycleTitleCode(Code) {
             var hdnval = document.getElementById("hdnDrpTitle");
             hdnval.value = Code;
             document.getElementById("hdnDrpTitle").value = Code;
         }


         var ddlLocation = document.getElementById("<%= txtlocation.ClientID %>");
        var productCodeQty = document.getElementById("<%=txtlocation.ClientID %>");

        function LocationQty() {
            var CountTitle = parseInt(CountTytle.options[CountTytle.selectedIndex].value);
            var txtProductCode = document.getElementById("<%= txtlocation.ClientID %>").value.trim();
            var loccodechkprod = ddlLocation.options[ddlLocation.selectedIndex].text;

              PageMethods.CheckProduct(txtProductCode, CountTitle, loccodechkprod, WMCheckProduct_onSuccessed);
          }


          function WMCheckProduct_onSuccessed(result) {
              if (result == "1") {
                  showAlert("Product Already Exist", "Error", '#');
                  document.getElementById("<%=txtlocation.ClientID%>").value = 0;
            }
            else {
                var hdnloctext = document.getElementById("hdnloctext");   //add by suresh for crossbrowsing
                hdnloctext.value = ddlLocation.options[ddlLocation.selectedIndex].text;
                var obj1 = new Object();
                obj1.LocationCode = parseInt(ddlLocation.options[ddlLocation.selectedIndex].value);
                obj1.ProductCodeLoc = productCodeQty.value;
                PageMethods.GetlocationQty(obj1, WMGet_LocQty);
            }
        }

        function WMGet_LocQty(result) {
          

        }

        function Onfail(result) {
            alert(result.get_message());
        }

        function RemainingQty() {
           
             var actQty = document.getElementById("<%=txtactualQty.ClientID %>").value;
          
         }



         function OnSuccessFillLocations(result) {

             // alert("successloc");
             DDLLoc.options.length = 0;

             for (var i in result) {
                 AddOption(result[i].Name, result[i].Id);
             }
         }

         function AddOption(text, value) {

             var option = document.createElement('option');
             option.value = value;
             option.innerHTML = text;
             DDLLoc.options.add(option);
         }

         function getProductCodeCycleTitle(Code) {
             var hdnval = document.getElementById("hdnCycleCountTitle");
             hdnval.value = Code;
             var CountTytle = CountTytle.options[CountTytle.selectedIndex].text;
             CountTytle.value = Code;
         }

         // code for clear button to clear the values n grid
         function jsAddNew() {
             var CountTitle = parseInt(CountTytle.options[CountTytle.selectedIndex].value);
             document.getElementById("hdnCycleCountTitle").value = "";
             document.getElementById("hdnDrpTitle").value = "";
             CountTytle.selectedIndex.value = 0;
             window.open('../POR/CycleCount.aspx', '_self', '');
             //PageMethods.WMpageAddNew(CountTitle,jsAddNewOnSuccess);
         }
         function jsAddNewOnSuccess() {
             window.open('../POR/CycleCount.aspx', '_self', '');
         }

         // code to save data in transactiontable
         function jsSaveData() {
             var CountTitle = parseInt(CountTytle.options[CountTytle.selectedIndex].value);
             PageMethods.WMSaveRequestHead(CountTitle, WMSaveRequestHead_onSuccessed, WMSaveRequestHead_onFailed);
         }

         function WMSaveRequestHead_onSuccessed(result) {

             if (result == "Some error occurred" || result == "") { showAlert("Error occurred", "Error", '#'); }
             if (result == "Cycle Count with Same data already exist") { showAlert("Cycle Count with Same data already exist", "Error", '#'); }
             else {
                 document.getElementById("hdnCycleCountTitle").value = "";
                 document.getElementById("hdnDrpTitle").value = "";
                 CountTytle.selectedIndex.value = 0;
                 showAlert(result, "info", "../POR/CycleCount.aspx");
                 //window.open('../POR/CycleCount.aspx', '_self', '');
             }

         }

         function WMSaveRequestHead_onFailed() {
             showAlert("Error occurred", "Error", "#");
         }


    </script>

    <script type="text/javascript">
        function GetWarehouse() {
            var hdnwarehouseId = document.getElementById("<%=ddlWarehouse.ClientID%>");
             document.getElementById("<%=hdnwarehouseId.ClientID%>").value = hdnwarehouseId.value;
         }
    </script>
    <style type="text/css">
        /*Grid css*/
        .excel-textbox
        {
            background-color: transparent;
            border: 0px;
            padding: 0px;
            outline: 0;
            font: inherit;
            width: 81%;
            padding-top: 4px;
            padding-right: 2px;
            padding-bottom: 4px;
            text-align: right;
        }
        .excel-textbox-focused
        {
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
        
        .excel-textbox-error
        {
            color: #FF0000;
        }
        
        .ob_gCc2
        {
            padding-left: 3px !important;
        }
        
        .ob_gBCont
        {
            border-bottom: 1px solid #C3C9CE;
        }
        
        .excel-checkbox
        {
            height: 20px;
            line-height: 20px;
        }
    </style>
    <style type="text/css">
        /*POR Collapsable Div*/
        
        .PanelCaption
        {
            color: Black;
            font-size: 13px;
            font-weight: bold;
            margin-top: -22px;
            margin-left: -5px;
            position: absolute;
            background-color: White;
            padding: 0px 2px 0px 2px;
        }
        .divHead
        {
            border: solid 2px #F5DEB3;
            width: 99%;
            text-align: left;
        }
        .divHead h4
        {
            /*color: #33CCFF;*/
            color: #483D8B;
            margin: 3px 3px 3px 3px;
        }
        .divHead a
        {
            float: Left;
            margin-top: -15px;
            margin-right: 5px;
        }
        .divHead a:hover
        {
            cursor: pointer;
            color: Red;
        }
        .divDetailExpand
        {
            border: solid 2px #F5DEB3;
            border-top: none;
            width: 99%;
            padding: 5px 0 5px 0;
        }
        .divDetailCollapse
        {
            display: none;
        }
        /*End POR Collapsable Div*/
        
        
        
    </style>
    <script type="text/javascript">
        function openLocation(sequence) {
            window.open('../POR/CycleCountAddNew.aspx', null, 'height=250, width=1200,status= 0, resizable= 1, scrollbars=0, toolbar=0,location=center,menubar=0, screenX=300; screenY=50');
        }
    </script>

    <script type="text/javascript">
        function openProductSearch(sequence) {
            var CustomerID = 0;
            var WarehouseID = document.getElementById("<%=hdnwarehouseId.ClientID%>").value;
           // window.open('../POR/SearchProduct.aspx', null, 'height=700px, width=825px,status= 0, resizable= 1, scrollbars=1, toolbar=0,location=center,menubar=0, screenX=300; screenY=50');
            window.open('../Product/WLocationSearch.aspx?CustomerID=' + CustomerID + '&WarehouseID=' + WarehouseID + '', 'height=500px, width=1080px,status= 0, resizable= 1, scrollbars=1, toolbar=0,location=center,menubar=0, screenX=300; screenY=50', target = "_blank");
        }

        function openSkuSearchCycle()
        {
            var WarehouseID = document.getElementById("<%=ddlWarehouse.ClientID%>");
            var LocationID = document.getElementById("<%=hdnselectedLocID.ClientID%>");
            window.open('../Warehouse/WSKUSearch.aspx?WarehouseID=' + WarehouseID.value + '&LocationID=' + LocationID.value + "", '', 'height=700px, width=1090px,status= 0, resizable= 1, scrollbars=1, toolbar=0,location=center,menubar=0, screenX=300; screenY=50', target = "_blank");
        }

        function OpenCycleCountHead()
        {
            window.open('../Warehouse/CycleCountPlan.aspx', null, 'height=800px, width=1205px,status= 0, resizable= 1, scrollbars=1, toolbar=0,location=center,menubar=0, screenX=300; screenY=50');
        }
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
    <%-- style to show three steps--%>
    <style type="text/css">
        .cartStepTitle
        {
            font-size: 18px;
            color: #cccccc;
            padding: 0px 50px 30px 0px;
            display: inline-block;
            padding-right: 60px;
            font-weight: bold;
        }
        .divCartSymbol, .divCartCurrentSymbol
        {
            position: relative;
            top: 30px;
            left: -10px;
            display: inline-block;
            background-image: url(../images/opt-bg-normal.png);
            background-repeat: no-repeat;
            background-position: center center;
            font-size: 30px;
            font-family: Trebuchet MS, Arial;
            color: #ffffff;
            padding: 20px;
            text-decoration: none;
            overflow: hidden;
            opacity: 0.7;
        }
        .tdCartStepHolder
        {
            padding-left: 10px;
        }
        .cartStepHolder
        {
            position: relative;
            top: -10px;
        }
        .divCartCurrentSymbol
        {
            background-image: url(../images/opt-bg-selected1.png);
            opacity: 1;
        }
        .cartStepCurrentTitle
        {
            color: #000000;
        }
        
        
        #btnNext
        {
            width: 69px;
        }
        #btnCancle
        {
            width: 69px;
        }
        
        
        .style2
        {
            height: 47px;
        }
        input.btnCommonStyle[type="button"]
        {
            position:relative;
            top:-7px;
        }
        .btnCommonStyle
        {
            font-family: inherit; /*font-weight: bold;*/
            font-size: 18px;
            color: #ffffff;
            text-decoration: none !important;
            
            padding-left: 50px;
            padding-right: 50px;
            padding-top: 10px;
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
        .tblcls
        {
            border: solid 2px #F5DEB3;
        }
    </style>
</asp:Content>

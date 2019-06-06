<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/CRM2.Master" AutoEventWireup="true" CodeBehind="CycleCountPlan.aspx.cs"
    Inherits="BrilliantWMS.Warehouse.CycleCountPlan" Theme="Blue" EnableEventValidation="false" %>

<%@ Register Assembly="obout_Interface" Namespace="Obout.Interface" TagPrefix="cc1" %>
<%@ Register Assembly="obout_Grid_NET" Namespace="Obout.Grid" TagPrefix="obout" %>
<%@ Register Src="../CommonControls/UCFormHeader.ascx" TagName="UCFormHeader" TagPrefix="uc7" %>
<%@ Register Src="../CommonControls/UCToolbar.ascx" TagName="UCToolbar" TagPrefix="uc8" %>
<%@ Register Src="../Product/UCProductSearch.ascx" TagName="UCProductSearch" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../CommonControls/UC_Date.ascx" TagName="UC_Date" TagPrefix="uc8" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <table >
           <tr>
               <td style="text-align: left">
                   <asp:Label ID="Label1" runat="server" Font-Size="Large"  Font-Bold="true" ForeColor="White" style="padding-left:10px" Text="Cycle Count Plan:"></asp:Label>
               </td>
          </tr>
         <tr style="height:15px"><td></td></tr>
       </table>
   <center>
                             <asp:HiddenField ID="hdnwarehouseId" runat="server" ClientIDMode="Static" />
                             <asp:HiddenField ID="hdnobject" runat="server" ClientIDMode="Static" />
                             <asp:HiddenField ID="hdnsessionID" runat="server" ClientIDMode="Static" />
                       <table class="tableForm">
                            <tr>
                                 <td>
                                    <req><asp:Label ID="lbltitle" runat="server" Text="Title:"></asp:Label></req>
                                </td>
                                <td style="text-align: left">
                                    <asp:TextBox ID="txtTitle" runat="server"  Width="300px"></asp:TextBox>
                                      <asp:TextBox ID="txtgridvalues" runat="server"  Width="300px" Visible="false"></asp:TextBox>
                                </td>
                                <td>
                                    <req><asp:Label Id="lblwarehouse" runat="server" Text="Warehouse"></asp:Label></req>
                                    :
                                </td>
                                <td style="text-align: left">
                                    <asp:DropDownList ID="ddlWarehouse" runat="server"  DataTextField="WarehouseName" DataValueField="ID" onchange="GetWarehouse()" Width="206px">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RfValCompany" runat="server" ControlToValidate="ddlWarehouse" InitialValue="0"
                                        Display="None" ErrorMessage="Please Select Waehouse" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                </td>
                                <td>
                                <req><asp:Label Id="lblstatus" runat="server" Text="Status"></asp:Label></req> :
                              </td>
                                <td style="text-align: left">
                                <asp:DropDownList ID="ddlStatus" runat="server" DataTextField="Name" DataValueField="ID" Width="206px">
                                    <asp:ListItem Text="Planned" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="InProgress" Value="2"></asp:ListItem>
                                    <asp:ListItem Text="Completed" Value="3"></asp:ListItem>
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="ddlfrequency" InitialValue="0"
                                        Display="None" ErrorMessage="Please Select Frequency" ValidationGroup="Save"></asp:RequiredFieldValidator>
                              </td>
                        </tr>
                            <tr>
                                 <td>
                                      <req><asp:Label Id="lblfrequency" runat="server" Text="Frequency"></asp:Label></req> :
                                </td>
                                <td style="text-align: left">
                                <asp:DropDownList ID="ddlfrequency" runat="server" DataTextField="Name" DataValueField="ID" Width="206px">
                                    <asp:ListItem Text="-Select-" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="Daily" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="Weekly" Value="2"></asp:ListItem>
                                    <asp:ListItem Text="Monthly" Value="3"></asp:ListItem>
                                    <asp:ListItem Text="Quarterly" Value="4"></asp:ListItem>
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RFQCompany" runat="server" ControlToValidate="ddlfrequency" InitialValue="0"
                                        Display="None" ErrorMessage="Please Select Frequency" ValidationGroup="Save"></asp:RequiredFieldValidator>
                              </td>
                                 <td>
                                       <req><asp:Label ID="lblfrmDate" runat="server" Text="From Date" /></req> :
                                 </td>
                                 <td style="text-align: left;">
                                       <uc8:UC_Date ID="UC_FromDate" runat="server" />
                                      <asp:CustomValidator ID="custValidatestartdate" runat="server" ClientValidationFunction="validatestartdate"
                                    ValidationGroup="Save" ErrorMessage="Please select Subscription Start Date" ></asp:CustomValidator>  
                                 </td>
                                 <td>
                                        <req><asp:Label ID="lblToDate" runat="server" Text="To Date" /></req> :
                                 </td>
                                 <td style="text-align: left;">
                                        <uc8:UC_Date ID="UC_ToDate" runat="server" />
                                     <asp:CustomValidator ID="custValienddate" runat="server" ClientValidationFunction="validatesEndDate"
                                    ValidationGroup="Save" ErrorMessage="Please select Subscription End Date" ></asp:CustomValidator> 
                                 </td>
                            </tr>
                            <tr>
                                <td>
                                    <req><asp:Label Id="lblcountbasis" runat="server" Text="Cycle Count Basis"></asp:Label></req>
                                    :
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
                                <td>
                                     <input type="button" id="btnContactPerson" runat="server" value="Add Product/Location" onclick="OpenSearch()" />
                                </td>
                                <td> </td>
                                <td></td><td></td>
                            </tr>
                            </table>
                            </center>
                              <center>
                        <table class="gridFrame" style="width: 100%">
                            <tr>
                                <td>
                                    <table style="width: 100%">
                                        <tr>
                                            <td style="text-align: left;">
                                                <a id="headerText">
                                                    <asp:Label ID="lblheasertext" runat="server" Text="List"></asp:Label></a>
                                            </td>
                                            <td style="text-align: right;">
                                                 <input type="button" id="btnsave" runat="server" value="  Save  " onclick="SaveCycleCountPlan()" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td id="tdproduct" style="display:none">
                                    <obout:Grid ID="grdcyclecount" runat="server" AutoGenerateColumns="False" AllowAddingRecords="False"
                                        AllowGrouping="True" AllowFiltering="True" Width="100%" OnSelect="grdcyclecount_Select" style="display:block" OnRebind="grdcyclecount_RebindGrid" >
                                        <ScrollingSettings ScrollHeight="250" />
                                        <Columns>
                                            <obout:Column DataField="ID" Visible="False" HeaderText="ID" Index="1">
                                            </obout:Column>
                                             <obout:Column DataField="ProductCode" HeaderText="ProductCode" Width="12%" Index="2">
                                            </obout:Column>
                                            <obout:Column DataField="Name" HeaderText="Product Name" Width="14%" Index="3">
                                            </obout:Column>
                                            <obout:Column HeaderText="Description" DataField="Description" Width="16%" Index="4" >
                                            </obout:Column>
                                        </Columns>
                                    </obout:Grid>
                                </td>
                                <td id="tdlocation" style="display:none">
                                    <obout:Grid ID="grdlocation" runat="server" AutoGenerateColumns="False" AllowAddingRecords="False"
                                        AllowGrouping="True" AllowFiltering="True" Width="100%" style="display:block" OnRebind="grdlocation_RebindGrid">
                                        <ScrollingSettings ScrollHeight="250" />
                                        <Columns>
                                            <obout:Column DataField="ID" Visible="False" HeaderText="ID" Index="1">
                                            </obout:Column>
                                             <obout:Column DataField="Code" HeaderText="Location Code" Width="15%" Index="2">
                                            </obout:Column>
                                            <obout:Column DataField="AliasCode" HeaderText="AliasCode" Width="16%" Index="3">
                                            </obout:Column>
                                        </Columns>
                                    </obout:Grid>
                                </td>
                            </tr>
                         </table>
                    </center>
      
    <script type="text/javascript">

        function GetWarehouse()
        {
            var hdnwarehouseId = document.getElementById("<%=ddlWarehouse.ClientID%>");
            document.getElementById("<%=hdnwarehouseId.ClientID%>").value = hdnwarehouseId.value;
        }


        function Showgrid()
        {
            var cyclebasis = document.getElementById("<%=ddlcountbasis.ClientID %>");
            document.getElementById("<%=hdnobject.ClientID %>").value = cyclebasis.value;
            if (cyclebasis.value == "Product")
            {
                document.getElementById('tdproduct').style.display = "block";
                document.getElementById('tdlocation').style.display = "none";
            }
            else if (cyclebasis.value == "Location")
            {
                document.getElementById('tdlocation').style.display = "block";
                document.getElementById('tdproduct').style.display = "none";
            }
           // CheckDate();
        }

        function OpenSearch()
        {
            var cyclebasis = document.getElementById("<%=ddlcountbasis.ClientID %>");
            var WarehouseID = document.getElementById("<%=ddlWarehouse.ClientID%>");
            if (cyclebasis.value == "Product") {
               window.open('../Warehouse/CycleSKUSearch.aspx?Object=' + cyclebasis.value + '&WarehouseID=' + WarehouseID.value + "", '', 'height=700px, width=955px,status= 0, resizable= 1, scrollbars=1, toolbar=0,location=center,menubar=0, screenX=300; screenY=50', target = "_blank");
            }
            else if (cyclebasis.value == "Location") {
                window.open('../Warehouse/CycleLocSearch.aspx?Object=' + cyclebasis.value + '&WarehouseID=' + WarehouseID.value + "", '', 'height=700px, width=955px,status= 0, resizable= 1, scrollbars=1, toolbar=0,location=center,menubar=0, screenX=300; screenY=50', target = "_blank");
            }
        }

    </script>

    <script type="text/javascript">

        function validatestartdate(source, args) {
            var Frequency = document.getElementById("<%=ddlfrequency.ClientID %>");
            var FromDate = getDateFromUC("<%= UC_FromDate.ClientID %>");
            var ToDate = getDateFromUC("<%= UC_ToDate.ClientID %>");

            if (SubscriptStartDate != "") {
                isValid = true;
                return;
            }
            args.IsValid = false;
        }

        function CheckDate()
        {
            var Frequency = document.getElementById("<%=ddlfrequency.ClientID %>");
            var FromDate = getDateFromUC("<%= UC_FromDate.ClientID %>");
            var ToDate = getDateFromUC("<%= UC_ToDate.ClientID %>");
            var StartDate = GetDate(FromDate)
            var EndDate = GetDate(ToDate)
        }


        function GetDate(str) {
            debugger;
            var arr = str.split('-');
            var months = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec']
            for (var i = 0; i <= months.length; i++) {
                if (months[i] == arr[1]) {
                      break;
                }
            }
            var formatddate = i + '/' + arr[0] + '/' + arr[2];
            return formatddate;
        }

      
    </script>

    <script type="text/javascript">

        function SaveCycleCountPlan()
        {
            var Title = document.getElementById("<%=txtTitle.ClientID%>");
            var Warehouse = document.getElementById("<%=ddlWarehouse.ClientID%>");
            var status = document.getElementById("<%=ddlStatus.ClientID%>");
            var Frequency = document.getElementById("<%=ddlfrequency.ClientID%>");
            var FromDate = getDateFromUC("<%= UC_FromDate.ClientID %>");
            var ToDate = getDateFromUC("<%= UC_ToDate.ClientID %>");
            var countBasis = document.getElementById("<%=ddlcountbasis.ClientID%>"); 
            var txtgridvalues = document.getElementById("<%=txtgridvalues.ClientID%>");
            var session = document.getElementById("<%=hdnsessionID.ClientID%>");


            if (document.getElementById("<%=txtTitle.ClientID %>").value == "") {
                showAlert("Please Enter Title!", "Error", "#");
                document.getElementById("<%=txtTitle.ClientID %>").focus();
            }
            else if (document.getElementById("<%=ddlWarehouse.ClientID %>").value == "0") {
                showAlert("Please Select Warehouse!", "Error", "#");
                document.getElementById("<%=ddlWarehouse.ClientID %>").focus();
            }
            else if (document.getElementById("<%=ddlfrequency.ClientID %>").value == "0") {
                showAlert("Please Select Frequency!", "Error", "#");
                document.getElementById("<%=ddlfrequency.ClientID %>").focus();
            }
            else if (FromDate == "") {
                showAlert("Please Select From Date!", "Error", "#");
                //document.getElementById("<%=ddlWarehouse.ClientID %>").focus();
            }
            else if (ToDate == "") {
                showAlert("Please Select To Date!", "Error", "#");
                //document.getElementById("<%=ddlWarehouse.ClientID %>").focus();
            }
            else if (document.getElementById("<%=ddlcountbasis.ClientID %>").value == "0") {
                showAlert("Please Select Cycle Count Basis!", "Error", "#");
                document.getElementById("<%=ddlcountbasis.ClientID %>").focus();
            }
           
            else {

                var obj = new Object();
                obj.Title = Title.value;
                obj.WarehouseID = Warehouse.value;
                obj.Status = status.options[status.selectedIndex].text;
                obj.Frequency = Frequency.options[Frequency.selectedIndex].text; 
                obj.FromDate = FromDate;
                obj.ToDate = ToDate;
                obj.CountBasis = countBasis.options[countBasis.selectedIndex].text;
                obj.session = session.value;
                PageMethods.PMSaveWLocation(obj, SubmitCycleCountPlan_onSuccess, SubmitCycleCountPlan_onFail);
            }
        }

        function SubmitCycleCountPlan_onSuccess(result) {
            if (result == "Some error occurred" || result == "") { showAlert("Error occurred", "Error", '#'); }
            else if (result == "More Than Daily") { showAlert("No. Of Days Should Be less than 12", "Error", '#'); }
            else if (result == "More Than Weekly") { showAlert("No. Of Days Should Be less than 84", "Error", '#'); }
            else if (result == "More Than Monthly") { showAlert("no. Of Days Should Be less than 365", "Error", '#'); }
            else if (result == "More Than Quarterly") { showAlert("no. Of Days Should Be less than 1080", "Error", '#'); }
            else if (result == "Cycle saved successfully") {
                showAlert(result, "info");
                document.getElementById("<%= txtTitle.ClientID %>").value = "";
                document.getElementById("<%= ddlWarehouse.ClientID %>").value = "0";
                document.getElementById("<%= ddlStatus.ClientID %>").value = "0";
                document.getElementById("<%= ddlfrequency.ClientID %>").value = "0";
                document.getElementById("<%= ddlcountbasis.ClientID %>").value = "0";
                window.opener.grdcyclecount.refresh();
                self.close();
               // var todate =  getDateFromUC("<%= UC_ToDate.ClientID %>");
                //todate.value = "";
               // document.getElementById("<%=UC_FromDate.ClientID %>").value = "";
               // grdlocation.refresh();
                
                //window.opener.GVLocation.refresh();
            }
    }

        function SubmitCycleCountPlan_onFail() {
        showAlert("Error occurred", "Error", "#");
    }

    </script>

</asp:Content>

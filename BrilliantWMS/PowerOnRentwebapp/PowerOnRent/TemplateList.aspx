<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TemplateList.aspx.cs" Inherits="BrilliantWMS.PowerOnRent.TemplateList"
     EnableEventValidation="false" Theme="Blue" %>

<%@ Register Assembly="obout_Grid_NET" Namespace="Obout.Grid" TagPrefix="obout" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true" />
    <div>
        <table class="gridFrame" width="800px" style="margin: 3px 3px 3px 3px;">
            <tr>
                <td>
                    <table style="width: 100%">
                        <tr>
                            <td style="text-align: left;">
                                <a class="headerText"><asp:Label ID="lblTemplateList" runat="server" Text="Template List"></asp:Label></a>
                            </td>
                            <td style="text-align: right;">
                                <input runat="server" type="button" value="Submit" id="btnSubmitProductSearch1" onclick="selectedRec();"  />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <obout:Grid ID="GVRequest" runat="server" AllowAddingRecords="false" AutoGenerateColumns="false"
                        AllowGrouping="true" Serialize="false" CallbackMode="true" AllowRecordSelection="true"
                        AllowMultiRecordSelection="false" AllowColumnReordering="true" AllowFiltering="true"
                        Width="100%" PageSize="10">
                        <Columns>                          
                            <obout:Column DataField="ID" HeaderText="Template Title" HeaderAlign="left"
                                Visible="false">
                            </obout:Column>
                            <obout:Column DataField="TemplateTitle" HeaderText="Template Title" HeaderAlign="left"
                                Align="left" Width="8%">
                            </obout:Column>
                            <obout:Column DataField="CustomerName" HeaderText="Created By" HeaderAlign="left"
                                Align="left" Width="5%">
                            </obout:Column>
                            <obout:Column DataField="AccessType" HeaderText="Access Type" HeaderAlign="left"
                                Align="left" Width="5%">
                            </obout:Column>
                            <obout:Column DataField="Territory" HeaderText="Department" HeaderAlign="left" Align="left"
                                Width="5%">
                            </obout:Column>
                            <obout:Column DataField="CustomerName" HeaderText="Customer Name" HeaderAlign="left"
                                Align="left" Width="5%">
                            </obout:Column>
                            <obout:Column DataField="CreatedDate" HeaderText="Created Date" HeaderAlign="left"
                                DataFormatString="{0:dd-MMM-yyyy}" Align="left" Width="5%">
                            </obout:Column>
                            <obout:Column DataField="Active" HeaderText="Active" HeaderAlign="left" Align="left"
                                Width="5%">
                            </obout:Column>
                        </Columns>                     
                    </obout:Grid>
                </td>
            </tr>
        </table>
        <%-- <asp:HiddenField ID="hdnSelTemplateID" runat="server" ClientIDMode="Static" ViewStateMode="Enabled"/>--%>
    </div>
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
            }
        }
        function selectedRec() {
            var hdnSelTemplateID = window.opener.document.getElementById("hdnSelTemplateID");
            hdnSelTemplateID.value = "";
            if (GVRequest.SelectedRecords.length > 0) {
                for (var i = 0; i < GVRequest.SelectedRecords.length; i++) {
                    var record = GVRequest.SelectedRecords[i];
                    if (hdnSelTemplateID.value == "") hdnSelTemplateID.value = record.ID;
                }
            }
           // alert(hdnSelTemplateID.value);
            PageMethods.WMGetSelectedTemplateID(hdnSelTemplateID.value, OnSuccessTemplateListID, null)            
        }
        function OnSuccessTemplateListID(result) {
            window.opener.location.reload(true);
            self.close();
        }        

    </script>
    </form>
</body>
</html>

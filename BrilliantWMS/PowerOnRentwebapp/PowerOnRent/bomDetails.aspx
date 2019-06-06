<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="bomDetails.aspx.cs" Inherits="BrilliantWMS.PowerOnRent.bomDetails" EnableEventValidation="false" Theme="Blue" %>
<%@ Register Assembly="obout_Grid_NET" Namespace="Obout.Grid" TagPrefix="obout" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Bill Of Material Details</title>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true" />
    <div>
     <table class="gridFrame" width="800px" style="margin: 3px 3px 3px 3px;">
            <%--<tr>
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
            </tr>--%>
            <tr>
                <td>
                    <obout:Grid ID="GVBOMDetail" runat="server" AllowAddingRecords="false" AutoGenerateColumns="false"
                        AllowGrouping="true" Serialize="false" CallbackMode="true" AllowRecordSelection="true"
                        AllowMultiRecordSelection="false" AllowColumnReordering="true" AllowFiltering="true"
                        Width="100%" PageSize="10">
                        <Columns>                          
                            <obout:Column DataField="SKUId" HeaderText="SKUId" HeaderAlign="left"
                                Visible="false">
                            </obout:Column>                           
                            <obout:Column DataField="ProductCode" HeaderText="Product Code" HeaderAlign="left"
                                Align="left" Width="5%">
                            </obout:Column>
                            <obout:Column DataField="Name" HeaderText="Name" HeaderAlign="left"
                                Align="left" Width="5%">
                            </obout:Column>
                            <obout:Column DataField="Description" HeaderText="Description" HeaderAlign="left" Align="left"
                                Width="8%">
                            </obout:Column>
                            <obout:Column DataField="Quantity" HeaderText="Quantity" HeaderAlign="left"
                                Align="left" Width="5%">
                            </obout:Column>
                           <%-- <obout:Column DataField="CreatedDate" HeaderText="Created Date" HeaderAlign="left"
                                DataFormatString="{0:dd-MMM-yyyy}" Align="left" Width="5%">
                            </obout:Column>
                            <obout:Column DataField="Active" HeaderText="Active" HeaderAlign="left" Align="left"
                                Width="5%">
                            </obout:Column>--%>
                        </Columns>                     
                    </obout:Grid>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>

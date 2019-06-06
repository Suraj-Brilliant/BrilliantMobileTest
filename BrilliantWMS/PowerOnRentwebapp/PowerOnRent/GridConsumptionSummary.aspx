<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GridConsumptionSummary.aspx.cs"
    Inherits="PowerOnRentwebapp.PowerOnRent.GridConsumptionSummary" EnableEventValidation="false" %>

<%@ Register Assembly="obout_Grid_NET" Namespace="Obout.Grid" TagPrefix="obout" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <center>
            <table>
                <tr>
                    <td>
                        <obout:Grid ID="GVConsumption" runat="server" AllowAddingRecords="false" AutoGenerateColumns="false"
                            AllowGrouping="true" Serialize="false" CallbackMode="true" AllowRecordSelection="false"
                            AllowMultiRecordSelection="false" AllowColumnReordering="true" AllowFiltering="true"
                            Width="100%" PageSize="10">
                            <Columns>
                                <obout:Column DataField="SiteID" Visible="false" Width="0px">
                                </obout:Column>
                                <obout:Column DataField="SiteName" HeaderText="Site" HeaderAlign="left" Align="left"
                                    Width="7%">
                                </obout:Column>
                                <obout:Column DataField="ConH_ID" HeaderText="Consumption No." HeaderAlign="left"
                                    Align="left" Width="7%">
                                </obout:Column>
                                <obout:Column DataField="ConsumptionDate" HeaderText="Consumption Date" HeaderAlign="center"
                                    Align="center" Width="9%" DataFormatString="{0:dd-MMM-yy}">
                                </obout:Column>
                                <obout:Column DataField="ConsumedByUserID" Visible="false" Width="0%">
                                </obout:Column>
                                <obout:Column DataField="ConsumedByUserName" HeaderText="Consumed By" HeaderAlign="left"
                                    Align="left" Width="10%">
                                </obout:Column>
                                <obout:Column DataField="EngineSerial" HeaderText="Engine Serial No." HeaderAlign="left"
                                    Align="left" Width="10%">
                                </obout:Column>
                                <obout:Column DataField="EngineModel" HeaderText="Engine Model" HeaderAlign="left"
                                    Align="left" Width="10%">
                                </obout:Column>
                                <obout:Column DataField="Container" HeaderText="Container" HeaderAlign="left" Align="left"
                                    Width="10%">
                                </obout:Column>
                                <obout:Column DataField="FailureHours" HeaderText="Hours Of Failure" HeaderAlign="left"
                                    Align="left" Width="10%">
                                </obout:Column>
                                <obout:Column DataField="FailureCause" HeaderText="Cause Of Failure" HeaderAlign="left"
                                    Align="left" Width="10%">
                                </obout:Column>
                                <obout:Column DataField="FailureNature" HeaderText="Nature Of Failure" HeaderAlign="left"
                                    Align="left" Width="10%">
                                </obout:Column>
                                <obout:Column DataField="ImgConsumption" HeaderText="Consumption" HeaderAlign="left"
                                    Align="left" Width="7%">
                                    <TemplateSettings TemplateId="GTStatusGUIConsumption" />
                                </obout:Column>
                            </Columns>
                            <Templates>
                                <obout:GridTemplate ID="GTStatusGUIConsumption" runat="server">
                                    <Template>
                                        <center>
                                            <div class='<%# ("POR" + Container.Value) %>' onclick="parent.ConsumptionOpenEntryForm('Consumption', '<%# Container.Value %>', '<%# Container.DataItem["ConH_ID"] %>')">
                                            </div>
                                        </center>
                                    </Template>
                                </obout:GridTemplate>
                            </Templates>
                        </obout:Grid>
                    </td>
                </tr>
            </table>
        </center>
    </div>
    </form>
</body>
</html>

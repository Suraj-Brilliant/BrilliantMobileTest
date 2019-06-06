<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCLocation.ascx.cs"
    Inherits="PowerOnRentwebapp.Location.UCLocation" %>
<%@ Register Assembly="obout_Interface" Namespace="Obout.Interface" TagPrefix="obout" %>
<%@ Register Assembly="obout_Grid_NET" Namespace="Obout.Grid" TagPrefix="obout" %>
<center>
    <table class="gridFrame" style="margin-top: 10px; width: 100%">
        <tr>
            <td style="text-align: left;">
                <span class="headerText">Location List</span>
            </td>
            <td style="text-align: right;">
                <input type="button" id="btnContactPerson" value="Add New" onclick="openLocationWindow('0')" />
                &nbsp;&nbsp;
                <input type="button" id="btnMoveToArchiveContactPerson" value="Archive" onclick="MoveToArchiveContactPerson()" />
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <obout:Grid ID="GVLocation" runat="server" AutoGenerateColumns="false" AllowAddingRecords="false"
                    AllowFiltering="true" AllowGrouping="true" AllowMultiRecordSelection="False"
                    AllowRecordSelection="true" Width="100%" AllowColumnResizing="true" AllowColumnReordering="true"
                    OnRebind="GVLocation_OnRebind">
                    <ClientSideEvents ExposeSender="true" />
                    <Columns>
                        <%--  <obout:CheckBoxSelectColumn ControlType="Obout" Width="3%" Align="center" AllowSorting="true"
                            HeaderAlign="center">
                        </obout:CheckBoxSelectColumn>--%>
                       <%-- <obout:Column DataField="Sequence" HeaderText="Edit" Width="5%" AllowSorting="false"
                            Align="center" HeaderAlign="center">
                            <TemplateSettings TemplateId="GvTempEdit" />
                        </obout:Column>--%>
                        <obout:Column DataField="ID1" HeaderText="ID" Visible="false">
                        </obout:Column>
                        <obout:Column HeaderText="Location Code" DataField="LocationCode" Width="20%" Wrap="true" Align="center" HeaderAlign="center">
                        </obout:Column>
                        <obout:Column HeaderText="Capacity" DataField="LocationCapacity" Width="20%" Wrap="true">
                        </obout:Column>
                        <obout:Column HeaderText="Velocity Type" DataField="VelocityType" Width="20%" Wrap="true">
                        </obout:Column>
                        <obout:Column HeaderText="Sort Code" DataField="SortCode" Width="20%" Wrap="true">
                        </obout:Column>
                        <%-- <obout:Column HeaderText="Active" DataField="Active" Width="5%">
                            <TemplateSettings TemplateId="tplActive" />
                        </obout:Column>--%>
                        <%-- <obout:Column DataField="selected" HeaderText="Default" Width="6%" AllowSorting="false"
                            Align="center" HeaderAlign="center">
                            <TemplateSettings TemplateId="GVDefault" />
                        </obout:Column>--%>
                    </Columns>
                    <Templates>
                        <%-- <obout:GridTemplate runat="server" ID="tplActive">
                            <Template>
                                <%# (Container.DataItem["Active"].ToString() == "Y" ? "Yes" : "No")%>
                            </Template>
                        </obout:GridTemplate>--%>
                       <%-- <obout:GridTemplate ID="GvTempEdit" runat="server">
                            <Template>
                                <img id="imgBtnEdit" src="../App_Themes/Blue/img/Edit16.png" title="Edit" onclick="openLocationWindow('<%# (Container.DataItem["Sequence"].ToString()) %>');"
                                    style="cursor: pointer;" />
                            </Template>
                        </obout:GridTemplate>--%>
                    </Templates>
                    <%-- <Templates>
                        <obout:GridTemplate ID="GVDefault" runat="server">
                            <Template>
                                <input type="checkbox" id="chkIsDefault" onclick="getDefault(this,'<%# (Container.DataItem["Sequence"].ToString()) %>')"
                                    <%# Container.Value.ToString().ToLower()=="true" ? "checked='checked'" : "" %> />
                            </Template>
                        </obout:GridTemplate>
                    </Templates>--%>
                </obout:Grid>
            </td>
        </tr>
    </table>
    <asp:HiddenField ID="hdnConPersonTargetObject" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hnddefaultchk" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnSelectedRec" runat="server" ClientIDMode="Static" />
</center>
<script type="text/javascript">
    function setContactPersonTargetObjectName(TragetObjectName) {
        document.getElementById("hdnConPersonTargetObject").value = TragetObjectName;
        document.getElementById("hnddefaultchk").value = "1";
    }
    function openLocationWindow(Sequence) {
        window.open('../Warehouse/WarehouseLocation.aspx?Sequence=' + Sequence + '', null, 'height=350px, width=1190px,status=0, resizable=0, scrollbars=0, toolbar=0,menubar=0');
    }

    function getDefault(sender, seuqence) {
        var hnddefaultchk = document.getElementById("hnddefaultchk");
        hnddefaultchk.value = seuqence;
        var rbtn = eval(sender);
        if (rbtn.checked == true) {
            var parentEle = rbtn.parentElement;
            onlyoneselectContact(sender);
        }
    }
    function onlyoneselectContact(invoker) {
        var m = GVLocation.GridBodyContainer;
        var allInput = m.getElementsByTagName('input');
        for (var c = 0; c <= allInput.length - 1; c++) {
            var chk = allInput[c];
            if (chk.type == "checkbox") { chk.checked = false; }
        }
        invoker.checked = true;
    }


    function MoveToArchiveContactPerson() {
        var Ids = document.getElementById("hdnSelectedRec");
        for (var i = 0; i < GVLocation.SelectedRecords.length; i++) {
            var record = GVLocation.SelectedRecords[i];
            if (Ids.value != "") Ids.value += ',' + record.Sequence;
            if (Ids.value == "") Ids.value = record.Sequence;
        }
        PageMethods.PMMoveContactPersonToArchive(Ids.value, "Y");
        Ids.value = "";
        GVLocation.refresh();
    }

</script>

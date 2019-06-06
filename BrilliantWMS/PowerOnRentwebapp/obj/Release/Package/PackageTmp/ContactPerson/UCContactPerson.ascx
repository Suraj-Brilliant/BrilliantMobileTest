<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCContactPerson.ascx.cs" Inherits="BrilliantWMS.ContactPerson.UCContactPerson" %>
<%@ Register Assembly="obout_Interface" Namespace="Obout.Interface" TagPrefix="obout" %>
<%@ Register Assembly="obout_Grid_NET" Namespace="Obout.Grid" TagPrefix="obout" %>
<center>
    <table class="gridFrame" style="margin-top: 10px; width: 100%">
        <tr>
            <td style="text-align: left;">
                <%--<span id="spnconlist" class="headerText">Contact Person List</span>--%>
                <asp:Label ID="lblcontlist" CssClass="headerText" runat="server" Text="Contact Person List"></asp:Label>
            </td>
            <td style="text-align: right;">
               <input type="button" id="btnContactPerson" runat="server" value="Add New" onclick="openContactPersonWindow('0')" />
                <%-- <asp:Button ID="btnsave" runat="server" Text="Next" OnClick="btnsave_Click" /> --%>
                &nbsp;&nbsp;
                <%--<input type="button" id="btnMoveToArchiveContactPerson" value="Archive" onclick="MoveToArchiveContactPerson()" />--%>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <obout:Grid ID="GVContactPerson" runat="server" AutoGenerateColumns="false" AllowAddingRecords="false"
                    AllowFiltering="true" AllowGrouping="true" Width="100%" Serialize="false" OnRebind="GVContactPerson_OnRebind"
                    CallbackMode="true">
                    <ClientSideEvents ExposeSender="true" />
                    <Columns>
                      <%--  <obout:CheckBoxSelectColumn ControlType="Obout" Width="3%" Align="center" AllowSorting="true"
                            HeaderAlign="center">
                        </obout:CheckBoxSelectColumn>--%>
                        <obout:Column DataField="Sequence" HeaderText="Edit" Width="5%" AllowSorting="false" 
                            Align="center" HeaderAlign="center">
                            <TemplateSettings TemplateId="GvTempEdit" />
                        </obout:Column>
                        <obout:Column DataField="ID1" HeaderText="ID" Visible="false">
                        </obout:Column>
                        <obout:Column HeaderText="Contact Type" DataField="ContactType" Width="10%" Wrap="true">
                        </obout:Column>
                        <obout:Column HeaderText="Name" DataField="Name" Width="20%" Wrap="true">
                        </obout:Column>
                       <%-- <obout:Column HeaderText="Department" DataField="Designation" Width="10%" Wrap="true">
                        </obout:Column>--%>
                        <obout:Column HeaderText="Email ID" DataField="EmailID" Width="15%" Wrap="true">
                        </obout:Column>
                        <obout:Column HeaderText="Mobile No." DataField="MobileNo" Width="10%" Wrap="true">
                        </obout:Column>
                        <obout:Column HeaderText="Office No." DataField="OfficeNo" Width="10%" Wrap="true">
                        </obout:Column>
                        <%-- <obout:Column HeaderText="Active" DataField="Active" Width="5%">
                            <TemplateSettings TemplateId="tplActive" />
                        </obout:Column>--%>
                        <obout:Column DataField="selected" HeaderText="Default" Width="6%" AllowSorting="false"
                            Align="center" HeaderAlign="center">
                            <TemplateSettings TemplateId="GVDefault" />
                        </obout:Column>
                    </Columns>
                    <Templates>
                        <%-- <obout:GridTemplate runat="server" ID="tplActive">
                            <Template>
                                <%# (Container.DataItem["Active"].ToString() == "Y" ? "Yes" : "No")%>
                            </Template>
                        </obout:GridTemplate>--%>
                        <obout:GridTemplate ID="GvTempEdit" runat="server">
                            <Template>
                                <img id="imgBtnEdit" src="../App_Themes/Blue/img/Edit16.png" title="Edit" onclick="openContactPersonWindow('<%# (Container.DataItem["Sequence"].ToString()) %>');"
                                    style="cursor: pointer;" />
                            </Template>
                        </obout:GridTemplate>
                    </Templates>
                    <Templates>
                        <obout:GridTemplate ID="GVDefault" runat="server">
                            <Template>
                                <input type="checkbox" id="chkIsDefault" onclick="getDefault(this,'<%# (Container.DataItem["Sequence"].ToString()) %>')"
                                    <%# Container.Value.ToString().ToLower()=="true" ? "checked='checked'" : "" %> />
                            </Template>
                        </obout:GridTemplate>
                    </Templates>
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
    function openContactPersonWindow(Sequence) {
        var hdnConPersonTargetObject = document.getElementById("hdnConPersonTargetObject");
        window.open('../ContactPerson/ContactPersonInfo.aspx?Sequence=' + Sequence + '&TargetObject=' + hdnConPersonTargetObject.value + '', null, 'height=200px, width=900px,status=0, resizable=0, scrollbars=0, toolbar=0,menubar=0');
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
        var m = GVContactPerson.GridBodyContainer;
        var allInput = m.getElementsByTagName('input');
        for (var c = 0; c <= allInput.length - 1; c++) {
            var chk = allInput[c];
            if (chk.type == "checkbox") { chk.checked = false; }
        }
        invoker.checked = true;
    }


    function MoveToArchiveContactPerson() {
        var Ids = document.getElementById("hdnSelectedRec");
        for (var i = 0; i < GVContactPerson.SelectedRecords.length; i++) {
            var record = GVContactPerson.SelectedRecords[i];
            if (Ids.value != "") Ids.value += ',' + record.Sequence;
            if (Ids.value == "") Ids.value = record.Sequence;
        }
        PageMethods.PMMoveContactPersonToArchive(Ids.value, "Y");
        Ids.value = "";
        GVContactPerson.refresh();
    }

</script>
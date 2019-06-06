<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/CRM2.Master" AutoEventWireup="true" CodeBehind="PMethodList.aspx.cs"
    Inherits="BrilliantWMS.Account.PMethodList" Theme="Blue" %>

<%@ Register Assembly="obout_Grid_NET" Namespace="Obout.Grid" TagPrefix="obout" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <asp:HiddenField ID="hdndeptID" runat="server" />
    <table class="gridFrame" width="800px" style="margin: 3px 3px 3px 3px;">
        <tr>
            <td>
                <table style="width: 100%">
                    <tr>
                        <td style="text-align: left;">
                            <asp:Label ID="lblheader" CssClass="headerText" runat="server" Text="Payment Method List"></asp:Label>
                        </td>
                        <td style="text-align: right;"></td>
                        <td style="text-align: right;">
                            <input type="button" runat="server" value="Submit" id="btnSubmitProductSearch1" onclick="SaveRecord();" />
                            <input type="button" runat="server" value="  Add  " id="btnadd" onclick="OpenPMethod();" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <obout:Grid ID="GridpayMethod" runat="server" AutoGenerateColumns="false" AllowFiltering="false" AllowMultiRecordSelection="false"
                    AllowGrouping="true" AllowColumnResizing="true" AllowAddingRecords="false" AllowColumnReordering="true"
                    CallbackMode="true" Width="100%" Serialize="false"
                    PageSize="25" AllowPageSizeSelection="false" AllowManualPaging="true" ShowTotalNumberOfPages="false"
                    KeepSelectedRecords="false">
                    <ClientSideEvents ExposeSender="true" />
                    <Columns>
                        <obout:Column DataField="Remove" HeaderText="Remove" Width="5%" AllowSorting="false"
                            Align="Center" HeaderAlign="Center" Index="1">
                            <TemplateSettings TemplateId="GvRemoveSku" />
                        </obout:Column>
                        <obout:Column DataField="ID" Visible="false">
                        </obout:Column>
                        <obout:Column DataField="MethodName" HeaderText="Method Name" Align="left" HeaderAlign="left"
                            Width="15%" AllowFilter="false" ParseHTML="true" Index="2">
                        </obout:Column>
                        <obout:Column DataField="Sequence" HeaderText="Sequence" Align="left" HeaderAlign="left"
                            Width="10%" AllowFilter="false" ParseHTML="true" Index="3">
                        </obout:Column>
                    </Columns>
                    <Templates>
                        <obout:GridTemplate ID="GvRemoveSku" runat="server">
                            <Template>
                                <img id="imgbuttonremove" src="../App_Themes/Grid/img/Remove16x16.png" alt="Remove" title="Remove" onclick="RemoveMethod('<%# (Container.DataItem["ID"].ToString()) %>');"
                                    style="cursor: pointer;" />
                                <%--RemoveSkuRecord--%>
                            </Template>
                        </obout:GridTemplate>
                    </Templates>
                </obout:Grid>
            </td>
        </tr>
        <tr>
            <td>
                <table style="width: 100%">
                    <tr>
                        <td style="text-align: left;"></td>
                        <td style="text-align: right;">
                            <input type="button" runat="server" value="Submit" id="btnSubmitProductSearch2" onclick="SaveRecord();" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <script type="text/javascript">
        function SaveRecord()
        {
            var DeptID = document.getElementById('<%=hdndeptID.ClientID %>').value;
            PageMethods.SaveDeptMethod(DeptID, SubmitMethod_onSuccess, SubmitMethod_onFail)
        }

        function SubmitMethod_onSuccess() {
          self.close();
        }

        function SubmitMethod_onFail() { alert("error"); }


        function RemoveMethod(Id) {
            var obj1 = new Object();
            var Detailid = Id;
            obj1.Methodid = Detailid;
            PageMethods.RemoveSku(obj1, Removesku_onSuccess);
        }

        function Removesku_onSuccess(result) {
            GridpayMethod.refresh();
        }

        function OpenPMethod()
        {
            window.open('../Account/PaymentMethod.aspx', '_blank', 'height=400px, width=505px,status= 0, resizable= 1, scrollbars=1, toolbar=0,location=center,menubar=0, screenX=300; screenY=50');
        }
    </script>
</asp:Content>

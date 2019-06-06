<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/CRM.Master" AutoEventWireup="true" CodeBehind="DirectImportV.aspx.cs"
     Inherits="BrilliantWMS.PowerOnRent.DirectImportV" Theme="Blue" %>

<%@ Register Assembly="obout_Grid_NET" Namespace="Obout.Grid" TagPrefix="obout" %>
<%@ Register Src="../CommonControls/UCFormHeader.ascx" TagName="UCFormHeader" TagPrefix="uc2" %>
<%@ Register Src="../CommonControls/Toolbar.ascx" TagName="Toolbar" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHolder_FormHeader" runat="server">
    <uc2:UCFormHeader ID="UCFormHeader1" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPHolder_Form" runat="server">
     <div id="divLoading" style="height: 110%; width: 100%; display: none; top: 0; left: 0;"
        class="modal" runat="server" clientidmode="Static">
    </div>
    <div  id="divRequestHead">
        <h4>
            <asp:Label ID="lblHeading" runat="server" Text="Import Sales Order"></asp:Label>
        </h4>
    </div>
    <div class="divHead" id="div1">
        <table>
            <tr>
                <td class="style3">
                    <center>
                        <table align="center">
                            <tr>
                                <td class="tdCartStepHolder">
                                    <span class="cartStepHolder"><span class="cartStepTitle"><span class="divCartSymbol">
                                        1</span><asp:Label ID="lblstep1" runat="server" Text="Upload File"></asp:Label></span><span class="cartStepTitle cartStepCurrentTitle"><span
                                            class="divCartSymbol divCartCurrentSymbol">2</span><asp:Label ID="lblstep2" runat="server" Text="Data Validation & Verification"></asp:Label></span><span
                                                class="cartStepTitle"><span class="divCartSymbol">3</span><asp:Label ID="lblstep3" runat="server" Text="Finished"></asp:Label></span></span>
                                </td>
                            </tr>
                        </table>
                    </center>
                </td>
            </tr>
            <tr>
                <td class="style3">
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:Label ID="lblbackMessage" runat="server" style="float: inherit;" ForeColor="Red"
                        Font-Size="Large" Font-Bold="true" Text=""></asp:Label>
                    <asp:Label ID="lblOkMessage" runat="server" style="float: inherit;" ForeColor="Blue"
                        Font-Size="Large" Font-Bold="true" Text=""></asp:Label>
                </td>
            </tr>

            <tr>
                <td colspan="2">
                    <table class="gridFrame" style="width: 100%">
            <tr>
                <td style="text-align: left;">
                   <asp:Label class="headerText" ID="lbladdresslist" runat="server" Text="SKU List"></asp:Label>
                </td>
                <td style="text-align: right;">
                    <asp:Button ID="btnnext" runat="server" Text="   Next   " style="width:90px;" font-size="18px" onclick="btnnext_Click" />
                    </td>
                <td style="text-align: right;">
                      <asp:Button ID="btnback" runat="server" Text="   Back   " style="width:90px;" font-size="18px" onclick="btnback_Click" />
                </td>
            </tr>
            <tr>
                <td >
                    <obout:Grid ID="GVImportView" runat="server" AllowGrouping="true" Serialize="false"
                        CallbackMode="true" AllowRecordSelection="true" AllowColumnReordering="true"
                        AllowMultiRecordSelection="false" AllowFiltering="false" Width="116%" PageSize="10"
                        AutoGenerateColumns="false" OnRowDataBound="GVImportView_OnRowDataBound" AllowAddingRecords="false">
                         <ScrollingSettings ScrollHeight="130" />
                        <Columns>
                            <obout:Column DataField="Sequence" HeaderText="Sr. No." AllowEdit="false" Width="2%"
                                Align="center" HeaderAlign="center">
                                <TemplateSettings TemplateId="grdSrNo" />
                            </obout:Column>
                            <obout:Column DataField="SKUCode" HeaderText="SKU Code" HeaderAlign="left" Align="left" Width="6%"> </obout:Column>
                            <obout:Column DataField="Name" HeaderText="SKU Name" HeaderAlign="left" Align="left" Width="8%"> </obout:Column>
                            <obout:Column DataField="Description" HeaderText="SKU Description" HeaderAlign="left" Align="left" Width="11%"></obout:Column>
                            <obout:Column DataField="moq" HeaderText="MOQ" HeaderAlign="center" Align="center" Width="4%"> </obout:Column>
                             <obout:Column DataField="AvailableBalance" HeaderText="Current Qty" HeaderAlign="center" Align="right" Width="5%"> </obout:Column>
                            <obout:Column DataField="ResurveQty" HeaderText="Reserve Qty" HeaderAlign="Center" Align="right" Width="5%"> </obout:Column>
                             <obout:Column DataField="asRequestQty" HeaderText="Request Qty" HeaderAlign="Center" Align="right" Width="5%"> </obout:Column>
                             <obout:Column DataField="UOM" HeaderText="UOM" HeaderAlign="Center" Align="Center" Width="4%"> </obout:Column>
                            <obout:Column DataField="OrderQty" HeaderText="Order Qty" HeaderAlign="center" Align="right" Width="5%"> </obout:Column>
                             <obout:Column DataField="Price" HeaderText="Price" HeaderAlign="center" Align="right" Width="5%"> </obout:Column>
                             <obout:Column DataField="Total" HeaderText="Total" HeaderAlign="center" Align="right" Width="5%"> </obout:Column>
                             <obout:Column DataField="skuchk" HeaderText="skuchk" HeaderAlign="left" Align="center" Width="1%" Visible="false">
                            </obout:Column>
                             <obout:Column DataField="ProdDeptChk" HeaderText="ProdDeptChk" HeaderAlign="left" Align="center" Width="1%" Visible="false">
                            </obout:Column>
                             <obout:Column DataField="Deptchk" HeaderText="Deptchk" HeaderAlign="left" Align="center" Width="1%" Visible="false">
                            </obout:Column>
                             <obout:Column DataField="Locationchk" HeaderText="Locationchk" HeaderAlign="left" Align="center" Width="1%" Visible="false">
                            </obout:Column>
                            <obout:Column DataField="Balancechk" HeaderText="Balancechk" HeaderAlign="left" Align="left" Width="1%" Visible="false">
                            </obout:Column>
                             <obout:Column DataField="MOQChk" HeaderText="MOQChk" HeaderAlign="left" Align="right" Width="1%" Visible="false">
                            </obout:Column>
                            <obout:Column DataField="DeptCode" HeaderText="Dept Code" HeaderAlign="left" Align="center" Width="6%"> </obout:Column>
                            <obout:Column DataField="LocationCode" HeaderText="Location Code" HeaderAlign="left" Align="center" Width="6%"> </obout:Column>

                                                      
                        </Columns>
                        <Templates>
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
                </td>
            </tr>
            <tr>
                <td class="style3"></td>
            </tr>
            <tr>
            <td ></td>
            </tr>
            <tr>
            <td ></td>
            </tr>
            <tr>
                <td style="margin-right">
                    <br/>
                    <table style="margin-right" align="right">
                        <tr>
                            <td>
                                


                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    <style type="text/css">
        /*Grid css*/
        .excel-textbox
        {
            background-color: transparent;
            border: 0px;
            padding: 0px;
            outline: 0;
            font: inherit;
            width: 91%;
            padding-top: 4px;
            padding-left: 2px;
            padding-bottom: 4px;
            text-align: left;
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
            float: left;
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
   
    <style type="text/css">
        .has-js .label_check, .has-js .label_radio
        {
            padding-left: 25px;
            padding-bottom: 10px;
        }
        .has-js .label_radio
        {
            background: url(../AdministratorPortal/images/radio-off.png) no-repeat;
        }
        .has-js .label_check
        {
            background: url("../AdministratorPortal/images/check-off.png") no-repeat;
        }
        .has-js label.c_on
        {
            background: url("../AdministratorPortal/images/check-on.png") no-repeat;
        }
        .has-js label.r_on
        {
            background: url(../AdministratorPortal/images/radio-on.png) no-repeat;
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
            padding: 0px 50px 40px 0px;
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
            background-image: url(../images/opt-bg-normal1.png);
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
            background-image: url(../images/opt-bg-selected2.png);
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
            width: 1338px;
        }
        .btnCommonStyle
        {
            font-family: inherit;
            font-weight: bold;
            font-size: 20px;
            color: #ffffff;
            text-decoration: none !important;
            padding-left: 50px;
            padding-right: 50px;
            padding-top: 14px;
            padding-bottom: 14px;
            border-radius: 7px; /* fallback */
            background-color: #1a82f7;
            background: url(../AdministratorPortal/images/btn-common-bg.jpg);
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
        .style3
        {
            width: 1338px;
        }
        
         .class1
       {
           opacity:0.4;
           filter:alpha(opacity=40);
           cursor:wait;
       }
       .class2
       {
           opacity:1;
           filter:alpha(opacity=100);
           cursor:pointer;
       }
    </style>
</asp:Content>

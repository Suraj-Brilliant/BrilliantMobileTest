<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCImport.ascx.cs" Inherits="BrilliantWMS.CommonControls.UCImport"%>
<%@ Register Assembly="obout_Interface" Namespace="Obout.Interface" TagPrefix="obout" %>
<%@ Register Assembly="obout_Grid_NET" Namespace="Obout.Grid" TagPrefix="obout" %>
<div id="divimport" title="Import">
     <asp:UpdateProgress ID="UpdateGirdProductProcess" runat="server">  <%--AssociatedUpdatePanelID="Up_PnlGirdProduct"--%>
                            <ProgressTemplate>
                                <center>
                                    <div class="modal">
                                        <img src="../App_Themes/Blue/img/test-mentor-loading.gif" style="top: 50%;" />      <%--ajax-loader.gif--%>
                                    </div>
                                </center>
                            </ProgressTemplate>
     </asp:UpdateProgress>
  
       <asp:UpdatePanel ID="Up_PnlGirdProduct" runat="server" >
           <ContentTemplate>
               <asp:Panel ID="pnlImport" runat="server" Visible="false">
     <div  id="divimport">
        <h4>
            <asp:Label ID="lblimportdata" runat="server" Text="Import Data"></asp:Label>
            <asp:HiddenField ID="hdnobject" runat="server" ClientIDMode="Static" />
        </h4>
    </div>
    <center>
        <table class="tblcls" cellpadding="0" cellspacing="20px" border="0" width="95%">
            <tr>
                <td align="center">
                    <table align="center" border="0">
                        <tr>
                            <td class="style3" align="center">
                                <span class="cartStepHolder"><span class="cartStepTitle cartStepCurrentTitle">
                                    <span class="divCartSymbol divCartCurrentSymbol"><asp:Label ID="lbl1" runat="server" Text="1"/></span><asp:Label ID="lblstep1" runat="server" Text="Upload File"></asp:Label></span>
                                    <span class="cartStepTitle"><span class="divCartSymbol"><asp:Label ID="lbl2" runat="server" Text="2"/></span><asp:Label ID="lblstep2" runat="server" Text="Data Validation & Verification"></asp:Label></span>
                                    <span class="cartStepTitle"><span class="divCartSymbol"><asp:Label ID="lbl3" runat="server" Text="3"/></span><asp:Label ID="lblstep3" runat="server" Text="Finished"></asp:Label></span>
                                </span>
                            </td>
                        </tr>
                    </table>
                    <table cellpadding="0" width="100%" cellspacing="5px" border="0" class="tableForm">
                        <tr>
                            <td style="text-align :left" colspan="4">
                               
                                     <asp:Label ID="lbltext1" runat="server" Font-Size="14px" Text="Import will faciliated Order Data to be directly imported into WMS."></asp:Label>
                                   <%-- <span style="font-size:14px;"> Import will faciliated Order Data to be directly imported into OMS.</span><br />--%>
                                    <asp:Label ID="Lbl" runat="server" Font-Size="14px" Text="For Downloading Order Import Data Template"></asp:Label>
                                    <a href="DirectOrderImport.xlsx" id="downloadlink" runat="server" target="_blank"><font color="blue"><u>Click Here.
                                   </u></font></a>
                               <%--  <a href="DirectOrderImport.xlsx" id="A1" runat="server" target="_blank"><font color="blue"><u>Click Here.
                                   </u></font></a>--%>
                               
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align :left" colspan="4">

                            </td>
                        </tr>
                         <tr>
                           <td style="text-align :left" colspan="4">
                              <table>
                                 <tr>
                                     <td>
                                        <asp:Label ID="lblSelecFile" runat="server" Text="Select Import File"></asp:Label> :
                                     </td>
                                     <td style="text-align: left;">
                                         <asp:FileUpload ID="FileuploadPO" runat="server" />
                                    </td>
                                     <td style="text-align: right;" >
                                         <asp:ImageButton ID="btnupload" runat="Server" ImageUrl="../App_Themes/Blue/img/Upload2.jpg" Width="88px" Height="39px" OnClientClick="return ProgressBar()" OnClick="btnUploadPo_Click"  CommandName="ClientSideButton" />
                                         <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="FileuploadPO" ForeColor="Red"
                                          ErrorMessage="Upload only excel file" ValidationExpression="^.+(.xls|.XLS|.xlsx|.XLSX)$">
                                          </asp:RegularExpressionValidator>
                                    </td>
                                </tr>
                             </table>
                           </td>
                        </tr>
                         <tr>
                        <td style="text-align:center;" colspan="4">
                        <div id="divUpload" style="display: none">
                       <%-- <b><i>Uploading file...</i></b><br />--%>
                            <span style="text-align:center; float:inherit"><asp:Label ID="lbluploading" runat="server" Font-Bold="true" Font-Italic="true" Text="Uploading file..."></asp:Label></span><br />
                            <asp:Image ID="uploadimg" runat="server" ImageUrl="~/images/upload-animation.gif" Width="380" Height="20" />
                             <asp:Panel ID="uploadMessage" runat="server" Font-Bold="true" style="text-align:center;">File uploaded successfully! Click On Next Button </asp:Panel>
                             
                            </div>
                           
                        </td>
                        </tr>
                        <tr>
                           <td style="text-align:center;" colspan="4">
                              <asp:Label ID="lblmessagesuccess" runat="server" style="text-align:center" Font-Size="16px" ForeColor="#206295" Font-Bold="true"  Text="File uploaded successfully! Click On Next Button"></asp:Label>
                           </td>
                        </tr>
                    </table>
                </td>
            </tr>

            <tr>
                <td >
                    <table align="right">
                        <tr>
                            <td>
                                <asp:ImageButton ID="btnimpnext" runat="Server" ImageUrl="../App_Themes/Blue/img/next.jpg" Width="100px" Height="53px" OnClick="btnimportNext_Click" CausesValidation="false"  CommandName="ClientSideButton" />
                                <%--<asp:Button ID="btnimportNext" runat="server" Text="  Next  " OnClick="btnimportNext_Click" CausesValidation="false"   />--%>     <%-- OnClientClick="return CheckDeptvalidations();"--%>
                            </td>
                            <td>
                                 <asp:ImageButton ID="ImageButton2" runat="Server" ImageUrl="../App_Themes/Blue/img/back.jpg" Width="100px" Height="51px" OnClick="btnimportcancel_Click" CausesValidation="false"  CommandName="ClientSideButton" />
                                <%--<asp:Button ID="btnimportcancel" runat="server" Text="Cancel" OnClick="btnimportcancel_Click" CausesValidation="false"  />  --%>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </center>
    </asp:Panel>
    <asp:Panel ID="pnlvalidate" runat="server" Visible="false">
     <div  id="divRequestHead">
        <h4>
            <asp:Label ID="lblHeading" runat="server" Text="Validate Data"></asp:Label>
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
                                        1</span><asp:Label ID="Label1" runat="server" Text="Upload File"></asp:Label></span><span class="cartStepTitle cartStepCurrentTitle"><span
                                            class="divCartSymbol divCartCurrentSymbol">2</span><asp:Label ID="Label2" runat="server" Text="Data Validation & Verification"></asp:Label></span><span
                                                class="cartStepTitle"><span class="divCartSymbol">3</span><asp:Label ID="Label3" runat="server" Text="Finished"></asp:Label></span></span>
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
                   <asp:Label class="headerText" ID="lbladdresslist" runat="server" Text="Import List"></asp:Label>
                </td>
                <td style="text-align: right;">
                    
                    </td>
                <td style="text-align: right;">
                    <asp:Button ID="btnnext" runat="server" Text="   Next   " style="width:90px;" font-size="18px" onclick="btnnext_Click" />
                      <asp:Button ID="btnback" runat="server" Text="   Back   " style="width:90px;" font-size="18px" onclick="btnback_Click" />
                </td>
            </tr>
            <tr>
                <td colspan="3">                                                                                        <%-- OnRowDataBound="GVImportView_OnRowDataBound"--%>
                    <obout:Grid ID="GVImportView" runat="server" AllowGrouping="true" Serialize="false" CallbackMode="true" AllowRecordSelection="true"
                      AllowColumnReordering="true" AllowMultiRecordSelection="false" AllowColumnResizing="true" AllowFiltering="false" Width="116%" PageSize="10"  OnRowDataBound="GVImportView_OnRowDataBound" 
                         AllowAddingRecords="false"> 
                        <ScrollingSettings ScrollHeight="130" ScrollWidth="1310"/>
                        <%--<ScrollingSettings ScrollWidth="1310" />    --%>    
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
    </asp:Panel>
    <asp:Panel ID="pnlfinish" runat="server" Visible="false">
           <div class="divHead" id="div2">
        <h4><asp:Label ID="Label4" runat="server" Text="Import Finish"></asp:Label></h4>
    </div>
           <div class="divHead" id="div3">
        <center>
            <table class="tblcls" cellpadding="0" cellspacing="20px" border="0" width="95%">
                <tr>
                    <td style="text-align:center" class="tdCartStepHolder">
                              <span class="cartStepHolder"><span class="cartStepTitle"><span class="divCartSymbol">
                            1</span><asp:Label ID="Label5" runat="server" Text="Upload File"></asp:Label></span><span class="cartStepTitle"><span class="divCartSymbol">2</span>
                                <asp:Label ID="Label6" runat="server" Text="Data Validation & Verification"></asp:Label></span><span class="cartStepTitle cartStepCurrentTitle"><span
                                    class="divCartSymbol divCartCurrentSymbol">3</span><asp:Label ID="Label7" runat="server" Text="Finished"></asp:Label></span></span>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <hr style="border-color: #F5DEB3" />
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td style="text-align:center">
                        <span>
                            <h1>
                                Data Importing Successfully Finished!
                            </h1>
                        </span>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td class="style2" style= "margin-left">
                       <asp:Button ID="btnfinish" runat="server" Text="Finish" style="width:90px; float:right;" font-size="18px" onclick="btnfinish_Click" />
                    </td>
                </tr>
            </table>
        </center>
    </div>

    </asp:Panel>
    </ContentTemplate>
            <Triggers>
                   <asp:PostBackTrigger ControlID = "btnupload" />
              </Triggers>
  </asp:UpdatePanel>
</div>
 <style type="text/css">
        /*POR Collapsable Div*/
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
        .PanelCaption
        {
            color: Black;
            font-size: 13px; /*font-weight: bold;*/
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
            color: #483D8B; /*margin: 3px 3px 3px 3px;*/
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
        .cartStepTitle
        {
            font-size: 20px;
            color: #cccccc;
            padding: 0px 50px 20px 0px;
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
               
        .style2
        {
            height: 47px;
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

 <%--******Progress bar script*******--%>
    <script type="text/javascript">
        function ProgressBar() {
            if (document.getElementById('<%=FileuploadPO.ClientID %>').value != "") {
                document.getElementById("divUpload").style.display = "block";
                var getFileUploadMessageObj = document.getElementById('<%=uploadMessage.ClientID %>');
                if (getFileUploadMessageObj != null) {
                    getFileUploadMessageObj.style.display = 'none';
                }

                return true;
            }
            else {
                alert("Select a file to upload");
                return false;
            }

        }
    </script>

<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UC_Approval.ascx.cs"
    Inherits="BrilliantWMS.Approval.UC_Approval" %>
<%@ Register Assembly="obout_Flyout2_NET" Namespace="OboutInc.Flyout2" TagPrefix="cc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<input type="button" value="Approval" id="btnApproval" runat="server" onclick="openApprovalFlyout()" />
<cc2:Flyout ID="FlyoutUCApproval" runat="server" AttachTo="btnApproval" OpenEvent="NONE"
    CloseEvent="NONE" IsModal="true" PageColor="Black" PageOpacity="60" zIndex="999"
    Position="ABSOLUTE" RelativeLeft="-530" RelativeTop="-20">
    <table class="gridFrame" width="450px">
        <tr>
            <td>
                <cc2:DragPanel ID="DragPanel1" runat="server">
                    <table width="100%">
                        <tr>
                            <td style="text-align: left;">
                                <a class="headerText">Approval</a>
                            </td>
                            <td style="text-align: right;">
                                <input type="button" value="Save" id="btnApprovalSave" onclick="UpdateApproval()" />
                                <input type="button" value="Clear" id="btnApprovalClear" onclick="clearallAssignTask()" />
                                <input type="button" value="Close" id="btnApprovalClose" onclick="<%=FlyoutUCApproval.getClientID()%>.Close();" />
                            </td>
                        </tr>
                    </table>
                </cc2:DragPanel>
            </td>
        </tr>
        <tr>
            <td>
                <table class="tableForm" style="background-color: White;" width="100%">
                    <tr>
                        <td>
                            <req>Approval Status :</req>
                        </td>
                        <td style="text-align: left;">
                            <asp:DropDownList ID="ddlApprovalStatus" runat="server" onchange="isReqApprovalRemark(this);"
                                Width="200px">
                                <asp:ListItem Text="-Select-" Value="0">
                                </asp:ListItem>
                                <asp:ListItem Text="Approved" Value="Approved">
                                </asp:ListItem>
                                <%-- <asp:ListItem Text="Approved with revision" Value="AwR">
                                </asp:ListItem>--%>
                                <asp:ListItem Text="Rejected" Value="Rejected">
                                </asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span id="spanApprovalRemark">Remark :</span>
                        </td>
                        <td style="text-align: left">
                            <asp:TextBox ID="txtApprovalRemark" runat="server" Width="300px" MaxLength="200"
                                TextMode="MultiLine" onkeyup="TextBox_KeyUp(this,'SpanCharCountertxtApprovalRemark','200');"
                                ClientIDMode="Static"></asp:TextBox>
                            <br />
                            <span class="watermark"><span id="SpanCharCountertxtApprovalRemark">200</span> characters
                                remaning out of 200 </span>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <asp:HiddenField ID="hdnApprovalDetailIDs" ClientIDMode="Static" runat="server" />
</cc2:Flyout>
<script type="text/javascript">
    function isReqApprovalRemark(invoker) {
        var spanApprovalRemark = document.getElementById("spanApprovalRemark");
        spanApprovalRemark.className = "";
        if (invoker.selectedIndex > 1) { spanApprovalRemark.className = "req"; }
    }

    function openApprovalFlyout() { 
    selectedRecForView();

    var hdnApprovalDetailIDs = document.getElementById("hdnApprovalDetailIDs");
    if(hdnApprovalDetailIDs.value == ""){ alert("Select atleast one record for approval");}
    else if(hdnApprovalDetailIDs.value == "0"){ alert("Access denied");}
    else { <%=FlyoutUCApproval.getClientID()%>.Open(); }
    }

    function UpdateApproval(){
        var hdnApprovalDetailIDs = document.getElementById("hdnApprovalDetailIDs");
        var ddlApprovalStatus = document.getElementById("<%= ddlApprovalStatus.ClientID %>");
        var txtApprovalRemark = document.getElementById("<%= txtApprovalRemark.ClientID %>");
        
        if((ddlApprovalStatus.selectedIndex > 1) && txtApprovalRemark.value == ""){
            alert("Enter remark");
            txtApprovalRemark.focus();
        }
        else{
            PageMethods.wmUpdateApproval(ddlApprovalStatus.options[ddlApprovalStatus.selectedIndex].value, txtApprovalRemark.value,hdnApprovalDetailIDs.value, wmUpdateApproval_OnSuccess, wmUpdateApproval_OnFail);
        }
    }

    function wmUpdateApproval_OnSuccess(result){
        var hdnApprovalDetailIDs = document.getElementById("hdnApprovalDetailIDs");
        hdnApprovalDetailIDs.value = ""; 
        GvDefaultView1.refresh();
        <%=FlyoutUCApproval.getClientID()%>.Close();
        alert(result);
    }

    function wmUpdateApproval_OnFail(){

    }
</script>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AssignTask.aspx.cs" Inherits="BrilliantWMS.WMS.AssignTask" EnableEventValidation="false" %>

<script src="../MasterPage/JavaScripts/crm.js" type="text/javascript"></script>
<%@ Register Assembly="obout_Interface" Namespace="Obout.Interface" TagPrefix="cc4" %>
<%@ Register Assembly="obout_Flyout2_NET" Namespace="OboutInc.Flyout2" TagPrefix="cc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../CommonControls/UC_Date.ascx" TagName="UC_Date" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Brilliant WMS</title>
</head>
<body onload="AssignTaskOnLoad();">
    <form id="form1AssignTask" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnablePageMethods="true"
        EnablePartialRendering="true">
    </asp:ToolkitScriptManager>
    <div>
        <center>
            <div id="divLoading" style="height: 308px; width: 465px; display: none; top: 0; left: 0;"
                class="modal">
                <center>
                    <br />
                    <span id="spanLoading" style="font-size: 17px; font-weight: bold; color: Yellow;">Processing
                        please wait...</span>
                </center>
            </div>
            <table class="gridFrame" style="margin-top: 13px; width: 465px;">
                <tr>
                    <td style="text-align: left;">
                        <span id="headerText">Create Task</span>
                    </td>
                    <td style="text-align: right;">
                        <input type="button" value="Save" id="btnSaveAssignTask" runat="server" onclick="SaveAssignTask()" />
                        <input type="button" value="Clear" id="btnClearAssignTask" onclick="clearallAssignTask(); TextBox_KeyUp(this, 'SpanCharCounterAssignTask', '205');" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <table class="tableForm" style="background-color: White; width: 100%">
                            <tr>
                                <td colspan="4" style="text-align: center;">
                                    <asp:Label ID="lblmgs" Font-Italic="true" Font-Size="Medium" Font-Bold="true" runat="server"
                                        ForeColor="Maroon" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <req>Object Name : </req>
                                </td>
                                <td colspan="3" style="text-align: left;">
                                    <asp:DropDownList ID="ddlObjectName" runat="server" 
                                        DataTextField="ObjectName" DataValueField="ObjectID" Width="340px" ValidationGroup="AssignTask">
                                    </asp:DropDownList> <%--onchange="FillAssignTo();FillActivity();"--%>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <req>Assign To : </req>
                                </td>
                                <td colspan="3" style="text-align: left;">
                                    <asp:DropDownList ID="ddlAssignTo" runat="server" DataTextField="userName" DataValueField="userID"
                                        Width="340px" ValidationGroup="AssignTask">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="req_ddlAssignTo" runat="server" ErrorMessage="Select Assign To"
                                        ControlToValidate="ddlAssignTo" InitialValue="0" ForeColor="Red" ValidationGroup="AssignTask"
                                        Display="None">
                                    </asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <req>Job Card Name : </req>
                                </td>
                                <td colspan="3" style="text-align: left;">
                                    <asp:TextBox ID="txtJobCardName" runat="server" Width="340px" ValidationGroup="AssignTask"></asp:TextBox>
                                    <%--<asp:DropDownList ID="ddlActivityType" runat="server" Width="340px" ValidationGroup="AssignTask"
                                        DataTextField="ActivityName" DataValueField="ID">
                                    </asp:DropDownList>--%>
                                    <asp:RequiredFieldValidator ID="req_ddlActivityType" runat="server" ErrorMessage="Enter Job Card Name"
                                        ControlToValidate="txtJobCardName"  ForeColor="Red" ValidationGroup="AssignTask"
                                        Display="None">
                                    </asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <req>Priority : </req>
                                </td>
                                <td style="text-align: left;">
                                    <asp:DropDownList ID="ddlPriority" runat="server" Width="125px" ValidationGroup="AssignTask">
                                        <asp:ListItem Text="-Select-" Selected="True" Value="0" />
                                        <asp:ListItem Text="High" Value="High" />
                                        <asp:ListItem Text="Medium" Value="Medium" />
                                        <asp:ListItem Text="Low" Value="Low" />
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="req_ddlPriority" runat="server" ErrorMessage="Select Priority"
                                        ControlToValidate="ddlPriority" InitialValue="0" ForeColor="Red" ValidationGroup="AssignTask"
                                        Display="None">
                                    </asp:RequiredFieldValidator>
                                </td>
                                <td>
                                    <req>ECD : </req>
                                </td>
                                <td style="text-align: left" id="tD_UC_ECDate">
                                    <uc1:UC_Date ID="UC_ECDate" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Remark :
                                </td>
                                <td colspan="3" style="text-align: left;">
                                    <asp:TextBox ID="txtRemarks" runat="server" Width="340px" MaxLength="200" TextMode="MultiLine"
                                        onkeyup="TextBox_KeyUp(this,'SpanCharCounterAssignTask','200');" onkeydown="return barNotallow(this,event);"
                                        onkeypress="return barNotallow(this,event);" ClientIDMode="Static"></asp:TextBox>
                                    <br />
                                    <span class="watermark"><span id="SpanCharCounterAssignTask">200</span> characters remaning
                                        out of 200 </span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Reminder :
                                </td>
                                <td style="text-align: left;" id="tD_UCReminderDate">
                                    <uc1:UC_Date ID="UCReminderDate" runat="server" />
                                </td>
                                <td>
                                    Reminder Type :
                                </td>
                                <td style="text-align: left;">
                                    <cc4:OboutCheckBox ID="chkbxEmail" runat="server" Text="Email" Checked="true">
                                    </cc4:OboutCheckBox>
                                    <cc4:OboutCheckBox ID="chkbxSMS" runat="server" Text="SMS">
                                    </cc4:OboutCheckBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <span class="watermark">ECD : Expected completion date</span>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </center>
    </div>
    <asp:HiddenField ID="hdnSelectedRec_CT" ClientIDMode="Static" runat="server" />
    </form>
</body>
</html>
<script type="text/javascript">
    function AssignTaskOnLoad() {
        document.getElementById("hdnSelectedRec_CT").value = window.opener.document.getElementById("hdnSelectedRec").value;
    }

    function clearallAssignTask() {
        var ddlAssignTo = document.getElementById("<%= ddlAssignTo.ClientID %>").value = 0;
        var ddlPriority = document.getElementById("<%= ddlPriority.ClientID %>").value = 0;
        var txtJobCardName = document.getElementById("<%= txtJobCardName.ClientID%>").value = 0;
        var ObjectName = document.getElementById("<%= ddlObjectName.ClientID %>").value = 0;


        var ECD_Date = document.getElementById("tD_UC_ECDate");
        var enddateallinput = ECD_Date.getElementsByTagName('input');
        for (i = 0; i < enddateallinput.length; i++) {
            if (enddateallinput[i].type == "text") {
                enddateallinput[i].value = '';
            }
        }

        var ReminderDate = document.getElementById("tD_UCReminderDate");
        var reminderdateallinput = ReminderDate.getElementsByTagName('input');
        for (i = 0; i < reminderdateallinput.length; i++) {
            if (reminderdateallinput[i].type == "text") {
                reminderdateallinput[i].value = '';
            }
        }

        var checkBoxsms = eval("chkbxSMS");
        checkBoxsms.checked(false);
        var checkBoxEmail = eval("chkbxEmail");
        checkBoxEmail.checked(false);
        var txtRemarks = document.getElementById("<%= txtRemarks.ClientID %>");

        txtRemarks.value = '';
    }


    function FillAssignTo() {
        LoadingOn();
        var ddlObjectName = document.getElementById("<%= ddlObjectName.ClientID %>").value;
        PageMethods.FillAssignTo(ddlObjectName, OnSucessCheckFillAssginTo, OnfailedCheckFillAssginTo);
    }


    function OnSucessCheckFillAssginTo(result) {
        var ddlAssignTo = document.getElementById("<%= ddlAssignTo.ClientID %>");
        document.getElementById("ddlAssignTo").options.length = 0;
        var option = document.createElement("option");
        option.text = '-Select-';
        option.value = '0';

        try {
            ddlAssignTo.add(option, null); //Standard 
        } catch (error) {
            ddlAssignTo.add(option); // IE only
        }

        for (var i = 0; i < result.length; i++) {
            var option = document.createElement("option");
            if (result[i].Assignto != "Report To Supervisor") {
                option.text = result[i].Assignto;
                option.value = result[i].ID;
            }
            else {
                option.text = "Report To Supervisor";
                option.value = "0.1";
            }

            try {
                ddlAssignTo.add(option, null); //Standard 
            } catch (error) {
                ddlAssignTo.add(option); // IE only
            }
        }

        LoadingOff();
    }

    function OnfailedCheckFillAssginTo() {
        LoadingOff();
    }


    function FillActivity() {
        LoadingOn();
        var ObjectName = document.getElementById("<%= ddlObjectName.ClientID %>").value;
        PageMethods.FillActivity(ObjectName, OnSucessCheckFillActivity, OnfailedCheckFillActivity);
    }

   

    function OnfailedCheckFillActivity() {
        LoadingOff();
    }

    function SaveAssignTask() {
        if (typeof (Page_ClientValidate) == 'function') {
            Page_ClientValidate();
        }
        if (Page_IsValid) {
            LoadingOn();
            var SelectedRec = "0";
            if (getParameterByName("ReferenceID") != "") {
                SelectedRec = getParameterByName("ReferenceID");
            }
            else {
                SelectedRec = document.getElementById("hdnSelectedRec_CT").value;
            }
            var QueryStringObject = getParameterByName("invoker");
            var State = getParameterByName("State");

            var ECD_Date = document.getElementById("tD_UC_ECDate");
            var ECDallinput = ECD_Date.getElementsByTagName('input');
            for (i = 0; i < ECDallinput.length; i++) {
                if (ECDallinput[i].type == "text") { var UC_ECDate = ECDallinput[i].value; }
            }

            var ReminderDate = document.getElementById("tD_UCReminderDate");

            var Remainderallinput = ReminderDate.getElementsByTagName('input');
            for (i = 0; i < Remainderallinput.length; i++) {
                if (Remainderallinput[i].type == "text") {
                    var UCReminderDate = Remainderallinput[i].value;
                }
            }
            var txtJobCardName = document.getElementById("<%= txtJobCardName.ClientID %>").value;
           // var ActivityID = strActivity[0];
            //var SubActivityID = strActivity[1];
            var Priority = document.getElementById("<%= ddlPriority.ClientID %>").value;
            var AssignTo = document.getElementById("<%= ddlAssignTo.ClientID %>").value;
            var Remarks = document.getElementById("<%= txtRemarks.ClientID %>").value;
            var chkbxEmail = document.getElementById("<%= chkbxEmail.ClientID %>");
            var checked = chkbxEmail.checked;
            var AssignTo = document.getElementById("<%= ddlAssignTo.ClientID %>").value;
            var ObjectName = document.getElementById("<%= ddlObjectName.ClientID %>").value;
            PageMethods.PMSaveAssignTask(ObjectName, txtJobCardName, AssignTo, UC_ECDate, Remarks, checked, UCReminderDate, Priority, OnSucessSaveAssignTask, OnfailedSaveAssignTask);
        }
    }

    function OnSucessSaveAssignTask(result) {
        if (result.toLowerCase() == "true") {
            RefreshDefaultViewGrid();
            alert('Task Saved Successfully');
            LoadingOff();
            var someSession = '<%= Session["ObjectName"].ToString() %>';
            //if (someSession == 'PurchaseOrder') {
            //    window.parent.grdPurchaseOrder.refresh();
            //}
            self.close();
        }
    }

    function OnfailedSaveAssignTask() { alert("Some error occurred"); LoadingOff(); }

</script>

<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UC_Territory.ascx.cs"
    Inherits="BrilliantWMS.Territory.UC_Territory" %>
<center>
    <table class="tableForm" style="border: none; -webkit-box-shadow: none; -moz-box-shadow: none;
        box-shadow: none; padding: 0;">
        <tr>
            <td>
                <asp:ListView ID="LstTerritory" runat="server" GroupItemCount="3">
                    <GroupTemplate>
                        <tr id="itemPlaceholderContainer" runat="server">
                            <td id="itemPlaceholder" runat="server">
                            </td>
                        </tr>
                    </GroupTemplate>
                    <ItemTemplate>
                        <td id="Td1" runat="server">
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblLevel" runat="server" Text='<%# Eval("GroupTitle") %>' CssClass='<%# Eval("IsMandatory").ToString() %>' />
                                        :
                                    </td>
                                    <td style="text-align: left;">
                                        <%--   <asp:DropDownList ID="ddlTerritory" Width="180px" runat="server" DataValueField="ID"
                                            DataTextField="Territory" onchange="fillDDL(this); setTerritoryID(this);">
                                        </asp:DropDownList>--%>
                                        <asp:DropDownList ID="ddlTerritory" Width="172px" runat="server" DataValueField="ID"
                                            DataTextField="Territory">
                                        </asp:DropDownList>
                                        <asp:HiddenField ID="hdnTerritoryID" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </ItemTemplate>
                    <LayoutTemplate>
                        <table id="groupPlaceholderContainer" runat="server" cellpadding="0" cellspacing="0"
                            class="tableForm" style="border: none; -webkit-box-shadow: none; -moz-box-shadow: none;
                            box-shadow: none; padding: 0;">
                            <tr id="groupPlaceholder" runat="server">
                            </tr>
                        </table>
                    </LayoutTemplate>
                </asp:ListView>
            </td>
        </tr>
        <tr>
            <td style="text-align: right;">
                <div style="margin-right: 16px; margin-top: -5px;" id="divUserList" runat="server">
                    <req>Material Allocate To :</req>
                    <asp:DropDownList runat="server" ID="ddlUserList" Width="300px" ClientIDMode="Static"
                        onchange="setUserID(this);">
                    </asp:DropDownList>
                </div>
            </td>
        </tr>
    </table>
    <asp:HiddenField ID="hdnUserID_UT" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnLastLevel_UT" runat="server" ClientIDMode="Static" />
</center>
<style type="text/css">
    .True
    {
        color: Maroon;
        font-size: 12px;
    }
    .False
    {
        color: Black;
        font-size: 12px;
    }
</style>
<script type="text/javascript">
    /*UC_Date Get Textbox or Value*/
    function getTerritoryDDLFromUC(UC_Name, ddlname) {
        var lstview = document.getElementById(UC_Name);
        var allDDL = lstview.getElementsByTagName("select");
        for (var i = 0; i < allDDL.length; i++) {
            if (allDDL[i].id.indexOf(ddlname) != -1) {
                var ddl = allDDL[i];
                return ddl;
            }
        }
    }

    /*End*/

    function setValueToHiddenField(invoker, HiddenFieldID) {
        document.getElementById(HiddenFieldID).value = invoker.value;
    }
    function setTerritoryID(invoker) {
        document.getElementById("hdnTerritoryID_UT").value = invoker.value;
    }
    function setUserID(invoker) {
        document.getElementById("hdnUserID_UT").value = invoker.value;
    }
    var CurrentLevel;


    var parentID = "";
    var childID = "";
    function fillDDL1(parent, child, level) {
        parentID = parent;
        childID = child;
        var childDDL = document.getElementById(child);
        childDDL.options.length = 0;
        ddlLoadingOn(childDDL);
        var parentDDL = document.getElementById(parent);
        PageMethods.PMFillddlLevel(level, parentDDL.value, FillddlLevel_OnSuccess1, FillddlLevel_OnFail);
        FillUserListByTerritory(level - 1, parentDDL);
        clearSelectedLocations();
    }

    function FillddlLevel_OnSuccess1(result) {
        var ddl = document.getElementById(childID);
        var option0 = document.createElement("option");
        if (result.length > 0) {

            if (result[0].IsMandatory.toString().toLowerCase() == "true") {
                option0.text = '-Select ' + result[0].GroupTitle + '-';
            }
            else {
                option0.text = '-Select ' + result[0].GroupTitle + '-';
            }

            option0.value = "0";
            try {
                ddl.add(option0, null);
            }
            catch (Error) {
                ddl.add(option0);
            }
        }

        for (var i = 0; i < result.length; i++) {
            var option1 = document.createElement("option");
            option1.text = result[i].Territory;
            option1.value = result[i].ID;
            try {
                ddl.add(option1, null);
            }
            catch (Error) {
                ddl.add(option1);
            }
        }

        ddlLoadingOff(ddl);
    }

    function FillddlLevel_OnFail() { alert("Error occurred"); }

    /* Code for Fill Co-ordinator */
    function FillUserListByTerritory(level, parentDDL) {
        if (childID == null || childID == "") { childID = parentDDL.id; }
        document.getElementById('ddlUserList').options.length = 0;
        ddlLoadingOn(document.getElementById('ddlUserList'));
        PageMethods.PMFillddlUserListByTerritory(level, parentDDL.value, FillddlUserList_OnSuccess, FillddlUserList_OnFail);
        setTerritoryIDFA(parentID);
    }

    function FillddlUserList_OnSuccess(result) {

        var ddl = document.getElementById('ddlUserList');
        var option0 = document.createElement("option");
        if (result.length > 0) {
            option0.text = '-Select-';
            option0.value = "0";
            try {
                ddl.add(option0, null);
            }
            catch (Error) {
                ddl.add(option0);
            }
        }
        else {
            option0.text = 'N/A';
            option0.value = "0";
            try {
                ddl.add(option0, null);
            }
            catch (Error) {
                ddl.add(option0);
            }
        }

        for (var i = 0; i < result.length; i++) {
            var option1 = document.createElement("option");
            option1.text = result[i].userName;
            option1.value = result[i].userID;
            try {
                ddl.add(option1, null);
            }
            catch (Error) {
                ddl.add(option1);
            }
        }

        ddlLoadingOff(document.getElementById('ddlUserList'));
    }

    function FillddlUserList_OnFail() { alert("Error occured"); }

</script>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PORInbox.aspx.cs" Inherits="BrilliantWMS.Inbox.PORInbox"
    Theme="Blue" %>

<%@ Register Assembly="obout_Grid_NET" Namespace="Obout.Grid" TagPrefix="obout" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <div id="divLoading" style="height: 110%; width: 100%; display: none; top: 0; left: 0;"
            class="modal">
            <center>
            </center>
        </div>
        <div class="divDetailExpandPopUpOff" id="divPopUp">
            <center>
                <div class="popupClose" onclick="CloseInboxDetails()">
                </div>
                <div class="divDetailExpand" id="divInboxDetails">
                </div>
            </center>
        </div>
       <%-- <input type="button" id="btnSelectAll" value="Select All" onclick="btnSelectAll_Click(this)" />
        <input type="button" id="btnArchive" value="Archive" style="display: none;" />--%>
        <asp:HiddenField runat="server" ID="hdnIDs" />
        <obout:Grid ID="GVInbox" runat="server" AutoGenerateColumns="false" CallbackMode="true"
            Serialize="false" AllowAddingRecords="false" AllowFiltering="false" AllowGrouping="false"
            Width="100%">
            <Columns>
                <obout:Column DataField="inboxID" Visible="false" Width="0%">
                </obout:Column>
                <obout:Column DataField="SiteName" HeaderText="Site" HeaderAlign="left" Align="left"
                    Width="10%">
                </obout:Column>
                <obout:Column DataField="ObjectName" HeaderText="Title" HeaderAlign="left" Align="left"
                    Width="10%">
                </obout:Column>
                <obout:Column DataField="Subject" HeaderText="Subject" HeaderAlign="left" Align="left"
                    Width="60%">
                </obout:Column>
                <obout:Column DataField="Details" Visible="false" Width="0%">
                </obout:Column>
                <obout:Column DataField="Status" HeaderText="Status" HeaderAlign="left" Align="left"
                    Width="10%">
                </obout:Column>
                <obout:Column DataField="Date" HeaderAlign="right" Align="right" Width="10%">
                </obout:Column>
                <obout:Column DataField="IsRead" Visible="false" Width="0%">
                </obout:Column>
            </Columns>
            <Templates>
                <obout:GridTemplate runat="server" ID="GVTemplateDetails">
                    <Template>
                        <span onclick="showDetails('<%# (Container.DataItem["Details"].ToString()) %>', this)"
                            class='<%# (Container.DataItem["IsRead"].ToString()) %>'>'<%# (Container.Value.ToString()) %>'
                        </span>
                    </Template>
                </obout:GridTemplate>
            </Templates>
        </obout:Grid>
    </div>
    <script type="text/javascript">
        function btnSelectAll_Click(invoker) {
            if (invoker.value == "Select All") {

                SelectAll();
            }
            else if (invoker.value == "DeSelect All") {
                btnArchive.style.display = "none";
                invoker.value == "Select All"
                DeSelectAll();
            }
        }

        function SelectAll() {
            var hdnIDs = document.getElementById("<%= hdnIDs.ClientID %>");
            hdnIDs.value = "";
            for (var i = 0; i < GVInbox.Rows.length; i++) {
                GVInbox.selectRecord(i);
                if (hdnIDs.value != "") {
                    hdnIDs.value = hdnIDs.value + "," + GVInbox.Rows[i].inboxID;
                }
                else {
                    hdnIDs.value = GVInbox.Rows[i].inboxID;
                }
            }

            if (hdnIDs.value != "") { btnArchive.style.display = ""; invoker.value == "DeSelect All" }

        }

        function DeSelectAll() {
            for (var i = 0; i < GVInbox.Rows.length; i++) {
                GVInbox.deselectRecord(i);
            }

        }

        function showDetails(msg, invoker) {
            invoker.className = "true";
            var innerdiv = "<div style='width:90%;'>" + msg + "</div>"
            divInboxDetails.appendChild(innerdiv);
        }
        function CloseInboxDetails() {
            LoadingOff();
            divPopUp.className = "divDetailExpandPopUpOff";
        }
    </script>
    <style type="text/css">
        .false
        {
            font-weight: bold;
        }
        
        .true
        {
            font-weight: normal;
        }
        
        .popupClose
        {
            background: url(../App_Themes/Blue/img/icon_close.png) no-repeat;
            height: 32px;
            width: 32px;
            float: right;
            margin-top: -30px;
            margin-right: -25px;
        }
        .popupClose:hover
        {
            cursor: pointer;
        }
        .divDetailExpandPopUpOff
        {
            display: none;
        }
        .divDetailExpandPopUpOn
        {
            border: solid 3px gray;
            width: 600px;
            height: 100%;
            padding: 10px;
            background-color: #FFFFFF;
            margin-top: 20px;
            top: 1%;
            left: 3%;
            position: absolute;
            z-index: 99999;
        }
    </style>
    </form>
</body>
</html>

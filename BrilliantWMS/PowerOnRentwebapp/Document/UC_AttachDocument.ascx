<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UC_AttachDocument.ascx.cs"
    Inherits="BrilliantWMS.Document.UC_AttachDocument" %>
<%@ Register Assembly="obout_Flyout2_NET" Namespace="OboutInc.Flyout2" TagPrefix="obout" %>
<%@ Register Assembly="Obout.Ajax.UI" Namespace="Obout.Ajax.UI.FileUpload" TagPrefix="obout" %>
<%@ Register Assembly="obout_Interface" Namespace="Obout.Interface" TagPrefix="obout" %>
<%@ Register Assembly="obout_Grid_NET" Namespace="Obout.Grid" TagPrefix="obout" %>
<center>
    <div runat="server" id="DivDoc">
    </div>
    <table class="gridFrame" width="100%">
        <tr>
            <td style="text-align: left;">
                <a class="headerText"><asp:Label ID="lbldoclisst" runat="server">Document List</asp:Label> </a>
            </td>
            <td>
                <input type="button" id="btnDocumentAdd" runat="server" value="Add New" onclick="openDocumentWindow('0')"
                    style="float: right;" />
            </td>
        </tr>
        <tr>
            <td colspan="2" align="center">
                <obout:Grid ID="GvDocument" runat="server" AutoGenerateColumns="false" AllowFiltering="true"
                    AllowGrouping="true" AllowColumnResizing="true" AllowAddingRecords="false" AllowColumnReordering="true"
                    AllowPageSizeSelection="true" AllowPaging="true" AllowRecordSelection="false"
                    AllowSorting="true" Width="100%" OnRebind="GvDocument_OnRebind">
                    <Columns>
                        <obout:Column DataField="ID" HeaderText="ID" Visible="false" Width="0%">
                        </obout:Column>
                        <obout:Column ID="Delete" DataField="DeleteAccess" HeaderText="Delete" Width="0%" 
                            Align="center" HeaderAlign="center">
                            <TemplateSettings TemplateId="GvTempDelete" />
                        </obout:Column>
                        <obout:Column DataField="Sequence" Width="0%" HeaderAlign="center" Align="center"
                            Visible="false">
                        </obout:Column>
                        <obout:Column DataField="DocumentName" HeaderText="Document Title" Width="20%" Wrap="true" HeaderAlign="center" Align="center">
                        </obout:Column>
                        <obout:Column DataField="Description" HeaderText="Description" Width="25%" Wrap="true">
                        </obout:Column>
                        <obout:Column DataField="Keywords" HeaderText="Key Words" Width="25%" Wrap="true">
                        </obout:Column>
                        <obout:Column DataField="DocumentType" HeaderText="Document Type" Width="10%" Align="center"
                            HeaderAlign="center">
                        </obout:Column>
                        <obout:Column DataField="FileType" HeaderText="File Type" Width="10%" Align="center"
                            HeaderAlign="center">
                        </obout:Column>
                        <obout:Column ID="Download" DataField="DowloadAccess" HeaderText="Download" Width="10%"
                            HeaderAlign="center" Align="center">
                            <TemplateSettings TemplateId="GvTempDownload" />
                        </obout:Column>
                        <obout:Column ID="DocumentDownloadPath" DataField="DocumentDownloadPath" HeaderText="Delete"
                            Visible="false" Width="0%">
                        </obout:Column>
                    </Columns>
                    <Templates>
                        <obout:GridTemplate ID="GvTempDownload">
                            <Template>
                                <a href='<%# (Container.Value == "true" ? (Container.DataItem["DocumentDownloadPath"]) :'#') %>'
                                    target="_blank">
                                    <img src='<%# (Container.Value == "true" ? "../CommonControls/HomeSetupImg/download.png" : "../CommonControls/HomeSetupImg/accessdDenied.jpg") %>' />
                                </a>
                            </Template>
                        </obout:GridTemplate>
                    </Templates>
                    <Templates>
                        <obout:GridTemplate ID="GvTempDelete">
                            <Template>
                                <img src="../App_Themes/Blue/img/Delete16.png" onclick="deleteDocument('<%# Container.DataItem["Sequence"].ToString() %>')"
                                    style="cursor: pointer;" />
                            </Template>
                        </obout:GridTemplate>
                    </Templates>
                </obout:Grid>
            </td>
        </tr>
    </table>
</center>
<asp:HiddenField ID="hndDocumentTargetObjectName" runat="server" ClientIDMode="Static" />
<script type="text/javascript">

    function openDocumentWindow(sequence) {
        var DocumentTargetObjectName = document.getElementById("hndDocumentTargetObjectName").value;
        window.open('../Document/Document.aspx?Sequence=' + sequence + '&TargetObjectName=' + DocumentTargetObjectName + '', null, 'height=250, width=780,status=0, resizable=0, scrollbars=0, toolbar=0,location=center,menubar=0');
    }

    function deleteDocument(Sequence) {
        var ans = confirm("Are you sure ?");
        if (ans) {

            PageMethods.PMDeleteDocument(Sequence, OnSuccessPMDeleteDocument, OnFailedPMDeleteDocument);
        }

    }

    function OnSuccessPMDeleteDocument(result) {
        if (result.toLowerCase() == "true") {
            GvDocument.refresh();
            alert("Selected document deleted successfully");
        }
    }

    function OnFailedPMDeleteDocument() {alert("Some error occurred");}
        
</script>

<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCToolbarHTML.ascx.cs"
    Inherits="BrilliantWMS.CommonControls.UCToolbarHTML" %>
<%@ Register Assembly="obout_Flyout2_NET" Namespace="OboutInc.Flyout2" TagPrefix="cc2" %>
<div id="dvbtn">
    <%--CheckBtn(this.id,'tabletest');--%>
    <table style="margin-top: 0px; float: right;" id="tabletoolbar">
        <tr>
            <td class="tdWidth">
                <input type="button" value="Add New" runat="server" id="btnAddNew" onclick="CheckBtn(this.id,'tabletest');"
                    style="width: 72px; height: 24px;" class="FixWidth" clientidmode="Static" />
            </td>
            <td class="tdWidth" id="tdEdit">
                <input type="button" value="Edit" runat="server" id="btnEdit" onclick="CheckBtn(this.id,'tabletest');"
                    style="width: 72px; height: 24px;" class="FixWidth" clientidmode="Static" />
            </td>
            <td class="tdWidth">
                <input type="button" value="Save" runat="server" id="btnSave" onclick="SaveTrans('true');"
                    style="width: 72px; height: 24px;" class="FixWidth" clientidmode="Static" validationgroup="Save" />
            </td>
            <td class="tdWidth">
                <input type="button" value="Clear" runat="server" id="btnClear" onclick="CheckBtn(this.id,'tabletest');"
                    style="width: 72px; height: 24px;" class="FixWidth" clientidmode="Static" causesvalidation="false" />
            </td>
            <td class="tdWidth">
                <input type="button" value="Export" runat="server" id="btnExport" onclick="CheckBtn(this.id,'tabletest');"
                    style="width: 72px; height: 24px;" class="FixWidth" clientidmode="Static" causesvalidation="false" />
            </td>
            <td class="tdWidth">
                <input type="button" value="Import" runat="server" id="btnImport" onclick="CheckBtn(this.id,'tabletest');"
                    style="width: 72px; height: 24px;" class="FixWidth" clientidmode="Static" causesvalidation="false" />
            </td>
            <td class="tdWidth">
                <input type="button" value="Mail" runat="server" id="btnMail" onclick="CheckBtn(this.id,'tabletest');"
                    style="width: 72px; height: 24px;" class="FixWidth" clientidmode="Static" causesvalidation="false" />
            </td>
            <td class="tdWidth">
                <input type="button" value="Print" runat="server" id="btnPrint" onclick="GetReferenceIDs_ForPrint();CheckBtn(this,'tabletest');"
                    style="width: 72px; height: 24px;" class="FixWidth" clientidmode="Static" causesvalidation="false" />
            </td>
            <td style="width: 74px;" runat="server" id="tdConvertTo">
                <input type="button" value="ConvertTo" runat="server" id="btnConvertTo" onclick="fillConvertTo();"
                    style="width: 72px; height: 24px;" class="FixWidth" clientidmode="Static" causesvalidation="false" />
            </td>
            <td>
                <asp:ImageButton ID="btnHelp1" runat="server" ToolTip="Help" CausesValidation="false"
                    ImageUrl="../App_Themes/Blue/img/help24.png" CssClass="help" />
            </td>
        </tr>
    </table>
</div>
<asp:HiddenField ID="hdnUCToolbarCurrentObject" ClientIDMode="Static" runat="server">
</asp:HiddenField>
<asp:HiddenField ID="hdnUCToolbarReferenceID" runat="server"></asp:HiddenField>
<cc2:Flyout ID="FlybtnConvertTo" runat="server" AttachTo="btnConvertTo" OpenEvent="NONE"
    CloseEvent="NONE" IsModal="true" PageColor="Black" PageOpacity="60" zIndex="999"
    Position="BOTTOM_LEFT">
    <table class="gridFrame">
        <tr>
            <td>
                <cc2:DragPanel ID="DragPanel1" runat="server">
                    <table style="width: 300px">
                        <tr>
                            <td style="text-align: left;">
                                <a id="headerText">Convert To</a>
                            </td>
                        </tr>
                    </table>
                </cc2:DragPanel>
            </td>
        </tr>
        <tr>
            <td>
                <table class="tableForm" style="background-color: White; width: 100%">
                    <tr>
                        <td colspan="2" style="text-align: center;">
                            <asp:Label ID="lblmgs" Font-Italic="true" Font-Size="Medium" Font-Bold="true" runat="server"
                                ForeColor="Maroon" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <req>Convert To :</req>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlConvertTo" runat="server" Width="200px">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <input type="button" value="Convert" id="btnConvertOk" onclick="delayer();" style="width: 65px;" />
                            <input type="button" value="Close" id="btnConToClose" onclick="<%=FlybtnConvertTo.getClientID()%>.Close();"
                                style="width: 65px;" />
                        </td>
                    </tr>
                    <asp:HiddenField ID="hdnObjectName" runat="server" />
                    <asp:HiddenField ID="hdnRefID" runat="server" />
                </table>
            </td>
        </tr>
    </table>
</cc2:Flyout>
<script type="text/javascript">
    function fillConvertTo() {
        var hdnUCToolbarCurrentObject = document.getElementById("<%= hdnUCToolbarCurrentObject.ClientID %>");
        var ddlConvertTo = document.getElementById("<%= ddlConvertTo.ClientID %>");
        if (hdnUCToolbarCurrentObject.value == "New") {
        //dhtmlx.alert({ type: '', title: 'Convert To', text: 'Convert to not allow when you insert a new record' });
         alert("Convert to not allow when you insert a new record");   
        }
        else if (hdnUCToolbarCurrentObject.value == "") {
            //dhtmlx.alert({ type: '', title: 'Convert To', text: 'Invalid Operation' });
            alert("Invalid Operation");
        }
        else if (hdnUCToolbarCurrentObject.value == "LeadClose") {
           // dhtmlx.alert({ type: '', title: 'Convert To', text: 'Convert to not allow when status closed' });
           alert("Convert to not allow when status closed");
        }
        else if (hdnUCToolbarCurrentObject.value != "New" && hdnUCToolbarCurrentObject.value != "") {
            if (hdnUCToolbarCurrentObject.value == "Lead") {
                ClearDropDown(ddlConvertTo);
                var opt = new Option("-Select-", "0");
                ddlConvertTo.options.add(opt);

                var opportunity = new Option("Opportunity", "../Opportunity/OpportunityAddEdit.aspx?LeadID=");
                ddlConvertTo.options.add(opportunity);

                var Quotation = new Option("Quotation", "../Quotation/QuotationAddEdit.aspx?LeadID=");
                ddlConvertTo.options.add(Quotation);

                var SalesOrder = new Option("Sales Order", "../SalesOrder/SalesOrderAddEdit.aspx?LeadID=");
                ddlConvertTo.options.add(SalesOrder);

                var Invoice = new Option("Invoice", "../Invoice/InvoiceAddEdit.aspx?LeadID=");
                ddlConvertTo.options.add(Invoice);

                <%=FlybtnConvertTo.getClientID()%>.Open();

            }
            else if (hdnUCToolbarCurrentObject.value == "Opportunity") {
                ClearDropDown(ddlConvertTo);
                var opt = new Option("-Select-", "0");
                ddlConvertTo.options.add(opt);

                var Quotation = new Option("Quotation", "../Quotation/QuotationAddEdit.aspx?OpportunityID=");
                ddlConvertTo.options.add(Quotation);

                var SalesOrder = new Option("Sales Order", "../SalesOrder/SalesOrderAddEdit.aspx?OpportunityID=");
                ddlConvertTo.options.add(SalesOrder);

                var Invoice = new Option("Invoice", "../Invoice/InvoiceAddEdit.aspx?OpportunityID=");
                ddlConvertTo.options.add(Invoice);

                <%=FlybtnConvertTo.getClientID()%>.Open();
            }
            else if (hdnUCToolbarCurrentObject.value == "Quotation") {
                ClearDropDown(ddlConvertTo);
                var opt = new Option("-Select-", "0");
                ddlConvertTo.options.add(opt);
                               
                var SalesOrder = new Option("Sales Order", "../SalesOrder/SalesOrderAddEdit.aspx?QuotationID=");
                ddlConvertTo.options.add(SalesOrder);

                var Invoice = new Option("Invoice", "../Invoice/InvoiceAddEdit.aspx?QuotationID=");
                ddlConvertTo.options.add(Invoice);

                <%=FlybtnConvertTo.getClientID()%>.Open();
            }
            else if (hdnUCToolbarCurrentObject.value == "SalesOrder") {
                ClearDropDown(ddlConvertTo);
                var opt = new Option("-Select-", "0");
                ddlConvertTo.options.add(opt);
                
                var Invoice = new Option("Invoice", "../Invoice/InvoiceAddEdit.aspx?SalesOrderID=");
                ddlConvertTo.options.add(Invoice);

                <%=FlybtnConvertTo.getClientID()%>.Open();
            }
            else if (hdnUCToolbarCurrentObject.value == "Invoice") { }
        }
    }

    function ClearDropDown(ddl)
    {
        var len = ddl.options.length;
            for (i=0; i<len; i++) {
                ddl.remove(0); //It is 0 (zero) intentionally
            }
    }

    function delayer(){
        <%=FlybtnConvertTo.getClientID()%>.Close();
        
        var ddlConvertTo = document.getElementById("<%= ddlConvertTo.ClientID %>");
        var hdnUCToolbarReferenceID  = document.getElementById("<%= hdnUCToolbarReferenceID.ClientID %>"); 
        if(hdnUCToolbarReferenceID.value != "") window.location = ddlConvertTo.options[ddlConvertTo.selectedIndex].value + hdnUCToolbarReferenceID.value;
    }

</script>

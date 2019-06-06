function getCustomerInfo(AccountID) {
    var txtAccountName = document.getElementById("txtAccountName");
    txtAccountName.value = AccountID;
    setContactPersonTargetObjectName("Lead");
    setAddressTargetObjectName("Lead");
    PageMethods.webGetCustomerHeadDetailByCustomerID(AccountID, onSuccessGetAccountDetails, onFailedGetAccountDetails);
}

function onSuccessGetAccountDetails(result) {
    var txtAccountName = document.getElementById("txtAccountName");
    txtAccountName.value = result.Name;

    var txtWebSite = document.getElementById("txtWebSite");
    txtWebSite.value = "";
    if (result.WebSite != null) txtWebSite.value = result.WebSite;

    document.getElementById("ddlLeadSector").value = result.SectorID;

    document.getElementById("ddlCompanyType").value = result.CustomerTypeID;

    GvAddressInfo.refresh();
    GVContactPerson.refresh();
}

function onFailedGetAccountDetails() { }


function getQueryStringByName(name) {
    name = name.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
    var regexS = "[\\?&]" + name + "=([^&#]*)";
    var regex = new RegExp(regexS);
    var results = regex.exec(window.location.search);
    if (results == null)
        return "";
    else
        return decodeURIComponent(results[1].replace(/\+/g, " "));
}

/*Save Transction*/
/*Lead, Opportunity, Quoatation, SalesOrder, Invoice*/
function SaveTrans(IsSubmit) {
    //tdTransDt, hdnCustomerID, txtTitle,ddlCampaign, lblTansNo, ddlLeadSource, txtAccountName, txtWebSite, ddlTransStatus, ddlSector, ddlCompanyType, txtConfidenceFactor, tdEOD, txtExpOrderAmount, txtRemark
    if (typeof (Page_ClientValidate) == 'function') {
        Page_ClientValidate();
    }
    if (Page_IsValid) {
        LoadingOn();
        var trans = new Object();

        /*Account Info*/
        if (document.getElementById("hdnCustomerID").value == "") document.getElementById("hdnCustomerID").value = "0";
        trans.CustomerHeadID = parseInt(document.getElementById("hdnCustomerID").value);
        trans.Name = document.getElementById("txtAccountName").value;
        trans.WebSite = document.getElementById("txtWebSite").value;
        trans.SectorID = document.getElementById("ddlSector").value;
        trans.CustomerTypeID = document.getElementById("ddlCompanyType").value;

        if (document.getElementById("lblTansNo").innerHTML == "") document.getElementById("lblTansNo").innerHTML = "0";
        trans.ID = parseInt(document.getElementById("lblTansNo").innerHTML);

        if (getQueryStringByName("LeadID") != "") { trans.ObjectName = "Lead"; trans.ReferenceID = getQueryStringByName("LeadID"); }
        else if (getQueryStringByName("OpportunityID") != "") { trans.ObjectName = "Opportunity"; trans.ReferenceID = getQueryStringByName("OpportunityID"); }
        else if (getQueryStringByName("QuotationID") != "") { trans.ObjectName = "Quotation"; trans.ReferenceID = getQueryStringByName("QuotationID"); }
        else if (getQueryStringByName("SalesOrderID") != "") { trans.ObjectName = "SalesOrder"; trans.ReferenceID = getQueryStringByName("SalesOrderID"); }
        else if (getQueryStringByName("InvoiceID") != "") { trans.ObjectName = "Invoice"; trans.ReferenceID = getQueryStringByName("InvoiceID"); }

        var tdTransDt = document.getElementById("tdTransDt");
        var tdTransDtInputs = tdTransDt.getElementsByTagName('input');
        for (var t = 0; t < tdTransDtInputs.length; t++) {
            if (tdTransDtInputs[t].type == "text") trans.LeadDate = tdTransDtInputs[t].value;
        }

        trans.LeadSourceID = document.getElementById("ddlLeadSource").value;
        trans.LeadStatus = document.getElementById("ddlTransStatus").value;
        trans.CampaignID = null; /*ddlCampaign*/

        trans.ConfidenceFactor = null;
        if (document.getElementById("txtConfidenceFactor") != null) {
            if (document.getElementById("txtConfidenceFactor").value == "") document.getElementById("txtConfidenceFactor").value = "0";
            trans.ConfidenceFactor = document.getElementById("txtConfidenceFactor").value;
        }

        trans.ExpOrderDate = null;
        if (document.getElementById("tdEOD") != null) {
            var tdEOD = document.getElementById("tdEOD");
            var tdEODInputs = tdEOD.getElementsByTagName('input');
            for (var t = 0; t < tdEODInputs.length; t++) {
                if (tdEODInputs[t].type == "text") { if (tdEODInputs[t].value != "") trans.ExpOrderDate = tdEODInputs[t].value; }
            }
        }
        trans.ExpOrderAmount = null;
        if (document.getElementById("txtExpOrderAmount") != null) {
            if (document.getElementById("txtExpOrderAmount").value == "") document.getElementById("txtExpOrderAmount").value = "0";
            trans.ExpOrderAmount = document.getElementById("txtExpOrderAmount").value;
        }

        trans.SubTotal = parseFloat(document.getElementById("hdnCartSubTotal").value);
        trans.ProductLevelTotalDiscount = "0";
        trans.DiscountOnSubTotal = parseFloat(document.getElementById("txtDiscountOnSubTotal").value);
        trans.DiscountOnSubTotalPercent = document.getElementById("chkboxDiscountOnSubTotal").checked;
        trans.TotalDiscount = parseFloat(document.getElementById("hdnCartDiscountOnSubTotal").value);
        trans.TotalAfterDiscount = parseFloat(document.getElementById("hdnCartSubTotal2").value);
        trans.ProductLevelTotalTax = "0";
        trans.TaxOnTotalAfterDiscount = "0";
        trans.TotalTax = parseFloat(document.getElementById("hdnCartTaxOnSubTotal").value);
        trans.ShippingCharges = parseFloat(document.getElementById("txtShippingCharges").value);
        trans.OtherChargesDescription = document.getElementById("txtAdditionalChargeDescription").value;
        trans.OtherCharges = parseFloat(document.getElementById("txtAdditionalCharges").value);
        trans.TotalAmount = parseFloat(document.getElementById("hdnCartGrandTotal").value);

        trans.Remark = document.getElementById("txtRemark").value;
        trans.Active = "Y";

        trans.BillingAddressID = document.getElementById("hdnbilling").value;
        trans.ShippingAddressID = document.getElementById("hdnshipping").value;
        trans.ConperID = document.getElementById("hnddefaultchk").value;

        trans.Title = document.getElementById("txtTitle").value;
        trans.IsCompleted = IsSubmit.toString();

        /*Additional Fields*/
        if (document.getElementById("txtFreeTextTransNo") != null) trans.FreeTextTransNo = document.getElementById("txtFreeTextTransNo").value;
        if (document.getElementById("txtParentReferenceID") != null) trans.ParentReferenceID = parseInt(document.getElementById("txtParentReferenceID").value);
        if (document.getElementById("txtQuotationValidityDays") != null) trans.QuotationValidityDays = parseInt(document.getElementById("txtQuotationValidityDays").value);

        if (document.getElementById("txtCustomerPONo") != null) trans.CustomerPONo = document.getElementById("txtCustomerPONo").value;

        if (document.getElementById("tdCustomerPoDate") != null) {
            var tdCustomerPoDate = document.getElementById("tdCustomerPoDate");
            var allinput = tdCustomerPoDate.getElementsByTagName("input");
            for (var i = 0; i < allinput.length; i++) {
                if (allinput[i].type == "text") {
                    if (allinput[i].value != "") { trans.CustomerPODate = allinput[i].value; }
                }
            }
        }


        if (document.getElementById("txtDispatchThrough") != null) trans.DispatchThrough = document.getElementById("txtDispatchThrough").value;

        if (document.getElementById("tdExpDispachDt") != null) {
            var tdExpDispachDt = document.getElementById("tdExpDispachDt");
            var allinput1 = tdExpDispachDt.getElementsByTagName("input");
            for (var i = 0; i < allinput1.length; i++) {
                if (allinput1[i].type == "text") {
                    if (allinput1[i].value != "") { trans.ExpDispatchDate = allinput1[i].value; }
                }
            }
        }
        if (document.getElementById("ddlInvoiceType") != null) trans.InvoiceType = document.getElementById("ddlInvoiceType").value;

        PageMethods.PMSaveTrans(trans, onSuccessSaveTrans, onFailSaveTrans);
    }
}
function onSuccessSaveTrans(result) {
    if (parseInt(result[0]) > 0) {
        LoadingOff();
        document.getElementById("lblTansNo").innerHTML = result[0];
        alert(result[1]);
    }
}
function onFailSaveTrans() { alert("Some error occurred"); }

function ClearUserControlTempData() {
    /*Cart controls clear*/
    document.getElementById("hdnItemCount").value = "0";
    document.getElementById("lblSubTotal").innerHTML = "0.00";
    document.getElementById("hdnCartSubTotal").value = "0.00";

    document.getElementById("txtDiscountOnSubTotal").value = "0.00";
    document.getElementById("chkboxDiscountOnSubTotal").checked = false;

    document.getElementById("hdnCartDiscountOnSubTotal").value = "0.00";
    document.getElementById("lblDiscountOnSubTotal").innerHTML = "0.00";

    document.getElementById("hdnCartSubTotal2").value = "0.00";
    document.getElementById("lblSubTotal2").innerHTML = "0.00";

    document.getElementById("hdnCartTaxOnSubTotal").value = "0.00";
    document.getElementById("lblTaxOnSubTotal").innerHTML = "0.00";

    document.getElementById("txtShippingCharges").value = "0.00";
    document.getElementById("txtAdditionalChargeDescription").value = "";
    document.getElementById("txtAdditionalCharges").value = "0.00";

    document.getElementById("hdnCartGrandTotal").value = "0.00";
    document.getElementById("lblGrandTotal").innerHTML = "0.00";

    document.getElementById("hdnIsTaxAmountChange").value = "";

    PageMethods.ResetUserControl(OnSuccessClear, OnFailClear);

}

function OnSuccessClear() {
    if (Grid1 != null) Grid1.refresh();
    if (GVContactPerson != null) GVContactPerson.refresh();
    if (GvAddressInfo != null) GvAddressInfo.refresh();
    if (GvDocument != null) GvDocument.refresh();

}
function OnFailClear() { }




/*End*/



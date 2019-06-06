function GetData(obj) {

    /*Account Info*/
    document.getElementById("hdnCustomerID").value =  parseInt(obj.CustomerHeadID);
    document.getElementById("txtAccountName").value = obj.Name.toString();
    document.getElementById("txtWebSite").value = obj.WebSite.toString();
    document.getElementById("ddlSector").value = obj.SectorID;
    document.getElementById("ddlCompanyType").value = obj.CustomerTypeID;

    document.getElementById("lblTansNo").innerHTML = parseInt(obj.ID);

    var tdTransDt = document.getElementById("tdTransDt");
    var tdTransDtInputs = tdTransDt.getElementsByTagName('input');
    for (var t = 0; t < tdTransDtInputs.length; t++) {
        if (tdTransDtInputs[t].type == "text") tdTransDtInputs[t].value=obj.LeadDate;
    }
    


    document.getElementById("ddlLeadSource").value = obj.LeadSourceID;
    document.getElementById("ddlTransStatus").value = obj.LeadStatus;
    document.getElementById("ddlCampaign").value = null;

    document.getElementById("txtConfidenceFactor").value = obj.ConfidenceFactor;


    var tdEOD = document.getElementById("tdEOD");
    var tdEODInputs = tdEOD.getElementsByTagName('input');
    for (var t = 0; t < tdEODInputs.length; t++) {
        if (tdEODInputs[t].type == "text") { if (tdEODInputs[t].value != "") tdEODInputs[t].value = obj.ExpOrderDate; }
    }
    


    document.getElementById("txtExpOrderAmount").value = obj.ExpOrderAmount;


    document.getElementById("hdnCartSubTotal").value = parseFloat(obj.SubTotal);
    document.getElementById("txtDiscountOnSubTotal").value =  parseFloat(obj.DiscountOnSubTotal);
    document.getElementById("chkboxDiscountOnSubTotal").checked = obj.DiscountOnSubTotalPercent;
    document.getElementById("hdnCartDiscountOnSubTotal").value = parseFloat(obj.TotalDiscount);
    document.getElementById("hdnCartSubTotal2").value = parseFloat(obj.TotalAfterDiscount);

    document.getElementById("hdnCartTaxOnSubTotal").value = parseFloat(obj.TotalTax);
    document.getElementById("txtShippingCharges").value = parseFloat(obj.ShippingCharges);
    document.getElementById("txtAdditionalChargeDescription").value = obj.OtherChargesDescription;
    document.getElementById("txtAdditionalCharges").value =parseFloat(obj.OtherCharges);
    document.getElementById("hdnCartGrandTotal").value = parseFloat(obj.TotalAmount);

    document.getElementById("txtRemark").value = obj.Remark;


    document.getElementById("hdnbilling").value = obj.BillingAddressID;
    document.getElementById("hdnshipping").value = obj.ShippingAddressID;
    document.getElementById("hnddefaultchk").value = obj.ConperID;

    document.getElementById("txtTitle").value = obj.Title;

}

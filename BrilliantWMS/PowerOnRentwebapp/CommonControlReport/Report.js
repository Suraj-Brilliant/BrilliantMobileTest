window.onload = function () {
    //oboutGrid.prototype.restorePreviousSelectedRecord = function () {
    //    return;
    //}
    //oboutGrid.prototype.markRecordAsSelectedOld = oboutGrid.prototype.markRecordAsSelected;
    //oboutGrid.prototype.markRecordAsSelected = function (row, param2, param3, param4, param5) {
    //    if (row.className != this.CSSRecordSelected) {
    //        this.markRecordAsSelectedOld(row, param2, param3, param4, param5);
    //    } else {
    //        var index = this.getRecordSelectionIndex(row);
    //        if (index != -1) {
    //            this.markRecordAsUnSelected(row, index);
    //        }
    //    }
    //}

}


var VendorVal;


$(function () {
    $("#dtFrom").datepicker();
    $("#dtTo").datepicker();

});

$(document).ready(function () {

});


$(document).on('change', '#ddlCustomer', function () {
    Val = $(this).val();
    var hdnCustId = document.getElementById("hdnCustId");
    hdnCustId.value = Val;
    GetDropDownData('18', '#ddlVendor', Val, '0');
    GetDropDownData('30', '#ddlWareHouse', Val, '0');
    GetDropDownData('73', '#ddlUser', Val, '0');

});

var prm = Sys.WebForms.PageRequestManager.getInstance();
if (prm != null) {
    prm.add_endRequest(function (sender, e) {
        if (sender._postBackSettings.panelsToUpdate != null) {
            DisplayCurrentTime();
        }
    });
};

function DisplayCurrentTime() {
    var hdnCustId = document.getElementById("hdnCustId");
    var hdnVenderId = document.getElementById("hdnVenderId").value;
    var hdnUserId = document.getElementById("hdnUserId").value;
    var hdnWareHouseId = document.getElementById("hdnWareHouseId").value;

    $("#dtFrom").datepicker();
    $("#dtTo").datepicker();




    if (hdnCustId.value != null && hdnCustId.value != "" && hdnCustId.value != "undefined") {
        GetDropDownData('18', '#ddlVendor', Val, hdnVenderId);
        GetDropDownData('30', '#ddlWareHouse', Val, hdnWareHouseId);
        GetDropDownData('73', '#ddlUser', Val, hdnUserId);
    }

};
$(document).on('change', '#ddlWareHouse', function () {

});

$(document).on('change', '#ddlVendor', function () {


    //if (VendorVal == null || VendorVal == "")
    //    VendorVal = $(this).val();
    //else
    //    VendorVal = VendorVal + "," + $(this).val();
    // alert(VendorVal);


});


function GetDropDownData(CtlID, FilterCrtl, FilterId, selectedID) {
    var hdnReportIdVal = document.getElementById("hdnRptId");
    var params = "{'CtlID':'" + CtlID + "', 'FilterId':'" + FilterId + "', 'ReportId':'" + hdnReportIdVal.value + "'}";


    $(FilterCrtl).find('option').not(':first').remove();
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "WebForm1.aspx/GetFilterData",
        data: params,
        dataType: "json",
        success: function (data) {
            Result = data.d;

            $.each(Result, function (key, value) {

                $(FilterCrtl).append($("<option></option>").val
               (value.Id).html(value.Name));
                if (value.Id == selectedID)
                    $(FilterCrtl).val(selectedID);
            });

        },
        error: function (result) {
            alert("ErrorGetDropDownData");
        }
    });
};

function ShowGrid() {


    var hdnVenderId = document.getElementById("hdnVenderId");
    hdnVenderId.value = $('#ddlVendor').val();

    var hdnUserId = document.getElementById("hdnUserId");
    hdnUserId.value = $('#ddlUser').val();

    var hdnWareHouseId = document.getElementById("hdnWareHouseId");
    hdnWareHouseId.value = $('#ddlWareHouse').val();

    


    var params = "{'CtlID':'1', 'FilterId':'2'}";
    var hdnSelVal = document.getElementById("hdnSelVal");
    hdnSelVal.value = "G" + "," + "0" + "," +

    document.getElementById("ddlCustomer").value + "," + document.getElementById("dtFrom").value + "," +
    document.getElementById("dtTo").value + "," + document.getElementById("ddlVendor").value + "," +
    document.getElementById("ddlUser").value + "," + document.getElementById("ddlWareHouse").value; //where "G" means Grid as objectID



}

function selectedRec() {

    var hdnListVal = document.getElementById("hdnListVal");
    var hdnSelAllVal = document.getElementById("hdnSelAll").value;
    var Checkbox1 = document.getElementById("Checkbox1");
    hdnListVal.value = "";
    if (GvList.Rows.length > 0) {
        if (Checkbox1.checked == true) {
            hdnListVal.value = hdnSelAllVal;
        }
        else {
            if (GvList.SelectedRecords.length > 0) {
                for (var i = 0; i < GvList.SelectedRecords.length; i++) {
                    var record = GvList.SelectedRecords[i];
                    if (hdnListVal.value != "") hdnListVal.value += ',' + record.id;
                    if (hdnListVal.value == "") hdnListVal.value = record.id;
                }
            }
            if (GvList.SelectedRecords.length == 0) {
                alert("Select atleast one list");
            }

        }
        if (hdnListVal.value != null && hdnListVal.value != "" && hdnListVal.value != "undefined") {
            var fromDt = document.getElementById("dtFrom").value;
            var ToDt = document.getElementById("dtTo").value;
            var ReportId = document.getElementById("hdnRptId").value;
            window.open("ReportViwer.aspx?Val=" + hdnListVal.value + "&fromDt=" + fromDt + "&ToDt=" + ToDt + "&RptType=" + "M" + "&RptID=" + ReportId, 'liveMatches', 'directories=no,titlebar=no,toolbar=no,location=no,status=no,menubar=no,scrollbars=yes,resizable=no,width=800,height=800');
            //where "M" means Query for MAIN List report as objectID
        }
    }
    else { alert("No list records to show report"); }
    return false;
}

function selectedRecDtls() {

    var hdnListVal = document.getElementById("hdnListVal");
    hdnListVal.value = "";
    if (GvList.Rows.length > 0) {

        if (GvList.SelectedRecords.length > 1)
        { alert("Select only one list"); }
        if (GvList.SelectedRecords.length > 0) {
            for (var i = 0; i < GvList.SelectedRecords.length; i++) {
                var record = GvList.SelectedRecords[i];
                //if (hdnListVal.value != "") hdnListVal.value += ',' + record.id;
                if (hdnListVal.value == "") hdnListVal.value = record.id;
            }
        }
        if (GvList.SelectedRecords.length == 0) {
            alert("Select atleast one list");
        }

        if (hdnListVal.value != null && hdnListVal.value != "" && hdnListVal.value != "undefined") {
            var fromDt = document.getElementById("dtFrom").value;
            var ToDt = document.getElementById("dtTo").value;
            var ReportId = document.getElementById("hdnRptId").value;
            window.open("ReportViwer.aspx?Val=" + hdnListVal.value + "&fromDt=" + fromDt + "&ToDt=" + ToDt + "&RptType=" + "S" + "&RptID=" + ReportId, 'liveMatches', 'directories=no,titlebar=no,toolbar=no,location=no,status=no,menubar=no,scrollbars=yes,resizable=no,width=500,height=500');
            //where "S" means Query for Sub Detail report as objectID
        }
    }
    else { alert("No list records to show report"); }
    return false;
}

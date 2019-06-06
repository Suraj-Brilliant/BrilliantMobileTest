/*Get Querystring parameter*/
function getParameterByName(name) {
    name = name.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
    var regexS = "[\\?&]" + name + "=([^&#]*)";
    var regex = new RegExp(regexS);
    var results = regex.exec(window.location.search);
    if (results == null)
        return "";
    else
        return decodeURIComponent(results[1].replace(/\+/g, " "));
}
/*End Get Querystring parameter*/

/*Show Loading div*/
function LoadingOn() {
    document.getElementById("divLoading").style.display = "block";
    var imgProcessing = document.getElementById("imgProcessing");
    if (imgProcessing != null) { imgProcessing.style.display = ""; }
}

function LoadingOn(ShowWaitMsg) {
    document.getElementById("divLoading").style.display = "block";
    var imgProcessing = document.getElementById("imgProcessing");
    if (imgProcessing != null) {
        if (ShowWaitMsg == true) { imgProcessing.style.display = ""; }
       if (ShowWaitMsg == false) { imgProcessing.style.display = "none"; }      
    }
}

function LoadingText(val) {
    if (val == true) { document.getElementById("pupUpLoading").style.display = ""; }
    else { document.getElementById("pupUpLoading").style.display = "none"; }
}

function LoadingOff() {
    if (document.getElementById("divLoading") != null) {
        document.getElementById("divLoading").style.display = "none";
    }
    var imgProcessing = document.getElementById("imgProcessing");
    if (imgProcessing != null) { imgProcessing.style.display = "none"; }
}
/*End Show Loading div*/

function RefreshDefaultViewGrid() {
    if (window.opener != null) {
        if (window.opener.GvDefaultView1 != null) { window.opener.GvDefaultView1.refresh(); }
        else if (window.opener.GVActionHistory != null) { window.opener.GVActionHistory.refresh(); }
    }
    else {
        window.document.parentDocument.RefreshGridUpdateTask();
    }
}
/*Max Length Counter*/
function barNotallow(sender, event) {
    var keyPressed = getKeyPressed_e(event);
    if (parseInt(keyPressed) == 220) { event.preventDefault ? event.preventDefault() : event.returnValue = false; }

}

function checkLength(sender, maxLength) {
    if (sender.value.length > parseInt(maxLength)) {
        sender.value = sender.value.substr(0, parseInt(maxLength));
        showAlert("Character limit is exceed", 'error', '#');
    }
}

function TextBox_KeyUp(sender, conunterspan, maxLength) {
    checkLength(sender, maxLength);

    var counterId = conunterspan;
    if (counterId != "") document.getElementById(counterId).innerHTML = parseInt(maxLength) - sender.value.length;
}
/*End Max Length Counter*/
function getKeyPressed_e(e) {
    if (!e) {
        e = window.event;
    }
    var keyCode;

    if (e.keyCode) {
        keyCode = e.keyCode;
    } else if (e.which) {
        keyCode = e.which;
    }

    return keyCode;
}

function AllowBackSpaceDelete(sender, event) {
    var keyPressed = getKeyPressed_e(event);

    if (parseInt(keyPressed) == 46 || parseInt(keyPressed) == 8 || parseInt(keyPressed) == 9) { if (parseInt(keyPressed) != 9) { sender.value = ''; } return true; }
    else { event.preventDefault ? event.preventDefault() : event.returnValue = false; }
}

function AllowDecimal(sender, event) {
    var keyPressed = getKeyPressed_e(event);
    if ((parseInt(keyPressed) == 46 || parseInt(keyPressed) == 8) ||
        (parseInt(keyPressed) >= 48 && parseInt(keyPressed) <= 57) ||
        (parseInt(keyPressed) >= 96 && parseInt(keyPressed) <= 105) ||
        (parseInt(keyPressed) >= 35 && parseInt(keyPressed) <= 40) ||
        parseInt(keyPressed) == 110 ||  /*decimal point*/
        parseInt(keyPressed) == 190 ||  /*period*/
        parseInt(keyPressed) == 9 || /*tab*/
        (event.ctrlKey == true && (event.which == '118' || event.which == '86'))
        ) {
        if (keyPressed == 110 || keyPressed == 190) {
            var patt1 = new RegExp("\\."); var ch = patt1.exec(sender.value);
            if (ch == ".") { event.preventDefault ? event.preventDefault() : event.returnValue = false; }
            else { return true; }
        }
        else if (event.ctrlKey == true && (event.which == '118' || event.which == '86')) {
            sender.value = parseFloat(sender.value);
            return true;
        }
        else { return true; }
    }
    else { event.preventDefault ? event.preventDefault() : event.returnValue = false; }
}

function AllowInt(sender, event) {
    var keyPressed = getKeyPressed_e(event);
    if ((parseInt(keyPressed) == 46 || parseInt(keyPressed) == 8) ||
        (parseInt(keyPressed) >= 48 && parseInt(keyPressed) <= 57) ||
        (parseInt(keyPressed) >= 96 && parseInt(keyPressed) <= 105) ||
        (parseInt(keyPressed) >= 35 && parseInt(keyPressed) <= 40) ||
        parseInt(keyPressed) == 9   /*tab*/ || parseInt(keyPressed) == 17 ||
        (event.ctrlKey == true && (event.which == '118' || event.which == '86'))
       ) {
        if (event.ctrlKey == true && (event.which == '118' || event.which == '86')) { sender.value = parseInt(sender.value); }
        return true;
    }
    else { event.preventDefault ? event.preventDefault() : event.returnValue = false; }

}


function AllowPhoneNo(sender, event) {
    var keyPressed = getKeyPressed_e(event);
    if ((parseInt(keyPressed) == 46 || parseInt(keyPressed) == 8) ||
        (parseInt(keyPressed) >= 48 && parseInt(keyPressed) <= 57) ||
        (parseInt(keyPressed) >= 96 && parseInt(keyPressed) <= 105) ||
        (parseInt(keyPressed) >= 35 && parseInt(keyPressed) <= 40) ||
        parseInt(keyPressed) == 110 ||      /*decimal point*/
        parseInt(keyPressed) == 190 ||      /*period*/
        parseInt(keyPressed) == 9 ||        /*tab*/
        parseInt(keyPressed) == 17 ||       /*Ctl*/
        parseInt(keyPressed) == 86 ||       /*v*/
        parseInt(keyPressed) == 186 ||      /*semi-colon*/
        parseInt(keyPressed) == 188 ||      /*comma*/
        parseInt(keyPressed) == 189 ||      /*substract*/
        parseInt(keyPressed) == 109 ||      /*dash*/
        parseInt(keyPressed) == 219 ||      /*open bracket*/
        parseInt(keyPressed) == 221 ||      /*close braket*/
        parseInt(keyPressed) == 32 ||        /*space*/
        (event.ctrlKey == true && (event.which == '118' || event.which == '86'))
       ) { return true; }
    else { event.preventDefault ? event.preventDefault() : event.returnValue = false; }
}

function AllowPostalCode(sender, event) {
    var keyPressed = getKeyPressed_e(event);
    if ((parseInt(keyPressed) == 46 || parseInt(keyPressed) == 8) ||
        (parseInt(keyPressed) >= 48 && parseInt(keyPressed) <= 57) ||
        (parseInt(keyPressed) >= 96 && parseInt(keyPressed) <= 105) ||
        (parseInt(keyPressed) >= 35 && parseInt(keyPressed) <= 40) ||
        parseInt(keyPressed) == 9 ||        /*tab*/
        parseInt(keyPressed) == 32 ||        /*space*/
        (event.ctrlKey == true && (event.which == '118' || event.which == '86'))
       ) { return true; }
    else { event.preventDefault ? event.preventDefault() : event.returnValue = false; }
}

function NotAllowSpecialChar(sender, event) {
    var keyPressed = getKeyPressed_e(event);
    if ((parseInt(keyPressed) > 64 && parseInt(keyPressed) < 91) || /*small a to z*/
        (parseInt(keyPressed) > 95 && parseInt(keyPressed) < 106) || /*numeric pad 0 to 9*/
        (parseInt(keyPressed) >= 33 && parseInt(keyPressed) <= 40) || /*insert ,delete.....,4 arrows*/
        (parseInt(keyPressed) >= 48 && parseInt(keyPressed) <= 57) || /*1 to 9*/
        parseInt(keyPressed) == 110 ||      /*decimal point*/
        parseInt(keyPressed) == 190 ||      /*period*/
        parseInt(keyPressed) == 9 ||        /*tab*/
        parseInt(keyPressed) == 109 ||      /*dash*/
        parseInt(keyPressed) == 32 ||        /*space*/
        parseInt(keyPressed) == 46 ||        /*delete*/
        parseInt(keyPressed) == 8 ||        /*backspace*/
    // parseInt(keyPressed) == 16 ||        /*shift*/
        parseInt(keyPressed) == 17 ||        /*ctl*/
        parseInt(keyPressed) == 20         /*caps lock*/
         ) { return true; }
    else { event.preventDefault ? event.preventDefault() : event.returnValue = false; }
}
//method used in frms for Not Allow Special Char
function alpha(sender, evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode
    if ((charCode > 64 && charCode < 91) || (charCode > 96 && charCode < 123) || charCode == 8 || charCode == 32 || (charCode >= 48 && charCode <= 57))
    { return true; }
    return false;

}

function validateDate(txtStartDate1, txtEndDate1, invoker, AlertMsg) {
    var txtStartDate = document.getElementById(txtStartDate1);
    var txtEndDate = document.getElementById(txtEndDate1);
    if (txtStartDate.value != '' && txtEndDate.value != '') {
        var startDate = new Date(getMonth(txtStartDate.value) + "/" + getDate(txtStartDate.value) + "/" + getYear(txtStartDate.value));
        var endDate = new Date(getMonth(txtEndDate.value) + "/" + getDate(txtEndDate.value) + "/" + getYear(txtEndDate.value));

        if (startDate > endDate) {
            if (invoker == "Start") { txtStartDate.value = ""; alert(AlertMsg); }
            if (invoker == "End") { txtEndDate.value = ""; alert(AlertMsg); }
        }
    }
}
function getDate(paradate) {
    var array = paradate.split('-');
    return array[0];
}
function getMonth(paradate) {
    var array = paradate.split('-');

    switch (array[1]) {
        case "Jan": return "01"; break;
        case "Feb": return "02"; break;
        case "Mar": return "03"; break;
        case "Apr": return "04"; break;
        case "May": return "05"; break;
        case "Jun": return "06"; break;
        case "Jul": return "07"; break;
        case "Aug": return "08"; break;
        case "Sep": return "09"; break;
        case "Oct": return "10"; break;
        case "Nov": return "11"; break;
        case "Dec": return "12"; break;
    }
    return array[1];
}

function getYear(paradate) {
    var array = paradate.split('-');
    return array[2];
}

function changemode(enabled, tablename1) {
    var tabletest = document.getElementById(tablename1.toString());
    var allinput = tabletest.getElementsByTagName("input");
    var allselect = tabletest.getElementsByTagName("select");
    var alltextarea = tabletest.getElementsByTagName("textarea");
    var allimg = tabletest.getElementsByTagName("img");

    for (var m = 0; m < allimg.length; m++) {
        if (enabled == false) { allimg[m].style.visibility = "visible"; }
        if (enabled == true) { allimg[m].style.visibility = "hidden"; }
    }

    for (var i = 0; i < allinput.length; i++) {
        allinput[i].disabled = enabled;
    }

    for (var s = 0; s < allselect.length; s++) {
        allselect[s].disabled = enabled;
    }

    for (var t = 0; t < alltextarea.length; t++) {
        alltextarea[t].disabled = enabled;
    }
}

function ClearMode(tablename) {
    var tbl = document.getElementById(tablename.toString());
    var allinput = tbl.getElementsByTagName("input");
    var allselect = tbl.getElementsByTagName("select");
    var allspan = tbl.getElementsByTagName("span");
    var allimg = tbl.getElementsByTagName("img");
    var alltextarea = tbl.getElementsByTagName("textarea");

    for (var m = 0; m < allimg.length; m++) {
        allimg[m].style.visibility = "visible";
    }

    var regex = /^\d+(?:\.\d{0,2})$/;

    for (var i = 0; i < alltextarea.length; i++) {
        if (alltextarea[i].type == "textarea") {
            alltextarea[i].value = "";
            alltextarea[i].disabled = false;
        }
    }

    for (var i = 0; i < allinput.length; i++) {
        if (allinput[i].type == "text" || allinput[i].type == "hidden") {
            allinput[i].value = "";
            allinput[i].disabled = false;
        }
    }

    for (var i = 0; i < allspan.length; i++) {
        if (regex.test(allspan[i].innerHTML)) {
            allspan[i].innerHTML = "0.00";
        }
        if (allspan[i].accessKey == '1') {
            allspan[i].innerHTML = "";
        }
        else if (allspan[i].accessKey == '2') {
            var childspan = allspan[i];
            var parentspan = allspan[i - 1];
            if (parentspan != null) {
                childspan.innerHTML = parentspan.lastChild.wholeText.replace('/', '');
            }
        }
    }

    for (var s = 0; s < allselect.length; s++) {
        if (allselect[s].options.length > 0) {
            allselect[s].selectedIndex = 0;
            allselect[s].disabled = false;
        }

        if (allselect[s].accessKey == '0') { allselect[s].options.length = 0; }
    }

}

//Button On-Off
function CheckBtn(invoker, tablename) {
    PageMethods.PMCheckBtn(invoker, tablename, CheckBtn_onSuccess, CheckBtn_onFail);

}
function CheckBtn_onSuccess(result) {

    var btnAddNew = document.getElementById("btnAddNew");
    var btnEdit = document.getElementById("btnEdit");
    var btnSave = document.getElementById("btnSave");
    var btnExport = document.getElementById("btnExport");
    var btnImport = document.getElementById("btnImport");
    var btmMail = document.getElementById("btnMail");
    var btnPrint = document.getElementById("btnPrint");
    var btnClear = document.getElementById("btnClear");
    changemode(true, result.DivName);
    if (result.ActiveButton == "btnAddNew") {
        changemode(false, result.DivName);
        ClearMode(result.DivName);

    }
    if (result.ActiveButton == "btnEdit") {
        changemode(false, result.DivName);

    }
    if (result.ActiveButton == "btnSave") {
        alert("Save");

        LeadDetails.SaveLeadDetails(onSuccess, onFailure);
        // PageMethods.PMSaveButtonClick(ButtonSave_onSuccess, ButtonSave_onFail);
    }
    if (result.ActiveButton == "btnClear") {

        changemode(true, result.DivName);
        PageMethods.ResetUserControl(ButtonClear_onSuccess, ButtonClear_onFail);
    }

    btnAddNew.className = "FixWidth";
    btnEdit.className = "FixWidth";
    btnSave.className = "FixWidth";
    btnExport.className = "FixWidth";
    btnImport.className = "FixWidth";
    btmMail.className = "FixWidth";
    btnPrint.className = "FixWidth";
    btnClear.className = "FixWidth";
    if (btnAddNew != null) { btnAddNew.disabled = !result.BtnAdd; }
    if (btnEdit != null) { btnEdit.disabled = !result.BtnEdit; }
    if (btnSave != null) { btnSave.disabled = !result.BtnSave; }
    if (btnExport != null) { btnExport.disabled = !result.BtnExport; }
    if (btnImport != null) { btnImport.disabled = !result.BtnImport; }
    if (btmMail != null) { btmMail.disabled = !result.BtnMail; }
    if (btnPrint != null) { btnPrint.disabled = !result.BtnPrint; }
    if (btnClear != null) { btnClear.disabled = !result.BtnClear; }

    if (btnAddNew.disabled == true) { btnAddNew.className = "Off FixWidth"; }
    if (btnEdit.disabled == true) { btnEdit.className = "Off FixWidth"; }
    if (btnSave.disabled == true) { btnSave.className = "Off FixWidth"; }
    if (btnExport.disabled == true) { btnExport.className = "Off FixWidth"; }
    if (btnImport.disabled == true) { btnImport.className = "Off FixWidth"; }
    if (btmMail.disabled == true) { btmMail.className = "Off FixWidth"; }
    if (btnPrint.disabled == true) { btnPrint.className = "Off FixWidth"; }
    if (btnClear.disabled == true) { btnClear.className = "Off FixWidth"; }
    if (result.ObjectCode == "Lead" || result.ObjectCode == "Opportunity" || result.ObjectCode == "Quotation" || result.ObjectCode == "SalesOrder") {

    }


}

function CheckBtn_onFail() { }


function ButtonClear_onSuccess() {
    alert("OK");
}

function ButtonClear_onFail() {
    alert("Oppsss");
}

// End button On-Off

/*File uploader validations 
1. Only image file uploading
2. validate the size of upload file 
*/
function fileUploaderValidationImgOnly(fileUploader, maxSizeInBytes) {
    var id_value = document.getElementById(fileUploader.id).value;
    if (id_value != '') {
        var valid_extensions = /(.jpg|.jpeg|.gif|.png|.bmp)$/i;
        if (valid_extensions.test(id_value)) {
            if (document.getElementById(fileUploader.id).files[0].size > parseInt(maxSizeInBytes)) {
                document.getElementById(fileUploader.id).value = "";
                alert('File size should not be greater than ' + parseInt(maxSizeInBytes / 1024).toString() + 'KB');
            }
        }
        else {
            document.getElementById(fileUploader.id).value = "";
            alert('Invalid File');
        }
    }
}

/*End*/

/*UC date validation*/
function onfocusSelect(dt) { dt.select(); }
function giveSeprator(dt) {
    if (dt.value.length == 2) { dt.value += "-"; }
    if (dt.value.length == 6) { dt.value += "-"; }
}
function validateDateFormat(dt) {
    if (dt.value != "") {
        var strArr = dt.value.toString().split('-');

        if (strArr.length != 3) { clearDt(dt); }
        else if (strArr.length == 3) {
            var mth = getMonth(strArr[1]);
            var days = daysInMonth(getMonth(strArr[1]), strArr[2].toString());
            if (strArr[2].length != 4) {
                clearDt(dt);
            }
            else if (mth == 0) {
                clearDt(dt);
            }
            else if (strArr[0].length != 2) {
                clearDt(dt);
            }
            else if (parseInt(strArr[0]) > parseInt(days)) {
                clearDt(dt);
            }
            else {
                var m1 = strArr[1].toString().substring(0, 1);
                var m2 = strArr[1].toString().substring(1, 3);
                var month = m1.toUpperCase() + m2.toLowerCase();
                dt.value = strArr[0].toString() + '-' + month + '-' + strArr[2].toString();
            }
        }
    }
}

function clearDt(dt) {
    dt.value = "";
    showAlert("Invalid date format", "error", "#");
}

function daysInMonth(mth, yr) {
    return new Date(yr, mth, 0).getDate();
}

function getMonthName(mth) {
    var result = "";
    mth = parseInt(mth) + 1;
    switch (mth) {
        case "1":
            result = "Jan";
            break;
        case "2":
            result = "Feb";
            break;
        case "3":
            result = "Mar";
            break;
        case "4":
            result = "Apr";
            break;
        case "5":
            result = "May";
            break;
        case "6":
            result = "Jun";
            break;
        case "7":
            result = "Jul";
            break;
        case "8":
            result = "Aug";
            break;
        case "9":
            result = "Sep";
            break;
        case "10":
            result = "Oct";
            break;
        case "11":
            result = "Nov";
            break;
        case "12":
            result = "Dec";
            break;
    }
    return result;
}

function getMonth(mth) {
    mth = mth.toString().toLowerCase();
    var result = 0;
    switch (mth) {
        case "jan":
            result = 1;
            break;
        case "feb":
            result = 2;
            break;
        case "mar":
            result = 3;
            break;
        case "apr":
            result = 4;
            break;
        case "may":
            result = 5;
            break;
        case "jun":
            result = 6;
            break;
        case "jul":
            result = 7;
            break;
        case "aug":
            result = 8;
            break;
        case "sep":
            result = 9;
            break;
        case "oct":
            result = 10;
            break;
        case "nov":
            result = 11;
            break;
        case "dec":
            result = 12;
            break;
    }
    return result;
}

/*UC_Date Get Textbox or Value*/
function getDateTextBoxFromUC(UC_Name) {
    var UCDate1 = document.getElementById(UC_Name + '_txtDate');
    return UCDate1;
}
function getDateFromUC(UC_Name) {
    var UCDate1 = document.getElementById(UC_Name + '_txtDate');
    return UCDate1.value;
}
/*End UC Date*/

/*Drop down loading*/
function ddlLoadingOn(ddl) {
    ddl.options.length = 0;
    var option1 = document.createElement("option");
    option1.text = "Loading...";
    option1.value = "0";
    try {
        ddl.add(option1, null);
    }
    catch (Error) {
        ddl.add(option1);
    }
}
function ddlLoadingOff(ddl) {
    ddl.remove(0);
}
function ddlFillError(ddl) {
    ddl.remove(0);
    var option1 = document.createElement("option");
    option1.text = "Error Occurred";
    option1.value = "0";
    option1.style.color = "red";
    try {
        ddl.add(option1, null);
    }
    catch (Error) {
        ddl.add(option1);
    }
}
/*End Drop down loading*/

function trim(str) {
    str = str.replace(/^\s+|\s+$/g, '');
    return str;
}

function validateForm(divName) {
    var validate = true;
    var tablename = document.getElementById(divName.toString());
    var allinput = tablename.getElementsByTagName("input");
    var allselect = tablename.getElementsByTagName("select");
    var allspan = tablename.getElementsByTagName("span");
    var alltextarea = tablename.getElementsByTagName("textarea");

    var regex = /^\d+(?:\.\d{0,2})$/;

    for (var i = 0; i < allinput.length; i++) {
        if (allinput[i].type == "text" && allinput[i].accessKey == '1' && allinput[i].value == '') {
            var txtbox = allinput[i];
            txtbox.style.borderColor = "red";
            validate = false;
        }
        else { allinput[i].style.borderColor = ""; }
    }

    for (var i = 0; i < allselect.length; i++) {
        if (allselect[i].accessKey == '1' && allselect[i].selectedIndex <= 0) {
            var ddl = allselect[i];
            ddl.style.borderColor = "red";
            validate = false;
        }
        else {
            allselect[i].style.borderColor = "";
        }
    }

    for (var i = 0; i < alltextarea.length; i++) {
        if (alltextarea[i].accessKey == '1' && alltextarea[i].value == '') {
            var txtarea = alltextarea[i];
            txtarea.style.borderColor = "red";
            validate = false;
        }
        else { alltextarea[i].style.borderColor = ""; }
    }
    return validate;
}

function validateCtl(invoker) {
    if (invoker.type == "text" && invoker.accessKey == "1" && invoker.value == "") {
        invoker.style.borderColor = "red";
    }
    else if (invoker.type == "text") { invoker.style.borderColor = ""; }

    if (invoker.type == "select-one" && invoker.accessKey == "1" && invoker.selectedIndex <= 0) {
        invoker.style.borderColor = "red";
    }
    else if (invoker.type == "select-one") { invoker.style.borderColor = ""; }

    if (invoker.type == "textarea" && invoker.accessKey == "1" && invoker.value == "") {
        invoker.style.borderColor = "red";
    }
    else if (invoker.type == "textarea") { invoker.style.borderColor = ""; }


}

/*Div Sections [Collapse / Expand]*/
function divcollapsOpen(divname, invoker) {
    var strText = trim(invoker.innerHTML.toString());

    if (strText == 'Expand') {
        document.getElementById(divname).className = "divDetailExpand";
        invoker.innerHTML = 'Collapse';
    }
    else if (strText == 'Collapse') {
        document.getElementById(divname).className = "divDetailCollapse";
        invoker.innerHTML = 'Expand';
    }
}
/*End Div Sections [Collapse / Expand]*/

/*Notification IE style */
var myTimer;
function showAlert(msg, msytype, formpath) {
    var newdiv = document.createElement('div');
    newdiv.id = "divAlerts";
    newdiv.className = "alertBox0";

    var newdiv2 = document.createElement('div');
    msytype = msytype.toString().toLowerCase();
    switch (msytype) {
        case "info":
            newdiv2.style.borderTopColor = "green";
            break;
        case "error":
            newdiv2.style.borderTopColor = "red";
            break;
        case "p": /*Process*/
            newdiv2.style.borderTopColor = "navy";
            break;
        case "":
            newdiv2.style.borderTopColor = "orange";
            break;
    }


    var span1 = document.createElement('span');
    span1.id = 'alertmsg';
    newdiv2.appendChild(span1);
    newdiv.appendChild(newdiv2);
    //var strdiv = "<div id='divAlerts' class='alertBox0'><div><span id='alertmsg'></span></div></div>"
    document.body.appendChild(newdiv);
    document.getElementById('alertmsg').innerHTML = msg;
    document.getElementById('divAlerts').className = "alertBox1";
    myTimer = self.setInterval(function () { msgclock(formpath) }, 3500);
}

function msgclock(formpath) {
    document.body.removeChild(document.getElementById("divAlerts"));
    if (formpath != '#' && formpath != '') {
        window.open(formpath, '_self', '');
        LoadingOff();
    }
    clearInterval(myTimer);
}
/*End Notification*/

/*Format date dd-MMM-yyyy*/
function get_ddMMMyyyy(dt) {
    var result = "";
    result = dt.getDate() + "-";
    var mth1 = dt.getMonth();
    mth1 = parseInt(mth1) + 1;

    switch (mth1) {
        case 1:
            result += "Jan";
            break;
        case 2:
            result += "Feb";
            break;
        case 3:
            result += "Mar";
            break;
        case 4:
            result += "Apr";
            break;
        case 5:
            result += "May";
            break;
        case 6:
            result += "Jun";
            break;
        case 7:
            result += "Jul";
            break;
        case 8:
            result += "Aug";
            break;
        case 9:
            result += "Sep";
            break;
        case 10:
            result += "Oct";
            break;
        case 11:
            result += "Nov";
            break;
        case 12:
            result += "Dec";
            break;
    }
    result += "-" + dt.getFullYear();
    return result;
}


/*Add by Suresh For Loading */
function pleaseWaitWhileLoading() {
    alertDomDiv = document.createElement('Div');
    alertDomDiv.setAttribute('class', 'custAlertDivContentHolder');
    var strAlertHtml = '';
    strAlertHtml += '<div class="custAlertDivBackground"></div><table cellpadding="0" cellspacing="0" border="0" width="100%" height="100%" class="tlbAlertHolder"><tr><td align="center" valign="center">';
    strAlertHtml += '<div class="custAlertDiv waitWhileLoadingDivMain" align="center">';
    strAlertHtml += '<div class="pageLoadingImg"></div>';
    strAlertHtml += '<br /><br /><b>Please wait...</b></div>';
    strAlertHtml += '</td></tr></table>';
    alertDomDiv.innerHTML = strAlertHtml;
    document.body.appendChild(alertDomDiv);
}

window.onbeforeunload = pleaseWaitWhileLoading;
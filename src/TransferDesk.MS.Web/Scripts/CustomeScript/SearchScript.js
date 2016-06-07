
$(document).ready(function () {
    jQuery.validator.addMethod(
              'date',
              function (value, element, params) {
                  if (this.optional(element)) {
                      return true;
                  };
                  var result = false;
                  try {
                      $.datepicker.parseDate('dd/mm/yy', value);
                      result = true;
                  } catch (err) {
                      result = false;
                  }
                  return result;
              },
              ''

          );

    //$("#btnArticleTitle").click(function () {
    //    $("#ArticleTitleModal").modal();
    //});

    jQuery("#scroll-up").hide();
    jQuery(function () {
        jQuery(window).scroll(function () {
            if (jQuery(this).scrollTop() > 150) {
                jQuery('#scroll-up').fadeIn();
            } else {
                jQuery('#scroll-up').fadeOut();
            }
        });
        jQuery('#scroll-up').click(function () {
            jQuery('body,html').animate({
                scrollTop: 0
            }, 400);
            return false;
        });
    });

    if ($("#dbID").val() == "" || $("#dbID").val() == 0) {
        $("#btnExportTransferReport").hide();
        $("#btnPreviewManuscript").hide();
    }
    else {
        $("#btnExportTransferReport").show();
        $("#btnPreviewManuscript").show();
    }

    $('#btnOk').click(function () {
        var rowindex = $("input[name='rbtnCount']:checked").closest("tr").index();
        var ID = $("#ID_" + (rowindex - 1)).val();
        $("#dbID").val(ID);
        $('#myModal').modal('hide');
        if (ID == 0 || ID == '' || ID == null) {
            var url = AppPath+'Manuscript/HomePage';
        } else {
            var url = AppPath+'Manuscript/HomePage/' + ID;
        }
        window.location.href = url;

    });
    $("#ValidationSummaryClose").click(function () {
        $("#dvValidationSummary").css({ "display": "none" });
    });
    $('#txtSearch').keypress(function (e) {
        if (e.which == 13) {
            $('#btnSubmit').click();
        }
    });

    //$(document).keyup(function (e) {
    //    if (e.keyCode == 27) { // escape key maps to keycode `27`
    //        $(".mce-close").click();
    //    }
    //});    
   
    $('#myModal').on('hidden.bs.modal', function (e) {
        $('#txtSearch').val('');
        $('#SelectedValue').find('option:first').attr('selected', 'selected');
    })

    $('#btnSubmit').click(function () {
        var selectedValue = $("#SelectedValue option:selected").val();
        var searchText = $('#txtSearch').val();
        if (selectedValue == 0 || selectedValue == null || selectedValue == "") {
            alert('Please, select search-by');
            return false;
        }
        if (searchText == "" || searchText == null) {
            alert('Please,Enter search-text');
            return false;
        }
        //SearchProgress()
        var URL = AppPath+"Manuscript/GetSearchResult";
        loadMSDetailsModal(URL, selectedValue, searchText);
    });//submit action end

    $("#ddlJournalTitle").on("change", loadArticleTypeDDL);
    $('#btnNewAdd').click(ResetAllControls);
    $('#QualityStartCheckDate').datepicker({ dateFormat: 'dd/mm/yy' });
    $('.datepicker').datepicker({ dateFormat: 'dd/mm/yy' });
    
    if ($("#ArticleTitle").val() == '' || $("#ArticleTitle").val() == '0') {
        document.getElementById("StartDate").value = new Date($.now()).getDate() + "/" + ("0" + (new Date($.now()).getMonth() + 1)).slice(-2) + "/" + new Date($.now()).getFullYear();
        $('.datepicker').val('');
        $("#MSID").val('');
        $(".ManuscriptDetailsLastRow").css({ "display": "none" });
    }
    else {        

        $(".ManuscriptDetailsLastRow").css({ "display": "normal" });
    }

    $('#myModal, #PreviewManuscript').on('shown.bs.modal', function () {
        $(this).find('.modal-dialog').css({
            width: 'auto',
            height: 'auto',
            'max-height': '100%'
        });
    });

    $('#TransferReportModal').on('shown.bs.modal', function () {
        $(".mce-tinymce.mce-container.mce-panel").css({ "width": "500px" });
        $("body#tinymce.mce-content-body p").css({ "word-wrap": "initial" });
    });

    //add other author info
    $('#btnAddotherAuthor').click(function () {
        var OtherAuthorName = $('#txtOtherAuhtorName').val();
        var OtherAuhtorAffillation = $('#txtAuthorAffilation').val();
        if (OtherAuthorName == '' || OtherAuthorName == null) {
            alert('Please, Enter other Author Name ');
            return false;
        }
        if (OtherAuhtorAffillation == '' || OtherAuhtorAffillation == null) {
            alert('Please, Enter Other Author Affillation');
            return false;
        }
        var rowCount = $('#tblOtherAuthorsbody').children().length;
        var tBody = $('#tblOtherAuthorsbody');
        var row = $(tBody[0].insertRow(-1));
        var cell = $("<td />");
        cell.html("<input type='text' id=OtherAuthors_" + rowCount + "_AuthorName class='form-control input-sm' name='OtherAuthors[" + rowCount + "].AuthorName'  style='width:100%;height:25px'  />");
        row.append(cell);
        $("#OtherAuthors_" + rowCount + "_AuthorName").val(OtherAuthorName);
        var cell = $("<td />");

        cell.html("<input type='text' id=OtherAuthors_" + rowCount + "_Affillation class='form-control input-sm' name='OtherAuthors[" + rowCount + "].Affillation'  style='width:100%;height:25px' />");
        row.append(cell);
        $("#OtherAuthors_" + rowCount + "_Affillation").val(OtherAuhtorAffillation);
        //Clear Textboxes
        $('#txtOtherAuhtorName').val('');
        $('#txtAuthorAffilation').val('');
    });

    $("#btnTransferReportSaveSubmit").on("click", function () {
        $('#TransferReportModal').modal('hide');
    });

    //if quality role is selected display quality view
    $("#ddlRole").change(function () {
        IsQualityRole();
        IsViewEditable();
    });

    if ($("#ParentManuscriptID").val() == null || $("#ParentManuscriptID").val() == "") {
        $('#lblRevisedDate, #RevisedDate').hide();
    }
    $("#AddedNewRevision").change(function () {
        IsAddNewRevision();
    });


    $('#StartDate').prop("readonly", true);

    $("#btnTransferReportClick").click(function () {
        var transferReport = $("#ddlTransferReport option:selected").text();
        if (transferReport.toLocaleLowerCase() == "yes")
            $("#TransferReportModal").modal();
        else {
            $("#TransferReportModal").modal('hide');
        }
    });

    //when first time page load
    IsTransferReport();

    //when ddlTransferReport change
    $("#ddlTransferReport").change(function () {
        IsTransferReport();
    });

    //// Initialize your tinyMCE Editor with your preferred options
    tinyMCE.init({
        // General options
        mode: "textareas",
        theme: "modern",
        toolbar: "sizeselect | bold italic | fontselect | fontsizeselect | superscript | subscript",
        theme_advanced_toolbar_location: "top",
        theme_advanced_toolbar_align: "left",
        theme_advanced_statusbar_location: "bottom",
        theme_advanced_resizing: true,
        fontsize_formats: "8pt 10pt 12pt 14pt 18pt 24pt 36pt",
        editor_selector: "mceEditor",
        //menubar: false,
        statusbar: false,
        width: "320",
        height: "230",
        plugins: "paste",
        menubar: "edit",
        paste_data_images: true
    });

    // Initialize your tinyMCE Editor with your preferred options
    tinyMCE.init({
        mode: "textareas",
        theme: "modern",
        toolbar: "sizeselect | bold italic | fontselect | fontsizeselect | superscript | subscript",
        theme_advanced_toolbar_location: "top",
        theme_advanced_toolbar_align: "left",
        theme_advanced_statusbar_location: "bottom",
        theme_advanced_resizing: true,
        fontsize_formats: "8pt 10pt 12pt 14pt 18pt 24pt 36pt",
        editor_selector: "mceEditor1",
        width: "340",
        height: "430",
        plugins: "paste",
        menubar: "edit",
        paste_data_images: true
    });

    //when ddl QualityCheck change
    $("#QualityCheck").change(function () {
        IsQualityCheck();
    });

    //when first time page load
    IsAccurate();
    //when ddlAccurate change
    $("#ddlAccurate").change(function () {
        IsAccurate();
    });


    $("#AssignedEditor").autocomplete({
        source: function (request, response) {
            $.ajax({
                url:AppPath+ 'Manuscript/GetAssignedEditor',
                data: { searchText: request.term, journalID: $("#ddlJournalTitle option:selected").val() },
                dataType: "json",
                type: "GET",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    response($.map(data, function (item) {
                        return {
                            label: item.AssignedEditor,
                            val: item.AssignedEditor
                        }
                    }))
                },
                error: function (response) {
                    alert("Please,select journal title");
                },
                failure: function (response) {
                    alert("Please,select journal title");
                }
            });
        },
        select: function (e, i) {
            $("#AssignedEditor").val(i.item.val);
        },
        minLength: 1
    });

    $("#btnAssociateSave,#btnAssociateIsFinalSubmit, #btnQualitySave,#btnIsQualityFinalSubmit").click(function () {
        //ImageDropdownValidation();
        var iThenticateResult = $('#hdniThenticateResult1').val();
        var EnglishlangQualityId = $('#hdnEnglishlangQuality1').val();
        var EthicsComplienceId = $('#hdnEthicsComplience1').val();
        if (iThenticateResult == 0) {
            alert("Please,select Cross Check / iThenticate result");
            return false;
        }
        else if (EnglishlangQualityId == 0) {
            alert("Please, select English language Quality");
            return false;
        }
        else if (EthicsComplienceId == 0) {
            alert("Please, select Ethics Compliance")
            return false;
        }
    });

    $("#tmpAssociateFinalSubmitValue").val($("#hdnIsAssociateFinalSubmit").val());
    $("#tmpQualityFinalSubmitValue").val($("#hdnIsQualityFinalSubmit").val());

    //$('#RevisedDate, #lblRevisedDate').hide();

    //when first time page load
    IsQualityRole();

    //when first time page load
    IsQualityCheck();

    IsViewEditable();

    $("#btnIsQualityFinalSubmit").click(function() {
        var role = $("#ddlRole option:selected").text();
        if (role == "Quality Analyst") {
            var QualityCheck = $("#QualityCheck option:selected").text();
            if (QualityCheck.toLocaleLowerCase() == "yes") {
                var accurate = $("#ddlAccurate option:selected").text();
                if (accurate.toLocaleLowerCase() == "no") {
                    if (!$(".chkQualityCheck:checked").length > 0) {
                        alert("Please, Select Error Categories");
                        return false;
                    }
                    if (!(jQuery.trim($("#ErrorDescription").val()).length>0)) {
                        //alert(jQuery.trim($("#ErrorDescription").val()).length);
                        alert("Please, Enter Error Description");
                        return false;
                    }
                }
            }
        }
    });

    $('span').removeClass("dd-pointer dd-pointer-down");

});//ready function end

function ImageDropdownValidation() {
    var iThenticateResult = $('#hdniThenticateResult1').val();
    var EnglishlangQualityId = $('#hdnEnglishlangQuality1').val();
    var EthicsComplienceId = $('#hdnEthicsComplience1').val();
    if (iThenticateResult == 0) {
        alert("Please,select Cross Check / iThenticate result");
        return false;
    }
    else if (EnglishlangQualityId == 0) {
        alert("Please, select English language Quality");
        return false;
    }
    else if (EthicsComplienceId == 0) {
        alert("Please, select Ethics Complience")
        return false;
    }
}

// enable fields if revision checkbox is selected
function IsAddNewRevision() {
    var isAssociateFinalSubmit = $("#tmpAssociateFinalSubmitValue").val();
    var isQualityFinalSubmit = $("#tmpQualityFinalSubmitValue").val();

    if ($("#AddedNewRevision").is(":checked")) {
        $('input[type="text"], textarea, select').prop("disabled", false);
        $('#ddlOverallAnalysis, #ddliThenticateResult, #ddlEnglishlangQuality, #ddlEthicsComplience').css({ "pointer-events": "normal" });
        IsQualityRole();
        $(".chkQualityCheck").prop("disabled", false);
        IsAccurate();
        IsQualityCheck();
        $("#hdnIsAssociateFinalSubmit").val('');
        $("#hdnIsQualityFinalSubmit").val('');
        IsTransferReport();
        $('#lblRevisedDate').show();
        $('#RevisedDate').show();
        $('#RevisedDate').val($.datepicker.formatDate("dd/mm/yy", new Date()));
    }
    else{
        $("#hdnIsAssociateFinalSubmit").val(isAssociateFinalSubmit);
        $("#hdnIsQualityFinalSubmit").val(isQualityFinalSubmit);
        var role = $("#ddlRole option:selected").text();
        if (role.toLocaleLowerCase() == "associate" && (isAssociateFinalSubmit.toLocaleLowerCase() == "true" || isQualityFinalSubmit.toLocaleLowerCase() == "true")) {
            $('input[type="text"], textarea, select').prop("disabled", true);
            $("#SelectedValue, #txtSearch").prop('disabled', false);
            $("#btnAssociateSave,#btnAssociateIsFinalSubmit").prop('disabled', true);
            $("#ddlRole").prop('disabled', false);
            $('#ddlOverallAnalysis, #ddliThenticateResult, #ddlEnglishlangQuality, #ddlEthicsComplience').css({ "pointer-events": "none" });
        }
        else if (role == "Quality Analyst" && isQualityFinalSubmit.toLocaleLowerCase() == "true") {
            $('input[type="text"], textarea, select').prop("disabled", true);
            $("#SelectedValue, #txtSearch").prop('disabled', false);
            $("#ddlRole").prop('disabled', false);
            $('#ddlOverallAnalysis, #ddliThenticateResult, #ddlEnglishlangQuality, #ddlEthicsComplience').css({ "pointer-events": "none" });
            $(".chkQualityCheck").prop("disabled", true);
            $("#btnAssociateSave,#btnAssociateIsFinalSubmit, #btnQualitySave,#btnIsQualityFinalSubmit").prop('disabled', true);
        }
        else if (role == "Quality Analyst") {

            $('input[type="text"], textarea, select').prop("disabled", false);
            $("#QualityCheck, #ErrorDescription, .chkQualityCheck,#ddlAccurate").prop('disabled', false);
            $("#QualityStartCheckDate").prop("readonly", false);
            $("#QualityStartCheckDate").datepicker("option", "disabled", false);
            $('#ddlOverallAnalysis, #ddliThenticateResult, #ddlEnglishlangQuality, #ddlEthicsComplience').css({ "pointer-events": "normal" });
            $("#btnAssociateSave,#btnAssociateIsFinalSubmit").prop('disabled', true);
            IsTransferReport();
            IsAccurate();
            IsQualityCheck();
        } 
	
        //for handling save and submit enable/disable depending on final submit value if revision checkbox is unchecked
        if (isAssociateFinalSubmit.toLocaleLowerCase() == "true") {
            $("#btnAssociateSave,#btnAssociateIsFinalSubmit,#btnTransferReportClick").prop('disabled', true);
        }
        else if (isQualityFinalSubmit.toLocaleLowerCase() == "true") {
            $("#btnAssociateSave,#btnAssociateIsFinalSubmit,#btnTransferReportClick, #btnQualitySave,#btnIsQualityFinalSubmit").prop('disabled', true);
        }
        

        if ($("#ParentManuscriptID").val() == null || $("#ParentManuscriptID").val() == "") {
            $('#lblRevisedDate,#RevisedDate').hide();
        }
    }
}

function IsQualityRole() {
    var role = $("#ddlRole option:selected").text();
    if (role == "Quality Analyst") {
        $("#divQualityAnalyst").css({ "display": "normal" });
        $("#btnAssociateSave, #btnAssociateIsFinalSubmit").prop('disabled', true);
        $("#hrUpQualityAnalyst").css({ "display": "normal" });
        $("#btnNewAdd").prop('disabled', true);
        $("#btnQualitySave,#btnIsQualityFinalSubmit").prop('disabled', false);
    }
    else {
        $("#divQualityAnalyst").css({ "display": "none" });
        $("#hrUpQualityAnalyst").css({ "display": "none" });
        $("#btnNewAdd").prop('disabled', false);
        $("#btnAssociateSave, #btnAssociateIsFinalSubmit").prop('disabled', false);
        $("#btnQualitySave,#btnIsQualityFinalSubmit").prop('disabled', true);
    }
}

// Enable/disable fields depend on the final submit value of associate and quality
function IsViewEditable() {
    var role = $("#ddlRole option:selected").text();
    if ($("#hdnIsAssociateFinalSubmit").val().toLocaleLowerCase() == "true" || $("#hdnIsQualityFinalSubmit").val().toLocaleLowerCase() == "true") {
        if (role.toLocaleLowerCase() == "associate") {
            $('input[type="text"], textarea, select').prop("disabled", true);
            $("#SelectedValue, #txtSearch").prop('disabled', false);
            $("#btnAssociateSave,#btnAssociateIsFinalSubmit,#btnTransferReportClick").prop('disabled', true);
            $("#ddlRole").prop('disabled', false);
            $('#ddlOverallAnalysis, #ddliThenticateResult, #ddlEnglishlangQuality, #ddlEthicsComplience').css({ "pointer-events": "none" });
            IsQualityRole();
            IsAddNewRevision();
        }
        else if (role == "Quality Analyst") {
            $('input[type="text"], textarea, select').prop("disabled", true);
            $("#SelectedValue, #txtSearch").prop('disabled', false);
            $("#ddlRole").prop('disabled', false);
            $("#btnAssociateSave,#btnAssociateIsFinalSubmit, #btnQualitySave,#btnIsQualityFinalSubmit,#btnTransferReportClick").prop('disabled', true);
            $('#ddlOverallAnalysis, #ddliThenticateResult, #ddlEnglishlangQuality, #ddlEthicsComplience').css({ "pointer-events": "none" });
            $(".chkQualityCheck").prop("disabled", true);
            IsQualityRole();
            IsAddNewRevision();
            IsTransferReport();
        }
        else {
            $('input[type="text"], textarea, select').prop("disabled", false);
            $("#SelectedValue, #txtSearch").prop('disabled', false);
            $("#btnAssociateSave, #btnAssociateIsFinalSubmit,#btnTransferReportClick").prop('disabled', true);
            $("#ddlRole").prop('disabled', false);
            $('#ddlOverallAnalysis, #ddliThenticateResult, #ddlEnglishlangQuality, #ddlEthicsComplience').css({ "pointer-events": "none" });
            IsQualityRole();
            IsAddNewRevision();
            IsTransferReport();
        }
    } 
}

function IsTransferReport() {
    var transferReport = $("#ddlTransferReport option:selected").text();
    if (transferReport.toLocaleLowerCase() == 'yes') {
        $('#btnTransferReportClick').prop('disabled', false);
    }
    else
        $('#btnTransferReportClick').prop('disabled', true);

    if(transferReport.toLocaleLowerCase() == 'yes' && ($("#hdnIsAssociateFinalSubmit").val().toLocaleLowerCase() == "true"))
    {
        $('#btnTransferReportClick').prop('disabled', true);
        var role = $("#ddlRole option:selected").text();
        if (role == "Quality Analyst") {
            $('#btnTransferReportClick').prop('disabled', false);
        }
    }

    if (transferReport.toLocaleLowerCase() == 'yes' && $("#hdnIsQualityFinalSubmit").val().toLocaleLowerCase() == "true") {
        $('#btnTransferReportClick').prop('disabled', true);
    }
}

function IsQualityCheck() {
    var QualityCheck = $("#QualityCheck option:selected").text();
    if (QualityCheck.toLocaleLowerCase() == "no") {
        $(".chkQualityCheck").prop("disabled", true);
        $("#ErrorDescription").prop('disabled', true);
        $("#QualityStartCheckDate").prop("readonly", true);
        $("#ddlAccurate").prop("disabled", true);
        $("#QualityStartCheckDate").prop("readonly", true);
        $("#QualityStartCheckDate").datepicker("option", "disabled", true);
        $("#QualityStartCheckDate").val('');
    }
    else {
        //$(".chkQualityCheck").prop("disabled", false);
        //$("#ErrorDescription").prop('disabled', false);
        $("#QualityStartCheckDate").prop("readonly", false);
        $("#ddlAccurate").prop("disabled", false);
        $("#QualityStartCheckDate").prop("readonly", false);
        $("#QualityStartCheckDate").datepicker("option", "disabled", false);
        IsAccurate();
    }
}

function IsAccurate() {
    var accurate = $("#ddlAccurate option:selected").text();
    if (accurate.toLocaleLowerCase() == "yes") {
        $(".chkQualityCheck").prop("disabled", true);
        $("#ErrorDescription").prop('disabled', true);
    }
    else {
        $(".chkQualityCheck").prop("disabled", false);
        $("#ErrorDescription").prop('disabled', false);
    }

}

function SearchProgress() {
    $('#circle').circleProgress({
        value: 0.5,
        size: 200,
        fill: {
            gradient: ["red", "orange"]
        },
        animation: { duration: 700 }
    });
}//searchProgress End

function loadMSDetailsModal(URL, selectedValue, searchText) {

    $.get(URL, { "SelectedValue": selectedValue, "SearchBy": searchText }, function (data) {
        var array;
        if (data == '' || data == null) {
            alert("No,record found");
            $('#myModal').modal('hide');
            return false;
        }
        else {
            $.each(data, function (key, val) {
                $.each(val, function (k, v) {
                    array += k + "--" + v + "##";
                });
                array += "---";
            });

            //Create a HTML Table element.
            var table = $("<table />");
            table[0].id = "trdMSDetails";
            table[0].className = "table table-striped table-hover";

            //Add headers
            var row = $(table[0].insertRow(-1));
            $.each(data, function (key, val) {
                var headerCell = $("<th style='padding-right: 15px;text-align: left;'  />");
                headerCell.html("Action");
                row.append(headerCell);
                $.each(val, function (k, v) {
                    if (k != "ID") {
                        if (k == "MSID") {
                            k = "Manuscript Number";
                        }
                        var headerCell = $("<th style='padding-right: 15px;text-align: left;' />");
                        headerCell.html(k);
                        row.append(headerCell);
                    }
                    else {
                        var headerCell = $("<th style='display:none;text-align: left;' />");
                        headerCell.html(k);
                        row.append(headerCell);
                    }
                });
                if (data.length > 0)
                    return false;
            });
            var tBody = $(table[0]).append("<tbody/>");
            rows = array.split('---');
            //add row data
            for (var i = 0; i < rows.length - 1; i++) {
                //var row = $(table[0].insertRow(-1));
                var row = $(tBody[0].insertRow(-1));
                var temp = rows[i].split('##');
                //add temp array data to td's
                for (var j = 0; j < temp.length - 1; j++) {
                    tdData = temp[j].split('--');
                    if (j == 0) {
                        var cell = $("<td align='left' valign='middle' style='padding-right: 15px'  />");
                        cell.html('<input type="radio" name="rbtnCount" class="rdbChecked1" id="rdbSelectMS"/>');
                        row.append(cell);
                        var cell = $("<td style='display:none;' />");
                        cell.html("<input type='hidden' id=ID_" + i + " name='ID[" + i + "]' value=" + tdData[1] + " />");
                        row.append(cell);
                    }
                    else {
                        var cell = $("<td align='left' valign='middle' style='padding-right: 15px'  />");
                        if (tdData[1] == "null") {
                            tdData[1] = '';
                        }
                        cell.html(tdData[1]);
                        row.append(cell);
                    }
                }
            }
            var dvTable = $("#myPartialViewDiv");
            dvTable.html("");
            $('#circle').hide();
            dvTable.append(table);
            $("#myModal").modal();
        }
    });

}

function ResetAllControls() {
        var url =AppPath +'Manuscript/';
    window.location.href = url;
}

function loadArticleTypeDDL() {
    var selectedJournal = $("#ddlJournalTitle option:selected").val();
    var articleDDL = $("#ddlArticleType");
    var sectionTypeDDL = $("#ddlSectionType");
    if (selectedJournal == 0 || selectedJournal == null || selectedJournal == "") {
        articleDDL.empty();
        articleDDL.append(new Option("Select-ArticleType", ""));
        sectionTypeDDL.empty();
        sectionTypeDDL.append(new Option("Select-Section", ""));
        return false;
    }
    $.ajax(
    {
        method: "GET",


        url:AppPath+ "Manuscript/GetArticleType",
        contentType: "application/json; charset=utf-8",
        data: { journalMasterID: selectedJournal },
        success: function (data) {
            var drpCntnt = "";
            if (data != "") {
                var array = data.split('~');
                var po;
                if (array.length == 0) {
                    po = "No Journal available";
                }
                else {
                    articleDDL.empty();
                    articleDDL.append(new Option("Select-ArticleType", ""));
                    //articleDDL.append(new Option(" ", 0));
                    for (var i = 0; i < array.length - 1; i++) {
                        var temp = array[i].split('---');
                        articleDDL.append(new Option(temp[1], temp[0]));
                    }
                }
            }
            else {
                //alert("no Article");
                articleDDL.empty();
                articleDDL.append(new Option("Select-ArticleType", ""));
                $("#ddlArticleType option[value='']").attr('selected', true)
            }
        },
        error: function (xhr, exception) {
            alert("Error occured while fetching Article Type details.");
        }
    });

    $.ajax(
    {
        method: "GET",


        url:AppPath +"Manuscript/GetSectionType",
        contentType: "application/json; charset=utf-8",
        data: { journalMasterID: selectedJournal },
        success: function (data) {
            var drpCntnt = "";
            if (data != "") {
                var array = data.split('~');
                var po;
                if (array.length == 0) {
                    po = "No Journal available";
                }
                else {
                    sectionTypeDDL.empty();
                    sectionTypeDDL.append(new Option("Select-Section", ""));
                    for (var i = 0; i < array.length - 1; i++) {
                        var temp = array[i].split('---');
                        sectionTypeDDL.append(new Option(temp[1], temp[0]));
                    }



                }
            }
            else {
                //alert("no section");
                $("#ddlSectionType option[value='']").attr('selected', true)
                sectionTypeDDL.empty();
                sectionTypeDDL.append(new Option("Select-Section", ""));
            }
        },
        error: function (xhr, exception) {
            alert("Error occured while fetching Section details.");
        }
    });
}





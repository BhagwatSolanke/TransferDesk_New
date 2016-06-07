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

    $('#btnOk').click(function () {
        //var row = $("input[name='rbtnCount']:checked").closest("tr").find("td");
        var rowindex = $("input[name='rbtnCount']:checked").closest("tr").index();
        var ID = $("#ID_" + (rowindex - 1)).val();
        $("#dbID").val(ID);
        $('#myModal').modal('hide');
        if (ID == 0 || ID == '' || ID == null) {
            //var url = '/TransferDeskService/Manuscript/HomePage/';
            //var url = '/ManuscriptScreening/Manuscript/HomePage/';
             var url = '/Manuscript/HomePage';
        } else {
            //var url = '/TransferDeskService/Manuscript/HomePage/' + ID;
            //var url = '/ManuscriptScreening/Manuscript/HomePage/' + ID;
            var url = '/Manuscript/HomePage/' + ID;
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
        //var URL = "/TransferDeskService/Manuscript/GetSearchResult";
       // var URL = "/ManuscriptScreening/Manuscript/GetSearchResult";
        var URL = "/Manuscript/GetSearchResult";
        
        

        loadMSDetailsModal(URL, selectedValue, searchText);
    });//submit action end

    //$("#dvValidationSummary").html('<button id="ValidationSummaryClose" class="close btnTransferent">&times;</button>')

    //$("input:checkbox").on('click', function () {
    //    var $box = $(this);
    //    if ($box.is(":checked")) {
    //        var group = "input:checkbox[name='" + $box.attr("name") + "']";
    //        $(group).prop("checked", false);
    //        $box.prop("checked", true);
    //    } else {
    //        $box.prop("checked", false);
    //    }
    //});
    $("#ddlJournalTitle").on("change", loadArticleTypeDDL);
    $('#btnNewAdd').click(ResetAllControls);
    $('#QualityStartCheckDate').datepicker({ dateFormat: 'dd/mm/yy' });
    $('#QualityStartCheckDate').val($.datepicker.formatDate("dd/mm/yy", new Date()));
    $('.datepicker').datepicker({ dateFormat: 'dd/mm/yy' });
    // $("#MSID").val('');
    if ($("#ArticleTitle").val() == '' || $("#ArticleTitle").val() == '0') {
        $('.datepicker').val($.datepicker.formatDate("dd/mm/yy", new Date()));
        document.getElementById("StartDate").value = new Date($.now()).getDate() + "/" + new Date($.now()).getMonth() + "/" + new Date($.now()).getFullYear();
        $("#MSID").val('');
    }

    $('#myModal').on('shown.bs.modal', function () {
        $(this).find('.modal-dialog').css({
            width: 'auto',
            height: 'auto',
            'max-height': '100%'
        });
    });

    $('#TransferReportModal').on('shown.bs.modal', function () {
        //$(this).find('.modal-dialog').css({
        //    width: 'auto',
        //    height: 'auto',
        //    'max-height': '100%'
        //});
        $(".mce-tinymce.mce-container.mce-panel").css({ "width": "500px" });
        $("div#mceu_28.mce-tinymce.mce-container.mce-panel").css({ "width": "310px" });
        //$("div#mceu_2-body.mce-container-body.mce-stack-layout").css({ "width": "500px" });


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
        //cell.html("<input type='text' id=OtherAuthors_" + rowCount + "_Affillation class='form-control input-sm' name='OtherAuthors[" + rowCount + "].Affillation' value=" + OtherAuhtorAffillation + "  style='width:100%' />");
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


    //when first time page load
    IsQualityRole()


    //if quality role is selected display quality view
    $("#ddlRole").change(function () {
        IsQualityRole()
    });

    
    $('#StartDate').prop("readonly", true);
    //$("#StartDate").datepicker("disable");
 

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


    // Initialize your tinyMCE Editor with your preferred options
    tinyMCE.init({
        // General options
        mode: "textareas",
        //setup: function (editor) {
        //    editor.on('change', function () {
        //        editor.save();
        //    });
        //},
        theme: "modern",
        // Theme options
        //theme_advanced_buttons1: "save,newdocument,|,bold,italic,underline,strikethrough,|,justifyleft,justifycenter,justifyright,justifyfull,|,styleselect,formatselect,fontselect,fontsizeselect",
        //theme_advanced_buttons2: "cut,copy,paste,pastetext,pasteword,|,search,replace,|,bullist,numlist,|,outdent,indent,blockquote,|,undo,redo,|,link,unlink,anchor,image,cleanup,help,code,|,insertdate,inserttime,preview,|,forecolor,backcolor",
        //theme_advanced_buttons3: "tablecontrols,|,hr,removeformat,visualaid,|,sub,sup,|,charmap,emotions,iespell,media,advhr,|,print,|,ltr,rtl,|,fullscreen",
        //theme_advanced_buttons4: "insertlayer,moveforward,movebackward,absolute,|,styleprops,spellchecker,|,cite,abbr,acronym,del,ins,attribs,|,visualchars,nonbreaking,template,blockquote,pagebreak,|,insertfile,insertimage",
        toolbar: "sizeselect | bold italic | fontselect |  fontsizeselect",
        theme_advanced_toolbar_location: "top",
        theme_advanced_toolbar_align: "left",
        theme_advanced_statusbar_location: "bottom",
        theme_advanced_resizing: true,
        fontsize_formats: "8pt 10pt 12pt 14pt 18pt 24pt 36pt",
        editor_selector: "mceEditor",
        //toolbar: false,
        menubar: false,
        statusbar: false,
        width: "320",
        height: "230"
    });

    // Initialize your tinyMCE Editor with your preferred options
    tinyMCE.init({
        mode: "textareas",
        theme: "modern",
        // Theme options
        //theme_advanced_buttons1: "save,newdocument,|,bold,italic,underline,strikethrough,|,justifyleft,justifycenter,justifyright,justifyfull,|,styleselect,formatselect,fontselect,fontsizeselect",
        //theme_advanced_buttons2: "cut,copy,paste,pastetext,pasteword,|,search,replace,|,bullist,numlist,|,outdent,indent,blockquote,|,undo,redo,|,link,unlink,anchor,image,cleanup,help,code,|,insertdate,inserttime,preview,|,forecolor,backcolor",
        //theme_advanced_buttons3: "tablecontrols,|,hr,removeformat,visualaid,|,sub,sup,|,charmap,emotions,iespell,media,advhr,|,print,|,ltr,rtl,|,fullscreen",
        //theme_advanced_buttons4: "insertlayer,moveforward,movebackward,absolute,|,styleprops,spellchecker,|,cite,abbr,acronym,del,ins,attribs,|,visualchars,nonbreaking,template,blockquote,pagebreak,|,insertfile,insertimage",
        toolbar: "sizeselect | bold italic | fontselect |  fontsizeselect",
        theme_advanced_toolbar_location: "top",
        theme_advanced_toolbar_align: "left",
        theme_advanced_statusbar_location: "bottom",
        theme_advanced_resizing: true,
        fontsize_formats: "8pt 10pt 12pt 14pt 18pt 24pt 36pt",

        editor_selector: "mceEditor1",
        //toolbar: false,
        menubar: false,
        statusbar: false,
        width: "340",
        height: "430"

    });
    
    //when first time page load
    IsQualityCheck();
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



    $('span').removeClass("dd-pointer dd-pointer-down");
    // loadArticleTypeDDL();



});//ready function end


function IsQualityRole() {
    var role = $("#ddlRole option:selected").text();
    if (role == "Quality Analyst") {
        $("#divQualityAnalyst").css({ "display": "normal" });
        $("#btnAssociateSaveSubmit").prop('disabled', true);
        $("#hrUpQualityAnalyst").css({ "display": "normal" });
        //$("#QualityUserID").val($("#hdnUserID").val());
        
    }
    else {
        $("#divQualityAnalyst").css({ "display": "none" });
        $("#btnAssociateSaveSubmit").prop('disabled', false);
        $("#hrUpQualityAnalyst").css({ "display": "none" });
        //$("#UserID").val($("#hdnUserID").val());
    }

}

function IsTransferReport() {
    var transferReport = $("#ddlTransferReport option:selected").text();
    if (transferReport.toLocaleLowerCase() == 'yes')
        $('#btnTransferReportClick').prop('disabled', false);
    else
        $('#btnTransferReportClick').prop('disabled', true);
}

function IsQualityCheck()
{
    var QualityCheck = $("#QualityCheck option:selected").text();
    if (QualityCheck.toLocaleLowerCase() == "no") {
        $(".chkQualityCheck").prop("disabled", true);
        $("#ErrorDescription").prop('disabled', true);
        $("#QualityStartCheckDate").prop("readonly", true);
        $("#ddlAccurate").prop("disabled", true);
        $("#QualityStartCheckDate").prop("readonly", true);
        var QualityStartCheckDate = $("#QualityStartCheckDate").datepicker({ dateFormat: 'dd/mm/yy' }).val();
        $("#QualityStartCheckDate").datepicker("option", "disabled", true);
        if (QualityStartCheckDate != null || QualityStartCheckDate != '') {
            $("#QualityStartCheckDate").val(QualityStartCheckDate.toString());
        }

    }
    else {
        $(".chkQualityCheck").prop("disabled", false);
        $("#ErrorDescription").prop('disabled', false);
        $("#QualityStartCheckDate").prop("readonly", false);
        $("#ddlAccurate").prop("disabled", false);
        $("#QualityStartCheckDate").prop("readonly", false);
        $("#QualityStartCheckDate").datepicker("option", "disabled", false);

    }
}

function IsAccurate()
{
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
            //table[0].border = "1";

            //Get the count of columns.
            //var columnCount = Object.keys(data[0]).length;

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
    ////$('#frmHomePage').get(0).reset();
    //$('input[type="text"]').val('');
    //$('textarea').val('');
    //$('select option[value=""]').attr("selected", true);
    //$('select option[value="0"]').attr("selected", true);
    //$('select option[value="False"]').attr("selected", true);
    //$("input[type='hidden']").val("0");
    //$('#tblOtherAuthorsbody').empty();
    ////$('#StartDate').prop("readonly", false);
    ////$("#StartDate").datepicker("enable")
    //$('#StartDate').val($.datepicker.formatDate("dd/mm/yy", new Date()));
    //$('#StartDate').prop("readonly", true);
    //$("#StartDate").datepicker("disable");
    //$('#StartDate').val($.datepicker.formatDate("dd/mm/yy", new Date()));
    ////$("#StartDate").datepicker({ 'setDate': new Date() });
    //$("#hrUpQualityAnalyst").css({ "display": "none" });
    //$("#divQualityAnalyst").css({ "display": "none" });
    //$("#btnAssociateSaveSubmit").prop('disabled', false);
    //$('#ddliThenticateResult').ddslick('select', { index: 0 });
    //$('#ddlEnglishlangQuality').ddslick('select', { index: 0 });
    //$('#ddlEthicsComplience').ddslick('select', { index: 0 });    
    //$("#dvValidationSummary").css({ "display": "none" });
    ////$('#ddlEthicsComplience').ddslick('select', { selectText: "Select-iThenticateResult" });
    //$("#dbID").val("0");

    //var url = '/TransferDeskService/Manuscript/';
    var URL = "/ManuscriptScreening/Manuscript/";
     // var url = '/Manuscript/';
    window.location.href = url;
}

function loadArticleTypeDDL() {
    var selectedJournal = $("#ddlJournalTitle option:selected").val();
    var articleDDL = $("#ddlArticleType");
    var sectionTypeDDL = $("#ddlSectionType");
    if (selectedJournal == 0 || selectedJournal == null || selectedJournal == "") {
        articleDDL.empty();
        articleDDL.append(new Option("Select-ArticleType", 0));
        sectionTypeDDL.empty();
        sectionTypeDDL.append(new Option("Select-SectionType", 0));
        return false;
    }
    $.ajax(
    {
        method: "GET",
        //url: "/TransferDeskService/Manuscript/GetArticleType",
        
       // url: "/ManuscriptScreening/Manuscript/GetArticleType",
        url: "/Manuscript/GetArticleType",
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
                    //articleDDL.append(new Option("Select-ArticleType", 0));
                    for (var i = 0; i < array.length - 1; i++) {
                        var temp = array[i].split('---');
                        articleDDL.append(new Option(temp[1], temp[0]));
                    }
                }
            }
            else {
                //alert("no Article");
                articleDDL.empty();
                articleDDL.append(new Option("Select-ArticleType", 0));
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
        //  url: "/TransferDeskService/Manuscript/GetSectionType",
        //url: "/ManuscriptScreening/Manuscript/GetSectionType",
        url: "/Manuscript/GetSectionType",
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
                    for (var i = 0; i < array.length - 1; i++) {
                        var temp = array[i].split('---');
                        sectionTypeDDL.append(new Option(temp[1], temp[0]));
                    }
                    //if ($("#ddlSectionType option").length > 1) {
                    //    $("#ddlSectionType").val('1');
                    //}
                }
            }
            else {
                //alert("no section");
                $("#ddlSectionType option[value='']").attr('selected', true)
                sectionTypeDDL.empty();
                sectionTypeDDL.append(new Option("Select-SectionType", 0));
            }
        },
        error: function (xhr, exception) {
            alert("Error occured while fetching Section details.");
        }
    });
}





$(document).ready(function () {

    $('#dvIsActive').css({ "display": "none" });
    $('.editButton').click(function () {
        $('.field-validation-error').text("")
        $('#ID').val($(this).closest("tr").find("td").eq(0).text());
        $('#JournalTitle').val($(this).closest("tr").find("td").eq(1).text().trim());
        $('#Link').val($(this).closest("tr").find("td").eq(2).text().trim());
        $('#IsActive').val($(this).closest("tr").find("td").eq(2).text());
        var chkvalue = $(this).closest("tr").children("td").find("input[type='checkbox']").attr("checked");
        $('#dvIsActive').css({ "display": "block" });
        if (chkvalue == "checked") {
            $("#IsActive").prop("checked", true);
            $("#IsActive").val("true");
        }
        else {
            $("#IsActive").prop("checked", false);
            $("#IsActive").val("true");
        }
        $('#btnJournalAdd').val("Update");
    });

    $('#dvIsActive').css({ "display": "none" });

    $('#btnReset').click(function () {
        //window.location.href = "/ManuscriptScreening/Manuscript/Journal/"; 
        window.location.href = AppPath + "Admin/JournalMaster";
    });


    $('#IsActive').change(function () {
        if ($(this).is(":checked")) {
            $(this).val("true");
        } else {
            $(this).val("false");
        }
    });

});//ready function end
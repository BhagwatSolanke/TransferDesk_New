$(document).ready(function () {
    $('#dvIsActive').css({ "display": "none" });
    $('.editButton').click(function () {
        $('.field-validation-error').text("");
        $('#ID').val($(this).closest("tr").find("td").eq(0).text());
        $('#BookTitle').val($(this).closest("tr").find("td").eq(1).text().trim());
        $('#GPUInformation').val($(this).closest("tr").find("td").eq(2).text().trim());
        $('#FTPLink').val($(this).closest("tr").find("td").eq(3).text().trim());
        $('#IsActive').val($(this).closest("tr").find("td").eq(4).text());
        var chkvalue = $(this).closest("tr").children("td").find("input[type='checkbox']").attr("checked");
        $('#dvIsActive').css({ "display": "" });
        if (chkvalue == "checked") {
            $("#IsActive").prop("checked", true);
            $("#IsActive").val("true");
        }
        else {
            $("#IsActive").prop("checked", false);
            $("#IsActive").val("true");
        }
        $('#btnBookMasterAdd').val("Update");
    });

    $('#dvIsActive').css({ "display": "none" });

    $('#btnReset').click(function () {
        window.location.href = AppPath + "Admin/BookMaster";
    });


    $('#IsActive').change(function () {
        if ($(this).is(":checked")) {
            $(this).val("true");
        } else {
            $(this).val("false");
        }
    });

});//ready function end
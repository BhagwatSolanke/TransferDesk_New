$(document).ready(function () {
    $('#dvIsActive').css({ "display": "none" });
    $(".editButton").click(function () {
        $('#ID').val($(this).closest("tr").find("td").eq(0).text());
        $('#EmpID').val($(this).closest("tr").find("td").eq(1).text());
        $('#EmpUserID').val($(this).closest("tr").find("td").eq(2).text());
        $('#EmpName').val($(this).closest("tr").find("td").eq(3).text());
        $('#Email').val($(this).closest("tr").find("td").eq(4).text());
        var chkValue = $(this).closest("tr").children("td").find("input[type='checkbox']").attr("checked");
        $('#dvIsActive').css({ "display": "block" });
        if (chkValue == "checked")
            $("#IsActive").prop("checked", true);
        else
            $("#IsActive").prop("checked", false);

        $('#btnUserSubmit').val("Update");
    });
    //$('#btnUserSubmit').val("Submit");
    //$('#ID').val('');
    //$('#EmpID').val('');
    //$('#EmpUserID').val('');
    //$('#EmpName').val('');
    //$('#Email').val('');
    //$('#dvStatus').css({ "display": "none" });

    $("#btnReset").click(function () {
        $('#btnUserSubmit').val("Submit");
        $('#ID').val('');
        $('#EmpID').val('');
        $('#EmpUserID').val('');
        $('#EmpName').val('');
        $('#Email').val('');
        $('#dvIsActive').css({ "display": "none" });
    });
});
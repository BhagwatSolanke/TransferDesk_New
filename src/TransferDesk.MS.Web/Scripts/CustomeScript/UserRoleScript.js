$(document).ready(function () {
    $('#dvIsActive').css({ "display": "none" });
    $(".editButton").click(function () {
        $('#ID').val($(this).closest("tr").find("td").eq(0).text());
        $('#UserID').val($(this).closest("tr").find("td").eq(1).text());
        $('#UserID').val($.trim($('#UserID').val()));
        $("#RollID option:contains(" + $(this).closest("tr").find("td").eq(2).text() + ")").attr('selected', 'selected');
        var chkValue = $(this).closest("tr").children("td").find("input[type='checkbox']").attr("checked");
        $('#dvIsActive').css({ "display": "block" });
        if (chkValue == "checked")
            $("#IsActive").prop("checked", true);   
        else
            $("#IsActive").prop("checked", false);

        $('#btnUserRoleAdd').val("Update");
    });   

    $("#btnReset").click(function () {
        $('#btnUserRoleAdd').val("Add");
        $('#ID').val('');
        $('#UserID').val('');
        $('#RollID option[value="1"]').attr("selected", true);
        $('#dvIsActive').css({ "display": "none" });
    });
   
});
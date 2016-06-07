
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


    $('#btnOk').click(function () {
        var rowindex = $("input[name='rbtnCount']:checked").closest("tr").index();
        var ID = $("#ID_" + (rowindex - 1)).val();
        $("#dbID").val(ID);
        $('#myModal').modal('hide');
        if (ID == 0 || ID == '' || ID == null) {
            var url = AppPath + 'Manuscript/HomePage';
        } else {
            var url = AppPath + 'Manuscript/HomePage/' + ID;
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
        var URL = AppPath + "Manuscript/GetSearchResult";
        loadMSDetailsModal(URL, selectedValue, searchText);
    });//submit action end

    $('#btnNewAdd').click(ResetAllControls);

    $('#QualityStartCheckDate').datepicker({ dateFormat: 'dd/mm/yy' });

    $('#ReceivedDate').datepicker({ dateFormat: 'dd/mm/yy', maxDate: '0' });

    if ($("#PageCount").val() == 0) {
        $("#PageCount").val('');
    }

    //if quality role is selected display quality view
    $("#ddlRole").change(function () {
        IsQualityRole();
        IsViewEditable();
    });

    $('#StartDate').prop("readonly", true);


    $("#btnAssociateSave,#btnAssociateIsFinalSubmit, #btnQualitySave,#btnIsQualityFinalSubmit").click(function () {
        //ImageDropdownValidation();
        var iThenticateResult = $('#hdniThenticateResult1').val();
        var EnglishlangQualityId = $('#hdnEnglishlangQuality1').val();
        var EthicsComplienceId = $('#hdnEthicsComplience1').val();
        if (iThenticateResult == 0) {
            alert("Please,Select Cross Check / iThenticate Result");
            return false;
        }
        else if (EnglishlangQualityId == 0) {
            alert("Please, Select English Language Quality");
            return false;
        }
        else if (EthicsComplienceId == 0) {
            alert("Please, Select Ethics Compliance")
            return false;
        }
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

    //when first time page load
    IsQualityRole();

    //when first time page load
    IsQualityCheck();

    $("#btnIsQualityFinalSubmit").click(function () {
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
                    if (!(jQuery.trim($("#ErrorDescription").val()).length > 0)) {
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

function ResetAllControls() {
    var url = AppPath + 'Manuscript/BookScreening';
    window.location.href = url;
}







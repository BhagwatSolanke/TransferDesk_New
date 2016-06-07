$(document).ready(function () {
    var crestId = null, serviceType = null, jobProcessingStatus = null, role = null;
    $("#btnAllocateID").click(function () {
        if (crestId == null) {
            alert("Select a MSID.");
        }
        else if (status == "On Hold") {
            var crestid = $("#CrestIdVM").val();
            var serviceType = $("#ServiceTypeVM").val();
            $("#AssociateNamesModal").modal();
            $.ajax(
               {
                   method: "GET",
                   url: AppPath + 'AdminDashBoard/GetLastAssociateName',
                   data: { crestid: crestid, servicetype: serviceType },
                   success: function (data) {
                       $("#AssociateName").val(data);
                   },
                   error: function (xhr, exception) {
                       alert('No associate name found');
                   }
               });
            $(menu[0].children[0]).trigger("click");
        }
        else {
            $("#AssociateName").val('');
            $("#AssociateNamesModal").modal();
        }


    });

    $("tr.grid-row").click(function (event) {
        crestId = ($(this).closest("tr").find("td").eq(1).text()).trim();
        serviceType = ($(this).closest("tr").find("td").eq(3).text()).trim();
        jobProcessingStatus = ($(this).closest("tr").find("td").eq(9).text()).trim();
        role = ($(this).closest("tr").find("td").eq(8).text()).trim();
        status = ($(this).closest("tr").find("td").eq(9).text()).trim();
        $("#CrestIdVM").val(crestId);
        $("#ServiceTypeVM").val(serviceType);
        $("#JobProcessingStatusVM").val(jobProcessingStatus);
        $("#RoleVM").val(role);
        $("#StatusVm").val(status);
        var selected = $(this).hasClass("highlight");
        $("table.table.table-striped.grid-table tr").removeClass("highlight");
        if (!selected)
            $(this).addClass("highlight");
    });



    $("#AssociateName").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: AppPath + 'AdminDashBoard/GetAssociateName',
                data: {
                    searchAssociate: request.term,
                    RoleName: role
                },
                dataType: "json",
                type: "GET",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    response($.map(data, function (item) {
                        return {
                            label: item.empname,
                            val: item.UserID
                        }
                    }))
                },
                error: function (response) {
                    //alert("Please,");
                },
                failure: function (response) {
                    //alert("Please,select journal title");
                }
            });
        },
        appendTo: $('.modal-body'),
        minLength: 1
    });


    $("#btnOk").click(function () {
        if ($("#AssociateName").val() == "") {
            alert("Select associate.");
            return false;
        }
        else {
            $("#AssociateNamesModal").modal('hide');
        }
        var crestid = $("#CrestIdVM").val();
        var associateName = $("#AssociateName").val();
        var serviceType = $("#ServiceTypeVM").val();
        var jobProcessingStatus = $("#JobProcessingStatusVM").val();
        var role = $("#RoleVM").val();
        //  window.location.href = AppPath + 'AdminDashboard/AdminActionResult?AssociateNameVM=' + associateName + "&CrestIdVM=" + crestid + "&ServiceTypeVM=" + serviceType + "&JobProcessingStatusVM=" + jobProcessingStatus + "&RoleVM=" + role;
        $.ajax(
        {
            method: "GET",
            url: AppPath + 'AdminDashboard/AdminActionResult',
            data: {
                AssociateNameVM: associateName,
                CrestIdVM: crestid,
                ServiceTypeVM: serviceType,
                JobProcessingStatusVM: jobProcessingStatus,
                RoleVM: role

            },
            contentType: "application/json; charset=utf-8",
            success: function (returnValue) {
                if (returnValue.toLowerCase() == "true") {
                    alert("Job is allocated to associate.");
                    location.reload(true);
                } else {
                    alert("Job is already allocated to associate.");
                    location.reload(true);
                }
            },
            error: function (xhr, exception) {
                //alert(xhr);
            }
        });
    });

    $("#btnExporttoExcel").click(function () {
        $("#myModal").modal();
        $('.datepicker').datepicker({ dateFormat: 'dd/mm/yy', maxDate: '0' }).datepicker("setDate", new Date());
    });

    $('.datepicker').datepicker({ dateFormat: 'dd/mm/yy' ,maxDate: '0'}).datepicker("setDate", new Date());

    $('#myModal').on('hidden.bs.modal', function () {
        $(this).find("input,textarea,select").val('').end();

    });

   
    //$('#myModal').on('shown.bs.modal', function () {
    //    $("#btnExportOk").click(function () {
    //        var FromDateValue = $.datepicker.parseDate("dd/mm/yy", $("#FromDate").val());
    //        var ToDateValue = $.datepicker.parseDate("dd/mm/yy", $("#ToDate").val());
    //        if (FromDateValue > ToDateValue) {
    //            alert("From date cannot be greater then To Date.");
    //            return false;
    //        }
    //        if (FromDateValue == "" && ToDateValue == "") {
    //            alert('Please select Dates');
    //            return false;
    //        }
    //        if (FromDateValue == "") {
    //            alert('Please select From Date');
    //            return false;
    //        }
    //        else if (ToDateValue == "") {
    //            alert('Please select To Date');
    //            return false;
    //        }
    //        window.location.href = AppPath + "ManuscriptLogin/ManuscriptLoginExportResult?FromDate=" + $("#FromDate").val() + "&ToDate=" + $("#ToDate").val();
    //        $("#myModal").modal('hide');
    //    });
    //});


    $('#myModal').on('shown.bs.modal', function () {
    $("#btnExportOk").click(function () {
        var FromDateValue = $.datepicker.parseDate("dd/mm/yy", $("#FromDate").val());
        var ToDateValue = $.datepicker.parseDate("dd/mm/yy", $("#ToDate").val());

        if (FromDateValue == null && ToDateValue == null) {
            alert('Please select Dates');
            return false;
        }
        if (FromDateValue == null) {
            alert('Please select From Date');
            return false;
        }
        else if (ToDateValue == null) {
            alert('Please select To Date');
            return false;
        }

        if (FromDateValue > ToDateValue) {
            alert("From date cannot be greater then To Date.");
            return false;
        }
            
        window.location.href = AppPath + "AdminDashboard/AdminDashBoardExportToExcelData?FromDate=" + $("#FromDate").val() + "&ToDate=" + $("#ToDate").val();
        $("#myModal").modal('hide');
    });
    });

    $('#AssociateNamesModal').on('shown.bs.modal', function () {
        $('#AssociateName').focus();
    });

    $("#btnUnAllocateID").click(function () {
        if (crestId == null) {
            alert("Select a MSID.");
            return false;
        } else {
            var crestid = $("#CrestIdVM").val();
            var associateName = $("#AssociateName").val();
            var serviceType = $("#ServiceTypeVM").val();
            var jobProcessingStatus = $("#JobProcessingStatusVM").val();
            var role = $("#RoleVM").val();
            $.ajax(
            {
                method: "GET",
                url: AppPath + 'AdminDashboard/AdminUnallocateMSID',
                data: {
                    AssociateNameVM: associateName,
                    CrestIdVM: crestid,
                    ServiceTypeVM: serviceType,
                    JobProcessingStatusVM: jobProcessingStatus,
                    RoleVM: role
                },
                contentType: "application/json; charset=utf-8",
                success: function (returnValue) {
                    if (returnValue.toLowerCase() == "true") {
                        alert("Job is unallocated from associate.");
                        window.location.reload(true);
                    } else {
                        alert("Job is not allocated to any associate.");
                        location.reload(true);
                    }
                },
                error: function (xhr, exception) {
                }
            });
        }
    });


    $("#btnHoldId").click(function () {

        var associateName1 = $("#AssociateNameVM").val();


        if (crestId == null) {
            alert("Select a MSID.");
            return false;
        }
        else {
            var crestid = $("#CrestIdVM").val();
            var associateName = associateName1;
            var jobProcessingStatus = $("#JobProcessingStatusVM").val();
            var role = $("#RoleVM").val();
            $.ajax(
            {
                method: "GET",
                url: AppPath + 'AdminDashboard/AdminHoldMSID',
                data: {
                    AssociateNameVM: associateName,
                    CrestIdVM: crestid,
                    ServiceTypeVM: serviceType,
                    JobProcessingStatusVM: jobProcessingStatus,
                    RoleVM: role
                },
                contentType: "application/json; charset=utf-8",
                success: function (returnValue) {
                    if (returnValue.toLowerCase() == "true") {
                        alert("Job is on hold");
                        window.location.reload(true);
                    } else {
                        alert("Job is already on hold.");
                        location.reload(true);
                    }
                },
                error: function (xhr, exception) {
                }
            });
        }
    });

});
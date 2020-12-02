
var id, employeeid, organisationid;
$(document).ready(function () {

    CheckDate();
    $('#NepaliDateFrom').val(AD2BS($('#DateFrom').val()));
    $('#NepaliDateTo').val(AD2BS($('#DateTo').val()));

  $(".create").off().on('click', function (e) {
        $("#hide").fadeIn(500);
        $("#show").hide();
        $("#calculateremainingleavehide").show();
        InitialDate();
    });
    $(".cancel").off().on('click', function (e) {

        $("#hide").hide();
        $("#show").fadeIn(500);
        ResetFormData();
        InitialDate();

    });
    $("#leavelist").show();
    $('#NepaliDateFrom').nepaliDatePicker({
        ndpEnglishInput: 'DateFrom',
        npdMonth: true,
        npdYear: true,
        npdYearCount: -25
    });

    $('#DateFrom').change(function () {
        $('#NepaliDateFrom').val(AD2BS($('#DateFrom').val()));
    });

    $('#NepaliDateTo').nepaliDatePicker({
        ndpEnglishInput: 'DateTo',
        npdMonth: true,
        npdYear: true,
        npdYearCount: -25
    });

    $('#DateTo').change(function () {
        $('#NepaliDateTo').val(AD2BS($('#DateFrom').val()));
    });


    //$('#DateFrom').attr("disabled", true);
    //$('#DateTo').attr("disabled", true);

    LoadOrgainsation();
    LoadLeaveDaysmaster();
    $("#OrganisationID").change(function () {
        var organisationid = $("#OrganisationID").val();
        GetOrganisationLeaveType(organisationid);
        GetEmployeeBasedOnOrganisation(organisationid);

    });
    $('#takeleaveSearch').click(function () {
        $("#TakeAccumulativeLeaveListGrid").data("kendoGrid").dataSource.read();

    });
    $("#OrganisationIDSearch").change(function () {
        var organisationid = $("#OrganisationIDSearch").val();
        GetOrganisationLeaveTypeSearch(organisationid);
        GetEmployeeBasedOnOrganisation(organisationid,employeeid);

    });
       
    $("#takeLeaveCancel").off().on('click', function () {
        ResetFormData();
        $("#create").hide();
        $("#leavelist").show();

    });



    $("#LeaveDaysID").change(function () {
        var datavalue = $(this).find(':selected').attr('data-val');
        $("#Days").attr('disabled','disabled')
        var datefrom = $('#DateFrom').val().split("-");
        var dateto = $('#DateTo').val().split("-");

        if ($("#Days").val() == 0)
        {
            ShowMessage("Warning !! You dont have any leave left", false);
            return false;
        }
        if (datefrom == "" || dateto == "") {
            ShowMessage("Warning !! Please Enter DateFrom and DateTo",false);
            return false;
        }

        var d1 = new Date(datefrom);
        var d2 = new Date(dateto);
        isWeekend = false;
        var i = 1;
        while (d1 < d2) {
            var day = d1.getDay();
            isWeekend = (day === 6)||(day===0);
            if (isWeekend) {
                if (confirm("It Contains Saturday And Sunday")) { //      
                    i = i - 1;
                }

            } // return immediately if weekend found
            d1.setDate(d1.getDate() + 1);
            i++;
        }

        $('#Days').val(i * datavalue);
        var totaldays = $('#Days').val();
        if (totaldays > parseFloat($('#RemainingLeave').val())) {
            ShowMessage("Warning!! You Cannot Take " + totaldays + "  Leave",false);
            $('#Days').val("");
            return false;
        }
        else
        {            
            $('#RemainingLeave').val(parseFloat($('#RemainingLeave').val()) - parseFloat($('#Days').val()));
        }


    });


    $("#Show").off().on('click', function (e) {

        $("#leavelist").hide();
        $("#create").show();


    });

    $("#takeLeaveSubmit").off().on('click', function (e) {

        if (!$('form#formTakeLeave').data('unobtrusiveValidation').validate()) {
            e.preventDefault();
            return false;
        } else if ($('#Days').val() == "" || parseFloat($('#RemainingLeave').val() == "")) {
            ShowMessage("Warning!! You Cannot Take Leave",false);
            $('#Days').val("");
            return false;
        }
        else {
            $.ajax({
                url: "/Admin/TakeAccumulativeLeave/SaveTakeAccumulativeLeave",
                type: 'POST',
                data: AddAntiForgeryToken({
                    ID: $('#ID').val(),
                    OrganisationID: $('#OrganisationID').val(),
                    EmployeeID: $('#EmployeeID').val(),
                    LeaveTypeID: $('#LeaveTypeID').val(),
                    NepaliDateFrom: $('#NepaliDateFrom').val(),
                    NepaliDateTo: $('#NepaliDateTo').val(),
                    DateFrom: $('#DateFrom').val(),
                    DateTo: $('#DateTo').val(),
                    Year: $('#Year').val(),
                    Month: $('#Month').val(),
                    Days: $('#Days').val(),
                    LeaveDaysID: $('#LeaveDaysID').val(),
                    RemainingLeave: $('#RemainingLeave').val()
                }),
                dataType: 'json',
                success: function (data) {
                    ResetFormData();
                    ShowMessage(data.Message,false);
                    $("#hide").hide();
                    $("#show").fadeIn(500);
                    $("#TakeAccumulativeLeaveListGrid").data("kendoGrid").dataSource.read();

                },
                error: function () {
                    ShowMessage("Warning !! Error Occured",false);
                }
            })
        }
    });

    $("#calculateremainingleave").off().on('click', function (e) {

        $.ajax({
            url: "/Admin/TakeAccumulativeLeave/CalculateRemainingLeave",
            type: 'POST',
            data: {
                OrganisationID: $('#OrganisationID').val(),
                EmployeeID: $('#EmployeeID').val(),
                LeaveTypeID: $('#LeaveTypeID').val(),
                Year: $('#Year').val(),
            },
            dataType: 'json',
            global: false,
            success: function (data) {

                $('#RemainingLeave').val(parseFloat(data));
                $('#RemainingLeave').attr('disabled','disabled');



            },
            error: function () {
                ShowMessage("Warning !! Error Occured",false);
            }
        })

    });

    $(".ok").off().on('click', function (e) {
        ApproveLeave(id, employeeid, organisationid);

    });



});



function ParamToTakeLeaveList(e) {
    var grid = $("#TakeAccumulativeLeaveListGrid").data("kendoGrid").dataSource;
    return {
        OrganisationIDSearch: $("#OrganisationIDSearch  :selected").val() == "" ? -1 : $("#OrganisationIDSearch :selected").val(),
        LeaveTypeIDsearch: $("#LeaveTypeIDsearch :selected").val() == "" ? -1 : $("#LeaveTypeIDsearch :selected").val(),
        EmployerIDSearch: $("#EmployerIDSearch :selected").val() == "" ? -1 : $("#EmployerIDSearch :selected").val(),
        YearSearch: $("#YearSearch").val() == "" ? -1 : $("#YearSearch").val(),
        MonthSearch: $("#MonthSearch").val() == "" ? -1 : $("#MonthSearch").val(),
        pageSize: grid._pageSize,
        pageNumber: grid._page

    };

}
function GetOrganisationLeaveType(organisation, leavetype) {

    $("#LeaveTypeID").empty();
    $("#LeaveTypeID").append('<option value>--Select--</option>')
    $.ajax({
        url: "/Admin/TakeLeave/GetAccumulativeLeaveTypeBasedOnOrganisation",
        type: 'POST',
        dataType: 'json',
        data: {
            ID: organisation,
        },
        global: false,
        async: true,
        success: function (data) {

            jQuery.each(data, function (index, value) {
                if (value.ID == leavetype) {
                    $("#LeaveTypeID").append('<option selected value=' + value.ID + '>' + value.LeaveTypeName + '</option>')
                }
                else {
                    $("#LeaveTypeID").append('<option value=' + value.ID + '>' + value.LeaveTypeName + '</option>')
                }

            })

        },

        error: function (response) {

            ShowMessage("Warning! Error Occured",true);  //
        }



    })

}
function GetOrganisationLeaveTypeSearch(organisation) {

    $("#LeaveTypeIDsearch").empty();
    $("#LeaveTypeIDsearch").append('<option value>--LeaveType--</option>')
    $.ajax({
        url: "/Admin/TakeLeave/GetAccumulativeLeaveTypeBasedOnOrganisation",
        type: 'POST',
        dataType: 'json',
        data: {
            ID: organisation,
        },
        global: false,
        async: true,
        success: function (data) {

            jQuery.each(data, function (index, value) {
                $("#LeaveTypeIDsearch").append('<option value=' + value.ID + '>' + value.LeaveTypeName + '</option>')
            })

        },

        error: function (response) {

            ShowMessage("Warning! Error Occured",false);  //
        }



    })

}



function GetEmployeeBasedOnOrganisation(organisation,employeeid) {

    $("#EmployeeID").empty();
    $("#EmployeeID").append('<option value>--Select--</option>')
    $("#EmployerIDSearch").empty();
    $("#EmployerIDSearch").append('<option value>--Employee--</option>')
    $.ajax({
        url: "/Admin/TakeLeave/GetEmployeeBaesdOrganisation",
        type: 'POST',
        dataType: 'json',
        data: {
            OrganisationID: organisation,

        },
        global: false,
        async: true,
        success: function (data) {

            jQuery.each(data, function (index, value) {
                if (value.ID == employeeid) {
                    $("#EmployeeID").append('<option selected value=' + value.ID + '>' + value.Name + '</option>')
                }
                else {
                    $("#EmployeeID").append('<option value=' + value.ID + '>' + value.Name + '</option>')
                }
               
                $("#EmployerIDSearch").append('<option value=' + value.ID + '>' + value.Name + '</option>')


            })

        },

        error: function (response) {

            ShowMessage("Warning! Error Occured",false);  //
        }



    })

}

function Delete(e) {
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    e.preventDefault();
    $("#window").kendoWindow({
        modal: true
    });
    $("#window").data("kendoWindow").open().center();

    $("#yes").off().on('click', function (e) {

        $.ajax({
            url: "/Admin/TakeAccumulativeLeave/DeleteTakeaccumulativeleave",
            data: { id: dataItem.ID },
            type: 'POST',
            dataType: 'json',
            success: function (result) {
                $("#window").data("kendoWindow").close();
                ShowMessage(result.Message,true);
                ResetFormData();
                $("#TakeAccumulativeLeaveListGrid").data("kendoGrid").dataSource.read();

            },
            error: function () {
                ShowMessage("Warning!! Error Occured",false);
            }


        });

    });

    $("#no").off().on('click', function (e) {

        $("#window").data("kendoWindow").close();
    });

}
function Edit(e) {
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    e.preventDefault();

    $.ajax({
        url: "/Admin/TakeLeave/EditTakeLeave",
        data: { id: dataItem.ID },
        type: "POST",
        dataType: "json",

        success: function (result) {

            $("#hide").fadeIn(500);
            $("#show").hide();
            $("#ID").val(result.ID);
            $("#OrganisationID").val(result.OrganisationID);
            $("#LeaveDaysID").val(result.LeaveDaysID);
            GetOrganisationLeaveType(result.OrganisationID, result.LeaveTypeID);
            GetEmployeeBasedOnOrganisation(result.OrganisationID,result.EmployeeID)
            $("#DateFrom").val(ConvertDateObjectToDate(result.DateFrom));
            $("#NepaliDateFrom").val(ConvertDateObjectToDate1(result.NepaliDateFrom));
            $("#DateTo").val(ConvertDateObjectToDate(result.DateTo));
            $("#NepaliDateTo").val(ConvertDateObjectToDate1(result.NepaliDateTo));            
            $("#Year").val(result.Year);
            $("#Month").val(result.Month);
            $("#Days").attr("disabled","disabledd").val(result.Days);          
            CalculateRemainingLeave(result.OrganisationID, result.EmployeeID, result.LeaveTypeID, result.Year,result.Days)
            $("#calculateremainingleavehide").hide();

        },

        error: function (result) {

            ShowMessage('Warning !! Error Occured',false);
        }
    });

}


function Details(e) {

    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));

    e.preventDefault();
    $('#customPopupDialogDescription').modal('show');
    $("#organisation").text(dataItem.Organisation);
    $("#leavetype").text(dataItem.LeaveTypeName);
    $("#requestedby").text(dataItem.EmployeeName);

    $("#year").text(dataItem.Years);
    $("#month").text(dataItem.Months);
    $("#days").text(dataItem.Days);
    $("#statusid").text(dataItem.Statuss);
    $("#datefrom").text(new Date(dataItem.DateFrom).toLocaleDateString());
    $("#dateto").text(new Date(dataItem.DateTo).toLocaleDateString());





}

function LoadOrgainsation() {

    $("#OrganisationID").empty();
    $("#OrganisationID").append('<option value>--Organisation--</option>')

    $("#OrganisationIDSearch").empty();
    $("#OrganisationIDSearch").append('<option value>--Organisation--</option>')
    $.ajax({
        url: "/Admin/DropDown/GetOrganisationDropDown",
        type: 'POST',
        dataType: 'json',
        global: false,
        async: true,
        success: function (data) {
            jQuery.each(data, function (index, value) {
                $("#OrganisationID").append('<option value=' + value.ID + '>' + value.Name + '</option>')

                $("#OrganisationIDSearch").append('<option value=' + value.ID + '>' + value.Name + '</option>')
            })

        },

        error: function (response) {

            ShowMessage("Warning! Error Occured",false);  //
        }



    })

}

function LoadLeaveDaysmaster() {
    $("#LeaveDaysID").empty();
    $("#LeaveDaysID").append('<option value>--Select--</option>')
    $.ajax({
        url: "/Admin/DropDown/GetLeaveDaysMaster",
        type: 'POST',
        dataType: 'json',
        async: true,
        global: false,
        success: function (data) {
            jQuery.each(data, function (index, value) {
                $("#LeaveDaysID").append('<option data-val=' + value.DataValue + ' value=' + value.ID + '>' + value.Name + '</option>')
            })

        },

        error: function (response) {

            ShowMessage("Warning! Error Occured",false);  //
        }



    })

}

function resetRowNumberTakeLeave(e) {

    $(".k-grid-Edit").find("span").addClass("fa fa-pencil");
    $(".k-grid-Edit").removeClass("k-button");
    $(".k-grid-Delete").find("span").addClass("fa fa-trash");
    $(".k-grid-Delete").removeClass("k-button");
    $(".k-grid-Details").find("span").addClass("fa fa-eye");
    $(".k-grid-Details").removeClass("k-button");
    $(".k-grid-Approve").find("span").addClass("fa fa-check");
    $(".k-grid-Approve").removeClass("k-button");
    $(".k-grid-Details").find("span").addClass("fa fa-eye");
    $(".k-grid-Details").removeClass("k-button");

    $("#TakeLeaveListGrid tbody tr .k-grid-Approve").each(function () {
        var currentDataItem = $("#TakeLeaveListGrid").data("kendoGrid").dataItem($(this).closest("tr"));

        //Check in the current dataItem if the row is deletable
        if (currentDataItem.Statuss == "Approved") {
            $(this).find("span").addClass("fa fa-check");
        }
        else {
            $(this).find("span").addClass("fa fa-ban");
        }
    })

    var grid = e.sender;
    if (grid.dataSource.total() == 0) {
        var colCount = 0;
        var columns = grid.columns;
        jQuery.each(columns, function (index) {
            if (!this.hidden) {
                colCount++;
            }
        });
        $(e.sender.wrapper)
            .find('tbody')
            .append('<tr class="kendo-data-row text-center"><td colspan="' + colCount + '" class="no-data">No Data Available</td></tr>');
    }
}

ConvertDateObjectToDate = function (dateObject) {
    var dateString = dateObject.substr(6);
    var currentTime = new Date(parseInt(dateString));
    var month = currentTime.getMonth() + 1;
    var day = currentTime.getDate();
    var year = currentTime.getFullYear();
    var date = month + "-" + day + "-" + year;
    return date;
};

ConvertDateObjectToDate1 = function (dateObject) {

    var dateString = dateObject.substr(6);
    var currentTime = new Date(parseInt(dateString));
    var month = currentTime.getMonth() + 1;
    var day = currentTime.getDate();
    var year = currentTime.getFullYear();
    var date = year + "-" + month + "-" + day;
    return date;
};

function CalculateRemainingLeave(OrganisationID, EmployeeID, LeaveTypeID, Year,Days) {
    $.ajax({
        url: "/Admin/TakeAccumulativeLeave/CalculateRemainingLeave",
        type: 'POST',
        data: {
            OrganisationID: OrganisationID,
            EmployeeID: EmployeeID,
            LeaveTypeID: LeaveTypeID,
            Year: Year           
        },
        dataType: 'json',
        global: false,
        success: function (data) {

            
            $('#RemainingLeave').val(parseFloat(data) + parseFloat(Days));
            $("#RemainingLeave").attr("disabled", "disabled");


        },
        error: function () {
            ShowMessage("Warning !! Error Occured",false);
        }
    })
}



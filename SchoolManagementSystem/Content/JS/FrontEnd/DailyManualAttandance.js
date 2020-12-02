
var id, employeeid, organisationid;
$(document).ready(function ()
{
    CheckDate();
    $(".create").off().on('click', function (e) {
        $("#hide").fadeIn(500);
        $("#show").hide();
        InitialDate();
    })
    $(".Cancel").off().on('click', function (e) {
        $("#hide").hide();
        $("#show").fadeIn(500);
        $('#OrganisationID').prop('selectedIndex', 0);
        InitialDate();
    });

    $("#Days").attr("disabled", "disabled");   
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
        debugger;
        $('#NepaliDateTo').val(AD2BS($('#DateTo').val()));
    });

   

    LoadOrgainsation();
    LoadLeaveDaysmaster();
    $("#OrganisationID").change(function () {
        var organisationid = $("#OrganisationID").val();
        GetEmployeeBasedOnOrganisation(organisationid);

    });
    $('#takeleaveSearch').click(function () {
        $("#DailyManualAttandanceListGrid").data("kendoGrid").dataSource.read();

    });
    $("#OrganisationIDSearch").change(function () {
        var organisationid = $("#OrganisationIDSearch").val();
        GetEmployeeBasedOnOrganisation(organisationid);

    });    
   


    $("#LeaveDaysID").change(function () {
        var datavalue = $(this).find(':selected').attr('data-val');
        var datefrom = $('#DateFrom').val().split("-");
        var dateto = $('#DateTo').val().split("-");
        if (datefrom == "" || dateto == "") {
            ShowMessage("Warning !! Please Enter DateFrom and DateTo");
            return false;
        }

        var d1 = new Date(datefrom);
        var d2 = new Date(dateto);
        isWeekend = false;
        var i = 1;
        while (d1 < d2) {
            var day = d1.getDay();
            isWeekend = (day === 6);
            if (isWeekend) {
                if (confirm("It Contains Saturday")) { //      
                    i = i - 1;
                }

            } // return immediately if weekend found
            d1.setDate(d1.getDate() + 1);
            i++;
        }

        $('#Days').val(i * datavalue);
        var totaldays = $('#Days').val();
        if (totaldays > parseFloat(1)) {
            ShowMessage("Warning!! Attandance Cannot Be greater than 1 days");
            $('#Days').val("");
            return false;
        }


    });


 


    $("#EmployeeDailyAttandanceSave").off().on('click', function (e) {

        if (!$('form#formEmployeeDailyAttandanceEntry').data('unobtrusiveValidation').validate()) {
            e.preventDefault();
            return false;
        }
        else {
            $.ajax({
                url: "/Client/DailyManualAttandance/SaveDailyManualAttandance",
                type: 'POST',
                data: AddAntiForgeryToken({
                    ID: $('#ID').val(),
                    OrganisationID: $('#OrganisationID').val(),                 
                    DateFrom: $('#DateFrom').val(),
                    DateTo: $('#DateTo').val(),
                    NepaliDateFrom: $('#NepaliDateFrom').val(),
                    NepaliDateTo: $('#NepaliDateTo').val(),
                    Year: $('#Year').val(),
                    Month: $('#Month').val(),
                    Days: $('#Days').val(),
                    LeaveDaysID: $('#LeaveDaysID').val()
                  
                }),
                dataType: 'json',
                success: function (data) {                   
                    ShowMessage(data.Message,true);                
                  

                },
                error: function () {
                    ResetFormData();
                    ShowMessage("Warning !! Error Occured",true);
                }
            })
        }
    });





});

function LineItems_Databound(status) {
    if (status == "Absent") {
        return "<div style='background: #e54040;text-align:center;color:white'>" + status + " </div>";
    }
    else if (status == "Present") {
        return "<div style='background:#52e540;text-align:center;color:white'>" + status + " </div>";
    }

}
function AddAntiForgeryToken(data) {
    data.__RequestVerificationToken = $('form input[name=__RequestVerificationToken]').val();
    return data;
};
function ParamToDailyManualAttandanceList(e) {
    var grid = $("#DailyManualAttandanceListGrid").data("kendoGrid").dataSource;
    return {
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
        url: "/Admin/OfficialLeave/GetLeaveTypeBasedOnOrganisation",
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

            ShowMessage("Warning! Error Occured");  //
        }



    })

}


function GetEmployeeBasedOnOrganisationandLeaveType(organisation, leavetypeid, employeeid) {

    $("#EmployeeID").empty();
    $("#EmployeeID").append('<option value>--Select--</option>')
    $.ajax({
        url: "/Admin/TakeLeave/GetEmployeeBaesdOnLeaveTypeAndOrganisation",
        type: 'POST',
        dataType: 'json',
        data: {
            OrganisationID: organisation,
            LeaveTypeID: leavetypeid
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
            })

        },

        error: function (response) {

            ShowMessage("Warning! Error Occured");  //
        }



    })

}

function ShowMessage(message, event) {
    if (event == true) {
        toastr.info(message, "Information", { timeOut: 2000 });
    }
    if (event == false) {
        toastr.info(message, "Information", { timeOut: 2000 });
    }

}
function GetEmployeeBasedOnOrganisation(organisation, employee) {


    $("#EmployeeID").empty();
    $("#EmployeeID").append('<option value>--Select--</option>')

    $("#EmployerIDSearch").empty();
    $("#EmployerIDSearch").append('<option value>--Employer--</option>')
    $.ajax({
        url: "/Admin/OfficialLeave/GetEmployeeBaesdOrganisation",
        type: 'POST',
        dataType: 'json',
        data: {
            OrganisationID: organisation,

        },
        global: false,
        async: true,
        success: function (data) {

            jQuery.each(data, function (index, value) {
                if (value.ID == employee) {

                    $("#EmployeeID").append('<option selected value=' + value.ID + '>' + value.Name + '</option>')
                }
                else {
                    $("#EmployeeID").append('<option value=' + value.ID + '>' + value.Name + '</option>')
                }

                $("#EmployerIDSearch").append('<option value=' + value.ID + '>' + value.Name + '</option>')


            })

        },

        error: function (response) {

            ShowMessage("Warning! Error Occured");  //
        }



    })

}

function LoadOrgainsation() {

    $("#OrganisationID").empty();  

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

            ShowMessage("Warning! Error Occured");  //
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

            ShowMessage("Warning! Error Occured");  //
        }



    })

}

function resetRowNumberDailyManualAttandance(e) {
     


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






var id, employeeid, organisationid;
$(document).ready(function () {

    CheckDate();
    $('#NepaliDateFrom').val(AD2BS($('#DateFrom').val()));
    $('#NepaliDateTo').val(AD2BS($('#DateTo').val()));


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
    $("#list").show();   

    LoadOrgainsation();
    LoadLeaveDaysmaster();
    LoadEmployee();
    LoadAdminAndSuperAdmin();
          

    
    $('#takeleaveSearch').click(function () {
        $("#TakeAttandanceGridClientList").data("kendoGrid").dataSource.read();

    });
    $("#OrganisationIDSearch").change(function () {
        var organisationid = $("#OrganisationIDSearch").val();
        GetEmployeeBasedOnOrganisation(organisationid);

    });

    $("#takeLeaveCancel").off().on('click', function () {
        ResetFormData();
        $("#create").hide();
        $("#list").show();
        InitialDate();

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
        if (totaldays > parseFloat($('#RemainingLeave').val())) {
            ShowMessage("Warning!! You Cannot Take " + totaldays + "  Leave");
            $('#Days').val("");
            return false;
        }


    });


    $("#Show").off().on('click', function (e) {

        $("#list").hide();
        $("#create").show();
        InitialDate();


    });

    $("#takeLeaveSubmit").off().on('click', function (e) {

        if (!$('form#formTakeLeave').data('unobtrusiveValidation').validate()) {
            e.preventDefault();
            return false;
        } 
        else {
            $.ajax({
                url: "/Client/TravellRequest/SaveTravellRequest",
                type: 'POST',
                data: AddAntiForgeryToken({
                    ID: $('#ID').val(),
                    OrganisationID: $('#OrganisationID').val(),
                    EmployeeID: $('#EmployeeID').val(),
                    LeaveTypeID: $('#LeaveTypeID').val(),                   
                    DateFrom: $('#DateFrom').val(),
                    DateTo: $('#DateTo').val(),
                    Year: $('#Year').val(),
                    Month: $('#Month').val(),
                    Days: $('#Days').val(),
                    Description: $('#Description').val(),
                    LeaveDaysID: $('#LeaveDaysID').val(),
                    ApprovedBy: $('#ApprovedBy').val(),
                    NotificationType: $('#NotificationType').val()
                }),
                dataType: 'json',
                success: function (data) {
                    ResetFormData();
                    ShowMessage(data.Message,true);
                    $("#list").show();
                    $("#create").hide();
                    $("#TakeAttandanceGridClientList").data("kendoGrid").dataSource.read();

                },
                error: function () {
                    ShowMessage("Warning !! Error Occured");
                }
            })
        }
    });



    $("#calculateremainingleave").off().on('click', function (e) {

        $.ajax({
            url: "/Admin/TakeLeave/CalculateRemainingLeave",
            type: 'POST',
            data: {
                OrganisationID: $('#OrganisationID').val(),
                EmployeeID: $('#EmployeeID').val(),
                LeaveTypeID: $('#LeaveTypeID').val(),
                Year: $('#Year').val(),
                Month: $('#Month').val(),
            },
            dataType: 'json',
            global: false,
            success: function (data) {

                $('#RemainingLeave').val(parseFloat(data));



            },
            error: function () {
                ShowMessage("Warning !! Error Occured");
            }
        })

    });

    $(".ok").off().on('click', function (e) {
        ApproveLeave(id, employeeid, organisationid);

    });



});

function LineItems_Databound(status) {
    if (status == "Rejected") {
        return "<div style='background: #e54040;text-align:center;color:white'>" + status + " </div>";
    }
    else if (status == "Pending") {
        return "<div style='background:#52e540;text-align:center;color:white'>" + status + " </div>";
    }
    else if (status == "Approved") {
        return "<div style='background: skyblue;text-align:center;color:white'>" + status + " </div>";
    }
}

function LoadAdminAndSuperAdmin() {

    $("#ApprovedBy").empty();

    $.ajax({
        url: "/Admin/DropDown/GetSuperAdminAndAdmin",
        type: 'POST',
        dataType: 'json',
        global: false,
        async: true,
        success: function (data) {
            jQuery.each(data, function (index, value) {
                $("#ApprovedBy").append('<option value=' + value.ID + '>' + value.Name + '</option>')


            })

        },

        error: function (response) {

            ShowMessage("Warning! Error Occured");  //
        }



    })

}

function ParamToAttandanceList(e) {
    var grid = $("#TakeAttandanceGridClientList").data("kendoGrid").dataSource;
    return {
        YearSearch: $("#YearSearch").val() == "" ? -1 : $("#YearSearch").val(),
        MonthSearch: $("#MonthSearch").val() == "" ? -1 : $("#MonthSearch").val(),
        StatusSearch: $("#StatusSearch").val() == "" ? -1 : $("#StatusSearch").val(),
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

function LoadEmployee() {
    $("#EmployeeID").empty();    
    $.ajax({
        url: "/Admin/DropDown/GetLoginEmployee",
        type: 'POST',
        dataType: 'json',
        async: true,
        global: false,
        success: function (data) {
            jQuery.each(data, function (index, value) {
                $("#EmployeeID").append('<option value=' + value.ID + '>' + value.Name + '</option>')
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

function Delete(e) {
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    e.preventDefault();
    $("#window").kendoWindow({
        modal: true
    });
    $("#window").data("kendoWindow").open().center();

    $("#yes").off().on('click', function (e) {

        $.ajax({
            url: "/Admin/OfficialLeave/DeleteOfficialLeave",
            data: { id: dataItem.ID },
            type: 'POST',
            dataType: 'json',
            success: function (result) {
                $("#window").data("kendoWindow").close();
                ShowMessage(result.Message);
                ResetFormData();
                $("#OfficialLeaveListGrid").data("kendoGrid").dataSource.read();

            },
            error: function () {
                ShowMessage("Warning!! Error Occured");
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
        url: "/Admin/OfficialLeave/EditOfficialLeave",
        data: { id: dataItem.ID },
        type: "POST",
        dataType: "json",

        success: function (result) {

            $("#create").show();
            $("#list").hide();
            $("#ID").val(result.ID);
            $("#OrganisationID").val(result.OrganisationID);
            GetOrganisationLeaveType(result.OrganisationID, result.LeaveTypeID);
            GetEmployeeBasedOnOrganisation(result.OrganisationID, result.EmployeeID);
            $("#DateFrom").val(ConvertDateObjectToDate(result.DateFrom));
            $("#NepaliDateFrom").val(ConvertDateObjectToDate1(result.NepaliDateFrom));
            $("#DateTo").val(ConvertDateObjectToDate(result.DateTo));
            $("#NepaliDateTo").val(ConvertDateObjectToDate1(result.NepaliDateTo));
            $("#YearID").val(result.YearID);
            $("#MonthID").val(result.MonthID);
            $("#Days").val(result.Days);
            $("#LeaveDaysID").val(result.LeaveDaysID);
            $("#Description").val(result.Description);




        },

        error: function (result) {

            ShowMessage('Warning !! Error Occured');
        }
    });

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
        success: function (data)
        {
            jQuery.each(data, function (index, value) {
                $("#OrganisationID").append('<option value=' + value.ID + '>' + value.Name + '</option>')

                $("#OrganisationIDSearch").append('<option value=' + value.ID + '>' + value.Name + '</option>')
            })
            
            var organisationid = $("#OrganisationID").val();
            GetOrganisationLeaveType(organisationid);
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

function resetRowNumberOfficialLeave(e) {

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

    //$("#TakeAttandanceGridClientList tbody tr .k-grid-Approve").each(function () {
    //    var currentDataItem = $("#TakeAttandanceGridClientList").data("kendoGrid").dataItem($(this).closest("tr"));

    //    //Check in the current dataItem if the row is deletable
    //    if (currentDataItem.Statuss == "Approved") {
    //        $(this).find("span").addClass("fa fa-check");
    //    }
    //    else {
    //        $(this).find("span").addClass("fa fa-ban");
    //    }
    //})

    var rows = e.sender.tbody[0].rows;
    //$(rows).each(function (e) {
    //    var grid = $("#TakeAttandanceGridClientList").data("kendoGrid");
    //    var row = this;
    //    var dataItem = grid.dataItem(row);
    //    //if (dataItem.Statuss == "Approved")
    //    //{
    //    //    grid.tbody.find("tr[data-uid='" + dataItem.uid + "']")
    //    //   .addClass("k-alt k-state-selected gridselect")
    //    //}
    //})


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







var id, employeeid, organisationid;
$(document).ready(function () {

    $("#list").show();
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


    $('#DateFrom').attr("disabled", true);
    $('#DateTo').attr("disabled", true);

    LoadOrgainsation();
    LoadLeaveDaysmaster();

  


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


    $("#Show").off().on('click', function (e) {

        $("#list").hide();
        $("#create").show();


    });

  

    $("#EmployeeDailyAttandanceSearch").off().on('click', function (e) {
        if (!$('form#formEmployeeDailyAttandanceEntry').data('unobtrusiveValidation').validate()) {
            e.preventDefault();
            return false;
        }
        else
        {
            $("#EmployeeDailyAttadanceListGrid").data("kendoGrid").dataSource.read();
            // $("#marksEntrySearch").hide();
        }

    })

    $('#EmployeeDailyAttadanceListGrid').off().on('click', '.chkbx', function () {

        var checked = $(this).is(':checked');
        var grid = $('#EmployeeDailyAttadanceListGrid').data().kendoGrid;
        var dataItem = grid.dataItem($(this).closest('tr'));
        row = $(this).closest("tr");
        checkedIds[dataItem.SN] = checked;
        dataItem.set('IsAttend', checked);

        var view = $("#EmployeeDailyAttadanceListGrid").data("kendoGrid").dataSource.view();
        for (var i = 0; i < view.length; i++) {

            if (checkedIds[view[i].SN]) {
                grid.tbody.find("tr[data-uid='" + view[i].uid + "']")
                .addClass("k-alt k-state-selected")
                .find(".chkbx")
                .attr("checked", "checked");
            }
            else {
                grid.tbody.find("tr[data-uid='" + view[i].uid + "']")
                row.removeClass("k-alt k-state-selected");
            }
        }
    })

    $("#EmployeeDailyAttandanceSave").off().on('click', function (e)
    {
        if (!$('form#formEmployeeDailyAttandanceEntry').data('unobtrusiveValidation').validate())
        {
            e.preventDefault();
            return false;
        }
        else
        {
            if ($("#Days").val() == 0 || $("#Days").val() == "" || $("#Days").val() > 1 || $("#Days").val()==null) {
                ShowMessage("Warning !!! Days ShouldNot Be Greater than 1");
            }

           else if ($("#EmployeeDailyAttadanceListGrid").data("kendoGrid").dataSource.total() == 0)
            {
                ShowMessage("Please Select At Least One Employee");
                e.preventDefault();
                return false;
            }
          
            else
            {
                var dataItem;
                dataItem = $("#EmployeeDailyAttadanceListGrid").data("kendoGrid").dataSource.data();
                debugger;
             
                for (i = 0; i < dataItem.length; i++)
                {
                    firstItem = $('#EmployeeDailyAttadanceListGrid').data().kendoGrid.dataSource.data()[i];
                    if (firstItem["IsAttend"] == false)
                    {
                        firstItem["Days"] = 0;
                    }

                    if (firstItem["IsAttend"] == true)
                    {
                        firstItem["Days"] = 1;
                    }
                                      

                    $('#EmployeeDailyAttadanceListGrid').data('kendoGrid').refresh();
                    // $("#marksEntrySearch").hide();
                }

                e.preventDefault();
                var studentdailylist = $("#EmployeeDailyAttadanceListGrid").data("kendoGrid").dataSource.data();
                $.ajax({
                    url: "/Admin/EmployeeDailyAttandance/SaveEmployeeDailyAttandance",
                    type: 'POST',
                    data: { data: JSON.stringify(studentdailylist), engfrom: $("#DateFrom").val(), nepfrom: $("#NepaliDateFrom").val(), engto: $("#DateTo").val(), nepto: $("#NepaliDateTo").val() },
                    dataType: 'json',
                    success: function (data)
                    {
                        ShowMessage(data.Message);
                        $("#EmployeeDailyAttadanceListGrid").data("kendoGrid").dataSource.read();

                    },
                    error: function (resonse) {
                        ShowMessage("Warning !! Error Occured")
                    }
                })

            }
        }
    })

   



});
function checkAll(ele) {

    var checked = $('.chkSelectAll').prop('checked'), grid = $("#EmployeeDailyAttadanceListGrid").data("kendoGrid");
    for (var i = 0; i < grid.dataSource.data().length; i++) {
        var item = grid.dataSource.data()[i];
        var row = grid.element.find("tr[data-uid='" + item.uid + "']");
        var checkBox = row.find(".chkbx");
        if (!checkBox.prop('checked')) {
            checkBox.trigger("click");
        }
        if (!checked) {
            if (checkBox.prop('checked')) {
                checkBox.trigger("click");
            }
        }
    }
    if (!checked) {
        requisitionid = [];

    }
}
function onError(e, status) {
    ShowMessage('Warning ! Error Occured');
}

function ParamDailyToAttandanceList(e) {
    var grid = $("#EmployeeDailyAttadanceListGrid").data("kendoGrid").dataSource;
    return {
        OrganisationID: $("#OrganisationID :selected").val() == "" ? -1 : $("#OrganisationID :selected").val(),
        Year: $("#Year :selected").val() == "" ? -1 : $("#Year :selected").val(),
        LeaveDaysID: $("#LeaveDaysID :selected").val() == "" ? -1 : $("#LeaveDaysID :selected").val(),
        Month: $("#Month :selected").val() == "" ? "" : $("#Month :selected").val(),        
        NepaliDateFrom: $("#NepaliDateFrom").val() == "" ? "" : $("#NepaliDateFrom").val(),
        DateFrom: $("#DateFrom").val() == "" ? "" : $("#DateFrom").val(),
        NepaliDateTo: $("#NepaliDateTo").val() == "" ? "" : $("#NepaliDateTo").val(),
        DateTo: $("#DateTo").val() == "" ? "" : $("#DateTo").val(),

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




        },

        error: function (result) {

            ShowMessage('Warning !! Error Occured');
        }
    });

}




function LoadOrgainsation() {

    $("#OrganisationID").empty();
    $("#OrganisationID").append('<option value>--Select--</option>')

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

function resetRowNumberEmployeeAttandance(e) {

    var rows = e.sender.tbody[0].rows;
    $(rows).each(function (e)
    {
        var grid = $("#EmployeeDailyAttadanceListGrid").data("kendoGrid");
        var row = this;
        var dataItem = grid.dataItem(row);

        if (dataItem.Days == 1) {
          
            grid.tbody.find("tr[data-uid='" + dataItem.uid + "']")
           .addClass("k-alt k-state-selected gridselect")
             .find(".chkbx")
             .attr("checked", "checked");
        }
    })


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





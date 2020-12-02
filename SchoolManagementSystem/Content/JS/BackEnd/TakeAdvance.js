var employee;
$(document).ready(function () {
      

    $(".create").off().on('click', function (e) {
        $("#hide").fadeIn(500);
        $("#show").hide();
    });
    $(".cancel").off().on('click', function (e) {

        $("#hide").hide();
        $("#show").fadeIn(500);
        ResetFormData();

    });
    $('#NepaliDate').nepaliDatePicker({
        ndpEnglishInput: 'Date',
        npdMonth: true,
        npdYear: true,
        npdYearCount: -25
    });

    $('#Date').change(function () {
        $('#NepaliDate').val(AD2BS($('#Date').val()));
    });

    $("#cancelTakeAdvance").off().on('click', function () {
        ResetFormData();
        Init();

    });

    $("#Show").off().on('click', function (e)
    {
        Operation();
    })
    

    LoadOrgainsation();       

    
    $("#OrganisationID").change(function () {
        var organisationid = $("#OrganisationID").val();
        GetEmployeeBasedOnOrganisation(organisationid,employee);

    });

    $("#saveTakeAdvance").off().on('click', function (e) {

        if (!$('form#formTakeAdvanceEntry').data('unobtrusiveValidation').validate()) {
            e.preventDefault();
            return false;
        }
        else {
            $.ajax({
                url: "/Admin/TakeAdvance/SaveTakeAdvance",
                type: 'POST',
                data: AddAntiForgeryToken({
                    ID: $('#ID').val(),
                    OrganisationID: $('#OrganisationID').val(),
                    EmployeeID: $('#EmployeeID').val(),             

                    NepaliDate: $('#NepaliDate').val(),
                    Date: $('#Date').val(),
                    DateFrom: $('#DateFrom').val(),                 
                    Year: $('#Year').val(),
                    Month: $('#Month').val(),                   
                    Amount: $('#Amount').val(),
                   
                }),
                dataType: 'json',
                success: function (data) {
                    ResetFormData();
                    ShowMessage(data.Message,true);
                    $("#hide").hide();
                    $("#show").fadeIn(500);
                    $("#TakeAdvanceListGrid").data("kendoGrid").dataSource.read();

                },
                error: function () {
                    ShowMessage("Warning !! Error Occured",false);
                }
            })
        }
    });



});

function onError(e, status) {
    ShowMessage('Warning ! Error Occured',false);
}







function GetEmployeeBasedOnOrganisation(organisation, employee) {


    $("#EmployeeID").empty();
    $("#EmployeeID").append('<option value>--Select--</option>')
   
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
            url: "/Admin/TakeAdvance/DeleteTakeAdvance",
            data: { id: dataItem.ID },
            type: 'POST',
            dataType: 'json',
            success: function (result) {
                $("#window").data("kendoWindow").close();
                ShowMessage(result.Message,true);
                ResetFormData();
                $("#TakeAdvanceListGrid").data("kendoGrid").dataSource.read();

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
        url: "/Admin/TakeAdvance/EditTakeAdvance",
        data: { id: dataItem.ID },
        type: "POST",
        dataType: "json",

        success: function (result) {

            $("#hide").fadeIn(500);
            $("#show").hide();
            $("#ID").val(result.ID);
            $("#OrganisationID").val(result.OrganisationID);
            GetEmployeeBasedOnOrganisation(result.OrganisationID, result.EmployeeID);
            $("#Date").val(ConvertDateObjectToDate(result.Date));
            $("#NepaliDate").val(ConvertDateObjectToDate1(result.NepaliDate));        
            $("#Year").val(result.Year);
            $("#Month").val(result.Month);
            $("#Amount").val(result.Amount);        



        },

        error: function (result) {

            ShowMessage('Warning !! Error Occured',false);
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

function resetRowNumber(e) {

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

function Init() {
    $("#create").hide();
    $("#list").fadeIn(500);

}

function Operation()
{
    $("#create").fadeIn(500);
    $("#list").hide();

}




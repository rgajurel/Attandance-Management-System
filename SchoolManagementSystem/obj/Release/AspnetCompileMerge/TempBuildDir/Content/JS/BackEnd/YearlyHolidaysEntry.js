$(document).ready(function () {
    LoadOrgainsation();
    LoadYear();
    $(".create").off().on('click', function (e) {
        $("#hide").fadeIn(500);
        $("#show").hide();
    });
    $(".cancel").off().on('click', function (e) {

        $("#hide").hide();
        $("#show").fadeIn(500);
        ResetFormData();
        
    });

    $("#Save").off().on('click', function (e) {

        if (!$('form#formHolidayEntry').data('unobtrusiveValidation').validate()) {
            e.preventDefault();
            return false;
        }
        else {
            $.ajax({
                url: "/Admin/YearlyHolidayEntry/SaveHolidayEntry",
                type: 'POST',
                data: AddAntiForgeryToken({
                    ID: $('#ID').val(),
                    Title: $('#Title').val(),
                    OrganisationID: $('#OrganisationID').val(),
                    YearID: $('#YearID').val(),
                    Date: $('#Date').val()

                }),
                dataType: 'json',
                success: function (data) {              
                    ResetFormData();
                    ShowMessage(data.Message, true);
                    $("#YearlyHolidayGrid").data("kendoGrid").dataSource.read();
                    $("#hide").hide();
                    $("#show").fadeIn(500);
                },
                error: function (result) {

                    ShowMessage('Error Occured', false);
                }
            })
        }
    });

    $("#FieldFilter").keyup(function () {
        var value = $("#FieldFilter").val();
        grid = $("#YearlyHolidayGrid").data("kendoGrid");
        rowNumber = 0;
        if (value) {
            grid.dataSource.filter({ field: "Title", operator: "contains", value: value });

        } else {
            grid.dataSource.filter({});
        }
    });

});
ConvertDateObjectToDate = function (dateObject) {
    var dateString = dateObject.substr(6);
    var currentTime = new Date(parseInt(dateString));
    var month = currentTime.getMonth() + 1;
    var day = currentTime.getDate();
    var year = currentTime.getFullYear();
    var date = month + "-" + day + "-" + year;
    return date;
};


function Edit(e) {

    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    e.preventDefault();


    $.ajax({
        url: "/Admin/YearlyHolidayEntry/EditHolidayEntry",
        data: { id: dataItem.ID },
        type: "POST",
        dataType: "json",
        success: function (result) {

            $("#ID").val(result.ID);
            $('#OrganisationID').val(result.OrganisationID);
            $('#YearID').val(result.YearID);
            $("#Title").val(result.Title);
            $("#Date").val(ConvertDateObjectToDate(result.Date));
            $("#hide").fadeIn(500);
            $("#show").hide();

        },
        error: function (result) {

            ShowMessage('Error Occured', false);
        }
    });

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
            url: "/Admin/YearlyHolidayEntry/DeleteHolidayEntry",
            data: { id: dataItem.ID },
            type: 'POST',
            dataType: 'json',
            success: function (result)
            {
                $("#window").data("kendoWindow").close();
                ShowMessage(result.Message, true);
                ResetFormData();
                $("#YearlyHolidayGrid").data("kendoGrid").dataSource.read();

            },
            error: function (result) {
                ShowMessage('Error Occured', false);
            }
        });

    });

    $("#no").off().on('click', function (e) {

        $("#window").data("kendoWindow").close();
    });

}
function LoadOrgainsation() {
    $("#OrganisationID").empty();
    $("#OrganisationID").append('<option value>--Select--</option>')
    $.ajax({
        url: "/Admin/DropDown/GetOrganisationDropDown",
        type: 'POST',
        dataType: 'json',
        global: false,
        success: function (data) {

            jQuery.each(data, function (index, value) {
                $("#OrganisationID").append('<option value=' + value.ID + '>' + value.Name + '</option>')
            })

        },

        error: function (response) {

            ShowMessage("Warning! Error Occured", false);  //
        }



    })

}
function LoadYear() {
    $("#YearID").empty();
    $("#YearID").append('<option value>--Select--</option>')
    $.ajax({
        url: "/Admin/DropDown/GetYearDropDown",
        type: 'POST',
        dataType: 'json',
        global: false,
        success: function (data) {
            jQuery.each(data, function (index, value) {
                $("#YearID").append('<option value=' + value.ID + '>' + value.Name + '</option>')
            })

        },

        error: function (response) {

            ShowMessage("Warning! Error Occured", false);  //
        }



    })

}



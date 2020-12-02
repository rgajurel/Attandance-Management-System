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

    $("#manageCalendarSubmit").off().on('click', function (e) {

        if (!$('form#formManageCalendar').data('unobtrusiveValidation').validate()) {
            e.preventDefault();
            return false;
        }
        else {

            $.ajax({
                url: "/Admin/ManageCalendar/SaveManageCalendar",
                type: 'POST',
                data: AddAntiForgeryToken({
                    ID: $('#ID').val(),
                    YearID: $('#YearID').val(),
                    MonthID: $('#MonthID').val(),
                    Days: $('#Days').val(),
                }),
                dataType: 'json',
                success: function (data) {

                    ResetFormData();
                    $("#hide").hide();
                    $("#show").fadeIn(500);
                    ShowMessage(data.Message,false);
                    $("#ManageCalendarList").data("kendoGrid").dataSource.read();               
                    
                }
            })
        }
    });



   


})

function Edit(e) {
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    e.preventDefault();  

    $.ajax({
        url: "/Admin/ManageCalendar/EditManageCalendar",
        data: { id: dataItem.ID },
        type: "POST",
        dataType: "json",
        cache:true,
        success: function (result)
        {
            $("#hide").fadeIn(500);
            $("#show").hide();
            $("#ID").val(result.ID);
            $("#YearID").val(result.YearID);
            $("#MonthID").val(result.MonthID);
            $("#Days").val(result.Days);

        },
        error: function (result) {

            ShowMessage('Error Occured',false);
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
            url: "/Admin/ManageCalendar/DeleteManageCalendar",
            data: { id: dataItem.ID },
            type: 'POST',
            dataType: 'json',
            success: function (result) {

                $("#window").data("kendoWindow").close();
                ShowMessage(result.Message,false);
                ResetFormData();
                $("#ManageCalendarList").data("kendoGrid").dataSource.read();
              

            }
        });

    });

    $("#no").off().on('click', function (e) {

        $("#window").data("kendoWindow").close();
    });

}
function Init()
{
    $("#Form").hide();
    $("#List").fadeIn(500);

}

function Operation() {
    $("#Form").fadeIn(500);
    $("#List").hide();

}



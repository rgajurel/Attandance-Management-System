$(document).ready(function ()
{
    $(".create").off().on('click', function (e) {
        $("#hide").fadeIn(500);
        $("#show").hide();
    })
    $(".cancel").off().on('click', function (e) {

        $("#hide").hide();
        $("#show").fadeIn(500);
        ResetFormData();



    });

    $("#Save").off().on('click', function (e) {

        if (!$('form#formJobType').data('unobtrusiveValidation').validate()) {
            e.preventDefault();
            return false;
        }
        else {

            $.ajax({
                url: "/Admin/JobType/SaveJobType",
                type: 'POST',
                data: AddAntiForgeryToken({
                    ID: $('#ID').val(),
                    JobTypeName: $('#JobTypeName').val(),

                }),
                dataType: 'json',
                success: function (data) {
                    $("#hide").hide();
                    $("#show").fadeIn(500);
                    ResetFormData();                  
                    ShowMessage(data.Message,true);
                    $("#JobTypeGrid").data("kendoGrid").dataSource.read();                   




                }
            })
        }
    });

    $("#FieldFilter").keyup(function () {
        var value = $("#FieldFilter").val();
        grid = $("#JobTypeGrid").data("kendoGrid");
        rowNumber = 0;
        if (value)
        {
            grid.dataSource.filter({ field: "JobTypeName", operator: "contains", value: value });

        } else {
            grid.dataSource.filter({});
        }
    });


})
function Init() {
    $("#Form").hide();
    $("#List").fadeIn(500);

}

function Operation() {
    $("#Form").fadeIn(500);
    $("#List").hide();

}

function Edit(e) {
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    e.preventDefault();
   

    $.ajax({
        url: "/Admin/JobType/EditJobType",
        data: { id: dataItem.ID },
        type: "POST",
        dataType: "json",

        success: function (result)
        {
            $("#hide").fadeIn(500);
            $("#show").hide();
            $("#ID").val(result.ID);
            $("#JobTypeName").val(result.JobTypeName);

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
            url: "/Admin/JobType/DeleteJobType",
            data: { id: dataItem.ID },
            type: 'POST',
            dataType: 'json',
            success: function (result) {

                $("#window").data("kendoWindow").close();
                ShowMessage(result.Message,true);
                ResetFormData();
                $("#JobTypeGrid").data("kendoGrid").dataSource.read();
               

            }
        });

    });

    $("#no").off().on('click', function (e) {

        $("#window").data("kendoWindow").close();
    });

}



$(document).ready(function () {

    $(".create").off().on('click', function (e)
    {
        $("#hide").fadeIn(500);
        $("#show").hide();
    })
    $(".cancel").off().on('click', function (e)
    {

        $("#hide").hide();
        $("#show").fadeIn(500);
        ResetFormData();

            

    });
    $("#facultySubmit").off().on('click', function (e) {

        if (!$('form#formFaculty').data('unobtrusiveValidation').validate()) {
            e.preventDefault();
            return false;
        }
        else {

            $.ajax({
                url: "/Admin/Faculty/SaveFaculty",
                type: 'POST',
                data: AddAntiForgeryToken({
                    ID: $('#ID').val(),
                    Faculty: $('#Faculty').val(),

                }),
                dataType: 'json',
                success: function (data) {

                    ResetFormData();
                    ShowMessage(data.Message);
                    $("#FacultyGrid").data("kendoGrid").dataSource.read();
                    document.getElementsByClassName("panel-title")[0].innerHTML = "Add Faculty";




                }
            })
        }
    });

    $("#FieldFilter").keyup(function () {
        var value = $("#FieldFilter").val();
        grid = $("#FacultyGrid").data("kendoGrid");
        rowNumber = 0;
        if (value) {
            grid.dataSource.filter({ field: "Faculty", operator: "contains", value: value });

        } else {
            grid.dataSource.filter({});
        }
    });


})

function Edit(e) {
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    e.preventDefault();
    document.getElementsByClassName("panel-title")[0].innerHTML = "Edit Faculty";

    $.ajax({
        url: "/Admin/Faculty/EditFaculty",
        data: { id: dataItem.ID },
        type: "POST",
        dataType: "json",

        success: function (result) {
            $("#hide").fadeIn(500);
            $("#show").hide();
            $("#ID").val(result.ID);
            $("#Faculty").val(result.Faculty);

        },
        error: function (result) {

            ShowMessage('Error Occured');
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
            url: "/Admin/Faculty/DeleteFaculty",
            data: { id: dataItem.ID },
            type: 'POST',
            dataType: 'json',
            success: function (result) {

                $("#window").data("kendoWindow").close();
                ShowMessage(result.Message);
                ResetFormData();
                $("#FacultyGrid").data("kendoGrid").dataSource.read();

            }
        });

    });

    $("#no").off().on('click', function (e) {

        $("#window").data("kendoWindow").close();
    });

}



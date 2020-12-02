$(document).ready(function () {

    $("#studentCategoryCancel").off().on('click', function (e) {
        document.getElementsByClassName("panel-title")[0].innerHTML = "Add Students Category";
        ResetFormData();
    })

    $("#studentCategorySubmit").off().on('click', function (e) {

        if (!$('form#formStudentCategory').data('unobtrusiveValidation').validate()) {
            e.preventDefault();
            return false;
        }
        else {

            $.ajax({
                url: "/Admin/StudentsCategory/SaveStudentCategory",
                type: 'POST',
                data: AddAntiForgeryToken({
                    ID: $('#ID').val(),
                    StudentsCategory: $('#StudentsCategory').val(),

                }),
                dataType: 'json',
                success: function (data) {

                    ResetFormData();
                    ShowMessage(data.Message);
                    $("#StudentCategoryGrid").data("kendoGrid").dataSource.read();
                    document.getElementsByClassName("panel-title")[0].innerHTML = "Add Students Category";




                }
            })
        }
    });

    $("#FieldFilter").keyup(function () {
        var value = $("#FieldFilter").val();
        grid = $("#StudentCategoryGrid").data("kendoGrid");
        rowNumber = 0;
        if (value) {
            grid.dataSource.filter({ field: "StudentsCategory", operator: "contains", value: value });

        } else {
            grid.dataSource.filter({});
        }
    });


})

function Edit(e) {
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    e.preventDefault();
    document.getElementsByClassName("panel-title")[0].innerHTML = "Edit Students Category";

    $.ajax({
        url: "/Admin/StudentsCategory/EditStudentCategory",
        data: { id: dataItem.ID },
        type: "POST",
        dataType: "json",

        success: function (result) {
            $("#ID").val(result.ID);
            $("#StudentsCategory").val(result.StudentsCategory);

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
            url: "/Admin/StudentsCategory/DeleteStudentCategory",
            data: { id: dataItem.ID },
            type: 'POST',
            dataType: 'json',
            success: function (result) {

                $("#window").data("kendoWindow").close();
                ShowMessage(result.Message);
                ResetFormData();
                $("#StudentCategoryGrid").data("kendoGrid").dataSource.read();
                document.getElementsByClassName("panel-title")[0].innerHTML = "Add Students Category";

            }
        });

    });

    $("#no").off().on('click', function (e) {

        $("#window").data("kendoWindow").close();
    });

}



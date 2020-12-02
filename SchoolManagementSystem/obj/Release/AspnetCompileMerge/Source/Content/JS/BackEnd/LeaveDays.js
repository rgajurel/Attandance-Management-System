$(document).ready(function () {

    $("#leaveDaysCancel").off().on('click', function (e) {
        document.getElementsByClassName("panel-title")[0].innerHTML = "Add Leave Days";
        ResetFormData();
    })

    $("#leaveDaysSubmit").off().on('click', function (e) {

        if (!$('form#formLeaveDays').data('unobtrusiveValidation').validate()) {
            e.preventDefault();
            return false;
        }
        else {

            $.ajax({
                url: "/Admin/LeaveDays/SaveLeaveDays",
                type: 'POST',
                data: AddAntiForgeryToken({
                    ID: $('#ID').val(),
                    Name: $('#Name').val(),
                    Weight: $('#Weight').val(),

                }),
                dataType: 'json',
                success: function (data) {

                    ResetFormData();
                    ShowMessage(data.Message);
                    $("#LeaveDaysList").data("kendoGrid").dataSource.read();
                    document.getElementsByClassName("panel-title")[0].innerHTML = "Add Leave Days";
                },
                error: function (e)
                {
                    ShowMessage("Warning ! Error Occured");
                }
            })
        }
    });

    $("#FieldFilter").keyup(function ()
    {
        var value = $("#FieldFilter").val();
        grid = $("#LeaveDaysList").data("kendoGrid");
        rowNumber = 0;
        if (value) {
            grid.dataSource.filter({ field: "Name", operator: "contains", value: value });

        } else {
            grid.dataSource.filter({});
        }
    });

    $("#createSchoolType").off().on('click', function (e) {
        $("#schoolTypeList").hide();
        $("#formSchoolType").show();
    })


})

function Edit(e) {
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    e.preventDefault();
    document.getElementsByClassName("panel-title")[0].innerHTML = "Edit Leave Days";

    $.ajax({
        url: "/Admin/LeaveDays/EditLeaveDays",
        data: { id: dataItem.ID },
        type: "POST",
        dataType: "json",

        success: function (result) {
            $("#ID").val(result.ID);
            $("#Name").val(result.Name);
            $("#Weight").val(result.Weight);

        },
        error: function (result) {

            ShowMessage('Warning ! Error Occured');
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
            url: "/Admin/LeaveDays/DeleteLeaveDays",
            data: { id: dataItem.ID },
            type: 'POST',
            dataType: 'json',
            success: function (result) {

                $("#window").data("kendoWindow").close();
                ShowMessage(result.Message);
                ResetFormData();
                $("#LeaveDaysList").data("kendoGrid").dataSource.read();

            }
        });

    });

    $("#no").off().on('click', function (e) {

        $("#window").data("kendoWindow").close();
    });

}



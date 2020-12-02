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
 


    $("#Save").off().on('click', function (e)
    {

        if (!$('form#formGrade').data('unobtrusiveValidation').validate())
        {
            e.preventDefault();
            return false;
        }
        else {

            $.ajax({
                url: "/Admin/GradeMaster/SaveGradeMaster",
                type: 'POST',
                data: AddAntiForgeryToken({
                    ID: $('#ID').val(),
                    Grade: $('#Grade').val(),
                    GradePoint: $('#GradePoint').val(),
                    MarksFrom: $('#MarksFrom').val(),
                    MarksTo: $('#MarksTo').val(),

                }),
                dataType: 'json',
                success: function (data) {
                    $("#hide").hide();
                    $("#show").fadeIn(500);
                    ResetFormData();
                    ShowMessage(data.Message);
                    $("#GradeMasterGrid").data("kendoGrid").dataSource.read(); 
                }
            })
        }
    });

    $("#FieldFilter").keyup(function () {
        var value = $("#FieldFilter").val();
        grid = $("#SessionInfoGrid").data("kendoGrid");
        rowNumber = 0;
        if (value) {
            grid.dataSource.filter({ field: "Session", operator: "contains", value: value });

        } else {
            grid.dataSource.filter({});
        }
    });

})

function Edit(e) {
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    e.preventDefault();
    

    $.ajax({
        url: "/Admin/GradeMaster/EditGradeMaster",
        data: { id: dataItem.ID },
        type: "POST",
        dataType: "json",

        success: function (result) {
            $("#ID").val(result.ID);
            $("#Grade").val(result.Grade);
            $("#GradePoint").val(result.GradePoint);
            $("#MarksFrom").val(result.MarksFrom);
            $("#MarksTo").val(result.MarksTo);
            $("#hide").fadeIn(500);
            $("#show").hide();

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
            url: "/Admin/GradeMaster/DeleteGradeMaster",
            data: { grade: dataItem.Grade },
            type: 'POST',
            dataType: 'json',
            success: function (result) {

                $("#window").data("kendoWindow").close();
                ShowMessage(result.Message);
                ResetFormData();
                $("#GradeMasterGrid").data("kendoGrid").dataSource.read();
              

            }
        });

    });

    $("#no").off().on('click', function (e) {

        $("#window").data("kendoWindow").close();
    });

}



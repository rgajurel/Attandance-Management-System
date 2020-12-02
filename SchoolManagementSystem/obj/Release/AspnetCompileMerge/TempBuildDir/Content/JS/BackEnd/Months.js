$(document).ready(function () {
  

    $(".create").off().on('click', function (e)
    {
        Init1();
    })
    $(".cancel").off().on('click', function (e) {

        Cancel1();
        ResetFormData();
    });

    $("#Save").off().on('click', function (e) {

        if (!$('form#formSessionInfo').data('unobtrusiveValidation').validate()) {

            e.preventDefault();
            return false;
        }
        else {

            $.ajax({
                url: "/Admin/Months/SaveSessionInfo",
                type: 'POST',
                data: AddAntiForgeryToken({
                    ID: $('#ID').val(),
                    Month: $('#Month').val(),
                    IsActive: $('#IsActive').val()

                }),
                dataType: 'json',
                success: function (data)
                {
                      Cancel1();
                      ResetFormData();
                      $("#IsActive").prop('selectedIndex', 0);
                    ShowMessage(data.Message,true);
                    $("#SessionInfoGrid").data("kendoGrid").dataSource.read();
                }
            })
        }
    });

    $("#FieldFilter").keyup(function () {
        var value = $("#FieldFilter").val();
        grid = $("#SessionInfoGrid").data("kendoGrid");
        rowNumber = 0;
        if (value) {
            grid.dataSource.filter({ field: "Month", operator: "contains", value: value });

        } else {
            grid.dataSource.filter({});
        }
    });

    $("#IsActive").change(function () {


        var isactive = $("#IsActive").val();
        CheckIfAlreadyActiveSession(isactive);


    });

});

function CheckIfAlreadyActiveSession(isactive)
{
    $.ajax({
        url: "/Admin/Months/CheckIfSessionAlreadyActive",
        type: 'POST',
        dataType: 'json',
        data: {
            IsActive: isactive,
        },
        global: false,
        success: function (data) {

            if (data != null || data != "")
            {
                ShowMessage(data.Message,false);
                $("#IsActive").prop('selectedIndex',0);

            }

        }



    })

}


function Edit(e)
{
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    e.preventDefault();
    
    $.ajax({
        url: "/Admin/Months/EditSessionInfo",
        data: { id: dataItem.ID },
        type: "POST",
        dataType: "json",       
        success: function (result) {            
            $("#ID").val(result.ID);
            $("#Month").val(result.Month);
            $("#IsActive").val(result.IsActive);
            Init1();
           
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
            url: "/Admin/Months/DeleteSessionInfo",
            data: { id: dataItem.ID },
            type: 'POST',
            dataType: 'json',
            success: function (result)
            {

                $("#window").data("kendoWindow").close();
                ShowMessage(result.Message,true);
                ResetFormData();
                $("#SessionInfoGrid").data("kendoGrid").dataSource.read();
              

            }
        });

    });

    $("#no").off().on('click', function (e) {

        $("#window").data("kendoWindow").close();
    });

}



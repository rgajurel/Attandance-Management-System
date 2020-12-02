$(document).ready(function ()
{

    $(".create").off().on('click', function (e) {
        $("#hide").fadeIn(500);
        $("#show").hide();
    })
    $(".cancel").off().on('click', function (e)
    {

        $("#hide").hide();
        $("#show").fadeIn(500);

        ResetFormData();

            

    });

    $("#FieldFilter").keyup(function () {       
        var value = $("#FieldFilter").val();
        grid = $("#FeeTypeGrid").data("kendoGrid");
        rowNumber = 0;
        if (value) {
            grid.dataSource.filter({ field: "Type", operator: "contains", value: value });

        } else {
            grid.dataSource.filter({});
        }
    });
    $("#Save").off().on('click', function (e) {

        //var isCommon = false;
        //var isCommon = $('form#formFeeType').find('#IsCommon').is(':checked');
        //if (isCommon) {
        //    isCommon = true;
        //}
        if (!$('form#formFeeType').data('unobtrusiveValidation').validate())
        {
            e.preventDefault();
            return false;
        }
        else {

            $.ajax({
                url: "/Admin/FeeType/SaveFeeType",
                type: 'POST',
                data: AddAntiForgeryToken({
                    ID: $('#ID').val(),
                    Type: $('#Type').val(),
                   
                }),
                dataType: 'json',
                success: function (data) {
                    $('#IsCommon').prop('checked', false);
                    ResetFormData();
                    ShowMessage(data.Message);
                    $("#FeeTypeGrid").data("kendoGrid").dataSource.read();
                    $("#hide").hide();
                    $("#show").fadeIn(500);


                }
            })
        }
    });
});

    function Edit(e) {
        var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
        e.preventDefault();
        $('#IsCommon').prop('checked', false);
        document.getElementsByClassName("panel-title")[0].innerHTML = "Edit Fee Type ";

        $.ajax({
            url: "/Admin/FeeType/EditFeeType",
            data: { id: dataItem.ID },
            type: "POST",
            dataType: "json",
            global: false,
            success: function (result) {
                $("#ID").val(result.ID);
                $("#Type").val(result.Type);
                $("#hide").fadeIn(500);
                $("#show").hide();
                //if (result.IsCommon == true)
                //{
                //    $('#IsCommon').prop('checked', true);

                //}


            },
            error: function (result) {

                ShowMessage('Error Occured');
            }
        });

    }


    function Delete(e)
    {

        var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
        e.preventDefault();
        $("#window").kendoWindow({
            modal: true
        });
        $("#window").data("kendoWindow").open().center();

        $("#yes").off().on('click', function (e) {

            $.ajax({
                url: "/Admin/FeeType/DeleteFeeType",
                data: { id: dataItem.ID },
                type: 'POST',
                dataType: 'json',
                success: function (result) {
                    //$('#IsCommon').prop('checked', false);
                    $("#window").data("kendoWindow").close();
                    ShowMessage(result.Message);
                    ResetFormData();
                    $("#FeeTypeGrid").data("kendoGrid").dataSource.read();

                }
            });

        });

        $("#no").off().on('click', function (e) {

            $("#window").data("kendoWindow").close();
        });

    }




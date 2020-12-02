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

    $("#Save").off().on('click', function (e) {

        if (!$('form#formTaxMaster').data('unobtrusiveValidation').validate()) {
            e.preventDefault();
            return false;
        }
        else {

            $.ajax({
                url: "/Admin/TaxMaster/SaveTaxMaster",
                type: 'POST',
                data: AddAntiForgeryToken({
                    ID: $('#ID').val(),
                    AmountFrom: $('#AmountFrom').val(),
                    AmountTo: $('#AmountTo').val(),
                    TaxPercentage: $('#TaxPercentage').val(),
                    SortOrder: $('#SortOrder').val(),
                }),
                dataType: 'json',
                success: function (data) {

                    ResetFormData();
                    $("#hide").hide();
                    $("#show").fadeIn(500);
                    ShowMessage(data.Message);
                    $("#TaxMasterGrid").data("kendoGrid").dataSource.read();               
                    
                }
            })
        }
    });



    $("#FieldFilter").keyup(function () {
        var value = $("#FieldFilter").val();
        grid = $("#DepartmentGrid").data("kendoGrid");
        rowNumber = 0;
        if (value) {
            grid.dataSource.filter({ field: "DepartmentName", operator: "contains", value: value });

        } else {
            grid.dataSource.filter({});
        }
    });


})

function Edit(e) {
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    e.preventDefault();  

    $.ajax({
        url: "/Admin/TaxMaster/EditTaxMaster",
        data: { id: dataItem.ID },
        type: "POST",
        dataType: "json",       
        success: function (result)
        {
            Operation();
            $("#ID").val(result.ID);
            $("#AmountFrom").val(result.AmountFrom);
            $("#AmountTo").val(result.AmountTo);
            $("#TaxPercentage").val(result.TaxPercentage);
            $("#SortOrder").val(result.SortOrder);
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
            url: "/Admin/TaxMaster/DeleteTaxMaster",
            data: { id: dataItem.ID },
            type: 'POST',
            dataType: 'json',
            success: function (result) {

                $("#window").data("kendoWindow").close();
                ShowMessage(result.Message);
                ResetFormData();
                $("#TaxMasterGrid").data("kendoGrid").dataSource.read();
              

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



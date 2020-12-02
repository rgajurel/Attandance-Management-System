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

    var termmaster = [];
    var index = 1;

    $("#Save").off().on('click', function (e) {

        if (!$('form#formClassType').data('unobtrusiveValidation').validate()) {
            e.preventDefault();
            return false;
        }
        else {

            var isfinal = false;
            var isfinal = $('form#formTermMaster').find('#IsFinalTerm').is(':checked');
            if (isfinal) {
                isfinal = true;
            }
            $.ajax({
                url: "/Admin/TermMaster/SaveTaxMaster",
                type: 'POST',
                data: AddAntiForgeryToken({
                    ID: $('#ID').val(),
                    Type: $('#TermName').val(),
                    TermPercentage: $('#TermPercentage').val(),
                    IsFinalTerm:isfinal                   

                }),
                dataType: 'json',
                success: function (data) {

                    ResetFormData();
                    ShowMessage(data.Message);
                    $("#TermMasterGrid").data("kendoGrid").dataSource.read();
                    $("#hide").hide();
                    $("#show").fadeIn(500);




                }
            })
        }
    });

    $(document).on('click', '.trashdelete', function () {

        $(this).closest('tr').remove();
        return false;
    });
    $("#termMasterAdd1").off().on('click', function (e)
    {
        

        $("#container").append("<tr><td><input class='form-control termname' data-val='true' data-val-required='Required' id='TermMasterList[" + index + "].TermName' name='TermMasterList[" + index + "].TermName' type='text' value=''><span class='field-validation-valid text-danger' data-valmsg-for='TermMasterList[" + index + "].TermName' data-valmsg-replace='true'></span></td><<td><input class='form-control termpercentage' data-val='true' data-val-required='This field is Required' id='TermMasterList[" + index + "].TermPercentage' name='TermMasterList[" + index + "].TermPercentage' type='number' value=''><span class='field-validation-valid text-danger' data-valmsg-for='TermMasterList[" + index + "].TermPercentage' data-valmsg-replace='true'></span></td><td><a href='#'><i class='fa fa-trash trashdelete' aria-hidden='true' style='margin-left:20px; color:blue' ></i></a></td></tr>")

        index++;
        // ResetFormData();



    });

    $(document).on('click', '.delete', function (e) {
        {
            $(this).closest('tr').remove();
            return false;
        }
    })

    $(document).on('click', '.update', function (e) {
        {
            $('#commit').show();
          
            e.preventDefault();
            $("#termMasterSubmit").val("Update")
            rowindex = $(this).parent().parent().index();
            var currentRow = $(this).parent().parent();
            $("#TermName").val(currentRow.find('td:eq(0)').html())
            $("#TermPercentage").val(currentRow.find('td:eq(1)').html())

        }
    })

   
  
    

    $("#FieldFilter").keyup(function ()
    {
        var value = $("#FieldFilter").val();
        grid = $("#TermMasterDynamicGrid").data("kendoGrid");
        rowNumber = 0;
        if (value)
        {
            grid.dataSource.filter({ field: "TermaName", operator: "contains", value: value });

        } else
        {
            grid.dataSource.filter({});
        }
    });
    

});


function OnSuccess(response)
{
    
    $("#TermMasterDynamicGrid").data("kendoGrid").dataSource.read();
    ShowMessage(response.Message);
    $("#hide").hide();
    $("#show").fadeIn(500);
    ResetFormData();
    //$("#container").find("tr:gt(1)").remove();
    //ResetFormData();
}
function OnFailure(response)
{
    ShowMessage("Warning ! Error Occured");
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
            url: "/Admin/TermMasterDynamic/DeleteTermMasterDynamic",
            data: { id: dataItem.ID },
            type: 'POST',
            dataType: 'json',
            success: function (result)
            {
                $("#window").data("kendoWindow").close();
                ShowMessage(result.Message);
                ResetFormData();
                $("#TermMasterDynamicGrid").data("kendoGrid").dataSource.read();

            }


        });

    });

    $("#no").off().on('click', function (e) {

        $("#window").data("kendoWindow").close();
    });

}
function Edit(e) {
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    e.preventDefault();
    var heading = document.getElementsByClassName("panel-title")[0].innerHTML = "Edit Term  Master";

    $.ajax({
        url: "/Admin/TermMasterDynamic/EditTermMaster",
        data: { id: dataItem.ID },
        type: "POST",
        dataType: "json",

        success: function (result)
        {
           
            $('#ID').val(result.ID);
           
            $('#TermName').val(result.TermName);
            $('#TermPercentage').val(result.TermPercentage);
            if (result.IsFinalTerm == true)
            {
                $('#IsFinalTerm').prop('checked', true);

             }
            if (result.IsFinalTerm == false)
            {
                $('#IsFinalTerm').prop('checked', false);

             }       
            $("#hide").fadeIn(500);
            $("#show").hide();
          
           
            
        },

        error: function (result)
        {

            ShowMessage('Error Occured');
        }
    });

}
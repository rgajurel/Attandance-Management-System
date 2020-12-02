$(document).ready(function ()
{    


$("#locationCancel").off().on('click', function (e)
{
    document.getElementsByClassName("panel-title")[0].innerHTML = "Add Location";
    ResetFormData();
})

$("#locationSubmit").off().on('click', function (e)
{  
   
    if (!$('form#formLocation').data('unobtrusiveValidation').validate())
    {
      e.preventDefault();
        return false;
    }
    else
    {

        $.ajax({
            url: "/Admin/Location/SaveLocation",
            type: 'POST',
            data: AddAntiForgeryToken({
                ID: $('#ID').val(),
                LocationName: $('#LocationName').val(),
                Fee: $('#Fee').val(),
            }),
            dataType: 'json',
            success: function (data)
            {
                
                ResetFormData();
                ShowMessage(data.Message);
                document.getElementsByClassName("panel-title")[0].innerHTML = "Add Location";
                $("#LocationGrid").data("kendoGrid").dataSource.read();            

               
            }
        })
    }
});

$("#FieldFilter").keyup(function ()
{
    var value = $("#FieldFilter").val();
    grid = $("#LocationGrid").data("kendoGrid");
    rowNumber = 0;
    if (value)
    {
        grid.dataSource.filter({ field: "LocationName", operator: "contains", value: value });

    } else
    {
        grid.dataSource.filter({});
    }
});
});



function Edit(e)
{
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    e.preventDefault();
     document.getElementsByClassName("panel-title")[0].innerHTML="Edit Location";
  
    $.ajax({
        url: "/Admin/Location/EditLocation",
        data: { id: dataItem.ID },
        type: "POST",
        dataType: "json",
   
        success: function (result)
        {           
            $("#ID").val(result.ID);
            $("#LocationName").val(result.LocationName);
            $("#Fee").val(result.Fee);

        },
        error: function (result)
        {
            
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
            url: "/Admin/Location/DeleteLocation",
            data: { id: dataItem.ID },
            type: 'POST',
            dataType: 'json',
            success: function (result)
            {

                $("#window").data("kendoWindow").close();
                ShowMessage(result.Message);
                ResetFormData();
                document.getElementsByClassName("panel-title")[0].innerHTML = "Add Location";
                $("#LocationGrid").data("kendoGrid").dataSource.read();

            }           
        });

    });

    $("#no").off().on('click', function (e)
    {

        $("#window").data("kendoWindow").close();
    });

}



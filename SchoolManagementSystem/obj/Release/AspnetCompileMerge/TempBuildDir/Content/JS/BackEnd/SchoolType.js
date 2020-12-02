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
$("#schoolTypeSubmit").off().on('click', function (e)
{  
   
    if (!$('form#formSchoolType').data('unobtrusiveValidation').validate())
    {
      e.preventDefault();
        return false;
    }
    else
    {

        $.ajax({
            url: "/Admin/OrganisationType/SaveSchoolType",
            type: 'POST',
            data: AddAntiForgeryToken({
                ID: $('#ID').val(),
                Type: $('#Type').val(),            

            }),
            dataType: 'json',
            success: function (data)
            {
                
                ResetFormData();
                ShowMessage(data.Message,true);              
                $("#SchoolTypeGrid").data("kendoGrid").dataSource.read();
                $("#hide").hide();
                $("#show").fadeIn(500);
               
                

               
            }
        })
    }
});

$("#FieldFilter").keyup(function ()
{
    var value = $("#FieldFilter").val();
    grid = $("#SchoolTypeGrid").data("kendoGrid");
    rowNumber = 0;
    if (value)
    {
        grid.dataSource.filter({ field: "Type", operator: "contains", value: value });

    } else
    {
        grid.dataSource.filter({});
    }
});

    


})

function Edit(e)
{
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    e.preventDefault();
  
  
    $.ajax({
        url: "/Admin/OrganisationType/EditSchoolType",
        data: { id: dataItem.ID },
        type: "POST",
        dataType: "json",
   global:false,
        success: function (result)
        {
            $("#hide").fadeIn(500);
            $("#show").hide();
            $("#ID").val(result.ID);
            $("#Type").val(result.Type);     

        },
        error: function (result)
        {
            
            ShowMessage('Error Occured',false);
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
            url: "/Admin/OrganisationType/DeleteSchoolType",
            data: { id: dataItem.ID },
            type: 'POST',
            dataType: 'json',
            success: function (result)
            {

                $("#window").data("kendoWindow").close();
                ShowMessage(result.Message, true);
                ResetFormData();             
                $("#SchoolTypeGrid").data("kendoGrid").dataSource.read();

            }           
        });

    });

    $("#no").off().on('click', function (e)
    {

        $("#window").data("kendoWindow").close();
    });

}



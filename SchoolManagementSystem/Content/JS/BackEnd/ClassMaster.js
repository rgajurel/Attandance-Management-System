$(document).ready(function () {
   
    $("#container").find("tr:gt(0)").remove();
    var Names = [];
    var itemIndex = 1;
    $(".create").off().on('click', function (e) {
        $("#hide").fadeIn(500);
        $("#show").hide();
    })
     

    $("#sectionCancel").off().on('click', function (e)
    {
       
        $("#container").find("tr:gt(0)").remove();
        $("#hide").hide();
        $("#show").fadeIn(500);
         ResetFormData();
      
    });

   $(document).on('click', '.trashdelete', function (e) {
        //  $(".trashdelete").click(function(){
        e.preventDefault();       
        $(this).closest('tr').remove();
        return false;
    });
   
    $(document).on('click', '#sectionAdd', function (e) {
                
        e.preventDefault();
        newItem = $("<tr><td><input type='text'class='form-control comment' style='text-transform:uppercase' data-val='true' data-val-required='Required' id='Name[" + itemIndex + "].Name' name='Name[" + itemIndex + "].Name'/><span class='field-validation-valid text-danger' data-valmsg-for='Name[" + itemIndex + "].Name' data-valmsg-replace='true'></span></td><td><a href='#'><i class='fa fa-trash trashdelete' aria-hidden='true' style='margin-left:20px; color:blue' ></i><a/></td></tr>");
        $("#container").append(newItem);
        itemIndex++;

    });


   

    $('form#formClassMaster').on('submit', function (e) {

        $('input.comment').each(function ()
        {           
            $(this).rules("add",
                {
                    required: true,                  

                })
        })
        if ($('form#formClassMaster').validate().form())
        {
            Names.splice(0, Names.length);
            $('input[name^="Name"]').each(function ()
            {
                Names.push
                               ({
                                   ID: $("#ID").val(),
                                   Name: $(this).val(),

                               });
            });
            $.ajax({
                url: "/Admin/ClassMaster/SaveClassMaster",
                type: 'POST',
                data: {
                    classmaster: Names,
                    __RequestVerificationToken: $('form input[name=__RequestVerificationToken]').val()
                },
                dataType: 'json',
                success: function (data)
                {
                    $("#hide").hide();
                    $("#show").fadeIn(500);
                    ResetFormData();
                    ShowMessage(data.Message);
                    $("#container").find("tr:gt(0)").remove();
                  
                    $("#ClassMasterGrid").data("kendoGrid").dataSource.read();
                    $('#sectionAdd').show();
                    $("#duplicatemessage").empty();


                }
            })
        } else {
            return false;
        }

    });





    $("#FieldFilter").keyup(function () {
        var value = $("#FieldFilter").val();
        grid = $("#ClassMasterGrid").data("kendoGrid");
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

    $(document).on("keyup", ".comment", function ()
    {       
        var classname = $(this).val().toUpperCase();;
        var data = $("#ClassMasterGrid").data("kendoGrid").dataSource.data();
        for (i = 0; i < data.length; i++) {

            if (data[i].Name == classname) {
                ShowMessage("Warning ! Subject Code Already Exist");
                this.value = "";

            }
        }

    });

   


});

function Edit(e) {
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    e.preventDefault();
   
    $('#sectionAdd').hide();

    $.ajax({
        url: "/Admin/ClassMaster/EditClassMaster",
        data: { id: dataItem.ID },
        type: "POST",
        dataType: "json",

        success: function (result) {
          
            $("#ID").val(result.ID);
            $(".comment").val(result.Name);
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

    $(document).on("keyup", ".comment", function () {

       
        var name = $(this).val().toUpperCase();;
        var data = $("#ClassMasterGrid").data("kendoGrid").dataSource.data();
        for (i = 0; i < data.length; i++) {

            if (data[i].Name == name) {
                ShowMessage("Warning ! Subject Code Already Exist");
                this.value = "";

            }
        }

    });

    $("#yes").off().on('click', function (e) {

        $.ajax({
            url: "/Admin/ClassMaster/DeleteClassMaster",
            data: { ID: dataItem.ID },
            type: 'POST',
            dataType: 'json',
            success: function (result) {

                $("#window").data("kendoWindow").close();
                ShowMessage(result.Message);
                ResetFormData();
                $("#ClassMasterGrid").data("kendoGrid").dataSource.read();

            }
        });

    });

    $("#no").off().on('click', function (e) {

        $("#window").data("kendoWindow").close();
    });

}

//onchange in kendogrid
//function onChange(e) {
//    if (e.action == "itemchange") {
//        if (e.field == "ColumnA" || e.field == "ColumnB") {
//            if (e.items[0] && (e.items[0].ColumnA == 0 || e.items[0].ColumnB == 0)) {
//                e.items[0].set("ColumnC", true);
//            }
 
//        }
//    }
//},
//function OnChange(arg) 
//{
//    debugger;
//    var selected = $.map(this.select(), function (item) {
//        return $(item).text();
//    });

//    kendoConsole.log("Selected: " + selected.length + " item(s), [" + selected.join(", ") + "]");
//}
function Save(e)
{
   
    if (e.values && (e.values.Name ))
    {
        var name = e.values.Name || e.model.Name;
        e.model.set("SN", 2);

       
    }

}

//save : function (e)
//{
//    if (e.values && (e.values.Qty || e.values.Price)) {
//        var qty = e.values.Qty || e.model.Qty;
//        var price = e.values.Price || e.model.Price;
//        e.model.set("Total", price * qty);
//    }
//}
//function myFunction(e)
//{
//    debugger;
//    var tt = $(this).val();
//    var pp = 'dfdf';
//    var currentDataItem = $("#SectionGrid").data("kendoGrid").dataItem($(this).closest("tr"));
   
//    $("#SectionGrid").find("tr[data-uid='" + currentDataItem.uid + "'] td:eq(1)").text(pp);

//    var grid = $("#SectionGrid").data("kendoGrid");                       
//                       grid.saveRow();
    
  
    
//}

    

    




$(document).ready(function () {

    var Name = [];
    var itemIndex = 1;

    $(".create").off().on('click', function (e) {
        $("#hide").fadeIn(500);
        $("#show").hide();
    })

    $("#sectionCancel").off().on('click', function (e) {

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
    //$("#sectionAdd").click(function(){
    $(document).on('click', '#sectionAdd', function (e) {

        e.preventDefault();
        var newItem = $("<tr><td><input type='text'class='form-control comment'   style='text-transform:uppercase' data-val='true' data-val-required='Required' id='Name[" + itemIndex + "].Name' name='Name[" + itemIndex + "].Name'/><span class='field-validation-valid text-danger' data-valmsg-for='Name[" + itemIndex + "].Name' data-valmsg-replace='true'></span></td><td><a href='#'><i class='fa fa-trash trashdelete' aria-hidden='true' style='margin-left:20px; color:blue' ></i><a/></td></tr>");
        $("#container").append(newItem);
        itemIndex++;

    });


    $(document).on("keyup", "input:text[class^='form-control comment']", function () {
    
        var val = this.value;

        $.ajax({
            url: "/Admin/Section/SectionCount",
            data: { section: val },
            type: "POST",
            dataType: "json",
            success: function (result) {
                if (result > 0) {
                    $(this).empty()
                    $("#duplicatemessage").empty();
                    $("#duplicatemessage").text("Section Exist")

                    return false;
                }
                else {
                    $("#duplicatemessage").empty();
                    return true;
                }
            }
        })

    });


    $('form#formSection').on('submit', function (e) {

     
        $('input.comment').each(function ()
        {
           
            $(this).rules("add",
                {
                    required: true,
                    maxlength: 3,


                })
        })
        if ($('form#formSection').validate().form())
        {
            Name.splice(0, Name.length);
            $('input[name^="Name"]').each(function ()
            {
                Name.push
                               ({
                                   ID: $("#ID").val(),
                                   Name: $(this).val(),

                               });
            });
            $.ajax({
                url: "/Admin/Section/SaveSection",
                type: 'POST',
                data: {
                    section: Name,
                    __RequestVerificationToken: $('form input[name=__RequestVerificationToken]').val()
                },
                dataType: 'json',
                success: function (data) {

                    ResetFormData();
                    ShowMessage(data.Message);
                    $("#container").find("tr:gt(0)").remove();
                  
                    $("#SectionGrid").data("kendoGrid").dataSource.read();
                    $("#container").find("tr:gt(0)").remove();
                    $("#hide").hide();
                    $("#show").fadeIn(500);
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
        grid = $("#SectionGrid").data("kendoGrid");
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

   


});

function Edit(e) {
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    e.preventDefault();
    document.getElementsByClassName("panel-title")[0].innerHTML = "Edit Section";
    $('#sectionAdd').hide();

    $.ajax({
        url: "/Admin/Section/EditSection",
        data: { id: dataItem.ID },
        type: "POST",
        dataType: "json",

        success: function (result) {
            
            $("#ID").val(result.ID);
            $(".comment").val(result.Name);

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
            url: "/Admin/Section/DeleteSection",
            data: { section: dataItem.Name },
            type: 'POST',
            dataType: 'json',
            success: function (result) {

                $("#window").data("kendoWindow").close();
                ShowMessage(result.Message);
                ResetFormData();
                $("#SectionGrid").data("kendoGrid").dataSource.read();

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

    

    




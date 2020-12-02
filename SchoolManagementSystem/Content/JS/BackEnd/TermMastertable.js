$(document).ready(function ()
{
    var termmaster = [];
    var rowindex = 0;
    $("#termMasterSubmit").off().on('click', function (e)
    {      
      
       
        if (!$('form#formTermMaster').data('unobtrusiveValidation').validate())
        {
            e.preventDefault();
            return false;
        }
        else
        {
            if (rowindex > 0)
            {
                var EditRow = $('#container tr').eq(rowindex);
                $(EditRow).find('td:eq(0)').html($("#TermName").val());
                $(EditRow).find('td:eq(1)').html($("#TermPercentage").val());
             
            }
            else
            {           
                $("#container").append("<tr><td>" + $("#TermName").val() + "</td><td>" + $("#TermPercentage").val() + "</td><td><button class='btn btn-info update' type='button'>Edit</button><button class='btn delete' type='button'>Delete</button></td></tr>")
           
            }
            $("#termMasterSubmit").val("+Add")
           // ResetFormData();
        }     
      
      
    })

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

    $("#termMasterAdd").off().on('click', function (e)
    {
       var table = $("table#container");
       table.find('tr').each(function (i)
       {
            var $tds = $(this).find('td');
            termmaster.push({
                ID: $("#ID").val(),
                TermName: $tds.eq(0).text(),
                TermPercentage: $tds.eq(1).text()
            })
           

       })
       termmaster.shift();
     
        if (termmaster.length == 0) {
            ShowMessage("Warning ! Please Add Terms");
        }
        else
        {            
            $.ajax({
                url: "/Admin/TermMaster/SaveTermMaster",
                type: 'POST',
                dataType: 'json',
                data: {
                    termMaster:termmaster,
                    __RequestVerificationToken: $('form input[name=__RequestVerificationToken]').val(),
                },

                success: function (data) {
                    ResetFormData();
                    ShowMessage(data.Message);
                    $('#hidebuton').show();
                    $("#container").find("tr:gt(0)").remove();
                    $("#TermMasterGrid").data("kendoGrid").dataSource.read();
                  //  document.getElementsByClassName("panel-title")[0].innerHTML = "Add Class/Course";
                  



                }
            })

        }

    })

    $("#FieldFilter").keyup(function ()
    {
        var value = $("#FieldFilter").val();
        grid = $("#TermMasterGrid").data("kendoGrid");
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

function Delete(e) {
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    e.preventDefault();
    $("#window").kendoWindow({
        modal: true
    });
    $("#window").data("kendoWindow").open().center();

    $("#yes").off().on('click', function (e) {

        $.ajax({
            url: "/Admin/Subjects/DeleteSubjects",
            data: { id: dataItem.ID },
            type: 'POST',
            dataType: 'json',
            success: function (result) {
                $("#window").data("kendoWindow").close();
                ShowMessage(result.Message);
                ResetFormData();
                $("#SubjectGrid").data("kendoGrid").dataSource.read();

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
        url: "/Admin/TermMaster/EditTermMaster",
        data: { id: dataItem.ID },
        type: "POST",
        dataType: "json",

        success: function (result)
        {
            $("#container").find("tr:gt(0)").remove();
            $('#ID').val(result.ID)
            $('#commit').hide();
            $('#hidebuton').hide();
           

            $("#container").append("<tr><td>" + result.TermName + "</td><td>" + result.TermPercentage + "</td><td><button class='btn btn-info update' type='button'>Edit</button><button class='btn delete' type='button'>Delete</button></td></tr>");
            
        },

        error: function (result)
        {

            ShowMessage('Error Occured');
        }
    });

}
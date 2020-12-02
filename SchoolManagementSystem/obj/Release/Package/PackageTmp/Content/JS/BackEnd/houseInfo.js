$(document).ready(function ()
{
    $("#houseInfoCancel").off().on('click', function (e) {
        ResetFormData();
    })


    $("#houseInfoSubmit").off().on('click', function (e) {
        //var a  = $('#HouseName').val()
        //alert(a);
        if (!$('form#formHouseInfo').data('unobtrusiveValidation').validate()) {
            e.preventDefault();
            return false;
        }
        else {
            $.ajax({
                url: "/Admin/HouseInfo/saveHouseInfo",
                type: 'POST',
                data: AddAntiForgeryToken({
                    ID: $('#ID').val(),
                    HouseName: $('#HouseName').val(),
                }),

                dataType: 'json',
                success: function (data) {
                    ResetFormData();
                    ShowMessage(data.Message);
                    $("#HouseInfoGrid").data("kendoGrid").dataSource.read();
                   document.getElementsByClassName("panel-title")[0].innerHTML = "Add House Info";
                }
            })
        }
    });


    $("#FieldFilter").keyup(function ()
    {
        var value = $("#FieldFilter").val();
        grid = $("#HouseInfoGrid").data("kendoGrid");
        if (value) {
            grid.dataSource.filter({ field: "HouseName", operator: "contains", value: value });

        } else {
            grid.dataSource.filter({});
        }
    });
})



function Edit(e) {
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    e.preventDefault();
    var heading = document.getElementsByClassName("panel-title")[0].innerHTML = "Edit House Info";
    
    $.ajax({
        url: "/Admin/HouseInfo/editHouseInfo",
        data: { id: dataItem.ID },
        type: "POST",
        dataType: "json",

        success: function (result) {
            $("#ID").val(result.ID);
            $("#HouseName").val(result.HouseName);

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
            url: "/Admin/HouseInfo/deleteHouseInfo",
            data: { id: dataItem.ID },
            type: 'POST',
            dataType: 'json',
            success: function (result) {

                $("#window").data("kendoWindow").close();
                ShowMessage(result.Message);
                ResetFormData();
                $("#HouseInfoGrid").data("kendoGrid").dataSource.read();

            }
        });

    });

    $("#no").off().on('click', function (e) {

        $("#window").data("kendoWindow").close();
    });

}


var rowNumber = 0;

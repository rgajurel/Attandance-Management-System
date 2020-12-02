$(document).ready(function () {

    $("#busInfoCancel").off().on('click', function (e) {
        ResetFormData();
       
    });


       

    $("#busInfoSubmit").off().on('click', function (e) {

        if (!$('form#formBusInfo').data('unobtrusiveValidation').validate()) {
            e.preventDefault();
            return false;
        }
        else
        {
            $.ajax({
                url: "/Admin/BusInfo/SaveBusInfo",
                type: 'POST',
                data: AddAntiForgeryToken({
                    ID: $('#ID').val(),
                    BusNo: $('#BusNo').val(),
                    DriverName: $('#DriverName').val(),
                    DriverPhoneNo: $('#DriverPhoneNo').val(),
                    SupporterName: $('#SupporterName').val(),
                    SupporterPhoneNo: $('#SupporterPhoneNo').val(),

                }),
                dataType: 'json',
                success: function (data)
                {
                    ResetFormData();
                    ShowMessage(data.Message);
                    $("#BusInfoGrid").data("kendoGrid").dataSource.read();
                    $("html, body").animate({ scrollTop: $(document).height() },200); 
                }
            })
        }
    });


    $("#FieldFilter").keyup(function ()
    {
        var value = $("#FieldFilter").val();
        $("#BusInfoGrid").data("kendoGrid").dataSource.filter({
            logic  : "or",
            filters: [
                {
                    field   : "DriverName",
                    operator: "contains",
                    value   : value
                },
                {
                    field   : "BusNo",
                    operator: "contains",
                    value   : value
                }
            ]
        });     
       
        
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
            url: "/Admin/BusInfo/DeleteBusInfo",
            data: { id: dataItem.ID },
            type: 'POST',
            dataType: 'json',
            success: function (result)
            {
                $("#window").data("kendoWindow").close();
                ShowMessage(result.Message);
                ResetFormData();              
                $("#BusInfoGrid").data("kendoGrid").dataSource.read();
                $("html, body").animate({scrollTop: 0,}, 1000);
                return false; 
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
    var heading = document.getElementsByClassName("panel-title")[0].innerHTML = "Edit Institution Details";

    $.ajax({
        url: "/Admin/BusInfo/EditBusInfo",
        data: { id: dataItem.ID },
        type: "POST",
        dataType: "json",

        success: function (result) {
            $("#ID").val(result.ID);
            $("#BusNo").val(result.BusNo);
            $("#DriverName").val(result.DriverName);
            $("#DriverPhoneNo").val(result.DriverPhoneNo);
            $("#SupporterName").val(result.SupporterName);
            $("#SupporterPhoneNo").val(result.SupporterPhoneNo);          


            $("html, body").animate({ scrollTop: 0, }, 1000);
            return false;
        },

        error: function (result) {

            ShowMessage('Error Occured');
        }
    });

}




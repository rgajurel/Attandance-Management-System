$(document).ready(function () {

    $("#hostelInfoCancel").off().on('click', function (e) {
        ResetFormData();
       
    });
       

    $("#hostelInfoSubmit").off().on('click', function (e)
    {
       if (!$('form#formHostelInfo').data('unobtrusiveValidation').validate())
         {
            e.preventDefault();
            return false;
        }
        else
        {
       
            $.ajax({
                url: "/Admin/HostelInfo/SaveHostelInfo",
                type: 'POST',
                data: AddAntiForgeryToken({
                    ID: $('#ID').val(),
                    HostelName: $('#HostelName').val(),
                    ContactNo: $('#ContactNo').val(),
                    Address: $('#Address').val(),
                    PersonIncharge: $('#PersonIncharge').val(),
                    InchargePhoneNo: $('#InchargePhoneNo').val(),

                }),
                dataType: 'json',
                success: function (data)
                {
                    ResetFormData();
                    ShowMessage(data.Message);                   
                    $("#HostelInfoGrid").data("kendoGrid").dataSource.read();
                    $("html, body").animate({ scrollTop: $(document).height() },100); 
                }
            })
        }
    });


    $("#FieldFilter").keyup(function () {
        var value = $("#FieldFilter").val();
        $("#HostelInfoGrid").data("kendoGrid").dataSource.filter({
            logic: "or",
            filters: [
                {
                    field: "HostelName",
                    operator: "contains",
                    value: value
                },
                {
                    field: "PersonIncharge",
                    operator: "contains",
                    value: value
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
            url: "/Admin/HostelInfo/DeleteHostelInfo",
            data: { id: dataItem.ID },
            type: 'POST',
            dataType: 'json',
            success: function (result)
            {
                $("#window").data("kendoWindow").close();
                ShowMessage(result.Message);
                ResetFormData();              
                $("#HostelInfoGrid").data("kendoGrid").dataSource.read();
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

    $.ajax({
        url: "/Admin/HostelInfo/EditHostelInfo",
        data: { id: dataItem.ID },
        type: "POST",
        dataType: "json",

        success: function (result) {
            $("#ID").val(result.ID);
            $("#HostelName").val(result.HostelName);
            $("#Address").val(result.Address);
            $("#ContactNo").val(result.ContactNo);
            $("#PersonIncharge").val(result.PersonIncharge);
            $("#InchargePhoneNo").val(result.InchargePhoneNo); 
            $("html, body").animate({ scrollTop: 0, }, 1000);
            return false;
        },

        error: function (result) {

            ShowMessage('Error Occured');
        }
    });

}




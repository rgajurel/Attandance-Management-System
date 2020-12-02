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

        if (!$('form#formLanguage').data('unobtrusiveValidation').validate()) {
            e.preventDefault();
            return false;
        }
        else {
           
            var formData = new FormData();
            formData.append("ID", $("#ID").val());
            formData.append("Name", $("#Name").val());
            formData.append("Code", $("#Code").val());
            formData.append("Status", $("#Status").val());           
            formData.append("ImageFile", $('#ImageFile')[0].files[0]);
            formData.append('__RequestVerificationToken', $('form input[name=__RequestVerificationToken]').val());
            $.ajax({
                url: "/Admin/Language/SaveLanguage",
                type: 'POST',
                data: formData,
                dataType: 'json',
                contentType: false,
                processData: false,
                success: function (data) {
                    ResetFormData();
                    ShowMessage(data.Message, true);
                    $("#LangaugeGrid").data("kendoGrid").dataSource.read();
                    $("#hide").hide();
                    $("#show").fadeIn(500);
                },
                error: function (result) {

                    ShowMessage('Error Occured', false);
                }
            })
        }
    });

    $("#FieldFilter").keyup(function () {
        var value = $("#FieldFilter").val();
        grid = $("#LangaugeGrid").data("kendoGrid");
        rowNumber = 0;
        if (value) {
            grid.dataSource.filter({ field: "Title", operator: "contains", value: value });

        } else {
            grid.dataSource.filter({});
        }
    });

});
ConvertDateObjectToDate = function (dateObject) {
    var dateString = dateObject.substr(6);
    var currentTime = new Date(parseInt(dateString));
    var month = currentTime.getMonth() + 1;
    var day = currentTime.getDate();
    var year = currentTime.getFullYear();
    var date = month + "-" + day + "-" + year;
    return date;
};


function Edit(e) {
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    debugger;
    e.preventDefault();

    $.ajax({
        url: "/Admin/Language/EditLanguageEntry",
        data: { id: dataItem.ID },
        type: "POST",
        dataType: "json",
        success: function (result) {
            $("#ID").val(result.ID);
            $('#Name').val(result.Name);
            $('#Code').val(result.Code);
            $("#Status").val(result.Status);
            $("#hide").fadeIn(500);
            $("#show").hide();

        },
        error: function (result) {

            ShowMessage('Error Occured', false);
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
            url: "/Admin/Language/DeleteLanguageEntry",
            data: { id: dataItem.ID },
            type: 'POST',
            dataType: 'json',
            success: function (result) {
                $("#window").data("kendoWindow").close();
                ShowMessage(result.Message, true);
                ResetFormData();
                $("#LangaugeGrid").data("kendoGrid").dataSource.read();

            },
            error: function (result) {
                ShowMessage('Error Occured', false);
            }
        });

    });

    $("#no").off().on('click', function (e) {

        $("#window").data("kendoWindow").close();
    });

}




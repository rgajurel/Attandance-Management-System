$(document).ready(function () {
    LoadLanguage();
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

        if (!$('form#formLanguageParameter').data('unobtrusiveValidation').validate()) {
            e.preventDefault();
            return false;
        }
        else {

            $.ajax({
                url: "/Admin/LanguageParameter/SaveLanguageParameter",
                type: 'POST',
                data: AddAntiForgeryToken({
                    ID: $('#ID').val(),
                    LanguageID: $('#LanguageID').val(),
                    Page: $('#Page').val(),
                    Key: $('#Key').val(),
                    OriginalWordInEnglish: $('#OriginalWordInEnglish').val(),
                    TranslatedWord: $('#TranslatedWord').val()
                }),
                dataType: 'json',
                success: function (data) {
                    ResetFormData();
                    ShowMessage(data.Message, true);
                    $("#LangaugeParmeterGrid").data("kendoGrid").dataSource.read();
                    $("#hide").hide();
                    $("#show").fadeIn(500);

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

function LoadLanguage() {
    $("#LanguageID").empty();
    $("#LanguageID").append('<option value>--Select--</option>')
    $.ajax({
        url: "/Admin/DropDown/GetAllLanguage",
        type: 'POST',
        dataType: 'json',
        async: true,
        global: false,
        success: function (data) {
            jQuery.each(data, function (index, value) {
                $("#LanguageID").append('<option value=' + value.ID + '>' + value.Name + '</option>')
            })

        },

        error: function (response) {

            ShowMessage("Warning! Error Occured");  //
        }



    })

}

function Edit(e) {
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    debugger;
    e.preventDefault();

    $.ajax({
        url: "/Admin/LanguageParameter/EditLanguageParameter",
        data: { id: dataItem.ID },
        type: "POST",
        dataType: "json",
        success: function (result) {
            $("#ID").val(result.ID);
            $('#Page').val(result.Page);
            $('#Key').val(result.Key);
            $("#OriginalWordInEnglish").val(result.OriginalWordInEnglish);
            $('#TranslatedWord').val(result.TranslatedWord);
            $("#LanguageID").val(result.LanguageID);
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




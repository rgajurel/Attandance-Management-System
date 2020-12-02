$(document).ready(function () {

    $("#Class").change(function () {
        var classs = $("#Class").val();
        GetFacultyBasedOnClass(classs);      

    });

    $("#Faculty").change(function () {
        var classs = $("#Class").val();
        var faculty = $("#Faculty").val();
        GetSectionBasedOnClassAndFaculty(classs, faculty);

    });

    $("#commonFeeSubmit").off().on('click', function (e)
    {
        if (!$('form#formCommonFeeEntry').data('unobtrusiveValidation').validate()) {
            e.preventDefault();
            return false;
        }
        else {
            var formData = new FormData();
            formData.append("ID", $("#ID").val());
            formData.append("Session", $("#Session").val());
            formData.append("Faculty", $("#Faculty").val());
            formData.append("Class", $("#Class").val());
            formData.append("Section", $("#Section").val());
            formData.append("Type", $("#Type").val());
            formData.append("Month", $("#Month").val());
            formData.append("Fee", $("#Fee").val());
            formData.append('__RequestVerificationToken', $('form input[name=__RequestVerificationToken]').val());
            $.ajax({
                url: "/Admin/CommonFeeEntry/SaveCommonFee",
                type: 'POST',
                data: formData,
                dataType: 'json',
                contentType: false,
                processData: false,
                success: function (data) {
                    $("#window").data("kendoWindow").close();
                    ShowMessage(data.Message);
                    ResetFormData();
                    $("#commonFeeGrid").data("kendoGrid").dataSource.read();
                    document.getElementsByClassName("panel-title")[0].innerHTML = "Add Common Fee";
                }
            })
        }
    });  



    $("#FieldFilterClass").keyup(function () {
        var value = $("#FieldFilterClass").val();
        var value1 = $("#FieldFilterType").val();
        grid = $("#commonFeeGrid").data("kendoGrid");
        if (value) {
            //
            if (value1) {
                grid.dataSource.filter({
                    logic: "and",
                    filters: [
                      { field: "Class", operator: "startswith", value: value },
                      { field: "Type", operator: "startswith", value: value1 }


                    ]
                });
            } else {
                grid.dataSource.filter({ field: "Class", operator: "startswith", value: value });
            }

            


        } else {
            grid.dataSource.filter({});
        }
    });

    $("#FieldFilterType").keyup(function () {
        var value = $("#FieldFilterType").val();
        var value1 = $("#FieldFilterClass").val();
        grid = $("#commonFeeGrid").data("kendoGrid");
        if (value) {
            if (value1) {
                grid.dataSource.filter({
                    logic: "and",
                    filters: [
                        { field: "Type", operator: "startswith", value: value },
                        { field: "Class", operator: "startswith", value: value1 }

                    ]

                });
            } else {
                grid.dataSource.filter({ field: "Type", operator: "startswith", value: value });
            }

            

        } else {
            grid.dataSource.filter({});
        }
    });

});

function Edit(e) {
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    e.preventDefault();
    
    $.ajax({
        url: "/Admin/CommonFeeEntry/EditCommonFee",
        data: { id: dataItem.ID },
        type: "POST",
        dataType: "json",

        success: function (result)
        {
            $("#ID").val(result.ID);
            $("#Session").val(result.Session)           
            $("#Class").val(result.Class)
            GetFacultyBasedOnClass(result.Class, result.Faculty)
            GetSectionBasedOnClassAndFaculty(result.Class, result.Faculty, result.Section)          
          
            $("#Type").val((result.Type))
            $("#Month").val(result.Month)
            $("#Fee").val(result.Fee)
            var heading = document.getElementsByClassName("panel-title")[0].innerHTML = "Edit Common Fee";

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
            url: "/Admin/CommonFeeEntry/DeleteCommonFee",
            data: { id: dataItem.ID },
            type: 'POST',
            dataType: 'json',
            success: function (result) {

                $("#window").data("kendoWindow").close();
                ShowMessage(result.Message);
                ResetFormData();
                $("#commonFeeGrid").data("kendoGrid").dataSource.read();

            }
        });

    });

    $("#no").off().on('click', function (e) {

        $("#window").data("kendoWindow").close();
    });

}

function GetFacultyBasedOnClass(classs, faculty) {
    $("#Faculty").empty();
    $("#Faculty").append('<option value>--Select--</option>')
    $.ajax({
        url: "/Admin/Students/GetFacultyBaseOnClass",
        type: 'POST',
        dataType: 'json',
        data: {
            ID: classs,
        },

        success: function (data)
        {
           
            jQuery.each(data, function (index, value)
            {
               
                if (value.ID === parseInt(faculty)) {
                    $("#Faculty").append('<option selected value=' + value.ID + '>' + value.Faculty + '</option>')
                }
                else {
                    $("#Faculty").append('<option value=' + value.ID + '>' + value.Faculty + '</option>')
                }

            });


        }
    })

}

function GetSectionBasedOnClassAndFaculty(classs, faculty, section) {
    $("#Section").empty();
    $("#Section").append('<option value>--Select--</option>')
    $.ajax({
        url: "/Admin/Students/GetSectionBaseOnClassAndFaculty",
        type: 'POST',
        dataType: 'json',
        data: {
            ClassID: classs,
            FacultyID: faculty
        },

        success: function (data) {
            var sectionArray = data.Sections.split(',');


            jQuery.each(sectionArray, function (index, value) {

                if (value === section) {
                    $("#Section").append('<option selected value=' + value + '>' + value + '</option>')
                }
                else {
                    $("#Section").append('<option value=' + value + '>' + value + '</option>')
                }

            });

        }
    })

}









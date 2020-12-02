$(document).ready(function () {
    LoadOrgainsation();
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

        if (!$('form#formDepartment').data('unobtrusiveValidation').validate()) {
            e.preventDefault();
            return false;
        }
        else {

            $.ajax({
                url: "/Admin/Department/SaveDepartment",
                type: 'POST',
                data: AddAntiForgeryToken({
                    ID: $('#ID').val(),
                    DepartmentName: $('#DepartmentName').val(),
                    OrganisationID: $('#OrganisationID').val(),


                }),
                dataType: 'json',
                success: function (data) {
                    ResetFormData();                   
                    ShowMessage(data.Message,true);
                    $("#DepartmentGrid").data("kendoGrid").dataSource.read();
                    $("#hide").hide();
                    $("#show").fadeIn(500);
                   




                }
            })
        }
    });

    $("#FieldFilter").keyup(function () {
        var value = $("#FieldFilter").val();
        grid = $("#DepartmentGrid").data("kendoGrid");
        rowNumber = 0;
        if (value) {
            grid.dataSource.filter({ field: "DepartmentName", operator: "contains", value: value });

        } else {
            grid.dataSource.filter({});
        }
    });

});

function Edit(e) {
   
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    e.preventDefault();
  
    
    $.ajax({
        url: "/Admin/Department/EditDepartment",
        data: { id: dataItem.ID },
        type: "POST",
        dataType: "json",

        success: function (result) {

            $("#ID").val(result.ID);
            $('#OrganisationID').val(result.OrganisationID);
            $("#DepartmentName").val(result.DepartmentName);
           
            $("#hide").fadeIn(500);
            $("#show").hide();

        },
        error: function (result) {

            ShowMessage('Error Occured',false);
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
            url: "/Admin/Department/DeleteDepartment",
            data: { id: dataItem.ID },
            type: 'POST',
            dataType: 'json',
            success: function (result) {
                $("#window").data("kendoWindow").close();
                ShowMessage(result.Message,true);
                ResetFormData();
                $("#DepartmentGrid").data("kendoGrid").dataSource.read();

            }
        });

    });

    $("#no").off().on('click', function (e) {

        $("#window").data("kendoWindow").close();
    });

}
function LoadOrgainsation() {
    $("#OrganisationID").empty();
    $("#OrganisationID").append('<option value>--Select--</option>')
    $.ajax({
        url: "/Admin/DropDown/GetOrganisationDropDown",
        type: 'POST',
        dataType: 'json',
        global: false,
        success: function (data) {

          
            jQuery.each(data, function (index, value) {
                $("#OrganisationID").append('<option value=' + value.ID + '>' + value.Name + '</option>')
            })

        },

        error: function (response) {

            ShowMessage("Warning! Error Occured",false);  //
        }



    })

}



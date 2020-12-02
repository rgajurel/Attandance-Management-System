$(document).ready(function () {

    $("#salaryHeadCancel").off().on('click', function (e)
    {
             
        ResetFormData();
    })

    $("#salaryHeadAdd").off().on('click', function (e)
    {
        debugger;
        var IsAdd = false;
        var IsSaving=false;
        var IsTax=false;
        var IsSalaryCalculatePoint = false;
        var IsBasicSalary = false;

        var IsAdd = $('form#formSalaryHead').find('#IsAdd').is(':checked');
        if (IsAdd)
        {
            IsAdd = true;
        }

        var IsBasicSalary = $('form#formSalaryHead').find('#IsBasicSalary').is(':checked');
        if (IsBasicSalary)
        {
            IsBasicSalary = true;
        }

        var IsSaving = $('form#formSalaryHead').find('#IsSaving').is(':checked');
        if (IsSaving)
        {
            IsSaving = true;
        }

        var IsTax = $('form#formSalaryHead').find('#IsTax').is(':checked');
        if (IsTax)
        {
            IsTax = true;
        }

        var IsSalaryCalculatePoint = $('form#formSalaryHead').find('#IsSalaryCalculatePoint').is(':checked');
        if (IsSalaryCalculatePoint)
        {
            IsSalaryCalculatePoint = true;
        }

        
        if (!$('form#formSalaryHead').data('unobtrusiveValidation').validate()) {
            e.preventDefault();
            return false;
        }
        else {

            $.ajax({
                url: "/Admin/SalaryHeadings/SaveSalaryHeadings",
                type: 'POST',
                data: AddAntiForgeryToken({
                    ID: $('#ID').val(),
                    HeadName: $('#HeadName').val(),
                    SortOrder: $('#SortOrder').val(),
                    IsAdd: IsAdd,
                    IsSaving: IsSaving,
                    IsTax: IsTax,
                    IsBasicSalary:IsBasicSalary,
                    IsSalaryCalculatePoint: IsSalaryCalculatePoint
                }),
                dataType: 'json',
                success: function (data) {

                    ResetFormData();
                    ShowMessage(data.Message,true);
                    $("#SalaryHeadingGrid").data("kendoGrid").dataSource.read();                    
                }
            })
        }
    });

    $("#FieldFilter").keyup(function () {
        var value = $("#FieldFilter").val();
        grid = $("#SalaryHeadingGrid").data("kendoGrid");
        rowNumber = 0;
        if (value) {
            grid.dataSource.filter({ field: "HeadName", operator: "contains", value: value });

        } else {
            grid.dataSource.filter({});
        }
    });


})

function Edit(e)
{
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    e.preventDefault();
    

    $.ajax({
        url: "/Admin/SalaryHeadings/EditSalaryHeadings",
        data: { id: dataItem.ID },
        type: "POST",
        dataType: "json",

        success: function (result)
        {
            $("#ID").val(result.ID);
            $("#HeadName").val(result.HeadName);
            $("#SortOrder").val(result.SortOrder);
            if (result.IsBasicSalary == true) {
                $('#IsBasicSalary').prop('checked', true);

            }
            if (result.IsBasicSalary == false) {
                $('#IsBasicSalary').prop('checked', false);

            }

            if (result.IsAdd == true)
            {
                $('#IsAdd').prop('checked', true);
                
            }
             if (result.IsAdd == false)
             {
                $('#IsAdd').prop('checked', false);

            }


             if (result.IsSaving == true)
             {
                $('#IsSaving').prop('checked', true);

             }
             if (result.IsSaving == false) {
                 $('#IsSaving').prop('checked', false);

             }

             if (result.IsTax == true)
             {
                $('#IsTax').prop('checked', true);

             }
             if (result.IsTax == false) {
                 $('#IsTax').prop('checked', false);

             }

             if (result.IsSalaryCalculatePoint == true)
             {
                $('#IsSalaryCalculatePoint').prop('checked', true);

             }
             if (result.IsSalaryCalculatePoint == false) {
                 $('#IsSalaryCalculatePoint').prop('checked', false);

             }

            

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
            url: "/Admin/SalaryHeadings/DeleteSalaryHeading",
            data: { id: dataItem.ID },
            type: 'POST',
            dataType: 'json',
            success: function (result) {

                $("#window").data("kendoWindow").close();
                ShowMessage(result.Message);
                ResetFormData();
                $("#SalaryHeadingGrid").data("kendoGrid").dataSource.read();
                document.getElementsByClassName("panel-title")[0].innerHTML = "Add Salary Head";

            }
        });

    });

    $("#no").off().on('click', function (e) {

        $("#window").data("kendoWindow").close();
    });

}



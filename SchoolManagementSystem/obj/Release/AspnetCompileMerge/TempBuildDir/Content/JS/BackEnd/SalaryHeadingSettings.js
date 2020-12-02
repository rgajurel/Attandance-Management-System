$(document).ready(function () {

    $("#JobTypeID").change(function () {

       
        $("#SalaryHeadingSettingsGrid").data("kendoGrid").dataSource.read();
       
    })

    $("#salaryHeadSettingsAdd").off().on('click', function (e)
    {
      
        if (!$('form#formSalaryHeadingSettings').data('unobtrusiveValidation').validate())
        {
            e.preventDefault();
            return false;
        }
        else 
        {
            var dataItem;
            dataItem = $("#SalaryHeadingSettingsGrid").data("kendoGrid").dataSource.data();
            var settingsHeaderList;
            for (i = 0; i < dataItem.length; i++)
            {

                firstItem = $('#SalaryHeadingSettingsGrid').data().kendoGrid.dataSource.data()[i];
                firstItem["JobTypeID"] = $("#JobTypeID").val();

                $('#SalaryHeadingSettingsGrid').data('kendoGrid').refresh();
                // $("#marksEntrySearch").hide();                
                settingsHeaderList = $("#SalaryHeadingSettingsGrid").data("kendoGrid").dataSource.data();               
             
            }
            $('#SalaryHeadingSettingsGrid').data('kendoGrid').refresh();
            $.ajax({
                url: "/Admin/SalaryHeadingSettings/SaveSalaryHeadingSettings",
                type: 'POST',
                data: AddAntiForgeryToken({ data: JSON.stringify(settingsHeaderList) }),
                dataType: 'json',
                success: function (data) {
                    ShowMessage(data.Message);


                }
            })
        }

    })



    $("#salaryHeadCancel").off().on('click', function (e) {

        document.getElementsByClassName("panel-title")[0].innerHTML = "Add Salary Head";
        ResetFormData();
    })

    $("#salaryHeadAdd").off().on('click', function (e) {
        var IsAdd = false;
        var IsSaving = false;
        var IsTax = false;
        var IsSalaryCalculatePoint = false;

        var IsAdd = $('form#formSalaryHead').find('#IsAdd').is(':checked');
        if (IsAdd) {
            IsAdd = true;
        }

        var IsSaving = $('form#formSalaryHead').find('#IsSaving').is(':checked');
        if (IsSaving) {
            IsSaving = true;
        }

        var IsTax = $('form#formSalaryHead').find('#IsTax').is(':checked');
        if (IsTax) {
            IsTax = true;
        }

        var IsSalaryCalculatePoint = $('form#formSalaryHead').find('#IsSalaryCalculatePoint').is(':checked');
        if (IsSalaryCalculatePoint) {
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
                    IsAdd: IsAdd,
                    IsSaving: IsSaving,
                    IsTax: IsTax,
                    IsSalaryCalculatePoint: IsSalaryCalculatePoint
                }),
                dataType: 'json',
                success: function (data)
                {                                      
                    $("#SalaryHeadingSettingsGrid").data("kendoGrid").dataSource.read();
                    ShowMessage(data.Message);                  
                    document.getElementsByClassName("panel-title")[0].innerHTML = "Add Salary Head";
                }
            })
        }
    });

   


})

function Edit(e) {
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    e.preventDefault();
    document.getElementsByClassName("panel-title")[0].innerHTML = "Edit Salary Head";

    $.ajax({
        url: "/Admin/SalaryHeadings/EditSalaryHeadings",
        data: { id: dataItem.ID },
        type: "POST",
        dataType: "json",

        success: function (result) {
            $("#ID").val(result.ID);
            $("#HeadName").val(result.HeadName);

            if (result.IsAdd == true) {
                $('#IsAdd').prop('checked', true);

            }
            if (result.IsAdd == false) {
                $('#IsAdd').prop('checked', false);

            }


            if (result.IsSaving == true) {
                $('#IsSaving').prop('checked', true);

            }
            if (result.IsSaving == false) {
                $('#IsSaving').prop('checked', false);

            }

            if (result.IsTax == true) {
                $('#IsTax').prop('checked', true);

            }
            if (result.IsTax == false) {
                $('#IsTax').prop('checked', false);

            }

            if (result.IsSalaryCalculatePoint == true) {
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
function ParamToSettingsHeadList(e) {
    var grid = $("#SalaryHeadingSettingsGrid").data("kendoGrid").dataSource;
    return {
        JobTypeID: $("#JobTypeID :selected").val() == "" ? -1 : $("#JobTypeID :selected").val(),


        // TermID: $("#TermID").val() == "" ? -1 : $("#TermID").val(),      
        //pageSize: grid._pageSize,
        //pageNumber: grid._page
    };

}
function Delete(e)
{
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    $(e.currentTarget).closest("tr").remove();   

}



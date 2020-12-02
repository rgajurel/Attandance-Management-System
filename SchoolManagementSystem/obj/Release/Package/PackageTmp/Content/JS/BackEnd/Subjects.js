$(document).ready(function () {
    $(document).ready(function ()
    {
        var index = 1;
        $('#NepaliJoioningDate').nepaliDatePicker({
            ndpEnglishInput: 'EnglishJoioningDate',
            npdMonth: true,
            npdYear: true,
            npdYearCount: -25
        });

        $(".create").off().on('click', function (e) {
            $("#hide").fadeIn(500);
            $("#addSubjects").show();
            $("#show").hide();
        });
        $(".cancel").off().on('click', function (e) {

            $("#hide").hide();
            $("#show").fadeIn(500);
            ResetFormData();



        });
        $("#subjectCancel").off().on('click', function (e) {

            $("#hide").hide();
            $("#show").fadeIn(500);
            $("#container").find("tr:gt(1)").remove();
            ResetFormData();
        });
        $(document).on('click', '.trashdelete', function () {

            $(this).closest('tr').remove();
            return false;
        });

        $("#addSubjects").off().on('click', function (e) {

            $("#container").append("<tr> <td style='display:none'><input class='form-control classid' data-val='true' data-val-required='This field is Required' id='SubjectList[" + index + "].ClassID' name='SubjectList[" + index + "].ClassID' type='hidden' value=''><span class='field-validation-valid text-danger' data-valmsg-for='SubjectList[" + index + "].ClassID' data-valmsg-replace='true'></span></td><td><input class='form-control subjectcode' data-val='true' data-val-required='This field is Required' id='SubjectList[" + index + "].SubjectCode' name='SubjectList[" + index + "].SubjectCode' pattern='[0-99]' type='text' value='' style='text-transform:uppercase'><span class='field-validation-valid text-danger' data-valmsg-for=SubjectList[" + index + "].SubjectCode data-valmsg-replace='true'></span></td><td><input class='form-control subjectname1' data-val='true' data-val-required='This field is Required' id='SubjectList[" + index + "].SubjectName' name='SubjectList[" + index + "].SubjectName' pattern='[0-99]' type='text' value=''><span class='field-validation-valid text-danger' data-valmsg-for='SubjectList[" + index + "].SubjectName' data-valmsg-replace='true'></span></td><td><input class='form-control creditpoints' data-val='true' data-val-required='This field is Required' id='SubjectList[" + index + "].CreditPoints' name='SubjectList[" + index + "].CreditPoints' pattern='[0-99]' type='number' value=''><span class='field-validation-valid text-danger' data-valmsg-for='SubjectList[" + index + "].CreditPoints' data-valmsg-replace='true'></span></td><td><a href='#'><i class='fa fa-trash trashdelete' aria-hidden='true' style='margin-left:20px; color:#d43f3a' ></i></a></td></tr>");
            index++;

            // ResetFormData();



        });

        $("#ClassID").change(function () {

            $(".classid").val(($("#ClassID").val()));

        });

        $("#subjectSubmit").off().on('click', function (e) {
            $('input.subjectname1').each(function () {


                $(this).rules("add",
                    {

                        required: true,


                    })
            });
            $('input.subjectcode').each(function () {


                $(this).rules("add",
                    {

                        required: true,


                    })
            });
            $('input.creditpoints').each(function () {


                $(this).rules("add",
                    {

                        required: true,


                    })
            });
            $(".classid").val(($("#ClassID").val()));


        })


        $("#FieldFilter").keyup(function () {
            var value = $("#FieldFilter").val();
            $("#SubjectGrid").data("kendoGrid").dataSource.filter({
                logic: "or",
                filters: [
                    {
                        field: "SubjectCode",
                        operator: "contains",
                        value: value
                    },
                    {
                        field: "SubjectName",
                        operator: "contains",
                        value: value
                    }
                ]
            });


        });

        $(document).on("keyup", ".subjectcode", function () {


            var subjectcode = $(this).val().toUpperCase();;
            var data = $("#SubjectGrid").data("kendoGrid").dataSource.data();
            for (i = 0; i < data.length; i++) {

                if (data[i].SubjectCode == subjectcode) {
                    ShowMessage("Warning ! Subject Code Already Exist");
                    this.value = "";

                }
            }

        });
    });
    
});


function OnBegin(response)
{
   
    if (!$('form#form0').validate().form())
    {
        return false;
    }
    else {

       
        return true;
    }

}

function OnSuccess(response)
{
    $("#SubjectGrid").data("kendoGrid").dataSource.read();
    ShowMessage(response.Message);
    $("#container").find("tr:gt(1)").remove();
    $("#hide").hide();
    $("#show").fadeIn(500);
    ResetFormData();
}
function OnFailure(response)
{
    ShowMessage("Warning ! Error Occured");
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
            url: "/Admin/Subjects/DeleteSubjects",
            data: { id: dataItem.ID },
            type: 'POST',
            dataType: 'json',
            success: function (result)
            {
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
function Edit(e)
{
   
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    e.preventDefault();
    

    $.ajax({
        url: "/Admin/Subjects/EditSubject",
        data: { id: dataItem.ID },
        type: "POST",
        dataType: "json",

        success: function (result)
        {

            $("#container").find("tr:gt(1)").remove();
          $('.id').val(result.ID)
           $('.subjectcode').val(result.SubjectCode)
             $('.subjectname').val(result.SubjectName)
            $('#ClassID').val(result.ClassID),
             $('.classid').val(result.ClassID),
            $('.creditpoints').val(result.CreditPoints)      
            
            
            $("#hide").fadeIn(500);
            $("#show").hide();
            $("#addSubjects").hide();
            $("html, body").animate({ scrollTop: 0}, 1000);
            return false;
        },

        error: function (result) {

            ShowMessage('Error Occured');
        }
    });

}







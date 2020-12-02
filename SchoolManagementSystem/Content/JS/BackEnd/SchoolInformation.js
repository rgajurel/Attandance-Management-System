$(document).ready(function () {

     
    $(".create").off().on('click', function (e) {
        $("#hide").fadeIn(500);
        $("#show").hide();
    })
    $(".cancel").off().on('click', function (e) {
       
        $("#imageupload").attr("src", "/Content/Images/School/School.png");
        $("#hide").hide();
        $("#show").fadeIn(500);
        $("#IsMainBranch").prop('selectedIndex', 0);
        ResetFormData();
    });

    $("#inputFile").change(function () {
        readURL(this);
    });

    $("#schoolInformationSubmit").off().on('click', function (e) {

      
        if (!$('form#formSchoolDetails').data('unobtrusiveValidation').validate()) {
            e.preventDefault();
            return false;
        }
        else {

            var formData = new FormData();
            formData.append("Name", $("#Name").val());
            formData.append("ID", $("#ID").val());
            formData.append("Address", $("#Address").val());
            formData.append("Email", $("#Email").val());
            formData.append("IsMainBranch", $("#IsMainBranch").val());
            formData.append("Phone", $("#Phone").val());
            formData.append("Mobile", $("#Mobile").val());
            formData.append("Fax", $("#Fax").val());
            formData.append("ContactPerson", $("#ContactPerson").val());
            formData.append("RegistrationNo", $("#RegistrationNo").val());
            formData.append("EstablishedYear", $("#EstablishedYear").val());
            formData.append("SchoolTypeID", $("#SchoolTypeID").val());
            formData.append("imageFile", $('#inputFile')[0].files[0]);
            formData.append('__RequestVerificationToken', $('form input[name=__RequestVerificationToken]').val());
            $.ajax({
                url: "/Admin/OrganisationInformation/SaveSchoolInformation",
                type: 'POST',
                data: formData,
                dataType: 'json',
                contentType: false,
                processData: false,
                success: function (data)
                {
                    $("#hide").hide();
                    $("#show").fadeIn(500);
                    $("#inputFile").val('');
                    ShowMessage(data.Message,true);
                    ResetFormData();
                    $("#IsMainBranch").prop('selectedIndex', 0);
                    $("#imageupload").attr("src", "/Content/Images/School/School.png")
                    $("#SchoolInformationGrid").data("kendoGrid").dataSource.read();

                }
            })
        }
    });

    $("#IsMainBranch").change(function () {


        var ismainbranch = $("#IsMainBranch").val();

        CheckIfAlreadyMainBranch(ismainbranch);
      

    });
    $("#FieldFilter").keyup(function () {
        var value = $("#FieldFilter").val();
        grid = $("#SchoolInformationGrid").data("kendoGrid");
        rowNumber = 0;
        if (value) {
            grid.dataSource.filter({ field: "Name", operator: "contains", value: value });

        } else {
            grid.dataSource.filter({});
        }
    });
})
   
function CheckIfAlreadyMainBranch(ismainbranch) {

    
    $.ajax({
        url: "/Admin/OrganisationInformation/CheckIfMainBranchExist",
        type: 'POST',
        dataType: 'json',
        data: {
            IsMainBranch: ismainbranch,
        },
        global: false,
        success: function (data)
        {
            
            if (data != null || data != "")
            {
                ShowMessage(data.Message,false);
                $("#IsMainBranch").prop('selectedIndex', 0);
            }
           


        }     


    })

}

function readURL(input)
{
  
    var fileExtension = "png";
 

    var ext = $('input[name=inputFile]').val();

    var fileSplitArray = ext.split('.')[1];

  
    if (input.files && input.files[0])
    {
            var reader = new FileReader();

            reader.onload = function (e)
            {
                $('#imageupload').attr('src', e.target.result);
            }

            reader.readAsDataURL(input.files[0]);
            return true;
        }

       

  
    //else {

     //  document.getElementById("inputFile").value = "";
     //  ShowMessage("Warning !!Only Upload Png Images")

   // }
    
}


function Delete(e)
{
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    e.preventDefault();
    $("#window").kendoWindow({
        modal: true
    });
    $("#window").data("kendoWindow").open().center();

    $("#yes").off().on('click', function (e) {

        $.ajax({
            url: "/Admin/OrganisationInformation/DeleteSchoolInformation",
            data: { id: dataItem.ID },
            type: 'POST',
            dataType: 'json',
            success: function (result) {
             
                $("#window").data("kendoWindow").close();
                ShowMessage(result.Message,true);
                ResetFormData();
                $("#imageupload").attr("src", "/Content/Images/School/School.png")
                $("#SchoolInformationGrid").data("kendoGrid").dataSource.read();

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
        url: "/Admin/OrganisationInformation/EditSchoolInformation",
        data: { id: dataItem.ID },
        type: "POST",
        dataType: "json",
        success: function (result) 
        {
            $("#hide").fadeIn(500);
            $("#show").hide();
            $("#ID").val(result.ID);
            $("#Address").val(result.Address);
               $("#Name").val(result.Name);
                  $("#Email").val(result.Email);
                     $("#Phone").val(result.Phone);
   $("#Mobile").val(result.Mobile);
   $("#Fax").val(result.Fax);
   $("#ContactPerson").val(result.ContactPerson);
   $("#RegistrationNo").val(result.RegistrationNo);
   $("#EstablishedYear").val(result.EstablishedYear);
   $("#SchoolTypeID").val(result.SchoolTypeID);
   $("#IsMainBranch").val(result.IsMainBranch);
   $("#imageupload").attr("src", result.Image);
   $('#inputFile').attr(result.Image);
   
     },       
        error: function (result)
        {

            ShowMessage('Error Occured',false);
        }
    });

}



   
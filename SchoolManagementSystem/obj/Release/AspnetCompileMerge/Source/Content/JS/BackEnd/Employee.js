$(document).ready(function () {
     CheckDate();  

    $('#NepaliJoioningDate').val(AD2BS($('#EnglishJoioningDate').val()));
    $('#NepaliDateOfBirth').val(AD2BS($('#EnglishDateOfBirth').val()));
    
    $(".create").off().on('click', function (e) {
        $("#hide").fadeIn(500);
        $("#show").hide();      
    })
    $(".cancel").off().on('click', function (e) {
        
        $("#hide").hide();
        $("#show").fadeIn(500);
        $('#OrganisationID').prop('selectedIndex', 0);
        ResetFormData1();
        GetDeviceUserID();
        InitialDate();
    });

    $("#studentsList").fadeIn(800);
    $("#studentsForm").hide();
    $("#details").hide();
    GetDeviceUserID();
    LoadOrgainsation();
    ShowPersonelHideOthers();

    $('#NepaliJoioningDate').nepaliDatePicker({
        ndpEnglishInput: 'EnglishJoioningDate',
        npdMonth: true,
        npdYear: true,
        npdYearCount: -25
    });

    $('#NepaliDateOfBirth').nepaliDatePicker({
        ndpEnglishInput: 'EnglishDateOfBirth',
        npdMonth: true,
        npdYear: true,
        npdYearCount: -25
    });

    $('#NepaliJoioningDate').change(function () {
        $('#EnglishJoioningDate').val(BS2AD($('#NepaliJoioningDate').val()));
    });

    $('#NepaliDateOfBirth').change(function () {
        $('#EnglishDateOfBirth').val(BS2AD($('#NepaliDateOfBirth').val()));
    });
   





    $("a[href=#contacts]").click(function () {
        $("#personalinfo").hide();
        $("#parentsdetails").hide();
        $("#previousqualification").hide();
        $("#contactsdetails").show();


    })

    $("#personal").off().on('click', function (e) {
        ShowPersonelHideOthers();

    });

    $("#parents").off().on('click', function (e) {

        $("#personalinfo").hide();
        $("#contactsdetails").hide();
        $("#parentsdetails").show();
        $("#previousqualification").hide();

    });


    $("#EntryTime").timepicker({
        timeFormat: 'HH:mm:ss '
    });
    $("#ExitTime").timepicker({
        timeFormat: 'HH:mm:ss'
    });

    $(document).bind('keypress', function (e) {

        if (e.keyCode == 13) {
            $('#employeeSearch').trigger('click');
        }
    });

    $('#employeeSearch').click(function () {
        //var grid = $("#EmployeeListGrid").data("kendoGrid");
        // grid.dataSource.transport.options.read.global = false;
        $("#EmployeeListGrid").data("kendoGrid").dataSource.read();

    });

    $("#employeeSubmit").off().on('click', function (e) {


        if (!$('form#formEmployee').data('unobtrusiveValidation').validate()) {
            e.preventDefault();
            return false;
        }
        else {
            var formData = new FormData();
            formData.append("ID", $("#ID").val());
            formData.append("Name", $("#Name").val());
            formData.append("Gender", $("#Gender").val());
            formData.append("EnglishJoioningDate", $("#EnglishJoioningDate").val());
            formData.append("NepaliJoioningDate", $("#NepaliJoioningDate").val());
            formData.append("Qualifications", $("#Qualifications").val());
            formData.append("EnglishDateOfBirth", $("#EnglishDateOfBirth").val());
            formData.append("NepaliDateOfBirth", $("#NepaliDateOfBirth").val());
            formData.append("Email", $("#Email").val());
            formData.append("MobileNo", $("#MobileNo").val());
            formData.append("PhoneNo", $("#PhoneNo").val());
            formData.append("CitizenshipNo", $("#CitizenshipNo").val());
            formData.append("imageFile", $('#inputFile')[0].files[0]);
            formData.append("UserID", $("#UserID").val());
            formData.append("OrganisationID", $("#OrganisationID").val());
            formData.append("DepartmentID", $("#DepartmentID").val());
            formData.append("DesignationID", $("#DesignationID").val());
            formData.append("JobTypeID", $("#JobTypeID").val());
            formData.append("EntryTime", $("#EntryTime").val());
            formData.append("ExitTime", $("#ExitTime").val());
            formData.append("Status", $("#Status").val());
            formData.append("FatherName", $("#FatherName ").val());
            formData.append("PermanentAddress", $("#PermanentAddress").val());
            formData.append("TemporaryAddress", $("#TemporaryAddress").val());
            formData.append("EmpCode", $("#EmpCode").val());
            formData.append("PFNumber", $("#PFNumber").val());
            formData.append("CITNumber", $("#CITNumber").val());
            formData.append("BankAccountNo", $("#BankAccountNo").val());
            formData.append("PANNumber", $("#PANNumber").val());


            formData.append('__RequestVerificationToken', $('form input[name=__RequestVerificationToken]').val());
            $.ajax({
                url: "/Admin/Employer/SaveEmployer",
                type: 'POST',
                data: formData,
                dataType: 'json',
                contentType: false,
                processData: false,
                success: function (data) {
                    ResetFormData1();
                    $("#hide").hide();
                    $("#show").fadeIn(500);
                    ShowMessage(data.Message,true);
                    $("#EmployeeListGrid").data("kendoGrid").dataSource.read();
                    InitialDate();


                },
                error: function (response) {
                    ShowMessage("Error Occured",false)
                }
            })
        }
    });

    $("#studentCancel").off().on('click', function (e) {
        ResetFormData1();
    });


    $("#employeeFormShow").off().on(' click', function (e) {

        $("#employeeForm").fadeIn(800);
        $("#employeeList").hide();

    })



    $("#OrganisationSearchID").change(function () {


        var organisation = $("#OrganisationSearchID").val();

        GetDepartmentBasedOnOrganisation(organisation);
        GetDesignationBasedOnOrganisation(organisation);


    });
    $("#OrganisationID").change(function () {


        var organisation = $("#OrganisationID").val();

        GetDepartmentBasedOnOrganisation1(organisation, null);
        GetDesignationBasedOnOrganisation1(organisation, null);


    });
  

});

function InitialDate() {
    var date = GetTodayDate();
    $('#EnglishJoioningDate').val(date);
    $('#EnglishDateOfBirth').val(date);

    $('#NepaliJoioningDate').val(AD2BS($('#EnglishJoioningDate').val()));
    $('#NepaliDateOfBirth').val(AD2BS($('#EnglishDateOfBirth').val()));

}
function ResetFormData1() {
    $('input:checkbox').removeAttr('checked');
    $("#ID").val("0");
    $("#DepartmentID").empty();
    $("#DepartmentID").append('<option value>--Department--</option>')
    $("#DesignationID").empty();
    $("#DesignationID").append('<option value>--Designation--</option>')
    $("input[type='text'], textarea,input[type='number'], input[type='password'],input[type='checkbox']").each(
      function ()
      {
          $(this).val('');

      }
    );
}
function Edit(e)
{
  
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    e.preventDefault();  

    $.ajax({
        url: "/Admin/Employer/EditEmployer",
        data: { id: dataItem.ID },
        type: "POST",
        dataType: "json",
        success: function (result)
        {
            GetDepartmentBasedOnOrganisation1(result.OrganisationID, result.DepartmentID)            
            GetDesignationBasedOnOrganisation1(result.OrganisationID, result.DesignationID)           

            $("#ID").val(result.ID);
            $("#Gender").val(result.Gender)
            $("#Name").val(result.Name)
            $("#RegistrationNo").val(result.RegistrationNo)

            $("#EnglishJoioningDate").val(ConvertDateObjectToDate(result.EnglishJoioningDate))
            $("#NepaliJoioningDate").val(ConvertDateObjectToDate(result.NepaliJoioningDate))

            $("#EnglishDateOfBirth").val(ConvertDateObjectToDate(result.EnglishDateOfBirth))
            $("#NepaliDateOfBirth").val(ConvertDateObjectToDate(result.NepaliDateOfBirth))

            
            

            $("#Qualifications").val(result.Qualifications);
            $("#Email").val(result.Email)
            $("#MobileNo").val(result.MobileNo)
            
            $("#PhoneNo").val(result.PhoneNo);
            $("#CitizenshipNo").val(result.CitizenshipNo)
            $("#UserID").val(result.UserID)
            $("#OrganisationID").val(result.OrganisationID);        

          

            $("#JobTypeID").val(result.JobTypeID);
            $("#EntryTime").val(result.EntryimeString);
            $("#ExitTime").val(result.ExitimeString);
            $("#Status").val(result.Status);

            $("#FatherName").val(result.FatherName);
            $("#PermanentAddress").val(result.PermanentAddress);
            $("#TemporaryAddress").val(result.TemporaryAddress);


            $("#EmpCode").val(result.EmpCode);
            $("#PFNumber").val(result.PFNumber);
            $("#CITNumber").val(result.CITNumber);
            $("#BankAccountNo").val(result.BankAccountNo);
            $("#PANNumber").val(result.PANNumber);


            $("#hide").fadeIn(500);
            $("#show").hide();
        },
        error: function (result)
        {           
            ShowMessage('Warning !Error Occured',false);
        }
    });

}

function resetRowNumber(e) {

    
    $(".k-grid-Edit").find("span").addClass("fa fa-pencil");
    $(".k-grid-Edit").removeClass("k-button");
    $(".k-grid-Delete").find("span").addClass("fa fa-trash");
    $(".k-grid-Delete").removeClass("k-button");
    $(".k-grid-Details").find("span").addClass("fa fa-eye");
    $(".k-grid-Details").removeClass("k-button");
    $(".k-grid-Download").find("span").addClass("fa fa-download");
    $(".k-grid-Download").removeClass("k-button");
    $(".k-grid-Approve").find("span").addClass("fa fa-check");
    $(".k-grid-Approve").removeClass("k-button");

    var grid = e.sender;
    if (grid.dataSource.total() == 0) {
        var colCount = 0;
        var columns = grid.columns;
        jQuery.each(columns, function (index) {
            if (!this.hidden) {
                colCount++;
            }
        });
        $(e.sender.wrapper)
            .find('tbody')
            .append('<tr class="kendo-data-row text-center"><td colspan="' + colCount + '" class="no-data">No Data Available</td></tr>');
    }
}
function Details(e) {

    $("#employeeList").hide();
    $("#employeeForm").hide();
    $("#details").show();

    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    e.preventDefault();

    $.ajax({
        url: "/Admin/Employer/DetailsEmployer",
        data: { id: dataItem.ID },
        type: 'POST',
        dataType: 'json',
        success: function (result)
        {
           

            if (result.NepaliJoioningDate == null) {
                $("#nepalijoin").text('Not Availiable')
            }
            else {
                $("#nepalijoin").text(ConvertDateObjectToDate(result.NepaliJoioningDate))
            }

             if (result.EnglishJoioningDate == null)
                {
                 $("#englishjoin").text('Not Availiable')
                }
                else
                {
                    $("#englishjoin").text(ConvertDateObjectToDate(result.EnglishJoioningDate))
                }

             if (result.EnglishDateOfBirth == null)
                {
                 $("#englbirth").text('Not Availiable')
                }
                else
                {
                    $("#englbirth").text(ConvertDateObjectToDate(result.EnglishDateOfBirth))
                }

             if (result.NepaliDateOfBirth == null)
                {
                 $("#nepalibirth").text('Not Availiable')
                }
                else
                {
                    $("#nepalibirth").text(ConvertDateObjectToDate(result.NepaliDateOfBirth))
                }

            if (result.Name == null) {
            $("#name").text("Not Availiable")
            }
            else {
                $("#name").text(result.Name)
            }

            if (result.Name == null) {
                $("#username").text("Not Availiable")
            }
            else {
                $("#username").text(result.Name)
            }


           
            if (result.Name == null) {
                $("#username1").text("Not Availiable")
            }
            else {
                $("#username1").text(result.Name)
            }


            
            if (result.Gender == null) {
                $("#gender").text("Not Availiable")
            }
            else {
                $("#gender").text(result.Gender)
            }

            
            if (result.Qualifications == null) {
                $("#qualifications").text("Not Availiable")
            }
            else {
                $("#qualifications").text(result.Qualifications)
            }


            if (result.Email == null) {
                $("#email").text("Not Availiable")
            }
            else {
                $("#email").text(result.Email)
            }


            if (result.MobileNo == null) {
                $("#mobile").text("Not Availiable")
            }
            else {
                $("#mobile").text(result.MobileNo)
            }


            if (result.PhoneNo == null) {
                $("#phone").text("Not Availiable")
            }
            else {
                $("#phone").text(result.PhoneNo)
            }


            if (result.CitizenshipNo == null) {
                $("#citizenship").text("Not Availiable")
            }
            else {
                $("#citizenship").text(result.CitizenshipNo)
            }


            if (result.Designation == null) {
                $("#designation").text("Not Availiable")
            }
            else {
                $("#designation").text(result.Designation)
            }


            if (result.UserID == null) {
                $("#userid").text("Not Availiable")
            }
            else {
                $("#userid").text(result.UserID)
            }



            if (result.Organisation == null) {
                $("#organisation").text("Not Availiable")
            }
            else {
                $("#organisation").text(result.Organisation)
            }


            if (result.DepartmentName == null) {
                $("#department").text("Not Availiable")
            }
            else {
                $("#department").text(result.DepartmentName)
            }


            if (result.Status == null) {
                $("#status").text("Not Availiable")
            }
            else {
                $("#status").text(result.Status)
            }

            if (result.JobTypeName == null) {
                $("#jobtype").text("Not Availiable")
            }
            else {
                $("#jobtype").text(result.JobTypeName)
            }


            if (result.EntryimeString == null) {
                 $("#entrytime").text("Not Availiable")
            }
            else {
                 $("#entrytime").text(result.EntryimeString)
             }


            if (result.ExitimeString == null) {
                 $("#exittime").text("Not Availiable")
             }
             else {
                $("#exittime").text(result.ExitimeString)
             }

            if (result.FatherName == null) {
                 $("#father").text("Not Availiable")
             }
             else {
                $("#father").text(result.FatherName)
            }


            
            if (result.TemporaryAddress == null) {
                $("#tempaddress").text("Not Availiable")
            }
            else {
                $("#tempaddress").text(result.TemporaryAddress)
            }

            if (result.PermanentAddress == null) {
                $("#permaddress").text("Not Availiable")
            }
            else {
                $("#permaddress").text(result.PermanentAddress)
            }

            $("#imageupload").attr("src", result.Image);


           

        },
        error: function (response)
        {
            ShowMessage('Warning !Error Occured',false);
        }
    });



}



function GetDepartmentBasedOnOrganisation1(organisation, department) {
   
    $("#DepartmentID").empty();
    $("#DepartmentID").append('<option value>--Department--</option>')
    $.ajax({
        url: "/Admin/Employer/GetDepartmentBasedOnOrganisation",
        type: 'POST',
        dataType: 'json',
        data: {
            ID: organisation,
        },
        global:false,
        success: function (data) {

            jQuery.each(data, function (index, value) {
                if (value.ID === department)
                {
                    $("#DepartmentID").append('<option selected value=' + value.ID + '>' + value.DepartmentName + '</option>')
                }
                else {
                $("#DepartmentID").append('<option value=' + value.ID + '>' + value.DepartmentName + '</option>')
                 }

            })

        },

        error: function (response) {

            ShowMessage("Warning! Error Occured",false);  //
        }



    })

}

function ShowPersonelHideOthers() {
    $("#personalinfo").show();
    $("#contactsdetails").hide();
    $("#parentsdetails").hide();
    $("#previousqualification").hide();
}

function GetDesignationBasedOnOrganisation1(organisation, designation) {
    $("#DesignationID").empty();
    $("#DesignationID").append('<option value>--Designation--</option>')
    $.ajax({
        url: "/Admin/Employer/GetDesignationBasedOnOrganisation",
        type: 'POST',
        dataType: 'json',
        data: {
            ID: organisation,
        },
        global:false,
        success: function (data) {


            jQuery.each(data, function (index, value) {

                if (value.ID === designation)
                {
                    $("#DesignationID").append('<option selected value=' + value.ID + '>' + value.Designation + '</option>')
                }
                else {
                    $("#DesignationID").append('<option value=' + value.ID + '>' + value.Designation + '</option>')
                 }

            });


        },
             error: function (response) 
            {
                ShowMessage("Error Occured",false);
            }   
    })

}


function GetDepartmentBasedOnOrganisation(organisation, faculty)
{
   
    $("#DepartmentSearchID").empty();
    $("#DepartmentSearchID").append('<option value>--Department--</option>')
    $.ajax({
        url: "/Admin/Employer/GetDepartmentBasedOnOrganisation",
        type: 'POST',
        dataType: 'json',
        data: {
            ID: organisation,
        },
        global:false,
        success: function (data)
        {
           
            jQuery.each(data, function (index, value)
            {
                    
                $("#DepartmentSearchID").append('<option value=' + value.ID + '>' + value.DepartmentName + '</option>')
                   

                })           
         
          
            },

        error: function (response) 
        {
            ShowMessage("Error Occured",false);
        }        
           


        })
   
}

function GetDesignationBasedOnOrganisation(organisation)
{
    $("#DesignationSearchID").empty();
    $("#DesignationSearchID").append('<option value>--Designation--</option>')
    $.ajax({
        url: "/Admin/Employer/GetDesignationBasedOnOrganisation",
        type: 'POST',
        dataType: 'json',
        data: {
            ID: organisation,
        },
        global:false,
        success: function (data) {


            jQuery.each(data, function (index, value) {

               
                $("#DesignationSearchID").append('<option value=' + value.ID + '>' + value.Designation + '</option>')
               

            });


        },
        error: function (response) 
        {
            ShowMessage("Warning!Error Occured",false);
        }   
    })

}


ConvertDateObjectToDate = function (dateObject)
{   
    var dateString = dateObject.substr(6);
    var currentTime = new Date(parseInt(dateString));
    var month = currentTime.getMonth() + 1;
    var day = currentTime.getDate();
    var year = currentTime.getFullYear();
    var date = year + "-" + month + "-" + day;    
    return date;
};


function ParamToEmployeeList(e) {
    var grid = $("#EmployeeListGrid").data("kendoGrid").dataSource;
    return {
        EmployerSearchName: $("#EmployerSearchName").val() == "" ? "" : $("#EmployerSearchName").val(),
        OrganisationSearchID: $("#OrganisationSearchID :selected").val() == "" ? -1 : $("#OrganisationSearchID :selected").val(),
        DepartmentSearchID: $("#DepartmentSearchID :selected").val() == "" ? -1 : $("#DepartmentSearchID :selected").val(),
        DesignationSearchID: $("#DesignationSearchID").val() == "" ? -1 : $("#DesignationSearchID").val(),
        UserIDSearch: $("#UserIDSearch").val() == "" ? -1 : $("#UserIDSearch").val(),
        pageSize: grid._pageSize,
        pageNumber: grid._page

    };

}

function GetDeviceUserID() {
    $.ajax({
        url: "/Admin/Employer/GetUniqueDeivceUserID",
        type: 'POST',
        dataType: 'json',
        global: false,
        data: {
            UserID: "1",
        },
        success: function (data) {          
            $('#UserID').val(data.UserID);
            $("#UserID").attr("disabled", "disabled");
            $("#EmpCode").val("EMP" + data.UserID).attr("disabled", "disabled");

        }
    })

}

function LoadOrgainsation() {
    $("#OrganisationSearchID").empty();
    $("#OrganisationSearchID").append('<option value>--Organisation--</option>')
    $("#OrganisationID").empty();
    $("#OrganisationID").append('<option value>--Organisation--</option>')
    
    $.ajax({
        url: "/Admin/DropDown/GetOrganisationDropDown",
        type: 'POST',
        dataType: 'json',
        global: false,
        success: function (data) {
          
            jQuery.each(data, function (index, value) 
            {
                $("#OrganisationSearchID").append('<option value=' + value.ID + '>' + value.Name + '</option>')
                $("#OrganisationID").append('<option value=' + value.ID + '>' + value.Name + '</option>')
            })

        },

        error: function (response) {

            ShowMessage("Warning! Error Occured",false);  //
        }



    })

}






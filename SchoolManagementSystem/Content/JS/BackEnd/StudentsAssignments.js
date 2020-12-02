$(document).ready(function () {
      
      Init();

        $('#NepaliDeadline').nepaliDatePicker({
        ndpEnglishInput: 'Deadline',
        npdMonth: true,
        npdYear: true,
        npdYearCount: -25
    });
    
    $('#Deadline').change(function () {
        $('#NepaliDeadline').val(AD2BS($('#Deadline').val()));
    });
    $("#ClassID").change(function () {

     
        var classs = $("#ClassID").val();
        GetFacultyBasedOnClass(classs);
        GetSubjectBasedOnClass(classs);
       
    });

    $("#SearchClassID").change(function () {

        var classs = $("#SearchClassID").val();
        GetFacultyBasedOnClassSearch(classs);
        GetSubjectBasedOnClassSearch(classs);

    });

    $("#SearchFacultyID").change(function () {

        var faculty = $("#SearchFacultyID").val();
        var classes = $("#SearchClassID").val();
        GetSectionBasedOnClassAndFacultySearch(classes, faculty);


    });

    $("#FacultyID").change(function () {

        var faculty = $("#FacultyID").val();
        var classes = $("#ClassID").val();

        GetSectionBasedOnClassAndFaculty(classes, faculty);
      

    });

    $("#Create").off().on('click', function (e)
    {
        $("#assignmentsForm").show();
        $("#assignmentsList").hide();
    });

    $("#Search").off().on('click', function (e)
    {
        $("#StudentsAssignmentsGrid").data("kendoGrid").dataSource.read();
    });

   

    $("#studentsAssignments").off().on('click', function (e) {

        if (!$('form#formStudentsAssignments').data('unobtrusiveValidation').validate()) {
            e.preventDefault();
            return false;
        }
        else {
          
            var formData = new FormData();
            formData.append("ID", $("#ID").val());
            formData.append("SessionID", $("#SessionID").val());
            formData.append("ClassID", $("#ClassID").val());
            formData.append("FacultyID", $("#FacultyID").val());
            formData.append("Section", $("#Section").val());
            formData.append("SubjectID", $("#SubjectID").val());
            formData.append("NotificationType", $("#NotificationType").val());
            formData.append("NepaliDeadline", $("#NepaliDeadline").val());
            formData.append("Deadline", $("#Deadline").val());
            formData.append("GroupID", $("#GroupID").val());
            formData.append("imageFile", $('#imageFile')[0].files[0]);          

            formData.append('__RequestVerificationToken', $('form input[name=__RequestVerificationToken]').val());

            $.ajax({
                url: "/Admin/StudentsAssignments/SaveStudentsAssignments",
                type: 'POST',
                data: formData,
                dataType: 'json',
                contentType: false,
                processData: false,
                success: function (data) {

                    $("#StudentsAssignmentsGrid").data("kendoGrid").dataSource.read();
                    ResetFormData();
                    ShowMessage(data.Message);
                    Init();


                },
                error: function (response) {
                    ShowMessage("Warning !! Error Occured");
                }
            })
        }
    });
    
  
});





function ParamToStudentsAssignmentsList(e) {
    var grid = $("#StudentsAssignmentsGrid").data("kendoGrid").dataSource;
    return {
       
        SearchClassID: $("#SearchClassID :selected").val() == "" ? -1 : $("#SearchClassID :selected").val(),
        SectionSearch: $("#SectionSearch :selected").val() == "" ? "" : $("#SectionSearch :selected").val(),
        SearchSubjectID: $("#SearchSubjectID").val() == "" ? -1 : $("#SearchSubjectID").val(),
        SearchFacultyID: $("#SearchFacultyID").val() == "" ? -1 : $("#SearchFacultyID").val(),     
        pageSize: grid._pageSize,
        pageNumber: grid._page           
       
    };

}

function Init()
{
    $("#assignmentsForm").hide();
    $("#assignmentsList").show();

}

function Download(e) {
  
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    var link = document.createElement("a");   
    link.href ="/Content/StudentsAssignments/" + dataItem.file;
   link.download = dataItem.file;
   // link.href = dataItem.FileNmae;
    //link.download = dataItem.FileNmae;
    link.click();

}

function Edit(e) {
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    e.preventDefault();


    $.ajax({
        url: "/Admin/StudentsAssignments/EditStudentsAssignments",
        data: { id: dataItem.ID },
        type: "POST",
        dataType: "json",
        global:false,
        success: function (result) {            
            $("#assignmentsForm").show();
            $("#assignmentsList").hide();
            $("#SessionID").val(result.SessionID);          
            $("#ID").val(result.ID);
            $("#ClassID").val(result.ClassID);
            $("#FacultyID").val(result.FacultyID);
            $("#Section").val(result.Section);
            $("#OrganisationID").val(result.OrganisationID);
            GetFacultyBasedOnClass(result.ClassID, result.FacultyID);
            GetSubjectBasedOnClass(result.ClassID, result.SubjectID);
            GetSectionBasedOnClassAndFaculty(result.ClassID, result.FacultyID, result.Section)
            $("#NotificationType").val(result.NotificationType);
            $("#GroupID").val(result.GroupID);

            $("#Deadline").val(ConvertDateObjectToDate(result.Deadline))
            $("#NepaliDeadline").val(ConvertDateObjectToDate1(result.NepaliDeadline))
        },

        error: function (result) {

            ShowMessage('Warning ! Error Occured');
        }
    });

}

function Delete(e) {
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    $(e.currentTarget).closest("tr").remove();
   
  
    e.preventDefault();
    $("#window").kendoWindow({
        modal: true
    });
    $("#window").data("kendoWindow").open().center();

    $("#yes").off().on('click', function (e)
    {
       
        $.ajax({
            url: "/Admin/StudentsAssignments/DeleteStudentsAssignments",
            data: { id: dataItem.ID },
            type: 'POST',
            dataType: 'json',
            success: function (result)
            {
              
                $("#window").data("kendoWindow").close();          
                ShowMessage(result.Message);
                $("#StudentsAssignmentsGrid").data("kendoGrid").dataSource.read();

            },
            error: function (response)
            {
                ShowMessage('Warning ! Error Occured');
            }
        });

    });

    $("#no").off().on('click', function (e) {

        $("#window").data("kendoWindow").close();
    });

}

function GetSubjectBasedOnClass(classs, section) {
    $("#SubjectID").empty();
    $("#SubjectID").append('<option value>--Select--</option>')
    $.ajax({
        url: "/Admin/MarksEntry/GetSubjectBasedOnClass",
        type: 'POST',
        dataType: 'json',
        data: {
            ID: classs,
        },
        global:false,
        success: function (data)
        {
           
           
           
            $(data).each(function (i, result)
            {
                if (result.ID == section) {
                    $("#SubjectID").append('<option selected value=' + result.ID + '>' + result.SubjectName + '</option>')
                }
                else {
                    $("#SubjectID").append('<option value=' + result.ID + '>' + result.SubjectName + '</option>')
                }
                

            });


        }
    })

}

function GetSubjectBasedOnClassSearch(classs, section) {
    $("#SearchSubjectID").empty();
    $("#SearchSubjectID").append('<option value>--Select--</option>')
    $.ajax({
        url: "/Admin/MarksEntry/GetSubjectBasedOnClass",
        type: 'POST',
        dataType: 'json',
        data: {
            ID: classs,
        },
        global: false,
        success: function (data) {


            $(data).each(function (i, result)
            {
                
                $("#SearchSubjectID").append('<option value=' + result.ID + '>' + result.SubjectName + '</option>')

            });


        }
    })

}
function GetFacultyBasedOnClass(classs, faculty) {
    $("#FacultyID").empty();
    $("#FacultyID").append('<option value>--Select--</option>')
    $.ajax({
        url: "/Admin/Students/GetFacultyBaseOnClass",
        type: 'POST',
        dataType: 'json',
        data: {
            ID: classs,
        },
        global:false,
        success: function (data) {


            jQuery.each(data, function (index, value) {

                if (value.ID === faculty) {
                    $("#FacultyID").append('<option selected value=' + value.ID + '>' + value.Faculty + '</option>')
                }
                else {
                    $("#FacultyID").append('<option value=' + value.ID + '>' + value.Faculty + '</option>')
                }

            });


        }
    })

}

function GetFacultyBasedOnClassSearch(classs, faculty) {
    $("#SearchFacultyID").empty();
    $("#SearchFacultyID").append('<option value>--Select--</option>')
    $.ajax({
        url: "/Admin/Students/GetFacultyBaseOnClass",
        type: 'POST',
        dataType: 'json',
        data: {
            ID: classs,
        },
        global: false,
        success: function (data) {


            jQuery.each(data, function (index, value) {

                if (value.ID === faculty) {
                    $("#SearchFacultyID").append('<option selected value=' + value.ID + '>' + value.Faculty + '</option>')
                }
                else {
                    $("#SearchFacultyID").append('<option value=' + value.ID + '>' + value.Faculty + '</option>')
                }

            });


        }
    })

}


function onError(e, status)
{
    ShowMessage('Warning ! Error Occured');
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
        global:false,
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

function GetSectionBasedOnClassAndFacultySearch(classs, faculty, section)
{
   
    $("#SectionSearch").empty();
    $("#SectionSearch").append('<option value>--Select--</option>')
    $.ajax({
        url: "/Admin/Students/GetSectionBaseOnClassAndFaculty",
        type: 'POST',
        dataType: 'json',
        data: {
            ClassID: classs,
            FacultyID: faculty
        },
        global: false,
        success: function (data) {
            var sectionArray = data.Sections.split(',');
            

            jQuery.each(sectionArray, function (index, value) {

              
                    $("#SectionSearch").append('<option value=' + value + '>' + value + '</option>')
               

            });







        }
    })

}

ConvertDateObjectToDate = function (dateObject) {
    var dateString = dateObject.substr(6);
    var currentTime = new Date(parseInt(dateString));
    var month = currentTime.getMonth() + 1;
    var day = currentTime.getDate();
    var year = currentTime.getFullYear();
    var date = month + "-" + day + "-" + year;
    return date;
};

ConvertDateObjectToDate1 = function (dateObject) {

    var dateString = dateObject.substr(6);
    var currentTime = new Date(parseInt(dateString));
    var month = currentTime.getMonth() + 1;
    var day = currentTime.getDate();
    var year = currentTime.getFullYear();
    var date = year + "-" + month + "-" + day;
    return date;
};



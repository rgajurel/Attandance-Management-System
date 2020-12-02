$(document).ready(function ()
{
    var data = "";
    InitializeDocuments();
    $("#Students_AcademicYear").prop('selectedIndex', 0);
    $(".create").off().on('click', function (e) {
        $("#hide").fadeIn(500);
        $("#show").hide();
        $("#Students_AcademicYear").prop('selectedIndex', 0);
       
    });
    $(".cancel").off().on('click', function (e) {

        $("#hide").hide();
        $("#show").fadeIn(500);

        ResetFormData();
      

    });
    $(".upload").off().on('click', function (e) {
        $('#customPopupDialog').modal('show');
    });

    $('#Students_NepaliJoinningDate').nepaliDatePicker({
        ndpEnglishInput: 'Students_EnglishJoinningDate',
        npdMonth: true,
        npdYear: true,
        npdYearCount: -25
    });

    $('#Students_NepaliDateOfBirth').nepaliDatePicker({
        ndpEnglishInput: 'Students_EnglishDateOfBirth',
        npdMonth: true,
        npdYear: true,
        npdYearCount: -25
    });


    $('#ok').off().on('click', function (e)
    {
        debugger;
        if (!$('form#formBatchUpload').data('unobtrusiveValidation').validate()) {
            e.preventDefault();
            return false;
        }
        else
        {
         debugger;
         ReadExcelFile($('#BulkStudentsSearch_BulkImage').prop('files'));
        }

       

    });

   




  
 $("input").attr("autocomplete", "off"); 
    var arraydata=[]
   
   

    $("#studentsList").show();
    $("#studentsForm").hide();
    $("#details").hide();    
    ShowPersonelHideOthers();    
    $("#studentCancel").off().on('click', function (e)
    {
        ResetFormData();
        window.location.reload();
       

    });
  
    InitializeDocuments();   

    $(document).bind('keypress', function (e)
    {
       
        if (e.keyCode == 13)
        {
            $('#studentSearch').trigger('click');
        }
    });

    $('#studentSearch').click(function ()
    {
        $("#StudentsGrid").data("kendoGrid").dataSource.read();
       
    });
    $("#Students_RollNo").keyup(function ()
    {
      
        $.ajax({
            url: "/Admin/Students/RollNumberCount",
            type: 'POST',
            dataType: 'json',
            global:false,
            data: {
                faculty: $("#Students_FacultyID").val(),
                classs: $("#Students_ClassID").val(),
               section: $("#Students_Section").val(),
             rollno: $("#Students_RollNo").val()

            },

            success: function (data)
            {
                if (data > 0) {                  
                
                    $("#duplicatemessage").text("Roll No Exist")
                    $("#Students_").val('');
                    
                    return false;
                }
                else {
                    $("#duplicatemessage").empty();
                    return true;
                }

            }
        })
    });
   
    $(".ok").off().on('click', function (e) {
        debugger;
       
        if (!$('form#formBatchUpload').data('unobtrusiveValidation').validate())
        {
            e.preventDefault();
            return false;
        }
    });

    $("#Save").off().on('click', function (e)
    {
        debugger;
        var selecteddocuments = [];
        selecteddocuments = $("#Students_DocumetsSubmittedID").val();
        if (!$('form#formStudents').data('unobtrusiveValidation').validate())
        {
            e.preventDefault();
            return false;
        }
        else
        {
           
            var formData = new FormData();
            formData.append("ID", $("#Students_ID").val());
            formData.append("AcademicYear", $("#Students_AcademicYear").val());
            formData.append("RegistrationNo", $("#Students_RegistrationNo").val());
            formData.append("EnglishJoinningDate", $("#EnglishJoinningDate").val());
           
            formData.append("FacultyID", $("#Students_FacultyID").val());
            formData.append("ClassID", $("#Students_ClassID").val());
            formData.append("Section", $("#Students_Section").val());
            formData.append("RollNo", $("#Students_RollNo").val());
            formData.append("Batch", $("#Students_Batch").val());
            formData.append("SymbolNo", $("#Students_SymbolNo").val());
            formData.append("HouseID", $("#Students_HouseID").val());
            formData.append("Email", $("#Students_Email").val());
            formData.append("MobileNo", $("#Students_MobileNo").val());
            formData.append("PhoneNo", $("#Students_PhoneNo").val());
            formData.append("StudentName", $("#Students_StudentName").val());
            formData.append("EnglishDateOfBirth", $("#EnglishDateOfBirth").val());           
            formData.append("Gender", $("#Students_Gender").val());
            formData.append("Status", $("#Students_Status").val());
            formData.append("BloodGroupID", $("#Students_BloodGroupID").val());
            formData.append("CategoryID", $("#Students_CategoryID ").val());
            formData.append("ReligionID", $("#Students_ReligionID").val());
            formData.append("CasteID", $("#Students_CasteID").val());
            formData.append("CitizenShipNumber", $("#Students_CitizenShipNumber").val());
            formData.append("TemporaryAddress", $("#Students_TemporaryAddress").val());
            formData.append("PermanentAddress", $("#Students_PermanentAddress").val());
            formData.append("LastSchoolAttended", $("#Students_LastSchoolAttended").val());
            formData.append("Result", $("#Students_Result").val());
           
            formData.append("DocumentsArray", selecteddocuments);
            formData.append("FatherName", $("#Students_FatherName").val());
            formData.append("FatherMobileNo", $("#Students_FatherMobileNo").val());
            formData.append("FatherEmail", $("#Students_FatherEmail ").val());
            formData.append("Fatherjob", $("#Students_Fatherjob").val());
            formData.append("MotherName", $("#Students_MotherName").val());
            formData.append("UserID", $("#Students_UserID").val());
            formData.append("MotherMobileNo", $("#Students_MotherMobileNo").val());
            formData.append("MotherEmail", $("#Students_MotherEmail").val());
            formData.append("MotherJob", $("#Students_MotherJob").val());
            formData.append("LastSchoolAttended", $("#Students_LastSchoolAttended").val());
            formData.append("imageFile", $('#inputFile')[0].files[0]);           
            formData.append('__RequestVerificationToken', $('form input[name=__RequestVerificationToken]').val());
            $.ajax({
                url:"/Admin/Students/SaveStudents",
                type: 'POST',
                data: formData,
                dataType: 'json',
                contentType: false,
                processData: false,
                success: function (data)
                {
                    ResetFormData();
                    $("#hide").hide();
                    $("#show").fadeIn(500);
                    ResetFormData();
                    ShowMessage(data.Message,true);
                    $("#StudentsGrid").data("kendoGrid").dataSource.read();
                    $('html, body').animate({ scrollTop: 0 }, 'fast');

                }
            })
      }
    });

    $("#Students_AcademicYear").change(function ()
    {       
       
        var academicyear = $("#Students_AcademicYear option:selected").text();
        var batch = academicyear.split('-')[0];   
       
        $("#Students_Batch").val(batch);      
       
        GetRegistrationNo();
        
    });


    $("#Students_ClassID").change(function ()
    {
        

        var classssearch = $("#Students_ClassID").val();

        GetFacultyBasedOnClass(classssearch);

    });

    $("#BulkStudentsSearch_BulkClassID").change(function () {
       
        var classssearch = $("#BulkStudentsSearch_BulkClassID").val();

        GetFacultyBasedOnClassSearchBulk(classssearch);

    });
    $("#ClassSearchID").change(function ()
    {

        var classs = $("#ClassSearchID").val();

        GetFacultyBasedOnClassSearch(classs);
       // GetSectionBasedOnClass(classs);

    });

    $("#Students_FacultyID").change(function () {

        var faculty = $("#Students_FacultyID").val();
        var classes = $("#Students_ClassID").val();

        GetSectionBasedOnClassAndFaculty(classes, faculty);
        // GetSectionBasedOnClass(classs);

    });

    $("#BulkStudentsSearch_BulkFacultyID").change(function () {

        var faculty = $("#BulkStudentsSearch_BulkFacultyID").val();
        var classes = $("#BulkStudentsSearch_BulkClassID").val();

        GetSectionBasedOnClassAndFacultyBulk(classes, faculty);
        // GetSectionBasedOnClass(classs);

    });

    $("#FacultySearchID").change(function () {

        var faculty = $("#FacultySearchID").val();
        var classes = $("#ClassSearchID").val();

        GetSectionBasedOnClassAndFacultySearch(classes, faculty);
        // GetSectionBasedOnClass(classs);

    });
  

    $("#canceldetails").off().on('click', function (e)
    {
        $("#studentsList").show();
        $("#details").hide();
      
    })

    $("#cancel").off().on('click', function (e) {
        $("#studentsList").show();
        $("#studentsForm").hide();
        ResetDocumentsSubmitted();

        $("#StudentsGrid").data("kendoGrid").dataSource.read();
        document.getElementsByClassName("panel-title")[0].innerHTML = "Student Admission";
        ResetFormData();

        $("html, body").animate({ scrollTop: 0, }, 1000);
        return false;
    });
    $("#studentsCancel").off().on('click', function (e) {
        $("#studentsList").show();
        $("#studentsForm").hide();

        $("#StudentsGrid").data("kendoGrid").dataSource.read();
        document.getElementsByClassName("panel-title")[0].innerHTML = "Student Admission";
        ResetFormData();

        $("html, body").animate({ scrollTop: 0, }, 1000);
        return false;
    });


    $("a[href=#contacts]").click(function ()
    {        
        $("#personalinfo").hide();
        $("#parentsdetails").hide();
        $("#previousqualification").hide();
        $("#contactsdetails").show();       
        

    })

    $("#personal").off().on('click', function (e)
    {
        ShowPersonelHideOthers();
      
    });  

    $("#parents").off().on('click', function (e)
    {
      
        $("#personalinfo").hide();
        $("#contactsdetails").hide();
        $("#parentsdetails").show();
        $("#previousqualification").hide();

    });

    $("#previousqualification1").off().on('click', function (e)
    {
       
        $("#personalinfo").hide();
        $("#contactsdetails").hide();
        $("#parentsdetails").hide();
        $("#previousqualification").show();

    });

    $("#studentsFormShow").off().on(' click', function (e)
    {
        InitalLoadingRegistration();
        document.getElementsByClassName("panel-title")[0].innerHTML = "Student Admission";
        $("#studentsForm").show();
        $("#studentsList").hide();      
       
    })


    $("html, body").animate({ scrollTop: 0, }, 1000);
    return false; 
  
   

});

function ReadExcelFile(e) {

    debugger;
    var files = e;
    var i, f;
    for (i = 0, f = files[i]; i != files.length; ++i) {
        var reader = new FileReader();
        var name = f.name;
        reader.onload = function (e) {
            var data = e.target.result;

            /* if binary string, read with type 'binary' */
            var result;
            var workbook = XLSX.read(data, { type: 'binary' });
            /* DO SOMETHING WITH workbook HERE */
            workbook.SheetNames.forEach(function (sheetName) {
                var roa = XLSX.utils.sheet_to_json(workbook.Sheets[sheetName]);
                if (roa.length > 0) {                  
                  //  data = JSON.stringify(roa).trim();//.replace(/ /g, "");
                    debugger;
                    SendExcelObject(roa);
                }
                else {
                    ShowMessage("Excel Sheet doesnot contains any records", false);
                }
            });
        };
        reader.readAsArrayBuffer(f);
    }
}


function SendExcelObject(data)
{
    var academicyear = $("#BulkStudentsSearch_BulkYearID option:selected").text();
    var batch = academicyear.split('-')[0];

    var studentbatch = batch;
    var academicyearid = $("#BulkStudentsSearch_BulkYearID").val();
    var classid = $("#BulkStudentsSearch_BulkClassID").val();
    var facultyid = $("#BulkStudentsSearch_BulkFacultyID").val();
    var sectionid = $("#BulkStudentsSearch_BulkSection").val();

    debugger;
    $.ajax({
        url: "/Admin/Students/BatchUploadStudents",
        data: { batchdata: data,studentbatch:studentbatch,academicyearid:academicyearid,classid:classid,facultyid:facultyid,sectionid:sectionid},
            type: "POST",
            dataType: "json",           
            success: function (data) {

                ShowMessage(data.Message, false);
                $("#hide").hide();
                $("#show").fadeIn(500);         
                $("#StudentsGrid").data("kendoGrid").dataSource.read();
            },
            error: function (result) {

                ShowMessage('Error Occured');
            }
        
    });
}

function ResetDocumentsSubmitted() {
    var obj = [];
    $('option:selected').each(function () {
        obj.push($(this).index());
    });

    for (var i = 0; i < obj.length; i++) {
        $('#DocumetsSubmittedID')[0].sumo.unSelectItem(obj[i]);
    }

}
function InitializeDocuments()
{
   
    $('#DocumetsSubmittedID').SumoSelect({
        okCancelInMulti: true,
    });

    // $('select#SectionID').prepend('<option selected disabled hidden value="-1">---- Select ----</option>');
    $('#DocumetsSubmittedID').prop("selectedIndex", -1);

}
function onAdditionalData() {
    return {
        text: $("#StudentsSearchName").val()
    };

}

function GetFacultyBasedOnClassSearch(classs) {
    $("#FacultySearchID").empty();
    $("#FacultySearchID").append('<option value>--Select Faculty--</option>')
    $.ajax({
        url: "/Admin/Students/GetFacultyBaseOnClass",
        type: 'POST',
        dataType: 'json',

        data: {
            ID: classs,
        },


        success: function (data) {


            jQuery.each(data, function (index, value) {

                              
                    $("#FacultySearchID").append('<option value=' + value.ID + '>' + value.Faculty + '</option>')
                

            });


        },
        global: false,
    })

}
function GetFacultyBasedOnClass(classs, faculty) {
    $("#Students_FacultyID").empty();
    $("#Students_FacultyID").append('<option value>--Select Faculty--</option>')
    $.ajax({
        url: "/Admin/Students/GetFacultyBaseOnClass",
        type: 'POST',
        dataType: 'json',
        
        data: {
            ID: classs,
        },
        

        success: function (data)
        {
           

            jQuery.each(data, function (index, value) {

                if (value.ID === faculty) {
                    $("#Students_FacultyID").append('<option selected value=' + value.ID + '>' + value.Faculty + '</option>')
                }
                else {
                    $("#Students_FacultyID").append('<option value=' + value.ID + '>' + value.Faculty + '</option>')
               }

            });


        },
        global: false,
    })

}
function GetSectionBasedOnClassAndFacultyBulk(classs, faculty, section) {
    $("#BulkStudentsSearch_BulkSection").empty();
    $("#BulkStudentsSearch_BulkSection").append('<option value>--Select--</option>')
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


            jQuery.each(sectionArray, function (index, value)
            {
                             
                    $("#BulkStudentsSearch_BulkSection").append('<option value=' + value + '>' + value + '</option>')
                

            });







        }
    })

}
function GetSectionBasedOnClassAndFaculty(classs, faculty,section)
{
    $("#Students_Section").empty();
    $("#Students_Section").append('<option value>--Select--</option>')
    $.ajax({
        url: "/Admin/Students/GetSectionBaseOnClassAndFaculty",
        type: 'POST',
        dataType: 'json',
        data: {
            ClassID: classs,
            FacultyID: faculty
        },

        success: function (data)
        {
            var sectionArray = data.Sections.split(',');
           

            jQuery.each(sectionArray, function (index, value)
            {
                
                if (value === section)
                        {
                    $("#Students_Section").append('<option selected value=' + value + '>' + value + '</option>')
                        }
                        else
                        {
                    $("#Students_Section").append('<option value=' + value + '>' + value + '</option>')
                      }
              
            });
           
          
          


          

        }
    })

}
function GetSectionBasedOnClassAndFacultySearch(classs, faculty) {
    $("#SectionSearch").empty();
    $("#SectionSearch").append('<option value>--Select Section--</option>')
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

               
                $("#SectionSearch").append('<option value=' + value + '>' + value + '</option>')
             

            });







        }
    })

}

function GetFacultyBasedOnClassSearch(classssearch) {
 
    $("#FacultySearchID").empty();
    $("#FacultySearchID").append('<option value>--Select--</option>')


    $.ajax({
        url: "/Admin/Students/GetFacultyBaseOnClass",
        type: 'POST',
        dataType: 'json',
        data: {
            ID: classssearch,
        },
        global:false,

        success: function (data) {
            jQuery.each(data, function (index, value)
            {              
               
                $("#FacultySearchID").append('<option value=' + value.ID + '>' + value.Faculty + '</option>')
              

            });


        }
    })

}

function GetFacultyBasedOnClassSearchBulk(classssearch) {

    $("#BulkStudentsSearch_BulkFacultyID").empty();
    $("#BulkStudentsSearch_BulkFacultyID").append('<option value>--Select--</option>')


    $.ajax({
        url: "/Admin/Students/GetFacultyBaseOnClass",
        type: 'POST',
        dataType: 'json',
        data: {
            ID: classssearch,
        },
        global: false,

        success: function (data) {
            jQuery.each(data, function (index, value) {

                $("#BulkStudentsSearch_BulkFacultyID").append('<option value=' + value.ID + '>' + value.Faculty + '</option>')


            });


        }
    })

}

function GetRegistrationNo()
{   
    $.ajax({
        url: "/Admin/Students/GetUniqueRegistrationNumber",
        type: 'POST',
        dataType: 'json',
        global:false,
        data: {
            Batch: $('#Students_Batch').val(),

        },

        success: function (data)
        {
            $('#Students_RegistrationNo').val(data.RegistrationNo)
            $('#Students_SymbolNo').val(data.RegistrationNo)
            $('#Students_UserID').val(data.UserID)
            $("#Students_UserID").attr("disabled", "disabled");
           // $('#RegistrationNo').attr("disabled", "disabled");
           
        }
    })

}
function InitalLoadingRegistration() {
    var academicyear1 = $("#AcademicYear option:selected").text();
    var batch1 = academicyear1.split('-')[0];
    $("#Batch").val(batch1);
    GetRegistrationNo();
}

function ParamToStudentsList(e) {
    var grid = $("#StudentsGrid").data("kendoGrid").dataSource;
    return {
        FacultySearchID: $("#FacultySearchID :selected").val() == "" ? -1 : $("#FacultySearchID :selected").val(),
        ClassSearchID: $("#ClassSearchID :selected").val() == "" ? -1 : $("#ClassSearchID :selected").val(),
        SectionSearch: $("#SectionSearch :selected").val() == "" ? "" : $("#SectionSearch :selected").val(),
        StudentsSearchName: $("#StudentsSearchName").val() == "" ? "" : $("#StudentsSearchName").val(),
        BatchSearch: $("#BatchSearch").val() == "" ? "" : $("#BatchSearch").val(),
        SectionSearch: $("#SectionSearch").val() == "" ? "" : $("#SectionSearch").val(),
      
        RegistratioNoSearch: $("#RegistratioNoSearch").val() == "" ? "" : $("#RegistratioNoSearch").val(),
        pageSize: grid._pageSize,
        pageNumber: grid._page

    };

}

function Edit(e)
{
       
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    e.preventDefault();      

    $.ajax({
        url: "/Admin/Students/EditStudents",
        data: { id: dataItem.ID },
        type: "POST",
        dataType: "json",
        global:false,
        success: function (result)
        {
            

            $("#hide").fadeIn(500);
            $("#show").hide();
            $("#Students_ID").val(result.ID);
            $("#Students_AcademicYear").val(result.AcademicYear)
            $("#Students_RegistrationNo").val(result.RegistrationNo)

                         
          
            $("#EnglishJoinningDate").val(ConvertDateObjectToDate(result.EnglishJoinningDate))

            $("#EnglishDateOfBirth").val(ConvertDateObjectToDate(result.EnglishDateOfBirth))

           

            $("#Students_FacultyID").val(result.FacultyID)
            $("#Students_ClassID").val(result.ClassID)
            GetFacultyBasedOnClass(result.ClassID, result.FacultyID)
            GetSectionBasedOnClassAndFaculty(result.ClassID, result.FacultyID,result.Section)
           // $('#ClassID').val($(this).val()).trigger('change');
           // $("#Section:selected").val(result.Section)
            $("#Students_RollNo").val(result.RollNo);
            $("#Students_Batch").val(result.Batch)
            $("#Students_SymbolNo").val(result.SymbolNo)
            $("#Students_HouseID").val(result.HouseID)
            $("#Students_UserID").val(result.UserID)
            $("#Students_UserID").prop('disable', true);
            $("#Students_Email").val(result.Email)
            $("#Students_MobileNo").val(result.MobileNo)
            $("#Students_PhoneNo").val(result.PhoneNo)
            $("#Students_StudentName").val(result.StudentName)
                    
        
          $("#Students_Gender").val(result.Gender)
          $("#Students_Status").val(result.Status)
          $("#Students_BloodGroupID").val(result.BloodGroupID)
          $("#Students_CategoryID").val(result.CategoryID)
          $("#Students_ReligionID").val(result.ReligionID)
            $("#Students_CasteID").val(result.CasteID)

            $("#Students_CitizenShipNumber").val(result.CitizenShipNumber)
            $("#Students_TemporaryAddress").val(result.TemporaryAddress)
            $("#Students_PermanentAddress").val(result.PermanentAddress)
            $("#Students_LastSchoolAttended").val(result.LastSchoolAttended)
            $("#Students_Result").val(result.Result)
           
            $("#Students_FatherName").val(result.FatherName)
            $("#Students_FatherMobileNo").val(result.FatherMobileNo)
            $("#Students_FatherEmail").val(result.FatherEmail)
            $("#Students_Fatherjob").val(result.Fatherjob)
            $("#Students_MotherName").val(result.MotherName)

            $("#Students_MotherMobileNo").val(result.MotherMobileNo)
            $("#Students_MotherEmail").val(result.MotherEmail)
            $("#Students_MotherJob").val(result.MotherJob)
            $("#Students_LastSchoolAttended").val(result.LastSchoolAttended)

            $('#Students_DocumetsSubmittedID')[0].sumo.unSelectAll();
            var documents = result.DocumentsSubmitted.split(",");
            var selectbox = $('#Students_DocumetsSubmittedID')[0];
            for (var i = 0; i < documents.length; i++) {
                selectbox.sumo.selectItem(documents[i]);
            }
 
        },
        error: function (result) {

            ShowMessage('Error Occured');
        }
    });

}
function InitializeDocuments() {
    $('#Students_DocumetsSubmittedID').SumoSelect({
        okCancelInMulti: true,
    });

    $('#Students_DocumetsSubmittedID').prop("selectedIndex", -1);

}
function Details(e) {

    $("#studentsList").hide();
    $("#studentsForm").hide();
    $("#details").show();
  
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    e.preventDefault();

        $.ajax({
            url: "/Admin/Students/DetailsStudents",
            data: { id: dataItem.ID },
            type: 'POST',
            dataType: 'json',
            success: function (result)
            {
                            
                $("#username1").text(result.StudentName)
                $("#username").text(result.StudentName)

                if (result.Session == null)
                {
                    $("#session").text('Not Availiable')
                }
                else
                {
                    $("#session").text(result.Session)
                }
              

                if (result.Faculty == null)
                {
                    $("#faculty").text('Not Availiable')
                }
                else {
                    $("#faculty").text(result.Faculty)
                }
               


                if (result.ClassName == null) {
                    $("#class").text('Not Availiable')
                }
                else {
                    $("#class").text(result.ClassName)
                }
               
                if (result.Section == null) {
                    $("#section").text('Not Availiable')
                }
                else {
                    $("#section").text(result.Section)
                }

               
                if (result.Batch == null)
                {
                    $("#batch").text('Not Availiable')
                }
                else
                {
                    $("#batch").text(result.Batch)
                }

                if (result.RegistrationNo == null) {
                    $("#registration").text('Not Availiable')
                }
                else {
                    $("#registration").text(result.RegistrationNo)
                }

                if (result.RollNo == null)
                {
                    $("#rollno").text('Not Availiable')
                } else {
                    $("#rollno").text(result.RollNo)
                }
               
                if (result.SymbolNo == null)
                {
                      $("#symbolno").text('Not Availiable')
                }
                else {
                    $("#symbolno").text(result.SymbolNo)
                }
              
              
                if (result.HouseName == null)
                {
                    $("#housename").text('Not Availiable')

                }
                else {
                    $("#housename").text(result.HouseName)
                }

               
                if (result.NepaliDateOfBirth == null) {

                        $("#dateofbirth").text('Not Availiable')
                }
                else {

                     $("#dateofbirth").text(ConvertDateObjectToDate(result.NepaliDateOfBirth))
                }
               

                if (result.NepaliJoinningDate == null)
                {
                    $("#joiningdate").text('Not Availiable')
                }
                else
                {
                    $("#joiningdate").text(ConvertDateObjectToDate(result.NepaliJoinningDate))
                }
              

                if (result.BloodGroup == null) {

                    $("#bloodgroup").text('Not Availaible')
                } else {

                    $("#bloodgroup").text(result.BloodGroup)
                }
               
                if (result.StudentsCategory == null)
                {

                    $("#studentscategory").text('Not Availiable')

                }
                else
                {

                    $("#studentscategory").text(result.StudentsCategory)

                }

                if (result.Name == null) {
                    $("#religion").text('Not Availiable')

                } else {

                    $("#religion").text(result.Name)
                }

               
                if (result.CasteName == null)
                {

                    $("#caste").text('Not Availiable')
                }
                else {

                    $("#caste").text(result.CasteName)
                }
              

                if (result.TemporaryAddress == null) {

                    $("#tempaddress").text('Not Availiable')
                }
                else {

                    $("#tempaddress").text(result.TemporaryAddress)
                }

                if (result.PermanentAddress == null)
                {
                    $("#permaddress").text('Not Availaible')
                }
                else {
                    $("#permaddress").text(result.PermanentAddress)
                }
               
                
                if(result.PhoneNo==null)
                {
                    $("#phone").text('Not Availaible')
                }
                else {
                    $("#phone").text(result.PhoneNo)
                }

                if (result.Email == null) {

                    $("#email").text('Not Availianle')
                } else {

                    $("#email").text(result.Email)
                }
               
               
                if (result.MobileNo == null) {
                    $("#mobile").text(result.MobileNo)
                }
                else {
                    $("#mobile").text(result.MobileNo)
                }
               

                if (result.FatherName == null) {
                    $("#fathername").text('Not Availiable')

                }
                else {
                    $("#fathername").text(result.FatherName)
                }
               

                if (result.FatherMobileNo == null) {

                    $("#fathermobileno").text('Not Availiable')
                }
                else {

                    $("#fathermobileno").text(result.FatherMobileNo)
                }
                

                if (result.Email == null) {

                    $("#fatheremail").text('Not Availiable')
                }
                else {

                    $("#fatheremail").text(result.Email)
                }
               

                if (result.fatherjob == null) {

                    $("#fatherjob").text('Not Availiable')
                }
                else {
                    $("#fatherjob").text(result.fatherjob)

                }

                if (result.MotherName == null)
                {
                    $("#mothername").text('Not Availiable')

                }
                else
                {
                    $("#mothername").text(result.MotherName)
                }
               
               
                if (result.LastSchoolAttended == null) {

                    $("#previousname").text('Not Availaible')
                }
                else {
                    $("#previousname").text(result.LastSchoolAttended)

                }
               
                if (result.Result == null)
                {

                    $("#result").text('Not Availiable')
                }
                else {
                    $("#result").text(result.Result)
                }
               

                $("#imageupload").attr("src", result.Image);


                //$("#EnglishJoioningDate").val(ConvertDateObjectToDate(result.EnglishJoinningDate))
                //$("#NepaliJoioningDate").val(ConvertDateObjectToDate(result.NepaliJoinningDate))


                //$("#FacultyID").val(result.FacultyID)
                //$("#ClassID").val(result.ClassID)
                //GetSectionBasedOnClass(result.ClassID, result.Section)

                // $('#ClassID').val($(this).val()).trigger('change');
                //$("#Section:selected").val(result.Section)
                //$("#RollNo").val(result.RollNo);
                //$("#Batch").val(result.Batch)
                //$("#SymbolNo").val(result.SymbolNo)
                //$("#HouseID").val(result.HouseID)

                //$("#Email").val(result.Email)
                //$("#MobileNo").val(result.MobileNo)
                //$("#PhoneNo").val(result.PhoneNo)
               

                //$("#EnglishDateOfBirth").val(ConvertDateObjectToDate(result.EnglishDateOfBirth))

                //$("#NepaliDateOfBirth").val(ConvertDateObjectToDate(result.NepaliDateOfBirth))

                //$("#Gender").val(result.Gender)
                //$("#Status").val(result.Status)
                //$("#BloodGroupID").val(result.BloodGroupID)
                //$("#CategoryID").val(result.CategoryID)
                //$("#ReligionID").val(result.ReligionID)
                //$("#CasteID").val(result.CasteID)

                //$("#CitizenShipNumber").val(result.CitizenShipNumber)
                //$("#TemporaryAddress").val(result.TemporaryAddress)
                //$("#PermanentAddress").val(result.PermanentAddress)
                //$("#LastSchoolAttended").val(result.LastSchoolAttended)
                //$("#Result").val(result.Result)

                //$("#FatherName").val(result.FatherName)
                //$("#FatherMobileNo").val(result.FatherMobileNo)
                //$("#FatherEmail").val(result.FatherEmail)
                //$("#Fatherjob").val(result.Fatherjob)
                //$("#MotherName").val(result.MotherName)

                //$("#MotherMobileNo").val(result.MotherMobileNo)
                //$("#MotherEmail").val(result.MotherEmail)
                //$("#MotherJob").val(result.MotherJob)
                //$("#LastSchoolAttended").val(result.LastSchoolAttended)

            }
        });   

   

}
function ShowPersonelHideOthers()
{
    $("#personalinfo").show();
    $("#contactsdetails").hide();
    $("#parentsdetails").hide();
    $("#previousqualification").hide();
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





$(document).ready(function () {
  


  $("#exportExport").off().on('click', function (e)
  {

      if (!$('form#formMarksSheetLedger').data('unobtrusiveValidation').validate()) {
          e.preventDefault();
          return false;
      }
      else
      {
        
          $(".MarksSheetLedger").table2excel({
              exclude: ".noExl",
              name: "MarksSheetLedger",
              fileext: ".xlsx",
              exclude_img: true,
              exclude_links: true,
              exclude_inputs: true
          });
      }
  });

  $("#ClassID").change(function ()
  {

        var classs = $("#ClassID").val();       
        GetFacultyBasedOnClass(classs);
        
  });

  $("#FacultyID").change(function () {

      var faculty = $("#FacultyID").val();
      var classes = $("#ClassID").val();

      GetSectionBasedOnClassAndFaculty(classes, faculty);
      // GetSectionBasedOnClass(classs);

  });

  $("#loadGrid").off().on('click', function (e)
  {

      if (!$('form#formMarksSheetLedger').data('unobtrusiveValidation').validate())
      {
            e.preventDefault();
            return false;
        }
      else
      {            
           
           
            $.ajax({
                url: "/Admin/MarksSheetLedger/GetFullMarksSheetLedger",
                type: 'POST',
                data: AddAntiForgeryToken({
                    SessionID: $('#SessionID').val(),
                    ClassID: $('#ClassID').val(),
                    Section: $('#Section').val(),
                    TermID: $('#TermID').val(),
                     FacultyID: $('#FacultyID').val(), 
                }),
                dataType: 'json',
                success: function (data)
                {                    
                   
                    $('#leftSubjectForMarksEntry').empty();
                    $('#divMarksSheetLedger').empty();                  
                    if (data.ErrorOccured == true)
                    {
                        var i, j, k,l,index,html = '',htmlforexcel=''; 
                       
                        var subjectsSummary;
                        var totalSubjectInClass = data.Data[0].Subjects.split(',');
                       // var subject = data.Data[0].Subjects.split(',');
                        //for (k = 0; k < subject.length; k++)
                        //{
                        //    if (totalSubjectInClass.includes(subject[k]))
                        //    {
                        //        index = totalSubjectInClass.indexOf(subject[k]);
                        //        totalSubjectInClass.splice(index, 1);
                        //    }
                        //}
                        if (totalSubjectInClass.length > 0)
                        {
                            htmlforexcel = '<h5 style="font-weight:bold;">Total Subjects In Class====>' + totalSubjectInClass.join(' || ') + '</h5></br>';
                            //htmlforexcel = '<h5 style="font-weight:bold;">Subjects Count==>' + totalSubjectInClass.length+'</h5>'
                            $('#leftSubjectForMarksEntry').append(htmlforexcel);
                        }
                        else
                        {
                            htmlforexcel = 'Error';
                            $('#leftSubjectForMarksEntry').append('<h5 style="font-weight:bold;">' + htmlforexcel + '</h5>');
                        }
                      
                        html += '<tr>';
                        html += '<th rowspan="2">Name</th>';
                        html += '<th rowspan="2">Class</th>';
                        html += '<th rowspan="2">Roll</th>';
                        html += '<th rowspan="2">Symbol No</th>';
                        html += '<th rowspan="2">Total</th>';
                        for (i = 0; i < totalSubjectInClass.length; i++)
                        {
                            html += '<th colspan="7">' + totalSubjectInClass[i] + '</th>';
                        }
                        html += '</tr>';

                        for (i = 0; i < totalSubjectInClass.length; i++)
                        {
                            html += '<th>Marks(T)</th>';
                            html += '<th>GPA(T)</th>';
                            html += '<th>Marks(P)</th>';
                            html += '<th>GPA(P)</th>';
                            html += '<th>Total</th>';
                            html += '<th>TotalGPA</th>';
                            html += '<th>GPN</th>';
                        }


                        $.each(data.Data, function (index, item)
                        {
                           
                           
                            html += '<tr>';                         
                            html += '<td>' + item.StudentName + '</td>';
                            html += '<td>' + item.ClassName + '</td>';
                            html += '<td>' + item.RollNo + '</td>';
                            html += '<td>' + item.SymbolNo + '</td>';
                            html += '<td>' + item.TotalObtained + '</td>';
                           
                            var subjects = item.SubjectName.split(',');
                            subjectsSummary = item.SubjectSummary.split(',');

                            for (j = 0; j < totalSubjectInClass.length; j++)
                            {
                                for (k = 0; k < subjects.length; k++) {
                                    if (totalSubjectInClass[j] == subjects[k])
                                    {
                                        var tt = subjectsSummary[k].split('/');
                                     for (l = 0; l < tt.length; l++)
                                       {
                                            html += '<td>' + tt[l] + '</td>';
                                      }    
                                        break;
                                    }

                                }
                                if (totalSubjectInClass[j] !== subjects[k]) {
                                    html += '<td colspan="7" style="color:red;">Not Availiable</td>';
                                }

                            }
                     //       for (k = 0; k < totalSubjectInClass.length; k++)
                     //    {
                     //           if (subjects.includes(totalSubjectInClass[k]))
                     //           {
                     //               if(subje)
                     //                   var tt = subjectsSummary[k].split('/');
                     //                   for (l = 0; l < tt.length; l++)
                     //                   {
                     //                       html += '<td>' + tt[l] + '</td>';
                     //                   }                                      
                                       
                     //              }
                     //              else
                     //              {
                     //                  html += '<td colspan="7" style="color:red;">Not Availiable</td>';
                     //              }
                                    
                                   
                     //}                            
                        
                                  
                            html += '</tr>';
                           
                        })
                        html += '<tr style=display:none>';
                        html += '<td style="font-weight:bold;">' + htmlforexcel + '</td>';                       
                        html += '</tr>';
                        $('#divMarksSheetLedger').append(html);
                       
                    }
                    else
                    {
                        ShowMessage(data.Message);
                    }
                   


                }
            })
        }
    });
});
function GetSectionBasedOnClass(classs, section) {
    $("#Section").empty();
    $("#Section").append('<option value>--Select--</option>')
    $.ajax({
        url: "/Admin/Students/GetSectionBaseOnClass",
        type: 'POST',
        dataType: 'json',
        data: {
            ID: classs,
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
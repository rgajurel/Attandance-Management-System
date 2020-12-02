
$(document).ready(function ()
{
    $("#ClassID").change(function () 
    {
      var classs = $("#ClassID").val();       
      GetFullMarksPassMarks();
      GetFacultyBasedOnClass(classs);
      GetSubjectBasedOnClass(classs);
        
    });    
  

    $("#TermID").change(function ()
    {
        GetFullMarksPassMarks();
      
    });

    $("#FacultyID").change(function () {
       
        var faculty = $("#FacultyID").val();
        var classes = $("#ClassID").val();

        GetSectionBasedOnClassAndFaculty(classes, faculty);
        GetFullMarksPassMarks();

    });

    $("#SessionID").change(function ()
    {
        GetFullMarksPassMarks();
   

    });

    $("#SubjectID").change(function ()
    {
        GetFullMarksPassMarks();
      

    });
    $("#Section").change(function ()
    {
        GetFullMarksPassMarks();
       

    });

    
    $("#marksEntryEditSearch").off().on('click', function (e) {
        if (!$('form#formMarksEntryEdit').data('unobtrusiveValidation').validate()) {
            e.preventDefault();
            return false;
        } else {
            $("#MarksEntryEditGrid").data("kendoGrid").dataSource.read();
            // $("#marksEntrySearch").hide();
        }

    });

    $("#marksEntryUpdate").off().on('click', function (e)
    {
        debugger;
        if (!$('form#formMarksEntryUpdated').data('unobtrusiveValidation').validate()) 
        {
            e.preventDefault();
            return false;
        } else
        {
            var i, fullmarkspracticalupdate,dataItem, fullmarkspractical,totalfullmarks,firstItem, totalobtainedmarks,obtainedmarks, fullmarkstheory, fullmarkstopassmarksratio, fullmarkspracticaltopassmarkspracticalration
            $("#marksEntryUpdate").attr("disabled", true);
             dataItem= $("#MarksEntryEditGrid").data("kendoGrid").dataSource.data();

            for ( i = 0; i < dataItem.length; i++)
            {
               
                firstItem = $('#MarksEntryEditGrid').data().kendoGrid.dataSource.data()[i];               
                firstItem["FullMarksTheory"] = $("#FullMarksTheoryupdate").val();
                firstItem["PassMarksTheory"] = $("#PassMarksTheoryUpdate").val();
                firstItem["FullMarksPractical"] = $("#FullMarksPracticalUpdate").val();
                firstItem["PassMarksPractical"] = $("#PassMarksPracticalUpdtae").val();
                firstItem["CreditPoint"] = $("#CreditPointUpdate").val();

                fullmarkspracticalupdate = parseInt($("#FullMarksPracticalUpdate").val());
                fullmarkspractical = parseInt($("#FullMarksPractical").val());
                fullmarkstopassmarksratio = ($("#FullMarksTheoryupdate").val() / $("#FullMarksTheory").val());
              

                if (fullmarkspracticalupdate == 0)
                {
                    fullmarkspracticaltopassmarkspracticalration = 0;
                }
                else
                {
                    
                    fullmarkspracticaltopassmarkspracticalration = (fullmarkspracticalupdate / fullmarkspractical);
                    if (!isFinite(fullmarkspracticaltopassmarkspracticalration))
                    {
                        fullmarkspracticaltopassmarkspracticalration = 0;
                    }
                }
                
                //if (fullmarkspracticaltopassmarkspracticalration == NaN || fullmarkspracticaltopassmarkspracticalration == 'undefined' || fullmarkspracticaltopassmarkspracticalration == null)
                //{
                //    fullmarkspracticaltopassmarkspracticalration = 0;
                //}
              
              
                firstItem["ObtainedMarksTheory"] = Math.ceil(firstItem["ObtainedMarksTheory"] * fullmarkstopassmarksratio);
                firstItem["ObtainedMarksPractical"] = (firstItem["ObtainedMarksPractical"] * fullmarkspracticaltopassmarkspracticalration);

               fullmarkstheory = firstItem["FullMarksTheory"];
                 obtainedmarks = firstItem["ObtainedMarksTheory"];
                 totalobtainedmarks = obtainedmarks + firstItem["ObtainedMarksPractical"];
                 totalfullmarks = (fullmarkstheory) + (firstItem["FullMarksPractical"]);


                //CallGradePointFunction(function (output) {

                //    $(output).each(function (i, result) {

                //        var checkneg1 = (firstItem["ObtainedMarksPractical"] / firstItem["FullMarksPractical"]) * 100 - result.MarksFrom;
                //        var checkneg2 = result.MarksTo - (firstItem["ObtainedMarksPractical"] / firstItem["FullMarksPractical"] * 100);

                //        if (checkneg1 >= 0 && checkneg2 >= 0) {
                //            firstItem["ObtaindedGradePractical"]= result.Grade;

                //        }


                //        })
                //    });

                //CallGradePointFunction(function (output)
                //{
                //    $(output).each(function (i, result) {

                //        var checkneg1 = (obtainedmarks / fullmarkstheory) * 100 - result.MarksFrom;
                //        var checkneg2 = result.MarksTo - (obtainedmarks / fullmarkstheory * 100);

                //        if (checkneg1 >= 0 && checkneg2 >= 0)
                //        {                           
                //            firstItem["ObtainedGradeTheory"]= result.Grade;
                //        }

                //    });

                //});
                //CallGradePointFunction(function (output)
                //{
                //    $(output).each(function (i, result) {

                //        var checkneg1 = (totalobtainedmarks / totalfullmarks) * 100 - result.MarksFrom;
                //        var checkneg2 = result.MarksTo - (totalobtainedmarks / totalfullmarks * 100);

                //        if (checkneg1 >= 0 && checkneg2 >= 0)
                //        {
                //            debugger;
                //            firstItem["FinalGrade"]= result.Grade;
                //            firstItem["GradePoint"]= result.GradePoint;

                //        }


                //    });
                //});

                $('#MarksEntryEditGrid').data('kendoGrid').refresh();


                
                  
            }
          
        }

    });

    $("#marksEntrySave").off().on('click', function (e) {
        if (!$('form#formMarksEntryUpdated').data('unobtrusiveValidation').validate()) {
            e.preventDefault();
            return false;
        } else {
            e.preventDefault();
            var batchmarksentry = $("#MarksEntryEditGrid").data("kendoGrid").dataSource.data();
           
            $.ajax({
                url: "/Admin/MarksEntry/SaveMarksEntry",
                type: 'POST',
                data: { data: JSON.stringify(batchmarksentry) },
                dataType: 'json',
                success: function (data) {
                    ShowMessage(data.Message);
                    $('.divMarksError').empty();
                    if (data.ErrorOccured == true) {
                        var html = '';
                        html += '<table class="MarksListWithError">';
                        html += '<tr>';
                        html += '<td>SN</td>';
                        html += '<td>StudentName</td>';
                        html += '<td>Full(T)</td>';
                        html += '<td>Pass(T)</td>';
                        html += '<td>Credit</td>';
                        html += '<td>Full(P)</td>';
                        html += '<td>Pass(P)</td>';
                        html += '<td>Obtained(T)</td>';
                        html += '<td>Obtained(P)</td>';
                        html += '<td>Grade(T)</td>';
                        html += '<td>Grade(P)</td>';
                        html += '<td>FinalGrade</td>';
                        html += '<td>GradePoint</td>';
                        html += '</tr>';
                        $.each(data.Data, function (index, item) {
                            html += '<tr>';
                            html += '<td>' + item.SN + '</td>';
                            html += '<td>' + item.StudentName + '</td>';
                            html += '<td>' + item.FullMarksTheory + '</td>';
                            html += '<td>' + item.PassMarksTheory + '</td>';
                            html += '<td>' + item.CreditPoint + '</td>';
                            html += '<td>' + item.FullMarksPractical + '</td>';
                            html += '<td>' + item.PassMarksPractical + '</td>';
                            html += '<td>' + item.ObtainedMarksTheory + '</td>';
                            html += '<td>' + item.ObtainedMarksPractical + '</td>';
                            html += '<td>' + item.ObtainedGradeTheory + '</td>';
                            html += '<td>' + item.ObtaindedGradePractical + '</td>';
                            html += '<td>' + item.FinalGrade + '</td>';
                            html += '<td>' + item.GradePoint + '</td>';
                            html += '</tr>';


                        })
                        html += '</table>';

                        $('.divMarksError').append(html);
                        $(function () {
                            $(".MarksListWithError").table2excel({
                                exclude: ".noExl",
                                name: "MarksEntryError",
                                fileext: ".xlsx",
                                exclude_img: true,
                                exclude_links: true,
                                exclude_inputs: true
                            });
                        });
                    }
                    else
                    {
                        ShowMessage(data.Message);
                        $('#MarksEntryEditGrid').data('kendoGrid').refresh();
                    }

                    //ResetFormData();
                    //ShowMessage(data.Message);
                    //document.getElementsByClassName("panel-title")[0].innerHTML = "Add Location";
                    //$("#LocationGrid").data("kendoGrid").dataSource.read();


                }
            })

        }
    })

   
})

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
           
          
            $(data).each(function (i, result) {
         

                $("#SubjectID").append('<option value=' + result.ID + '>' + result.SubjectName + '</option>')

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
    $("#Section").append('<option value>Select</option>')
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
function ParamToMarksEditList(e) {
    var grid = $("#MarksEntryEditGrid").data("kendoGrid").dataSource;
    return {
        SessionID: $("#SessionID :selected").val() == "" ? -1 : $("#SessionID :selected").val(),
        ClassID: $("#ClassID :selected").val() == "" ? -1 : $("#ClassID :selected").val(),
        Section: $("#Section :selected").val() == "" ? "" : $("#Section :selected").val(),
        SubjectID: $("#SubjectID").val() == "" ? -1 : $("#SubjectID").val(),
        TermID: $("#TermID").val() == "" ? -1 : $("#TermID").val(),
      
    };

}
function Save(e) {


    if (e.values && (e.values.ObtainedMarksTheory)) {

        var fullmarkstheory = e.values.FullMarksTheory || e.model.FullMarksTheory;
        var obtainedmarks = e.values.ObtainedMarksTheory || e.model.ObtainedMarksTheory;
        var totalobtainedmarks = (e.values.ObtainedMarksTheory || e.model.ObtainedMarksTheory) + (e.values.ObtainedMarksPractical || e.model.ObtainedMarksPractical);
        var totalfullmarks = (e.values.FullMarksTheory || e.model.FullMarksTheory) + (e.values.FullMarksPractical || e.model.FullMarksPractical);
        if (obtainedmarks > fullmarkstheory || obtainedmarks < 0) {


            ShowMessage("Marks is greater than " + fullmarkstheory + " Or Less than 0");
            e.model.set("ObtainedMarksTheory", 0);
            //    this.val(0);


        }
        else {
            CallGradePointFunction(function (output) {

                $(output).each(function (i, result) {

                    var checkneg1 = (obtainedmarks / fullmarkstheory) * 100 - result.MarksFrom;
                    var checkneg2 = result.MarksTo - (obtainedmarks / fullmarkstheory * 100);

                    if (checkneg1 >= 0 && checkneg2 >= 0) {
                        e.model.set("ObtainedGradeTheory", result.Grade);

                    }



                });

                $(output).each(function (i, result) {

                    var checkneg1 = (totalobtainedmarks / totalfullmarks) * 100 - result.MarksFrom;
                    var checkneg2 = result.MarksTo - (totalobtainedmarks / totalfullmarks * 100);

                    if (checkneg1 >= 0 && checkneg2 >= 0) {
                        e.model.set("FinalGrade", result.Grade);
                        e.model.set("GradePoint", result.GradePoint);

                    }



                });

            })
        }


    }
    if (e.values && (e.values.ObtainedMarksPractical)) {
        var fullmarkspractical = e.values.FullMarksPractical || e.model.FullMarksPractical;
        var obtainedmarks = e.values.ObtainedMarksPractical || e.model.ObtainedMarksPractical;
        var totalobtainedmarks = (e.values.ObtainedMarksTheory || e.model.ObtainedMarksTheory) + (e.values.ObtainedMarksPractical || e.model.ObtainedMarksPractical);
        var totalfullmarks = (e.values.FullMarksTheory || e.model.FullMarksTheory) + (e.values.FullMarksPractical || e.model.FullMarksPractical);
        if (obtainedmarks > fullmarkspractical || obtainedmarks < 0) {

            ShowMessage("Marks is greater than " + fullmarkspractical + " Or Less than 0");
            //    this.val(0);



        }
        else {
            CallGradePointFunction(function (output) {

                $(output).each(function (i, result) {

                    var checkneg1 = (obtainedmarks / fullmarkspractical) * 100 - result.MarksFrom;
                    var checkneg2 = result.MarksTo - (obtainedmarks / fullmarkspractical * 100);

                    if (checkneg1 >= 0 && checkneg2 >= 0) {
                        e.model.set("ObtaindedGradePractical", result.Grade);

                    }



                });

                $(output).each(function (i, result) {

                    var checkneg1 = (totalobtainedmarks / totalfullmarks) * 100 - result.MarksFrom;
                    var checkneg2 = result.MarksTo - (totalobtainedmarks / totalfullmarks * 100);

                    if (checkneg1 >= 0 && checkneg2 >= 0) {
                        e.model.set("FinalGrade", result.Grade);
                        e.model.set("GradePoint", result.GradePoint);

                    }



                });

            })
        }
    }

}
function CallGradePointFunction(handleData) {
    $.ajax({
        url: "/Admin/GradeMaster/GetAllGrade",
        type: "POST",
        dataType: "json",
        global:false,
        success: function (result) {

            handleData(result);


        },
        error: function (result) {

            ShowMessage('Error Occured');
        }
    });

}

function GetFullMarksPassMarks() {
    $.ajax({
        url: "/Admin/MarksEntry/GetFullMarksPassMarks",
        type: 'POST',
        dataType: 'json',
        data: {
            ClassID: $("#ClassID").val(),
            SessionID: $("#SessionID").val(),
            Section: $("#Section").val(),
            SubjectID: $("#SubjectID").val(),
            FacultyID: $("#FacultyID").val(),
            TermID: $("#TermID").val()
        },
        global:false,
        success: function (data) {            
            $("#FullMarksTheory").val(data.FullMarksTheory);
            $("#PassMarksTheory").val(data.PassMarksTheory);
            $("#FullMarksPractical").val(data.FullMarksPractical);
            $("#PassMarksPractical").val(data.PassMarksPractical);
            $("#CreditPoint").val(data.CreditPoint);
           
            var grid = $("#MarksEntryEditGrid").data("kendoGrid");
            grid.dataSource.transport.options.read.global = false;
            $("#MarksEntryEditGrid").data("kendoGrid").dataSource.read();
           
        },
        error: function (response) {

            alert(response.responseText);
        }
    })
}
var gradedata = [];
$(document).ready(function () {
   
    Init();
    $("#ClassID").change(function () {

        var classs = $("#ClassID").val();
        GetFacultyBasedOnClass(classs);
        GetSubjectBasedOnClass(classs);
        GetFullMarksPassMarks();
    });

    $("#TermID").change(function ()
    {
        GetFullMarksPassMarks();
        

    });

  

    $("#FacultyID").change(function () {
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

    $("#calculate").off().on('click', function (e)
    {       
        var dataItem;
        dataItem = $("#MarksEntryGrid").data("kendoGrid").dataSource.data();
        for (i = 0; i < dataItem.length; i++) {
            
            debugger;
            firstItem = $('#MarksEntryGrid').data().kendoGrid.dataSource.data()[i];          
             fullmarkstheory = firstItem["FullMarksTheory"];
             fullmarkspractical = firstItem["FullMarksPractical"];
             obtainedmarkstheory = firstItem["ObtainedMarksTheory"];
             obtainedmarkspractical = firstItem["ObtainedMarksPractical"];
             totalobtainedmarks = obtainedmarkstheory + obtainedmarkspractical;
             totalfullmarks = fullmarkstheory + fullmarkspractical;
             if (obtainedmarkstheory == 0 || obtainedmarkstheory == "0" || obtainedmarkstheory > fullmarkstheory) {
                firstItem["IsAdmin"] = false;
                firstItem["ObtainedGradeTheory"] = "";
                firstItem["ObtaindedGradePractical"] = "";
                firstItem["FinalGrade"] = "";
                firstItem["GradePoint"] = "";
                continue;
            }
           if (obtainedmarkstheory > fullmarkstheory || obtainedmarkstheory < 0)
            {
                firstItem["IsAdmin"] = false;
                firstItem["ObtainedGradeTheory"] = "";
                firstItem["ObtaindedGradePractical"] = "";
                firstItem["FinalGrade"] = "";
                firstItem["GradePoint"] = "";

                continue;
            }
            else if (obtainedmarkspractical > fullmarkspractical || obtainedmarkspractical < 0)
            {
                firstItem["IsAdmin"] = false;
                firstItem["ObtainedGradeTheory"] = "";
                firstItem["ObtaindedGradePractical"] = "";
                firstItem["FinalGrade"] = "";
                firstItem["GradePoint"] = "";

                continue;
            }
            else {

                $(gradedata).each(function (i, result)
                                    {
                    var checkneg1 = (obtainedmarkstheory.toFixed(0) / fullmarkstheory) * 100 - result.MarksFrom;
                    var checkneg2 = result.MarksTo - (obtainedmarkstheory.toFixed(0) / fullmarkstheory * 100);

                                        if (checkneg1 >= 0 && checkneg2 >= 0)
                                        {                                           
                                            firstItem["ObtainedGradeTheory"] = result.Grade;                                                                                    

                                        }

                                        var checkneg11 = (obtainedmarkspractical.toFixed(0) / fullmarkspractical) * 100 - result.MarksFrom;
                                        var checkneg21 = result.MarksTo - (obtainedmarkspractical.toFixed(0) / fullmarkspractical * 100);

                                       
                                        if (checkneg11 >= 0 && checkneg21 >= 0) {

                                            firstItem["ObtaindedGradePractical"] = result.Grade;

                                        }
                                        var checkneg111 = (totalobtainedmarks.toFixed(0) / totalfullmarks) * 100 - result.MarksFrom;
                                        var checkneg211 = result.MarksTo - (totalobtainedmarks.toFixed(0) / totalfullmarks * 100);

                                        if (checkneg1 >= 0 && checkneg2 >= 0) {

                                            firstItem["FinalGrade"] = result.Grade;
                                            firstItem["GradePoint"] = result.GradePoint;

                                        }
                                        firstItem["IsAdmin"] =true;


                });
                         

              

            }           

        }
        $('#MarksEntryGrid').data('kendoGrid').refresh();
    });

    

    $("#FacultyID").change(function () {

        var faculty = $("#FacultyID").val();
        var classes = $("#ClassID").val();

        GetSectionBasedOnClassAndFaculty(classes, faculty);
      

    });


    $("#marksEntrySearch").off().on('click', function (e)
    {
        if (!$('form#formMarksEntry').data('unobtrusiveValidation').validate())
        {
            e.preventDefault();
            return false;
        } else
        {
            $("#MarksEntryGrid").data("kendoGrid").dataSource.read();
           // $("#marksEntrySearch").hide();
        }
        
    })

    $("#marksSaveRecords").off().on('click', function (e)
    {
        if (!confirm("Have You Calculate Result"))
        {
            ShowMessage("Warning !! Please First Calculte?");
            return false;
        }

        if (!$('form#formMarksEntry').data('unobtrusiveValidation').validate()) {
            e.preventDefault();
            return false;
        } else
        {
           
            e.preventDefault();
            var batchmarksentry = $("#MarksEntryGrid").data("kendoGrid").dataSource.data();
           
            $.ajax({
                url: "/Admin/MarksEntry/SaveMarksEntry",
                type: 'POST',
                data: { data: JSON.stringify(batchmarksentry) },
                dataType: 'json',
                async:true,
                success: function (data)
                {
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
                                name:"MarksEntryError",
                                fileext: ".xlsx",
                                exclude_img: true,
                                exclude_links: true,
                                exclude_inputs: true
                            });
                        });
                    }
                    else {
                        ShowMessage(data.Message);
                    }

                    //ResetFormData();
                    //ShowMessage(data.Message);
                    //document.getElementsByClassName("panel-title")[0].innerHTML = "Add Location";
                    //$("#LocationGrid").data("kendoGrid").dataSource.read();


                }
            })

        }
    })

    $("#locationSubmit").off().on('click', function (e) {

        if (!$('form#formLocation').data('unobtrusiveValidation').validate()) {
            e.preventDefault();
            return false;
        }
        else {

            $.ajax({
                url: "/Admin/Location/SaveLocation",
                type: 'POST',
                data: AddAntiForgeryToken({
                    ID: $('#ID').val(),
                    LocationName: $('#LocationName').val(),
                    Fee: $('#Fee').val(),
                }),
                dataType: 'json',
                success: function (data) {

                    ResetFormData();
                    ShowMessage(data.Message);
                    document.getElementsByClassName("panel-title")[0].innerHTML = "Add Location";
                    $("#LocationGrid").data("kendoGrid").dataSource.read();


                }
            })
        }
    });
    
  
    $('#MarksEntryGrid').off().on('click', '.chkbx', function ()
    {
              
        var checked = $(this).is(':checked');
        var grid = $('#MarksEntryGrid').data().kendoGrid;
        var dataItem = grid.dataItem($(this).closest('tr'));
        row = $(this).closest("tr");
        checkedIds[dataItem.SN] = checked;
        dataItem.set('IsAdmin', checked);      
       
       
        var view = $("#MarksEntryGrid").data("kendoGrid").dataSource.view();
        for (var i = 0; i < view.length; i++)
        {
            
            if (checkedIds[view[i].SN]) {
                grid.tbody.find("tr[data-uid='" + view[i].uid + "']")
                .addClass("k-alt k-state-selected")
                .find(".chkbx")
                .attr("checked", "checked");
            }
            else {
                grid.tbody.find("tr[data-uid='" + view[i].uid + "']")
                row.removeClass("k-alt k-state-selected");
            }
        }
    })
    $("#MarksEntryGrid").on("keydown", "#ObtainedMarksTheory", function (e) {
       
        var arrows = [38, 40,39,37]; // Down and Up arrow keys
        var key = e.keyCode;

        if (arrows.indexOf(key) >= 0) {
         
            e.preventDefault();
           

            var grid = $("#MarksEntryGrid").data("kendoGrid");

            var row = $(this).closest("tr");
            var rowIdx = $("tr", grid.tbody).index(row);
            var data = $('#MarksEntryGrid').data().kendoGrid.dataSource.data()[rowIdx];
         
           // var item = $(this).closest("tr")
            //get current row index
          

            // to check first row and proceed further else exit - index start from 0 (first row)
            // 38 - Up key, 40 - Down key
            if (key == 38 && rowIdx == 0) {
                return false;
            }

            //get total number of records in grid
            var count = grid.dataSource.total();

            // to check last row and proceed further else exit - index start from 0 (first row)
            if (key == 40 && rowIdx == (count - 1)) {
                return false;
            }

            this.blur();
          

            row.trigger("change");

            if (key == 40) {
              
                var nextCell = $(this).closest("tr").next("tr[role='row']").find("td").eq(14);

              //  $(this).closest('tr').next().addClass('k-state-selected');
            }
            else if (key == 38) {
                var nextCell = $(this).closest("tr").prev("tr[role='row']").find("td").eq(14);
               // $(this).closest('tr').prev().addClass('k-state-selected');
            }
            else if(key==39) {
                var nextCell = $(this).closest("tr").find("td").eq(15);
            }
            else if (key == 37) {
                var nextCell = $(this).closest("tr").find("td").eq(14);
            }

            grid.editCell(nextCell);          

           // LatestLine_PreSelect();
        }
    });
    $("#MarksEntryGrid").on("keydown", "#ObtainedMarksPractical", function (e) {
       
        var arrows = [38, 40, 39, 37]; // Down and Up arrow keys
        var key = e.keyCode;

        if (arrows.indexOf(key) >= 0) {

            e.preventDefault();


            var grid = $("#MarksEntryGrid").data("kendoGrid");

            var row = $(this).closest("tr");
            var rowIdx = $("tr", grid.tbody).index(row);
            var data = $('#MarksEntryGrid').data().kendoGrid.dataSource.data()[rowIdx];
          
          
            // var item = $(this).closest("tr")
            //get current row index


            // to check first row and proceed further else exit - index start from 0 (first row)
            // 38 - Up key, 40 - Down key
            if (key == 38 && rowIdx == 0) {
                return false;
            }

            //get total number of records in grid
            var count = grid.dataSource.total();

            // to check last row and proceed further else exit - index start from 0 (first row)
            if (key == 40 && rowIdx == (count - 1)) {
                return false;
            }

            this.blur();


            row.trigger("change");

            if (key == 40) {

                var nextCell = $(this).closest("tr").next("tr[role='row']").find("td").eq(15);

                //  $(this).closest('tr').next().addClass('k-state-selected');
            }
            else if (key == 38) {
                var nextCell = $(this).closest("tr").prev("tr[role='row']").find("td").eq(15);
                // $(this).closest('tr').prev().addClass('k-state-selected');
            }
            else if (key == 39) {
                var nextCell = $(this).closest("tr").find("td").eq(15);
            }
            else if (key == 37) {
                var nextCell = $(this).closest("tr").find("td").eq(14);
            }

            grid.editCell(nextCell);

          
            // LatestLine_PreSelect();
        }
    });
});


//function Save(e)
//{   
//    debugger;
   
//    if (e.values && ((e.values.ObtainedMarksTheory)))
//    {       
//        e.model.set("IsAdmin",true);
//        var fullmarkstheory = e.values.FullMarksTheory || e.model.FullMarksTheory;
//        var obtainedmarks = e.values.ObtainedMarksTheory || e.model.ObtainedMarksTheory;
//        var totalobtainedmarks = (e.values.ObtainedMarksTheory || e.model.ObtainedMarksTheory) + (e.values.ObtainedMarksPractical || e.model.ObtainedMarksPractical);
//        var totalfullmarks = (e.values.FullMarksTheory || e.model.FullMarksTheory) + (e.values.FullMarksPractical || e.model.FullMarksPractical);
//        if (obtainedmarks > fullmarkstheory || obtainedmarks<0)
//        {                      
//            ShowMessage("Marks is greater than " + fullmarkstheory + " Or Less than 0");          
//            e.model.set("ObtainedMarksTheory", 0);      
           
         
//        }
//        else
//        {
//            CallGradePointFunction(function (output)
//            {

//                $(output).each(function (i, result) 
//                {

//                    var checkneg1 = (obtainedmarks.toFixed(0)/fullmarkstheory)*100 - result.MarksFrom;
//                    var checkneg2 = result.MarksTo - (obtainedmarks.toFixed(0) / fullmarkstheory * 100);
                 
//                    if (checkneg1 >= 0 && checkneg2 >= 0)
//                    {
                       
//                        e.model.set("ObtainedGradeTheory", result.Grade);
                                           
//                    }



//                });

//                $(output).each(function (i, result)
//                {

//                    var checkneg1 = (totalobtainedmarks.toFixed(0) / totalfullmarks) * 100 - result.MarksFrom;
//                    var checkneg2 = result.MarksTo - (totalobtainedmarks.toFixed(0) / totalfullmarks * 100);

//                    if (checkneg1 >= 0 && checkneg2 >= 0)
//                    {
//                        e.model.set("FinalGrade", result.Grade);
//                        e.model.set("GradePoint", result.GradePoint);

//                    }



//                });

//            })
          
          
          
           
           

//        }

      
//    }
//    if (e.values && (e.values.ObtainedMarksPractical))
//    {
       
//        e.model.set("IsAdmin", true);
//        var fullmarkspractical = e.values.FullMarksPractical || e.model.FullMarksPractical;
//        var obtainedmarks = e.values.ObtainedMarksPractical || e.model.ObtainedMarksPractical;
//        var totalobtainedmarks = (e.values.ObtainedMarksTheory || e.model.ObtainedMarksTheory) + (e.values.ObtainedMarksPractical || e.model.ObtainedMarksPractical);
//        var totalfullmarks = (e.values.FullMarksTheory || e.model.FullMarksTheory) + (e.values.FullMarksPractical || e.model.FullMarksPractical);
//        if (obtainedmarks > fullmarkspractical || obtainedmarks < 0) {
           
//            ShowMessage("Marks is greater than " + fullmarkspractical + " Or Less than 0");
                  


//        }
//        else {
//            CallGradePointFunction(function (output) {

//                $(output).each(function (i, result) {

//                    var checkneg1 = (obtainedmarks.ToFixed(0) / fullmarkspractical) * 100 - result.MarksFrom;
//                    var checkneg2 = result.MarksTo - (obtainedmarks.ToFixed(0) / fullmarkspractical * 100);

//                    if (checkneg1 >= 0 && checkneg2 >= 0) {
//                        e.model.set("ObtaindedGradePractical", result.Grade);

//                    }



//                });

//                $(output).each(function (i, result) {

//                    var checkneg1 = (totalobtainedmarks.toFixed(0) / totalfullmarks) * 100 - result.MarksFrom;
//                    var checkneg2 = result.MarksTo - (totalobtainedmarks.toFixed(0)/ totalfullmarks * 100);

//                    if (checkneg1 >= 0 && checkneg2 >= 0) {
//                        e.model.set("FinalGrade", result.Grade);
//                        e.model.set("GradePoint", result.GradePoint);

//                    }



//                });

//            })           
           
//        }
//    }
  
//}


function Init()
{
    $.ajax({
        url: "/Admin/GradeMaster/GetAllGrade",
        type: "POST",
        dataType: "json",
        
        success: function (output) {
            $(output).each(function (i, result)
            {
                gradedata.push(result);
            });
            

        },
        error: function (result) {

            ShowMessage('Error Occured');
        },
    });
}  



function ParamToMarksList(e) {
    var grid = $("#MarksEntryGrid").data("kendoGrid").dataSource;
    return {
        SessionID: $("#SessionID :selected").val() == "" ? -1 : $("#SessionID :selected").val(),
        ClassID: $("#ClassID :selected").val() == "" ? -1 : $("#ClassID :selected").val(),
        Section: $("#Section :selected").val() == "" ? "" : $("#Section :selected").val(),
        SubjectID: $("#SubjectID").val() == "" ? -1 : $("#SubjectID").val(),
        FacultyID: $("#FacultyID").val() == "" ? -1 : $("#FacultyID").val(),
        TermID: $("#TermID").val() == "" ? -1 : $("#TermID").val(),
        FullMarksTheory: $("#FullMarksTheory").val() == "" ? "" : $("#FullMarksTheory").val(),
        PassMarksTheory: $("#PassMarksTheory").val() == "" ? "" : $("#PassMarksTheory").val(),
        FullMarksPractical: $("#FullMarksPractical").val() == "" ? "" : $("#FullMarksPractical").val(),
        PassMarksPractical: $("#PassMarksPractical").val() == "" ? "" : $("#PassMarksPractical").val(),


       // TermID: $("#TermID").val() == "" ? -1 : $("#TermID").val(),      
        //pageSize: grid._pageSize,
        //pageNumber: grid._page
    };

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
            url: "/Admin/MarksEntry/DeleteMarksEntry",
            data: { id: dataItem.ID },
            type: 'POST',
            dataType: 'json',
            global:false,
            success: function (result)
            {

                $("#window").data("kendoWindow").close();             
                             
               // $("#MarksEntryGrid").data("kendoGrid").dataSource.read();

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
function GetFullMarksPassMarks()
{
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
            var grid = $("#MarksEntryGrid").data("kendoGrid");
            grid.dataSource.transport.options.read.global = false;
            $("#MarksEntryGrid").data("kendoGrid").dataSource.read();
          

        },
        error: function (response) {

            ShowMessage("Warning ! Error Occured")
        }
    })
}
function resetRowNumberMarksEntryGrid(e) {
       

    //$("#MarksEntryGrid").on("focus", "td", function (e)
    //{
    //    $("input").on("keydown", function (event)
    //    {
    //        debugger;
    //        if (event.keyCode == 13)
    //        {
    //                setTimeout(function ()
    //                {
    //                    debugger;
    //                    var curCell = $("#MarksEntryGrid").find(".k-state-selected")
    //                    var eCell = $("#MarksEntryGrid").find(".k-edit-cell")

    //                    curCell.removeClass("k-state-selected");
    //                    curCell.removeClass("k-state-focused");
    //                    curCell.removeAttr("data-role");
    //                    curCell.next().addClass("k-state-selected");
    //                    curCell.next().addClass("k-state-focused");
    //                    try {
    //                        $('#MarksEntryGrid').data('kendoGrid').closeCell(eCell);
    //                    } catch (ex) {
    //                    }
    //                  //  $('#MarksEntryGrid').data('kendoGrid').select();       
    //                    $('#MarksEntryGrid').data('kendoGrid').editCell(curCell.next());

    //                },5000);
    //            }
    //        });
    //    });
     var rows = e.sender.tbody[0].rows;

    $(rows).each(function (e) {
        var grid = $("#MarksEntryGrid").data("kendoGrid");
        var row = this;
        var dataItem = grid.dataItem(row);

        if (dataItem.IsAdmin == true) {
         
            grid.tbody.find("tr[data-uid='" + dataItem.uid + "']")
           .addClass("k-alt k-state-selected gridselect")
            // .find(".chkbx")
            // .attr("checked", "checked");
        }
        
    })
    $(".k-grid-Edit").find("span").addClass("fa fa-pencil");
    $(".k-grid-Edit").removeClass("k-button");
    $(".k-grid-Delete").find("span").addClass("fa fa-trash");
    $(".k-grid-Delete").removeClass("k-button");
    $(".k-grid-Details").find("span").addClass("fa fa-eye");
    $(".k-grid-Details").removeClass("k-button");
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



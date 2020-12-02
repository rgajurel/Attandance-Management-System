$(document).ready(function () {
   
    $("#Print").click(function ()
    {

       
        if (requisitionid === undefined || requisitionid.length == 0)
        {
            ShowMessage("Warning ! Please Select Students")
            return false;
        }

        else if (requisitionid.length>10)
        {
            ShowMessage("Warning ! Please Select 10 Students at a Time")
            return false;
        }
        else
        {
            GetDataForMarkSheet();

           // ShowMessage(requisitionid.length);
        }

     

    })
    

    $("#ClassID").change(function () {

        var classs = $("#ClassID").val();
        GetFacultyBasedOnClass(classs);

    });

    $("#FacultyID").change(function ()
    {
        var faculty = $("#FacultyID").val();
        var classes = $("#ClassID").val();
        GetSectionBasedOnClassAndFaculty(classes, faculty);   
    });

    $("#loadGrid").off().on('click', function (e) {

        if (!$('form#formMarksSheetPrint').data('unobtrusiveValidation').validate())
        {
            e.preventDefault();
            return false;
        }
        else
        {
            $("#MarksSheetPrint").data("kendoGrid").dataSource.read();
            requisitionid = [];
        }
    });
});

function ParamToMarksList(e)
{
    var grid = $("#MarksSheetPrint").data("kendoGrid").dataSource;
    return {
        SessionID: $("#SessionID :selected").val() == "" ? -1 : $("#SessionID :selected").val(),
        ClassID: $("#ClassID :selected").val() == "" ? -1 : $("#ClassID :selected").val(),
        Section: $("#Section :selected").val() == "" ? "" : $("#Section :selected").val(),      
        FacultyID: $("#FacultyID").val() == "" ? -1 : $("#FacultyID").val(),
        TermID: $("#TermID").val() == "" ? -1 : $("#TermID").val(),       
        
    };

}
var requisitionid = [];
var checkedIds = {};
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

function resetRowNumberMarksSheetGrid(e) {
         
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

function onError(e, status) {
    ShowMessage('Warning ! Error Occured');
}

function checkAll(ele) {

    var checked = $('.chkSelectAll').prop('checked'), grid = $("#MarksSheetPrint").data("kendoGrid");
    for (var i = 0; i < grid.dataSource.data().length; i++) {
        var item = grid.dataSource.data()[i];
        var row = grid.element.find("tr[data-uid='" + item.uid + "']");
        var checkBox = row.find(".checkbox");
        if (!checkBox.prop('checked')) {
            checkBox.trigger("click");
        }
        if (!checked) {
            if (checkBox.prop('checked')) {
                checkBox.trigger("click");
            }
        }
    }
    if (!checked)
    {
        requisitionid = [];

    }
}

$(function (e)
{
    $('#MarksSheetPrint').on('click', '.checkbox', function ()
    {
        var checked = $(this).is(':checked');
        row = $(this).closest("tr");
        var grid = $('#MarksSheetPrint').data().kendoGrid;
        var dataItem = grid.dataItem($(this).closest('tr'));
        checkedIds[dataItem.StudentID] = checked;
               
        if (checked)
        {
            row.addClass("k-alt k-state-selected");

            if (requisitionid.length < 1)
            {
                requisitionid.push(dataItem.StudentID);

                       
            }
            else {               
                    requisitionid.push(dataItem.StudentID);            

            }
           
        }
        else
        {
            row.removeClass("k-alt k-state-selected");

            $(requisitionid).each(function (i, data) {

                if (data== dataItem.StudentID) {

                    requisitionid.splice(i, 1);

                }

            });
        }
           


    });


})

function GetDataForMarkSheet()
{
    $.ajax({
        url: "/Admin/MarkSheetPrint/GetAllMarkSheet",
        type: 'POST',
        data:{
            SessionID: $('#SessionID').val(),
            ClassID: $('#ClassID').val(),
            FacultyID: $('#FacultyID').val(),
            Section: $('#Section').val(),
            TermID: $('#TermID').val(),
            ResultType: $('#ResultType').val(),
            StudentName:requisitionid.join(",")
        },
        dataType: 'json',
        success: function (data)
        {
           
            
            var html = '', i1 = 1, j, k, resulttype;
            $("#divPrintContiner").empty();
            resulttype = $('#ResultType').val();
            if (data.Data[0].IsFinal == false)
            {
                if (resulttype == 2) {


                    if (data.ErrorOccured == false) {
                        $(data.Data).each(function (i, result)
                        {
                            
                            html += '<main class="marksheet"><header>';
                            if (result.Logo == null || result.Logo == "") {
                                html += '<img src="/Content/Images/School/School.png" height="120" width="150"/><dir class="row centered txt-centered"><h4>For Academic Excellence</h4>';

                            }
                            else {
                                html += '<img src=' + result.Logo + ' height="120" width="150"/><dir class="row centered txt-centered"><h4>For Academic Excellence</h4>';
                            }

                            html += '<h2>' + result.SchoolName + '</h2>';
                            html += '<div class="progress-report"><h2></h2><h2 style="text-align:center">Progress Report</h2><h5>' + result.Phone + '</h5></div>';
                            html += '<h4><span class="md-bot-border">' + result.TermName + '</span> Examination &nbsp;&nbsp ' + result.ActiveSession + '</h4></dir></header>';

                            html += '<section class="student-detail"><div class="row centered txt-centered"><h4>The Marks Secured By &nbsp;&nbsp; <span class="lg-bot-border">' + result.StudentName + '</span> Son/Daughter of &nbsp;&nbsp; <span class="md-bot-border">' + result.FatherName + '</span>';
                            html += 'and &nbsp;&nbsp; <span class="md-bot-border">' + result.MotherName + '</span> of class &nbsp;&nbsp;<span class="sm-bot-border">' + result.Class + '</span> Section &nbsp;&nbsp; <span class="sm-bot-border">' + result.Section + '</span> Roll No &nbsp;&nbsp; <span class="sm-bot-border">' + result.RollNo + '</span> in the &nbsp;&nbsp<span class="md-bot-border">' + result.TermName + '</span> Examination are</h4>';
                            html += '</div></section>';
                            html += '<section class="tables"><dir class="row"><table><thead><tr>';
                            html += '<th>S.N</th><th>Subject</th><th>FM</th><th>PM</th><th>Obtained</th><th>Grade</th><th>Grade Point</th><th>H.O.M</th></tr></thead>';
                            html += '<tbody>';
                            $(result.AllResultData).each(function (index, result1) {
                                if (result1.Obtained == 0) {
                                    var totalobt = '0'
                                }
                                else {
                                    var totalobt = result1.Obtained
                                }

                                html += '<tr><td>' + i1 + '</td><td>' + result1.SubjectName + '</td><td>' + result1.FM + '</td><td>' + result1.PM + '</td><td>' + totalobt + '</td><td>' + result1.Grade + '</td><td>' + result1.GradePoint + '</td><td>' + result1.HighestMarksObtained + '</td></tr>';
                                i1++;
                            })
                            html += '<tr><td colspan="2">Total</td><td>' + result.TotalFM + '</td><td>' + result.TotalPM + '</td><td>' + result.TotalObtained + '</td><td>' + result.FinalGrade + '</td><td>' + parseFloat(result.GradePoint).toFixed(2) + '</td><td></td></tr>';
                            html += '</tbody></table></dir>';
                            html += '<div class="flex"><h4>Attendance: ' + result.PresentDays + '/' + result.TotalDays + ' Days</h4><h4>Percentage:' + result.Percentage.toFixed(2) + '</h4></div></section>';
                            html += '<section class="table-detail"><dir class="row"><table><thead><tr><th colspan="3">Details of Grade Sheet</th></tr></thead><tbody><tr><td>90-100%</td><td>A+</td>';
                            html += ' <td>Outstanding</td></tr><tr><td>80-Below 90%</td><td>A</td><td>Excellent</td></tr><tr><td>70-Below 80%</td><td>B+</td><td>Very Good</td></tr>';

                            html += '<tr><td>60-Below 70%</td><td>B</td><td>Good</td></tr><tr><td>50-Below 60%</td><td>C+</td><td>Satisfactory</td></tr><tr><td>40-Below 50%</td><td>C</td><td>Acceptable</td></tr>';
                            html += '<tr><td>30-Below 40%</td><td>D+</td><td>Partially Acceptable</td></tr><tr><td>20-Below 30%</td><td>D</td><td>Insufficient</td></tr><tr><td>00-Below 20%</td><td>E</td><td>Very Insuffecient</td></tr></tbody></table>';

                            html += '<article><label>Comments</label><ul>';

                            if(result.FinalGrade=="A+")
                            {
                                html+='<li>Outstanding<input type="checkbox" name="vehicle" value="Bike" checked="checked"></li>';
                            }
                            else
                            {
                                html+='<li>Outstanding<input type="checkbox" name="vehicle" value="Bike"></li>';
                            }

                            if(result.FinalGrade=="A")
                            {
                                html+='<li>Excellent<input type="checkbox" name="vehicle" value="Bike" checked="checked"></li>';
                            }
                            else
                            {
                                html+='<li>Excellent<input type="checkbox" name="vehicle" value="Bike"></li>';
                            }
                            if(result.FinalGrade=="B+")
                            {
                                html+='<li>Very Good<input type="checkbox" name="vehicle" value="Bike" checked="checked"></li>';
                            }
                            else
                            {
                                html+='<li>Very Good<input type="checkbox" name="vehicle" value="Bike"></li>';
                            }

                            if(result.FinalGrade=="B")
                            {
                                html+='<li>Good<input type="checkbox" name="vehicle" value="Bike" checked="checked"></li>';

                            }
                            else
                            {
                                html+='<li>Good<input type="checkbox" name="vehicle" value="Bike"></li>';
                            }

                            if (result.FinalGrade == "C+")
                            {
                                html += '<li>Satisfactory<input type="checkbox" name="vehicle" value="Bike" checked="checked"></li>';
                            }
                            else
                            {
                                html += '<li>Satisfactory<input type="checkbox" name="vehicle" value="Bike"></li>';
                            }

                            if(result.FinalGrade=="C")
                            {
                                html+='<li>Acceptable<input type="checkbox" name="vehicle" value="Bike" checked="checked"></li>';
                            }
                            else
                            {
                                html+='<li>Acceptable<input type="checkbox" name="vehicle" value="Bike"></li>';
                            }
                            if(result.FinalGrade=="D+")
                            {
                                html+='<li>Partially Acceptable<input type="checkbox" name="vehicle" value="Bike" checked="checked"></li>';
                            }
                            else
                            {
                                html+='<li>Partially Acceptable<input type="checkbox" name="vehicle" value="Bike"></li>';
                            }

                            if(result.FinalGrade=="D")
                            {
                                html+='<li>Insufficient<input type="checkbox" name="vehicle" value="Bike" checked="checked"></li>';
                            }
                            else
                            {
                                html+='<li>Insufficient<input type="checkbox" name="vehicle" value="Bike"></li>';
                            }

                            if (result.FinalGrade == "E")
                            {
                                html += '<li>Very Insufficient<input type="checkbox" name="vehicle" value="Bike" checked="checked"></li>';
                            }
                            else {
                                html += '<li>Very Insufficient<input type="checkbox" name="vehicle" value="Bike"></li>';
                            }
                           
                            html += '</ul></article></dir></section>';
                            html += '<footer><dir class="row"><ul><li><span class="md-bot-border">' + ConvertDateObjectToDate(result.Date) + '</span>Date</li><li><span class="md-bot-border">&nbsp;&nbsp;&nbsp;&nbsp</span>Class Teacher</li><li><span class="md-bot-border">&nbsp;&nbsp;&nbsp;&nbsp</span>Exam Co-ordinator</li><li><span class="md-bot-border">&nbsp;&nbsp;&nbsp;&nbsp</span>Principal</li></ul></dir></footer></main><div style="margin-top:200px"></div>';
                            i1 = 1;
                        });


                        $("#divPrintContiner").append(html);
                        $("#divPrintContiner").print();

                    }



                    if (data.ErrorOccured == true) {
                        ShowMessage(data.Messge);
                    }
                }
                else {

                    if (data.ErrorOccured == false) {
                        $(data.Data).each(function (i, result) {
                            html += '<main class="marksheet"><header>';
                            if (result.Logo == null || result.Logo == "") {
                                html += '<img src="/Content/Images/School/School.png" height="120" width="150"/><dir class="row centered txt-centered"><h4>For Academic Excellence</h4>';

                            }
                            else {
                                html += '<img src=' + result.Logo + ' height="120" width="150"/><dir class="row centered txt-centered"><h4>For Academic Excellence</h4>';
                            }
                            html += '<h2>' + result.SchoolName + '</h2>';
                            html += '<div class="progress-report"><h2></h2><h2 style="text-align:center">Progress Report</h2><h5>' + result.Phone + '</h5></div>';
                            html += '<h4><span class="md-bot-border">' + result.TermName + '</span> Examination &nbsp;&nbsp ' + result.ActiveSession + '</h4></dir></header>';

                            html += '<section class="student-detail"><div class="row centered txt-centered"><h4>The Grade Secured By &nbsp;&nbsp; <span class="lg-bot-border">' + result.StudentName + '</span> Son/Daughter of &nbsp;&nbsp; <span class="md-bot-border">' + result.FatherName + '</span>';
                            html += 'and &nbsp;&nbsp; <span class="md-bot-border">' + result.MotherName + '</span> of class &nbsp;&nbsp;<span class="sm-bot-border">' + result.Class + '</span> Section &nbsp;&nbsp; <span class="sm-bot-border">' + result.Section + '</span> Roll No &nbsp;&nbsp; <span class="sm-bot-border">' + result.RollNo + '</span> in the &nbsp;&nbsp<span class="md-bot-border">' + result.TermName + '</span> Examination are</h4>';
                            html += '</div></section>';
                            html += '<section class="tables"><dir class="row"><table><thead><tr>';
                            html += '<th>S.N</th><th>Subject</th><th>CreditHour</th><th>Grade(TH)</th><th>Grade(PR)</th><th>FinalGrade</th><th>GradePoint</th><th>Highest Grade</th></tr></thead>';
                            html += '<tbody>';
                            $(result.AllResultData).each(function (index, result1) {
                                html += '<tr><td>' + i1 + '</td><td>' + result1.SubjectName + '</td><td>' + result1.CreditPoint + '</td><td>' + result1.ObtainedGradeTheory + '</td><td>' + result1.ObtaindedGradePractical + '</td><td>' + result1.Grade + '</td><td>' + result1.GradePoint + '</td><td>' + result1.HighestGradeObtained + '</td></tr>';
                                i1++;
                            })
                            html += '<tr><td colspan="5">Final</td><td>' + result.FinalGrade + '</td><td>' + parseFloat(result.GradePoint).toFixed(2) + '</td><td></td></tr>';
                            html += '</tbody></table></dir>';
                            html += '<div class="flex"><h4>Attendance: ' + result.PresentDays + '/' + result.TotalDays + ' Days</h4><h4>Grade Point Average(GPA):' + parseFloat(result.GradePoint).toFixed(2) + '&nbsp;&nbsp;&nbsp;&nbsp;' + result.FinalGrade + ' </h4></div></section>';
                            html += '<section class="table-detail"><dir class="row"><table><thead><tr><th colspan="3">Details of Grade Sheet</th></tr></thead><tbody><tr><td>90-100%</td><td>A+</td>';
                            html += ' <td>Outstanding</td></tr><tr><td>80-Below 90%</td><td>A</td><td>Excellent</td></tr><tr><td>70-Below 80%</td><td>B+</td><td>Very Good</td></tr>';

                            html += '<tr><td>60-Below 70%</td><td>B+</td><td>Good</td></tr><tr><td>50-Below 60%</td><td>C+</td><td>Satisfactory</td></tr><tr><td>40-Below 50%</td><td>C</td><td>Acceptable</td></tr>';
                            html += '<tr><td>30-Below 40%</td><td>D+</td><td>Partially Acceptable</td></tr><tr><td>20-Below 30%</td><td>D</td><td>Insufficient</td></tr><tr><td>00-Below 20%</td><td>E</td><td>Very Insuffecient</td></tr></tbody></table>';

                            html += '<article><label>Comments</label><ul>';

                            if (result.FinalGrade == "A+") {
                                html += '<li>Outstanding<input type="checkbox" name="vehicle" value="Bike" checked="checked"></li>';
                            }
                            else {
                                html += '<li>Outstanding<input type="checkbox" name="vehicle" value="Bike"></li>';
                            }

                            if (result.FinalGrade == "A") {
                                html += '<li>Excellent<input type="checkbox" name="vehicle" value="Bike" checked="checked"></li>';
                            }
                            else {
                                html += '<li>Excellent<input type="checkbox" name="vehicle" value="Bike"></li>';
                            }
                            if (result.FinalGrade == "B+") {
                                html += '<li>Very Good<input type="checkbox" name="vehicle" value="Bike" checked="checked"></li>';
                            }
                            else {
                                html += '<li>Very Good<input type="checkbox" name="vehicle" value="Bike"></li>';
                            }

                            if (result.FinalGrade == "B") {
                                html += '<li>Good<input type="checkbox" name="vehicle" value="Bike" checked="checked"></li>';

                            }
                            else {
                                html += '<li>Good<input type="checkbox" name="vehicle" value="Bike"></li>';
                            }

                            if (result.FinalGrade == "C+") {
                                html += '<li>Satisfactory<input type="checkbox" name="vehicle" value="Bike" checked="checked"></li>';
                            }
                            else {
                                html += '<li>Satisfactory<input type="checkbox" name="vehicle" value="Bike"></li>';
                            }

                            if (result.FinalGrade == "C") {
                                html += '<li>Acceptable<input type="checkbox" name="vehicle" value="Bike" checked="checked"></li>';
                            }
                            else {
                                html += '<li>Acceptable<input type="checkbox" name="vehicle" value="Bike"></li>';
                            }
                            if (result.FinalGrade == "D+") {
                                html += '<li>Partially Acceptable<input type="checkbox" name="vehicle" value="Bike" checked="checked"></li>';
                            }
                            else {
                                html += '<li>Partially Acceptable<input type="checkbox" name="vehicle" value="Bike"></li>';
                            }

                            if (result.FinalGrade == "D") {
                                html += '<li>Insufficient<input type="checkbox" name="vehicle" value="Bike" checked="checked"></li>';
                            }
                            else {
                                html += '<li>Insufficient<input type="checkbox" name="vehicle" value="Bike"></li>';
                            }

                            if (result.FinalGrade == "E") {
                                html += '<li>Very Insufficient<input type="checkbox" name="vehicle" value="Bike" checked="checked"></li>';
                            }
                            else {
                                html += '<li>Very Insufficient<input type="checkbox" name="vehicle" value="Bike"></li>';
                            }

                            html += '</ul></article></dir></section>';
                            html += '<footer><dir class="row"><ul><li><span class="md-bot-border">' + ConvertDateObjectToDate(result.Date) + '</span>Date</li><li><span class="md-bot-border">&nbsp;&nbsp;&nbsp;&nbsp</span>Class Teacher</li><li><span class="md-bot-border">&nbsp;&nbsp;&nbsp;&nbsp</span>Exam Co-ordinator</li><li><span class="md-bot-border">&nbsp;&nbsp;&nbsp;&nbsp</span>Principal</li></ul></dir></footer></main><div style="margin-top:200px"></div>';
                            i1 = 1;
                        });


                        $("#divPrintContiner").append(html);

                        $("#divPrintContiner").print();

                    }



                    if (data.ErrorOccured == true) {
                        ShowMessage(data.Messge);
                    }
                }
            }
            else
            {
              
                if (resulttype == 2)
                {
                    if (data.ErrorOccured == false)
                    {
                        $(data.Data).each(function (i, result)
                        {
                                                      

                            html += '<main class="marksheet"><header>';
                            if (result.Logo == null || result.Logo == "") {
                                html += '<img src="/Content/Images/School/School.png" height="120" width="150"/><dir class="row centered txt-centered"><h4>For Academic Excellence</h4>';

                            }
                            else {
                                html += '<img src=' + result.Logo + ' height="120" width="150"/><dir class="row centered txt-centered"><h4>For Academic Excellence</h4>';
                            }

                            html += '<h2>' + result.SchoolName + '</h2>';
                            html += '<div class="progress-report"><h2></h2><h2 style="text-align:center">Progress Report</h2><h5>' + result.Phone + '</h5></div>';
                            html += '<h4><span class="md-bot-border">Final</span> Examination &nbsp;&nbsp ' + result.ActiveSession + '</h4></dir></header>';

                            html += '<section class="student-detail"><div class="row centered txt-centered"><h4>The Marks Secured By &nbsp;&nbsp; <span class="lg-bot-border">' + result.StudentName + '</span> Son/Daughter of &nbsp;&nbsp; <span class="md-bot-border">' + result.FatherName + '</span>';
                            html += 'and &nbsp;&nbsp; <span class="md-bot-border">' + result.MotherName + '</span> of class &nbsp;&nbsp;<span class="sm-bot-border">' + result.Class + '</span> Section &nbsp;&nbsp; <span class="sm-bot-border">' + result.Section + '</span> Roll No &nbsp;&nbsp; <span class="sm-bot-border">' + result.RollNo + '</span> in the &nbsp;&nbsp<span class="md-bot-border">' + result.TermName + '</span> Examination are</h4>';
                            html += '</div></section>';
                            html += '<section class="tables"><dir class="row"><table id='+i+'><thead><tr>';
                            html += '<th>S.N</th><th>Subject</th><th>FM</th><th>PM</th>';

                            $(result.AllTermsForHeadings).each(function (index1, result1)
                            {
                               
                                html += '<th>' + result1.TermName + '(' + result1.TermPercentage + '%)</th>';
                            })
                            html+= '<th>Final</th><th>Grade</th><th>Grade Point</th><th>H.O.M</th></tr></thead>';
                            html += '<tbody>';
                            $(result.AllResultData).each(function (index11, result11)
                            {                               

                                html += '<tr><td>' + i1 + '</td><td>' + result11.SubjectName + '</td><td>' + result11.FM + '</td><td>'+result11.PM+'</td>';
                                $(result11.AllTerms).each(function (index111, result111)
                                {                                  
                                  
                                  html += '<td class=' + index111 + '>' + result111.TotalObtained + '</td>'
                                                                      
                                })
                                html += '<td>' + result11.TotalObtained+ '</td><td>'+result11.Grade+'</td><td>'+result11.GradePoint+'<td>'+result11.FinalTotal+'</td></td>';
                                i1++;
                            })

                          
                               
                          

                            html += '<tr><td colspan="2">Total</td><td>' + result.TotalFM + '</td><td>' + result.TotalPM + '</td>';

                           
                            html += '<td colspan='+result.AllTermsForHeadings.length+'></td>';
                            html+='<td>' + result.TotalObtained + '</td><td>' + result.FinalGrade + '</td><td>' + parseFloat(result.GradePoint).toFixed(2) + '</td><td></td></tr>';
                            html += '</tbody></table></dir>';
                            html += '<div class="flex"><h4>Attendance: ' + result.PresentDays + '/' + result.TotalDays + ' Days</h4><h4>Percentage:' + result.Percentage.toFixed(2) + '&nbsp;&nbsp;&nbsp;&nbsp;' + result.FinalGrade + '</h4></div></section>';
                            html += '<section class="table-detail"><dir class="row"><table><thead><tr><th colspan="3">Details of Grade Sheet</th></tr></thead><tbody><tr><td>90-100%</td><td>A+</td>';
                            html += ' <td>Outstanding</td></tr><tr><td>80-Below 90%</td><td>A</td><td>Excellent</td></tr><tr><td>70-Below 80%</td><td>B+</td><td>Very Good</td></tr>';

                            html += '<tr><td>60-Below 70%</td><td>B+</td><td>Good</td></tr><tr><td>50-Below 60%</td><td>C+</td><td>Satisfactory</td></tr><tr><td>40-Below 50%</td><td>C</td><td>Acceptable</td></tr>';
                            html += '<tr><td>30-Below 40%</td><td>D+</td><td>Partially Acceptable</td></tr><tr><td>20-Below 30%</td><td>D</td><td>Insufficient</td></tr><tr><td>00-Below 20%</td><td>E</td><td>Very Insuffecient</td></tr></tbody></table>';

                            html += '<article><label>Comments</label><ul>';

                            if (result.FinalGrade == "A+") {
                                html += '<li>Outstanding<input type="checkbox" name="vehicle" value="Bike" checked="checked"></li>';
                            }
                            else {
                                html += '<li>Outstanding<input type="checkbox" name="vehicle" value="Bike"></li>';
                            }

                            if (result.FinalGrade == "A") {
                                html += '<li>Excellent<input type="checkbox" name="vehicle" value="Bike" checked="checked"></li>';
                            }
                            else {
                                html += '<li>Excellent<input type="checkbox" name="vehicle" value="Bike"></li>';
                            }
                            if (result.FinalGrade == "B+") {
                                html += '<li>Very Good<input type="checkbox" name="vehicle" value="Bike" checked="checked"></li>';
                            }
                            else {
                                html += '<li>Very Good<input type="checkbox" name="vehicle" value="Bike"></li>';
                            }

                            if (result.FinalGrade == "B") {
                                html += '<li>Good<input type="checkbox" name="vehicle" value="Bike" checked="checked"></li>';

                            }
                            else {
                                html += '<li>Good<input type="checkbox" name="vehicle" value="Bike"></li>';
                            }

                            if (result.FinalGrade == "C+") {
                                html += '<li>Satisfactory<input type="checkbox" name="vehicle" value="Bike" checked="checked"></li>';
                            }
                            else {
                                html += '<li>Satisfactory<input type="checkbox" name="vehicle" value="Bike"></li>';
                            }

                            if (result.FinalGrade == "C") {
                                html += '<li>Acceptable<input type="checkbox" name="vehicle" value="Bike" checked="checked"></li>';
                            }
                            else {
                                html += '<li>Acceptable<input type="checkbox" name="vehicle" value="Bike"></li>';
                            }
                            if (result.FinalGrade == "D+") {
                                html += '<li>Partially Acceptable<input type="checkbox" name="vehicle" value="Bike" checked="checked"></li>';
                            }
                            else {
                                html += '<li>Partially Acceptable<input type="checkbox" name="vehicle" value="Bike"></li>';
                            }

                            if (result.FinalGrade == "D") {
                                html += '<li>Insufficient<input type="checkbox" name="vehicle" value="Bike" checked="checked"></li>';
                            }
                            else {
                                html += '<li>Insufficient<input type="checkbox" name="vehicle" value="Bike"></li>';
                            }

                            if (result.FinalGrade == "E") {
                                html += '<li>Very Insufficient<input type="checkbox" name="vehicle" value="Bike" checked="checked"></li>';
                            }
                            else {
                                html += '<li>Very Insufficient<input type="checkbox" name="vehicle" value="Bike"></li>';
                            }

                            html += '</ul></article></dir></section>';
                            html += '<footer><dir class="row"><ul><li><span class="md-bot-border">' + ConvertDateObjectToDate(result.Date) + '</span>Date</li><li><span class="md-bot-border">&nbsp;&nbsp;&nbsp;&nbsp</span>Class Teacher</li><li><span class="md-bot-border">&nbsp;&nbsp;&nbsp;&nbsp</span>Exam Co-ordinator</li><li><span class="md-bot-border">&nbsp;&nbsp;&nbsp;&nbsp</span>Principal</li></ul></dir></footer></main><div style="margin-top:200px"></div>';
                            i1 = 1;
                        });


                        $("#divPrintContiner").append(html);
                        $("#divPrintContiner").print();

                    }



                    if (data.ErrorOccured == true) {
                        ShowMessage(data.Messge);
                    }
                }
                else {

                    {
                        if (data.ErrorOccured == false) {
                            $(data.Data).each(function (i, result) {


                                html += '<main class="marksheet"><header>';
                                if (result.Logo == null || result.Logo == "") {
                                    html += '<img src="/Content/Images/School/School.png" height="120" width="150"/><dir class="row centered txt-centered"><h4>For Academic Excellence</h4>';

                                }
                                else {
                                    html += '<img src=' + result.Logo + ' height="120" width="150"/><dir class="row centered txt-centered"><h4>For Academic Excellence</h4>';
                                }

                                html += '<h2>' + result.SchoolName + '</h2>';
                                html += '<div class="progress-report"><h2></h2><h2 style="text-align:center">Progress Report</h2><h5>' + result.Phone + '</h5></div>';
                                html += '<h4><span class="md-bot-border">Final</span> Examination &nbsp;&nbsp ' + result.ActiveSession + '</h4></dir></header>';

                                html += '<section class="student-detail"><div class="row centered txt-centered"><h4>The Grade Secured By &nbsp;&nbsp; <span class="lg-bot-border">' + result.StudentName + '</span> Son/Daughter of &nbsp;&nbsp; <span class="md-bot-border">' + result.FatherName + '</span>';
                                html += 'and &nbsp;&nbsp; <span class="md-bot-border">' + result.MotherName + '</span> of class &nbsp;&nbsp;<span class="sm-bot-border">' + result.Class + '</span> Section &nbsp;&nbsp; <span class="sm-bot-border">' + result.Section + '</span> Roll No &nbsp;&nbsp; <span class="sm-bot-border">' + result.RollNo + '</span> in the &nbsp;&nbsp<span class="md-bot-border">' + result.TermName + '</span> Examination are</h4>';
                                html += '</div></section>';
                                html += '<section class="tables"><dir class="row"><table id=' + i + '><thead><tr>';
                                html += '<th>S.N</th><th>Subject</th>';

                                $(result.AllTermsForHeadings).each(function (index1, result1) {

                                    html += '<th>' + result1.TermName + '</th>';
                                })
                                html += '<th> Final Grade</th><th>Grade Point</th><th>Highest Grade</th></tr></thead>';
                                html += '<tbody>';
                                $(result.AllResultData).each(function (index11, result11) {

                                    html += '<tr><td>' + i1 + '</td><td>' + result11.SubjectName + '</td>';
                                    $(result11.AllTerms).each(function (index111, result111) {

                                        html += '<td class=' + index111 + '>' + result111.Grade + '</td>'

                                    })
                                    html += '<td>' + result11.Grade + '</td><td>' + result11.GradePoint + '<td>' + result11.HighestGradeObtained + '</td></td>';
                                    i1++;
                                })
                                 

                                 


                                html += '<tr><td colspan="6">Final</td>';
                              
                                html += '<td>' + result.FinalGrade + '</td><td>' + parseFloat(result.GradePoint).toFixed(2) + '</td><td></td></tr>';
                                html += '</tbody></table></dir>';
                                html += '<div class="flex"><h4>Attendance: ' + result.PresentDays + '/' + result.TotalDays + ' Days</h4><h4>Grade Point Average(GPA):' + parseFloat(result.GradePoint).toFixed(2) +'&nbsp;&nbsp;&nbsp;&nbsp;' + result.FinalGrade +  '</h4></div></section>';
                                html += '<section class="table-detail"><dir class="row"><table><thead><tr><th colspan="3">Details of Grade Sheet</th></tr></thead><tbody><tr><td>90-100%</td><td>A+</td>';
                                html += ' <td>Outstanding</td></tr><tr><td>80-Below 90%</td><td>A</td><td>Excellent</td></tr><tr><td>70-Below 80%</td><td>B+</td><td>Very Good</td></tr>';

                                html += '<tr><td>60-Below 70%</td><td>B+</td><td>Good</td></tr><tr><td>50-Below 60%</td><td>C+</td><td>Satisfactory</td></tr><tr><td>40-Below 50%</td><td>C</td><td>Acceptable</td></tr>';
                                html += '<tr><td>30-Below 40%</td><td>D+</td><td>Partially Acceptable</td></tr><tr><td>20-Below 30%</td><td>D</td><td>Insufficient</td></tr><tr><td>00-Below 20%</td><td>E</td><td>Very Insuffecient</td></tr></tbody></table>';

                                html += '<article><label>Comments</label><ul>';

                                if (result.FinalGrade == "A+") {
                                    html += '<li>Outstanding<input type="checkbox" name="vehicle" value="Bike" checked="checked"></li>';
                                }
                                else {
                                    html += '<li>Outstanding<input type="checkbox" name="vehicle" value="Bike"></li>';
                                }

                                if (result.FinalGrade == "A") {
                                    html += '<li>Excellent<input type="checkbox" name="vehicle" value="Bike" checked="checked"></li>';
                                }
                                else {
                                    html += '<li>Excellent<input type="checkbox" name="vehicle" value="Bike"></li>';
                                }
                                if (result.FinalGrade == "B+") {
                                    html += '<li>Very Good<input type="checkbox" name="vehicle" value="Bike" checked="checked"></li>';
                                }
                                else {
                                    html += '<li>Very Good<input type="checkbox" name="vehicle" value="Bike"></li>';
                                }

                                if (result.FinalGrade == "B") {
                                    html += '<li>Good<input type="checkbox" name="vehicle" value="Bike" checked="checked"></li>';

                                }
                                else {
                                    html += '<li>Good<input type="checkbox" name="vehicle" value="Bike"></li>';
                                }

                                if (result.FinalGrade == "C+") {
                                    html += '<li>Satisfactory<input type="checkbox" name="vehicle" value="Bike" checked="checked"></li>';
                                }
                                else {
                                    html += '<li>Satisfactory<input type="checkbox" name="vehicle" value="Bike"></li>';
                                }

                                if (result.FinalGrade == "C") {
                                    html += '<li>Acceptable<input type="checkbox" name="vehicle" value="Bike" checked="checked"></li>';
                                }
                                else {
                                    html += '<li>Acceptable<input type="checkbox" name="vehicle" value="Bike"></li>';
                                }
                                if (result.FinalGrade == "D+") {
                                    html += '<li>Partially Acceptable<input type="checkbox" name="vehicle" value="Bike" checked="checked"></li>';
                                }
                                else {
                                    html += '<li>Partially Acceptable<input type="checkbox" name="vehicle" value="Bike"></li>';
                                }

                                if (result.FinalGrade == "D") {
                                    html += '<li>Insufficient<input type="checkbox" name="vehicle" value="Bike" checked="checked"></li>';
                                }
                                else {
                                    html += '<li>Insufficient<input type="checkbox" name="vehicle" value="Bike"></li>';
                                }

                                if (result.FinalGrade == "E") {
                                    html += '<li>Very Insufficient<input type="checkbox" name="vehicle" value="Bike" checked="checked"></li>';
                                }
                                else {
                                    html += '<li>Very Insufficient<input type="checkbox" name="vehicle" value="Bike"></li>';
                                }

                                html += '</ul></article></dir></section>';
                                html += '<footer><dir class="row"><ul><li><span class="md-bot-border">' + ConvertDateObjectToDate(result.Date) + '</span>Date</li><li><span class="md-bot-border">&nbsp;&nbsp;&nbsp;&nbsp</span>Class Teacher</li><li><span class="md-bot-border">&nbsp;&nbsp;&nbsp;&nbsp</span>Exam Co-ordinator</li><li><span class="md-bot-border">&nbsp;&nbsp;&nbsp;&nbsp</span>Principal</li></ul></dir></footer></main><div style="margin-top:200px"></div>';
                                i1 = 1;
                            });


                            $("#divPrintContiner").append(html);
                            $("#divPrintContiner").print();

                        }



                        if (data.ErrorOccured == true) {
                            ShowMessage(data.Messge);
                        }
                    }



                    if (data.ErrorOccured == true) {
                        ShowMessage(data.Messge);
                    }
                }
            }

        },
        error: function (response)
        {
            ShowMessage("Warning ! Error Occured");

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


$(document).ready(function () {

});
var p;
var q;
var r;
var s;
var t;

function findStudent(a, b) {
    $.ajax({
        url: "/Client/StudentProfile/Index",
        type: 'POST',
        data: {
            a: a,
        },
        success: function (result) {
            $('#loadcontent').html(result);
        }
    })
}

function findFeeDetails(a, b, c) {
    $.ajax({
        url: "/Client/FeeDetails/Index",
        type: 'POST',
        data: { a: a, b: b },
        success: function (result) {
            $('#loadcontent').html(result);
        }
    })
}


function getTerms(a, b, c, d, e, f) {
    p = b;
    q = d;
    r = e;
    s = f;
    t = a;
    $.ajax({
        url: "/Client/Result/Index",
        type: 'POST',
        data: { a: a, b: b },
        success: function (result) {
            $('#loadcontent').html(result);
        }
    })
}
function getMarksheet(u, v) {
    $.ajax({
        url: "/Client/Result/ViewResult",
        type: 'POST',
        data: { p: p, q: q, r: r, s: s, t: t, u: u },
        success: function (data) {
            console.log(data)
            var html = '', i1 = 1, j, k, resulttype;
            $("#divPrintContiner").empty();
            resulttype = v;
            if (resulttype == 2) {
                if (data.ErrorOccured == false) {
                    $(data.Data).each(function (i, result) {
                        html += '<main class="marksheet"><header>';
                        html += '<img src=' + result.Logo + ' height="120" width="150"/><dir class="row centered txt-centered"><h4>For Academic Excellence</h4>';
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
                                var totalobt = 'Not Entered'
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

                        html += '<tr><td>60-Below 70%</td><td>B+</td><td>Good</td></tr><tr><td>50-Below 60%</td><td>C+</td><td>Satisfactory</td></tr><tr><td>40-Below 50%</td><td>C</td><td>Acceptable</td></tr>';
                        html += '<tr><td>30-Below 40%</td><td>D+</td><td>Partially Acceptable</td></tr><tr><td>20-Below 30%</td><td>D</td><td>Insufficient</td></tr><tr><td>00-Below 20%</td><td>E</td><td>Very Insuffecient</td></tr></tbody></table>';

                        html += '<article><label>Comments</label><ul><li>Outstanding<input type="checkbox" name="vehicle" value="Bike"></li><li>Excellent<input type="checkbox" name="vehicle" value="Bike"></li><li>Very Good<input type="checkbox" name="vehicle" value="Bike"></li><li>Good<input type="checkbox" name="vehicle" value="Bike"></li>';

                        html += '<li>Satisfactory<input type="checkbox" name="vehicle" value="Bike"></li><li>Acceptable<input type="checkbox" name="vehicle" value="Bike"></li><li>Partially Acceptable<input type="checkbox" name="vehicle" value="Bike"></li><li>Insufficient<input type="checkbox" name="vehicle" value="Bike"></li><li>Very Insuffecient<input type="checkbox" name="vehicle" value="Bike"></li>';
                        html += '</ul></article></dir></section>';
                        html += '<footer><dir class="row"><ul><li><span class="md-bot-border">' + ConvertDateObjectToDate(result.Date) + '</span>Date</li><li><span class="md-bot-border">&nbsp;&nbsp;&nbsp;&nbsp</span>Class Teacher</li><li><span class="md-bot-border">&nbsp;&nbsp;&nbsp;&nbsp</span>Exam Co-ordinator</li><li><span class="md-bot-border">&nbsp;&nbsp;&nbsp;&nbsp</span>Principal</li></ul></dir></footer></main><div style="margin-top:200px"></div>';
                        i1 = 1;
                    });


                    $("#divPrintContiner").append(html);

                    $("#divPrintContiner").print();

                }



                if (data.ErrorOccured == true) {
                    ShowMessage("Please Contact School/College.");
                }
            }
            else {

                if (data.ErrorOccured == false) {
                    $(data.Data).each(function (i, result) {
                        html += '<main class="marksheet"><header>';
                        html += '<img src=' + result.Logo + ' height="120" width="150"/><dir class="row centered txt-centered"><h4>For Academic Excellence</h4>';
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

                        html += '<article><label>Comments</label><ul><li>Outstanding<input type="checkbox" name="vehicle" value="Bike"></li><li>Excellent<input type="checkbox" name="vehicle" value="Bike"></li><li>Very Good<input type="checkbox" name="vehicle" value="Bike"></li><li>Good<input type="checkbox" name="vehicle" value="Bike"></li>';

                        html += '<li>Satisfactory<input type="checkbox" name="vehicle" value="Bike"></li><li>Acceptable<input type="checkbox" name="vehicle" value="Bike"></li><li>Partially Acceptable<input type="checkbox" name="vehicle" value="Bike"></li><li>Insufficient<input type="checkbox" name="vehicle" value="Bike"></li><li>Very Insuffecient<input type="checkbox" name="vehicle" value="Bike"></li>';
                        html += '</ul></article></dir></section>';
                        html += '<footer><dir class="row"><ul><li><span class="md-bot-border">' + ConvertDateObjectToDate(result.Date) + '</span>Date</li><li><span class="md-bot-border">&nbsp;&nbsp;&nbsp;&nbsp</span>Class Teacher</li><li><span class="md-bot-border">&nbsp;&nbsp;&nbsp;&nbsp</span>Exam Co-ordinator</li><li><span class="md-bot-border">&nbsp;&nbsp;&nbsp;&nbsp</span>Principal</li></ul></dir></footer></main><div style="margin-top:200px"></div>';
                        i1 = 1;
                    });


                    $("#divPrintContiner").append(html);

                    $("#divPrintContiner").print();

                }



                if (data.ErrorOccured == true) {
                    ShowMessage("Please Contact School/College.");
                }
            }

        },
        error: function () {
            ShowMessage("Please Contact School/College.");
        }
    })
}

function printMarksheet() {
    var u = document.getElementById("ID").value;
    var resultType = 1;
    $.ajax({
        url: "/Client/Result/getResultType",
        type: 'POST',
        data: { p: p, q: q, r: r, s: s, t: t, u: u },
        success: function (data) {
            resultType = data;
            getMarksheet(u, resultType);
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
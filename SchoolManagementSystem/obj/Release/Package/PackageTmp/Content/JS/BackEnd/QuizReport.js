$(document).ready(function () {
    ReadGrid();
    InitializeForm();
    InitializeStartDateDependency();
    InitializeEndDateDependency();
    UIEvent();
})

var counter = 0;
var rowNumber = 0;
var wnd;
function InitializeForm() {
    $('#ddlUserGroup').SumoSelect({
        csvDispCount: 3,
        search: true,
    });
}
function FormatCompletedDate(CompletedDate) {
    console.log(CompletedDate);
    if (kendo.toString(new Date(CompletedDate), "yyyy/MM/dd") == kendo.toString(new Date("01-01-1900"), "yyyy/MM/dd")) {
        CompletedDate = "-";
    }
    else {
        CompletedDate = kendo.toString(new Date(CompletedDate), CustomDateFormat.replace("{", "").replace("}", "").replace("0:", ""));
    }
    return CompletedDate;
}
function ReadGrid() {
    var grid = $("#QuizReportListing").data("kendoGrid");
    grid.dataSource.page(1);
}

AddAntiForgeryToken = function (data) {
    data.__RequestVerificationToken = $('form input[name=__RequestVerificationToken]').val();
    return data;
};
function ConvertDateObjectToDate(dateObject) {
    var dateString = dateObject.substr(6);
    var currentTime = new Date(parseInt(dateString));
    var month = currentTime.getMonth() + 1;
    var day = currentTime.getDate();
    var year = currentTime.getFullYear();
    //var date = year + "-" + month + "-" + day;
    var date = month + "/" + day + "/" + year;
    return date;
}
function onDatabound() {
}
function trimQuizTitle(data) {
    if (data.length > 50) {
        data = data.substring(0, 50) + '...';
    }
    return data;
}

function trimQuestion(data) {
    if (data.length > 50) {
        data = data.substring(0, 50) + '...';
    }
    return data;
}


function InitializeStartDateDependency() {
    function startChangeForStartedDate() {
        var startDate = start1.value(),
            endDate = end1.value();

        if (startDate) {
            startDate = new Date(startDate);
            startDate.setDate(startDate.getDate());
            end1.min(startDate);
        } else if (endDate) {
            start1.max(new Date(endDate));
        } else {
            endDate = new Date();
            start1.max(endDate);
            end1.min(endDate);
        }
    }

    function endChangeForStartedDate() {
        var endDate = end1.value(),
            startDate = start1.value();

        if (endDate) {
            endDate = new Date(endDate);
            endDate.setDate(endDate.getDate());
            start1.max(endDate);
        } else if (startDate) {
            end1.min(new Date(startDate));
        } else {
            endDate = new Date();
            start1.max(endDate);
            end1.min(endDate);
        }
    }
    var start1 = $("#txtSearchQuizStartedFrom").kendoDatePicker({
        change: startChangeForStartedDate,
        format: "MM/dd/yyyy",

    }).data("kendoDatePicker");

    var end1 = $("#txtSearchQuizStartedTo").kendoDatePicker({
        change: endChangeForStartedDate,
        format: "MM/dd/yyyy",

    }).data("kendoDatePicker");

    start1.max(end1.value());
    end1.min(start1.value());
}

function InitializeEndDateDependency() {

    function startChangeForEndedDate() {
        var startDate = start2.value(),
            endDate = end2.value();

        if (startDate) {
            startDate = new Date(startDate);
            startDate.setDate(startDate.getDate());
            end2.min(startDate);
        } else if (endDate) {
            start2.max(new Date(endDate));
        } else {
            endDate = new Date();
            start2.max(endDate);
            end2.min(endDate);
        }
    }

    function endChangeForEndedDate() {
        var endDate = end2.value(),
            startDate = start2.value();

        if (endDate) {
            endDate = new Date(endDate);
            endDate.setDate(endDate.getDate());
            start2.max(endDate);
        } else if (startDate) {
            end2.min(new Date(startDate));
        } else {
            endDate = new Date();
            start2.max(endDate);
            end2.min(endDate);
        }
    }

    var start2 = $("#txtSearchQuizEndFrom").kendoDatePicker({
        change: startChangeForEndedDate,
        format: "MM/dd/yyyy",

    }).data("kendoDatePicker");

    var end2 = $("#txtSearchQuizEndTo").kendoDatePicker({
        change: endChangeForEndedDate,
        format: "MM/dd/yyyy",

    }).data("kendoDatePicker");

    start2.max(end2.value());
    end2.min(start2.value());
}
function UnSelectUserGroup() {
    var num = $('select#ddlUserGroup option').length;
    for (var i = 0; i < num; i++) {
        $('select#ddlUserGroup')[0].sumo.unSelectItem(i);
    }

}

function onDataboundQuizListing(e) {
    $(".k-grid-Details").attr('title', 'View Detail');

    $(".k-grid-Details").find("span").addClass("fa fa-eye");
    $(".k-grid-Details").removeClass("k-button");

    var grid = e.sender;
    if (grid.dataSource.total() == 0) {
        var colCount = grid.columns.length;
        $(e.sender.wrapper)
            .find('tbody')
            .append('<tr class="kendo-data-row text-center"><td colspan="' + colCount + '" class="no-data">No Data Available</td></tr>');
    }
    var pageSizes = [10, 20, 30, 50, 80];
    var pageSizearr = [];
    if (pageSizes.length > 0) {
        $.each(pageSizes, function (val, size) {
            pageSizearr.push({ text: size, value: size });
        });
    } else {
        pageSizearr = [10, 20, 30, 50, 80];
    }

    $('.k-pager-sizes select[data-role="dropdownlist"]').data('kendoDropDownList').setDataSource(new kendo.data.DataSource({ data: pageSizearr }));
}



function renderNumber(data) {
    return ++rowNumber;
}

function renderRecordNumber(data) {
    var page = parseInt($("#QuizReportListing").data("kendoGrid").dataSource.page()) - 1;
    var pagesize = $("#QuizReportListing").data("kendoGrid").dataSource.pageSize();
    return parseInt(rowNumber + (parseInt(page) * parseInt(pagesize)));
}
function ParamToLoadQuizListing() {

    var objInfo1 =
       {

           SearchStartedFrom: $('#txtSearchQuizStartedFrom').val(),
           SearchStartedTo: $('#txtSearchQuizStartedTo').val(),
           SearchEndFrom: $('#txtSearchQuizEndFrom').val(),
           SearchEndTo: $('#txtSearchQuizEndTo').val(),
           SearchStatusID: $('#ddlSearchStatus :selected').val() == "" ? -1 : $('#ddlSearchStatus :selected').val(),
           SearchQuizTitle: $('#txtSearchQuizTitle').val(),
           UserGroupID: $('#ddlUserGroup :selected').val(),
           QuizCategory: $('#ddlSearchQuizCategory :selected').val() == "" ? -1 : $('#ddlSearchQuizCategory :selected').val(),
       };
    return {
        objInfo1: JSON.stringify(objInfo1)
    };
}
function QuizDetails(e) {
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    var mydata = {
        QuizID: dataItem.QuizID,
    };
    $.ajax({
        type: "post",
        dataType: "json",
        url: '/Admin/QuizReport/GetQuizByID',
        data: AddAntiForgeryToken(mydata),
        success: function (data) {
            debugger;
            if (data.success = "200") {
                var JsonData;
                var UserInfo;
                $("#divDetailsContainer").show();
                $("#divQuizGrid").hide();
                $("#divSearchContainer").hide();
                $("#QuiztitleDesc").html(trimQuizTitle(data.data.QuizTitle));
                $("#QuizStartDate").html(FormatCompletedDate(ConvertDateObjectToDate(data.data.StartDate)));
                $("#QuizEndDate").html(FormatCompletedDate(ConvertDateObjectToDate(data.data.EndDate)));
                $("#QuizTotalQuestion").html(data.data.TotalQuestion);
                $("#QuizAppearingPoints").html(data.data.QuizAppearingPoints);
                $("#QuizStatus").html(data.data.StatusName);
                $("#QuizUserGroup").html(data.data.UserGroup);
                $("#QuizShowCorrectAnswer").html(data.data.CanShowCorrectAnswer == true ? "Yes" : "No");
                $("#StartPageDescription").html(data.data.StartPageDescription);
                $("#EndPageDescription").html(data.data.EndPageDescription);
                var CorrectAnswer = ((data.data.TotalCorrectAnswerForAll / data.data.TotalQuizQuestionForAll) * 100).toFixed(2);
                var IncorrectAnswer = ((data.data.TotalInCorrectAnswerForAll / data.data.TotalQuizQuestionForAll) * 100).toFixed(2);
                $("#AvgCorrectAnswer").html(isNaN(CorrectAnswer) ? 0 : CorrectAnswer);
                $("#AvgIncorrectAnswer").html(isNaN(IncorrectAnswer) ? 0 : IncorrectAnswer);
                $("#AvgQuizTimeSpent").html(fancyTimeFormat(data.data.AverageTimeOnQuiz.toFixed(2)));
                $("#DescQuizTitle").html('').html(trimQuizTitle(data.data.QuizTitle));
                $("#MarkSheetQuizTitle").html('').html(trimQuizTitle(data.data.QuizTitle));
                $("#NoofUser").html(data.data.TotalUserInQuiz);
                var QuestionArray;
                var UserArray;
                if (data.data.JSONDATA == null) {
                    QuestionArray = [];
                }
                else if (data.data.JSONDATA.indexOf("[") != -1) {
                    QuestionArray = JSON.parse(data.data.JSONDATA);
                }
                else {
                    QuestionArray = JSON.parse("[" + data.data.JSONDATA + "]");
                }
                console.log(data.data.JsonUserInfo);
                if (data.data.JsonUserInfo == null) {
                    UserArray = [];
                }
                else if (data.data.JsonUserInfo.indexOf("[") != -1) {
                    UserArray = JSON.parse(data.data.JsonUserInfo);
                }
                else {
                    UserArray = JSON.parse("[" + data.data.JsonUserInfo + "]");
                }

                if (data.data.IsQuestionManual) {
                    $("div.ManualGridParentContainer").show();
                    $("#divManualQuestionContainer").show();
                    $("#divDynamicQuestionContainer").hide();
                    $("#ManualGrid").kendoGrid({
                        dataSource: {
                            data: QuestionArray,
                            pageSize: CustomRecordPerPage
                        },
                        filterable: false,
                        groupable: false,
                        sortable: true,
                        ServerOperation: false,
                        dataBound: onDataBoundSelectedQuestionGrid,
                        pageable: {
                            pageSizes: true,
                            refresh: true,
                            buttonCount: 5
                        },
                        columns: [{
                            field: "QuizQuestion",
                            title: "Question",
                           template: "#= trimQuestion(QuizQuestion) #",
                            width: 240
                        },
                        {
                            field: "DifficultyLevel",
                            title: "DifficultyLevel"
                        }
                        , {
                            field: "QuestionWeight",
                            title: "QuestionWeight"
                        }
                        , {
                            field: "PointsToEachAnswer",
                            title: "Score"
                        }
                        ]
                    });
                }
                else {
                    $("#divManualQuestionContainer").hide();
                    $("#divDynamicQuestionContainer").show();
                    $("#divTotalMandatoryQuestion>p").remove();
                    $("#divTotalOptionalQuestion>p").remove();
                    $("#divQuestionCategory>p").remove();
                    $("#divQuestionDifficulty>p").remove();
                    $.each(QuestionArray, function (index, item) {
                        $("#divTotalMandatoryQuestion").append("<p class='form-control-static'>" + item.TotalMandatoryQuestion + "</p>");
                        $("#divTotalOptionalQuestion").append("<p class='form-control-static'>" + item.TotalOptionalQuestion + "</p>");
                        $("#divQuestionCategory").append("<p class='form-control-static'>" + item.QuestionCategory + "</p>");
                        $("#divQuestionDifficulty").append("<p class='form-control-static'>" + item.QuestionDifficulty + "</p>");
                    });
                    $("div.ManualGridParentContainer").hide();
                }
                $("#UserQuizInfo").kendoGrid({
                    dataSource: {
                        data: UserArray,
                        pageSize: CustomRecordPerPage
                    },
                    filterable: false,
                    groupable: false,
                    sortable: false,
                    ServerOperation: false,
                    dataBound: onDataBoundUserQuizInfoGrid,
                    excel: {
                        fileName: "UserQuizExcel-Export.xlsx",
                        allPages: true,
                        filterable: true
                    },
                    excelExport: function (e) {
                        var sheet = e.workbook.sheets[0];
                        var data, DateStr;

                        for (var rowIndex = 1; rowIndex < sheet.rows.length; rowIndex++) {
                            //skipping first row, because type is header
                            var row = sheet.rows[rowIndex];
                            data = row.cells[2].value;
                            DateStr = FormatCompletedDate(data);
                            row.cells[2].value = DateStr

                            data = row.cells[3].value;
                            if (data != "-") {
                                DateStr = FormatCompletedDate(data);
                            }
                            else {
                                DateStr = data;
                            }
                            row.cells[3].value = DateStr
                        }
                    },
                    pdf: {
                        allPages: true,
                        avoidLinks: true,
                        paperSize: "A4",
                        // margin: { top: "2cm", left: "1cm", right: "1cm", bottom: "1cm" },
                        landscape: true,
                        repeatHeaders: true,
                        template: $("#page-template").html(),
                        scale: 0.8,
                        creator: "Ncell LMS",
                        fileName: "UserQuizPDF-Export.pdf",
                        keywords: "northwind products",
                        title: "Products title",
                        subject: "Products subject",
                        date: new Date("2014/10/10"),
                        //forceProxy: true,
                        //proxyURL: "https://demos.telerik.com/kendo-ui/service/Northwind.svc/Products"
                        // .ClientTemplate("#= trimQuestion(QuizQuestion) #").Width(200)
                    },
                    pageable: {
                        pageSizes: true,
                        refresh: true,
                        buttonCount: 5
                    },
                    columns: [
                        {
                            field: "QuizUserID",
                            title: "QuizUserID",
                            width: 100,
                            hidden: true
                        },
                        {
                            field: "UserName",
                            title: "UserName",
                            width: 100
                        },
                    {
                        field: "QuizStatus",
                        title: "Status",
                        width: 100
                    }
                    , {
                        field: "JoinedDate",
                        title: "JoinedDate",
                        template: "#= FormatCompletedDate(JoinedDate) #",
                        width: 100
                    }
                    , {
                        field: "CompletedDate",
                        title: "CompletedDate",
                        template: "#= FormatCompletedDate(CompletedDate) #",
                        width: 100
                    }
                  , {
                      field: "ElapsedTime",
                      title: "ElapsedTime(In Second)",
                      width: 100
                  }
                , {
                    field: "UserScore",
                    title: "Score",
                    width: 100
                },
                {
                    command:
                        {
                            name: "Details", text: " ", click: showDetails
                        },
                    title: "Details", width: 100
                }
                    ]
                });
            }

        }
    });
}
function onDataBoundSelectedQuestionGrid(e) {
    var grid = e.sender;
    if (grid.dataSource.total() == 0) {
        $('#ManualGrid').hide();
    }
    else {
        $('#ManualGrid').show();
    }

    var pageSizes = [10, 20, 30, 50, 80];//LoadGridRecordPerPage(RecordPerPage, RecordPerPage, 5);
    var pageSizearr = [];
    if (pageSizes.length > 0) {
        $.each(pageSizes, function (val, size) {
            pageSizearr.push({ text: size, value: size });
        });
    } else {
        pageSizearr = [10, 20, 30, 50, 80];
    }

    $('.k-pager-sizes select[data-role="dropdownlist"]').data('kendoDropDownList').setDataSource(new kendo.data.DataSource({ data: pageSizearr }));
}
function showDetails(e) {
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    var mydata = {
        QuizUserID: dataItem.QuizUserID,
    };
    $.ajax({
        type: "post",
        dataType: "json",
        url: 'QuizReport/GetUserQuizAnswerByUserID',
        data: AddAntiForgeryToken(mydata),
        success: function (data) {
            if (data.success == "200") {
                var QuizUserQuestion;
                if (data.data.UserQuizQuestion == null) {
                    QuizUserQuestion = [];
                }
                else if (data.data.UserQuizQuestion.indexOf("[") != -1) {
                    QuizUserQuestion = JSON.parse(data.data.UserQuizQuestion);
                }
                else {
                    QuizUserQuestion = JSON.parse("[" + data.data.UserQuizQuestion + "]");
                }
               // $("#DescQuizTitle").html('').html(dataItem.QuizTitle);
                $("#DescUserFullName").html('').html(dataItem.UserName);
                $("#DescJoinedDate").html('').html(FormatCompletedDate(dataItem.JoinedDate));
                $("#DescCompletionDate").html('').html(FormatCompletedDate(dataItem.CompletedDate));
                $("#DescCompletedTime").html('').html(fancyTimeFormat(dataItem.ElapsedTime));
                $("#DescQuizStatus").html('').html(dataItem.QuizStatus);
                $("#DescTotalQuestion").html('').html(data.data.TotalQuestion);
                $("#DescCorrectAnswer").html('').html(data.data.CorrectAnswer);
                $("#DescIncorrectAnswer").html('').html(data.data.IncorrectAnswer);
                $("#MarkSheetUserName").html('').html(dataItem.UserName);
                $("#MarkSheetJoinedDate").html('').html(FormatCompletedDate(dataItem.JoinedDate));
                $("#MarkSheetCompletedDate").html('').html(FormatCompletedDate(dataItem.CompletedDate));
                $("#MarkSheetCompletionTime").html('').html(fancyTimeFormat(dataItem.ElapsedTime));
                $("#MarkSheetQuizCurrentScore").html('').html(data.data.TotalUserScore + "/" + data.data.TotalQuizScore)
                var html = "";
                var UserSelectedOption;
                $.each(QuizUserQuestion, function (index, item) {

                    if (parseInt(item.UserScore) > 0) {
                        html += "<tr class='notifications-status'><td>";
                    }
                    else {
                        html += "<tr class='notification-status-zero'><td>";
                    }
                    html += index + 1;
                    html += "</td>";
                    html += "<td>";
                    html += "<p>";
                    html += trimQuestion(item.QuizQuestion);
                    html += "<p>";
                    if (item.IsObjective == "0") {
                        html += "<p>";
                        html += item.SubjectiveDescription;
                        html += "</p>";
                    }
                    else {
                        $.each(item.UserAnswer.split('`'), function (index, item) {
                            html += "<p>";
                            html += item;
                            html += "</p>";
                        });
                    }
                    html += "</td>";
                    html += "<td><p class='score-point'>Score:";
                    html += "<span><span class='purple'>";
                    html += item.UserScore;
                    html += "</span>/";
                    html += item.TotalQuestionScore;
                    html += "</span>";
                    html += "</p></td>";
                    html += "<td><p>";
                    html += fancyTimeFormat(item.CompletedTime);
                    html += "</p></td>";
                    html += "<td class='edit-btn'>";
                    html += "<p>";
                    html += "<div id='divUserMark' style='display:none'>"
                    html += "<input class='txtUserPointScore' type='number' value='" + item.UserScore + "' max='" + item.TotalQuestionScore + "' min='0'/>";
                    html += "<button class='btn btn-default btn-xs btnCustom btnAssignSubjectiveMark mlr-xsm' mydata='" + item.UserQuizQuestionAnswerID + "'>Save</button><button class='btn btn-default btnCancle btn-xs btnDefault btnUserMarkScoreClear'>Cancel</button></div>";
                    html += "<a href='#' id='btnPointQuestion'><i class='fa fa-pencil' aria-hidden='true'></i></a></p>";
                    html += "</td>";
                    html += "</tr>";
                    $("#tblUserQuizQuestion").html('').append(html);
                });
                $("#divDetailsContainer").hide();
                $("#divUserDetailsContainer").show();
                UIEvent();
            }
        }
    })
}
function onDataBoundUserQuizInfoGrid(e) {
    var grid = e.sender;
    console.log(grid.dataSource.total());
    if (grid.dataSource.total() == 0) {
        $('#UserQuizInfo').parent().hide();
        $('#UserQuizInfo').hide();
    }
    else {
        $('#UserQuizInfo').parent().show();
        $('#UserQuizInfo').show();
    }
    $(".k-grid-Details").attr('title', 'View Detail');

    $(".k-grid-Details").find("span").addClass("fa fa-eye");
    $(".k-grid-Details").removeClass("k-button");


    var pageSizes = [10, 20, 30, 50, 80];// LoadGridRecordPerPage(RecordPerPage, RecordPerPage, 5);
    var pageSizearr = [];
    if (pageSizes.length > 0) {
        $.each(pageSizes, function (val, size) {
            pageSizearr.push({ text: size, value: size });
        });
    } else {
        pageSizearr = [10, 20, 30, 50, 80];
    }

    $('#UserQuizInfo .k-pager-sizes select[data-role="dropdownlist"]').data('kendoDropDownList').setDataSource(new kendo.data.DataSource({ data: pageSizearr }));
}
function ClearAll() {
}
function UIEvent() {
    $("#btnSearch").off().on("click", function (e) {
        e.preventDefault();
        ReadGrid();
    });
    $("#btnBackToGrid").off().on('click', function (e) {
        e.preventDefault();
        $("#divDetailsContainer").hide();
        $("#divSearchContainer").show();
        $("#divQuizGrid").show();
    });
    $("#btnReset").off().on('click', function (e) {
        e.preventDefault();
        $("#txtSearchQuizTitle").val('');
        $("#ddlSearchStatus").val('');
        $("#txtSearchQuizStartedFrom").val('');
        $("#txtSearchQuizStartedTo").val('');
       // $("#ddlUserGroup")[0].sumo.unSelectAll();
        UnSelectUserGroup();
        $("#txtSearchQuizEndFrom").val('');
        $("#txtSearchQuizEndTo").val('');
        $("#ddlSearchQuizCategory").val('');
        ReadGrid();
    });
    $("#btnBackToQuizDetail").on('click', function (e) {
        e.preventDefault();
        $("#divDetailsContainer").show();
        $("#divUserDetailsContainer").hide();
    });

    $("div#MarkSheetContainer a#btnPointQuestion").off().on('click', function (e) {
        e.preventDefault();
        $(this).prev().show();
        $(this).hide();
    });
    $("#tblUserQuizQuestion button.btnUserMarkScoreClear").off().on('click', function (e) {
        e.preventDefault();
        $(this).closest('td').find('div#divUserMark').hide();
        $(this).closest('td').find('input.txtUserPointScore').val('0');
        $(this).closest('td').find('a#btnPointQuestion').show();
    });

    $("#btnExportPDF").off().on('click', function (e) {
        e.preventDefault();
        $(this).hide();
        kendo.drawing.drawDOM($("div#MarkSheetContainer"))
    .then(function (group) {
        // Render the result as a PDF file
        return kendo.drawing.exportPDF(group, {
            paperSize: "auto",
            margin: { left: "1cm", top: "1cm", right: "1cm", bottom: "1cm" }
        });
    })
    .done(function (data) {

        kendo.saveAs({
            dataURI: data,
            fileName: "MarkSheet.pdf",
        });

    });
        $(this).show();
    });
    $("#btn_ExportPDFUserQuizInfo").on('click', function (e) {
        e.preventDefault();
        $("#UserQuizInfo").getKendoGrid().saveAsPDF();
    });
    $("#btn_ExportExcelUserQuizInfo").on('click', function (e) {
        e.preventDefault();
        $("#UserQuizInfo").getKendoGrid().saveAsExcel();
    });
}




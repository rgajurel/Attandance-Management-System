$(document).ready(function () {
    LoadQuizUserGrid();
    InitializeUserGroup();
    InitializeUserJoinedDateDependency();
    InitializeUserCompletedDateDependency();
    UIEvent();
});
var rowNumber = 0;
function ParamToLoadQuizUserReportList(e) {
    var grid = $("#QuizUserReport").data("kendoGrid").dataSource;
    var objInfo1 =
   {
       SearchEntranceName: $('#txtSearchQuizTitle').val(),
       SearchUserGroup: $('#ddlSearchUserGroup').val() == null ? '' : $('#ddlSearchUserGroup').val().join(','),
       SearchUserID: $("#ddlSearchUserID :selected").val() == "" ? -1 : $("#ddlSearchUserID :selected").val(),
       SearchCompletionTime: $("#txtSearchCompletionTime").val() == "" ? -1 : $("#txtSearchCompletionTime").val(),
       SearchJoinedFrom: $("#txtSearchUserJoinedFrom").val(),
       SearchJoinedTo: $("#txtSearchUserJoinedTo").val(),
       SearchCompletedFrom: $("#txtSearchUserCompletedFrom").val(),
       SearchCompletedTo: $("#txtSearchUserCompletedTo").val(),
       SearchEntranceStatus: $("#ddlSearchQuizStatus :selected").val(),
       pageSize: grid._pageSize,
       pageNumber: grid._page
   };
    return {
        objInfo: JSON.stringify(objInfo1)
    };
}
AddAntiForgeryToken = function (data) {
    data.__RequestVerificationToken = $('form input[name=__RequestVerificationToken]').val();
    return data;
};
function LoadQuizUserGrid() {
    var grid = $("#QuizUserReport").data("kendoGrid");
    grid.dataSource.page(1);
}
function onDatabound(e) {
    rowNumber = 0;
    $(".k-grid-Details").attr('title', 'View Detail');

    $(".k-grid-Details").addClass("fa fa-eye");
    $(".k-grid-Details").removeClass("k-button");
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
function renderNumber(data) {
    return ++rowNumber;
}
function FormatCompletedDate(CompletedDate) {
    if (kendo.toString(new Date(CompletedDate), "yyyy/MM/dd") == kendo.toString(new Date("01-01-1900"), "yyyy/MM/dd")) {
        CompletedDate = "-";
    }
    else {
        CompletedDate = kendo.toString(new Date(CompletedDate), CustomDateFormat.replace("{", "").replace("}", "").replace("0:", ""));
    }
    return CompletedDate;
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
function renderRecordNumber(data) {
    var page = parseInt($("#QuizUserReport").data("kendoGrid").dataSource.page()) - 1;
    var pagesize = $("#QuizUserReport").data("kendoGrid").dataSource.pageSize();
    return parseInt(rowNumber + (parseInt(page) * parseInt(pagesize)));
}


function InitializeUserJoinedDateDependency() {

    function startChangeForJoinedDate() {
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

    function endChangeForJoinedDate() {
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

    var start2 = $("#txtSearchUserJoinedFrom").kendoDatePicker({
        change: startChangeForJoinedDate,
        format: "MM/dd/yyyy",

    }).data("kendoDatePicker");

    var end2 = $("#txtSearchUserJoinedTo").kendoDatePicker({
        change: endChangeForJoinedDate,
        format: "MM/dd/yyyy",

    }).data("kendoDatePicker");

    start2.max(end2.value());
    end2.min(start2.value());
}

function InitializeUserCompletedDateDependency() {

    function startChangeForCompletedDate() {
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

    function endChangeForCompletedDate() {
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

    var start2 = $("#txtSearchUserCompletedFrom").kendoDatePicker({
        change: startChangeForCompletedDate,
        format: "MM/dd/yyyy",

    }).data("kendoDatePicker");

    var end2 = $("#txtSearchUserCompletedTo").kendoDatePicker({
        change: endChangeForCompletedDate,
        format: "MM/dd/yyyy",

    }).data("kendoDatePicker");

    start2.max(end2.value());
    end2.min(start2.value());
}

function InitializeUserGroup() {

    $('#ddlSearchUserGroup').SumoSelect({
        search: true,
    });
    $('select#ddlSearchUserGroup').prepend('<option selected disabled hidden value="-1">Select User Group</option>');
    $('#ddlSearchUserGroup').prop("selectedIndex", -1);
}
function ResetSearcForm() {
    $("#txtSearchQuizTitle").val('');
   // $('#ddlSearchUserGroup')[0].sumo.unSelectAll();

    for (var i = 0; i < $('select#ddlSearchUserGroup option').length; i++) {
        $('select#ddlSearchUserGroup')[0].sumo.unSelectItem(i);
    }
    $("#ddlSearchUserID").val('');
    $("#txtSearchCompletionTime").val('');
    $("#txtSearchUserJoinedFrom").val('');
    $("#txtSearchUserJoinedTo").val('');
    $("#ddlSearchQuizStatus").val('');
    $("#txtSearchUserCompletedFrom").val("");
    $("#txtSearchUserCompletedTo").val("");
}
function AssignMarkToUserAnswer(UserScore,ID)
{
    var mydata = {
        UserScore: UserScore,
        ID: ID
    };
    $.ajax({
        type: "post",
        dataType: "json",
        url: 'EntranceUserReport/AssignMarksToUser',
        data: AddAntiForgeryToken(mydata),
        success: function (data)
        {
            if (data.data == true)
            {
                ShowAlertMessage(false, "Marks has been assigned");
            }
            else {
                ShowAlertMessage(true, "Operation failed");
            }
            $("#SearchContainer").show();
            $("#GridContainer").show();
            $("#DetailContainer").hide();
            $("#MarkSheetContainer").hide();
            LoadQuizUserGrid();
        }
    });

}
function QuizUserDetail(e) {
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    var mydata = {
        EntranceUserID: dataItem.EntranceUserID,
    };
    $.ajax({
        type: "post",
        dataType: "json",
        url: 'EntranceUserReport/GetUserEntranceAnswerByUserID',
        data: AddAntiForgeryToken(mydata),
        success: function (data) {
            if (data.success == "200") {
                var QuizUserQuestion;
                if (data.data.UserEntranceQuestion == null) {
                    QuizUserQuestion = [];
                }
                else if (data.data.UserEntranceQuestion.indexOf("[") != -1) {
                    QuizUserQuestion = JSON.parse(data.data.UserEntranceQuestion);
                }
                else {
                    QuizUserQuestion = JSON.parse("[" + data.data.UserEntranceQuestion + "]");
                }
                $("#DescQuizTitle").html('').html(trimQuizTitle(dataItem.EntranceTitle));
                $("#DescUserFullName").html('').html(dataItem.UserName);
                $("#DescJoinedDate").html('').html(FormatCompletedDate(dataItem.JoinedDate));
                $("#DescCompletionDate").html('').html(FormatCompletedDate(dataItem.CompletedDate));
                $("#DescCompletedTime").html('').html(fancyTimeFormat(dataItem.CompletedTime));
                $("#DescQuizStatus").html('').html(dataItem.EntranceStatus);
                $("#DescTotalQuestion").html('').html(data.data.TotalQuestion);
                $("#DescCorrectAnswer").html('').html(data.data.CorrectAnswer);
                $("#DescIncorrectAnswer").html('').html(data.data.IncorrectAnswer);
                $("#MarkSheetUserName").html('').html(dataItem.UserName);
                $("#MarkSheetJoinedDate").html('').html(FormatCompletedDate(dataItem.JoinedDate));
                $("#MarkSheetCompletedDate").html('').html(FormatCompletedDate(dataItem.CompletedDate));
                $("#MarkSheetCompletionTime").html('').html(fancyTimeFormat(dataItem.CompletedTime));
                $("#MarkSheetQuizCurrentScore").html('').html(data.data.TotalUserScore + "/" + data.data.TotalEntranceScore)
                $("#MarkSheetQuizTitle").html('').html(trimQuizTitle(dataItem.EntranceTitle));
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
                    html += trimQuestion(item.EntranceQuestion);
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
                    html += "<button class='btn btn-default btn-xs btnCustom btnAssignSubjectiveMark mlr-xsm' mydata='" + item.UserEntranceQuestionAnswerID + "'>Save</button><button class='btn btn-default btnCancle btn-xs btnDefault btnUserMarkScoreClear'>Cancel</button></div>";
                    html += "<a href='#' id='btnPointQuestion'><i class='fa fa-pencil' aria-hidden='true'></i></a></p>";
                    html += "</td>";
                    html += "</tr>";
                    $("#tblUserQuizQuestion").html('').append(html);
                });
                $("#SearchContainer").hide();
                $("#GridContainer").hide();
                $("#DetailContainer").show();
                $("#MarkSheetContainer").show();
                UIEvent();
            }
        }
    })
}
function UIEvent() {
    $("#btnSearch").off().on('click', function (e) {
        e.preventDefault();
        LoadQuizUserGrid();
    });

    $("#btnReset").off().on('click', function (e) {
        e.preventDefault();
        ResetSearcForm();
        LoadQuizUserGrid();
    });
    $("#btnBackToGrid").off().on('click', function (e) {
        e.preventDefault();
        $("#SearchContainer").show();
        $("#GridContainer").show();
        $("#DetailContainer").hide();
        $("#MarkSheetContainer").hide();
    });
    $("div#MarkSheetContainer a#btnPointQuestion").off().on('click', function (e) {
        e.preventDefault();
        $(this).prev().show();
        $(this).hide();
    });
    $('div#MarkSheetContainer input.txtUserPointScore').off().on('keypress keyup blur paste change', function (event) {
        var maxNumber = $(this).attr('max');
        $(this).val($(this).val().replace(/[^\d].+/, ""));
        if (parseInt($(this).val()) > parseInt(maxNumber)) {
            $(this).val('');
        }
    });
    $("#tblUserQuizQuestion button.btnUserMarkScoreClear").off().on('click', function (e) {
        e.preventDefault();
        $(this).closest('td').find('div#divUserMark').hide();
        $(this).closest('td').find('input.txtUserPointScore').val('0');
        $(this).closest('td').find('a#btnPointQuestion').show();
    });
    $("#tblUserQuizQuestion button.btnAssignSubjectiveMark").off().on('click', function (e) {
        e.preventDefault();
        AssignMarkToUserAnswer($(this).closest('td').find('input.txtUserPointScore').val(),$(this).attr('mydata'));
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
}
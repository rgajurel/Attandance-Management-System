$(document).ready(function () {
    ReadGrid();
    InitailizeSumoSelect();
    ReadEntranceListingGrid();
    InitializeForm();
    UIEvent();
    InitializeStartDateDependency();
    InitializeEndDateDependency();
    RemoveHiddenClass();
})
var counter = 0;
var rowNumber = 0;
var checkedIds = [];
var getRecentlyAdded = false;
var IDwithData = [];
var EntranceIDwithData = [];
var selectedIDs = null;
var wnd;
function InitializeForm() {
    var isQuestionManual = $('input[name=IsQuestionManual]').prop('checked');
    if (isQuestionManual) {
        $('#btnChooseQuestionManually').show();
        $('#btnChooseQuestionDynamically').hide();
    }
    else {
        $('#btnChooseQuestionManually').hide();
        $('#btnChooseQuestionDynamically').show();
    }
}
function QuizcheckAll(e) {
    var checked = $('.EntrancechkSelectAll').prop('checked'), grid = $("#EntranceListingGrid").data("kendoGrid");
    for (var i = 0; i < grid.dataSource.data().length; i++) {
        var item = grid.dataSource.data()[i];
        var row = grid.element.find("tr[data-uid='" + item.uid + "']");
        var checkBox = row.find(".EntrancemultiSelect");
        if (!checkBox.prop('checked')) {
            checkBox.trigger("click");

        }
        if (!checked) {
            if (checkBox.prop('checked')) {
                checkBox.trigger("click");
            }
        }
    }
}
function FormatMandatory(data) {
    if (data) {
        return "Yes";
    }
    else {
        return "No";
    }
}
function InitailizeSumoSelect() {
    $('#NotificationID').SumoSelect({
        placeholder: 'Select Notification Type',
        csvDispCount: 3,
        search: true,
    });
}
function ReadGrid() {
    var grid = $("#EntranceQuestionGrid").data("kendoGrid");
    grid.dataSource.read({
        SearchEntranceQuestion: $('#txtSearchQuestion').val(),
        SearchQuestionTypeID: $('#ddlSearchQuestionType :selected').val(),
        SearchCategoryID: $('#ddlSearchQuestionCategory :selected').val(),
        SearchDifficultyLevelID: $('#ddlSearchQuestionDifficulty :selected').val(),
        SearchWeightageID: $('#ddlSearchWeightage :selected').val(),
        SearchStatus: $('#ddlSearchStatus :selected').val(),
        SearchQuestionType: $("#ddlSearchQuestionType :selected").val(),
        pageSize: grid._pageSize,
        pageNumber: grid._page
    });
    grid.refresh();

}
function trimEntranceTitle(data) {
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
function RemoveHiddenClass() {
    $("#modalWindow").removeClass('popup hide');
    $("#modalTagAdd").removeClass('popup hide');
}

// Date picker Dependency in search of Grid
function InitializeStartDateDependency() {
    function startChangeForStartedDate() {
        console.log('hti');
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
    var start1 = $("#txtSearchEntranceStartedFrom").kendoDatePicker({
        change: startChangeForStartedDate,
        format: "MM/dd/yyyy",

    }).data("kendoDatePicker");

    var end1 = $("#txtSearchEntranceStartedTo").kendoDatePicker({
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

    var start2 = $("#txtSearchEntranceEndFrom").kendoDatePicker({
        change: startChangeForEndedDate,
        format: "MM/dd/yyyy",

    }).data("kendoDatePicker");

    var end2 = $("#txtSearchEntranceEndTo").kendoDatePicker({
        change: endChangeForEndedDate,
        format: "MM/dd/yyyy",

    }).data("kendoDatePicker");

    start2.max(end2.value());
    end2.min(start2.value());
}

function ReadEntranceListingGrid() {
    var grid = $("#EntranceListingGrid").data("kendoGrid");
    grid.dataSource.page(1);
}
AddAntiForgeryToken = function (data) {
    data.__RequestVerificationToken = $('form input[name=__RequestVerificationToken]').val();
    return data;
};
function ResetValidation() {
    $('.input-validation-error').addClass('input-validation-valid');
    $('.input-validation-error').removeClass('input-validation-error');
    $('.field-validation-error').addClass('field-validation-valid');
    $('.field-validation-error').removeClass('field-validation-error');
    $('.validation-summary-errors').addClass('validation-summary-valid');
    $('.validation-summary-errors').removeClass('validation-summary-errors');
}
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
function onDatabound(e) {
    rowNumber = 0;
    var view = this.dataSource.view();
    var count = 0;
    for (var i = 0; i < view.length; i++) {
        if (IDwithData.length > 0) {
            $.each(IDwithData, function (index, item) {
                if (item.ActualQuestionID == view[i].QuestionID && item.Status == true) {
                    $('#EntranceQuestionGrid').find("tr[data-uid='" + view[i].uid + "']")
                        .addClass("k-alt")
                        .find(".multiSelect")
                        .attr("checked", "checked");
                    count++;
                }
            })
        }
    }
    //var pageSizes = LoadGridRecordPerPage(RecordPerPage, RecordPerPage, 5);
    var pageSizes = [];
    var pageSizearr = [];
    if (pageSizes.length > 0) {
        $.each(pageSizes, function (val, size) {
            pageSizearr.push({ text: size, value: size });
        });
    } else {
        pageSizearr = [10, 20, 30, 50, 80];
    }
    $('.k-pager-sizes select[data-role="dropdownlist"]').data('kendoDropDownList').setDataSource(new kendo.data.DataSource({ data: pageSizearr }));
    if (count == i && count != 0) {
        $('#EntranceQuestionGrid .chkSelectAll').prop('checked', true);
    }
    else {
        $('#EntranceQuestionGrid .chkSelectAll').prop('checked', false);
    }

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

function onDataboundEntranceListing(e) {
    $(".k-grid-Edit").attr('title', 'Edit');
    $(".k-grid-Delete").attr('title', 'Delete');

    $(".k-grid-Edit").find("span").addClass("fa fa-pencil");
    $(".k-grid-Edit").removeClass("k-button");

    $(".k-grid-Delete").find("span").addClass("fa fa-trash");
    $(".k-grid-Delete").removeClass("k-button");

    var grid = e.sender;
    // var pageSizes = LoadGridRecordPerPage(RecordPerPage, RecordPerPage, 5);
    var pageSizes = [];
    var pageSizearr = [];
    if (pageSizes.length > 0) {
        $.each(pageSizes, function (val, size) {
            pageSizearr.push({ text: size, value: size });
        });
    } else {
        pageSizearr = [10, 20, 30, 50, 80];
    }
    $('div#EntranceListingGrid .k-pager-sizes select[data-role="dropdownlist"]').data('kendoDropDownList').setDataSource(new kendo.data.DataSource({ data: pageSizearr }));
    if (grid.dataSource.total() == 0) {
        var colCount = grid.columns.length;
        $(e.sender.wrapper)
            .find('tbody')
            .append('<tr class="kendo-data-row text-center"><td colspan="' + colCount + '" class="no-data">No Data Available</td></tr>');
    }

    rowNumber = 0;
    var view = this.dataSource.view();
    var count = 0;
    console.log(EntranceIDwithData);
    for (var i = 0; i < view.length; i++) {
        if (EntranceIDwithData.length > 0) {
            $.each(EntranceIDwithData, function (index, item) {
                console.log(item);
                if (item.EntranceID == view[i].EntranceID && item.Status == true) {
                    $('#EntranceListingGrid').find("tr[data-uid='" + view[i].uid + "']")
                        .addClass("k-alt")
                        .find(".EntrancemultiSelect")
                        .attr("checked", "checked");
                    count++;
                }
            })
        }
    }
    if (count == i && count != 0) {
        $('#EntranceListingGrid .EntrancechkSelectAll').prop('checked', true);
    }
    else {
        $('#EntranceListingGrid .EntrancechkSelectAll').prop('checked', false);
    }

    $("#EntranceListingGrid .EntrancemultiSelect").off().on("click", function () {
        var checked = this.checked,
            row = $(this).closest("tr"),
            grid = $("#EntranceListingGrid").data("kendoGrid"),
            dataItem = grid.dataItem(row);
        var IsElementExist;
        if (EntranceIDwithData.length == 0) {
            EntranceIDwithData.push({
                EntranceID: dataItem.EntranceID,
                Status: true,
            });
        }
        if (checked) {
            $(EntranceIDwithData).each(function (i, data) {
                if (data.EntranceID == dataItem.EntranceID) {
                    IsElementExist = 1;
                    data.Status = true;
                    return false;
                }
                else {
                    IsElementExist = 0;
                }
                row.addClass("k-alt");
            });
            if (IsElementExist == 0) {
                EntranceIDwithData.push({
                    EntranceID: dataItem.EntranceID,
                    Status: true,
                });
            }
        }
        else {
            $(EntranceIDwithData).each(function (i, data) {

                if (data.EntranceID == dataItem.EntranceID) {
                    data.Status = false;
                }
            });
            row.removeClass("k-alt");
        }
        $(EntranceIDwithData).each(function (i, data) {
            if (data.Status == true) {
                $("div.changeStatusOption").removeClass('hide').addClass('show');
                return false;
            }
            else {
                $("div.changeStatusOption").removeClass('show').addClass('hide');
            }
        });
    });

}
function startChange() {
    var startDate = start.value(),
        endDate = end.value();

    if (startDate) {
        startDate = new Date(startDate);
        startDate.setDate(startDate.getDate());
        end.min(startDate);
    } else if (endDate) {
        start.max(new Date(endDate));
    } else {
        endDate = new Date();
        start.max(endDate);
        end.min(endDate);
    }
}

function endChange() {
    var endDate = end.value(),
        startDate = start.value();

    if (endDate) {
        endDate = new Date(endDate);
        endDate.setDate(endDate.getDate());
        start.max(endDate);
    } else if (startDate) {
        end.min(new Date(startDate));
    } else {
        endDate = new Date();
        start.max(endDate);
        end.min(endDate);
    }
}

function renderNumber(data) {
    return ++rowNumber;
}

function renderRecordNumber(data) {
    var page = parseInt($("#EntranceQuestionGrid").data("kendoGrid").dataSource.page()) - 1;
    var pagesize = $("#EntranceQuestionGrid").data("kendoGrid").dataSource.pageSize();
    return parseInt(rowNumber + (parseInt(page) * parseInt(pagesize)));
}
function renderNumberQuizListing(data) {
    return ++rowNumber;
}

function renderRecordNumberEntranceListing(data) {
    var page = parseInt($("#EntranceListingGrid").data("kendoGrid").dataSource.page()) - 1;
    var pagesize = $("#EntranceListingGrid").data("kendoGrid").dataSource.pageSize();
    return parseInt(rowNumber + (parseInt(page) * parseInt(pagesize)));
}
function ParamToLoadEntranceQuestionList() {
    var grid = $("#EntranceQuestionGrid").data("kendoGrid").dataSource;
    return {
        SearchEntranceQuestion: $('#txtSearchKeyword').val(),
        SearchQuestionTypeID: $('#ddlSearchQuestionType :selected').val(),
        SearchCategoryID: $('#ddlSearchQuestionCategory :selected').val(),
        SearchDifficultyLevelID: $('#ddlSearchDifficultyLevel :selected').val(),
        SearchWeightageID: $('#ddlSearchWeightage :selected').val(),
        SearchStatus: $('#ddlSearchStatus :selected').val(),
        SearchQuestionType: $("#ddlSearchMandatory :selected").val(),
        pageSize: grid._pageSize,
        pageNumber: grid._page
    };
}
function ParamToLoadEntranceListing() {

    var objInfo1 =
        {

            SearchStartedFrom: $('#txtSearchEntranceStartedFrom').val(),
            SearchStartedTo: $('#txtSearchEntranceStartedTo').val(),
            SearchEndFrom: $('#txtSearchEntranceEndFrom').val(),
            SearchEndTo: $('#txtSearchEntranceEndTo').val(),
            SearchStatusID: $('#ddlSearchEntranceStatus :selected').val() == "" ? -1 : $('#ddlSearchEntranceStatus :selected').val(),
            SearchEntranceTitle: $('#txtSearchEntranceTitle').val(),
        };
    return {
        objInfo: JSON.stringify(objInfo1)
    };
}
function checkAll(ele) {
    var checked = $('.chkSelectAll').prop('checked'), grid = $("#EntranceQuestionGrid").data("kendoGrid");
    for (var i = 0; i < grid.dataSource.data().length; i++) {
        var item = grid.dataSource.data()[i];
        var row = grid.element.find("tr[data-uid='" + item.uid + "']");
        var checkBox = row.find(".multiSelect");
        //check unchecked checkbox in grid
        if (!checkBox.prop('checked')) {
            checkBox.trigger("click");

        }
        if (!checked) {
            if (checkBox.prop('checked')) {
                checkBox.trigger("click");
            }
        }
    }
}
function EditEntrance(e) {
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    var mydata = {
        EntranceID: dataItem.EntranceID,
    };
    $.ajax({
        type: "post",
        dataType: "json",
        url: 'Entrance/GetEntranceByID',
        data: AddAntiForgeryToken(mydata),
        success: function (data) {
            data = data.data;
            var Categorylist;
            try { Categorylist = data.NotificationID.split(','); } catch (e) { Categorylist = ""; }
            $('#NotificationID')[0].sumo.unSelectAll();
            var cDDL = $('#NotificationID')[0];
            $.each(Categorylist, function (i, v) {
                cDDL.sumo.selectItem(Categorylist[i]);
            });
            $('#divEntranceForm').show();
            $('#divEntranceGrid').hide();
            $('#EntranceID').val(data.EntranceID);
            $('#CourseID').val(data.ChapterID);
            $('#EntranceTitle').val(data.EntranceTitle);
            var StartDate = kendo.toString(kendo.parseDate(data.StartDate));
            var EndDate = kendo.toString(kendo.parseDate(data.EndDate));
            if (StartDate > new Date()) {
                $("#StartDate").data("kendoDateTimePicker").min(new Date());
            }
            else {
                $("#StartDate").data("kendoDateTimePicker").min(data.StartDate);
            }
            //  $("#StartDate").data("kendoDateTimePicker").min(data.StartDate);
            $("#EndDate").data("kendoDateTimePicker").min(data.StartDate);
            $("#StartDate").data("kendoDateTimePicker").value(null);
            $("#StartDate").data("kendoDateTimePicker").value(StartDate);
            $("#EndDate").data("kendoDateTimePicker").value(null);
            $("#EndDate").data("kendoDateTimePicker").value(EndDate);
            $('#TotalQuestion').val(data.TotalQuestion);
            $('#EntranceAppearingPoints').val(data.EntranceAppearingPoints);
            $('#SortOrder').val(data.SortOrder);
            $('#StatusValue').val(data.StatusValue);
           // $('#CategoryID').val(data.CategoryID);;
            $("#IsPauseAllowed[value='" + data.IsPauseAllowed + "']").prop("checked", true);
            $("#CanShowCorrectAnswer[value='" + data.CanShowCorrectAnswer + "']").prop("checked", true);
            $("#CanShowAllQuestions[value='" + data.CanShowAllQuestions + "']").prop("checked", true);
            $("#CanSeePreviousAnswer[value='" + data.CanSeePreviousAnswer + "']").prop("checked", true);
            $("#IsQuestionManual[value='" + data.IsQuestionManual + "']").prop("checked", true);
            var tagArr = [];
            if (data.Tag != null) {
                tagArr = data.Tag.split(',');
            }
            //$("#Tag").data("kendoMultiSelect").value(tagArr);
            $("input#MetaTitle").val(data.MetaTitle);
            $("textarea#MetaDescription").val(data.MetaDescription);
            $("input#MetaKeyword").val(data.MetaKeyword);
            //CKEDITOR.instances.container_StartPageDescription.setData(data.StartPageDescription);
            //CKEDITOR.instances.container_QuizDescription.setData(data.EntranceDescription);
            //CKEDITOR.instances.container_EndPageDescription.setData(data.EndPageDescription);
            $("#StartPageDescription").val(data.StartPageDescription);
            $("#EntranceDescription").val(data.EntranceDescription);
            $("#EndPageDescription").val(data.EndPageDescription);

          //  $("#btnBrowse_QuizImage").attr("filepath", data.QuizImage);
           // $("#ImgMediaManagementImage_QuizImage").prop('src', data.QuizImage);
            //$("#ImgMediaManagementImage_QuizImage").show();
           // $("select#Priority").val(data.Priority);
            var ArrayLst;
            if (data.JSONDATA.indexOf("[") != -1) {
                ArrayLst = JSON.parse(data.JSONDATA);
            }
            else {
                ArrayLst = JSON.parse("[" + data.JSONDATA + "]");
            }
            if (data.IsQuestionManual) {
                SetUpQuestionGridAndAnswerGrid(ArrayLst);
                IDwithData = ArrayLst;
                ReadGrid();
            }
            //else {
            //    console.log(ArrayLst);
            //    setUpDynamicallyAddedQuestion(ArrayLst);
            //}

            if (data.IsUpdatable) {
            }
            else {
                $("#StartDate").data("kendoDateTimePicker").enable(false);
              //  $("#CategoryID").prop('disabled', true);
                $("input[name=IsQuestionManual]").prop('disabled', true);
                //$("#btnChooseQuestionManually").prop('disabled', true);
                //$("#btnChooseQuestionDynamically").prop('disabled', true);
                // ShowAlertMessage(true, "Cannot be updated. Quiz is in use");
            }
        }, beforeSend: function () {
            //loadingNow($('div#divEntranceForm'), true);
        },
        complete: function () {
            //loadingNow($('div#divEntranceForm'), false);
        },
        error: function () {
            //loadingNow($('div#divEntranceForm'), false);
        }
    });
}

function DeleteEntrance(e) {

    wnd.center().open();

    $("#yes").off().on('click', function (e) {
        e.preventDefault();
        $.ajax({
            type: "post",
            dataType: "json",
            url: 'Entrance/DeleteEntranceByID',
            data: AddAntiForgeryToken(mydata),
            success: function (data) {
                if (data.ReturnCode == 200) {
                    ShowAlertMessage(false, data.Message);
                }
                else {
                    ShowAlertMessage(true, data.Message);
                }
                ReadEntranceListingGrid();
            }, beforeSend: function () {
                //loadingNow($('div#divEntranceGrid'), true);
            },
            complete: function () {
                //loadingNow($('div#divEntranceGrid'), false);
            },
            error: function () {
                //loadingNow($('div#divEntranceGrid'), false);
            }
        })
        wnd.close();
    });

    $("#no").off().on('click', function (e) {
        e.preventDefault();
        wnd.close();
    });

    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    var mydata = {
        EntranceID: dataItem.EntranceID,
    };
}

wnd = $("#modalWindow").kendoWindow({
    title: "Delete confirmation",
    modal: true,
    visible: false,
    resizable: false,
    width: 400
}).data("kendoWindow");
function ClearAll() {
    $('#divEntranceForm').hide();
    $('#divEntranceGrid').show();
    $('#EntranceID').val('-1');
    $('#CourseID').val('');
    $('#EntranceTitle').val('');
    $('#StartDate').val('');
    $('#EndDate').val('');
    $('#TotalQuestion').val('');
    $('#EntranceAppearingPoints').val('');
    //$('#SortOrder').val('');
   // $("select#CategoryID")[0].selectedIndex = 0;
    $("select#StatusValue")[0].selectedIndex = 0;
    //CKEDITOR.instances["container_QuizDescription"].setData("");
    //CKEDITOR.instances["container_StartPageDescription"].setData("");
    //CKEDITOR.instances["container_EndPageDescription"].setData("");

    $("#StartPageDescription").val();
    $("#EntranceDescription").val();
    $("#EndPageDescription").val();

  //  $("#btnBrowse_QuizImage").attr("filepath", "");
   // $("#ImgMediaManagementImage_QuizImage").hide();
    $('input#IsPauseAllowed').prop('checked', false);
    $('input#CanShowCorrectAnswer').prop('checked', false);
    $('input#CanShowAllQuestions').prop('checked', false);
    $('input#CanSeePreviousAnswer').prop('checked', false);
   // $('#Tag').data('kendoMultiSelect').value("");
    $("input#MetaTitle").val("");
    $("textarea#MetaDescription").val("");
    $("input#MetaKeyword").val("");
    $('#NotificationID')[0].sumo.unSelectAll();
  //  $("select#Priority")[0].selectedIndex = 0;
    try {
        $('#divActualAnswerGrid').data('kendoGrid').dataSource.data([]);
        $('#SelectedQuestionGrid').data('kendoGrid').dataSource.data([]);

    } catch (e) {

    }
    $("#EntranceQuestionGrid").data("kendoGrid").dataSource.page(1);
    IDwithData = [];
    selectedIDs = null;
    $('#NotificationID').val('');
    //var divContainerLst = $('#divChooseDynamicQuizQuestion .dynamicQuestionContainer>div');
    //$.each(divContainerLst, function (index, item) {
    //    if (index == 0) {
    //        $(item).find('input').val('-1').not(':first').remove();
    //    }
    //    else if (index == 1 || index == 2) {
    //        $(item).find('select').val('-1').not(':first').remove();
    //        $(item).find('div').children('span').not(':first').remove();
    //        $(item).find('select').val('')
    //    }
    //    else if (index == 3 || index == 4) {
    //        $(item).find('input').val('').not(':first').remove();
    //        $(item).find('div').children('span').not(':first').remove();
    //    }
    //    else if (index == 5) {
    //        $(item).find('i').remove();
    //    }
    //});
    ResetValidation();
    $("#StartDate").data("kendoDateTimePicker").value('');
    $("#EndDate").data("kendoDateTimePicker").value('');
    $("#StartDate").data("kendoDateTimePicker").min(new Date());
    $("#EndDate").data("kendoDateTimePicker").min(new Date());
    $("#StartDate").data("kendoDateTimePicker").enable(true);
    $("#EndDate").data("kendoDateTimePicker").enable(true);
    $("#TotalQuestion").prop('disabled', false);
    $("#EntranceAppearingPoints").prop('disabled', false);
   // $("#SortOrder").prop('disabled', false);
    $("#StatusValue").prop('disabled', false);
   // $("#CategoryID").prop('disabled', false);
    $("input[name=CanShowCorrectAnswer]").prop('disabled', false);
    $("input[name=IsPauseAllowed]").prop('disabled', false);
    $("input[name=CanShowAllQuestions]").prop('disabled', false);
    $("input[name=IsQuestionManual]").prop('disabled', false);
    $("input[name=CanSeePreviousAnswer]").prop('disabled', false);
    $('select#NotificationID')[0].sumo.enable();
    $("#btnChooseQuestionManually").prop('disabled', false);
    $("#btnChooseQuestionDynamically").prop('disabled', false);

}

function onDataBoundSelectedQuestionGrid(e) {
    var grid = e.sender;
    if (grid.dataSource.total() == 0) {
        $('#divActualAnswerGrid').hide();
    }
    else {
        $('#divActualAnswerGrid').show();
    }
    //var pageSizes = LoadGridRecordPerPage(RecordPerPage, RecordPerPage, 5);
    var pageSizes = [];
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


function onDataBoundActualAnswerGrid(e) {
    var grid = e.sender;
    if (grid.dataSource.total() == 0) {
        $('#SelectedQuestionGrid').hide();
    }
    else {
        $('#SelectedQuestionGrid').show();
    }
    //var pageSizes = LoadGridRecordPerPage(RecordPerPage, RecordPerPage, 5);
    var pageSizes = [];
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
function BatchUpdateEntranceStatus(JsonObject) {
    var mydata = {
        JsonObject: JsonObject
    }
    $.ajax({
        type: "post",
        dataType: "json",
        url: 'Entrance/BatchUpdateStatusForEntrance',
        data: AddAntiForgeryToken(mydata),
        success: function (data) {
            if (data) {
                ShowAlertMessage(false, "Status Updated");
            }
            else {
                ShowAlertMessage(true, "Operation Failed");
            }

            EntranceIDwithData = [];
            ReadEntranceListingGrid();
            $("div.changeStatusOption").removeClass('show').addClass('hide');
        }, beforeSend: function () {
            //loadingNow($('div#divEntranceGrid'), true);
        },
        complete: function () {
            //loadingNow($('div#divEntranceGrid'), false);
        },
        error: function () {
            //loadingNow($('div#divEntranceGrid'), false);
        }
    })
}
function GetStatusForBatchUpdate(JsonObject) {
    var mydata = {
        JsonObject: JsonObject
    }
    $.ajax({
        type: "post",
        dataType: "json",
        url: 'Entrance/GetStatusForBatchEntranceUpdate',
        data: AddAntiForgeryToken(mydata),
        success: function (data) {
            var ele = $("div#EntranceBatchUpdateModal").find('ol.UpdateStatusListingBody');
            var html = '';
            var objJson;
            if (data == null) {
                objJson = [];
            }
            else if (data.indexOf("[") != -1) {
                objJson = JSON.parse(data);
            }
            else {
                objJson = JSON.parse("[" + data + "]");
            }
            $(ele).empty();
            $.each(objJson, function (index, item) {
                html += "<li><span>";
                html += item.QuizStatus;
                html += "</span></li>";
                $(ele).append(html);
                html = "";
            });
            $("#QuizBatchUpdateModal").modal('show');
        }, beforeSend: function () {
            //loadingNow($('div#divEntranceGrid'), true);
        },
        complete: function () {
            //loadingNow($('div#divEntranceGrid'), false);
        },
        error: function () {
            //loadingNow($('div#divEntranceGrid'), false);
        }
    })
}
function UIEvent() {

    $('#btnChooseQuestionManually').off().on('click', function (e) {
        e.preventDefault();
        $('#divChooseEntranceQuestion').show();
        $('#divEntranceForm').hide();
    });

    //$('#btnChooseQuestionDynamically').off().on('click', function (e) {
    //    e.preventDefault();
    //    $('#divChooseDynamicQuizQuestion').show();
    //    $('#divEntranceForm').hide();
    //});

    $('#btnBackToMainForm').off().on('click', function (e) {
        e.preventDefault();
        $('#divChooseEntranceQuestion').hide();
        $('#divEntranceForm').show();
    })
    $("#EntranceQuestionGrid").off().on("click", ".multiSelect", function () {
        var checked = this.checked,
            row = $(this).closest("tr"),
            grid = $("#EntranceQuestionGrid").data("kendoGrid"),
            dataItem = grid.dataItem(row);
        var IsElementExist;
        if (IDwithData.length == 0) {
            IDwithData.push({
                EntranceQuestion: dataItem.EntranceQuestion,
                DifficultyLevel: dataItem.DifficultyLevel,
                QuestionWeight: dataItem.QuestionWeight,
                ActualQuestionID: dataItem.QuestionID,
                Status: true,
            });
        }
        if (checked) {
            //-select the row
            $(IDwithData).each(function (i, data) {
                if (data.ActualQuestionID == dataItem.QuestionID) {
                    IsElementExist = 1;
                    data.Status = true;
                    return false;
                }
                else {
                    IsElementExist = 0;
                }
                row.addClass("k-alt");
            });
            if (IsElementExist == 0) {
                IDwithData.push({
                    EntranceQuestion: dataItem.EntranceQuestion,
                    DifficultyLevel: dataItem.DifficultyLevel,
                    QuestionWeight: dataItem.QuestionWeight,
                    ActualQuestionID: dataItem.QuestionID,
                    Status: true,
                });
            }
        }
        else {
            //-remove selection
            $(IDwithData).each(function (i, data) {

                if (data.ActualQuestionID == dataItem.QuestionID) {
                    //IDwithData.splice(i, 1);
                    data.Status = false;
                }
                else {
                }
            });
            row.removeClass("k-alt");
        }
    });

    $('#btnSearchEntranceQuestion').off().on('click', function (e) {
        e.preventDefault();
        $("#EntranceQuestionGrid").data("kendoGrid").dataSource.page(1);
        ReadGrid();
    })
    $('#btnSearchEntrance').off().on('click', function (e) {
        e.preventDefault();
        ReadEntranceListingGrid();
    })
    $('#btnClearSearch').off().on('click', function (e) {
        e.preventDefault();
        $('#txtSearchQuestion').val('');
        $('#ddlSearchQuestionCategory').val('');
        $('#ddlSearchQuestionDifficulty').val('');
        $("#EntranceQuestionGrid").data("kendoGrid").dataSource.page(1);
        ReadGrid();
    })
    $('#btnClearEntranceSearch').off().on('click', function (e) {
        e.preventDefault();
        $('#txtSearchEntranceTitle').val('');
        $('#ddlSearchEntranceStatus').val('');
        $('#txtSearchEntranceStartedFrom').val('');
        $('#txtSearchEntranceStartedTo').val('');
        $('#txtSearchEntranceEndFrom').val('');
        $('#txtSearchEntranceEndTo').val('');
        ReadEntranceListingGrid();
    })
    $('#btnConfirmSelectedAnswer').off().on('click', function (e) {
        e.preventDefault();
        $('#btnViewAllSelectedQuestion').trigger('click');
        $('#divEntranceForm').show();
        $('#divChooseEntranceQuestion').hide();

        var grid = $('#divActualAnswerGrid').data('kendoGrid').dataSource._data.length;
        if (grid == 0) {
            $('#divActualAnswerGrid').hide();
        }
        else {
            $('#divActualAnswerGrid').show();
        }
    })
    $('#btnClearAllSelectedAnswer').off().on('click', function (e) {
        IDwithData = [];
        $("#divActualAnswerGrid").data('kendoGrid').dataSource.data([]);
        $("#SelectedQuestionGrid").data('kendoGrid').dataSource.data([]);
        $("#EntranceQuestionGrid").data("kendoGrid").dataSource.page(1);
    })
    $('#btnCancel').off().on('click', function (e) {
        e.preventDefault();
        ClearAll();
    })
    $('#btnSave').off().on('click', function (e) {
        e.preventDefault();
        if ($('form.entrance-form').data('unobtrusiveValidation').validate()) {
            //var isValid = true;
            //var errorArray = {};
            //if (CKEDITOR.instances["container_StartPageDescription"].getData().length < 1) {
            //    errorArray["StartPageDescription"] = "Start Page Description Required";
            //    isValid = false;
            //}
            //else {
            //    errorArray["StartPageDescription"] = "";
            //}
            //if (CKEDITOR.instances["container_EndPageDescription"].getData().length < 1) {
            //    errorArray["EndPageDescription"] = "End Page Description Required";
            //    isValid = false;
            //}
            //else {
            //    errorArray["EndPageDescription"] = "";
            //}
            //if (CKEDITOR.instances["container_QuizDescription"].getData().length < 1) {
            //    errorArray["EntranceDescription"] = "Quiz Description Required";
            //    isValid = false;
            //}
            //else {
            //    errorArray["EntranceDescription"] = "";
            //}
            //if (!isValid) {
            //    $('form').validate().showErrors(errorArray);
            //    $('form').validate().focusInvalid();
            //}
            //else {
            var IsQuestionManual = $('input[name=IsQuestionManual]').prop('checked');
            //var tag;
            var NotificationIDs;
            try { NotificationIDs = $('#NotificationID').val().join(','); } catch (e) { NotificationIDs = ""; }
            // try { tag = $('#Tag').val().join(','); } catch (e) { tag = ""; }
            if (IsQuestionManual) {
                $('#btnViewAllSelectedQuestion').trigger('click');
                if (selectedIDs != null) {
                    var ValueObject = [];
                    $.each(IDwithData, function (index, item) {
                        ValueObject.push({
                            QuestionID: item.ActualQuestionID,
                            Status: item.Status,
                            Data: item
                        });
                    });
                    var mydata = {
                        EntranceID: $('#EntranceID').val() == "" ? -1 : $('#EntranceID').val(),
                        CourseID: $('#CourseID :selected').val(),
                        EntranceTitle: $('#EntranceTitle').val(),
                        EndDate: $('#EndDate').val(),
                        StartDate: $('#StartDate').val(),
                        TotalQuestion: $('#TotalQuestion').val(),
                        EntranceAppearingPoints: $('#EntranceAppearingPoints').val(),
                        SortOrder: $('#SortOrder').val(),
                        StatusValue: $('#StatusValue :selected').val(),
                     //   CategoryID: $('#CategoryID :selected').val(),

                        //$("#StartPageDescription").val(data.StartPageDescription);
                        //    $("#EntranceDescription").val(data.EntranceDescription);
                        //    $("#EndPageDescription").val(data.EndPageDescription);

                        StartPageDescription: $("#StartPageDescription").val(),
                        EndPageDescription: $("#EndPageDescription").val(),
                        CanShowCorrectAnswer: $('#CanShowCorrectAnswer:checked').val(),
                        IsPauseAllowed: $('#IsPauseAllowed').prop('checked'),
                        CanShowAllQuestions: $('#CanShowAllQuestions').prop('checked'),
                        NotificationID: NotificationIDs,
                        CanSeePreviousAnswer: $('#CanSeePreviousAnswer').prop('checked'),
                        SelectedAnswers: selectedIDs,
                        IsQuestionManual: $('#IsQuestionManual').prop('checked'),
                        EntranceDescription: $("#EntranceDescription").val(),
                       // QuizImage: $('#btnBrowse_QuizImage').attr('filepath'),
                        MetaTitle: $('input#MetaTitle').val(),
                        MetaKeyword: $('input#MetaKeyword').val(),
                        MetaDescription: $('textarea#MetaDescription').val(),
                        //NotifyNow: $('#NotifyNow').prop('checked'),
                        //Tag: tag,
                        //Priority: $("select#Priority :selected").val(),
                    };
                    $.ajax({
                        type: "post",
                        dataType: "json",
                        url: 'Entrance/InsertUpdateEntrance',
                        data: AddAntiForgeryToken(mydata),
                        success: function (data) {
                            if (data.operationStatus == 200) {
                                ShowAlertMessage(false, data.MessageStatus);
                            }
                            else {
                                ShowAlertMessage(false, data.MessageStatus);
                            }
                            ClearAll();
                            ReadEntranceListingGrid();
                        }, beforeSend: function () {
                            //loadingNow($('div#divEntranceForm'), true);
                        },
                        complete: function () {
                            //loadingNow($('div#divEntranceForm'), false);
                        },
                        error: function () {
                            //loadingNow($('div#divEntranceForm'), false);
                        }
                    });
                }
                else {
                    ShowAlertMessage(true, "No Question has been selected for entrance");
                }
            }
            //else {
            //    var tempobj = {
            //        ID: [],
            //        QuizQuestionMandatoryNo: [],
            //        QuizQuestionOptionalNo: [],
            //        QuestionCategory: [],
            //        QuestionDifficulty: [],
            //    };
            //    var containerList = $('#divChooseDynamicQuizQuestion .dynamicQuestionContainer>div');
            //    $.each(containerList, function (index, item) {
            //        if (index < 5) {
            //            if (index == 0) {
            //                $.each($(item).find('input'), function (index1, item1) {
            //                    tempobj.ID.push($(item1).val());
            //                });
            //            }
            //            else if (index == 1) {
            //                $.each($(item).find('select'), function (index2, item2) {
            //                    tempobj.QuestionCategory.push($(item2).val());
            //                });
            //            }
            //            else if (index == 2) {
            //                $.each($(item).find('select'), function (index3, item3) {
            //                    tempobj.QuestionDifficulty.push($(item3).val());
            //                });
            //            }
            //            else if (index == 3) {
            //                $.each($(item).find('input'), function (index4, item4) {
            //                    tempobj.QuizQuestionMandatoryNo.push($(item4).val());
            //                });
            //            }
            //            else if (index == 4) {
            //                $.each($(item).find('input'), function (index5, item5) {
            //                    tempobj.QuizQuestionOptionalNo.push($(item5).val());
            //                });
            //            }
            //        }
            //    });
            //    var mydata = {
            //        EntranceID: $('#EntranceID').val() == "" ? -1 : $('#EntranceID').val(),
            //        CourseID: $('#CourseID :selected').val(),
            //        EntranceTitle: $('#EntranceTitle').val(),
            //        EndDate: $('#EndDate').val(),
            //        StartDate: $('#StartDate').val(),
            //        TotalQuestion: $('#TotalQuestion').val(),
            //        EntranceAppearingPoints: $('#EntranceAppearingPoints').val(),
            //        SortOrder: $('#SortOrder').val(),
            //        StatusValue: $('#StatusValue :selected').val(),
            //       // CategoryID: $('#CategoryID :selected').val(),
            //        CanShowCorrectAnswer: $('#CanShowCorrectAnswer:checked').val(),
            //        IsPauseAllowed: $('#IsPauseAllowed').prop('checked'),
            //        CanShowAllQuestions: $('#CanShowAllQuestions').prop('checked'),
            //        NotificationID: $('#NotificationID').val(),
            //        CanSeePreviousAnswer: $('#CanSeePreviousAnswer').prop('checked'),
            //        StartPageDescription: $("#StartPageDescription").val(),
            //        EndPageDescription: $("#EndPageDescription").val(),
            //        QuestionDynamicList: tempobj,
            //        IsQuestionManual: $('#IsQuestionManual').prop('checked'),
            //        EntranceDescription: $("#EntranceDescription").val(),
            //       // QuizImage: $('#btnBrowse_QuizImage').attr('filepath'),
            //        MetaTitle: $('input#MetaTitle').val(),
            //        MetaKeyword: $('input#MetaKeyword').val(),
            //        MetaDescription: $('textarea#MetaDescription').val(),
            //        //NotifyNow: $('#NotifyNow').prop('checked'),
            //        //Tag: tag,
            //       // Priority: $("select#Priority :selected").val(),
            //    };
            //    $.ajax({
            //        type: "post",
            //        dataType: "json",
            //        url: 'Quiz/InsertUpdateQuiz',
            //        data: AddAntiForgeryToken(mydata),
            //        success: function (data) {
            //            if (data.operationStatus == 200) {
            //                ShowAlertMessage(false, data.MessageStatus);
            //            }
            //            else {
            //                ShowAlertMessage(true, data.MessageStatus);
            //            }
            //            ClearAll();
            //            ReadEntranceListingGrid();
            //        }, beforeSend: function () {
            //            //loadingNow($('div#divEntranceForm'), true);
            //        },
            //        complete: function () {
            //            //loadingNow($('div#divEntranceForm'), false);
            //        },
            //        error: function () {
            //            //loadingNow($('div#divEntranceForm'), false);
            //        }
            //    });
            //}
            //}
        }
        //else {
        //    var errorArray = {};
        //    if (CKEDITOR.instances["container_StartPageDescription"].getData().length < 1) {
        //        errorArray["StartPageDescription"] = "Start Page Description Required";
        //    }
        //    else {
        //        errorArray["StartPageDescription"] = "";
        //    }
        //    if (CKEDITOR.instances["container_EndPageDescription"].getData().length < 1) {
        //        errorArray["EndPageDescription"] = "End Page Description Required";
        //    }
        //    else {
        //        errorArray["EndPageDescription"] = "";
        //    }
        //    if (CKEDITOR.instances["container_QuizDescription"].getData().length < 1) {
        //        errorArray["EntranceDescription"] = "Quiz Description Required";
        //    }
        //    else {
        //        errorArray["EntranceDescription"] = "";
        //    }
        //    $('form').validate().showErrors(errorArray);
        //    $('form').validate().focusInvalid();
        //}
    });
    $('#btnCreateEntrance').off().on('click', function (e) {
        e.preventDefault();
        $('#divEntranceGrid').hide();
        $('#divEntranceForm').show();
    })
    $('#btnViewAllSelectedQuestion').off().on('click', function (e) {
        e.preventDefault();
        selectedIDs = null;
        var sourcegrid = $('#EntranceQuestionGrid').data('kendoGrid');
        var NewDataToMove = [];
        console.log(IDwithData);
        var QuestionCount = 0;
        $(IDwithData).each(function (i, data) {
            if (data.Status == true) {
                NewDataToMove.push(data);
                if (selectedIDs == null) {
                    selectedIDs = data.ActualQuestionID;
                }
                else {
                    selectedIDs = selectedIDs + ',' + data.ActualQuestionID;
                }
                QuestionCount++;
            }
        });
        console.log(NewDataToMove);
        SetUpQuestionGridAndAnswerGrid(NewDataToMove);
        $('#TotalQuestion').val(QuestionCount);
    });

    $('input[name=IsQuestionManual]').off().on('change', function (e) {
        e.preventDefault();
        var IsQuestionManual = $('input[name=IsQuestionManual]').prop('checked');
        if (IsQuestionManual) {
            $('#btnChooseQuestionManually').show();
            $('#btnChooseQuestionDynamically').hide();
            try {
                $('#divActualAnswerGrid').data('kendoGrid').dataSource.data([]);
                $('#SelectedQuestionGrid').data('kendoGrid').dataSource.data([]);
            } catch (e) {

            }
            $('#btnCancelDynamicQuestionDetail').trigger('click');
        }
        else {
            $('#btnChooseQuestionManually').hide();
            $('#btnChooseQuestionDynamically').show();
            try {
                $("#divActualAnswerGrid").data('kendoGrid').dataSource.data([]);
                $("#SelectedQuestionGrid").data('kendoGrid').dataSource.data([]);
            } catch (e) {

            }

            selectedIDs = null;
            IDwithData = [];
            $("#EntranceQuestionGrid").data('kendoGrid').dataSource.page(1);

        }
    });
    //$('#divChooseDynamicQuizQuestion .btnAddMoreQuestion').off().on('click', function (e) {
    //    e.preventDefault();
    //    var obj = $('#divChooseDynamicQuizQuestion .dynamicQuestionContainer>div');
    //    var QuestionCategoryDropDown;
    //    var ValidationSpan;
    //    counter++;
    //    $.each(obj, function (index, item) {
    //        if (index == 0) {
    //            $(item).append('<input type="hidden" value="-1">');
    //        }
    //        else if (index == 1) {
    //            ValidationSpan = '<span class="field-validation-valid" data-valmsg-for="ddlDynamicquestionCategory' + counter + '" data-valmsg-replace="true"></span>';
    //            QuestionCategoryDropDown = $(item).find('.form-group select:eq(0)').clone();
    //            QuestionCategoryDropDown.prop('name', 'ddlDynamicquestionCategory' + counter);
    //            $(item).find('.form-group').append(QuestionCategoryDropDown).append(ValidationSpan);
    //            QuestionCategoryDropDown = "";
    //            ValidationSpan = "";
    //        }
    //        else if (index == 2) {
    //            ValidationSpan = '<span class="field-validation-valid" data-valmsg-for="ddlDynamicquestionDifficulty' + counter + '" data-valmsg-replace="true"></span>';
    //            QuestionCategoryDropDown = $(item).find('.form-group select:eq(0)').clone();
    //            QuestionCategoryDropDown.prop('name', 'ddlDynamicquestionDifficulty' + counter);
    //            $(item).find('.form-group').append(QuestionCategoryDropDown).append(ValidationSpan);
    //            QuestionCategoryDropDown = "";
    //            ValidationSpan = "";
    //        }
    //        else if (index == 3) {
    //            ValidationSpan = '<span data-valmsg-for="dyanmicMandatoryQuestionno' + counter + '" data-valmsg-replace="true"></span>';
    //            $(item).find('.form-group').append('<input class="form-control" type="number" placeholder="Mandatory Question" name="dyanmicMandatoryQuestionno' + counter + '" data-val="true" data-val-required="This field is required." min="0"/>').append(ValidationSpan);
    //            ValidationSpan = "";
    //        }
    //        else if (index == 4) {
    //            ValidationSpan = '<span data-valmsg-for="dyanmicOptionalQuestionno' + counter + '" data-valmsg-replace="true"></span>';
    //            $(item).find('.form-group').append('<input class="form-control" type="number" placeholder="Optional Question" name="dyanmicOptionalQuestionno' + counter + '" data-val="true" data-val-required="This field is required." min="0"/>').append(ValidationSpan);
    //            ValidationSpan = "";
    //        }
    //        else if (index == 5) {
    //            $(item).find('.form-group').append("<i class='fa fa-times deleteDynamicQuestion' aria-hidden='true'></i>");
    //        }
    //    });
    //    UIEvent();
    //    //Remove Validator
    //    $("form.quiz-form").removeData("validator").removeData("unobtrusiveValidation");
    //    //Parse the form again
    //    $.validator.unobtrusive.parse("form");
    //});
    //$('#divChooseDynamicQuizQuestion .dynamicQuestionContainer .deleteDynamicQuestion').off().on('click', function (e) {
    //    e.preventDefault();
    //    var currentIndex = $('#divChooseDynamicQuizQuestion .dynamicQuestionContainer .deleteDynamicQuestion').index($(this));
    //    $.each($('#divChooseDynamicQuizQuestion .dynamicQuestionContainer>div'), function (index, item) {
    //        if (index == 0) {
    //            $(item).find('input').eq(currentIndex).remove();
    //        }
    //        else if (index == 1) {
    //            $(item).find('div').children('span').eq(parseInt(currentIndex)).remove();
    //            $(item).find('select').eq(currentIndex).remove();
    //        }
    //        else if (index == 2) {
    //            $(item).find('div').children('span').eq(parseInt(currentIndex)).remove();
    //            $(item).find('select').eq(currentIndex).remove();
    //        }
    //        else if (index == 3) {
    //            $(item).find('div').children('span').eq(parseInt(currentIndex)).remove();
    //            $(item).find('input').eq(currentIndex).remove();
    //        }
    //        else if (index == 4) {
    //            $(item).find('div').children('span').eq(parseInt(currentIndex)).remove();
    //            $(item).find('input').eq(currentIndex).remove();
    //        }
    //        else if (index == 5) {
    //            $(item).find('div').children('span').eq(parseInt(currentIndex)).remove();
    //            $(item).find('.fa.fa-times').eq(currentIndex).remove();
    //        }
    //    });

    //});
    //$('#btnSaveDynamicQuestionDetail').off().on('click', function (e) {
    //    e.preventDefault();
    //    var IsValid = $('form.quiz-form').data('unobtrusiveValidation').validate();
    //    if (IsValid) {

    //        var divContainerLst = $('#divChooseDynamicQuizQuestion .dynamicQuestionContainer>div');
    //        var OptionalCount = 1;
    //        var MandatoryCount = 1;
    //        var IsCountMatch = true;
    //        $.each(divContainerLst, function (index, item) {
    //            if (index == 3) {
    //                MandatoryCount = $(item).find('input').val();
    //            }
    //            else if (index == 4) {
    //                OptionalCount = $(item).find('input').val();
    //            }
    //            if (OptionalCount == 0 && MandatoryCount == 0) {
    //                IsCountMatch = false;
    //            }
    //        });
    //        if (IsCountMatch) {
    //            $('#divChooseDynamicQuizQuestion').hide();
    //            $('#divEntranceForm').show();
    //        }
    //        else {
    //            ShowAlertMessage(true, "Mandatory And Optional Question cannot be zero");
    //        }
    //    }
    //});
    //$('#btnCancelDynamicQuestionDetail').off().on('click', function (e) {
    //    e.preventDefault();
    //    var divContainerLst = $('#divChooseDynamicQuizQuestion .dynamicQuestionContainer>div');
    //    $.each(divContainerLst, function (index, item) {
    //        if (index == 0) {
    //            $(item).find('input').val('-1').not(':first').remove();
    //        }
    //        else if (index == 1 || index == 2) {
    //            $(item).find('select').val('-1').not(':first').remove();
    //            $(item).find('div').children('span').not(':first').remove();
    //        }
    //        else if (index == 3 || index == 4) {
    //            $(item).find('input').val('').not(':first').remove();
    //            $(item).find('div').children('span').not(':first').remove();
    //        }
    //        else if (index == 5) {
    //            $(item).find('i').remove();
    //        }
    //    });
    //    $('.field-validation-error').removeClass('field-validation-error').addClass('field-validation-valid');
    //    $('.input-validation-error').removeClass('input-validation-error').addClass('valid');
    //    $('#divChooseDynamicQuizQuestion').hide();
    //    $('#divEntranceForm').show();
    //});

    //$('#btn_addtags').off('click').on('click', function (e) {
    //    e.preventDefault();

    //    tagsJS.center().open();

    //    $("#btn_tagsave").off('click').on('click', function (e) {
    //        e.preventDefault();
    //        var inpdata = $('#text_tagvalue').val().split(',');
    //        $.ajax({
    //            url: '/Admin/Quiz/SaveTag',
    //            type: 'post',
    //            contentType: "application/json; charset=utf-8",
    //            data: JSON.stringify(inpdata),
    //            success: function (item) {
    //                ShowAlertMessage(item.ErrorOccured, item.Message);
    //                if (item.data.length != null) {
    //                    var KTagDDL = $('#Tag').data('kendoMultiSelect');
    //                    var existingID = KTagDDL.value();

    //                    for (var i = 0; i < item.data.length; i++) {
    //                        $('#Tag').append('<option value="' + item.data[i].TagID + '">' + item.data[i].TagName + '</option>');

    //                        KTagDDL.dataSource.data().push({ text: item.data[i].TagName, value: item.data[i].TagID.toString() });
    //                    }
    //                }
    //                $("#text_tagvalue").val('');
    //            }, beforeSend: function () {
    //                //loadingNow($('div#divEntranceForm'), true);
    //            },
    //            complete: function () {
    //                //loadingNow($('div#divEntranceForm'), false);
    //            },
    //            error: function () {
    //                //loadingNow($('div#divEntranceForm'), false);
    //            }
    //        });
    //        tagsJS.close();
    //    });

    //    $("#btn_tagcancel").on('click', function (e) {
    //        e.preventDefault();

    //        tagsJS.close();
    //    });


    //});
    $("button#btnSaveStatusForSelectedQuiz").off().on('click', function (e) {
        e.preventDefault();
        var SelectedQuizIDWithStatus = [];
        console.log(EntranceIDwithData);
        var Status = $("#ddlChangeQuizStatus :selected").val();
        $.each(EntranceIDwithData, function (index, item) {
            console.log(item.Status);
            if (item.Status == true) {
                SelectedQuizIDWithStatus.push({
                    EntranceID: item.EntranceID,
                    Status: Status
                });
            }
        });
        GetStatusForBatchUpdate(JSON.stringify(SelectedQuizIDWithStatus));
        // BatchUpdateEntranceStatus(JSON.stringify(SelectedQuizIDWithStatus));
        $("#btnConfirmUpdate").off().on('click', function (e) {
            e.preventDefault();
            BatchUpdateEntranceStatus(JSON.stringify(SelectedQuizIDWithStatus));
        });
        $("#btnCancelUpdate").off().on('click', function (e) {
            e.preventDefault();
            EntranceIDwithData = [];
            ReadEntranceListingGrid();
            $("div.changeStatusOption").removeClass('show').addClass('hide');
        })
    });
}
var start = $("#StartDate").kendoDateTimePicker({
    change: startChange,
    min: new Date(),
    interval: 5,
    format: "MM/dd/yyyy h:mm tt",

}).data("kendoDateTimePicker");

var end = $("#EndDate").kendoDateTimePicker({
    change: endChange,
    interval: 5,
    min: new Date(),
    format: "MM/dd/yyyy h:mm tt",

}).data("kendoDateTimePicker");

start.max(end.value());
end.min(start.value());
function SetUpQuestionGridAndAnswerGrid(NewDataToMove) {
    $("#SelectedQuestionGrid").kendoGrid({
        dataSource: {
            data: NewDataToMove,
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
            field: "EntranceQuestion",
            title: "Question",
            template: "#= trimQuestion(EntranceQuestion) #",
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
        ]
    });
    $("#divActualAnswerGrid").kendoGrid({
        dataSource: {
            data: NewDataToMove,
            pageSize: CustomRecordPerPage
        },
        filterable: false,
        groupable: false,
        sortable: true,
        ServerOperation: false,
        dataBound: onDataBoundActualAnswerGrid,
        pageable: {
            pageSizes: true,
            refresh: true,
            buttonCount: 5
        },
        columns: [{
            field: "EntranceQuestion",
            title: "Question",
            template: "#= trimQuestion(EntranceQuestion) #",
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
        ]
    });
}

//function setUpDynamicallyAddedQuestion(ArrayLst) {
//    var arraylength = ArrayLst.length;
//    if (arraylength > 0) {
//        var containerLst = $('#divChooseDynamicQuizQuestion .dynamicQuestionContainer>div');
//        $.each(ArrayLst, function (index, item) {
//            $.each(containerLst, function (indexsub, itemsub) {
//                if (index == 0) {
//                    if (indexsub == 0) {
//                        $(itemsub).children("input").val(item.DynamicQuizQuestionID);
//                    }
//                    else if (indexsub == 1) {
//                        $(itemsub).find("select").val(item.QuestionCategory);
//                    }
//                    else if (indexsub == 2) {
//                        $(itemsub).find("select").val(item.QuestionDifficulty);
//                    }
//                    else if (indexsub == 3) {
//                        $(itemsub).find("input").val(item.TotalMandatoryQuestion);
//                    }
//                    else if (indexsub == 4) {
//                        $(itemsub).find("input").val(item.TotalOptionalQuestion);
//                    }
//                }
//                else {
//                    if (indexsub == 0) {
//                        $(itemsub).children("input:last-child").val(item.DynamicQuizQuestionID);
//                    }
//                    else if (indexsub == 1) {
//                        $(itemsub).find("select:last").val(item.QuestionCategory);
//                    }
//                    else if (indexsub == 2) {
//                        $(itemsub).find("select:last").val(item.QuestionDifficulty);
//                    }
//                    else if (indexsub == 3) {
//                        $(itemsub).find("input:last").val(item.TotalMandatoryQuestion);
//                    }
//                    else if (indexsub == 4) {
//                        $(itemsub).find("input:last").val(item.TotalOptionalQuestion);
//                    }
//                }
//            });
//            if (index < arraylength - 1) {
//                $("#formHeading .btnAddMoreQuestion").trigger("click");
//            }
//        });
//    }
//    $("#btnChooseQuestionManually").hide();
//    $("#btnChooseQuestionDynamically").show();
//}



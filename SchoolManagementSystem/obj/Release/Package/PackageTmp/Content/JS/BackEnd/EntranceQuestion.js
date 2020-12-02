var rowNumber = 0;
var identifier = 0;
var validelement = 1;
var DeleteAnswerPoolID = "";
var IDwithData = [];
validator = function () {
    var valid = $("#formEntranceQuestion").validate({
        ignore: ".IgnoreClass, :hidden",
        rules: {
            ddlQuestionCategory: {
                required: true,
            },
            ddlQuestionDifficulty: {
                required: true,
            },
            dynProductCode: {
                required: true,
            },
            ddlQuestionWeightage: {
                required: true,
            },
            txtNoOfAnswer: {
                required: true,
                positiveinteger: true,
            },
            ddlQuestionType: {
                required: true,
            },
            txtPointsForEachAnswer: {
                required: true,
                PositiveIntOrTwoDigitdecimal: true,
            },
            txtQuestionCompletionTime: {
                required: true,
                PositiveIntOrTwoDigitdecimal: true,
            },
            ddlStatus: {
                required: true,
            },
            txtSortOrder: {
                positiveinteger: true,
            },
            txtEntranceQuestion: {
                required: true,
                validateScript: true
            },
            DynamicddlAnswer: {
                required: true,
            },
            DynamictxtAnswer: {
                required: true,
                validateScript: true
            },
        },
        messages: {
            ddlQuestionCategory: {
                required: "Category Required",
            },
            ddlQuestionDifficulty: {
                required: "Difficulty Required",
            },
            dynProductCode: {
                required: "ProductCode Required",
            },
            ddlQuestionWeightage: {
                required: "Weightage Required",
            },
            txtNoOfAnswer: {
                required: "Total Answer Required",
                positiveinteger: "Positive Number Only",
            },
            ddlQuestionType: {
                required: "QuestionType Required",
            },
            txtPointsForEachAnswer: {
                required: "PointsForEachAnswer Required",
                PositiveIntOrTwoDigitdecimal: true,
            },
            txtQuestionCompletionTime: {
                required: "CompletionTime Required",
                PositiveIntOrTwoDigitdecimal: true,
            },
            ddlStatus: {
                required: "Status Required",
            },
            txtSortOrder: {
                positiveinteger: "Positive Number Only",
            },
            txtEntranceQuestion: {
                required: "Question Required",
                validateScript: "Harmful Code Detected"
            },
            DynamicddlAnswer: {
                required: "Answer Required",
            },
            DynamictxtAnswer: {
                required: "Answer Required",
                validateScript: "Harmful Code Detected"
            },
        }
    });
    return valid
}
function LoadQuizQuestionGrid() {
    //var grid = $("#EntranceQuestionGrid").data("kendoGrid");
    //grid.dataSource.page(1);


    var grid = $("#EntranceQuestionGrid").data("kendoGrid");
    grid.dataSource.read({
        SearchQuizQuestion: $('#txtSearchKeyword').val(),
        SearchQuestionTypeID: $('#ddlSearchQuestionType :selected').val(),
        SearchCategoryID: $('#ddlSearchQuestionCategory :selected').val(),
        SearchDifficultyLevelID: $('#ddlSearchDifficultyLevel :selected').val(),
        SearchWeightageID: $('#ddlSearchWeightage :selected').val(),
        SearchStatus: $('#ddlSearchStatus :selected').val(),
        SearchQuestionType: $("#ddlSearchMandatory :selected").val(),
        pageSize: grid._pageSize,
        pageNumber: grid._page
    });
    grid.refresh();
}
function AddAntiForgeryToken(data) {
    data.__RequestVerificationToken = $('form input[name=__RequestVerificationToken]').val();
    return data;
};
function ParamToLoadEntranceQuestionList() {
    var grid = $("#EntranceQuestionGrid").data("kendoGrid").dataSource;
    return {
        SearchQuizQuestion: $('#txtSearchKeyword').val(),
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
$(document).ready(function () {
    ShowListAndHideForm();
    LoadQuestionDifficultyLevelDropDown();
    LoadQuestionWeightageLevelDropDown();
   // GetQuizQuestionCategoryType();
    LoadQuizQuestionTypeDropDown();
    LoadQuizQuestionGrid();
    $('#hfAnswerPoolID').val('-1');
    $("#txtNoOfAnswer").val('1');
    RemoveHiddenClass();
    UIEvent();
});
function UIEvent() {
    $("#btnSearchEntranceQuestion").off().on('click', function (e) {
        e.preventDefault();
        $("#EntranceQuestionGrid").data("kendoGrid").dataSource.page(1);
        LoadQuizQuestionGrid();
    });
    $("#btnResetEntranceQuestion").off().on('click', function (e) {
        e.preventDefault();
        $("#txtSearchKeyword").val('');
        $("#ddlSearchStatus").prop('selectedIndex', 0);
        $("#ddlSearchQuestionType").val('-1');
        $("#ddlSearchQuestionCategory").val('-1');
        $("#ddlSearchDifficultyLevel").val('-1');
        $("#ddlSearchWeightage").val('-1');
        $("#ddlSearchMandatory").val('-1');
        $("#EntranceQuestionGrid").data("kendoGrid").dataSource.page(1);
        LoadQuizQuestionGrid();
    });

    $('#ddlQuestionType').off().on('change', function (e) {
        e.preventDefault();
        var IsSingleTextBox = $('#ddlQuestionType :selected').attr('textboxsingle');
        var TrueFalseFlag = $('#ddlQuestionType :selected').attr('trueorfasle');
        var totalHolder = $('#formEntranceQuestion .dynamicFormContainer .divSubClassHolder');
        $.each(totalHolder, function (index, item) {
            if (index > 0) {
                var IsEdit = parseInt($('#hfID').val()) > 0 ? true : false;
                if (IsEdit) {
                    if (index - 1 == 0) {
                        if (DeleteAnswerPoolID == "") {
                            DeleteAnswerPoolID = $(item).find('.hfAnswerPoolID').val();
                        }
                        else {
                            DeleteAnswerPoolID = DeleteAnswerPoolID + ',' + $(item).find('.hfAnswerPoolID').val();
                        }
                    }
                    else {
                        DeleteAnswerPoolID = DeleteAnswerPoolID + ',' + $(item).find('.hfAnswerPoolID').val();
                    }
                }
                $(item).remove();
            }
            else {
                if ($(item).find('.hfAnswerPoolID').val() != "-1") {
                    if (DeleteAnswerPoolID == "") {
                        DeleteAnswerPoolID = $(item).find('.hfAnswerPoolID').val();
                    }
                    else {
                        DeleteAnswerPoolID = DeleteAnswerPoolID + ',' + $(item).find('.hfAnswerPoolID').val();
                    }
                }
                $(item).find('.hfAnswerPoolID').val('-1');
                $(item).find('.clsNotTrueFalse').val('');
                //$(item).find('.clsTrueFalse').val('');
                $(item).find('.clsTrueFalse')[0].selectedIndex=1;
            }
        });
        if (IsSingleTextBox == "true") {
            $('#formEntranceQuestion .dynamicFormContainer .divSubClassHolder').find('.clsAnswerStatus').hide();
            $('#formEntranceQuestion .dynamicFormContainer .divSubClassHolder').find('.clsNotTrueFalse').hide();
            $('#formEntranceQuestion .dynamicFormContainer').find('.AddMoreDynamic').hide();
            $('#formEntranceQuestion .dynamicFormContainer').find('.RemoveDynamicContent').hide();
          //  $("#divAnswerLabelContainer").hide();

        } else if (IsSingleTextBox == "false") {
            $('#formEntranceQuestion .dynamicFormContainer .divSubClassHolder').find('.clsAnswerStatus').show();
            $('#formEntranceQuestion .dynamicFormContainer .divSubClassHolder').find('.clsNotTrueFalse').show();
           // $("#divAnswerLabelContainer").show();
        }

        if (TrueFalseFlag == "true") {
            $('#formEntranceQuestion .dynamicFormContainer .divSubClassHolder').find('.clsNotTrueFalse').hide();
            $('#formEntranceQuestion .dynamicFormContainer .divSubClassHolder').find('.clsTrueFalse').show();
            $('#formEntranceQuestion .dynamicFormContainer').find('.AddMoreDynamic').trigger('click').hide();
            $('#formEntranceQuestion .dynamicFormContainer').find('.RemoveDynamicContent').hide();
        } else if (TrueFalseFlag == "false") {
            $('#formEntranceQuestion .dynamicFormContainer .divSubClassHolder').find('.clsNotTrueFalse').show();
            $('#formEntranceQuestion .dynamicFormContainer .divSubClassHolder').find('.clsTrueFalse').hide();
            $('#formEntranceQuestion .dynamicFormContainer').find('.AddMoreDynamic').show();
            $('#formEntranceQuestion .dynamicFormContainer').find('.RemoveDynamicContent').show();

        }

        if (TrueFalseFlag == "false" && IsSingleTextBox == "true") {
            $('#formEntranceQuestion .dynamicFormContainer .divSubClassHolder').find('.clsNotTrueFalse').hide();
            $('#formEntranceQuestion .dynamicFormContainer').find('.AddMoreDynamic').hide();
            $('#formEntranceQuestion .dynamicFormContainer').find('.RemoveDynamicContent').hide();
        }
        else if (TrueFalseFlag == "false" && IsSingleTextBox == "false") {
            $('#formEntranceQuestion .dynamicFormContainer').find('.RemoveDynamicContent').hide();
        }
        $('#txtNoOfAnswer').val("1");
    });

    $('#formEntranceQuestion .dynamicFormContainer .AddMoreDynamic').off().on('click', function (e) {
        e.preventDefault();
        var html = '';
        html += '<div class="row divSubClassHolder">';
        html += '<div class="col-lg-6 col-md-6">';
        html += '<div class="form-group">';
        html += "<input type='hidden' value='-1' class='hfAnswerPoolID' />";
        html += '<input type="text" class="form-control clsNotTrueFalse" name="DynamictxtAnswer' + validelement + '" />';
        html += '<select class="form-control clsTrueFalse" style="display:none" name="DynamicddlAnswer' + validelement + '"><option value="true" selected="selected">True</option><option value="false">False</option></select>';
        html += '</div></div>';
        html += '<div class="col-lg-4 col-md-4">';
        html += '<div class="form-group">';
        html += '<select class="form-control clsAnswerStatus"><option value="true">Correct</option><option value="false">Incorrect</option></select>';
        html += '</div></div>';
        html += '<div class="col-lg-2 col-md-2 but-cancel">';
        html += '<button class="RemoveDynamicContent"><i class="fa fa-times-circle-o" aria-hidden="true"></i></button>';
        html += '</div></div>';
        $('.dynamicFormContainer').append(html);
        var v = validator();
        $('input[name="DynamictxtAnswer' + validelement + '"]').rules("add", {
            required: true,
            validateScript: true
        });
        $('select[name="DynamicddlAnswer' + validelement + '"]').rules("add", {
            required: true
        });
        html = "";
        UIEvent();
        var IsSingleTextBox = $('#ddlQuestionType :selected').attr('textboxsingle');
        var TrueFalseFlag = $('#ddlQuestionType :selected').attr('trueorfasle');
        if (IsSingleTextBox == "true") {
            $('#formEntranceQuestion .dynamicFormContainer .divSubClassHolder').find('.clsAnswerStatus').hide();
        } else if (IsSingleTextBox == "false") {
            $('#formEntranceQuestion .dynamicFormContainer .divSubClassHolder').find('.clsAnswerStatus').show();
        }
        if (TrueFalseFlag == "true") {
            $('#formEntranceQuestion .dynamicFormContainer .divSubClassHolder').find('.clsNotTrueFalse').hide();
            $('#formEntranceQuestion .dynamicFormContainer .divSubClassHolder').find('.clsTrueFalse').show();
        } else if (TrueFalseFlag == "false") {
            $('#formEntranceQuestion .dynamicFormContainer .divSubClassHolder').find('.clsNotTrueFalse').show();
            $('#formEntranceQuestion .dynamicFormContainer .divSubClassHolder').find('.clsTrueFalse').hide();
        }
        validelement++;
        var temp = parseInt($('#txtNoOfAnswer').val());
        $('#txtNoOfAnswer').val(temp + 1);
    });
    $('#formEntranceQuestion .dynamicFormContainer .RemoveDynamicContent').off().on('click', function (e) {
        e.preventDefault();
        var dynamicDataContainer = $('#formEntranceQuestion .dynamicFormContainer .divSubClassHolder').length;
        var value = "";
        value = $(this).parent().parent().find('.hfAnswerPoolID').val();
        if (dynamicDataContainer > 1) {
            if (parseInt(value) > 0) {
                if (DeleteAnswerPoolID == "") {
                    DeleteAnswerPoolID = value;
                }
                else {
                    DeleteAnswerPoolID = DeleteAnswerPoolID + ',' + value;
                }
            }
            var temp = parseInt($('#txtNoOfAnswer').val());
            $('#txtNoOfAnswer').val(temp - 1);
            $(this).parent().parent().remove();
        }
    })
    $("#btnCreateEntranceQuestion").off().on('click', function (e) {
        e.preventDefault();
        $('#formHeading').val('Add Entrance Question');
        $('#btnSave').val('Submit');
        $('#btnCancel').val('Cancel');
        ShowFormAndHideList();
        $("#ddlQuestionType").prop('disabled', false);
    });
    wnd = $("#modalWindow").kendoWindow({
        title: "Delete confirmation",
        modal: true,
        visible: false,
        resizable: false,
        width: 400
    }).data("kendoWindow");
    $("#btnSave").off().on('click', function (e) {
        $("#txtNoOfAnswer").val($("div.dynamicFormContainer").find('div.row.divSubClassHolder').length);
        var temp = validator();
        if (temp.form()) {
            var AddUpdateQuestionID = $('#hfID').val();
            var QuestionTypeID = $('#ddlQuestionType :selected').val();
            var EntranceQuestion = $('#txtEntranceQuestion').val().replace(/\s+/g, " ");
            var DifficultyLevelID = $('#ddlQuestionDifficulty :selected').val();
            var WeightageID = $('#ddlQuestionWeightage :selected').val();
            var IsActive = $('#ddlStatus :selected').val() == "0" ? false : true;
            var IsMandatory = $('#chkMandatoryQuestion').is(":checked");
            var SortOrder = $('#txtSortOrder').val();
            var PointsToEachAnswer = $('#txtPointsForEachAnswer').val();
            var Duration = $('#txtQuestionCompletionTime').val();
            var QuestionCategoryID = $('#ddlQuestionCategory :selected').val();
            var DynamicFormContainer = $('#formEntranceQuestion .dynamicFormContainer .divSubClassHolder');
            var QuestionAnswers = "";
            var IsAnswerCorrectStatus = "";
            var AddUpdateAnswerPoolID = "";
            if ($('#ddlQuestionType :selected').attr('textboxsingle') == "false") {
                $.each(DynamicFormContainer, function (index, item) {
                    if (index == 0) {
                        if (!$(item).find('.clsNotTrueFalse').is(":hidden")) {
                            QuestionAnswers = $(item).find('.clsNotTrueFalse').val();
                            IsAnswerCorrectStatus = $(item).find('.clsAnswerStatus :selected').val();
                            AddUpdateAnswerPoolID = $(item).find('.hfAnswerPoolID').val();
                        }
                        else {
                            QuestionAnswers = $(item).find('.clsTrueFalse :selected').val();
                            IsAnswerCorrectStatus = $(item).find('.clsAnswerStatus :selected').val();
                            AddUpdateAnswerPoolID = $(item).find('.hfAnswerPoolID').val();
                        }
                    }
                    else {
                        if (!$(item).find('.clsNotTrueFalse').is(":hidden")) {
                            QuestionAnswers = QuestionAnswers + '`' + $(item).find('.clsNotTrueFalse').val();
                            IsAnswerCorrectStatus = IsAnswerCorrectStatus + ',' + $(item).find('.clsAnswerStatus :selected').val();
                            AddUpdateAnswerPoolID = AddUpdateAnswerPoolID + ',' + $(item).find('.hfAnswerPoolID').val();
                        }
                        else {
                            QuestionAnswers = QuestionAnswers + '`' + $(item).find('.clsTrueFalse :selected').val();
                            IsAnswerCorrectStatus = IsAnswerCorrectStatus + ',' + $(item).find('.clsAnswerStatus :selected').val();
                            AddUpdateAnswerPoolID = AddUpdateAnswerPoolID + ',' + $(item).find('.hfAnswerPoolID').val();
                        }
                    }
                })
            }
            var mydata = {
                AddUpdateQuestionID: AddUpdateQuestionID,
                QuestionTypeID: QuestionTypeID,
                EntranceQuestion: EntranceQuestion,
                DifficultyLevelID: DifficultyLevelID,
                WeightageID: WeightageID,
                IsActive: IsActive,
                IsMandatory: IsMandatory,
                SortOrder: SortOrder,
                PointsToEachAnswer: PointsToEachAnswer,
                Duration: Duration,
                QuestionCategoryID: QuestionCategoryID,
                QuestionAnswers: QuestionAnswers,
                IsAnswerCorrectStatus: IsAnswerCorrectStatus,
                AddUpdateAnswerPoolID: AddUpdateAnswerPoolID,
                DeleteAnswerPoolID: DeleteAnswerPoolID,
                QuestionID: $('#hfID').val(),
            };
            $.ajax({
                url: 'EntranceQuestion/AddUpdateEntranceQuestion',
                type: 'POST',
                dataType: "json",
                data: AddAntiForgeryToken(mydata),
                success: function (statusData) {
                    if (statusData.ReturnCode == 200) {
                        ShowAlertMessage(false, statusData.Message);

                    }
                    else {
                        ShowAlertMessage(true, statusData.Message);
                    }
                    ShowListAndHideForm();
                    LoadQuizQuestionGrid();
                    ClearAll();
                }, beforeSend: function () {
                    //loadingNow($('div#divEntranceQuestionForm'), true);
                },
                complete: function () {
                    //loadingNow($('div#divEntranceQuestionForm'), false);
                },
                error: function () {
                    //loadingNow($('div#divEntranceQuestionForm'), false);
                }

            });
        }
    });

    $('#btnCancel').off().on('click', function (e) {
        e.preventDefault();
        ClearAll();
        $("#divEntranceQuestionForm").hide();
        $("#divEntranceQuestionList").show();
    });

    $("#FieldFilter").off().on('keyup', function (e) {
        e.preventDefault();
        var value = $("#FieldFilter").val();
        grid = $("#EntranceQuestionGrid").data("kendoGrid");
        rowNumber = 0;
        if (value) {
            grid.dataSource.filter({
                logic: "or",
                filters: [
                    { field: "Title", operator: "contains", value: value },
                    { field: "Description", operator: "contains", value: value }
                ]
            });
        } else {
            grid.dataSource.filter({});
        }
    });
    $('#btnBackToMainForm').off().on('click', function (e) {
        e.preventDefault();
        $('#divChooseQuizQuestion').hide();
        $('#divEntranceForm').show();
    });

    $("button#btnSaveStatusForSelectedQuizQuestion").off().on('click', function (e) {
        e.preventDefault();
        var SelectedQuestionIDWithStatus = [];
        var Status = $("#ddlChangeQuizStatus :selected").val();
        $.each(IDwithData, function (index, item) {
            if (item.Status == true) {
                SelectedQuestionIDWithStatus.push({
                    QuestionID: item.ActualQuestionID,
                    Status: Status
                });
            }
        });
        GetStatusForBatchUpdateQuestionUpdate(JSON.stringify(SelectedQuestionIDWithStatus));
        $("#btnConfirmUpdate").off().on('click', function (e) {
            e.preventDefault();
            BatchUpdateQuizQuestionStatus(JSON.stringify(SelectedQuestionIDWithStatus));
        });
        $("#btnCancelUpdate").off().on('click', function (e) {
            e.preventDefault();
            IDwithData = [];
            LoadQuizQuestionGrid();
            $("div.changeStatusOption").removeClass('show').addClass('hide');
        })
    });
}
function BatchUpdateQuizQuestionStatus(JsonObject) {
    var mydata = {
        JsonObject: JsonObject
    }
    $.ajax({
        type: "post",
        dataType: "json",
        url: 'EntranceQuestion/BatchUpdateEntranceQuestionStatus',
        data: AddAntiForgeryToken(mydata),
        success: function (data) {
            console.log(data);
            if (data.Message == "Success") {
                ShowAlertMessage(false, "Status Updated");
            }
            else {
                ShowAlertMessage(true, "Operation Failed");
            }

            IDwithData = [];
            LoadQuizQuestionGrid();
            $("div.changeStatusOption").removeClass('show').addClass('hide');
        }, beforeSend: function () {
            //loadingNow($('div#divEntranceQuestionForm'), true);
        },
        complete: function () {
            //loadingNow($('div#divEntranceQuestionForm'), false);
        },
        error: function () {
            //loadingNow($('div#divEntranceQuestionForm'), false);
        }
    })
}

function FormatMandatory(data) {
    if (data) {
        return "Yes";
    }
    else {
        return "No";
    }
}

function GetStatusForBatchUpdateQuestionUpdate(JsonObject) {
    var mydata = {
        JsonObject: JsonObject
    }
    $.ajax({
        type: "post",
        dataType: "json",
        url: 'EntranceQuestion/GetStatusForBatchUpdateQuestionUpdate',
        data: AddAntiForgeryToken(mydata),
        success: function (data) {
            console.log(data);
            if (data.Message == "Success") {
                var ele = $("div#QuestionBatchUpdateModal").find('ol.UpdateStatusListingBody');
                var html = '';
                var objJson;
                if (data.data == null) {
                    objJson = [];
                }
                else if (data.data.indexOf("[") != -1) {
                    objJson = JSON.parse(data.data);
                }
                else {
                    objJson = JSON.parse("[" + data.data + "]");
                }

                $(ele).empty();
                $.each(objJson, function (index, item) {
                    html += "<li><span>";
                    html += item.QuestionStatus;
                    html += "</span></li>";
                    $(ele).append(html);
                    html = "";
                });
                $("#QuestionBatchUpdateModal").modal('show');
            }
            else {
                ShowAlertMessage(true, "Operation Failed");
            }


        }, beforeSend: function () {
            //loadingNow($('div#divEntranceQuestionForm'), true);
        },
        complete: function () {
            //loadingNow($('div#divEntranceQuestionForm'), false);
        },
        error: function () {
            //loadingNow($('div#divEntranceQuestionForm'), false);
        }
    })
}
function EditEntranceQuestion(e) {
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    $('#formHeading').val('Edit Entrance Question');
    $('#btnSave').val('Save');
    $('#btnCancel').val('Close');
    var mydata = {
        QuestionID: dataItem.QuestionID,
    }
    $.ajax({
        url: 'EntranceQuestion/GetEntranceQuestionByID',
        type: 'POST',
        dataType: "json",
        data: AddAntiForgeryToken(mydata),
        success: function (QuestionInfo) {
            if (QuestionInfo.Message == "Success") {
                if (QuestionInfo.data.IsUpdatable == true) {
                    $('#hfID').val(dataItem.QuestionID);
                    $('#ddlQuestionCategory').val(QuestionInfo.data.QuestionCategoryID);
                    $('#ddlQuestionWeightage').val(QuestionInfo.data.WeightageID);
                    $('#ddlQuestionDifficulty').val(QuestionInfo.data.DifficultyLevelID);
                    $('#txtNoOfAnswer').val(QuestionInfo.data.NoOfAnswer);
                    $('#ddlQuestionType').val(QuestionInfo.data.QuestionTypeID);
                    $('#txtQuestionCompletionTime').val(QuestionInfo.data.Duration);
                    $('#txtPointsForEachAnswer').val(QuestionInfo.data.PointsToEachAnswer);
                    $('#ddlStatus').val(QuestionInfo.data.IsActive == true ? 'Active' : 'Inactive');
                    $('#chkMandatoryQuestion').prop('checked', QuestionInfo.data.IsMandatory);
                    $('#txtSortOrder').val(QuestionInfo.data.SortOrder);
                    $('#txtEntranceQuestion').val(QuestionInfo.data.EntranceQuestion);
                    var IsSingleTextBox = $("#ddlQuestionType option:selected").attr("textboxsingle");;
                    var IsTrueOrFalse = $("#ddlQuestionType option:selected").attr("trueorfasle");
                    var html = "";
                    var v = validator();
                    $.each(QuestionInfo.data.EntranceAnswerList, function (index, item) {
                        if (index == 0) {
                            $('#formEntranceQuestion .dynamicFormContainer').find('.divSubClassHolder').remove();
                        }
                        if (IsSingleTextBox == "true") {

                            $('#formEntranceQuestion .dynamicFormContainer .divSubClassHolder').find('.clsAnswerStatus').hide();
                            $('#formEntranceQuestion .dynamicFormContainer .divSubClassHolder').find('.clsNotTrueFalse').hide();

                        }
                        else {
                            html += '<div class="row divSubClassHolder">';
                            html += '<div class="col-lg-6">';
                            html += '<div class="form-group">';
                            html += "<input type='hidden' value='" + item.AnswerPoolID + "' class='hfAnswerPoolID'/>";
                            if (IsTrueOrFalse == "true") {
                                html += "<input type='text' class='form-control clsNotTrueFalse' style='display:none' name='DynamictxtAnswer" + validelement + "'/>";
                                if (item.QuizOption == "true") {
                                    html += "<select class='form-control clsTrueFalse' name='DynamicddlAnswer" + validelement + "'>";
                                    html += "<option value='true' selected>True</option><option value='false'>False</option></select>";
                                }
                                else {
                                    html += "<select class='form-control clsTrueFalse' name='DynamicddlAnswer" + validelement + "'>";
                                    html += "<option value='true'>True</option><option value='false' selected>False</option></select>";
                                }
                                html += '</div></div>';
                                html += '<div class="col-lg-4">';
                                html += '<div class="form-group">';
                                if (item.IsCorrectAnswer == true) {
                                    html += "<select class='form-control clsAnswerStatus'><option value='true' selected>Correct</option><option value='false'>Incorrect</option></select>";
                                }
                                else {
                                    html += "<select class='form-control clsAnswerStatus'><option value='true'>Correct</option><option value='false' selected>Incorrect</option></select>";
                                }
                                html += '</div></div>';

                                html += '<div class="col-lg-2 but-cancel">';
                                html += '<button class="RemoveDynamicContent"><i class="fa fa-times-circle-o" aria-hidden="true"></i></button>';
                                html += '</div></div>'
                                $('.dynamicFormContainer').append(html);

                            }
                            else {
                                html += "<input type='text' class='form-control clsNotTrueFalse' value='" + item.QuizOption + "' name='DynamictxtAnswer" + validelement + "'/>";
                                html += "<select class='form-control clsTrueFalse' style='display:none' name='DynamicddlAnswer" + validelement + "'>";
                                html += "<option value>Choose option</option><option value='true'>True</option><option value='false'>False</option></select>";
                                html += '</div></div>';
                                html += '<div class="col-lg-4">';
                                html += '<div class="form-group">';
                                if (item.IsCorrectAnswer == true) {
                                    html += "<select class='form-control clsAnswerStatus'><option value='true' selected>Correct</option><option value='false'>Incorrect</option></select>";
                                }
                                else {
                                    html += "<select class='form-control clsAnswerStatus' ><option value='true'>Correct</option><option value='false' selected>Incorrect</option></select>";
                                }
                                html += '</div></div>';
                                html += '<div class="col-lg-2 but-cancel">';
                                html += '<button class="RemoveDynamicContent"><i class="fa fa-times-circle-o" aria-hidden="true"></i></button>';
                                html += '</div></div>';
                                $('.dynamicFormContainer').append(html);

                            }
                            $('input[name="DynamictxtAnswer' + validelement + '"]').rules("add", {
                                required: true,
                                validateScript: true
                            });
                            $('select[name="DynamicddlAnswer' + validelement + '"]').rules("add", {
                                required: true
                            });
                        }
                        var v = validator();


                        validelement++;
                        html = "";
                    });

                    if (IsTrueOrFalse == "true") {
                        $(".dynamicFormContainer .AddMoreDynamic").hide();
                        $(".divSubClassHolder .RemoveDynamicContent").hide();
                    }
                    else {
                        $(".dynamicFormContainer .AddMoreDynamic").show();
                        $(".divSubClassHolder .RemoveDynamicContent").show();
                    }

                    if (IsSingleTextBox == "true" && IsTrueOrFalse == "false") {
                        $('#formEntranceQuestion .dynamicFormContainer .divSubClassHolder').find('.clsAnswerStatus').hide();
                        $('#formEntranceQuestion .dynamicFormContainer .divSubClassHolder').find('.clsNotTrueFalse').hide();
                        $('#formEntranceQuestion .dynamicFormContainer').find('.AddMoreDynamic').hide();
                        $('#formEntranceQuestion .dynamicFormContainer').find('.RemoveDynamicContent').hide();
                        $("#txtNoOfAnswer").val("1");

                    }
                    $(".divSubClassHolder:first").find('.RemoveDynamicContent').hide();
                    ShowFormAndHideList();
                    $("#ddlQuestionType").prop('disabled', true);
                    UIEvent();
                }
                else {
                    ShowAlertMessage(true, "Cannot be updated. Question is in use");
                }
            }
        }, beforeSend: function () {
            //loadingNow($('div#divEntranceQuestionForm'), true);
        },
        complete: function () {
            //loadingNow($('div#divEntranceQuestionForm'), false);
        },
        error: function () {
            //loadingNow($('div#divEntranceQuestionForm'), false);
        }
    });

}
function ResetValidation() {
    validator().resetForm();
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
function ClearAll() {
    $('#hfID').val('-1');
    //$("select#ddlQuestionCategory")[0].selectedIndex = 0;
    $("select#ddlQuestionWeightage")[0].selectedIndex = 0;
    $("select#ddlQuestionDifficulty")[0].selectedIndex = 0;
    $("select#ddlStatus")[0].selectedIndex = 0;
    $('#txtNoOfAnswer').val('1');
    $('#ddlQuestionType').val('');
    $('#txtQuestionCompletionTime').val('');
    $('#txtPointsForEachAnswer').val('');
    $('#chkMandatoryQuestion').prop('checked', false);
    $('#txtSortOrder').val('');
    $('#txtEntranceQuestion').val('');
    var totalHolder = $('#formEntranceQuestion .dynamicFormContainer .divSubClassHolder');
    $.each(totalHolder, function (index, item) {
        if (index > 0) {
            $(item).remove();
        }
        else {
            $(item).find('.hfAnswerPoolID').val('-1');
           $(item).find('.clsNotTrueFalse').val('');
            $(item).find('.clsTrueFalse')[0].selectedIndex = 1;
           // $(item).find('.clsTrueFalse').val('');
            $(item).find('.clsNotTrueFalse').show();
            $(item).find('.clsTrueFalse').hide();
        }
    });
    ResetValidation();
}
function DeleteEntranceQuestion(e) {
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    wnd.center().open();
    $("#modalWindow #yes").off().on('click', function (e) {
        e.preventDefault();
        var mydata = {
            QuestionID: dataItem.QuestionID,
        }
        $.ajax({
            url: 'EntranceQuestion/DeleteEntranceQuestion',
            type: 'POST',
            dataType: "json",
            data: AddAntiForgeryToken(mydata),
            success: function (data) {
                if (data.ReturnCode == 200) {
                    ShowAlertMessage(false, data.Message);
                }
                else {
                    ShowAlertMessage(true, data.Message);
                }
                LoadQuizQuestionGrid();
                ShowListAndHideForm();
            }, beforeSend: function () {
                //loadingNow($('div#divEntranceQuestionForm'), true);
            },
            complete: function () {
                //loadingNow($('div#divEntranceQuestionForm'), false);
            },
            error: function () {
                //loadingNow($('div#divEntranceQuestionForm'), false);
            }
        });
        wnd.close();
    });

    $("#modalWindow #no").off().on('click', function (e) {
        e.preventDefault();
        wnd.close();
    });
}

function onDatabound(e) {
    rowNumber = 0;
    $(".k-grid-Edit").attr('title', 'Edit');
    $(".k-grid-Delete").attr('title', 'Delete');

    $(".k-grid-Edit").find("span").addClass("fa fa-pencil");
    $(".k-grid-Edit").removeClass("k-button");

    $(".k-grid-Delete").find("span").addClass("fa fa-trash");
    $(".k-grid-Delete").removeClass("k-button");

    var grid = e.sender;
    if (grid.dataSource.total() == 0) {
        var colCount = grid.columns.length;
        $(e.sender.wrapper)
            .find('tbody')
            .append('<tr class="kendo-data-row text-center"><td colspan="' + colCount + '" class="no-data">No Data Available</td></tr>');
    }
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
    if (count == i && count != 0) {
        $('#EntranceQuestionGrid .chkSelectAll').prop('checked', true);
    }
    else {
        $('#EntranceQuestionGrid .chkSelectAll').prop('checked', false);
    }
    CheckBoxEvent();
}

function renderNumber(data) {
    return ++rowNumber;
}

function renderRecordNumber(data) {
    var page = parseInt($("#EntranceQuestionGrid").data("kendoGrid").dataSource.page()) - 1;
    var pagesize = $("#EntranceQuestionGrid").data("kendoGrid").dataSource.pageSize();
    return parseInt(rowNumber + (parseInt(page) * parseInt(pagesize)));
}
function trimQuestion(data) {
    if (data.length > 50) {
        data = data.substring(0, 50) + '...';
    }
    return data;
}
function FormatModifiedDate(data) {
    if (kendo.toString(new Date(data), "yyyy/MM/dd") == kendo.toString(new Date("01-01-1900"), "yyyy/MM/dd")) {
        data = "-";
    }
    else {
        data = kendo.toString(new Date(data), CustomDateFormat.replace("{", "").replace("}", "").replace("0:", ""));
    }
    return data;
}

function ShowFormAndHideList() {
    $("#divEntranceQuestionForm").show();
    $("#divEntranceQuestionList").hide();
}

function ShowListAndHideForm() {
    $("#divEntranceQuestionForm").hide();
    $("#divEntranceQuestionList").show();
}
function RemoveHiddenClass() {
    $("#divEntranceQuestionForm").removeClass('popup hide');
    $("#modalWindow").removeClass('popup hide');
}

function LoadStatusDropDownForQuizQuestion() {
    $("#ddlSearchStatus").empty();
    $("#ddlStatus").empty();
    $.ajax({
        url: '/api/StatusAdminService/GetStatus/' + identifier,
        type: 'POST',
        async:false,
        success: function (statusData) {
            $('select#ddlSearchStatus').append('<option value="-1"> All Status </option>');
            $.each(statusData.Data, function (val, Status) {
                $('select#ddlSearchStatus').append('<option value="' + Status.StatusValue + '">' + Status.StatusName + '</option>');
                $('select#ddlStatus').append('<option value="' + Status.StatusValue + '">' + Status.StatusName + '</option>');
            });
        }

    });
}

function GetQuizQuestionCategoryType() {
    //$.ajax({
    //    url: '/api/CategoryTypeAdminService/GetQuizQuestionCategoryType',
    //    type: 'GET',
    //    async: false,
    //    success: function (data) {
    //        categoryType = data;
    //        LoadParentDropDown();

    //    }
    //});
}

function LoadParentDropDown() {
    //$("#ddlSearchQuestionCategory").empty();
    //$("#ddlQuestionCategory").empty();
    //$('select#ddlSearchQuestionCategory').append('<option value="-1">All Category </option>');
    //$.ajax({
    //    url: '/api/CategoryTreeAdminService/GetAllActiveParentForAdmin/' + categoryType,
    //    type: 'POST',
    //    async: false,
    //    success: function (categoryData) {
    //        if (categoryData != null) {
    //            $.each(categoryData.Data, function (val, CategoryTree) {
    //                $('select#ddlSearchQuestionCategory').append('<option value="' + CategoryTree.CategoryTreeID + '">' + CategoryTree.CategoryName + '</option>');
    //                $('select#ddlQuestionCategory').append('<option value="' + CategoryTree.CategoryTreeID + '">' + CategoryTree.CategoryName + '</option>');

    //            });
    //        }
    //    }

    //});
}

function LoadQuestionDifficultyLevelDropDown() {
    $("#ddlQuestionDifficulty").empty();
    $("#ddlSearchDifficultyLevel").empty();
    var mydata = {};
    $.ajax({
        url: 'EntranceQuestion/GetAllEntranceQuestionDifficultyLevel',
        type: 'POST',
        async: false,
        data: AddAntiForgeryToken(mydata),
        success: function (categoryData) {
            //$('select#ddlQuestionDifficulty').append('<option value> All Difficulty Level </option>');
            $('select#ddlSearchDifficultyLevel').append('<option value="-1"> All Difficulty Level </option>');
            if (categoryData != null) {
                $.each(categoryData.data, function (val, QuestionDifficulty) {
                    $('select#ddlQuestionDifficulty').append('<option value="' + QuestionDifficulty.DifficultyLevelID + '">' + QuestionDifficulty.DifficultyLevel + '</option>');
                    $('select#ddlSearchDifficultyLevel').append('<option value="' + QuestionDifficulty.DifficultyLevelID + '">' + QuestionDifficulty.DifficultyLevel + '</option>');

                });
            }
        }

    });
}

function LoadQuestionWeightageLevelDropDown() {
    $("#ddlSearchWeightage").empty();
    $("#ddlQuestionWeightage").empty();
    var mydata = {};
    $.ajax({
        url: 'EntranceQuestion/GetAllEntranceQuestionWeightageLevel',
        type: 'POST',
        async: false,
        data: AddAntiForgeryToken(mydata),
        success: function (categoryData) {
            $('select#ddlSearchWeightage').append('<option value="-1"> All Weightage </option>');
            //$('select#ddlQuestionWeightage').append('<option value> All Weightage </option>');
            if (categoryData != null) {
                $.each(categoryData.data, function (val, QuestionWeightage) {
                    $('select#ddlSearchWeightage').append('<option value="' + QuestionWeightage.QuestionWeigthageID + '">' + QuestionWeightage.QuestionWeight + '</option>');
                    $('select#ddlQuestionWeightage').append('<option value="' + QuestionWeightage.QuestionWeigthageID + '">' + QuestionWeightage.QuestionWeight + '</option>');
                });
            }
        }
    });
}

function LoadQuizQuestionTypeDropDown() {
    $("#ddlQuestionType").empty();
    $("#ddlSearchQuestionType").empty();
    var mydata = {};
    $.ajax({
        url: 'EntranceQuestion/GetAllEntranceQuestionType',
        type: 'POST',
        async: true,
        dataType: 'json',
        data: AddAntiForgeryToken(mydata),
        success: function (statusData) {
           // $('select#ddlQuestionType').append('<option value> All Question Type </option>');
            $('select#ddlSearchQuestionType').append('<option value="-1"> All Question Type </option>');
            $.each(statusData.data, function (val, EntranceQuestionType) {
                $('select#ddlQuestionType').append('<option value="' + EntranceQuestionType.EntranceQuestionTypeID + '" TextBoxSingle="' + EntranceQuestionType.IsSingleTextBox + '"  TrueOrFasle="' + EntranceQuestionType.IsTrueFalse + '">' + EntranceQuestionType.TypeDescription + '</option>');
                $('select#ddlSearchQuestionType').append('<option value="' + EntranceQuestionType.EntranceQuestionTypeID + '">' + EntranceQuestionType.TypeDescription + '</option>');
            });
            //$("#ddlQuestionType").val($("#ddlQuestionType option:eq(2)").val());
            $("#ddlQuestionType").trigger('change');
        }
    });
}

function LoadStatusIdentifierForQuizQuestion() {
    $.ajax({
        url: '/api/StatusIdentifierAdminService/GetEntranceQuestionStatusIdentifier',
        type: 'GET',
        success: function (data) {
            identifier = data;
            LoadStatusDropDownForQuizQuestion();
        }
    });
}

function CheckBoxEvent() {
    $("#EntranceQuestionGrid .multiSelect").off().on("click", function () {
        var checked = this.checked,
            row = $(this).closest("tr"),
            grid = $("#EntranceQuestionGrid").data("kendoGrid"),
            dataItem = grid.dataItem(row);
        var IsElementExist;
        if (IDwithData.length == 0) {
            IDwithData.push({
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
        $(IDwithData).each(function (i, data) {
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



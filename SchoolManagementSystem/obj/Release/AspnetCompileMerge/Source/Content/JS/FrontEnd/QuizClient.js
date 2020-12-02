var totalPages1 = Math.ceil(parseInt(totalQuizEntry) / NoofQuizPerPage);
var IsPageLoaded = 0;
var PageIndex;
var UserCurrentPage;
var IsSearched = 0;
var timer;
var hr;
var minute;
var second;
var timeFormat;
var Mandatory;
var duration;
var timeElpased = 1;
var tempSecond = "-1";
var tempcustomdata = "";
var tempmydata;
var FreeWrititngSkip = false;
var FreeWrititngTimeOut = false;

function onClose(e) {
    location.reload();
}
function onCloseMandatoryQuestion(e) {

}
$(document).ready(function () {
    InitiatePagination(totalPages1, 1);
    IsPageLoaded = 1;
    UIEvent();
    //$("div.sidebar.sidebar-main").hide();
    //$("div.pace.pace-inactive").hide();
    //$("div.navbar.navbar-default.header-highlight").hide();

    function InitiatePagination(totalPages, CurrentPage) {
        totalPages = (parseInt(totalPages));
        if (totalPages < 1) {

        }
        else {
            $.jqPaginator('#pagination1', {
                totalPages: totalPages,
                visiblePages: 3,
                currentPage: CurrentPage,
                onPageChange: function (num, type) {
                    if (IsPageLoaded == 1) {
                        GetQuizListing(num);
                    }
                }
            });
        }
    }
    function AddAntiForgeryToken(data) {
        data.__RequestVerificationToken = $('form.quiz-play-form input[name=__RequestVerificationToken]').val();
        return data;
    };
    function watchCountdown() {
        timer = $('div#defaultCountdown>span').children('span');
        hr = "";
        minute = "";
        second = "";
        timeFormat = "";
        $.each(timer, function (index, item) {
            if (index == 0) {
                hr = $(item).children('span.countdown-amount:eq(0)').html();
            }
            else if (index == 1) {
                minute = $(item).children('span.countdown-amount:eq(0)').html();
            }
            else if (index == 2) {
                second = $(item).children('span.countdown-amount:eq(0)').html();
            }
        });
        timeFormat += "<h5>";
        if (hr != "0") {
            timeFormat += "<span>";
            timeFormat += hr;
            timeFormat += "</span>hr";
        }
        if (minute != "0") {
            timeFormat += " <span>";
            timeFormat += minute;
            timeFormat += "</span>min ";
        }
        timeFormat += "<span>";
        timeFormat += second;
        timeFormat += "</span>sec";
        timeFormat += "</h5>";
        $("div.quiz-time").find("h5").remove();
        $("div.quiz-time").append(timeFormat);
        $("div.QuizTimerSection").show();
        if (parseInt(timeElpased) % 10 == 0 && parseInt(timeElpased) != 0) {
            tempcustomdata = $("form.quiz-play-form div.quiz-btn-list").find('.QuizPause').attr('my-data');
            tempmydata = {
                CustomData: tempcustomdata,
                TimeElapsed: timeElpased,
            };
            $.ajax({
                type: "post",
                dataType: "json",
                url: 'QuizClient/SetElapsedTime',
                async: true,
                data: AddAntiForgeryToken(tempmydata),
                success: function (data) {

                }
            });
        }
        if (second != tempSecond) {
            timeElpased = timeElpased + 1;
        }
        tempSecond = second;
        console.log(timeElpased);
    }
    function liftOff() {
        FreeWrititngTimeOut = true;
        Mandatory = false;
        $('form.quiz-play-form ul.QuizAnswerContainer>li:first>a').trigger('click');
        $('form.quiz-play-form div.quiz-btn-list').find('.QuizNext').trigger('click');
        timeElpased = 0;
        tempSecond = "-1";
        second = "";
    }
    function GetQuizListing(pageIndex) {
        var mydata = {
            SearchQuizTitle: $('form.quiz-play-form #QuizTitle').val(),
            PageIndex: pageIndex,
            SortBy: $("form.quiz-play-form #ddlSortBy :selected").val(),
        }
        $.ajax({
            type: "post",
            dataType: "json",
            url: 'QuizClient/QuizListingPagination',
            data: AddAntiForgeryToken(mydata),
            success: function (data) {
                if (data.totalCount > 0) {
                    $('form.quiz-play-form ul.quiz-model').empty();
                    $('form.quiz-play-form ul.quiz-model').append(data.renderString);
                    var totalQuiz = data.totalCount;
                    totalPages1 = Math.ceil(parseInt(totalQuiz) / NoofQuizPerPage);
                    if (IsSearched == 1) {
                        try {
                            $('form.quiz-play-form #pagination1').jqPaginator('option',
                                {
                                    totalPages: totalPages1,
                                    visiblePages: 3,
                                    currentPage: 1
                                });
                        } catch (e) {
                            InitiatePagination(totalPages1, 1)
                        }
                        IsSearched = 0;
                    }
                    $("div.page-container").css('min-height', $('body').height());
                    UIEvent();
                }
                else {
                    $('form.quiz-play-form ul.quiz-model').empty();
                    $('form.quiz-play-form #pagination1').empty();
                    $('form.quiz-play-form ul.quiz-model').append(data.renderString);
                }
            },
            beforeSend: function () {
                //loadingNow($('form.quiz-play-form.quiz-play-form.quiz-play-form'), true);
            },
            complete: function () {
                //loadingNow($('form.quiz-play-form.quiz-play-form.quiz-play-form'), false);
            },
            error: function () {
                //loadingNow($('form.quiz-play-form.quiz-play-form.quiz-play-form'), false);
            }
        });
    }
    function GetDetailsForQuiz(Data) {
        var mydata = {
            CustomData: Data
        };
        $.ajax({
            type: "post",
            dataType: "json",
            url: 'QuizClient/GetQuizInformation',
            data: AddAntiForgeryToken(mydata),
            success: function (data) {
                if (!$.isEmptyObject(data)) {
                    var ErrorJSONObject;
                    if (data.ErrorMessage.indexOf("[") != -1) {
                        ErrorJSONObject = JSON.parse('[' + data.ErrorMessage + ']');
                    }
                    else {
                        ErrorJSONObject = JSON.parse(data.ErrorMessage);
                    }
                    if (data.RenderContent == null) {
                        if (ErrorJSONObject.HasErrorOccurred == true) {
                            $("#popupWindow").data("kendoWindow").content(ErrorJSONObject.ErrorMessage);
                            $("#popupWindow").kendoWindow({
                                modal: true
                            });
                            $("#popupWindow").data("kendoWindow").center().open();
                        }
                        else {
                            $("#popupWindow").data("kendoWindow").center().open();
                            $("#popupWindow").kendoWindow({
                                modal: true
                            });
                        }
                        $(".QuizTimerSection").hide();
                    }
                    else {
                        $('form.quiz-play-form>div:not(#QuizEndedModal)').remove();
                        $('form.quiz-play-form').append(data.RenderContent);
                        $('body').addClass('quiz-start');
                        if (data.IsMultiple) {
                            $('form.quiz-play-form div.quiz-one ul.QuizAnswerContainer li').off().on('click', function (e) {
                                e.preventDefault();
                                var allOptions = $('form.quiz-play-form div.quiz-one ul.QuizAnswerContainer li');
                                if ($(this).hasClass('selected')) {
                                    $(this).removeClass('selected');
                                }
                                else {
                                    $(this).addClass('selected');
                                }
                            });
                        }
                        else {
                            $('form.quiz-play-form div.quiz-one ul.QuizAnswerContainer li').off().on('click', function (e) {
                                e.preventDefault();
                                var allOptions = $('form.quiz-play-form div.quiz-one ul.QuizAnswerContainer li');
                                $(allOptions).removeClass('selected');
                                $(this).addClass('selected');
                            });
                        }
                        $("div.page-container").css('min-height', $('body').height());
                        UIEvent();
                        duration = data.Duration;
                        Mandatory = data.Mandatory;
                        if (data.HasQuizStarted) {
                            timeElpased = parseInt(data.TimeElapsed);
                            $('form.quiz-play-form>div.row#QuizStartedModal').remove();
                            $('form.quiz-play-form>div.row#QuizQuestionAnswerModal').show();
                            $("#defaultCountdown").countdown({ until: '+' + data.Duration + 's', onTick: watchCountdown, onExpiry: liftOff });
                        }
                    }
                }
            }
            , beforeSend: function () {
                //loadingNow($('form.quiz-play-form.quiz-play-form.quiz-play-form'), true);
            },
            complete: function () {
                //loadingNow($('form.quiz-play-form.quiz-play-form.quiz-play-form'), false);
            }
            , error: function () {
                //loadingNow($('form.quiz-play-form.quiz-play-form.quiz-play-form'), false);
            }
        });
    }
    function ProceedQuiz(data, IsFW) {
        if (IsFW) {
            var mydata = {
                FreeWritingAnswer: data,
                IsFreeWriting: true,
                TimeElapsed: timeElpased,
                FreeWritingSkip: FreeWrititngSkip,
                FreeWritingTimeOut: FreeWrititngTimeOut,
            };
        }
        else {
            var mydata = {
                CustomData: data,
                IsFreeWriting: false,
                TimeElapsed: timeElpased,
            };
        }
        $.ajax({
            type: "post",
            dataType: "json",
            url: 'QuizClient/QuizProcced',
            data: AddAntiForgeryToken(mydata),
            success: function (data) {
                if (!$.isEmptyObject(data)) {
                    var ErrorJSONObject;
                    $('form.quiz-play-form>div:not(#QuizEndedModal)').remove();
                    $('form.quiz-play-form').append(data.RenderContent);
                    if (data.ErrorMessage.indexOf("[") != -1) {
                        ErrorJSONObject=JSON.parse('[' + data.ErrorMessage + ']');
                    }
                    else {
                       ErrorJSONObject=JSON.parse(data.ErrorMessage);
                    }
                    if (data.IsMultiple) {
                        $('form.quiz-play-form div.quiz-one ul.QuizAnswerContainer li').off().on('click', function (e) {
                            e.preventDefault();
                            var allOptions = $('form.quiz-play-form div.quiz-one ul.QuizAnswerContainer li');
                            if ($(this).hasClass('selected')) {
                                $(this).removeClass('selected');
                            }
                            else {
                                $(this).addClass('selected');
                            }
                        });
                    }
                    else {
                        $('form.quiz-play-form div.quiz-one ul.QuizAnswerContainer li').off().on('click', function (e) {
                            e.preventDefault();
                            var allOptions = $('form.quiz-play-form div.quiz-one ul.QuizAnswerContainer li');
                            $(allOptions).removeClass('selected');
                            $(this).addClass('selected');
                        });
                    }
                    if (data.RenderContent == null) {
                        if (ErrorJSONObject.HasErrorOccurred==true) {
                            $("#popupWindow").data("kendoWindow").content(ErrorJSONObject.ErrorMessage);
                            $("#popupWindow").kendoWindow({
                                modal: true
                            });
                            $("#popupWindow").data("kendoWindow").center().open();
                        }
                        else {
                            var QuizEndModel = $('form.quiz-play-form #QuizEndedModal');
                            if ($(QuizEndModel).length > 1) {
                                $.each(QuizEndModel, function (index, item) {
                                    if (index > 0) {
                                        $(item).remove();
                                    }
                                });
                            }
                            $(QuizEndModel).show();
                        }
                        $(".QuizTimerSection").hide();
                    }
                    $('#defaultCountdown').countdown('destroy');
                    duration = data.Duration;
                    Mandatory = data.Mandatory;
                    if (data.HasQuizStarted) {
                        $('form.quiz-play-form>div.row#QuizStartedModal').remove();
                        $('form.quiz-play-form>div.row:not(#QuizEndedModal):first').show();
                        timeElpased = parseInt(data.TimeElapsed);
                        $("#defaultCountdown").countdown({ until: '+' + data.Duration + 's', onTick: watchCountdown, onExpiry: liftOff });
                    }

                    UIEvent();
                }

            }
            , beforeSend: function () {
                //loadingNow($('form.quiz-play-form.quiz-play-form.quiz-play-form'), true);
            },
            complete: function () {
                //loadingNow($('form.quiz-play-form.quiz-play-form.quiz-play-form'), false);
            },
            error: function () {
                //loadingNow($('form.quiz-play-form.quiz-play-form.quiz-play-form'), false);
            }
        });
        FreeWrititngSkip = false;
        FreeWrititngTimeOut = false;

    }
    function QuizReport(data) {
        var mydata = {
            CustomData: data,
        };
        $.ajax({
            type: "post",
            dataType: "json",
            url: 'QuizClient/QuizReport',
            data: AddAntiForgeryToken(mydata),
            success: function (data) {
                if (!$.isEmptyObject(data)) {
                    $('body').addClass('quiz-start');
                    $('form.quiz-play-form>div:not(#QuizEndedModal)').remove();
                    $('form.quiz-play-form').append(data);
                    UIEvent();
                }
            }
            , beforeSend: function () {
                //loadingNow($('form.quiz-play-form.quiz-play-form.quiz-play-form'), true);
            },
            complete: function () {
                //loadingNow($('form.quiz-play-form.quiz-play-form.quiz-play-form'), false);
            },
            error: function () {
                //loadingNow($('form.quiz-play-form.quiz-play-form.quiz-play-form'), false);
            }
        });
    }
    function InitaiteQuiz(data) {
        var mydata = {
            CustomData: data,
        };
        $.ajax({
            type: "post",
            dataType: "json",
            url: 'QuizClient/QuizStart',
            data: AddAntiForgeryToken(mydata),
            success: function (data) {
            }
        });
    }

    function PauseQuiz(data) {
        var mydata = {
            CustomData: data,
            TimeElapsed: timeElpased,
        };
        $.ajax({
            type: "post",
            dataType: "json",
            url: 'QuizClient/PauseQuiz',
            data: AddAntiForgeryToken(mydata),
            success: function (data) {
                if (data) {
                    $('form.quiz-play-form div.quiz-btn-list').find('i.mdi-pause-circle-outline').removeClass('mdi-pause-circle-outline').addClass('mdi-play-circle-outline');
                }
                else {
                    $("#defaultCountdown").countdown('resume');
                    $('form.quiz-play-form div.quiz-btn-list').find('i.mdi-play-circle-outline').removeClass('mdi-play-circle-outline').addClass('mdi-pause-circle-outline');
                }
            }
            , beforeSend: function () {
                //loadingNow($('form.quiz-play-form.quiz-play-form.quiz-play-form'), true);
            },
            complete: function () {
                //loadingNow($('form.quiz-play-form.quiz-play-form.quiz-play-form'), false);
            },
            error: function () {
                //loadingNow($('form.quiz-play-form.quiz-play-form.quiz-play-form'), false);
            }
        });
    }

    function SetElapsedTime(data) {
        var mydata = {
            CustomData: data,
            TimeElapsed: timeElpased,
        };
        $.ajax({
            type: "post",
            dataType: "json",
            url: 'QuizClient/SetElapsedTime',
            data: AddAntiForgeryToken(mydata),
            success: function (data) {

            }
        });
    }
    function PreviousQuestion(data) {
        var mydata = {
            CustomData: data,
        };
        $.ajax({
            type: "post",
            dataType: "json",
            url: 'QuizClient/GetPreviousQuestion',
            data: AddAntiForgeryToken(mydata),
            success: function (data) {
                if (!$.isEmptyObject(data)) {
                    $('#previousEntry>div.row').remove();
                    $('#previousEntry').append(data);
                    $('form.quiz-play-form div#QuizQuestionAnswerModal').hide();
                    UIEvent();
                }
            }
        });
    }

    function NextQuestion(data) {
        var mydata = {
            CustomData: data,
        };
        $.ajax({
            type: "post",
            dataType: "json",
            url: 'QuizClient/GetNextQuestion',
            data: AddAntiForgeryToken(mydata),
            success: function (data) {
                if (!$.isEmptyObject(data)) {
                    $('#previousEntry>div.row').remove();
                    $('#previousEntry').append(data);
                    UIEvent();
                }
                else {
                    $('#previousEntry>div.row').remove();
                    $('form.quiz-play-form div#QuizQuestionAnswerModal').show();
                }
            },
            error: function () {
            }
        });
    }

    function UIEvent() {
        $('form.quiz-play-form #btnQuizSearch').off().on('click', function (e) {
            e.preventDefault();
            UserCurrentPage = 1;
            IsSearched = 1;
            $("form.quiz-play-form select#ddlSortBy")[0].selectedIndex = 0
            GetQuizListing(1);
        });
        $('form.quiz-play-form #ddlSortBy').off().on('change', function (e) {
            e.preventDefault();
            UserCurrentPage = 1;
            IsSearched = 1;
            GetQuizListing(1);
        });
        $('form.quiz-play-form ul.quiz-model>li>a.QuizPlay').off().on('click', function (e) {
            e.preventDefault();
            var mydata = $(this).attr('data-val');
            var getClass = $(this).parent().prop('class');
            if (getClass == "first100") {
                var dataArry = [];
                dataArry.push($(this).attr('data-val'));
                QuizReport(dataArry);
            }
            else {
                GetDetailsForQuiz(mydata);
            }

        });
        $('form.quiz-play-form div.quiz-button #btnStartClientQuiz').off().on('click', function (e) {
            e.preventDefault();
            //$('form.quiz-play-form').children('div:first').hide();
            //$('form.quiz-play-form').children('div:last').show();
            $("#QuizStartedModal").hide();
            $("#QuizQuestionAnswerModal").show();
            $("#QuizEndedModal").hide();
            var dataArry = [];
            dataArry.push($(this).attr('my-data'));
            InitaiteQuiz(dataArry);
            if (parseInt(duration) > 0) {
                timeElpased = 0;
                $("#defaultCountdown").countdown({ until: '+' + duration + 's', onTick: watchCountdown, onExpiry: liftOff });
            }
        });
        $("form.quiz-play-form #QuizEndedModal #btnFinishClientQuiz").off().on('click', function (e) {
            e.preventDefault();
            var temp = $(this).attr('mydata');
            if (temp == undefined || temp == "") {
                location.reload();
            }
            else {
                var dataArry = [];
                dataArry.push(temp);
                QuizReport(dataArry);
                $("form.quiz-play-form #QuizEndedModal").hide();
            }
        });
        $('form.quiz-play-form div.quiz-btn-list').find('a.QuizPause').off().on('click', function (e) {
            e.preventDefault();
            $("#defaultCountdown").countdown('pause');
            var temp = $(this).attr('my-data');
            var dataArry = [];
            dataArry.push(temp);
            PauseQuiz(dataArry);
        });
        $("form.quiz-play-form #QuizEndedModal #btnFinishClientQuiz").off().on('click', function (e) {
            e.preventDefault();
            var temp = $(this).attr('mydata');
            if (temp == undefined || temp == "") {
                location.reload();
            }
            else {
                var dataArry = [];
                dataArry.push(temp);
                QuizReport(dataArry);
                $("form.quiz-play-form #QuizEndedModal").hide();
            }
        });

        $('form.quiz-play-form div.quiz-btn-list a.QuizNext').off().on('click', function (e) {
            e.preventDefault();
            var IsFreeWriting = $('form.quiz-play-form div.quiz-one-mid').find('textarea');
            console.log(IsFreeWriting.length);
            var FreeWritingAnswer = $.trim($(IsFreeWriting).val());
            if (IsFreeWriting.length > 0) {
                if (Mandatory) {
                    if (FreeWritingAnswer.length > 0) {
                        ProceedQuiz(IsFreeWriting.attr('data-val') + ',' + FreeWritingAnswer, true);
                    }
                    else {
                        $("#PopUpQuestionMandatory").data("kendoWindow").center().open();
                        $("#PopUpQuestionMandatory").kendoWindow({
                            modal: true
                        });
                    }
                }
                else {
                    if (FreeWritingAnswer.length < 1) {
                        FreeWrititngSkip = true;
                    }
                    ProceedQuiz(IsFreeWriting.attr('data-val') + ',' + FreeWritingAnswer, true);
                }
            }
            else {
                var ClassAction = $(this).prop('class');
                var Selected = $('form.quiz-play-form ul.QuizAnswerContainer li.selected').find('a');
                var SelectedValues;
                if (Mandatory) {
                    if (Selected.length > 0) {
                        if (ClassAction.indexOf("QuizNext") > -1) {
                            var dataArry = [];
                            $(Selected).each(function (index, item) {
                                dataArry.push($(item).prop('id'));
                            });

                            ProceedQuiz(dataArry, false);
                        }
                    }
                    else {
                        $("#PopUpQuestionMandatory").data("kendoWindow").center().open();
                        $("#PopUpQuestionMandatory").kendoWindow({
                            modal: true
                        });
                    }
                }
                else {
                    if (Selected.length > 0) {
                        if (ClassAction.indexOf("QuizNext") > -1) {
                            var dataArry = [];
                            $(Selected).each(function (index, item) {
                                dataArry.push($(item).prop('id'));
                            });
                            ProceedQuiz(dataArry, false);
                        }
                    }
                    else {
                        $('form.quiz-play-form ul.QuizAnswerContainer>li:first').next().children('a').trigger('click');
                        Selected = $('form.quiz-play-form ul.QuizAnswerContainer li.selected').find('a');
                        if (ClassAction.indexOf("QuizNext") > -1) {
                            var dataArry = [];
                            $(Selected).each(function (index, item) {
                                dataArry.push($(item).prop('id'));
                            });
                            ProceedQuiz(dataArry, false);
                        }
                    }
                }
            }
        });

        $('form.quiz-play-form div.quiz-one-last a.QuizBack').off().on('click', function (e) {
            e.preventDefault();
            var temp = $(this).attr('my-data');
            var dataArry = [];
            dataArry.push(temp);
            PreviousQuestion(dataArry);
        });

        $('#previousEntry a.QuizBack').off().on('click', function (e) {
            e.preventDefault();
            var temp = $(this).attr('my-data');
            var dataArry = [];
            dataArry.push(temp);
            PreviousQuestion(dataArry);
        });
        $('#previousEntry a.QuizNext').off().on('click', function (e) {
            e.preventDefault();
            var temp = $(this).attr('my-data');
            var dataArry = [];
            dataArry.push(temp);
            NextQuestion(dataArry);
        });
        $("#QuizTitle").off().on('keypress', function (e) {
            if (e.which === 13) {
                $('#btnQuizSearch').trigger('click');
                e.preventDefault();
            }
        });

        $("form.quiz-play-form a.QuizReportClose").off().on('click', function (e) {
            e.preventDefault();
            location.reload();
        });

        $("div.QuizTimerSection a.QuizClose").off().on('click', function (e) {
            e.preventDefault();
            location.reload();
        });

        $("#popupWindow #Yes").off().on('click', function (e) {
            e.preventDefault();
            location.reload();
        })

        $("#PopUpQuestionMandatory #ConfirmMandatoryquestion").off().on('click', function (e) {
            e.preventDefault();
            $("#PopUpQuestionMandatory").data("kendoWindow").close();
        });

        $("div#QuizTabMenu>h6>a").off().on('click', function (e) {
            e.preventDefault();
            var data = $(this).attr('my-data');
            $("div#QuizTabMenu>h6>a").removeClass('active');
            $("select#ddlSortBy").prop("selected", false);
            $("select#ddlSortBy option").each(function () {
                if ($(this).text() == data) {
                    $(this).prop('selected', 'selected').trigger('change');
                }
            });
            $(this).addClass('active');
        });

       
    }
})
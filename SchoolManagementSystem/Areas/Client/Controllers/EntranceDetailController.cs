using DomainEntities;
using DomainInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace SchoolManagementSystem.Areas.Client.Controllers
{
    public class EntranceDetailController : Controller
    {
        private readonly IEntranceRepository _EntranceRepo;
        private readonly IEntranceClientRepository _EntranceClientRepo;
        //private readonly IStatusRepository _statusRepo;
        //private readonly IMetaDataRepository _metaDataRepo;


        public EntranceDetailController(IEntranceRepository EntranceRepo, IEntranceClientRepository EntranceClientRepo)//, IStatusRepository statusRepo, IMetaDataRepository metaDataRepo)
        {
            this._EntranceRepo = EntranceRepo;
            this._EntranceClientRepo = EntranceClientRepo;
            //this._statusRepo = statusRepo;
            //this._metaDataRepo = metaDataRepo;
        }
        //[AuthorizeUser(Controls = "View")]
        public ActionResult Index(string EntranceSlug)
        {
            //await PageHitHelper.UpdatePageHit(System.Web.HttpContext.Current, PageName.quizDetail);

            string UserName = new LoginUser().UserName;
            string html = "";
            EntranceClientSide objInfo = _EntranceClientRepo.GetEntranceDetailsFromSlug(EntranceSlug, UserName);
            //  EntranceClientSide objInfo2 = _EntranceClientRepo.GetQuizProgress(UserName);
            if (objInfo != null)
            {
                // LoadMetaData(CategoryType.CategoryQuiz, objInfo.EntranceID);
                if (objInfo.CanShowAllQuestions)
                {
                    // string html = "";
                    html += "<ol class='all-question'>";
                    IEnumerable<string> QuestionList = _EntranceClientRepo.GetAllEntranceQuestion(objInfo.EntranceID);
                    foreach (string item in QuestionList)
                    {
                        html += "<li>" + item + "</li>";

                    }
                    html += "</ol>";
                    ViewBag.Questions = html;
                }
                ViewBag.QuizDescription = objInfo.EntranceDescription;
                ViewBag.QuizImage = objInfo.EntranceImage == null ? "" : objInfo.EntranceImage;
                ViewBag.EntranceTitle = objInfo.EntranceTitle;
                if (objInfo.IsCompleted)
                {
                    ViewBag.QuizEncryptData = Crypto.Encrypt(objInfo.EntranceID.ToString());
                }
                else
                {
                    ViewBag.QuizEncryptData = Crypto.Encrypt(objInfo.EntranceID.ToString()) + "," + Crypto.Encrypt("0");
                }
                ViewBag.IsQuizCompleted = objInfo.IsCompleted;
                ViewBag.CanShowAllQuestion = objInfo.CanShowAllQuestions;

                html = "";
                html += "<h4><strong class='txt-purple'>";
                html += Math.Round(((float)objInfo.TotalEntranceAnswered / (float)objInfo.TotalQuestionInEntrance) * 100.0, 2).ToString();
                html += "%</strong> completed (<strong class='txt-purple'>";
                html += objInfo.TotalEntranceAnswered.ToString() + "/" + objInfo.TotalQuestionInEntrance.ToString();
                html += "</strong> question)</h4>";
                ViewBag.UserProgress = html;
            }
            else
            {
                return RedirectToAction("Index", "PageNotFound", new { Area = "Client" });
            }
            return View();
        }
        [ValidateAntiForgeryToken]
        public ActionResult GetEntranceInformation(EntranceStartInfo objInfo)
        {
            string html = "";
            string encryptEntranceID;
            string encryptQuestionID;
            char[] alpha = "#ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
            string[] encryptData;
            string PercentageValue;
            string ErrorMessage = "[]";
            bool IsMultipleChoice = false;
            bool HasEntranceStarted = false;
            int QuestionDuration;
            bool IsMandatory = false;
            int TimeElapsed = 0;
            int i = 0;
            foreach (string item in objInfo.CustomData)
            {
                encryptData = item.Split(',');
                objInfo.EntranceID = Crypto.Decrypt(encryptData[0]);
                objInfo.QuestionID = Crypto.Decrypt(encryptData[1]);
                i++;
            }
            encryptEntranceID = Crypto.Encrypt(objInfo.EntranceID);
            objInfo.UserName = new LoginUser().UserName;
            EntranceStartInfo list = _EntranceClientRepo.GetEntranceQuestionAndAnswer(int.Parse(objInfo.EntranceID), int.Parse(objInfo.QuestionID), objInfo.UserName,objInfo.Identifier);
            if (list != null)
            {
                encryptQuestionID = Crypto.Encrypt(list.QuestionID);
                if (!list.HasEntranceStarted)
                {
                    html += "<div class='row mtp-sm' id='QuizStartedModal'><div class='col-lg-10 col-md-10 col-sm-10 col-xs-10 quiz-start-mg'>";
                    html += "<div class='quiz-start-top'><h2>Are you Ready?</h2>";
                    html += "<div class='quiz-button'><a href ='#' id='btnStartClientQuiz' my-data='" + encryptQuestionID + "," + encryptEntranceID + "'>";
                    html += "<span>Start Now<i class='mdi mdi-play-circle-outline'></i> ";
                    html += "</span></a></div>";
                    html += "</div>";
                    html += "<div class='quiz-start-bottom'>";
                    html += "<p>";
                    html += list.StartPageDescription;
                    html += "</div></div></div>";
                }
                html += "<div class='row' style='display:none' id='QuizQuestionAnswerModal'><div class='col-lg-10 col-md-10 col-sm-10 col-xs-10 quiz-one'>";
                html += "<div class='quiz-one-first'><span>";
                html += "<i class='mdi mdi-circle-outline'></i>";
                html += list.EntranceTitle;
                html += "</span></div><div class='quiz-one-mid'>";
                html += "<h3>";
                html += list.EntranceQuestion;
                html += "</h3>";
                if (list.IsFreeWriting == false)
                {
                    html += "<ul class='QuizAnswerContainer'>";
                    #region Time Out Default Option
                    html += "<li  style='display:none'><a href='#' id='";
                    //-1 for time out
                    html += Crypto.Encrypt("-1");
                    html += ",";
                    html += encryptQuestionID;
                    html += ",";
                    html += encryptEntranceID;
                    html += "'><span>";
                    html += "</span>";
                    html += "</a></li>";
                    #endregion

                    #region Skip Default Default Option
                    html += "<li style='display:none'><a href='#' id='";
                    //-2 for User skip Answer
                    html += Crypto.Encrypt("-2");
                    html += ",";
                    html += encryptQuestionID;
                    html += ",";
                    html += encryptEntranceID;
                    html += "'><span>";
                    html += "</span>";
                    html += "</a></li>";
                    #endregion
                    int counter = 1;
                    foreach (EntranceAnswerInfo item in list.EntranceAnswerList)
                    {
                        html += "<li class=''><a href='#' id='";
                        html += Crypto.Encrypt(item.AnswerPoolID.ToString());
                        html += ",";
                        html += encryptQuestionID;
                        html += ",";
                        html += encryptEntranceID;
                        html += "'><span>";
                        html += alpha[counter];
                        html += "</span>";
                        html += item.EntranceOption;
                        html += "</a></li>";
                        counter++;
                    }
                    html += "</ul>";
                }
                else
                {
                    html += "<div class='form-group'><textarea style='width:90%' rows='3' class='form-control' data-val='" + encryptQuestionID + "," + encryptEntranceID + "'></textarea></div>";
                }
                html += "</div>";
                html += "<div class='quiz-one-last'><div class='row'><div class='quiz-btn-list'>";
                if (list.CanSeePreviousAnswer && !list.IsFirst)
                {
                    html += "<span><a href='#' class='QuizBack btn btn-outline-info quiz-buttons' my-data='" + Crypto.Encrypt("0") + "," + Crypto.Encrypt(objInfo.EntranceID) + "'><span class='mdi mdi-chevron-left'></span>Prev</a></span>";
                }
                else
                {
                    html += "<span style='display:none'><a href='#' class='QuizBack btn btn-outline-info quiz-buttons' my-data='" + Crypto.Encrypt("0") + "," + Crypto.Encrypt(objInfo.EntranceID) + "'><span class='mdi mdi-chevron-left'></span>Prev</a></span>";
                }
                if (list.IsPauseAllowed)
                {
                    html += "<span><a href='#' class='QuizPause' my-data='" + encryptQuestionID + "," + encryptEntranceID + "'><i class='mdi mdi-pause-circle-outline'></i></a></span>";
                }
                else
                {
                    html += "<span style='display:none'><a href='#' class='QuizPause' my-data='" + encryptQuestionID + "," + encryptEntranceID + "'><i class='mdi mdi-pause-circle-outline'></i></a></span>";
                }
                html += "<span><a href='#' class='QuizNext btn btn-outline-info quiz-buttons'>Next<span class='mdi mdi-chevron-right'></span></a></span>";
                html += "</div>";
                html += "<div class='quiz-play'>";
                html += "<h5>";
                html += "Question ";
                PercentageValue = Math.Ceiling(((float)list.RowNum / (float)list.TotalQuestion) * 100.0).ToString();
                html += list.RowNum + " out of " + list.TotalQuestion;
                html += "</h5>";
                html += "<div class='quiz-line'><span style='width:" + PercentageValue + "%'></span></div></div>";
                html += "</div></div></div></div>";
                //if (list.IsLast)
                //{
                html += "<div class='row' style='display:none' id='QuizEndedModal'><div class='col-lg-10 col-md-10 col-sm-10 col-xs-10 quiz-start-mg'>";
                html += "<div class='quiz-start-top'><h2>Quiz Completed</h2>";
                html += "<div class='quiz-button'><a href ='#' id='btnFinishClientQuiz' mydata='" + encryptEntranceID + "' title='Quiz Report'>";
                html += "<span>Finish<i class='mdi mdi-play-circle-outline' display='none'></i> ";
                html += "</span></a></div>";
                html += "</div>";
                html += "<div class='quiz-start-bottom'>";
                html += "<p>";
                html += list.EndPageDescription;
                html += "</div></div></div>";
                //}
                IsMultipleChoice = list.IsMultipleChoice;
                QuestionDuration = list.Duration;
                IsMandatory = list.IsMandatory;
                HasEntranceStarted = list.HasEntranceStarted;
                TimeElapsed = list.TimeElapsed;
                list.UserName = new LoginUser().UserName;
                list.EntranceID = objInfo.EntranceID;
                //if (list.QuestionID != "0")
                //{
                //    _EntranceClientRepo.StartQuiz(list);
                //}
            }
            else
            {
                html = null;
                IsMultipleChoice = false;
                ErrorMessage = _EntranceClientRepo.GetErrorMessage(int.Parse(objInfo.EntranceID), int.Parse(objInfo.QuestionID), objInfo.UserName, objInfo.Identifier);
                QuestionDuration = -1;
            }
            return Json(new { RenderContent = html, IsMultiple = IsMultipleChoice, Duration = QuestionDuration, Mandatory = IsMandatory, HasEntranceStarted = HasEntranceStarted, TimeElapsed = TimeElapsed, ErrorMessage = ErrorMessage });
        }
        [ValidateAntiForgeryToken]
        public ActionResult EntranceProcced(EntranceStartInfo objInfo)
        {
            string html = "";
            string encryptEntranceID;
            string encryptQuestionID;
            string[] encryptData;
            string PercentageValue;
            string ErrorMessage = "[]";
            bool IsMultipleChoice = false;
            int QuestionDuration;
            bool IsMandatory = false;
            bool HasEntranceStarted = false;
            int TimeElapsed = 0;
            char[] alpha = "#ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
            if (objInfo.IsFreeWriting == false)
            {
                List<string> temporyValue = new List<string>(); ;
                foreach (string item in objInfo.CustomData)
                {
                    encryptData = item.Split(',');
                    var t = encryptData[2];
                    objInfo.EntranceID = Crypto.Decrypt(t);
                    objInfo.QuestionID = Crypto.Decrypt(encryptData[1]);
                    temporyValue.Add(Crypto.Decrypt(encryptData[0]));
                }
                objInfo.AnswerID = temporyValue;
            }
            else
            {
                encryptData = objInfo.FreeWritingAnswer.Split(',');
                objInfo.EntranceID = Crypto.Decrypt(encryptData[1]);
                objInfo.QuestionID = Crypto.Decrypt(encryptData[0]);
                Regex rRemScript = new Regex(@"<script[^>]*>[\s\S]*?</script>");
                objInfo.FreeWritingAnswer = rRemScript.Replace(encryptData[2], ""); ;
            }
            encryptEntranceID = Crypto.Encrypt(objInfo.EntranceID);
            objInfo.UserName = new LoginUser().UserName;
            bool status = _EntranceClientRepo.IsAnswerCorrect(objInfo);
            EntranceStartInfo list = _EntranceClientRepo.GetEntranceQuestionAndAnswer(int.Parse(objInfo.EntranceID), int.Parse(objInfo.QuestionID), objInfo.UserName,objInfo.Identifier);
            if (list != null)
            {
                encryptQuestionID = Crypto.Encrypt(list.QuestionID);
                if (!list.HasEntranceStarted)
                {
                    html += "<div class='row mtp-sm' id='QuizStartedModal'><div class='col-lg-10 col-md-10 col-sm-10 col-xs-10 quiz-start-mg'>";
                    html += "<div class='quiz-start-top'><h2>Are you Ready?</h2>";
                    html += "<div class='quiz-button'><a href ='#' id='btnStartClientQuiz' my-data='" + encryptQuestionID + "," + encryptEntranceID + "'>";
                    html += "<span>Start Now<i class='mdi mdi-play-circle-outline'></i> ";
                    html += "</span></a></div>";
                    html += "</div>";
                    html += "<div class='quiz-start-bottom'>";
                    html += "<p>";
                    html += list.StartPageDescription;
                    html += "</div></div></div>";
                }
                html += "<div class='row' id='QuizQuestionAnswerModal'><div class='col-lg-10 col-md-10 col-sm-10 col-xs-10 quiz-one'>";
                html += "<div class='quiz-one-first'><span>";
                html += "<i class='mdi mdi-circle-outline'></i>";
                html += list.EntranceTitle;
                html += "</span></div><div class='quiz-one-mid'>";
                html += "<h3>";
                html += list.EntranceQuestion;
                html += "</h3>";
                if (list.IsFreeWriting == false)
                {
                    html += "<ul class='QuizAnswerContainer'>";
                    html += "<li class='ExpiryTime' style='display:none'><a href='#' id='";
                    //-1 for time out
                    html += Crypto.Encrypt("-1");
                    html += ",";
                    html += encryptQuestionID;
                    html += ",";
                    html += encryptEntranceID;
                    html += "'><span>";
                    html += "</span>";
                    html += "</a></li>";
                    html += "<li class='ExpiryTime' style='display:none'><a href='#' id='";
                    //-2 for User skip Answer
                    html += Crypto.Encrypt("-2");
                    html += ",";
                    html += encryptQuestionID;
                    html += ",";
                    html += encryptEntranceID;
                    html += "'><span>";
                    html += "</span>";
                    html += "</a></li>";
                    int counter = 1;
                    foreach (EntranceAnswerInfo item in list.EntranceAnswerList)
                    {
                        html += "<li class=''><a href='#' id='";
                        html += Crypto.Encrypt(item.AnswerPoolID.ToString());
                        html += ",";
                        html += encryptQuestionID;
                        html += ",";
                        html += encryptEntranceID;
                        html += "'><span>";
                        html += alpha[counter];
                        html += "</span>";
                        html += item.EntranceOption;
                        html += "</a></li>";
                        counter++;
                    }
                    html += "</ul>";
                }
                else
                {
                    html += "<div class='form-group'><textarea style='width:90%' rows='3' class='form-control' data-val='" + encryptQuestionID + "," + encryptEntranceID + "'></textarea></div>";
                }
                html += "</div>";
                html += "<div class='quiz-one-last'><div class='row'><div class='quiz-btn-list'>";
                if (list.CanSeePreviousAnswer)
                {
                    html += "<span><a href='#' class='QuizBack btn btn-outline-info quiz-buttons' my-data='" + Crypto.Encrypt("0") + "," + encryptEntranceID + "'><span class='mdi mdi-chevron-left'></span>Prev</a></span>";
                }
                if (list.IsPauseAllowed)
                {
                    html += "<span><a href='#' class='QuizPause' my-data='" + encryptQuestionID + "," + encryptEntranceID + "'><i class='mdi mdi-pause-circle-outline'></i></a></span>";
                }
                else
                {
                    html += "<span style='display:none'><a href='#' class='QuizPause' my-data='" + encryptQuestionID + "," + encryptEntranceID + "'><i class='mdi mdi-pause-circle-outline'></i></a></span>";
                }
                html += "<span><a href='#' class='QuizNext btn btn-outline-info quiz-buttons'>Next<span class='mdi mdi-chevron-right'></span></a></span>";
                html += "</div>";
                html += "<div class='quiz-play'>";
                html += "<h5>";
                html += "Question ";
                PercentageValue = Math.Ceiling(((float)list.RowNum / (float)list.TotalQuestion) * 100.0).ToString();
                html += list.RowNum + " out of " + list.TotalQuestion;
                html += "</h5>";
                html += "<div class='quiz-line'><span style='width:" + PercentageValue + "%'></span></div></div>";
                html += "</div></div></div></div>";
                //if (list.IsLast)
                //{
                html += "<div class='row' style='display:none' id='QuizEndedModal'><div class='col-lg-10 col-md-10 col-sm-10 col-xs-10 quiz-start-mg'>";
                html += "<div class='quiz-start-top'><h2>Quiz Completed</h2>";
                html += "<div class='quiz-button'><a href ='#' id='btnFinishClientQuiz' mydata='" + encryptEntranceID + "' title='Quiz Report'>";
                html += "<span>Finish<i class='mdi mdi-play-circle-outline' display='none'></i> ";
                html += "</span></a></div>";
                html += "</div>";
                html += "<div class='quiz-start-bottom'>";
                html += "<p>";
                html += list.EndPageDescription;
                html += "</div></div></div>";
                //}
                IsMultipleChoice = list.IsMultipleChoice;
                QuestionDuration = list.Duration;
                IsMandatory = list.IsMandatory;
                HasEntranceStarted = list.HasEntranceStarted;
                list.UserName = new LoginUser().UserName;
                list.EntranceID = objInfo.EntranceID;
                //if (list.QuestionID != "0")
                //{
                //    _EntranceClientRepo.StartQuiz(list);
                //}
                TimeElapsed = list.TimeElapsed;
            }
            else
            {
                html = null;
                ErrorMessage = _EntranceClientRepo.GetErrorMessage(int.Parse(objInfo.EntranceID), int.Parse(objInfo.QuestionID), objInfo.UserName, objInfo.Identifier);
                QuestionDuration = -1;
            }
            return Json(new { RenderContent = html, IsMultiple = IsMultipleChoice, Duration = QuestionDuration, Mandatory = IsMandatory, HasEntranceStarted = HasEntranceStarted, TimeElapsed = TimeElapsed, ErrorMessage = ErrorMessage });
        }
        [ValidateAntiForgeryToken]
        public ActionResult EntranceReport(EntranceStartInfo objInfo)
        {
            string html = "";
            objInfo.EntranceID = Crypto.Decrypt(objInfo.CustomData[0]);
            objInfo.UserName = new LoginUser().UserName;
            string[] answer;
            int PercentageValue;
            string tempAnswer;
            IEnumerable<EntranceCompletionReport> ReportLst = _EntranceClientRepo.GetEntranceReport(int.Parse(objInfo.EntranceID), objInfo.UserName,objInfo.Identifier);
            PercentageValue = int.Parse(Math.Ceiling(((float)ReportLst.FirstOrDefault().CorrectAnswerCount / (float)ReportLst.FirstOrDefault().TotalQuestion) * 100.0).ToString());
            html += "<div class='row mtp-sm'><div class='col-lg-10 col-md-10 col-sm-10 col-xs-10'>";
            html += "<div class='quiz-start-top quiz-report-top quiz-report-tab2 quiz-start-mg'>";
            html += "<a href='#' class='close-button close-button-question QuizReportClose'>✖</a>";
            html += "<div class='pull-left'>";
            html += "<img src ='";
            html += new LoginUser().UserImage;
            html += "'>";
            html += "<h5>";
            html += new LoginUser().UserName;
            html += "</h5></div>";
            html += "<ul>";
            if (PercentageValue == 0)
            {
                html += "<li><i class='mdi mdi-star-outline'></i></li>";
                html += "<li><i class='mdi mdi-star-outline'></i></li>";
                html += "<li><i class='mdi mdi-star-outline'></i></li>";
            }
            else if (PercentageValue > 0 && PercentageValue <= 34)
            {
                html += "<li class='star-color-rate'><i class='mdi mdi-star'></i></li>";
                html += "<li><i class='mdi mdi-star-outline'></i></li>";
                html += "<li><i class='mdi mdi-star-outline'></i></li>";
            }
            else if (PercentageValue > 34 && PercentageValue < 100)
            {
                html += "<li class='star-color-rate'><i class='mdi mdi-star'></i></li>";
                html += "<li class='star-color-rate'><i class='mdi mdi-star'></i></li>";
                html += "<li><i class='mdi mdi-star-outline'></i></li>";
            }
            else
            {
                html += "<li class='star-color-rate'><i class='mdi mdi-star'></i></li>";
                html += "<li class='star-color-rate'><i class='mdi mdi-star'></i></li>";
                html += "<li class='star-color-rate'><i class='mdi mdi-star'></i></li>";
            }
            html += "</ul>";
            html += "<h4 class='out-of'><span>";
            html += ReportLst.FirstOrDefault().CorrectAnswerCount;
            html += "</span> out of ";
            html += ReportLst.FirstOrDefault().TotalQuestion;
            html += "</h4>";
            html += "<div class='quiz-button quiz-report-button quiz-button-tab2'>";
            html += "<a href ='#'>";
            html += "<span>";
            html += "Well Done !";
            html += "</span></a></div>";
            html += "</div>";
            html += "<div class='quiz-start-bottom quiz-report-bottom'>";
            foreach (EntranceCompletionReport item in ReportLst)
            {
                html += "<div class='quest-bottom pull-left'>";
                html += "<h4>";
                html += item.EntranceQuestion;
                html += "</h4>";
                html += "<span></span>";
                if (item.IsCorrect)
                {
                    if (item.UserAnswer.Contains("`"))
                    {
                        answer = item.UserAnswer.Split('`');
                        foreach (string item1 in answer)
                        {
                            html += "<h5><i class='mdi mdi-checkbox-marked-circle-outline pull-left'></i>";
                            html += item1;
                            html += "</h5>";
                        }
                    }
                    else
                    {
                        html += "<h5><i class='mdi mdi mdi-checkbox-marked-circle-outline pull-left'></i>";
                        html += item.UserAnswer;
                        html += "</h5>";
                    }
                }
                else
                {
                    if (item.UserAnswer.Contains("`"))
                    {
                        answer = item.UserAnswer.Split('`');
                        foreach (string item1 in answer)
                        {
                            html += "<h6><i class='mdi mdi mdi-close-circle-outline pull-left'></i>";
                            html += item1;
                            html += "</h6>";
                        }
                    }
                    else
                    {
                        html += "<h6><i class='mdi mdi mdi-close-circle-outline pull-left'></i>";
                        tempAnswer = item.UserAnswer;
                        if (item.IsSkipped)
                        {
                            tempAnswer = "(This Answer was Skipped)";
                        }
                        if (item.IsTimeOut)
                        {
                            tempAnswer = "(Not Answered but of TimeOut)";
                        }
                        if (item.IsEntranceExpired)
                        {
                            tempAnswer = "(Not Answered because Entrance Expired)";
                        }
                        html += tempAnswer;
                        tempAnswer = "";
                        html += "</h6>";
                    }
                }
                if (!item.IsCorrect)
                {
                    if (item.CorrectAnswer.Contains("`"))
                    {
                        answer = item.CorrectAnswer.Split('`');
                        foreach (string item1 in answer)
                        {
                            html += "<h5><i class='mdi mdi mdi-checkbox-marked-circle-outline pull-left'></i>";
                            html += item1;
                            html += "</h5>";
                        }
                    }
                    else
                    {
                        if (item.IsAnswerApproved)
                        {
                            html += "<h5><i class='mdi mdi mdi-checkbox-marked-circle-outline pull-left'></i>";
                            html += item.CorrectAnswer;
                            html += "</h5>";
                        }
                    }
                }
                html += "</div>";
            }
            html += "</div></div></div>";
            return Json(html);
        }
        [ValidateAntiForgeryToken]
        public ActionResult EntranceStart(EntranceStartInfo objInfo)
        {
            string[] encryptData;
            foreach (string item in objInfo.CustomData)
            {
                encryptData = item.Split(',');
                string t = encryptData[1];
                objInfo.EntranceID = Crypto.Decrypt(t);
                objInfo.QuestionID = Crypto.Decrypt(encryptData[0]);
            }
            objInfo.UserName = new LoginUser().UserName;
            _EntranceClientRepo.StartEntrance(objInfo);
            return Json(null);
        }

        [ValidateAntiForgeryToken]
        public ActionResult PauseEntrance(EntranceStartInfo objInfo)
        {
            string[] encryptData;
            foreach (string item in objInfo.CustomData)
            {
                encryptData = item.Split(',');
                var t = encryptData[1];
                objInfo.EntranceID = Crypto.Decrypt(t);
                objInfo.QuestionID = Crypto.Decrypt(encryptData[0]);
            }
            objInfo.UserName = new LoginUser().UserName;
            return Json(_EntranceClientRepo.IsPaused(objInfo));
        }

        [ValidateAntiForgeryToken]
        public ActionResult SetElapsedTime(EntranceStartInfo objInfo)
        {
            string[] encryptData;
            foreach (string item in objInfo.CustomData)
            {
                encryptData = item.Split(',');
                var t = encryptData[1];
                objInfo.EntranceID = Crypto.Decrypt(t);
                objInfo.QuestionID = Crypto.Decrypt(encryptData[0]);
            }
            objInfo.UserName = new LoginUser().UserName;
            _EntranceClientRepo.SetTimeElapsed(objInfo);
            return Json(null);
        }
        [ValidateAntiForgeryToken]
        public ActionResult GetPreviousQuestion(EntranceStartInfo objInfo)
        {
            char[] alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
            string[] encryptData;
            string html = null;
            string PercentageValue;
            foreach (string item in objInfo.CustomData)
            {
                encryptData = item.Split(',');
                var t = encryptData[1];
                objInfo.EntranceID = Crypto.Decrypt(t);
                objInfo.QuestionID = Crypto.Decrypt(encryptData[0]);
            }
            objInfo.UserName = new LoginUser().UserName;
            EntranceStartInfo list = _EntranceClientRepo.GetPreviousQuestion(objInfo);
            if (list != null)
            {
                html += "<div class='row'><div class='col-lg-10 col-md-10 col-sm-10 col-xs-10 quiz-one'>";
                html += "<div class='quiz-one-first'><span>";
                html += "<i class='mdi mdi-circle-outline'></i>";
                html += list.EntranceTitle;
                html += "</span></div><div class='quiz-one-mid'>";
                html += "<h3>";
                html += list.EntranceQuestion;
                html += "</h3>";
                if (list.IsFreeWriting == false)
                {
                    string[] UserAnswer = list.EntranceAnswerList[0].UserSelectedOption.Split(',');
                    html += "<ul class='QuizAnswerContainer'>";
                    html += "<li class='ExpiryTime' style='display:none'><a href='#' id='";
                    html += "'><span>";
                    html += "</span>";
                    html += "</a></li>";
                    html += "<li class='ExpiryTime' style='display:none'><a href='#' id='";
                    html += "'><span>";
                    html += "</span>";
                    html += "</a></li>";
                    int ArrayCounter = 0;
                    int counter = 0;
                    foreach (EntranceAnswerInfo item in list.EntranceAnswerList)
                    {
                        if (UserAnswer[ArrayCounter].ToString().Trim() == item.AnswerPoolID.ToString())
                        {
                            html += "<li class='selected'><a href='#' id='";
                            if (ArrayCounter < (UserAnswer.Length - 1))
                            {
                                ArrayCounter++;
                            }
                        }
                        else
                        {
                            html += "<li class=''><a href='#' id='";
                        }
                        html += "'><span>";
                        html += alpha[counter];
                        html += "</span>";
                        html += item.EntranceOption;
                        html += "</a></li>";
                        counter++;
                    }
                    html += "</ul>";
                }
                else
                {
                    html += "<div class='form-group'><textarea style='width:90%' rows='3' class='form-control' value=''>" + list.EntranceAnswerList.FirstOrDefault().Detail + "</textarea></div>";
                }
                html += "</div>";
                html += "<div class='quiz-one-last'><div class='row'><div class='quiz-btn-list'>";
                if (list.RowNum > 1)
                {
                    html += "<span><a href='#' class='QuizBack btn btn-outline-info quiz-buttons' my-data='" + Crypto.Encrypt(list.QuestionID) + "," + Crypto.Encrypt(objInfo.EntranceID) + "'><span class='mdi mdi-chevron-left'></span>Prev</a></span>";
                }

                html += "<span><a href='#' class='QuizNext btn btn-outline-info quiz-buttons' my-data='" + Crypto.Encrypt(list.QuestionID) + "," + Crypto.Encrypt(objInfo.EntranceID) + "'>Next<span class='mdi mdi-chevron-right'></span></a></span>";
                html += "</div>";
                html += "<div class='quiz-play'>";
                html += "<h5>";
                html += "Question ";
                PercentageValue = Math.Ceiling(((float)list.RowNum / (float)list.TotalQuestion) * 100.0).ToString();
                html += list.RowNum + " out of " + list.TotalQuestion;
                html += "</h5>";
                html += "<div class='quiz-line'><span style='width:" + PercentageValue + "%'></span></div></div>";
                html += "</div></div></div></div>";
            }
            return Json(html);
        }

        [ValidateAntiForgeryToken]
        public ActionResult GetNextQuestion(EntranceStartInfo objInfo)
        {
            char[] alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
            string[] encryptData;
            string html = "";
            string PercentageValue;
            foreach (string item in objInfo.CustomData)
            {
                encryptData = item.Split(',');
                var t = encryptData[1];
                objInfo.EntranceID = Crypto.Decrypt(t);
                objInfo.QuestionID = Crypto.Decrypt(encryptData[0]);
            }
            objInfo.UserName = new LoginUser().UserName;
            EntranceStartInfo list = _EntranceClientRepo.GetNextQuestion(objInfo);
            if (list != null)
            {
                html += "<div class='row'><div class='col-lg-10 col-md-10 col-sm-10 col-xs-10 quiz-one'>";
                html += "<div class='quiz-one-first'><span>";
                html += "<i class='mdi mdi-circle-outline'></i>";
                html += list.EntranceTitle;
                html += "</span></div><div class='quiz-one-mid'>";
                html += "<h3>";
                html += list.EntranceQuestion;
                html += "</h3>";
                if (list.IsFreeWriting == false)
                {
                    string[] UserAnswer = list.EntranceAnswerList[0].UserSelectedOption.Split(',');
                    html += "<ul class='QuizAnswerContainer'>";
                    html += "<li class='ExpiryTime' style='display:none'><a href='#' id='";
                    html += "'><span>";
                    html += "</span>";
                    html += "</a></li>";
                    html += "<li class='ExpiryTime' style='display:none'><a href='#' id='";
                    html += "'><span>";
                    html += "</span>";
                    html += "</a></li>";
                    int ArrayCounter = 0;
                    int counter = 0;
                    foreach (EntranceAnswerInfo item in list.EntranceAnswerList)
                    {
                        if (UserAnswer[ArrayCounter].ToString().Trim() == item.AnswerPoolID.ToString())
                        {
                            html += "<li class='selected'><a href='#' id='";
                            if (ArrayCounter < (UserAnswer.Length - 1))
                            {
                                ArrayCounter++;
                            }
                        }
                        else
                        {
                            html += "<li class=''><a href='#' id='";
                        }
                        html += "'><span>";
                        html += alpha[counter];
                        html += "</span>";
                        html += item.EntranceOption;
                        html += "</a></li>";
                        counter++;
                    }
                    html += "</ul>";
                }
                else
                {
                    html += "<div class='form-group'><textarea style='width:90%' rows='3' class='form-control' value=''>" + list.EntranceAnswerList.FirstOrDefault().Detail + "</textarea></div>";
                }
                html += "</div>";
                html += "<div class='quiz-one-last'><div class='row'><div class='quiz-btn-list'>";
                if (list.RowNum > 1)
                {
                    html += "<span><a href='#' class='QuizBack btn btn-outline-info quiz-buttons' my-data='" + Crypto.Encrypt(list.QuestionID) + "," + Crypto.Encrypt(objInfo.EntranceID) + "'><span class='mdi mdi-chevron-left'></span>Prev</a></span>";
                }
                html += "<span><a href='#' class='QuizNext btn btn-outline-info quiz-buttons' my-data='" + Crypto.Encrypt(list.QuestionID) + "," + Crypto.Encrypt(objInfo.EntranceID) + "'>Next<span class='mdi mdi-chevron-right'></span></a></span>";
                html += "</div>";
                html += "<div class='quiz-play'>";
                html += "<h5>";
                html += "Question ";
                PercentageValue = Math.Ceiling(((float)list.RowNum / (float)list.TotalQuestion) * 100.0).ToString();
                html += list.RowNum + " out of " + list.TotalQuestion;
                html += "</h5>";
                html += "<div class='quiz-line'><span style='width:" + PercentageValue + "%'></span></div></div>";
                //html += "<div class='quiz-time'>";
                //html += "<span><i class='mdi mdi-timer'></i></span>";
                //html += "<h5><span></span></h5></div>";
                html += "</div></div></div></div>";
            }
            return Json(html);
        }
    }
}
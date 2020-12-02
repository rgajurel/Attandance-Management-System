using DomainEntities;
using DomainInterface;
using InfrastructureData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
//using TechtonneMS.Helper;

namespace SchoolManagementSystem.Areas.Client.Controllers
{
    public class QuizDetailController : Controller
    {
        // GET: Client/QuizDetail
        private readonly IQuizRepository _QuizRepo;
        private readonly IQuizClientRepository _QuizClientRepo;
        //private readonly IStatusRepository _statusRepo;
        //private readonly IMetaDataRepository _metaDataRepo;


        public QuizDetailController(IQuizRepository QuizRepo, IQuizClientRepository QuizClientRepo)//, IStatusRepository statusRepo, IMetaDataRepository metaDataRepo)
        {
            this._QuizRepo = QuizRepo;
            this._QuizClientRepo = QuizClientRepo;
            //this._statusRepo = statusRepo;
            //this._metaDataRepo = metaDataRepo;
        }
        //[AuthorizeUser(Controls = "View")]
        public ActionResult Index(string QuizSlug)
        {
            //await PageHitHelper.UpdatePageHit(System.Web.HttpContext.Current, PageName.quizDetail);

            string UserName = new LoginUser().UserName;
            string html = "";
            QuizClientSide objInfo = _QuizClientRepo.GetQuizDetailsFromSlug(QuizSlug, UserName);
            //  QuizClientSide objInfo2 = _QuizClientRepo.GetQuizProgress(UserName);
            if (objInfo != null)
            {
               // LoadMetaData(CategoryType.CategoryQuiz, objInfo.QuizID);
                if (objInfo.CanShowAllQuestions)
                {
                    // string html = "";
                    html += "<ol class='all-question'>";
                    IEnumerable<string> QuestionList = _QuizClientRepo.GetAllQuizQuestion(objInfo.QuizID);
                    foreach (string item in QuestionList)
                    {
                        html += "<li>" + item + "</li>";

                    }
                    html += "</ol>";
                    ViewBag.Questions = html;
                }
                ViewBag.QuizDescription = objInfo.QuizDescription;
                ViewBag.QuizImage = objInfo.QuizImage==null?"": objInfo.QuizImage;
                ViewBag.QuizTitle = objInfo.QuizTitle;
                if (objInfo.IsCompleted)
                {
                    ViewBag.QuizEncryptData = Crypto.Encrypt(objInfo.QuizID.ToString());
                }
                else
                {
                    ViewBag.QuizEncryptData = Crypto.Encrypt(objInfo.QuizID.ToString()) + "," + Crypto.Encrypt("0");
                }
                ViewBag.IsQuizCompleted = objInfo.IsCompleted;
                ViewBag.CanShowAllQuestion = objInfo.CanShowAllQuestions;

                html = "";
                html += "<h4><strong class='txt-purple'>";
                html += Math.Round(((float)objInfo.TotalQuizAnswered / (float)objInfo.TotalQuestionInQuiz) * 100.0, 2).ToString();
                html += "%</strong> completed (<strong class='txt-purple'>";
                html += objInfo.TotalQuizAnswered.ToString() + "/" + objInfo.TotalQuestionInQuiz.ToString();
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
        public ActionResult GetQuizInformation(QuizStartInfo objInfo)
        {
            string html = "";
            string encryptQuizID;
            string encryptQuestionID;
            char[] alpha = "#ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
            string[] encryptData;
            string PercentageValue;
            string ErrorMessage = "[]";
            bool IsMultipleChoice = false;
            bool HasQuizStarted = false;
            int QuestionDuration;
            bool IsMandatory = false;
            int TimeElapsed = 0;
            int i = 0;
            foreach (string item in objInfo.CustomData)
            {
                encryptData = item.Split(',');
                objInfo.QuizID = Crypto.Decrypt(encryptData[0]);
                objInfo.QuestionID = Crypto.Decrypt(encryptData[1]);
                i++;
            }
            encryptQuizID = Crypto.Encrypt(objInfo.QuizID);
            objInfo.UserName = new LoginUser().UserName;
            QuizStartInfo list = _QuizClientRepo.GetQuizQuestionAndAnswer(int.Parse(objInfo.QuizID), int.Parse(objInfo.QuestionID), objInfo.UserName);
            if (list != null)
            {
                encryptQuestionID = Crypto.Encrypt(list.QuestionID);
                if (!list.HasQuizStarted)
                {
                    html += "<div class='row mtp-sm' id='QuizStartedModal'><div class='col-lg-10 col-md-10 col-sm-10 col-xs-10 quiz-start-mg'>";
                    html += "<div class='quiz-start-top'><h2>Are you Ready?</h2>";
                    html += "<div class='quiz-button'><a href ='#' id='btnStartClientQuiz' my-data='" + encryptQuestionID + "," + encryptQuizID + "'>";
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
                html += list.QuizTitle;
                html += "</span></div><div class='quiz-one-mid'>";
                html += "<h3>";
                html += list.QuizQuestion;
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
                    html += encryptQuizID;
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
                    html += encryptQuizID;
                    html += "'><span>";
                    html += "</span>";
                    html += "</a></li>";
                    #endregion
                    int counter = 1;
                    foreach (QuizAnswerInfo item in list.QuizAnswerList)
                    {
                        html += "<li class=''><a href='#' id='";
                        html += Crypto.Encrypt(item.AnswerPoolID.ToString());
                        html += ",";
                        html += encryptQuestionID;
                        html += ",";
                        html += encryptQuizID;
                        html += "'><span>";
                        html += alpha[counter];
                        html += "</span>";
                        html += item.QuizOption;
                        html += "</a></li>";
                        counter++;
                    }
                    html += "</ul>";
                }
                else
                {
                    html += "<div class='form-group'><textarea style='width:90%' rows='3' class='form-control' data-val='" + encryptQuestionID + "," + encryptQuizID + "'></textarea></div>";
                }
                html += "</div>";
                html += "<div class='quiz-one-last'><div class='row'><div class='quiz-btn-list'>";
                if (list.CanSeePreviousAnswer && !list.IsFirst)
                {
                    html += "<span><a href='#' class='QuizBack btn btn-outline-info quiz-buttons' my-data='" + Crypto.Encrypt("0") + "," + Crypto.Encrypt(objInfo.QuizID) + "'><span class='mdi mdi-chevron-left'></span>Prev</a></span>";
                }
                else
                {
                    html += "<span style='display:none'><a href='#' class='QuizBack btn btn-outline-info quiz-buttons' my-data='" + Crypto.Encrypt("0") + "," + Crypto.Encrypt(objInfo.QuizID) + "'><span class='mdi mdi-chevron-left'></span>Prev</a></span>";
                }
                if (list.IsPauseAllowed)
                {
                    html += "<span><a href='#' class='QuizPause' my-data='" + encryptQuestionID + "," + encryptQuizID + "'><i class='mdi mdi-pause-circle-outline'></i></a></span>";
                }
                else
                {
                    html += "<span style='display:none'><a href='#' class='QuizPause' my-data='" + encryptQuestionID + "," + encryptQuizID + "'><i class='mdi mdi-pause-circle-outline'></i></a></span>";
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
                    html += "<div class='quiz-button'><a href ='#' id='btnFinishClientQuiz' mydata='" + encryptQuizID + "' title='Quiz Report'>";
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
                HasQuizStarted = list.HasQuizStarted;
                TimeElapsed = list.TimeElapsed;
                list.UserName = new LoginUser().UserName;
                list.QuizID = objInfo.QuizID;
                //if (list.QuestionID != "0")
                //{
                //    _QuizClientRepo.StartQuiz(list);
                //}
            }
            else
            {
                html = null;
                IsMultipleChoice = false;
                ErrorMessage = _QuizClientRepo.GetErrorMessage(int.Parse(objInfo.QuizID), int.Parse(objInfo.QuestionID), objInfo.UserName);
                QuestionDuration = -1;
            }
            return Json(new { RenderContent = html, IsMultiple = IsMultipleChoice, Duration = QuestionDuration, Mandatory = IsMandatory, HasQuizStarted = HasQuizStarted, TimeElapsed = TimeElapsed, ErrorMessage = ErrorMessage });
        }
        [ValidateAntiForgeryToken]
        public ActionResult QuizProcced(QuizStartInfo objInfo)
        {
            string html = "";
            string encryptQuizID;
            string encryptQuestionID;
            string[] encryptData;
            string PercentageValue;
            string ErrorMessage = "[]";
            bool IsMultipleChoice = false;
            int QuestionDuration;
            bool IsMandatory = false;
            bool HasQuizStarted = false;
            int TimeElapsed = 0;
            char[] alpha = "#ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
            if (objInfo.IsFreeWriting == false)
            {
                List<string> temporyValue = new List<string>(); ;
                foreach (string item in objInfo.CustomData)
                {
                    encryptData = item.Split(',');
                    var t = encryptData[2];
                    objInfo.QuizID = Crypto.Decrypt(t);
                    objInfo.QuestionID = Crypto.Decrypt(encryptData[1]);
                    temporyValue.Add(Crypto.Decrypt(encryptData[0]));
                }
                objInfo.AnswerID = temporyValue;
            }
            else
            {
                encryptData = objInfo.FreeWritingAnswer.Split(',');
                objInfo.QuizID = Crypto.Decrypt(encryptData[1]);
                objInfo.QuestionID = Crypto.Decrypt(encryptData[0]);
                Regex rRemScript = new Regex(@"<script[^>]*>[\s\S]*?</script>");
                objInfo.FreeWritingAnswer = rRemScript.Replace(encryptData[2], ""); ;
            }
            encryptQuizID = Crypto.Encrypt(objInfo.QuizID);
            objInfo.UserName = new LoginUser().UserName;
            bool status = _QuizClientRepo.IsAnswerCorrect(objInfo);
            QuizStartInfo list = _QuizClientRepo.GetQuizQuestionAndAnswer(int.Parse(objInfo.QuizID), int.Parse(objInfo.QuestionID), objInfo.UserName);
            if (list != null)
            {
                encryptQuestionID = Crypto.Encrypt(list.QuestionID);
                if (!list.HasQuizStarted)
                {
                    html += "<div class='row mtp-sm' id='QuizStartedModal'><div class='col-lg-10 col-md-10 col-sm-10 col-xs-10 quiz-start-mg'>";
                    html += "<div class='quiz-start-top'><h2>Are you Ready?</h2>";
                    html += "<div class='quiz-button'><a href ='#' id='btnStartClientQuiz' my-data='" + encryptQuestionID + "," + encryptQuizID + "'>";
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
                html += list.QuizTitle;
                html += "</span></div><div class='quiz-one-mid'>";
                html += "<h3>";
                html += list.QuizQuestion;
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
                    html += encryptQuizID;
                    html += "'><span>";
                    html += "</span>";
                    html += "</a></li>";
                    html += "<li class='ExpiryTime' style='display:none'><a href='#' id='";
                    //-2 for User skip Answer
                    html += Crypto.Encrypt("-2");
                    html += ",";
                    html += encryptQuestionID;
                    html += ",";
                    html += encryptQuizID;
                    html += "'><span>";
                    html += "</span>";
                    html += "</a></li>";
                    int counter = 1;
                    foreach (QuizAnswerInfo item in list.QuizAnswerList)
                    {
                        html += "<li class=''><a href='#' id='";
                        html += Crypto.Encrypt(item.AnswerPoolID.ToString());
                        html += ",";
                        html += encryptQuestionID;
                        html += ",";
                        html += encryptQuizID;
                        html += "'><span>";
                        html += alpha[counter];
                        html += "</span>";
                        html += item.QuizOption;
                        html += "</a></li>";
                        counter++;
                    }
                    html += "</ul>";
                }
                else
                {
                    html += "<div class='form-group'><textarea style='width:90%' rows='3' class='form-control' data-val='" + encryptQuestionID + "," + encryptQuizID + "'></textarea></div>";
                }
                html += "</div>";
                html += "<div class='quiz-one-last'><div class='row'><div class='quiz-btn-list'>";
                if (list.CanSeePreviousAnswer)
                {
                    html += "<span><a href='#' class='QuizBack btn btn-outline-info quiz-buttons' my-data='" + Crypto.Encrypt("0") + "," + encryptQuizID + "'><span class='mdi mdi-chevron-left'></span>Prev</a></span>";
                }
                if (list.IsPauseAllowed)
                {
                    html += "<span><a href='#' class='QuizPause' my-data='" + encryptQuestionID + "," + encryptQuizID + "'><i class='mdi mdi-pause-circle-outline'></i></a></span>";
                }
                else
                {
                    html += "<span style='display:none'><a href='#' class='QuizPause' my-data='" + encryptQuestionID + "," + encryptQuizID + "'><i class='mdi mdi-pause-circle-outline'></i></a></span>";
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
                    html += "<div class='quiz-button'><a href ='#' id='btnFinishClientQuiz' mydata='" + encryptQuizID + "' title='Quiz Report'>";
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
                HasQuizStarted = list.HasQuizStarted;
                list.UserName = new LoginUser().UserName;
                list.QuizID = objInfo.QuizID;
                //if (list.QuestionID != "0")
                //{
                //    _QuizClientRepo.StartQuiz(list);
                //}
                TimeElapsed = list.TimeElapsed;
            }
            else
            {
                html = null;
                ErrorMessage = _QuizClientRepo.GetErrorMessage(int.Parse(objInfo.QuizID), int.Parse(objInfo.QuestionID), objInfo.UserName);
                QuestionDuration = -1;
            }
            return Json(new { RenderContent = html, IsMultiple = IsMultipleChoice, Duration = QuestionDuration, Mandatory = IsMandatory, HasQuizStarted = HasQuizStarted, TimeElapsed = TimeElapsed ,ErrorMessage = ErrorMessage });
        }
        [ValidateAntiForgeryToken]
        public ActionResult QuizReport(QuizStartInfo objInfo)
        {
            string html = "";
            objInfo.QuizID = Crypto.Decrypt(objInfo.CustomData[0]);
            objInfo.UserName = new LoginUser().UserName;
            string[] answer;
            int PercentageValue;
            string tempAnswer;
            IEnumerable<QuizCompletionReport> ReportLst = _QuizClientRepo.GetQuizReport(int.Parse(objInfo.QuizID), objInfo.UserName);
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
            foreach (QuizCompletionReport item in ReportLst)
            {
                html += "<div class='quest-bottom pull-left'>";
                html += "<h4>";
                html += item.QuizQuestion;
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
                        if (item.IsQuizExpired)
                        {
                            tempAnswer = "(Not Answered because Quiz Expired)";
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
        public ActionResult QuizStart(QuizStartInfo objInfo)
        {
            string[] encryptData;
            foreach (string item in objInfo.CustomData)
            {
                encryptData = item.Split(',');
                string t = encryptData[1];
                objInfo.QuizID = Crypto.Decrypt(t);
                objInfo.QuestionID = Crypto.Decrypt(encryptData[0]);
            }
            objInfo.UserName = new LoginUser().UserName;
            _QuizClientRepo.StartQuiz(objInfo);
            return Json(null);
        }

        [ValidateAntiForgeryToken]
        public ActionResult PauseQuiz(QuizStartInfo objInfo)
        {
            string[] encryptData;
            foreach (string item in objInfo.CustomData)
            {
                encryptData = item.Split(',');
                var t = encryptData[1];
                objInfo.QuizID = Crypto.Decrypt(t);
                objInfo.QuestionID = Crypto.Decrypt(encryptData[0]);
            }
            objInfo.UserName = new LoginUser().UserName;
            return Json(_QuizClientRepo.IsPaused(objInfo));
        }

        [ValidateAntiForgeryToken]
        public ActionResult SetElapsedTime(QuizStartInfo objInfo)
        {
            string[] encryptData;
            foreach (string item in objInfo.CustomData)
            {
                encryptData = item.Split(',');
                var t = encryptData[1];
                objInfo.QuizID = Crypto.Decrypt(t);
                objInfo.QuestionID = Crypto.Decrypt(encryptData[0]);
            }
            objInfo.UserName = new LoginUser().UserName;
            _QuizClientRepo.SetTimeElapsed(objInfo);
            return Json(null);
        }
        [ValidateAntiForgeryToken]
        public ActionResult GetPreviousQuestion(QuizStartInfo objInfo)
        {
            char[] alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
            string[] encryptData;
            string html = null;
            string PercentageValue;
            foreach (string item in objInfo.CustomData)
            {
                encryptData = item.Split(',');
                var t = encryptData[1];
                objInfo.QuizID = Crypto.Decrypt(t);
                objInfo.QuestionID = Crypto.Decrypt(encryptData[0]);
            }
            objInfo.UserName = new LoginUser().UserName;
            QuizStartInfo list = _QuizClientRepo.GetPreviousQuestion(objInfo);
            if (list != null)
            {
                html += "<div class='row'><div class='col-lg-10 col-md-10 col-sm-10 col-xs-10 quiz-one'>";
                html += "<div class='quiz-one-first'><span>";
                html += "<i class='mdi mdi-circle-outline'></i>";
                html += list.QuizTitle;
                html += "</span></div><div class='quiz-one-mid'>";
                html += "<h3>";
                html += list.QuizQuestion;
                html += "</h3>";
                if (list.IsFreeWriting == false)
                {
                    string[] UserAnswer = list.QuizAnswerList[0].UserSelectedOption.Split(',');
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
                    foreach (QuizAnswerInfo item in list.QuizAnswerList)
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
                        html += item.QuizOption;
                        html += "</a></li>";
                        counter++;
                    }
                    html += "</ul>";
                }
                else
                {
                    html += "<div class='form-group'><textarea style='width:90%' rows='3' class='form-control' value=''>" + list.QuizAnswerList.FirstOrDefault().Detail + "</textarea></div>";
                }
                html += "</div>";
                html += "<div class='quiz-one-last'><div class='row'><div class='quiz-btn-list'>";
                if (list.RowNum > 1)
                {
                    html += "<span><a href='#' class='QuizBack btn btn-outline-info quiz-buttons' my-data='" + Crypto.Encrypt(list.QuestionID) + "," + Crypto.Encrypt(objInfo.QuizID) + "'><span class='mdi mdi-chevron-left'></span>Prev</a></span>";
                }

                html += "<span><a href='#' class='QuizNext btn btn-outline-info quiz-buttons' my-data='" + Crypto.Encrypt(list.QuestionID) + "," + Crypto.Encrypt(objInfo.QuizID) + "'>Next<span class='mdi mdi-chevron-right'></span></a></span>";
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
        public ActionResult GetNextQuestion(QuizStartInfo objInfo)
        {
            char[] alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
            string[] encryptData;
            string html = "";
            string PercentageValue;
            foreach (string item in objInfo.CustomData)
            {
                encryptData = item.Split(',');
                var t = encryptData[1];
                objInfo.QuizID = Crypto.Decrypt(t);
                objInfo.QuestionID = Crypto.Decrypt(encryptData[0]);
            }
            objInfo.UserName = new LoginUser().UserName;
            QuizStartInfo list = _QuizClientRepo.GetNextQuestion(objInfo);
            if (list != null)
            {
                html += "<div class='row'><div class='col-lg-10 col-md-10 col-sm-10 col-xs-10 quiz-one'>";
                html += "<div class='quiz-one-first'><span>";
                html += "<i class='mdi mdi-circle-outline'></i>";
                html += list.QuizTitle;
                html += "</span></div><div class='quiz-one-mid'>";
                html += "<h3>";
                html += list.QuizQuestion;
                html += "</h3>";
                if (list.IsFreeWriting == false)
                {
                    string[] UserAnswer = list.QuizAnswerList[0].UserSelectedOption.Split(',');
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
                    foreach (QuizAnswerInfo item in list.QuizAnswerList)
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
                        html += item.QuizOption;
                        html += "</a></li>";
                        counter++;
                    }
                    html += "</ul>";
                }
                else
                {
                    html += "<div class='form-group'><textarea style='width:90%' rows='3' class='form-control' value=''>" + list.QuizAnswerList.FirstOrDefault().Detail + "</textarea></div>";
                }
                html += "</div>";
                html += "<div class='quiz-one-last'><div class='row'><div class='quiz-btn-list'>";
                if (list.RowNum > 1)
                {
                    html += "<span><a href='#' class='QuizBack btn btn-outline-info quiz-buttons' my-data='" + Crypto.Encrypt(list.QuestionID) + "," + Crypto.Encrypt(objInfo.QuizID) + "'><span class='mdi mdi-chevron-left'></span>Prev</a></span>";
                }
                html += "<span><a href='#' class='QuizNext btn btn-outline-info quiz-buttons' my-data='" + Crypto.Encrypt(list.QuestionID) + "," + Crypto.Encrypt(objInfo.QuizID) + "'>Next<span class='mdi mdi-chevron-right'></span></a></span>";
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

        //public void LoadMetaData(string type, int uniqueID)
        //{
        //    MetaData metadata = _metaDataRepo.GetMetaData(type, uniqueID);
        //    ViewBag.MetaTitle = metadata.MetaTitle;
        //    ViewBag.MetaKeyword = metadata.MetaKeyword;
        //    ViewBag.MetaDescription = metadata.MetaDescription;
        //}
    }
}
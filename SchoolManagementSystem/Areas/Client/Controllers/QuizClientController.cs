using DomainEntities;
using DomainInterface;
using InfrastructureData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SchoolManagementSystem.Areas.Client.Controllers
{
    public class QuizClientController : Controller
    {
        private readonly IQuizRepository _QuizRepo;
        private readonly IQuizClientRepository _QuizClientRepo;
        //private readonly IStatusRepository _statusRepo;
       
        int QuizPerpage = 10;
        public QuizClientController(IQuizRepository QuizRepo, IQuizClientRepository QuizClientRepo)
        {
            this._QuizRepo = QuizRepo;
            this._QuizClientRepo = QuizClientRepo;
        }
        // GET: Client/QuizClient\

        //[AuthorizeUser(Controls = "View")]
        public ActionResult Index()
        {
         //   await PageHitHelper.UpdatePageHit(System.Web.HttpContext.Current,PageName.quiz);

           // GetAllQuizCategory();
            LoadQuizListinginPageLoad();
            GetValueForSorting();
            ViewBag.UserName = new LoginUser().UserName;
            ViewBag.RecordsPerPage = QuizPerpage;
            return View();
        }
        private void GetValueForSorting()
        {
          //  ViewBag.QuizSortingOption = new SelectList(_statusRepo.GetStatusBasedOnIdentifier(StatusIdentifier.identifierQuizClientSort), "StatusValue", "StatusName");
        }
        //private void GetAllQuizCategory()
        //{
        //    IEnumerable<CategoryTree> LstCategory = _QuizRepo.GetAllQuizCategory(CategoryType.CategoryQuiz, StatusIdentifier.identifierQuiz);
        //    ViewBag.QuizCategory = new SelectList(LstCategory, "CategoryID", "CategoryName");
        //}
        private void LoadQuizListinginPageLoad()
        {
            QuizSearchingClientSide objInfo = new QuizSearchingClientSide();
            objInfo.PageSize = QuizPerpage;
            objInfo.PageIndex = 1;
            objInfo.SearchQuizTitle = "";
            objInfo.SortBy = 0;
            objInfo.UserName = new LoginUser().UserName;
            IEnumerable<QuizClientSide> QuizLst = _QuizClientRepo.GetQuizListingForClient(objInfo);
            string RenderQuizInfo = "";
            string EncryptValue;
            string PercentageValue;
            string ClassName;
            if (QuizLst.Count() == 0)
            {
                RenderQuizInfo += "<div class='table'><div class='cell'>";
                RenderQuizInfo += "<div class='caption text-center'>";
                RenderQuizInfo += "<i class='fa fa-lightbulb-o fa-fw'></i>";
                RenderQuizInfo +="<h3 class='text-js'>Quiz Data Not Found</h3>";
                RenderQuizInfo += "</div></div></div>";
            }
            else
            {
                foreach (QuizClientSide item in QuizLst)
                {
                    PercentageValue = Math.Round(((float)item.TotalQuizAnswered / (float)item.TotalQuestionInQuiz) * 100.0, 2).ToString();
                    EncryptValue = Crypto.Encrypt(item.QuizID.ToString());
                    ClassName = GetClass(PercentageValue);
                    RenderQuizInfo += "<li class='";
                    RenderQuizInfo += ClassName;
                    RenderQuizInfo += "'>";
                    if (ClassName != "first100")
                    {
                        RenderQuizInfo += "<a class='QuizPlay' href='#' data-val='";
                        RenderQuizInfo += EncryptValue;
                        RenderQuizInfo += ",";
                        RenderQuizInfo += Crypto.Encrypt("0");
                        RenderQuizInfo += "' title='";
                        RenderQuizInfo += item.QuizTitle;
                        RenderQuizInfo += "' > ";
                    }
                    else
                    {
                        RenderQuizInfo += "<a class='QuizPlay' href='#' data-val='";
                        RenderQuizInfo += EncryptValue;
                        RenderQuizInfo += "' title='";
                        RenderQuizInfo += item.QuizTitle;
                        RenderQuizInfo += "' >";

                    }
                    RenderQuizInfo += "<i class='mdi mdi-play-circle-outline mr-xsm'></i>";
                    RenderQuizInfo += "<span>";
                    RenderQuizInfo += item.QuizTitle.Length > 35 ? item.QuizTitle.Substring(0, 34) + ".." : item.QuizTitle;
                    RenderQuizInfo += "</span></a><a class='QuizDetails' href='QuizDetail/?QuizSlug=" + item.QuizSlug + "'><p>";
                    RenderQuizInfo += PercentageValue;
                    RenderQuizInfo += "%</p>";
                    RenderQuizInfo += "<h5>";
                    RenderQuizInfo += item.TotalQuizAnswered.ToString();
                    RenderQuizInfo += "/";
                    RenderQuizInfo += item.TotalQuestionInQuiz.ToString();
                    RenderQuizInfo += " Question Answer</h5></a>";
                    RenderQuizInfo += "</li>";
                }
            }
            ViewBag.ContentOnPageLoad = RenderQuizInfo;
            try
            {
               // ViewBag.ProgressPercentage = QuizLst.FirstOrDefault().ProgressPercentage == null ? "0" : QuizLst.FirstOrDefault().ProgressPercentage;
                ViewBag.TotalQuiz = QuizLst.FirstOrDefault().TotalQuiz;
            }
            catch (Exception)
            {
               // ViewBag.ProgressPercentage = "0";
                ViewBag.TotalQuiz = 0;
            }
        }

        [ValidateAntiForgeryToken]
        public ActionResult QuizListingPagination(QuizSearchingClientSide objInfo)
        {
            objInfo.PageSize = QuizPerpage;
            objInfo.PageIndex = objInfo.PageIndex;
            objInfo.UserName = new LoginUser().UserName;
            IEnumerable<QuizClientSide> QuizLst = _QuizClientRepo.GetQuizListingForClient(objInfo);
            string RenderQuizInfo = "";
            string EncryptValue;
            string PercentageValue;
            int totalCount;
            string ClassName;
            if (QuizLst.Count()==0)
            {
                RenderQuizInfo += "<div class='table'><div class='cell'>";
                RenderQuizInfo += "<div class='caption text-center'>";
                RenderQuizInfo += "<i class='fa fa-lightbulb-o fa-fw'></i>";
                RenderQuizInfo += "<h3 class='text-js'>Quiz Data Not Found</h3>";
                RenderQuizInfo += "</div></div></div>";
            }
            else
            {
                foreach (QuizClientSide item in QuizLst)
                {
                    PercentageValue = Math.Round(((float)item.TotalQuizAnswered / (float)item.TotalQuestionInQuiz) * 100.0, 2).ToString();
                    EncryptValue = Crypto.Encrypt(item.QuizID.ToString());
                    ClassName = GetClass(PercentageValue);
                    RenderQuizInfo += "<li class='";
                    RenderQuizInfo += GetClass(PercentageValue);
                    RenderQuizInfo += "'>";
                    if (ClassName != "first100")
                    {
                        RenderQuizInfo += "<a class='QuizPlay' href='#' data-val='";
                        RenderQuizInfo += EncryptValue;
                        RenderQuizInfo += ",";
                        RenderQuizInfo += Crypto.Encrypt("0");
                        RenderQuizInfo += "' title='";
                        RenderQuizInfo += item.QuizTitle;
                        RenderQuizInfo += "' > ";
                    }
                    else
                    {
                        RenderQuizInfo += "<a class='QuizPlay' href='#' data-val='";
                        RenderQuizInfo += EncryptValue;
                        RenderQuizInfo += "' title='";
                        RenderQuizInfo += item.QuizTitle;
                        RenderQuizInfo += "' >";
                    }
                    RenderQuizInfo += "<i class=mdi mdi-play-circle-outline mr-xsm'></i>";
                    RenderQuizInfo += "<span>";
                    RenderQuizInfo += item.QuizTitle.Length > 35 ? item.QuizTitle.Substring(0, 34) + ".." : item.QuizTitle;
                    RenderQuizInfo += "</span></a><a class='QuizDetails' href='QuizDetail/?QuizSlug=" + item.QuizSlug + "'><p>";
                    RenderQuizInfo += PercentageValue;
                    RenderQuizInfo += "%</p>";
                    RenderQuizInfo += "<h5>";
                    RenderQuizInfo += item.TotalQuizAnswered.ToString();
                    RenderQuizInfo += "/";
                    RenderQuizInfo += item.TotalQuestionInQuiz.ToString();
                    RenderQuizInfo += " Question Answer</h5></a>";
                    RenderQuizInfo += "</li>";
                }
            }
            try
            {
                totalCount = QuizLst.FirstOrDefault().TotalQuiz;
            }
            catch (Exception)
            {
                totalCount = 0;
            }
            return Json(new { renderString = RenderQuizInfo, totalCount = totalCount });
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
                PercentageValue = ((float)list.RowNum / (float)list.TotalQuestion * 100.0).ToString();
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
               // }
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
                QuestionDuration = -1;
                ErrorMessage = _QuizClientRepo.GetErrorMessage(int.Parse(objInfo.QuizID), int.Parse(objInfo.QuestionID), objInfo.UserName);
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
                List<string> temporyValue = new List<string>();
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
                QuestionDuration = -1;
                ErrorMessage = _QuizClientRepo.GetErrorMessage(int.Parse(objInfo.QuizID), int.Parse(objInfo.QuestionID), objInfo.UserName);
            }
            return Json(new { RenderContent = html, IsMultiple = IsMultipleChoice, Duration = QuestionDuration, Mandatory = IsMandatory, HasQuizStarted = HasQuizStarted, TimeElapsed = TimeElapsed,ErrorMessage= ErrorMessage });
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
            html +="'>";
            html += "<h5>";
            html += new LoginUser().UserName; ;
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
                //html += "<div class='col-lg-2 pull-right quiz-one-last'>";
                //html += "<div class='quiz-time quiz-time-back'>";
                //html += "<span><i class='mdi mdi-timer'></i></span>";
                //html += " <h5><span>0</span>min<span>0</span>sec</h5></div></div>";


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
                    html += "<div class='form-group'><textarea style='width:90%' rows='3' class='form-control'>"+ list.QuizAnswerList.FirstOrDefault().Detail +"</textarea></div>";
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
        private string GetClass(string Percentage)
        {
            double tempPercentage = double.Parse(Percentage);
            if (tempPercentage >= 0 && tempPercentage < 10)
            {
                return "first10";
            }
            else if (tempPercentage >= 10 && tempPercentage < 20)
            {
                return "first20";
            }
            else if (tempPercentage >= 20 && tempPercentage < 30)
            {
                return "first30";
            }
            else if (tempPercentage >= 30 && tempPercentage < 40)
            {
                return "first40";
            }
            else if (tempPercentage >= 40 && tempPercentage < 50)
            {
                return "first50";
            }
            else if (tempPercentage >= 50 && tempPercentage < 60)
            {
                return "first60";
            }
            else if (tempPercentage >= 60 && tempPercentage < 70)
            {
                return "first70";
            }
            else if (tempPercentage >= 70 && tempPercentage < 80)
            {
                return "first80";
            }
            else if (tempPercentage >= 80 && tempPercentage < 90)
            {
                return "first90";
            }
            else
            {
                return "first100";
            }
        }

    }
}
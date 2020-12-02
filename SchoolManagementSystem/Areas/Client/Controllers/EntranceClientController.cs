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
    public class EntranceClientController : Controller
    {
        private readonly IEntranceRepository _EntranceRepo;
        private readonly IEntranceClientRepository _EntranceClientRepo;
        //private readonly IStatusRepository _statusRepo;

        int EntrancePerpage = 10;
        public EntranceClientController(IEntranceRepository EntranceRepo, IEntranceClientRepository EntranceClientRepo)
        {
            this._EntranceRepo = EntranceRepo;
            this._EntranceClientRepo = EntranceClientRepo;
        }
        // GET: Client/QuizClient\

        //[AuthorizeUser(Controls = "View")]
        public ActionResult Index()
        {
            //   await PageHitHelper.UpdatePageHit(System.Web.HttpContext.Current,PageName.quiz);

            // GetAllQuizCategory();
            LoadEntranceListinginPageLoad();
            GetValueForSorting();
            ViewBag.UserName = new LoginUser().UserName;
            ViewBag.RecordsPerPage = EntrancePerpage;
            return View();
        }
        private void GetValueForSorting()
        {
            //  ViewBag.QuizSortingOption = new SelectList(_statusRepo.GetStatusBasedOnIdentifier(StatusIdentifier.identifierQuizClientSort), "StatusValue", "StatusName");
        }
        //private void GetAllQuizCategory()
        //{
        //    IEnumerable<CategoryTree> LstCategory = _EntranceRepo.GetAllQuizCategory(CategoryType.CategoryQuiz, StatusIdentifier.identifierQuiz);
        //    ViewBag.QuizCategory = new SelectList(LstCategory, "CategoryID", "CategoryName");
        //}
        private void LoadEntranceListinginPageLoad()
        {
            EntranceSearchingClientSide objInfo = new EntranceSearchingClientSide();
            objInfo.PageSize = EntrancePerpage;
            objInfo.PageIndex = 1;
            objInfo.SearchEntranceTitle = "";
            objInfo.SortBy = 0;
            objInfo.UserName = new LoginUser().UserName;
            IEnumerable<EntranceClientSide> EntranceLst = _EntranceClientRepo.GetEntranceListingForClient(objInfo);
            string RenderEntranceInfo = "";
            string EncryptValue;
            string PercentageValue;
            string ClassName;
            if (EntranceLst.Count() == 0)
            {
                RenderEntranceInfo += "<div class='table'><div class='cell'>";
                RenderEntranceInfo += "<div class='caption text-center'>";
                RenderEntranceInfo += "<i class='fa fa-lightbulb-o fa-fw'></i>";
                RenderEntranceInfo += "<h3 class='text-js'>Quiz Data Not Found</h3>";
                RenderEntranceInfo += "</div></div></div>";
            }
            else
            {
                foreach (EntranceClientSide item in EntranceLst)
                {
                    PercentageValue = Math.Round(((float)item.TotalEntranceAnswered / (float)item.TotalQuestionInEntrance) * 100.0, 2).ToString();
                    EncryptValue = Crypto.Encrypt(item.EntranceID.ToString());
                    ClassName = GetClass(PercentageValue);
                    RenderEntranceInfo += "<li class='";
                    RenderEntranceInfo += ClassName;
                    RenderEntranceInfo += "'>";
                    if (ClassName != "first100")
                    {
                        RenderEntranceInfo += "<a class='QuizPlay' href='#' data-val='";
                        RenderEntranceInfo += EncryptValue;
                        RenderEntranceInfo += ",";
                        RenderEntranceInfo += Crypto.Encrypt("0");
                        RenderEntranceInfo += "' title='";
                        RenderEntranceInfo += item.EntranceTitle;
                        RenderEntranceInfo += "' > ";
                    }
                    else
                    {
                        RenderEntranceInfo += "<a class='QuizPlay' href='#' data-val='";
                        RenderEntranceInfo += EncryptValue;
                        RenderEntranceInfo += "' title='";
                        RenderEntranceInfo += item.EntranceTitle;
                        RenderEntranceInfo += "' >";

                    }
                    RenderEntranceInfo += "<i class='mdi mdi-play-circle-outline mr-xsm'></i>";
                    RenderEntranceInfo += "<span>";
                    RenderEntranceInfo += item.EntranceTitle.Length > 35 ? item.EntranceTitle.Substring(0, 34) + ".." : item.EntranceTitle;
                    RenderEntranceInfo += "</span></a><a class='QuizDetails' href='EntranceDetail/?EntranceSlug=" + item.EntranceSlug + "'><p>";
                    RenderEntranceInfo += PercentageValue;
                    RenderEntranceInfo += "%</p>";
                    RenderEntranceInfo += "<h5>";
                    RenderEntranceInfo += item.TotalEntranceAnswered.ToString();
                    RenderEntranceInfo += "/";
                    RenderEntranceInfo += item.TotalQuestionInEntrance.ToString();
                    RenderEntranceInfo += " Question Answer</h5></a>";
                    RenderEntranceInfo += "</li>";
                }
            }
            ViewBag.ContentOnPageLoad = RenderEntranceInfo;
            try
            {
                // ViewBag.ProgressPercentage = EntranceLst.FirstOrDefault().ProgressPercentage == null ? "0" : EntranceLst.FirstOrDefault().ProgressPercentage;
                ViewBag.TotalEntrance = EntranceLst.FirstOrDefault().TotalEntrance;
            }
            catch (Exception)
            {
                // ViewBag.ProgressPercentage = "0";
                ViewBag.TotalEntrance = 0;
            }
        }

        [ValidateAntiForgeryToken]
        public ActionResult EntranceListingPagination(EntranceSearchingClientSide objInfo)
        {
            objInfo.PageSize = EntrancePerpage;
            objInfo.PageIndex = objInfo.PageIndex;
            objInfo.UserName = new LoginUser().UserName;
            IEnumerable<EntranceClientSide> EntranceLst = _EntranceClientRepo.GetEntranceListingForClient(objInfo);
            string RenderEntranceInfo = "";
            string EncryptValue;
            string PercentageValue;
            int totalCount;
            string ClassName;
            if (EntranceLst.Count() == 0)
            {
                RenderEntranceInfo += "<div class='table'><div class='cell'>";
                RenderEntranceInfo += "<div class='caption text-center'>";
                RenderEntranceInfo += "<i class='fa fa-lightbulb-o fa-fw'></i>";
                RenderEntranceInfo += "<h3 class='text-js'>Quiz Data Not Found</h3>";
                RenderEntranceInfo += "</div></div></div>";
            }
            else
            {
                foreach (EntranceClientSide item in EntranceLst)
                {
                    PercentageValue = Math.Round(((float)item.TotalEntranceAnswered / (float)item.TotalQuestionInEntrance) * 100.0, 2).ToString();
                    EncryptValue = Crypto.Encrypt(item.EntranceID.ToString());
                    ClassName = GetClass(PercentageValue);
                    RenderEntranceInfo += "<li class='";
                    RenderEntranceInfo += GetClass(PercentageValue);
                    RenderEntranceInfo += "'>";
                    if (ClassName != "first100")
                    {
                        RenderEntranceInfo += "<a class='QuizPlay' href='#' data-val='";
                        RenderEntranceInfo += EncryptValue;
                        RenderEntranceInfo += ",";
                        RenderEntranceInfo += Crypto.Encrypt("0");
                        RenderEntranceInfo += "' title='";
                        RenderEntranceInfo += item.EntranceTitle;
                        RenderEntranceInfo += "' > ";
                    }
                    else
                    {
                        RenderEntranceInfo += "<a class='QuizPlay' href='#' data-val='";
                        RenderEntranceInfo += EncryptValue;
                        RenderEntranceInfo += "' title='";
                        RenderEntranceInfo += item.EntranceTitle;
                        RenderEntranceInfo += "' >";
                    }
                    RenderEntranceInfo += "<i class=mdi mdi-play-circle-outline mr-xsm'></i>";
                    RenderEntranceInfo += "<span>";
                    RenderEntranceInfo += item.EntranceTitle.Length > 35 ? item.EntranceTitle.Substring(0, 34) + ".." : item.EntranceTitle;
                    RenderEntranceInfo += "</span></a><a class='QuizDetails' href='EntranceDetail/?EntranceSlug=" + item.EntranceSlug + "'><p>";
                    RenderEntranceInfo += PercentageValue;
                    RenderEntranceInfo += "%</p>";
                    RenderEntranceInfo += "<h5>";
                    RenderEntranceInfo += item.TotalEntranceAnswered.ToString();
                    RenderEntranceInfo += "/";
                    RenderEntranceInfo += item.TotalQuestionInEntrance.ToString();
                    RenderEntranceInfo += " Question Answer</h5></a>";
                    RenderEntranceInfo += "</li>";
                }
            }
            try
            {
                totalCount = EntranceLst.FirstOrDefault().TotalEntrance;
            }
            catch (Exception)
            {
                totalCount = 0;
            }
            return Json(new { renderString = RenderEntranceInfo, totalCount = totalCount });
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
                PercentageValue = ((float)list.RowNum / (float)list.TotalQuestion * 100.0).ToString();
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
                // }
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
                QuestionDuration = -1;
                ErrorMessage = _EntranceClientRepo.GetErrorMessage(int.Parse(objInfo.EntranceID), int.Parse(objInfo.QuestionID), objInfo.UserName,objInfo.Identifier);
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
                List<string> temporyValue = new List<string>();
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
                QuestionDuration = -1;
                ErrorMessage = _EntranceClientRepo.GetErrorMessage(int.Parse(objInfo.EntranceID), int.Parse(objInfo.QuestionID), objInfo.UserName, objInfo.Identifier);
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
                //html += "<div class='col-lg-2 pull-right quiz-one-last'>";
                //html += "<div class='quiz-time quiz-time-back'>";
                //html += "<span><i class='mdi mdi-timer'></i></span>";
                //html += " <h5><span>0</span>min<span>0</span>sec</h5></div></div>";


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
                    html += "<div class='form-group'><textarea style='width:90%' rows='3' class='form-control'>" + list.EntranceAnswerList.FirstOrDefault().Detail + "</textarea></div>";
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
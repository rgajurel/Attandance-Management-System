using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public static class CategoryType
    {
        public static readonly string categoryFAQ = "FAQ";
        public static readonly string categoryNews = "News";
        public static readonly string categoryCourse = "Course";
        public static readonly string categoryCoursePage = "Page";
        public static readonly string categoryInformationCenter = "InformationCenter";
        public static readonly string CategoryQuiz = "Quiz";
        public static readonly string CategoryQuizQuestion = "QuizQuestion";
        public static readonly string CategoryArticle = "Article";
       
        public static readonly string CategorySurvey = "Survey";
        public static readonly string CategorySurveyQuestion = "SurveyQuestion";

        public static readonly string CategoryEntrance = "Entrance";
        public static readonly string CategoryEntranceQuestion = "EntranceQuestion";
    }
    public static class CourseIndividualProgressStatus
    {
        public static readonly string statComplete = "Complete";
        public static readonly string statIncomplete = "Incomplete";

    }
    //public static class DummyLoggedInUserID
    //{
    //    public static readonly int adminloggedInUserID = 1;
    //    public static readonly int clientloggedInUserID = 6;
    //}
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public  class MessageHolder
    {
       
        public string Message { get; set; }
        public int Code { get; set; }
        public bool isAuthorized { get; set; }
        public bool ErrorOccured { get; set; }

        public bool success { get; set; }


    }

    public class ListDataHolder
    {
        public string Message { get; set; }
        public IEnumerable Data { get; set; }
        public bool ErrorOccured { get; set; }
        public int Code { get; set; }
        public bool isAuthorized { get; set; }
            
        public object Errors { get; set; }
        public int Total { get; set; }
    }
    public static  class MassageDescription
    {
        public static string SingleData = "Warning ! Only Check One CheckBox";
        public static string NoData = " Sorry ! No Data Present";
        public static string SaveSuccess = " Success ! Saved Successfully";
        public static string SaveFailure = "Warning !  Failed To Save ";
        public static string UpdateSuccess = " Success !  Updated Successfully";
        public static string AlreadyExist = " Warning !  Already Exist";
        public static string UpdateFailure = "Warning ! Failed To Update";
        public static string DeleteSuccess = "Success !  Delete Success ";
        public static string Deleteailure = "Warning !  Delete Failure ";
        public static string CannotDeleteDependency = "Warning !  CannotDelete Dependency Exist";
        public static string CannotDelete = "Warning !  Only SuperAdmin Can Delete";
        public static string ModelErrorOccured = "Warning ! Model Error Occured";
        public static string ExceptionOrNullError = "Warning ! Error Occured";
        public static string SelectFile = "File is Attach File is Required";
        public static string EmailSendSuccess = "Success ! Email Send Successfully";
        public static string EmailSendFailure = "Warning ! Email Cannot Be Send";

        public static string ApproveSuccess = " Success ! Done Successfully";
        public static string ApproveFailure = "Warning !  Fail To Approve ";
        public static string AttandanceALreadyDone = "Warning !  Attandance Already Done for this Date ";

        public static string ConnectionSuccess = " Success ! Device Connected Successfully";

        public static string DataPullSuccess = " Success ! Records Added Successfully";

        public static string DataPullFail = " Warning ! Failed To Add Records";
        public static string ConnectionFailure = "Warning !  Fail To Connect To Device ";

        public static string ConnectDeviceFirst = "Warning !  Connect Device First";

        public static string NoRecordsInDevice = "Warning ! There is no Records in Device";

        public static string DisconnectDevice = "Success ! Device Disconnect Successfully";

        public static string ErrorOccured = "Error Occured While Processing";
    }

    public static class StatusCodeDescription
    {
        #region Basic Code and Description
        public static readonly int success = 200;

        public static readonly int failure = 401;

        public static readonly int invalidRequest = 402;

        public static readonly int invalidToken = 403;

        public static readonly string successMessage = "Success";

        public static readonly string FailureMessage = "Failed";

        public static readonly string invalidRequestMessage = "Invalid Request";

        public static readonly string invalidTokenMessage = "Token Invalid";

        public static readonly string noDataMessage = "No Data";
        #endregion

        #region Category
        public static readonly string categoryAddSuccess = "Category Added Successfully";

        public static readonly string categoryUpdateSuccess = "Category Update Process Successful";

        public static readonly string categoryDeleteSuccess = "Category Deletion Process Successful";

        public static readonly string categoryErrorMessage = "Category Save Process Failed";

        public static readonly string categoryDoNotExistMessage = "Category Does Not Exist";

        public static readonly string categoryDeleteFaliureMessage = "Category Deletion Process Unsuccessful";
        #endregion

        #region Glossary
        public static readonly string glossaryAddSuccess = "Glossary Added Successfully";

        public static readonly string glossaryUpdateSuccess = "Glossary Update Process Successful";

        public static readonly string glossaryDeleteSuccess = "Glossary Deletion Process Successful";

        public static readonly string glossaryErrorMessage = "Glossary Save Process Failed";

        public static readonly string glossaryExistsMessage = "Glossary Already Exists";

        public static readonly string glossaryDoNotExistMessage = "Glossary Does Not Exist";

        public static readonly string glossaryDeleteFaliureMessage = "Glossary Deletion Process Unsuccessful ";
        #endregion

        #region FAQ
        public static readonly string faqAddSuccess = "FAQ Added Successfully";

        public static readonly string faqUpdateSuccess = "FAQ Update Process Successful";

        public static readonly string faqDeleteSuccess = "FAQ Deletion Process Successful";

        public static readonly string faqErrorMessage = "FAQ Save Failed";

        public static readonly string faqDoNotExistMessage = "FAQ Does Not Exist";

        public static readonly string faqDeleteFaliureMessage = "FAQ Deletion Process Unsuccessful ";
        #endregion

        #region News
        public static readonly string newsAddSuccess = "News Added Successfully";

        public static readonly string newsUpdateSuccess = "News Update Process Successful";

        public static readonly string newsDeleteSuccess = "News Deletion Process Successful";

        public static readonly string newsErrorMessage = "News Save Process Failed";

        public static readonly string newsDoNotExistMessage = "News Does Not Exist";

        public static readonly string newsDeleteFaliureMessage = "News Deletion Process Unsuccessful";

        public static string NewsTagSaveMessage = " News New Tag Added Successfully";

        public static string NewsTagSaveFaliureMessage = "Tag Adding operation Failed";
        #endregion

        #region Course
        public static readonly string courseAddSuccess = "Course Added Successfully";

        public static readonly string courseUpdateSuccess = "Course Update Process Successful";

        public static readonly string courseDeleteSuccess = "Course Deletion Process Successful";

        public static readonly string courseErrorMessage = "Course Save Process Failed";

        public static readonly string courseDoNotExistMessage = "Course Does Not Exist";

        public static readonly string courseDeleteFaliureMessage = "Course Deletion Process Unsuccessful ";

        public static string CourseTagSaveMessage = " Course New Tag Added Successfully";

        public static string CourseTagSaveFaliureMessage = "Tag Adding operation Failed";
        #endregion

        #region Course Content
        public static readonly string courseContentAddSuccess = "Course Content Added Successfully";

        public static readonly string courseContentUpdateSuccess = "Course Content Update Process Successful";

        public static readonly string courseContentDeleteSuccess = "Course Content Deletion Process Successful";

        public static readonly string courseContentErrorMessage = "Course Content Save Process Failed";

        public static readonly string courseContentDoNotExistMessage = "Course Content Does Not Exist";

        public static readonly string courseContentDeleteFaliureMessage = "Course Content Deletion Process Unsuccessful";
        #endregion

        #region Course Page
        public static readonly string coursePageAddSuccess = "Course Page Added Successfully";

        public static readonly string coursePageUpdateSuccess = "Course Page Update Process Successful";

        public static readonly string coursePageDeleteSuccess = "Course Page Deletion Process Successful";

        public static readonly string coursePageErrorMessage = "Course Page Save Process Failed";

        public static readonly string coursePageDoNotExistMessage = "Course Page Does Not Exist";

        public static readonly string coursePageDeleteFaliureMessage = "Course Page Deletion Process Unsuccessful ";
        #endregion

        #region ArticleQuery
        public static readonly string queryNotSelectedMessage = "No Query Selected";

        #endregion

        #region InformationCenter
        public static string informationAddSuccess = "InformationCenter Added Successfully";

        public static string informationUpdateSuccess = "InformationCenter Update Process Successful";
        public static string informationUpdateFailure = "InformationCenter Update Process Failed";

        public static string informationDeleteSuccess = "InformationCenter Deletion Process Successful";

        public static string informationErrorMessage = "InformationCenter Save Process Failed";

        public static string informationDoNotExistMessage = "InformationCenter Does Not Exist";

        public static string informationDeleteFaliureMessage = "InformationCenter Deletion Process Unsuccessful";
        #endregion

        #region Notification
        public static string notificationAddSuccess = "Notification Added Successfully";

        public static string notificationUpdateSuccess = "Notification Update Process Successful";

        public static string notificationDeleteSuccess = "Notification Deletion Process Successful";

        public static string notificationErrorMessage = "Notification Save Process Failed";

        public static string notificationDoesnotExist = "Notification Not Exist";

        public static string notificationDeleteFaliureMessage = "Notification Deletion Process Unsuccessful ";
        #endregion

        #region InformationCenterCategory
        public static string informationCenterCategoryAddSuccess = "InformationCenterCategory Added Successfully";

        public static string informationCenterCategoryUpdateSuccess = "InformationCenterCategory Update Process Successful";

        public static string informationCenterCategoryDeleteSuccess = "InformationCenterCategory Deletion Process Successful";

        public static string informationCenterCategoryErrorMessage = "InformationCenterCategory Save Process Failed";

        public static string informationCenterCategoryDoNotExistMessage = "InformationCenterCategory Does Not Exist";

        public static string informationCenterCategoryDeleteFaliureMessage = "InformationCenterCategory deletion Process Unsuccessful  ";
        #endregion

        #region DataBaseBackup



        public static string databaseBackupErrorMessage = "Database Files Doesnot Exist;";
        public static string databaseBackupSuccessMessage = "Database Files Successfully loaded;";


        #endregion

        #region State
        public static string stateAddSuccess = "State Added Successfully";
        public static string stateAddFailure = "Failed To Add State";
        public static string stateUpdateSuccess = "State Update Process Successful";
        public static string stateUpdateFailure = "State Update Process Failure";
        public static string stateDeleteSuccess = "State Deletion Process Successful";

        public static string stateErrorMessage = "State Does Not Exist";
        public static string stateDeleteFailure = "State Deletion Process Failure";
        #endregion

        #region City
        public static string cityAddSuccess = "City Added Successfully";
        public static string cityAddFailure = " Failed To Add City";
        public static string cityUpdateSuccess = "City Update Process Successful";
        public static string cityUpdateFailure = "City Update Process Failed";
        public static string cityDeleteSuccess = "City Deletion Process Successful";

        public static string cityErrorMessage = "City Does Not Exist";
        public static string cityDeleteFailure = "City Deletion Process Failure";
        #endregion


        #region Quiz
        public static string QuizAddSuccess = "Quiz Added Successfully";

        public static string QuizUpdateSuccess = "Quiz Updated Successfully";

        public static string QuizDeleteSuccess = "Quiz Deleted Successfully";

        public static string QuizDependencyDeleteMessage = "Quiz is in use";

        public static string QuizErrorMessage = "Quiz Save Failed";

        public static string QuizDonotExist = "Quiz Does Not Exist";

        public static string QuizDeleteFailure = "Quiz not be Deleted";

        public static string QuizTagSaveMessage = " Quiz New Tag Added Successfully";

        public static string QuizTagSaveFaliureMessage = "Tag Adding operation  Failed";
        #endregion

        #region Quiz Question
        public static string QuizQuestionDeleteSuccess = "Question Deleted Successfully";

        public static string QuizQuestionDependencyMessage = "Question is in use";

        public static string QuizQuestionDeleteMessage = "Question cannot be deleted";

        public static string QuizQuestionAddSuccess = "Question Added Successfully";

        public static string QuizQuestionUpdateSuccess = "Question Updated Successfully";

        public static string QuizQuestionErrorMessage = "Question Save Failed";

        public static string QuizQuestionDonotExist = "Question Does Not Exist";

        #endregion


        #region Entrance
        public static string EntranceAddSuccess = "Entrance Added Successfully";

        public static string EntranceUpdateSuccess = "Entrance Updated Successfully";

        public static string EntranceDeleteSuccess = "Entrance Deleted Successfully";

        public static string EntranceDependencyDeleteMessage = "Entrance is in use";

        public static string EntranceErrorMessage = "Entrance Save Failed";

        public static string EntranceDonotExist = "Entrance Does Not Exist";

        public static string EntranceDeleteFailure = "Entrance not be Deleted";

        public static string EntranceTagSaveMessage = "Entrance New Tag Added Successfully";

        public static string EntranceTagSaveFaliureMessage = "Tag Adding operation  Failed";
        #endregion

        #region Entrance Question
        public static string EntranceQuestionDeleteSuccess = "Question Deleted Successfully";

        public static string EntranceQuestionDependencyMessage = "Question is in use";

        public static string EntranceQuestionDeleteMessage = "Question cannot be deleted";

        public static string EntranceQuestionAddSuccess = "Question Added Successfully";

        public static string EntranceQuestionUpdateSuccess = "Question Updated Successfully";

        public static string EntranceQuestionErrorMessage = "Question Save Failed";

        public static string EntranceQuestionDonotExist = "Question Does Not Exist";

        #endregion



        #region Survey
        public static string SurveyAddSuccess = "Survey Added Successfully";

        public static string SurveyUpdateSuccess = "Survey Updated Successfully";

        public static string SurveyDeleteSuccess = "Survey Deleted Successfully";

        public static string SurveyErrorMessage = "Survey Save Failed";

        public static string SurveyDonotExist = "Survey Does Not Exist";

        public static string SurveyDeleteFailure = "Survey Not Deleted";

        public static string SurveyDependencyMessage = "Survey is in use";

        public static string SurveyTagSaveMessage = " survey New Tag Added Successfully";

        public static string SurveyTagSaveFaliureMessage = "Tag Adding operation  Failed";
        #endregion

        #region Survey Question
        public static string SurveyQuestionDeleteSuccess = "Question Deleted Successfully";

        public static string SurveyQuestionDependencyMessage = "Question is in use";

        public static string SurveyQuestionDeleteMessage = "Question cannot be deleted";

        public static string SurveyQuestionAddSuccess = "Question Added Successfully";

        public static string SurveyQuestionUpdateSuccess = "Question Updated Successfully";

        public static string SurveyQuestionErrorMessage = "Question Save Failed";

        public static string SurveyQuestionDonotExist = "Question Does Not Exist";
        #endregion

        #region UserGroup
        public static readonly string userGroupAddSuccess = "User Group Added Successfully";

        public static readonly string userGroupUpdateSuccess = "User Group Update Process Successful";

        public static readonly string userGroupDeleteSuccess = "User Group Deletion Process Successful";

        public static readonly string userGroupErrorMessage = "User Group Save Process Failed";

        public static readonly string userGroupExistsMessage = "User Group Already Exists";

        public static readonly string userGroupDoNotExistMessage = "User Group Does Not Exist";

        public static readonly string userGroupDeleteFaliureMessage = "User Group Deletion Process Failure";
        #endregion

        #region Article
        public static string ArticleDraftedSuccess = " Article Drafted Successfully";
        public static string ArticleAddSuccess = " Article Added Successfully";

        public static string ArticleUpdateSuccess = " Article Update Process Successful";

        public static string ArticleDeleteSuccess = " Article Deletion Process Successful";

        public static string ArticleDraftErrorMessage = " Article Drafting Process Failed";

        public static string ArticleErrorMessage = " Article Save Process Failed";

        public static string ArticleDoNotExistMessage = " Article Does Not Exist";

        public static string ArticleDeleteFaliureMessage = " Article Deletion Process Unsuccessful";

        public static string ArticleTagSaveMessage = " Article New Tag Added Successfully";

        public static string ArticleTagSaveFaliureMessage = "Tag Adding operation Failed";
        #endregion

        #region Template
        public static string TemplateAddSuccess = " Template Added Successfully";

        public static string TemplateUpdateSuccess = " Template Update Process Successfully";

        public static string TemplateDeleteSuccess = " Template Deletion Process Successful";

        public static string TemplateErrorMessage = " Template Save Process Failed";

        public static string TemplateDoNotExistMessage = " Template Does Not Exist";

        public static string TemplateDeleteFaliureMessage = " Template Deletion Process Failure";


        #endregion

        #region Users
        public static string UserAddSuccess = " User Added Successfully";

        public static string UserUpdateSuccess = " User Update Process Successful";

        public static string UserDeleteSuccess = " User Deletion Process Successful";

        public static string UserErrorMessage = " User Save Process Failed";

        public static string UserDoNotExistMessage = " User Not Exist";

        public static string UserNameExistMessage = " User Name Already Exists";

        public static string UserDeleteFaliureMessage = " User Deletion Process Unsuccessful ";
        #endregion

        #region Department
        public static string departmentAddSuccess = " Department Added Successfully";
        public static string departmentAddFailure = " Department Save Process Failed";
        public static string departmentUpdateSuccess = " Department Update Process Successful";
        public static string departmentUpdateFailure = " Department Update Process Failed";
        public static string departmentDeleteSuccess = " Department Deletion Process Successful";
        public static string departmentErrorMessage = " Department Does Not Exist";
        public static string departmentDeleteFailure = " Department Deletion Process Unsuccessful";
        #endregion

        #region Designation
        public static string designationAddSuccess = " Designation Added Successfully";
        public static string designationAddFailure = " Designation Save Process Failed";
        public static string designationUpdateSuccess = " Designation Update Process Successful";
        public static string designationUpdateFailure = " Designation Update Process Failed";
        public static string designationDeleteSuccess = " Designation Deletion Process Successful";
        public static string designationErrorMessage = " Designation Does Not Exist";
        public static string designationDeleteFailure = " Designation Deletion Process Unsuccessful";
        #endregion

        #region User Profile

        public static string UserProfileUpdateSuccess = " Profile Updated Successfully";

        public static string UserProfileUpdateFaliure = " Profile Not Updated";

        #endregion

        #region Query
        public static string QueryArchivedSuccess = "Query Archived Successfully";
        public static string QueryDeleteSuccess = "Query Deleted Successfully";

        public static string QueryRepliedSuccess = "Query Replied Successfully";

        #endregion

        #region SMS
        public static string SMSSentSuccess = "SMS Sent Successfully";
        public static string NotInTestMode = "Not In Test Mode";
        public static string InValidPhoneNumber = "Invalid Phonenumber";
        #endregion

        #region LDAP
        public static string LDAPSyncStarted = "LDAP Sync Started";
        public static string LDAPSyncFailed = "LDAP Data could not be synced due to internal error";
        #endregion
    }
}

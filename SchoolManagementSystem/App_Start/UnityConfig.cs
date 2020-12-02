using System.Web.Mvc;
using Microsoft.Practices.Unity;
using Unity.Mvc5;
using DomainInterface;
using Infrastructure;
using InfrastructureData;
using System.Web.Http;

namespace SchoolManagementSystem
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();
            
           
            
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
          

            container.RegisterType<IMessageHandlerRepository, MessageHandlerRepository>();
            container.RegisterType<IShoolTypeRepository, SchoolTypeRepository>();
            container.RegisterType<IDropDownRepository, DropDownRepository>();
            container.RegisterType<ISchoolInformationRepository, SchoolInformationRepository>();
            container.RegisterType<ISessionInfoRepository, SessionInfoRepository>();
            container.RegisterType<IBusInfoRepository, BusInfoRepository>();
            container.RegisterType<IFeeTypeRepository, FeeTypeRepository>();
            container.RegisterType<IHostelInfoRepository, HostelIndoRepository>();
            //container.RegisterType<ILocationInfoRepository, LocationRepository>();
            container.RegisterType<IClassTypeRepository, ClassTypeRepository>();
            container.RegisterType<ISectionRepository, SectionRepository>();
            container.RegisterType<IHouseInfoRepository, HouseInfoRepository>();
            container.RegisterType<IClassRepository, ClassRepository>();
            container.RegisterType<IFacultyRepository, FacultyRepository>();
            container.RegisterType<IStudentsRepository, StudentsRepository>();
            container.RegisterType<IDesignationRepository, DesignationRepository>();
            container.RegisterType<IDepartmentRepository, DepartmentRepository>();
            container.RegisterType<IStudentsCategoryRepository, StudentsCategoryRepository>();
            container.RegisterType<ISubjectsRepository, SubjectsRepository>();
            container.RegisterType<ITermMasterRepository, TermMasterRepository>();
            container.RegisterType<IGradeMasterRepository, GradeMasterRepository>();
            container.RegisterType<IMarksEntryRepository, MarksEntryRepository>();
            container.RegisterType<IEditMarksEntryRepository, EditMarksEntryRepository>();
            container.RegisterType<IMarkSheetLedgerRepository, MarksSheetLedgerRepository>();
            container.RegisterType<IClassMasterRepository, ClassMasterRepository>();
            container.RegisterType<IStudentsAttandanceRepository, StudentsAttandanceRepository>();
            container.RegisterType<IMarksSheetPrintRepository, MarksSheetPrintRepository>();
            container.RegisterType<ICommonFeeRepository, CommonFeeRepository>();
            container.RegisterType<ICommonFeeDiscountRepository, CommonFeeDiscountRepository>();
            container.RegisterType<IPersonalFeeRepository, PersonalFeeRepository>();
            container.RegisterType<IFeeCollectionRepository, FeeCollectionRepository>();
            container.RegisterType<IJobTypeRepository, JobTypeRepository>();
            container.RegisterType<ILeaveTypeRepository, LeaveTypeRepository>();
            container.RegisterType<ITaxMasterRepository, TaxMasterRepository>();
            container.RegisterType<ISalaryHeadRepository, SalaryHeadingRepository>();
            container.RegisterType<ISalaryHeadSettingsRepository, SalaryHeadingSettingsRepository>();
            container.RegisterType<IEmployerRepository, EmployerRepository>();
            container.RegisterType<ILeavEntryRepository, LeaveEntryRepository>();
            container.RegisterType<ILeaveDaysRepository, LeaveDaysRepository>();
            container.RegisterType<IAccumulativeLeaveRepository, AccumulativeLeaveRepository>();
            container.RegisterType<IUserGroupRepository, UserGroupRepository>();
            container.RegisterType<IFeeDailyCollectionRepository, FeeDailyCollectionRepository>();
            container.RegisterType<IUserRole, UserRoleRepository>();
            container.RegisterType<IUserRepository, UserRepository>();
            container.RegisterType<ILoginRepository, LoginRepository>();
            container.RegisterType<INotificationRepository,NotificationRepository>();
            container.RegisterType<IStudentAssignmentsRepository, StudentsAssignmentsRepository>();
            container.RegisterType<ISettingsRepository, SettingsRepository>();
            container.RegisterType<IOrganisationEventsRepository, OrganisationEventsRepository>();
            container.RegisterType<IStudentsDailyAttandanceRepository, StudentsDailyAttandanceRepository>();
            container.RegisterType<IManageCalendarRepository, ManageCalendarRepository>();
            container.RegisterType<IMonthsRepository, MonthsRepository>();

            container.RegisterType<IQuizReport, QuizReportRepository>();
            container.RegisterType<IQuizCategoryReport, QuizCategoryReportRepository>();
            container.RegisterType<IQuizQuestionReportRepository, QuizQuestionReportRepository>();
            container.RegisterType<IQuizQuestionRepository, QuizQuestionRepository>();
            container.RegisterType<IQuizRepository, QuizRepository>();
            container.RegisterType<IQuizClientRepository, QuizClientRepository>();
            container.RegisterType<IQuizUserReportRepository, QuizUserReportRepository>();
            container.RegisterType<IDashBoardRepository, DashBoardRepository>();
            container.RegisterType<ITakeLeaveRepository, TakeLeaveRepository>();
            container.RegisterType<IOfficialLeaveRepository, OfficialLeaveRepository>();
            container.RegisterType<ITakeAccumulativeLeaveRepository, TakeAccumulativeLeaveRepository>();
            container.RegisterType<IEmployeeDailyAttandanceRepository, EmployeeDailyAttandanceRepository>();
            container.RegisterType<IManualAttandanceRepository, ManualAttandanceRepository>();
            container.RegisterType<IManagePublicHolidayAndSaturday, ManagePublicHolidayAndSaturday>();
            container.RegisterType<ITravellingAllowanceRepository, TravellingAllownceRepository>();
            container.RegisterType<ITakeAdvanceRepository, TakeAdvanceRepository>();
            container.RegisterType<ISalaryHeadAmountRepository, SalaryHeadAmountRepository>();
            container.RegisterType<ISalarCalculationRepository, SalaryCalculationRepository>();

            container.RegisterType<IQuizReport, QuizReportRepository>();
            container.RegisterType<IQuizCategoryReport, QuizCategoryReportRepository>();
            container.RegisterType<IQuizQuestionReportRepository, QuizQuestionReportRepository>();
            container.RegisterType<IQuizQuestionRepository, QuizQuestionRepository>();
            container.RegisterType<IQuizRepository, QuizRepository>();
            container.RegisterType<IQuizClientRepository, QuizClientRepository>();
            container.RegisterType<IQuizUserReportRepository, QuizUserReportRepository>();

            container.RegisterType<ICategoryTreeRepository, CategoryTreeRepository>();
            container.RegisterType<IEntranceQuestionRepository, EntranceQuestionRepository>();

            container.RegisterType<IEntranceRepository, EntranceRepository>();
            container.RegisterType<IEntranceClientRepository, EntranceClientRepository>();

            container.RegisterType<IEntranceUserReportRepository, EntranceUserReportRepository>();
            container.RegisterType<IReportRepository, ReportRepository>();

            container.RegisterType<IMobileRepository, MobileRepository>();

            container.RegisterType<IYearlyHolidaysEntryRepository, YearlyHolidaysEntryRepository>();
            container.RegisterType<ILanguageRepository, LanguageRepository>();

            container.RegisterType<ILanguageParameterRepository, LanguageParameterRepository>();


            container.RegisterType<IClientLoginRepository, ClientLoginRepository>();
            container.RegisterType<IParentsChildRepository, ParentsChildRepository>();
            container.RegisterType<IStudentsProfileRepository, StudentsProfileRepository>();
            container.RegisterType<IFeeDetailsForClientRepository, FeeDetailsForClientRepository>();
            container.RegisterType<IClientResultRepository, ClientResultRepository>();
            container.RegisterType<IChangePasswordRepository, ChangePasswordRepository>();
            container.RegisterType<IForgotPasswordRepository, ForgotPasswordRepository>();
            container.RegisterType<IMailSendRepository, MailSendRepository>();

            GlobalConfiguration.Configuration.DependencyResolver = new Unity.WebApi.UnityDependencyResolver(container);

        }
    }
}
using DomainInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainEntities;
using System.Data;
using Dapper;

namespace Infrastructure
{
    public class ReportRepository : IReportRepository
    {
        public List<Attandance> GetAllAttandanceReports(Report reports)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@Month", reports.Month);
                    param.Add("@Year", reports.Year);
                    param.Add("@User", new LoginUser().LoggedInEmployeeID);
                    List<Attandance> attandanceList = SqlMapper.Query<Attandance>(connection, "[dbo].[GetAllUserAttandance]", param, commandType: CommandType.StoredProcedure).ToList();
                    return attandanceList;
                }
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public SalaryList GetAllSalaryReport(Report reports)
        {
            try
            {
                var listSalary = new SalaryList();
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@Month", reports.Month);
                    param.Add("@Year", reports.Year);                  
                                  
                    List<SalaryReport> salaryList = SqlMapper.Query<SalaryReport>(connection, "[dbo].[GetAllUserSalaryList]", param, commandType: CommandType.StoredProcedure).OrderBy(x=>x.SortOrder).ToList();

                    List<SalaryHead> headingList = SqlMapper.Query<SalaryHeading>(connection, "[dbo].[GetAllSalaryHeading]", commandType: CommandType.StoredProcedure).Where(x=>x.IsTaxSaving!=true).OrderBy(x=>x.SortOrder).Select(x=>new SalaryHead() {
                        HeadName=x.HeadName
                    }).ToList();

                    var test = salaryList.GroupBy(x => x.EmployeeID);
                   
                    listSalary.SalaryHead = new List<SalaryHead>();
                    listSalary.SalaryHead = headingList;

                    listSalary.SalaryData = new List<SalaryReport>();
                    listSalary.SalaryData = salaryList;


                    return listSalary;                  


                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public SalarySavingList GetSalarySavingsReport(SavingsReport reports)
        {
            try
            {
                var listSalary = new SalarySavingList();
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@EmployeeID", reports.EmployeeID);
                    param.Add("@SavingsTypeID", reports.SavingsTypeID);
                    listSalary.SalarySavings = SqlMapper.Query<SavingViewModel>(connection, "[dbo].[GetAllSalarySavingsOfEmployee]", param, commandType: CommandType.StoredProcedure).ToList();
                    return listSalary;
                                 }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DailyAttandanceListViewModel GetDailyAttandanceReport(DailyAttandanceReport reports)
        {
            try
            {
                var listDailyAttandance = new DailyAttandanceListViewModel();
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@OrganisationID", reports.OrganisationID);
                    param.Add("@Date", reports.Date);
                    param.Add("@Year", reports.Year);
                    param.Add("@Month", reports.Month);
                    listDailyAttandance.DailyAttandanceList = SqlMapper.Query<DailyAttandanceList>(connection, "[dbo].[GetDailyAttandanceReport]", param, commandType: CommandType.StoredProcedure).ToList();

                    return listDailyAttandance;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public List<TakeLeave> GetAllTakeLeaveAndPublicHoliday(Report reports)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@Month", reports.Month);
                    param.Add("@Year", reports.Year);
                    param.Add("@User", new LoginUser().LoggedInEmployeeID);
                    List<TakeLeave>leaveList = SqlMapper.Query<TakeLeave>(connection, "[dbo].[GetAllUserLeaveAndHolidays]", param, commandType: CommandType.StoredProcedure).ToList();
                    return leaveList;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public EmployeeSalaryInfo GetEmployeeSalaryDetails(SalarySlip salaryslip)
        {
            EmployeeSalaryInfo employeeSalInfo = new EmployeeSalaryInfo();
            try
            {
                //SalaryInfoDetail
               
                
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@Month", salaryslip.Month);
                    param.Add("@Year", salaryslip.Year);
                    param.Add("@EmployeeID", salaryslip.EmployeeID);
                    param.Add("@OrganisationID", salaryslip.OrganisationID);
                    employeeSalInfo.EmployeeDetails = SqlMapper.Query<EmployeeDetails>(connection, "[dbo].[GetEmployeeInfoAndLeaveDetails]", param, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    var data = SqlMapper.Query<SalaryInfoDetail>(connection, "[dbo].[GetSalaryInfoOfUser]", param, commandType: CommandType.StoredProcedure).ToList();
                   
                    if (data.Count() > 0)
                    {
                         employeeSalInfo.AddSalaryDetails = data.Where(x => x.IsBasicSalary == true || x.IsAdd == true).Select(x => new SalaryDetails() {
                            SalaryHeading = x.SalHeadingName,
                            Amount = x.Amount.ToString()
                        }).ToList();
                        employeeSalInfo.SalaryDeductionDetails= data.Where(x => x.IsTax).Select(x => new SalaryDetails()
                        {
                            SalaryHeading = x.SalHeadingName,
                            Amount = x.Amount.ToString()
                        }).ToList();
                        employeeSalInfo.TotalSaving = data.Where(x => x.IsSaving).ToList().Sum(y => y.Amount).ToString();
                        employeeSalInfo.GrossSalary = data.Where(x => x.IsSalaryCalculatePoint==true).OrderBy(x => x.SortOrder).ToList()[0].Amount.ToString();
                        employeeSalInfo.FinalSalary = data.Where(x => x.IsSalaryCalculatePoint==true).OrderBy(x => x.SortOrder).ToList()[1].Amount.ToString();
                        employeeSalInfo.TotalDeduction = employeeSalInfo.SalaryDeductionDetails.Sum(x => Convert.ToDecimal(x.Amount)).ToString();

                    }                   
                    else
                    {
                        return employeeSalInfo;
                    }
                   




                    return employeeSalInfo;
                }
            }
            catch (Exception ex)
            {
                return employeeSalInfo;
            }
        }



        public MonthlyAttandanceSummaryDetails GetEmployeeMonthlySummaryAttandanceDetails(MonthlyAttandanceSummaryReport salaryslip)
        {
            MonthlyAttandanceSummaryDetails employeeAttendSummaryInfo = new MonthlyAttandanceSummaryDetails();
            List<MonthlyAttandanceSummary> monthlyAttandanceSummary = new List<MonthlyAttandanceSummary>();
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@Month", salaryslip.Month);
                    param.Add("@Year", salaryslip.Year);
                    param.Add("@EmployeeID", salaryslip.EmployeeID);
                    param.Add("@OrganisationID", salaryslip.OrganisationID);
                    var summaryData = SqlMapper.Query<MonthlyAttandanceSummary>(connection, "[dbo].[GetEmployeeMonthlyAttandanceSummary]", param, commandType: CommandType.StoredProcedure).ToList();
                    var userDetails = SqlMapper.Query<MonthlyAttandanceSummary>(connection, "[dbo].[GetEmployeeDetails]", param, commandType: CommandType.StoredProcedure).FirstOrDefault();

                    if (summaryData.Count() > 0 && summaryData!=null)
                    {
                        foreach(var attendSummary in summaryData)
                        {
                            if (attendSummary.DateFrom.Date == attendSummary.DateTo.Date)
                            {
                                monthlyAttandanceSummary.Add(new MonthlyAttandanceSummary()
                                {
                                    DateFrom=attendSummary.DateFrom,
                                    Type=attendSummary.Type
                                });
                            }
                            else if (attendSummary.DateFrom.Date != attendSummary.DateTo.Date)
                            {
                                int days = attendSummary.DateTo.Date.Day-(attendSummary.DateTo.Day) + 1;
                               for (int i=0; i < days; i++)
                                {
                                    monthlyAttandanceSummary.Add(new MonthlyAttandanceSummary()
                                    {
                                        DateFrom = attendSummary.DateFrom.AddDays(i),
                                        Type = attendSummary.Type
                                    });
                                }
                            }
                        }

                        employeeAttendSummaryInfo.MonthlyAttandanceSummary = monthlyAttandanceSummary.OrderBy(x=>x.DateFrom).ToList();
                       
                    }
                    employeeAttendSummaryInfo.Name = userDetails.Name;
                    employeeAttendSummaryInfo.Designation = userDetails.Designation;
                    employeeAttendSummaryInfo.Organisation = userDetails.Organisation;
                    employeeAttendSummaryInfo.TotalDaysInMonth = userDetails.TotalDaysInMonth;
                    return employeeAttendSummaryInfo;
                }
            }
            catch (Exception ex)
            {
                return employeeAttendSummaryInfo; 
            }
        }
    }
}

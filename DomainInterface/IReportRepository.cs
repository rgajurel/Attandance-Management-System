using DomainEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainInterface
{
   public interface IReportRepository
    {
        List<Attandance> GetAllAttandanceReports(Report reports);

        List<TakeLeave> GetAllTakeLeaveAndPublicHoliday(Report reports);       
        SalaryList GetAllSalaryReport(Report reports);

        SalarySavingList GetSalarySavingsReport(SavingsReport reports);

        DailyAttandanceListViewModel GetDailyAttandanceReport(DailyAttandanceReport reports);

        EmployeeSalaryInfo GetEmployeeSalaryDetails(SalarySlip salaryslip);

        MonthlyAttandanceSummaryDetails GetEmployeeMonthlySummaryAttandanceDetails(MonthlyAttandanceSummaryReport salaryslip);

    }
}

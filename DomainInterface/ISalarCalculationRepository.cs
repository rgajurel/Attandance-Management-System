using DomainEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainInterface
{
   public interface ISalarCalculationRepository
    {
        List<LeaveNameDays> GetEmployeeLeaveList(string id,string year,string month);
        List<AttandanceDays> AttandanceInformation(string id, string year, string month);
        ListSalaryInfoAdd GetEmployeeSalaryInfo(string id);
        ListSalaryInfoAdd CalculateTax(decimal taxableamount,string employeeid);
        int DeleteData(SalaryCalculate salaryHeadAmount);
        int SalaryBatchUpload(List<SalaryCalculate> leaveEntry);


    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
  public  class SalaryCalculation
    {

    }
    public class LeaveNameDays
    {
        public string LeaveName { get; set; } 
        public string Days { get; set; }
        public string Total { get; set; }
    }

    public class AttandanceDays
    {
        public string AttandanceName { get; set; }
        public string Days { get; set; }
        public string Total { get; set; }
    }

    public class ListSalaryInfoAdd
    {
        public List<SalaryInfo> SalInfoAdd { get; set; }
        public List<SalaryInfo> SalInfoSaving { get; set; }
        public SalaryInfo SalAddInfoTotal { get; set; }
        public SalaryInfo SalInfoSavingTotal { get; set; }
        public SalaryInfo SalInfoFinalTotal { get; set; }
        public List<SalaryInfo> TaxInfo { get; set; }
    }

    public class SalaryCalculate
    {
        public int ID { get; set; }
        public string SalaryHeadingID { get; set; }
        public string SalHeadingName { get; set; }
        public decimal Amount { get; set; }
        public int SortOrder { get; set; }
        public string Month { get; set; }
        public string Year { get; set; }
        public string EmployeeID { get; set; }
        
    }

    public class SalaryInfo
    {
        public string SalaryHeadingID { get; set; }
        public string SalHeadingName { get; set; }
        public decimal Amount { get; set; }
        public int SortOrder { get; set; }
    }
    public class SalaryInfoDetail
    {
        public string SalaryHeadingID { get; set; }
        public string SalHeadingName { get; set; }
        public decimal Amount { get; set; }
        public int SortOrder { get; set; }
        public bool IsAdd { get; set; }
        public bool IsBasicSalary { get; set; }
        public bool IsTaxSaving { get; set; }
        public bool IsTax { get; set; }
        public bool IsSaving { get; set; }
        public bool IsSalaryCalculatePoint { get; set; }

    }
    public enum GetTotalMonths
    {
        Months=12
    }
}

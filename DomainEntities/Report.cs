using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
  public  class Report
    {
        [DisplayName("Year")]
        [Required(ErrorMessage = "Year is Required")]
        public int Year { get; set; }

        [DisplayName("Month")]
        [Required(ErrorMessage = "Month is Required")]
        public int Month { get; set; }
    }

    public class DailyAttandanceList
    {
        public string Employee { get; set; }
        public string Designation { get; set; }

        public string ActualEntryTime { get; set; }
        public string ActualExitTime { get; set; }
        public string EntryTime { get; set; }
        public string ExitTime { get; set; }
        public string ActualWorkingHours { get; set; }
        public string WorkingHours { get; set; }
        public string OverTime { get; set; }
        public bool IsLateEntry { get; set; }
    }
    public class DailyAttandanceListViewModel
    {
        public string Organisation { get; set; }
        public string Year { get; set; }
        public string Month { get; set; }
        public string Date { get; set; }
        public bool IsExport { get; set; }
        public List<DailyAttandanceList> DailyAttandanceList { get; set; }
    }
    public class DailyAttandanceReport: Report
    {
        [DisplayName("Organisation")]
        [Required(ErrorMessage = "Organisation is Required")]
        public int? OrganisationID { get; set; }

        [DisplayName("Date")]
        [Required(ErrorMessage = "Date is Required")]
        public DateTime? Date { get; set; }


        [DisplayName("Date")]
        [Required(ErrorMessage = "Date is Required")]
        public string NepaliDate { get; set; }
    }

    public class MonthlyAttandanceSummaryReport : Report
    {
        [DisplayName("Organisation")]
        [Required(ErrorMessage = "Organisation is Required")]
        public int OrganisationID { get; set; }

        [DisplayName("Employee")]
        [Required(ErrorMessage = "Employee is Required")]
        public int EmployeeID { get; set; }


    }

    public class MonthlyAttandanceSummaryDetails
    {
        public string  Name { get; set; }
        public string Designation { get; set; }
        public string Organisation { get; set; }
        public int TotalDaysInMonth { get; set; }
        public List<MonthlyAttandanceSummary> MonthlyAttandanceSummary { get; set; }
    }

    public class MonthlyAttandanceSummary
    {
        public string Name { get; set; }
        public string Designation { get; set; }
        public string Organisation { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public string Days { get; set; }
        public int TotalDaysInMonth { get; set; }
        public string Type { get; set; }
    }

    public class SavingsReport
    {
        [Required(ErrorMessage = "This Field is required")]

        [Display(Name ="Savings Type")]
        public string SavingsTypeID { get; set; } 

        [Required(ErrorMessage = "This Field is required")]
        [Display(Name = "Organisation")]
        public string OrganisationID { get; set; }

        [Required(ErrorMessage = "This Field is required")]
        [Display(Name ="Employee")]
        public string EmployeeID { get; set; }
    }



    public class SalarySlip
    {
        [Required(ErrorMessage ="This Field is required")]
        public int Year { get; set; }

        [Required(ErrorMessage = "This Field is required")]
        public int Month { get; set; }

        [Required(ErrorMessage = "This Field is required")]
        public int OrganisationID { get; set; }

        [Required(ErrorMessage = "This Field is required")]
        public int EmployeeID { get; set; }
    }

    public class EmployeeSalaryInfo
    {
        public string FinalSalary { get; set; }
        public string TotalDeduction { get; set; }
        public string TotalSaving { get; set; }         
        public string GrossSalary { get; set; }     
        public  EmployeeDetails EmployeeDetails { get; set; }
        public List<SalaryDetails> AddSalaryDetails { get; set; }
        public List<SalaryDetails> SalaryDeductionDetails { get; set; }
    }
    public class SalaryDetails
    {
        public string SalaryHeading { get; set; }
        public string Amount { get; set; }
    }



    public class EmployeeDetails
    {
        public string Name { get; set; }
        public string Designation { get; set; }
        public string Department { get; set; }
        public string JoiningDate { get; set; }
        public string Employment { get; set; }
        public string PANNumber { get; set; }
        public string PFNumber { get; set; }
        public string CITNumber { get; set; }
        public string DailyHour { get; set; }
        public string BankAccountNumber { get; set; }
        public string OrganisationAddress { get; set; }


        public string HolidaysInMonth { get; set; }
        public string TotalDaysinMonth { get; set; }
        public string TotalPaidLeaveTaken { get; set; }
        public string TotalUnpaidLeaveTaken { get; set; }
        public string TotalPresentDays { get; set; }
        public string TotaAbsentDays { get; set; }

        public string TotalWorkingDays { get; set; }




    }

    public class SalaryReport
    {
        public string Name { get; set; }
        public string Amount { get; set; }
        public int SortOrder { get; set; }
        public string EmployeeID { get; set; }
    }
    public class SalaryHead
    {
        public string HeadName { get; set; }
    }
    public class SalaryList
    {
        public string Year { get; set; }
        public string Month { get; set; }
        public bool IsExport { get; set; }
       public List<SalaryHead> SalaryHead { get; set; }
       public List<SalaryReport> SalaryData { get; set; }
    }

    public class SalarySavingList
    {
        public string Organisation { get; set; }
        public string Employee { get; set; }
        public string SavingType { get; set; }
        public bool IsExport { get; set; }
        public List<SavingViewModel> SalarySavings { get; set; }
        
    }

    public class SavingViewModel
    {
        public string Amount { get; set; }
        public string Month { get; set; }
        public string Year { get; set; }
    }

}

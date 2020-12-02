using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace DomainEntities
{
 public class Employee
    {
        public int? ID { get; set; }
        public int SN { get; set; }

        [Required(ErrorMessage ="Name is Required")]
        [DisplayName("Name")]
        public string Name { get; set; }

        public HttpPostedFileBase imageFile { get; set; }

        public string Image { get; set; }
        public string Gender { get; set; }

        [DisplayName("Joined Date (A.D)")]
        [Required(ErrorMessage = "Joined Date Is Required")]
        public DateTime EnglishJoioningDate { get; set; }

        [DisplayName("Nepali Joined Date (B.S)")]
        [Required(ErrorMessage = "Joined Date is Required")]

        public DateTime? NepaliJoioningDate { get; set; }

        [DisplayName("Birth Date (A.D)")]

        [Required(ErrorMessage = "Date Of Birth  Is Required")]
        public DateTime EnglishDateOfBirth { get; set; }

        [DisplayName("Nepali Birth Date (A.D)")]
        [Required(ErrorMessage = "Joined Date is Required")]
        public DateTime? NepaliDateOfBirth { get; set; }

        [Required(ErrorMessage = "Entry Time is Required")]
        [DisplayName("Entry Time")]
        public TimeSpan EntryTime { get; set; }


        [Required(ErrorMessage = "Exit Time is Required")]

        [DisplayName("Exit Time")]       
        public TimeSpan ExitTime { get; set; }

        public string EntryimeString { get { return EntryTime.Hours + ":" + EntryTime.Minutes + ":" + EntryTime.Seconds; } }

        public string ExitimeString { get { return ExitTime.Hours + ":" + ExitTime.Minutes + ":" + ExitTime.Seconds; } }


        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        public string MobileNo { get; set; }

        [DisplayName("CitizenShip No")]
        public string CitizenshipNo { get; set; }

        public string PhoneNo { get; set; }
        public string Qualifications { get; set; }

        [Required(ErrorMessage = "JobType is Required")]
        [DisplayName("JobType")]
        public int JobTypeID { get; set; }

        [Required(ErrorMessage = "Organisation is Required")]

        [DisplayName("Organisation")]
        public int OrganisationID { get; set; }

        [Required(ErrorMessage = "Department is Required")]

        [DisplayName("Department")]
        public int DepartmentID { get; set; }

        [Required(ErrorMessage = "Designation is Required")]
        [DisplayName("Designation")]
        public int DesignationID { get; set; }

        [Required(ErrorMessage = "UserID is Required")]
        [Remote("CheckExistingUserID", "Employer", ErrorMessage = "UserID already exists!", AdditionalFields ="ID")]

        [DisplayName("Device UserID")]
        public int UserID { get; set; }

        [DisplayName("Father Name")]
        public string FatherName { get; set; }

        [DisplayName("Temporaty Address")]
        public string TemporaryAddress { get; set; }

        [DisplayName("Permanent Address")]
        public string PermanentAddress { get; set; }

        [DisplayName("Bank Account Number")]
        public string BankAccountNo { get; set; }
        [DisplayName("Provident Fund Number")]
        public string PFNumber { get; set; }
        [DisplayName("CIT Number")]
        public string CITNumber { get; set; }

        [DisplayName("Employee Code")]
        public string EmpCode { get; set; }

        [DisplayName("PAN Number")]
        public string PANNumber { get; set; }
        public string Status { get; set; }
        public string AddedBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime AddedOn { get; set; }
        public DateTime UpdatedOn { get; set; }

        //for detatils

        public string Organisation { get; set; }
        public string DepartmentName { get; set; }
        public string Designation { get; set; }
        public string JobTypeName { get; set; }
    }

    public class EmployeeSearch
    {
        public int ID { get; set; }
        public int SN { get; set; }
        public int Total { get; set; }
        public int pageSize { get; set; }
        public int pageNumber { get; set; }
        public int offset { get; set; }


        public bool IsView { get; set; }

        public string Organisation { get; set; }
        public string DepartmentName { get; set; }
        public string Designation { get; set; }
        public string Image { get; set; }
        public string  EmployerSearchName { get; set; }
        public int OrganisationSearchID { get; set; }
        public int DepartmentSearchID { get; set; }
        public int DesignationSearchID { get; set; }

        public string Name { get; set; }

        public int UserIDSearch { get; set; }

        public TimeSpan EntryTime { get; set; }


        public string EntryimeString { get { return EntryTime.Hours + ":" + EntryTime.Minutes + ":" + EntryTime.Seconds; } }

        public string ExitimeString { get { return ExitTime.Hours + ":" + ExitTime.Minutes + ":" + ExitTime.Seconds; } }

        public string Status { get; set; }
        public TimeSpan ExitTime { get; set; }

    }

    public class Parents
    {
        public string EmployeeID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string UserID { get; set; }
    }
}

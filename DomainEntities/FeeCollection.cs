using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class FeeCollection
    {
        public int? ID { get; set; }
        public string PaidStatus { get; set; }
        public bool IsAdmin { get; set; }
        [DisplayName("Session")]
       
        public string Session { get; set; }

        public bool IsChecked { get; set; }

        [DisplayName("Faculty")]
       
        public string Faculty { get; set; }

        [DisplayName("Class")]
       
        public string Class { get; set; }


        [DisplayName("Section")]
       
        public string Section { get; set; }

        [DisplayName("Fee Type")]
        [Required(ErrorMessage = "Required")]
        public string Type { get; set; }

        [DisplayName("Month")]
        [Required(ErrorMessage = "Required")]
        public string Month { get; set; }

        [DisplayName("Total Fee")]
        public decimal TotalFee { get; set; }

        [DisplayName("Total Discount")]
        public decimal TotalDiscount { get; set; }

        [DisplayName("Previous Due")]
        [ReadOnly(true)]
        public decimal PreviousDue { get; set; }

        [DisplayName("Grand Total")]
        public decimal GrandTotal { get; set; }

        [DisplayName("Total Paid")]
        [RegularExpression("^[1-9]\\d*(\\.\\d+)?$", ErrorMessage = "Only Numbers are Allowed")]
        [Required(ErrorMessage = "Required")]
        public decimal TotalPaid { get; set; }

        [DisplayName("Balance")]
        public decimal Balance { get; set; }
 

        public int SessionID { get; set; }
        public string FacultyID { get; set; }
        public string ClassID { get; set; }
        public string TypeId { get; set; }
        public int MonthId { get; set; }
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public string RollNo { get; set; }


        [RegularExpression("^[1-9]\\d*(\\.\\d+)?$", ErrorMessage = "Only Numbers are Allowed")]
        [Required(ErrorMessage = "Required")]
        [Range(0.0, Double.MaxValue)]
        public decimal Fee { get; set; }

        [RegularExpression("^[1-9]\\d*(\\.\\d+)?$", ErrorMessage = "Only Numbers are Allowed")]
        [Required(ErrorMessage = "Required")]
        public decimal Discount { get; set; }

        public string SN { get; set; }
        public string IsCommon { get; set; }
        public string FeeName { get; set; }

        public string AddedBy { get; set; }
        public DateTime AddedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }


        public int pageSize { get; set; }
        public int pageNumber { get; set; }
        public int offset { get; set; }

        //data for grid
        public int Total { get; set; }
    }
    public class FeeCollectionReport
    {
        public string SchoolName { get; set; }
        public string SchoolAddress { get; set; }
        public string SchoolEmail { get; set; }
        public string SchoolPhone { get; set; }
        public string SchoolMobile { get; set; }
        public string SchoolFax { get; set; }
        public string ContactPerson { get; set; }
        public string SchoolRegistrationNo { get; set; }
        public string EstablishedYear { get; set; }
        public string SchoolImage { get; set; }
        public string ID { get; set; }
        public string Session { get; set; }
        public string RollNo { get; set; }
        public string RegistrationNo { get; set; }
        public string StudentCode { get; set; }
        public string StudentName { get; set; }
        public string FatherName { get; set; }
        public string MotherName { get; set; }
        public string FatherMobileNo { get; set; }
        public string MotherMobileNo { get; set; }
        public string PermanentAddress { get; set; }
        public string TemporaryAddress { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public string Email { get; set; }
        public DateTime EnglishDateOfBirth { get; set; }
        public string NepaliDateOfBirth { get; set; }
        public string Gender { get; set; }
        public DateTime EnglishJoiningDate { get; set; }
        public string CasteName { get; set; }
        public string ReligionName { get; set; }
        public string Image { get; set; }
        public string Batch { get; set; }
        public string Faculty { get; set; }
        public string StudentClass { get; set; }
        public string Section { get; set; }
        public int FeeCollectionId { get; set; }
        public decimal TotalFee { get; set; }
        public decimal TotalDiscount { get; set; }
        public decimal PreviousDue { get; set; }
        public decimal GrandTotal { get; set; }
        public decimal TotalPaid { get; set; }
        public DateTime PaymentDate { get; set; }
        public string PaymentMiti { get; set; }
        public decimal PaymentDue { get; set; }
        public int FeePaymentDetailsId { get; set; }
        public int TypeId { get; set; }
        public string FeeType { get; set; }
        public decimal Fee { get; set; }
        public decimal Discount { get; set; }
        public int Month { get; set; }
        public string MonthName { get; set; }

    }


    public class FeeDueReport
    {
        public string SchoolName { get; set; }
        public string SchoolAddress { get; set; }
        public string SchoolEmail { get; set; }
        public string SchoolPhone { get; set; }
        public string SchoolMobile { get; set; }
        public string SchoolFax { get; set; }
        public string ContactPerson { get; set; }
        public string SchoolRegistrationNo { get; set; }
        public string EstablishedYear { get; set; }
        public string SchoolImage { get; set; }
        public string ID { get; set; }
        public string Session { get; set; }
        public string RollNo { get; set; }
        public string RegistrationNo { get; set; }
        public string StudentCode { get; set; }
        public string StudentName { get; set; }
        public string FatherName { get; set; }
        public string MotherName { get; set; }
        public string FatherMobileNo { get; set; }
        public string MotherMobileNo { get; set; }
        public string PermanentAddress { get; set; }
        public string TemporaryAddress { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public string Email { get; set; }
        public DateTime EnglishDateOfBirth { get; set; }
        public string NepaliDateOfBirth { get; set; }
        public string Gender { get; set; }
        public DateTime EnglishJoiningDate { get; set; }
        public string CasteName { get; set; }
        public string ReligionName { get; set; }
        public string Image { get; set; }
        public string Batch { get; set; }
        public string Faculty { get; set; }
        public string StudentClass { get; set; }
        public string Section { get; set; }
        public int FeeDueId { get; set; }
        public decimal TotalFee { get; set; }
        public decimal TotalDiscount { get; set; }
        public decimal PreviousDue { get; set; }
        public decimal GrandTotal { get; set; }
        public DateTime BillingDate { get; set; }
        public string BillingMiti { get; set; }
        public int FeePaymentDueDetailsId { get; set; }
        public int TypeId { get; set; }
        public string FeeType { get; set; }
        public decimal Fee { get; set; }
        public decimal Discount { get; set; }
        public int Month { get; set; }
        public string MonthName { get; set; }

    }
}

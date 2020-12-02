using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DomainEntities
{
    public class Students
    {
        //Official Details
        public int? ID { get; set; }

        [DisplayName("Device UserID")]
        public string UserID { get; set; }
        public string StudentCode { get; set; }


        
        public DateTime EnglishJoinningDate { get; set; }

        
        public DateTime EnglishDateOfBirth { get; set; }


        [DisplayName("Academic Year")]
        [Required(ErrorMessage = "Required")]
        public string AcademicYear { get; set; }

        [DisplayName("Registration Number")]
        [Required(ErrorMessage = "Required")]
        public string RegistrationNo { get; set; }


        [DisplayName("Phone Number")]

        public string PhoneNo { get; set; }

        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [DisplayName("Email")]

        public string Email { get; set; }

        [DisplayName("Mobile Number")]

        public string MobileNo { get; set; }


        [DisplayName("Symbol No")]
        [Required(ErrorMessage = "Required")]
        public string SymbolNo { get; set; }
        [DisplayName("Class")]
        [Required(ErrorMessage = "Required")]
        public int? ClassID { get; set; }
        [DisplayName("Faculty")]
        [Required(ErrorMessage = "Required")]
        public int? FacultyID { get; set; }

        [DisplayName("Section")]
        [Required(ErrorMessage = "Required")]
        public string Section { get; set; }
        public string Image { get; set; }
        [DisplayName("House")]
        [Required(ErrorMessage = "Required")]
        public int? HouseID { get; set; }
        public string Batch { get; set; }

        [DisplayName("Roll No")]
        [Required(ErrorMessage = "Required")]

        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "Only Numbers are Allowed")]
        public string RollNo { get; set; }

    

       
       

        //Officials Detail end

        //Personnel Details Starts


        [Required(ErrorMessage = "Required")]
        public string StudentName { get; set; }
       

        [Required(ErrorMessage = "Required")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Required")]
        public string Status { get; set; }


        [DisplayName("Blood Group")]
        [Required(ErrorMessage = "Required")]
        public int? BloodGroupID { get; set; }


        [DisplayName("Category")]
        [Required(ErrorMessage = "Required")]
        public int? CategoryID { get; set; }

        [DisplayName("Religion")]
        [Required(ErrorMessage = "Required")]
        public int? ReligionID { get; set; }

        [DisplayName("Caste")]
        [Required(ErrorMessage = "Required")]
        public int? CasteID { get; set; }


        [DisplayName("Temporary Address")]
        [Required(ErrorMessage = "Required")]
        public string TemporaryAddress { get; set; }


        [DisplayName("Permanent Address")]
        [Required(ErrorMessage = "Required")]
        public string PermanentAddress { get; set; }

        [DisplayName("National Identity Number")]

        public string CitizenShipNumber { get; set; }

        //Personnel Details Ends
        //Parents Details Starts

        [Required(ErrorMessage = "Required")]
        public string FatherName { get; set; }
        [DisplayName("Mobile No")]
        [Required(ErrorMessage = "Required")]
        public string FatherMobileNo { get; set; }

        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [DisplayName("Email")]

        public string FatherEmail { get; set; }

        [Required(ErrorMessage = "Required")]
        public string MotherName { get; set; }

        [DisplayName("Mobile No")]
        //[Required(ErrorMessage = "Required")]
        public string MotherMobileNo { get; set; }

        [DisplayName("Job")]

        public string Fatherjob { get; set; }



        //  [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [DisplayName("Email")]

        public string MotherEmail { get; set; }

        [DisplayName("Job")]

        public string MotherJob { get; set; }
        //Parents Details Ends

        //Previous Qualificaiton DetailsStarts

        [DisplayName("Previous Institution Attended")]

        public string LastSchoolAttended { get; set; }

        public HttpPostedFileBase imageFile { get; set; }

        [DisplayName("Doucments Submitted")]

        public string DocumetsSubmittedID { get; set; }

        public List<string> DocumentsArray { get; set; }
        public string DocumentsSubmitted { get; set; }
       
        public string Result { get; set; }

        public string AddedBy { get; set; }
        public string AddedOn { get; set; }
        public string UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }

        //Previous Qualification Details Ends

        public bool IsHostelStudents { get; set; }
        public bool IsBusNeed { get; set; }
        //Other Details
        public string  Session { get; set; }
        public string Faculty { get; set; }

        public string ClassName { get; set; }
        public string HouseName { get; set; }

        public string BloodGroup { get; set; }
        public string StudentsCategory { get; set; }

        public string Name { get; set; }
        public string CasteName { get; set; }


    }

    public class BulkUploadStudents
    {
        [DisplayName("Joinning Date")]
        [Required(ErrorMessage = "Required")]
        public DateTime EnglishJoinningDate { get; set; }

        [Required(ErrorMessage = "Required")]

        [DisplayName("Date Of Birth")]
        public DateTime EnglishDateOfBirth { get; set; }

        public Students Students { get; set; }
        public BulkStudentsSearch BulkStudentsSearch { get; set; }

    }

    public class StudentBulkUpload
    {
        public string StudentName { get; set; }
        public string RollNo { get; set; }
        public string FatherName { get; set; }
        public string FatherMobileNo { get; set; }
        public string MotherName { get; set; }      
        public DateTime EnglishJoinningDate { get; set; }     
        public DateTime EnglishDateOfBirth { get; set; }
    }
    public class BulkStudentsSearch
    {
        [Required(ErrorMessage = "Required")]
        public string BulkYearID { get; set; }
        [Required(ErrorMessage = "Required")]
        public string BulkClassID { get; set; }
        [Required(ErrorMessage = "Required")]
        public string BulkFacultyID { get; set; }

        [Required(ErrorMessage = "Required")]
        public string BulkSection { get; set; }

        [Required(ErrorMessage = "Required")]
        public HttpPostedFileBase BulkImage { get; set; }

    }
    public class UniqueNoGeneration
    {

        public string RegistrationNo { get; set; }
        public string UserID { get; set; }

    }

    public class StudentsSearch
    {
        public int ID { get; set; }
        public string StudentsSearchName { get; set; }

        [DisplayName("Faculty")]
        public int FacultySearchID { get; set; }

        [DisplayName("Class")]
        public int ClassSearchID { get; set; }
        public string SectionSearch { get; set; }
        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "Only Numbers are Allowed")]
        public string RollNo { get; set; }
        public string RegistratioNoSearch { get; set; }
        public string BatchSearch { get; set; }
        public int pageSize { get; set; }
        public int pageNumber { get; set; }
        public int offset { get; set; }

        //data for grid
        public int Total { get; set; }
        public int SN { get; set; }

        public string StudentName { get; set; }

        public string Batch { get; set; }

        public string Faculty { get; set; }

        public string ClassName { get; set; }

        public string RegistrationNo { get; set; }
        
        public string Status { get; set; }
        public string Section { get; set; }


    }



}

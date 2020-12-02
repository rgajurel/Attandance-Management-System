using DomainEntities;
using DomainInterface;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SchoolManagementSystem.Areas.Admin.Controllers
{
    public class StudentsController : Controller
    {
        private readonly IStudentsRepository studentsRepo;
        private readonly IMessageHandlerRepository messageHandlerRepo;
        private readonly IDropDownRepository dropDownRepo;
        private string message = "";
        public StudentsController(IStudentsRepository studentsRepo, IDropDownRepository dropDownRepo, IMessageHandlerRepository messageHandlerRepo)
        {
            this.studentsRepo = studentsRepo;
            this.messageHandlerRepo = messageHandlerRepo;
            this.dropDownRepo = dropDownRepo;
         }
        // GET: Admin/Students
        //[OutputCache(Duration = 3600, VaryByParam = "none", Location = System.Web.UI.OutputCacheLocation.Client)]
        public ActionResult Index()
        {
            LoadDropDown();
            return View();
        }
        [OutputCache(Duration = 3600, VaryByParam = "none", Location = System.Web.UI.OutputCacheLocation.Client)]
        public ActionResult LoadPartialView()
        {
            LoadDropDown();
            return PartialView("View");
        }

        public FileResult DownloadSample()
        {
            string file = @"\Content\BatchUploadUserSampleExcel/BatchUserUpload.xlsx";
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            return File(file, contentType, Path.GetFileName(file));
        }
        public void LoadDropDown()
        {
           
           
            var sessionList = dropDownRepo.GetSessionDropDown();
            if (sessionList != null)
            {
                ViewBag.sessionList = new SelectList(sessionList, "ID", "Name");
            }
            else
            {
                ViewBag.sessionList = new SelectList(dropDownRepo.GetErrorList(), "ID", "Name");
            }
           

            var facultyList = dropDownRepo.GetFacultyDropDown();
            if (facultyList != null)
            {
                ViewBag.facultyList = new SelectList(facultyList, "ID", "Name");
            }
            else
            {
                ViewBag.facultyList = new SelectList(dropDownRepo.GetErrorList(), "ID", "Name");
            }
         

            var classList = dropDownRepo.GetClasswDropDown();
            if (classList != null)
            {
                ViewBag.classList = new SelectList(classList, "ID", "Name");
            }
            else
            {
                ViewBag.classList = new SelectList(dropDownRepo.GetErrorList(), "ID", "Name");
            }
           

            var bloodGroupList = dropDownRepo.GetBloodGroupDropDown();
            if (bloodGroupList != null)
            {
                ViewBag.bloodGroupList = new SelectList(bloodGroupList, "ID", "Name");
            }
            else
            {
                ViewBag.bloodGroupList = new SelectList(dropDownRepo.GetErrorList(), "ID", "Name");
            }
            

            var houseList = dropDownRepo.GetHouseDropDown();
            if (houseList != null)
            {
                ViewBag.houseList = new SelectList(houseList, "ID", "Name");
            }
            else
            {
                ViewBag.houseList = new SelectList(dropDownRepo.GetErrorList(), "ID", "Name");
            }
          

            var categoryList = dropDownRepo.GetStudentsCategoryDropDown();
            if (categoryList != null)
            {
                ViewBag.categoryList = new SelectList(categoryList, "ID", "Name");
            }
            else
            {
                ViewBag.categoryList = new SelectList(dropDownRepo.GetErrorList(), "ID", "Name");
            }
          

            var religionList = dropDownRepo.GetReligionDropDown();
            if (religionList != null)
            {
                ViewBag.religionList = new SelectList(religionList, "ID", "Name");
            }
            else
            {
                ViewBag.religionList = new SelectList(dropDownRepo.GetErrorList(), "ID", "Name");
            }
          

            var casteList = dropDownRepo.GetCasteDropDown();
            if (casteList != null)
            {
                ViewBag.casteList = new SelectList(casteList, "ID", "Name");
            }
            else
            {
                ViewBag.casteList = new SelectList(dropDownRepo.GetErrorList(), "ID", "Name");
            }
                     

            var documentsList = dropDownRepo.GetDocumentsDropDown();
            if (documentsList != null)
            {
                ViewBag.documentsList = new SelectList(documentsList, "ID", "Name");
            }
            else
            {
                ViewBag.documentsList = new SelectList(dropDownRepo.GetErrorList(), "ID", "Name");
            }
            


        }

        public JsonResult CreateBatchUpload(IEnumerable<Students> LstExcelObject)
        {
            try
            {
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public JsonResult GetUniqueRegistrationNumber(string Batch)
        {
            try
            {
                if (Batch != null)
                {
                    var uniqueregistration = studentsRepo.GetUniqueRegistrationNo(Batch);
                    return new JsonResult()
                    {
                        Data = uniqueregistration,
                        ContentType = "application/json",
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet,

                    };
                }
                else
                {
                    return Json(new { success = false, responseText = MassageDescription.ExceptionOrNullError }, JsonRequestBehavior.AllowGet);
                }


            }
            catch (Exception)
            {
                return Json(new { success = false, responseText = MassageDescription.ExceptionOrNullError }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult GetSectionBaseOnClassAndFaculty(string ClassID,string FacultyID)
        {
            try
            {
                if (ClassID != null && FacultyID!=null)
                {
                    var sections = studentsRepo.GetSectionBasedOnClass(ClassID,FacultyID);
                    return new JsonResult()
                    {
                        Data = sections,
                        ContentType = "application/json",
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet,

                    };
                }
                else
                {
                    return Json(new { success = false, responseText = MassageDescription.ExceptionOrNullError }, JsonRequestBehavior.AllowGet);
                }


            }
            catch (Exception )
            {
                return Json(new { success = false, responseText = MassageDescription.ExceptionOrNullError }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult GetFacultyBaseOnClass(string ID)
        {
            try
            {
                if (ID != null)
                {
                    var facultys = studentsRepo.GetFacultyBasedOnClass(ID);
                    return new JsonResult()
                    {
                        Data = facultys,
                        ContentType = "application/json",
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet,

                    };
                }
                else
                {
                    return Json(new { success = false, responseText = MassageDescription.ExceptionOrNullError }, JsonRequestBehavior.AllowGet);
                }


            }
            catch (Exception)
            {
                return Json(new { success = false, responseText = MassageDescription.ExceptionOrNullError }, JsonRequestBehavior.AllowGet);
            }
        }



        [HttpPost]

        public JsonResult BatchUploadStudents(List<StudentBulkUpload> batchdata,string studentbatch,string facultyid,string classid,string sectionid,string academicyearid)
        {
            try
            {
                if (batchdata.Count() != 0)
                {
                    var getuniquedata = this.GetUniqueRegistrationNumber(studentbatch);
                 var emptycheck= batchdata.Any(x => String.IsNullOrEmpty(x.FatherMobileNo) || String.IsNullOrEmpty(x.FatherName) || String.IsNullOrEmpty(x.StudentName) || String.IsNullOrEmpty(x.RollNo) || String.IsNullOrEmpty(x.MotherName));
                    if (emptycheck)
                    {
                        message = "Any field in Excel cannot be empty. Please check excel sheet";
                    }
                    else
                    {
                        Students students = new Students();
                        foreach (var stud in batchdata)
                        {                            
                            students.StudentName = stud.StudentName;
                            students.AcademicYear = academicyearid;
                            students.RegistrationNo = studentsRepo.GetUniqueRegistrationNo(studentbatch).RegistrationNo;
                            students.FacultyID = Convert.ToInt16(facultyid);
                            students.ClassID= Convert.ToInt16(classid);
                            students.Section = sectionid;
                            students.RollNo = stud.RollNo;
                            students.Batch = studentbatch;
                            students.SymbolNo = students.RegistrationNo;
                            students.UserID = studentsRepo.GetUniqueRegistrationNo(studentbatch).UserID;
                            students.Status = "Active";
                            students.FatherName = stud.FatherName;
                            students.FatherMobileNo = stud.FatherMobileNo;
                            students.MotherName = stud.MotherName;
                            students.EnglishDateOfBirth =stud.EnglishDateOfBirth;
                            students.EnglishJoinningDate =stud.EnglishJoinningDate;
                            var savechange = studentsRepo.AddUpdateStudents(students);
                            message = (savechange == true) ? MassageDescription.SaveSuccess : MassageDescription.SaveFailure;

                        }
                        
                    }
                }
                else
                {
                    message = MassageDescription.ExceptionOrNullError;
                }
                return Json(messageHandlerRepo.GetMessage(message));
            }
            catch(Exception ex)
            {
                message = MassageDescription.ExceptionOrNullError;
                return Json(messageHandlerRepo.GetMessage(message));
            }
           
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SaveStudents(Students students)
        {

            try
            {
                students.DocumentsSubmitted = string.Join(",", students.DocumentsArray.ToArray());
                if (students != null)
                {
                    if (ModelState.IsValid)
                    {
                        if (students.ID > 0)
                        {
                            students.Image = UploadImageUpdate(students.imageFile);
                            var savechange = studentsRepo.AddUpdateStudents(students);
                            message = (savechange == true) ? MassageDescription.UpdateSuccess : MassageDescription.UpdateFailure;
                            //update
                        }
                        else
                        {
                            students.Image = UploadImageSave(students.imageFile);
                            var savechange = studentsRepo.AddUpdateStudents(students);
                            message = (savechange == true) ? MassageDescription.SaveSuccess : MassageDescription.SaveFailure;
                            //add
                        }

                    }
                    else
                    {
                        message = MassageDescription.ModelErrorOccured;

                        //model error occured
                    }
                }
                else
                {
                    message = MassageDescription.ExceptionOrNullError;
                    //null error occured
                }
                return Json(messageHandlerRepo.GetMessage(message));
            }
            catch (Exception)
            {
                message = MassageDescription.ExceptionOrNullError;
                return Json(messageHandlerRepo.GetMessage(message));

            }


        }

        [HttpPost]
        public JsonResult EditStudents(int id)
        {
            try
            {
                if (id != 0)
                {
                    var editStudents = studentsRepo.EditStudents(id);
                    return new JsonResult()
                    {
                        Data = editStudents,
                        ContentType = "application/json",
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet,

                    };
                }
                else
                {
                    return Json(new { success = false, responseText = MassageDescription.ExceptionOrNullError }, JsonRequestBehavior.AllowGet);
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public JsonResult DetailsStudents(int id)
        {
            try
            {
                if (id != 0)
                {
                    var detailsStudents = studentsRepo.DetailsStudents(id);
                    return new JsonResult()
                    {
                        Data = detailsStudents,
                        ContentType = "application/json",
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet,

                    };
                }
                else
                {
                    return Json(new { success = false, responseText = MassageDescription.ExceptionOrNullError }, JsonRequestBehavior.AllowGet);
                }


            }
            catch (Exception)
            {
                return Json(new { success = false, responseText = MassageDescription.ExceptionOrNullError }, JsonRequestBehavior.AllowGet);
            }
        }



        // [HttpPost]
        public JsonResult AutoComplete(string text)
        {
            try
            {
                if (String.IsNullOrEmpty(text))
                    {
                    text = null;
                }

                var students = studentsRepo.GetAllStudents(text);
                if (students != null)
                {
                    return Json(students, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new StudentsSearch()
                    {
                        StudentsSearchName = "No Data Availiable",
                    });
                       
                }
               
            }
            catch (Exception)
            {
                return Json(new { success = false, responseText = "No Records Found" }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetAll([DataSourceRequest] DataSourceRequest request, StudentsSearch search)
        {

            search.offset = request.PageSize * (request.Page - 1);
            var studentsList = studentsRepo.GetAllStudents(search);        

            if (studentsList != null)
            {
                var result = new DataSourceResult()
                {
                    Data = studentsList,
                    Total = studentsList.Select(model => model.Total).FirstOrDefault()
                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {

                var result = new DataSourceResult()
                {
                    Data = studentsList,
                    Total = 0
                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }

        }

        private string UploadImageSave(HttpPostedFileBase image)
        {
            var SchoolPicImage = "";

            if (image != null)
            {
                string schoolPicExtension = Path.GetExtension(image.FileName);
                //var ImageName = Guid.NewGuid();
                image.SaveAs(HttpContext.Server.MapPath("~/Content/Images/Students/" + image.FileName));

                SchoolPicImage = "/Content/Images/Students/" + image.FileName;
            }
            else
            {
                SchoolPicImage = DefaultImages.studentImage;
            }

            return SchoolPicImage;
        }

        private string UploadImageUpdate(HttpPostedFileBase image)
        {
            var SchoolPicImage = String.Empty;

            if (image != null)
            {
                string schoolPicExtension = Path.GetExtension(image.FileName);
                //var ImageName = Guid.NewGuid();
                image.SaveAs(HttpContext.Server.MapPath("~/Content/Images/Students/" + image.FileName));

                SchoolPicImage = "/Content/Images/Students/" + image.FileName;
            }


            return SchoolPicImage;
        }

        public JsonResult RollNumberCount(string faculty, string classs, string section, string rollno)
        {
            try
            {
                if (!String.IsNullOrEmpty(section))
                {
                    var duplicateRollNo = studentsRepo.GetClassRollNoCount(faculty, classs, section, rollno) ;

                    return new JsonResult()
                    {
                        Data = duplicateRollNo,
                        ContentType = "application/json",
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet,

                    };
                }
                else
                {
                    message = MassageDescription.ExceptionOrNullError;
                    return Json(messageHandlerRepo.GetMessage(message));
                }
            }
            catch (Exception)
            {
                message = MassageDescription.ExceptionOrNullError;
                return Json(messageHandlerRepo.GetMessage(message));
            }

        }
    }
}
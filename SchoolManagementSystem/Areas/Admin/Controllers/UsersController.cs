using DomainEntities;
using DomainInterface;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SchoolManagementSystem.Areas.Admin.Controllers
{
     public class UsersController : Controller
    {
        private readonly IUserRepository userRepo;
        private readonly IAccumulativeLeaveRepository accumulativeLeaveRepo;
        private readonly IMessageHandlerRepository messageHandlerRepo;
        private readonly IDropDownRepository dropDownRepo;
        private readonly ISettingsRepository settingRepo;
        private readonly string emailSettingGroup = SettingsGroupName.EmailGroup;
        private string message = "";
        private string emailsendmessage = "";
        // GET: Admin/Users
        public UsersController(IDropDownRepository dropDownRepo, ISettingsRepository settingRepo, IMessageHandlerRepository messageHandlerRepo, IUserRepository userRepo, IAccumulativeLeaveRepository accumulativeLeaveRepo)
        {
            this.dropDownRepo = dropDownRepo;
            this.messageHandlerRepo = messageHandlerRepo;
            this.userRepo = userRepo;
            this.accumulativeLeaveRepo = accumulativeLeaveRepo;
            this.settingRepo = settingRepo;
        }
      
        public ActionResult Index()
        {
            LoadDropDown();
            return View();
        }

        public ActionResult LoadPartialView()
        {
            LoadDropDown();
            return PartialView("View");
        }

        public void LoadDropDown()
        {
            var allOrganisation = dropDownRepo.GetAllOrganisation();
            if (allOrganisation != null)
            {
                ViewBag.allOrganisation = new SelectList(allOrganisation, "ID", "Name");
            }
            else
            {
                ViewBag.allOrganisation = new SelectList(dropDownRepo.GetErrorList(), "ID", "Name");
            }

        }

        public JsonResult GetUserGroup()
        {
            return Json(dropDownRepo.GetUserGroup(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult AutoComplete(string text, int organisation,bool IsSuperUser,bool IsAdmin,bool IsStudentUser,bool IsClientUser,bool IsParentUser)
       {
            try
            {
                if (String.IsNullOrEmpty(text))
                {
                    text = null;
                }
                if (IsSuperUser==true || IsAdmin==true || IsClientUser == true)
                {
                    var employee = (accumulativeLeaveRepo.GetAllEmployee(text, organisation));
                    if (employee != null)
                    {
                        return Json(employee, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {

                        return Json(null, JsonRequestBehavior.AllowGet);
                    }
                }
                else if(IsParentUser == true)
                {
                    var parents = (userRepo.GetAllParents(text));
                    if (parents != null)
                    {
                        return Json(parents, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {

                        return Json(null, JsonRequestBehavior.AllowGet);
                    }
                }
                else if (IsStudentUser == true)
                {
                    var students = (userRepo.GetAllStudents(text));
                    if (students != null)
                    {
                        return Json(students, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {

                        return Json(null, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return null;
                }


                //var employee = (userRepo.GetAllEmployee(text, organisation));
                //if (employee != null)
                //{
                //    return Json(employee, JsonRequestBehavior.AllowGet);
                //}
                //else
                //{

                //    return Json(null, JsonRequestBehavior.AllowGet);
                //}

            }
            catch (Exception)
            {
                return null;
            }
        }

        public JsonResult GetUserRole()
        {
            return Json(dropDownRepo.RoleGet(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAll([DataSourceRequest] DataSourceRequest request, User search)
        {
            search.offset = request.PageSize * (request.Page - 1);
            var studentsList = userRepo.GetAllUsers(search);

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


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UserSave(User user)
        {
            var password = user.Password;
            List<bool> AllUser = new List<bool>();
            AllUser.Add(user.IsAdmin);
            AllUser.Add(user.IsSuperAdmin);
            AllUser.Add(user.IsClientUser);
            AllUser.Add(user.IsParentUser);
            AllUser.Add(user.IsStudentUser);
            var count = AllUser.Where(x => x.Equals(true)).ToList().Count();
            try
            {
                if (user != null)
                {
                    if (ModelState.IsValid)
                    {
                        if (count== 1)
                        {                            
                                user.Password = Crypto.OneWayEncryter(user.Password);
                                user.ConformPassword = Crypto.OneWayEncryter(user.ConformPassword);
                                if (user.ID > 0)
                                {
                                if (!String.IsNullOrEmpty(user.Email))
                                {
                                    var emailsend = SendEmailToUser(user.Email, user.UserName, password, Convert.ToInt16(user.ID));
                                    emailsendmessage = (emailsend == true) ? MassageDescription.EmailSendSuccess : MassageDescription.EmailSendFailure;

                                }
                                var savechange = userRepo.AddUpdateUser(user);
                                 message = (savechange == true) ? MassageDescription.UpdateSuccess : MassageDescription.UpdateFailure;
                                    //update
                                }
                                else
                                {
                                if (!String.IsNullOrEmpty(user.Email))
                                {
                                    var emailsend = SendEmailToUser(user.Email, user.UserName, password,Convert.ToInt16(user.ID));
                                    emailsendmessage = (emailsend == true) ? MassageDescription.EmailSendSuccess : MassageDescription.EmailSendFailure;

                                }

                                var isalreadyattend = userRepo.ChekUserAlreadyExist(user);

                                if (isalreadyattend)
                                {
                                    message = MassageDescription.AlreadyExist;
                                }
                                else
                                {
                                    var savechange = userRepo.AddUpdateUser(user);
                                    message = (savechange == true) ? MassageDescription.SaveSuccess : MassageDescription.SaveFailure;
                                }
                               
                                    
                                }
                            

                    }
                        else
                        {
                            message = MassageDescription.SingleData;
                        }
                    }
                else
                {
                    message = MassageDescription.ModelErrorOccured;
                   emailsendmessage = MassageDescription.EmailSendFailure;
                    
                }
                }
                else
                {
                    message = MassageDescription.ExceptionOrNullError;
                    emailsendmessage = MassageDescription.EmailSendFailure;
                    //null error occured
                }
                return Json(messageHandlerRepo.GetMessage(message+" and "+emailsendmessage));
            }
            catch (Exception ex)
            {
                throw ex;

            }

        }

        public bool SendEmailToUser(string email, string userName,string password,int id)
        {
            bool emailsend=false;
            if (!String.IsNullOrEmpty(email))
            {
                Template template = new Template();
                if (id > 0)
                {
                    template.Body = "Your Credentials Has Been Changed. Please Contact Respective Personnel";
                    template.Subject = "Credential Updated Status";
                }
                else
                {
                    template.Body = "Your UserName is </br> " + userName + " and </br> Password is " + password;
                    template.Subject = "Credential Has Been Created For You";
                }
               
                    EmailSenderReceiverData emailSenderData = new EmailSenderReceiverData();
                    emailSenderData.EmailTo = email;
                    emailSenderData.SMTPHost = settingRepo.GetSettingByIDandGroup("1003", emailSettingGroup);
                    emailSenderData.SMTPUserName = settingRepo.GetSettingByIDandGroup("1005", emailSettingGroup);
                    emailSenderData.SMTPPassword = settingRepo.GetSettingByIDandGroup("1004", emailSettingGroup);
                    emailSenderData.SMTPPort = Convert.ToInt16(settingRepo.GetSettingByIDandGroup("1006", emailSettingGroup));
                  emailsend= EmailHelper.SendEmail(emailSenderData, template);
               
                }
            return emailsend;
            }
        
        public JsonResult CheckExistingUserName(string UserName,int? ID)
        {
            if (ID == 0|| ID==null)
            {
                var userdata = userRepo.GetUserName(UserName);
                if (userdata != null)
                {
                    return Json(!userdata.UserName.Equals((UserName)), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                var userdata = userRepo.GetUserName(UserName);
                if (userdata != null)
                {
                    if (userdata.ID == ID)
                    {
                        return Json(true, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(false, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(true, JsonRequestBehavior.AllowGet);
                }

            }
           

        }

        [HttpPost]
        public JsonResult DeleteUser(int id)
        {
            try
            {
                if (id != 0)
                {
                    if(!new LoginUser().IsSuperAdmin)
                    {
                        message = MassageDescription.CannotDelete;
                    }
                    else
                    {
                        var savechanges = userRepo.DeleteUser(id);
                        message = (savechanges == true) ? MassageDescription.DeleteSuccess : MassageDescription.CannotDeleteDependency;
                    }
                   
                }
                else
                {
                    message = MassageDescription.ExceptionOrNullError;
                }
                return Json(messageHandlerRepo.GetMessage(message));

            }
            catch (Exception ex)
            {
                message = MassageDescription.ExceptionOrNullError;
                return Json(messageHandlerRepo.GetMessage(message));
            }
        }

        [HttpPost]
        public JsonResult EditUser(int id)
        {
            try
            {
                if (id != 0)
                {
                    var editUser = userRepo.EditUser(id);
                    return new JsonResult()
                    {
                        Data = editUser,
                        ContentType = "application/json",
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet,

                    };
                }
                else
                {
                    throw new Exception();
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
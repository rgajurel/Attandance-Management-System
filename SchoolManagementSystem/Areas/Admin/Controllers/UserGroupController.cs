using DomainEntities;
using DomainInterface;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SchoolManagementSystem.Areas.Admin.Controllers
{
    public class UserGroupController : Controller
    {
        private readonly IDropDownRepository dropDownRepo;
        private readonly IUserGroupRepository userGroupRepo;
        private readonly IMessageHandlerRepository messageHandlerRepo;
        private string message = "";
        // GET: Admin/UserGroup

       public UserGroupController(IUserGroupRepository userGroupRepo, IMessageHandlerRepository messageHandlerRepo,IDropDownRepository dropDownRepo)
        {
            this.userGroupRepo = userGroupRepo;
            this.messageHandlerRepo = messageHandlerRepo;
            this.dropDownRepo = dropDownRepo;
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SaveUserGroup(UserGroup usergroup)
        {
            try
            {
                if (usergroup != null)
                {
                   
                    if (ModelState.IsValid)
                    {
                        if (usergroup.ID > 0)
                        {
                            var savechange = userGroupRepo.AddUpdateUserGroup(usergroup);
                            message = (savechange == true) ? MassageDescription.UpdateSuccess : MassageDescription.UpdateFailure;
                            //update
                        }
                        else
                        {
                            var savechange = userGroupRepo.AddUpdateUserGroup(usergroup);
                            message = (savechange == true) ? MassageDescription.SaveSuccess : MassageDescription.SaveFailure;
                            //add
                        }

                    }
                    else
                    {
                        message = MassageDescription.ModelErrorOccured;
                        // return Json(messageHandlerRepo.GetMessage(message));
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
            catch (Exception ex)
            {
                message = MassageDescription.ExceptionOrNullError;
                return Json(messageHandlerRepo.GetMessage(message));

            }


        }

        public JsonResult GetAll([DataSourceRequest] DataSourceRequest request, UserGroup userGroup)
        {
            try
            {
                userGroup.offset = request.PageSize * (request.Page - 1);
                var allUserGroup = userGroupRepo.GetAllUserGroup(userGroup);// iDepartRepo.GetAllDepartment().OrderByDescending(model => model.DepartmentID);
                if (allUserGroup != null)
                {
                    var result = new DataSourceResult()
                    {
                        Data = allUserGroup,
                        Total = allUserGroup.Select(model => model.Total).FirstOrDefault()
                    };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var result = new DataSourceResult()
                    {
                        Data = allUserGroup,
                        Total = 0
                    };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                return null;
            }



        }

        [HttpPost]
        public JsonResult EditUserGroup(int id)
        {
            try
            {
                if (id != 0)
                {
                    var editUserGroup = userGroupRepo.EditUserGroup(id);
                    return new JsonResult()
                    {
                        Data = editUserGroup,
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
        public JsonResult DeleteUserGroup(int id)
        {
            try
            {
                if (id != 0)
                {
                    var savechanges = userGroupRepo.DeleteUserGroup(id);
                    message = (savechanges == true) ? MassageDescription.DeleteSuccess : MassageDescription.CannotDeleteDependency;
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

        public JsonResult CheckExistingUserGroup(string GroupName, int ID)
        {
            if (ID == 0)
            {
                var usergroupData = userGroupRepo.GetUserGroup(GroupName);
                if (usergroupData != null)
                {
                    return Json(!usergroupData.GroupName.Equals((GroupName)), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                var usergroupData = userGroupRepo.GetUserGroup(GroupName);
                if (usergroupData != null)
                {
                    if (usergroupData.ID == ID)
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



    }
}
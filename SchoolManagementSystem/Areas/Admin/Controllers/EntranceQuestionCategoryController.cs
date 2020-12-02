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
    public class EntranceQuestionCategoryController : Controller
    {
        private readonly ICategoryTreeRepository categoryTreeRepo;
        private readonly IMessageHandlerRepository messageRepo;
        private readonly IDropDownRepository dropDownRepo;
        private readonly string categoryType;
        private readonly int identifierCategoryTree;
        public EntranceQuestionCategoryController( IDropDownRepository dropDownRepo, IMessageHandlerRepository messageRepo, ICategoryTreeRepository categoryTreeRepo)
        {
            this.categoryTreeRepo = categoryTreeRepo;
            this.messageRepo = messageRepo;
            this.dropDownRepo = dropDownRepo;
            categoryType = CategoryType.CategoryEntranceQuestion;
            //identifierCategoryTree = StatusIdentifier.identifierCategoryTree;
        }
        //[AuthorizeUser(Controls = "View")]
        // GET: Admin/CourseCategory
        public ActionResult Index()
        {
            LoadDropDown();
            return View();
        }

        public void LoadDropDown()
        {
            //ViewBag.statusList = new SelectList(dropDownRepo.GetStatusBasedOnIdentifier(identifierCategoryTree), "StatusValue", "StatusName");
            //ViewBag.userGroupList = new SelectList(dropDownRepo.GetUserGroup(), "UserGroupID", "GroupName");
            ViewBag.categoryParentList = new SelectList(GetAllParentForAdmin(), "CategoryTreeID", "CategoryName");

            //IEnumerable<DomainEntities.Menu> allowedMenus = MenuRepository.GetMenuAccessBasedOnRole(new LoginUser().UserName);

            //ViewBag.editAccess = AuthorizeUser.AuthorizeControlForButton("Edit", allowedMenus);
            //ViewBag.createAccess = AuthorizeUser.AuthorizeControlForButton("Add", allowedMenus);
            //ViewBag.deleteAccess = AuthorizeUser.AuthorizeControlForButton("Delete", allowedMenus);

            //string generalSettingGroup = SettingsGroupName.GeneralGroup;
            //string itemPerPageSettingValue = settingRepo.GetSettingByIDandGroup("1001", generalSettingGroup);
            //int itemPerPage;
            //try
            //{
            //    itemPerPage = Convert.ToInt16(itemPerPageSettingValue);
            //}
            //catch
            //{
            //    itemPerPage = 10;
            //}

            //ViewBag.ItemPerPage = itemPerPage;
            //string settingDate = settingRepo.GetSettingByIDandGroup("1023", generalSettingGroup);
            //if (String.IsNullOrEmpty(settingDate))
            //{
            //    settingDate = "MM-dd-yyyy";
            //}
            ViewBag.settingDateFormat = "{0:" + "MM-dd-yyyy" + "}";
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CategoryTreeSave(CategoryTree categoryTree)
        {
           // var dataAddUpdateAccess = AuthorizeUser.AuthorizeAddUpdate(categoryTree.CategoryTreeID);
            string message = string.Empty;
            //if (dataAddUpdateAccess)
            //{
                try
                {
                    categoryTree.CategoryName = categoryTree.CategoryName;
                    ModelState.Clear();
                    if (TryValidateModel(categoryTree))
                    {
                        categoryTree.CategoryType = categoryType;
                        categoryTree.AddedBy = new LoginUser().UserName;
                        categoryTree.UserGroup = null;

                        if (string.IsNullOrEmpty(categoryTree.Image))
                        {
                            categoryTree.Image = "";
                        }

                        categoryTreeRepo.AddUpdateCategoryTree(categoryTree);

                        if (categoryTree.CategoryTreeID <= 0)
                        {
                            message = StatusCodeDescription.categoryAddSuccess;
                        }
                        else
                        {
                            message = StatusCodeDescription.categoryUpdateSuccess;
                        }
                        return Json(messageRepo.GetSuccessMessage(true, message));
                    }
                    else
                    {
                        message = "Error Occured";//get error message here model error
                        return Json(messageRepo.GetErrorMessage(true, message));
                    }
                }
                catch (Exception ex)
                {
                    return Json(messageRepo.GetErrorMessage(true, ex.Message.ToString()));
                }
            //}
            //else
            //{
            //    message = "You are not authorized for this operation";
            //    return Json(messageRepo.GetErrorMessage(true, message));
            //}
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CategoryTreeDelete(int categoryTreeID)
        {
           // var dataDeleteAccess = AuthorizeUser.AuthorizeControl("Delete");
            string message = string.Empty;
            //if (dataDeleteAccess)
            //{
                try
                {
                    if (categoryTreeID > 0)
                    {
                        ReturnType returnData = categoryTreeRepo.DeleteCategory(categoryType, categoryTreeID);
                        if (returnData != null)
                        {
                            if (returnData.Result)
                            {
                                message = StatusCodeDescription.categoryDeleteSuccess;
                                return Json(messageRepo.GetSuccessMessage(true, message));
                            }
                            else
                            {
                                message = returnData.Message;
                            }
                        }
                        else
                        {
                            message = StatusCodeDescription.categoryDeleteFaliureMessage;
                        }
                        return Json(messageRepo.GetErrorMessage(true, message));
                    }
                    else
                    {
                        message = StatusCodeDescription.categoryDoNotExistMessage;
                        return Json(messageRepo.GetErrorMessage(true, message));
                    }

                }
                catch (Exception ex)
                {
                    return Json(messageRepo.GetErrorMessage(true, ex.Message.ToString()));
                }
            //}
            //else
            //{
            //    message = "You are not authorized for this operation";
            //    return Json(messageRepo.GetErrorMessage(true, message));
            //}
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CategoryTreeInfoByID(int categoryTreeID)
        {

            string message = String.Empty;
            try
            {
                if (categoryTreeID > 0)
                {
                    CategoryTree category = categoryTreeRepo.GetCategoryTreeByID(categoryTreeID);
                    if (category != null)
                    {
                        message = StatusCodeDescription.successMessage;

                    }
                    return Json(messageRepo.GetErrorMessageWithData(true, message, category));
                }
                else
                {
                    message = StatusCodeDescription.categoryDoNotExistMessage;
                    return Json(messageRepo.GetErrorMessageWithData(true, message, null));
                }

            }
            catch (Exception ex)
            {
                return Json(messageRepo.GetErrorMessageWithData(true, ex.Message.ToString(), null));
            }
        }

        [HttpPost]
        public JsonResult CategoryTreeGetBasedOnCategoryType(CategoryTreeSearch categoryTreeSearch)
        {
            string message = String.Empty;
            try
            {
                categoryTreeSearch.categoryType = categoryType;
                if (String.IsNullOrEmpty(categoryTreeSearch.searchParam))
                {
                    categoryTreeSearch.searchParam = "";

                }
                int page = categoryTreeSearch.pageNumber;
                categoryTreeSearch.offSet = categoryTreeSearch.pageSize * (page - 1);

                if (categoryTreeSearch.pageSize <= 0)
                {
                    categoryTreeSearch.pageSize = 5;
                }


                List<CategoryTree> categories = categoryTreeRepo.GetAllCategoryTree(categoryTreeSearch);
                int total = categoryTreeRepo.GetTotalCategoryTreeFound(categoryTreeSearch);

                var result = new DataSourceResult()
                {
                    Data = categories,
                    Total = total
                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CategoryTree> GetAllParentForAdmin()
        {
            string message = String.Empty;
            string loggedInUserName = new LoginUser().UserName;

            List<CategoryTree> activeParents = categoryTreeRepo.GetAllParentCategory(categoryType, loggedInUserName);

            List<CategoryTree> activeParentDropDown = new List<CategoryTree>();
            CategoryTree category = new CategoryTree();
            foreach (CategoryTree item in activeParents)
            {

                int CategoryTreeDepth = item.Depth;
                var slash = "";
                for (int i = 2; i <= CategoryTreeDepth; i++)
                {
                    slash = slash + "--";
                }
                var newItem = slash + item.CategoryName;
                category.CategoryName = newItem;
                category.CategoryTreeID = item.CategoryTreeID;
                activeParentDropDown.Add(category);
                category = new CategoryTree();
            }

            return activeParentDropDown;

        }

        [HttpPost]
        public JsonResult GetAllActiveParentForClient(string categoryType)
        {
            string message = String.Empty;
            ListDataHolder basicData = new ListDataHolder();
            try
            {
                string loggedInUserName = new LoginUser().UserName;

                List<CategoryTree> activeParents = categoryTreeRepo.GetAllParentCategory(categoryType, loggedInUserName);

                List<CategoryTree> activeParentDropDown = new List<CategoryTree>();
                CategoryTree category = new CategoryTree();
                foreach (CategoryTree item in activeParents)
                {

                    int CategoryTreeDepth = item.Depth;
                    var slash = "";
                    for (int i = 2; i <= CategoryTreeDepth; i++)
                    {
                        slash = slash + "--";
                    }
                    var newItem = slash + item.CategoryName;
                    category.CategoryName = newItem;
                    category.CategoryTreeID = item.CategoryTreeID;
                    activeParentDropDown.Add(category);
                    category = new CategoryTree();
                }

                if (activeParentDropDown != null)
                {
                    message = StatusCodeDescription.successMessage;

                    return Json(messageRepo.GetSuccessMessageWithList(true, message, activeParentDropDown.OfType<dynamic>().ToList(), 0));
                }
                else
                {
                    message = StatusCodeDescription.glossaryErrorMessage;
                    return Json(messageRepo.GetErrorMessageWithList(true, message));
                }

            }
            catch (Exception ex)
            {
                return Json(messageRepo.GetErrorMessageWithList(true, ex.Message.ToString()));
            }

        }

        //[HttpPost]
        //public JsonResult GetAllActiveUserGroup()
        //{
        //    string message = String.Empty;
        //    ListDataHolder basicData = new ListDataHolder();
        //    try
        //    {
        //        List<UserGroup> activeUserGroup = userGroupRepo.GetUserGroup();


        //        if (activeUserGroup != null)
        //        {
        //            message = StatusCodeDescription.successMessage;

        //            return Json(messageRepo.GetSuccessMessageWithList(true, message, activeUserGroup.OfType<dynamic>().ToList(), 0));
        //        }
        //        else
        //        {
        //            message = StatusCodeDescription.glossaryErrorMessage;
        //            return Json(messageRepo.GetErrorMessageWithList(true, message));
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(messageRepo.GetErrorMessageWithList(true, ex.Message.ToString()));
        //    }

        //}


        [HttpPost]
        public JsonResult GetAllActiveParentForAdmin()
        {
            string message = String.Empty;
            ListDataHolder basicData = new ListDataHolder();
            try
            {
                string loggedInUserName = new LoginUser().UserName;

                List<CategoryTree> activeParents = categoryTreeRepo.GetAllParentCategory(categoryType, loggedInUserName);

                List<CategoryTree> activeParentDropDown = new List<CategoryTree>();
                CategoryTree category = new CategoryTree();
                foreach (CategoryTree item in activeParents)
                {

                    int CategoryTreeDepth = item.Depth;
                    var slash = "";
                    for (int i = 2; i <= CategoryTreeDepth; i++)
                    {
                        slash = slash + "&nbsp;&nbsp;&nbsp;";
                    }
                    var newItem = slash + item.CategoryName;
                    category.CategoryName = newItem;
                    category.CategoryTreeID = item.CategoryTreeID;
                    activeParentDropDown.Add(category);
                    category = new CategoryTree();
                }

                if (activeParentDropDown != null)
                {
                    message = StatusCodeDescription.successMessage;

                    return Json(messageRepo.GetSuccessMessageWithList(true, message, activeParentDropDown.OfType<dynamic>().ToList(), 0));
                }
                else
                {
                    message = StatusCodeDescription.glossaryErrorMessage;
                    return Json(messageRepo.GetErrorMessageWithList(true, message));
                }

            }
            catch (Exception ex)
            {
                return Json(messageRepo.GetErrorMessageWithList(true, ex.Message.ToString()));
            }

        }
    }
}
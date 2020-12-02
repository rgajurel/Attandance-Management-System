using DomainInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DomainEntities;
using System.Data;
using Dapper;
using Infrastructure;

namespace Infrastructure
{
    public class CategoryTreeRepository : ICategoryTreeRepository
    {
        public bool AddUpdateCategoryTree(CategoryTree categoryTree)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters categoryParam = new DynamicParameters();

                    categoryParam.Add("@categoryTreeID", categoryTree.CategoryTreeID);
                    categoryParam.Add("@parentCategoryID", categoryTree.ParentCategoryID);
                    categoryParam.Add("@categoryName", categoryTree.CategoryName);
                    categoryParam.Add("@categoryType", categoryTree.CategoryType);
                    categoryParam.Add("@statusValue", categoryTree.StatusValue);
                    categoryParam.Add("@isPublic", categoryTree.IsPublic);
                    categoryParam.Add("@userGroup", categoryTree.UserGroup);
                    categoryParam.Add("@addedBy", categoryTree.AddedBy);
                    categoryParam.Add("@image", categoryTree.Image);
                    connection.Execute("[dbo].[usp_CategoryTreeSave]", categoryParam, commandType: CommandType.StoredProcedure);

                    return true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public bool DeleteCategoryTree(int categoryTreeID)
        //{
        //    try
        //    {
        //        using (IDbConnection connection = DBManager.DbConnect())
        //        {
        //            DynamicParameters categoryParam = new DynamicParameters();
        //            categoryParam.Add("@categoryTreeID", categoryTreeID);

        //           // connection.Execute("[dbo].[usp_CategoryTreeDelete]", categoryParam, commandType: CommandType.StoredProcedure);

        //            return false;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public bool DeleteCategoryTree(int categoryTreeID, int Identifier, string username)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters categoryParam = new DynamicParameters();
                    categoryParam.Add("@categoryTreeID", categoryTreeID);
                    categoryParam.Add("@Identifier", Identifier);
                    categoryParam.Add("@username", username);

                    connection.Execute("[dbo].[usp_LMS_CategoryTreeDelete]", categoryParam, commandType: CommandType.StoredProcedure);

                    return true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public List<CategoryTree> GetAllParentCategory(string categoryType, string loggedInUserName)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    //DynamicParameters categoryParam = new DynamicParameters();
                    //categoryParam.Add("@categoryType", categoryType);

                    //var categoryTreeList = SqlMapper.Query<CategoryTreeDropDown>(
                    //                  connection, "[dbo].[usp_CategoryTreeGetAllActiveParent]", categoryParam, commandType: CommandType.StoredProcedure).ToList();

                    var categoryTreeList = GetCategoryBasedOnUser(categoryType, loggedInUserName);
                    return categoryTreeList;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CategoryTree> GetAllCategoryTree(CategoryTreeSearch categoryTreeSearch)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters categoryParam = new DynamicParameters();
                    categoryParam.Add("@searchParam", categoryTreeSearch.searchParam);
                    categoryParam.Add("@categoryType", categoryTreeSearch.categoryType);
                    categoryParam.Add("@statusID", categoryTreeSearch.statusID);
                    //categoryParam.Add("@identifier", StatusIdentifier.identifierCategoryTree.ToString());
                    categoryParam.Add("@pageSize", categoryTreeSearch.pageSize);
                    categoryParam.Add("@offset", categoryTreeSearch.offSet);

                    var categoryTreeList = SqlMapper.Query<CategoryTree>(
                                      connection, "[dbo].[usp_CategoryTreeGetAll]", categoryParam, commandType: CommandType.StoredProcedure).ToList();

                    List<CategoryTree> categories = new List<CategoryTree>();
                    CategoryTree categoryInfo = new CategoryTree(); ;
                    foreach (CategoryTree category in categoryTreeList)
                    {
                        categoryInfo.CategoryName = TableCategoryName(category.childs, categoryTreeList.ToList());
                        categoryInfo.CategoryTreeID = category.CategoryTreeID;
                        categoryInfo.Depth = category.Depth;
                        categoryInfo.Status = category.Status;
                        categoryInfo.AddedOn = category.AddedOn;
                        categoryInfo.UpdatedOn = category.UpdatedOn;
                        categoryInfo.CreatedOn = category.CreatedOn;
                        categoryInfo.ModifiedOn = category.ModifiedOn;
                        categories.Add(categoryInfo);
                        categoryInfo = new CategoryTree();
                    }


                    return categories.Skip(categoryTreeSearch.offSet).Take(categoryTreeSearch.pageSize).ToList();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string TableCategoryName(string childs, List<CategoryTree> objLst)
        {
            string temp = "";
            List<string> lstCategoryName = new List<string>();
            List<string> TagIds = childs.Split(',').ToList();
            int Count = TagIds.Count;
            for (int i = 1; i < Count; i++)
            {
                foreach (var item in objLst)
                {
                    if (item.CategoryTreeID.ToString() == TagIds[i - 1])
                    {
                        if (i != Count - 1)
                        {
                            temp = temp + item.CategoryName + "->";
                        }
                        else
                        {
                            temp = temp + item.CategoryName;
                        }
                    }
                }
                lstCategoryName.Add(temp);
            }
            return temp;
        }

        public CategoryTree GetCategoryTreeByID(int categoryTreeID)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters categoryParam = new DynamicParameters();
                    categoryParam.Add("@categoryTreeID", categoryTreeID);
                    var categoryTreeInfo = SqlMapper.Query<CategoryTree>(
                                      connection, "[dbo].[usp_CategoryTreeGetByID]", categoryParam, commandType: CommandType.StoredProcedure).SingleOrDefault();

                    return categoryTreeInfo;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int GetTotalCategoryTreeFound(CategoryTreeSearch categoryTreeSearch)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters categoryParam = new DynamicParameters();
                    categoryParam.Add("@searchParam", categoryTreeSearch.searchParam);
                    categoryParam.Add("@categoryType", categoryTreeSearch.categoryType);
                    categoryParam.Add("@statusID", categoryTreeSearch.statusID);
                    categoryParam.Add("@total", dbType: DbType.Int32, direction: ParameterDirection.Output);

                    connection.Execute("[dbo].[usp_CategoryTreeGetTotal]", categoryParam, commandType: CommandType.StoredProcedure);

                    var total = categoryParam.Get<int>("@total");
                    return total;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CategoryTree> GetCategoryBasedOnUser(string categoryType, string loggedInUserName)
        {
            using (IDbConnection connection = DBManager.DbConnect())
            {
                DynamicParameters categoryParam = new DynamicParameters();
                categoryParam.Add("@categoryType", categoryType);
                categoryParam.Add("@userName", loggedInUserName);

                List<CategoryTree> rawlist = SqlMapper.Query<CategoryTree>(
                                  connection, "[dbo].[usp_CategoryGet]", categoryParam, commandType: CommandType.StoredProcedure).ToList();

                StringBuilder html = new StringBuilder();

                CategoryTree c = new CategoryTree();
                List<CategoryTree> d = new List<CategoryTree>();
                List<CategoryTree> temp = new List<CategoryTree>();
                foreach (CategoryTree item in rawlist.Where(m => m.ParentCategoryID == 0))
                {
                    if (item.ParentCategoryID == 0)
                    {
                        c.CategoryTreeID = item.CategoryTreeID;
                        c.CategoryName = item.CategoryName;
                        c.ParentCategoryID = item.ParentCategoryID;
                        c.Depth = item.Depth;

                        d.Add(c);
                        temp = GetChildCategory(rawlist, item.CategoryTreeID);
                        foreach (var v in temp)
                        {
                            d.Add(v);
                        }
                        c = new CategoryTree();
                        temp = new List<CategoryTree>(); ;
                    }
                }
                return d;
            }
        }

        private List<CategoryTree> GetChildCategory(List<CategoryTree> rawList, int catID)
        {


            CategoryTree c = new CategoryTree();
            List<CategoryTree> d = new List<CategoryTree>();
            List<CategoryTree> temp = new List<CategoryTree>();
            d.Clear();

            StringBuilder html = new StringBuilder();

            foreach (CategoryTree childItem in rawList)
            {

                if (childItem.ParentCategoryID == catID)
                {
                    c.CategoryTreeID = childItem.CategoryTreeID;
                    c.CategoryName = childItem.CategoryName;
                    c.ParentCategoryID = childItem.ParentCategoryID;
                    c.Depth = childItem.Depth;


                    d.Add(c);
                    c = new CategoryTree();
                    temp = GetChildCategory(rawList, childItem.CategoryTreeID);
                    foreach (var v in temp)
                    {
                        d.Add(v);
                    }
                    temp = new List<CategoryTree>(); ;
                }


            }

            return d;
        }

        //public List<string> GetUserGroupIDinCategory(string categoryType, int courseCode)
        //{
        //    try
        //    {
        //        using (IDbConnection connection = DBManager.DbConnect())
        //        {
        //            DynamicParameters categoryParam = new DynamicParameters();
        //            categoryParam.Add("@categoryType", categoryType);
        //            //categoryParam.Add("@identifier", StatusIdentifier.identifierCourse);
        //            categoryParam.Add("@courseCode", courseCode);
        //            List<String> categoryTreeList = SqlMapper.Query<string>(
        //                              connection, "[dbo].[usp_CategoryTreeGetByCategoryType]", categoryParam, commandType: CommandType.StoredProcedure).ToList();

        //            return categoryTreeList.ToList();
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public List<string> GetUserNameinCategory(string categoryType, int courseCode)
        //{
        //    try
        //    {
        //        using (IDbConnection connection = DBManager.DbConnect())
        //        {

        //            return GetUserGroupIDinCategory(categoryType, courseCode);
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public int GetNumberOfUserinCategory(string categoryType, int courseCode)
        //{
        //    int numberOfUserIds = GetUserNameinCategory(categoryType, courseCode).Count();
        //    return numberOfUserIds;
        //}

        public ReturnType DeleteCategory(string categoryType, int categoryTreeID)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters categoryParam = new DynamicParameters();
                    categoryParam.Add("@categoryTreeID", categoryTreeID);
                    categoryParam.Add("@categoryType", categoryType);
                    var returnType = new ReturnType();
                    ReturnType categoryHasParent = SqlMapper.Query<ReturnType>(
                                    connection, "[dbo].[usp_CategoryTreeCheckIfParent]", categoryParam, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    if (categoryHasParent.Result)
                    {
                        if (categoryType.Equals(CategoryType.categoryFAQ))
                        {
                            returnType = SqlMapper.Query<ReturnType>(
                                         connection, "[dbo].[usp_CategoryTreeFAQDelete]", categoryParam, commandType: CommandType.StoredProcedure).FirstOrDefault();
                        }
                        else if (categoryType.Equals(CategoryType.categoryNews))
                        {
                            returnType = SqlMapper.Query<ReturnType>(
                                         connection, "[dbo].[usp_CategoryTreeNewsDelete]", categoryParam, commandType: CommandType.StoredProcedure).FirstOrDefault();
                        }
                        else if (categoryType.Equals(CategoryType.categoryCourse))
                        {
                            returnType = SqlMapper.Query<ReturnType>(
                                         connection, "[dbo].[usp_CategoryTreeCourseDelete]", categoryParam, commandType: CommandType.StoredProcedure).FirstOrDefault();
                        }
                        else if (categoryType.Equals(CategoryType.CategoryArticle))
                        {
                            returnType = SqlMapper.Query<ReturnType>(
                                         connection, "[dbo].[usp_LMS_ArticleCategoryTreeDelete]", categoryParam, commandType: CommandType.StoredProcedure).FirstOrDefault();
                        }
                        else if (categoryType.Equals(CategoryType.CategoryQuiz))
                        {
                            returnType = SqlMapper.Query<ReturnType>(
                                         connection, "[Quiz].[usp_LMS_QuizCategoryTreeDelete]", categoryParam, commandType: CommandType.StoredProcedure).FirstOrDefault();
                        }
                        else if (categoryType.Equals(CategoryType.CategoryQuizQuestion))
                        {
                            returnType = SqlMapper.Query<ReturnType>(
                                         connection, "[Quiz].[usp_LMS_QuizQuestionCategoryTreeDelete]", categoryParam, commandType: CommandType.StoredProcedure).FirstOrDefault();
                        }
                        else if (categoryType.Equals(CategoryType.CategorySurvey))
                        {
                            returnType = SqlMapper.Query<ReturnType>(
                                         connection, "[Survey].[usp_LMS_SurveyCategoryTreeDelete]", categoryParam, commandType: CommandType.StoredProcedure).FirstOrDefault();
                        }
                        else if (categoryType.Equals(CategoryType.CategorySurveyQuestion))
                        {
                            returnType = SqlMapper.Query<ReturnType>(
                                         connection, "[Survey].[usp_SurveyQuestionCategory_DeleteSurveyQuestionCategory]", categoryParam, commandType: CommandType.StoredProcedure).FirstOrDefault();
                        }
                        else if (categoryType.Equals(CategoryType.categoryInformationCenter))
                        {
                            returnType = SqlMapper.Query<ReturnType>(
                                         connection, "[dbo].[usp_CategoryTreeInformationCenterDelete]", categoryParam, commandType: CommandType.StoredProcedure).FirstOrDefault();
                        }
                        else if (categoryType.Equals(CategoryType.CategoryEntranceQuestion))
                        {
                            returnType = SqlMapper.Query<ReturnType>(
                                         connection, "[dbo].[usp_CategoryTreeEntranceQuestionDelete]", categoryParam, commandType: CommandType.StoredProcedure).FirstOrDefault();
                        }
                        return returnType;
                    }
                    else {
                        return categoryHasParent;
                    }
                 
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public List<CategoryTree> GetAllParentCategory(string categoryType, string loggedInUserName)
        //{
        //    throw new NotImplementedException();
        //}
    }
}

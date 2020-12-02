using DomainEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainInterface
{
    public interface ICategoryTreeRepository
    {
        List<CategoryTree> GetAllCategoryTree(CategoryTreeSearch categoryTreeSearch);
        bool AddUpdateCategoryTree(CategoryTree categoryTree);
        CategoryTree GetCategoryTreeByID(int categoryTreeID);
        //bool DeleteCategoryTree(int categoryTreeID);
        bool DeleteCategoryTree(int categoryTreeID, int Identifier, string username);
        int GetTotalCategoryTreeFound(CategoryTreeSearch categoryTreeSearch);
        List<CategoryTree> GetAllParentCategory(string categoryType, string loggedInUserName);
        //List<string> GetUserGroupIDinCategory(string categoryType, int courseCode);
        //List<string> GetUserNameinCategory(string categoryType, int courseCode);
        //int GetNumberOfUserinCategory(string categoryType, int courseCode);
        ReturnType DeleteCategory(string categoryType, int categoryTreeID);
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{

    public class CategoryTree
    {
        public int CategoryTreeID { get; set; }

        public string categoryIDs { get; set; }//to hold comma separated categorytreeid as well

        [Required(ErrorMessage = "Parent Required")]
        [DisplayName("Parent")]
        public int ParentCategoryID { get; set; }
        public string CategoryType { get; set; }
        [Required(ErrorMessage = "Category Name Required")]
        [DisplayName("Category Name")]
        //[StringLength(50)]
        public string CategoryName { get; set; }
        public bool IsParent { get; set; }
        
        [DisplayName("is Public")]
        public bool IsPublic { get; set; }
        public int Depth { get; set; }
        [DisplayName("Status")]
        [Required(ErrorMessage = "Status Required")]
        public int StatusValue { get; set; }
        [DisplayName("User Group")]
        public string UserGroup { get; set; }
        public DateTime AddedOn { get; set; }
        public string AddedBy { get; set; }
        public Nullable<DateTime> UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public string childs { get; set; }
        public string Image { get; set; }
        public int[] userGroupArray { get; set; }
        public int CategoryID { get; set; }
        public string CategoryTreeID1 { get; set; }

        
        #region For Grid
        public string Status { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedOn { get; set; }
        #endregion
    }

    public class CategoryTreeSearch
    {
        public int statusID { get; set; }
        public string searchParam { get; set; }
        public int pageSize { get; set; }
        public int pageNumber { get; set; }
        public int offSet { get; set; }
        public string categoryType { get; set; }
    }

    public class CategoryTreeDropDown
    {

        public int CategoryTreeID { get; set; }
        public int Depth { get; set; }
        public string CategoryName { get; set; }
    }
}

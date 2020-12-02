using DomainEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainInterface
{
    public interface ICommonFeeDiscountRepository
    {
        #region Admin
        Class GetSectionBasedOnClass(string Class,string Faculty);
        List<CommonFeeDiscount> GetClassBasedOnFaculty(string faculty);
        List<CommonFeeDiscount> GetFeeTypeBasedOnSection(string facultyID, string sessionId,string classId,string section);
        List<CommonFeeDiscount> GetMonthBasedOnFeeType(string facultyID, string sessionId, string classId, string section,string type);
        string AddUpdateCommonFeeDiscount(List<CommonFeeDiscount> discounts, string facultyID, string sessionId, string classId, string section, string type, string month);
        List<CommonFeeDiscount> GetAllCommonFeeDiscount(CommonFeeDiscount commonFeeDiscount);
        #endregion
    }
}

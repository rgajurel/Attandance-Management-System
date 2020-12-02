using DomainEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainInterface
{
    public interface ICommonFeeRepository
    {
        #region Admin
        Class GetSectionBasedOnClass(string id,string faculty);
        List<CommonFee> GetClassBasedOnFaculty(string faculty);
        string AddUpdateCommonFee(CommonFee fee);
        List<CommonFee> GetAllCommonFee();
        bool DeleteCommonFee(int id);
        CommonFee EditCommonFee(int id);
        #endregion
    }
}

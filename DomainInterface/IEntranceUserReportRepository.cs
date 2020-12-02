using DomainEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainInterface
{
   public interface IEntranceUserReportRepository
    {
        IEnumerable<EntranceUserReport> GetAllEntranceUserListing(SearchParamEntranceUserReport objInfo);
        IEnumerable<EntranceAllUser> GetAllUserForEntrance();
        EntranceQuestionUserReport UserEntranceAnswerByUserID(int EntranceUserID);
        bool AssignMarktoUser(int UserScore, int ID);
    }
}

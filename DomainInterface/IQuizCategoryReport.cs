using DomainEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainInterface
{
    public interface IQuizCategoryReport 
    {
        IEnumerable<QuizCategoryReport> GetAllQuizListing(SearchParamQuizCategoryreport objInfo);
    }
}

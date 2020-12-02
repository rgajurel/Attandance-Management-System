using DomainEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainInterface
{
    public interface ILanguageRepository
    {
        #region Admin
        bool AddUpdateLanguage(Language department);
        List<Language> GetAllLanguage();
        bool DeleteLanguage(int id);
        Language EditLanguage(int id);
        #endregion
    }
}

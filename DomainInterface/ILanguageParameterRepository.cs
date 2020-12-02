using DomainEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainInterface
{
   public interface ILanguageParameterRepository
    {
        #region Admin
        bool AddUpdateLanguageParameter(LangaugeParameter languageParameter);
        List<LangaugeParameter> GetAllLanguageParameter();
        bool DeleteLanguageParameter(int id);
        LangaugeParameter EditLanguageParameter(int id);
        #endregion
    }
}

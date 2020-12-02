using DomainEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainInterface
{
  public interface IMessageHandlerRepository
    {
        MessageHolder GetMessage(string ErrorMessage);
        MessageHolder GetErrorMessage(bool isAuthorized, string ErrorMessage);
        MessageHolder GetSuccessMessage(bool isAuthorized, string SuccessMessage);
        ListDataHolder GetErrorMessageWithList(bool isAuthorized, string ErrorMessage);
        ListDataHolder GetErrorMessageWithListAlongWithErrorList(bool isErrorOccured, string messages, List<dynamic> data);

        #region To Get Message along with data
        ListDataHolder GetSuccessMessageWithList(bool isAuthorized, string SuccessMessage, List<dynamic> data, int total);
        DataHolder GetErrorMessageWithData(bool isAuthorized, string ErrorMessage, dynamic data);
        DataHolder GetSuccessMessageWithData(bool isAuthorized, string SuccessMessage, dynamic data);
        #endregion
    }
}

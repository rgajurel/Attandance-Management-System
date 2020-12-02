using DomainInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainEntities;

namespace Infrastructure
{
    public class MessageHandlerRepository : IMessageHandlerRepository
    {
        public MessageHolder GetMessage(string message)
        {
            MessageHolder messageHolder = new MessageHolder();
            messageHolder.Message = message;           
            return messageHolder;           

        }

        public MessageHolder GetSuccessMessage(bool isAuthorized, string successMessage)
        {
            MessageHolder message = new MessageHolder();
            message.Code = StatusCodeDescription.success;
            message.isAuthorized = isAuthorized;
            message.Message = successMessage;
            message.ErrorOccured = false;
            return message;
        }

        public MessageHolder GetErrorMessage(bool isAuthorized, string errorMessage)
        {
            MessageHolder message = new MessageHolder();
            message.Code = StatusCodeDescription.failure;
            message.isAuthorized = isAuthorized;
            message.Message = errorMessage;
            message.ErrorOccured = true;
            return message;
        }

        public ListDataHolder GetSuccessMessageWithList(bool isAuthorized, string successMessage, List<dynamic> data, int total)
        {
            ListDataHolder message = new ListDataHolder();
            message.Code = StatusCodeDescription.success;
            message.isAuthorized = isAuthorized;
            message.Message = successMessage;
            message.ErrorOccured = false;
            message.Data = data;
            message.Total = total;
            return message;
        }

        public ListDataHolder GetErrorMessageWithList(bool isAuthorized, string errorMessage)
        {
            ListDataHolder message = new ListDataHolder();
            message.Code = StatusCodeDescription.failure;
            message.isAuthorized = isAuthorized;
            message.Message = errorMessage;
            message.ErrorOccured = true;
            message.Data = null;
            message.Total = 0;
            return message;
        }


        public ListDataHolder GetErrorMessageWithListAlongWithErrorList(bool isErrorOccured, string messages, List<dynamic> data)
        {
            ListDataHolder message = new ListDataHolder();
            message.Message = messages;
            message.ErrorOccured = isErrorOccured;
            message.Data = data;           
            return message;
        }
        public DataHolder GetErrorMessageWithData(bool isAuthorized, string errorMessage, dynamic data)
        {
            DataHolder message = new DataHolder();
            message.Code = StatusCodeDescription.failure;
            message.isAuthorized = isAuthorized;
            message.Message = errorMessage;
            message.ErrorOccured = true;
            message.data = data;
            return message;
        }

        public DataHolder GetSuccessMessageWithData(bool isAuthorized, string successMessage, dynamic data)
        {
            DataHolder message = new DataHolder();
            message.Code = StatusCodeDescription.failure;
            message.isAuthorized = isAuthorized;
            message.Message = successMessage;
            message.ErrorOccured = false;
            message.data = data;
            return message;
        }
    }
}

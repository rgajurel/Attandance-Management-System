using DomainEntities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;

namespace SchoolManagementSystem
{
    public class CustomExceptionHandler: ExceptionHandler
    {
        public override void Handle(ExceptionHandlerContext context)
        {
            var message = new List<ResponseMessage>();
            try
            {
                var ex = context.Exception as ApiException ?? new ApiException(HttpStatusCode.InternalServerError, null);
                if (ex.StatusCode==HttpStatusCode.BadRequest)
                { 
                    message.Add(new ResponseMessage() { Title = "Authorized", Message = "UnAuthorized" });
                }
                else
                {
                    message.Add(new ResponseMessage() { Title = "message", Message = context.Exception.Message });
                }
                context.Result = new ExceptionResponse()
                {
                    statusCode = ex.StatusCode.Value,
                    message = JsonConvert.SerializeObject(message),
                    request = context.Request
                };
            }
            catch
            {
                context.Result = new ExceptionResponse()
                {
                    statusCode = HttpStatusCode.InternalServerError,
                    message = JsonConvert.SerializeObject(new ResponseMessage() { Message="Error Occured"}),
                    request = context.Request
                };
            }
        }


        public override bool ShouldHandle(ExceptionHandlerContext context)
        {
          return  true;
        }

    }

    public class ExceptionResponse:IHttpActionResult
    {
        public HttpStatusCode statusCode { get; set; }
        public string message { get; set; }
        public HttpRequestMessage request { get; set; }

        public Task<HttpResponseMessage>ExecuteAsync(CancellationToken cancellationToken)
        {
            var response = new HttpResponseMessage(statusCode);
            response.RequestMessage = request;
            response.Content = new StringContent(message);
            return Task.FromResult(response);
        }
    }        

     

      

    public class ResponseMessage
    {
        public string Title { get; set; }
        public string Message { get; set; }
    }
}
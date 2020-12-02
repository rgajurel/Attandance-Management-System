using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;

namespace DomainEntities
{      
     public class ApiException : Exception
    {
            public ApiException(HttpStatusCode statuscode, string jsonData)            
            {
                StatusCode = statuscode;
                JsonData = jsonData;
            }
            public HttpStatusCode? StatusCode { get; set; }
            public string JsonData { get; set; }
        }



        public class ResponseMessage
        {
            public string Title { get; set; }
            public string Message { get; set; }
        }
    }


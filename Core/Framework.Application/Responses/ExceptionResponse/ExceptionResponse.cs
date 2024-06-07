using Framework.Application.Exceptions.Common;
using Framework.Application.Responses.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using JsonIgnoreAttribute = Newtonsoft.Json.JsonIgnoreAttribute;

namespace Framework.Application.Responses.ExceptionResponse
{
    public class ExceptionResponse : IExceptionResponse
    {
        [JsonIgnore]
        public Exception Error { get; }

        public ExceptionDetailsBase ExceptionDetails { get; set; }
        public String Title { get; set; }
        public String Message { get; set; }
        public bool IsSucceeded { get { return false; } }
        public HttpStatusCode StatusCode { get; set; }

        public String GetDetails() => JsonConvert.SerializeObject(this);

        public ExceptionResponse(Exception error, ExceptionDetailsBase exceptionDetails, String title, String message, HttpStatusCode statusCode)
        {
            Error = error;
            ExceptionDetails = exceptionDetails;
            Title = title;
            Message = message;
            StatusCode = statusCode;
        }
    }
}

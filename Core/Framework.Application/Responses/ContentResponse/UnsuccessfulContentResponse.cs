using Framework.Application.Responses.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Application.Responses.ContentResponse
{
    public class UnsuccessfulContentResponse<T> : ContentResponseBase<T>
    {
        public UnsuccessfulContentResponse(T content, String title, String message, HttpStatusCode httpStatusCode) : base(content, title, message, httpStatusCode, false)
        {
        }

        public UnsuccessfulContentResponse(String title, String message, HttpStatusCode httpStatusCode) : base(title, message, httpStatusCode, false)
        {
        }

        public UnsuccessfulContentResponse(String message, HttpStatusCode httpStatusCode) : base(message, httpStatusCode, false)
        {
        }

        public UnsuccessfulContentResponse(HttpStatusCode httpStatusCode) : base(httpStatusCode, false)
        {
        }

        public UnsuccessfulContentResponse() : base(false)
        {
        }
    }
}

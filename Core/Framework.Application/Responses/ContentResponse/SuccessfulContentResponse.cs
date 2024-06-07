using Framework.Application.Responses.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Application.Responses.ContentResponse
{
    public class SuccessfulContentResponse<T> : ContentResponseBase<T>
    {
        public SuccessfulContentResponse(T content, String title, String message, HttpStatusCode httpStatusCode) : base(content, title, message, httpStatusCode, true)
        {
        }

        public SuccessfulContentResponse(T content, String message, HttpStatusCode httpStatusCode) : base(content, message, httpStatusCode, true)
        {
        }

        public SuccessfulContentResponse(T content, HttpStatusCode httpStatusCode) : base(content, httpStatusCode, true)
        {
        }

        public SuccessfulContentResponse(T content) : base(content, true)
        {
        }
    }
}

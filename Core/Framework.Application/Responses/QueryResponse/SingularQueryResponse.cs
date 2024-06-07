using Framework.Application.Responses.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Application.Responses.QueryResponse
{
    public class SingularQueryResponse<T>
    {
        public IContentResponse<T> Response { get; set; }

        public SingularQueryResponse(IContentResponse<T> response)
        {
            Response = response;
        }
    }
}

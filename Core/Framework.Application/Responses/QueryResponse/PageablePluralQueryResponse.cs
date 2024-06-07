using Framework.Application.Responses.Common;
using Framework.Persistence.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Application.Responses.QueryResponse
{
    public class PageablePluralQueryResponse<T>
    {
        public IContentResponse<Paginate<T>> Response { get; set; }

        public PageablePluralQueryResponse(IContentResponse<Paginate<T>> response)
        {
            Response = response;
        }
    }
}

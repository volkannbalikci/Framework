using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Application.Requests
{
    public class PagedQueryRequest
    {
        public int PageSize { get; set; }
        public int PageIndex { get; set; }

        public PagedQueryRequest()
        {

        }

        public PagedQueryRequest(int pageSize, int pageIndex)
        {
            PageSize = pageSize;
            PageIndex = pageIndex;
        }
    }
}

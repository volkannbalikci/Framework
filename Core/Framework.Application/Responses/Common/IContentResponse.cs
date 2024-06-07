using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Application.Responses.Common
{
    public interface IContentResponse<T> : IResponse
    {
        T Content { get; }
    }
}

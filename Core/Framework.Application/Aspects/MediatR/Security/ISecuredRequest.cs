using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Application.Aspects.MediatR.Security
{
    public interface ISecuredRequest
    {
        public String[] OperationClaimNames { get; }
    }
}

using Framework.Application.Exceptions.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Application.Exceptions.Auth
{
    public class AuthenticationExceptionDetails : ExceptionDetailsBase
    {
        public AuthenticationExceptionDetails()
        {
            this.ThrownDate = DateTime.Now;
        }
    }
}

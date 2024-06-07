using Framework.Application.Exceptions.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Application.Exceptions.InternalServer
{
    public class InternalServerExceptionDetails : ExceptionDetailsBase
    {
        public String ServerName { get; set; }

        public InternalServerExceptionDetails(String serverName)
        {
            this.ThrownDate = DateTime.Now;
            this.ServerName = serverName;
        }

    }
}

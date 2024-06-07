using Framework.Application.Exceptions.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Application.Exceptions.Validation
{
    public class ValidationExceptionDetails : ExceptionDetailsBase
    {
        public object Errors { get; set; }

        public ValidationExceptionDetails(object errors)
        {
            this.ThrownDate = DateTime.Now;
            this.Errors = errors;
        }
    }
}

﻿using Framework.Application.Exceptions.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Application.Exceptions.BusinessLogic
{
    public class BusinessLogicExceptionDetails : ExceptionDetailsBase
    {
        public String OperationName { get; set; }


        public BusinessLogicExceptionDetails()
        {

        }

        public BusinessLogicExceptionDetails(String operationName)
        {
            this.ThrownDate = DateTime.Now;
            this.OperationName = operationName;
        }
    }
}

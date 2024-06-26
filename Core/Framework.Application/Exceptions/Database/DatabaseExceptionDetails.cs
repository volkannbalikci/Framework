﻿using Framework.Application.Exceptions.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Application.Exceptions.Database
{
    public class DatabaseExceptionDetails : ExceptionDetailsBase
    {
        public String DatabaseName { get; set; }

        public DatabaseExceptionDetails(String databaseName)
        {
            this.ThrownDate = DateTime.Now;
            this.DatabaseName = databaseName;
        }

    }
}

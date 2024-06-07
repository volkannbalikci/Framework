using Framework.Application.Constants;
using Framework.Application.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Application.Exceptions.Database
{
    public class DatabaseException : Exception
    {
        public DatabaseException(String title, String message, String databaseName) : base(StringFormatter.GetAlignedItemsByPunctuation(PunctuationConstants.Colon, title, message, databaseName))
        {
        }
    }   
}

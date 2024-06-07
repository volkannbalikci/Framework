using Framework.Application.Constants;
using Framework.Application.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Application.Exceptions.InternalServer
{
    public class InternalServerException : Exception
    {
        public InternalServerException(String title, String message, String serverName) : base(StringFormatter.GetAlignedItemsByPunctuation(PunctuationConstants.Colon, title, message, serverName))
        {
        }
    }
}

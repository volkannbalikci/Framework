using Framework.Application.Constants;
using Framework.Application.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Application.Exceptions.Auth
{
    public class AuthorizationException : Exception
    {
        public AuthorizationException(String title, String message, String userEmail) : base(StringFormatter.GetAlignedItemsByPunctuation(PunctuationConstants.Colon, title, message, userEmail))
        {

        }
    }
}

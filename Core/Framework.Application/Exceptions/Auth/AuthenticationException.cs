using Framework.Application.Constants;
using Framework.Application.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Application.Exceptions.Auth
{
    public class AuthenticationException : Exception
    {
        public AuthenticationException(String title, String message) : base(StringFormatter.GetAlignedItemsByPunctuation(PunctuationConstants.Colon, title, message))
        {
        }
    }

}

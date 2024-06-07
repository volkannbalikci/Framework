using Framework.Application.Constants;
using Framework.Application.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Application.Exceptions.BusinessLogic
{
    public class BusinessLogicException : Exception
    {
        public BusinessLogicException(String title, String message, String operationName) : base(StringFormatter.GetAlignedItemsByPunctuation(PunctuationConstants.Colon, title, message, operationName))
        {
        }
    }
}

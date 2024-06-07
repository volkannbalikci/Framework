using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Application.Utilities
{
    public static class StringFormatter
    {
        public static String GetAlignedItemsByPunctuation(String punctuation, params String[] items)
        {
            StringBuilder stringBuilder = new StringBuilder();

            int lastIndex = items.Length - 1;
            
            for (int i = 0; i < items.Length; i++)
            {
                if(i != lastIndex)
                {
                    stringBuilder.Append(items[i]);
                    stringBuilder.Append(punctuation);
                }    
            }


            return stringBuilder.ToString();
        }
    }
}

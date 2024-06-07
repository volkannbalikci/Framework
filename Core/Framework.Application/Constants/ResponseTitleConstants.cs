using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Application.Constants
{
    public static class ResponseTitleConstants
    {
        public const String Success = "Başarılı";
        public const String Error = "Hata";
        public const String Information = "Bilgi";
        public const String Warning = "Dikkat";
        public const String Unsuccesful = "Başarısız";

        public const String InternalExceptionTitle = "Sunucu Hatası";
        public const String BusinessLogicExceptionTitle = "Operasyonel Mantık Hatası";
        public const String AuthorizationExceptionTitle = "Yetkilendirme Hatası";
        public const String DatabaseExceptionTitle = "Veritabanı Hatası";
        public const String ValidationExceptionTitle = "Doğrulama Hatası";
    }
}

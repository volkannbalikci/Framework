using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Application.Security.JWT;

public class AccessToken
{
    public string Token { get; set; }
    public DateTime Expiration { get; set; }

    public AccessToken()
    {
        Token = string.Empty;
    }

    public AccessToken(string token, DateTime expiration)
    {
        Token = token;
        Expiration = expiration;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Application.Dtos.Security;

public class AccessToken
{
    public string Token { get; set; }
    public DateTime ExpirationDate { get; set; }
}

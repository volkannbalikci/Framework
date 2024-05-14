using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Application.Security.JWT;

public interface ITokenHelper
{
        AccessToken CreateTokenForIndividualUser(Guid userId, String individualUserFirstName, String individualUserLastName, String userEmail, String[] usersRoleNames);

        AccessToken CreateTokenForCorporateUser(Guid userId, String corporateUserComapanyName, String corporateUserTaxIdentityNumber, String userEmail, String[] usersRoleNames);

        AccessToken CreateTokenForAuthorizedUser(Guid userId, String registryNumber, String userEmail, String[] usersRoleNames);
}
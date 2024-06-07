using Framework.Application.Constants;
using Framework.Application.Exceptions.Auth;
using Framework.Application.Extensions.ClaimExtensions;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Application.Aspects.MediatR.Security
{
    public class AuthorizationAspect<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
       where TRequest : IRequest<TResponse>, ISecuredRequest
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        //private readonly IUserQueryRepository _userQueryRepository;

        public AuthorizationAspect(IHttpContextAccessor httpContextAccessor/*, IUserQueryRepository userQueryRepository*/)
        {
            _httpContextAccessor = httpContextAccessor;
            //_userQueryRepository = userQueryRepository;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var usersEmail = _httpContextAccessor.HttpContext.GetUsersEmail();
            var usersRoleNames = _httpContextAccessor.HttpContext.GetUsersRoles();

            //var usersOperationClaimNames = _userQueryRepository.GetOperationClaimsByRoleNames(usersRoleNames).Select(e => e.ClaimName).ToArray();

            //this.CheckIfUserHasAnyOperationClaims(usersOperationClaimNames, usersEmail);

            //this.CheckIfUserOperationClaimsSatisfiesRequest(request, usersOperationClaimNames, usersEmail);

            return await next();
        }

        private void CheckIfUserHasAnyOperationClaims(String[] usersOperationClaimNames, String usersEmail)
        {
            if (usersOperationClaimNames is null || usersOperationClaimNames.Length == 0)
                throw new AuthorizationException(ResponseTitleConstants.AuthorizationExceptionTitle, ResponseMessageConstants.AuthorizationExceptionMessage, usersEmail);
        }

        private void CheckIfUserOperationClaimsSatisfiesRequest(TRequest request, String[] usersOperationClaims, String usersEmail)
        {
            if (this.CheckIfUsersOperationClaimsNotSatisfiesRequestsOperationClaims(request, usersOperationClaims) is true)
                throw new AuthorizationException(ResponseTitleConstants.AuthorizationExceptionTitle, ResponseMessageConstants.AuthorizationExceptionMessage, usersEmail);
        }

        private bool CheckIfUsersOperationClaimsNotSatisfiesRequestsOperationClaims(TRequest request, String[] usersOperationClaimsNames)
        {
            var result = usersOperationClaimsNames.FirstOrDefault(userOperationClaimName => request.OperationClaimNames.Any(requestOperationClaimName => requestOperationClaimName == userOperationClaimName));

            if (result is null || result == String.Empty)
                return true;

            return false;
        }
    }
}

using Framework.Application.Aspects.MediatR.Security;
using Framework.Application.Aspects.MediatR.Transaction;
using Framework.Application.Aspects.MediatR.Validation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Application.Extensions.Registrations.DIRegistrationExtensions
{
    internal static class AspectRegistrationExtension
    {
        internal static void RegisterMediatRPipelines(this IServiceCollection services)
        {
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(AuthorizationAspect<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionAspect<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationAspect<,>));
        }
    }
}

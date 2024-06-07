using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Application.Extensions.Registrations.DIRegistrationExtensions
{
    internal static class MediatRRegistrationExtension
    {
        internal static void RegisterMediatR(this IServiceCollection services, Assembly assembly)
        {
            services.AddMediatR(configuration =>
            {
                configuration.RegisterServicesFromAssembly(assembly);
            });
        }
    }
}

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Application.Extensions.Registrations.DIRegistrationExtensions
{
    public static class LayerRegistrationExtension
    {
        public static void RegisterApplicationDependencies(this IServiceCollection services, IConfiguration configuration, Assembly assembly)
        {
            services.RegisterMediatR(assembly);

            services.RegisterMediatRPipelines();
        }
    }
}

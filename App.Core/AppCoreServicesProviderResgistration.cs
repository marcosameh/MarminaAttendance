using App.Core.Managers;
using AppCore.Infrastructure;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;

namespace AppCore
{
    public static class AppCoreServicesProviderResgistration
    {
      


        public static IServiceCollection AddAppCoreServices(this IServiceCollection services)
        {


            services.AddScoped<ClassManager>();
                       

            return services;
        }
    }
}

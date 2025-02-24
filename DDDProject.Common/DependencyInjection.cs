using Microsoft.Extensions.DependencyInjection;
using DDDProject.Common.Localization;

namespace DDDProject.Common
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddCommonServices(this IServiceCollection services)
        {
            services.AddLocalization(options => options.ResourcesPath = "Resources");
            services.AddSingleton<SharedLocalizer>();

            return services;
        }
    }
}
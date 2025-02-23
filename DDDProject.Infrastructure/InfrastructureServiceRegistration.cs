using Microsoft.Extensions.DependencyInjection;
using DDDProject.Infrastructure.Caching;

namespace DDDProject.Infrastructure
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddSingleton<RedisCacheService>();
            return services;
        }
    }
}
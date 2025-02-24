
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DDDProject.Infrastructure.Configurations
{
    public static class HangfireConfig
    {
        public static void ConfigureHangfire(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHangfire(config => config
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UsePostgreSqlStorage(configuration.GetConnectionString("HangfireConnection"), new PostgreSqlStorageOptions
                {
                    SchemaName = "hangfire"
                }));

            services.AddHangfireServer();
        }
    }
}

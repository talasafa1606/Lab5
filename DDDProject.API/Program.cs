using DDDProject.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.OData;
using Microsoft.Extensions.DependencyInjection;
using DDDProject.Infrastructure.Caching;
using StackExchange.Redis;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddInfrastructureServices();

        builder.Services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ReportApiVersions = true;
        });

        builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect("localhost:6379"));
        builder.Services.AddScoped<IRedisCachingService, RedisCacheService>();

        builder.Services.AddVersionedApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
        });

        builder.Services.AddControllers()
            .AddOData(options => options.Select().Filter().OrderBy().Count().Expand());

        var app = builder.Build();

        app.UseRouting();
        app.UseAuthorization();
        app.MapControllers();

        app.Run();
    }
}
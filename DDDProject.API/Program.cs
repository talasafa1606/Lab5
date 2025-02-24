using DDDProject.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.OData;
using Microsoft.Extensions.DependencyInjection;
using DDDProject.Infrastructure.Caching;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using StackExchange.Redis;
using Microsoft.Extensions.Caching.Distributed;
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


        builder.Services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = builder.Configuration["Redis:ConnectionString"];
            options.InstanceName = "MyAppRedisCache";
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

        app.MapGet("/", async (HttpContext context) =>
        {
            var cache = context.RequestServices.GetRequiredService<IDistributedCache>();

            string cacheKey = "cachedTime";
            var cachedData = await cache.GetStringAsync(cacheKey);

            if (cachedData == null)
            {
                cachedData = DateTime.UtcNow.ToString();
                await cache.SetStringAsync(cacheKey, cachedData, new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10) // Cache for 10 minutes
                });
            }

            return Results.Json(new { message = "Distributed Cache", time = cachedData });
        });
        app.Run();
    }
}
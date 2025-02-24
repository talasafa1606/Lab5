using DDDProject.Application.BackgroundJobs;
using DDDProject.Common;
using DDDProject.Common.Localization;
using DDDProject.Infrastructure;
using DDDProject.Infrastructure.BackgroundJobs;
using DDDProject.Infrastructure.Caching;
using DDDProject.Infrastructure.Configurations;
using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace DDDProject.API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddCommonServices();
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

        
        builder.Services.ConfigureHangfire(builder.Configuration);

        builder.Services.AddSharedLocalization();

        builder.Services.AddScoped<StudentGradeRecalculationJob>();
        builder.Services.AddScoped<EnrollmentNotificationJob>();

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
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30) 
                });
            }

            return Results.Json(new { message = "Distributed Cache", time = cachedData });
        });
        
        var supportedCultures = new[] { "en-US", "de-DE", "fr-FR" };
        app.UseRequestLocalization(new RequestLocalizationOptions()
            .SetDefaultCulture(supportedCultures[0])
            .AddSupportedCultures(supportedCultures)
            .AddSupportedUICultures(supportedCultures));

        app.UseHangfireDashboard("/hangfire");

        HangfireJobScheduler.ScheduleRecurringJobs();
        app.Run();
    }
}
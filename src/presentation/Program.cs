using DDDProject.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddDbContext<ApplicationDBContext>(options =>
    options.UseNpgsql("Host=localhost;Database=UniDB;Username=talasafa16;Password=mypassword"));


// public static IHostBuilder CreateHostBuilder(string[] args) =>
//     Host.CreateDefaultBuilder(args)
//         .ConfigureServices((hostContext, services) =>
//         {
//             services.AddDbContext<ApplicationDbContext>(options =>
//                 options.UseSqlServer("Host=localhost;Database=UniDB;Username=talasafa16;Password=mypassword"));
//         });

var app = builder.Build();
app.UseHttpsRedirection();
app.Run();

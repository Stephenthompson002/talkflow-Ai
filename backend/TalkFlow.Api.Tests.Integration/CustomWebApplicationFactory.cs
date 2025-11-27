using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Linq;
using TalkFlow.Api.Data;
using System.Diagnostics.CodeAnalysis; // Required for [ExcludeFromCodeCoverage]

// 🎯 CRITICAL FIX: Wrap the entire class in the correct namespace
namespace TalkFlow.Api.Tests.Integration
{
    [ExcludeFromCodeCoverage]
    // 🎯 CRITICAL FIX: Must be explicitly public to be used by the public test constructor (CS0051)
    public class CustomWebApplicationFactory<TEntryPoint> : WebApplicationFactory<TEntryPoint> where TEntryPoint : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // 1. REMOVE THE FAILING POSTGRES HEALTH CHECK
                // This is the core logic that stops the "Resource temporarily unavailable" error.
                var healthCheckDescriptor = services
                    .Where(d => d.ServiceType == typeof(IHealthCheck) && d.ImplementationType?.Name == "NpgSqlHealthCheck")
                    .ToList();
                
                foreach (var descriptor in healthCheckDescriptor)
                {
                    services.Remove(descriptor);
                }

                // 2. ENSURE IN-MEMORY DB CONTEXT IS USED
                services.RemoveAll(typeof(DbContextOptions<AppDbContext>));
                
                services.AddDbContext<AppDbContext>(options =>
                {
                    options.UseInMemoryDatabase("TestDatabaseForIntegrationTests");
                });
            });
        }
    }
}
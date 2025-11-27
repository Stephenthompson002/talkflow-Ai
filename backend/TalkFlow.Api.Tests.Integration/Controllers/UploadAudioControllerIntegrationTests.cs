using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using TalkFlow.Api.Data;
using Xunit;
using Microsoft.EntityFrameworkCore;
using System.Linq; // Added for IAsyncLifetime

namespace TalkFlow.Api.Tests.Integration.Controllers
{
    // Fix: Use the generic CustomWebApplicationFactory
    // Fix: Ensure the class is public (required by xUnit)
    public class UploadAudioControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>, IAsyncLifetime
    {
        private readonly CustomWebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;
        private readonly IServiceScope _scope;
        private readonly AppDbContext _dbContext;

        // Fix: Constructor is public
        public UploadAudioControllerIntegrationTests(CustomWebApplicationFactory<Program> factory)
        {
            // The factory and services (DB/Health Check removal) are configured once in CustomWebApplicationFactory.cs.
            // We just store the factory and create a scope for this test class instance.
            _factory = factory;
            _client = _factory.CreateClient();

            // Create a dedicated scope to get the DbContext instance used for cleanup/setup
            _scope = _factory.Services.CreateScope();
            _dbContext = _scope.ServiceProvider.GetRequiredService<AppDbContext>();
        }

        [Fact]
        public async Task Get_HealthEndpoint_ReturnsOk()
        {
            // Act
            var response = await _client.GetAsync("/health");

            // Assert
            // This now asserts against an endpoint where the failing Postgres check has been removed.
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Get_TestDbEndpoint_ReturnsSuccess()
        {
            // Act
            var response = await _client.GetAsync("/api/test-db");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            
            var content = await response.Content.ReadAsStringAsync();
            content.Should().Contain("Database test successful");
        }

        [Fact]
        public async Task Get_RootEndpoint_ReturnsApiRunning()
        {
            // Act
            var response = await _client.GetAsync("/");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            
            var content = await response.Content.ReadAsStringAsync();
            content.Should().Contain("TalkFlow API is running");
        }

        // Implementation of IAsyncLifetime
        public Task InitializeAsync() => Task.CompletedTask;

        // Implementation of IAsyncLifetime for cleanup
        public async Task DisposeAsync()
        {
            // Cleanup test data by deleting the in-memory database instance
            await _dbContext.Database.EnsureDeletedAsync();
            
            // Dispose the scope to release the AppDbContext and other scoped services
            _scope.Dispose();
        }
    }
}
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using TalkFlow.Api.Data;
using TalkFlow.Api.Models;
using Xunit;

namespace TalkFlow.Api.Tests.Integration.Database
{
    public class AppDbContextTests : IAsyncLifetime
    {
        private readonly AppDbContext _dbContext;
        private readonly DbContextOptions<AppDbContext> _dbContextOptions;

        public AppDbContextTests()
        {
            _dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: $"TalkFlowTest_{Guid.NewGuid()}")
                .Options;

            _dbContext = new AppDbContext(_dbContextOptions);
        }

        [Fact]
        public async Task SaveConversation_ShouldPersistToDatabase()
        {
            // Arrange
            var conversation = new Conversation
            {
                Title = "Test Conversation",
                CustomerId = "test-customer-1"
            };

            // Act
            _dbContext.Conversations.Add(conversation);
            await _dbContext.SaveChangesAsync();

            // Assert
            var savedConversation = await _dbContext.Conversations.FindAsync(conversation.Id);
            savedConversation.Should().NotBeNull();
            savedConversation!.Title.Should().Be("Test Conversation");
            savedConversation.CustomerId.Should().Be("test-customer-1");
        }

        [Fact]
        public async Task SaveMessage_WithConversation_ShouldPersistToDatabase()
        {
            // Arrange
            var conversation = new Conversation { Title = "Test", CustomerId = "customer-1" };
            _dbContext.Conversations.Add(conversation);
            await _dbContext.SaveChangesAsync();

            var message = new Message
            {
                ConversationId = conversation.Id,
                Sender = "customer",
                Content = "Hello, I need help"
            };

            // Act
            _dbContext.Messages.Add(message);
            await _dbContext.SaveChangesAsync();

            // Assert
            var savedMessage = await _dbContext.Messages.FindAsync(message.Id);
            savedMessage.Should().NotBeNull();
            savedMessage!.Content.Should().Be("Hello, I need help");
            savedMessage.Sender.Should().Be("customer");
            savedMessage.ConversationId.Should().Be(conversation.Id);
        }

        public async Task InitializeAsync()
        {
            await _dbContext.Database.EnsureCreatedAsync();
        }

        public async Task DisposeAsync()
        {
            await _dbContext.Database.EnsureDeletedAsync();
            _dbContext.Dispose();
        }
    }
}

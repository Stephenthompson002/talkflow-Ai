using FluentAssertions;
using TalkFlow.Api.Models;
using Xunit;

namespace TalkFlow.Api.Tests.Unit.Models
{
    public class ConversationTests
    {
        [Fact]
        public void Conversation_WhenCreated_HasValidDefaults()
        {
            // Act
            var conversation = new Conversation();

            // Assert
            conversation.Id.Should().NotBeEmpty();
            conversation.Title.Should().BeEmpty();
            conversation.CustomerId.Should().BeEmpty();
            conversation.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        }

        [Fact]
        public void Conversation_WithProperties_SetCorrectly()
        {
            // Arrange
            var title = "Test Conversation";
            var customerId = "customer-123";

            // Act
            var conversation = new Conversation
            {
                Title = title,
                CustomerId = customerId
            };

            // Assert
            conversation.Title.Should().Be(title);
            conversation.CustomerId.Should().Be(customerId);
            conversation.Id.Should().NotBeEmpty();
        }
    }
}

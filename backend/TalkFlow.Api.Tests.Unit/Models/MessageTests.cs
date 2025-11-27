using FluentAssertions;
using TalkFlow.Api.Models;
using Xunit;

namespace TalkFlow.Api.Tests.Unit.Models
{
    public class MessageTests
    {
        [Fact]
        public void Message_WhenCreated_HasValidDefaults()
        {
            // Act
            var message = new Message();

            // Assert
            message.Id.Should().NotBeEmpty();
            message.ConversationId.Should().NotBeEmpty();
            message.Sender.Should().BeEmpty();
            message.Content.Should().BeEmpty();
            message.SentAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        }

        [Fact]
        public void Message_WithProperties_SetCorrectly()
        {
            // Arrange
            var conversationId = Guid.NewGuid();
            var sender = "customer";
            var content = "Hello, I need help";

            // Act
            var message = new Message
            {
                ConversationId = conversationId,
                Sender = sender,
                Content = content
            };

            // Assert
            message.ConversationId.Should().Be(conversationId);
            message.Sender.Should().Be(sender);
            message.Content.Should().Be(content);
            message.Id.Should().NotBeEmpty();
        }
    }
}

using FluentAssertions;
using TalkFlow.Api.Services;
using Xunit;

namespace TalkFlow.Api.Tests.Unit.Services
{
    public class GptIntentServiceTests
    {
        private readonly GptIntentService _sut;

        public GptIntentServiceTests()
        {
            _sut = new GptIntentService();
        }

        [Fact]
        public async Task HandleIntentAsync_WithValidText_ReturnsFormattedResponse()
        {
            // Arrange
            var inputText = "Hello, I need help with my account";
            var cancellationToken = CancellationToken.None;

            // Act
            var result = await _sut.HandleIntentAsync(inputText, cancellationToken);

            // Assert
            result.Should().NotBeNullOrEmpty();
            result.Should().Contain("(intent-handled)");
            result.Should().Contain("Hello, I need help with my account");
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData(null)]
        public async Task HandleIntentAsync_WithInvalidInput_ReturnsResponse(string invalidInput)
        {
            // Act
            var result = await _sut.HandleIntentAsync(invalidInput);

            // Assert
            result.Should().NotBeNullOrEmpty();
        }
    }
}

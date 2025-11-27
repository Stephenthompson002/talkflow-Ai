using FluentAssertions;
using TalkFlow.Api.Services;
using Xunit;

namespace TalkFlow.Api.Tests.Unit.Services
{
    public class WhisperServiceTests
    {
        private readonly WhisperService _sut;

        public WhisperServiceTests()
        {
            _sut = new WhisperService();
        }

        [Fact]
        public async Task TranscribeAsync_WithAudioData_ReturnsPlaceholderText()
        {
            // Arrange
            var audioData = new byte[] { 0x1, 0x2, 0x3 };
            var cancellationToken = CancellationToken.None;

            // Act
            var result = await _sut.TranscribeAsync(audioData, cancellationToken);

            // Assert
            result.Should().NotBeNullOrEmpty();
            result.Should().Contain("(transcribed text placeholder)");
        }

        [Fact]
        public async Task TranscribeAsync_WithEmptyAudio_ReturnsResponse()
        {
            // Arrange
            var emptyAudioData = Array.Empty<byte>();

            // Act
            var result = await _sut.TranscribeAsync(emptyAudioData);

            // Assert
            result.Should().NotBeNullOrEmpty();
        }
    }
}

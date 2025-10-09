namespace TalkFlow.Api.Services
{
    // Placeholder implementation - integrate with Whisper/OpenAI speech-to-text API
    public class WhisperService : IAudioService
    {
        public Task<string> TranscribeAsync(byte[] audioData, CancellationToken ct = default)
        {
            // Simulated transcription
            return Task.FromResult("(transcribed text placeholder)");
        }
    }
}

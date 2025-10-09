namespace TalkFlow.Api.Services
{
    public interface IAudioService
    {
        Task<string> TranscribeAsync(byte[] audioData, CancellationToken ct = default);
    }
}

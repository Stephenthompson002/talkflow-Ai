namespace TalkFlow.Api.Services
{
    public interface IIntentService
    {
        Task<string> HandleIntentAsync(string text, CancellationToken ct = default);
    }
}

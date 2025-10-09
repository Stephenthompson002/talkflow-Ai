namespace TalkFlow.Api.Services
{
    // Placeholder: call OpenAI/GPT to classify intent / craft response
    public class GptIntentService : IIntentService
    {
        public Task<string> HandleIntentAsync(string text, CancellationToken ct = default)
        {
            // Simulate response
            return Task.FromResult($"(intent-handled) Responding to: {text.Substring(0, Math.Min(50, text.Length))}");
        }
    }
}

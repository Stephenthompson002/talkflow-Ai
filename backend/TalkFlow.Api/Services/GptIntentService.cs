namespace TalkFlow.Api.Services
{
    // Placeholder: call OpenAI/GPT to classify intent / craft response
    public class GptIntentService : IIntentService
    {
        public Task<string> HandleIntentAsync(string text, CancellationToken ct = default)
        {
            // FIX: Add null check
            if (string.IsNullOrEmpty(text))
                return Task.FromResult("(intent-handled) Empty input");
            
            // Simulate response
            return Task.FromResult($"(intent-handled) Responding to: {text.Substring(0, Math.Min(50, text.Length))}");
        }
    }
}
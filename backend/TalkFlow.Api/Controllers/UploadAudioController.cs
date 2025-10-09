using Microsoft.AspNetCore.Mvc;
using TalkFlow.Api.Services;

namespace TalkFlow.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UploadAudioController : ControllerBase
    {
        private readonly IAudioService _audio;
        private readonly IIntentService _intent;
        public UploadAudioController(IAudioService audio, IIntentService intent) { _audio = audio; _intent = intent; }

        [HttpPost("transcribe")]
        public async Task<IActionResult> Transcribe()
        {
            using var ms = new MemoryStream();
            await Request.Body.CopyToAsync(ms);
            var text = await _audio.TranscribeAsync(ms.ToArray());
            var response = await _intent.HandleIntentAsync(text);
            return Ok(new { text, response });
        }
    }
}

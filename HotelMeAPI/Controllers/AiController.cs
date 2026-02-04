using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace HotelMeAPI.Controllers
{
    [ApiController]
    [Route("api/ai")]
    public class AiController : ControllerBase
    {
        private readonly IHttpClientFactory _httpFactory;
        private readonly IConfiguration _config;

        public AiController(IHttpClientFactory httpFactory, IConfiguration config)
        {
            _httpFactory = httpFactory;
            _config = config;
        }

        public record SmartReplyRequest(string GuestMessage, string? RoomNumber, string? GuestName);
        public record SmartReplyResponse(List<string> Replies);

        [HttpPost("smart-replies")]
        public async Task<ActionResult<SmartReplyResponse>> SmartReplies([FromBody] SmartReplyRequest req)
        {
            var apiKey = _config["OpenAI:ApiKey"];
            if (string.IsNullOrWhiteSpace(apiKey))
                return StatusCode(500, "Missing OpenAI:ApiKey configuration.");

            var http = _httpFactory.CreateClient();

            http.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", apiKey);

            var system = """
You are a hotel receptionist assistant.
Generate exactly 3 short, professional reply suggestions to the guest message.
Keep them concise (1-2 sentences), friendly, and actionable.
If you need info, ask one clarifying question.
Do not invent policies/prices; if unknown, say you will check.
Return JSON array of 3 strings only.
""";

            var user = $"""
Guest message: {req.GuestMessage}
Room (if known): {req.RoomNumber ?? "unknown"}
Guest name (if known): {req.GuestName ?? "unknown"}
""";

            var payload = new
            {
                model = "gpt-4.1-mini",
                messages = new object[]
                {
                    new { role = "system", content = system },
                    new { role = "user", content = user }
                },
                temperature = 0.4
            };

            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var resp = await http.PostAsync("https://api.openai.com/v1/chat/completions", content);
            if (!resp.IsSuccessStatusCode)
            {
                var err = await resp.Content.ReadAsStringAsync();
                return StatusCode((int)resp.StatusCode, err);
            }

            using var doc = JsonDocument.Parse(await resp.Content.ReadAsStringAsync());
            var text = doc.RootElement
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString() ?? "[]";

            List<string>? replies;
            try
            {
                replies = JsonSerializer.Deserialize<List<string>>(text);
            }
            catch
            {
                replies = new List<string>
                {
                    "Thanks for your message! I’ll check and get back to you shortly.",
                    "Could you please share your room number so I can assist?",
                    "I’m on it — I’ll confirm availability and update you soon."
                };
            }

            replies ??= new List<string>();
            replies = replies.Take(3).ToList();

            return Ok(new SmartReplyResponse(replies));
        }
    }
}

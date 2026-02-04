using System.Net.Http.Json;

public class AiService
{
    private readonly HttpClient _http;
    public AiService(HttpClient http) => _http = http;

    public record SmartReplyRequest(string GuestMessage, string? RoomNumber, string? GuestName);
    public record SmartReplyResponse(List<string> Replies);

    public async Task<List<string>> GetSmartReplies(string guestMessage, string? roomNumber = null, string? guestName = null)
    {
        var res = await _http.PostAsJsonAsync("api/ai/smart-replies",
            new SmartReplyRequest(guestMessage, roomNumber, guestName));

        if (!res.IsSuccessStatusCode) return new();

        var data = await res.Content.ReadFromJsonAsync<SmartReplyResponse>();
        return data?.Replies ?? new();
    }
}

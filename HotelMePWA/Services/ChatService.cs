using System.Net.Http.Json;

public class ChatService
{
    private readonly HttpClient _http;
    private readonly AuthService _authService;

    public ChatService(HttpClient http, AuthService authService)
    {
        _http = http;
        _authService = authService;
    }

    public async Task<bool> SendMessage(string message)
    {
        var httpClient = await _authService.GetAuthorizedHttpClient();
        var response = await httpClient.PostAsJsonAsync("api/chat/send", message);
        return response.IsSuccessStatusCode;
    }
}

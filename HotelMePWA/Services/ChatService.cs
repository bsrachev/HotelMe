using HotelMe.Shared.Models;
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

    public async Task<List<ChatMessage>> GetAllMessages()
    {
        var client = await _authService.GetAuthorizedHttpClient();
        var all = await client.GetFromJsonAsync<List<ChatMessage>>("api/chat/all");
        return all ?? new List<ChatMessage>();
    }


    public async Task<bool> SendMessage(string message)
    {
        var client = await _authService.GetAuthorizedHttpClient();
        var response = await client.PostAsJsonAsync("api/chat/send", message);
        return response.IsSuccessStatusCode;
    }
}

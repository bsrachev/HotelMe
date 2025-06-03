using HotelMe.Shared.Models;
using System.Net.Http.Json;

public class RoomService // Food and drink menu service
{
    private readonly HttpClient _http;
    private readonly AuthService _authService;

    public RoomService(HttpClient http, AuthService authService)
    {
        _http = http;
        _authService = authService;
    }

    public async Task<List<MenuItem>> GetMenu()
    {
        var httpClient = await _authService.GetAuthorizedHttpClient();
        return await httpClient.GetFromJsonAsync<List<MenuItem>>("api/menu") ?? new List<MenuItem>();
    }
}
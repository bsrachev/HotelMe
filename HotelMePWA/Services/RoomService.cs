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
        var client = await _authService.GetAuthorizedHttpClient();
        return await client.GetFromJsonAsync<List<MenuItem>>("api/menu") ?? new List<MenuItem>();
    }

    public async Task<bool> PlaceOrder(IEnumerable<int> menuItemIds)
    {
        var client = await _authService.GetAuthorizedHttpClient();
        var payload = new OrderRequest { Items = menuItemIds.ToList() };
        var response = await client.PostAsJsonAsync("api/orders", payload);
        return response.IsSuccessStatusCode;
    }
}
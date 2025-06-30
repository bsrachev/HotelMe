using System.Collections.Generic;
using System.Net.Http.Json;
using System.Threading.Tasks;
using HotelMe.Shared.Models;

public class OrderService
{
    private readonly HttpClient _http;
    private readonly AuthService _auth;

    public OrderService(HttpClient http, AuthService auth)
    {
        _http = http;
        _auth = auth;
    }

    public async Task<bool> PlaceOrder(IEnumerable<int> itemIds)
    {
        var client = await _auth.GetAuthorizedHttpClient();
        var req = new OrderRequest { Items = itemIds.ToList() };
        var res = await client.PostAsJsonAsync("api/orders", req);
        return res.IsSuccessStatusCode;
    }

    public async Task<List<Order>> GetMyOrders()
    {
        var client = await _auth.GetAuthorizedHttpClient();
        return await client.GetFromJsonAsync<List<Order>>("api/orders/user")
               ?? new List<Order>();
    }
}

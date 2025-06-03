using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using HotelMe.Shared.Models;

public class BookingService
{
    private readonly HttpClient _http;
    private readonly AuthService _authService;

    public BookingService(HttpClient http, AuthService authService)
    {
        _http = http;
        _authService = authService;
    }

    public async Task<Booking?> GetCurrentBooking()
    {
        var client = await _authService.GetAuthorizedHttpClient();
        var response = await client.GetAsync("api/bookings/user");
        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            // simply return null – caller can detect and redirect
            return null;
        }
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<Booking>();
    }

    public async Task<bool> CheckIn()
    {
        var client = await _authService.GetAuthorizedHttpClient();
        var res = await client.PostAsync("api/bookings/check-in", null);
        return res.IsSuccessStatusCode;
    }

    public async Task<bool> CheckOut()
    {
        var client = await _authService.GetAuthorizedHttpClient();
        var res = await client.PostAsync("api/bookings/check-out", null);
        return res.IsSuccessStatusCode;
    }

    public async Task<bool> CreateBooking(BookingRequest req)
    {
        var client = await _authService.GetAuthorizedHttpClient();
        var response = await client.PostAsJsonAsync("api/bookings", req);
        return response.IsSuccessStatusCode;
    }
}

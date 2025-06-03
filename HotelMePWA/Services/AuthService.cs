using HotelMe.Shared.Helpers;
using Microsoft.JSInterop;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

public class AuthService
{
    private readonly HttpClient _httpClient;
    private readonly IJSRuntime _jsRuntime;

    public event Action OnAuthStateChanged;

    public AuthService(HttpClient httpClient, IJSRuntime jsRuntime)
    {
        _httpClient = httpClient;
        _jsRuntime = jsRuntime;
    }

    public async Task<bool> Register(string email, string password)
    {
        var response = await _httpClient.PostAsJsonAsync("api/auth/register", new { Email = email, Password = password });

        return response.IsSuccessStatusCode;
    }


    public async Task<bool> Login(string email, string password)
    {
        var response = await _httpClient.PostAsJsonAsync("api/auth/login", new { Email = email, Password = password });
        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<AuthResponse>();
            if (result?.Token != null)
            {
                await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "authToken", result.Token);
                OnAuthStateChanged?.Invoke();
                return true;
            }
        }
        return false;
    }

    public async Task Logout()
    {
        await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "authToken");
        OnAuthStateChanged?.Invoke();
    }

    public async Task<bool> IsUserAuthenticated()
    {
        var token = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "authToken");
        return !string.IsNullOrEmpty(token);
    }

    public async Task<string> GetUserRole()
    {
        var token = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "authToken");
        return JwtHelper.GetUserRole(token);
    }

    public async Task<string> GetToken()
    {
        return await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "authToken");
    }

    public async Task<HttpClient> GetAuthorizedHttpClient()
    {
        var token = await GetToken();
        if (!string.IsNullOrEmpty(token))
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
        return _httpClient;
    }

}

public class AuthResponse
{
    public string Token { get; set; }
}

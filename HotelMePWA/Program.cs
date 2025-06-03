using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using HotelMePWA;
using Microsoft.EntityFrameworkCore;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7273/") });

Console.WriteLine($"Blazor is using API BaseAddress: {builder.HostEnvironment.BaseAddress}");


builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7273/") });
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<BookingService>();
builder.Services.AddScoped<RoomService>();
builder.Services.AddScoped<ChatService>();


await builder.Build().RunAsync();

//builder.Services.AddDbContext<HotelMeContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
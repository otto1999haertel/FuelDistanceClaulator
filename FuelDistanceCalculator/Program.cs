using Microsoft.EntityFrameworkCore;
using FuelDistanceCalculator.Data;
using Npgsql;
using FuelDistanceCalculator.Services;
using Microsoft.Extensions.Caching.Distributed;


var builder = WebApplication.CreateBuilder(args);




// Datenbankverbindung setzen (SQLite)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Registrieren des FuelPriceService in der DI-Container
// 🚀 Registriere FuelPriceService als Singleton, ABER mit einem Factory-Provider
builder.Services.AddSingleton<FuelPriceService>(provider =>
    new FuelPriceService(10, 2.5));

// 🚀 Registriere MarketFuelPriceService mit HttpClientFactory
builder.Services.AddHttpClient<MarketFuelPriceService>();

// 🚀 Registriere Redis für Distributed Caching
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = "redis:6379";
});

// 🚀 Registriere GeoLocationService mit HttpClient UND Redis-Cache
builder.Services.AddHttpClient<GeoLocationService>(); // HttpClient für API-Calls
builder.Services.AddScoped<GeoLocationService>(provider =>
{
    var httpClientFactory = provider.GetRequiredService<IHttpClientFactory>();
    var cache = provider.GetRequiredService<IDistributedCache>();
    return new GeoLocationService(httpClientFactory);
});




// Add services to the container.
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.UseStaticFiles();
app.MapRazorPages();
app.Run("http://0.0.0.0:8080"); //Anwendung aluscht auf allen IPs nicht nur auf localhost

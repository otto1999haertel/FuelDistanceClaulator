using Microsoft.EntityFrameworkCore;
using FuelDistanceCalculator.Data;
using Npgsql;


var builder = WebApplication.CreateBuilder(args);




// Datenbankverbindung setzen (SQLite)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Registrieren des FuelPriceService in der DI-Container
builder.Services.AddSingleton<FuelPriceService>(provider =>
    new FuelPriceService(10, 2.5));

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

using Microsoft.EntityFrameworkCore;
using FuelDistanceCalculator.Data;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

// Registrieren des FuelPriceService in der DI-Container
builder.Services.AddSingleton<FuelPriceService>(provider =>
    new FuelPriceService(10, 2.5));


// Datenbankverbindung setzen (SQLite)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

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

using Microsoft.EntityFrameworkCore;
using MNight_works;
using MNight_works.Models;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Add services for API documentation using Scalar
builder.Services.AddDbContext<AppDbContext>(options =>
    // Use Npgsql (PostgreSQL). Ensure the connection string in appsettings.json is a PostgreSQL connection string.
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Restaurant.MenuItems and MenuItem.Restaurant point at each other.
        // This tells the JSON serializer to stop once it hits something it's
        // already written, instead of looping forever and crashing like it just did.
        options.JsonSerializerOptions.ReferenceHandler =
            System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();


var app = builder.Build();

//if someone visits the boot URL,automatically serve index.html
app.UseDefaultFiles();
//allows serving plain files (html, css, js) from a folder called wwwroot 
app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    // Add Scalar UI for API documentation
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

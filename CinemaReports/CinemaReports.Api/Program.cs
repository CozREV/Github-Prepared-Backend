using CinemaReports.Api;
using CinemaReports.Api.Models;
using Dapper;
using Microsoft.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<ScreeningRepository>();

var app = builder.Build();

app.MapGet("/screenings", async (ScreeningRepository repo) =>
{
    var screenings = await repo.GetAllAsync();
    return Results.Ok(screenings);
});

app.MapGet("/screenings/{id:int}", async (int id, ScreeningRepository repo) =>
{
    var details = await repo.FindAsync(id);
    return details is null ? Results.NotFound() : Results.Ok(details);
});

app.Run();
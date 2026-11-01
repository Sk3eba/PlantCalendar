using PlantCalendar.Api.Data;
using PlantCalendar.Api.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddValidation();
builder.AddPlantCalendarDb();

var app = builder.Build();

app.MapGamesEndpoints();

app.MigrateDb();

app.Run();
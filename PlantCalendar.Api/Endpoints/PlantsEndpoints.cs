using PlantCalendar.Api.Data;
using PlantCalendar.Api.DTOs;
using PlantCalendar.Api.Model;
using Microsoft.EntityFrameworkCore;

namespace PlantCalendar.Api.Endpoints;

public static class PlantsEndpoints
{
    const string GetPlantEndpointName = "GetPlant";
    public static void MapGamesEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/plants");

        // GET /plants
        group.MapGet("/", async (PlantCalendarContext dbContext) 
            => await dbContext.Plants.Include(plant => plant.Species).Select(plant => new PlantSummaryDto (
                plant.Id,
                plant.Name,
                plant.WateringFrequencyDays,
                plant.Species!.Name
            ))
            .AsNoTracking()
            .ToListAsync());

        // GET /plants/1
        group.MapGet("/{id}", async (int id, PlantCalendarContext dbContext) => 
        {
            var plant = await dbContext.Plants.FindAsync(id);

            return plant is null ? Results.NotFound() : Results.Ok(
                new PlantDetailsDto(
                    plant.Id,
                    plant.Name,
                    plant.WateringFrequencyDays,
                    plant.SpeciesId
                )
            );
        })
        .WithName(GetPlantEndpointName);

        // POST /plants
        group.MapPost("/", async (CreatePlantDto newPlant, PlantCalendarContext dbContext) =>
        {   
            Plant plant = new()
            {
                Name = newPlant.Name,
                SpeciesId = newPlant.SpeciesId,
                WateringFrequencyDays = newPlant.WateringFrequencyDays
            };

            dbContext.Plants.Add(plant);
            await dbContext.SaveChangesAsync();

            PlantDetailsDto plantDto = new(
                plant.Id,
                plant.Name,
                plant.WateringFrequencyDays,
                plant.SpeciesId
            );

            return Results.CreatedAtRoute(GetPlantEndpointName, new { id = plantDto.Id }, plantDto);
        });

        // PUT /games/1
        group.MapPut("/{id}", async (int id, UpdatePlantDto updatedPlant, PlantCalendarContext dbContext) =>
        {
            var existingPlant = await dbContext.Plants.FindAsync(id);

            if (existingPlant is null) return Results.NotFound();

            existingPlant.Name = updatedPlant.Name;
            existingPlant.WateringFrequencyDays = updatedPlant.WateringFrequencyDays;
            existingPlant.SpeciesId = updatedPlant.SpeciesId;

            await dbContext.SaveChangesAsync();

            return Results.NoContent();
        });

        // DELETE /games/1
        group.MapDelete("/{id}", async (int id, PlantCalendarContext dbContext) =>
        {
            await dbContext.Plants.Where(plant => plant.Id == id).ExecuteDeleteAsync();

            return Results.NoContent();
        });
    }
}

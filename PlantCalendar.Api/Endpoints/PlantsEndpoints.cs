using PlantCalendar.Api.DTOs;

namespace PlantCalendar.Api.Endpoints;

public static class PlantsEndpoints
{
    const string GetPlantEndpointName = "GetPlant";
    private static readonly List<PlantDto> plants = [
        new(1, "Kalanchoe"),
        new(2, "Sukulent"),
        new(3, "Monstera")
    ];  

    public static void MapGamesEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/plants");

        // GET /plants
        group.MapGet("/", () => plants);

        // GET /plants/1
        group.MapGet("/{id}", (int id) => 
        {
            var game = plants.Find(plant => plant.Id == id);

            return game is null ? Results.NotFound() : Results.Ok(game);
        })
            .WithName(GetPlantEndpointName);

        // POST /plants
        group.MapPost("/", (CreatePlantDto newPlant) =>
        {
            int nextId = plants.Any() ? plants.Max(p => p.Id) + 1 : 1;

            PlantDto plant = new(
                nextId,
                newPlant.Name
            );

            plants.Add(plant);

            return Results.CreatedAtRoute(GetPlantEndpointName, new { id = plant.Id }, plant);
        });

        // PUT /games/1
        group.MapPut("/{id}", (int id, UpdatePlantDto updatedPlant) =>
        {
            var index = plants.FindIndex(plant => plant.Id == id);

            if (index == -1) return Results.NotFound();

            plants[index] = new PlantDto(
                id,
                updatedPlant.Name
            );

            return Results.NoContent();
        });

        // DELETE /games/1
        group.MapDelete("/{id}", (int id) =>
        {
            plants.RemoveAll(plant => plant.Id == id);

            return Results.NoContent();
        });
    }
}

using Microsoft.EntityFrameworkCore;
using PlantCalendar.Api.Data;
using PlantCalendar.Api.DTOs;

namespace PlantCalendar.Api.Endpoints;

public static class SpeciesEndpoints
{
    public static void MapSpeciesEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/species");

        //GET /species
        group.MapGet("/", async (PlantCalendarContext dbContext) => 
            await dbContext.Species
                            .Select(species => new SpeciesDto(species.Id, species.Name))
                            .AsNoTracking()
                            .ToListAsync()
            ); 
    }
}

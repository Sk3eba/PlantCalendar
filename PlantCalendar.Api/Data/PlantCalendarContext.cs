using Microsoft.EntityFrameworkCore;
using PlantCalendar.Api.Model;

namespace PlantCalendar.Api.Data;

public class PlantCalendarContext(DbContextOptions<PlantCalendarContext> options) 
    : DbContext(options)
{
    public DbSet<Plant> Plants => Set<Plant>();

    public DbSet<Species> Species => Set<Species>();
}

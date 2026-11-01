using Microsoft.EntityFrameworkCore;
using PlantCalendar.Api.Model;

namespace PlantCalendar.Api.Data;

public static class DataExtensions
{
    public static void MigrateDb(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<PlantCalendarContext>();
        dbContext.Database.Migrate();
    }

    public static void AddPlantCalendarDb(this WebApplicationBuilder builder)
    {
        var connString = "Data Source=PlantCalendar.db";
        builder.Services.AddSqlite<PlantCalendarContext>(
            connString,
            optionsAction: options => options.UseSeeding((context, _) =>
            {
                if (!context.Set<Species>().Any())
                {
                    context.Set<Species>().AddRange(
                        new Species {Name = "Monstera deliciosa"},
                        new Species {Name = "Spathiphyllum"},
                        new Species {Name = "Zamioculcas zamiifolia"},
                        new Species {Name = "Sansevieria"},
                        new Species {Name = "Epipremnum aureum"}
                    );

                    context.SaveChanges();
                }
            })
        );
    }
}

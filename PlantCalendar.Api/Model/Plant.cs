namespace PlantCalendar.Api.Model;

public class Plant
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public int WateringFrequencyDays { get; set; }
    public Species? Species { get; set; }
    public int SpeciesId { get; set; }
}

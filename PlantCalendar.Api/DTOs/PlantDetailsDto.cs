using System.ComponentModel.DataAnnotations;

namespace PlantCalendar.Api.DTOs;

public record PlantDetailsDto(
    int Id,
    [Required][StringLength(50)] string Name,
    int WateringFrequencyDays,
    int SpeciesId
);
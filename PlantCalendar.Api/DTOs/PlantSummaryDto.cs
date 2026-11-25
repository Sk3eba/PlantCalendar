using System.ComponentModel.DataAnnotations;

namespace PlantCalendar.Api.DTOs;

public record PlantSummaryDto(
    int Id,
    [Required][StringLength(50)] string Name,
    int WateringFrequencyDays,
    string Species
);
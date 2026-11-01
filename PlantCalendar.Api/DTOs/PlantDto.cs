using System.ComponentModel.DataAnnotations;

namespace PlantCalendar.Api.DTOs;

public record PlantDto(
    int Id,
    [Required][StringLength(50)] string Name
);
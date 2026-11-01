using System.ComponentModel.DataAnnotations;

namespace PlantCalendar.Api.DTOs;

public record CreatePlantDto(
    [Required][StringLength(50)] string Name
);

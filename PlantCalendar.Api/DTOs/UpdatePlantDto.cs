using System.ComponentModel.DataAnnotations;

namespace PlantCalendar.Api.DTOs;

public record UpdatePlantDto(
    [Required][StringLength(50)] string Name
);

using System.ComponentModel.DataAnnotations;

namespace SmartLockPlatform.Host.Controllers.V1.Models;

public class RegisterLockDTO
{
    [Required, StringLength(128)] public string Uuid { get; init; } = default!;
    [Required, StringLength(50)] public string Name { get; init; } = default!;
}
using System.ComponentModel.DataAnnotations;

namespace SmartLockPlatform.Host.Controllers.V1.Models;

public class UnLockDTO
{
    [Required] public string Otp { get; init; } = default!;
}
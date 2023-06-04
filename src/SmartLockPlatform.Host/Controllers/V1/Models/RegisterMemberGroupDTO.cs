using System.ComponentModel.DataAnnotations;

namespace SmartLockPlatform.Host.Controllers.V1.Models;

public class RegisterMemberGroupDTO
{
    [Required, StringLength(50)] public string Name { get; init; } = default!;
}
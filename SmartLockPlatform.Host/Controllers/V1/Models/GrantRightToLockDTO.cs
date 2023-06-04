using System.ComponentModel.DataAnnotations;

namespace SmartLockPlatform.Host.Controllers.V1.Models;

public class GrantRightToLockDTO
{
    [Required] public long LockId { get; init; }
}
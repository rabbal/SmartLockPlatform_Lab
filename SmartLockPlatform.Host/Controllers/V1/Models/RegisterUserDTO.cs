using System.ComponentModel.DataAnnotations;

namespace SmartLockPlatform.Host.Controllers.V1;

public record RegisterUserDTO(
    [Required, StringLength(50)] string FirstName,
    [Required, StringLength(50)] string LastName,
    [Required, EmailAddress] string Email,
    [Required, StringLength(128)] string Password);
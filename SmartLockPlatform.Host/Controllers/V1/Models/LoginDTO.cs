using System.ComponentModel.DataAnnotations;

namespace SmartLockPlatform.Host.Controllers.V1.Models;

public record LoginDTO([Required, EmailAddress] string Email, [Required] string Password);
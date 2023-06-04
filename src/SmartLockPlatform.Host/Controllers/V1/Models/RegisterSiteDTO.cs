using System.ComponentModel.DataAnnotations;

namespace SmartLockPlatform.Host.Controllers.V1.Models;

public record RegisterSiteDTO([Required, StringLength(50)] string Name);
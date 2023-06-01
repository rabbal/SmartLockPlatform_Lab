using Microsoft.AspNetCore.Mvc;

namespace SmartLockPlatform.Host.Controllers.V1.Models;

public record PaginatedListQueryParams
{
    [FromQuery(Name = "$filtering")] public string? Filtering { get; set; }
    [FromQuery(Name = "$sorting")] public string? Sorting { get; set; }
    [FromQuery(Name = "$skip")] public int Skip { get; set; }
    [FromQuery(Name = "$top")] public int Top { get; set; }
    [FromQuery(Name = "$include_count")] public bool IncludeCount { get; set; }
}
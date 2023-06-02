using Microsoft.AspNetCore.Mvc;
using SmartLockPlatform.Application.Base;

namespace SmartLockPlatform.Host.Controllers.V1.Models;

public record PaginatedListQueryParams : IPaginatedRequest
{
    [FromQuery(Name = "$filtering")] public string? Filtering { get; init; }
    [FromQuery(Name = "$sorting")] public string? Sorting { get; init; }
    [FromQuery(Name = "$skip")] public int Skip { get; init; }
    [FromQuery(Name = "$top")] public int Top { get; init; }
    [FromQuery(Name = "$include_count")] public bool IncludeCount { get; init; }
}
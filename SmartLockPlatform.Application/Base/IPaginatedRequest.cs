namespace SmartLockPlatform.Application.Base;

public interface IPaginatedRequest
{
    public string? Filtering { get; init; }
    public string? Sorting { get; init; }
    public int Skip { get; init; }
    public int Top { get; init; }
    public bool IncludeCount { get; init; }
}
namespace SmartLockPlatform.Application.Base;

public record PaginatedList<T>(IReadOnlyList<T> Items, long? Count);
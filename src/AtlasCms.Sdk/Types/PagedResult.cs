namespace AtlasCms.Sdk.Types;

public record PagedMetadata
{
    // Matches swagger `metadata.count`.
    public int Count { get; set; }
    public int TotalPages { get; set; }
    public int CurrentPage { get; set; }
    public int PageSize { get; set; }
    public bool HasPreviousPage { get; set; }
    public bool HasNextPage { get; set; }
}

public record PagedResult<T>
{
    // Swagger allows `data` to be null.
    public IReadOnlyList<T>? Data { get; set; }
    public required PagedMetadata Metadata { get; set; }
}

[System.Obsolete("Use KeyResult<string> (Swagger `{ result }` wrapper) instead of CreateResult.")]
public record CreateResult
{
    public required string Id { get; set; }
}

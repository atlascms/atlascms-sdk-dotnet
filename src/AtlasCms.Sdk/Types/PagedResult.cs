namespace AtlasCms.Sdk.Types;

public record PagedMetadata
{
    public int TotalCount { get; set; }
    public int TotalPages { get; set; }
    public int CurrentPage { get; set; }
    public int PageSize { get; set; }
    public bool HasPreviousPage { get; set; }
    public bool HasNextPage { get; set; }
}

public record PagedResult<T>
{
    public required IReadOnlyList<T> Data { get; set; }
    public required PagedMetadata Metadata { get; set; }
}

public record CreateResult
{
    public required string Id { get; set; }
}

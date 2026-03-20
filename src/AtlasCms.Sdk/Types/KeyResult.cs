namespace AtlasCms.Sdk.Types;

/// <summary>
/// Swagger wrapper for `{ "result": ... }` responses.
/// </summary>
public record KeyResult<T>
{
    public T? Result { get; init; }
}


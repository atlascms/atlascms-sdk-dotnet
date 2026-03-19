# Atlas CMS .NET SDK

Simple .NET SDK for Atlas CMS with full async support and ASP.NET Core integration.

## Compatibility

### Core SDK (`AtlasCms.Sdk`)
- **.NET:** 10.0, 9.0, 8.0, 7.0
- **C#:** 11.0+

### ASP.NET Core Extensions (`AtlasCms.Sdk.AspnetCore`)
- **.NET:** 10.0, 9.0, 8.0, 7.0
- **C#:** 11.0+

## Features

- REST modules for `Contents`, `Media Library`, `Users`, `Roles`, `Models`, and `Components`
- GraphQL client helper
- Centralized HTTP client with consistent error handling
- Fluent query builders for query string DSL:
  - `filter[{path}][{operator}]={value}`
- Request-level API key override
- Fully typed APIs and test suite
- ASP.NET Core DI integration

## Install

```bash
dotnet add package AtlasCms.Sdk
```

ASP.NET Core extensions (optional):

```bash
dotnet add package AtlasCms.Sdk.AspnetCore
```

## Quick Start

```csharp
using AtlasCms.Sdk;
using AtlasCms.Sdk.QueryBuilder;

var client = AtlasCmsClient.Create(new AtlasClientConfig
{
    ProjectId = "my-project-id",
    ApiKey = "YOUR_BEARER_TOKEN"
});

var query = new ContentsQueryBuilder()
    .Page(1)
    .Size(20)
    .Status(QueryStatus.Published)
    .Locale("en-US")
    .Filter("createdAt", "gte", "2026-01-01T00:00:00.000Z")
    .Sort("createdAt", SortDirection.Desc)
    .Build();

var result = await client.Contents.ListAsync("pages", query);
```

The SDK uses `https://api.atlascms.io` as the default base URL. Derived endpoints:

| Type    | URL                                              |
|---------|--------------------------------------------------|
| REST    | `https://api.atlascms.io/{projectId}`            |
| GraphQL | `https://api.atlascms.io/{projectId}/graphql`    |

## API Overview

### `AtlasCmsClient.Create(config)`

```csharp
public class AtlasClientConfig
{
    public required string ProjectId { get; init; }
    public required string ApiKey { get; init; }
    /// <summary>Defaults to "https://api.atlascms.io".</summary>
    public string? BaseUrl { get; init; }
    /// <summary>Optional custom HttpClient for testing or advanced scenarios.</summary>
    public HttpClient? HttpClient { get; init; }
}
```

### Modules

#### `client.Contents`

| Method | Description |
|---|---|
| `ListAsync(type, query?, options?)` | List contents of a given type |
| `GetByIdAsync(type, id, options?)` | Get a single content by ID |
| `GetSingleAsync(type, query?, options?)` | Get a singleton content |
| `CountAsync(type, query?, options?)` | Count contents |
| `CreateAsync(type, payload, options?)` | Create a new content |
| `UpdateAsync(type, id, payload, options?)` | Update an existing content |
| `RemoveAsync(type, id, options?)` | Delete a content |
| `ChangeStatusAsync(type, id, status, options?)` | Publish or unpublish a content |
| `CreateTranslationAsync(type, id, locale?, options?)` | Create a translation copy |
| `DuplicateAsync(type, id, locales?, options?)` | Duplicate a content |
| `UpdateSeoAsync(type, id, payload, options?)` | Update SEO metadata |

#### `client.Media`

| Method | Description |
|---|---|
| `ListAsync(query?, options?)` | List media items |
| `GetByIdAsync(id, options?)` | Get a media item by ID |
| `UploadAsync(input, options?)` | Upload a file |
| `SetTagsAsync(id, tags, options?)` | Set tags on a media item |
| `RemoveAsync(id, options?)` | Delete a media item |

#### `client.Users`

| Method | Description |
|---|---|
| `ListAsync(query?, options?)` | List users |
| `CountAsync(query?, options?)` | Count users |
| `GetByIdAsync(id, options?)` | Get a user by ID |
| `CreateAsync(payload, options?)` | Register a new user |
| `UpdateAsync(id, payload, options?)` | Update a user |
| `RemoveAsync(id, options?)` | Delete a user |
| `ChangeStatusAsync(id, isActive, options?)` | Activate or deactivate a user |
| `ChangePasswordAsync(id, password, options?)` | Change a user's password |

#### `client.Roles`

| Method | Description |
|---|---|
| `ListAsync(options?)` | List roles |
| `CreateAsync(payload, options?)` | Create a role |
| `UpdateAsync(id, payload, options?)` | Update a role |
| `RemoveAsync(id, options?)` | Delete a role |
| `GetPermissionsAsync(options?)` | Get all available permissions |

#### `client.Models`

| Method | Description |
|---|---|
| `ListAsync(query?, options?)` | List content models |
| `GetByIdAsync(id, options?)` | Get a model by ID |
| `CreateAsync(payload, options?)` | Create a model |
| `UpdateAsync(payload, options?)` | Update a model |
| `RemoveAsync(id, options?)` | Delete a model |
| `PublishAsync(id, options?)` | Publish a model |
| `UnpublishAsync(id, options?)` | Unpublish a model |

#### `client.Components`

| Method | Description |
|---|---|
| `ListAsync(query?, options?)` | List components |
| `GetByIdAsync(id, options?)` | Get a component by ID |
| `CreateAsync(payload, options?)` | Create a component |
| `UpdateAsync(payload, options?)` | Update a component |
| `RemoveAsync(id, options?)` | Delete a component |

#### `client.Graphql`

| Method | Description |
|---|---|
| `ExecuteAsync<TData>(request, options?)` | Execute a GraphQL operation |

```csharp
var response = await client.Graphql.ExecuteAsync<MyData>(
    new GraphqlRequest<Dictionary<string, object?>>
    {
        Query = "query GetPages { pages { id title } }"
    });
```

### Query Builders

- `ContentsQueryBuilder`
- `MediaQueryBuilder`
- `UsersQueryBuilder`

All builders support:

| Method | Description |
|---|---|
| `Page(number)` | Set the page number |
| `Size(number)` | Set the page size |
| `Search(string)` | Full-text search |
| `Filter(field, operator, value)` | Add a filter |
| `Sort(field, direction)` | Set sort order |
| `Build()` | Produce the query string |

Additional methods:

- `ContentsQueryBuilder`: `.Status(QueryStatus.Published)`, `.Locale("en-US")`, `.Resolve("media", "relations")`
- `MediaQueryBuilder`: `.Folder("images")`, `.SearchSubdirectory(true)`
- `UsersQueryBuilder`: `.Email("...")`, `.FirstName("...")`, `.IsActive(true)`, `.Roles([...])`

### Per-request API key override

```csharp
await client.Media.ListAsync(options: new AtlasRequestOptions
{
    ApiKey = "ANOTHER_BEARER_TOKEN"
});
```

### Error handling

```csharp
using AtlasCms.Sdk.Http;

try
{
    var content = await client.Contents.GetByIdAsync("pages", "id");
}
catch (AtlasHttpException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
{
    // Content not found — ex.StatusCode, ex.Message, ex.RequestId
}
catch (AtlasTimeoutException)
{
    // Request timed out
}
catch (AtlasNetworkException)
{
    // Network error
}
```

### Custom base URL

Useful for local development or self-hosted instances:

```csharp
var client = AtlasCmsClient.Create(new AtlasClientConfig
{
    ProjectId = "my-project-id",
    ApiKey = "YOUR_BEARER_TOKEN",
    BaseUrl = "https://my-custom-instance.example.com"
});
```

### Custom HttpClient

Useful for testing or advanced scenarios (e.g. delegating handlers, mocks):

```csharp
var client = AtlasCmsClient.Create(new AtlasClientConfig
{
    ProjectId = "my-project-id",
    ApiKey = "YOUR_BEARER_TOKEN",
    HttpClient = myCustomHttpClient
});
```

## ASP.NET Core Integration

### From configuration

Register in `Program.cs`:

```csharp
builder.Services.AddAtlasCms(builder.Configuration);
```

In `appsettings.json`:

```json
{
  "AtlasCms": {
    "ProjectId": "my-project-id",
    "ApiKey": "YOUR_BEARER_TOKEN",
    "BaseUrl": "https://api.atlascms.io"
  }
}
```

`BaseUrl` is optional and defaults to `https://api.atlascms.io`.

### Using the fluent builder

```csharp
builder.Services.AddAtlasCms(options => options
    .WithProjectId("my-project-id")
    .WithApiKey("YOUR_BEARER_TOKEN")
);
```

### Direct configuration

```csharp
builder.Services.AddAtlasCms(new AtlasClientConfig
{
    ProjectId = "my-project-id",
    ApiKey = "YOUR_BEARER_TOKEN"
});
```

### Injection

```csharp
public class PageController(AtlasCmsClient client)
{
    public async Task<IActionResult> GetPage(string id)
    {
        var page = await client.Contents.GetByIdAsync("pages", id);
        return Ok(page);
    }
}
```

## Working with Dynamic Attributes

Content attributes are represented as `JsonObject` to support dynamic schemas:

```csharp
// Read attributes
var title = content.Attributes?["title"]?.GetValue<string>();
var count = content.Attributes?["count"]?.GetValue<int>();

// Create with attributes
var created = await client.Contents.CreateAsync("pages", new CreateContentInput
{
    Locale = "en",
    Attributes = new JsonObject
    {
        ["title"] = "Page Title",
        ["featured"] = true
    }
});
```

## Development

```bash
dotnet build
dotnet test
```

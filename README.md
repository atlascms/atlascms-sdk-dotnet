# Atlas CMS .NET SDK

A comprehensive .NET SDK for integrating with Atlas CMS. Build content-driven applications with easy-to-use APIs for managing content, media, users, and roles.

## Compatibility

### Core SDK (AtlasCms.Sdk)
- **.NET:** 10.0, 9.0, 8.0, 7.0
- **C#:** 11.0+

### ASP.NET Core Extensions (AtlasCms.Sdk.AspnetCore)
- **.NET:** 10.0, 9.0, 8.0, 7.0
- **C#:** 11.0+

## Features

- **Content Management:** Query and manipulate content with flexible filtering, sorting, and pagination
- **Media Library:** Upload, manage, and organize media assets
- **User Management:** Manage users, roles, and permissions
- **GraphQL Support:** Execute GraphQL queries directly
- **Dynamic Attributes:** Work with untyped attributes using `JsonObject`
- **Async/Await:** Full async support with cancellation tokens

## Installation

### Install the SDK

```bash
dotnet add package AtlasCms.Sdk
```

### Install ASP.NET Core Extensions (Optional)

```bash
dotnet add package AtlasCms.Sdk.AspnetCore
```

## Quick Start

### Using the Core SDK

```csharp
using AtlasCms.Sdk;

var config = new AtlasClientConfig
{
    Project = "my-project",
    RestBaseUrl = "https://api.atlascms.com",
    GraphqlBaseUrl = "https://api.atlascms.com/graphql",
    ApiKey = "your-api-key"
};

var client = AtlasCmsClient.Create(config);

// List content
var contents = await client.Contents.ListAsync("pages");

// Get content by ID
var page = await client.Contents.GetByIdAsync("pages", "page-id");

// Access dynamic attributes
var title = page.Attributes?["title"]?.GetValue<string>();
```

### Using with ASP.NET Core

Register the SDK in `Program.cs`:

**From Configuration:**

```csharp
builder.Services.AddAtlasCms(builder.Configuration);
```

In `appsettings.json`:

```json
{
  "AtlasCms": {
    "Project": "my-project",
    "RestBaseUrl": "https://api.atlascms.com",
    "GraphqlBaseUrl": "https://api.atlascms.com/graphql",
    "ApiKey": "your-api-key"
  }
}
```

**Using Configuration Builder:**

```csharp
builder.Services.AddAtlasCms(options => options
    .WithProject("my-project")
    .WithRestBaseUrl("https://api.atlascms.com")
    .WithGraphqlBaseUrl("https://api.atlascms.com/graphql")
    .WithApiKey("your-api-key")
);
```

**Direct Configuration:**

```csharp
builder.Services.AddAtlasCms(new AtlasClientConfig
{
    Project = "my-project",
    RestBaseUrl = "https://api.atlascms.com",
    GraphqlBaseUrl = "https://api.atlascms.com/graphql",
    ApiKey = "your-api-key"
});
```

Inject and use in your controllers or services:

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

## API Overview

### Contents API

```csharp
// List content with query
var query = new QueryBuilder.ContentsQueryBuilder()
    .Status(QueryStatus.Published)
    .Page(1)
    .Size(10)
    .Build();
var result = await client.Contents.ListAsync("pages", query);

// Get content by ID
var content = await client.Contents.GetByIdAsync("pages", "id");

// Get singleton content
var settings = await client.Contents.GetSingleAsync("settings");

// Create content
var created = await client.Contents.CreateAsync("pages", new CreateContentInput
{
    Locale = "en",
    Attributes = new JsonObject { ["title"] = "New Page" }
});

// Update content
await client.Contents.UpdateAsync("pages", "id", new UpdateContentInput
{
    Attributes = new JsonObject { ["title"] = "Updated" }
});

// Change content status
await client.Contents.ChangeStatusAsync("pages", "id", ContentStatus.Published);

// List with attribute filtering
var query = new QueryBuilder.ContentsQueryBuilder()
    .Status(QueryStatus.Published)
    .Filter("attributes.featured", "eq", "true")
    .Filter("attributes.category", "eq", "news")
    .Sort("attributes.publishedDate", SortDirection.Desc)
    .Page(1)
    .Size(20)
    .Build();
var featuredNews = await client.Contents.ListAsync("articles", query);

// Access filtered content attributes
foreach (var article in featuredNews.Data)
{
    var title = article.Attributes?["title"]?.GetValue<string>();
    var category = article.Attributes?["category"]?.GetValue<string>();
    var featured = article.Attributes?["featured"]?.GetValue<bool>();

    Console.WriteLine($"{title} ({category}) - Featured: {featured}");
}
```

### Media API

```csharp
// List media
var media = await client.Media.ListAsync();

// Upload media
using var stream = File.OpenRead("photo.jpg");
var uploaded = await client.Media.UploadAsync(new MediaUploadInput
{
    File = stream,
    FileName = "photo.jpg",
    ContentType = "image/jpeg",
    Folder = "images"
});

// Set media tags
await client.Media.SetTagsAsync("media-id", ["hero", "featured"]);
```

### Users API

```csharp
// List users
var users = await client.Users.ListAsync();

// Get user by ID
var user = await client.Users.GetByIdAsync("user-id");

// Create user
var created = await client.Users.CreateAsync(new CreateUserInput
{
    Email = "user@example.com",
    FirstName = "John",
    Password = "secret",
    Roles = ["editor"]
});

// Update user
await client.Users.UpdateAsync("user-id", new UpdateUserInput
{
    FirstName = "Jane",
    Notes = "VIP User"
});

// Change user status
await client.Users.ChangeStatusAsync("user-id", isActive: true);
```

### Roles API

```csharp
// List roles
var roles = await client.Roles.ListAsync();

// Get role by ID
var role = await client.Roles.GetByIdAsync("role-id");
```

### GraphQL API

```csharp
var query = @"
    query GetPages($locale: String!) {
        pages(locale: $locale) {
            id
            title
        }
    }
";

var response = await client.Graphql.ExecuteAsync<dynamic>(
    new GraphqlRequest<Dictionary<string, object?>>
    {
        Query = query,
        Variables = new() { ["locale"] = "en" }
    }
);
```

## Query Building

Use the fluent query builders for filtering and sorting:

```csharp
var query = new QueryBuilder.ContentsQueryBuilder()
    .Status(QueryStatus.Published)
    .Page(1)
    .Size(20)
    .Sort("createdAt", SortDirection.Desc)
    .Filter("type", "eq", "article")
    .Build();
```

### Filtering on Attributes

Filter content by dynamic attribute fields using the `Filter` method with attribute paths:

```csharp
// Filter by a single attribute value
var query = new QueryBuilder.ContentsQueryBuilder()
    .Filter("attributes.category", "eq", "featured")
    .Build();

var featured = await client.Contents.ListAsync("pages", query);

// Filter by multiple attribute conditions
var query = new QueryBuilder.ContentsQueryBuilder()
    .Filter("attributes.status", "eq", "published")
    .Filter("attributes.author", "eq", "John Doe")
    .Page(1)
    .Size(10)
    .Build();

var results = await client.Contents.ListAsync("articles", query);

// Filter with operators
var query = new QueryBuilder.ContentsQueryBuilder()
    .Filter("attributes.views", "gte", "100")  // Greater than or equal
    .Filter("attributes.rating", "lte", "5")   // Less than or equal
    .Sort("attributes.views", SortDirection.Desc)
    .Build();

var popular = await client.Contents.ListAsync("posts", query);

// Filter by nested attributes (dot notation)
var query = new QueryBuilder.ContentsQueryBuilder()
    .Filter("attributes.metadata.tags", "contains", "tutorial")
    .Build();

var tutorials = await client.Contents.ListAsync("pages", query);
```

**Common Filter Operators:**
- `eq` - Equals
- `neq` - Not equals
- `gt` - Greater than
- `gte` - Greater than or equal
- `lt` - Less than
- `lte` - Less than or equal
- `contains` - Contains (for strings and arrays)
- `in` - In array
- `exists` - Field exists

## Error Handling

The SDK throws specific exception types:

```csharp
try
{
    var content = await client.Contents.GetByIdAsync("pages", "id");
}
catch (AtlasHttpException ex) when (ex.StatusCode == 404)
{
    // Content not found
}
catch (AtlasTimeoutException ex)
{
    // Request timed out
}
catch (AtlasNetworkException ex)
{
    // Network error
}
```

## Working with Dynamic Attributes

Attributes are represented as `JsonObject` to support dynamic schemas:

```csharp
// Read attributes
var attributes = content.Attributes;
var title = attributes?["title"]?.GetValue<string>();
var count = attributes?["count"]?.GetValue<int>();

// Create with attributes
var input = new CreateContentInput
{
    Attributes = new JsonObject
    {
        ["title"] = "Page Title",
        ["description"] = "Page Description",
        ["featured"] = true
    }
};
```

## License

This SDK is provided under the MIT License.
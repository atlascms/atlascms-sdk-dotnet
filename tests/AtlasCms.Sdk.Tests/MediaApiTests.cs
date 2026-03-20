using AtlasCms.Sdk.Modules;
using AtlasCms.Sdk.Tests.Helpers;
using FluentAssertions;
using Xunit;

namespace AtlasCms.Sdk.Tests;

public class MediaApiTests
{
    private static readonly string PagedJson = """{"data":[],"metadata":{"count":0,"totalPages":0,"currentPage":1,"pageSize":10,"hasPreviousPage":false,"hasNextPage":false}}""";
    private static readonly string MediaJson = """{"id":"m1","code":"photo","folder":"/","type":"image","createdAt":"2024-01-01T00:00:00Z","createdBy":"u1","modifiedAt":"2024-01-01T00:00:00Z","modifiedBy":"u1","originalFileName":"photo.jpg","name":"photo","format":"jpg","hash":"h","mimeType":"image/jpeg","size":1024,"url":"/assets/photo.jpg","provider":"local"}""";

    [Fact]
    public async Task List_CallsCorrectUrl()
    {
        var handler = new MockHttpHandler(PagedJson);
        var client = MockHttpHandler.BuildClient(handler);

        await client.Media.ListAsync();

        handler.LastRequest!.RequestUri!.AbsolutePath.Should().Be("/my-project/media-library/media");
        handler.LastRequest.Method.Should().Be(HttpMethod.Get);
    }

    [Fact]
    public async Task List_AppendsQueryString()
    {
        var handler = new MockHttpHandler(PagedJson);
        var client = MockHttpHandler.BuildClient(handler);
        var query = new QueryBuilder.MediaQueryBuilder().Page(1).Size(25).Build();

        await client.Media.ListAsync(query);

        handler.LastRequest!.RequestUri!.Query.Should().Contain("page=1");
        handler.LastRequest.RequestUri!.Query.Should().Contain("size=25");
    }

    [Fact]
    public async Task GetById_CallsCorrectUrl()
    {
        var handler = new MockHttpHandler(MediaJson);
        var client = MockHttpHandler.BuildClient(handler);

        await client.Media.GetByIdAsync("m1");

        handler.LastRequest!.RequestUri!.AbsolutePath.Should().Be("/my-project/media-library/media/m1");
    }

    [Fact]
    public async Task Remove_SendsDelete()
    {
        var handler = new MockHttpHandler("", 200);
        var client = MockHttpHandler.BuildClient(handler);

        await client.Media.RemoveAsync("m1");

        handler.LastRequest!.Method.Should().Be(HttpMethod.Delete);
        handler.LastRequest.RequestUri!.AbsolutePath.Should().Be("/my-project/media-library/media/m1");
    }

    [Fact]
    public async Task SetTags_PostsTagsArray()
    {
        var handler = new MockHttpHandler("", 200);
        var client = MockHttpHandler.BuildClient(handler);

        await client.Media.SetTagsAsync("m1", ["hero", "featured"]);

        handler.LastRequest!.Method.Should().Be(HttpMethod.Post);
        handler.LastRequest.RequestUri!.AbsolutePath.Should().Be("/my-project/media-library/media/m1/tags");
        handler.LastRequestBody.Should().Contain("\"tags\"");
        handler.LastRequestBody.Should().Contain("hero");
    }

    [Fact]
    public async Task Upload_SendsMultipartToCorrectUrl()
    {
        var handler = new MockHttpHandler(MediaJson);
        var client = MockHttpHandler.BuildClient(handler);

        using var stream = new MemoryStream("binary"u8.ToArray());
        await client.Media.UploadAsync(new MediaUploadInput
        {
            File = stream,
            FileName = "photo.jpg",
            ContentType = "image/jpeg",
            Folder = "images"
        });

        handler.LastRequest!.Method.Should().Be(HttpMethod.Post);
        handler.LastRequest.RequestUri!.AbsolutePath.Should().Be("/my-project/media-library/media/upload");
        handler.LastRequest.Content.Should().BeOfType<MultipartFormDataContent>();
        handler.LastRequest.Content!.Headers.ContentType!.MediaType.Should().Be("multipart/form-data");
    }

    [Fact]
    public async Task Upload_WithId_IncludesIdField()
    {
        var handler = new MockHttpHandler(MediaJson);
        var client = MockHttpHandler.BuildClient(handler);

        using var stream = new MemoryStream("binary"u8.ToArray());
        await client.Media.UploadAsync(new MediaUploadInput
        {
            File = stream,
            Id = "existing-id",
            FileName = "photo.jpg"
        });

        // Verify the id field is present in the captured request body
        handler.LastRequest!.Content.Should().BeOfType<MultipartFormDataContent>();
        handler.LastRequestBody.Should().Contain("existing-id");
    }

    [Fact]
    public async Task Upload_DoesNotSetJsonContentType()
    {
        var handler = new MockHttpHandler(MediaJson);
        var client = MockHttpHandler.BuildClient(handler);

        using var stream = new MemoryStream("binary"u8.ToArray());
        await client.Media.UploadAsync(new MediaUploadInput { File = stream });

        handler.LastRequest!.Content!.Headers.ContentType!.MediaType.Should().NotBe("application/json");
    }
}

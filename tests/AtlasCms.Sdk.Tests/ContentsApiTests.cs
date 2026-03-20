using System.Text.Json;
using System.Text.Json.Nodes;
using AtlasCms.Sdk.Modules;
using AtlasCms.Sdk.Tests.Helpers;
using AtlasCms.Sdk.Types;
using FluentAssertions;
using Xunit;

namespace AtlasCms.Sdk.Tests;

public class ContentsApiTests
{
    private static readonly string PagedJson = """
        {"data":[],"metadata":{"count":0,"totalPages":0,"currentPage":1,"pageSize":10,"hasPreviousPage":false,"hasNextPage":false}}
        """;

    [Fact]
    public async Task List_CallsCorrectUrl()
    {
        var handler = new MockHttpHandler(PagedJson);
        var client = MockHttpHandler.BuildClient(handler);

        await client.Contents.ListAsync("pages");

        handler.LastRequest!.RequestUri!.AbsolutePath.Should().Be("/my-project/contents/pages");
        handler.LastRequest.Method.Should().Be(HttpMethod.Get);
    }

    [Fact]
    public async Task List_AppendsQueryString()
    {
        var handler = new MockHttpHandler(PagedJson);
        var client = MockHttpHandler.BuildClient(handler);
        var query = new QueryBuilder.ContentsQueryBuilder().Status(QueryBuilder.QueryStatus.Published).Page(2).Build();

        await client.Contents.ListAsync("pages", query);

        var qs = handler.LastRequest!.RequestUri!.Query;
        qs.Should().Contain("status=published");
        qs.Should().Contain("page=2");
    }

    [Fact]
    public async Task GetById_CallsCorrectUrl()
    {
        var handler = new MockHttpHandler("""{"id":"abc","modelKey":"pages","locale":"en","createdAt":"2024-01-01T00:00:00Z","createdBy":"u1","modifiedAt":"2024-01-01T00:00:00Z","modifiedBy":"u1","hash":"h","status":"published"}""");
        var client = MockHttpHandler.BuildClient(handler);

        await client.Contents.GetByIdAsync("pages", "abc");

        handler.LastRequest!.RequestUri!.AbsolutePath.Should().Be("/my-project/contents/pages/abc");
    }

    [Fact]
    public async Task GetSingle_HitsSingleSuffix()
    {
        var handler = new MockHttpHandler("""{"id":"abc","modelKey":"settings","locale":"en","createdAt":"2024-01-01T00:00:00Z","createdBy":"u1","modifiedAt":"2024-01-01T00:00:00Z","modifiedBy":"u1","hash":"h","status":"published"}""");
        var client = MockHttpHandler.BuildClient(handler);

        await client.Contents.GetSingleAsync("settings");

        handler.LastRequest!.RequestUri!.AbsolutePath.Should().Be("/my-project/contents/settings/single");
    }

    [Fact]
    public async Task Count_ReturnsValue()
    {
        var handler = new MockHttpHandler("""{"result":42}""");
        var client = MockHttpHandler.BuildClient(handler);

        var count = await client.Contents.CountAsync("pages");

        count.Should().Be(42);
        handler.LastRequest!.RequestUri!.AbsolutePath.Should().Be("/my-project/contents/pages/count");
    }

    [Fact]
    public async Task Create_PostsAndReturnsId()
    {
        var handler = new MockHttpHandler("""{"result":"new-id"}""");
        var client = MockHttpHandler.BuildClient(handler);

        var result = await client.Contents.CreateAsync("pages",
            new CreateContentInput { Attributes = new JsonObject { ["title"] = "Hello" } });

        result.Result.Should().Be("new-id");
        handler.LastRequest!.Method.Should().Be(HttpMethod.Post);
        handler.LastRequest.RequestUri!.AbsolutePath.Should().Be("/my-project/contents/pages");
    }

    [Fact]
    public async Task Update_SendsPut()
    {
        var handler = new MockHttpHandler("", 200);
        var client = MockHttpHandler.BuildClient(handler);

        await client.Contents.UpdateAsync("pages", "abc",
            new UpdateContentInput { Attributes = new JsonObject { ["title"] = "Updated" } });

        handler.LastRequest!.Method.Should().Be(HttpMethod.Put);
        handler.LastRequest.RequestUri!.AbsolutePath.Should().Be("/my-project/contents/pages/abc");
    }

    [Fact]
    public async Task Remove_SendsDelete()
    {
        var handler = new MockHttpHandler("", 200);
        var client = MockHttpHandler.BuildClient(handler);

        await client.Contents.RemoveAsync("pages", "abc");

        handler.LastRequest!.Method.Should().Be(HttpMethod.Delete);
        handler.LastRequest.RequestUri!.AbsolutePath.Should().Be("/my-project/contents/pages/abc");
    }

    [Fact]
    public async Task ChangeStatus_PostsToStatusEndpoint()
    {
        var handler = new MockHttpHandler("", 200);
        var client = MockHttpHandler.BuildClient(handler);

        await client.Contents.ChangeStatusAsync("pages", "abc", ContentStatus.Published);

        handler.LastRequest!.Method.Should().Be(HttpMethod.Post);
        handler.LastRequest.RequestUri!.AbsolutePath.Should().Be("/my-project/contents/pages/abc/status");
        handler.LastRequestBody.Should().Contain("\"status\":\"published\"");
    }

    [Fact]
    public async Task ChangeStatus_Unpublished_SerializesCorrectly()
    {
        var handler = new MockHttpHandler("", 200);
        var client = MockHttpHandler.BuildClient(handler);

        await client.Contents.ChangeStatusAsync("pages", "abc", ContentStatus.Unpublished);

        handler.LastRequestBody.Should().Contain("\"status\":\"unpublished\"");
    }

    [Fact]
    public async Task CreateTranslation_PostsToCorrectEndpoint()
    {
        var handler = new MockHttpHandler("""{"result":"translated-id"}""");
        var client = MockHttpHandler.BuildClient(handler);

        var result = await client.Contents.CreateTranslationAsync("pages", "abc", "it-IT");

        result.Result.Should().Be("translated-id");
        handler.LastRequest!.RequestUri!.AbsolutePath.Should().Be("/my-project/contents/pages/abc/create-translation");
        handler.LastRequestBody.Should().Contain("\"locale\":\"it-IT\"");
    }

    [Fact]
    public async Task Duplicate_PostsToCorrectEndpoint()
    {
        var handler = new MockHttpHandler("""{"result":"dup-id"}""");
        var client = MockHttpHandler.BuildClient(handler);

        var result = await client.Contents.DuplicateAsync("pages", "abc", locales: true);

        result.Result.Should().Be("dup-id");
        handler.LastRequest!.RequestUri!.AbsolutePath.Should().Be("/my-project/contents/pages/abc/duplicate");
        handler.LastRequestBody.Should().Contain("\"locales\":true");
    }

    [Fact]
    public async Task List_DeserializesJsonObjectAttributes()
    {
        var handler = new MockHttpHandler("""
            {"data":[{"id":"c1","modelKey":"pages","locale":"en","createdAt":"2024-01-01T00:00:00Z","createdBy":"u1","modifiedAt":"2024-01-01T00:00:00Z","modifiedBy":"u1","hash":"h","status":"published","attributes":{"title":"Hello","count":5}}],"metadata":{"count":1,"totalPages":1,"currentPage":1,"pageSize":10,"hasPreviousPage":false,"hasNextPage":false}}
            """);
        var client = MockHttpHandler.BuildClient(handler);

        var result = await client.Contents.ListAsync("pages");

        result.Data.Should().HaveCount(1);
        result.Data[0].Attributes?["title"]?.GetValue<string>().Should().Be("Hello");
    }
}

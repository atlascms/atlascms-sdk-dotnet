using System.Text.Json;
using System.Text.Json.Nodes;
using AtlasCms.Sdk.Modules;
using AtlasCms.Sdk.Tests.Helpers;
using FluentAssertions;
using Xunit;

namespace AtlasCms.Sdk.Tests;

public class UsersApiTests
{
    private static readonly string PagedJson = """{"data":[],"metadata":{"count":0,"totalPages":0,"currentPage":1,"pageSize":10,"hasPreviousPage":false,"hasNextPage":false}}""";

    [Fact]
    public async Task List_HitsUsersEndpoint_NotMemberships()
    {
        var handler = new MockHttpHandler(PagedJson);
        var client = MockHttpHandler.BuildClient(handler);

        await client.Users.ListAsync();

        handler.LastRequest!.RequestUri!.AbsolutePath.Should().Be("/my-project/users");
        handler.LastRequest.RequestUri!.AbsolutePath.Should().NotContain("memberships");
    }

    [Fact]
    public async Task List_AppendsQueryFilters()
    {
        var handler = new MockHttpHandler(PagedJson);
        var client = MockHttpHandler.BuildClient(handler);
        var query = new QueryBuilder.UsersQueryBuilder().Email("john@acme.com").IsActive(true).Page(2).Build();

        await client.Users.ListAsync(query);

        var qs = handler.LastRequest!.RequestUri!.Query;
        qs.Should().Contain("filter%5Bemail%5D%5Beq%5D=john%40acme.com");
        qs.Should().Contain("filter%5BisActive%5D%5Beq%5D=true");
        qs.Should().Contain("page=2");
    }

    [Fact]
    public async Task List_ReturnsPagedResult()
    {
        var handler = new MockHttpHandler(PagedJson);
        var client = MockHttpHandler.BuildClient(handler);

        var result = await client.Users.ListAsync();

        result.Should().NotBeNull();
        result.Data.Should().BeEmpty();
        result.Metadata.Count.Should().Be(0);
    }

    [Fact]
    public async Task Count_ReturnsNumber()
    {
        var handler = new MockHttpHandler("""{"result":7}""");
        var client = MockHttpHandler.BuildClient(handler);

        var count = await client.Users.CountAsync();

        count.Should().Be(7);
        handler.LastRequest!.RequestUri!.AbsolutePath.Should().Be("/my-project/users/count");
    }

    [Fact]
    public async Task GetById_CallsCorrectUrl()
    {
        var handler = new MockHttpHandler("""{"id":"u1","isActive":true,"createdAt":"2024-01-01T00:00:00Z","modifiedAt":"2024-01-01T00:00:00Z"}""");
        var client = MockHttpHandler.BuildClient(handler);

        await client.Users.GetByIdAsync("u1");

        handler.LastRequest!.RequestUri!.AbsolutePath.Should().Be("/my-project/users/u1");
    }

    [Fact]
    public async Task Create_PostsToRegisterAndReturnsId()
    {
        var handler = new MockHttpHandler("""{"result":"new-user-id"}""");
        var client = MockHttpHandler.BuildClient(handler);

        var result = await client.Users.CreateAsync(new CreateUserInput
        {
            Email = "jane@acme.com",
            FirstName = "Jane",
            Password = "secret",
            Roles = ["editor"]
        });

        result.Result.Should().Be("new-user-id");
        handler.LastRequest!.Method.Should().Be(HttpMethod.Post);
        handler.LastRequest.RequestUri!.AbsolutePath.Should().Be("/my-project/users/register");
        handler.LastRequestBody.Should().Contain("\"email\":\"jane@acme.com\"");
        handler.LastRequestBody.Should().Contain("editor");
    }

    [Fact]
    public async Task Update_SendsPutWithBody()
    {
        var handler = new MockHttpHandler("", 200);
        var client = MockHttpHandler.BuildClient(handler);

        await client.Users.UpdateAsync("u1", new UpdateUserInput
        {
            FirstName = "Jane",
            Notes = "VIP"
        });

        handler.LastRequest!.Method.Should().Be(HttpMethod.Put);
        handler.LastRequest.RequestUri!.AbsolutePath.Should().Be("/my-project/users/u1");
        handler.LastRequestBody.Should().Contain("\"firstName\":\"Jane\"");
        handler.LastRequestBody.Should().Contain("\"notes\":\"VIP\"");
    }

    [Fact]
    public async Task Remove_SendsDelete()
    {
        var handler = new MockHttpHandler("", 200);
        var client = MockHttpHandler.BuildClient(handler);

        await client.Users.RemoveAsync("u1");

        handler.LastRequest!.Method.Should().Be(HttpMethod.Delete);
        handler.LastRequest.RequestUri!.AbsolutePath.Should().Be("/my-project/users/u1");
    }

    [Fact]
    public async Task ChangeStatus_PostsIsActiveFlag()
    {
        var handler = new MockHttpHandler("", 200);
        var client = MockHttpHandler.BuildClient(handler);

        await client.Users.ChangeStatusAsync("u1", false);

        handler.LastRequest!.RequestUri!.AbsolutePath.Should().Be("/my-project/users/u1/status");
        handler.LastRequestBody.Should().Contain("\"isActive\":false");
    }

    [Fact]
    public async Task ChangePassword_PostsPassword()
    {
        var handler = new MockHttpHandler("", 200);
        var client = MockHttpHandler.BuildClient(handler);

        await client.Users.ChangePasswordAsync("u1", "newSecret123");

        handler.LastRequest!.RequestUri!.AbsolutePath.Should().Be("/my-project/users/u1/change-password");
        handler.LastRequestBody.Should().Contain("\"password\":\"newSecret123\"");
    }

    [Fact]
    public async Task List_DeserializesJsonObjectAttributes()
    {
        var handler = new MockHttpHandler("""
            {"data":[{"id":"u1","isActive":true,"createdAt":"2024-01-01T00:00:00Z","modifiedAt":"2024-01-01T00:00:00Z","attributes":{"department":"engineering","level":3}}],"metadata":{"count":1,"totalPages":1,"currentPage":1,"pageSize":10,"hasPreviousPage":false,"hasNextPage":false}}
            """);
        var client = MockHttpHandler.BuildClient(handler);

        var result = await client.Users.ListAsync();

        result.Data.Should().HaveCount(1);
        result.Data[0].Attributes?["department"]?.GetValue<string>().Should().Be("engineering");
        result.Data[0].Attributes?["level"]?.GetValue<int>().Should().Be(3);
    }
}

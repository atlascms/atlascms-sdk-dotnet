using AtlasCms.Sdk.Modules;
using AtlasCms.Sdk.Tests.Helpers;
using FluentAssertions;
using Xunit;

namespace AtlasCms.Sdk.Tests;

public class RolesApiTests
{
    [Fact]
    public async Task List_GetsUserRoles()
    {
        var handler = new MockHttpHandler("""[{"id":"r1","name":"editor","system":false,"permissions":["content.read"]}]""");
        var client = MockHttpHandler.BuildClient(handler);

        var result = await client.Roles.ListAsync();

        result.Should().HaveCount(1);
        result[0].Name.Should().Be("editor");
        handler.LastRequest!.RequestUri!.AbsolutePath.Should().Be("/my-project/users/roles");
        handler.LastRequest.Method.Should().Be(HttpMethod.Get);
    }

    [Fact]
    public async Task Create_PostsAndReturnsId()
    {
        var handler = new MockHttpHandler("""{"value":"role-id"}""");
        var client = MockHttpHandler.BuildClient(handler);

        var result = await client.Roles.CreateAsync(new CreateRoleInput
        {
            Name = "editor",
            Permissions = ["content.read", "content.write"]
        });

        result.Id.Should().Be("role-id");
        handler.LastRequest!.Method.Should().Be(HttpMethod.Post);
        handler.LastRequest.RequestUri!.AbsolutePath.Should().Be("/my-project/users/roles");
        handler.LastRequestBody.Should().Contain("\"name\":\"editor\"");
        handler.LastRequestBody.Should().Contain("content.read");
    }

    [Fact]
    public async Task Update_SendsPutWithIdInPath()
    {
        var handler = new MockHttpHandler("", 200);
        var client = MockHttpHandler.BuildClient(handler);

        await client.Roles.UpdateAsync("role-id", new UpdateRoleInput
        {
            Name = "senior-editor",
            Permissions = ["content.read"]
        });

        handler.LastRequest!.Method.Should().Be(HttpMethod.Put);
        handler.LastRequest.RequestUri!.AbsolutePath.Should().Be("/my-project/users/roles/role-id");
        handler.LastRequestBody.Should().Contain("\"name\":\"senior-editor\"");
    }

    [Fact]
    public async Task Remove_SendsDelete()
    {
        var handler = new MockHttpHandler("", 200);
        var client = MockHttpHandler.BuildClient(handler);

        await client.Roles.RemoveAsync("role-id");

        handler.LastRequest!.Method.Should().Be(HttpMethod.Delete);
        handler.LastRequest.RequestUri!.AbsolutePath.Should().Be("/my-project/users/roles/role-id");
    }

    [Fact]
    public async Task GetPermissions_GetsPermissionsEndpoint()
    {
        var handler = new MockHttpHandler("""[{"group":"content","type":"cms","key":"content.read","sections":[]}]""");
        var client = MockHttpHandler.BuildClient(handler);

        var result = await client.Roles.GetPermissionsAsync();

        result.Should().HaveCount(1);
        result[0].Group.Should().Be("content");
        handler.LastRequest!.RequestUri!.AbsolutePath.Should().Be("/my-project/users/roles/permissions");
    }
}

namespace AtlasCms.Sdk.QueryBuilder;

public sealed class UsersQueryBuilder : BaseQueryBuilder<UsersQueryBuilder>
{
    public UsersQueryBuilder FirstName(string value, string op = "eq") => Filter("firstName", op, value);
    public UsersQueryBuilder LastName(string value, string op = "eq") => Filter("lastName", op, value);
    public UsersQueryBuilder Username(string value, string op = "eq") => Filter("username", op, value);
    public UsersQueryBuilder Email(string value, string op = "eq") => Filter("email", op, value);
    public UsersQueryBuilder MobilePhone(string value, string op = "eq") => Filter("mobilePhone", op, value);
    public UsersQueryBuilder IsActive(bool value, string op = "eq") => Filter("isActive", op, value);

    public UsersQueryBuilder Roles(IEnumerable<string> values, string op = "any")
        => Filter("roles", op, values);

    public UsersQueryBuilder Roles(string value, string op = "any")
        => Filter("roles", op, value);

    // Swagger exposes `roleId` as a dedicated query parameter (not via `filter[...]`).
    public UsersQueryBuilder RoleId(string value)
        => Extra("roleId", value);

    // Swagger exposes `resolve` as a dedicated query parameter (media, mediagallery, references).
    public UsersQueryBuilder Resolve(string value)
        => Extra("resolve", value);

    public UsersQueryBuilder Resolve(params string[] values)
        => Extra("resolve", string.Join(",", values));
}

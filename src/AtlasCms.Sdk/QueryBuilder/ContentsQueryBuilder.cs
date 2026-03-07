namespace AtlasCms.Sdk.QueryBuilder;

public sealed class ContentsQueryBuilder : BaseQueryBuilder<ContentsQueryBuilder>
{
    public ContentsQueryBuilder Status(QueryStatus value)
        => Extra("status", value.ToString().ToLowerInvariant());

    public ContentsQueryBuilder Locale(string value)
        => Extra("locale", value);

    public ContentsQueryBuilder Resolve(params string[] values)
        => Extra("resolve", string.Join(",", values));
}

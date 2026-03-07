namespace AtlasCms.Sdk.QueryBuilder;

public sealed class MediaQueryBuilder : BaseQueryBuilder<MediaQueryBuilder>
{
    public MediaQueryBuilder Folder(string value)
        => Extra("folder", value);

    public MediaQueryBuilder SearchSubdirectory(bool value)
        => Extra("searchSubdirectory", value ? "true" : "false");
}

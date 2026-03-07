using System.Text;

namespace AtlasCms.Sdk.QueryBuilder;

public enum QueryStatus { All, Published, Unpublished }

public enum SortDirection { Asc, Desc }

internal record FilterEntry(string Path, string Operator, IEnumerable<string> Values);

internal record SortEntry(string Field, SortDirection Direction);

public abstract class BaseQueryBuilder<TSelf> where TSelf : BaseQueryBuilder<TSelf>
{
    private int? _page;
    private int? _size;
    private string? _search;
    private readonly List<FilterEntry> _filters = [];
    private readonly List<SortEntry> _sorts = [];
    private readonly Dictionary<string, string> _extras = [];

    public TSelf Page(int value) { _page = value; return (TSelf)this; }
    public TSelf Size(int value) { _size = value; return (TSelf)this; }
    public TSelf Search(string value) { _search = value; return (TSelf)this; }

    public TSelf Filter(string field, string @operator, string value)
        => Filter(field, @operator, [value]);

    public TSelf Filter(string field, string @operator, bool value)
        => Filter(field, @operator, value ? "true" : "false");

    public TSelf Filter(string field, string @operator, IEnumerable<string> values)
    {
        _filters.Add(new FilterEntry(field, @operator, values));
        return (TSelf)this;
    }

    public TSelf FilterRaw(string path, string @operator, string value)
        => Filter(path, @operator, value);

    public TSelf Sort(string field, SortDirection direction = SortDirection.Asc)
    {
        _sorts.Add(new SortEntry(field, direction));
        return (TSelf)this;
    }

    protected TSelf Extra(string key, string value)
    {
        _extras[key] = value;
        return (TSelf)this;
    }

    public string Build()
    {
        var parts = new List<string>();

        if (_page.HasValue) parts.Add($"page={_page}");
        if (_size.HasValue) parts.Add($"size={_size}");
        if (_search is not null) parts.Add($"search={Uri.EscapeDataString(_search)}");

        foreach (var f in _filters)
        {
            var key = $"filter[{f.Path}][{f.Operator}]";
            foreach (var v in f.Values)
                parts.Add($"{Uri.EscapeDataString(key)}={Uri.EscapeDataString(NormalizeValue(v))}");
        }

        if (_sorts.Count > 0)
        {
            var sortValue = string.Join(",", _sorts.Select(s =>
                $"{s.Field}:{(s.Direction == SortDirection.Asc ? "asc" : "desc")}"));
            parts.Add($"sort={Uri.EscapeDataString(sortValue)}");
        }

        foreach (var (key, value) in _extras)
            parts.Add($"{key}={Uri.EscapeDataString(value)}");

        return string.Join("&", parts);
    }

    private static string NormalizeValue(string value) => value;
}

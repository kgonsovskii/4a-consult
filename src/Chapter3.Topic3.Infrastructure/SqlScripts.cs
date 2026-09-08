using System.Collections.Concurrent;

namespace Chapter3.Topic3.Infrastructure;

public sealed class SqlScripts
{
    private readonly ConcurrentDictionary<string, string> _cache = new();

    public string Load(string file) =>
        _cache.GetOrAdd(file, Read);

    private static string Read(string file)
    {
        var name = $"Chapter3.Topic3.Infrastructure.Sql.{file}";
        using var stream = typeof(SqlScripts).Assembly.GetManifestResourceStream(name)
            ?? throw new InvalidOperationException(name);
        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }
}

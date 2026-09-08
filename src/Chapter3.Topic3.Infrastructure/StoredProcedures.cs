using System.Collections.Concurrent;
using Microsoft.EntityFrameworkCore;

namespace Chapter3.Topic3.Infrastructure;

public sealed class StoredProcedures(LibraryContext db)
{
    private readonly ConcurrentDictionary<string, string> _cache = new();

    public string Load(string name) =>
        _cache.GetOrAdd(name, key =>
            db.Procedures.AsNoTracking()
                .Where(p => p.Name == key)
                .Select(p => p.Body)
                .SingleOrDefault()
            ?? throw new InvalidOperationException(key));
}

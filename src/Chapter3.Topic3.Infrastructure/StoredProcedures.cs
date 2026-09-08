using System.Collections.Concurrent;
using Microsoft.Data.Sqlite;

namespace Chapter3.Topic3.Infrastructure;

public sealed class StoredProcedures(string connectionString)
{
    private readonly ConcurrentDictionary<string, string> _cache = new();

    public string Load(string name) =>
        _cache.GetOrAdd(name, Read);

    private string Read(string name)
    {
        using var connection = new SqliteConnection(connectionString);
        connection.Open();
        using var cmd = connection.CreateCommand();
        cmd.CommandText = "SELECT body FROM stored_procedure WHERE name = @name";
        cmd.Parameters.AddWithValue("@name", name);
        return cmd.ExecuteScalar() as string
            ?? throw new InvalidOperationException(name);
    }
}

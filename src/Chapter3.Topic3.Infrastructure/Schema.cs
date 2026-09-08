using Microsoft.Data.Sqlite;

namespace Chapter3.Topic3.Infrastructure;

public sealed class Schema(SqlScripts sql)
{
    public static string Resolve(string connectionString, string contentRoot)
    {
        var settings = new SqliteConnectionStringBuilder(connectionString);
        if (settings.DataSource is not ":memory:" && !Path.IsPathRooted(settings.DataSource))
        {
            settings.DataSource = Path.GetFullPath(Path.Combine(contentRoot, settings.DataSource));
        }

        return settings.ConnectionString;
    }

    public async Task ApplyAsync(string connectionString)
    {
        await using var connection = new SqliteConnection(connectionString);
        await connection.OpenAsync();
        await Exec(connection, sql.Load("schema.sql"));
        await Exec(connection, sql.Load("seed.sql"));
    }

    private static async Task Exec(SqliteConnection connection, string sql)
    {
        await using var cmd = connection.CreateCommand();
        cmd.CommandText = sql;
        await cmd.ExecuteNonQueryAsync();
    }
}

using Microsoft.Data.Sqlite;

namespace Chapter3.Topic3.Infrastructure;

public sealed class Schema(SqlScripts sql)
{
    private static readonly string[] ProcedureFiles =
    [
        "book_select",
        "book_select_by_id",
        "book_insert",
        "book_update",
        "book_delete"
    ];

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
        await InstallProcedures(connection);
        await Exec(connection, sql.Load("seed.sql"));
    }

    private async Task InstallProcedures(SqliteConnection connection)
    {
        foreach (var name in ProcedureFiles)
        {
            await using var cmd = connection.CreateCommand();
            cmd.CommandText =
                "INSERT OR REPLACE INTO stored_procedure (name, body) VALUES (@name, @body)";
            cmd.Parameters.AddWithValue("@name", name);
            cmd.Parameters.AddWithValue("@body", sql.Load($"{name}.sql"));
            await cmd.ExecuteNonQueryAsync();
        }
    }

    private static async Task Exec(SqliteConnection connection, string sql)
    {
        await using var cmd = connection.CreateCommand();
        cmd.CommandText = sql;
        await cmd.ExecuteNonQueryAsync();
    }
}

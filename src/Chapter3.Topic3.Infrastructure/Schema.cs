using Npgsql;

namespace Chapter3.Topic3.Infrastructure;

public static class Schema
{
    public static async Task ApplyAsync(string connectionString)
    {
        await EnsureDatabase(connectionString);
        await using var db = NpgsqlDataSource.Create(connectionString);
        await Exec(db, Read("schema.sql"));
        await Exec(db, Read("seed.sql"));
    }

    private static async Task EnsureDatabase(string connectionString)
    {
        var builder = new NpgsqlConnectionStringBuilder(connectionString);
        var name = builder.Database ?? "library";
        builder.Database = "postgres";

        await using var conn = new NpgsqlConnection(builder.ConnectionString);
        await conn.OpenAsync();

        await using var exists = new NpgsqlCommand(
            "SELECT 1 FROM pg_database WHERE datname = $1", conn);
        exists.Parameters.AddWithValue(name);
        if (await exists.ExecuteScalarAsync() is not null)
        {
            return;
        }

        await using var create = new NpgsqlCommand(
            $"CREATE DATABASE \"{name.Replace("\"", "\"\"")}\"", conn);
        await create.ExecuteNonQueryAsync();
    }

    private static async Task Exec(NpgsqlDataSource db, string sql)
    {
        await using var cmd = db.CreateCommand(sql);
        await cmd.ExecuteNonQueryAsync();
    }

    private static string Read(string file)
    {
        var name = $"Chapter3.Topic3.Infrastructure.Sql.{file}";
        using var stream = typeof(Schema).Assembly.GetManifestResourceStream(name)
            ?? throw new InvalidOperationException(name);
        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }
}

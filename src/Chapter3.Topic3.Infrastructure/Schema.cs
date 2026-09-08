using Microsoft.EntityFrameworkCore;

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
        var settings = new Microsoft.Data.Sqlite.SqliteConnectionStringBuilder(connectionString);
        if (settings.DataSource is not ":memory:" && !Path.IsPathRooted(settings.DataSource))
        {
            settings.DataSource = Path.GetFullPath(Path.Combine(contentRoot, settings.DataSource));
        }

        return settings.ConnectionString;
    }

    public async Task ApplyAsync(string connectionString)
    {
        var options = new DbContextOptionsBuilder<LibraryContext>().UseSqlite(connectionString).Options;
        await using var db = new LibraryContext(options);
        await db.Database.OpenConnectionAsync();
        await db.Database.ExecuteSqlRawAsync(sql.Load("schema.sql"));
        foreach (var name in ProcedureFiles)
        {
            await db.Database.ExecuteSqlRawAsync(
                "INSERT OR REPLACE INTO stored_procedure (name, body) VALUES ({0}, {1})",
                name,
                sql.Load($"{name}.sql"));
        }

        await db.Database.ExecuteSqlRawAsync(sql.Load("seed.sql"));
    }
}

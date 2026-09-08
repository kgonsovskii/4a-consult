using Chapter3.Topic3.Infrastructure;
using FluentAssertions;
using Microsoft.Data.Sqlite;

namespace Chapter3.Topic3.Tests.Infrastructure;

public sealed class SchemaTests
{
    [Fact]
    public void Embedded_sql_is_in_assembly()
    {
        var names = typeof(SqlScripts).Assembly.GetManifestResourceNames();

        names.Should().Contain("Chapter3.Topic3.Infrastructure.Sql.schema.sql");
        names.Should().Contain("Chapter3.Topic3.Infrastructure.Sql.seed.sql");
        names.Should().Contain("Chapter3.Topic3.Infrastructure.Sql.book_select.sql");
    }

    [Fact]
    public async Task Apply_creates_seeded_sqlite_file()
    {
        var path = Path.Combine(Path.GetTempPath(), $"library-{Guid.NewGuid():N}.db");
        var cs = $"Data Source={path};Pooling=False";
        try
        {
            var sql = new SqlScripts();
            await new Schema(sql).ApplyAsync(cs);
            var procedures = new StoredProcedures(cs);
            var books = await new SqliteBookRepository(cs, procedures).ListAsync();

            books.Should().HaveCount(2);
            books[0].Title.Should().Be("Обломов");
            books[0].Toc.Html.Should().Contain("Глава 1");
            procedures.Load("book_select").Should().Contain("SELECT");
            procedures.Load("book_insert").Should().Contain("INSERT");
            procedures.Load("book_update").Should().Contain("UPDATE");
            procedures.Load("book_delete").Should().Contain("DELETE");
        }
        finally
        {
            SqliteConnection.ClearAllPools();
            File.Delete(path);
        }
    }
}

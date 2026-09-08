using System.Xml;
using Chapter3.Topic3.Domain;
using Microsoft.Data.Sqlite;

namespace Chapter3.Topic3.Infrastructure;

public sealed class SqliteBookRepository(string connectionString, SqlScripts sql) : IBookRepository
{
    public Task<IReadOnlyList<Book>> ListAsync() =>
        Query(sql.Load("book_select.sql"));

    public async Task<Book?> GetAsync(int id)
    {
        var books = await Query(sql.Load("book_select_by_id.sql"), cmd =>
            cmd.Parameters.AddWithValue("@id", id));
        return books.Count == 0 ? null : books[0];
    }

    public async Task<Book> AddAsync(Book book)
    {
        var books = await Query(sql.Load("book_insert.sql"), cmd => AddBookParams(cmd, book));
        return books[0];
    }

    public async Task<Book?> SaveAsync(Book book)
    {
        var books = await Query(sql.Load("book_update.sql"), cmd =>
        {
            cmd.Parameters.AddWithValue("@id", book.Id);
            AddBookParams(cmd, book);
        });
        return books.Count == 0 ? null : books[0];
    }

    public async Task RemoveAsync(int id)
    {
        await using var connection = new SqliteConnection(connectionString);
        await connection.OpenAsync();
        await using var cmd = connection.CreateCommand();
        cmd.CommandText = sql.Load("book_delete.sql");
        cmd.Parameters.AddWithValue("@id", id);
        await cmd.ExecuteNonQueryAsync();
    }

    public async Task<IReadOnlyList<string>> HeadingsAsync(int id)
    {
        var book = await GetAsync(id);
        return book is null ? [] : HeadingsOf(book);
    }

    public async Task<IReadOnlyList<TocHeading>> AllHeadingsAsync()
    {
        var books = await ListAsync();
        return books
            .SelectMany(book => HeadingsOf(book).Select(h => new TocHeading(book.Id, book.Title, h)))
            .ToList();
    }

    private async Task<IReadOnlyList<Book>> Query(string sql, Action<SqliteCommand>? bind = null)
    {
        await using var connection = new SqliteConnection(connectionString);
        await connection.OpenAsync();
        await using var cmd = connection.CreateCommand();
        cmd.CommandText = sql;
        bind?.Invoke(cmd);
        return await ReadBooks(cmd);
    }

    private static void AddBookParams(SqliteCommand cmd, Book book)
    {
        cmd.Parameters.AddWithValue("@title", book.Title);
        cmd.Parameters.AddWithValue("@author", book.Author);
        cmd.Parameters.AddWithValue("@year", book.Year);
        cmd.Parameters.AddWithValue("@publisher", book.Publisher);
        cmd.Parameters.AddWithValue("@toc", book.Toc.Xml);
    }

    private static async Task<List<Book>> ReadBooks(SqliteCommand cmd)
    {
        await using var reader = await cmd.ExecuteReaderAsync();
        var books = new List<Book>();
        while (await reader.ReadAsync())
        {
            books.Add(Book.Rehydrate(
                reader.GetInt32(reader.GetOrdinal("id")),
                reader.GetString(reader.GetOrdinal("title")),
                reader.GetString(reader.GetOrdinal("author")),
                reader.GetInt32(reader.GetOrdinal("year")),
                reader.GetString(reader.GetOrdinal("publisher")),
                TableOfContents.FromXml(reader.GetString(reader.GetOrdinal("toc")))));
        }

        return books;
    }

    private static List<string> HeadingsOf(Book book)
    {
        var doc = new XmlDocument();
        doc.LoadXml(book.Toc.Xml);
        return doc.SelectNodes("//*[local-name()='h2']")?
            .Cast<XmlNode>()
            .Select(n => n.InnerText)
            .ToList() ?? [];
    }
}

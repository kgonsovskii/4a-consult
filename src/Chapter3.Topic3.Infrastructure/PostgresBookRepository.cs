using Chapter3.Topic3.Domain;
using Npgsql;

namespace Chapter3.Topic3.Infrastructure;

public sealed class PostgresBookRepository(NpgsqlDataSource db) : IBookRepository
{
    public async Task<IReadOnlyList<Book>> ListAsync()
    {
        await using var cmd = db.CreateCommand("SELECT * FROM book_select()");
        return await ReadBooks(cmd);
    }

    public async Task<Book?> GetAsync(int id)
    {
        await using var cmd = db.CreateCommand("SELECT * FROM book_select_by_id($1)");
        cmd.Parameters.AddWithValue(id);
        var books = await ReadBooks(cmd);
        return books.Count == 0 ? null : books[0];
    }

    public async Task<Book> AddAsync(Book book)
    {
        await using var cmd = db.CreateCommand(
            "SELECT * FROM book_insert($1, $2, $3, $4, $5::xml)");
        AddBookParams(cmd, book);
        return (await ReadBooks(cmd))[0];
    }

    public async Task<Book?> SaveAsync(Book book)
    {
        await using var cmd = db.CreateCommand(
            "SELECT * FROM book_update($1, $2, $3, $4, $5, $6::xml)");
        cmd.Parameters.AddWithValue(book.Id);
        AddBookParams(cmd, book);
        var books = await ReadBooks(cmd);
        return books.Count == 0 ? null : books[0];
    }

    public async Task RemoveAsync(int id)
    {
        await using var cmd = db.CreateCommand("SELECT book_delete($1)");
        cmd.Parameters.AddWithValue(id);
        await cmd.ExecuteNonQueryAsync();
    }

    public async Task<IReadOnlyList<string>> HeadingsAsync(int id)
    {
        await using var cmd = db.CreateCommand("SELECT heading FROM book_toc_headings($1)");
        cmd.Parameters.AddWithValue(id);
        await using var reader = await cmd.ExecuteReaderAsync();
        var headings = new List<string>();
        while (await reader.ReadAsync())
        {
            headings.Add(reader.GetString(0));
        }

        return headings;
    }

    public async Task<IReadOnlyList<TocHeading>> AllHeadingsAsync()
    {
        await using var cmd = db.CreateCommand("SELECT id, title, heading FROM book_toc_all_headings()");
        await using var reader = await cmd.ExecuteReaderAsync();
        var rows = new List<TocHeading>();
        while (await reader.ReadAsync())
        {
            rows.Add(new TocHeading(reader.GetInt32(0), reader.GetString(1), reader.GetString(2)));
        }

        return rows;
    }

    private static void AddBookParams(NpgsqlCommand cmd, Book book)
    {
        cmd.Parameters.AddWithValue(book.Title);
        cmd.Parameters.AddWithValue(book.Author);
        cmd.Parameters.AddWithValue(book.Year);
        cmd.Parameters.AddWithValue(book.Publisher);
        cmd.Parameters.AddWithValue(book.Toc.Xml);
    }

    private static async Task<List<Book>> ReadBooks(NpgsqlCommand cmd)
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
}

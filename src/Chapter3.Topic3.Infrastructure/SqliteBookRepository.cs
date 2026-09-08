using System.Xml;
using Chapter3.Topic3.Domain;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Chapter3.Topic3.Infrastructure;

public interface ISqliteBookRepository : IBookRepository;

public sealed class SqliteBookRepository(LibraryContext db, StoredProcedures procedures) : ISqliteBookRepository
{
    public Task<IReadOnlyList<Book>> ListAsync() =>
        Query(procedures.Load("book_select"));

    public async Task<Book?> GetAsync(int id)
    {
        var books = await Query(procedures.Load("book_select_by_id"), P("@id", id));
        return books.Count == 0 ? null : books[0];
    }

    public async Task<Book> AddAsync(Book book)
    {
        var books = await Query(procedures.Load("book_insert"), BookParams(book));
        return books[0];
    }

    public async Task<Book?> SaveAsync(Book book)
    {
        var books = await Query(procedures.Load("book_update"), [P("@id", book.Id), .. BookParams(book)]);
        return books.Count == 0 ? null : books[0];
    }

    public async Task RemoveAsync(int id)
    {
        await db.Database.ExecuteSqlRawAsync(procedures.Load("book_delete"), P("@id", id));
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

    private async Task<IReadOnlyList<Book>> Query(string sql, params object[] parameters)
    {
        var rows = await db.Database.SqlQueryRaw<BookRecord>(sql, parameters).ToListAsync();
        return rows.Select(ToBook).ToList();
    }

    private static Book ToBook(BookRecord row) =>
        Book.Rehydrate(row.Id, row.Title, row.Author, row.Year, row.Publisher, TableOfContents.FromXml(row.Toc));

    private static object[] BookParams(Book book) =>
    [
        P("@title", book.Title),
        P("@author", book.Author),
        P("@year", book.Year),
        P("@publisher", book.Publisher),
        P("@toc", book.Toc.Xml)
    ];

    private static SqliteParameter P(string name, object value) => new(name, value);

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

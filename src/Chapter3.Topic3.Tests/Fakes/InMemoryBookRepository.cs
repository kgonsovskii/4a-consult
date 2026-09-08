using System.Xml;
using Chapter3.Topic3.Domain;

namespace Chapter3.Topic3.Tests.Fakes;

public sealed class InMemoryBookRepository : IBookRepository
{
    private readonly List<Book> _books = [];
    private int _nextId = 1;

    public InMemoryBookRepository(params Book[] seed)
    {
        foreach (var book in seed)
        {
            _books.Add(WithId(book, _nextId++));
        }
    }

    public Task<IReadOnlyList<Book>> ListAsync() =>
        Task.FromResult<IReadOnlyList<Book>>(_books.ToList());

    public Task<Book?> GetAsync(int id) =>
        Task.FromResult(_books.FirstOrDefault(b => b.Id == id));

    public Task<Book> AddAsync(Book book)
    {
        var stored = WithId(book, _nextId++);
        _books.Add(stored);
        return Task.FromResult(stored);
    }

    public Task<Book?> SaveAsync(Book book)
    {
        var index = _books.FindIndex(b => b.Id == book.Id);
        if (index < 0)
        {
            return Task.FromResult<Book?>(null);
        }

        _books[index] = book;
        return Task.FromResult<Book?>(book);
    }

    public Task RemoveAsync(int id)
    {
        _books.RemoveAll(b => b.Id == id);
        return Task.CompletedTask;
    }

    public Task<IReadOnlyList<string>> HeadingsAsync(int id)
    {
        var book = _books.FirstOrDefault(b => b.Id == id);
        IReadOnlyList<string> headings = book is null ? [] : HeadingsOf(book);
        return Task.FromResult(headings);
    }

    public Task<IReadOnlyList<TocHeading>> AllHeadingsAsync()
    {
        var rows = _books
            .SelectMany(book => HeadingsOf(book).Select(h => new TocHeading(book.Id, book.Title, h)))
            .ToList();
        return Task.FromResult<IReadOnlyList<TocHeading>>(rows);
    }

    private static Book WithId(Book book, int id) =>
        Book.Rehydrate(id, book.Title, book.Author, book.Year, book.Publisher, book.Toc);

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

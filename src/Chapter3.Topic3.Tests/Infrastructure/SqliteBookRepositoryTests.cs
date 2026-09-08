using Chapter3.Topic3.Domain;
using Chapter3.Topic3.Infrastructure;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace Chapter3.Topic3.Tests.Infrastructure;

public sealed class SqliteBookRepositoryTests : IAsyncLifetime, IDisposable
{
    private readonly string _path = Path.Combine(Path.GetTempPath(), $"lib-{Guid.NewGuid():N}.db");
    private readonly string _connectionString;
    private ServiceProvider _provider = null!;
    private IServiceScope _scope = null!;
    private ISqliteBookRepository _books = null!;

    public SqliteBookRepositoryTests()
    {
        _connectionString = $"Data Source={_path};Pooling=False";
    }

    public async Task InitializeAsync()
    {
        _provider = new ServiceCollection()
            .AddLibrary(_connectionString)
            .BuildServiceProvider();
        await _provider.GetRequiredService<Schema>().ApplyAsync(_connectionString);
        _scope = _provider.CreateScope();
        _books = _scope.ServiceProvider.GetRequiredService<ISqliteBookRepository>();
    }

    public async Task DisposeAsync()
    {
        _scope.Dispose();
        await _provider.DisposeAsync();
    }

    public void Dispose()
    {
        try
        {
            File.Delete(_path);
            File.Delete(_path + "-wal");
            File.Delete(_path + "-shm");
        }
        catch (IOException)
        {
        }
    }

    [Fact]
    public async Task List_returns_seeded_books()
    {
        var list = await _books.ListAsync();

        list.Select(b => b.Title).Should().Equal("Обломов", "Мастер и Маргарита");
    }

    [Fact]
    public async Task Add_then_get_roundtrip()
    {
        var added = await _books.AddAsync(Book.Create(
            "Идиот",
            "Ф. М. Достоевский",
            1869,
            "Стелловский",
            TableOfContents.FromXml("<toc><h2>Часть первая</h2></toc>")));

        var loaded = await _books.GetAsync(added.Id);

        loaded.Should().BeEquivalentTo(added);
    }

    [Fact]
    public async Task Save_updates_title()
    {
        var book = (await _books.GetAsync(1))!;
        book.Update("Обломов (изд.)", book.Author, book.Year, book.Publisher, book.Toc);

        var saved = await _books.SaveAsync(book);

        saved!.Title.Should().Be("Обломов (изд.)");
        (await _books.GetAsync(1))!.Title.Should().Be("Обломов (изд.)");
    }

    [Fact]
    public async Task Save_missing_returns_null()
    {
        var ghost = Book.Rehydrate(
            999,
            "Нет",
            "Нет",
            1900,
            "Нет",
            TableOfContents.FromXml("<toc/>"));

        (await _books.SaveAsync(ghost)).Should().BeNull();
    }

    [Fact]
    public async Task Remove_drops_book()
    {
        await _books.RemoveAsync(1);

        (await _books.GetAsync(1)).Should().BeNull();
        (await _books.ListAsync()).Should().HaveCount(1);
    }

    [Fact]
    public async Task Headings_come_from_xml()
    {
        var headings = await _books.HeadingsAsync(1);

        headings.Should().Equal("Часть I. Глава 1", "Часть I. Глава 2");
    }

    [Fact]
    public async Task All_headings_include_book_title()
    {
        var rows = await _books.AllHeadingsAsync();

        rows.Should().Contain(h => h.BookTitle == "Обломов" && h.Heading == "Часть I. Глава 1");
    }
}

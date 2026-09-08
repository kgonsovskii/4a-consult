using Chapter3.Topic3.Domain;
using FluentAssertions;

namespace Chapter3.Topic3.Tests.Domain;

public sealed class BookTests
{
    [Fact]
    public void Create_then_update_keeps_new_author()
    {
        var createdToc = TableOfContents.FromXml("<toc/>");
        var updatedToc = TableOfContents.FromXml("<toc><h2>Часть первая</h2></toc>");
        var book = Book.Create("Обломов", "Гончаров", 1859, "Глазунов", createdToc);

        book.Update("Идиот", "Ф. М. Достоевский", 1869, "Стелловский", updatedToc);

        book.Should().BeEquivalentTo(new
        {
            Id = 0,
            Title = "Идиот",
            Author = "Ф. М. Достоевский",
            Year = 1869,
            Publisher = "Стелловский",
            Toc = new { updatedToc.Xml, updatedToc.Html }
        });
    }
}

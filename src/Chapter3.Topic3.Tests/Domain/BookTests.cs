using Chapter3.Topic3.Domain;
using FluentAssertions;

namespace Chapter3.Topic3.Tests.Domain;

public sealed class BookTests
{
    [Fact]
    public void Create_then_update_keeps_new_author()
    {
        var book = Book.Create("Обломов", "Гончаров", 1859, "Глазунов", TableOfContents.FromXml("<toc/>"));
        book.Update("Обломов", "И. А. Гончаров", 1859, "Глазунов", book.Toc);

        book.Author.Should().Be("И. А. Гончаров");
        book.Id.Should().Be(0);
    }
}

using Chapter3.Topic3.Application;
using FluentAssertions;

namespace Chapter3.Topic3.Tests.Application;

public sealed class BookFormTests
{
    [Fact]
    public void Roundtrip_preserves_fields()
    {
        var form = BookForm.From(SampleBook.Oblomov());
        var book = form.ToBook();

        book.Title.Should().Be("Обломов");
        book.Author.Should().Be("И. А. Гончаров");
        book.Year.Should().Be(1859);
        book.Toc.Html.Should().Contain("Часть I. Глава 1");
    }
}

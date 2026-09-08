using System.ComponentModel.DataAnnotations;
using Chapter3.Topic3.Domain;

namespace Chapter3.Topic3.Application;

public sealed class BookForm
{
    public int Id { get; set; }

    [Required, Display(Name = "Название")]
    public string Title { get; set; } = "";

    [Required, Display(Name = "Автор")]
    public string Author { get; set; } = "";

    [Range(1, 3000), Display(Name = "Год")]
    public int Year { get; set; }

    [Required, Display(Name = "Издательство")]
    public string Publisher { get; set; } = "";

    [Display(Name = "Оглавление")]
    public string TocHtml { get; set; } = "";

    public static BookForm From(Book book) => new()
    {
        Id = book.Id,
        Title = book.Title,
        Author = book.Author,
        Year = book.Year,
        Publisher = book.Publisher,
        TocHtml = book.Toc.Html
    };

    public Book ToBook()
    {
        var toc = TocParser.Parse(TocHtml);
        return Id == 0
            ? Book.Create(Title, Author, Year, Publisher, toc)
            : Book.Rehydrate(Id, Title, Author, Year, Publisher, toc);
    }
}

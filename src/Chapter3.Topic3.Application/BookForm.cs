using System.ComponentModel.DataAnnotations;
using Chapter3.Topic3.Domain;

namespace Chapter3.Topic3.Application;

public sealed class BookForm
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Нужно название")]
    [StringLength(256, ErrorMessage = "Название слишком длинное")]
    [Display(Name = "Название")]
    public string Title { get; set; } = "";

    [Required(ErrorMessage = "Нужен автор")]
    [StringLength(256, ErrorMessage = "Автор слишком длинный")]
    [Display(Name = "Автор")]
    public string Author { get; set; } = "";

    [Required(ErrorMessage = "Нужен год")]
    [Range(1, 3000, ErrorMessage = "Год от 1 до 3000")]
    [Display(Name = "Год")]
    public int? Year { get; set; }

    [Required(ErrorMessage = "Нужно издательство")]
    [StringLength(256, ErrorMessage = "Издательство слишком длинное")]
    [Display(Name = "Издательство")]
    public string Publisher { get; set; } = "";

    [Required(ErrorMessage = "Нужно оглавление")]
    [StringLength(20000, ErrorMessage = "Оглавление слишком длинное")]
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
        var year = Year ?? 0;
        return Id == 0
            ? Book.Create(Title, Author, year, Publisher, toc)
            : Book.Rehydrate(Id, Title, Author, year, Publisher, toc);
    }
}

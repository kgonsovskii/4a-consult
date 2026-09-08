namespace Chapter3.Topic3.Domain;

public sealed class Book
{
    public int Id { get; private set; }
    public string Title { get; private set; }
    public string Author { get; private set; }
    public int Year { get; private set; }
    public string Publisher { get; private set; }
    public TableOfContents Toc { get; private set; }

    private Book(int id, string title, string author, int year, string publisher, TableOfContents toc)
    {
        Id = id;
        Title = title;
        Author = author;
        Year = year;
        Publisher = publisher;
        Toc = toc;
    }

    public static Book Create(string title, string author, int year, string publisher, TableOfContents toc) =>
        new(0, title, author, year, publisher, toc);

    public static Book Rehydrate(int id, string title, string author, int year, string publisher, TableOfContents toc) =>
        new(id, title, author, year, publisher, toc);

    public void Update(string title, string author, int year, string publisher, TableOfContents toc)
    {
        Title = title;
        Author = author;
        Year = year;
        Publisher = publisher;
        Toc = toc;
    }
}

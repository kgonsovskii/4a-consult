namespace Chapter3.Topic3.Domain;

public sealed class TocHeading
{
    public int BookId { get; }
    public string BookTitle { get; }
    public string Heading { get; }

    public TocHeading(int bookId, string bookTitle, string heading)
    {
        BookId = bookId;
        BookTitle = bookTitle;
        Heading = heading;
    }
}

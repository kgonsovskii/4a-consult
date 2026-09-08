namespace Chapter3.Topic3.Domain;

public interface IBookRepository
{
    Task<IReadOnlyList<Book>> ListAsync();
    Task<Book?> GetAsync(int id);
    Task<Book> AddAsync(Book book);
    Task<Book?> SaveAsync(Book book);
    Task RemoveAsync(int id);
    Task<IReadOnlyList<string>> HeadingsAsync(int id);
    Task<IReadOnlyList<TocHeading>> AllHeadingsAsync();
}

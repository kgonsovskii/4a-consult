using Microsoft.EntityFrameworkCore;

namespace Chapter3.Topic3.Infrastructure;

public sealed class LibraryContext(DbContextOptions<LibraryContext> options) : DbContext(options)
{
    public DbSet<BookRecord> Books => Set<BookRecord>();
    public DbSet<ProcedureRecord> Procedures => Set<ProcedureRecord>();

    protected override void OnModelCreating(ModelBuilder model)
    {
        model.Entity<BookRecord>(e =>
        {
            e.ToTable("book");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnName("id");
            e.Property(x => x.Title).HasColumnName("title");
            e.Property(x => x.Author).HasColumnName("author");
            e.Property(x => x.Year).HasColumnName("year");
            e.Property(x => x.Publisher).HasColumnName("publisher");
            e.Property(x => x.Toc).HasColumnName("toc");
        });
        model.Entity<ProcedureRecord>(e =>
        {
            e.ToTable("stored_procedure");
            e.HasKey(x => x.Name);
            e.Property(x => x.Name).HasColumnName("name");
            e.Property(x => x.Body).HasColumnName("body");
        });
    }
}

public sealed class BookRecord
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public string Author { get; set; } = "";
    public int Year { get; set; }
    public string Publisher { get; set; } = "";
    public string Toc { get; set; } = "";
}

public sealed class ProcedureRecord
{
    public string Name { get; set; } = "";
    public string Body { get; set; } = "";
}

using Chapter3.Topic3.Domain;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chapter3.Topic3.WebForms.Pages;

public sealed class IndexModel(IBookRepository books) : PageModel
{
    public IReadOnlyList<Book> Books { get; private set; } = [];

    public async Task OnGetAsync() =>
        Books = await books.ListAsync();
}

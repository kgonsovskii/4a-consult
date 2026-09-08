using Chapter3.Topic3.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chapter3.Topic3.WebForms.Pages;

public sealed class DetailsModel(IBookRepository books) : PageModel
{
    public Book Book { get; private set; } = null!;
    public IReadOnlyList<string> Headings { get; private set; } = [];

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var book = await books.GetAsync(id);
        if (book is null)
        {
            return NotFound();
        }

        Book = book;
        Headings = await books.HeadingsAsync(id);
        return Page();
    }
}

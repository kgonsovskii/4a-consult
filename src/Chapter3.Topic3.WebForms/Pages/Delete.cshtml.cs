using Chapter3.Topic3.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chapter3.Topic3.WebForms.Pages;

public sealed class DeleteModel(IBookRepository books) : PageModel
{
    public Book Book { get; private set; } = null!;

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var book = await books.GetAsync(id);
        if (book is null)
        {
            return NotFound();
        }

        Book = book;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        await books.RemoveAsync(id);
        return RedirectToPage("Index");
    }
}

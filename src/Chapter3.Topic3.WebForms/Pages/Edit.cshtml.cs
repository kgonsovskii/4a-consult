using Chapter3.Topic3.Application;
using Chapter3.Topic3.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chapter3.Topic3.WebForms.Pages;

public sealed class EditModel(IBookRepository books) : PageModel
{
    [BindProperty]
    public BookForm Form { get; set; } = new();

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id is null)
        {
            return Page();
        }

        var book = await books.GetAsync(id.Value);
        if (book is null)
        {
            return NotFound();
        }

        Form = BookForm.From(book);
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int? id)
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        if (id is null)
        {
            var created = await books.AddAsync(Form.ToBook());
            return RedirectToPage("Details", new { id = created.Id });
        }

        Form.Id = id.Value;
        var updated = await books.SaveAsync(Form.ToBook());
        return updated is null ? NotFound() : RedirectToPage("Details", new { id });
    }
}

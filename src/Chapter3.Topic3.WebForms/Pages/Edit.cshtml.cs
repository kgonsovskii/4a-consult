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

        try
        {
            if (id is null)
            {
                var created = await books.AddAsync(Form.ToBook());
                TempData["Status"] = "Книга добавлена.";
                return RedirectToPage("Details", new { id = created.Id });
            }

            Form.Id = id.Value;
            var updated = await books.SaveAsync(Form.ToBook());
            if (updated is null)
            {
                TempData["Error"] = "Книга не найдена.";
                return RedirectToPage("Index");
            }

            TempData["Status"] = "Книга сохранена.";
            return RedirectToPage("Details", new { id });
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return Page();
        }
    }
}

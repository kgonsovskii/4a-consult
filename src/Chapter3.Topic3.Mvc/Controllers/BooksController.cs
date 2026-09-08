using Chapter3.Topic3.Application;
using Chapter3.Topic3.Domain;
using Microsoft.AspNetCore.Mvc;

namespace Chapter3.Topic3.Mvc;

public sealed class BooksController(IBookRepository books) : Controller
{
    public async Task<IActionResult> Index() =>
        View(await books.ListAsync());

    public async Task<IActionResult> Details(int id)
    {
        var book = await books.GetAsync(id);
        if (book is null)
        {
            return NotFound();
        }

        ViewBag.Headings = await books.HeadingsAsync(id);
        return View(book);
    }

    public IActionResult Create() => View("Edit", new BookForm());

    [HttpPost]
    public async Task<IActionResult> Create(BookForm form)
    {
        if (!ModelState.IsValid)
        {
            return View("Edit", form);
        }

        try
        {
            var book = await books.AddAsync(form.ToBook());
            TempData["Status"] = "Книга добавлена.";
            return RedirectToAction(nameof(Details), new { id = book.Id });
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View("Edit", form);
        }
    }

    public async Task<IActionResult> Edit(int id)
    {
        var book = await books.GetAsync(id);
        return book is null ? NotFound() : View(BookForm.From(book));
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, BookForm form)
    {
        if (!ModelState.IsValid)
        {
            return View(form);
        }

        form.Id = id;
        try
        {
            var book = await books.SaveAsync(form.ToBook());
            if (book is null)
            {
                TempData["Error"] = "Книга не найдена.";
                return RedirectToAction(nameof(Index));
            }

            TempData["Status"] = "Книга сохранена.";
            return RedirectToAction(nameof(Details), new { id });
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(form);
        }
    }

    public async Task<IActionResult> Delete(int id)
    {
        var book = await books.GetAsync(id);
        return book is null ? NotFound() : View(book);
    }

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await books.RemoveAsync(id);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Xml() =>
        View(await books.AllHeadingsAsync());
}

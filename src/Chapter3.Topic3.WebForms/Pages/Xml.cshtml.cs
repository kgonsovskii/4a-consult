using Chapter3.Topic3.Domain;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chapter3.Topic3.WebForms.Pages;

public sealed class XmlModel(IBookRepository books) : PageModel
{
    public IReadOnlyList<TocHeading> Headings { get; private set; } = [];

    public async Task OnGetAsync() =>
        Headings = await books.AllHeadingsAsync();
}

using System.Net;
using FluentAssertions;

namespace Chapter3.Topic3.Tests.WebForms;

public sealed class BooksHappyPathTests(WebFormsFactory factory) : IClassFixture<WebFormsFactory>
{
    private readonly HttpClient _client = factory.CreateClient(new() { AllowAutoRedirect = false });

    [Fact]
    public async Task List_shows_seeded_book()
    {
        var response = await _client.GetAsync("/");
        var html = System.Net.WebUtility.HtmlDecode(await response.Content.ReadAsStringAsync());

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        html.Should().Contain("Обломов");
    }

    [Fact]
    public async Task Create_redirects_to_card()
    {
        var response = await _client.PostAsync("/Edit", new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["Form.Title"] = "Идиот",
            ["Form.Author"] = "Ф. М. Достоевский",
            ["Form.Year"] = "1869",
            ["Form.Publisher"] = "Стелловский",
            ["Form.TocHtml"] = "<h2>Часть первая</h2>"
        }));

        response.StatusCode.Should().Be(HttpStatusCode.Redirect);
        response.Headers.Location!.ToString().Should().Contain("/Details/");
    }
}

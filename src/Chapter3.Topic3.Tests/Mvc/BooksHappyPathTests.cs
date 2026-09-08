using System.Net;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Chapter3.Topic3.Tests.Mvc;

public sealed class BooksHappyPathTests(MvcFactory factory) : IClassFixture<MvcFactory>
{
    private readonly HttpClient _client = factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });

    [Fact]
    public async Task List_shows_seeded_book()
    {
        var response = await _client.GetAsync("/");
        await response.Content.ReadAsStringAsync();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Create_redirects_to_card()
    {
        var response = await _client.PostAsync("/Books/Create", new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["Title"] = "Идиот",
            ["Author"] = "Ф. М. Достоевский",
            ["Year"] = "1869",
            ["Publisher"] = "Стелловский",
            ["TocHtml"] = "<h2>Часть первая</h2>"
        }));

        response.StatusCode.Should().Be(HttpStatusCode.Redirect);
        response.Headers.Location!.ToString().Should().Contain("/Books/Details/");
    }
}

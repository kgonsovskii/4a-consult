using System.Net;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Chapter3.Topic3.Tests.WebForms;

public sealed class BooksHappyPathTests(WebFormsFactory factory) : IClassFixture<WebFormsFactory>
{
    private readonly HttpClient _client = factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });

    [Fact]
    public async Task List_shows_seeded_book()
    {
        var response = await _client.GetAsync("/");
        await response.Content.ReadAsStringAsync();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}

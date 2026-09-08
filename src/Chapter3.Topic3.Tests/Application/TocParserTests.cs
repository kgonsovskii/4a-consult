using Chapter3.Topic3.Application;
using FluentAssertions;

namespace Chapter3.Topic3.Tests.Application;

public sealed class TocParserTests
{
    [Fact]
    public void Parse_wraps_html_as_xml()
    {
        var toc = TocParser.Parse("<h2>Глава 1</h2><p>Текст</p>");

        toc.Xml.Should().Contain("<toc>");
        toc.Html.Should().Contain("Глава 1");
    }
}

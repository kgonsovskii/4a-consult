using Chapter3.Topic3.Domain;
using FluentAssertions;

namespace Chapter3.Topic3.Tests.Domain;

public sealed class TableOfContentsTests
{
    [Fact]
    public void FromXml_keeps_inner_html()
    {
        var toc = TableOfContents.FromXml("<toc><h2>Глава 1</h2></toc>");

        toc.Html.Should().Contain("Глава 1");
        toc.Xml.Should().Contain("<h2>");
    }
}

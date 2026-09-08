using Chapter3.Topic3.Domain;
using HtmlAgilityPack;

namespace Chapter3.Topic3.Application;

public static class TocParser
{
    public static TableOfContents Parse(string? html)
    {
        var doc = new HtmlDocument
        {
            OptionOutputAsXml = true,
            OptionAutoCloseOnEnd = true
        };
        doc.LoadHtml($"<toc>{html ?? ""}</toc>");
        var toc = doc.DocumentNode.SelectSingleNode("//toc");
        return TableOfContents.FromXml(toc?.OuterHtml ?? "<toc/>");
    }
}

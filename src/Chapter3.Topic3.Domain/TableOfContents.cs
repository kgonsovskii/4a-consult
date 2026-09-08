using System.Xml;

namespace Chapter3.Topic3.Domain;

public sealed class TableOfContents
{
    public string Xml { get; }
    public string Html { get; }

    private TableOfContents(string xml, string html)
    {
        Xml = xml;
        Html = html;
    }

    public static TableOfContents FromXml(string? xml)
    {
        if (string.IsNullOrWhiteSpace(xml))
        {
            return new TableOfContents("<toc/>", "");
        }

        var doc = new XmlDocument();
        doc.LoadXml(xml);
        return new TableOfContents(doc.OuterXml, doc.DocumentElement?.InnerXml ?? "");
    }
}

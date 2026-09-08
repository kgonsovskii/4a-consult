using System.Xml;
using System.Xml.Schema;
using Chapter3.Topic1.Checks;

namespace Chapter3.Topic1;

public sealed class PurchaseXmlValidator(params XmlWellFormednessCheck[] checks)
{
    private readonly XmlDocument _document = new();
    private readonly XmlWellFormednessScanner _scanner = new(checks);

    public PurchaseXmlValidator()
        : this(
            new MultipleRootsCheck(),
            new UnescapedLessThanCheck(),
            new MismatchedCloseTagCheck(),
            new UnquotedAttributeCheck(),
            new UnexpectedOpenTagCheck(),
            new UnclosedTagCheck())
    {
    }

    public IReadOnlyList<string> FindErrors(string xml)
    {
        var schemaErrors = new List<string>();
        try
        {
            _document.LoadXml(xml);
            if (_document.Schemas.Count == 0)
            {
                return [];
            }

            _document.Validate((_, e) =>
            {
                if (e.Severity == XmlSeverityType.Error)
                {
                    schemaErrors.Add(e.Message);
                }
            });
            return schemaErrors;
        }
        catch (XmlException)
        {
            return _scanner.Scan(xml);
        }
    }
}

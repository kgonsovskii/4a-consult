namespace Chapter3.Topic1;

public sealed class XmlScanState(string xml, XmlErrorMessages messages)
{
    public string Xml { get; } = xml;
    public XmlErrorMessages Messages { get; } = messages;
    public Stack<string> Open { get; } = new();
    public List<string> Roots { get; } = [];
    public List<string> Errors { get; } = [];
    public bool SkipPush { get; set; }
}

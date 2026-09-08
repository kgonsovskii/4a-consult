namespace Chapter3.Topic1.Checks;

public abstract class XmlWellFormednessCheck
{
    public virtual void OnStartTag(XmlScanState state, string name, string tagBody)
    {
    }

    public virtual void OnEndTag(XmlScanState state, string name, int positionAfter)
    {
    }

    public virtual void OnUnescapedLt(XmlScanState state)
    {
    }

    public virtual void OnFinished(XmlScanState state)
    {
    }
}

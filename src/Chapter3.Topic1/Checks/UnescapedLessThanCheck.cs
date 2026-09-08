namespace Chapter3.Topic1.Checks;

public sealed class UnescapedLessThanCheck : XmlWellFormednessCheck
{
    public override void OnUnescapedLt(XmlScanState state)
    {
        var owner = state.Open.Count == 0 ? "xml" : state.Open.Peek();
        state.Errors.Add(state.Messages.UnescapedLt(owner));
    }
}

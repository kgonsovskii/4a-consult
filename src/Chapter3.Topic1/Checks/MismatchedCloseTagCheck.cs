namespace Chapter3.Topic1.Checks;

public sealed class MismatchedCloseTagCheck : XmlWellFormednessCheck
{
    public override void OnEndTag(XmlScanState state, string name, int positionAfter)
    {
        if (state.Open.Count == 0 || state.Open.Peek() == name)
        {
            return;
        }

        if (!state.Open.Contains(name))
        {
            return;
        }

        var skipped = state.Open.Peek();
        var laterClose = state.Xml.IndexOf($"</{skipped}>", positionAfter, StringComparison.Ordinal) >= 0;
        state.Errors.Add(
            laterClose
                ? state.Messages.WrongCloseOrder(name, skipped)
                : state.Messages.Unclosed(skipped));
    }
}

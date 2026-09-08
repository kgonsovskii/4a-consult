namespace Chapter3.Topic1.Checks;

public sealed class UnexpectedOpenTagCheck : XmlWellFormednessCheck
{
    public override void OnStartTag(XmlScanState state, string name, string tagBody)
    {
        if (state.Open.Count > 0 && state.Open.Peek() == name)
        {
            state.Errors.Add(state.Messages.UnexpectedOpen(name));
            state.Open.Pop();
            state.SkipPush = true;
        }
    }
}

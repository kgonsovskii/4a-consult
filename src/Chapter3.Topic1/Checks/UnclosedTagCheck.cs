namespace Chapter3.Topic1.Checks;

public sealed class UnclosedTagCheck : XmlWellFormednessCheck
{
    public override void OnFinished(XmlScanState state)
    {
        foreach (var leftover in state.Open.Reverse())
        {
            state.Errors.Add(state.Messages.Unclosed(leftover));
        }
    }
}

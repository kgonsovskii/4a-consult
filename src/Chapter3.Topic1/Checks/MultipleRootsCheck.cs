namespace Chapter3.Topic1.Checks;

public sealed class MultipleRootsCheck : XmlWellFormednessCheck
{
    public override void OnFinished(XmlScanState state)
    {
        if (state.Roots.Count > 1)
        {
            state.Errors.Insert(0, state.Messages.MultipleRoots(state.Roots));
        }
    }
}

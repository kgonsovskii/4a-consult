namespace Chapter3.Topic1.Checks;

public sealed class UnquotedAttributeCheck : XmlWellFormednessCheck
{
    public override void OnStartTag(XmlScanState state, string name, string tagBody)
    {
        var eq = tagBody.IndexOf('=');
        while (eq >= 0)
        {
            var nameStart = eq - 1;
            while (nameStart >= 0 && !char.IsWhiteSpace(tagBody[nameStart]))
            {
                nameStart--;
            }

            var attribute = tagBody[(nameStart + 1)..eq];
            var after = tagBody.AsSpan(eq + 1).TrimStart();
            if (after.Length == 0 || after[0] is not '"' and not '\'')
            {
                state.Errors.Add(state.Messages.UnquotedAttribute(name, attribute));
            }

            eq = tagBody.IndexOf('=', eq + 1);
        }
    }
}

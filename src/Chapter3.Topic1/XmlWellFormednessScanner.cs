using Chapter3.Topic1.Checks;

namespace Chapter3.Topic1;

public sealed class XmlWellFormednessScanner(IReadOnlyList<XmlWellFormednessCheck> checks)
{
    public IReadOnlyList<string> Scan(string xml)
    {
        var state = new XmlScanState(xml, new XmlErrorMessages());
        var i = 0;

        while (i < xml.Length)
        {
            if (xml[i] != '<')
            {
                i++;
                continue;
            }

            if (i + 1 < xml.Length && !IsTagStart(xml[i + 1]))
            {
                Notify(check => check.OnUnescapedLt(state));
                i++;
                continue;
            }

            var end = xml.IndexOf('>', i);
            if (end < 0)
            {
                break;
            }

            var body = xml[(i + 1)..end].Trim();
            var afterTag = end + 1;

            if (body.StartsWith('/'))
            {
                var name = TagName(body[1..]);
                Notify(check => check.OnEndTag(state, name, afterTag));
                Close(state, name);
                i = afterTag;
                continue;
            }

            var tagName = TagName(body);
            state.SkipPush = false;
            Notify(check => check.OnStartTag(state, tagName, body));

            if (state.Open.Count == 0 && !state.SkipPush)
            {
                state.Roots.Add(tagName);
            }

            if (!state.SkipPush && !body.EndsWith('/'))
            {
                state.Open.Push(tagName);
            }

            i = afterTag;
        }

        Notify(check => check.OnFinished(state));
        return state.Errors;
    }

    private void Notify(Action<XmlWellFormednessCheck> action)
    {
        foreach (var check in checks)
        {
            action(check);
        }
    }

    private void Close(XmlScanState state, string name)
    {
        if (state.Open.Count == 0)
        {
            return;
        }

        if (state.Open.Peek() == name)
        {
            state.Open.Pop();
            return;
        }

        if (!state.Open.Contains(name))
        {
            return;
        }

        while (state.Open.Count > 0 && state.Open.Pop() != name)
        {
        }
    }

    private bool IsTagStart(char next) =>
        next is '/' or '?' or '!' || char.IsLetter(next) || next == '_';

    private string TagName(string body)
    {
        var span = body.AsSpan().TrimStart('/');
        var space = span.IndexOfAny(' ', '/');
        return space < 0 ? span.ToString() : span[..space].ToString();
    }
}

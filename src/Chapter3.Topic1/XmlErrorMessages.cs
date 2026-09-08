namespace Chapter3.Topic1;

public sealed class XmlErrorMessages
{
    public string MultipleRoots(IReadOnlyCollection<string> roots) =>
        $"Несколько корневых элементов: {string.Join(", ", roots)}.";

    public string UnescapedLt(string owner) =>
        $"{owner}: символ < не экранирован (&lt;).";

    public string WrongCloseOrder(string closer, string skipped) =>
        $"{closer}/{skipped}: теги закрыты в неправильном порядке.";

    public string Unclosed(string name) =>
        $"{name}: нет закрывающего тега.";

    public string UnquotedAttribute(string tag, string name) =>
        $"{tag}: атрибут {name} без кавычек.";

    public string UnexpectedOpen(string name) =>
        $"{name}: вместо </{name}> стоит открывающий <{name}>.";
}

namespace Chapter3.Topic3.Infrastructure;

internal static class SqlScripts
{
    public static string Load(string file)
    {
        var name = $"Chapter3.Topic3.Infrastructure.Sql.{file}";
        using var stream = typeof(SqlScripts).Assembly.GetManifestResourceStream(name)
            ?? throw new InvalidOperationException(name);
        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }
}

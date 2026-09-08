using Chapter3.Topic3.Infrastructure;
using FluentAssertions;

namespace Chapter3.Topic3.Tests.Infrastructure;

public sealed class SchemaTests
{
    [Fact]
    public void Embedded_sql_is_in_assembly()
    {
        var names = typeof(Schema).Assembly.GetManifestResourceNames();

        names.Should().Contain("Chapter3.Topic3.Infrastructure.Sql.schema.sql");
        names.Should().Contain("Chapter3.Topic3.Infrastructure.Sql.seed.sql");
    }
}

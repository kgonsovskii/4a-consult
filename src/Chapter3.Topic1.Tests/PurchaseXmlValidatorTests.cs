using FluentAssertions;

namespace Chapter3.Topic1.Tests;

public sealed class PurchaseXmlValidatorTests
{
    private readonly PurchaseXmlValidator _validator = new();

    [Fact]
    public void Invalid_xml_has_errors()
    {
        var messages = new XmlErrorMessages();
        var errors = _validator.FindErrors(Read("invalid.xml"));

        errors.Should().HaveCount(6);
        errors.Should().Contain(messages.MultipleRoots(["PurchaseInfo", "BidInfo", "RequestInfo"]));
        errors.Should().Contain(messages.UnescapedLt("PurchaseName"));
        errors.Should().Contain(messages.WrongCloseOrder("TypeInfo", "TypeName"));
        errors.Should().Contain(messages.Unclosed("BidCurrency"));
        errors.Should().Contain(messages.UnquotedAttribute("BuId", "AccessByOrganization"));
        errors.Should().Contain(messages.UnexpectedOpen("RequestNo"));
    }

    [Fact]
    public void Valid_xml_has_no_errors()
    {
        _validator.FindErrors(Read("valid.xml")).Should().BeEmpty();
    }

    private string Read(string fileName) =>
        File.ReadAllText(Path.Combine(AppContext.BaseDirectory, fileName));
}

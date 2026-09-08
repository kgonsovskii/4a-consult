using Chapter3.Topic1;
using FluentAssertions;
using System.Xml;

namespace Chapter3.Topic1.Tests;

public sealed class InvalidPurchaseXmlTests
{
    [Fact]
    public void Sample_is_not_well_formed()
    {
        var load = () => new XmlDocument().LoadXml(InvalidPurchaseXml.Sample);

        load.Should().Throw<XmlException>();
    }

    [Fact]
    public void All_validation_errors_are_listed()
    {
        InvalidPurchaseXml.Errors.Should().Equal(
            "Несколько корневых элементов: PurchaseInfo, BidInfo, RequestInfo.",
            "PurchaseName: символ < не экранирован (&lt;).",
            "TypeInfo/TypeName: теги закрыты в неправильном порядке.",
            "BidCurrency: нет закрывающего тега.",
            "BuId: атрибут AccessByOrganization без кавычек.",
            "RequestNo: вместо </RequestNo> стоит открывающий <RequestNo>.");
    }
}

namespace Chapter3.Topic1;

public static class InvalidPurchaseXml
{
    public const string Sample = """
        <PurchaseInfo>
            <PurchaseId>7380554</PurchaseId>
            <PurchaseCode>SBR031-1910280001</PurchaseCode>
            <PurchaseName>Конкурс с ценой < 500 000 руб.</PurchaseName>
            <TypeInfo><TypeName>Конкурс</TypeInfo></TypeName>
        </PurchaseInfo>
        <BidInfo>
            <BidId>652245</BidId>
            <BidName>Право на заключение договора</BidName>
            <BidNo>1</BidNo>
            <BidPrice>2000000.00</BidPrice>
            <BidCurrency>Российский рубль
            <BidCurrencyName>57287</BidCurrencyName>
        </BidInfo>
        <RequestInfo>
            <BuId AccessByOrganization=1>20535</BuId>
            <RequestBuName>ИП Анар Ростовский</RequestBuName>
            <RequestCreatedDate>28.10.2019 17:42</RequestCreatedDate>
            <RequestId>157545</RequestId>
            <RequestINN>1000000000004</RequestINN>
            <RequestNo>2<RequestNo>
        </RequestInfo>
        """;

    public static readonly string[] Errors =
    [
        "Несколько корневых элементов: PurchaseInfo, BidInfo, RequestInfo.",
        "PurchaseName: символ < не экранирован (&lt;).",
        "TypeInfo/TypeName: теги закрыты в неправильном порядке.",
        "BidCurrency: нет закрывающего тега.",
        "BuId: атрибут AccessByOrganization без кавычек.",
        "RequestNo: вместо </RequestNo> стоит открывающий <RequestNo>.",
    ];
}

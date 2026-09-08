using Chapter3.Topic3.Domain;

namespace Chapter3.Topic3.Tests;

internal static class SampleBook
{
    public static Book Oblomov() =>
        Book.Rehydrate(
            1,
            "Обломов",
            "И. А. Гончаров",
            1859,
            "А. И. Глазунов",
            TableOfContents.FromXml("<toc><h2>Часть I. Глава 1</h2></toc>"));
}

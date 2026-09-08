INSERT INTO book (title, author, year, publisher, toc)
SELECT v.title, v.author, v.year, v.publisher, v.toc::xml
FROM (VALUES
    (
        'Обломов',
        'И. А. Гончаров',
        1859,
        'А. И. Глазунов',
        '<toc><h2>Часть I. Глава 1</h2><p>Обломов лежит на диване.</p><h2>Часть I. Глава 2</h2><p>Утро в доме.</p></toc>'
    ),
    (
        'Мастер и Маргарита',
        'М. А. Булгаков',
        1967,
        'Художественная литература',
        '<toc><h2>Глава 1. Никогда не разговаривайте с неизвестными</h2><p>Патриаршие пруды.</p><h2>Глава 2. Понтий Пилат</h2><p>Дворец Ирода.</p></toc>'
    )
) AS v(title, author, year, publisher, toc)
WHERE NOT EXISTS (SELECT 1 FROM book);

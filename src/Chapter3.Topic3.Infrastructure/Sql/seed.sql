INSERT INTO book (title, author, year, publisher, toc)
SELECT v.title, v.author, v.year, v.publisher, v.toc
FROM (
    SELECT
        'Обломов' AS title,
        'И. А. Гончаров' AS author,
        1859 AS year,
        'А. И. Глазунов' AS publisher,
        '<toc><h2>Часть I. Глава 1</h2><p>Обломов лежит на диване.</p><h2>Часть I. Глава 2</h2><p>Утро в доме.</p></toc>' AS toc
    UNION ALL
    SELECT
        'Мастер и Маргарита',
        'М. А. Булгаков',
        1967,
        'Художественная литература',
        '<toc><h2>Глава 1. Никогда не разговаривайте с неизвестными</h2><p>Патриаршие пруды.</p><h2>Глава 2. Понтий Пилат</h2><p>Дворец Ирода.</p></toc>'
) AS v
WHERE NOT EXISTS (SELECT 1 FROM book);

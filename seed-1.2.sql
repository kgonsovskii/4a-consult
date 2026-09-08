CREATE TABLE IF NOT EXISTS T (
    Id       int          NOT NULL PRIMARY KEY,
    Code     varchar(50)  NULL,
    Name     varchar(100) NULL,
    StatusId int          NULL
);

INSERT INTO T (Id, Code, Name, StatusId)
SELECT v.Id, v.Code, v.Name, v.StatusId
FROM (VALUES
    (1, 'gargadgadfga',       'Запрос предложений 1', 45),
    (2, 'bsftrggdfgadfgdfat', 'Запрос предложений 2',  2),
    (3, 'gfadgdfsgdfsg',      'Запрос предложений 3', 45),
    (4, 'afgereaerffdgvdf',   'Запрос предложений 4',  3),
    (5, 'dgadfterdsgsdgad',   'Запрос предложений 5', 45),
    (6, 'argrgag',            'Запрос предложений 6',  2)
) AS v(Id, Code, Name, StatusId)
WHERE NOT EXISTS (SELECT 1 FROM T);

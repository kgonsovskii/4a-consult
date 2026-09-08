CREATE TABLE IF NOT EXISTS Purchase (
    Id       int          NOT NULL PRIMARY KEY,
    Code     varchar(50)  NULL,
    Name     varchar(100) NULL,
    StatusId int          NULL
);

INSERT INTO Purchase (Id, Code, Name, StatusId)
SELECT v.Id, v.Code, v.Name, v.StatusId
FROM (VALUES
    (1, 'SBR003-202001', 'Конкурс СМСП',        45),
    (2, 'SBR003-202002', 'Запрос предложений',  2)
) AS v(Id, Code, Name, StatusId)
WHERE NOT EXISTS (SELECT 1 FROM Purchase);

CREATE TABLE IF NOT EXISTS T1 (
    ID     int         NOT NULL PRIMARY KEY,
    Text1  varchar(50) NULL,
    Text2  varchar(50) NULL,
    B      boolean     NULL
);

CREATE TABLE IF NOT EXISTS T2 (
    ID     int         NOT NULL PRIMARY KEY,
    Text1  varchar(50) NULL,
    Text2  varchar(50) NULL,
    B      boolean     NULL
);

INSERT INTO T1 (ID, Text1, Text2, B)
SELECT v.ID, v.Text1, v.Text2, v.B
FROM (VALUES
    (1, 'A1', 'one',   true),
    (2, 'A2', 'two',   false),
    (3, 'A3', 'three', true)
) AS v(ID, Text1, Text2, B)
WHERE NOT EXISTS (SELECT 1 FROM T1);

INSERT INTO T2 (ID, Text1, Text2, B)
SELECT v.ID, v.Text1, v.Text2, v.B
FROM (VALUES
    (2, 'B2', 'two',   false),
    (3, 'B3', 'three', true),
    (4, 'B4', 'four',  false)
) AS v(ID, Text1, Text2, B)
WHERE NOT EXISTS (SELECT 1 FROM T2);

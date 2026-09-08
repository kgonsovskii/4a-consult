# Seed

<a id="seed"></a>

```sql
IF OBJECT_ID(N'dbo.T1', N'U') IS NULL
    CREATE TABLE dbo.T1 (
        ID int NOT NULL PRIMARY KEY,
        Text1 nvarchar(50) NULL,
        Text2 nvarchar(50) NULL,
        B bit NULL
    );

IF OBJECT_ID(N'dbo.T2', N'U') IS NULL
    CREATE TABLE dbo.T2 (
        ID int NOT NULL PRIMARY KEY,
        Text1 nvarchar(50) NULL,
        Text2 nvarchar(50) NULL,
        B bit NULL
    );

IF NOT EXISTS (SELECT 1 FROM dbo.T1)
    INSERT INTO dbo.T1 (ID, Text1, Text2, B) VALUES
        (1, N'A1', N'one',   1),
        (2, N'A2', N'two',   0),
        (3, N'A3', N'three', 1);

IF NOT EXISTS (SELECT 1 FROM dbo.T2)
    INSERT INTO dbo.T2 (ID, Text1, Text2, B) VALUES
        (2, N'B2', N'two',   0),
        (3, N'B3', N'three', 1),
        (4, N'B4', N'four',  0);
```

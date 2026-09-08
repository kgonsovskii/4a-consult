# Тестовое задание для компании «4А.Консалтинг»

**Исполнитель:** Гонсовский Константин

- HH: https://hh.ru/resume/40b8acccff064127520039ed1f3861676b6837?hhtmFrom=applicant_profile
- Telegram: [@KonstantinGonsovskii](https://t.me/KonstantinGonsovskii)

## 1. SQL — базовые знания

### 1.1 Вопрос на знание SELECT … JOIN

Есть две таблицы:

- `T1` (`ID int`, `Text1`, `Text2`, `B`, …)
- `T2` (`ID int`, `Text1`, `Text2`, `B`, …)

<div align="right"><small><a href="seed-1.1.sql">seed - 1.1.sql</a></small></div>

### 1. Вывести все поля из обеих таблиц, вывести записи при условии, что ID обеих таблиц совпадают.

```sql
SELECT T1.*, T2.*
FROM T1
INNER JOIN T2 ON T1.ID = T2.ID;
```

### 2. Вывести все поля из обеих таблиц, вывести все записи из T1 и только имеющиеся в T2.

```sql
SELECT T1.*, T2.*
FROM T1
LEFT JOIN T2 ON T1.ID = T2.ID;
```

### 3. Вывести все записи из T1, при условии, что таких ID нет в T2.

```sql
SELECT T1.*
FROM T1
LEFT JOIN T2 ON T1.ID = T2.ID
WHERE T2.ID IS NULL;
```

---

### 1.2 Как вывести результат запроса в XML?

Пусть есть таблица `T` со следующим видом и содержанием. Что вернет SQL запрос?

| Id | Code | Name | StatusId |
| ---: | --- | --- | ---: |
| 1 | gargadgadfga | Запрос предложений 1 | 45 |
| 2 | bsftrggdfgadfgdfat | Запрос предложений 2 | 2 |
| 3 | gfadgdfsgdfsg | Запрос предложений 3 | 45 |
| 4 | afgereaerffdgvdf | Запрос предложений 4 | 3 |
| 5 | dgadfterdsgsdgad | Запрос предложений 5 | 45 |
| 6 | argrgag | Запрос предложений 6 | 2 |

<div align="right"><small><a href="seed-1.2.sql">seed - 1.2.sql</a></small></div>

```sql
SELECT xmlelement(
    name root,
    xmlagg(
        xmlelement(
            name "T",
            xmlforest(
                Id AS "Id",
                Code AS "Code",
                Name AS "Name",
                StatusId AS "StatusId"
            )
        )
        ORDER BY Id
    )
)
FROM T;
```

```xml
<root>
  <T>
    <Id>1</Id>
    <Code>gargadgadfga</Code>
    <Name>Запрос предложений 1</Name>
    <StatusId>45</StatusId>
  </T>
  <T>
    <Id>2</Id>
    <Code>bsftrggdfgadfgdfat</Code>
    <Name>Запрос предложений 2</Name>
    <StatusId>2</StatusId>
  </T>
  <T>
    <Id>3</Id>
    <Code>gfadgdfsgdfsg</Code>
    <Name>Запрос предложений 3</Name>
    <StatusId>45</StatusId>
  </T>
  <T>
    <Id>4</Id>
    <Code>afgereaerffdgvdf</Code>
    <Name>Запрос предложений 4</Name>
    <StatusId>3</StatusId>
  </T>
  <T>
    <Id>5</Id>
    <Code>dgadfterdsgsdgad</Code>
    <Name>Запрос предложений 5</Name>
    <StatusId>45</StatusId>
  </T>
  <T>
    <Id>6</Id>
    <Code>argrgag</Code>
    <Name>Запрос предложений 6</Name>
    <StatusId>2</StatusId>
  </T>
</root>
```

---

### 1.3 Как выбрать данные из поля с XML?

Написать запрос, выбирающий данные из XML из предыдущего вопроса.

Отфильтровать данные по **StatusId != 3**.

```sql
WITH xml_data AS (
    SELECT xmlelement(
        name root,
        xmlagg(
            xmlelement(
                name "T",
                xmlforest(
                    Id AS "Id",
                    Code AS "Code",
                    Name AS "Name",
                    StatusId AS "StatusId"
                )
            )
            ORDER BY Id
        )
    ) AS data
    FROM T
)
SELECT
    x."Id",
    x."Code",
    x."Name",
    x."StatusId"
FROM xml_data,
     xmltable(
         '/root/T'
         PASSING data
         COLUMNS
             "Id"       int  PATH 'Id',
             "Code"     text PATH 'Code',
             "Name"     text PATH 'Name',
             "StatusId" int  PATH 'StatusId'
     ) AS x
WHERE x."StatusId" != 3;
```

---

### 1.4 Что такое hints

В SQL Server это `WITH (NOLOCK)`, `OPTION (RECOMPILE)`. В Postgres крутить планировщик: `SET enable_seqscan = off`, `enable_hashjoin`.

---

### 1.5 Какие виды блокировок существуют?

На чтение и на запись. Читать можно вдвоём, писать — уже нет: остальные ждут.

---

### 1.6 Что такое транзакция?

Пачка изменений, которая либо вся сохраняется (`COMMIT`), либо вся отменяется (`ROLLBACK`). Пока не закоммитили — для других её как будто нет (в обычном `READ COMMITTED`).

---

### 1.7 Чем DELETE отличается от TRUNCATE?

`DELETE` снимает строки, можно `WHERE`, срабатывают триггеры, каждая строка пишется в лог. `TRUNCATE` сразу опустошает таблицу: быстрее, без `WHERE` и без delete-триггеров. В транзакции оба можно откатить.

---

## 2. SQL — практические задачи

### 2.1 Напишите хранимую процедуру

Есть таблица `T` со счетами банка. Поля: `N` — номер счёта, `S` — сумма на счёте.

Написать процедуру: аргументы `@N1`, `@N2`, `@S`. Перевести сумму `@S` со счёта `@N1` на счёт `@N2`, проверить что на `@N1` хватает денег. Перевод обернуть в транзакцию.

<div align="right"><small><a href="seed-2.1.sql">seed - 2.1.sql</a></small></div>

```sql
CREATE OR REPLACE PROCEDURE transfer(n1 int, n2 int, amount numeric)
LANGUAGE plpgsql
AS $$
DECLARE
    bal numeric;
BEGIN
    SELECT S INTO bal FROM T WHERE N = n1 FOR UPDATE;

    IF bal IS NULL THEN
        RAISE EXCEPTION 'Нет счёта %', n1;
    END IF;

    IF NOT EXISTS (SELECT 1 FROM T WHERE N = n2 FOR UPDATE) THEN
        RAISE EXCEPTION 'Нет счёта %', n2;
    END IF;

    IF bal < amount THEN
        RAISE EXCEPTION 'Недостаточно средств';
    END IF;

    UPDATE T
    SET S = CASE N
        WHEN n1 THEN S - amount
        WHEN n2 THEN S + amount
    END
    WHERE N IN (n1, n2);
END;
$$;
```

<small>

```sql
BEGIN;
CALL transfer(1, 2, 50);
COMMIT;
```

</small>

---

### 2.2 Задача на вывод в XML

Пусть есть таблица `Purchase` со следующим видом и содержанием. Что вернет SQL запрос?

| Id | Code | Name | StatusId |
| ---: | --- | --- | ---: |
| 1 | SBR003-202001 | Конкурс СМСП | 45 |
| 2 | SBR003-202002 | Запрос предложений | 2 |

<div align="right"><small><a href="seed-2.2.sql">seed - 2.2.sql</a></small></div>

```sql
SELECT Id, Code, Name, StatusId
FROM Purchase
FOR XML PATH('row'), ROOT('data');
```

```xml
<data>
  <row>
    <Id>1</Id>
    <Code>SBR003-202001</Code>
    <Name>Конкурс СМСП</Name>
    <StatusId>45</StatusId>
  </row>
  <row>
    <Id>2</Id>
    <Code>SBR003-202002</Code>
    <Name>Запрос предложений</Name>
    <StatusId>2</StatusId>
  </row>
</data>
```

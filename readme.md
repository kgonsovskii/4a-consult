# Тестовое задание для компании «4А.Консалтинг»

**Автор:** Гонсовский Константин

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

```sql
SELECT xmlelement(
    name data,
    xmlagg(
        xmlelement(
            name "row",
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
FROM Purchase;
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

---

### 2.3 Задача на диагностику проблемы

Есть автоматическое задание, каждую минуту регистрирует электронный документ «Результаты торгов». При регистрации можно менять атрибуты в БД. Лоты выбираются так:

```sql
SELECT Bid.Id, Purchase.OrgBuId, Purchase.TypeId
FROM Bid
JOIN Purchase ON Purchase.Id = Bid.PurchaseId
WHERE Bid.StatusId = 6
```

Задание вечером перенесли с теста на бой. Утром оказалось: по всем лотам со `StatusId = 6` документ всю ночь создавался каждую минуту.

Почему так произошло и что надо исправить?

После регистрации статус остался 6. Задание каждую минуту снова берёт те же лоты.

Менять `Bid.StatusId` в той же транзакции, что и документ, или сразу брать лоты так:

```sql
UPDATE Bid
SET StatusId = 7
FROM Purchase
WHERE Purchase.Id = Bid.PurchaseId
  AND Bid.StatusId = 6
RETURNING Bid.Id, Purchase.OrgBuId, Purchase.TypeId;
```

---

## 3. Понимание базовых концепций программирования

### 3.1 Задача на валидность XML

Перед вами пример XML файла, но он не пройдет валидацию. Найдите все ошибки.

<div align="right"><small><a href="src/Chapter3.Topic1">src/Chapter3.Topic1</a></small></div>

```xml
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
```

- Несколько корневых элементов: `PurchaseInfo`, `BidInfo`, `RequestInfo`.
- `PurchaseName`: символ `<` не экранирован (`&lt;`).
- `TypeInfo` / `TypeName`: теги закрыты в неправильном порядке.
- `BidCurrency`: нет закрывающего тега.
- `BuId`: атрибут `AccessByOrganization` без кавычек.
- `RequestNo`: вместо `</RequestNo>` стоит открывающий `<RequestNo>`.

---

### 3.2 Поиск дублей в массиве

Дано: массив `M` типа `int32` из `N` элементов (`N` очень много). Компьютер с неограниченной памятью, пользоваться можно как угодно.

Вопрос: циклом за один проход определить, есть ли повторяющиеся элементы. Написать алгоритм или объяснить идею.

<div align="right"><small><a href="src/Chapter3.Topic2">src/Chapter3.Topic2</a></small></div>

```csharp
var seen = new HashSet<int>();
foreach (var value in M)
{
    if (!seen.Add(value))
        return true;
}
return false;
```

---

### 3.3 Опциональное задание (его выполнение будет Вашим большим преимуществом)

1. PostgreSQL вместо MS SQL Express.
2. База `library`: книги домашней библиотеки. Поля: название, автор, год, издательство, оглавление в XML.
3. Функции: `book_insert`, `book_update`, `book_delete`, `book_select`, `book_select_by_id`.
4. Два проекта: MVC и Web Forms.
5. Список, карточка, создание, правка, удаление — только через функции из п.3.
6. В карточке HTML-редактор оглавления, в базу уходит XML.
7. Выборка из XML: заголовки `h2` через `xmltable`.

При старте приложение само создаёт базу, таблицы, функции и данные.

<div align="right"><small><a href="src/Chapter3.Topic3.Domain">src/Chapter3.Topic3.Domain</a></small></div>
<div align="right"><small><a href="src/Chapter3.Topic3.Application">src/Chapter3.Topic3.Application</a></small></div>
<div align="right"><small><a href="src/Chapter3.Topic3.Infrastructure">src/Chapter3.Topic3.Infrastructure</a></small></div>
<div align="right"><small><a href="src/Chapter3.Topic3.Mvc">src/Chapter3.Topic3.Mvc</a></small></div>
<div align="right"><small><a href="src/Chapter3.Topic3.WebForms">src/Chapter3.Topic3.WebForms</a></small></div>

```sql
SELECT b.title, t.heading
FROM book b,
     xmltable('//h2' PASSING b.toc COLUMNS heading text PATH '.') AS t;
```

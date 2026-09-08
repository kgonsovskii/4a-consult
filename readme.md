# Тестовое задание для компании «4А.Консалтинг»

**Исполнитель:** Гонсовский Константин

- HH: https://hh.ru/resume/40b8acccff064127520039ed1f3861676b6837?hhtmFrom=applicant_profile
- Telegram: [@KonstantinGonsovskii](https://t.me/KonstantinGonsovskii)

## 1. SQL — базовые знания

### 1.1 Вопрос на знание SELECT … JOIN

Есть две таблицы:

- `T1` (`ID int`, `Text1`, `Text2`, `B`, …)
- `T2` (`ID int`, `Text1`, `Text2`, `B`, …)

### 1. Все поля из обеих таблиц, ID совпадают

Нужны только строки, у которых есть пара в обеих таблицах — `INNER JOIN`.

```sql
SELECT T1.*, T2.*
FROM T1
INNER JOIN T2 ON T1.ID = T2.ID;
```

Эквивалентная запись: `JOIN` без слова `INNER`.

### 2. Все поля из обеих таблиц, все записи из T1 и только имеющиеся в T2

Нужны все строки `T1`. Если в `T2` нет такого `ID`, поля `T2` будут `NULL` — `LEFT JOIN`.

```sql
SELECT T1.*, T2.*
FROM T1
LEFT JOIN T2 ON T1.ID = T2.ID;
```

### 3. Все записи из T1, у которых такого ID нет в T2

Нужны строки `T1` без пары в `T2` — anti-join: `LEFT JOIN` и отсев совпадений по `T2.ID IS NULL`.

```sql
SELECT T1.*
FROM T1
LEFT JOIN T2 ON T1.ID = T2.ID
WHERE T2.ID IS NULL;
```

Другой вариант — `NOT EXISTS` (удобно, если в `T2.ID` могут быть `NULL`):

```sql
SELECT T1.*
FROM T1
WHERE NOT EXISTS (
    SELECT 1
    FROM T2
    WHERE T2.ID = T1.ID
);
```

### 1.2 Как вывести результат запроса в XML?

В SQL Server результат `SELECT` превращается в XML предложением `FOR XML`.

Пусть есть таблица `T`:

| Id | Code | Name | StatusId |
| ---: | --- | --- | ---: |
| 1 | gargadgadfga | Запрос предложений 1 | 45 |
| 2 | bsftrggdfgadfgdfat | Запрос предложений 2 | 2 |
| 3 | gfadgdfsgdfsg | Запрос предложений 3 | 45 |
| 4 | afgereaerffdgvdf | Запрос предложений 4 | 3 |
| 5 | dgadfterdsgsdgad | Запрос предложений 5 | 45 |
| 6 | argrgag | Запрос предложений 6 | 2 |

`FOR XML AUTO` — элемент по имени таблицы, столбцы как атрибуты:

```sql
SELECT Id, Code, Name, StatusId
FROM T
FOR XML AUTO, ROOT('root');
```

```xml
<root>
  <T Id="1" Code="gargadgadfga" Name="Запрос предложений 1" StatusId="45" />
  <T Id="2" Code="bsftrggdfgadfgdfat" Name="Запрос предложений 2" StatusId="2" />
  <T Id="3" Code="gfadgdfsgdfsg" Name="Запрос предложений 3" StatusId="45" />
  <T Id="4" Code="afgereaerffdgvdf" Name="Запрос предложений 4" StatusId="3" />
  <T Id="5" Code="dgadfterdsgsdgad" Name="Запрос предложений 5" StatusId="45" />
  <T Id="6" Code="argrgag" Name="Запрос предложений 6" StatusId="2" />
</root>
```

`FOR XML PATH` — столбцы как вложенные элементы (имя строки задаётся в `PATH`):

```sql
SELECT Id, Code, Name, StatusId
FROM T
FOR XML PATH('T'), ROOT('root');
```

```xml
<root>
  <T>
    <Id>1</Id>
    <Code>gargadgadfga</Code>
    <Name>Запрос предложений 1</Name>
    <StatusId>45</StatusId>
  </T>
  <!-- … остальные строки аналогично … -->
</root>
```

`FOR XML RAW` даёт элемент `<row>` с атрибутами. Без `ROOT` SQL Server возвращает фрагмент XML, не документ с одним корнем.

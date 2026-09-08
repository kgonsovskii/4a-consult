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

CREATE TABLE IF NOT EXISTS book (
    id        INTEGER PRIMARY KEY AUTOINCREMENT,
    title     TEXT    NOT NULL,
    author    TEXT    NOT NULL,
    year      INTEGER NOT NULL,
    publisher TEXT    NOT NULL,
    toc       TEXT    NOT NULL DEFAULT '<toc/>'
);

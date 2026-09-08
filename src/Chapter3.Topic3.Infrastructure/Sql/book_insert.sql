INSERT INTO book (title, author, year, publisher, toc)
VALUES (@title, @author, @year, @publisher, @toc)
RETURNING *;

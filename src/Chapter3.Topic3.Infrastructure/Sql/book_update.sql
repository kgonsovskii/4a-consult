UPDATE book
SET title = @title,
    author = @author,
    year = @year,
    publisher = @publisher,
    toc = @toc
WHERE id = @id
RETURNING *;

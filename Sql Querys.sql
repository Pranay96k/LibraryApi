SELECT TOP (5) 
b.BookId, b.Title, b.Author, COUNT(br.BorrowId) AS BorrowCount
FROM BorrowRecords br
JOIN Books b ON br.BookId = b.BookId
GROUP BY b.BookId, b.Title, b.Author
ORDER BY BorrowCount DESC;

SELECT m.MemberId, m.Name, m.Email, COUNT(br.BorrowId) AS BorrowCount
FROM BorrowRecords br
JOIN Members m ON br.MemberId = m.MemberId
WHERE br.BorrowDate >= DATEADD(MONTH, -1, GETDATE())
GROUP BY m.MemberId, m.Name, m.Email
HAVING COUNT(br.BorrowId) > 3;


SELECT br.BorrowId, b.BookId, b.Title, b.Author, br.BorrowDate, br.ReturnDate
FROM BorrowRecords br
JOIN Books b ON br.BookId = b.BookId
WHERE br.MemberId = 1
  AND br.IsReturned = 0;




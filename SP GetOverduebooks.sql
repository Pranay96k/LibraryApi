CREATE PROCEDURE sp_GetOverdueBooks
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        br.BorrowId,
        br.MemberId,
        m.Name AS MemberName,
        m.Email,
        br.BookId,
        b.Title AS BookTitle,
        br.BorrowDate,
        DATEADD(day,14,br.BorrowDate) AS DueDate,
        DATEDIFF(day, DATEADD(day,14,br.BorrowDate), GETDATE()) AS DaysOverdue
    FROM BorrowRecords br
    JOIN Members m ON br.MemberId = m.MemberId
    JOIN Books b ON br.BookId = b.BookId
    WHERE br.IsReturned = 0
      AND DATEADD(day, 14, br.BorrowDate) < GETDATE()
    ORDER BY DueDate ASC;
END;
GO

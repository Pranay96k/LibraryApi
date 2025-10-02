CREATE PROCEDURE sp_GetOverdueBooks
  @MarkReturned BIT = 0
AS
BEGIN
  SET NOCOUNT ON;

  IF @MarkReturned = 0
  BEGIN
    SELECT br.BorrowId, br.MemberId, m.Name AS MemberName, m.Email, br.BookId, b.Title AS BookTitle, br.BorrowDate,
DATEADD(DAY, 14, br.BorrowDate) AS DueDate, DATEDIFF(DAY, DATEADD(DAY, 14, br.BorrowDate), GETDATE()) AS DaysOverdue
    FROM BorrowRecords br
    JOIN Members m ON br.MemberId = m.MemberId
    JOIN Books b ON br.BookId = b.BookId
    WHERE br.IsReturned = 0
      AND DATEADD(DAY, 14, br.BorrowDate) < GETDATE()
    ORDER BY DaysOverdue DESC;
  END
  ELSE
  BEGIN
    UPDATE br
    SET br.IsReturned = 1, br.ReturnDate = GETDATE()
    FROM BorrowRecords br
    WHERE br.IsReturned = 0
      AND DATEADD(DAY, 14, br.BorrowDate) < GETDATE();

    SELECT @@ROWCOUNT AS RowsUpdated;
  END
END
GO
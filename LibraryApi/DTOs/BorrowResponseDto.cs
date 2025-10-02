namespace LibraryApi.DTOs
{
    public class BorrowResponseDto
    {
        public int BorrowId { get; set; }
        public int BookId { get; set; }
        public int MemberId { get; set; }
        public DateTime BorrowDate { get; set; }
        public bool IsReturned { get; set; }

    }
}

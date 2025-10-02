using System.ComponentModel.DataAnnotations;

namespace LibraryApi.Models
{
    public class BorrowRecord
    {
        [Key]
        public int BorrowId { get; set; }
        public int MemberId { get; set; }
        public Member? Member { get; set; }
        public int BookId { get; set; }
        public Book? Book { get; set; }
        public DateTime BorrowDate { get; set; } = DateTime.UtcNow;
        public DateTime? ReturnDate { get; set; }
        public bool IsReturned { get; set; } = false;

    }
}

using System.ComponentModel.DataAnnotations;

namespace LibraryApi.DTOs
{
    public class BorrowDto
    {
        [Required]
        public int MemberId { get; set; }

        [Required]
        public int BookId { get; set; }

    }
}

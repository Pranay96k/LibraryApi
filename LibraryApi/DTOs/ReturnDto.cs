using System.ComponentModel.DataAnnotations;

namespace LibraryApi.DTOs
{
    public class ReturnDto
    {
        [Required]
        public int BorrowId { get; set; }
    }
}

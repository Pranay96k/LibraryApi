using System.ComponentModel.DataAnnotations;

namespace LibraryApi.DTOs
{
    public class CreateBookDto
    {
        [Required]
        public string Title { get; set; } = null!;

        [Required]
        public string Author { get; set; } = null!;

        [Required]
        [StringLength(50)]
        public string ISBN { get; set; } = null!;

        public int? PublishedYear { get; set; }

        [Range(0, int.MaxValue)]
        public int AvailableCopies { get; set; } = 0;
    }
}

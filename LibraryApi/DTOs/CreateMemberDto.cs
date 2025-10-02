using System.ComponentModel.DataAnnotations;

namespace LibraryApi.DTOs
{
    public class CreateMemberDto
    {
        [Required]
        public string Name { get; set; } = null!;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        public string? Phone { get; set; }

        [Required, MinLength(6)]
        public string Password { get; set; } = null!;
    }
}

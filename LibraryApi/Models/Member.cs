namespace LibraryApi.Models
{
    public class Member
    {
        public int MemberId { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? Phone { get; set; }
        public DateTime JoinDate { get; set; } = DateTime.UtcNow;
        public string PasswordHash { get; set; } = null!;
        public ICollection<BorrowRecord>? BorrowRecords { get; set; }

    }
}

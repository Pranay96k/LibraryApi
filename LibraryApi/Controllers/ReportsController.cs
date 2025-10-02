using LibraryApi.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly LibraryContext _context;
        public ReportsController(LibraryContext context) => _context = context;

        [HttpGet("top-borrowed")]
        public async Task<IActionResult> GetTopBorrowed()
        {
            var top = await _context.BorrowRecords
                .GroupBy(br => br.BookId)
                .Select(g => new
                {
                    BookId = g.Key,
                    BorrowCount = g.Count()
                })
                .OrderByDescending(x => x.BorrowCount)
                .Take(5)
                .Join(_context.Books, t => t.BookId, b => b.BookId, (t, b) => new
                {
                    b.BookId,
                    b.Title,
                    b.Author,
                    t.BorrowCount
                }).ToListAsync();

            return Ok(top);
        }

        [HttpGet("overdue")]
        public async Task<IActionResult> GetOverdue()
        {
            var cutoff = DateTime.UtcNow.AddDays(-14);

            var overdueRecords = await _context.BorrowRecords
                .Where(b => !b.IsReturned && b.BorrowDate <= cutoff)
                .Include(b => b.Member)
                .Include(b => b.Book)
                .ToListAsync();

            var result = overdueRecords
                .Select(b => new
                {
                    b.BorrowId,
                    MemberName = b.Member.Name,
                    BookTitle = b.Book.Title,
                    BorrowDate = b.BorrowDate,
                    DaysOverdue = (DateTime.UtcNow - b.BorrowDate.AddDays(14)).Days
                })
                .OrderByDescending(x => x.DaysOverdue)
                .ToList();

            return Ok(result);
        }

    }
}

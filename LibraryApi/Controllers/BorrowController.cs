using LibraryApi.Data;
using LibraryApi.DTOs;
using LibraryApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryApi.Controllers
{
    [Authorize]
    [Route("api")]
    [ApiController]
    public class BorrowController : ControllerBase
    {
        private readonly LibraryContext _context;
        public BorrowController(LibraryContext context) => _context = context;

        [HttpPost("borrow")]
        public async Task<IActionResult> Borrow([FromBody] BorrowDto dto)
        {

            var memberIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(memberIdClaim, out var tokenMemberId))
                return Forbid();

            if (!ModelState.IsValid) return BadRequest(ModelState);

            var member = await _context.Members.FindAsync(dto.MemberId);
            if (member == null) return NotFound(new { message = "Member not found." });

            var book = await _context.Books.FindAsync(dto.BookId);
            if (book == null) return NotFound(new { message = "Book not found." });

            if (book.AvailableCopies <= 0) return BadRequest(new { message = "No copies available." });

            using var tx = await _context.Database.BeginTransactionAsync();
            try
            {
                book.AvailableCopies -= 1;
                var borrow = new BorrowRecord
                {
                    BookId = book.BookId,
                    MemberId = member.MemberId,
                    BorrowDate = DateTime.UtcNow,
                    IsReturned = false
                };
                _context.BorrowRecords.Add(borrow);
                await _context.SaveChangesAsync();
                await tx.CommitAsync();

                var response = new BorrowResponseDto
                {
                    BorrowId = borrow.BorrowId,
                    BookId = borrow.BookId,
                    MemberId = borrow.MemberId,
                    BorrowDate = borrow.BorrowDate,
                    IsReturned = borrow.IsReturned
                };

                return CreatedAtAction(nameof(Borrow), new { id = borrow.BorrowId }, response);
            }
            catch
            {
                await tx.RollbackAsync();
                throw;
            }
        }

        [HttpPost("return")]
        public async Task<IActionResult> ReturnBook([FromBody] ReturnDto dto)
        {
            var borrow = await _context.BorrowRecords
                           .Include(br => br.Book)
                           .FirstOrDefaultAsync(br => br.BorrowId == dto.BorrowId);

            if (borrow == null) return NotFound(new { message = "Borrow record not found." });
            if (borrow.IsReturned) return BadRequest(new { message = "Already returned." });

            borrow.IsReturned = true;
            borrow.ReturnDate = DateTime.UtcNow;
            if (borrow.Book != null) borrow.Book.AvailableCopies += 1;


            var response = new ReturnResponseDto
            {
                   BorrowId = borrow.BorrowId
            };
            await _context.SaveChangesAsync();
            return Ok(response);
        }
    }
}

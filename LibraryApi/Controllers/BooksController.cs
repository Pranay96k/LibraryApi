using LibraryApi.Data;
using LibraryApi.DTOs;
using LibraryApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly LibraryContext _context;
        public BooksController(LibraryContext context) => _context = context;

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateBookDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            // Check unique ISBN
            if (await _context.Books.AnyAsync(b => b.ISBN == dto.ISBN))
                return Conflict(new { message = "ISBN already exists." });

            var book = new Book
            {
                Title = dto.Title,
                Author = dto.Author,
                ISBN = dto.ISBN,
                PublishedYear = dto.PublishedYear,
                AvailableCopies = dto.AvailableCopies
            };

            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = book.BookId }, book);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _context.Books
                .Select(b => new {
                    b.BookId,
                    b.Title,
                    b.Author,
                    b.ISBN,
                    b.PublishedYear,
                    b.AvailableCopies,
                    IsAvailable = b.AvailableCopies > 0
                }).ToListAsync();

            return Ok(list);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null) return NotFound();
            return Ok(book);
        }

    }
}

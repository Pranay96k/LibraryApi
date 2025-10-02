using LibraryApi.Data;
using LibraryApi.DTOs;
using LibraryApi.Models;
using LibraryApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MembersController : ControllerBase
    {
        private readonly LibraryContext _context;
        private readonly IPasswordHasher<Member> _passwordHasher;
        public MembersController(LibraryContext context, IPasswordHasher<Member> passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateMemberDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (await _context.Members.AnyAsync(m => m.Email == dto.Email))
                return Conflict(new { message = "Email already registered." });

            var member = new Member
            {
                Name = dto.Name,
                Email = dto.Email,
                Phone = dto.Phone,
                JoinDate = DateTime.UtcNow
            };

            member.PasswordHash = _passwordHasher.HashPassword(member, dto.Password);

            _context.Members.Add(member);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = member.MemberId }, member);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var member = await _context.Members.FindAsync(id);
            if (member == null) return NotFound();
            return Ok(member);
        }
    }
}

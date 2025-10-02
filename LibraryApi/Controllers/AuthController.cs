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
    public class AuthController : ControllerBase
    {
        private readonly LibraryContext _context;
        private readonly ITokenService _tokenService;
        private readonly IPasswordHasher<Member> _passwordHasher;

        public AuthController(LibraryContext context, ITokenService tokenService, IPasswordHasher<Member> passwordHasher)
        {
            _context = context;
            _tokenService = tokenService;
            _passwordHasher = passwordHasher;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var member = await _context.Members.SingleOrDefaultAsync(m => m.Email == dto.Email);
            if (member == null) return Unauthorized(new { message = "Invalid credentials." });

            var verifyResult = _passwordHasher.VerifyHashedPassword(member, member.PasswordHash, dto.Password);
            if (verifyResult == PasswordVerificationResult.Failed)
                return Unauthorized(new { message = "Invalid credentials." });

            var token = _tokenService.CreateToken(member);
            return Ok(new { token });
        }
    }
}

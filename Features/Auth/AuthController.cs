using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JoinBackendDotnet.Data;
using JoinBackendDotnet.Models;
using System.Security.Cryptography;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace JoinBackendDotnet.Features.Auth
{
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AuthController(AppDbContext context)
        {
            _context = context;
        }

        // POST: /register
        [HttpPost]
        [Route("register")]
        public async Task<ActionResult> Register(UserRegisterDto dto)
        {
            if (!new EmailAddressAttribute().IsValid(dto.Email))
                return BadRequest(new { message = "Invalid email format." });

            if (await _context.Users.AnyAsync(u => u.Username == dto.Username))
                return BadRequest(new { message = "This username already exists." });

            if (await _context.Users.AnyAsync(u => u.Email == dto.Email))
                return BadRequest(new { message = "This email already exists." });

            var hashedPassword = HashPassword(dto.Password);

            var user = new User
            {
                Username = dto.Username,
                Email = dto.Email,
                PasswordHash = hashedPassword
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new {
                username = dto.Username,
                email = dto.Email
            });

        }

        // POST: /login
        [HttpPost]
        [Route("login")]
        public async Task<ActionResult> Login(UserLoginDto dto)
        {
            var hashed = HashPassword(dto.Password);

            var user = await _context.Users.FirstOrDefaultAsync(u =>
                (u.Username == dto.UsernameOrEmail || u.Email == dto.UsernameOrEmail) &&
                u.PasswordHash == hashed);

            if (user == null)
                return Unauthorized(new { message = "Invalid credentials" });

            var token = await _context.Tokens.FirstOrDefaultAsync(t => t.UserId == user.Id);
            if (token == null)
            {
                token = new AuthToken
                {
                    UserId = user.Id,
                    Token = GenerateToken()
                };
                _context.Tokens.Add(token);
                await _context.SaveChangesAsync();
            }

            return Ok(new
            {
                token = token.Token,
                user_id = user.Id,
                email = user.Email,
                name = user.Username
            });
        }

        // POST: /guest-login
        [HttpPost]
        [Route("guest_login")]
        public async Task<ActionResult> GuestLogin()
        {
            var guest = await _context.Users.FirstOrDefaultAsync(u => u.Username == "Guest");
            if (guest == null)
            {
                guest = new User
                {
                    Username = "Guest",
                    Email = "guest@example.com",
                    PasswordHash = HashPassword("guest")
                };
                _context.Users.Add(guest);
                await _context.SaveChangesAsync();
            }

            var token = await _context.Tokens.FirstOrDefaultAsync(t => t.UserId == guest.Id);
            if (token == null)
            {
                token = new AuthToken
                {
                    UserId = guest.Id,
                    Token = GenerateToken()
                };
                _context.Tokens.Add(token);
                await _context.SaveChangesAsync();
            }

            return Ok(new
            {
                token = token.Token,
                user_id = guest.Id,
                email = guest.Email,
                name = guest.Username
            });
        }

        private string HashPassword(string password)
        {
            using var sha = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        private string GenerateToken()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
        }
    }
}
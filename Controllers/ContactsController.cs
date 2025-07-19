using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JoinBackendDotnet.Data;
using JoinBackendDotnet.Models;
using JoinBackendDotnet.DTOs;

namespace JoinBackendDotnet.Controllers
{
    [ApiController]
    [Route("contacts")]
    public class ContactsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ContactsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: /contacts
        [HttpGet]
        public async Task<ActionResult<List<ContactResponseDto>>> GetContacts([FromHeader(Name = "Authorization")] string authHeader)
        {
            var user = await GetUserFromToken(authHeader);
            if (user == null) return Unauthorized();

            var contacts = await _context.Contacts
                .Where(c => c.UserId == user.Id)
                .ToListAsync();

            var dtoList = contacts.Select(MapToDto).ToList();
            return Ok(dtoList);
        }

        // POST: /contacts
        [HttpPost]
        public async Task<ActionResult<ContactResponseDto>> CreateContact(
            [FromHeader(Name = "Authorization")] string authHeader,
            [FromBody] ContactCreateDto dto)
        {
            var user = await GetUserFromToken(authHeader);
            if (user == null) return Unauthorized();

            if (_context.Contacts.Any(c => c.Email == dto.Email))
                return BadRequest(new { email = "This email already exists." });

            var contact = new Contact
            {
                Name = dto.Name,
                Email = dto.Email,
                Phone = dto.Phone,
                BgColor = GetRandomColor(),
                UserId = user.Id
            };

            _context.Contacts.Add(contact);
            await _context.SaveChangesAsync();
            var response = new List<ContactCreateUpdateResponseDto>
            {
                new ContactCreateUpdateResponseDto
                {
                    Pk = contact.Id,
                    Fields = new ContactCreateUpdateFields
                    {
                        Name = contact.Name,
                        Email = contact.Email,
                        Phone = contact.Phone,
                        Bg_Color = GetColorCode(contact.BgColor),
                        User = contact.UserId
                    }
                }
            };

            return Ok(response);
        }


        // PUT: /contacts/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateContact(
            int id,
            [FromBody] ContactUpdateDto dto,
            [FromHeader(Name = "Authorization")] string authHeader)
        {
            var user = await GetUserFromToken(authHeader);
            if (user == null) return Unauthorized();

            var contact = await _context.Contacts.FirstOrDefaultAsync(c => c.Id == id && c.UserId == user.Id);
            if (contact == null) return NotFound();

            // ✅ E-Mail darf nur verwendet werden, wenn sie eindeutig oder gleich bleibt
            if (!string.IsNullOrEmpty(dto.Email) && dto.Email != contact.Email)
            {
                if (await _context.Contacts.AnyAsync(c => c.Email == dto.Email && c.Id != id))
                {
                    return BadRequest(new { email = "This email already exists." });
                }
            }

            contact.Name = dto.Name ?? contact.Name;
            contact.Email = dto.Email ?? contact.Email;
            contact.Phone = dto.Phone ?? contact.Phone;

            await _context.SaveChangesAsync();

            var response = new List<ContactCreateUpdateResponseDto>
            {
                new ContactCreateUpdateResponseDto
                {
                    Pk = contact.Id,
                    Fields = new ContactCreateUpdateFields
                    {
                        Name = contact.Name,
                        Email = contact.Email,
                        Phone = contact.Phone,
                        Bg_Color = GetColorCode(contact.BgColor),
                        User = contact.UserId
                    }
                }
            };

            return Ok(response);
        }

        // DELETE: /contacts/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContact(int id, [FromHeader(Name = "Authorization")] string authHeader)
        {
            var user = await GetUserFromToken(authHeader);
            if (user == null) return Unauthorized();

            var contact = await _context.Contacts.FirstOrDefaultAsync(c => c.Id == id && c.UserId == user.Id);
            if (contact == null) return NotFound();

            _context.Contacts.Remove(contact);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // 🔐 Token-Handling (einfach wie in AuthController)
        private async Task<User?> GetUserFromToken(string authHeader)
        {
            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Token ")) return null;

            var token = authHeader.Substring("Token ".Length);
            var tokenRecord = await _context.Tokens.Include(t => t.User).FirstOrDefaultAsync(t => t.Token == token);
            return tokenRecord?.User;
        }

        // 🎨 Random-Farbe
        private static BgColor GetRandomColor()
        {
            var values = Enum.GetValues<BgColor>();
            return values[new Random().Next(values.Length)];
        }

        // 🎨 Enum zu Hex-Code
        private static string GetColorCode(BgColor color)
        {
            return color switch
            {
                BgColor.Color1 => "#FF7A00",
                BgColor.Color2 => "#462F8A",
                BgColor.Color3 => "#FFBB2B",
                BgColor.Color4 => "#FC71FF",
                BgColor.Color5 => "#6E52FF",
                BgColor.Color6 => "#1FD7C1",
                BgColor.Color7 => "#9327FF",
                BgColor.Color8 => "#FF4646",
                _ => "#FF7A00"
            };
        }

        // 🧠 DTO Mapping
        private static ContactResponseDto MapToDto(Contact contact)
        {
            return new ContactResponseDto
            {
                Id = contact.Id,
                Name = contact.Name,
                Email = contact.Email,
                Phone = contact.Phone,
                BgColor = GetColorCode(contact.BgColor),
                UserId = contact.UserId
            };
        }
    }
}

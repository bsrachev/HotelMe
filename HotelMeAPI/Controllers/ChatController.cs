using HotelMe.Shared.Models;
using HotelMeAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HotelMeAPI.Controllers
{
    [ApiController]
    [Route("api/chat")]
    public class ChatController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly HotelMeContext _context;

        public ChatController(UserManager<ApplicationUser> userManager, HotelMeContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendMessage([FromBody] string message)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            var chatMessage = new ChatMessage
            {
                UserId = user.Id,
                Message = message,
                SentAt = DateTime.UtcNow
            };

            _context.ChatMessages.Add(chatMessage);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Message sent!" });
        }
    }
}

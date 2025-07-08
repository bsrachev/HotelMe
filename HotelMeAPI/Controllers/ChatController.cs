using HotelMe.Shared.Models;
using HotelMeAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelMeAPI.Controllers
{
    [ApiController]
    [Route("api/chat")]
    public class ChatController : ControllerBase
    {
        private readonly HotelMeContext _context;
        public ChatController(HotelMeContext context)
        {
            _context = context;
        }

        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<ChatMessage>>> GetAllMessages()
        {
            // project ChatMessage including RoomNumber
            var list = await _context.ChatMessages
                .Select(cm => new ChatMessage
                {
                    Id = cm.Id,
                    UserId = cm.UserId,
                    Message = cm.Message,
                    SentAt = cm.SentAt,
                    HasActiveBooking = _context.Bookings
                                           .Any(b => b.UserId == cm.UserId && b.Status == "CheckedIn"),
                    // pull the checked-in booking’s room number, if any
                    RoomNumber = _context.Bookings
                                           .Where(b => b.UserId == cm.UserId && b.Status == "CheckedIn")
                                           .Select(b => b.RoomNumber)
                                           .FirstOrDefault()
                })
                .ToListAsync();

            return Ok(list);
        }



        // POST /api/chat/send
        [HttpPost("send")]
        public async Task<IActionResult> SendMessage([FromBody] string message)
        {
            var userId = Request.Headers["X-User-Id"].ToString() ?? "demo-user";

            var chatMessage = new ChatMessage
            {
                UserId = userId,
                Message = message,
                SentAt = DateTime.UtcNow
            };

            _context.ChatMessages.Add(chatMessage);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Message sent!" });
        }
    }
}

using HotelMe.Shared.Models;
using HotelMeAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelMeAPI.Controllers
{
    [ApiController]
    [Route("api/bookings")]
    public class BookingsController : ControllerBase
    {
        private readonly HotelMeContext _context;
        public BookingsController(HotelMeContext context)
        {
            _context = context;
        }

        // GET /api/bookings/user
        [HttpGet("user")]
        public async Task<ActionResult<Booking>> GetUserBooking()
        {
            var userId = Request.Headers["X-User-Id"].ToString();
            if (string.IsNullOrEmpty(userId))
                return BadRequest("Missing X-User-Id");

            var booking = await _context.Bookings
                .FirstOrDefaultAsync(b => b.UserId == userId && b.Status != "CheckedOut");
            return booking == null ? NotFound() : Ok(booking);
        }

        // POST /api/bookings
        [HttpPost]
        public async Task<ActionResult<Booking>> CreateBooking([FromBody] BookingRequest req)
        {
            var userId = Request.Headers["X-User-Id"].ToString() ?? "demo-user";

            var booking = new Booking
            {
                UserId = userId,
                RoomNumber = req.RoomNumber,
                CheckInDate = req.CheckInDate,
                CheckOutDate = req.CheckOutDate,
                Status = "Pending"
            };

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUserBooking), new { id = booking.Id }, booking);
        }

        // POST /api/bookings/check-in
        [HttpPost("check-in")]
        public async Task<IActionResult> CheckIn()
        {
            var userId = Request.Headers["X-User-Id"].ToString() ?? "demo-user";

            var booking = await _context.Bookings
                .FirstOrDefaultAsync(b => b.UserId == userId && b.Status == "Pending");
            if (booking == null) return NotFound();

            booking.Status = "CheckedIn";
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // POST /api/bookings/check-out
        [HttpPost("check-out")]
        public async Task<IActionResult> CheckOut()
        {
            var userId = Request.Headers["X-User-Id"].ToString() ?? "demo-user";

            var booking = await _context.Bookings
                .FirstOrDefaultAsync(b => b.UserId == userId && b.Status == "CheckedIn");
            if (booking == null) return NotFound();

            booking.Status = "CheckedOut";
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}

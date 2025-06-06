﻿using HotelMe.Shared.Models;
using HotelMeAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelMeAPI.Controllers
{
    [ApiController]
    [Route("api/bookings")]
    public class BookingsController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly HotelMeContext _context;

        public BookingsController(UserManager<ApplicationUser> userManager, HotelMeContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        [HttpGet("user")]
        public async Task<ActionResult<Booking>> GetUserBooking()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            var booking = await _context.Bookings
                .FirstOrDefaultAsync(b => b.UserId == user.Id && b.Status != "CheckedOut");

            return booking ?? (ActionResult<Booking>)NotFound();
        }

        [HttpPost]
        public async Task<ActionResult<Booking>> CreateBooking([FromBody] BookingRequest req)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            // optionally: check for conflicts, etc.
            var booking = new Booking
            {
                UserId = user.Id,
                RoomNumber = req.RoomNumber,
                CheckInDate = req.CheckInDate,
                CheckOutDate = req.CheckOutDate,
                Status = "Pending"
            };

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUserBooking), new { id = booking.Id }, booking);
        }

        [HttpPost("check-in")]
        public async Task<IActionResult> CheckIn()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            var booking = await _context.Bookings.FirstOrDefaultAsync(b => b.UserId == user.Id && b.Status == "Pending");
            if (booking == null) return NotFound();

            booking.Status = "CheckedIn";
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost("check-out")]
        public async Task<IActionResult> CheckOut()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            var booking = await _context.Bookings.FirstOrDefaultAsync(b => b.UserId == user.Id && b.Status == "CheckedIn");
            if (booking == null) return NotFound();

            booking.Status = "CheckedOut";
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }

}

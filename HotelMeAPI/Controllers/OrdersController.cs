using HotelMe.Shared.Models;
using HotelMeAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelMeAPI.Controllers
{
    [ApiController]
    [Route("api/orders")]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly HotelMeContext _context;

        public OrdersController(UserManager<ApplicationUser> userManager, HotelMeContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        [HttpGet("ping")]
        public IActionResult Ping() => Ok("pong");

        [HttpGet("user")]
        public async Task<ActionResult<IEnumerable<Order>>> GetUserOrders()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            var orders = await _context.Orders
                .Where(o => o.UserId == user.Id)
                .Include(o => o.Items)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();

            return Ok(orders);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Order>> GetById(int id)
        {
            var order = await _context.Orders
                .Include(o => o.Items)
                .FirstOrDefaultAsync(o => o.Id == id);

            return order == null ? NotFound() : Ok(order);
        }

        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder([FromBody] OrderRequest req)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            var order = new Order
            {
                UserId = user.Id,
                CreatedAt = DateTime.UtcNow,
                Status = "Pending"
            };
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            foreach (var menuId in req.Items)
            {
                var menu = await _context.MenuItems.FindAsync(menuId);
                if (menu == null) continue;

                _context.OrderItems.Add(new OrderItem
                {
                    OrderId = order.Id,
                    MenuItemId = menu.Id,
                    MenuItemName = menu.Name,
                    Price = menu.Price,
                    Quantity = 1
                });
            }
            await _context.SaveChangesAsync();

            order = await _context.Orders
                .Include(o => o.Items)
                .FirstAsync(o => o.Id == order.Id);

            return CreatedAtAction(nameof(GetById), new { id = order.Id }, order);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetAllOrders()
        {
            var orders = await _context.Orders
                .Include(o => o.Items)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();

            return Ok(orders);
        }
    }
}

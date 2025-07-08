using HotelMe.Shared.Models;
using HotelMeAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelMeAPI.Controllers
{
    [ApiController]
    [Route("api/orders")]
    public class OrdersController : ControllerBase
    {
        private readonly HotelMeContext _db;
        public OrdersController(HotelMeContext db)
        {
            _db = db;
        }

        [HttpGet("ping")]
        public IActionResult Ping() => Ok("pong");

        // POST /api/orders
        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder([FromBody] OrderRequest req)
        {
            var userId = Request.Headers["X-User-Id"].ToString() ?? "demo-user";

            var order = new Order
            {
                UserId = userId,
                CreatedAt = DateTime.UtcNow,
                Status = "Pending"
            };
            _db.Orders.Add(order);
            await _db.SaveChangesAsync();

            foreach (var menuId in req.Items)
            {
                var menu = await _db.MenuItems.FindAsync(menuId);
                if (menu == null) continue;
                _db.OrderItems.Add(new OrderItem
                {
                    OrderId = order.Id,
                    MenuItemId = menu.Id,
                    MenuItemName = menu.Name,
                    Price = menu.Price,
                    Quantity = 1
                });
            }
            await _db.SaveChangesAsync();

            order = await _db.Orders
                .Include(o => o.Items)
                .FirstAsync(o => o.Id == order.Id);

            return CreatedAtAction(nameof(GetById), new { id = order.Id }, order);
        }

        [HttpGet("user")]
        public async Task<ActionResult<IEnumerable<Order>>> GetUserOrders()
        {
            var userId = Request.Headers["X-User-Id"].ToString();
            if (string.IsNullOrEmpty(userId))
                return BadRequest("Missing X-User-Id");

            var orders = await _db.Orders
                .Where(o => o.UserId == userId)
                .Include(o => o.Items)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();

            return Ok(orders);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Order>> GetById(int id)
        {
            var order = await _db.Orders
                .Include(o => o.Items)
                .FirstOrDefaultAsync(o => o.Id == id);
            return order == null ? NotFound() : Ok(order);
        }
    }
}

using HotelMe.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelMeAPI.Controllers
{
    [ApiController]
    [Route("api/menu")]
    public class MenuController : ControllerBase
    {
        private readonly HotelMeContext _context;

        public MenuController(HotelMeContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IEnumerable<MenuItem>> GetMenu()
        {
            return await _context.MenuItems.ToListAsync();
        }
    }

}

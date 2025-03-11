using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelMe.Shared;
using Microsoft.AspNetCore.Authorization;
using HotelMeAPI.Attributes;

[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly HotelMeContext _context;

    public UsersController(HotelMeContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IEnumerable<User>> GetUsers()
    {
        return await _context.Users.ToListAsync();
    }

    [HttpPost]
    public async Task<ActionResult<User>> CreateUser(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetUsers), new { id = user.Id }, user);
    }

    [AdminOnly] //[Authorize(Roles = "Admin")]
    [HttpGet("all-users")]
    public async Task<IEnumerable<User>> GetAllUsers()
    {
        return await _context.Users.ToListAsync();
    }

}

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelMeAPI.Attributes;
using HotelMeAPI.Models;
using Microsoft.AspNetCore.Identity;

//[AdminOnly]
[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;

    public UsersController(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    [HttpGet]
    public async Task<IEnumerable<ApplicationUser>> GetUsers()
    {
        return await _userManager.Users.ToListAsync();
    }

    [HttpPost]
    public async Task<ActionResult<ApplicationUser>> CreateUser([FromBody] RegisterModel model)
    {
        var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
        var result = await _userManager.CreateAsync(user, model.Password);

        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        return CreatedAtAction(nameof(GetUsers), new { id = user.Id }, user);
    }

    [HttpGet("all-users")]
    public async Task<IEnumerable<ApplicationUser>> GetAllUsers()
    {
        return await _userManager.Users.ToListAsync();
    }
}

using HotelMe.Shared;
using HotelMeAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class HotelMeContext : IdentityDbContext<ApplicationUser>
{
    public HotelMeContext(DbContextOptions<HotelMeContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
}

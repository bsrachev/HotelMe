using HotelMePWA.Data;
using Microsoft.EntityFrameworkCore;

public class HotelMeContext : DbContext
{
    public HotelMeContext(DbContextOptions<HotelMeContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
}
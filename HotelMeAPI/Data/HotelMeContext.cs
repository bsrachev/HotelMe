using HotelMe.Shared.Models;
using HotelMeAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class HotelMeContext : IdentityDbContext<ApplicationUser>
{
    public HotelMeContext(DbContextOptions<HotelMeContext> options) : base(options) { }

    public DbSet<Booking> Bookings { get; set; }

    public DbSet<MenuItem> MenuItems { get; set; }

    public DbSet<ChatMessage> ChatMessages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<MenuItem>()
            .Property(m => m.Price)
            .HasColumnType("decimal(18,2)");

        /*modelBuilder.Entity<MenuItem>().HasData(
            new MenuItem { Id = 1, Name = "Cheeseburger", Description = "Juicy beef burger with cheese", Price = 12.99m, Category = "Food" },
            new MenuItem { Id = 2, Name = "Coca-Cola", Description = "Refreshing soda", Price = 3.99m, Category = "Drinks" }
        );*/
    }
}

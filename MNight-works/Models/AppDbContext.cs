// Target framework for the project: .NET 10.0
using Microsoft.EntityFrameworkCore;
using MNightWorks.Shared.Models;

namespace MNight_works.Models
{
    public class AppDbContext : DbContext
    {
        // Constructor that accepts DbContextOptions and passes them to the base DbContext class
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        // Define a DbSet for the MenuItem model
        public DbSet<MenuItem> MenuItems { get; set; }

        // Register the Restaurants table so EF Core knows about the Restaurant entity
        public DbSet<Restaurant> Restaurants => Set<Restaurant>();

        // Register the Users table so EF Core knows about the User entity
        public DbSet<User> Users => Set<User>();
    }
}

using CitiesManager.WebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CitiesManager.WebAPI.DataBaseContext;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }

    public ApplicationDbContext()
    {
    }

    public virtual DbSet<City> Cities { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<City>().HasData(
            new City
            {
                CityId = Guid.Parse("E3A5D882-5099-4F64-B06C-D8DB7BF6A202"),
                Name = "New York",
            });

        modelBuilder.Entity<City>().HasData(
            new City
            {
                CityId = Guid.Parse("F049BA0E-2152-4434-BB47-4DBFA0A76217"),
                Name = "Tokyo",
            }
        );
    }
}
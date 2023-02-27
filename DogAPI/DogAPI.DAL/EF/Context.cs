using DogAPI.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DogAPI.DAL.EF;

public sealed class Context : DbContext
{
    public DbSet<Dog> Dogs { get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!;

    private Context(DbContextOptions options) : base(options) { }

    public Context()
    {
        Database.EnsureCreated();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(
            "Server=(local);Database=DogAPI;Trusted_Connection=True;TrustServerCertificate=True");
    }
}
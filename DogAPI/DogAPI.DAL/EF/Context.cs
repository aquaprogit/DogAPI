using DogAPI.DAL.Entities;

using Microsoft.EntityFrameworkCore;

namespace DogAPI.DAL.EF;
public class Context : DbContext
{
    public virtual DbSet<Dog> Dogs { get; set; }

    protected Context(DbContextOptions options) : base(options) { }

    public Context()
    {
        Database.EnsureCreated();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=(local);Database=DogAPI;Trusted_Connection=True;TrustServerCertificate=True");
    }
}

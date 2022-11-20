using DDStudy2022.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DDStudy2022.DAL;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DataContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<UserSession> Sessions => Set<UserSession>();
    public DbSet<UserAccount> UserAccounts => Set<UserAccount>();
    public DbSet<Attach> Attaches => Set<Attach>();
    public DbSet<Post> Posts => Set<Post>();
    public DbSet<Avatar> Avatars => Set<Avatar>();
    public DbSet<Photo> Photos => Set<Photo>();
    public DbSet<Comment> Comments => Set<Comment>();
}
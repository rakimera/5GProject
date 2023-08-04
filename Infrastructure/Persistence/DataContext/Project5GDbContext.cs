using Application.Models.ContrAgents;
using Domain.Common;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.DataContext;

public class Project5GDbContext : DbContext
{
    public DbSet<Antenna> Antennae { get; set; }
    public DbSet<ContrAgent> ContrAgents { get; set; }
    public DbSet<District> Districts { get; set; }
    public DbSet<Location> Locations { get; set; }
    public DbSet<Project> Projects { get; set; }
    public DbSet<ProjectAntenna> ProjectsAntennae { get; set; }
    public DbSet<ProjectStatus> ProjectsStatuses { get; set; }
    public DbSet<Town> Towns { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }

    public Project5GDbContext(DbContextOptions<Project5GDbContext> options) : base(options) {}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasIndex(x => x.Login).IsUnique();
        modelBuilder.Entity<ContrAgent>().HasQueryFilter(x => x.IsDelete == false);
        modelBuilder.Entity<User>().HasQueryFilter(x => x.IsDelete == false);
        modelBuilder.Entity<District>().HasQueryFilter(x => x.IsDelete == false);
        modelBuilder.Entity<Town>().HasQueryFilter(x => x.IsDelete == false);
        modelBuilder.Entity<Antenna>().HasQueryFilter(x => x.IsDelete == false);
        modelBuilder.Entity<EnergyResult>().HasQueryFilter(x => x.IsDelete == false);
        modelBuilder.Entity<Location>().HasQueryFilter(x => x.IsDelete == false);
        modelBuilder.Entity<Project>().HasQueryFilter(x => x.IsDelete == false);
        modelBuilder.Entity<ProjectAntenna>().HasQueryFilter(x => x.IsDelete == false);
        modelBuilder.Entity<ProjectStatus>().HasQueryFilter(x => x.IsDelete == false);
        modelBuilder.Entity<TranslatorSpecs>().HasQueryFilter(x => x.IsDelete == false);
        modelBuilder.Entity<User>().HasIndex(x => x.Id).IsUnique();
        modelBuilder.Entity<Antenna>().HasIndex(x => x.Id).IsUnique();
        modelBuilder.Entity<ContrAgent>().HasIndex(x => x.Id).IsUnique();
        modelBuilder.Entity<District>().HasIndex(x => x.Id).IsUnique();
        modelBuilder.Entity<EnergyResult>().HasIndex(x => x.Id).IsUnique();
        modelBuilder.Entity<Location>().HasIndex(x => x.Id).IsUnique();
        modelBuilder.Entity<Project>().HasIndex(x => x.Id).IsUnique();
        modelBuilder.Entity<ProjectAntenna>().HasIndex(x => x.Id).IsUnique();
        modelBuilder.Entity<ProjectStatus>().HasIndex(x => x.Id).IsUnique();
        modelBuilder.Entity<RefreshToken>().HasIndex(x => x.Id).IsUnique();
        modelBuilder.Entity<Town>().HasIndex(x => x.Id).IsUnique();
        modelBuilder.Entity<TranslatorSpecs>().HasIndex(x => x.Id).IsUnique();
        base.OnModelCreating(modelBuilder);
    }

    public async Task<int> SaveChangesAsync()
    {
        var entries = ChangeTracker
            .Entries()
            .Where(e => e.Entity is BaseEntity && (
                e.State == EntityState.Added
                || e.State == EntityState.Modified));

        foreach (var entityEntry in entries)
        {
            ((BaseEntity)entityEntry.Entity).LastModified = DateTime.Now;
            if (entityEntry.State == EntityState.Added)
            {
                ((BaseEntity)entityEntry.Entity).Created = DateTime.Now;
            }
        }

        return await base.SaveChangesAsync();
    }
}
using Application.Models.ContrAgents;
using Domain.Common;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.DataContext;

public class Project5GDbContext : DbContext
{
    public DbSet<Antenna> Antennas { get; set; }
    public DbSet<AntennaTranslator> AntennaTranslators { get; set; }
    public DbSet<ContrAgent> CounterAgents { get; set; }
    public DbSet<District> Districts { get; set; }
    public DbSet<EnergyResult> EnergyResults { get; set; }
    public DbSet<ExecutiveCompany> ExecutiveCompanies { get; set; }
    public DbSet<Project> Projects { get; set; }
    public DbSet<ProjectAntenna> ProjectsAntennae { get; set; }
    public DbSet<ProjectStatus> ProjectsStatuses { get; set; }
    public DbSet<RadiationZone> RadiationZones { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<SanPinDock> SanPinDocks { get; set; }
    public DbSet<TotalFluxDensity> TotalFluxDensities { get; set; }
    public DbSet<Town> Towns { get; set; }
    public DbSet<TranslatorSpecs> TranslatorsSpecs { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<BiohazardRadius> BiohazardRadii { get; set; }
    public DbSet<SummaryBiohazardRadius> SummaryBiohazardRadii { get; set; }
    public DbSet<TranslatorType> TranslatorTypes { get; set; }

    public Project5GDbContext(DbContextOptions<Project5GDbContext> options) : base(options) {}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasIndex(x => x.Login).IsUnique();
        modelBuilder.Entity<ContrAgent>().HasQueryFilter(x => x.IsDelete == false);
        modelBuilder.Entity<TranslatorType>().HasQueryFilter(x => x.IsDelete == false);
        modelBuilder.Entity<User>().HasQueryFilter(x => x.IsDelete == false);
        modelBuilder.Entity<District>().HasQueryFilter(x => x.IsDelete == false);
        modelBuilder.Entity<Town>().HasQueryFilter(x => x.IsDelete == false);
        modelBuilder.Entity<Antenna>().HasQueryFilter(x => x.IsDelete == false);
        modelBuilder.Entity<EnergyResult>().HasQueryFilter(x => x.IsDelete == false);
        modelBuilder.Entity<Project>().HasQueryFilter(x => x.IsDelete == false);
        modelBuilder.Entity<Project>().Navigation(e=> e.ContrAgent).AutoInclude();
        modelBuilder.Entity<Project>().Navigation(e=> e.Executor).AutoInclude();
        modelBuilder.Entity<Project>().Navigation(e=> e.ExecutiveCompany).AutoInclude();
        modelBuilder.Entity<Project>().Navigation(e=> e.ProjectStatus).AutoInclude();
        modelBuilder.Entity<Project>().Navigation(e=> e.ProjectAntennae).AutoInclude();
        modelBuilder.Entity<Project>().Navigation(e=> e.SummaryBiohazardRadius).AutoInclude();
        modelBuilder.Entity<Project>().Navigation(e=> e.TotalFluxDensity).AutoInclude();
        // modelBuilder.Entity<TranslatorSpecs>().Navigation(e=> e.RadiationZones).AutoInclude();
        modelBuilder.Entity<ProjectAntenna>().HasQueryFilter(x => x.IsDelete == false);
        modelBuilder.Entity<ProjectAntenna>().Navigation(e=> e.Antenna).AutoInclude();
        modelBuilder.Entity<AntennaTranslator>().Navigation(e=> e.TranslatorSpecs).AutoInclude();
        modelBuilder.Entity<ProjectStatus>().HasQueryFilter(x => x.IsDelete == false);
        modelBuilder.Entity<TranslatorSpecs>().HasQueryFilter(x => x.IsDelete == false);
        modelBuilder.Entity<AntennaTranslator>().HasQueryFilter(x => x.IsDelete == false);
        modelBuilder.Entity<EnergyResult>().HasQueryFilter(x => x.IsDelete == false);
        modelBuilder.Entity<ExecutiveCompany>().HasQueryFilter(x => x.IsDelete == false);
        modelBuilder.Entity<RadiationZone>().HasQueryFilter(x => x.IsDelete == false);
        modelBuilder.Entity<SanPinDock>().HasQueryFilter(x => x.IsDelete == false);
        modelBuilder.Entity<TotalFluxDensity>().HasQueryFilter(x => x.IsDelete == false);
        modelBuilder.Entity<UserRole>().HasQueryFilter(x => x.IsDelete == false);
        modelBuilder.Entity<BiohazardRadius>().HasQueryFilter(x => x.IsDelete == false);
        modelBuilder.Entity<SummaryBiohazardRadius>().HasQueryFilter(x => x.IsDelete == false);
        modelBuilder.Entity<Role>().HasQueryFilter(x => x.IsDelete == false);
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
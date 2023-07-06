using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistance.DataContext;

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

    public Project5GDbContext(DbContextOptions<Project5GDbContext> options) : base(options) {}
}
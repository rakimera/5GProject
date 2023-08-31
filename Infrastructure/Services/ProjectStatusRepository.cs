using Application.Interfaces.RepositoryContract;
using Domain.Entities;
using Infrastructure.Persistence.DataContext;
using Infrastructure.Repositories;

namespace Infrastructure.Services;

public class ProjectStatusRepository : BaseRepository<ProjectStatus>, IProjectStatusRepository
{
    public ProjectStatusRepository(Project5GDbContext dbContext) : base(dbContext)
    {
    }
}
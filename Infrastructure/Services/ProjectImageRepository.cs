using Application.Interfaces.RepositoryContract;
using Domain.Entities;
using Infrastructure.Persistence.DataContext;
using Infrastructure.Repositories;

namespace Infrastructure.Services;

public class ProjectImageRepository : BaseRepository<ProjectImage>, IProjectImageRepository
{
    public ProjectImageRepository(Project5GDbContext dbContext) : base(dbContext)
    {
        
    }
}
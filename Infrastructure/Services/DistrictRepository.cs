using Application.Interfaces.RepositoryContract;
using Domain.Entities;
using Infrastructure.Persistence.DataContext;
using Infrastructure.Repositories;

namespace Infrastructure.Services;

public class DistrictRepository : BaseRepository<District>, IDistrictRepository
{
    public DistrictRepository(Project5GDbContext dbContext) : base(dbContext)
    {
    }
}
using Application.Interfaces.RepositoryContract;
using Domain.Entities;
using Infrastructure.Persistence.DataContext;
using Infrastructure.Repositories;

namespace Infrastructure.Services;

public class TownRepository : BaseRepository<Town>, ITownRepository
{
    public TownRepository(Project5GDbContext dbContext) : base(dbContext)
    {
    }
}
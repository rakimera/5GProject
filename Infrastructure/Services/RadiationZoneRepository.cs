using Application.Interfaces.RepositoryContract;
using Domain.Entities;
using Infrastructure.Persistence.DataContext;
using Infrastructure.Repositories;

namespace Infrastructure.Services;

public class RadiationZoneRepository : BaseRepository<RadiationZone>, IRadiationZoneRepository
{
    public RadiationZoneRepository(Project5GDbContext dbContext) : base(dbContext)
    {
    }
}
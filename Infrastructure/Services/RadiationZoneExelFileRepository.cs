using Application.Interfaces.RepositoryContract;
using Domain.Entities;
using Infrastructure.Persistence.DataContext;
using Infrastructure.Repositories;

namespace Infrastructure.Services;

public class RadiationZoneExelFileRepository : BaseRepository<RadiationZoneExelFile>, IRadiationZoneExelFileRepository
{
    public RadiationZoneExelFileRepository(Project5GDbContext dbContext) : base(dbContext)
    {
        
    }
    
    public void Delete(RadiationZoneExelFile exelFile)
    {
        DbContext.RadiationZoneExelFiles.Remove(exelFile);
    }
}
using Application.Interfaces.RepositoryContract;
using Domain.Entities;
using Infrastructure.Persistence.DataContext;
using Infrastructure.Repositories;

namespace Infrastructure.Services;

public class AntennaRepository : BaseRepository<Antenna>, IAntennaRepository
{
    public AntennaRepository(Project5GDbContext dbContext) : base(dbContext)
    {
    }
}
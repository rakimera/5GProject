using Application.Interfaces.RepositoryContract;
using Domain.Entities;
using Infrastructure.Persistence.DataContext;
using Infrastructure.Repositories;

namespace Infrastructure.Services;

public class AntennaTranslatorRepository : BaseRepository<AntennaTranslator>, IAntennaTranslatorRepository
{
    public AntennaTranslatorRepository(Project5GDbContext dbContext) : base(dbContext)
    {
    }
}
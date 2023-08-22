using Application.Interfaces.RepositoryContract;
using Domain.Entities;
using Infrastructure.Persistence.DataContext;
using Infrastructure.Repositories;

namespace Infrastructure.Services;

public class TranslatorSpecsRepository : BaseRepository<TranslatorSpecs>, ITranslatorSpecsRepository
{
    public TranslatorSpecsRepository(Project5GDbContext dbContext) : base(dbContext)
    {
    }
}

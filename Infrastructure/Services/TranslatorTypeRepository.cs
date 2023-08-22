using Application.Interfaces.RepositoryContract;
using Domain.Entities;
using Infrastructure.Persistence.DataContext;
using Infrastructure.Repositories;

namespace Infrastructure.Services;

public class TranslatorTypeRepository : BaseRepository<TranslatorType>, ITranslatorTypeRepository
{
    public TranslatorTypeRepository(Project5GDbContext dbContext) : base(dbContext)
    {
    }
}
using Application.Interfaces.RepositoryContract;
using Domain.Entities;
using Infrastructure.Persistence.DataContext;
using Infrastructure.Repositories;

namespace Infrastructure.Services;

public class SummaryBiohazardRadiusRepository : BaseRepository<SummaryBiohazardRadius>, ISummaryBiohazardRadiusRepository
{
    public SummaryBiohazardRadiusRepository(Project5GDbContext dbContext) : base(dbContext)
    {
    }
}
using Application.Interfaces.RepositoryContract;
using Domain.Entities;
using Infrastructure.Persistence.DataContext;
using Infrastructure.Repositories;

namespace Infrastructure.Services;

public class ContrAgentRepository : BaseRepository<CounterAgent>, IContrAgentRepository
{
    public ContrAgentRepository(Project5GDbContext dbContext) : base(dbContext)
    {
    }
}
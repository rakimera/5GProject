using Application.Interfaces.RepositoryContract;
using Domain.Entities;
using Infrastructure.Persistence.DataContext;
using Infrastructure.Repositories;

namespace Infrastructure.Services;

public class ExecutiveCompanyRepository : BaseRepository<ExecutiveCompany>, IExecutiveCompanyRepository
{
    public ExecutiveCompanyRepository(Project5GDbContext dbContext) : base(dbContext)
    {
    }
}
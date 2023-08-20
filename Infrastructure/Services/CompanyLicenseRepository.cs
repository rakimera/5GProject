using Application.Interfaces.RepositoryContract;
using Domain.Entities;
using Infrastructure.Persistence.DataContext;
using Infrastructure.Repositories;

namespace Infrastructure.Services;

public class CompanyLicenseRepository : BaseRepository<CompanyLicense>, ICompanyLicenseRepository
{
    public CompanyLicenseRepository(Project5GDbContext dbContext) : base(dbContext)
    {
    }
}
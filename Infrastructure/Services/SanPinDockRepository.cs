using Application.Interfaces.RepositoryContract;
using Domain.Entities;
using Infrastructure.Persistence.DataContext;
using Infrastructure.Repositories;

namespace Infrastructure.Services;

public class SanPinDockRepository : BaseRepository<SanPinDock>, ISanPinDockRepository
{
    public SanPinDockRepository(Project5GDbContext dbContext) : base(dbContext)
    {
    }
}
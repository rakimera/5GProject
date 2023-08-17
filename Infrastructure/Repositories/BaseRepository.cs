using System.Linq.Expressions;
using Application.Interfaces.RepositoryContract.Common;
using Infrastructure.Persistence.DataContext;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public abstract class BaseRepository <T> : IBaseRepository<T> where T : class
{
    protected Project5GDbContext DbContext { get; set; }

    public BaseRepository(Project5GDbContext dbContext)
    {
        DbContext = dbContext;
    }

    public IQueryable<T> GetAll()
    {
        return DbContext.Set<T>();
    }

    public async Task CreateAsync(T entity)
    {
        await DbContext.Set<T>().AddAsync(entity);
    }

    public async Task<T> GetByCondition(Expression<Func<T, bool>> expression)
    {
        return await DbContext.Set<T>().FirstOrDefaultAsync(expression);
    }

    public IQueryable<T>? GetAllByCondition(Expression<Func<T, bool>> expression)
    {
        return DbContext.Set<T>().Where(expression);
    }

    public void Update(T entity)
    {
        DbContext.Set<T>().Update(entity);
    }
}
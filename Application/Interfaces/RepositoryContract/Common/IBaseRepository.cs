using System.Linq.Expressions;

namespace Application.Interfaces.RepositoryContract.Common;

public interface IBaseRepository<T>
{
    IQueryable<T> GetAll();
    Task CreateAsync(T entity);
    Task<T?> GetByCondition(Expression<Func<T, bool>> expression);
    void Update(T entity);
}
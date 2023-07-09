using System.Linq.Expressions;

namespace Application.Interfaces.Common;

public interface ICrudService <T>
{
    IEnumerable<T> GetAll();
    Task CreateAsync(T entity);
    Task<T> GetByCondition(Expression<Func<T, bool>> expression);
    void Update(T entity);
}
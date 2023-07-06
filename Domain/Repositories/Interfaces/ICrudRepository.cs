namespace Domain.Repositories.Interfaces;

public interface ICrudRepository<T>
{
    IEnumerable<T?> GetAll();
    Task CreateAsync(T entity);
    Task<T?> GetByIdAsync(int id);
    Task<T?> UpdateAsync(T entity);
    Task DeleteAsync(int id);
}
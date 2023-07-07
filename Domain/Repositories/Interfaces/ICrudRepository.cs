namespace Domain.Repositories.Interfaces;

public interface ICrudRepository<T>
{
    IEnumerable<T?> GetAll();
    Task CreateAsync(T model);
    Task<T?> GetByIdAsync(int id);
    Task<T?> UpdateAsync(T model);
    Task DeleteAsync(int id);
}
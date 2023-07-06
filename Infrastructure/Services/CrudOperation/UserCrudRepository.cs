using Domain.Entities;
using Domain.Repositories;
using Domain.Repositories.Interfaces;
using Infrastructure.Persistance.DataContext;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.CrudOperation;

public class UserCrudRepository : ICrudRepository<User>
{
    private readonly Project5GDbContext _db;

    public UserCrudRepository(Project5GDbContext db)
    {
        _db = db;
    }

    public IEnumerable<User?> GetAll() => _db.Users.ToList();

    public async Task CreateAsync(User entity)
    {
        await _db.Users.AddAsync(entity);
        await _db.SaveChangesAsync();
    } 
    
    public async Task<User?> GetByIdAsync(int id) => await _db.Users.FirstOrDefaultAsync(x=> x != null && x.Id == id);

    public async Task<User?> UpdateAsync(User entity)
    {
        entity.LastModified = DateTime.Now;
        _db.Users.Update(entity);
        await _db.SaveChangesAsync();
        return entity;
    }

    public async Task DeleteAsync(int id)
    {
        User? user = await _db.Users.FirstOrDefaultAsync(x => x.Id == id);
        if (user is not null)
            _db.Users.Remove(user);
    }
}
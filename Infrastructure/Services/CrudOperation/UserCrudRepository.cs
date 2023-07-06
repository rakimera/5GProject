using Application.Models;
using Domain.Entities;
using Domain.Repositories.Interfaces;
using Infrastructure.Persistance.DataContext;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.CrudOperation;

public class UserCrudRepository : ICrudRepository<UserDTO>
{
    private readonly Project5GDbContext _db;

    public UserCrudRepository(Project5GDbContext db)
    {
        _db = db;
    }


    public IEnumerable<UserDTO?> GetAll()
    {
        var users = _db.Users.ToList();
        List<UserDTO> models = new List<UserDTO>() { }; // типо замапил
        return models;
    } 

    public async Task CreateAsync(UserDTO dtoModel)
    {
        User user = new User() { }; // типо замапил
        await _db.Users.AddAsync(user);
        await _db.SaveChangesAsync();
    } 
    
    public async Task<UserDTO?> GetByIdAsync(int id)
    { 
        
       User? user = await _db.Users.FirstOrDefaultAsync(x=> x != null && x.Id == id);
       UserDTO model = new UserDTO() { }; // типо замапил
       return model;
    }

    public async Task<UserDTO?> UpdateAsync(UserDTO entity)
    {
        entity.LastModified = DateTime.Now;
        User user = new User() { }; // типо замапил
        _db.Users.Update(user);
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
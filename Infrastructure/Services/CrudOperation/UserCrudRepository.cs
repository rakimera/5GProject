using Application.Mappers;
using Application.Models;
using Application.Validation;
using AutoMapper;
using Domain.Entities;
using Domain.Repositories.Interfaces;
using FluentValidation.Results;
using Infrastructure.Persistance.DataContext;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.CrudOperation;

public class UserCrudRepository : ICrudRepository<UserDTO>
{
    private readonly Project5GDbContext _db;
    private readonly UserValidator _validator; //тут должны быть только интефейсы или такая реализация допустима? И как мы будем делать зависимости без фабрики, новая библа ?
    private readonly IMapper _mapProfile;
    public UserCrudRepository(Project5GDbContext db, UserValidator validator, IMapper mapProfile)
    {
        _db = db;
        _validator = validator;
        _mapProfile = mapProfile;
    }


    public IEnumerable<UserDTO?> GetAll()
    {
        List<User> users = _db.Users.ToList();
        List<UserDTO> models = _mapProfile.Map<List<UserDTO>>(users);
        
        return models;
    } 

    public async Task CreateAsync(UserDTO model)
    {
        ValidationResult? validResult = await _validator.ValidateAsync(model);
        if (validResult.IsValid)
        {
            User user = _mapProfile.Map<User>(model);
            await _db.Users.AddAsync(user);
            await _db.SaveChangesAsync();
        }
    } 
    
    public async Task<UserDTO?> GetByIdAsync(int id)
    {
        User? user = await _db.Users.FirstOrDefaultAsync(x=> x.Id == id);
        UserDTO model = _mapProfile.Map<UserDTO>(user);
        return model; //надо обсудить будем ли бы делать BaseResponse(T, Message) хороший вариант для передачи ответов, но не единственный
    }

    public async Task<UserDTO?> UpdateAsync(UserDTO model)
    {
        ValidationResult? validResult = await _validator.ValidateAsync(model);
        if (validResult.IsValid)
        {
            model.LastModified = DateTime.Now;
            model.LastModifiedBy = "totalUser"; //тут реализация зависит от того, будет ли использоваться Entity
            User user = _mapProfile.Map<User>(model);
            _db.Users.Update(user);
            await _db.SaveChangesAsync();
        }
        
        return model;
    }

    public async Task DeleteAsync(int id)
    {
        User? user = await _db.Users.FirstOrDefaultAsync(x => x.Id == id);
        if (user is not null)
            _db.Users.Remove(user);
    }
}
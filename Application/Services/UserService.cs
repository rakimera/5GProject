using System.Linq.Expressions;
using Application.Interfaces;
using Application.Interfaces.RepositoryContract;
using Application.Models;

namespace Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public IEnumerable<UserDTO> GetAll()
    {
       var entity =  _userRepository.GetAll().ToList();
       List<UserDTO> models = new List<UserDTO>(); //mapping

       return models;
    }

    public Task CreateAsync(UserDTO entity)
    {
        throw new NotImplementedException();
    }

    public Task<UserDTO?> GetByCondition(Expression<Func<UserDTO, bool>> expression)
    {
        throw new NotImplementedException();
    }

    public void UpdateAsync(UserDTO entity)
    {
        throw new NotImplementedException();
    }
}
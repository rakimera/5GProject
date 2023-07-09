using System.Linq.Expressions;
using Application.Interfaces;
using Application.Interfaces.RepositoryContract;
using Application.Models;
using AutoMapper;
using Domain.Entities;

namespace Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public UserService(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public IEnumerable<UserDTO> GetAll()
    {
        IQueryable<User> users = _userRepository.GetAll();
        List<UserDTO> models = _mapper.Map<List<UserDTO>>(users);

        return models;
    }

    public Task CreateAsync(UserDTO entity)
    {
        throw new NotImplementedException();
    }

    public Task<UserDTO> GetByCondition(Expression<Func<UserDTO, bool>> expression)
    {
        throw new NotImplementedException();
    }

    public void Update(UserDTO entity)
    {
        throw new NotImplementedException();
    }
}
using Application.Models;
using Domain.Repositories.Interfaces;

namespace Application.Services;

public class UserService
{
    private readonly ICrudRepository<UserDTO> _userRepository;

    public UserService(ICrudRepository<UserDTO> userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UserDTO?> GetByIdAsync(int id)
    {
        UserDTO? userDto = await _userRepository.GetByIdAsync(id);
        return userDto;
    }
    
}
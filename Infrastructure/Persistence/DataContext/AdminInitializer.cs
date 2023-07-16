using Application.Interfaces;
using Application.Interfaces.RepositoryContract.Common;
using Application.Models.Users;
using Domain.Entities;

namespace Infrastructure.Persistence.DataContext;

public class AdminInitializer
{
    private readonly IServiceWrapper _serviceWrapper;
    private readonly IRepositoryWrapper _repositoryWrapper;

    public AdminInitializer(IServiceWrapper serviceWrapper, IRepositoryWrapper repositoryWrapper)
    {
        _serviceWrapper = serviceWrapper;
        _repositoryWrapper = repositoryWrapper;
    }
    

    public async Task TrySeedAsync()
    {
        User? user = await _repositoryWrapper.UserRepository.GetByCondition(u => u.Login.Equals("admin@gmail.com"));
        if (user is null)
        {
            UserDto admin = new UserDto()
            {
                Id = Guid.NewGuid(),
                Name = "Admin",
                Surname = "Admin",
                Login = "admin@gmail.com",
                Password = "Qwerty@123",
                Role = "Admin"
            };
            await _serviceWrapper.UserService.CreateAsync(admin);
            await _repositoryWrapper.Save();
        }
    }
}
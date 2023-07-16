using Domain.Common;

namespace Application.Models.Users;

public class UserDto : BaseEntity
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Role { get; set; }
}
using Domain.Common;

namespace Application.Models.Users;

public class UserDto : BaseEntity
{
    public string Login { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Password { get; set; }
    public string Role { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime RefreshTokenExpiryTime { get; set; }
}
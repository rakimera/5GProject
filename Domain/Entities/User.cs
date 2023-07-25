using Domain.Common;

namespace Domain.Entities;

public class User : BaseEntity
{
    public string Login { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Password { get; set; }
    public string Role { get; set; }
    public List<RefreshToken> RefreshTokens { get; set; }
}
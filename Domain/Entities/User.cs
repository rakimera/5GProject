using Domain.Common;

namespace Domain.Entities;

public class User : BaseEntity
{
    public string Login { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Password { get; set; }
    public List<RefreshToken> RefreshTokens { get; set; }
    public Guid? ExecutiveCompanyId { get; set; }
    public ExecutiveCompany ExecutiveCompany { get; set; }
}
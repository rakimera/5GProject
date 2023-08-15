using System.Text.Json.Serialization;
using Domain.Common;

namespace Application.Models.Users;

public class UserDto : BaseEntity
{
    public string Login { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Password { get; set; }
    [property: JsonPropertyName("roles")] public List<string> Roles { get; set; }
}
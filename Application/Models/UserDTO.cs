using Domain.Common;

namespace Application.Models;

public class UserDTO : BaseEntity
{
    public string Name { get; set; }
    public string Surname { get; set; }
}
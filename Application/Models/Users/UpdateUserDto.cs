namespace Application.Models.Users;

public class UpdateUserDto
{
    public Guid Oid { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
}
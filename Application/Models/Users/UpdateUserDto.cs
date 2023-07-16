namespace Application.Models.Users;

public class UpdateUserDto
{
    public string Oid { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Role { get; set; }
}
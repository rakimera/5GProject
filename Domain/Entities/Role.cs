using Domain.Common;

namespace Domain.Entities;

public class Role : BaseEntity
{
    public Guid Id { get; set; }
    public required string RoleName { get; set; }
}
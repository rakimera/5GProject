using Domain.Common;

namespace Domain.Entities;

public class ProjectStatus : BaseEntity
{
    public required string Status { get; set; }
}
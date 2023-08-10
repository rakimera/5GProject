using Domain.Common;

namespace Domain.Entities;

public class SanPinDock : BaseEntity
{
    public string Number { get; set; }
    public DateTime DateOfIssue { get; set; }
}
using Domain.Common;

namespace Domain.Entities;

public class TranslatorType : BaseEntity
{
    public required string Type { get; set; }
}
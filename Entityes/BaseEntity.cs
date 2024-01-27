namespace Entities;

public abstract class BaseEntity(DateTime createdAt)
{
    public DateTime CreatedAt { get; set; } = createdAt;
}

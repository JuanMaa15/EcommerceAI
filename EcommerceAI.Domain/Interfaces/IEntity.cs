namespace EcommerceAI.Domain.Interfaces;

public interface IEntity
{
    Guid Id { get; }
    bool IsActive { get; }
    void Activate();
    void Deactivate();
}

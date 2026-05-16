namespace Abstractions.Interfaces;

public interface ICurrentUser
{
    public Guid? OrganizationId { get; }
}
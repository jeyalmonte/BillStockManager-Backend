namespace SharedKernel.Domain;

public interface IEntity
{
	List<IDomainEvent> PopDomainEvents();
}
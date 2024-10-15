namespace SharedKernel.Domain;

public abstract class Entity : IEntity
{
	public Guid Id { get; private init; }
	protected Entity() { }
	protected Entity(Guid id) => Id = id;

	private readonly List<IDomainEvent> _domainEvents = [];
	protected void RaiseEvent(IDomainEvent domainEvent)
		=> _domainEvents.Add(domainEvent);
	public List<IDomainEvent> PopDomainEvents()
	{
		var copy = _domainEvents.ToList();
		_domainEvents.Clear();

		return copy;
	}

}

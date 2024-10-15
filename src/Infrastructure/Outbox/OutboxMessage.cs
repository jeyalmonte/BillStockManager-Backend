namespace Infrastructure.Outbox;

public record OutboxMessage(
	Guid Id,
	string Type,
	string Content,
	DateTime CreatedOn,
	bool Processed,
	DateTime? ProcessedOn)
{
	public static OutboxMessage Create(string type, string content)
	{
		return new OutboxMessage(
			Id: Guid.NewGuid(),
			Type: type,
			Content: content,
			CreatedOn: DateTime.UtcNow,
			Processed: false,
			ProcessedOn: null);
	}
}

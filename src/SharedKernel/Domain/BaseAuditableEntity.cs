namespace SharedKernel.Domain;

public class BaseAuditableEntity : Entity
{
	public DateTime CreatedOn { get; set; }
	public string? CreatedBy { get; set; }
	public DateTime? UpdatedOn { get; set; }
	public DateTime? UpdatedBy { get; set; }
	public bool IsDeleted { get; private set; }

	protected void MarkAsDeleted()
	{
		IsDeleted = true;
	}
}

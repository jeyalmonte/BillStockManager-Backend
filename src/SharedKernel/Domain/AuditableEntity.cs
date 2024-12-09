namespace SharedKernel.Domain;

public class AuditableEntity : Entity
{
	public DateTime CreatedOn { get; set; }
	public string? CreatedBy { get; set; }
	public DateTime? UpdatedOn { get; set; }
	public string? UpdatedBy { get; set; }
	public bool IsDeleted { get; private set; }
	public void MarkAsDeleted()
	{
		IsDeleted = true;
	}
}

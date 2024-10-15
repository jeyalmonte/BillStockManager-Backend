namespace SharedKernel.Domain;

public abstract class BaseAuditableEntity : Entity
{
	public DateTime CreatedAt { get; set; }
	public string? CreatedBy { get; set; }
	public DateTime? UpdatedAt { get; set; }
	public DateTime? UpdatedBy { get; set; }
	public bool IsDeleted { get; set; }
	public DateTime? DeletedAt { get; set; }
	public string? DeletedBy { get; set; }
}

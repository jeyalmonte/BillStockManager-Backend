using SharedKernel.Interfaces.Services;

namespace Infrastructure.Services;

public class SystemDateTimeProvider : IDateTimeProvider
{
	public DateTime UtcNow => DateTime.UtcNow;

	public DateTime Now => DateTime.Now;
}

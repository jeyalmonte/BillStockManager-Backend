using SharedKernel.Interfaces.Services;

namespace Infrastructure.Common.Services;

public class SystemDateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;

    public DateTime Now => DateTime.Now;
}

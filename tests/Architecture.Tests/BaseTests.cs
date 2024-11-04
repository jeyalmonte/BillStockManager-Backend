using System.Reflection;

namespace Architecture.Tests;
public abstract class BaseTests
{
    protected readonly Assembly DomainAssembly = Assembly.Load("Domain");
    protected readonly Assembly ApplicationAssembly = Assembly.Load("Application");
    protected readonly Assembly InfrastructureAssembly = Assembly.Load("Infrastructure");
}


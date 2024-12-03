namespace Api.Utils;

public static class ApiVersions
{
	public const string V1 = "1.0";
	public const string V2 = "2.0";
}

public class ApiRoute
{
	public const string GlobalPrefix = "api/v{version:apiVersion}/";
}

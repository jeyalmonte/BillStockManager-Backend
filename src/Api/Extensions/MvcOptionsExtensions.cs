using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Routing;

namespace Api.Extensions;

public static class MvcOptionsExtensions
{
	public static void UseGeneralRoutePrefix(this MvcOptions options, string prefix)
	{
		options.Conventions.Insert(0, new RoutePrefixCOnvention(new RouteAttribute(prefix)));
	}

}

public class RoutePrefixCOnvention(IRouteTemplateProvider routeTemplate) : IApplicationModelConvention
{
	private readonly AttributeRouteModel _centralPrefix = new(routeTemplate);
	public void Apply(ApplicationModel application)
	{

		foreach (SelectorModel selector in application.Controllers.SelectMany(c => c.Selectors))
		{
			if (selector.AttributeRouteModel is not null)
			{
				selector.AttributeRouteModel = AttributeRouteModel.CombineAttributeRouteModel(_centralPrefix, selector.AttributeRouteModel);
			}
			else
			{
				selector.AttributeRouteModel = _centralPrefix;
			}
		}
	}
}

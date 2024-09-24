using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Routing;

namespace NotificationService.Api.Extensions;

/// <summary>
/// Provides extension methods for configuring routing options in the API.
/// </summary>
public static class MvcOptionsExtensions
{
    /// <summary>
    /// Adds a general route prefix to all controllers in the application.
    /// This method allows specifying a route template provider, which defines the route prefix
    /// for all controllers in the current application model.
    /// </summary>
    /// <param name="opts">The <see cref="MvcOptions"/> to configure.</param>
    /// <param name="routeAttribute">The route template provider that defines the prefix.</param>
    public static void UseGeneralRoutePrefix(this MvcOptions opts, IRouteTemplateProvider routeAttribute)
    {
        opts.Conventions.Add(new RoutePrefixConvention(routeAttribute));
    }

    /// <summary>
    /// Adds a general route prefix to all controllers in the application using a string.
    /// This method allows specifying a route prefix as a string, which is converted to a 
    /// <see cref="RouteAttribute"/> for routing configuration.
    /// </summary>
    /// <param name="opts">The <see cref="MvcOptions"/> to configure.</param>
    /// <param name="prefix">The string representing the route prefix.</param>
    public static void UseGeneralRoutePrefix(this MvcOptions opts, string prefix)
    {
        opts.UseGeneralRoutePrefix(new RouteAttribute(prefix));
    }
}

/// <summary>
/// Represents a convention that applies a route prefix to the application model.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="RoutePrefixConvention"/> class.
/// </remarks>
/// <param name="route">The route template provider defining the prefix.</param>
public class RoutePrefixConvention(IRouteTemplateProvider route) : IApplicationModelConvention
{
    private readonly AttributeRouteModel _routePrefix = new AttributeRouteModel(route);

    /// <summary>
    /// Applies the route prefix to the specified <see cref="ApplicationModel"/>.
    /// </summary>
    /// <param name="application">The application model to which the prefix is applied.</param>
    public void Apply(ApplicationModel application)
    {
        foreach (var selector in application.Controllers.SelectMany(c => c.Selectors))
        {
            if (selector.AttributeRouteModel != null)
            {
                selector.AttributeRouteModel = AttributeRouteModel.CombineAttributeRouteModel(_routePrefix, selector.AttributeRouteModel);
            }
            else
            {
                selector.AttributeRouteModel = _routePrefix;
            }
        }
    }
}
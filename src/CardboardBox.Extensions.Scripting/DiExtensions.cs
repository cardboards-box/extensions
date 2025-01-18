namespace CardboardBox.Extensions.Scripting;

using Templating;
using Tokening;

/// <summary>
/// Dependency Injection Extensions
/// </summary>
public static class DiExtensions
{
    /// <summary>
    /// Adds the templating services to the service collection
    /// </summary>
    /// <param name="services">The service collection to add to</param>
    /// <returns>The service collection for fluent method chaining</returns>
    public static IServiceCollection AddTemplatingServices(this IServiceCollection services)
    {
        return services
            .AddTransient<ITemplateService, TemplateService>()
            .AddTransient<ITokenService, TokenService>();
    }
}

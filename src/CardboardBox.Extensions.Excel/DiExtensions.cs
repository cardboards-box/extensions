using Microsoft.Extensions.DependencyInjection;

namespace CardboardBox.Extensions.Excel;

/// <summary>
/// Extensions for registering services with the DI container
/// </summary>
public static class DiExtensions
{
    /// <summary>
    /// Adds the excel parser services to the DI container
    /// </summary>
    /// <param name="services">The service collection</param>
    /// <returns>The service collection for chaining</returns>
    public static IServiceCollection AddExcel(this IServiceCollection services)
    {
        return services
            .AddTransient<IExcelParserService, ExcelParserService>()
            .AddTransient<IExcelWriterService, ExcelWriterService>();
    }
}

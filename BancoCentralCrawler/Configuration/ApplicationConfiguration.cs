using BancoCentralCrawler.Domain;
using BancoCentralCrawler.Services.Implementation;
using BancoCentralCrawler.Services.Interfaces;

namespace BancoCentralCrawler.Configuration;

public static class ApplicationConfiguration
{
    public static void AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IBancoCentralService, BancoCentralService>();
    }

    public static void DefineUrls(this IServiceCollection services, IConfiguration configuration)
    {
        UrlsConfig.BancoCentralListagemNotasUrl = configuration["BancoCentralListagemNotasUrl"] ?? string.Empty;
        UrlsConfig.BancoCentralListagemUrl = configuration["BancoCentralListagemUrl"] ?? string.Empty;
        UrlsConfig.BancoCentralDetalheNotasApiUrl = configuration["BancoCentralDetalheNotasApiUrl"] ?? string.Empty;
        UrlsConfig.BancoCentralDetalheApiUrl = configuration["BancoCentralDetalheApiUrl"] ?? string.Empty;
        UrlsConfig.BancoCentralDetalheNotasUrl = configuration["BancoCentralDetalheNotasUrl"] ?? string.Empty;
        UrlsConfig.BancoCentralDetalheUrl = configuration["BancoCentralDetalheUrl"] ?? string.Empty;
    }
}
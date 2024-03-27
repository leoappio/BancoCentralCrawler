using BancoCentralCrawler.Domain;

namespace BancoCentralCrawler.Services.Interfaces;

public interface IBancoCentralWebScrapper
{
    Task<ExtractedDataDto> GetWebDetailAsync(ObtainedNewsEvent evento);
}
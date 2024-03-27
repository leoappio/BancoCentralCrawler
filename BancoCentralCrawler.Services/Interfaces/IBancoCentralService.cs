using BancoCentralCrawler.Domain;

namespace BancoCentralCrawler.Services.Interfaces;

public interface IBancoCentralService
{
    Task<List<ExtractedDataDto>> GetNewsByYearAsync(int year);
    Task<List<ExtractedDataDto>> GetPressReleasesByYearAsync(int year);
    Task<List<ExtractedDataDto>> GetAllPressReleasesAsync();
    Task<List<ExtractedDataDto>> GetAllNewsAsync();
    Task<List<ExtractedDataDto>> GetAllNewsAndPressReleasesAsync();
}
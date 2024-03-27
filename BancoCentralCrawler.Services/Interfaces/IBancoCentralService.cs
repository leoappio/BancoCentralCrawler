using BancoCentralCrawler.Domain;

namespace BancoCentralCrawler.Services.Interfaces;

public interface IBancoCentralService
{
    Task<List<NoticiaDetalheResponseDto>> GetNewsByYearAsync(int year);
    Task<List<NoticiaDetalheResponseDto>> GetPressReleasesByYearAsync(int year);
    Task<List<NoticiaDetalheResponseDto>> GetAllPressReleasesAsync();
    Task<List<NoticiaDetalheResponseDto>> GetAllNewsAsync();
    Task<List<NoticiaDetalheResponseDto>> GetAllNewsAndPressReleasesAsync();
}
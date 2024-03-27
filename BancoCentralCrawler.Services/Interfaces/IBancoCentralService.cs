using BancoCentralCrawler.Domain;

namespace BancoCentralCrawler.Services.Interfaces;

public interface IBancoCentralService
{
    Task<List<NoticiaDetalheResponseDto>> GetNewsByYearAsync(int year);
}
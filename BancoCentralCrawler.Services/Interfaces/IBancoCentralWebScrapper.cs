using BancoCentralCrawler.Domain;

namespace BancoCentralCrawler.Services.Interfaces;

public interface IBancoCentralWebScrapper
{
    Task<NoticiaDetalheResponseDto> ObterDetalhe(NoticiaBancoCentralObtidaEvent evento);
}
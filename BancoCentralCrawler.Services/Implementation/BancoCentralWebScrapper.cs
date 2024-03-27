using System.Text;
using BancoCentralCrawler.Domain;
using BancoCentralCrawler.Services.Helpers;
using BancoCentralCrawler.Services.Interfaces;
using Newtonsoft.Json;

namespace BancoCentralCrawler.Services.Implementation;

public class BancoCentralWebScrapper : IBancoCentralWebScrapper
{
    public async Task<NoticiaDetalheResponseDto> ObterDetalhe(NoticiaBancoCentralObtidaEvent evento)
    {
        var detalheApiUrl = evento.UrlOriginal.ToString().Contains("nota")
            ? UrlsConfig.BancoCentralDetalheNotasApiUrl
            : UrlsConfig.BancoCentralDetalheApiUrl;

        var detalheUrl = evento.UrlOriginal.ToString().Contains("nota")
            ? UrlsConfig.BancoCentralDetalheNotasUrl
            : UrlsConfig.BancoCentralDetalheUrl;

        var urlApi =
            detalheApiUrl.Replace("[id]", evento.Id.ToString());

        var jsonNorma = await CircuitBreakerHelper.TryNTimesAsync(async () =>
            await WebHelper.GetAsync(urlApi, Encoding.UTF8), 5, 2000);

        if (jsonNorma is null)
            throw new ApplicationException("Detalhe da notícia não disponível.");

        var detalheNoticia = JsonConvert.DeserializeObject<dynamic>(jsonNorma);

        if (detalheNoticia is null || detalheNoticia.conteudo is null)
            throw new ApplicationException("Detalhe da notícia não disponível.");

        var urlOriginal =
            new Uri(detalheUrl.Replace("[id]",
                evento.Id.ToString()));

        return new NoticiaDetalheResponseDto
        {
            Conteudo = detalheNoticia.conteudo[0].corpo,
            Data = Convert.ToDateTime(detalheNoticia.conteudo[0].dataPublicacao),
            Id = detalheNoticia.conteudo[0].Id,
            Titulo = detalheNoticia.conteudo[0].titulo,
            Descricao = detalheNoticia.conteudo[0].lead,
            UrlOriginal = urlOriginal
        };
    }
}
using System.Text;
using BancoCentralCrawler.Domain;
using BancoCentralCrawler.Services.Helpers;
using BancoCentralCrawler.Services.Interfaces;
using Newtonsoft.Json;

namespace BancoCentralCrawler.Services.Implementation;

public class BancoCentralWebScrapper : IBancoCentralWebScrapper
{
    public async Task<ExtractedDataDto> GetWebDetailAsync(ObtainedNewsEvent evento)
    {
        var detailWebApi = evento.UrlOriginal.ToString().Contains("nota")
            ? UrlsConfig.BancoCentralDetalheNotasApiUrl
            : UrlsConfig.BancoCentralDetalheApiUrl;

        var detailUrl = evento.UrlOriginal.ToString().Contains("nota")
            ? UrlsConfig.BancoCentralDetalheNotasUrl
            : UrlsConfig.BancoCentralDetalheUrl;

        var urlApi =
            detailWebApi.Replace("[id]", evento.Id.ToString());

        var jsonData = await CircuitBreakerHelper.TryNTimesAsync(async () =>
            await WebHelper.GetAsync(urlApi, Encoding.UTF8), 5, 2000);

        if (jsonData is null)
            throw new ApplicationException("Detalhe não disponível.");

        var itemDetail = JsonConvert.DeserializeObject<dynamic>(jsonData);

        if (itemDetail is null || itemDetail.conteudo is null)
            throw new ApplicationException("Detalhe não disponível.");

        var urlOriginal =
            new Uri(detailUrl.Replace("[id]",
                evento.Id.ToString()));

        return new ExtractedDataDto
        {
            Conteudo = itemDetail.conteudo[0].corpo,
            Data = Convert.ToDateTime(itemDetail.conteudo[0].dataPublicacao),
            Id = itemDetail.conteudo[0].Id,
            Titulo = itemDetail.conteudo[0].titulo,
            Descricao = itemDetail.conteudo[0].lead,
            UrlOriginal = urlOriginal
        };
    }
}
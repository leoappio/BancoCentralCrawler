using System.Text;
using BancoCentralCrawler.Domain;
using BancoCentralCrawler.Services.Helpers;
using BancoCentralCrawler.Services.Interfaces;
using Newtonsoft.Json;

namespace BancoCentralCrawler.Services.Implementation;

public class BancoCentralService : IBancoCentralService
{
    private readonly IBancoCentralWebScrapper _bancoCentralWebScrapper;

    public BancoCentralService(IBancoCentralWebScrapper bancoCentralWebScrapper)
    {
        _bancoCentralWebScrapper = bancoCentralWebScrapper;
    }

    public async Task<List<NoticiaDetalheResponseDto>> GetNewsByYearAsync(int year)
    {
        var newsListing = await ObterResultadosListagemPorAnoAsync(year);

        var response = new List<NoticiaDetalheResponseDto>();

        foreach (var newsEvent in newsListing)
        {
            var detalhe = await _bancoCentralWebScrapper.ObterDetalhe(newsEvent);
            
            response.Add(detalhe);
        }

        return response;
    }
    
    private async Task<List<NoticiaBancoCentralObtidaEvent>> ObterResultadosListagemNotas(int ano)
    {
        var url = UrlsConfig.BancoCentralListagemNotasUrl;

        var response = new List<NoticiaBancoCentralObtidaEvent>();

        var urlAno = url.Replace("[ano]", ano.ToString());

        var jsonLista = await CircuitBreakerHelper.TryNTimesAsync(() =>
            WebHelper.GetAsync(urlAno, Encoding.UTF8), 5, 2000);

        if (jsonLista is null)
            return response;

        var listaNotas = JsonConvert.DeserializeObject<dynamic>(jsonLista);

        if (listaNotas is null || listaNotas.conteudo is null)
            return response;;

        foreach (var item in listaNotas.conteudo)
            response.Add(new NoticiaBancoCentralObtidaEvent(
                id: Convert.ToInt32(item.Id),
                urlOriginal: new Uri(new Uri(UrlsConfig.BancoCentralDetalheNotasUrl),
                    item.Url.ToString()),
                titulo: item.titulo.ToString(),
                dataDivulgacao: Convert.ToDateTime(item.DataPublicacao),
                dataModificacao: Convert.ToDateTime(item.DataModificacao)));

        return response;
    }

    private async Task<List<NoticiaBancoCentralObtidaEvent>> ObterResultadosListagemPorAnoAsync(int ano)
    {
        var url = UrlsConfig.BancoCentralListagemUrl;

        var response = new List<NoticiaBancoCentralObtidaEvent>();

        var urlAno = url.Replace("[ano]", ano.ToString());

        var jsonLista = await CircuitBreakerHelper.TryNTimesAsync(() =>
            WebHelper.GetAsync(urlAno, Encoding.UTF8), 5, 2000);

        if (jsonLista is null)
            return response;

        var listaNoticias = JsonConvert.DeserializeObject<dynamic>(jsonLista);

        if (listaNoticias is null || listaNoticias.conteudo is null)
            return response;

        foreach (var item in listaNoticias.conteudo)
            response.Add(new NoticiaBancoCentralObtidaEvent(
                id: Convert.ToInt32(item.Id),
                urlOriginal: new Uri(new Uri(UrlsConfig.BancoCentralDetalheUrl),
                    item.Url.ToString()),
                titulo: item.titulo.ToString(),
                dataDivulgacao: Convert.ToDateTime(item.DataPublicacao),
                dataModificacao: Convert.ToDateTime(item.DataModificacao)));

        return response;
    }
}
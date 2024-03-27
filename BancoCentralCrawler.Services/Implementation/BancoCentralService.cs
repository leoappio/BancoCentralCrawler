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
        var newsListing = await GetNewsListingByYearAsync(year);

        var response = new List<NoticiaDetalheResponseDto>();

        foreach (var newsEvent in newsListing)
        {
            var detail = await _bancoCentralWebScrapper.GetWebDetailAsync(newsEvent);
            
            response.Add(detail);
        }

        return response;
    }

    public async Task<List<NoticiaDetalheResponseDto>> GetPressReleasesByYearAsync(int year)
    {
        var newsListing = await GetPressReleasesListingByYearAsync(year);

        var response = new List<NoticiaDetalheResponseDto>();

        foreach (var newsEvent in newsListing)
        {
            var detail = await _bancoCentralWebScrapper.GetWebDetailAsync(newsEvent);
            
            response.Add(detail);
        }

        return response;
    }

    public async Task<List<NoticiaDetalheResponseDto>> GetAllPressReleasesAsync()
    {
        var response = new List<NoticiaDetalheResponseDto>();

        for (var year = 2000; year <= DateTime.Now.Year; year++)
        {
            var newsListing = await GetPressReleasesListingByYearAsync(year);
            
            foreach (var newsEvent in newsListing)
            {
                var detail = await _bancoCentralWebScrapper.GetWebDetailAsync(newsEvent);
            
                response.Add(detail);
            }
        }
        return response;
    }

    public async Task<List<NoticiaDetalheResponseDto>> GetAllNewsAsync()
    {
        var response = new List<NoticiaDetalheResponseDto>();

        for (var year = 2000; year <= DateTime.Now.Year; year++)
        {
            var newsListing = await GetNewsListingByYearAsync(year);
            
            foreach (var newsEvent in newsListing)
            {
                var detail = await _bancoCentralWebScrapper.GetWebDetailAsync(newsEvent);
            
                response.Add(detail);
            }
        }
        return response;
    }

    public async Task<List<NoticiaDetalheResponseDto>> GetAllNewsAndPressReleasesAsync()
    {
        var allPressReleases = await GetAllPressReleasesAsync();

        var allNews = await GetAllNewsAsync();

        allPressReleases.AddRange(allNews);

        return allPressReleases;
    }

    private async Task<List<NoticiaBancoCentralObtidaEvent>> GetPressReleasesListingByYearAsync(int ano)
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

    private async Task<List<NoticiaBancoCentralObtidaEvent>> GetNewsListingByYearAsync(int ano)
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
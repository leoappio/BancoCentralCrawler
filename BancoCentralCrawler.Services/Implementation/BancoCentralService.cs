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

    public async Task<List<ExtractedDataDto>> GetNewsByYearAsync(int year)
    {
        var newsListing = await GetNewsListingByYearAsync(year);

        var response = new List<ExtractedDataDto>();

        foreach (var newsEvent in newsListing)
        {
            var detail = await _bancoCentralWebScrapper.GetWebDetailAsync(newsEvent);
            
            response.Add(detail);
        }

        return response;
    }

    public async Task<List<ExtractedDataDto>> GetPressReleasesByYearAsync(int year)
    {
        var pressReleaseListing = await GetPressReleasesListingByYearAsync(year);

        var response = new List<ExtractedDataDto>();

        foreach (var pressReleaseEvent in pressReleaseListing)
        {
            var detail = await _bancoCentralWebScrapper.GetWebDetailAsync(pressReleaseEvent);
            
            response.Add(detail);
        }

        return response;
    }

    public async Task<List<ExtractedDataDto>> GetAllPressReleasesAsync()
    {
        var response = new List<ExtractedDataDto>();

        for (var year = 2000; year <= DateTime.Now.Year; year++)
        {
            var pressReleaseListing = await GetPressReleasesListingByYearAsync(year);
            
            foreach (var pressReleaseEvent in pressReleaseListing)
            {
                var detail = await _bancoCentralWebScrapper.GetWebDetailAsync(pressReleaseEvent);
            
                response.Add(detail);
            }
        }
        return response;
    }

    public async Task<List<ExtractedDataDto>> GetAllNewsAsync()
    {
        var response = new List<ExtractedDataDto>();

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

    public async Task<List<ExtractedDataDto>> GetAllNewsAndPressReleasesAsync()
    {
        var allPressReleases = await GetAllPressReleasesAsync();

        var allNews = await GetAllNewsAsync();

        allPressReleases.AddRange(allNews);

        return allPressReleases;
    }

    private async Task<List<ObtainedNewsEvent>> GetPressReleasesListingByYearAsync(int ano)
    {
        var url = UrlsConfig.BancoCentralListagemNotasUrl;

        var response = new List<ObtainedNewsEvent>();

        var yearUrl = url.Replace("[ano]", ano.ToString());

        var itemsListing = await CircuitBreakerHelper.TryNTimesAsync(() =>
            WebHelper.GetAsync(yearUrl, Encoding.UTF8), 5, 2000);

        if (itemsListing is null)
            return response;

        var pressReleaseListing = JsonConvert.DeserializeObject<dynamic>(itemsListing);

        if (pressReleaseListing is null || pressReleaseListing.conteudo is null)
            return response;

        foreach (var item in pressReleaseListing.conteudo)
            response.Add(new ObtainedNewsEvent(
                id: Convert.ToInt32(item.Id),
                urlOriginal: new Uri(new Uri(UrlsConfig.BancoCentralDetalheNotasUrl),
                    item.Url.ToString()),
                titulo: item.titulo.ToString(),
                dataDivulgacao: Convert.ToDateTime(item.DataPublicacao),
                dataModificacao: Convert.ToDateTime(item.DataModificacao)));

        return response;
    }

    private async Task<List<ObtainedNewsEvent>> GetNewsListingByYearAsync(int ano)
    {
        var url = UrlsConfig.BancoCentralListagemUrl;

        var response = new List<ObtainedNewsEvent>();

        var yearUrl = url.Replace("[ano]", ano.ToString());

        var itemsListing = await CircuitBreakerHelper.TryNTimesAsync(() =>
            WebHelper.GetAsync(yearUrl, Encoding.UTF8), 5, 2000);

        if (itemsListing is null)
            return response;

        var newsListing = JsonConvert.DeserializeObject<dynamic>(itemsListing);

        if (newsListing is null || newsListing.conteudo is null)
            return response;

        foreach (var item in newsListing.conteudo)
            response.Add(new ObtainedNewsEvent(
                id: Convert.ToInt32(item.Id),
                urlOriginal: new Uri(new Uri(UrlsConfig.BancoCentralDetalheUrl),
                    item.Url.ToString()),
                titulo: item.titulo.ToString(),
                dataDivulgacao: Convert.ToDateTime(item.DataPublicacao),
                dataModificacao: Convert.ToDateTime(item.DataModificacao)));

        return response;
    }
}
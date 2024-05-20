using BancoCentralCrawler.Domain;
using BancoCentralCrawler.Services.Helpers;
using BancoCentralCrawler.Services.Interfaces;
using HtmlAgilityPack;
using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Text;
using System.Text.RegularExpressions;

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
                titulo: item.titulo.ToString()));

        return response;
    }

    public async Task<List<ObtainedNewsEvent>> GetNewsListingByYearAsync(int ano)
    {
        var bcbInitialPage = "https://www.bcb.gov.br";

        var response = new List<ObtainedNewsEvent>();

        var options = new ChromeOptions();
        options.AddArgument("--headless");
        using var driver = new ChromeDriver(options);

        driver.Navigate().GoToUrl(bcbInitialPage);

        var noticiasLinkNode = driver.FindElement(By.XPath("//a[contains(@href, '/noticias') and contains(text(), 'Mais notas à imprensa')]"));
        ClickElement(driver, noticiasLinkNode);

        await Task.Delay(2000);

        var noticiasPorAnoLinkNode = driver.FindElement(By.XPath("//a[contains(@href, '/noticiasporano') and contains(@class, 'mais-noticias')]"));
        ClickElement(driver, noticiasPorAnoLinkNode);

        await Task.Delay(2000);

        var noticiasPorAnoUrl = $"{bcbInitialPage}/noticiasporano?ano={ano}";

        driver.Navigate().GoToUrl(noticiasPorAnoUrl);

        await Task.Delay(2000);

        var newsByYearUrl = driver.PageSource;

        if (string.IsNullOrEmpty(newsByYearUrl))
            return response;

        var doc = new HtmlDocument();
        doc.LoadHtml(newsByYearUrl);

        var newsNodes = doc.DocumentNode.SelectNodes("//div[contains(@class, 'item-custom')]");
        if (newsNodes == null)
            return response;

        foreach (var node in newsNodes)
        {
            try
            {
                var tituloNode = node.SelectSingleNode(".//div[contains(@class, 'titulo')]/a");
                var urlOriginal = new Uri(new Uri(bcbInitialPage), tituloNode.GetAttributeValue("href", ""));
                var titulo = tituloNode?.InnerText.Trim();

                var id = ExtractIdFromUrl(urlOriginal.ToString());

                response.Add(new ObtainedNewsEvent(
                    id: id,
                    urlOriginal: urlOriginal,
                    titulo: titulo
                ));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao processar notícia: {ex.Message}");
            }
        }

        return response;
    }

    private void ClickElement(IWebDriver driver, IWebElement element)
    {
        try
        {
            element.Click();
        }
        catch (Exception)
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("arguments[0].click();", element);
        }
    }

    private int ExtractIdFromUrl(string url)
    {
        var match = Regex.Match(url, @"detalhenoticia/(\d+)/noticia");
        return match.Success ? int.Parse(match.Groups[1].Value) : 0;
    }
}